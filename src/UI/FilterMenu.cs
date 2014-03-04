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
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using ESRIBinaryViewer.GFX;

namespace ESRIBinaryViewer.UI
{
    /// <summary>
    /// Event handler delegate for convolution filter application.
    /// </summary>
    /// <param name="convolutionKernel">The convolution's kernel.</param>
    public delegate void ApplyConvolutionFilterEventHandler (double[] convolutionKernel);

    /// <summary>
    /// Event handler delegate for median filter application.
    /// </summary>
    /// <param name="kernelSize">The filter's kernel size.</param>
    public delegate void ApplyMedianFilterEventHandler (int kernelSize);

    /// <summary>
    /// Provides a container for the convolution filter's option panel (see <see cref="ConvolutionFilterPanel"/>).
    /// </summary>
    class ConvolutionFilterMenuItem : ToolStripControlHost
    {
        public event ApplyConvolutionFilterEventHandler FilterApplied;

        public ConvolutionFilterMenuItem () : base(new ConvolutionFilterPanel()) {
            // Pass on FilterApplied events coming from the panel
            (this.Control as ConvolutionFilterPanel).FilterApplied += (m) => {
                if (FilterApplied != null) {
                    FilterApplied(m);
                }
            };
        }

        public override Size GetPreferredSize (Size constrainingSize) {
            return Control.Size;
        }
    }

    /// <summary>
    /// Defines a custom panel for setting the convolution filter's options.
    /// </summary>
    class ConvolutionFilterPanel : Panel
    {
        private const int textBoxSize = 25;
        private const int optionsRowHeight = 25;
        private const int labelWidth = 80;
        private const int upDownWidth = 40;
        private const int maxMatSize = 15;
        private int matSize = 8;
        private DataGridView mat = new DataGridView();
        private TableLayoutPanel tlp = new TableLayoutPanel();
        private MenuStrip mnu = new MenuStrip();
        private NumericUpDown nudMatSize = new NumericUpDown();
        private Label lblMatSize = new Label();

        public event ApplyConvolutionFilterEventHandler FilterApplied;

        public double[] ConvolutionKernel {
            get {
                double[] value = new double[matSize * matSize];
                for (int x = 0; x < matSize; x++) {
                    for (int y = 0; y < matSize; y++) {
                        value[x * matSize + y] = Convert.ToDouble(mat.Rows[y].Cells[x].Value);
                    }
                }

                return value;
            }

            set {
                matSize = (int) Math.Sqrt(value.Length);
                mat.Rows.Clear();
                mat.Columns.Clear();

                for (int x = 0; x < matSize; x++) {
                    var col = new DataGridViewColumn();
                    col.Width = textBoxSize;
                    mat.Columns.Add(col);
                }

                for (int x = 0; x < matSize; x++) {
                    var row = new DataGridViewRow();
                    row.Height = textBoxSize;

                    for (int y = 0; y < matSize; y++) {
                        row.Cells.Add(new DataGridViewTextBoxCell() { Value = value[x * matSize + y] });
                    }
                    mat.Rows.Add(row);
                }

                nudMatSize.Value = matSize;

                mat.Size = new Size(matSize * textBoxSize, matSize * textBoxSize);
                mat.MinimumSize = new Size(matSize * textBoxSize + 4, matSize * textBoxSize + 4);
                tlp.MinimumSize = new Size(
                    mnu.Width + labelWidth + upDownWidth + 20,
                    Math.Max(matSize * textBoxSize + 2 * optionsRowHeight + 40, mnu.Height)
                );
                tlp.Size = new Size(
                    matSize * textBoxSize + mnu.Width + 20,
                    Math.Max(matSize * textBoxSize + 2 * optionsRowHeight + 40, mnu.Height)
                );
                Console.WriteLine(Math.Max(matSize * textBoxSize + 2 * optionsRowHeight + 40, mnu.Height));
                this.MinimumSize = tlp.MinimumSize;
                this.Size = tlp.Size;
            }
        }

