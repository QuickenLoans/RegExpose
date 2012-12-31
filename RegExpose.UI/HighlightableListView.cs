using System;
using System.Drawing;
using System.Windows.Forms;

namespace RegExpose.UI
{
    public class HighlightableListView : ListView
    {
        public HighlightableListView()
        {
            OwnerDraw = true;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            DoubleBuffered = true;
            base.OnHandleCreated(e);
        }

        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
        {
            if (e.Item.Selected)
            {
                e.Graphics.FillRectangle(Brushes.Yellow, e.Bounds);
            }
            else
            {
                e.DrawBackground();
            }

            e.Graphics.DrawString(e.SubItem.Text, Font, Brushes.Black, e.Bounds);
        }

        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {
            // do nothing
        }
    }
}