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
            thrusterNum = data[1];
            rpmDemand = data[2];
            rpmMode = data[3];
            pitchDemand = data[4];
            pitchMode = data[5];
            azimuthDemand = data[6];
            operatorLocation = data[7];
            sentenceStatus = data[8];
            checkSum = data[9];
            updateDisplay(data[1]);
        }

        private void updateDisplay(String s)
        {
            if (s == "1")
            {
                rpmDemandDisplay1.Text = rpmDemand + convMode(rpmMode);
                pitchDemandDisplay1.Text = pitchDemand + convMode(pitchMode);
                azimuthDemandDisplay1.Text = azimuthDemand + "°";
                locationDisplay1.Text = convLocation(operatorLocation);
                sentenceStatusDisplay1.Text = convStatus(sentenceStatus);
            }
            else if (s == "2")
            {
                rpmDemandDisplay2.Text = rpmDemand + convMode(rpmMode);
                pitchDemandDisplay2.Text = pitchDemand + convMode(pitchMode);
                azimuthDemandDisplay2.Text = azimuthDemand + "°";
                locationDisplay2.Text = convLocation(operatorLocation);
                sentenceStatusDisplay2.Text = convStatus(sentenceStatus);
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

        string thrusterNum = null;
        string rpmDemand = null;
        string rpmMode = null;
        string pitchDemand = null;
        string pitchMode = null;
        string azimuthDemand = null;
        string operatorLocation = null;
        string sentenceStatus = null;
        string checkSum = null;

    }
}
