using System.Text;
using Newtonsoft.Json;

namespace UnityTemplateProjects.DBServer.NetMsg
{
    public class NetMessage
    {
        public int code;
        public int status;

        public byte[] ToByteMsg()
        {
            string json = JsonConvert.SerializeObject(this);
            byte[] byteArrar = Encoding.UTF8.GetBytes(json);
            return byteArrar;
        }

        public string ToString()
        {
            string json = JsonConvert.SerializeObject(this);
            return json;
        }
    }
}