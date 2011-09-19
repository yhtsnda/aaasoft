using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using aaaSoft.Helpers;
using System.Drawing;

namespace aaaSoft.SkinEngine.MyControls
{
    public class MyDataGridView:IMyControl
    {
        DataGridView dgvBase;
        public static SkinEngine skinEng
        {
            get
            {
                return SkinEngine.MainSkinEngine;
            }
        }

        #warning 临时作此处理
        public Color DataGridViewBackColor = Color.FromArgb(248, 249, 253);

        public MyDataGridView(DataGridView dgv)
        {
            dgvBase = dgv;
        }

        public void StopControlSkin()
        {
            skinEng.BackColorChanged -= new EventHandler(skinEng_BackColorChanged);

            dgvBase.BackgroundColor = SystemColors.Control;
        }

        public void StartControlSkin()
        {
            skinEng.BackColorChanged += new EventHandler(skinEng_BackColorChanged);

            skinEng_BackColorChanged(dgvBase, null);
        }

        void skinEng_BackColorChanged(object sender, EventArgs e)
        {
            Color tmpColor = ColorHelper.ReplaceColor(DataGridViewBackColor, skinEng.BackColor);
            dgvBase.ColumnHeadersDefaultCellStyle.BackColor = tmpColor;
            dgvBase.RowHeadersDefaultCellStyle.BackColor = tmpColor;
        }
    }
}
