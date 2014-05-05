using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Xml;

namespace BridgeIface
{
    class Ioio_Com
    {
        /// <summary> Returns requested url as a String. </summary>
        /// <param name="url"> URL to gete</param>
        public string webRequest(string url)
        {
            try
            {
                WebRequest wrGETURL;
                wrGETURL = WebRequest.Create(url);
                wrGETURL.Timeout = 200;

                Stream objStream;
                objStream = wrGETURL.GetResponse().GetResponseStream();

                StreamReader objReader = new StreamReader(objStream);

                string sLine = "";
                int i = 0;
                string readText = "";
                while (sLine != null)
                {
                    i++;
                    sLine = objReader.ReadLine();
                    if (sLine != null)
                    {
                        readText += sLine;
                    }
                }
                return readText;
            }
            catch
            {
                return "Error with Web Request";
            }
        }

        public void setState(IPAddress ip, int pin, int state)
        {
            string website = "http://" + ip.ToString() + ":8181/api/trigger?pin=" + pin + "&state=" + state;
            WebRequest wrGETURL;
            wrGETURL = WebRequest.Create(website);
            wrGETURL.Timeout = 200;
            wrGETURL.GetResponse();
            wrGETURL.Abort();
        }

        public void requestData(String ipAddress)
        {
            //set lever positions based on hardware levers
            string website = "http://" + ipAddress + ":8181/api/status";
            string xmlResponse = webRequest(website);
            parseXml(xmlResponse);
        }

        /// <summary> Parses a given string and pulls out attributes of "pin" names. </summary>
        /// <param name="xmlString"> String to parse</param>
        private void parseXml(string xmlString)
        {
            using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
            {
                if (xmlString != "Error with Web Request")
                {
                    while (reader.Read())
                    {
                        if (reader.Name == "pin")
                        {
                            if (reader.GetAttribute("name") == "RPM Lever") //Sending RPM to Nautis doesn't actually do anything, as of 4/16/14
                            {
                                //float val = (float.Parse(reader.GetAttribute("status")) - 1) * -100;
                                float val = float.Parse(reader.GetAttribute("calibrated"));
                                val = (100 - val); //invert val
                                DataHolderIface.SetFloatVal("HW RPM Send", val);
                            }
                            if (reader.GetAttribute("name") == "Pitch Lever")
                            {
                                //float val = (float.Parse(reader.GetAttribute("status")) - 0.5f) * -200;
                                float val = float.Parse(reader.GetAttribute("calibrated"));
                                val = -val; //invert val
                                DataHolderIface.SetFloatVal("HW Pitch Send", val);
                            }
                            if (reader.GetAttribute("name") == "Rudder")
                            {
                                //float val = 35 - (float.Parse(reader.GetAttribute("status")) * 70);
                                float val = float.Parse(reader.GetAttribute("calibrated"));
                                val = -val; //invert val
                                DataHolderIface.SetFloatVal("HW ROR Send", val);
                            }
                        }
                    }
                }
            }
        }
    }
}
