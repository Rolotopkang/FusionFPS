using System;
using DefaultNamespace;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityTemplateProjects.DBServer.NetMsg;
using UnityTemplateProjects.Login;

namespace UnityTemplateProjects.DBServer.NetWork
{
    public class MessageDistributer: SingletonNoMono<MessageDistributer>
    {

        public Action<LoginStatus> LoginResponse;
        public Action<RegisterStatus> RegisterResponse;
        public Action<DefaultStatus> DefaultResponse;
        public Action<EconomicMsg> EconomicResponse;


        public void Dispatch(string json)
        {
            NetMessage reqMsg = ToObj<NetMessage>(json);
            switch ((NetCode)reqMsg.code)
            {
                case NetCode.None:
                    break;
                case NetCode.UserLogin:
                    LoginResponse.Invoke((LoginStatus)reqMsg.status);
                    break;
                case NetCode.UserRegister:
                    RegisterResponse.Invoke((RegisterStatus)reqMsg.status);
                    break;
                case NetCode.UserMoneyGet:
                case NetCode.VersionControl:
                    EconomicMsg economicMsg = ToObj<EconomicMsg>(json);
                    EconomicResponse.Invoke(economicMsg);
                    break;
                case NetCode.UserMoneyAdd:
                    DefaultResponse.Invoke((DefaultStatus)reqMsg.status);
                    break;

            }
        }


        public static T ToObj<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}