using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            label1.Text = "";
            string s="";
            if (e.Modifiers == Keys.Control) { s += "Ctrl"; }
            if (e.Modifiers == Keys.Alt) { s += "Alt"; }
            if (e.Modifiers == Keys.Shift) { s += "Shift"; }
            
            label1.Text = s;
        }

        private void Form2_KeyUp(object sender, KeyEventArgs e)
        {
            label2.Text = "";
            string s = "";
            if (e.Modifiers == Keys.Control) { s += "Ctrl"; }
            if (e.Modifiers == Keys.Alt) { s += "Alt"; }
            if (e.Modifiers == Keys.Shift) { s += "Shift"; }

            label2.Text = s;

        }
    }
}
