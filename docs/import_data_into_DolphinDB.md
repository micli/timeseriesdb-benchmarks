# Import data into DolphinDB

In this document, you will learn how to create database, partionTable and import Nasdaq data into database.

# Creating Database

DophinDB supports functions interacte with users. To create a database, it uses database function as below:

```shell
database(directory, [partitionType], [partitionScheme], [locations])
```
For optmizing I/O, user can specify partitionType parameter to make data store into different partitions. To create a database, it at least specified a database file.

The function returns dbHandle object for future use.

```shell
database("dfs://nasdaq")
```

# Creating Partitioned Table

Partitioned table used for store data into different partition to get max I/O performance. The function declaration as below:

```shell
createPartitionedTable(dbHandle, table, [tableName], partitionColumns)
```

Before createPartitionedTable, it needs to call table function to make a table declaration.

```shell
table(capacity:size, colNames, colTypes)
```

capacity means initial memory size of this table. If data size more than capacity, table will automatically allocate more memory to maintain data. size means inital data occupied memory.

```shell

table(20:0, 'date'code'opening_price'highest_price'lowest_price'closing_price'adjusted_closing_price'trade_volume,[TIMESTAMP,STRING,DOUBLE,DOUBLE,DOUBLE,DOUBLE,DOUBLE,DOUBLE])

```

To build both database and partitioned table, please use below code:

```shell
yearRange=date(2011.01M + 12*0..22)
dbPath="dfs://nasdaqdb"
login("admin","123456")
if(existsDatabase(dbPath)){
	dropDatabase(dbPath)
}
db=database(dbPath,VALUE,yearRange)
saveDatabase(db);
tb_nasdaq=table(20:0, 'date''code''opening_price''highest_price''lowest_price''closing_price''adjusted_closing_price''trade_volume',[TIMESTAMP,SYMBOL,DOUBLE,DOUBLE,DOUBLE,DOUBLE,DOUBLE,INT])
pdt=createPartitionedTable(db, tb_nasdaq, 'tb_nasdaq', 'date');
```

# Imoporting data

There are three ways to retrieve data from a CSV file.

+ loadText: Import a text file as a memory table.
+ ploadText: Parallel import text files as partition memory tables.
+ loadTextEx: Import text files into databases, including distributed databases, local disk databases or memory databases.
+ textChunkDS: divide the text file into multiple small data sources, and then use the mr function for flexible data processing.

ploadText is the be one in current scenario. Before start dolphindb session, please make sure CSV file locate in current directory. To get cost time and verify data loss, It's needed output execution time(timer) and loaded item count(count(*)).

```shell
dbPath="dfs://nasdaqdb"
login("admin","123456")
db=database(dbPath)
tb_nasdaq=loadTable(db,'tb_nasdaq')
timer tb_nasdaq=ploadText('nasdaq_dolphin_data.csv')
select count(*) from tb_nasdaq
pdt=loadTable(db,'tb_nasdaq')
pdt.append!(tb_nasdaq);
```

# Experiment Result

|Number of experiments|Time cost(ms)|
|:-|:-|
|1st|2026.2 ms|
|2nd|1998.19 ms|
|3rd|1993.18 ms|
|4th|1949.8 ms|

# Reference
+ [DolphinDB 文本数据加载教程](https://gitee.com/dolphindb/Tutorials_CN/blob/master/import_csv.md) 
+ [DolphinDB 文本数据数据库教程](https://zhuanlan.zhihu.com/p/46299595)