using System;

namespace BridgeIface
{
    class NMEA_Parser
    {
        private const float MAX_DEGREES = 12.3f;  // Should this really be hard-coded? -nse
        private const float MAX_RPM = 2000f; //Just a wag at max rpm. Should be corrected later.
        
        public string parser(String sentence)
        {
            string[] data = sentence.Split(',', '*');

            string index = data[0];//NMEA Sentence Header

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
                    break;
            }
            return index;
        }

        private void parseTRC(string[] data)
        {
            if (data.Length < 10)
            {
                return;
            }

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
            DataHolderIface.SetFloatVal("Thruster " + thrusterNum + " Azimuth Demand", float.Parse(azimuthDemand) / 10);

        }
        private void parseETL(string[] data)
        {
            if (data.Length < 8)
            {
                return;
            }

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
           }
        }
        private void parsePRC(string[] data)
        {
            if (data.Length < 10)
            {
                return;
            }

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
                return;
            }

            string source = data[1];    //Source, shaft/engine S/E
            string engineNum = data[2]; //Engine or shaft number, numbered from centreline; 0=single or on centreline; odd=starboard; even=port
            string speed = data[3];     //Speed, rev/min, “-“ = counter-clockwise
            string propPitch = data[4]; //Propeller pitch, % of max, “-“ = astern
            string status = data[5];    //Status; A=Data valid, V=Data invalid

            if (status == "A")
            {
                DataHolderIface.SetFloatVal("Engine " + engineNum + " " + convSource(source) + " RPM", float.Parse(speed));
                DataHolderIface.SetFloatVal("Engine " + engineNum + " Propeller Pitch", float.Parse(propPitch));
            }
        }
        private void parseTRD(string[] data)
        {
            if (data.Length < 8)
            {
                return;
            }

            string thrusterNum = data[1];
            string rpmResponse = data[2];
            string rpmMode = data[3];
            string pitchResponse = data[4];
            string pitchMode = data[5];
            string azimuthResponse = data[6];

            DataHolderIface.SetFloatVal("Thruster " + thrusterNum + " RPM", calcPercentDemand(rpmResponse, rpmMode));
            DataHolderIface.SetFloatVal("Thruster " + thrusterNum + " Pitch", calcPercentDemand(pitchResponse, pitchMode));
            DataHolderIface.SetFloatVal("Thruster " + thrusterNum + " Azimuth", float.Parse(azimuthResponse) / 10);
        }
        private void parseBTR(string[] data)
        {
            if (data.Length < 5)
            {
                return;
            }

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
                return;
            }

            string missionStatus = data[1];
            string elapsedTime = data[2];

            DataHolderIface.SetFloatVal("Mission Status", float.Parse(missionStatus));
            DataHolderIface.SetFloatVal("Mission Elapsed Time", float.Parse(elapsedTime));
        }
        private void parseBOW(string[] data)//from V-Step Manual: "This sentence will be deprecated in the future and is replaced by TRC."
        {
            if (data.Length < 4)
            {
                return;
            }

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
                return;
            }

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
                return;
            }

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
                return;
            }

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
                return;
            }

            string angle0 = data[1];
            string angle0valid = data[2];
            string angle1 = data[3];
            string angle1valid = data[4];
            string checkSum = data[5];

            if (angle0valid == "A")
            {
                DataHolderIface.SetFloatVal("Received Rudder Sensor Angle", float.Parse(angle0));
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
    }
}
