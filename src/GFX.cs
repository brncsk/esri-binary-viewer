// Copyright (C) 2013 Barancsuk Ádám
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

using ESRIBinaryViewer.Model;

namespace ESRIBinaryViewer.GFX
{
    /// <summary>
    /// Represents a preset for the convolution filter.
    /// </summary>
    public struct ConvolutionFilterPreset
    {
        /// <summary>
        /// The preset's kernel.
        /// </summary>
        public double[] kernel;
        /// <summary>
        /// The preset's name.
        /// </summary>
        public string name;
    }

    /// <summary>
    /// Graphic operations on raster datasets.
    /// </summary>
    public static class GraphicOperations
    {
        #region Convolution filter presets
        /// <summary>
        /// A bunch of presets for the convolution filter.
        /// </summary>
        public static ConvolutionFilterPreset[] ConvolutionFilterPresets = {
            new ConvolutionFilterPreset { name = "Low Pass",                kernel = new[] {
                1d,  1,  1, 
                1,  2,  1, 
                1,  1,  1 } },
            new ConvolutionFilterPreset { name = "High Pass",               kernel = new[] {
                0d, -1,  0,
                -1, -5, -1, 
                0, -1,  0 } },
            new ConvolutionFilterPreset { name = "Laplace",                 kernel = new[] {
                0d,  1,  0, 
                1, -4,  1, 
                0,  1,  0 } },
            new ConvolutionFilterPreset { name = "Mean",                    kernel = new[] { 
                1d,  1,  1, 
                1,  1,  1, 
                1,  1,  1 } },
            new ConvolutionFilterPreset { name = "Edge detection",          kernel = new[] { 
                -1d, -1, -1,
                -1,  8, -1,
                -1, -1, -1 } },
            new ConvolutionFilterPreset { name = "Sharpen",                 kernel = new[] { 
                -1d, -1, -1,
                -1,  9, -1,
                -1, -1, -1 } },
            new ConvolutionFilterPreset { name = "Emboss (3×3)",            kernel = new[] { 
                -1d, -1,  0,
                -1,  0,  1,
                    0,  1,  1 } }, 
            new ConvolutionFilterPreset { name = "Emboss (5×5)",            kernel = new[] { 
                -1d, -1, -1, -1,  0,
                -1, -1, -1,  0,  1,
                -1, -1,  0,  1,  1,
                -1,  0,  1,  1,  1,
                0,  1,  1,  1,  1 } },
            new ConvolutionFilterPreset { name = "Blur",                    kernel = new[] { 
                0d, .2,  0, 
                .2, .2, .2, 
                0, .2,  0 } },
            new ConvolutionFilterPreset { name = "NW-SE motion blur (9×9)", kernel = new[] {
                1d, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 1, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 1, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 1, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 1, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 1, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 1, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 1, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 1 } },
        };
        #endregion

