# Import data into InfluxDB database

In this document you will learn how to imoport CSV file into InfluxDB.


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

  Total time cost:  minutes

Telegraf is not an efficient way to import data. It's offical suggestion.
