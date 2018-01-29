using System;
using PipServices.Settings.Container;

namespace run
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var task = (new SettingsProcess()).RunAsync(args);
                task.Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }
    }
}
