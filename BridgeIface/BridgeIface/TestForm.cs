﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace BridgeIface
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
            
            //TRC Components
            m_inputControls.Add(new DHControl("Thruster 0 RPM Demand", FloatIntBoolNone.Float, trcRpmDemandDisplay1, false));
            m_inputControls.Add(new DHControl("Thruster 0 Pitch Demand", FloatIntBoolNone.Float, trcPitchDemandDisplay1, false));
            m_inputControls.Add(new DHControl("Thruster 0 Azimuth Demand", FloatIntBoolNone.Float, trcAzimuthDemandDisplay1, false));
            m_inputControls.Add(new DHControl("Thruster 2 RPM Demand", FloatIntBoolNone.Float, trcRpmDemandDisplay2, false));
            m_inputControls.Add(new DHControl("Thruster 2 Pitch Demand", FloatIntBoolNone.Float, trcPitchDemandDisplay2, false));
            m_inputControls.Add(new DHControl("Thruster 2 Azimuth Demand", FloatIntBoolNone.Float, trcAzimuthDemandDisplay2, false));

            m_outputControls.Add(new DHControl("Thruster 0 RPM Demand", FloatIntBoolNone.Float, trcRpmTrackbar1, true));
            m_outputControls.Add(new DHControl("Thruster 0 Pitch Demand", FloatIntBoolNone.Float, trcPitchTrackbar1, true));
            m_outputControls.Add(new DHControl("Thruster 0 Azimuth Demand", FloatIntBoolNone.Float, trcAzimuthTrackbar1, true));
            m_outputControls.Add(new DHControl("Thruster 2 RPM Demand", FloatIntBoolNone.Float, trcRpmTrackbar2, true));
            m_outputControls.Add(new DHControl("Thruster 2 Pitch Demand", FloatIntBoolNone.Float, trcPitchTrackbar2, true));
            m_outputControls.Add(new DHControl("Thruster 2 Azimuth Demand", FloatIntBoolNone.Float, trcAzimuthTrackbar2, true));

            //ETL Components
            m_inputControls.Add(new DHControl("Engine 0 Telegraph Position", FloatIntBoolNone.Float, etlTelegraphPos1, false));
            m_inputControls.Add(new DHControl("Engine 0 Sub-Telegraph Position", FloatIntBoolNone.Float, etlSubTelPos1, false));
            m_inputControls.Add(new DHControl("Engine 2 Telegraph Position", FloatIntBoolNone.Float, etlTelegraphPos2, false));
            m_inputControls.Add(new DHControl("Engine 2 Sub-Telegraph Position", FloatIntBoolNone.Float, etlSubTelPos2, false));

            m_inputControls.Add(new DHControl("Engine 0 Telegraph Position", FloatIntBoolNone.Float, etlTelegraphTrackbar1, true));
            m_inputControls.Add(new DHControl("Engine 0 Sub-Telegraph Position", FloatIntBoolNone.Float, etlSubTelTrackbar1, true));
            m_inputControls.Add(new DHControl("Engine 2 Telegraph Position", FloatIntBoolNone.Float, etlTelegraphTrackbar2, true));
            m_inputControls.Add(new DHControl("Engine 2 Sub-Telegraph Position", FloatIntBoolNone.Float, etlSubTelTrackbar2, true));

            //PRC Components
            m_inputControls.Add(new DHControl("Remote Engine 0 Lever Demand Position", FloatIntBoolNone.Float, prcLeverPos1, false));
            m_inputControls.Add(new DHControl("Remote Engine 0 RPM Demand", FloatIntBoolNone.Float, prcRpmDemand1, false));
            m_inputControls.Add(new DHControl("Remote Engine 0 Pitch Demand", FloatIntBoolNone.Float, prcPitchDemand1, false));
            m_inputControls.Add(new DHControl("Remote Engine 2 Lever Demand Position", FloatIntBoolNone.Float, prcLeverPos2, false));
            m_inputControls.Add(new DHControl("Remote Engine 2 RPM Demand", FloatIntBoolNone.Float, prcRpmDemand2, false));
            m_inputControls.Add(new DHControl("Remote Engine 2 Pitch Demand", FloatIntBoolNone.Float, prcPitchDemand2, false));

            m_outputControls.Add(new DHControl("Remote Engine 0 Lever Demand Position", FloatIntBoolNone.Float, prcLeverTrackbar1, true));
            m_outputControls.Add(new DHControl("Remote Engine 0 RPM Demand", FloatIntBoolNone.Float, prcRpmTrackbar1, true));
            m_outputControls.Add(new DHControl("Remote Engine 0 Pitch Demand", FloatIntBoolNone.Float, prcPitchTrackbar1, true));
            m_outputControls.Add(new DHControl("Remote Engine 2 Lever Demand Position", FloatIntBoolNone.Float, prcLeverTrackbar2, true));
            m_outputControls.Add(new DHControl("Remote Engine 2 RPM Demand", FloatIntBoolNone.Float, prcRpmTrackbar2, true));
            m_outputControls.Add(new DHControl("Remote Engine 2 Pitch Demand", FloatIntBoolNone.Float, prcPitchTrackbar2, true));

            //RPM Components
            m_inputControls.Add(new DHControl("Engine 0 Shaft RPM", FloatIntBoolNone.Float, rpmShaftSpeed1, false));
            m_inputControls.Add(new DHControl("Engine 0 Engine RPM", FloatIntBoolNone.Float, rpmEngSpeed1, false));
            m_inputControls.Add(new DHControl("Engine 0 Propeller Pitch", FloatIntBoolNone.Float, rpmPropPitch1, false));
            m_inputControls.Add(new DHControl("Engine 2 Shaft RPM", FloatIntBoolNone.Float, rpmShaftSpeed2, false));
            m_inputControls.Add(new DHControl("Engine 2 Engine RPM", FloatIntBoolNone.Float, rpmEngSpeed2, false));
            m_inputControls.Add(new DHControl("Engine 2 Propeller Pitch", FloatIntBoolNone.Float, rpmPropPitch2, false));

            m_outputControls.Add(new DHControl("Engine 0 Shaft RPM", FloatIntBoolNone.Float, rpmShaftSpeedTrackbar1, true));
            m_outputControls.Add(new DHControl("Engine 0 Engine RPM", FloatIntBoolNone.Float, rpmEngSpeedTrackbar1, true));
            m_outputControls.Add(new DHControl("Engine 0 Propeller Pitch", FloatIntBoolNone.Float, rpmPropPitchTrackbar1, true));
            m_outputControls.Add(new DHControl("Engine 2 Shaft RPM", FloatIntBoolNone.Float, rpmShaftSpeedTrackbar2, true));
            m_outputControls.Add(new DHControl("Engine 2 Engine RPM", FloatIntBoolNone.Float, rpmEngSpeedTrackbar2, true));
            m_outputControls.Add(new DHControl("Engine 2 Propeller Pitch", FloatIntBoolNone.Float, rpmPropPitchTrackbar2, true));

            //TRD Components
            m_inputControls.Add(new DHControl("Thruster 0 RPM", FloatIntBoolNone.Float, trdRpmResponse1, false));
            m_inputControls.Add(new DHControl("Thruster 0 Pitch", FloatIntBoolNone.Float, trdPitchResponse1, false));
            m_inputControls.Add(new DHControl("Thruster 0 Azimuth", FloatIntBoolNone.Float, trdAzimuthResponse1, false));
            m_inputControls.Add(new DHControl("Thruster 2 RPM", FloatIntBoolNone.Float, trdRpmResponse2, false));
            m_inputControls.Add(new DHControl("Thruster 2 Pitch", FloatIntBoolNone.Float, trdPitchResponse2, false));
            m_inputControls.Add(new DHControl("Thruster 2 Azimuth", FloatIntBoolNone.Float, trdAzimuthResponse2, false));

            m_outputControls.Add(new DHControl("Thruster 0 RPM", FloatIntBoolNone.Float, trdRpmTrackbar1, true));
            m_outputControls.Add(new DHControl("Thruster 0 Pitch", FloatIntBoolNone.Float, trdPitchTrackbar1, true));
            m_outputControls.Add(new DHControl("Thruster 0 Azimuth", FloatIntBoolNone.Float, trdAzimuthTrackbar1, true));
            m_outputControls.Add(new DHControl("Thruster 2 RPM", FloatIntBoolNone.Float, trdRpmTrackbar2, true));
            m_outputControls.Add(new DHControl("Thruster 2 Pitch", FloatIntBoolNone.Float, trdPitchTrackbar2, true));
            m_outputControls.Add(new DHControl("Thruster 2 Azimuth", FloatIntBoolNone.Float, trdAzimuthTrackbar2, true));

            //BTR Components
            m_inputControls.Add(new DHControl("Bow Thruster Set Value", FloatIntBoolNone.Float, btrSetValue, false));
            m_inputControls.Add(new DHControl("Bow Thruster Real Value", FloatIntBoolNone.Float, btrRealValue, false));
            
            //EOM Components
            m_inputControls.Add(new DHControl("Mission Status", FloatIntBoolNone.Float, eomMissionStatus, false));
            m_inputControls.Add(new DHControl("Mission Elapsed Time", FloatIntBoolNone.Float, eomElapsedTime, false));

            //ROR
            m_outputControls.Add(new DHControl("Rudder Angle", FloatIntBoolNone.Float, rorLever, true));

            timer1.Enabled = true;
            tbUdpPort.Text = portReceive.ToString();

        }
        class nmeaObject
        {
            public string sentence { get; set; }
            public DateTime time { get; set; }
        }
        Dictionary<string, int> nmeaTypes = new Dictionary<string, int>();

        BindingList<nmeaObject> tableReceivedNmeaStrings = new BindingList<nmeaObject>();
        private void updateTable(string sentence, string index)
        {
            //Insert new sentence at index
            try
            {
                tableReceivedNmeaStrings[nmeaTypes[index]] = new nmeaObject() { sentence = sentence, time = System.DateTime.Now };
            }
            catch
            {
                tableReceivedNmeaStrings.Insert(nmeaTypes[index], new nmeaObject() { sentence = sentence, time = System.DateTime.Now });
            }
            dataGridReceivedNMEA.DataSource = tableReceivedNmeaStrings;
        }

        //Parser Stuff        
        private void parser(String sentence)
        {
            errorMessage.Text = "";
            try { lastStringReceived.Text = sentence; }
            catch {}
            string[] data = sentence.Split(',', '*');

            //Check if string has been seen before. If not, add it to Dictionary for array indexing.
            if (nmeaTypes.ContainsKey(data[0]) == false)
            {
                nmeaTypes.Add(data[0], nmeaTypes.Count);
            }
            updateTable(sentence, data[0]);
            
            switch (data[0])
            {
                case "$SSTRC":
                case "$--TRC":
                    parseTRC(data);
                    break;
                case "$SSTRD":
                case "$GPTRD":
                case "$--TRD":
                    parseTRD(data);
                    break;
                case "$SSETL":
                case "$--ETL":
                    parseETL(data);
                    break;
                case "$SSPRC":
                case "$--PRC":
                    parsePRC(data);
                    break;
                case "$SSRPM":
                case "$--RPM":
                    parseRPM(data);
                    break;
                case "$GPRSA":
                case "$--RSA":
                    parseRSA(data);
                    break;
                case "$DPBOW":
                    parseBOW(data);
                    break;
                case "$PBBTR":
                    parseBTR(data);
                    break;
                case "$NTEOM":
                    parseEOM(data);
                    break;
                case "$DPMPS":
                    parseMPS(data);
                    break;
                case "$DPMSB":
                    parseMSB(data);
                    break;
                case "$DPSTN":
                    parseSTN(data);
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
            sentenceTypeReceivedDisplay.Text = "Thruster Control Data";

            string thrusterNum = data[1];
            string rpmDemand = data[2];
            string rpmMode = data[3];
            string pitchDemand = data[4];
            string pitchMode = data[5];
            string azimuthDemand = data[6];
            string operatingLocation = data[7];
            string sentenceStatus = data[8];
            string checkSum = data[9];

            string thrusterName = convBowStern(thrusterNum);

            DataHolderIface.SetFloatVal("Thruster " + thrusterNum + " RPM Demand", calcPercentDemand(rpmDemand, rpmMode));          //Always write to dataholder the percent
            DataHolderIface.SetFloatVal("Thruster " + thrusterNum + " Pitch Demand", calcPercentDemand(pitchDemand, pitchMode));          //Always write to dataholder the percent
            DataHolderIface.SetFloatVal("Thruster " + thrusterNum + " Azimuth Demand", float.Parse(azimuthDemand)/10);

        }
        private void parseETL(string[] data)
        {
            if (data.Length < 8)
            {
                errorMessage.Text = "NMEA Sentence not long enough";
                return;
            }
            sentenceTypeReceivedDisplay.Text = "Engine telegraph operation status";

            string eventTime = data[1];
            string messageType = data[2];
            string positionEngineTelegraph = data[3];
            string positionSubTelegraph = data[4];
            string operatingLocation = data[5];
            string engineNum = data[6];
            string checkSum = data[7];

            DataHolderIface.SetFloatVal("Engine " + engineNum + " Event Time", float.Parse(eventTime));
            DataHolderIface.SetStringVal("Engine " + engineNum + " Message Type", convMessageType(messageType));
            DataHolderIface.SetFloatVal("Engine " + engineNum + " Telegraph Position", float.Parse(positionEngineTelegraph));
            DataHolderIface.SetFloatVal("Engine " + engineNum + " Sub-Telegraph Position", float.Parse(positionSubTelegraph));
            DataHolderIface.SetStringVal("Engine " + engineNum + " Operating Location", convLocation(operatingLocation));
        }
        private void parsePRC(string[] data)
        {
            if (data.Length < 10)
            {
                errorMessage.Text = "NMEA Sentence not long enough";
                return;
            }
            sentenceTypeReceivedDisplay.Text = "Propulsion Remote Control Status";

            string leverDemandPosition = data[1];
            string leverDemandValid = data[2];
            string rpmDemand = data[3];
            string rpmMode = data[4];
            string pitchDemand = data[5];
            string pitchMode = data[6];
            string operatingLocation = data[7];
            string engineNum = data[8];
            string checkSum = data[9];

            if (leverDemandValid == "A") //TODO: should this negate entire nmea sentence or just lever demand?
            {
                DataHolderIface.SetFloatVal("Remote Engine " + engineNum + " Lever Demand Position", float.Parse(leverDemandPosition));
                DataHolderIface.SetFloatVal("Remote Engine " + engineNum + " RPM Demand", calcPercentDemand(rpmDemand, rpmMode));
                DataHolderIface.SetFloatVal("Remote Engine " + engineNum + " Pitch Demand", calcPercentDemand(pitchDemand, pitchMode));
                DataHolderIface.SetStringVal("Remote Engine " + engineNum + " Operating Location", convLocation(operatingLocation));
            }
        }
        private void parseRPM(string[] data)
        {
            if (data.Length < 7)
            {
                errorMessage.Text = "NMEA Sentence not long enough";
                return;
            }
            sentenceTypeReceivedDisplay.Text = "Revolutions";

            string source = data[1];    //Source, shaft/engine S/E
            string engineNum = data[2]; //Engine or shaft number, numbered from centreline; 0=single or on centreline; odd=starboard; even=port
            string speed = data[3];     //Speed, rev/min, “-“ = counter-clockwise
            string propPitch = data[4]; //Propeller pitch, % of max, “-“ = astern
            string status = data[5];    //Status; A=Data valid, V=Data invalid

            if (status == "A")
            {
                //TODO: Enumerate "source" after we know what it will look like
                DataHolderIface.SetFloatVal("Engine " + engineNum + " " + convSource(source) + " RPM", float.Parse(speed));
                DataHolderIface.SetFloatVal("Engine " + engineNum + " Propeller Pitch", float.Parse(propPitch));
            }
        }
        private void parseTRD(string[] data)
        {
            if (data.Length < 8)
            {
                errorMessage.Text = "NMEA Sentence not long enough";
                return;
            }
            sentenceTypeReceivedDisplay.Text = "Thruster response data";

            string thrusterNum = data[1];
            string rpmResponse = data[2];
            string rpmMode = data[3];
            string pitchResponse = data[4];
            string pitchMode = data[5];
            string azimuthResponse = data[6];

            DataHolderIface.SetFloatVal("Thruster " + thrusterNum + " RPM", calcPercentDemand(rpmResponse, rpmMode));
            DataHolderIface.SetFloatVal("Thruster " + thrusterNum + " Pitch", calcPercentDemand(pitchResponse, pitchMode));
            DataHolderIface.SetFloatVal("Thruster " + thrusterNum + " Azimuth", float.Parse(azimuthResponse)/10);
        }
        private void parseBTR(string[] data)
        {
            if (data.Length < 5)
            {
                errorMessage.Text = "NMEA Sentence not long enough";
                return;
            }
            sentenceTypeReceivedDisplay.Text = "Bow thruster set value";

            string setValue = data[1];
            string setStatus = data[2];
            string realValue = data[3];
            string realStatus = data[4];

            if (setStatus == "A") DataHolderIface.SetFloatVal("Bow Thruster Set Value", float.Parse(setValue));
            if (realStatus == "A") DataHolderIface.SetFloatVal("Bow Thruster Real Value", float.Parse(realValue));
        }
        private void parseEOM(string[] data)
        {
            if (data.Length < 3)
            {
                errorMessage.Text = "NMEA Sentence not long enough";
                return;
            }
            sentenceTypeReceivedDisplay.Text = "End of Mission";

            string missionStatus = data[1];
            string elapsedTime = data[2];

            DataHolderIface.SetFloatVal("Mission Status", float.Parse(missionStatus));
            DataHolderIface.SetFloatVal("Mission Elapsed Time", float.Parse(elapsedTime));
        }
        private void parseBOW(string[] data)//from V-Step Manual: "This sentence will be deprecated in the future and is replaced by TRC."
        {
            if (data.Length < 4)
            {
                errorMessage.Text = "NMEA Sentence not long enough";
                return;
            }
            sentenceTypeReceivedDisplay.Text = "Bow thruster thrust";

            string angle = data[1];
            string thrust = data[2];
            string depth = data[3];

            DataHolderIface.SetFloatVal("Bow Thruster Angle", calcPercentDemand(angle, "D"));
            DataHolderIface.SetFloatVal("Bow Thruster Thrust", float.Parse(thrust));
            DataHolderIface.SetFloatVal("Bow Thruster Depth", float.Parse(depth));
        }
        private void parseMPS(string[] data)//from V-Step Manual: "This sentence will be deprecated in the future and is replaced by TRC."
        {
            if (data.Length < 4)
            {
                errorMessage.Text = "NMEA Sentence not long enough";
                return;
            }
            sentenceTypeReceivedDisplay.Text = "Portside thrust and angle";

            string angle = data[1];
            string thrust = data[2];
            string depth = data[3];

            DataHolderIface.SetFloatVal("Port Thruster Angle", calcPercentDemand(angle, "D"));
            DataHolderIface.SetFloatVal("Port Thruster Thrust", float.Parse(thrust));
            DataHolderIface.SetFloatVal("Port Thruster Depth", float.Parse(depth));
        }
        private void parseMSB(string[] data)//from V-Step Manual: "This sentence will be deprecated in the future and is replaced by TRC."
        {
            if (data.Length < 4)
            {
                errorMessage.Text = "NMEA Sentence not long enough";
                return;
            }
            sentenceTypeReceivedDisplay.Text = "Starboard thrust and angle";

            string angle = data[1];
            string thrust = data[2];
            string depth = data[3];

            DataHolderIface.SetFloatVal("Starboard Thruster Angle", calcPercentDemand(angle, "D"));
            DataHolderIface.SetFloatVal("Starboard Thruster Thrust", float.Parse(thrust));
            DataHolderIface.SetFloatVal("Starboard Thruster Depth", float.Parse(depth));
        }
        private void parseSTN(string[] data)//from V-Step Manual: "This sentence will be deprecated in the future and is replaced by TRC."
        {
            if (data.Length < 4)
            {
                errorMessage.Text = "NMEA Sentence not long enough";
                return;
            }
            sentenceTypeReceivedDisplay.Text = "Stern thrust and angle";

            string angle = data[1];
            string thrust = data[2];
            string depth = data[3];

            DataHolderIface.SetFloatVal("Stern Thruster Angle", calcPercentDemand(angle, "D"));
            DataHolderIface.SetFloatVal("Stern Thruster Thrust", float.Parse(thrust));
            DataHolderIface.SetFloatVal("Stern Thruster Depth", float.Parse(depth));
        }
        private void parseRSA(string[] data) { } // This Sentence is not in NMEA Interface Manual

        private const float MAX_DEGREES = 12.3f;  // Should this really be hard-coded? -nse
        private const float MAX_RPM = 2000f; //Just a wag at max rpm. Should be corrected later.
        private float calcPercentDemand(string rpmDemand, string rpmMode)
        {
            float value = float.Parse(rpmDemand);
            switch (rpmMode)
            {
                case "P":
                    return value;       //It is already a percent
                case "D":
                    return value * 100 / MAX_DEGREES;           //Turn into a percent
                case "R":
                    return value * 100 / MAX_RPM;           //Turn into a percent
                case "V":
                    return 0.0f;        //???
                default:
                    return 0.0f;        //???
            }
        }
        private string convBowStern(string s)
        {
            if (int.Parse(s) % 2 == 0) //return Stern if even
                return "Stern";
            else return "Bow"; //return Bow if not even
        }
        private string convSource(String s)
        {
            if (s == "S") return "Shaft";
            else if (s == "E") return "Engine";
            else return ("Unrecognized Source");
        }
        private string convMode(String s)
        {
            switch (s)
            {
                case "P":
                    return "%";
                case "D":
                    return "°";
                case "V":
                    return "(invalid)";
                default:
                    return "UNRECOGNIZED MODE";
            }
        }
        private string convLocation(String s)
        {
            switch (s)
            {
                case "B":
                    return "Bridge";
                case "P":
                    return "Port Wing";
                case "S":
                    return "Starboard Wing";
                case "C":
                    return "Engine Control Room";
                case "E":
                    return "Engine Side / Local";
                case "W":
                    return "Wing";
                default:
                    return "UNRECOGNIZED LOCATION";
            }
        }
        private string convMessageType(String s)
        {
            switch (s)
            {
                case "O":
                    return "Order";
                case "A":
                    return "Answer-back";
                default:
                    return "UNRECOGNIZED MESSAGE TYPE";
            }
        }
        private string convStatus(String s)
        {
            switch (s)
            {
                case "R":
                    return "Status Report";
                case "C":
                    return "Config Command";
                default:
                    return "UNRECOGNIZED STATUS";
            }
        }
        private string calcChecksum(string s)
        {
            int checksum = 0;
            for (int i = 0; i < s.Length; i++)
            {
                checksum ^= Convert.ToByte(s[i]);
            }
            return checksum.ToString("X2");
        }
        
        static int portReceive = 1254;
        static int portSend = 8011;
        private volatile bool runThread;
        UdpClient listener = new UdpClient(portReceive);
        //UdpClient udpSender = new UdpClient(portSend);
        private void receiveNmeaMessage()
        {
            while (runThread)
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, portReceive);
                byte[] content = listener.Receive(ref endPoint);
                if (content.Length > 0)
                {
                    string nmeaMessage = Encoding.ASCII.GetString(content);
                    parser(nmeaMessage);
                }
            }
        }
        IPEndPoint sendEP = new IPEndPoint(IPAddress.Parse("192.168.0.6"), portSend);
        Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        // Creates one SocketPermission object for access restrictions
        SocketPermission permission;
        private void sendNmeaMessage(string s)
        {
            // Creates one SocketPermission object for access restrictions
                permission = new SocketPermission(
                NetworkAccess.Accept,     // Allowed to accept connections 
                TransportType.Udp,        // Defines transport types 
                "",                       // The IP addresses of local host 
                SocketPermission.AllPorts // Specifies all ports 
                );

            try {
                listener.Connect(sendEP);
                Byte[] sendBytes = Encoding.ASCII.GetBytes(s);
                listener.Send(sendBytes, sendBytes.Length);
                //sock.SendTo(sendBytes, sendEP);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void prcSendButton1_Click(object sender, EventArgs e)
        {
            string command = "XXPRC," + prcLeverPos1.Text + ",A," + prcRpmDemand1.Text + ",P," + prcPitchDemand1.Text + ",P,S,0";
            string sOut = "$" + command + "*" + calcChecksum(command) + "\r\n";
            lastStringSent.Text = sOut;
            sendNmeaMessage(sOut);
        }
        private void etlSendButton1_Click(object sender, EventArgs e)
        {
            string command = "XXETL,0,O," + etlTelegraphPos1.Text + "," + etlSubTelPos1.Text + ",S,0";
            string sOut = "$" + command + "*" + calcChecksum(command) + "\r\n";
            lastStringSent.Text = sOut;
            sendNmeaMessage(sOut);
        }
        private void rorSendButton_Click(object sender, EventArgs e)
        {
            string command = "XXROR," + rorLever.Value + ",A," + rorLever.Value + ",A,C";
            string sOut = "$" + command + "*" + calcChecksum(command) + "\r\n";
            lastStringSent.Text = sOut;
            sendNmeaMessage(sOut);
        }

        //Unused Button Clicks
        private void rpmEngSendButton1_Click(object sender, EventArgs e)
        {
            string s = "$--RPM,E," + ",1," + rpmEngSpeed1.Text + "," + rpmPropPitch1.Text + ",A*hh\r\n";
            parser(s);
            lastStringSent.Text = s;
        }
        private void rpmShaftSendButton1_Click(object sender, EventArgs e)
        {
            string s = "$--RPM,E," + ",1," + rpmShaftSpeed1.Text + "," + rpmPropPitch1.Text + ",A*hh\r\n";
            parser(s);
            lastStringSent.Text = s;
        }  
        private void sendTrc1Button_Click(object sender, EventArgs e)
        {
            string s_1 = "--TRC,1," + trcRpmDemandDisplay1.Text + ",P," + trcPitchDemandDisplay1.Text + ",P," + float.Parse(trcAzimuthDemandDisplay1.Text) * 10 + ",S,C";
            string s = "$" + s_1 + "*" + calcChecksum(s_1) + "\r\n";
            lastStringSent.Text = s;
            sendNmeaMessage(s);
        }
        private void trdSendButton1_Click(object sender, EventArgs e)
        {

            string s = "$--TRD,1," + trdRpmResponse1.Text + ",P," + trdPitchResponse1.Text + ",P," + float.Parse(trdAzimuthResponse1.Text) * 10 + "*hh\r\n";
            parser(s);
            lastStringSent.Text = s;
        }
        private void sendTrc2Button_Click(object sender, EventArgs e)
        {
            string s = "$--TRC,2," + trcRpmDemandDisplay2.Text + ",P," + trcPitchDemandDisplay2.Text + ",P," + float.Parse(trcAzimuthDemandDisplay2.Text) * 10 + ",S,C*hh\r\n";
            parser(s);
            lastStringSent.Text = s;
        }
        private void trdSendButton2_Click(object sender, EventArgs e)
        {
            string s = "$--TRD,2," + trdRpmResponse2.Text + ",P," + trdPitchResponse2.Text + ",P," + float.Parse(trdAzimuthResponse2.Text) * 10 + "*hh\r\n";
            parser(s);
            lastStringSent.Text = s;
        }
        private void prcSendButton2_Click_1(object sender, EventArgs e)
        {
            string s = "$--PRC," + prcLeverPos2.Text + ",A," + prcRpmDemand2.Text + ",P," + prcPitchDemand2.Text + ",P,S,2*hh\r\n";
            parser(s);
            lastStringSent.Text = s;
        }
        private void rpmEngSendButton2_Click(object sender, EventArgs e)
        {
            string s = "$--RPM,E," + ",1," + rpmEngSpeed2.Text + "," + rpmPropPitch2.Text + ",A*hh\r\n";
            parser(s);
            lastStringSent.Text = s;
        }
        private void rpmShaftSendButton2_Click(object sender, EventArgs e)
        {
            string s = "$--RPM,E," + ",1," + rpmShaftSpeed2.Text + "," + rpmPropPitch2.Text + ",A*hh\r\n";
            parser(s);
            lastStringSent.Text = s;
        }
        private void etlSendButton2_Click(object sender, EventArgs e)
        {
            string s = "$--ETL,0,O," + etlTelegraphPos2.Text + "," + etlSubTelPos2.Text + ",S,2*hh\r\n";
            parser(s);
            lastStringSent.Text = s;
        }
       
        private void udpReceiveButton_Click(object sender, EventArgs e)
        {
            Thread udpReadThread = new Thread(receiveNmeaMessage);
            if (udpReceiveButton.Text == "Start")
            {
                CheckForIllegalCrossThreadCalls = false;
                runThread = true;
                udpReadThread.IsBackground = true;
                udpReadThread.Start();
                udpReceiveButton.Text = "Stop";
                updReceiveLabel.Visible = true;
            }
            else
            {
                runThread = false;
                udpReceiveButton.Text = "Start";
                updReceiveLabel.Visible = false;

            }
        }        
        private void parse_button_Click(object sender, EventArgs e)
        {
            parser(NMEA_String_Box.Text);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (DHControl dh in m_inputControls)
            {
                dh.readFromDataHolder();
            }
        }
        List<DHControl> m_outputControls = new List<DHControl>();           //This is for later, when trying to stimulate our output to VStep
        List<DHControl> m_inputControls = new List<DHControl>();//This is for taking DataHolder values (written to by parsing NMEA), and displaying them on the screen
        
    }
}
