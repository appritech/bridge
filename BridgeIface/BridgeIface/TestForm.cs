using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BridgeIface
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        
        private void parse_button_Click(object sender, EventArgs e)
        {
            if (NMEA_String.Text.Length == 0)
                NMEA_String.Text = "insert string here";
            else;//run the parsing algorithm
            ;
        }
    }
}
