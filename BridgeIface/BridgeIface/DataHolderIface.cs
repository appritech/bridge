using System;

using System.Runtime.InteropServices;

namespace BridgeIface
{
    /// <summary>
    /// 
    /// </summary>
    public static class DataHolderIface
    {
        private const string DLL_NAME = "DataHolder.dll";

        #region Extern/Imported DLL functions
        [DllImport(ImportDir + DLL_NAME, EntryPoint = "setDoubleVal", CallingConvention = CallingConvention.Cdecl)]
        private static extern void setDoubleVal(System.String name, double val);

        [DllImport(ImportDir + DLL_NAME, EntryPoint = "setFloatVal", CallingConvention = CallingConvention.Cdecl)]
        private static extern void setFloatVal(System.String name, float val);

        [DllImport(ImportDir + DLL_NAME, EntryPoint = "setIntVal", CallingConvention = CallingConvention.Cdecl)]
        private static extern void setIntVal(System.String name, int val);

        [DllImport(ImportDir + DLL_NAME, EntryPoint = "setBoolVal", CallingConvention = CallingConvention.Cdecl)]
        private static extern void setBoolVal(System.String name, bool val);

        [DllImport(ImportDir + DLL_NAME, EntryPoint = "setSingleStringVal", CallingConvention = CallingConvention.Cdecl)]
        private static extern void setStringVal(System.String name, System.String val);

        [DllImport(ImportDir + DLL_NAME, EntryPoint = "getDoubleVal", CallingConvention = CallingConvention.Cdecl)]
        private static extern double getDoubleVal(System.String name);

        [DllImport(ImportDir + DLL_NAME, EntryPoint = "getFloatVal", CallingConvention = CallingConvention.Cdecl)]
        private static extern float getFloatVal(System.String name);

        [DllImport(ImportDir + DLL_NAME, EntryPoint = "getIntVal", CallingConvention = CallingConvention.Cdecl)]
        private static extern int getIntVal(System.String name);