        /// <summary>
        /// Creates an RGB composite of three or less bands of a dataset, asynchronously.
        /// </summary>
        /// <param name="gb">The dataset.</param>
        /// <param name="b1">The band representing the red values.</param>
        /// <param name="b2">The band representing the green values.</param>
        /// <param name="b3">The band representing the blue values.</param>
        /// <returns>A BackgroundWorker representing the asynchronous operation.</returns>
        public static BackgroundWorker CreateRGBComposite (Dataset gb, int b1, int b2, int b3) {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (s, e) => {
                MessageLog.Add("Creating RGB composite.");

                // Start timekeeping.
                Stopwatch st = new Stopwatch();
                st.Start();

                Bitmap b = new Bitmap((int) gb.Header.NCols, (int) gb.Header.NRows, PixelFormat.Format24bppRgb);
                BitmapData bd = b.LockBits(
                    new Rectangle(0, 0, (int) gb.Header.NCols, (int) gb.Header.NRows),
                    ImageLockMode.WriteOnly,
                    PixelFormat.Format24bppRgb
                );

                // b1, b2, b3 are coming directly from the UI
                // This is where we check if any of them are omitted.
                RasterBand band1 = (b1 > -1) ? gb.Bands[(int) b1] : null;
                RasterBand band2 = (b2 > -1) ? gb.Bands[(int) b2] : null;
                RasterBand band3 = (b3 > -1) ? gb.Bands[(int) b3] : null;

                // Lock image bits
                if (band1 != null) { band1.LockBits(ImageLockMode.ReadOnly); }
                if (band2 != null) { band2.LockBits(ImageLockMode.ReadOnly); }
                if (band3 != null) { band3.LockBits(ImageLockMode.ReadOnly); }

                unsafe {
                    for (int y = 0; y < gb.Header.NRows; y++) {
                        byte* row = (byte*) bd.Scan0 + (y * bd.Stride), row1 = null, row2 = null, row3 = null;

                        // We preset a pointer before looping through rows,
                        // this is faster than calculating the offset in every iteration of the inner loop.
                        if (band1 != null) {
                            row1 = ((byte*) band1.BitmapData.Scan0 + (y * band1.BitmapData.Stride));
                        }
                        if (band2 != null) {
                            row2 = ((byte*) band2.BitmapData.Scan0 + (y * band2.BitmapData.Stride));
                        }
                        if (band3 != null) {
                            row3 = ((byte*) band3.BitmapData.Scan0 + (y * band3.BitmapData.Stride));
                        }

                        // GDI bitmaps are BGR instead of RGB (!)
                        for (int x = 0; x < gb.Header.NCols; x++) {
                            row[x * 3 + 2] = (band1 != null) ? row1[x] : (byte) 0;
                            row[x * 3 + 1] = (band2 != null) ? row2[x] : (byte) 0;
                            row[x * 3 + 0] = (band3 != null) ? row3[x] : (byte) 0;
                        }

                        if (bw.CancellationPending) {
                            e.Cancel = true;
                            goto cleanup;
                        }

                        bw.ReportProgress((int) (((double) y / gb.Header.NRows) * 100));
                    }
                }

            cleanup:
                if (band1 != null) { band1.UnlockBits(); }
                if (band2 != null) { band2.UnlockBits(); }
                if (band3 != null) { band3.UnlockBits(); }

                b.UnlockBits(bd);

                e.Result = b;

                // Stop timekeeping and log elapsed time.
                st.Stop();
                MessageLog.Add(String.Format("Composition took {0} msecs.", st.ElapsedMilliseconds));
            };

            return bw;
        }

        /// <summary>
        /// Equalizes the histogram on a single raster band, asynchronously.
        /// </summary>
        /// <param name="band">The raster band.</param>
        /// <returns>A Tuple containing a BackgroundWorker representing the asynchronous operation and its parameter.</returns>
        public static BackgroundWorker EqualizeHistogram (RasterBand band) {
            return EqualizeHistogram(new[] { band });
        }

        /// <summary>
        /// Equalizes the histogram on a collection of raster bands, asynchronously.
        /// </summary>
        /// <param name="bands">The collection of raster bands.</param>
        /// <returns>A Tuple containing a BackgroundWorker representing the asynchronous operation and its parameter.</returns>
        public static BackgroundWorker EqualizeHistogram (ICollection<RasterBand> bands) {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (s, e) => {
                MessageLog.Add("Equalizing histograms.");

                // Start timekeeping.
                Stopwatch st = new Stopwatch();
                st.Start();

                // We need to report progress while processing each band twice,
                // so we precompute this ratio here.
                double progressPerBandHalf = 100 / (bands.Count * 2);
                int bi = 0;

                foreach (RasterBand b in bands) {
                    if (b == null) {
                        continue;
                    }

                    BitmapData bd = b.BitmapData;

                    b.LockBits(ImageLockMode.ReadWrite);

                    int[] hist = new int[256],
                          sumOfHist = new int[256];
                    int sum = 0;
                    double k = 255.0 / (bd.Width * bd.Height);

                    unsafe {
                        byte* data = (byte*) bd.Scan0;

                        // Classify pixels
                        for (int y = 0; y < bd.Height; y++) {
                            for (int x = 0; x < bd.Width; x++) {
                                hist[data[y * bd.Stride + x]]++;
                            }

                            if (bw.CancellationPending) {
                                e.Cancel = true;
                                goto cleanup;
                            }

                            bw.ReportProgress((int) (((double) y / bd.Height) * progressPerBandHalf + bi * 2 * progressPerBandHalf));
                        }

                        // Calculate the cumulative frequency
                        for (int i = 0; i <= 255; i++) {
                            sum += hist[i];
                            sumOfHist[i] = sum;
                        }

                        // Do the actual equalization
                        for (int y = 0; y < bd.Height; y++) {
                            for (int x = 0; x < bd.Width; x++) {
                                data[y * bd.Stride + x] =
                                    (byte) (sumOfHist[data[y * bd.Stride + x]] * k);
                            }

                            if (bw.CancellationPending) {
                                e.Cancel = true;
                                goto cleanup;
                            }

                            bw.ReportProgress((int) ((((double) y / bd.Height) * progressPerBandHalf) + progressPerBandHalf + bi * 2 * progressPerBandHalf));
                        }
                    }
                    bi++;
                }

            cleanup:

                foreach (RasterBand b in bands) {
                    if (b != null) {
                        b.RefreshThumbnail();
                        b.UnlockBits();
                    }
                }

                // Stop timekeeping and log elapsed time.
                st.Stop();
                MessageLog.Add(String.Format("Histogram equalization took {0} msecs.", st.ElapsedMilliseconds));
            };
            return bw;
        }

