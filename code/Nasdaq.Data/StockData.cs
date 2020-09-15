using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Nasdaq.Data
{
    public class StockData
    {
        public string date { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public float opening_price { get; set; }
        public float highest_price { get; set; }
        public float lowest_price { get; set; }
        public float closing_price { get; set; }
        public float adjusted_closing_price { get; set; }
        public float trade_volume { get; set; }


        public string ToString(string dbType)
        {
            switch(dbType.ToLower())
            {
                case "influxdb":
                    return string.Format("code=\"{0}\",opening_price=\"{1}\",highest_price=\"{2}\",lowest_price=\"{3}\",closing_price=\"{4}\",adjusted_closing_price=\"{5}\",trade_volume=\"{6}\" {7}",
                code, opening_price, highest_price, lowest_price, closing_price, adjusted_closing_price, trade_volume, DateTime2Int(DateTime.Parse(date)));
                // return string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", code, opening_price, highest_price, lowest_price, closing_price, adjusted_closing_price, trade_volume, DateTime2Int(DateTime.Parse(date)));
                case "tdengine":
                    return string.Format("\'{0} 00:00:00.000\',{2},{3},{4},{5},{6},{7},\"{1}\"",
                                date, code, opening_price, highest_price, lowest_price,
                                closing_price, adjusted_closing_price, trade_volume);
                case "dolphindb":
                    return string.Format("{0} 00:00:00.000,{1},{2},{3},{4},{5},{6},{7}",
                                date.Replace("-", "."), code, opening_price, highest_price, lowest_price,
                                closing_price, adjusted_closing_price, trade_volume);
                default:
                    return string.Empty;
            }
        }


        private static int DateTime2Int(System.DateTime time)
        {
            DateTime start = DateTime.Parse("1970-01-01").ToLocalTime();
            return (int)(time - start).TotalSeconds;
        }
    }

    public class Stock
    {
        public string JsonFilename { get; set; }

        public List<StockData> Data { get; private set; }

        public DateTime StartTime { get; private set; }

        public DateTime EndTime { get; private set; }

        public string Code { get; private set; }

        public string Name { get; private set; }

        private JObject jsonObj = null;


        public Stock(string jsonFilename)
        {
            JsonFilename = jsonFilename;
        }

        public void LoadData()
        {
            if (string.IsNullOrEmpty(JsonFilename) || !File.Exists(JsonFilename))
                return;
            using (StreamReader sr = new StreamReader(JsonFilename))
            {
                var content = sr.ReadToEnd();
                jsonObj = JObject.Parse(content);
            }
            if (null == jsonObj)
                return;
            StartTime = jsonObj.ContainsKey("oldest") ? DateTime.Parse(jsonObj["oldest"].ToString()) : DateTime.MinValue;
            EndTime = jsonObj.ContainsKey("latest") ? DateTime.Parse(jsonObj["latest"].ToString()) : DateTime.MinValue;

            if (StartTime != DateTime.MinValue && EndTime != DateTime.MinValue)
            {
                try
                {
                    Data = new List<StockData>(jsonObj.Count);
                    for (DateTime date = StartTime; date <= EndTime; date = date.AddDays(1))
                    {
                        var key = date.ToString("yyyy-MM-dd");
                        if (jsonObj.ContainsKey(key))
                        {
                            Data.Add(jsonObj[key].ToObject<StockData>());
                        }
                    }
                    if (Data.Count > 0)
                    {
                        Code = Data[0].code;
                        Name = Data[0].name;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

    }
}
