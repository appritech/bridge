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
        List<DHControl> m_outputControls = new List<DHControl>();
        List<DHControl> m_inputControls = new List<DHControl>();
        
        Dictionary<string, int> sentNmeaTypes = new Dictionary<string, int>();
        Dictionary<string, int> receivedNmeaTypes = new Dictionary<string, int>();

        NMEA_Parser nmeaParser = new NMEA_Parser();
        NMEA_Com nmeaCom = new NMEA_Com();
        Utilities util = new Utilities();
        NMEA_Object nmeaObject = new NMEA_Object();

        Ioio_Com ioioCom = new Ioio_Com();

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
            //RSA
            m_outputControls.Add(new DHControl("Rudder Sensor Angle", FloatIntBoolNone.Float, rsaLever, true));
            m_inputControls.Add(new DHControl("Rudder Sensor Angle", FloatIntBoolNone.Float, rsaDhTb, false));


            //Inputs FROM Nautis 
            //ROR
            m_outputControls.Add(new DHControl("Rudder Angle", FloatIntBoolNone.Float, rorLever, true));
            m_inputControls.Add(new DHControl("Rudder Angle", FloatIntBoolNone.Float, rorDhTb, false));
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

           // if (InvokeRequired)
           //     Invoke(new MethodInvoker(() => nmeaCom.receiveNmeaMessage(portReceive, tableReceivedNmeaStrings)));

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
           // if (dataGridReceivedNMEA.InvokeRequired)
           // {
           //     dataGridReceivedNMEA.Invoke(new MethodInvoker(() => dataGridReceivedNMEA.DataSource = tableReceivedNmeaStrings));
           // }
            //dataGridReceivedNMEA.DataSource = tableReceivedNmeaStrings; //Something about this is causing program to hang when table content exceeds datagrid height.
        }

        private void parse_button_Click(object sender, EventArgs e)
        {
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
        
        private void timer_HW_input_Tick(object sender, EventArgs e)
        {
            if (cbHardwareControl.Checked && outputEnabled)
            {
                //set lever positions based on hardware levers
                string website = "http://" + tbIpAddress.Text + ":8181/api/status";
                string xmlResponse = ioioCom.webRequest(website);
                ioioCom.parseXml(xmlResponse);
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
                StreamReader file = new StreamReader(tbSelectedFile.Text);
                nmeaCom.sendDynamicNmea(sendEP, file, tableSentNmeaStrings);
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
            //sentNmeaTypes.Clear();
            tableReceivedNmeaStrings.Clear();
        }

        private void clearSendTable_button_Click(object sender, EventArgs e)
        {
            //receivedNmeaTypes.Clear();
            tableSentNmeaStrings.Clear();
        }
    }
}
