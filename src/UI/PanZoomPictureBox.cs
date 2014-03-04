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
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ESRIBinaryViewer.UI
{
    /// <summary>
    /// Defines a custom PictureBox with panning and zooming capabilities and more.
    /// </summary>
    public partial class PanZoomPictureBox : PictureBox
    {
        /// <summary>
        /// Defines the image position the mouse cursor is currently on.
        /// </summary>
        public PointF CurrentImagePosition;

        /// <summary>
        /// Defines an event that is fired when the zoom level is changed.
        /// </summary>
        public event EventHandler ZoomChanged;

        /// <summary>
        /// Defines an event that is fired when the map coordinates are to be copied.
        /// </summary>
        
        public delegate void CopyCoordsEventHandler(object sender, bool mapCoordinates);
        public event CopyCoordsEventHandler CopyCoords;

        /// <summary>
        /// Defines the possible zoom levels.
        /// </summary>
        public float[] ZoomFactors = {
            .1f, .2f, .3f, .4f, .5f, .6f, .7f, .8f, .9f, 1,
            1.1f, 1.2f, 1.5f, 2, 3, 5, 7, 10, 15, 20, 30
        };

        /// <summary>
        /// Gets or sets the current zoom level.
        /// </summary>
        public int CurrentZoom {
            get { return currentZoom; }
            set {
                if (value > ZoomFactors.Length - 1 || value < 0) {
                    return;
                }

                currentZoom = value;

                if (Image != null) {
                    translation.X = (Width / 2) - (ZoomFactors[currentZoom] * Image.Width / 2);
                    translation.Y = (Height / 2) - (ZoomFactors[currentZoom] * Image.Height / 2);

                    this.ZoomChanged(this, EventArgs.Empty);
                    this.Refresh();
                }
            }
        }

        /// <summary>
        /// Gets or sets the interpolation mode.
        /// </summary>
        public InterpolationMode InterpolationMode { get; set; }

        private const int thumbSize = 100;
        private const int thumbMargin = 20;
        private const int thumbPadding = 5;

        private PointF mouseDragOrigin;
        private PointF translation;
        private bool panning = false;
        private bool showThumbnail = true;
        private int currentZoom = 5;

        private Image thumbnail;

        /// <summary>
        /// Gets or sets the displayed image.
        /// </summary>
        public new Image Image {
            get {
                return base.Image;
            }

            set {
                base.Image = value;

                if (value == null) {
                    return;
                }

                int thumbW, thumbH;

                if (value.Width > value.Height) {
                    thumbW = thumbSize;
                    thumbH = (int) (((double) value.Height / value.Width) * thumbSize);
                } else {
                    thumbH = thumbSize;
                    thumbW = (int) (((double) value.Width / value.Height) * thumbSize);
                }

                thumbnail = value.GetThumbnailImage(thumbW, thumbH, null, IntPtr.Zero);
            }
        }

        public PanZoomPictureBox() {
            // Set up the context menu
            this.ContextMenuStrip = new ContextMenuStrip();

            this.ContextMenuStrip.Items.Add(
                "Zoom in",
                ESRIBinaryViewer.Properties.Resources.ZoomIn,
                (o, a) => { CurrentZoom++; }
            );
            this.ContextMenuStrip.Items.Add(
                "Zoom out",
                ESRIBinaryViewer.Properties.Resources.ZoomOut,
                (o, a) => { CurrentZoom--; }
            );

            this.ContextMenuStrip.Items.Add(new ToolStripSeparator());

            var i = this.ContextMenuStrip.Items.Add("Show thumbnail when panning");
            (i as ToolStripMenuItem).Checked = true;
            i.Click += (o, a) => {
                showThumbnail = !showThumbnail;
                (o as ToolStripMenuItem).Checked = showThumbnail;
            };

            this.ContextMenuStrip.Items.Add(new ToolStripSeparator());

            this.ContextMenuStrip.Items.Add("Copy RGB value",
                null,
                (o, a) => {
                    var c =(base.Image as Bitmap).GetPixel(
                        (int) CurrentImagePosition.X,
                        (int) CurrentImagePosition.Y);
                    Clipboard.SetText(String.Format("rgb({0}, {1}, {2})", c.R, c.G, c.B));
            });
            this.ContextMenuStrip.Items.Add(
                "Copy image coordinates",
                null,
                (o, a) => { CopyCoords(this, false); }
            );
            this.ContextMenuStrip.Items.Add(
                "Copy map coordinates",
                null,
                (o, a) => { CopyCoords(this, true); }
            );

            InterpolationMode = InterpolationMode.NearestNeighbor;
        }

        /// <summary>
        /// Handles painting of the picture box contents.
        /// </summary>
        protected override void OnPaint(PaintEventArgs pe) {
            if (Image == null) {
                return;
            }

            // Set up a new container for the translated and resized image.
            var container = pe.Graphics.BeginContainer();
            pe.Graphics.TranslateTransform(translation.X, translation.Y);
            pe.Graphics.ScaleTransform(ZoomFactors[currentZoom], ZoomFactors[currentZoom]);
            pe.Graphics.InterpolationMode = InterpolationMode;
            base.OnPaint(pe);
            pe.Graphics.EndContainer(container);

            // Draw the thumbnail if shown.
            if (thumbnail != null && panning && showThumbnail) {
                int thumbX = this.Width - thumbnail.Width - thumbMargin - 2 * thumbPadding;
                int thumbY = this.Height - thumbnail.Height - thumbMargin - 2 * thumbPadding;

                pe.Graphics.FillRectangle(
                    new SolidBrush(Color.FromArgb(127, Color.White)),
                    new Rectangle(
                        thumbX,
                        thumbY,
                        thumbnail.Width + 2 * thumbPadding,
                        thumbnail.Height + 2 * thumbPadding
                    )
                );

                pe.Graphics.DrawImage(thumbnail,
                    this.Width - thumbnail.Width - thumbMargin - thumbPadding,
                    this.Height - thumbnail.Height - thumbMargin - thumbPadding
                );

                Rectangle r = new Rectangle(
                    thumbX + thumbPadding +
                        (int) ((- translation.X / ZoomFactors[currentZoom]) * ((double) thumbnail.Width / Image.Width)),
                    thumbY + thumbPadding +
                        (int) ((- translation.Y / ZoomFactors[currentZoom]) * ((double) thumbnail.Height / Image.Height)),
                   (int) ((Width / ZoomFactors[currentZoom]) * ((double) thumbnail.Width / Image.Width)),
                   (int) ((Height / ZoomFactors[currentZoom])* ((double) thumbnail.Height / Image.Height))
                );

                if (r.X < thumbX + thumbPadding) { r.X = thumbX + thumbPadding; }
                if (r.Y < thumbY + thumbPadding) { r.Y = thumbY + thumbPadding; }
                if (r.Width > thumbnail.Width) { r.Width = thumbnail.Width; }
                if (r.Height > thumbnail.Height) { r.Height = thumbnail.Height; }

                pe.Graphics.DrawRectangle(new Pen(Color.White, 2), r);
            }
        }

        /// <summary>
        /// Handles the mouse down event on the picture box.
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e) {
            if (Image == null) {
                return;
            }

            // Start dragging.
            if (e.Button.HasFlag(MouseButtons.Left)) {
                mouseDragOrigin = new PointF(e.X - translation.X, e.Y - translation.Y);
                this.Cursor = Cursors.SizeAll;
            }

            base.OnMouseDown(e);
        }

        /// <summary>
        /// Handles the mouse move event on the picture box.
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e) {
            if (Image == null) {
                return;
            }

            CurrentImagePosition = new PointF(
                (e.X - translation.X) / ZoomFactors[currentZoom],
                (e.Y - translation.Y) / ZoomFactors[currentZoom]
            );

            // Update the current pan based on the drag amount and the current zoom.
            if (e.Button.HasFlag(MouseButtons.Left) &&
                    ((Image.Width * ZoomFactors[currentZoom] > Width) ||
                     (Image.Height * ZoomFactors[currentZoom] > Height))
            ) {
                translation = new PointF(e.X - mouseDragOrigin.X, e.Y - mouseDragOrigin.Y);

                if (Image.Width * ZoomFactors[currentZoom] < Width) {
                    translation.X = (Width / 2) - (ZoomFactors[currentZoom] * Image.Width / 2);
                } else if (translation.X > 0) {
                    translation.X = 0;
                } else if (translation.X < Width - (Image.Width * ZoomFactors[currentZoom])) {
                    translation.X = Width - (Image.Width * ZoomFactors[currentZoom]);
                }

                if (Image.Height * ZoomFactors[currentZoom] < Height) {
                    translation.Y = (Height / 2) - (ZoomFactors[currentZoom] * Image.Height / 2);
                } else if (translation.Y > 0) {
                    translation.Y = 0;
                } else if (translation.Y < Height - (Image.Height * ZoomFactors[currentZoom])) {
                    translation.Y = Height - (Image.Height * ZoomFactors[currentZoom]);
                }

                panning = true;
                this.Refresh();
            }

            base.OnMouseMove(e);
        }

        /// <summary>
        /// Handles the mouse up event on the picture box.
        /// </summary>
        protected override void OnMouseUp(MouseEventArgs e) {
            this.Cursor = Cursors.Arrow;
            panning = false;
            this.Refresh();
            base.OnMouseUp(e);
        }


        /// <summary>
        /// Handles mouse scrolling on the picture box.
        /// </summary>
        protected override void OnMouseWheel(MouseEventArgs e) {
            if (Image == null) {
                return;
            }
            PointF imagePos = new PointF(
                (e.X - translation.X) / ZoomFactors[currentZoom],
                (e.Y - translation.Y) / ZoomFactors[currentZoom]
            );

            // Update zoom based on the current mouse position and zoom.
            currentZoom += ((e.Delta > 0) ? +1 : -1);
            if (currentZoom > (ZoomFactors.Length - 1)) {
                currentZoom = ZoomFactors.Length - 1;
            } else if (currentZoom < 0) {
                currentZoom = 0;
            }

            if (Image.Width * ZoomFactors[currentZoom] > Width)
            {
                translation.X = e.X - ZoomFactors[currentZoom] * imagePos.X;
            } else {
                translation.X = (Width / 2) - (ZoomFactors[currentZoom] * Image.Width / 2);
            }

            if (Image.Height * ZoomFactors[currentZoom] > Height)
            {
                translation.Y = e.Y - ZoomFactors[currentZoom] * imagePos.Y;
            } else {
                translation.Y = (Height / 2) - (ZoomFactors[currentZoom] * Image.Height / 2);
            }

            this.ZoomChanged(this, null);
            this.Refresh();

            base.OnMouseWheel(e);
        }

        /// <summary>
        /// Handles size change of the picture box.
        /// </summary>
        protected override void OnSizeChanged(EventArgs e) {
            if (Image == null) {
                return;
            }

            translation.X = (Width / 2) - (ZoomFactors[currentZoom] * Image.Width / 2);
            translation.Y = (Height / 2) - (ZoomFactors[currentZoom] * Image.Height / 2);

            this.Refresh();
        }


        /// <summary>
        /// Handles the mouse enter event on the picture box.
        /// </summary>
        protected override void OnMouseEnter(EventArgs e) {
            // Acquire focus, so panning and zooming works without having to manually
            // focus the control.
            this.Focus();

            base.OnMouseEnter(e);
        }
    }
}