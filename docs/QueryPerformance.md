# Query Performance


## Table scan


DolphinDB:

```sql

timer select count(*) from tb_nasdaq;

```
time spent: 0.528 ms

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

TDEngine:

```sql

select min(opening_price) from tb_nasdaq group by code;

```
time spent: 7.648549s


## Quer by time conditions


DolphinDB:

```sql

timer select sum(trade_volume) from tb_nasdaq where code='AAPL';

```

time spent: 6.847 ms


TDEngine:

```sql

select sum(trade_volume) from tb_nasdaq where code='AAPL';

```

time spent: 0.068869s


## Query by Time Sliding

DolphinDB:

```sql

```

TDEngine:

```sql

```