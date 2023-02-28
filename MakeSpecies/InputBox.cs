using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MakeSpecies
{
    public partial class InputBox : Form
    {
        public InputBox(string prompt,bool hidetext)
        {
            InitializeComponent();
            label1.Text = prompt;
            if (hidetext)
            {
                textBox1.PasswordChar = '*';
                textBox1.UseSystemPasswordChar = true;
            }
            textBox1.Focus();
            textBox1.Select();

            
        }

        private void OKbutton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public string gettext()
        {
            return textBox1.Text;
        }
    }
}
