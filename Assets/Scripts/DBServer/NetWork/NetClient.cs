using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using InfimaGames.LowPolyShooterPack;
using UnityEngine;
using UnityTemplateProjects.DBServer.NetMsg;

namespace UnityTemplateProjects.DBServer.NetWork
{
    public class NetClient : Singleton<NetClient>
    {
        private IPEndPoint address;
        private Socket clientSocket;

        private MemoryStream receiveBuffer = new MemoryStream(64 * 1024);
        private MemoryStream stream = new MemoryStream(64 * 1024);
        private int readOffset = 0;

        public void Init(string serverIP, int port)
        {
            this.address = new IPEndPoint(IPAddress.Parse(serverIP), port);
        }

        public void Connect()
        {
            if (this.clientSocket != null)
            {
                this.clientSocket.Close();
            }

            if (this.address == default(IPEndPoint))
            {
                throw new Exception("连接数据库缺少初始化");
            }

            this.DoConnect();
        }

        private void DoConnect()
        {
            try
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientSocket.Blocking = true;

                Debug.Log("开始连接数据库服务器:" + address);
                IAsyncResult result = clientSocket.BeginConnect(address, null, null);
                bool success = result.AsyncWaitHandle.WaitOne(1000);
                if (success)
                {
                    clientSocket.EndConnect(result);
                }
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode == SocketError.ConnectionRefused)
                {
                    CloseConnection();
                }
                Debug.Log("数据库连接错误" + e);
            }
            catch (Exception e)
            {
                Debug.Log("数据库连接错误" + e);
            }
        }

        public bool Connected => clientSocket is { Connected: true };

        public void CloseConnection()
        {
            if (clientSocket != null)
            {
                clientSocket.Close();
            }
            Debug.Log("关闭数据库连接");
        }

        private void Update()
        {
            ProcessReceive();
        }

        private bool ProcessReceive()
        {
            bool ret = false;
            try
            {
                if (clientSocket.Blocking)
                {
                    // Debug.Log("socket被阻断！");
                }

                bool error = clientSocket.Poll(0, SelectMode.SelectError);
                if (error)
                {
                    Debug.Log("Socket错误");
                    CloseConnection();
                    return false;
                }

                ret = clientSocket.Poll(0, SelectMode.SelectRead);
                if (ret)
                {
                    int n = this.clientSocket.Receive(receiveBuffer.GetBuffer(), 0, receiveBuffer.Capacity,
                        SocketFlags.Broadcast);
                    if (n <= 0)
                    {
                        CloseConnection();
                        return false;
                    }

                    ReceiveData(receiveBuffer.GetBuffer(), 0, n);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }

        public void ReceiveData(byte[] data, int offset, int count)
        {
            if (stream.Position + count > stream.Capacity)
            {
                throw new Exception("缓冲器满");
            }
            
            stream.Write(data,offset,count);
            ParsePackage();
        }

        private void ParsePackage()
        {
            if (readOffset + 4 < stream.Position)
            {
                int packageSize = BitConverter.ToInt32(stream.GetBuffer(), readOffset);
                if (packageSize + readOffset + 4 <= stream.Position)
                {
                    string message = UnpackMessage(stream.GetBuffer(),readOffset + 4,packageSize);
                    if (message == null)
                    {
                        throw new Exception("无法解包");
                    }

                    MessageDistributer.GetInstance().Dispatch(message);

                    readOffset += (packageSize + 4);
                    ParsePackage();
                }
            }
        }

        public void SendMessage(NetMessage msg)
        {
            if (!Connected)
            {
                Connect();
                if (!Connected)
                {
                    Debug.Log("与服务器断开连接");
                    CloseConnection();
                }
            }

            byte[] package = msg.ToByteMsg();
            clientSocket.Send(PackMessage(package));
        }

        private static byte[] PackMessage(byte[] msg)
        {
            byte[] package = new byte[msg.Length + 4];
            byte[] IntArr = IntToArr(msg.Length);
            IntArr.CopyTo(package,0);
            msg.CopyTo(package,IntArr.Length);
            return package;
        }

        private string UnpackMessage(byte[] packet, int offset, int length)
        {
            byte[] final = new byte[length];
            for (int i = 0; i < final.Length; i++)
            {
                final[i] = packet[i + offset];
            }

            return Encoding.UTF8.GetString(final);
        }

        static byte[] IntToArr(int num)
        {
            byte[] data = new byte[4];
            data[3] = (byte) (num & 0xff);
            data[2] = (byte) (num >> 8 & 0xff);
            data[1] = (byte) (num >> 16 & 0xff);
            data[0] = (byte) (num >> 24 & 0xff);
            return data;
        }
    }
}