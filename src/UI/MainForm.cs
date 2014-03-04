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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

using ESRIBinaryViewer.GFX;
using ESRIBinaryViewer.Model;

namespace ESRIBinaryViewer.UI
{
    /// <summary>
    /// The applications main window.
    /// </summary>
    public partial class MainForm : Form
    {
        #region Private fields
        /// <summary>
        ///  The currently displayed dataset.
        /// </summary>
        private Dataset dataset;

        /// <summary>
        /// The currently running background operation.
        /// </summary>
        private BackgroundWorker currentBackgroundOperation;

        /// <summary>
        /// True, if automatic update of the RGB composite is temporarily suspended; false, otherwise.
        /// </summary>
        private bool suspendCompositeUpdate = false;

        /// <summary>
        /// True, if the composite has been changed and the histogram chart needs updating; false, otherwise.
        /// </summary>
        private bool histogramDirty = true;

        /// <summary>
        /// Contains the button in the band list header for equalizing all band's histograms.
        /// </summary>
        private Button btnHistEqAll;

        /// <summary>
        /// Contains a list of band names for the RGB channel selection combos.
        /// </summary>
        private List<string> bandList = new List<string>() { "(none)" };

        /// <summary>
        /// Contains the RGB channel selection combos for common operations.
        /// </summary>
        private List<ToolStripComboBox> rgbCombos;
        #endregion

        #region Form initialization
        public MainForm() {
            InitializeComponent();
        }

        /// <summary>
        /// Loads the form and initializes controls.
        /// </summary>
        private void MainForm_Load(object sender, EventArgs e) {

        // Set up the band list.
            dgvBands.AutoGenerateColumns =
            dgvHeaderAttributes.AutoGenerateColumns =
            dgvEventLog.AutoGenerateColumns =
                false;
            dgvEventLog.DataSource = MessageLog.Events;

        // Add button to band list header for equalizing histograms on all bands.
            btnHistEqAll = new Button();
            Rectangle btnPos = dgvBands.GetCellDisplayRectangle(2, -1, true);
            btnHistEqAll.Text = "";
            btnHistEqAll.Image = ESRIBinaryViewer.Properties.Resources.HistEQ;
            btnHistEqAll.Size = btnPos.Size;
            btnHistEqAll.Location = btnPos.Location;
            btnHistEqAll.Font = new Font(btnHistEqAll.Font.FontFamily, 8);
            btnHistEqAll.Visible = false;

            btnHistEqAll.Click += (o, a) => {
                addBackgroundOperation(
                    GraphicOperations.EqualizeHistogram(dataset.Bands),
                    "Equalizing histograms on all bands of the current dataset.",
                    (ps, pe) => {
                        dgvBands.Refresh();
                        updateRGBComposite();
                    }
                );
            };

        // Manually reposition & resize the button in the header...
            EventHandler eh = (o, a) => {
                Rectangle r = dgvBands.GetCellDisplayRectangle(2, -1, true);
                btnHistEqAll.Size = r.Size;
                btnHistEqAll.Location = r.Location;
            };

        // ... on size change or data source change.
            dgvBands.SizeChanged += eh;
            dgvBands.DataSourceChanged += eh;
            dgvBands.DataSourceChanged += (o, a) => { btnHistEqAll.Visible = true; };
            dgvBands.Controls.Add(btnHistEqAll);

        // Set up RGB band selection combo boxes.
            rgbCombos = new List<ToolStripComboBox>() { cmbR, cmbG, cmbB };
            rgbCombos.ForEach(i => {
                i.SelectedIndexChanged += (o, a) => updateRGBComposite();
                i.ComboBox.DataSource = new BindingSource(bandList, "");
                i.SelectedIndex = 0;
            });

        // Set up the zoom combo box in the status bar.
            var zoomItems = pbxDisplay.ZoomFactors.Select(
                f => {
                    var i = new ToolStripMenuItem(String.Format("{0:0}%", f * 100));
                    i.Click += (o, a) => {
                        foreach (var mi in (from object obj in i.Owner.Items
                                            let mi = obj as ToolStripMenuItem
                                            where mi != null
                                            select mi)) {
                            mi.Checked = false;
                            if (mi.Text == "100%") {
                                mi.Font = new Font(mi.Font, FontStyle.Bold);
                                mi.Checked = true;
                            }
                        }
                        i.Checked = true;
                        pbxDisplay.CurrentZoom = ddnZoom.DropDownItems.IndexOf(i);
                    };
                    return i;
                }
            ).ToArray();
            ddnZoom.DropDownItems.AddRange(zoomItems);

        // Hide the histogram panel.
            splDisplay.Panel2Collapsed = true;
            splDisplay.Panel2.Hide();

        // Set up the custom menu item for applying convolution filters.
            var convoItem = new ConvolutionFilterMenuItem();
            convoItem.FilterApplied += (k) => {
                addBackgroundOperation(
                    GraphicOperations.ApplyConvolutionFilter((Bitmap) pbxDisplay.Image, k),
                    "Applying convolution filter",
                    (ps, pe) => { pbxDisplay.Image = (Bitmap) pe.Result; }
                );
            };
            mnuConvoFilter.DropDownItems.Add(convoItem);

        // Also set up the custom menu item for applying a median filter.
            var medianItem = new MedianFilterMenuItem();
            medianItem.FilterApplied += (s) => {
                addBackgroundOperation(
                    GraphicOperations.ApplyMedianFilter((Bitmap) pbxDisplay.Image, s),
                    "Applying median filter",
                    (ps, pe) => { pbxDisplay.Image = (Bitmap) pe.Result; }
                );
            };
            mnuMedianFilter.DropDownItems.Add(medianItem);

        // Compute build timestamp from the assembly version number.
            var version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
            var buildDateTime = new DateTime(2000, 1, 1).Add(new TimeSpan(
                TimeSpan.TicksPerDay * version.Build +
                TimeSpan.TicksPerSecond * 2 * version.Revision)
            );

        // Print welcome banner to the message log.
            MessageLog.Add(String.Format("ESRIBinaryViewer {0}; built on {1}.", version, buildDateTime));
            MessageLog.Add("(C) GPLv3 2013 Barancsuk Ádám <adam.barancsuk@gmail.com>");
        }
        #endregion

