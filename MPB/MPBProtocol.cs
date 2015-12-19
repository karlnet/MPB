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
   public  class MPBProtocol
    {
        private static byte CONTROL_FLAG = 0xBB;
       
        private static byte KeepAlive_FLAG = 0x65;
        private static byte[] CameraImage_FLAG = { 0x76, 0x00 };

        private static int HA_CMD_HEAD_SIZE = 8;
        private static int HA_CMD_OFFSET = 2;
        private static int HA_CMD_DATALENGTH_OFFSET = 6;
        private static int HA_CMD_MPBID_OFFSET = HA_CMD_HEAD_SIZE + 0;
        private static int HA_CMD_IPADDRESS_OFFSET = HA_CMD_HEAD_SIZE + 4;
        private static int HA_CMD_TEMP_OFFSET = HA_CMD_HEAD_SIZE + 8;
        private static int HA_CMD_RELAY_OFFSET = HA_CMD_HEAD_SIZE + 16;
        private static int HA_CMD_HOSTIPADDRESS_OFFSET = HA_CMD_HEAD_SIZE + 20;
        private static int HA_CMD_MAC_OFFSET = HA_CMD_HEAD_SIZE + 24;
        private static int HA_CMD_CRC_SIZE = 2;

        private static byte[] MPBCommand = { 0xBB, 0x00, 0x0C, 00, 00, 00, 04, 00, 00, 00, 0xCC, 00, 00, 00 };
        private static byte GetStateCommand = 0x0B;


       private MPBManager mpbManager;

       public  MPBProtocol()
       {
       }

       public  MPBProtocol(MPBManager theMPBManager)
       {

           mpbManager = theMPBManager;
       
       }
       private bool CheckData(Byte[] inBuf, int inBufLen)
       {
           int cmdLen = 0;
           cmdLen = HA_CMD_HEAD_SIZE + inBuf[6] + (inBuf[7] << 8) + HA_CMD_CRC_SIZE;
           if ((inBuf[0] == CONTROL_FLAG) && (inBuf[1] == 0x00) && (cmdLen <= inBufLen))
               return true;
           return false;
       }
       
       //public void ProtocolProcess(SocketAsyncEventArgs receiveSendEventArgs/*Socket tcpClient*/, DataHoldingUserToken receiveSendToken, int inBufLen)
       public void ProtocolProcess(TcpClient tcpClient, byte[] inBuf, int inBufLen)
       {
            
           //bb 00 , 0b 80, 00 00 ,16 00, ff ff 00 00, 33 02 a8 c0, 00 00 80 3d, cc cc cc cc, 1d 00 00 00, 10 e5  
            /*
            -------------------------------------------------------------------------------
                起始标志  |  命令   | 命令返回状态  |	数据长度  |	 数据     |   校验
            -------------------------------------------------------------------------------
               BB  00    |  U16    |    U16        |    U16	 |	  U8	 |    U16
            --------------------------------------------------------------------------------
             */

           //IPEndPoint enp = (IPEndPoint)receiveSendEventArgs.AcceptSocket.RemoteEndPoint;
           //IPEndPoint enp = (IPEndPoint)tcpClient.Client.RemoteEndPoint;
           //string ip = enp.Address.ToString();
           //byte[] inBuf=new byte[512];

           //Buffer.BlockCopy(receiveSendEventArgs.Buffer, receiveSendToken.receiveMessageOffset, inBuf, receiveSendToken.receivedMessageBytesDoneCount, inBufLen);
          

           if (inBuf[0] == KeepAlive_FLAG)
           {
               //mpbManager.MPBKeepAlive( receiveSendEventArgs,/*tcpClient, */ip);
               mpbManager.MPBKeepAlive(tcpClient);
               return;
           }
           if (inBuf[0] == CameraImage_FLAG[0] && inBuf[1] == CameraImage_FLAG[1])
           {

               mpbManager.MPBCameraProcess(tcpClient, inBuf, inBufLen);
               return;
           }

           int cmdLen = 0;
           cmdLen = HA_CMD_HEAD_SIZE + inBuf[HA_CMD_DATALENGTH_OFFSET] + (inBuf[HA_CMD_DATALENGTH_OFFSET + 1] << 8) + HA_CMD_CRC_SIZE;
           if ((inBuf[0] == CONTROL_FLAG) && (inBuf[1] == 0x00) && (cmdLen <= inBufLen))
           {
               switch (inBuf[HA_CMD_OFFSET])
               {
                   case 0x0B:  // mpb state report
                       uint MPBID = BitConverter.ToUInt32(inBuf, HA_CMD_MPBID_OFFSET);
                       uint relay = BitConverter.ToUInt32(inBuf, HA_CMD_RELAY_OFFSET);
                       string MAC = string.Format("{0:X}:{1:X}:{2:X}:{3:X}:{4:X}:{5:X}", inBuf[HA_CMD_MAC_OFFSET], inBuf[HA_CMD_MAC_OFFSET + 1], inBuf[HA_CMD_MAC_OFFSET + 2], inBuf[HA_CMD_MAC_OFFSET + 3], inBuf[HA_CMD_MAC_OFFSET + 4], inBuf[HA_CMD_MAC_OFFSET + 5]);
                       uint temp = BitConverter.ToUInt32(inBuf, HA_CMD_TEMP_OFFSET);
                       mpbManager.MPBProcess(tcpClient,  MAC, MPBID, relay, temp);
                       break;

               }
           }

       }

       public static byte[] getRelayCommand(RelayOP op, int index)
       {
           byte[] outBuf = new byte[14];
           Array.Copy(MPBCommand, outBuf, outBuf.Length);
           outBuf[10] = (byte)op;
           outBuf[8] |= (byte)(0x1 << index);
           return outBuf;
       }
       public static byte[] getMPBStateCommand()
       {
           byte[] outBuf = new byte[14];
           Array.Copy(MPBCommand, outBuf, outBuf.Length);
           outBuf[HA_CMD_OFFSET] = GetStateCommand;
          
           return outBuf;
       }
    }
}
