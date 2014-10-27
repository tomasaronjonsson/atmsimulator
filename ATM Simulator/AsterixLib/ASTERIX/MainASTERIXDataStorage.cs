using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsterixLib
{
    // This class is the main data storage for each message type
    // If the user selects an option to store the data, then each 
    // individual message category will store the latest received 
    // message into this storage area. 
    //
    // This is nothig more than a central place for all data to be
    // stored.
    public class MainASTERIXDataStorage
    {


  
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // CAT 62 Messages
        //
        // Define collection for CAT62 data items. This is where each data item will be stored 
        public class CAT62Data
        {
            public System.Collections.Generic.List<CAT62.CAT062DataItem> CAT62DataItems = new System.Collections.Generic.List<CAT62.CAT062DataItem>();
        }

        // This is the main storage of all CAT62 Messages
        public static System.Collections.Generic.List<CAT62Data> CAT62Message = new System.Collections.Generic.List<CAT62Data>();

        // Define a method to reset all data buffers. The method is to used when switching beetween data sources.
        public static void ResetAllData()
        {

            CAT62Message.Clear();
        }

    }
}
