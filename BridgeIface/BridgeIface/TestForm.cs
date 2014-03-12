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

            m_inputControls.Add(new DHControl("Bow Thruster RPM Demand", FloatIntBoolNone.Float, rpmDemandDisplay1, false));
            m_inputControls.Add(new DHControl("Stern Thruster RPM Demand", FloatIntBoolNone.Float, rpmDemandDisplay2, false));

            //Add similar m_inputControls.Add statements here for all the text boxes you want to display


            timer1.Enabled = true;
        }
        
        private void parse_button_Click(object sender, EventArgs e)
        {
            parser(NMEA_String.Text);
            //updateDisplay();
        }
        private void parser(String sentence)
        {
            errorMessage.Text = "";
            lastStringEntered.Text = sentence;
            string[] data = sentence.Split(',','*');

            switch (data[0])
            {
                case "$GCTRC":          //This one isn't real, just an example. We should get rid of it soon.
                case "$--TRC":
                    parseTRC(data);
                    break;
                case "$--ETL":
                    break;
                default:
                    errorMessage.Text = "Unrecognized NMEA_String String";
                    break;
            }
        }

        private void parseTRC(string[] data)
        {
            if (data.Length < 10)
            {
                errorMessage.Text = "NMEA Sentence not long enough";
                return;
            }
            sentenceTypeDisplay.Text = "Thruster Control Data";

            string thrusterNum = data[1];
            string rpmDemand = data[2];
            string rpmMode = data[3];
            string pitchDemand = data[4];
            string pitchMode = data[5];
            string azimuthDemand = data[6];
            string operatorLocation = data[7];
            string sentenceStatus = data[8];
            string checkSum = data[9];

            string thrusterName = "";

            switch(data[1])
            {
                case "1":
                    thrusterName = "Bow";
                    
                    break;
                case "2":
                    thrusterName = "Stern";
                    break;
                default:
                    errorMessage.Text = "Unsupported Thruster";
                    return;
            }

            DataHolderIface.SetFloatVal(thrusterName + " Thruster RPM Demand", calcPercentDemand(rpmDemand, rpmMode));          //Always write to dataholder the percent

           //thrusterNum = data[1];
            //rpmDemand = data[2];
            //rpmMode = data[3];
            //pitchDemand = data[4];
            //pitchMode = data[5];
            //azimuthDemand = data[6];
            //operatorLocation = data[7];
            //sentenceStatus = data[8];
            //checkSum = data[9];
            //updateDisplay(data[1]);
        }

        private const float MAX_DEGREES = 12.3f;
        private float calcPercentDemand(string rpmDemand, string rpmMode)
        {
            float value = float.Parse(rpmDemand);
            switch (rpmMode)
            {
                case "P":
                    return value;       //It is already a percent
                case "D":
                    return value * 100 / MAX_DEGREES;           //Turn into a percent
                case "V":
                    return 0.0f;        //???
                default:
                    return 0.0f;        //???
            }
        }

        private void updateDisplay(String s)
        {
            if (s == "1")
            {
                //rpmDemandDisplay1.Text = rpmDemand + convMode(rpmMode);
                //pitchDemandDisplay1.Text = pitchDemand + convMode(pitchMode);
                //azimuthDemandDisplay1.Text = azimuthDemand + "°";
                //locationDisplay1.Text = convLocation(operatorLocation);
                //sentenceStatusDisplay1.Text = convStatus(sentenceStatus);
            }
            else if (s == "2")
            {
                //rpmDemandDisplay2.Text = rpmDemand + convMode(rpmMode);
                //pitchDemandDisplay2.Text = pitchDemand + convMode(pitchMode);
                //azimuthDemandDisplay2.Text = azimuthDemand + "°";
                //locationDisplay2.Text = convLocation(operatorLocation);
                //sentenceStatusDisplay2.Text = convStatus(sentenceStatus);
            }
            else errorMessage.Text = "Unsupported Thruster";
        }

        private String convMode(String s)
        {
            switch (s)
            {
                case "P":
                    return "%";
                    break;
                case "D":
                    return "°";
                    break;
                case "V":
                    return "(invalid)";
                    break;
                default:
                    return "UNRECOGNIZED MODE";
                    break;
            }
        }
        private String convLocation(String s)
        {
            switch (s)
            {
                case "B":
                    return "Bridge";
                    break;
                case "P":
                    return "Port Wing";
                    break;
                case "S":
                    return "Starboard Wing";
                    break;
                case "C":
                    return "Engine Control Room";
                    break;
                case "E":
                    return "Engine Side / Local";
                    break;
                case "W":
                    return "Wing";
                    break;
                default:
                    return "UNRECOGNIZED LOCATION";
                    break;
            }
        }
        private String convStatus(String s)
        {
            switch (s)
            {
                case "R":
                    return "Status Report";
                    break;
                case "C":
                    return "Config Command";
                    break;
                default:
                    return "UNRECOGNIZED STATUS";
                    break;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (DHControl dh in m_inputControls)
            {
                dh.readFromDataHolder();
            }
        }

        //string thrusterNum = null;
        //string rpmDemand = null;
        //string rpmMode = null;
        //string pitchDemand = null;
        //string pitchMode = null;
        //string azimuthDemand = null;
        //string operatorLocation = null;
        //string sentenceStatus = null;
        //string checkSum = null;

        List<DHControl> m_outputControls = new List<DHControl>();           //This is for later, when trying to stimulate our output to VStep
        List<DHControl> m_inputControls = new List<DHControl>();            //This is for taking DataHolder values (written to by parsing NMEA), and displaying them on the screen

    }
}
