using System;
using System.Threading;
using RMES.Util;

namespace RMES.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                var time = DateTime.Now;
                Console.WriteLine($"{DateTimeHelper.ConvertToLong(time)} = {time:yyyy-MM-dd HH:mm:ss ffff}");
                Thread.Sleep(1000);
            }

            var timestamp = 1567868724201;
            var dt = DateTimeHelper.ConvertToDateTime(timestamp);
            Console.WriteLine(dt.ToString("yyyy-MM-dd HH:mm:ss "));

            Console.Read();
        }
    }
}
