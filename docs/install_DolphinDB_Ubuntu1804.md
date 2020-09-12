# Install DolphinDB on Ubuntu 18.04

In this document you will learn how to install and configure DolphinDB on Ubuntu 18.04.

## Manually Installation

1. Install unzip for exacting binary files.

```shell

sudo apt-get update
sudo apt-get install unzip

```

2. Download DolphinDB Community Edition from offical site.

```shell

wget http://www.dolphindb.cn/downLinux64-ABI.php

```
DolphinDB 32-bit version is not supports running on 64-bit Linux. Please carefully select right version for downloading.

3. Create a folder and unzip DolphinDB into it.

```shell

mkdir DolphinDB
cd DolphinDB
mv ~/DolphinDB_Linux64_V1.10.14_ABI.zip ./
unzip DolphinDB_Linux64_V1.10.14_ABI.zip

```

4. Starting service

To starting service, you need to create a service configuration file first.

```
./dolphindb -maxMemSize 16

```


Notes:

***

Quit dolphinDB interactive session, please input 'quit' directly and press enter. quit is a command not a function.

***