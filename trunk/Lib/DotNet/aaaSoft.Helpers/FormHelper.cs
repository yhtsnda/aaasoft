using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace aaaSoft.Helpers
{
    public class FormHelper
    {
        private static Dictionary<Control, Point> dictControlLocationHistory = new Dictionary<Control, Point>();

        /// <summary>
        /// 弹出信息对话框
        /// </summary>
        /// <param name="InformationText">内容</param>
        public static void ShowInformationDialog(String InformationText)
        {
            ShowInformationDialog(Application.ProductName, InformationText);
        }

        /// <summary>
        /// 弹出信息对话框
        /// </summary>
        /// <param name="InformationTitle">标题</param>
        /// <param name="InformationText">内容</param>
        public static void ShowInformationDialog(String InformationTitle, String InformationText)
        {
            MessageBox.Show(InformationText, InformationTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 弹出警告对话框
        /// </summary>
        /// <param name="WarningText"></param>
        public static void ShowWarningDialog(String WarningText)
        {
            ShowWarningDialog(Application.ProductName, WarningText);
        }
        /// <summary>
        /// 弹出警告对话框
        /// </summary>
        /// <param name="WarningTitle"></param>
        /// <param name="WarningText"></param>
        public static void ShowWarningDialog(String WarningTitle, String WarningText)
        {
            MessageBox.Show(WarningText, WarningTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        /// <summary>
        /// 指定窗体在位置和大小上是否能在当前屏幕完全可见
        /// </summary>
        /// <param name="frm"></param>
        /// <returns></returns>
        public static Boolean IsFormVisibleInScreen(Form frm)
        {
            var rect = Screen.PrimaryScreen.Bounds;
            if (frm.Left >= 0 && frm.Right <= rect.Right
                && frm.Top >= 0 && frm.Bottom <= rect.Bottom)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        
        public static void DragMove(Control control)
        {
            control.MouseMove += new MouseEventHandler(control_MouseMove);
            control.MouseUp += new MouseEventHandler(control_MouseUp);

            lock (dictControlLocationHistory)
            {
                if (dictControlLocationHistory.ContainsKey(control))
                    dictControlLocationHistory.Remove(control);
                dictControlLocationHistory.Add(control, Form.MousePosition);
            }
        }

        private static void control_MouseMove(object sender, MouseEventArgs e)
        {
            Control control = (Control)sender;
            Control faControl = null;
            while (true)
            {
                if (dictControlLocationHistory.ContainsKey(control))
                {
                    faControl = control;
                    break;
                }
                if (control.Parent == null)
                    break;
                control = control.Parent;
            }

            if (faControl == null)
                return;

            Point historyMouseLocation = dictControlLocationHistory[faControl];
            Point currentMouseLocation = Form.MousePosition;

            lock (dictControlLocationHistory)
            {
                dictControlLocationHistory.Remove(faControl);
                dictControlLocationHistory.Add(faControl, currentMouseLocation);
            }

            Form faForm = null;
            while (true)
            {
                if (faControl is Form)
                {
                    faForm = (Form)faControl;
                    break;
                }
                if (faControl.Parent == null)
                    break;
                faControl = faControl.Parent;
            }
            if (faForm == null)
                return;

            Point currentFormLocation = faForm.Location;
            //增加偏移量
            currentFormLocation.X += currentMouseLocation.X - historyMouseLocation.X;
            currentFormLocation.Y += currentMouseLocation.Y - historyMouseLocation.Y;

            faForm.Location = currentFormLocation;
        }

        private static void control_MouseUp(object sender, MouseEventArgs e)
        {
            Control control = (Control)sender;
            control.MouseMove -= new MouseEventHandler(control_MouseMove);
            control.MouseUp -= new MouseEventHandler(control_MouseUp);

            while (true)
            {
                if (dictControlLocationHistory.ContainsKey(control))
                {
                    dictControlLocationHistory.Remove(control);
                    break;
                }
                if (control.Parent == null)
                    break;
                control = control.Parent;
            }
        }
    }
}
