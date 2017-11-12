using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ascii_Painter
{
    public partial class NewCanvasDialog : Form
    {

        public bool Clear;
        public Size Resolution;

        public NewCanvasDialog(Size current)
        {
            InitializeComponent();
            C.Checked = true;
            W.Value = current.Width;
            H.Value = current.Height;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clear = C.Checked;
            Resolution = new Size((int)W.Value, (int)H.Value);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
