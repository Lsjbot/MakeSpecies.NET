namespace MakeSpecies
{
    partial class Form1
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
            this.CompareTreebutton = new System.Windows.Forms.Button();
            this.Quitbutton = new System.Windows.Forms.Button();
            this.langfixbutton = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.duplicatebutton = new System.Windows.Forms.Button();
            this.makebutton = new System.Windows.Forms.Button();
            this.cebengbutton = new System.Windows.Forms.Button();
            this.Badfindbutton = new System.Windows.Forms.Button();
            this.Distributionbutton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.namelistbutton = new System.Windows.Forms.Button();
            this.testbutton = new System.Windows.Forms.Button();
            this.Duplicatefixbutton = new System.Windows.Forms.Button();
            this.Disambigbutton = new System.Windows.Forms.Button();
            this.disambigDBbutton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CompareTreebutton
            // 
            this.CompareTreebutton.Location = new System.Drawing.Point(726, 19);
            this.CompareTreebutton.Name = "CompareTreebutton";
            this.CompareTreebutton.Size = new System.Drawing.Size(111, 23);
            this.CompareTreebutton.TabIndex = 0;
            this.CompareTreebutton.Text = "Compare trees";
            this.CompareTreebutton.UseVisualStyleBackColor = true;
            this.CompareTreebutton.Click += new System.EventHandler(this.CompareTreebutton_Click);
            // 
            // Quitbutton
            // 
            this.Quitbutton.Location = new System.Drawing.Point(726, 352);
            this.Quitbutton.Name = "Quitbutton";
            this.Quitbutton.Size = new System.Drawing.Size(110, 47);
            this.Quitbutton.TabIndex = 1;
            this.Quitbutton.Text = "Quit";
            this.Quitbutton.UseVisualStyleBackColor = true;
            this.Quitbutton.Click += new System.EventHandler(this.Quitbutton_Click);
            // 
            // langfixbutton
            // 
            this.langfixbutton.Location = new System.Drawing.Point(726, 48);
            this.langfixbutton.Name = "langfixbutton";
            this.langfixbutton.Size = new System.Drawing.Size(111, 23);
            this.langfixbutton.TabIndex = 2;
            this.langfixbutton.Text = "Fix COL languages";
            this.langfixbutton.UseVisualStyleBackColor = true;
            this.langfixbutton.Click += new System.EventHandler(this.langfixbutton_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 41);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(552, 358);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // duplicatebutton
            // 
            this.duplicatebutton.Location = new System.Drawing.Point(727, 145);
            this.duplicatebutton.Name = "duplicatebutton";
            this.duplicatebutton.Size = new System.Drawing.Size(111, 37);
            this.duplicatebutton.TabIndex = 4;
            this.duplicatebutton.Text = "Find duplicate names";
            this.duplicatebutton.UseVisualStyleBackColor = true;
            this.duplicatebutton.Click += new System.EventHandler(this.duplicatebutton_Click);
            // 
            // makebutton
            // 
            this.makebutton.Location = new System.Drawing.Point(726, 266);
            this.makebutton.Name = "makebutton";
            this.makebutton.Size = new System.Drawing.Size(110, 48);
            this.makebutton.TabIndex = 5;
            this.makebutton.Text = "Make articles";
            this.makebutton.UseVisualStyleBackColor = true;
            this.makebutton.Click += new System.EventHandler(this.makebutton_Click);
            // 
            // cebengbutton
            // 
            this.cebengbutton.Location = new System.Drawing.Point(727, 77);
            this.cebengbutton.Name = "cebengbutton";
            this.cebengbutton.Size = new System.Drawing.Size(110, 23);
            this.cebengbutton.TabIndex = 6;
            this.cebengbutton.Text = "Enwp checking";
            this.cebengbutton.UseVisualStyleBackColor = true;
            this.cebengbutton.Click += new System.EventHandler(this.cebengbutton_Click);
            // 
            // Badfindbutton
            // 
            this.Badfindbutton.Location = new System.Drawing.Point(727, 106);
            this.Badfindbutton.Name = "Badfindbutton";
            this.Badfindbutton.Size = new System.Drawing.Size(110, 33);
            this.Badfindbutton.TabIndex = 7;
            this.Badfindbutton.Text = "Find bad articles";
            this.Badfindbutton.UseVisualStyleBackColor = true;
            this.Badfindbutton.Click += new System.EventHandler(this.Badfindbutton_Click);
            // 
            // Distributionbutton
            // 
            this.Distributionbutton.Location = new System.Drawing.Point(727, 188);
            this.Distributionbutton.Name = "Distributionbutton";
            this.Distributionbutton.Size = new System.Drawing.Size(109, 25);
            this.Distributionbutton.TabIndex = 8;
            this.Distributionbutton.Text = "Distribution stats";
            this.Distributionbutton.UseVisualStyleBackColor = true;
            this.Distributionbutton.Click += new System.EventHandler(this.Distributionbutton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(570, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(150, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "Test human_touched";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // namelistbutton
            // 
            this.namelistbutton.Location = new System.Drawing.Point(571, 48);
            this.namelistbutton.Name = "namelistbutton";
            this.namelistbutton.Size = new System.Drawing.Size(149, 23);
            this.namelistbutton.TabIndex = 10;
            this.namelistbutton.Text = "Harmonize name lists";
            this.namelistbutton.UseVisualStyleBackColor = true;
            this.namelistbutton.Click += new System.EventHandler(this.namelistbutton_Click);
            // 
            // testbutton
            // 
            this.testbutton.Location = new System.Drawing.Point(571, 77);
            this.testbutton.Name = "testbutton";
            this.testbutton.Size = new System.Drawing.Size(150, 23);
            this.testbutton.TabIndex = 11;
            this.testbutton.Text = "Test misc";
            this.testbutton.UseVisualStyleBackColor = true;
            this.testbutton.Click += new System.EventHandler(this.testbutton_Click);
            // 
            // Duplicatefixbutton
            // 
            this.Duplicatefixbutton.Location = new System.Drawing.Point(571, 188);
            this.Duplicatefixbutton.Name = "Duplicatefixbutton";
            this.Duplicatefixbutton.Size = new System.Drawing.Size(149, 25);
            this.Duplicatefixbutton.TabIndex = 12;
            this.Duplicatefixbutton.Text = "Manage duplicate names";
            this.Duplicatefixbutton.UseVisualStyleBackColor = true;
            this.Duplicatefixbutton.Click += new System.EventHandler(this.Duplicatefixbutton_Click);
            // 
            // Disambigbutton
            // 
            this.Disambigbutton.Location = new System.Drawing.Point(571, 106);
            this.Disambigbutton.Name = "Disambigbutton";
            this.Disambigbutton.Size = new System.Drawing.Size(149, 33);
            this.Disambigbutton.TabIndex = 13;
            this.Disambigbutton.Text = "Handle old disambigs";
            this.Disambigbutton.UseVisualStyleBackColor = true;
            this.Disambigbutton.Click += new System.EventHandler(this.Disambigbutton_Click);
            // 
            // disambigDBbutton
            // 
            this.disambigDBbutton.Location = new System.Drawing.Point(571, 145);
            this.disambigDBbutton.Name = "disambigDBbutton";
            this.disambigDBbutton.Size = new System.Drawing.Size(149, 37);
            this.disambigDBbutton.TabIndex = 14;
            this.disambigDBbutton.Text = "Disambigs into database";
            this.disambigDBbutton.UseVisualStyleBackColor = true;
            this.disambigDBbutton.Click += new System.EventHandler(this.disambigDBbutton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(909, 469);
            this.Controls.Add(this.disambigDBbutton);
            this.Controls.Add(this.Disambigbutton);
            this.Controls.Add(this.Duplicatefixbutton);
            this.Controls.Add(this.testbutton);
            this.Controls.Add(this.namelistbutton);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Distributionbutton);
            this.Controls.Add(this.Badfindbutton);
            this.Controls.Add(this.cebengbutton);
            this.Controls.Add(this.makebutton);
            this.Controls.Add(this.duplicatebutton);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.langfixbutton);
            this.Controls.Add(this.Quitbutton);
            this.Controls.Add(this.CompareTreebutton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CompareTreebutton;
        private System.Windows.Forms.Button Quitbutton;
        private System.Windows.Forms.Button langfixbutton;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button duplicatebutton;
        private System.Windows.Forms.Button makebutton;
        private System.Windows.Forms.Button cebengbutton;
        private System.Windows.Forms.Button Badfindbutton;
        private System.Windows.Forms.Button Distributionbutton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button namelistbutton;
        private System.Windows.Forms.Button testbutton;
        private System.Windows.Forms.Button Duplicatefixbutton;
        private System.Windows.Forms.Button Disambigbutton;
        private System.Windows.Forms.Button disambigDBbutton;
    }
}

