namespace MakeSpecies
{
    partial class CebEng
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
            this.Gobutton = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.Quitbutton = new System.Windows.Forms.Button();
            this.Distbutton = new System.Windows.Forms.Button();
            this.Partofbutton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Gobutton
            // 
            this.Gobutton.Location = new System.Drawing.Point(450, 293);
            this.Gobutton.Name = "Gobutton";
            this.Gobutton.Size = new System.Drawing.Size(108, 55);
            this.Gobutton.TabIndex = 0;
            this.Gobutton.Text = "Check species names against enwp";
            this.Gobutton.UseVisualStyleBackColor = true;
            this.Gobutton.Click += new System.EventHandler(this.Gobutton_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(24, 20);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(389, 261);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // Quitbutton
            // 
            this.Quitbutton.Location = new System.Drawing.Point(450, 384);
            this.Quitbutton.Name = "Quitbutton";
            this.Quitbutton.Size = new System.Drawing.Size(108, 39);
            this.Quitbutton.TabIndex = 2;
            this.Quitbutton.Text = "Quit";
            this.Quitbutton.UseVisualStyleBackColor = true;
            this.Quitbutton.Click += new System.EventHandler(this.Quitbutton_Click);
            // 
            // Distbutton
            // 
            this.Distbutton.Location = new System.Drawing.Point(450, 210);
            this.Distbutton.Name = "Distbutton";
            this.Distbutton.Size = new System.Drawing.Size(108, 53);
            this.Distbutton.TabIndex = 3;
            this.Distbutton.Text = "Find distribution links";
            this.Distbutton.UseVisualStyleBackColor = true;
            this.Distbutton.Click += new System.EventHandler(this.Distbutton_Click);
            // 
            // Partofbutton
            // 
            this.Partofbutton.Location = new System.Drawing.Point(450, 140);
            this.Partofbutton.Name = "Partofbutton";
            this.Partofbutton.Size = new System.Drawing.Size(108, 50);
            this.Partofbutton.TabIndex = 4;
            this.Partofbutton.Text = "Get part of for maindist";
            this.Partofbutton.UseVisualStyleBackColor = true;
            this.Partofbutton.Click += new System.EventHandler(this.Partofbutton_Click);
            // 
            // CebEng
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 450);
            this.Controls.Add(this.Partofbutton);
            this.Controls.Add(this.Distbutton);
            this.Controls.Add(this.Quitbutton);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.Gobutton);
            this.Name = "CebEng";
            this.Text = "CebEng";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Gobutton;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button Quitbutton;
        private System.Windows.Forms.Button Distbutton;
        private System.Windows.Forms.Button Partofbutton;
    }
}