﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace BridgeIface
{
    class NMEA_Com
    {

        Utilities util = new Utilities();
        Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        
        Dictionary<string, int> receivedNmeaTypes = new Dictionary<string, int>();
        Dictionary<string, int> sentNmeaTypes = new Dictionary<string, int>();

        public volatile bool runThread;
        NMEA_Parser nmeaParser = new NMEA_Parser();

        /// <summary>
        /// Sends all NMEA Commands in a file to IPEndPoint.
        /// Also updates BindingList with most recently sent sentence sent of each type. </summary>
        /// <param name="sendEP"> IPEndPoint for destination</param>
        /// <param name="file"> Text file containing NMEA commands. "$" and Checksum will be added in function</param>
        /// <param name="list"> (optional) BindingList to update as nmea sentences are sent</param>
        public void sendDynamicNmea(IPEndPoint sendEP, StreamReader file, BindingList<NMEA_Object> list)
        {
            try
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    //remove comments
                    int index = line.IndexOf("//");
                    string nmeaCommand = (index == -1) ? line : line.Substring(0, index);

                    if (nmeaCommand != "") //don't send blank lines
                        sendDynamicNmea(sendEP, nmeaCommand.Trim(), list);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Sends all NMEA Commands in a file to IPEndPoint.
        /// Also updates BindingList with most recently sent sentence sent of each type. </summary>
        /// <param name="sendEP"> IPEndPoint for destination</param>
        /// <param name="file"> Text file containing NMEA commands. "$" and Checksum will be added in function</param>
        public void sendDynamicNmea(IPEndPoint sendEP, StreamReader file)
        {
            try
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    //remove comments
                    int index = line.IndexOf("//");
                    string nmeaCommand = (index == -1) ? line : line.Substring(0, index);

                    if (nmeaCommand != "") //don't send blank lines
                        sendDynamicNmea(sendEP, nmeaCommand.Trim());
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Sends NMEA Command to IPEndPoint.
        /// Also updates BindingList with most recently sent sentence sent of each type. </summary>
        /// <param name="sendEP"> IPEndPoint for destination</param>
        /// <param name="pattern"> NMEA command. "$" and Checksum will be added in function</param>
        /// <param name="list"> (optional) BindingList to update as nmea sentences are sent</param>
        public void sendDynamicNmea(IPEndPoint sendEP, String pattern, BindingList<NMEA_Object> list)
        {
            string[] splitter = pattern.Split(',');
            string command = "";
            string sOut = "";
            string index = "";

            for (int i = 0; i < splitter.Length; i++)
            {
                if (splitter[i].StartsWith("%"))
                {
                    command += DataHolderIface.GetFloatVal(splitter[i].Substring(1));
                }
                else
                {
                    command += splitter[i];
                }
                command += ",";
            }
            sOut = "$" + command + "*" + util.calcChecksum(command) + "\r\n";
            index = splitter[0];
            if (index.Contains("PRC"))
                index += splitter[8];
            //update Sent Table here
            if (receivedNmeaTypes.ContainsKey(index) == false)
            {
                receivedNmeaTypes.Add(index, receivedNmeaTypes.Count);
            }
            updateSentTable(sOut, index, list);

            sendNmeaMessage(sendEP, sOut);
        }

        /// <summary>
        /// Sends NMEA Command to IPEndPoint.</summary>
        /// <param name="sendEP"> IPEndPoint for destination</param>
        /// <param name="pattern"> NMEA command. "$" and Checksum will be added in function</param>
        public void sendDynamicNmea(IPEndPoint sendEP, String pattern)
        {
            string[] splitter = pattern.Split(',');
            string command = "";
            string sOut = "";
            string index = "";

            for (int i = 0; i < splitter.Length; i++)
            {
                if (splitter[i].StartsWith("%"))
                {
                    command += DataHolderIface.GetFloatVal(splitter[i].Substring(1));
                }
                else
                {
                    command += splitter[i];
                }
                command += ",";
            }
            sOut = "$" + command + "*" + util.calcChecksum(command) + "\r\n";
            index = splitter[0];
            if (index.Contains("PRC"))
                index += splitter[8];
            //update Sent Table here
            if (receivedNmeaTypes.ContainsKey(index) == false)
            {
                receivedNmeaTypes.Add(index, receivedNmeaTypes.Count);
            }

            sendNmeaMessage(sendEP, sOut);
        }

        private void updateSentTable(string sentence, string index, BindingList<NMEA_Object> list)
        {
            //Insert new sentence at index
            try
            {
                list[receivedNmeaTypes[index]] = new NMEA_Object() { sentence = sentence, time = System.DateTime.Now };
            }
            catch (System.ArgumentOutOfRangeException)
            {
                list.Insert(receivedNmeaTypes[index], new NMEA_Object() { sentence = sentence, time = System.DateTime.Now });
            }
        }

        private void sendNmeaMessage(IPEndPoint sendEP, string s)
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

        /// <summary>
        /// Receives NMEA Command over specified port through UDP.
        /// Also updates BindingList with most recently received sentence sent of each type. </summary>
        /// <param name="portReceive"> Port to receive on</param>
        /// <param name="list"> (optional) BindingList to update as nmea sentences are received</param>
        public void receiveNmeaMessage(int portReceive, BindingList<NMEA_Object> list)
        {
            UdpClient listener = new UdpClient(portReceive);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, portReceive);
            while (runThread)
            {
                byte[] content = listener.Receive(ref endPoint);
                if (content.Length > 0)
                {
                    string nmeaMessage = Encoding.ASCII.GetString(content);
                    string index = nmeaParser.parser(nmeaMessage);
                    updateReceivedTable(nmeaMessage, index, list);


                }
                else
                { // set breakpoint here cause curious if this code is ever reached. (so far it is not)
                    
                }
            }
            listener.Close();
        }

        /// <summary>
        /// Receives NMEA Command over specified port through UDP.
        /// Also updates BindingList with most recently received sentence sent of each type. </summary>
        /// <param name="portReceive"> Port to receive on</param>
        public void receiveNmeaMessage(int portReceive)
        {
            UdpClient listener = new UdpClient(portReceive);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, portReceive);
            while (runThread)
            {
                byte[] content = listener.Receive(ref endPoint);
                if (content.Length > 0)
                {
                    string nmeaMessage = Encoding.ASCII.GetString(content);
                    string index = nmeaParser.parser(nmeaMessage);
                }
            }
            listener.Close();
        }

        public void updateReceivedTable(string sentence, string index, BindingList<NMEA_Object> list)
        {
            //Check if string has been seen before. If not, add it to Dictionary for array indexing.
            if (sentNmeaTypes.ContainsKey(index) == false)
            {
                sentNmeaTypes.Add(index, sentNmeaTypes.Count);
            }

            //delegate void updateList(list);

            //Insert new sentence at index
            try
            {
                list[sentNmeaTypes[index]] = new NMEA_Object() { sentence = sentence, time = System.DateTime.Now };
            }
            catch (System.ArgumentOutOfRangeException e)
            {
                try
                {
                    list.Insert(sentNmeaTypes[index], new NMEA_Object() { sentence = sentence, time = System.DateTime.Now });
                }
                catch { }
            }
        }
    }
}
