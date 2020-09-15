using System;
using System.IO;
using Nasdaq.Data;

namespace Nasdaq2InfluxDB
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Measurement name or data folder path not specified.");
                return;
            }
            var path = args[0];
            var measurement = args[1];
            using (StreamWriter sw = new StreamWriter("nasdaq_influx_batch.csv"))
            {
                //sw.WriteLine("measurement,code,opening_price,highest_price,lowest_price,closing_price,adjusted_closing_price,trade_volume,date");
                //sw.Flush();
                string[] subdirectiores = System.IO.Directory.GetDirectories(path);
                int subFolderCount = subdirectiores.Length;
                Console.WriteLine("Total found {0} sub folders", subFolderCount);
                int currentFolder = 1;
                foreach (string folder in subdirectiores)
                {
                    Console.WriteLine("Now handling {0} / {1} folder, Symbol =  {2}",
                        currentFolder, subFolderCount, folder.Substring(folder.LastIndexOf("/") + 1, folder.Length - folder.LastIndexOf("/") - 1));
                    string[] files = Directory.GetFiles(folder, "*.json");
                    foreach (string f in files)
                    {
                        Console.WriteLine("Now reading file : {0}", f);
                        var stock = new Stock(f);
                        stock.LoadData();
                        Console.WriteLine("File loaded, total {0} items", stock.Data.Count);
                        foreach (StockData d in stock.Data)
                        {
                            sw.WriteLine("{0},{1}", measurement, d.ToString("influxdb"));
                        }
                        sw.Flush();

                    }
                    currentFolder++;
                }
            }
        }
    }
}
