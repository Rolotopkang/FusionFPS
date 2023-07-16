using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DefaultNamespace;
using DotNetty.Buffers;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using InfimaGames.LowPolyShooterPack;
using UnityEngine;
using UnityEngine.Events;
using UnityTemplateProjects.DBServer.NetMsg;

namespace UnityTemplateProjects.DBServer.NetWork
{
    public class DotNettyClient : Singleton<DotNettyClient>
    {
        [SerializeField] private string HostIP = "127.0.0.1";
        [SerializeField] private int HostPort = 16888;
        
        private IChannel channel;
        private MultithreadEventLoopGroup group;
        private Bootstrap currentBootstrap;

        public async void Connect()
        {
            try
            {
                // Debug.Log(@group== null);
                // Debug.Log(channel == null);
                // Debug.Log(currentBootstrap == null);

                group ??= new MultithreadEventLoopGroup();
                currentBootstrap ??= new Bootstrap()
                    .Group(@group)
                    .Channel<TcpSocketChannel>()
                    .Option(ChannelOption.TcpNodelay, true)
                    .Handler(new DotNettyClientInitializer(this));
                channel = await currentBootstrap.ConnectAsync
                    (new IPEndPoint(
                        IPAddress.Parse(HostIP), HostPort));
                Debug.Log("连接服务器成功");
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
                Close();
                UI_Error.GetInstance().OpenErrorUI("无法连接至数据库服务器","退出", () =>
                {
                    UI_Error.GetInstance().Exit();
                    // UI_Error.GetInstance().CloseUI();
                    // Connect();
                });
            }
        }

        private void Close()
        {
            if (channel != null)
            {
                Debug.Log("关通道！");
                channel.CloseAsync();
                channel = null;
            }

            if (group != null)
            {
                group.ShutdownGracefullyAsync();
                group = null;
            }
        }

        public void SendData(NetMessage netMessage)
        {
            if (channel != null && channel.Active)
            {
                Debug.Log("发送" + netMessage.ToString());
                channel.WriteAndFlushAsync(netMessage.ToString()).ContinueWith(task =>
                {
                    // 异步发送完成后的回调函数
                    if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
                    {
                        
                    }
                    else
                    {
                        Debug.LogError("Failed to send login request: " + task.Exception);
                    }
                });
            }
        }

        public void SendData(NetMessage netMessage , Action responseAction)
        {
            if (channel != null && channel.Active)
            {
                Debug.Log("发送" + netMessage.ToString());
                channel.WriteAndFlushAsync(netMessage.ToString()).ContinueWith(task =>
                {
                    // 异步发送完成后的回调函数
                    if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
                    {
                        responseAction.Invoke();
                    }
                    else
                    {
                        Debug.LogError("Failed to send login request: " + task.Exception);
                    }
                });
            }
            else
            {
                Debug.Log("通道未完毕");
                
            }
        }

        public async void Disconnect()
        {
            if (channel != null)
            {
                await channel.CloseAsync();
            }

            if (group != null)
            {
                await group.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));
            }
        }

        private void OnDestroy()
        {
            Disconnect();
        }

        public bool isConnected() => channel is { Active: true };
    }
}