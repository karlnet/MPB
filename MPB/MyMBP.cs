using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Concurrent;
namespace MPB
{

    [Flags]
    public enum MPBState : uint
    {
        Connected = 0x01,
        DisConnected = 0x02,
        KeepAlive = 0x04,
        
    }
    [Flags]
    public enum RelayOP : uint
    {
        ON = 0xCC,
        OFF = 0xDD
    }
    public class MyMBP
    {
       

        private static int RemoteServerPort = 8080;

        private static int no = 0;
        private const int FREQ = 30;

        public event EventHandler newMPBValue;       
        public event EventHandler UpdateMPBValue;
        public event EventHandler newRelayValue;
        public event EventHandler newTempValue;
        public event EventHandler newStateValue;
        public event EventHandler newImageValue;
        //public TimerCallback MPBDisConnect;
        
        private Timer DisConnectTimer { get; set; }
        public int ListViewNO { get; set; }
        private uint _relay;
        public uint Relay
        {
            get { return _relay; }
            set
            {
                if (_relay != value)
                {
                    _relay = value;
                    OnNewRelay();
                }
            }
        }
        private bool _isNewCameraImage;
        public bool IsNewCameraImage
        {
            get { return _isNewCameraImage; }
            set
            {
                if (_isNewCameraImage != value)
                {
                    _isNewCameraImage = value;
                    if (_isNewCameraImage) OnNewImage();
                }
            }
        }
        private string _mac;
        public string MAC
        {
            get { return _mac; }
            set
            {
                if (_mac != value)
                {
                    _mac = value;
                    OnNewTemp();
                }
            }
        }
        private uint _temp;
        public uint Temp
        {
            get { return _temp; }
            set
            {
                if (_temp != value)
                {
                    _temp = value;
                    OnNewTemp();
                }
            }
        }
        private uint _mpbID;
        public uint MPBID
        {
            get { return _mpbID; }
            set
            {
                if (_mpbID != value)
                {
                    _mpbID = value;
                    if (_mpbID > 0) CurrentMPBState = MPBState.Connected;
                }
            }
        }
        private MPBState _currentMPBState;
        public MPBState CurrentMPBState
        {
            get { return _currentMPBState; }
            set
            {
                if (_currentMPBState != value)
                {
                    if (_currentMPBState == MPBState.DisConnected)
                    {
                        //((DataHoldingUserToken)ReceiveEventArgs.UserToken).IsMPBExit.Set();
                        MPBMsgQueue = new BlockingCollection<byte[]>();
                    }
                    _currentMPBState = value;
                    OnNewState();
                    //if (_currentMPBState == MPBState.Connected)
                    //    GetMPBState();
                }
            }
        }
        public int CameraImageBuffLength { get; set; }
        public byte[] CameraImageBuff { get; set; }
        public string MPBIPAddress { get; set; }
        public DateTime NextStartTime { get; set; }
        public string HostIPAddress { get; set; }
        //public SocketAsyncEventArgs ReceiveEventArgs { get; set; }
        public TcpClient MyTcpClient { get; set; }
        //public TcpClient LocalTcpClient { get; set; }
        public BlockingCollection<byte[]> MPBMsgQueue;

        /// /////////////////////////////////////////////////////////

        public MyMBP()
        {
            
        }
        //public MyMBP(SocketAsyncEventArgs theReceiveEventArgs,/*Socket tcpClient,*/ string ip, string theMAC, uint theMPBID, uint theRelay, uint theTemp)
        public MyMBP(TcpClient tcpClient, string theMAC, uint theMPBID, uint theRelay, uint theTemp)
        {
            MyTcpClient = tcpClient;
           
            ListViewNO = Interlocked.Increment(ref no);
            MPBIPAddress = ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.ToString();
            MPBID = theMPBID;
            Relay = theRelay;
            MAC = theMAC;
            Temp = theTemp;
            MPBMsgQueue = new BlockingCollection<byte[]>();
            //ReceiveEventArgs = theReceiveEventArgs;
        }

        /// /////////////////////////////////////////////////
        /// 
        //public MyMBP UpdateMPB(SocketAsyncEventArgs theReceiveEventArgs, string theMAC, uint theMPBID, uint theRelay, uint theTemp)
        public MyMBP UpdateMPB(TcpClient tcpClient, string theMAC, uint theMPBID, uint theRelay, uint theTemp)
        {
            //ReceiveEventArgs = theReceiveEventArgs;
            MyTcpClient = tcpClient;
            MAC = theMAC;
            MPBID = theMPBID;
            Temp = theTemp;
            Relay = theRelay;
            OnUpdateMPB();
            return this;
        }

        public void ConnectCheck()
        {
            DisConnectTimer = new System.Threading.Timer(MPBDisConnect, MPBIPAddress, FREQ * 1000, Timeout.Infinite);
           
        }

        public void ConnectRefresh()
        {

            DisConnectTimer.Change(FREQ * 1000, Timeout.Infinite);
        }
        private void MPBDisConnect(object ip)
        {
            DisConnectTimer.Change(Timeout.Infinite, Timeout.Infinite);
            MPBMsgQueue.CompleteAdding();
            MPBMsgQueue.Dispose();
            CurrentMPBState = MPBState.DisConnected;
            MPBID = 0;
            //ReceiveEventArgs.AcceptSocket.Close();
        }
        //public void ConnectMPB()
        //{
        //    LocalTcpClient = new TcpClient();
        //    LocalTcpClient.Connect(MPBIPAddress, RemoteServerPort);
        //}

        //public void DisConnectMPB()
        //{
        //    LocalTcpClient.Close();
        //}

        public void RelayProcess(RelayOP OP, int index)
        {
            byte[] outBuf = MPBProtocol.getRelayCommand(OP, index);

            //LocalTcpClient.GetStream().Write(outBuf, 0, outBuf.Length);
            MPBMsgQueue.Add(outBuf);
        }
        public void GetMPBState()
        {
            byte[] outBuf = MPBProtocol.getMPBStateCommand();

            //LocalTcpClient.GetStream().Write(outBuf, 0, outBuf.Length);
            MPBMsgQueue.Add(outBuf);
        }
        /// ////////////////////////////////////////////
      
        public void OnNewMPB()
        {
            EventHandler temp = Interlocked.CompareExchange(ref newMPBValue, null, null);
            if (temp != null) temp(this, new EventArgs());
        }

        private void OnUpdateMPB()
        {
            EventHandler temp = Interlocked.CompareExchange(ref UpdateMPBValue, null, null);
            if (temp != null) temp(this, new EventArgs());
        }
        private void OnNewRelay()
        {
            EventHandler temp = Interlocked.CompareExchange(ref newRelayValue, null, null);
            if (temp != null) temp(this, new EventArgs());
        }
        private void OnNewTemp()
        {
            EventHandler temp = Interlocked.CompareExchange(ref newTempValue, null, null);
            if (temp != null) temp(this, new EventArgs());
        }
        private void OnNewImage()
        {
            EventHandler temp = Interlocked.CompareExchange(ref newImageValue, null, null);
            if (temp != null) temp(this, new EventArgs());
        }
        private void OnNewState()
        {
            EventHandler temp = Interlocked.CompareExchange(ref newStateValue, null, null);
            if (temp != null) temp(this, new EventArgs());
        }
        /// ////////////////////////////////////////////
      
    }
}
