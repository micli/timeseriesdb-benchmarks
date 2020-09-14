# Import data into InfluxDB database

In this document you will learn how to imoport CSV file into InfluxDB.


## Creating Database

Before import data into database, you have to create one first. Now need a database with 3650 retenation policy.

```shell
> influx

CREATE DATABASE nasdaq WITH DURATION 3650d
use nasdaq

```


## Install Telegraf

Telegraf is an agent written in Go for collecting metrics and writing them into InfluxDB or other possible outputs. This guide will get you up and running with Telegraf. It walks you through the download, installation, and configuration processes, and it shows how to use Telegraf to get data into InfluxDB. Telegraf can effecificntly import CSV data into InflxDB.

Requirements:

+ Telegraf 1.8.0 or higher
+ InfluxDB 1.7.0 or higher

Download and install Telegraf directly:

```shell
wget https://dl.influxdata.com/telegraf/releases/telegraf_1.15.3-1_amd64.deb
sudo dpkg -i telegraf_1.15.3-1_amd64.deb
```

## Configuring Telegraf

Configuration file location by installation type

Linux debian and RPM packages: /etc/telegraf/telegraf.conf

Creating and editing the configuration file

Before starting the Telegraf server you need to edit and/or create an initial configuration that specifies your desired inputs (where the metrics come from) and outputs (where the metrics go). There are several ways to create and edit the configuration file. Here, we’ll generate a configuration file and simultaneously specify the desired inputs with the -input-filter flag and the desired output with the -output-filter flag.

Now need to generate a configure file for Telegraf:

```shell
telegraf -sample-config -input-filter file -output-filter influxdb > file.conf
```

Use nano open file.conf and do some edit action.

```shell

nano file.conf

```
At line 116, set database equals "nasdaq"

```shell
database = "nasdaq"
```

At Input Plugins section, add below lines to describe CSV file content:

```shell

###############################################################################
#                            INPUT PLUGINS                                    #
###############################################################################

# Reload and gather from file[s] on telegraf's interval.
[[inputs.file]]
  ## Files to parse each interval.
  ## These accept standard unix glob matching rules, but with the addition of
  ## ** as a "super asterisk". ie:
  ##   /var/log/**.log     -> recursively find all .log files in /var/log
  ##   /var/log/*/*.log    -> find all .log files with a parent dir in /var/log
  ##   /var/log/apache.log -> only read the apache log file
  files = ["example.csv"]

  ## The dataformat to be read from files
  ## Each data format has its own unique set of configuration options, read
  ## more about them here:
  ## https://github.com/influxdata/telegraf/blob/master/docs/DATA_FORMATS_INPUT.md
  data_format = "csv"
  csv_header_row_count = 1
  csv_comment = "#"
  csv_measurement_column = "measurement"
  csv_tag_columns = ["code"]
  csv_timestamp_column = "date"
  csv_timestamp_format = "unix_ns"

```

## Starting Telegraf

Put CSV data file and file.conf together in current floder. Then start telegraf with specified configuration file.

```shell

telegraf --config file.conf

```

CSV sample as below:

```csv

measurement,code,opening_price,highest_price,lowest_price,closing_price,adjusted_closing_price,trade_volume,date
tb_nasdaq,LANC,69.49,71.58,69.04,70.47,52.8236,107000,1325520000
tb_nasdaq,LANC,68.92,69.65,68.59,69.08,52.39031,67100,1325606400
tb_nasdaq,LANC,69.17,69.3,67.9,68.72,52.580345,97300,1325692800
tb_nasdaq,LANC,68.7,69.33,68.55,68.98,52.223064,81700,1325779200
tb_nasdaq,LANC,69.35,69.51,68.55,69.03,52.71719,125700,1326038400

```

  ## Result

  Total time cost > 10 hours.

Telegraf is not an efficient way to import data. It's offical suggestion.

## importing data by HTTP service

Since Telegraf is too slowly to import, we have to find a other way to import data. In this secion, we will use a InfluxDB client to access REST API by line protocol to import data into InfluxDB.

## Install package

```shell

Install-Package InfluxDB.LineProtocol

```
In the code, it will search all sub directories that contain Nasdaq trading data for each company. In each sub directory, It will retrieve trading data from .json file. And then, it will build an array of LinePoint objects that will be sent by PayLoad object trough HTTP connection.

InfluxDB will get all of LinePoint objects and write them to database: nasdaq.

```csharp

        static void Main(string[] args)
        {
            var path = "/Users/micl/Documents/Nasdaq/us/nasdaq/";
            var database = "nasdaq";
           
            LineProtocolClient client = new LineProtocolClient(new Uri("http://{server}:8086"), database);
           
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
            Console.WriteLine("Total spent: {0} seconds", (end - start).Seconds);
        }

```
In this way, time spent: 2068.86100 seconds(about 35 minutes).

