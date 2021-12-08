using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PainTrainStation
{
    public static class PainTrainStation
    {
        private static int delayTime = 259200;
        public static long groupID = 0;
        public static void Enter()
        {
            while (true)
            {
                processUpdates();
                Thread.Sleep(50);
            }
        }

        static long lastUpdate = 0;
        public static void processUpdates()
        {
            var up = Telegram.getUpdates(lastUpdate);
            if (up == null)
            {
                Console.WriteLine("TGAPI Response failure update==null");
                return;
            }
            Console.WriteLine("Updates: {0}", up.Length);

            for (int i = 0; i < up.Length; i++)
            {
                var currentUpdate = up[i];
                if (currentUpdate.update_id >= lastUpdate)
                {
                    lastUpdate = currentUpdate.update_id + 1;
                }
                if (currentUpdate.edited_message != null)
                {
                    currentUpdate.message = currentUpdate.edited_message; // ahax.
                }
                //Console.WriteLine(JsonConvert.SerializeObject(currentUpdate));
                {
                    if (currentUpdate.message != null)
                    {
                        processIndividualUpdate(currentUpdate);
                    }
                }
            }
        }
        
   


        public static void processIndividualUpdate(TGUpdate update)
        {
            var msg = update.message;
            if (msg.from.is_bot)
                return;
            if (msg.from == null)
                return;
            if (msg.from.id == 777000)
                msg.deleteAsync();
        }
    }
}

