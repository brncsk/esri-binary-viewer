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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace ESRIBinaryViewer.Model
{
    /// <summary>
    /// Represents an error that is raised while parsing a dataset.
    /// </summary>
    public class ParseException : System.Exception
    {
        public ParseException(string message) : base(message) { }
    }

    /// <summary>
    /// Provides access to the dataset's header fields.
    /// </summary>
    public class Header : IListSource
    {
        public readonly UInt32 NRows; 			    /// Number of rows
        public readonly UInt32 NCols;			    /// Number of columns
        public readonly UInt32 NBands;			    /// Number of bands
        public readonly UInt32 NBits;			    /// Number of bits per pixel
        public readonly PixelTypeEnum PixelType;	/// Signedness of pixels
        public readonly ByteOrderEnum ByteOrder;	/// Endianness
        public readonly LayoutEnum Layout;			/// Data layout
        public readonly UInt32 SkipBytes;			/// Bytes to be skipped
        public readonly double ULXMap;			    /// Projected X coordinate of the upper left pixel
        public readonly double ULYMap;			    /// Projected Y coordinate of the upper left pixel
        public readonly double XDim;			    /// Size of a pixel along the horizontal dimension in projected units
        public readonly double YDim;			    /// Size of a pixel along the vertical dimension in projected units
        public readonly UInt32 BandRowBytes;		/// Number of bytes per band per row (currently unused)
        public readonly UInt32 TotalRowBytes;		/// Total number of bytes per row (currently unused)
        public readonly UInt32 BandGapBytes;		/// Number of bytes between bands in a BSQ format image (currently unused)

        // IListSource implementation - so we can use a Header instance is used as a data source in the UI.
        public bool ContainsListCollection { get { return false; } }
        /// <summary>
        /// Returns the list of header fields.
        /// </summary>
        public IList GetList() { return Fields; }

        /// <summary>
        /// Signedness of pixels
        /// </summary>
        public enum PixelTypeEnum { UNSIGNEDINT, SIGNEDINT }

        /// <summary>
        /// Data layout
        /// </summary>
        public enum LayoutEnum { BIL, BIP, BSQ }

        /// <summary>
        /// Dataset endianness
        /// </summary>
        public enum ByteOrderEnum { I, M, LSBFIRST = I, MSBFIRST = M }

        /// <summary>
        /// Represents a field in the dataset header.
        /// </summary>
        public class Field
        {
            /// <summary>
            /// Field name
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// The field's value converted to string.
            /// Displayed in the UI.
            /// </summary>
            public string StringValue { get; set; }

            /// <summary>
            /// Field type
            /// </summary>
            public readonly Type Type;

            /// <summary>
            /// Default value of the field.
            /// Null, if the field is required.
            /// </summary>
            public readonly object DefaultValue;

            /// <summary>
            /// Regular expression used for validating the field's value.
            /// Null, if no validation is required.
            /// </summary>
            public readonly string Validator;

            public Field(string n, Type t, object d, string v) {
                Name = n;
                Type = t;
                DefaultValue = d;
                Validator = v;
            }
        }

        /// <summary>
        /// The architecture's byte order.
        /// </summary>
        private static readonly ByteOrderEnum ArchByteOrder = BitConverter.IsLittleEndian ? ByteOrderEnum.I : ByteOrderEnum.M;

        /// <summary>
        /// Header fields.
        /// </summary>
        private static readonly Field[] Fields = new[] {
            //         Name             Type                    Default value               Validator regex
            new Field( "NRows",         typeof(UInt32),         null,                       null ),
            new Field( "NCols",         typeof(UInt32),         null,                       null ),
            new Field( "NBands",        typeof(UInt32),         1,                          null ),
            new Field( "NBits",         typeof(UInt32),         8,                          "(1|4|8|16|32)" ),
            new Field( "PixelType",     typeof(PixelTypeEnum),  PixelTypeEnum.UNSIGNEDINT,  "SIGNEDINT" ),
            new Field( "ByteOrder",     typeof(ByteOrderEnum),  ArchByteOrder,              "(I|M|LSBFIRST|MSBFIRST)" ),
            new Field( "Layout",        typeof(LayoutEnum),     LayoutEnum.BIL,             "(BIL|BIP|BSQ)" ),
            new Field( "SkipBytes",     typeof(UInt32),         0u,                         null ),
            new Field( "ULXMap",        typeof(double),         0.0,                        null ),
            new Field( "ULYMap",        typeof(double),         0.0,                        null ),
            new Field( "XDim",          typeof(double),         1.0,                        null ),
            new Field( "YDim",          typeof(double),         1.0,                        null ),
            new Field( "BandRowBytes",  typeof(UInt32),         0u,                         null ),
            new Field( "TotalRowBytes", typeof(UInt32),         0u,                         null ),
            new Field( "BandGapBytes",  typeof(UInt32),         0u,                         null )
	    };

        private Header() { }

        /// <summary>
        /// Parses the header file of a raster dataset.
        /// </summary>
        /// <param name="fn">The header file's path.</param>
        /// <returns>an instance containing the fields in the header file.</returns>
        public static Header ParseFromFile(string fn) {
            Header h = new Header();
            MessageLog.Add(String.Format("Parsing header {0}.", fn));
            string fl = string.Join("\r\n", File.ReadAllLines(fn)).ToUpper();

            // We temporarily assume `en-US` locale so that string-to-double conversion
            // always uses '.' as the decimal separator.
            CultureInfo ci = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfoByIetfLanguageTag("en-US");

            try {
                // Loop through possible fields looking for them in the header file.
                foreach (Field f in Fields) {
                    Match m = Regex.Match(fl, string.Format("{0}\\s+(.+)", f.Name.ToUpper()));

                    // Check if there's a match and also validate it if applicable.
                    if (m.Success &&
                        ((f.Validator == null) || Regex.Match(m.Groups[1].Value, f.Validator).Success)
                    ) {
                        // Perform conversion to the field's target type.
                        TypeConverter tc = TypeDescriptor.GetConverter(f.Type);
                        f.StringValue = m.Groups[1].Value;
                        typeof(Header).GetField(f.Name).SetValue(h, tc.ConvertFromString(f.StringValue));
                    } else if (f.DefaultValue != null) {
                        // If a default value is present but either the field is missing
                        // from the header or validation failed, use the default but emit a warning.
                        MessageLog.Add(
                            String.Format(
                                "Value of header attribute {0} is missing or invalid, using default of \"{1}\".",
                                f.Name, f.DefaultValue
                            ),
                            MessageLog.EventType.WARN
                        );

                        typeof(Header).GetField(f.Name).SetValue(h, f.DefaultValue);
                        f.StringValue = f.DefaultValue.ToString();
                    } else {
                        // We fail here because the field is required (no default value), but
                        // is missing from the header file or its validation failed.
                        throw new ParseException(string.Format("Missing required header attribute {0}.", f.Name));
                    }
                }
            } catch (Exception ex) {
                // Bail out, but first convert to a ParseException.
                if (ex is ParseException) {
                    throw;
                } else {
                    throw new ParseException(ex.Message);
                }
            } finally {
                // Reset locale settings.             
                Thread.CurrentThread.CurrentCulture = ci;

                // TODO: Compute other field values.
            }
            return h;
        }
    }

    /// <summary>
    /// Represents a band in the raster dataset.
    /// </summary>
    public class RasterBand : IDisposable
    {
        /// <summary>
        /// Size of the thumbnail images.
        /// </summary>
        private const int ThumbnailSize = 96;

        /// <summary>
        /// Whether the bitmap data is currently locked.
        /// </summary>
        private bool locked = false;

        /// <summary>
        /// Holds the color palette for the band.
        /// </summary>
        private static ColorPalette cp;

        /// <summary>
        /// The band's index for UI display.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// The band's name for UI display.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The bitmap containing the raster data.
        /// </summary>
        public Bitmap Bitmap { get; private set; }

        /// <summary>
        /// The thumbnail image of the bitmap.
        /// </summary>
        public Image Thumbnail { get; private set; }

        /// <summary>
        /// The raw data of the band's bitmap.
        /// </summary>
        public BitmapData BitmapData;

        /// <summary>
        /// Holds the grayscale palette for the band.
        /// </summary>
        public ColorPalette GrayscalePalette {
            get {
                // We construct a simple grayscale palette if we haven't yet done so.
                if (cp == null) {
                    Bitmap b = new Bitmap(1, 1, PixelFormat.Format8bppIndexed);
                    cp = b.Palette;

                    for (int i = 0; i <= 255; i++) {
                        cp.Entries[i] = Color.FromArgb(i, i, i);
                    }
                }

                return cp;
            }
        }

        /// <summary>
        /// Creates a raster band.
        /// </summary>
        /// <param name="width">Image width</param>
        /// <param name="height">Image height</param>
        /// <param name="index">Band index</param>
        /// <param name="name">Band name</param>
        public RasterBand(int width, int height, int index, string name) {
            Bitmap = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            Bitmap.Palette = GrayscalePalette;
            Index = index;
            Name = name;
        }

        /// <summary>
        /// Locks the band's raw bitmap data for reading/writing.
        /// </summary>
        /// <param name="lm"></param>
        public void LockBits(ImageLockMode lm = ImageLockMode.WriteOnly) {
            if (locked) {
                return;
            }

            locked = true;

            BitmapData = Bitmap.LockBits(new Rectangle(0, 0, Bitmap.Width, Bitmap.Height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format8bppIndexed
            );
        }

        /// <summary>
        /// Unlocks the band's raw bitmap data.
        /// </summary>
        public void UnlockBits () {
            if (!locked) {
                return;
            }

            Bitmap.UnlockBits(BitmapData);
            Thumbnail = Bitmap.GetThumbnailImage(96, 96, null, System.IntPtr.Zero);
            locked = false;
        }

        /// <summary>
        /// Recreates the thumbnail from the bitmap.
        /// </summary>
        public void RefreshThumbnail () {
            if (!locked) {
                Thumbnail = Bitmap.GetThumbnailImage(ThumbnailSize, ThumbnailSize, null, System.IntPtr.Zero);
            }
        }

        /// <summary>
        /// Releases bitmap resources.
        /// </summary>
        public void Dispose () {
            Thumbnail.Dispose();
            Bitmap.Dispose();

            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// Represents a raster dataset that is in the ESRI binary format (.BIL, .BIP, .BSQ).
    /// </summary>
    public class Dataset
    {
        /// <summary>
        /// File name and path of the dataset.
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// The Header instance representing fields of metadata.
        /// </summary>
        public Header Header { get; private set; }

        /// <summary>
        /// List of raster bands in the dataset.
        /// </summary>
        public List<RasterBand> Bands = new List<RasterBand>();

        /// <summary>
        /// Creates a dataset instance.
        /// </summary>
        /// <param name="fn">The dataset's path.</param>
        private Dataset(string fn) {
            FileName = fn;
            MessageLog.Add(String.Format("Dataset path is {0}.", fn));
        // Parse header.
            Header = Header.ParseFromFile(Regex.Replace(fn, "\\.(bil|bsq|bip)", ".hdr", RegexOptions.IgnoreCase));

        // Create bands and lock their bitmap data.
            for (int i = 0; i < Header.NBands; i++) {
                RasterBand bd;
                Bands.Add(bd = new RasterBand(
                    (int)Header.NCols,
                    (int)Header.NRows,
                    i,
                    String.Format("Band #{0}", i + 1))
                );
                bd.LockBits();
            }
        }

        /// <summary>
        /// Creates an asynchronous operation for parsing a dataset.
        /// </summary>
        /// <param name="fn">The dataset's path.</param>
        /// <returns>A Tuple containing a BackgroundWorker representing the asynchronous operation and its parameter.</returns>
        public static BackgroundWorker Parse(string fn) {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (s, e) => {
                Dataset instance = new Dataset(fn);
                MessageLog.Add(String.Format("Starting async parse of {0} bands of binary data, pixel dimensions are {1}×{2}.",
                    instance.Header.NBands, instance.Header.NCols, instance.Header.NRows));

                // Start timekeeping.
                Stopwatch st = new Stopwatch();
                st.Start();

                try {
                    // Create a FileStream that is optimized for sequential scanning.
                    using (FileStream fs = new FileStream(
                        instance.FileName, FileMode.Open, FileAccess.Read, FileShare.Read,
                        0x100000, FileOptions.SequentialScan
                    )) {
                        BitmapData[] bd = (from RasterBand b in instance.Bands select b.BitmapData).ToArray();
                        int dataSize = (int)(instance.Header.NRows * instance.Header.NCols * instance.Header.NBands);

                        byte[] buffer = new byte[dataSize];
                        fs.Read(buffer, 0, (int)(dataSize));

                        unsafe {
                            // We read into unmanaged memory streams, as this seems to be faster than array operations.
                            UnmanagedMemoryStream[] ums = new UnmanagedMemoryStream[instance.Header.NBands];

                            for (int i = 0; i < instance.Header.NBands; i++) {
                                ums[i] = new UnmanagedMemoryStream((byte *) bd[i].Scan0, dataSize, dataSize, FileAccess.Write);
                            }

                            // Scanlines of GDI bitmaps must be memory aligned due to hardware requirements,
                            // we have to take this into account when reading.
                            int stridePadding = bd[0].Stride - (int) instance.Header.NCols;

                            if (stridePadding > 0) {
                                MessageLog.Add(String.Format("Taking scanline padding of {0} bytes into account.", stridePadding));
                            }

                            // We read data based on the file layout:
                            // BIL: rows -> bands -> a row of a band at a time
                            // BIP: rows -> cols -> bands -> a single pixel at a time
                            // BSQ: bands -> a whole band at a time

                            // We also check if the user has cancelled the operation and break accordingly.
                            switch (instance.Header.Layout) {
                                case Header.LayoutEnum.BIL:
                                    for (int y = 0; y < instance.Header.NRows; y++) {
                                        for (int b = 0; b < instance.Header.NBands; b++) {
                                            ums[b].Write(buffer,
                                                (int) ((y * instance.Header.NBands + b) * instance.Header.NCols),
                                                (int) instance.Header.NCols
                                            );

                                            // Seek to match stride size.
                                            ums[b].Seek(stridePadding, SeekOrigin.Current);
                                        }

                                        if (bw.CancellationPending) {
                                            e.Cancel = true;
                                            goto cleanup;
                                        }

                                        bw.ReportProgress((int) (((double) y / instance.Header.NRows) * 100));
                                    }
                                    break;

                                case Header.LayoutEnum.BIP:
                                    for (int y = 0; y < instance.Header.NRows; y++) {
                                        for (int x = 0; x < instance.Header.NCols; x++) {
                                            for (int b = 0; b < instance.Header.NBands; b++) {
                                                ums[b].Write(buffer,
                                                    (int) ((x + y * instance.Header.NCols) * instance.Header.NBands + b),
                                                    1
                                                );
                                            }
                                        }

                                        if (bw.CancellationPending) {
                                            e.Cancel = true;
                                            goto cleanup;
                                        }

                                        bw.ReportProgress((int) (((double) y / instance.Header.NRows) * 100));
                                    }
                                    break;

                                case Header.LayoutEnum.BSQ:
                                    for (int b = 0; b < instance.Header.NBands; b++) {
                                        ums[b].Write(buffer,
                                            (int) (b * instance.Header.NRows * instance.Header.NCols),
                                            (int) (instance.Header.NCols * instance.Header.NRows)
                                        );

                                        if (bw.CancellationPending) {
                                            e.Cancel = true;
                                            goto cleanup;
                                        }

                                        bw.ReportProgress((int) ((double) b / instance.Header.NBands));
                                    }

                                    break;
                            }
                        }
                    }
                } catch (Exception ex) {
                    // Throw if anything unexpected happens.
                    throw new ParseException(ex.Message);
                }

            cleanup:
                // Unlock bitmaps.
                instance.Bands.ForEach(b => b.UnlockBits());
                e.Result = instance;

                // Stop timekeeping and log.
                st.Stop();
                MessageLog.Add(String.Format("Parsing took {0} msecs.", st.ElapsedMilliseconds));
            };

            return bw;
        }
    }
}