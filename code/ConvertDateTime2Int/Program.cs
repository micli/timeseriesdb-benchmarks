using System;

namespace ConvertDateTime2Int
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please input DateTime value:");
            string strVal = Console.ReadLine();
            DateTime dt = DateTime.MinValue;
            if (DateTime.TryParse(strVal, out dt))
            {
                int ret = DateTime2Int(dt);
                Console.WriteLine("Value = {0}", ret);
            }
            else
            {
                Console.WriteLine("Invalid DateTime format.");
            }
        }

        private static int DateTime2Int(System.DateTime time)
        {
            DateTime start = DateTime.Parse("1970-01-01").ToLocalTime();
            return (int)(time - start).TotalSeconds;
        }
    }
}
