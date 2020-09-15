using System;
using System.IO;
using InfluxDB;
using InfluxDB.Client;
using InfluxDB.Client.Core;
using InfluxDB.LineProtocol.Payload;
using InfluxDB.LineProtocol.Client;

using Nasdaq.Data;
using System.Collections.Generic;

namespace NasdaqImport2InfluxDB
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = "/Users/micl/Documents/Nasdaq/us/nasdaq/";
            var database = "nasdaq";
           
            LineProtocolClient client = new LineProtocolClient(new Uri("http://40.73.35.55:8086"), database);
           
            string[] subdirectiores = System.IO.Directory.GetDirectories(path);
            int subFolderCount = subdirectiores.Length;
            Console.WriteLine("Total found {0} sub folders", subFolderCount);
            int currentFolder = 1;
            DateTime start = DateTime.Now;
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

                    LineProtocolPayload payload = new LineProtocolPayload();
                    foreach (StockData d in stock.Data)
                    {
                        LineProtocolPoint point = new LineProtocolPoint(
                            "tb_nasdaq",
                            new Dictionary<string, object>
                            {
                                { "opening_price",  d.opening_price},
                                { "highest_price", d.highest_price},
                                { "lowest_price", d.lowest_price},
                                { "closing_price", d.closing_price},
                                { "adjusted_closing_price", d.adjusted_closing_price},
                                { "trade_volume", d.trade_volume},
                            },
                            new Dictionary<string, string>
                            {
                                { "code", d.code }
                            },
                            DateTime.Parse(d.date).ToUniversalTime());
                        payload.Add(point);

                    }
                    var result = client.WriteAsync(payload).GetAwaiter().GetResult();
                    if (!result.Success)
                    {
                        Console.WriteLine(result.ErrorMessage);
                    }
                }
                currentFolder++;
            }

            DateTime end = DateTime.Now;
            Console.WriteLine("Total spent: {0} seconds", (end - start).TotalSeconds);
        }
    }
}
