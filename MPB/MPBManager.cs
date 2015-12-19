using System;
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


namespace MPB
{
    public class MPBManager
    {
        public static ConcurrentDictionary<string, MyMBP> MPBs = new ConcurrentDictionary<string, MyMBP>();
        public static ConcurrentBag<string> currentMPB = new ConcurrentBag<string>();
 

        private Form1 MyForm;

        public MPBManager(Form1 form) {
            MyForm = form;
        }
        //public MyMBP GetNewMPB(SocketAsyncEventArgs receiveSendEventArgs,/*Socket tcpClient, */string ip, string MAC, uint MPBID, uint relay, uint temp)
        public MyMBP GetNewMPB(TcpClient tcpClient, string MAC, uint MPBID, uint relay, uint temp)
        {
            MyMBP theMPB = new MyMBP(tcpClient, MAC, MPBID, relay, temp);
            MyForm.MPBAddEvect(theMPB);
            theMPB.OnNewMPB();
            theMPB.ConnectCheck();
            //((DataHoldingUserToken)receiveSendEventArgs.UserToken).IsMPBExit.Set();
            theMPB.GetMPBState();
            return theMPB;
        }
        //public void MPBKeepAlive(SocketAsyncEventArgs receiveSendEventArgs,/*Socket tcpClient,*/ string ip)
        public void MPBKeepAlive(TcpClient tcpClient)
        {
            string ip = ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.ToString();
            MyMBP m = MPBs.GetOrAdd(ip, key => GetNewMPB(tcpClient, "", 0, 0, 0));
            if (m.MPBID == 0)
                m.CurrentMPBState = MPBState.KeepAlive;
            else
                m.CurrentMPBState = MPBState.Connected;
            m.ConnectRefresh();

        }
        public void MPBCameraProcess(TcpClient tcpClient, byte[] theCameraImageBuff, int theCameraImageBuffLength)
        {
            string ip = ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.ToString();
            MyMBP m = MPBs[ip];
            m.CameraImageBuff = theCameraImageBuff;
            m.CameraImageBuffLength = theCameraImageBuffLength;
            m.IsNewCameraImage = true;
            m.ConnectRefresh();

        }
        public void MPBProcess(TcpClient tcpClient,  string MAC, uint MPBID, uint relay, uint temp)
        {
            string ip = ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.ToString();
            MyMBP m = MPBs.AddOrUpdate(
                    ip,
                    key => GetNewMPB(tcpClient, MAC, MPBID, relay, temp),
                    (key, OldMPB) => OldMPB.UpdateMPB(tcpClient,MAC, MPBID, relay, temp)
                );
            m.CurrentMPBState = MPBState.Connected;
           m.ConnectRefresh();
       }

    }

}
