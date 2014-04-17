using System;

namespace BridgeIface
{
    class Utilities
    {
        public string calcChecksum(string s)
        {
            int checksum = 0;
            for (int i = 0; i < s.Length; i++)
            {
                checksum ^= Convert.ToByte(s[i]);
            }
            return checksum.ToString("X2");
        }

        
    }
}