        /// <summary>
        /// Calculates the histogram on a collection of raster bands, asynchronously.
        /// </summary>
        /// <param name="bands">The collection of raster bands.</param>
        /// <returns>A Tuple containing a BackgroundWorker representing the asynchronous operation and its parameter.</returns>
        public static BackgroundWorker CalculateHistogram (ICollection<RasterBand> bands) {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (s, e) => {
                MessageLog.Add("Calculating histogram.");

                // Start timekeeping.
                Stopwatch st = new Stopwatch();
                st.Start();

                double progressPerBand = 100 / bands.Count;
                int bi = 0;
                double[][] hist = new double[bands.Count][];

                foreach (RasterBand b in bands) {
                    hist[bi] = new double[256];

                    if (b == null) {
                        continue;
                    }

                    BitmapData bd = b.BitmapData;

                    b.LockBits(ImageLockMode.ReadOnly);

                    unsafe {
                        byte* data = (byte*) bd.Scan0;

                        // Simply classify pixels.
                        for (int y = 0; y < bd.Height; y++) {
                            for (int x = 0; x < bd.Width; x++) {
                                hist[bi][data[y * bd.Stride + x]]++;
                            }

                            if (bw.CancellationPending) {
                                e.Cancel = true;
                                goto cleanup;
                            }

                            bw.ReportProgress((int) (((double) y / bd.Height) * progressPerBand + bi * progressPerBand));
                        }
                    }
                    bi++;
                }

            cleanup:

                foreach (RasterBand b in bands) {
                    if (b != null) { b.UnlockBits(); }
                }
                e.Result = hist;

                // Stop timekeeping and log elapsed time.
                st.Stop();
                MessageLog.Add(String.Format("Histogram calculation took {0} msecs.", st.ElapsedMilliseconds));
            };

            return bw;
        }

