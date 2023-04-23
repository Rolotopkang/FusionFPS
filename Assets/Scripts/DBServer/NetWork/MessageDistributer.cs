using Newtonsoft.Json;
using UnityEngine.Events;
using UnityTemplateProjects.DBServer.NetMsg;

namespace UnityTemplateProjects.DBServer.NetWork
{
    public class MessageDistributer: Singleton<MessageDistributer>
    {
        public UnityAction<string> MsgUpdate;

        public void Dispatch(string json)
        {
            NetMessage reqMsg = ToObj<NetMessage>(json);
            switch ((NetCode)reqMsg.code)
            {
                case NetCode.None:
                    break;
                case NetCode.UserLogin:
                    break;
                case NetCode.UserRegister:
                    break;
            }
        }


        public static T ToObj<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}