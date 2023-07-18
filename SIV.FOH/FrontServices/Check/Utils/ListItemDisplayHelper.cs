using System;
using System.Drawing;
using System.Windows.Forms;
using SIV.Entities;

namespace SIV.Utils
{
    static class ListItemDisplayHelper
    {
        public static void AddItem(Panel panel, Item item)
        {
            int Y = 10;
            if (panel.Controls.Count > 0)
            {
                foreach (Control control in panel.Controls)
                {
                    Y += control.Height;
                }
            }

            Label label = new Label { Location = new Point(30, Y), AutoSize = false, Width = panel.Width };
            label.Text = item.DisplayName;
            label.Click += (object sender, EventArgs e) => { ((Control)sender).BackColor = Color.FromArgb(106, 106, 106); };
            panel.Controls.Add(label);
        }

        public static void DisplayInPanel(ref Panel panel, Items items)
        {



        }
    }
}
