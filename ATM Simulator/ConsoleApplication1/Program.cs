using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsterixLib;
using System.IO;


namespace AsterixLib
{
    public class Program
    {
        static void Main(string[] args)
        {



            //SystemAdaptationDataSet.InitializeData();
           // DynamicDisplayBuilder.Initialise();

            // Here call constructor 
            // for each ASTERIX type

            CAT62.Intitialize(true);
            
            //INSERT DATA HERe


            byte[] array = File.ReadAllBytes("sample.pcap"); ;

            CAT62DecodeAndStore.Do(array);

            Console.WriteLine("filesize: " + array.Length);
            if (MainASTERIXDataStorage.CAT62Message.Count > 0)
            {
                Console.WriteLine("Found something!");
                foreach (MainASTERIXDataStorage.CAT62Data Msg in MainASTERIXDataStorage.CAT62Message)
                {

                    foreach (CAT62.CAT062DataItem item in Msg.CAT62DataItems.Where(x => x.ID == "100"))
                    {
                        Console.WriteLine("id: " + item.ID + " value: " + item.value);
                    }
                }
            }
            Console.ReadLine();
        }
    }
}