        /// <summary>
        /// Applies a custom convolution filter to a bitmap, asynchronously.
        /// </summary>
        /// <param name="b">The bitmap.</param>
        /// <param name="kernel">The kernel of the convolution filter.</param>
        /// <returns>A Tuple containing a BackgroundWorker representing the asynchronous operation and its parameter.</returns>
        public static BackgroundWorker ApplyConvolutionFilter (Bitmap b, double[] kernel) {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (s, e) => {
                MessageLog.Add("Applying convolution filter.");

                Stopwatch st = new Stopwatch();
                st.Start();

                // This is the output bitmap.
                // IDK if there's a way to do this in place (I think no), but
                // this whole thing should be offloaded to the GPU anyway.
                Bitmap ob = new Bitmap(b);

                // Lock bits.
                BitmapData bd = b.LockBits(
                    new Rectangle(new Point(0, 0), b.Size),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format24bppRgb
                );
                BitmapData obd = ob.LockBits(
                    new Rectangle(new Point(0, 0), ob.Size),
                    ImageLockMode.WriteOnly,
                    PixelFormat.Format24bppRgb
                );

                // Assume a square kernel with an odd size.
                int kernelSize = (int) Math.Sqrt(kernel.Length);
                int kernelHalf = (int) Math.Floor(kernelSize / 2d);
                double kernelSum = kernel.Sum();

                unsafe {
                    byte* data = (byte*) bd.Scan0;
                    byte* odata = (byte*) obd.Scan0;
                    double[] sum = new double[3] { 0, 0, 0 };

                    // Both the kernel and the images are stored in one-dimensional arrays.
                    // We use two-dimensional indexing for clarity.
                    // 
                    for (int y = kernelHalf; y < bd.Height - kernelHalf; y++) {
                        for (int x = kernelHalf; x < bd.Width - kernelHalf; x++) {
                            sum[0] = sum[1] = sum[2] = 0;

                            // Here comes the trick.
                            for (int i = 0; i < kernelSize; i++) {
                                for (int j = 0; j < kernelSize; j++) {
                                    int c = (y + i - kernelHalf) * bd.Stride + (x + j - kernelHalf) * 3;

                                    sum[0] += data[c] * kernel[i * kernelSize + j];
                                    sum[1] += data[c + 1] * kernel[i * kernelSize + j];
                                    sum[2] += data[c + 2] * kernel[i * kernelSize + j];
                                }
                            }

                            sum[0] /= kernelSum;
                            sum[1] /= kernelSum;
                            sum[2] /= kernelSum;

                            odata[y * bd.Stride + x * 3 + 0] = (byte)
                                ((sum[0] >= 0) ? (sum[0] <= 255 ? sum[0] : 255) : 0);
                            odata[y * bd.Stride + x * 3 + 1] = (byte)
                                ((sum[1] >= 0) ? (sum[1] <= 255 ? sum[1] : 255) : 0);
                            odata[y * bd.Stride + x * 3 + 2] = (byte)
                                ((sum[2] >= 0) ? (sum[2] <= 255 ? sum[2] : 255) : 0);
                        }

                        if (bw.CancellationPending) {
                            e.Cancel = true;
                            goto cleanup;
                        }

                        bw.ReportProgress((int) ((double) y * 100 / bd.Height));
                    }
                }

            cleanup:

                b.UnlockBits(bd);
                ob.UnlockBits(obd);
                e.Result = ob;

                st.Stop();
                MessageLog.Add(String.Format("Filter application took {0} msecs.", st.ElapsedMilliseconds));
            };

            return bw;
        }

        /// <summary>
        /// Applies a median filter to a bitmap, asynchronously.
        /// </summary>
        /// <param name="b">The bitmap.</param>
        /// <param name="kernelSize">The kernel size of the median filter.</param>
        /// <returns>A Tuple containing a BackgroundWorker representing the asynchronous operation and its parameter.</returns>
        public static BackgroundWorker ApplyMedianFilter (Bitmap b, int kernelSize) {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (s, e) => {
                MessageLog.Add("Applying median filter.");

                Stopwatch st = new Stopwatch();
                st.Start();

                Bitmap ob = new Bitmap(b);

                BitmapData bd = b.LockBits(
                    new Rectangle(new Point(0, 0), b.Size),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format24bppRgb
                );
                BitmapData obd = ob.LockBits(
                    new Rectangle(new Point(0, 0), ob.Size),
                    ImageLockMode.WriteOnly,
                    PixelFormat.Format24bppRgb
                );

                int kernelHalf = (int) Math.Floor(kernelSize / 2d);

                unsafe {
                    byte* data = (byte*) bd.Scan0;
                    byte* odata = (byte*) obd.Scan0;
                    double[][] kernel = new double[3][] {
                        new double[kernelSize * kernelSize],
                        new double[kernelSize * kernelSize],
                        new double[kernelSize * kernelSize]
                    };

                    int k = 0;

                    for (int y = kernelHalf; y < bd.Height - kernelHalf; y++) {
                        for (int x = kernelHalf; x < bd.Width - kernelHalf; x++) {
                            k = 0;

                            for (int i = 0; i < kernelSize; i++) {
                                for (int j = 0; j < kernelSize; j++, k++) {
                                    int c = (y + i - kernelHalf) * bd.Stride + (x + j - kernelHalf) * 3;

                                    kernel[0][k] = data[c];
                                    kernel[1][k] = data[c + 1];
                                    kernel[2][k] = data[c + 2];
                                }
                            }

                            Array.Sort(kernel[0]);
                            Array.Sort(kernel[1]);
                            Array.Sort(kernel[2]);

                            odata[y * bd.Stride + x * 3 + 0] =
                                (byte) kernel[0][kernelSize / 2 + 1];
                            odata[y * bd.Stride + x * 3 + 1] =
                                (byte) kernel[1][kernelSize / 2 + 1];
                            odata[y * bd.Stride + x * 3 + 2] =
                                (byte) kernel[2][kernelSize / 2 + 1];
                        }

                        if (bw.CancellationPending) {
                            e.Cancel = true;
                            goto cleanup;
                        }
                        bw.ReportProgress((int) ((double) y * 100 / bd.Height));
                    }
                }

            cleanup:

                b.UnlockBits(bd);
                ob.UnlockBits(obd);
                e.Result = ob;

                st.Stop();
                MessageLog.Add(String.Format("Filter application took {0} msecs.", st.ElapsedMilliseconds));
            };
            return bw;
        }

