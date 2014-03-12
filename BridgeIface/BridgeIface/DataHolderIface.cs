using System;

using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace BridgeIface
{
    /// <summary>
    /// 
    /// </summary>
    public static class DataHolderIface
    {
        private static Dictionary<string, double> doubleMap = new Dictionary<string, double>();
        private static Dictionary<string, float> floatMap = new Dictionary<string, float>();
        private static Dictionary<string, int> intMap = new Dictionary<string, int>();
        private static Dictionary<string, string> stringMap = new Dictionary<string, string>();
        

        #region DataHolder methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        public static void SetDoubleVal(System.String name, double val)
        {
            doubleMap[name] = val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        public static void SetFloatVal(System.String name, float val)
        {
            floatMap[name] = val;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        public static void SetIntVal(System.String name, int val)
        {
            intMap[name] = val;
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
            stringMap[name] = val;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static double GetDoubleVal(System.String name)
        {
            if(doubleMap.ContainsKey(name))
                return doubleMap[name];
            return 0.0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static float GetFloatVal(System.String name)
        {
            if (floatMap.ContainsKey(name))
                return floatMap[name];
            return 0.0f;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int GetIntVal(System.String name)
        {
            if (intMap.ContainsKey(name))
                return intMap[name];
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
            if (stringMap.ContainsKey(name))
                return stringMap[name];
            return "";
        }
        #endregion
    }
}