        public ConvolutionFilterPanel () : base() {
            ComboBox cmb = new ComboBox();
            Button btn = new Button();
            TableLayoutPanel pnl = new TableLayoutPanel();

            mnu.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;

            ToolStripLabel lblPresets = new ToolStripLabel("Presets:");
            lblPresets.TextAlign = ContentAlignment.MiddleLeft;
            lblPresets.Font = new Font(lblPresets.Font, FontStyle.Bold);
            lblPresets.Height = optionsRowHeight;
            lblPresets.Dock = DockStyle.Fill;
            mnu.Items.Add(lblPresets);

            foreach (var f in GraphicOperations.ConvolutionFilterPresets) {
                var i = mnu.Items.Add(f.name);
                i.TextAlign = ContentAlignment.MiddleLeft;
                i.Click += (o, e) => {
                    ConvolutionKernel = (from ft in GraphicOperations.ConvolutionFilterPresets
                                         where ft.name == (o as ToolStripItem).Text
                                         select ft.kernel).First();
                };
                i.DoubleClick += (o, e) => {
                    FilterApplied((from ft in GraphicOperations.ConvolutionFilterPresets
                                   where ft.name == (o as ToolStripItem).Text
                                   select ft.kernel).First());
                };
            }

            var defaultItem = mnu.Items.Add("(custom)");
            defaultItem.TextAlign = ContentAlignment.MiddleLeft;
            defaultItem.Font = new Font(defaultItem.Font, FontStyle.Italic);
            defaultItem.Click += (o, e) => { ConvolutionKernel = new[] { 0d, 0, 0, 0, 0, 0, 0, 0, 0 }; };
            defaultItem.DoubleClick += (o, e) => { FilterApplied(ConvolutionKernel); };

            btn.Text = "Apply filter";
            btn.MinimumSize = new Size(0, optionsRowHeight);
            btn.Click += (o, e) => {
                FilterApplied(ConvolutionKernel);
            };

            nudMatSize.Size = new Size(upDownWidth, optionsRowHeight);
            nudMatSize.Minimum = 3;
            nudMatSize.Maximum = maxMatSize;
            nudMatSize.Increment = 2;
            nudMatSize.ValueChanged += (o, e) => {
                // If the value has changed, we update the matrix size,
                // but keep whatever content we already have
                // (with cropping or extending, ofc).
                double[] prevKernel = ConvolutionKernel;
                int prevMatSize = matSize;
                int newSize = (int) nudMatSize.Value;
                double[] newKernel = new double[newSize * newSize];
                for (int x = 0; x < newSize; x++) {
                    for (int y = 0; y < newSize; y++) {
                        newKernel[newSize * x + y] =
                            (x < prevMatSize && y < prevMatSize) ?
                            prevKernel[x * prevMatSize + y] : 0;
                    }
                    ConvolutionKernel = newKernel;
                }
            };

            lblMatSize.Size = new Size(labelWidth, optionsRowHeight);
            lblMatSize.TextAlign = ContentAlignment.MiddleLeft;
            lblMatSize.Text = "Kernel size:";
            lblMatSize.Font = new Font(lblMatSize.Font, FontStyle.Bold);

            pnl.RowCount = 1;
            pnl.ColumnCount = 2;
            pnl.Controls.Add(lblMatSize, 0, 0);
            pnl.Controls.Add(nudMatSize, 1, 0);

            tlp.Parent = this;
            tlp.RowCount = 3;
            tlp.ColumnCount = 2;

            tlp.Controls.Add(mnu, 0, 0);
            tlp.SetRowSpan(mnu, 3);
            tlp.Controls.Add(pnl, 1, 0);
            tlp.Controls.Add(mat, 1, 1);
            tlp.Controls.Add(btn, 1, 2);
            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));

            mat.RowCount = mat.ColumnCount = matSize;
            mat.Padding = new Padding(0);
            mat.Dock = DockStyle.Top;
            mat.BorderStyle = System.Windows.Forms.BorderStyle.None;
            mat.ScrollBars = ScrollBars.None;
            mat.Size = new Size(matSize * textBoxSize, matSize * textBoxSize);
            mat.MinimumSize = new Size(matSize * textBoxSize, matSize * textBoxSize);
            mat.SelectionMode = DataGridViewSelectionMode.CellSelect;
            mat.RowHeadersVisible = false;
            mat.ColumnHeadersVisible = false;
            mat.AllowUserToResizeColumns = false;
            mat.AllowUserToResizeRows = false;
            mat.AllowUserToAddRows = false;
            mat.BackgroundColor = SystemColors.Control;

            ConvolutionKernel = new[] { 0d, 0, 0, 0, 0, 0, 0, 0, 0 };
        }
    }

    /// <summary>
    /// Provides a container for the median filter's option panel (see <see cref="MedianFilterPanel"/>).
    /// </summary>
    class MedianFilterMenuItem : ToolStripControlHost
    {
        public event ApplyMedianFilterEventHandler FilterApplied;

        public MedianFilterMenuItem () : base(new MedianFilterPanel()) {
            // Pass on FilterApplied events coming from the panel
            (this.Control as MedianFilterPanel).FilterApplied += (m) => {
                if (FilterApplied != null) {
                    FilterApplied(m);
                }
            };
        }

        public override Size GetPreferredSize (Size constrainingSize) {
            return Control.Size;
        }
    }

    /// <summary>
    /// Defines a custom panel for setting the median filter's options.
    /// </summary>
    class MedianFilterPanel : Panel
    {
        private const int optionsRowHeight = 20;
        private const int labelWidth = 60;
        private const int upDownWidth = 40;
        private NumericUpDown nudKernelSize = new NumericUpDown();
        private Label lblKernelSize = new Label();

        public event ApplyMedianFilterEventHandler FilterApplied;

        public int KernelSize {
            get {
                return (int) nudKernelSize.Value;
            }
        }

        public MedianFilterPanel () : base() {
            ComboBox cmb = new ComboBox();
            Button btn = new Button();
            TableLayoutPanel tlp = new TableLayoutPanel();

            lblKernelSize.Size = new Size(labelWidth, optionsRowHeight);
            lblKernelSize.TextAlign = ContentAlignment.MiddleLeft;
            lblKernelSize.Text = "Kernel size:";
            lblKernelSize.Font = new Font(lblKernelSize.Font, FontStyle.Bold);
            lblKernelSize.Dock = DockStyle.Fill;

            nudKernelSize.Size = new Size(upDownWidth, optionsRowHeight);
            nudKernelSize.Minimum = 3;
            nudKernelSize.Maximum = 15;
            nudKernelSize.Increment = 2;
            nudKernelSize.Dock = DockStyle.Fill;

            btn.Text = "Apply filter";
            btn.MinimumSize = new Size(0, optionsRowHeight);
            btn.Click += (o, e) => {
                FilterApplied(KernelSize);
            };
            btn.Dock = DockStyle.Fill;

            tlp.Parent = this;
            tlp.RowCount = 1;
            tlp.ColumnCount = 3;
            tlp.Size = new Size(200, optionsRowHeight + 10);

            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, optionsRowHeight));
            tlp.Controls.Add(lblKernelSize, 0, 0);
            tlp.Controls.Add(nudKernelSize, 1, 0);
            tlp.Controls.Add(btn, 2, 0);

            this.Height = optionsRowHeight + 10;
         }
    }
}
