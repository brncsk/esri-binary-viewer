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
using System.IO;
using System.Windows.Forms;

namespace ESRIBinaryViewer.UI
{
    /// <summary>
    /// Provides basic UI for displaying help.
    /// </summary>
    public partial class HelpWindow : Form
    {
        private static HelpWindow instance;

        /// <summary>
        /// Returns an instance of the help window.
        /// </summary>
        public static HelpWindow Instance {
            get {
                if (instance == null || instance.IsDisposed) {
                    instance = new HelpWindow();
                }

                return instance;
            }
        }

        private HelpWindow () {
            InitializeComponent();
        }

        /// <summary>
        /// Shows the help file when the form loads.
        /// </summary>
        private void HelpWindow_Load (object sender, EventArgs e) {
            wbHelp.Url = new Uri(String.Format(@"file://{0}/Resources/Help/help.html", Directory.GetCurrentDirectory()));
        }
    }
}
