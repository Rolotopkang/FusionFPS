using System;
using System.Net;
using System.Text;
using DefaultNamespace;
using DotNetty.Buffers;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
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

        public async void Connect()
        {
            group = new MultithreadEventLoopGroup();

            try
            {
                var bootstrap = new Bootstrap()
                    .Group(group)
                    .Channel<TcpSocketChannel>()
                    .Option(ChannelOption.TcpNodelay, true)
                    .Handler(new DotNettyClientInitializer(this));


                channel = await bootstrap.ConnectAsync
                    (new IPEndPoint(
                        IPAddress.Parse(HostIP), HostPort));
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
                
                //TODO  弹窗错误
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