        #region Main toolbar event handlers
        /// <summary>
        /// Loads a raster dataset and updates data bound controls on success.
        /// An RGB composite is generated if the dataset has three or more bands,
        /// otherwise a grayscale image of the first band is shown.
        /// </summary>
        private void btnLoadBIL_Click(object sender, EventArgs e) {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "ESRI binary raster files|*.bil; *.bip; *.bsq";

            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                addBackgroundOperation(
                    Dataset.Parse(fd.FileName),
                    String.Format("Loading dataset {0}", fd.FileName),
                    (ps, pe) => {
                        dataset = null;

                        // Temporarily disable automatic updating of the composite.
                            suspendCompositeUpdate = true;
                            dataset = pe.Result as Dataset;

                        // Enable controls and update the data-bound ones.
                            dgvHeaderAttributes.DataSource = dataset.Header;
                            dgvBands.DataSource = dataset.Bands;
                            lblFileInfo.Text = dataset.FileName;
                            btnExportImage.Enabled = true;
                            btnSwapRB.Enabled = true;
                            btnDefaultRGB.Enabled = true;
                            ddnFilters.Enabled = true;
                            ddnHistogram.Enabled = true;
                            ddnZoom.Enabled = true;
                            ddnInterpolationMethod.Enabled = true;
                            pbxDisplay.Enabled = true;

                            ddnCoords.Enabled = true;
                            var hasMapCoordinates = (dataset.Header.ULXMap > 0 && dataset.Header.ULYMap > 0);
                            ddiCoordsMap.Enabled = ddiCoordsMap.Checked = hasMapCoordinates;
                            ddiCoordsImage.Checked = !hasMapCoordinates;

                        // Populate band list and band selection combo boxes.
                            bandList.Clear();
                            bandList.Add("(none)");
                            bandList.AddRange(from RasterBand b in dataset.Bands select b.Name);
                            rgbCombos.ForEach(i => {
                                i.ComboBox.DataSource = new BindingSource(bandList, "");
                                i.Enabled = true;
                            });

                        // Select what to show based on band count.
                            if (dataset.Bands.Count >= 3) {
                                int idx = 1;
                                rgbCombos.ForEach(i => i.SelectedIndex = idx++);
                            } else {
                                rgbCombos.ForEach(i => i.SelectedIndex = 1);
                            }

                        // Update the composite.
                            suspendCompositeUpdate = false;
                            updateRGBComposite();

                    }
                );                
            }
        }

