﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsterixLib
{
    class CAT62I510UserData
    {

        public static void DecodeCAT62I510(byte[] Data)
        {
            // Increase data buffer index so it ready for the next data item.
            CAT62.CurrentDataBufferOctalIndex = CAT62.CurrentDataBufferOctalIndex + 3;
        }
    }
}
