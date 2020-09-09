# Install Influxdb database on Ubuntu 18.04

In this document you will learn how to install InfluxDB database on Ubuntu 18.04.

## Install Manaually

1. Downlaod Influxdb from offical website

```shell
mkdir influxdb
cd influxdb
wget https://dl.influxdata.com/influxdb/releases/influxdb_1.8.2_amd64.deb
```

2. Run installation by dpkg

```shell
sudo dpkg -i influxdb_1.8.2_amd64.deb
```

3. Ensure influxdb service maintained by systemd.

```shell
sudo systemctl enable --now influxdb
```

## Reference

+ [Influxdb 1.8.2 download page](https://portal.influxdata.com/downloads/)
+ [Influxdb sample data](https://docs.influxdata.com/influxdb/v1.7/query_language/data_download/)