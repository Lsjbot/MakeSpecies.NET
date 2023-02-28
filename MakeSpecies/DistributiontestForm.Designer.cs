namespace MakeSpecies
{
    partial class DistributiontestForm
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
            this.Quitbutton = new System.Windows.Forms.Button();
            this.Statsbutton = new System.Windows.Forms.Button();
            this.Parsebutton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(51, 43);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(444, 351);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // Quitbutton
            // 
            this.Quitbutton.Location = new System.Drawing.Point(658, 407);
            this.Quitbutton.Name = "Quitbutton";
            this.Quitbutton.Size = new System.Drawing.Size(109, 23);
            this.Quitbutton.TabIndex = 1;
            this.Quitbutton.Text = "Quit";
            this.Quitbutton.UseVisualStyleBackColor = true;
            this.Quitbutton.Click += new System.EventHandler(this.Quitbutton_Click);
            // 
            // Statsbutton
            // 
            this.Statsbutton.Location = new System.Drawing.Point(658, 316);
            this.Statsbutton.Name = "Statsbutton";
            this.Statsbutton.Size = new System.Drawing.Size(109, 48);
            this.Statsbutton.TabIndex = 2;
            this.Statsbutton.Text = "Distribution statistics";
            this.Statsbutton.UseVisualStyleBackColor = true;
            this.Statsbutton.Click += new System.EventHandler(this.Statsbutton_Click);
            // 
            // Parsebutton
            // 
            this.Parsebutton.Location = new System.Drawing.Point(662, 233);
            this.Parsebutton.Name = "Parsebutton";
            this.Parsebutton.Size = new System.Drawing.Size(105, 51);
            this.Parsebutton.TabIndex = 3;
            this.Parsebutton.Text = "Test parsing";
            this.Parsebutton.UseVisualStyleBackColor = true;
            this.Parsebutton.Click += new System.EventHandler(this.Parsebutton_Click);
            // 
            // DistributiontestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Parsebutton);
            this.Controls.Add(this.Statsbutton);
            this.Controls.Add(this.Quitbutton);
            this.Controls.Add(this.richTextBox1);
            this.Name = "DistributiontestForm";
            this.Text = "DistributiontestForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button Quitbutton;
        private System.Windows.Forms.Button Statsbutton;
        private System.Windows.Forms.Button Parsebutton;
    }
}