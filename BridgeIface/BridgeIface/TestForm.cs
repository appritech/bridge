using System;
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
        List<DHControl> m_outputControls = new List<DHControl>();           //This is for later, when trying to stimulate our output to VStep
        List<DHControl> m_inputControls = new List<DHControl>();//This is for taking DataHolder values (written to by parsing NMEA), and displaying them on the screen
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
            m_outputControls.Add(new DHControl("Rudder Angle2", FloatIntBoolNone.Float, rorLever, true));
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
            tbUdpRecPort.Text = portReceive.ToString();
            tbUdpSendPort.Text = portSend.ToString();
            tbUdpSendIP.Text = ipAddress;

        }

        private const float MAX_DEGREES = 12.3f;  // Should this really be hard-coded? -nse
        private const float MAX_RPM = 2000f; //Just a wag at max rpm. Should be corrected later.
        static int portReceive = 1254;
        static int portSend = 8011;
        static string ipAddress = "192.168.0.6";

        class nmeaObject
        {
            public string sentence { get; set; }
            public DateTime time { get; set; }
        }
        private enum nmeaType
        {
            ROR,
            PRC,
            ETL,
            RSA,
            TRC,
            TRD,
            RPM
        }
        Dictionary<string, int> sentNmeaTypes = new Dictionary<string, int>();
        Dictionary<string, int> receivedNmeaTypes = new Dictionary<string, int>();

        BindingList<nmeaObject> tableSentNmeaStrings = new BindingList<nmeaObject>();
        BindingList<nmeaObject> tableReceivedNmeaStrings = new BindingList<nmeaObject>();

        private void updateReceivedTable(string sentence, string index)
        {
            //Insert new sentence at index
            try
            {
                tableReceivedNmeaStrings[sentNmeaTypes[index]] = new nmeaObject() { sentence = sentence, time = System.DateTime.Now };
            }
            catch
            {
                try
                {
                    tableReceivedNmeaStrings.Insert(sentNmeaTypes[index], new nmeaObject() { sentence = sentence, time = System.DateTime.Now });
                }
                catch { }
            }
            dataGridReceivedNMEA.DataSource = tableReceivedNmeaStrings;
        }

        //Parsers        
        private void parser(String sentence)
        {
            try
            {
                errorMessage.Text = "";
                lastStringReceived.Text = sentence;
            }
            catch {}
            string[] data = sentence.Split(',', '*');

            string index = data[0];//NMEA Sentecne Header
            
            switch (data[0])
            {
                case "$SSTRC":
                case "$--TRC":
                    parseTRC(data);
                    index += data[1];//thruster number
                    break;
                case "$SSTRD":
                case "$GPTRD":
                case "$DPTRD":
                case "$--TRD":
                    parseTRD(data);
                    index += data[1];//thruster number
                    break;
                case "$SSETL":
                case "$--ETL":
                    parseETL(data);
                    index += data[6];//engine number
                    break;
                case "$SSPRC":
                case "$--PRC":
                    parsePRC(data);
                    index += data[8];//engine number
                    break;
                case "$SSRPM":
                case "$--RPM":
                    parseRPM(data);
                    index += data[2];//engine number
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
            //Check if string has been seen before. If not, add it to Dictionary for array indexing.
            if (sentNmeaTypes.ContainsKey(index) == false)
            {
                sentNmeaTypes.Add(index, sentNmeaTypes.Count);
            }
            //updateReceivedTable(sentence, index);
            Thread worker = new Thread(() => updateReceivedTable(sentence, index));
            worker.Start();
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

            if (messageType == "O") //Order
            {
                //set Sub-Telegraph setting to pending
            }
            if (messageType == "A")//Answer-back
            {
                DataHolderIface.SetFloatVal("Engine " + engineNum + " Telegraph Position", float.Parse(positionEngineTelegraph));
                DataHolderIface.SetFloatVal("Engine " + engineNum + " Sub-Telegraph Position", float.Parse(positionSubTelegraph));
                flash20 = false;
                flash30 = false;
                flash40 = false;
            }

            //DataHolderIface.SetFloatVal("Engine " + engineNum + " Event Time", float.Parse(eventTime));
            //DataHolderIface.SetStringVal("Engine " + engineNum + " Message Type", convMessageType(messageType));
            //DataHolderIface.SetFloatVal("Engine " + engineNum + " Telegraph Position", float.Parse(positionEngineTelegraph));
            //DataHolderIface.SetFloatVal("Engine " + engineNum + " Sub-Telegraph Position", float.Parse(positionSubTelegraph));
            //DataHolderIface.SetStringVal("Engine " + engineNum + " Operating Location", convLocation(operatingLocation));
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
            }
            DataHolderIface.SetFloatVal("Remote Engine " + engineNum + " RPM Demand", calcPercentDemand(rpmDemand, rpmMode));
            DataHolderIface.SetFloatVal("Remote Engine " + engineNum + " Pitch Demand", calcPercentDemand(pitchDemand, pitchMode));
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
        private void parseRSA(string[] data)
        {
            //Example: $GPRSA,20.3,A,0.0,V*71
            if (data.Length < 6)
            {
                errorMessage.Text = "NMEA Sentence not long enough";
                return;
            }
            sentenceTypeReceivedDisplay.Text = "Rudder Sensor Angle";

            string angle0 = data[1];
            string angle0valid = data[2];
            string angle1 = data[3];
            string angle1valid = data[4];
            string checkSum = data[5];

            if (angle0valid == "A")
            {
                DataHolderIface.SetFloatVal("Rudder Sensor Angle", float.Parse(angle0));
            }
        } 
        
        //Calculators & Converters
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
        private string convSource(String s)
        {
            if (s == "S") return "Shaft";
            else if (s == "E") return "Engine";
            else return ("Unrecognized Source");
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
        
        private volatile bool runThread;
        UdpClient listener = new UdpClient(portReceive);
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
        IPEndPoint sendEP = new IPEndPoint(IPAddress.Parse(ipAddress), portSend);
        Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        

        private void prcSendButton1_Click(object sender, EventArgs e)
        {
            sendNmea(nmeaType.PRC);
        }
        private void etlSendButton1_Click(object sender, EventArgs e)
        {
            sendNmea(nmeaType.ETL);
        }
        private void sendNmea(nmeaType type)
        {
            string command;
            string sOut = "";
            string index = "";
            switch (type)
            {
                case nmeaType.ROR:
                    command = "XXROR," + rorLever.Value + ",A,,A,C";
                    sOut = "$" + command + "*" + calcChecksum(command) + "\r\n";
                    index = "$XXROR";
                    break;
                case nmeaType.PRC:
                    //command = "XXPRC," + prcLeverPos1.Text + ",A," + prcRpmDemand1.Text + ",P," + prcPitchDemand1.Text + ",P,S,0";
                    //command = "XXPRC," + prcLeverPos1.Text + ",A,,V,,V,S,0";
                    command = "XXPRC,,V," + prcRpmTrackbar1.Value + ",P," + prcPitchTrackbar1.Value + ",P,C,0";
                    sOut = "$" + command + "*" + calcChecksum(command) + "\r\n";
                    index = "$XXPRC";
                    break;
                case nmeaType.ETL:
                    command = "XXETL,0,O,0," + etlToSend + ",S,0";
                    //command = "XXETL,0,O," + etlTelegraphTrackbar1.Value + "," + etlSubTelTrackbar1.Text + "0,S,0";
                    //command = "XXETL,0,O,," + etlSubTelPos1.Text + "0,S,0";
                    sOut = "$" + command + "*" + calcChecksum(command) + "\r\n";
                    index = "$XXETL";
                    break;
                case nmeaType.RSA:
                    //Example: $GPRSA,20.3,A,0.0,V*71
                    command = "XXRSA," + rsaLever.Value + ",A,,V";
                    sOut = "$" + command + "*" + calcChecksum(command) + "\r\n";
                    index = "$XXRSA";
                    break;
                case nmeaType.RPM:
                    //Example: $SSRPM,S,0,39.5,0.00,A*4E
                    command = "XXRPM,S," + rpmEngSpeedTrackbar1.Value + "," + rpmShaftSpeedTrackbar1.Value + "," + rpmPropPitchTrackbar1.Value + ",A";
                    sOut = "$" + command + "*" + calcChecksum(command) + "\r\n";
                    index = "$XXRPM";
                    break;

            }
            lastStringSent.Text = sOut;

            //update Sent Table here
            if (receivedNmeaTypes.ContainsKey(index) == false)
            {
                receivedNmeaTypes.Add(index, receivedNmeaTypes.Count);
            }
            updateSentTable(sOut, index);

            sendNmeaMessage(sOut);
        }


        private void updateSentTable(string sentence, string index)
        {
            //Insert new sentence at index
            try
            {
                tableSentNmeaStrings[receivedNmeaTypes[index]] = new nmeaObject() { sentence = sentence, time = System.DateTime.Now };
            }
            catch
            {
                tableSentNmeaStrings.Insert(receivedNmeaTypes[index], new nmeaObject() { sentence = sentence, time = System.DateTime.Now });
            }
            dataGridSentNMEA.DataSource = tableSentNmeaStrings;
        }
        private void sendNmeaMessage(string s)
        {
            try
            {
                Byte[] sendBytes = Encoding.ASCII.GetBytes(s);
                sock.SendTo(sendBytes, sendEP);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
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
        int count = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (DHControl dh in m_inputControls)
            {
                dh.readFromDataHolder();
            }
            count += count < 19 ? 1:-19;//increment count, reset at 10
            if (count == 0)
            {
                if (flash20)
                {
                    etlRecSubTelCB20.Checked = true;
                    etlSendSubTelCB20.Checked = true;
                }
                if (flash30)
                {
                    etlRecSubTelCB30.Checked = true;
                    etlSendSubTelCB30.Checked = true;
                }

                if (flash40)
                {
                    etlRecSubTelCB40.Checked = true;
                    etlSendSubTelCB40.Checked = true;
                }
            }
            else if (count == 10)
            {
                if (flash20)
                {
                    etlRecSubTelCB20.Checked = false;
                    etlSendSubTelCB20.Checked = false;
                }
                if (flash30)
                {
                    etlRecSubTelCB30.Checked = false;
                    etlSendSubTelCB30.Checked = false;
                }

                if (flash40)
                {
                    etlRecSubTelCB40.Checked = false;
                    etlSendSubTelCB40.Checked = false;
                }
            }
        }
        bool outputEn;
        private void outputEnableButton_Click(object sender, EventArgs e)
        {
            if (outputEnableButton.Text == "Enable")
            {
                outputEn = true;
                outputEnableLabel.Text = "Output is Enabled";
                rorLever.Enabled = true;
                prcPitchTrackbar1.Enabled = true;
                prcRpmTrackbar1.Enabled = true;
                etlTelegraphTrackbar1.Enabled = true;
                etlSubTelTrackbar1.Enabled = true;
                rsaLever.Enabled = true;
                outputEnableButton.Text = "Disable";
            }
            else
            {
                outputEn = false;
                outputEnableLabel.Text = "Output is Disabled";
                rorLever.Enabled = false;
                prcPitchTrackbar1.Enabled = false;
                prcRpmTrackbar1.Enabled = false;
                etlTelegraphTrackbar1.Enabled = false;
                etlSubTelTrackbar1.Enabled = false;
                rsaLever.Enabled = false;
                outputEnableButton.Text = "Enable";

            }
        }
        private void trackbar_ValueChanged(object sender, EventArgs e)
        {
            TrackBar tb = (TrackBar)sender;
            switch (tb.Name)
            {
                case ("rpmShaftSpeedTrackbar1"):
                case ("rpmPropPitchTrackbar1"):
                case ("rpmEngSpeedTrackbar1"):
                    sendNmea(nmeaType.RPM);
                    break;
                case ("prcRpmTrackbar1"):
                case ("prcPitchTrackbar1"):
                    sendNmea(nmeaType.PRC);
                    break;
                case ("rorLever"):
                    sendNmea(nmeaType.ROR);
                    break;
                case ("rsaLever"):
                    sendNmea(nmeaType.RSA);
                    break;
                default:
                    break; //place breakpoint here to catch unhandled trackbar changes.
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            button4.Enabled = false;
            new ECR_Sim().Show();
        }
        bool button1pressed = false;
        private void button1_Click(object sender, EventArgs e)
        {
            button1pressed = !button1pressed;
        }

        bool flash20 = false;
        bool flash30 = false;
        bool flash40 = false;

        private void etlSendCheckBox_Click(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if (cb.Checked)
            {
                switch (cb.Text)
                {
                    case ("20"):
                        etlToSend = 20;
                        flash20 = true;
                        sendNmea(nmeaType.ETL);
                        //etlSendSubTelCB30.Checked = false;
                        //etlSendSubTelCB40.Checked = false;
                        break;
                    case ("30"):
                        etlToSend = 30;
                        flash30 = true;
                        sendNmea(nmeaType.ETL);
                        //etlSendSubTelCB20.Checked = false;
                        //etlSendSubTelCB40.Checked = false;
                        break;
                    case ("40"):
                        etlToSend = 40;
                        flash40 = true;
                        sendNmea(nmeaType.ETL);
                        //etlSendSubTelCB20.Checked = false;
                        //etlSendSubTelCB30.Checked = false;
                        break;
                }
            }
        }
        int etlToSend;

        private void clearTable_button_Click(object sender, EventArgs e)
        {
            sentNmeaTypes.Clear();
            tableReceivedNmeaStrings.Clear();
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
       
    }
}
