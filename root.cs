using System;
using System.Threading;
using System.Threading.Tasks;
using System.Data;

namespace PainTrainStation
{
    class root
    {
        private const string tag = "PainTrainStation@boot";
        public static string botUsername;
        public static string botName;
        static void Main(string[] args)
        {
            Console.WriteLine("PainTrainStation- 12-8-2021");
            // Param check 
            if (args.Length > 0)
            {
                var ptg = "PainTrainStation@preboot";
                Console.WriteLine("parameter check.....");
            }
            Helpers.writeOut(tag, "Initializing configuration.");
            Config.init("config.ini");
            Telegram.SetAPIKey(Config.getValue("TGAPIKey"));
            {
                var tries = 0;
                var me = Telegram.getMe(); // Synchronous call for result.
                while (me == null)
                {
                    tries++;
                    Thread.Sleep(1200);
                    Helpers.warn("Failed. Trying again");
                    me = Telegram.getMe();
                    if (tries > 3)
                    {
                        Helpers.warn("Invalid telegram API key or cannot connect to tgapi.");
                        Environment.Exit(-1);
                    }
                }
                botUsername = me.username; // set bot username
                botName = me.first_name; // set bot name
                Console.WriteLine($"Hello, I'm {botName} under the handle {botUsername}");
            }


            PainTrainStation.Enter();
        }
    }
}
