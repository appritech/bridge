using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace BridgeIface
{
    public partial class TestForm : Form
    {
        List<DHControl> m_outputControls = new List<DHControl>();           //This is for later, when trying to stimulate our output to VStep
        List<DHControl> m_inputControls = new List<DHControl>();//This is for taking DataHolder values (written to by parsing NMEA), and displaying them on the screen
        
        Dictionary<string, int> sentNmeaTypes = new Dictionary<string, int>();
        Dictionary<string, int> receivedNmeaTypes = new Dictionary<string, int>();

        NMEA_Parser nmeaParser = new NMEA_Parser();
        NMEA_Com nmeaCom = new NMEA_Com();
        Utilities util = new Utilities();
        NMEA_Object nmeaObject = new NMEA_Object();

        ioioIO myIoioIo = new ioioIO();


        BindingList<NMEA_Object> tableSentNmeaStrings = new BindingList<NMEA_Object>();
        BindingList<NMEA_Object> tableReceivedNmeaStrings = new BindingList<NMEA_Object>();

        IPEndPoint sendEP = new IPEndPoint(IPAddress.Parse("192.168.0.21"), 8011); //This gets overwritten by GUI

        public TestForm()
        {
            InitializeComponent();

            //Outpus TO Nautis
            //PRC Components
            m_inputControls.Add(new DHControl("Remote Engine 0 Lever Demand Position Command", FloatIntBoolNone.Float, prcLeverPos1, false));
            m_outputControls.Add(new DHControl("Remote Engine 0 Lever Demand Position Command", FloatIntBoolNone.Float, prcLeverTrackbar1, true));
            m_inputControls.Add(new DHControl("Remote Engine 0 RPM Demand Command", FloatIntBoolNone.Float, prcRpmDemand1, false));
            m_outputControls.Add(new DHControl("Remote Engine 0 RPM Demand Command", FloatIntBoolNone.Float, prcRpmTrackbar1, true));
            m_inputControls.Add(new DHControl("Remote Engine 0 Pitch Demand Command", FloatIntBoolNone.Float, prcPitchDemand1, false));
            m_outputControls.Add(new DHControl("Remote Engine 0 Pitch Demand Command", FloatIntBoolNone.Float, prcPitchTrackbar1, true));
            //ETL Components
            m_inputControls.Add(new DHControl("Engine 0 Telegraph Position Command", FloatIntBoolNone.Float, etlTelegraphPos1, false));
            m_inputControls.Add(new DHControl("Engine 0 Telegraph Position Command", FloatIntBoolNone.Float, etlTelegraphTrackbar1, true));
            m_inputControls.Add(new DHControl("Engine 0 Sub-Telegraph Position Command", FloatIntBoolNone.Float, etlSubTelPos1, false));
            m_inputControls.Add(new DHControl("Engine 0 Sub-Telegraph Position Command", FloatIntBoolNone.Float, etlSubTelTrackbar1, true));
            //RSA
            m_outputControls.Add(new DHControl("Rudder Sensor Angle", FloatIntBoolNone.Float, rsaLever, true));
            m_inputControls.Add(new DHControl("Rudder Sensor Angle", FloatIntBoolNone.Float, rsaDhTb, false));


            //Inputs FROM Nautis 
            //ROR
            m_outputControls.Add(new DHControl("Rudder Angle", FloatIntBoolNone.Float, rorLever, true));
            m_inputControls.Add(new DHControl("Rudder Angle", FloatIntBoolNone.Float, rorDhTb, false));
            //EOM Components
            m_inputControls.Add(new DHControl("Mission Status", FloatIntBoolNone.Float, eomMissionStatus, false));
            m_inputControls.Add(new DHControl("Mission Elapsed Time", FloatIntBoolNone.Float, eomElapsedTime, false));
            //RPM Components
            m_inputControls.Add(new DHControl("Engine 0 Shaft RPM", FloatIntBoolNone.Float, rpmShaftSpeed1, false));
            m_outputControls.Add(new DHControl("Engine 0 Shaft RPM", FloatIntBoolNone.Float, rpmShaftSpeedTrackbar1, true));
            m_inputControls.Add(new DHControl("Engine 0 Engine RPM", FloatIntBoolNone.Float, rpmEngSpeed1, false));
            m_outputControls.Add(new DHControl("Engine 0 Engine RPM", FloatIntBoolNone.Float, rpmEngSpeedTrackbar1, true));
            m_inputControls.Add(new DHControl("Engine 0 Propeller Pitch", FloatIntBoolNone.Float, rpmPropPitch1, false));
            m_outputControls.Add(new DHControl("Engine 0 Propeller Pitch", FloatIntBoolNone.Float, rpmPropPitchTrackbar1, true));
            //PRC Components
            m_inputControls.Add(new DHControl("Remote Engine 0 Lever Demand Position", FloatIntBoolNone.Float, prcLeverPos1Rec, false));
            m_inputControls.Add(new DHControl("Remote Engine 0 Lever Demand Position", FloatIntBoolNone.Float, prcLeverTrackbar1Rec, false));
            m_inputControls.Add(new DHControl("Remote Engine 0 RPM Demand", FloatIntBoolNone.Float, prcRpmDemand1Rec, false));
            m_inputControls.Add(new DHControl("Remote Engine 0 RPM Demand", FloatIntBoolNone.Float, prcRpmTrackbar1Rec, false));
            m_inputControls.Add(new DHControl("Remote Engine 0 Pitch Demand", FloatIntBoolNone.Float, prcPitchDemand1Rec, false));
            m_inputControls.Add(new DHControl("Remote Engine 0 Pitch Demand", FloatIntBoolNone.Float, prcPitchTrackbar1Rec, false));
            //ETL Components
            m_inputControls.Add(new DHControl("Engine 0 Sub-Telegraph Position", FloatIntBoolNone.Float, etlSubTelPos1Rec, false));
            m_inputControls.Add(new DHControl("Engine 0 Sub-Telegraph Position", FloatIntBoolNone.Float, etlSubTelTrackbar1Rec, false));


            timer1.Enabled = true;
            timer_HW_input.Enabled = true;

            dataGridReceivedNMEA.DataSource = tableReceivedNmeaStrings; //Something about this is causing program to hang when table content exceeds datagrid height.
            dataGridSentNMEA.DataSource = tableSentNmeaStrings;

        }

        private void prcSendButton1_Click(object sender, EventArgs e)
        {
            DataHolderIface.SetFloatVal("GUI PRC Send", prcRpmTrackbar1.Value);
        }
        private void etlSendButton1_Click(object sender, EventArgs e)
        {
            DataHolderIface.SetFloatVal("GUI ETL-Sub Send", float.Parse(etlSubTelPos1.Text));
        }

        private void udpReceiveButton_Click(object sender, EventArgs e)
        {
            int portReceive = int.Parse(tbUdpRecPort.Text);
            Thread udpReadThread = new Thread(() => nmeaCom.receiveNmeaMessage(portReceive, tableReceivedNmeaStrings));
            CheckForIllegalCrossThreadCalls = false;
            udpReadThread.IsBackground = true;

            bool receiveEnabled = (udpReceiveButton.Text == "Start");
            if (receiveEnabled)
            {
                udpReadThread.Start();
                udpReceiveButton.Text = "Stop";
                nmeaCom.runThread = true;
                updReceiveLabel.Visible = true;
                tbUdpRecPort.Enabled = false;
            }
            else
            {
                udpReadThread.Abort();
                udpReceiveButton.Text = "Start";
                nmeaCom.runThread = false;
                updReceiveLabel.Visible = false;
                tbUdpRecPort.Enabled = true;
            }
        }
        private void parse_button_Click(object sender, EventArgs e)
        {
            //parser(NMEA_String_Box.Text);
            string sentence = NMEA_String_Box.Text;
            string index = nmeaParser.parser(sentence);

            nmeaCom.updateReceivedTable(sentence, index, tableReceivedNmeaStrings);
        }
        bool outputEnabled;
        private void outputEnableButton_Click(object sender, EventArgs e)
        {
            outputEnabled = (outputEnableButton.Text == "Enable");
            if (outputEnabled)
            {
                sendEP = new IPEndPoint(IPAddress.Parse(tbUdpSendIP.Text), int.Parse(tbUdpSendPort.Text));
                outputEnableLabel.Text = "Output is Enabled";
                outputEnableButton.Text = "Disable";
            }
            else
            {
                outputEnableLabel.Text = "Output is Disabled";
                outputEnableButton.Text = "Enable";
            }

            gbSendNmea.Enabled = outputEnabled;
            tbSelectedFile.Enabled = !outputEnabled;
            buttonFileSelect.Enabled = !outputEnabled;
            tbUdpSendIP.Enabled = !outputEnabled;
            tbUdpSendPort.Enabled = !outputEnabled;
        }

        private void trackbar_ValueChanged(object sender, EventArgs e)
        {
            TrackBar tb = (TrackBar)sender;
            switch (tb.Name)
            {
                //case ("rpmShaftSpeedTrackbar1"):
                //case ("rpmPropPitchTrackbar1"):
                //case ("rpmEngSpeedTrackbar1"):
                //    DataHolderIface.SetFloatVal("GUI RPM Send", rpmShaftSpeedTrackbar1.Value);
                //    break;
                case ("prcRpmTrackbar1"):
                    DataHolderIface.SetFloatVal("GUI RPM Send", prcRpmTrackbar1.Value);
                    break;
                case ("prcPitchTrackbar1"):
                    DataHolderIface.SetFloatVal("GUI Pitch Send", prcPitchTrackbar1.Value);
                    break;
                case ("rorLever"):
                    DataHolderIface.SetFloatVal("GUI ROR Send", rorLever.Value);
                    break;
                case ("rsaLever"):
                    //Example: $GPRSA,20.3,A,0.0,V*71
                    DataHolderIface.SetFloatVal("GUI RSA Send", rsaLever.Value);
                    break;
                default:
                    break; //place breakpoint here to catch unhandled trackbar changes.
            }
        }


        //Unused Button Clicks
        private void rpmEngSendButton1_Click(object sender, EventArgs e)
        {
            string s = "$--RPM,E," + ",1," + rpmEngSpeed1.Text + "," + rpmPropPitch1.Text + ",A*hh\r\n";
            lastStringSent.Text = s;
        }
        private void rpmShaftSendButton1_Click(object sender, EventArgs e)
        {
            string s = "$--RPM,E," + ",1," + rpmShaftSpeed1.Text + "," + rpmPropPitch1.Text + ",A*hh\r\n";
            lastStringSent.Text = s;
        }
        private void sendTrc1Button_Click(object sender, EventArgs e)
        {
          //  string s_1 = "--TRC,1," + trcRpmDemandDisplay1.Text + ",P," + trcPitchDemandDisplay1.Text + ",P," + float.Parse(trcAzimuthDemandDisplay1.Text) * 10 + ",S,C";
          //  string s = "$" + s_1 + "*" + calcChecksum(s_1) + "\r\n";
          //  lastStringSent.Text = s;
          //  sendNmeaMessage(s);
        }
        private void trdSendButton1_Click(object sender, EventArgs e)
        {

            string s = "$--TRD,1," + trdRpmResponse1.Text + ",P," + trdPitchResponse1.Text + ",P," + float.Parse(trdAzimuthResponse1.Text) * 10 + "*hh\r\n";
            lastStringSent.Text = s;
        }
        private void sendTrc2Button_Click(object sender, EventArgs e)
        {
            string s = "$--TRC,2," + trcRpmDemandDisplay2.Text + ",P," + trcPitchDemandDisplay2.Text + ",P," + float.Parse(trcAzimuthDemandDisplay2.Text) * 10 + ",S,C*hh\r\n";
            lastStringSent.Text = s;
        }
        private void trdSendButton2_Click(object sender, EventArgs e)
        {
            string s = "$--TRD,2," + trdRpmResponse2.Text + ",P," + trdPitchResponse2.Text + ",P," + float.Parse(trdAzimuthResponse2.Text) * 10 + "*hh\r\n";
            lastStringSent.Text = s;
        }
        private void prcSendButton2_Click_1(object sender, EventArgs e)
        {
            string s = "$--PRC," + prcLeverPos2.Text + ",A," + prcRpmDemand2.Text + ",P," + prcPitchDemand2.Text + ",P,S,2*hh\r\n";
            lastStringSent.Text = s;
        }
        private void rpmEngSendButton2_Click(object sender, EventArgs e)
        {
            string s = "$--RPM,E," + ",1," + rpmEngSpeed2.Text + "," + rpmPropPitch2.Text + ",A*hh\r\n";
            lastStringSent.Text = s;
        }
        private void rpmShaftSendButton2_Click(object sender, EventArgs e)
        {
            string s = "$--RPM,E," + ",1," + rpmShaftSpeed2.Text + "," + rpmPropPitch2.Text + ",A*hh\r\n";
            lastStringSent.Text = s;
        }
        private void etlSendButton2_Click(object sender, EventArgs e)
        {
            string s = "$--ETL,0,O," + etlTelegraphPos2.Text + "," + etlSubTelPos2.Text + ",S,2*hh\r\n";
            lastStringSent.Text = s;
        }


        
        
        private void timer_HW_input_Tick(object sender, EventArgs e)
        {
            if (cbHardwareControl.Checked && outputEnabled)
            {
                //set lever positions based on hardware levers
                string website = "http://" + tbIpAddress.Text + ":8181/api/status";
                nmeaCom.parseXml(myIoioIo.webRequest(website));
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (DHControl dh in m_inputControls)
            {
                dh.readFromDataHolder();
            }
            if (cbRsaHold.Checked)
            {
                DataHolderIface.SetFloatVal("GUI RSA Send", rsaLever.Value);
            }

            if (outputEnabled)
            {
                //Send NMEA Commands based upon streamreader file
                try
                {
                    StreamReader file = new StreamReader(tbSelectedFile.Text);
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        //remove comments
                        int index = line.IndexOf("//");
                        string nmeaCommand = (index == -1) ? line : line.Substring(0, index);
                        //nmeaCommand.Trim();
                        if (nmeaCommand != "") //don't send blank lines
                            nmeaCom.sendDynamicNmea(sendEP, nmeaCommand.Trim(), tableSentNmeaStrings);
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void buttonFileSelect_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                tbSelectedFile.Text = openFileDialog1.FileName;
            }
        }

        private void clearTable_button_Click(object sender, EventArgs e)
        {
            sentNmeaTypes.Clear();
            tableReceivedNmeaStrings.Clear();
        }

        private void clearSendTable_button_Click(object sender, EventArgs e)
        {
            //receivedNmeaTypes.Clear();
            tableSentNmeaStrings.Clear();
        }
    }
}
