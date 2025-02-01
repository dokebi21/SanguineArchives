using Bloody.Core.API.v1;
using SanguineArchives.Common.BloodyNotify.DB;
using ProjectM;

namespace SanguineArchives.Common.BloodyNotify.AutoAnnouncer
{
    public  class AutoAnnouncerFunction
    {
        private static int __indexMessage;

        public static void OnTimedAutoAnnouncer()
        {
            var messages = Database.getAutoAnnouncerMessages();

            if (messages.Count > 0)
            {
                if (messages.Count == 1)
                {
                    foreach (var line in messages[0].MessageLines)
                    {
                        ServerChatUtils.SendSystemMessageToAllClients(Plugin.SystemsCore.EntityManager, line);
                    }
                }
                else
                {
                    foreach (var line in messages[__indexMessage].MessageLines)
                    {
                        ServerChatUtils.SendSystemMessageToAllClients(Plugin.SystemsCore.EntityManager, line);
                    }

                    __indexMessage++;

                    if (__indexMessage >= messages.Count)
                    {
                        __indexMessage = 0;
                    }
                }
            }
        }

        public static void StartAutoAnnouncer()
        {
            static void AutoAnnouncerAction()
            {
                if (Database.EnabledFeatures[NotifyFeature.auto])
                {
                    OnTimedAutoAnnouncer();
                }
            }

            CoroutineHandler.StartRepeatingCoroutine(AutoAnnouncerAction, Database.getIntervalAutoAnnouncer());
        }

    }
}
