using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.IO.Pipes;


namespace MPB
{
    public class TCPManager
    {

        public static int LocalServerPort = 8080;
          TcpListener server = null;
 
        private MPBManager mpbManager;
        private MPBProtocol protocol;

        private const int MPBTcpBufferSizeMax=1024*20;
        byte[] MPBTcpBuffer = new byte[MPBTcpBufferSizeMax];

        
        public TCPManager(MPBManager theMPBManager)
        {
            mpbManager = theMPBManager;
            protocol = new MPBProtocol(mpbManager);

            //IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            IPHostEntry ipHostEntry = Dns.Resolve(Dns.GetHostName());
            IPAddress localAddr = ipHostEntry.AddressList[0];
            server = new TcpListener(localAddr, LocalServerPort);
            server.Start();

            Task.Run(() => RemoteMPBProcess());

        }
        private async Task  RemoteMPBProcess()
        {
            TcpClient tcpClient = await server.AcceptTcpClientAsync();
            tcpClient.ReceiveBufferSize = 20 * 1024;
            Task.Run(() => RemoteMPBProcess());
            NetworkStream ns = tcpClient.GetStream();
            AutoResetEvent IsMPBExit = new AutoResetEvent(false);
            MyMBP mm;
            byte[] CmdMsg;
            string ip = ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.ToString();

            Task.Run(async () =>
            {
                int i = 0;
                bool flag = false;
                while (true)
                {
                    i = await ns.ReadAsync(MPBTcpBuffer, 0, MPBTcpBufferSizeMax);
                    protocol.ProtocolProcess(tcpClient, MPBTcpBuffer, i);
                    if (!flag)
                    {
                        IsMPBExit.Set();
                        flag = true;
                    }
                }
            });

            IsMPBExit.WaitOne();

            while (true)
            {
                MPBManager.MPBs.TryGetValue(ip, out mm);
                if (mm != null)
                {
                    CmdMsg = mm.MPBMsgQueue.Take();
                    if (CmdMsg != null)
                        await ns.WriteAsync(CmdMsg, 0, CmdMsg.Length);
                }
            }
        }
    }
}