        /// <summary>
        /// Inverts the colors of a bitmap, asynchronously.
        /// </summary>
        /// <param name="b">The bitmap.</param>
        /// <returns>A Tuple containing a BackgroundWorker representing the asynchronous operation and its parameter.</returns>
        public static BackgroundWorker ApplyInvertFilter (Bitmap b) {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (s, e) => {
                MessageLog.Add("Applying invert filter.");

                Stopwatch st = new Stopwatch();
                st.Start();

                Bitmap ob = new Bitmap(b);
                BitmapData bd = b.LockBits(
                    new Rectangle(new Point(0, 0), b.Size),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format24bppRgb
                );
                BitmapData obd = ob.LockBits(
                    new Rectangle(new Point(0, 0), ob.Size),
                    ImageLockMode.WriteOnly,
                    PixelFormat.Format24bppRgb
                );

                unsafe {
                    byte* data = (byte*) bd.Scan0;
                    byte* odata = (byte*) obd.Scan0;
                    for (int y = 0; y < bd.Height; y++) {
                        for (int x = 0; x < bd.Width; x++) {
                            odata[y * bd.Stride + x * 3 + 0] = (byte) (255 - data[y * bd.Stride + x * 3 + 0]);
                            odata[y * bd.Stride + x * 3 + 1] = (byte) (255 - data[y * bd.Stride + x * 3 + 1]);
                            odata[y * bd.Stride + x * 3 + 2] = (byte) (255 - data[y * bd.Stride + x * 3 + 2]);
                        }

                        if (bw.CancellationPending) {
                            e.Cancel = true;
                            goto cleanup;
                        }
                        bw.ReportProgress((int) ((double) y * 100 / bd.Height));
                    }
                }

            cleanup:

                b.UnlockBits(bd);
                ob.UnlockBits(obd);
                e.Result = ob;

                st.Stop();
                MessageLog.Add(String.Format("Filter application took {0} msecs.", st.ElapsedMilliseconds));
            };
            return bw;
        }

        /// <summary>
        /// Converts a bitmap to grayscale, asynchronously.
        /// </summary>
        /// <param name="b">The bitmap.</param>
        /// <returns>A Tuple containing a BackgroundWorker representing the asynchronous operation and its parameter.</returns>
        public static BackgroundWorker ApplyGrayscaleFilter (Bitmap b) {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (s, e) => {
                MessageLog.Add("Applying grayscale filter.");

                Stopwatch st = new Stopwatch();
                st.Start();

                Bitmap ob = new Bitmap(b);
                BitmapData bd = b.LockBits(
                    new Rectangle(new Point(0, 0), b.Size),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format24bppRgb
                );
                BitmapData obd = ob.LockBits(
                    new Rectangle(new Point(0, 0), ob.Size),
                    ImageLockMode.WriteOnly,
                    PixelFormat.Format24bppRgb
                );

                unsafe {
                    byte* data = (byte*) bd.Scan0;
                    byte* odata = (byte*) obd.Scan0;
                    for (int y = 0; y < bd.Height; y++) {
                        for (int x = 0; x < bd.Width; x++) {
                            odata[y * bd.Stride + x * 3 + 0] =
                            odata[y * bd.Stride + x * 3 + 1] =
                            odata[y * bd.Stride + x * 3 + 2] =
                                (byte) (
                                      .0722 * data[y * bd.Stride + x * 3 + 0] +
                                      .7152 * data[y * bd.Stride + x * 3 + 1] +
                                      .2126 * data[y * bd.Stride + x * 3 + 2]
                                );
                        }

                        if (bw.CancellationPending) {
                            e.Cancel = true;
                            goto cleanup;
                        }
                        bw.ReportProgress((int) ((double) y * 100 / bd.Height));
                    }
                }

            cleanup:

                b.UnlockBits(bd);
                ob.UnlockBits(obd);
                e.Result = ob;

                st.Stop();
                MessageLog.Add(String.Format("Filter application took {0} msecs.", st.ElapsedMilliseconds));
            };
            return bw;
        }
    }
}