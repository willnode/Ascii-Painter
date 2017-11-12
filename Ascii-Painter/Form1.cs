using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ascii_Painter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            canvas.Text = "Hello\nWorld\nI'm\nComing\n  Now";

            versionStrip.Text = "V" + Assembly.GetEntryAssembly().GetName().Version.ToString();

            flipXToolStripMenuItem.Tag = CanvasMirror.MirrorX;
            flipYToolStripMenuItem.Tag = CanvasMirror.MirrorY;
            rotateCCWToolStripMenuItem.Tag = CanvasMirror.RotateCCW;
            rotateCWToolStripMenuItem.Tag = CanvasMirror.RotateCW;

            int iter = 0;
            foreach (int v in Enum.GetValues(typeof(CanvasTool)))
            {
                var btn = new ToolStripButton
                {
                    Name = "btn" + iter,
                    Text = Enum.GetName(typeof(CanvasTool), v),
                    Tag = (CanvasTool)v,
                    DisplayStyle = ToolStripItemDisplayStyle.ImageAndText,

                };
                if (iter++ == 0)
                {
                    btn.Checked = true;
                    checkedTool = btn;
                }
                toolHotKeys.Add((Keys)(111 + iter), btn);
                btn.Click += _tool_Click;
                toolStrip.Items.Add(btn);
                toolButtons.Add(btn);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (toolHotKeys.ContainsKey(e.KeyCode))
            {
                toolHotKeys[e.KeyCode].PerformClick();
                e.Handled = true;
            }

            if (canvas.Tool >= CanvasTool.Brush)
            {
                if (e.Modifiers != Keys.None && e.Modifiers != Keys.Shift) return;
                var r = Utility.KeyCodeToUnicode(e.KeyCode);
                if (r.Length == 1)
                {
                    toolArt.Text = r[0] < ' ' ? "" : new string(r[0], 1);
                    e.Handled = true;
                }
            }
        }

        Dictionary<Keys, ToolStripButton> toolHotKeys = new Dictionary<Keys, ToolStripButton>();
        List<ToolStripButton> toolButtons = new List<ToolStripButton>();
        ToolStripButton checkedTool;

        private void _tool_Click(object sender, EventArgs e)
        {
            checkedTool.Checked = false;
            canvas.Tool = (CanvasTool)(checkedTool = ((ToolStripButton)sender)).Tag;
            checkedTool.Checked = true;

        }

        private void ToolArt_TextChanged(object sender, EventArgs e)
        {
            var t = toolArt.Text;
            if (t.Length > 1)
                toolArt.Text = t = new string(t[Math.Max(0, toolArt.SelectionStart - 1)], 1);
            canvas.ToolArt = t.Length == 0 ? '\0' : t[0];
        }

        private void gridlinesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            canvas.Gridlines = gridlinesToolStripMenuItem.Checked;
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (canvas.Tool != CanvasTool.Freetype)
            {
                canvas.ClearSelected();
            }
        }

        private void canvas_SelectionChanged(object sender, EventArgs args)
        {
            var s = canvas.Selection;
            _sel.Text = string.Format("Selection:   ( {0},{1} )   {2} x {3}", s.X, s.Y, s.Height, s.Width);
        }

        private void flipXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            canvas.Mirror((CanvasMirror)((ToolStripMenuItem)sender).Tag);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var n = new NewCanvasDialog(canvas.ImageSize);
            if (n.ShowDialog() == DialogResult.OK)
            {
                if (n.Clear)    
                    canvas.ImageSize = n.Resolution;
                else
                {
                    var txt = canvas.Text;
                    canvas.ImageSize = n.Resolution;
                    canvas.Text = txt;
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var n = new SaveFileDialog()
            {
                AddExtension = true,
                DefaultExt = "txt",
                OverwritePrompt = true,
                Filter = "Text Files|*.txt|All Files|*.*"                
            };

            if (n.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(n.FileName, canvas.Text);
            }
          
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
          if (MessageBox.Show("Ascii-Painter made with <3 by WelloSoft. Click OK to visit Repo.", "About Ascii-Painter", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Process.Start("https://github.com/willnode/Ascii-Painter");
            }
        }
    }
}
