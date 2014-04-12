using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace BridgeIface
{
    class ioioIO
    {
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
    }
}
