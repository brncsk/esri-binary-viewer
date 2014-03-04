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

namespace ESRIBinaryViewer.UI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.stsMain = new System.Windows.Forms.StatusStrip();
            this.pgbAsyncOpProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.lblAsyncOpInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblAsyncOpFailure = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblFileInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSpring = new System.Windows.Forms.ToolStripStatusLabel();
            this.ddnCoords = new System.Windows.Forms.ToolStripDropDownButton();
            this.ddiCoordsMap = new System.Windows.Forms.ToolStripMenuItem();
            this.ddiCoordsImage = new System.Windows.Forms.ToolStripMenuItem();
            this.ddnZoom = new System.Windows.Forms.ToolStripDropDownButton();
            this.ddnInterpolationMethod = new System.Windows.Forms.ToolStripDropDownButton();
            this.nearestNeighborToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bilinearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bilinearHQToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bicubicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bicubicHQToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tstMain = new System.Windows.Forms.ToolStrip();
            this.btnLoadBIL = new System.Windows.Forms.ToolStripButton();
            this.btnExportImage = new System.Windows.Forms.ToolStripButton();
            this.ddnHelp = new System.Windows.Forms.ToolStripDropDownButton();
            this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.splMain = new System.Windows.Forms.SplitContainer();
            this.splLeft = new System.Windows.Forms.SplitContainer();
            this.dgvHeaderAttributes = new System.Windows.Forms.DataGridView();
            this.dgcAttributeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgcAttributeValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvBands = new System.Windows.Forms.DataGridView();
            this.dgcBandThumbnail = new System.Windows.Forms.DataGridViewImageColumn();
            this.dgcBandName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgcBandHistEq = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgcBandSetAsMono = new System.Windows.Forms.DataGridViewButtonColumn();
            this.tcViewSwitcher = new System.Windows.Forms.TabControl();
            this.tpDisplay = new System.Windows.Forms.TabPage();
            this.splDisplay = new System.Windows.Forms.SplitContainer();
            this.pbxDisplay = new ESRIBinaryViewer.UI.PanZoomPictureBox();
            this.chtHistogram = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tspDisplay = new System.Windows.Forms.ToolStrip();
            this.lblR = new System.Windows.Forms.ToolStripLabel();
            this.cmbR = new System.Windows.Forms.ToolStripComboBox();
            this.lblG = new System.Windows.Forms.ToolStripLabel();
            this.cmbG = new System.Windows.Forms.ToolStripComboBox();
            this.lblB = new System.Windows.Forms.ToolStripLabel();
            this.cmbB = new System.Windows.Forms.ToolStripComboBox();
            this.ddnHistogram = new System.Windows.Forms.ToolStripDropDownButton();
            this.mnuShowHistogram = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEqualizeHistogram = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSwapRB = new System.Windows.Forms.ToolStripButton();
            this.btnDefaultRGB = new System.Windows.Forms.ToolStripButton();
            this.ddnFilters = new System.Windows.Forms.ToolStripDropDownButton();
            this.mnuInvert = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuGrayscale = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMedianFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuConvoFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.tpMessageLog = new System.Windows.Forms.TabPage();
            this.dgvEventLog = new System.Windows.Forms.DataGridView();
            this.dgcEventTypeIcon = new System.Windows.Forms.DataGridViewImageColumn();
            this.dgcEventTimestamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgcEventMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stsMain.SuspendLayout();
            this.tstMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splMain)).BeginInit();
            this.splMain.Panel1.SuspendLayout();
            this.splMain.Panel2.SuspendLayout();
            this.splMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splLeft)).BeginInit();
            this.splLeft.Panel1.SuspendLayout();
            this.splLeft.Panel2.SuspendLayout();
            this.splLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHeaderAttributes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBands)).BeginInit();
            this.tcViewSwitcher.SuspendLayout();
            this.tpDisplay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splDisplay)).BeginInit();
            this.splDisplay.Panel1.SuspendLayout();
            this.splDisplay.Panel2.SuspendLayout();
            this.splDisplay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxDisplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chtHistogram)).BeginInit();
            this.tspDisplay.SuspendLayout();
            this.tpMessageLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEventLog)).BeginInit();
            this.SuspendLayout();
            // 
            // stsMain
            // 
            this.stsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pgbAsyncOpProgress,
            this.lblAsyncOpInfo,
            this.lblAsyncOpFailure,
            this.lblFileInfo,
            this.lblSpring,
            this.ddnCoords,
            this.ddnZoom,
            this.ddnInterpolationMethod});
            this.stsMain.Location = new System.Drawing.Point(0, 632);
            this.stsMain.Name = "stsMain";
            this.stsMain.Size = new System.Drawing.Size(1024, 22);
            this.stsMain.TabIndex = 0;
            this.stsMain.Text = "stsMain";
            // 
            // pgbAsyncOpProgress
            // 
            this.pgbAsyncOpProgress.Name = "pgbAsyncOpProgress";
            this.pgbAsyncOpProgress.Size = new System.Drawing.Size(100, 16);
            this.pgbAsyncOpProgress.Step = 1;
            this.pgbAsyncOpProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pgbAsyncOpProgress.Visible = false;
            // 
            // lblAsyncOpInfo
            // 
            this.lblAsyncOpInfo.IsLink = true;
            this.lblAsyncOpInfo.Name = "lblAsyncOpInfo";
            this.lblAsyncOpInfo.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.lblAsyncOpInfo.Size = new System.Drawing.Size(0, 17);
            // 
            // lblAsyncOpFailure
            // 
            this.lblAsyncOpFailure.IsLink = true;
            this.lblAsyncOpFailure.LinkColor = System.Drawing.Color.Red;
            this.lblAsyncOpFailure.Name = "lblAsyncOpFailure";
            this.lblAsyncOpFailure.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.lblAsyncOpFailure.Size = new System.Drawing.Size(477, 17);
            this.lblAsyncOpFailure.Text = "An error happened while executing the last operation. Click here to view the mess" +
    "age log.";
            this.lblAsyncOpFailure.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblAsyncOpFailure.Visible = false;
            this.lblAsyncOpFailure.Click += new System.EventHandler(this.lblAsyncOpFailure_Click);
            // 
            // lblFileInfo
            // 
            this.lblFileInfo.Name = "lblFileInfo";
            this.lblFileInfo.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.lblFileInfo.Size = new System.Drawing.Size(228, 17);
            this.lblFileInfo.Text = "Click \'Load data...\' to load a raster dataset.";
            // 
            // lblSpring
            // 
            this.lblSpring.Name = "lblSpring";
            this.lblSpring.Size = new System.Drawing.Size(528, 17);
            this.lblSpring.Spring = true;
            // 
            // ddnCoords
            // 
            this.ddnCoords.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ddiCoordsMap,
            this.ddiCoordsImage});
            this.ddnCoords.Enabled = false;
            this.ddnCoords.Image = global::ESRIBinaryViewer.Properties.Resources.Coordinates;
            this.ddnCoords.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ddnCoords.Name = "ddnCoords";
            this.ddnCoords.Size = new System.Drawing.Size(62, 20);
            this.ddnCoords.Text = "(0; 0)";
            // 
            // ddiCoordsMap
            // 
            this.ddiCoordsMap.Checked = true;
            this.ddiCoordsMap.CheckOnClick = true;
            this.ddiCoordsMap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ddiCoordsMap.Name = "ddiCoordsMap";
            this.ddiCoordsMap.Size = new System.Drawing.Size(172, 22);
            this.ddiCoordsMap.Text = "Map coordinates";
            this.ddiCoordsMap.Click += new System.EventHandler(this.ddiCoordsMap_Click);
            // 
            // ddiCoordsImage
            // 
            this.ddiCoordsImage.CheckOnClick = true;
            this.ddiCoordsImage.Name = "ddiCoordsImage";
            this.ddiCoordsImage.Size = new System.Drawing.Size(172, 22);
            this.ddiCoordsImage.Text = "Image coordinates";
            this.ddiCoordsImage.Click += new System.EventHandler(this.ddiCoordsImage_Click);
            // 
            // ddnZoom
            // 
            this.ddnZoom.Enabled = false;
            this.ddnZoom.Image = global::ESRIBinaryViewer.Properties.Resources.ZoomIn;
            this.ddnZoom.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ddnZoom.Name = "ddnZoom";
            this.ddnZoom.Size = new System.Drawing.Size(64, 20);
            this.ddnZoom.Text = "100%";
            // 
            // ddnInterpolationMethod
            // 
            this.ddnInterpolationMethod.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nearestNeighborToolStripMenuItem,
            this.bilinearToolStripMenuItem,
            this.bilinearHQToolStripMenuItem,
            this.bicubicToolStripMenuItem,
            this.bicubicHQToolStripMenuItem,
            this.noneToolStripMenuItem});
            this.ddnInterpolationMethod.Enabled = false;
            this.ddnInterpolationMethod.Image = global::ESRIBinaryViewer.Properties.Resources.InterpolationMethod;
            this.ddnInterpolationMethod.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ddnInterpolationMethod.Name = "ddnInterpolationMethod";
            this.ddnInterpolationMethod.Size = new System.Drawing.Size(127, 20);
            this.ddnInterpolationMethod.Text = "Nearest neighbor";
            this.ddnInterpolationMethod.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ddnInterpolationMethod_DropDownItemClicked);
            // 
            // nearestNeighborToolStripMenuItem
            // 
            this.nearestNeighborToolStripMenuItem.Checked = true;
            this.nearestNeighborToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.nearestNeighborToolStripMenuItem.Name = "nearestNeighborToolStripMenuItem";
            this.nearestNeighborToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.nearestNeighborToolStripMenuItem.Text = "Nearest neighbor";
            // 
            // bilinearToolStripMenuItem
            // 
            this.bilinearToolStripMenuItem.Name = "bilinearToolStripMenuItem";
            this.bilinearToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.bilinearToolStripMenuItem.Text = "Bilinear";
            // 
            // bilinearHQToolStripMenuItem
            // 
            this.bilinearHQToolStripMenuItem.Name = "bilinearHQToolStripMenuItem";
            this.bilinearHQToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.bilinearHQToolStripMenuItem.Text = "Bilinear (HQ)";
            // 
            // bicubicToolStripMenuItem
            // 
            this.bicubicToolStripMenuItem.Name = "bicubicToolStripMenuItem";
            this.bicubicToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.bicubicToolStripMenuItem.Text = "Bicubic";
            // 
            // bicubicHQToolStripMenuItem
            // 
            this.bicubicHQToolStripMenuItem.Name = "bicubicHQToolStripMenuItem";
            this.bicubicHQToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.bicubicHQToolStripMenuItem.Text = "Bicubic (HQ)";
            // 
            // noneToolStripMenuItem
            // 
            this.noneToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.noneToolStripMenuItem.Name = "noneToolStripMenuItem";
            this.noneToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.noneToolStripMenuItem.Text = "(default)";
            // 
            // tstMain
            // 
            this.tstMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tstMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnLoadBIL,
            this.btnExportImage,
            this.ddnHelp});
            this.tstMain.Location = new System.Drawing.Point(0, 0);
            this.tstMain.Name = "tstMain";
            this.tstMain.Size = new System.Drawing.Size(1024, 25);
            this.tstMain.TabIndex = 1;
            // 
            // btnLoadBIL
            // 
            this.btnLoadBIL.Image = global::ESRIBinaryViewer.Properties.Resources.Open;
            this.btnLoadBIL.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLoadBIL.Name = "btnLoadBIL";
            this.btnLoadBIL.Size = new System.Drawing.Size(88, 22);
            this.btnLoadBIL.Text = "Load data...";
            this.btnLoadBIL.Click += new System.EventHandler(this.btnLoadBIL_Click);
            // 
            // btnExportImage
            // 
            this.btnExportImage.Enabled = false;
            this.btnExportImage.Image = global::ESRIBinaryViewer.Properties.Resources.Save;
            this.btnExportImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExportImage.Name = "btnExportImage";
            this.btnExportImage.Size = new System.Drawing.Size(105, 22);
            this.btnExportImage.Text = "Export image...";
            this.btnExportImage.Click += new System.EventHandler(this.btnExportImage_Click);
            // 
            // ddnHelp
            // 
            this.ddnHelp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ddnHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ddnHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuHelp,
            this.mnuAbout});
            this.ddnHelp.Image = global::ESRIBinaryViewer.Properties.Resources.Help;
            this.ddnHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ddnHelp.Name = "ddnHelp";
            this.ddnHelp.Size = new System.Drawing.Size(29, 22);
            // 
            // mnuHelp
            // 
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.Size = new System.Drawing.Size(116, 22);
            this.mnuHelp.Text = "Help...";
            this.mnuHelp.Click += new System.EventHandler(this.mnuHelp_Click);
            // 
            // mnuAbout
            // 
            this.mnuAbout.Name = "mnuAbout";
            this.mnuAbout.Size = new System.Drawing.Size(116, 22);
            this.mnuAbout.Text = "About...";
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
            // 
            // splMain
            // 
            this.splMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splMain.Location = new System.Drawing.Point(0, 25);
            this.splMain.Name = "splMain";
            // 
            // splMain.Panel1
            // 
            this.splMain.Panel1.Controls.Add(this.splLeft);
            // 
            // splMain.Panel2
            // 
            this.splMain.Panel2.Controls.Add(this.tcViewSwitcher);
            this.splMain.Size = new System.Drawing.Size(1024, 607);
            this.splMain.SplitterDistance = 295;
            this.splMain.TabIndex = 2;
            // 
            // splLeft
            // 
            this.splLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splLeft.Location = new System.Drawing.Point(0, 0);
            this.splLeft.Name = "splLeft";
            this.splLeft.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splLeft.Panel1
            // 
            this.splLeft.Panel1.Controls.Add(this.dgvHeaderAttributes);
            // 
            // splLeft.Panel2
            // 
            this.splLeft.Panel2.Controls.Add(this.dgvBands);
            this.splLeft.Size = new System.Drawing.Size(295, 607);
            this.splLeft.SplitterDistance = 249;
            this.splLeft.TabIndex = 0;
            // 
            // dgvHeaderAttributes
            // 
            this.dgvHeaderAttributes.AllowUserToAddRows = false;
            this.dgvHeaderAttributes.AllowUserToDeleteRows = false;
            this.dgvHeaderAttributes.AllowUserToResizeColumns = false;
            this.dgvHeaderAttributes.AllowUserToResizeRows = false;
            this.dgvHeaderAttributes.BackgroundColor = System.Drawing.Color.White;
            this.dgvHeaderAttributes.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvHeaderAttributes.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvHeaderAttributes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHeaderAttributes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgcAttributeName,
            this.dgcAttributeValue});
            this.dgvHeaderAttributes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHeaderAttributes.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvHeaderAttributes.GridColor = System.Drawing.SystemColors.Control;
            this.dgvHeaderAttributes.Location = new System.Drawing.Point(0, 0);
            this.dgvHeaderAttributes.MultiSelect = false;
            this.dgvHeaderAttributes.Name = "dgvHeaderAttributes";
            this.dgvHeaderAttributes.ReadOnly = true;
            this.dgvHeaderAttributes.RowHeadersVisible = false;
            this.dgvHeaderAttributes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHeaderAttributes.Size = new System.Drawing.Size(295, 249);
            this.dgvHeaderAttributes.TabIndex = 0;
            // 
            // dgcAttributeName
            // 
            this.dgcAttributeName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgcAttributeName.DataPropertyName = "Name";
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.dgcAttributeName.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgcAttributeName.HeaderText = "Attribute";
            this.dgcAttributeName.Name = "dgcAttributeName";
            this.dgcAttributeName.ReadOnly = true;
            this.dgcAttributeName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgcAttributeValue
            // 
            this.dgcAttributeValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgcAttributeValue.DataPropertyName = "StringValue";
            this.dgcAttributeValue.HeaderText = "Value";
            this.dgcAttributeValue.Name = "dgcAttributeValue";
            this.dgcAttributeValue.ReadOnly = true;
            this.dgcAttributeValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvBands
            // 
            this.dgvBands.AllowUserToAddRows = false;
            this.dgvBands.AllowUserToDeleteRows = false;
            this.dgvBands.AllowUserToResizeColumns = false;
            this.dgvBands.AllowUserToResizeRows = false;
            this.dgvBands.BackgroundColor = System.Drawing.Color.White;
            this.dgvBands.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvBands.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvBands.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(0, 6, 0, 6);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBands.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvBands.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBands.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgcBandThumbnail,
            this.dgcBandName,
            this.dgcBandHistEq,
            this.dgcBandSetAsMono});
            this.dgvBands.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBands.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvBands.GridColor = System.Drawing.Color.White;
            this.dgvBands.Location = new System.Drawing.Point(0, 0);
            this.dgvBands.Name = "dgvBands";
            this.dgvBands.ReadOnly = true;
            this.dgvBands.RowHeadersVisible = false;
            this.dgvBands.RowTemplate.Height = 128;
            this.dgvBands.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBands.Size = new System.Drawing.Size(295, 354);
            this.dgvBands.TabIndex = 1;
            this.dgvBands.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBands_CellContentClick);
            this.dgvBands.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvBands_CellPainting);
            // 
            // dgcBandThumbnail
            // 
            this.dgcBandThumbnail.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dgcBandThumbnail.DataPropertyName = "Thumbnail";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.NullValue = null;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(2);
            this.dgcBandThumbnail.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgcBandThumbnail.HeaderText = "Thumbnail";
            this.dgcBandThumbnail.Name = "dgcBandThumbnail";
            this.dgcBandThumbnail.ReadOnly = true;
            this.dgcBandThumbnail.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgcBandThumbnail.Width = 62;
            // 
            // dgcBandName
            // 
            this.dgcBandName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgcBandName.DataPropertyName = "Name";
            this.dgcBandName.HeaderText = "Band name";
            this.dgcBandName.Name = "dgcBandName";
            this.dgcBandName.ReadOnly = true;
            this.dgcBandName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgcBandName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgcBandHistEq
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(0, 50, 0, 50);
            this.dgcBandHistEq.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgcBandHistEq.HeaderText = "";
            this.dgcBandHistEq.Name = "dgcBandHistEq";
            this.dgcBandHistEq.ReadOnly = true;
            this.dgcBandHistEq.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgcBandHistEq.Text = "HistEQ";
            this.dgcBandHistEq.ToolTipText = "Equalize band histogram";
            this.dgcBandHistEq.Width = 30;
            // 
            // dgcBandSetAsMono
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.Padding = new System.Windows.Forms.Padding(0, 50, 0, 50);
            this.dgcBandSetAsMono.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgcBandSetAsMono.HeaderText = "";
            this.dgcBandSetAsMono.Name = "dgcBandSetAsMono";
            this.dgcBandSetAsMono.ReadOnly = true;
            this.dgcBandSetAsMono.Text = "Mono";
            this.dgcBandSetAsMono.ToolTipText = "Display single band as grayscale";
            this.dgcBandSetAsMono.Width = 30;
            // 
            // tcViewSwitcher
            // 
            this.tcViewSwitcher.Controls.Add(this.tpDisplay);
            this.tcViewSwitcher.Controls.Add(this.tpMessageLog);
            this.tcViewSwitcher.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcViewSwitcher.Location = new System.Drawing.Point(0, 0);
            this.tcViewSwitcher.Name = "tcViewSwitcher";
            this.tcViewSwitcher.SelectedIndex = 0;
            this.tcViewSwitcher.Size = new System.Drawing.Size(725, 607);
            this.tcViewSwitcher.TabIndex = 2;
            // 
            // tpDisplay
            // 
            this.tpDisplay.Controls.Add(this.splDisplay);
            this.tpDisplay.Controls.Add(this.tspDisplay);
            this.tpDisplay.Location = new System.Drawing.Point(4, 22);
            this.tpDisplay.Name = "tpDisplay";
            this.tpDisplay.Padding = new System.Windows.Forms.Padding(3);
            this.tpDisplay.Size = new System.Drawing.Size(717, 581);
            this.tpDisplay.TabIndex = 0;
            this.tpDisplay.Text = "Display";
            this.tpDisplay.UseVisualStyleBackColor = true;
            // 
            // splDisplay
            // 
            this.splDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splDisplay.Location = new System.Drawing.Point(3, 28);
            this.splDisplay.Name = "splDisplay";
            this.splDisplay.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splDisplay.Panel1
            // 
            this.splDisplay.Panel1.Controls.Add(this.pbxDisplay);
            // 
            // splDisplay.Panel2
            // 
            this.splDisplay.Panel2.Controls.Add(this.chtHistogram);
            this.splDisplay.Size = new System.Drawing.Size(711, 550);
            this.splDisplay.SplitterDistance = 421;
            this.splDisplay.SplitterWidth = 7;
            this.splDisplay.TabIndex = 8;
            // 
            // pbxDisplay
            // 
            this.pbxDisplay.CurrentZoom = 5;
            this.pbxDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbxDisplay.Enabled = false;
            this.pbxDisplay.Image = null;
            this.pbxDisplay.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            this.pbxDisplay.Location = new System.Drawing.Point(0, 0);
            this.pbxDisplay.Name = "pbxDisplay";
            this.pbxDisplay.Size = new System.Drawing.Size(711, 421);
            this.pbxDisplay.TabIndex = 0;
            this.pbxDisplay.TabStop = false;
            this.pbxDisplay.ZoomChanged += new System.EventHandler(this.pbxDisplay_ZoomChanged);
            this.pbxDisplay.CopyCoords += new ESRIBinaryViewer.UI.PanZoomPictureBox.CopyCoordsEventHandler(this.pbxDisplay_CopyCoords);
            this.pbxDisplay.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbxDisplay_MouseMove);
            // 
            // chtHistogram
            // 
            chartArea1.AxisX.Interval = 1D;
            chartArea1.AxisX.LabelAutoFitStyle = System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.None;
            chartArea1.AxisX.LabelStyle.Interval = 0D;
            chartArea1.AxisX.LineWidth = 0;
            chartArea1.AxisX.MajorGrid.LineWidth = 0;
            chartArea1.AxisX.Title = "Luminosity";
            chartArea1.AxisY.LineWidth = 0;
            chartArea1.AxisY.MajorGrid.LineWidth = 0;
            chartArea1.AxisY.Title = "Pixel count";
            chartArea1.Name = "ChartArea1";
            this.chtHistogram.ChartAreas.Add(chartArea1);
            this.chtHistogram.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chtHistogram.Location = new System.Drawing.Point(0, 0);
            this.chtHistogram.Name = "chtHistogram";
            this.chtHistogram.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Grayscale;
            series1.BorderColor = System.Drawing.Color.Red;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
            series1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            series1.Legend = "Legend1";
            series1.Name = "R";
            series2.BorderColor = System.Drawing.Color.Green;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
            series2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(244)))), ((int)(((byte)(219)))));
            series2.Legend = "Legend1";
            series2.Name = "G";
            series3.BorderColor = System.Drawing.Color.Blue;
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
            series3.Color = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(242)))), ((int)(((byte)(253)))));
            series3.Legend = "Legend1";
            series3.Name = "B";
            this.chtHistogram.Series.Add(series1);
            this.chtHistogram.Series.Add(series2);
            this.chtHistogram.Series.Add(series3);
            this.chtHistogram.Size = new System.Drawing.Size(711, 122);
            this.chtHistogram.TabIndex = 0;
            // 
            // tspDisplay
            // 
            this.tspDisplay.BackColor = System.Drawing.Color.White;
            this.tspDisplay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tspDisplay.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tspDisplay.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblR,
            this.cmbR,
            this.lblG,
            this.cmbG,
            this.lblB,
            this.cmbB,
            this.ddnHistogram,
            this.btnSwapRB,
            this.btnDefaultRGB,
            this.ddnFilters});
            this.tspDisplay.Location = new System.Drawing.Point(3, 3);
            this.tspDisplay.Name = "tspDisplay";
            this.tspDisplay.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.tspDisplay.Size = new System.Drawing.Size(711, 25);
            this.tspDisplay.TabIndex = 7;
            // 
            // lblR
            // 
            this.lblR.BackColor = System.Drawing.Color.Red;
            this.lblR.Margin = new System.Windows.Forms.Padding(5);
            this.lblR.Name = "lblR";
            this.lblR.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.lblR.Size = new System.Drawing.Size(15, 15);
            // 
            // cmbR
            // 
            this.cmbR.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbR.Enabled = false;
            this.cmbR.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cmbR.Name = "cmbR";
            this.cmbR.Size = new System.Drawing.Size(75, 25);
            // 
            // lblG
            // 
            this.lblG.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.lblG.Enabled = false;
            this.lblG.Margin = new System.Windows.Forms.Padding(10, 5, 5, 5);
            this.lblG.Name = "lblG";
            this.lblG.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.lblG.Size = new System.Drawing.Size(15, 15);
            // 
            // cmbG
            // 
            this.cmbG.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbG.Enabled = false;
            this.cmbG.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cmbG.Name = "cmbG";
            this.cmbG.Size = new System.Drawing.Size(75, 25);
            // 
            // lblB
            // 
            this.lblB.BackColor = System.Drawing.Color.Blue;
            this.lblB.Enabled = false;
            this.lblB.Margin = new System.Windows.Forms.Padding(10, 5, 5, 5);
            this.lblB.Name = "lblB";
            this.lblB.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.lblB.Size = new System.Drawing.Size(15, 15);
            // 
            // cmbB
            // 
            this.cmbB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbB.Enabled = false;
            this.cmbB.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cmbB.Name = "cmbB";
            this.cmbB.Size = new System.Drawing.Size(75, 25);
            // 
            // ddnHistogram
            // 
            this.ddnHistogram.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ddnHistogram.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ddnHistogram.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuShowHistogram,
            this.mnuEqualizeHistogram});
            this.ddnHistogram.Enabled = false;
            this.ddnHistogram.Image = ((System.Drawing.Image)(resources.GetObject("ddnHistogram.Image")));
            this.ddnHistogram.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ddnHistogram.Name = "ddnHistogram";
            this.ddnHistogram.Size = new System.Drawing.Size(76, 22);
            this.ddnHistogram.Text = "Histogram";
            // 
            // mnuShowHistogram
            // 
            this.mnuShowHistogram.Name = "mnuShowHistogram";
            this.mnuShowHistogram.Size = new System.Drawing.Size(174, 22);
            this.mnuShowHistogram.Text = "Show histogram";
            this.mnuShowHistogram.Click += new System.EventHandler(this.mnuShowHistogram_Click);
            // 
            // mnuEqualizeHistogram
            // 
            this.mnuEqualizeHistogram.Image = global::ESRIBinaryViewer.Properties.Resources.HistEQ;
            this.mnuEqualizeHistogram.Name = "mnuEqualizeHistogram";
            this.mnuEqualizeHistogram.Size = new System.Drawing.Size(174, 22);
            this.mnuEqualizeHistogram.Text = "Equalize histogram";
            this.mnuEqualizeHistogram.Click += new System.EventHandler(this.mnuEqualizeHistogram_Click);
            // 
            // btnSwapRB
            // 
            this.btnSwapRB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSwapRB.Enabled = false;
            this.btnSwapRB.Image = global::ESRIBinaryViewer.Properties.Resources.SwapRB;
            this.btnSwapRB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSwapRB.Name = "btnSwapRB";
            this.btnSwapRB.Size = new System.Drawing.Size(23, 22);
            this.btnSwapRB.Text = "Swap R/B";
            this.btnSwapRB.Click += new System.EventHandler(this.btnSwapRB_Click);
            // 
            // btnDefaultRGB
            // 
            this.btnDefaultRGB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDefaultRGB.Enabled = false;
            this.btnDefaultRGB.Image = global::ESRIBinaryViewer.Properties.Resources.DefaultRGB;
            this.btnDefaultRGB.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnDefaultRGB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDefaultRGB.Name = "btnDefaultRGB";
            this.btnDefaultRGB.Size = new System.Drawing.Size(23, 22);
            this.btnDefaultRGB.Text = "Create default RGB composite";
            this.btnDefaultRGB.Click += new System.EventHandler(this.btnDefaultRGBComposite_Click);
            // 
            // ddnFilters
            // 
            this.ddnFilters.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ddnFilters.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ddnFilters.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuInvert,
            this.mnuGrayscale,
            this.mnuMedianFilter,
            this.mnuConvoFilter});
            this.ddnFilters.Enabled = false;
            this.ddnFilters.Image = ((System.Drawing.Image)(resources.GetObject("ddnFilters.Image")));
            this.ddnFilters.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ddnFilters.Name = "ddnFilters";
            this.ddnFilters.Size = new System.Drawing.Size(78, 22);
            this.ddnFilters.Text = "Apply filter";
            // 
            // mnuInvert
            // 
            this.mnuInvert.Name = "mnuInvert";
            this.mnuInvert.Size = new System.Drawing.Size(152, 22);
            this.mnuInvert.Text = "Invert";
            this.mnuInvert.Click += new System.EventHandler(this.mnuInvert_Click);
            // 
            // mnuGrayscale
            // 
            this.mnuGrayscale.Name = "mnuGrayscale";
            this.mnuGrayscale.Size = new System.Drawing.Size(152, 22);
            this.mnuGrayscale.Text = "Grayscale";
            this.mnuGrayscale.Click += new System.EventHandler(this.mnuGrayscale_Click);
            // 
            // mnuMedianFilter
            // 
            this.mnuMedianFilter.Name = "mnuMedianFilter";
            this.mnuMedianFilter.Size = new System.Drawing.Size(152, 22);
            this.mnuMedianFilter.Text = "Median";
            // 
            // mnuConvoFilter
            // 
            this.mnuConvoFilter.Name = "mnuConvoFilter";
            this.mnuConvoFilter.Size = new System.Drawing.Size(152, 22);
            this.mnuConvoFilter.Text = "Convolution";
            // 
            // tpMessageLog
            // 
            this.tpMessageLog.Controls.Add(this.dgvEventLog);
            this.tpMessageLog.Location = new System.Drawing.Point(4, 22);
            this.tpMessageLog.Name = "tpMessageLog";
            this.tpMessageLog.Size = new System.Drawing.Size(717, 581);
            this.tpMessageLog.TabIndex = 1;
            this.tpMessageLog.Text = "Message log";
            this.tpMessageLog.UseVisualStyleBackColor = true;
            // 
            // dgvEventLog
            // 
            this.dgvEventLog.AllowUserToAddRows = false;
            this.dgvEventLog.AllowUserToDeleteRows = false;
            this.dgvEventLog.AllowUserToResizeColumns = false;
            this.dgvEventLog.AllowUserToResizeRows = false;
            this.dgvEventLog.BackgroundColor = System.Drawing.Color.White;
            this.dgvEventLog.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvEventLog.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvEventLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEventLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgcEventTypeIcon,
            this.dgcEventTimestamp,
            this.dgcEventMessage});
            this.dgvEventLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvEventLog.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvEventLog.GridColor = System.Drawing.SystemColors.Control;
            this.dgvEventLog.Location = new System.Drawing.Point(0, 0);
            this.dgvEventLog.MultiSelect = false;
            this.dgvEventLog.Name = "dgvEventLog";
            this.dgvEventLog.ReadOnly = true;
            this.dgvEventLog.RowHeadersVisible = false;
            this.dgvEventLog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEventLog.Size = new System.Drawing.Size(717, 581);
            this.dgvEventLog.TabIndex = 1;
            // 
            // dgcEventTypeIcon
            // 
            this.dgcEventTypeIcon.DataPropertyName = "Icon";
            this.dgcEventTypeIcon.HeaderText = "";
            this.dgcEventTypeIcon.Name = "dgcEventTypeIcon";
            this.dgcEventTypeIcon.ReadOnly = true;
            this.dgcEventTypeIcon.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgcEventTypeIcon.Width = 20;
            // 
            // dgcEventTimestamp
            // 
            this.dgcEventTimestamp.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgcEventTimestamp.DataPropertyName = "Timestamp";
            this.dgcEventTimestamp.FillWeight = 56.49717F;
            this.dgcEventTimestamp.HeaderText = "Time";
            this.dgcEventTimestamp.Name = "dgcEventTimestamp";
            this.dgcEventTimestamp.ReadOnly = true;
            this.dgcEventTimestamp.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgcEventTimestamp.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgcEventMessage
            // 
            this.dgcEventMessage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgcEventMessage.DataPropertyName = "Message";
            this.dgcEventMessage.FillWeight = 143.5028F;
            this.dgcEventMessage.HeaderText = "Message";
            this.dgcEventMessage.Name = "dgcEventMessage";
            this.dgcEventMessage.ReadOnly = true;
            this.dgcEventMessage.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgcEventMessage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 654);
            this.Controls.Add(this.splMain);
            this.Controls.Add(this.tstMain);
            this.Controls.Add(this.stsMain);
            this.DoubleBuffered = true;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ESRI Binary Viewer";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.stsMain.ResumeLayout(false);
            this.stsMain.PerformLayout();
            this.tstMain.ResumeLayout(false);
            this.tstMain.PerformLayout();
            this.splMain.Panel1.ResumeLayout(false);
            this.splMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splMain)).EndInit();
            this.splMain.ResumeLayout(false);
            this.splLeft.Panel1.ResumeLayout(false);
            this.splLeft.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splLeft)).EndInit();
            this.splLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHeaderAttributes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBands)).EndInit();
            this.tcViewSwitcher.ResumeLayout(false);
            this.tpDisplay.ResumeLayout(false);
            this.tpDisplay.PerformLayout();
            this.splDisplay.Panel1.ResumeLayout(false);
            this.splDisplay.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splDisplay)).EndInit();
            this.splDisplay.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbxDisplay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chtHistogram)).EndInit();
            this.tspDisplay.ResumeLayout(false);
            this.tspDisplay.PerformLayout();
            this.tpMessageLog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEventLog)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip stsMain;
        private System.Windows.Forms.ToolStrip tstMain;
        private System.Windows.Forms.SplitContainer splMain;
        private System.Windows.Forms.SplitContainer splLeft;
        private System.Windows.Forms.DataGridView dgvHeaderAttributes;
        private System.Windows.Forms.DataGridView dgvBands;
        private System.Windows.Forms.ToolStripButton btnLoadBIL;
        private System.Windows.Forms.ToolStripButton btnExportImage;
        private System.Windows.Forms.ToolStripProgressBar pgbAsyncOpProgress;
        private System.Windows.Forms.ToolStripStatusLabel lblFileInfo;
        private System.Windows.Forms.TabControl tcViewSwitcher;
        private System.Windows.Forms.TabPage tpDisplay;
        private System.Windows.Forms.TabPage tpMessageLog;
        private System.Windows.Forms.DataGridView dgvEventLog;
        private System.Windows.Forms.DataGridViewImageColumn dgcEventTypeIcon;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgcEventTimestamp;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgcEventMessage;
        private System.Windows.Forms.ToolStripStatusLabel lblSpring;
        private System.Windows.Forms.ToolStripDropDownButton ddnCoords;
        private System.Windows.Forms.ToolStripMenuItem ddiCoordsMap;
        private System.Windows.Forms.ToolStripMenuItem ddiCoordsImage;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgcAttributeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgcAttributeValue;
        private System.Windows.Forms.ToolStripDropDownButton ddnHelp;
        private System.Windows.Forms.ToolStripMenuItem mnuHelp;
        private System.Windows.Forms.ToolStripMenuItem mnuAbout;
        private System.Windows.Forms.ToolStripStatusLabel lblAsyncOpInfo;
        private System.Windows.Forms.ToolStripDropDownButton ddnInterpolationMethod;
        private System.Windows.Forms.ToolStripMenuItem nearestNeighborToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bilinearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bicubicToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bilinearHQToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bicubicHQToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem noneToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton ddnZoom;
        private System.Windows.Forms.SplitContainer splDisplay;
        private PanZoomPictureBox pbxDisplay;
        private System.Windows.Forms.ToolStrip tspDisplay;
        private System.Windows.Forms.ToolStripLabel lblR;
        private System.Windows.Forms.ToolStripComboBox cmbR;
        private System.Windows.Forms.ToolStripLabel lblG;
        private System.Windows.Forms.ToolStripComboBox cmbG;
        private System.Windows.Forms.ToolStripLabel lblB;
        private System.Windows.Forms.ToolStripComboBox cmbB;
        private System.Windows.Forms.ToolStripDropDownButton ddnHistogram;
        private System.Windows.Forms.ToolStripMenuItem mnuShowHistogram;
        private System.Windows.Forms.ToolStripMenuItem mnuEqualizeHistogram;
        private System.Windows.Forms.ToolStripButton btnSwapRB;
        private System.Windows.Forms.ToolStripButton btnDefaultRGB;
        private System.Windows.Forms.DataVisualization.Charting.Chart chtHistogram;
        private System.Windows.Forms.ToolStripDropDownButton ddnFilters;
        private System.Windows.Forms.DataGridViewImageColumn dgcBandThumbnail;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgcBandName;
        private System.Windows.Forms.DataGridViewButtonColumn dgcBandHistEq;
        private System.Windows.Forms.DataGridViewButtonColumn dgcBandSetAsMono;
        private System.Windows.Forms.ToolStripMenuItem mnuConvoFilter;
        private System.Windows.Forms.ToolStripMenuItem mnuMedianFilter;
        private System.Windows.Forms.ToolStripMenuItem mnuInvert;
        private System.Windows.Forms.ToolStripMenuItem mnuGrayscale;
        private System.Windows.Forms.ToolStripStatusLabel lblAsyncOpFailure;
    }
}

