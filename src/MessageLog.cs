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
using System.ComponentModel;
using System.Drawing;

namespace ESRIBinaryViewer
{
    /// <summary>
    /// Provides a simple logging facility.
    /// </summary>
    public static class MessageLog
    {
        public enum EventType { INFO, WARN, ERROR };

        /// <summary>
        /// Represents an event in the message log.
        /// </summary>
        public struct Event
        {
            public string Message { get; private set; }     /// Message string.
            public EventType Type { get; private set; }     /// Type (severity).
            public DateTime Timestamp { get; private set; } /// Time stamp.
            public Icon Icon {                              /// Icon (generated from <see cref="Type" />).
                get {
                    Icon i;
                    switch (Type) {
                        case EventType.WARN:
                            i = SystemIcons.Warning;
                            break;
                        case EventType.ERROR:
                            i = SystemIcons.Error;
                            break;
                        case EventType.INFO:
                        default:
                            i = SystemIcons.Information;
                            break;
                    }

                    Size iconSize = System.Windows.Forms.SystemInformation.SmallIconSize;
                    Bitmap bitmap = new Bitmap(iconSize.Width, iconSize.Height);

                    using (Graphics g = Graphics.FromImage(bitmap)) {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.DrawImage(i.ToBitmap(), new Rectangle(Point.Empty, iconSize));
                    }

                    return Icon.FromHandle(bitmap.GetHicon());
                }
            }

            /// <summary>
            /// Constructs a new message log event.
            /// </summary>
            /// <param name="message">The message.</param>
            /// <param name="type">The type of the event.</param>
            public Event(string message, EventType type = EventType.INFO)
                : this() {
                Message = message;
                Type = type;
                Timestamp = DateTime.Now;
            }
        }

        private static BindingList<Event> events = new BindingList<Event>();

        /// <summary>
        /// A list of events that can be bound to a data control.
        /// </summary>
        public static BindingList<Event> Events { get { return events; } }

        /// <summary>
        /// Add an event to the message log.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="type">The type of the event.</param>
        public static void Add(string message, EventType type = EventType.INFO) {
            // This needs to be called on the UI thread, otherwise armageddon ensues.
            Program.MainForm.Invoke((Action) (() => {
                Events.Add(new Event(message, type));
            }));
        }
    }
}