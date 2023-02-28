namespace MakeSpecies
{
    partial class MScomparetrees
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.TBtaxon = new System.Windows.Forms.TextBox();
            this.LBwiki = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Wikibutton = new System.Windows.Forms.Button();
            this.COLbutton = new System.Windows.Forms.Button();
            this.loadtreebutton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(40, 40);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(465, 592);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // TBtaxon
            // 
            this.TBtaxon.Location = new System.Drawing.Point(617, 52);
            this.TBtaxon.Name = "TBtaxon";
            this.TBtaxon.Size = new System.Drawing.Size(171, 20);
            this.TBtaxon.TabIndex = 1;
            // 
            // LBwiki
            // 
            this.LBwiki.FormattingEnabled = true;
            this.LBwiki.Location = new System.Drawing.Point(595, 117);
            this.LBwiki.Name = "LBwiki";
            this.LBwiki.Size = new System.Drawing.Size(120, 95);
            this.LBwiki.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(571, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Taxon:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // Wikibutton
            // 
            this.Wikibutton.Location = new System.Drawing.Point(612, 257);
            this.Wikibutton.Name = "Wikibutton";
            this.Wikibutton.Size = new System.Drawing.Size(103, 23);
            this.Wikibutton.TabIndex = 4;
            this.Wikibutton.Text = "List wiki tree";
            this.Wikibutton.UseVisualStyleBackColor = true;
            this.Wikibutton.Click += new System.EventHandler(this.Wikibutton_Click);
            // 
            // COLbutton
            // 
            this.COLbutton.Location = new System.Drawing.Point(614, 294);
            this.COLbutton.Name = "COLbutton";
            this.COLbutton.Size = new System.Drawing.Size(101, 23);
            this.COLbutton.TabIndex = 5;
            this.COLbutton.Text = "List COL tree";
            this.COLbutton.UseVisualStyleBackColor = true;
            this.COLbutton.Click += new System.EventHandler(this.COLbutton_Click);
            // 
            // loadtreebutton
            // 
            this.loadtreebutton.Location = new System.Drawing.Point(612, 334);
            this.loadtreebutton.Name = "loadtreebutton";
            this.loadtreebutton.Size = new System.Drawing.Size(103, 54);
            this.loadtreebutton.TabIndex = 6;
            this.loadtreebutton.Text = "Load both COL and Wiki trees";
            this.loadtreebutton.UseVisualStyleBackColor = true;
            this.loadtreebutton.Click += new System.EventHandler(this.loadtreebutton_Click);
            // 
            // MScomparetrees
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 678);
            this.Controls.Add(this.loadtreebutton);
            this.Controls.Add(this.COLbutton);
            this.Controls.Add(this.Wikibutton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LBwiki);
            this.Controls.Add(this.TBtaxon);
            this.Controls.Add(this.richTextBox1);
            this.Name = "MScomparetrees";
            this.Text = "MScomparetrees";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TextBox TBtaxon;
        private System.Windows.Forms.ListBox LBwiki;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Wikibutton;
        private System.Windows.Forms.Button COLbutton;
        private System.Windows.Forms.Button loadtreebutton;
    }
}