# Testing Data

The orignial data cannot be directly import into database. 
Because the database has its own compatible data format. We need convert data before importing.

+ The original data located at [**HERE**](https://dxact.blob.core.chinacloudapi.cn/testdata/nasdaq.zip).

+ DolphinDB data can be dowload from [**HERE**](https://dxact.blob.core.chinacloudapi.cn/testdata/nasdaq_dolphin_data.zip).

+ The data for TDEngine importing located at [**HERE**](https://dxact.blob.core.chinacloudapi.cn/testdata/tdengine_data.zip).
  
+ To import data into InfluxDb, please use .NET Core app NasdaqImport2InfluxDB which is located at /code/NasdaqImport2InfluxDB/

# Installation Pacakages

+ [DolphinDB 64-bits](https://dxact.blob.core.chinacloudapi.cn/testdata/DolphinDB_Linux64_V1.00.24.zip)

+ [KDB+](https://dxact.blob.core.chinacloudapi.cn/testdata/kdb-linux_x86.zip)

+ [TDEngine](https://dxact.blob.core.chinacloudapi.cn/testdata/TDengine-server-2.0.3.0-Linux-x64.deb)