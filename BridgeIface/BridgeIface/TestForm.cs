﻿using System;
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
            
            //TRC Components
            m_inputControls.Add(new DHControl("Bow Thruster 1 RPM Demand", FloatIntBoolNone.Float, trcRpmDemandDisplay1, false));
            m_inputControls.Add(new DHControl("Bow Thruster 1 Pitch Demand", FloatIntBoolNone.Float, trcPitchDemandDisplay1, false));
            m_inputControls.Add(new DHControl("Bow Thruster 1 Azimuth Demand", FloatIntBoolNone.Float, trcAzimuthDemandDisplay1, false));
            //m_inputControls.Add(new DHControl("Bow Thruster 1 Operating Location",FloatIntBoolNone.None, locationDisplay1, false)); //TODO: decide if this needs enum or toss it.
            //m_inputControls.Add(new DHControl("Bow Thruster 1 Sentence Status", FloatIntBoolNone.None, sentenceStatusDisplay1, false)); //TODO: decide if this needs enum or toss it.
            m_inputControls.Add(new DHControl("Stern Thruster 2 RPM Demand", FloatIntBoolNone.Float, trcRpmDemandDisplay2, false));
            m_inputControls.Add(new DHControl("Stern Thruster 2 Pitch Demand", FloatIntBoolNone.Float, trcPitchDemandDisplay2, false));
            m_inputControls.Add(new DHControl("Stern Thruster 2 Azimuth Demand", FloatIntBoolNone.Float, trcAzimuthDemandDisplay2, false));
            //m_inputControls.Add(new DHControl("Stern Thruster 2 Operating Location", FloatIntBoolNone.None, locationDisplay2, false)); //TODO: decide if this needs enum or toss it.
            //m_inputControls.Add(new DHControl("Stern Thruster 2 Sentence Status", FloatIntBoolNone.None, sentenceStatusDisplay2, false)); //TODO: decide if this needs enum or toss it.

            //ETL Components
            m_inputControls.Add(new DHControl("Engine 1 Event Time", FloatIntBoolNone.Float, etlEventTime1, false));
            m_inputControls.Add(new DHControl("Engine 1 Telegraph Position", FloatIntBoolNone.Float, etlTelegraphPos1, false));
            m_inputControls.Add(new DHControl("Engine 1 Sub-Telegraph Position", FloatIntBoolNone.Float, etlSubTelPos1, false));
            m_inputControls.Add(new DHControl("Engine 2 Event Time", FloatIntBoolNone.Float, etlEventTime2, false));
            m_inputControls.Add(new DHControl("Engine 2 Telegraph Position", FloatIntBoolNone.Float, etlTelegraphPos2, false));
            m_inputControls.Add(new DHControl("Engine 2 Sub-Telegraph Position", FloatIntBoolNone.Float, etlSubTelPos2, false));

            //PRC Components
            m_inputControls.Add(new DHControl("Remote Engine 1 Lever Demand Position", FloatIntBoolNone.Float, prcLeverPos1, false));
            m_inputControls.Add(new DHControl("Remote Engine 1 RPM Demand", FloatIntBoolNone.Float, prcRpmDemand1, false));
            m_inputControls.Add(new DHControl("Remote Engine 1 Pitch Demand", FloatIntBoolNone.Float, prcPitchDemand1, false));
            m_inputControls.Add(new DHControl("Remote Engine 2 Lever Demand Position", FloatIntBoolNone.Float, prcLeverPos2, false));
            m_inputControls.Add(new DHControl("Remote Engine 2 RPM Demand", FloatIntBoolNone.Float, prcRpmDemand2, false));
            m_inputControls.Add(new DHControl("Remote Engine 2 Pitch Demand", FloatIntBoolNone.Float, prcPitchDemand2, false));

            //RPM Components
            m_inputControls.Add(new DHControl("Engine 1 Shaft RPM", FloatIntBoolNone.Float, rpmShaftSpeed1, false));
            m_inputControls.Add(new DHControl("Engine 1 Engine RPM", FloatIntBoolNone.Float, rpmEngSpeed1, false));
            m_inputControls.Add(new DHControl("Engine 1 Propeller Pitch", FloatIntBoolNone.Float, rpmPropPitch1, false));
            m_inputControls.Add(new DHControl("Engine 2 Shaft RPM", FloatIntBoolNone.Float, rpmShaftSpeed2, false));
            m_inputControls.Add(new DHControl("Engine 2 Engine RPM", FloatIntBoolNone.Float, rpmEngSpeed2, false));
            m_inputControls.Add(new DHControl("Engine 2 Propeller Pitch", FloatIntBoolNone.Float, rpmPropPitch2, false));

            //TRD Components
            m_inputControls.Add(new DHControl("Thruster 1 RPM", FloatIntBoolNone.Float, trdRpmResponse1, false));
            m_inputControls.Add(new DHControl("Thruster 1 Pitch", FloatIntBoolNone.Float, trdPitchResponse1, false));
            m_inputControls.Add(new DHControl("Thruster 1 Azimuth", FloatIntBoolNone.Float, trdAzimuthResponse1, false));
            m_inputControls.Add(new DHControl("Thruster 2 RPM", FloatIntBoolNone.Float, trdRpmResponse2, false));
            m_inputControls.Add(new DHControl("Thruster 2 Pitch", FloatIntBoolNone.Float, trdPitchResponse2, false));
            m_inputControls.Add(new DHControl("Thruster 2 Azimuth", FloatIntBoolNone.Float, trdAzimuthResponse2, false));

            //BTR Components
            m_inputControls.Add(new DHControl("Bow Thruster Set Value", FloatIntBoolNone.Float, btrSetValue, false));
            m_inputControls.Add(new DHControl("Bow Thruster Real Value", FloatIntBoolNone.Float, btrRealValue, false));
            
            //EOM Components
            m_inputControls.Add(new DHControl("Mission Status", FloatIntBoolNone.Float, eomMissionStatus, false));
            m_inputControls.Add(new DHControl("Mission Elapsed Time", FloatIntBoolNone.Float, eomElapsedTime, false));

            timer1.Enabled = true;
        }
        
        private void parse_button_Click(object sender, EventArgs e)
        {
            parser(NMEA_String_Box.Text);
        }
        private void parser(String sentence)
        {
            errorMessage.Text = "";
            lastStringReceived.Text = sentence;
            string[] data = sentence.Split(',','*');

            switch (data[0])
            {
                case "$GCTRC":          //This one isn't real, just an example. We should get rid of it soon.
                case "$--TRC":
                    parseTRC(data);
                    break;
                case "$--ETL":
                    parseETL(data);
                    break;
                case "$--PRC":
                    parsePRC(data);
                    break;
                case "$--RPM":
                    parseRPM(data);
                    break;
                case "$--RSA":
                    parseRSA(data);
                    break;
                case "$--TRD":
                    parseTRD(data);
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

            DataHolderIface.SetFloatVal(thrusterName + " Thruster " + thrusterNum + " RPM Demand", calcPercentDemand(rpmDemand, rpmMode));          //Always write to dataholder the percent
            DataHolderIface.SetFloatVal(thrusterName + " Thruster " + thrusterNum + " Pitch Demand", calcPercentDemand(pitchDemand, pitchMode));          //Always write to dataholder the percent
            DataHolderIface.SetFloatVal(thrusterName + " Thruster " + thrusterNum + " Azimuth Demand", float.Parse(azimuthDemand)/10);
            DataHolderIface.SetStringVal(thrusterName + " Thruster " + thrusterNum + " Operating Location", convLocation(operatingLocation));
            DataHolderIface.SetStringVal(thrusterName + " Thruster " + thrusterNum + " Sentence Status", convStatus(sentenceStatus));

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

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (DHControl dh in m_inputControls)
            {
                dh.readFromDataHolder();
            }

            trcSendButton1.Enabled = (trcAzimuthDemandSet1.Text != "" && trcRpmDemandSet1.Text != "" && trcPitchDemandSet1.Text != "");
            trcSendButton2.Enabled = (trcAzimuthDemandSet2.Text != "" && trcRpmDemandSet2.Text != "" && trcPitchDemandSet2.Text != "");
            trdSendButton1.Enabled = (trdAzimuthResponseSet1.Text != "" && trdRpmResponseSet1.Text != "" && trdPitchResponseSet1.Text != "");
            trdSendButton2.Enabled = (trdAzimuthResponseSet2.Text != "" && trdRpmResponseSet2.Text != "" && trdPitchResponseSet2.Text != "");
            prcSendButton1.Enabled = (prcLeverPosSet1.Text != "" && prcPitchDemandSet1.Text != "" && prcRpmDemandSet1.Text != "");
            prcSendButton2.Enabled = (prcLeverPosSet2.Text != "" && prcPitchDemandSet2.Text != "" && prcRpmDemandSet2.Text != "");
            etlSendButton1.Enabled = (etlEventTimeSet1.Text != "" && etlSubTelPosSet1.Text != "" && etlEventTimeSet1.Text != "");
            etlSendButton2.Enabled = (etlEventTimeSet2.Text != "" && etlSubTelPosSet2.Text != "" && etlEventTimeSet2.Text != "");
            //rpm can only send engine or shaft in each nmea sentence. therefor xor operator is used here.
            rpmSendButton1.Enabled = ((rpmEngSpeedSet1.Text != "" ^ rpmShaftSpeedSet1.Text != "") && rpmPropPitchSet1.Text != "");
            rpmSendButton2.Enabled = ((rpmEngSpeedSet2.Text != "" ^ rpmShaftSpeedSet2.Text != "") && rpmPropPitchSet2.Text != "");
        }

        List<DHControl> m_outputControls = new List<DHControl>();           //This is for later, when trying to stimulate our output to VStep
        List<DHControl> m_inputControls = new List<DHControl>();//This is for taking DataHolder values (written to by parsing NMEA), and displaying them on the screen
        
        private void sendTrc1Button_Click(object sender, EventArgs e)
        {
            string s = "$--TRC,1," + trcRpmDemandSet1.Text + ",P," + trcPitchDemandSet1.Text + ",P," + float.Parse(trcAzimuthDemandSet1.Text)*10 + ",S,C*hh<CR><LF>";//TODO: should we calc checksums?
            parser(s);//TODO: should this just set dataholder instead?
            lastStringSent.Text = s;

            //null out Set boxes & button
            trcRpmDemandSet1.Text = null;
            trcPitchDemandSet1.Text = null;
            trcAzimuthDemandSet1.Text = null;
            trcSendButton1.Enabled = false;
        }
        private void sendTrc2Button_Click(object sender, EventArgs e)
        {
            string s = "$--TRC,2," + trcRpmDemandSet2.Text + ",P," + trcPitchDemandSet2.Text + ",P," + float.Parse(trcAzimuthDemandSet2.Text) * 10 + ",S,C*hh<CR><LF>";
            parser(s);//TODO: should this just set dataholder instead?
            lastStringSent.Text = s;

            //null out Set boxes & button
            trcRpmDemandSet2.Text = null;
            trcPitchDemandSet2.Text = null;
            trcAzimuthDemandSet2.Text = null;
            trcSendButton2.Enabled = false;
        }
        private void trdSendButton1_Click(object sender, EventArgs e)
        {
            string s = "$--TRD,1," + trdRpmResponseSet1.Text + ",P," + trdPitchResponseSet1.Text + ",P," + float.Parse(trdAzimuthResponseSet1.Text)*10 + "*hh<CR><LF>";
            parser(s);
            lastStringSent.Text = s;

            //null out Set boxes & button
            trdRpmResponseSet1.Text = null;
            trdPitchResponseSet1.Text = null;
            trdAzimuthResponseSet1.Text = null;
            trdSendButton1.Enabled = false;
        }
        private void trdSendButton2_Click(object sender, EventArgs e)
        {
            string s = "$--TRD,2," + trdRpmResponseSet2.Text + ",P," + trdPitchResponseSet2.Text + ",P," + float.Parse(trdAzimuthResponseSet2.Text) * 10 + "*hh<CR><LF>";
            parser(s);
            lastStringSent.Text = s;

            //null out Set boxes & button
            trdRpmResponseSet2.Text = null;
            trdPitchResponseSet2.Text = null;
            trdAzimuthResponseSet2.Text = null;
            trdSendButton2.Enabled = false;
        }
        private void prcSendButton1_Click(object sender, EventArgs e)
        {
            string s = "$--PRC," + prcLeverPosSet1.Text + ",A," + prcRpmDemandSet1.Text + ",P," + prcPitchDemandSet1.Text + ",P,S,1*hh<CR><LF>";
            parser(s);
            lastStringSent.Text = s;

            //null out Set boxes & button
            prcLeverPosSet1.Text = null;
            prcRpmDemandSet1.Text = null;
            prcPitchDemandSet1.Text = null;
            prcSendButton1.Enabled = false;
        }
        private void prcSendButton2_Click(object sender, EventArgs e)
        {
            string s = "$--PRC," + prcLeverPosSet2.Text + ",A," + prcRpmDemandSet2.Text + ",P," + prcPitchDemandSet2.Text + ",P,S,2*hh<CR><LF>";
            parser(s);
            lastStringSent.Text = s;

            //null out Set boxes & button
            prcLeverPosSet2.Text = null;
            prcRpmDemandSet2.Text = null;
            prcPitchDemandSet2.Text = null;
            prcSendButton2.Enabled = false;
        }
        private void rpmSendButton1_Click(object sender, EventArgs e)
        {
            string src = (rpmEngSpeedSet1.Text != "") ? "E" : "S";
            string s = "$--RPM," + src + ",1," + rpmEngSpeedSet1.Text + rpmShaftSpeedSet1.Text + "," + rpmPropPitchSet1.Text + ",A*hh<CR><LF>";
            parser(s);
            lastStringSent.Text = s;

            //null out Set boxes & button
            rpmEngSpeedSet1.Text = null;
            rpmShaftSpeedSet1.Text = null;
            rpmPropPitchSet1.Text = null;
            rpmSendButton1.Enabled = false;
        }
        private void rpmSendButton2_Click(object sender, EventArgs e)
        {
            string src = (rpmEngSpeedSet2.Text != "") ? "E" : "S";
            string s = "$--RPM," + src + ",2," + rpmEngSpeedSet2.Text + rpmShaftSpeedSet2.Text + "," + rpmPropPitchSet2.Text + ",A*hh<CR><LF>";
            parser(s);
            lastStringSent.Text = s;

            //null out Set boxes & button
            rpmEngSpeedSet2.Text = null;
            rpmShaftSpeedSet2.Text = null;
            rpmPropPitchSet2.Text = null;
            rpmSendButton2.Enabled = false;
        }
        private void etlSendButton1_Click(object sender, EventArgs e)
        {
            string s = "$--ETL," + etlEventTimeSet1.Text + ",O," + etlSubTelPosSet1.Text + "," + etlTelegraphPosSet1.Text + ",S,1*hh<CR><LF>";
            parser(s);
            lastStringSent.Text = s;

            //null out Set boxes & button
            etlEventTimeSet1.Text = null;
            etlSubTelPosSet1.Text = null;
            etlTelegraphPosSet1.Text = null;
            etlSendButton1.Enabled = false;
        }
        private void etlSendButton2_Click(object sender, EventArgs e)
        {
            string s = "$--ETL," + etlEventTimeSet2.Text + ",O," + etlSubTelPosSet2.Text + "," + etlTelegraphPosSet2.Text + ",S,2*hh<CR><LF>";
            parser(s);
            lastStringSent.Text = s;

            //null out Set boxes & button
            etlEventTimeSet2.Text = null;
            etlSubTelPosSet2.Text = null;
            etlTelegraphPosSet2.Text = null;
            etlSendButton2.Enabled = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(newNmeaBox.SelectedItem.ToString())
            {
                case ("TRC"):
                    {
                        newNmeaDisp1.Text = "RPM Demand(%):";
                        newNmeaDisp2.Text = "Pitch Demand(%):";
                        newNmeaDisp3.Text = "Azimuth Demand:";
                        break;
                    }
            }
            newNmeaDisp1.Visible = true;
            newNmeaDisp2.Visible = true;
            newNmeaDisp3.Visible = true;
            newNmeaInput1.Visible = true;
            newNmeaInput2.Visible = true;
            newNmeaInput3.Visible = true;
        }
    }
}