        [DllImport(ImportDir + DLL_NAME, EntryPoint = "getBoolVal", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool getBoolVal(System.String name);

        [DllImport(ImportDir + DLL_NAME, EntryPoint = "getSingleStringVal", CallingConvention = CallingConvention.Cdecl)]
        private static unsafe extern void getStringVal(System.String name, byte* ret, ref int length);

        [DllImport(ImportDir + DLL_NAME, EntryPoint = "saveSnapshot", CallingConvention = CallingConvention.Cdecl)]
        private static extern int saveSnapshot(System.String filename);

        [DllImport(ImportDir + DLL_NAME, EntryPoint = "loadSnapshot", CallingConvention = CallingConvention.Cdecl)]
        private static extern int loadSnapshot(System.String filename);

        [DllImport(ImportDir + DLL_NAME, EntryPoint = "loadReplay", CallingConvention = CallingConvention.Cdecl)]
        private static extern int loadReplay(double timestamp);

        [DllImport(ImportDir + DLL_NAME, EntryPoint = "loadReplayKeys", CallingConvention = CallingConvention.Cdecl)]
        private static extern int loadReplayKeys(System.String filename);

        [DllImport(ImportDir + DLL_NAME, EntryPoint = "saveReplay", CallingConvention = CallingConvention.Cdecl)]
        private static extern int saveReplay(double timestamp, bool fullFrame);

        [DllImport(ImportDir + DLL_NAME, EntryPoint = "saveReplayKeys", CallingConvention = CallingConvention.Cdecl)]
        private static extern int saveReplayKeys(System.String filename);

        [DllImport(ImportDir + DLL_NAME, EntryPoint = "setReplayLoaderFilename", CallingConvention = CallingConvention.Cdecl)]
        private static extern int setReplayLoaderFilename(System.String filename);

        [DllImport(ImportDir + DLL_NAME, EntryPoint = "setReplaySaverFilename", CallingConvention = CallingConvention.Cdecl)]
        private static extern int setReplaySaverFilename(System.String filename);

        [DllImport(ImportDir + DLL_NAME, EntryPoint = "setReplayLoaderKeyFilename", CallingConvention = CallingConvention.Cdecl)]
        private static extern int setReplayLoaderKeyFilename(System.String filename1,
                                                             System.String filename2,
                                                             System.String filename3,
                                                             System.String filename4);

        [DllImport(ImportDir + DLL_NAME, EntryPoint = "setReplaySaverKeyFilename", CallingConvention = CallingConvention.Cdecl)]
        private static extern int setReplaySaverKeyFilename(System.String filename1,
                                                            System.String filename2,
                                                            System.String filename3,
                                                            System.String filename4);

        [DllImport(ImportDir + DLL_NAME, EntryPoint = "stopReplaySaver", CallingConvention = CallingConvention.Cdecl)]
        private static extern int stopReplaySaver();

        [DllImport(ImportDir + DLL_NAME, EntryPoint = "startReplaySaver", CallingConvention = CallingConvention.Cdecl)]
        private static extern int startReplaySaver();

        [DllImport(ImportDir + DLL_NAME, EntryPoint = "startReplayLoader", CallingConvention = CallingConvention.Cdecl)]
        private static extern int startReplayLoader();

        [DllImport(ImportDir + DLL_NAME, EntryPoint = "stopReplayLoader", CallingConvention = CallingConvention.Cdecl)]
        private static extern int stopReplayLoader();

        [DllImport(ImportDir + DLL_NAME, EntryPoint = "getNumReplayFrames", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint getNumReplayFrames();

        [DllImport(ImportDir + DLL_NAME, EntryPoint = "loadReplayThruFrame", CallingConvention = CallingConvention.Cdecl)]
        private static extern int loadReplayThruFrame(uint frame);

        [DllImport(ImportDir + DLL_NAME, EntryPoint = "getFirstReplayTimestamp", CallingConvention = CallingConvention.Cdecl)]
        private static extern double getFirstReplayTimestamp();

        [DllImport(ImportDir + DLL_NAME, EntryPoint = "getLastReplayTimestamp", CallingConvention = CallingConvention.Cdecl)]
        private static extern double getLastReplayTimestamp();

        [DllImport(ImportDir + DLL_NAME, EntryPoint = "setReplayMaxFileSize", CallingConvention = CallingConvention.Cdecl)]
        private static extern void setReplayMaxFileSize(ulong fileSize);

        [DllImport(ImportDir + DLL_NAME, EntryPoint = "getBranchPoint", CallingConvention = CallingConvention.Cdecl)]
        private static extern ulong getBranchPoint();

        [DllImport(ImportDir + DLL_NAME, EntryPoint = "branchReplay", CallingConvention = CallingConvention.Cdecl)]
        private static extern int branchReplay(System.String preBranchFile, ulong branchPoint, uint frameID);

        [DllImport(ImportDir + DLL_NAME, EntryPoint = "getLastLoadedFrameID", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint getLastLoadedFrameID();

        [DllImport("MPRIMarinIface.dll", EntryPoint = "runIface", CallingConvention = CallingConvention.Cdecl)]
        private static extern void runIface(System.String IPAddress);

        [DllImport("MPRIMarinIface.dll", EntryPoint = "setNetworkStringPacket", CallingConvention = CallingConvention.Cdecl)]
        private static extern void setNetworkStringPacket(String[] names, int numStrings);

        [DllImport("MPRIMarinIface.dll", EntryPoint = "setNetworkDataPacket", CallingConvention = CallingConvention.Cdecl)]
        private static extern void setNetworkDataPacket(String[] intNames, int numInts, String[] floatNames, int numFloats);

        [DllImport("MPRIMarinIface.dll", EntryPoint = "loadNetworkData", CallingConvention = CallingConvention.Cdecl)]
        private static extern void loadNetworkData(System.String filename);
        


        #endregion

        
        /// <summary>
        /// 
        /// </summary>
        public const string ImportDir = "";

        /// <summary>
        /// 
        /// </summary>
        public static bool isDllAvailable;

        static DataHolderIface()
        {
            initDataHolder();
        }

        /// <summary>
        /// 
        /// </summary>
        public static void initDataHolder()
        {
            string curDir = System.IO.Directory.GetCurrentDirectory() + @"\";

            isDllAvailable = System.IO.File.Exists(curDir + DLL_NAME);
            if (isDllAvailable)
            {
                System.Diagnostics.Debug.WriteLine("I have the DLL from: " + curDir);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("No DLL to be found from: " + curDir);
                // TODO: don't suppress this exception!
                //throw new DllNotFoundException(DLL_NAME + " not found");
            }

        }

        #region DataHolder methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        public static void SetDoubleVal(System.String name, double val)
        {
            // TODO: don't do this "isDllAvailable" defensive-code-exception-suppression stuff
            // Just call the function, if the dll isn't available a dll-not-found exception will be thrown
            // the trick is, additional exception handling will needed to be added where this class is initialized

            if (isDllAvailable)
                setDoubleVal(name, val);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        public static void SetFloatVal(System.String name, float val)
        {
            if (isDllAvailable)
                setFloatVal(name, val);
        }

        // 11-July-2011 - RW- This is a VERY bad idea. We shouldn't use strings to hold our enum vals... Only ints
        //                I don't have time to re-write this on-site (and it isn't used anywhere) so I removed it.
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="name"></param>
        ///// <param name="enumVal"></param>
        //public static void SetEnumVal<T>(String name, T enumVal) where T : struct
        //{
        //    if (isDllAvailable)
        //        setStringVal(name, enumVal.ToString());
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        public static void SetIntVal(System.String name, int val)
        {
            if (isDllAvailable)
                setIntVal(name, val);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        public static void SetBoolVal(System.String name, bool val)
        {
            //For now, we will re-route the bools to be ints in order to talk with the hardware
            // because Iface treats all bools as ints right now
            SetIntVal(name, val ? 1 : 0);

            //In the future, we will want to separate bools from ints
            
            //if (isDllAvailable)
            //  setBoolVal(name, val);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        public static void SetStringVal(System.String name, System.String val)
        {
            //lock (stringLock)
            {
                if (isDllAvailable)
                    setStringVal(name, val);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static double GetDoubleVal(System.String name)
        {
            if (isDllAvailable)
                return getDoubleVal(name);
            return 0.0;
        }

        public static DateTime GetDateTimeVal(string name)
        {
            // storing as bytes
            byte[] bytes = new byte[8];
            for (int i = 0; i < 8; i++)
			{
                bytes[i] = (byte)GetIntVal(string.Format("{0}[{1}]", name, i));
			}
            long binary = BitConverter.ToInt64(bytes, 0);
            return DateTime.FromBinary(binary);
        }

        public static void SetDateTimeVal(string name, DateTime dateTime)
        {
            // DateTime is stored as bytes
            byte[] bytes = BitConverter.GetBytes(dateTime.Ticks);
            for (int i = 0; i < 8; i++)
            {
                SetIntVal(string.Format("{0}[{1}]", name, i), bytes[i]);
            }
        }

        // 11-July-2011 - RW- This is a VERY bad idea. We shouldn't use strings to hold our enum vals... Only ints
        //                I don't have time to re-write this on-site (and it isn't used anywhere) so I removed it.
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="name"></param>
        ///// <returns></returns>
        //public static T GetEnumVal<T>(string name) where T : struct
        //{
        //    return Utils.EnumSafeParse<T>(name, default(T));
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static float GetFloatVal(System.String name)
        {
            if (isDllAvailable)
                return getFloatVal(name);
            return 0.0f;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int GetIntVal(System.String name)
        {
            if (isDllAvailable)
                return getIntVal(name);
            return 0;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool GetBoolVal(System.String name)
        {
            //For now, we will re-route the bools to be ints in order to talk with the hardware
            // because Iface treats all bools as ints right now
            return GetIntVal(name) == 0 ? false : true;

            //In the future, we will want to separate bools from ints

            //TODO: exception handling
            //if (isDllAvailable)
            //    return getBoolVal(name);
            //else
            //    return false;  //TODO: avoid this kind of defensive programming
        }
        private static object stringLock = new object();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static unsafe System.String GetStringVal(System.String name)
        {
            //lock (stringLock)
            {
                if (isDllAvailable)
                {
                    if (string.IsNullOrEmpty(name))
                        return "";

                    int length = 255;           //Strings limited to 255 chars (fairly standard number... we shouldn't be getting here though)
                    //getStringVal(name, null, ref length);     //11-July-2011 - Checking length was causing problems. Assume all strings are less than 150???

                    byte[] ret = new byte[length];
                    string retString = "";
                    fixed (byte* temp = ret)
                    {
                        getStringVal(name, temp, ref length);       //Get string (150 characters in array

                        byte[] retCopy = new byte[length];          //Copy into the right size of array
                        for (int i = 0; i < length; i++)
                        {
                            retCopy[i] = ret[i];
                        }
                        retString = System.Text.Encoding.ASCII.GetString(retCopy);       //Convert to managed string
                    }

                    return retString;
                    //return System.Text.Encoding.ASCII.GetString(ret);

                    //return getStringVal(name, pos);
                }
                return "";
            }
        }
        #endregion
    }
}
