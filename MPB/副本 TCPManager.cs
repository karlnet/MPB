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

namespace MPB_
{
    public class TCPManager
    {
        internal Int32 numberOfAcceptedSockets;
        public static int LocalServerPort = 8080;
 
        public const Int32 maxNumberOfConnections = 3000;
        public const Int32 NumberOfSaeaForRecSend = maxNumberOfConnections + excessSaeaObjectsInPool;
     
        public const Int32 testBufferSize = 128;
        public const Int32 MaxAcceptOps = 10;

        public const Int32 backlog = 100;

        public const Int32 opsToPreAlloc = 2;    // 1 for receive, 1 for send

        public Int32 totalBytes = opsToPreAlloc * testBufferSize * NumberOfSaeaForRecSend;
        public Int32 totalBufferBytesInEachSaeaObject = opsToPreAlloc * testBufferSize;

        //allows excess SAEA objects in pool.
        public const Int32 excessSaeaObjectsInPool = 1;

        public const Int32 receivePrefixLength = 0;
        public const Int32 sendPrefixLength = 0;

        BufferManager theBufferManager;

        SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();
        // pool of reusable SocketAsyncEventArgs objects for accept operations
        SocketAsyncEventArgsPool poolOfAcceptEventArgs;
        // pool of reusable SocketAsyncEventArgs objects for receive and send socket operations
        SocketAsyncEventArgsPool poolOfRecSendEventArgs;

        Socket listenSocket;
        IPEndPoint localEndPoint;
        private MPBManager mpbManager;
        private MPBProtocol protocol;
        Semaphore theMaxConnectionsEnforcer;

        public TCPManager(MPBManager theMPBManager)
        {
            mpbManager = theMPBManager;
            protocol = new MPBProtocol(mpbManager);
            //ThreadPool.QueueUserWorkItem(LocalTcpServer);
            
            IPHostEntry ipHostEntry = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostEntry.AddressList[0];
            localEndPoint = new IPEndPoint(ipAddress, LocalServerPort);
          
            theBufferManager = new BufferManager(totalBytes, totalBufferBytesInEachSaeaObject);
            poolOfRecSendEventArgs = new SocketAsyncEventArgsPool(NumberOfSaeaForRecSend);
            poolOfAcceptEventArgs = new SocketAsyncEventArgsPool(MaxAcceptOps);
            theMaxConnectionsEnforcer = new Semaphore(maxNumberOfConnections, maxNumberOfConnections);
            Init();
            StartListen();
        }
        internal void Init()
        {
            this.theBufferManager.InitBuffer();

            SocketAsyncEventArgs acceptEventArg ; 
            for (Int32 i = 0; i < MaxAcceptOps; i++)
            {
                acceptEventArg = new SocketAsyncEventArgs();
                acceptEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(AcceptEventArg_Completed);
                poolOfAcceptEventArgs.Push(acceptEventArg);
            }

            SocketAsyncEventArgs eventArgObjectForPool;
            for (Int32 i = 0; i < NumberOfSaeaForRecSend; i++)
            {
                eventArgObjectForPool = new SocketAsyncEventArgs();
                theBufferManager.SetBuffer(eventArgObjectForPool);
                eventArgObjectForPool.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);   
                DataHoldingUserToken theTempReceiveSendUserToken = new DataHoldingUserToken(eventArgObjectForPool, eventArgObjectForPool.Offset, eventArgObjectForPool.Offset + testBufferSize, receivePrefixLength, sendPrefixLength);
                theTempReceiveSendUserToken.CreateNewDataHolder();
                eventArgObjectForPool.UserToken = theTempReceiveSendUserToken;
                this.poolOfRecSendEventArgs.Push(eventArgObjectForPool);
            }
        }
        internal void StartListen()
        {
            listenSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            listenSocket.Bind(localEndPoint);
            listenSocket.Listen(backlog);
            StartAccept();
        }
        internal void StartAccept()
        {
            SocketAsyncEventArgs acceptEventArg = null;
      
            if (this.poolOfAcceptEventArgs.Count > 1)
            {
                try
                {
                    acceptEventArg = this.poolOfAcceptEventArgs.Pop();
                }
                catch
                {
                    //acceptEventArg = CreateNewSaeaForAccept(poolOfAcceptEventArgs);
                }
            }
            else
            {
                //acceptEventArg = CreateNewSaeaForAccept(poolOfAcceptEventArgs);
            }
    
            this.theMaxConnectionsEnforcer.WaitOne();
     
            bool willRaiseEvent = listenSocket.AcceptAsync(acceptEventArg);

            if (!willRaiseEvent)
            {  
                ProcessAccept(acceptEventArg);
            }

        }
        private void AcceptEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
          
