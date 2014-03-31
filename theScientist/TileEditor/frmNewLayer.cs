using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TileEditor
{
    public partial class frmNewLayer : Form
    {
        public bool OKPressed = false;

        public frmNewLayer()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            OKPressed = true;
            Close();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            OKPressed = false;
            Close();
        }



    }
}
