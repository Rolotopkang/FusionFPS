using System;
using System.Text;
using DotNetty.Codecs;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;

namespace UnityTemplateProjects.DBServer.NetWork
{
    public class DotNettyClientInitializer : ChannelInitializer<ISocketChannel>
    {
        private readonly DotNettyClient _client;
        private static int TimeOutSec = 15;
        public DotNettyClientInitializer(DotNettyClient dotNettyClient)
        {
            _client = dotNettyClient;
        }
        
        protected override void InitChannel(ISocketChannel channel)
        {
            var pipeline = channel.Pipeline;

            pipeline.AddLast(new LengthFieldBasedFrameDecoder(1024, 0, 4, 0, 4));
            pipeline.AddLast(new LengthFieldPrepender(4));
            pipeline.AddLast(new StringEncoder(Encoding.UTF8));
            pipeline.AddLast(new StringDecoder(Encoding.UTF8));

            // pipeline.AddLast(new IdleStateHandler(TimeOutSec, 0, 0));

            pipeline.AddLast(new IdleStateHandler(TimeOutSec, 0, TimeOutSec * 2));
            pipeline.AddLast(new DotNettyHeartBeatHandler());
            pipeline.AddLast(new DotNettyClientHandler(_client));
        }
    }
}