            ProcessAccept(e);
        }
        private   void ProcessAccept(SocketAsyncEventArgs acceptEventArgs)
        {
            if (acceptEventArgs.SocketError != SocketError.Success)
            {
                StartAccept();
                HandleBadAccept(acceptEventArgs);
                return;
            }
            StartAccept();

           

            SocketAsyncEventArgs receiveEventArgs = this.poolOfRecSendEventArgs.Pop();
            receiveEventArgs.AcceptSocket = acceptEventArgs.AcceptSocket;
            StartReceive(receiveEventArgs);
            /*
            receiveEventArgs.AcceptSocket.IOControl(0x0008, BitConverter.GetBytes(1), null);
            receiveEventArgs.AcceptSocket.IOControl(0x03, BitConverter.GetBytes(8), null);
            receiveEventArgs.AcceptSocket.IOControl(0x04, BitConverter.GetBytes(3), null);
            receiveEventArgs.AcceptSocket.IOControl(0x05, BitConverter.GetBytes(5), null);
            */
            acceptEventArgs.AcceptSocket = null;
            this.poolOfAcceptEventArgs.Push(acceptEventArgs);

            ((DataHoldingUserToken)receiveEventArgs.UserToken).IsMPBExit.WaitOne();

            IPEndPoint enp = (IPEndPoint)receiveEventArgs.AcceptSocket.RemoteEndPoint;
            string ip = enp.Address.ToString();
            //List<Socket> socketList = new List<Socket>();
            MyMBP mm; 
            
            SocketAsyncEventArgs SendEventArgs =  this.poolOfRecSendEventArgs.Pop();

            byte[] CmdMsg;
         
            DataHoldingUserToken receiveSendToken;

            while (true)
            {
                try
                {
                    MPBManager.MPBs.TryGetValue(ip, out mm);
                    if (mm != null)
                    {
                        CmdMsg = mm.MPBMsgQueue.Take();

                        if (CmdMsg != null)
                        {
                            SendEventArgs.AcceptSocket = mm.ReceiveEventArgs.AcceptSocket;
                            receiveSendToken = (DataHoldingUserToken)SendEventArgs.UserToken;
                            receiveSendToken.dataToSend = CmdMsg;
                            receiveSendToken.sendBytesRemainingCount = CmdMsg.Length;
                            receiveSendToken.bytesSentAlreadyCount = 0;
                            //socketList.Clear();
                            //socketList.Add(SendEventArgs.AcceptSocket);

                            //Socket.Select(null, socketList, null, -1);
                            //if (socketList.Contains(SendEventArgs.AcceptSocket))               
                            //    if (SendEventArgs.AcceptSocket != null)
                            StartSend(SendEventArgs);
                        }
                    }
                }
                catch (Exception e)
                {
                    //receiveSendEventArgs.AcceptSocket.Close();
                    break;
                }

            }
        }
        void IO_Completed(object sender, SocketAsyncEventArgs e)
        {
            DataHoldingUserToken receiveSendToken = (DataHoldingUserToken)e.UserToken;
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessReceive(e);
                    break;
                case SocketAsyncOperation.Send:
                    ProcessSend(e);
                    break;
                default:
                    throw new ArgumentException("The last operation completed on the socket was not a receive or send");
            }
        }
        private void HandleBadAccept(SocketAsyncEventArgs acceptEventArgs)
        {
            var acceptOpToken = (acceptEventArgs.UserToken as AcceptOpUserToken);
            acceptEventArgs.AcceptSocket.Close();
            poolOfAcceptEventArgs.Push(acceptEventArgs);
        }
        private  void StartReceive(SocketAsyncEventArgs receiveSendEventArgs)
        {
            DataHoldingUserToken receiveSendToken = (DataHoldingUserToken)receiveSendEventArgs.UserToken;   
            receiveSendEventArgs.SetBuffer(receiveSendToken.bufferOffsetReceive,testBufferSize);
            bool willRaiseEvent =  receiveSendEventArgs.AcceptSocket.ReceiveAsync(receiveSendEventArgs);

            if (!willRaiseEvent)
            {
                ProcessReceive(receiveSendEventArgs);
            }
        }
        private void ProcessReceive(SocketAsyncEventArgs receiveSendEventArgs)
        {
            DataHoldingUserToken receiveSendToken = (DataHoldingUserToken)receiveSendEventArgs.UserToken;
     
            if (receiveSendEventArgs.SocketError != SocketError.Success)
            {
                receiveSendToken.Reset();
                CloseClientSocket(receiveSendEventArgs);
                return;
            }
            if (receiveSendEventArgs.BytesTransferred == 0)
            {
                receiveSendToken.Reset();
                CloseClientSocket(receiveSendEventArgs);
                return;
            }

            Int32 remainingBytesToProcess = receiveSendEventArgs.BytesTransferred;
            protocol.ProtocolProcess(receiveSendEventArgs, receiveSendToken, remainingBytesToProcess);

            StartReceive(receiveSendEventArgs);
           
        }
        private void CloseClientSocket(SocketAsyncEventArgs e)
        {
            var receiveSendToken = (e.UserToken as DataHoldingUserToken);

            try
            {
                e.AcceptSocket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception)
            {
            }
            e.AcceptSocket.Close();

            if (receiveSendToken.theDataHolder.dataMessageReceived != null)
            {
                receiveSendToken.CreateNewDataHolder();
            }
            this.poolOfRecSendEventArgs.Push(e);
            Interlocked.Decrement(ref this.numberOfAcceptedSockets);
            this.theMaxConnectionsEnforcer.Release();
        }
        private void StartSend(SocketAsyncEventArgs receiveSendEventArgs)
        {
            DataHoldingUserToken receiveSendToken = (DataHoldingUserToken)receiveSendEventArgs.UserToken;
            if (receiveSendToken.sendBytesRemainingCount <= testBufferSize)
            {
                receiveSendEventArgs.SetBuffer(receiveSendToken.bufferOffsetSend, receiveSendToken.sendBytesRemainingCount);
                Buffer.BlockCopy(receiveSendToken.dataToSend, receiveSendToken.bytesSentAlreadyCount, receiveSendEventArgs.Buffer, receiveSendToken.bufferOffsetSend, receiveSendToken.sendBytesRemainingCount);
            }
            else
            {
                receiveSendEventArgs.SetBuffer(receiveSendToken.bufferOffsetSend, testBufferSize);
                Buffer.BlockCopy(receiveSendToken.dataToSend, receiveSendToken.bytesSentAlreadyCount, receiveSendEventArgs.Buffer, receiveSendToken.bufferOffsetSend, testBufferSize);
            }

            bool willRaiseEvent = receiveSendEventArgs.AcceptSocket.SendAsync(receiveSendEventArgs);

            if (!willRaiseEvent)
            {
                ProcessSend(receiveSendEventArgs);
            }
        }
        private void ProcessSend(SocketAsyncEventArgs receiveSendEventArgs)
        {
            DataHoldingUserToken receiveSendToken = (DataHoldingUserToken)receiveSendEventArgs.UserToken;         
            if (receiveSendEventArgs.SocketError == SocketError.Success)
            {
                receiveSendToken.sendBytesRemainingCount = receiveSendToken.sendBytesRemainingCount - receiveSendEventArgs.BytesTransferred;

                if (receiveSendToken.sendBytesRemainingCount == 0)
                {
                    // If we are within this if-statement, then all the bytes in
                    // the message have been sent. 
                    //StartReceive(receiveSendEventArgs);
                    //receiveSendEventArgs.AcceptSocket = null;
                    //receiveSendEventArgs.AcceptSocket.Close();
                    //this.poolOfRecSendEventArgs.Push(receiveSendEventArgs);
                }
                else
                {        
                    receiveSendToken.bytesSentAlreadyCount += receiveSendEventArgs.BytesTransferred;                   
                    StartSend(receiveSendEventArgs);
                }
            }
            else
            {
                receiveSendToken.Reset();
                CloseClientSocket(receiveSendEventArgs);
            }
        }
        //private void LocalTcpServer(Object state)
        //{
        //    IPHostEntry ipHostEntry = Dns.Resolve(Dns.GetHostName());
        //    IPAddress ipAddress = ipHostEntry.AddressList[0];
        //    Socket socket0 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //    socket0.Bind(new IPEndPoint(ipAddress, MyMBP.LocalServerPort));
        //    socket0.Listen(1024);
        //    while (socket0.Poll(-1, SelectMode.SelectRead))
        //    {
        //        //TcpListener tcpListener = new TcpListener(MyMBP.LocalServerPort);
        //        //tcpListener.Start();
        //        //while (true)
        //        //{
        //        //TcpClient client = tcpListener.AcceptTcpClient();

        //        ThreadPool.QueueUserWorkItem(RemoteMPBProcess, socket0.Accept());
        //    }
        //}
        //private void RemoteMPBProcess(Object state)
        //{
        //    //err = check_sum(inBuf+idx+HA_CMD_HEAD_SIZE, cmdLen);
        //    //err = check_sum(inBuf + idx, cmdLen);
        //    //require_noerr(err, exit);

        //    //TcpClient tcpClient = state as TcpClient;
        //    Socket remoteSocket = (Socket)state;
        //    //ArrayList socketList = new ArrayList();
        //    List<Socket> socketList = new List<Socket>();
        //    //NetworkStream ns = tcpClient.GetStream();
        //    //NamedPipeServerStream p = new NamedPipeServerStream();
        //    Byte[] inBuf = new Byte[ReceiveBufferSize];
        //    int inBufLen = 0;
        //    //string theMPBIP = string.Empty;
        //    //MyMBP m;
        //    //if(tcpClient.Connected)
        //    //    tcpClient.Client.IOControl(IOControlCode.KeepAliveValues, BitConverter.GetBytes(30), null);

        //    while (true)
        //    {


        //        socketList.Clear();
        //        socketList.Add(remoteSocket);

        //        Socket.Select(socketList, null, null, -1);
        //        if (socketList.Contains(remoteSocket))
        //        {
        //            //inBufLen = ns.Read(inBuf, 0, ReceiveBufferSize);
        //            inBufLen = remoteSocket.Receive(inBuf);
        //            if (inBufLen > 0)
        //            {
        //                MPBProtocol protocol = new MPBProtocol(mpbManager);
        //                protocol.ProtocolProcess(remoteSocket, inBuf, inBufLen);

        //            }
        //        }
        //    }

        //}


    }
}
