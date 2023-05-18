using System;
using System.Runtime.InteropServices;
using System.Text;
using DotNetty.Buffers;
using DotNetty.Common.Utilities;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using UnityEngine;

namespace UnityTemplateProjects.DBServer.NetWork
{
    public class DotNettyHeartBeatHandler : ChannelDuplexHandler
    {
        private static readonly IByteBuffer HEARTBEAT_SEQUENCE =
            Unpooled.UnreleasableBuffer(Unpooled.CopiedBuffer("heartbeat", Encoding.UTF8));
        // Encoding.UTF8.GetBytes("heartbeat");

        private long lastHeartbeatTimestamp = 0;

        // public override void ChannelActive(IChannelHandlerContext context)
        // {
        //     // 连接建立时发送第一个心跳包
        //     SendHeartbeat(context);
        // }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            if (message.Equals("heartbeat"))
            {
                // 处理心跳响应包
                HandleHeartbeatResponse(context);
                SendHeartbeat(context);
                ReferenceCountUtil.Release(message);
                return;
            }
            base.ChannelRead(context,message);
        }

        // 发送心跳包
        private void SendHeartbeat(IChannelHandlerContext context)
        {
            if (context.Channel.Active)
            {
                // Debug.Log("Send Heartbeat!");
                context.WriteAndFlushAsync(HEARTBEAT_SEQUENCE.Duplicate());
            }
        }

        // 处理心跳响应包
        private void HandleHeartbeatResponse(IChannelHandlerContext context)
        {
            long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            currentTimestamp = lastHeartbeatTimestamp;
            // Debug.LogFormat("Received heartbeat response from server {0}, timestamp: {1}",
            //     context.Channel.RemoteAddress, currentTimestamp);
        }

        // 发送心跳包，判断服务器是否在线
        public override void UserEventTriggered(IChannelHandlerContext context, object evt)
        {
            if (evt is IdleStateEvent idleStateEvent)
            {
                // if (idleStateEvent.State == IdleState.WriterIdle)
                // {
                //     // 写空闲事件，发送心跳包
                //     SendHeartbeat(context);
                // }
                if (idleStateEvent.State == IdleState.ReaderIdle)
                {
                    // 读空闲事件，判断服务器是否在线
                    long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    if (currentTimestamp - lastHeartbeatTimestamp > 30)
                    {
                        Debug.LogFormat("Server {0} is offline", context.Channel.RemoteAddress);
                        context.CloseAsync();
                        // TODO: 服务器断线处理
                    }
                }
            }
        }
    }
}