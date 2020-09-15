using System;
using System.IO;
using Nasdaq.Data;

namespace Nasdaq2TDengine
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length < 1)
            {
                Console.WriteLine("Please specify Nasdaq data folder path.");
                return;
            }
            string path = args[0];  //"/Users/micl/Documents/Nasdaq/us/nasdaq/";
            if (!Directory.Exists(path))
            {
                Console.WriteLine("The path {0} does not exist.", path);
                return;
            }

            // Delete history data folder and re-create.
            if (Directory.Exists("data"))
            {
                Directory.Delete("data", true);
            }
            Directory.CreateDirectory("data");

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
                    var filename = "./data/" + Path.GetFileName(folder) + ".csv";
                    if (File.Exists(filename))
                        File.Delete(filename);
                    using (StreamWriter sw = new StreamWriter(filename))
                    {
                        Console.WriteLine("Now reading file : {0}", f);
                        var stock = new Stock(f);
                        stock.LoadData();
                        Console.WriteLine("File loaded, total {0} items", stock.Data.Count);
                        foreach (StockData d in stock.Data)
                        {
                            sw.WriteLine(d.ToString("tdengine"));
                        }
                        sw.Flush();
                    }

                }
                currentFolder++;
            }

            // Create batch files
            StreamWriter swCreate = new StreamWriter("create_table_tdengine.txt");
            StreamWriter swImport = new StreamWriter("import_table_tdengine.txt");
            swImport.WriteLine("use nasdaq;");
            swCreate.WriteLine("use nasdaq;");
            foreach (string folder in subdirectiores)
            {
                Console.WriteLine("Now handling {0} / {1} folder, Symbol =  {2}",
                    currentFolder, subFolderCount, folder.Substring(folder.LastIndexOf("/") + 1, folder.Length - folder.LastIndexOf("/") - 1));
                var symbol = Path.GetFileName(folder);
                swCreate.WriteLine(string.Format("CREATE TABLE {0} USING tb_nasdaq TAGS (\"{0}\");", symbol));
                swImport.WriteLine(string.Format("INSERT INTO {0} FILE \'{0}.csv\';", symbol));
            }

            swCreate.Close();
            swImport.Close();
        }
    }
}
