namespace MakeSpecies
{
    partial class MakeSpecies
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
            this.components = new System.ComponentModel.Container();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.Gobutton = new System.Windows.Forms.Button();
            this.Setupbutton = new System.Windows.Forms.Button();
            this.LBwiki = new System.Windows.Forms.ListBox();
            this.LBlang = new System.Windows.Forms.ListBox();
            this.treebutton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TBtaxon = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TBresumeat = new System.Windows.Forms.TextBox();
            this.LBregnum = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TBtestprefix = new System.Windows.Forms.TextBox();
            this.CBmemo = new System.Windows.Forms.CheckBox();
            this.Quitbutton = new System.Windows.Forms.Button();
            this.CB_recursive = new System.Windows.Forms.CheckBox();
            this.makelistbutton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.CB_veto = new System.Windows.Forms.CheckBox();
            this.CB_overridedone = new System.Windows.Forms.CheckBox();
            this.disttestbutton = new System.Windows.Forms.Button();
            this.CB_onlydistribution = new System.Windows.Forms.CheckBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(279, 733);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // Gobutton
            // 
            this.Gobutton.Enabled = false;
            this.Gobutton.Location = new System.Drawing.Point(653, 320);
            this.Gobutton.Name = "Gobutton";
            this.Gobutton.Size = new System.Drawing.Size(100, 48);
            this.Gobutton.TabIndex = 1;
            this.Gobutton.Text = "Make taxon!";
            this.Gobutton.UseVisualStyleBackColor = true;
            this.Gobutton.Click += new System.EventHandler(this.Gobutton_Click);
            // 
            // Setupbutton
            // 
            this.Setupbutton.Location = new System.Drawing.Point(653, 48);
            this.Setupbutton.Name = "Setupbutton";
            this.Setupbutton.Size = new System.Drawing.Size(100, 56);
            this.Setupbutton.TabIndex = 2;
            this.Setupbutton.Text = "Setup";
            this.Setupbutton.UseVisualStyleBackColor = true;
            this.Setupbutton.Click += new System.EventHandler(this.Setupbutton_Click);
            // 
            // LBwiki
            // 
            this.LBwiki.FormattingEnabled = true;
            this.LBwiki.Location = new System.Drawing.Point(432, 320);
            this.LBwiki.Name = "LBwiki";
            this.LBwiki.Size = new System.Drawing.Size(120, 95);
            this.LBwiki.TabIndex = 3;
            // 
            // LBlang
            // 
            this.LBlang.FormattingEnabled = true;
            this.LBlang.Location = new System.Drawing.Point(432, 219);
            this.LBlang.Name = "LBlang";
            this.LBlang.Size = new System.Drawing.Size(120, 95);
            this.LBlang.TabIndex = 4;
            this.LBlang.SelectedIndexChanged += new System.EventHandler(this.LBlang_SelectedIndexChanged);
            // 
            // treebutton
            // 
            this.treebutton.Location = new System.Drawing.Point(653, 110);
            this.treebutton.Name = "treebutton";
            this.treebutton.Size = new System.Drawing.Size(100, 36);
            this.treebutton.TabIndex = 5;
            this.treebutton.Text = "Make tree chart";
            this.treebutton.UseVisualStyleBackColor = true;
            this.treebutton.Click += new System.EventHandler(this.treebutton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(386, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Taxon:";
            // 
            // TBtaxon
            // 
            this.TBtaxon.Location = new System.Drawing.Point(432, 35);
            this.TBtaxon.Name = "TBtaxon";
            this.TBtaxon.Size = new System.Drawing.Size(171, 20);
            this.TBtaxon.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(365, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Resume at:";
            // 
            // TBresumeat
            // 
            this.TBresumeat.Location = new System.Drawing.Point(432, 67);
            this.TBresumeat.Name = "TBresumeat";
            this.TBresumeat.Size = new System.Drawing.Size(171, 20);
            this.TBresumeat.TabIndex = 8;
            // 
            // LBregnum
            // 
            this.LBregnum.FormattingEnabled = true;
            this.LBregnum.Location = new System.Drawing.Point(297, 320);
            this.LBregnum.Name = "LBregnum";
            this.LBregnum.Size = new System.Drawing.Size(120, 95);
            this.LBregnum.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(370, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Testprefix:";
            // 
            // TBtestprefix
            // 
            this.TBtestprefix.Location = new System.Drawing.Point(432, 101);
            this.TBtestprefix.Name = "TBtestprefix";
            this.TBtestprefix.Size = new System.Drawing.Size(171, 20);
            this.TBtestprefix.TabIndex = 12;
            this.TBtestprefix.TextChanged += new System.EventHandler(this.TBtestprefix_TextChanged);
            // 
            // CBmemo
            // 
            this.CBmemo.AutoSize = true;
            this.CBmemo.Location = new System.Drawing.Point(432, 187);
            this.CBmemo.Name = "CBmemo";
            this.CBmemo.Size = new System.Drawing.Size(102, 17);
            this.CBmemo.TabIndex = 13;
            this.CBmemo.Text = "Memo, not save";
            this.CBmemo.UseVisualStyleBackColor = true;
            // 
            // Quitbutton
            // 
            this.Quitbutton.Location = new System.Drawing.Point(653, 468);
            this.Quitbutton.Name = "Quitbutton";
            this.Quitbutton.Size = new System.Drawing.Size(100, 48);
            this.Quitbutton.TabIndex = 14;
            this.Quitbutton.Text = "Quit";
            this.Quitbutton.UseVisualStyleBackColor = true;
            this.Quitbutton.Click += new System.EventHandler(this.Quitbutton_Click);
            // 
            // CB_recursive
            // 
            this.CB_recursive.AutoSize = true;
            this.CB_recursive.Location = new System.Drawing.Point(653, 374);
            this.CB_recursive.Name = "CB_recursive";
            this.CB_recursive.Size = new System.Drawing.Size(103, 17);
            this.CB_recursive.TabIndex = 15;
            this.CB_recursive.Text = "Recursive make";
            this.CB_recursive.UseVisualStyleBackColor = true;
            // 
            // makelistbutton
            // 
            this.makelistbutton.Enabled = false;
            this.makelistbutton.Location = new System.Drawing.Point(653, 251);
            this.makelistbutton.Name = "makelistbutton";
            this.makelistbutton.Size = new System.Drawing.Size(100, 41);
            this.makelistbutton.TabIndex = 16;
            this.makelistbutton.Text = "Make from list";
            this.makelistbutton.UseVisualStyleBackColor = true;
            this.makelistbutton.Click += new System.EventHandler(this.Makelistbutton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // CB_veto
            // 
            this.CB_veto.AutoSize = true;
            this.CB_veto.Location = new System.Drawing.Point(653, 297);
            this.CB_veto.Name = "CB_veto";
            this.CB_veto.Size = new System.Drawing.Size(87, 17);
            this.CB_veto.TabIndex = 17;
            this.CB_veto.Text = "With veto list";
            this.CB_veto.UseVisualStyleBackColor = true;
            // 
            // CB_overridedone
            // 
            this.CB_overridedone.AutoSize = true;
            this.CB_overridedone.Location = new System.Drawing.Point(653, 398);
            this.CB_overridedone.Name = "CB_overridedone";
            this.CB_overridedone.Size = new System.Drawing.Size(111, 17);
            this.CB_overridedone.TabIndex = 18;
            this.CB_overridedone.Text = "Override donetree";
            this.CB_overridedone.UseVisualStyleBackColor = true;
            // 
            // disttestbutton
            // 
            this.disttestbutton.Location = new System.Drawing.Point(653, 152);
            this.disttestbutton.Name = "disttestbutton";
            this.disttestbutton.Size = new System.Drawing.Size(100, 25);
            this.disttestbutton.TabIndex = 19;
            this.disttestbutton.Text = "Test distribution";
            this.disttestbutton.UseVisualStyleBackColor = true;
            this.disttestbutton.Click += new System.EventHandler(this.disttestbutton_Click);
            // 
            // CB_onlydistribution
            // 
            this.CB_onlydistribution.AutoSize = true;
            this.CB_onlydistribution.Location = new System.Drawing.Point(653, 421);
            this.CB_onlydistribution.Name = "CB_onlydistribution";
            this.CB_onlydistribution.Size = new System.Drawing.Size(122, 17);
            this.CB_onlydistribution.TabIndex = 20;
            this.CB_onlydistribution.Text = "Only with distribution";
            this.CB_onlydistribution.UseVisualStyleBackColor = true;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 600000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // MakeSpecies
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 757);
            this.Controls.Add(this.CB_onlydistribution);
            this.Controls.Add(this.disttestbutton);
            this.Controls.Add(this.CB_overridedone);
            this.Controls.Add(this.CB_veto);
            this.Controls.Add(this.makelistbutton);
            this.Controls.Add(this.CB_recursive);
            this.Controls.Add(this.Quitbutton);
            this.Controls.Add(this.CBmemo);
            this.Controls.Add(this.TBtestprefix);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.LBregnum);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TBresumeat);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TBtaxon);
            this.Controls.Add(this.treebutton);
            this.Controls.Add(this.LBlang);
            this.Controls.Add(this.LBwiki);
            this.Controls.Add(this.Setupbutton);
            this.Controls.Add(this.Gobutton);
            this.Controls.Add(this.richTextBox1);
            this.Name = "MakeSpecies";
            this.Text = "MakeSpecies";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button Gobutton;
        private System.Windows.Forms.Button Setupbutton;
        private System.Windows.Forms.ListBox LBwiki;
        private System.Windows.Forms.ListBox LBlang;
        private System.Windows.Forms.Button treebutton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TBtaxon;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TBresumeat;
        private System.Windows.Forms.ListBox LBregnum;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TBtestprefix;
        private System.Windows.Forms.CheckBox CBmemo;
        private System.Windows.Forms.Button Quitbutton;
        private System.Windows.Forms.CheckBox CB_recursive;
        private System.Windows.Forms.Button makelistbutton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.CheckBox CB_veto;
        private System.Windows.Forms.CheckBox CB_overridedone;
        private System.Windows.Forms.Button disttestbutton;
        private System.Windows.Forms.CheckBox CB_onlydistribution;
        private System.Windows.Forms.Timer timer1;
    }
}