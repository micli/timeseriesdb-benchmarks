# Query Performance


## Table scan


DolphinDB:

```sql
timer select count(*) from tb_nasdaq;
```
time spent: 0.528 ms

InfluxDB:

```sql
select count(*) from tb_nasdaq;
```
report error **ERR: no data received**
During Calaulation, CPU nearly 100%, memory nearly 100%.

TDEngine:

```sql
select count(*) from tb_nasdaq;
```
time spent: 4.928847s


## Query Min/Max valud and group by code



DolphinDB:

```sql
timer select min(opening_price) from tb_nasdaq group by code;
```
time spent: 43.113 ms

InfluxDB:

```sql
select min(opening_price) from tb_nasdaq group by code;
```
time spent: 44.37s

TDEngine:

```sql
select min(opening_price) from tb_nasdaq group by code;
```
time spent: 7.648549s


## Quer by symbol code


DolphinDB:

```sql
timer select sum(trade_volume) from tb_nasdaq where code='AAPL';
```

time spent: 6.847 ms

InfluxDB:

```sql
select sum(trade_volume) from tb_nasdaq where code='AAPL';
```

time spent: 0.41s

TDEngine:

```sql
select sum(trade_volume) from tb_nasdaq where code='AAPL';
```

time spent: 0.068869s


## Query by symbol code and time

DolphinDB:

```sql
timer select count(*) from tb_nasdaq where code='AAPL' and date < 2017.01.01;
```

Time spent: 4.681 ms

InfluxDB:

```sql
select count(*) from tb_nasdaq where code='AAPL' and time < 1478188800000000000;
```
Time spent: 0.7s

TDEngine:

```sql
select count(*) from tb_nasdaq where code='AAPL' and date < '2017-01-01 00:00:00';
```
Time spent: 0.037594s
