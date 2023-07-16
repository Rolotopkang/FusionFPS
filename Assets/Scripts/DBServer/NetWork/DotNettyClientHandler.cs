using System;
using DotNetty.Transport.Channels;
using InfimaGames.LowPolyShooterPack.Interface;
using Photon.Pun;
using UnityEngine;

namespace UnityTemplateProjects.DBServer.NetWork
{
    public class DotNettyClientHandler : SimpleChannelInboundHandler<string>
    {
        private readonly DotNettyClient client;

        public DotNettyClientHandler(DotNettyClient client)
        {
            this.client = client;
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, string msg)
        {
            Debug.Log("Received message: " + msg);
            UnityMainThreadDispatcher.GetInstance().Enqueue((() =>
                    {
                        MessageDistributer.GetInstance().Dispatch(msg);
                    }
                    ));
        }

        public override void ChannelRegistered(IChannelHandlerContext context)
        {
            Debug.Log("通道"+context.Name+"注册完毕");
        }

        public override void ChannelUnregistered(IChannelHandlerContext context)
        {
            Debug.Log("通道"+context.Name+"注销完毕");
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            Debug.Log("通道"+context.Name+"激活:"+context.Channel.RemoteAddress);
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            Debug.Log("通道"+context.Name+"关闭:"+context.Channel.RemoteAddress);
            Debug.LogFormat("Server {0} is offline", context.Channel.RemoteAddress);
            context.CloseAsync();
            UnityMainThreadDispatcher.GetInstance().Enqueue((() =>
                    {
                        if (PhotonNetwork.InRoom)
                        {
                            In_Game_SettingsMenu.GetInstance().Quit();
                        }
                        UI_Error.GetInstance().OpenErrorUI("服务器连接异常断开","退出", () =>
                        {
                            UI_Error.GetInstance().Exit();
                        });
                    }
                ));
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            UnityMainThreadDispatcher.GetInstance().Enqueue(() =>
                    {
                        if (PhotonNetwork.InRoom)
                        {
                            In_Game_SettingsMenu.GetInstance().Quit();
                        }
                        UI_Error.GetInstance().OpenErrorUI("服务器已下线","退出", () =>
                        {
                            UI_Error.GetInstance().Exit();
                        });
                    }
                );
            Debug.LogError(exception.Message);
            context.CloseAsync();
        }
    }
}