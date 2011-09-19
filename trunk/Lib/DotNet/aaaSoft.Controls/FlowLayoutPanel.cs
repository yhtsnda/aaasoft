using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace aaaSoft.Controls
{
    public class FlowLayoutPanel : System.Windows.Forms.FlowLayoutPanel
    {
        private const Int32 ScrollBarWidth = 24;
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            foreach (Control control in this.Controls)
                ResizeControl(control);
        }

        private void ResizeControl(Control control)
        {
            if (control.Width != this.Width - ScrollBarWidth)
            {
                var tmpWidth = this.Width - ScrollBarWidth;
                if (tmpWidth < control.MinimumSize.Width)
                    tmpWidth = control.MinimumSize.Width;
                control.Width = tmpWidth;
            }
        }

        protected override void OnControlAdded(System.Windows.Forms.ControlEventArgs e)
        {
            base.OnControlAdded(e);
            ResizeControl(e.Control);
        }
    }
}