        /// <summary>
        /// Handles image export.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportImage_Click (object sender, EventArgs e) {
            SaveFileDialog fd = new SaveFileDialog();
            fd.Filter = ".JPEG|*.jpg;*.jpeg|.PNG|*.png|.GIF|*.gif";

            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                // File format selection is handled implicitly as Image.Save(string)
                // can decide what format to emit based on the file name.
                pbxDisplay.Image.Save(fd.FileName);
            }
        }

        /// <summary>
        /// Displays the about box.
        /// </summary>
        private void mnuAbout_Click (object sender, EventArgs e) {
            AboutBox.Instance.ShowDialog(this);
        }

        /// <summary>
        /// Displays the help window.
        /// </summary>
        private void mnuHelp_Click (object sender, EventArgs e) {
            HelpWindow.Instance.Show(this);
        }
        #endregion

        #region Sidebar event handlers
        /// <summary>
        /// Handles clicking on buttons in the band list.
        /// Equalizes histogram or selects a single band for grayscale display based on the column
        /// that was clicked.
        /// </summary>
        private void dgvBands_CellContentClick (object sender, DataGridViewCellEventArgs e) {
            if (e.ColumnIndex == 2) {
                addBackgroundOperation(
                    GraphicOperations.EqualizeHistogram(dataset.Bands[e.RowIndex]),
                    String.Format("Equalizing histogram on {0}", dataset.Bands[e.RowIndex].Name),
                    (ps, pe) => {
                        dgvBands.Refresh();
                        updateRGBComposite();
                    }
                );
            } else if (e.ColumnIndex == 3) {
                suspendCompositeUpdate = true;

                rgbCombos.ForEach(i => i.SelectedIndex = e.RowIndex + 1);

                suspendCompositeUpdate = false;
                updateRGBComposite();
            }
        }

        /// <summary>
        /// Paints images on buttons in the band list.
        /// </summary>
        private void dgvBands_CellPainting (object sender, DataGridViewCellPaintingEventArgs e) {
            if (e.ColumnIndex >= 2 && e.RowIndex > -1) {

                // Decide which image to use based on column index.
                Image i = e.ColumnIndex == 2 ?
                    ESRIBinaryViewer.Properties.Resources.HistEQ :
                    ESRIBinaryViewer.Properties.Resources.Mono;

                // Paint cell background, then content (an empty button).
                e.PaintBackground(e.CellBounds, e.State.HasFlag(DataGridViewElementStates.Selected));
                e.PaintContent(e.CellBounds);

                // Draw the image on top of the button.
                e.Graphics.DrawImage(i, new Point(
                    e.CellBounds.X + (e.CellBounds.Width - i.Width) / 2,
                    e.CellBounds.Y + (e.CellBounds.Height - i.Height) / 2
                ));

                e.Handled = true;
            }
        }

        #endregion

        #region Image manipulation toolbar event handlers
        /// <summary>
        /// Swaps the red and blue bands of the composite image.
        /// Useful because of the inconsistent order (RGB/BGR) of bands among datasets from different sources.
        /// </summary>
        private void btnSwapRB_Click (object sender, EventArgs e) {
            suspendCompositeUpdate = true;

            int temp = cmbR.SelectedIndex;
            cmbR.SelectedIndex = cmbB.SelectedIndex;
            cmbB.SelectedIndex = temp;

            suspendCompositeUpdate = false;
            updateRGBComposite();
        }

        /// <summary>
        /// Resets the composite view to its default state.
        /// </summary>
        private void btnDefaultRGBComposite_Click (object sender, EventArgs e) {
            suspendCompositeUpdate = true;

            if (dataset.Bands.Count() >= 3) {
                cmbR.SelectedIndex = 1;
                cmbG.SelectedIndex = 2;
                cmbB.SelectedIndex = 3;
            } else {
                cmbR.SelectedIndex =
                cmbG.SelectedIndex =
                cmbB.SelectedIndex = 1;
            }
            suspendCompositeUpdate = false;
            updateRGBComposite();
        }

        /// <summary>
        /// Inverts the current composite image.
        /// </summary>
        private void mnuInvert_Click (object sender, EventArgs e) {
            addBackgroundOperation(
                GraphicOperations.ApplyInvertFilter((Bitmap) pbxDisplay.Image),
                "Applying invert filter",
                (ps, pe) => { pbxDisplay.Image = (Bitmap) pe.Result; }
            );
        }

        /// <summary>
        /// Converts the current composite image to grayscale.
        /// </summary>
        private void mnuGrayscale_Click (object sender, EventArgs e) {
            addBackgroundOperation(
                GraphicOperations.ApplyGrayscaleFilter((Bitmap) pbxDisplay.Image),
                "Applying grayscale filter",
                (ps, pe) => { pbxDisplay.Image = (Bitmap) pe.Result; }
            );
        }

        /// <summary>
        /// Equalizes histograms of bands of the current RGB composite.
        /// </summary>
        private void mnuEqualizeHistogram_Click (object sender, EventArgs e) {
            addBackgroundOperation(
                GraphicOperations.EqualizeHistogram(new[] {
                        (cmbR.SelectedIndex > 0) ? dataset.Bands[cmbR.SelectedIndex - 1] : null,
                        (cmbG.SelectedIndex > 0) ? dataset.Bands[cmbG.SelectedIndex - 1] : null,
                        (cmbB.SelectedIndex > 0) ? dataset.Bands[cmbB.SelectedIndex - 1] : null
                }),
                "Equalizing histogram on the RGB composite.",
                (ps, pe) => {
                    dgvBands.Refresh();
                    updateRGBComposite();
                });
        }

        /// <summary>
        /// Toggles display of the histogram panel.
        /// </summary>
        private void mnuShowHistogram_Click (object sender, EventArgs e) {
            if (splDisplay.Panel2Collapsed) {
                mnuShowHistogram.Checked = true;
                splDisplay.Panel2Collapsed = false;
                splDisplay.Panel2.Show();
                // We are only updated here if the composite view is changed since the last
                // time we were shown.
                maybeUpdateHistogram();
            } else {
                mnuShowHistogram.Checked = false;
                splDisplay.Panel2Collapsed = true;
                splDisplay.Panel2.Hide();
            }
        }
        #endregion

        #region Main picture box event handlers
        /// <summary>
        /// Handles updating the coordinate display in the status bar.
        /// </summary>
        private void pbxDisplay_MouseMove (object sender, MouseEventArgs e) {
            PointF cip = pbxDisplay.CurrentImagePosition;

            if (ddiCoordsMap.Checked) {
                ddnCoords.Text = String.Format("Map: ({0:0.000}; {1:0.000})",
                    dataset.Header.ULXMap + (cip.X * dataset.Header.XDim),
                    dataset.Header.ULYMap + (cip.Y * dataset.Header.YDim)
                );
            } else {
                ddnCoords.Text = String.Format("Image: ({0:0}; {1:0})", cip.X, cip.Y);
            }
        }

        /// <summary>
        /// Handles zoom change in the composite view.
        /// </summary>
        private void pbxDisplay_ZoomChanged (object sender, EventArgs e) {
            ddnZoom.Text = String.Format("{0:0}%", pbxDisplay.ZoomFactors[pbxDisplay.CurrentZoom] * 100);
            foreach (var mi in (from object obj in ddnZoom.DropDownItems
                                let mi = obj as ToolStripMenuItem
                                where mi != null
                                select mi)) {
                mi.Checked = false;
            }
            (ddnZoom.DropDownItems[pbxDisplay.CurrentZoom] as ToolStripMenuItem).Checked = true;
        }

        private void pbxDisplay_CopyCoords (object sender, bool mapCoordinates) {
            PointF cip = pbxDisplay.CurrentImagePosition;
            if (mapCoordinates) {
                Clipboard.SetText(String.Format("({0:0.000}; {1:0.000})",
                        dataset.Header.ULXMap + (cip.X * dataset.Header.XDim),
                        dataset.Header.ULYMap + (cip.Y * dataset.Header.YDim)
                    ));
            } else {
                Clipboard.SetText(String.Format("({0:0}; {1:0})", cip.X, cip.Y));
            }
        }
        #endregion

        #region Status bar event handlers

        /// <summary>
        /// Enables display of projected coordinates in the status bar.
        /// </summary>
        private void ddiCoordsMap_Click(object sender, EventArgs e) {
            ddiCoordsImage.Checked = false;
        }

        /// <summary>
        /// Enables display of image coordinates in the status bar.
        /// </summary>
        private void ddiCoordsImage_Click(object sender, EventArgs e) {
            ddiCoordsMap.Checked = false;
        }

        /// <summary>
        /// Handles changing of the interpolation method.
        /// </summary>
        private void ddnInterpolationMethod_DropDownItemClicked (object sender, ToolStripItemClickedEventArgs e) {
            switch (e.ClickedItem.Text) {
                case "Bicubic":
                    pbxDisplay.InterpolationMode = InterpolationMode.Bicubic;
                    break;
                case "Bicubic (HQ)":
                    pbxDisplay.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    break;
                case "Bilinear":
                    pbxDisplay.InterpolationMode = InterpolationMode.Bilinear;
                    break;
                case "Bilinear (HQ)":
                    pbxDisplay.InterpolationMode = InterpolationMode.HighQualityBilinear;
                    break;
                case "Nearest neighbor":
                    pbxDisplay.InterpolationMode = InterpolationMode.NearestNeighbor;
                    break;
                case "(default)":
                    pbxDisplay.InterpolationMode = InterpolationMode.Default;
                    break;
            }

            foreach (ToolStripMenuItem i in ddnInterpolationMethod.DropDownItems) {
                i.Checked = false;
            }

            (e.ClickedItem as ToolStripMenuItem).Checked = true;
            ddnInterpolationMethod.Text = e.ClickedItem.Text;
            pbxDisplay.Refresh();
        }

        private void lblAsyncOpFailure_Click (object sender, EventArgs e) {
            lblAsyncOpFailure.Visible = false;
            lblFileInfo.Visible = true;
            tcViewSwitcher.SelectedIndex = 1;
        }
        #endregion

        #region Common methods
        /// <summary>
        /// Updates the RGB composite view.
        /// </summary>
        private void updateRGBComposite () {
            if (dataset == null || suspendCompositeUpdate) {
                return;
            }

            addBackgroundOperation(
                GraphicOperations.CreateRGBComposite(
                    dataset,
                    cmbR.SelectedIndex - 1,
                    cmbG.SelectedIndex - 1,
                    cmbB.SelectedIndex - 1
                ),
                "Generating RGB composite",
                (ps, pe) => {
                    pbxDisplay.Image = pe.Result as Bitmap;

                    // Mark histogram chart as dirty and update.
                    histogramDirty = true;
                    maybeUpdateHistogram();
                }
            );
        }

        /// <summary>
        /// Starts a new asynchronous operation.
        /// </summary>
        /// <param name="bw">A tuple of a BackgroundWorker object and its parameter, representing the operation.</param>
        /// <param name="message">The human readable message to be shown during the operation is running.</param>
        /// <param name="rwceh">The callback to be run when the operation is finished or cancelled.</param>
        private void addBackgroundOperation (BackgroundWorker bw, string message, RunWorkerCompletedEventHandler rwceh) {

            // We cancel any currently running operations before starting a new one.
            // This might seem pointless but we don't actually have any operations that
            // can be run in parallel. The reason for having async operations is not
            // parallelism, but rather that we don't want to perform resource-heavy
            // operations on the UI thread. We also want async ops to report their progress
            // and be cancellable by the user if they're taking too much time.

            if (currentBackgroundOperation != null) {
                currentBackgroundOperation.CancelAsync();
            }

            EventHandler cancelEventHandler = (o, a) => { currentBackgroundOperation.CancelAsync(); };

            // Set up event handlers on the BackgroundWorker.
            currentBackgroundOperation = bw;
            currentBackgroundOperation.ProgressChanged += (ps, pe) => { pgbAsyncOpProgress.Value = pe.ProgressPercentage; };
            currentBackgroundOperation.RunWorkerCompleted += (o, a) => {
                lblAsyncOpInfo.Text = "";
                lblAsyncOpInfo.Click -= cancelEventHandler;
                lblAsyncOpInfo.Visible = false;
                pgbAsyncOpProgress.Visible = false;
                lblFileInfo.Visible = true;

                if (a.Error != null) {
                    // Show the error in the status bar, if there was one.
                    MessageLog.Add(
                        String.Format(
                            "An exception was thrown while executing the last operation: \n{0}",
                            a.Error.Message
                        ),
                        MessageLog.EventType.ERROR);
                    lblFileInfo.Visible = false;
                    lblAsyncOpFailure.Visible = true;

                } else if (a.Cancelled) {
                    MessageLog.Add("Operation cancelled.");

                } else {
                    rwceh(o, a);
                }
            };

            // Set up UI.
            lblFileInfo.Visible = false;
            lblAsyncOpFailure.Visible = false;
            lblAsyncOpInfo.Visible = true;
            lblAsyncOpInfo.Text = String.Format("{0} - {1}", message, "Click here to cancel operation...");
            lblAsyncOpInfo.Click += cancelEventHandler;

            pgbAsyncOpProgress.Value = 0;
            pgbAsyncOpProgress.Visible = true;

            // Enable progress reporting and cancellation, then start the BackgroundWorker.
            currentBackgroundOperation.WorkerReportsProgress = true;
            currentBackgroundOperation.WorkerSupportsCancellation = true;
            currentBackgroundOperation.RunWorkerAsync();
        }

        /// <summary>
        /// Updates the histogram chart if it is marked dirty and is visible.
        /// </summary>
        private void maybeUpdateHistogram () {
            // We check
            if (!splDisplay.Panel2Collapsed && histogramDirty) {
                addBackgroundOperation(
                    GraphicOperations.CalculateHistogram(new[] {
                        (cmbR.SelectedIndex > 0) ? dataset.Bands[cmbR.SelectedIndex - 1] : null,
                        (cmbG.SelectedIndex > 0) ? dataset.Bands[cmbG.SelectedIndex - 1] : null,
                        (cmbB.SelectedIndex > 0) ? dataset.Bands[cmbB.SelectedIndex - 1] : null
                    }),
                    "Calculating histogram of the RGB composite.",
                    (ps, pe) => {
                        double[][] data = pe.Result as double[][];

                        // Bind histogram values to the chart series.
                        chtHistogram.Series["R"].Points.DataBindY(data[0]);
                        chtHistogram.Series["G"].Points.DataBindY(data[1]);
                        chtHistogram.Series["B"].Points.DataBindY(data[2]);
                        histogramDirty = false;
                    });
            }
        }
        #endregion
    }
}