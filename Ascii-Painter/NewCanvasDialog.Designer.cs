namespace Ascii_Painter
{
    partial class NewCanvasDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.W = new System.Windows.Forms.NumericUpDown();
            this.H = new System.Windows.Forms.NumericUpDown();
            this.C = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.W)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.H)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 82F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.W, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.H, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.C, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.button1, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.button2, 2, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(334, 109);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "Dimension \r\n(W x H)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // W
            // 
            this.W.Dock = System.Windows.Forms.DockStyle.Fill;
            this.W.Location = new System.Drawing.Point(82, 5);
            this.W.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.W.Maximum = new decimal(new int[] {
            8192,
            0,
            0,
            0});
            this.W.Name = "W";
            this.W.Size = new System.Drawing.Size(126, 20);
            this.W.TabIndex = 1;
            // 
            // H
            // 
            this.H.Dock = System.Windows.Forms.DockStyle.Fill;
            this.H.Location = new System.Drawing.Point(208, 5);
            this.H.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.H.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.H.Name = "H";
            this.H.Size = new System.Drawing.Size(126, 20);
            this.H.TabIndex = 2;
            // 
            // C
            // 
            this.C.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.C, 2);
            this.C.Dock = System.Windows.Forms.DockStyle.Fill;
            this.C.Location = new System.Drawing.Point(85, 33);
            this.C.Name = "C";
            this.C.Size = new System.Drawing.Size(246, 17);
            this.C.TabIndex = 3;
            this.C.Text = "Clear current content";
            this.C.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.Location = new System.Drawing.Point(85, 76);
            this.button1.MinimumSize = new System.Drawing.Size(0, 30);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 30);
            this.button1.TabIndex = 4;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.AutoSize = true;
            this.button2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button2.Location = new System.Drawing.Point(211, 76);
            this.button2.MinimumSize = new System.Drawing.Size(0, 30);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(120, 30);
            this.button2.TabIndex = 5;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // NewCanvasDialog
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(334, 109);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewCanvasDialog";
            this.Text = "New Canvas ...";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.W)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.H)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown W;
        private System.Windows.Forms.NumericUpDown H;
        private System.Windows.Forms.CheckBox C;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}