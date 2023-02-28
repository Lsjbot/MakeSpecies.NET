namespace MakeSpecies
{
    partial class BadArticles
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
            this.Quitbutton = new System.Windows.Forms.Button();
            this.CatCheckbutton = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.Sortbutton = new System.Windows.Forms.Button();
            this.retrybutton = new System.Windows.Forms.Button();
            this.towikibutton = new System.Windows.Forms.Button();
            this.Trivnamebutton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Quitbutton
            // 
            this.Quitbutton.Location = new System.Drawing.Point(701, 400);
            this.Quitbutton.Name = "Quitbutton";
            this.Quitbutton.Size = new System.Drawing.Size(75, 23);
            this.Quitbutton.TabIndex = 0;
            this.Quitbutton.Text = "Quit";
            this.Quitbutton.UseVisualStyleBackColor = true;
            this.Quitbutton.Click += new System.EventHandler(this.Quitbutton_Click);
            // 
            // CatCheckbutton
            // 
            this.CatCheckbutton.Location = new System.Drawing.Point(663, 15);
            this.CatCheckbutton.Name = "CatCheckbutton";
            this.CatCheckbutton.Size = new System.Drawing.Size(105, 65);
            this.CatCheckbutton.TabIndex = 1;
            this.CatCheckbutton.Text = "Check botmade categories";
            this.CatCheckbutton.UseVisualStyleBackColor = true;
            this.CatCheckbutton.Click += new System.EventHandler(this.CatCheckbutton_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(26, 38);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(474, 310);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // Sortbutton
            // 
            this.Sortbutton.Location = new System.Drawing.Point(663, 86);
            this.Sortbutton.Name = "Sortbutton";
            this.Sortbutton.Size = new System.Drawing.Size(105, 58);
            this.Sortbutton.TabIndex = 3;
            this.Sortbutton.Text = "Sort cat lists";
            this.Sortbutton.UseVisualStyleBackColor = true;
            this.Sortbutton.Click += new System.EventHandler(this.Sortbutton_Click);
            // 
            // retrybutton
            // 
            this.retrybutton.Location = new System.Drawing.Point(663, 150);
            this.retrybutton.Name = "retrybutton";
            this.retrybutton.Size = new System.Drawing.Size(105, 46);
            this.retrybutton.TabIndex = 4;
            this.retrybutton.Text = "Retry failed loads";
            this.retrybutton.UseVisualStyleBackColor = true;
            this.retrybutton.Click += new System.EventHandler(this.retrybutton_Click);
            // 
            // towikibutton
            // 
            this.towikibutton.Location = new System.Drawing.Point(663, 292);
            this.towikibutton.Name = "towikibutton";
            this.towikibutton.Size = new System.Drawing.Size(105, 46);
            this.towikibutton.TabIndex = 5;
            this.towikibutton.Text = "Move bad list to wiki";
            this.towikibutton.UseVisualStyleBackColor = true;
            this.towikibutton.Click += new System.EventHandler(this.towikibutton_Click);
            // 
            // Trivnamebutton
            // 
            this.Trivnamebutton.Location = new System.Drawing.Point(663, 202);
            this.Trivnamebutton.Name = "Trivnamebutton";
            this.Trivnamebutton.Size = new System.Drawing.Size(105, 44);
            this.Trivnamebutton.TabIndex = 6;
            this.Trivnamebutton.Text = "Find trivnames in cat lists";
            this.Trivnamebutton.UseVisualStyleBackColor = true;
            this.Trivnamebutton.Click += new System.EventHandler(this.Trivnamebutton_Click);
            // 
            // BadArticles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Trivnamebutton);
            this.Controls.Add(this.towikibutton);
            this.Controls.Add(this.retrybutton);
            this.Controls.Add(this.Sortbutton);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.CatCheckbutton);
            this.Controls.Add(this.Quitbutton);
            this.Name = "BadArticles";
            this.Text = "BadArticles";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Quitbutton;
        private System.Windows.Forms.Button CatCheckbutton;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button Sortbutton;
        private System.Windows.Forms.Button retrybutton;
        private System.Windows.Forms.Button towikibutton;
        private System.Windows.Forms.Button Trivnamebutton;
    }
}