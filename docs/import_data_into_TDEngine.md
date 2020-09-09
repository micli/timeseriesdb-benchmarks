# Import data into TDengine database

In this document, you will learn how to import Nasdaq stock data into TDEngine database.

## 1. Create a database in TDEngine service. 

TDEngine use SQL-like language to intract with users. You can use below command to create a database named nasdaq.

```shell
CREATE DATABASE nasdaq [KEEP 3650];
USE nasdaq;
```
KEEP means data retention, any data stay long than keep days will be delete from database. The default value is 3650 == 10 years. USE command can help user swith databases in TDEngine service.

## 2. Creating a super table.

In TDEngine, every device or identified object should has its owned data table. All of this kind of data table has same data struct. The super table is a template that defines data struct for a kind of device or identified object.

```sql
create table tb_nasdaq(date TIMESTAMP,opening_price float,highest_price float,lowest_price float,closing_price float,adjusted_closing_price float,trade_volume float) tags (code BINARY(10));
```

In this table, date column is timer series index. It's required. The column 'code' is used to identify data belongs which company. It used for group by calculation on super table.

The super table looks like a data view in RMDBS. It doesn't maintain any data physically but can offer a way to query physical data table in common statements.

TDEngine only supports 10 kinds of data types: 
|Data Type|	Bytes|Note|
|:-|:-|:-|
|TINYINT|1|A nullable integer type with a range of [-127, 127]​|
|SMALLINT|2|A nullable integer type with a range of [-32767, 32767]​|
|INT|4|A nullable integer type with a range of [-2^31+1, 2^31-1 ]|
|BIGINT|8|A nullable integer type with a range of [-2^59, 2^59 ]|​
|FLOAT|4|A standard nullable float type with 6 -7 significant digits and a range of [-3.4E38, 3.4E38]|
|DOUBLE|8|A standard nullable double float type with 15-16 significant| digits and a range of [-1.7E308, 1.7E308]​|
|BOOL|1|A nullable boolean type, [true, false]|
|TIMESTAMP|8|A nullable timestamp type with the same usage as the primary column timestamp|
|BINARY(M)|M|A nullable string type whose length is M, error should be threw with exceeded chars, the maximum length of M is 65526, but as maximum row size is 64K bytes, the actual upper limit will generally less than 65526. This type of string only supports ASCii encoded chars.|
|NCHAR(M)|4|* M	A nullable string type whose length is M, error should be threw with exceeded chars. The NCHAR type supports Unicode encoded chars.|

The symbol column a string type data, has to specify to type BINARY(10).


## 3. Creating data table for each code name.

In Nasdaq stock data, the code means a abbr. of a company It used for identify stock data belongs which company. We need to create spcific data table for each company and use tb_nasdaq as reference. For example:

```sql
CREATE TABLE LANC USING tb_nasdaq TAGS ("LANC");
```
To import data into table, it needed to create a batch file that contains thoudands of SQL commands to create data table for each code name.

The file can be founded [HERE](https://dxact.blob.core.chinacloudapi.cn/21mfilms/create_table_tdengine.txt)


## 4. using insert into clause to import data.

TDEngine cannot su[]

```shell
insert into tb_nasdaq file '[code].csv'
```
The data file can be found at [HERE](https://dxact.blob.core.chinacloudapi.cn/21mfilms/tdengine_data.zip).

Notice:
***
```
The timestamp column 'date' must be at least accurate to the second.
The code column should be surrounded by double quote.
The sample data as below:
'2012-01-03 00:00:00',69.49,71.58,69.04,70.47,52.8236,107000,"LANC"
'2012-01-04 00:00:00',68.92,69.65,68.59,69.08,52.39031,67100,"LANC"
```
***