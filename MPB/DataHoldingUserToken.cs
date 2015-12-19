using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
namespace MPB
{
    public class DataHoldingUserToken
    {
        public AutoResetEvent IsMPBExit = new AutoResetEvent(false);

        internal DataHolder theDataHolder;

        internal readonly Int32 bufferOffsetReceive;
        internal readonly Int32 permanentReceiveMessageOffset;
        internal readonly Int32 bufferOffsetSend;

        internal Int32 lengthOfCurrentIncomingMessage;

        internal Int32 receiveMessageOffset;
        internal Byte[] byteArrayForPrefix;
        internal readonly Int32 receivePrefixLength;
        internal Int32 receivedPrefixBytesDoneCount = 0;
        internal Int32 receivedMessageBytesDoneCount = 0;
   
        internal Int32 recPrefixBytesDoneThisOp = 0;

        internal Int32 sendBytesRemainingCount;
        internal readonly Int32 sendPrefixLength;
        public Byte[] dataToSend;//=new byte[128];
        internal Int32 bytesSentAlreadyCount=0;

        private SocketAsyncEventArgs saeaObject;

        public DataHoldingUserToken(SocketAsyncEventArgs e, Int32 rOffset, Int32 sOffset, Int32 receivePrefixLength, Int32 sendPrefixLength/*, Int32 identifier*/)
        {
            //this.idOfThisObject = identifier;
            saeaObject = e;
  
            this.bufferOffsetReceive = rOffset;
            this.bufferOffsetSend = sOffset;
            this.receivePrefixLength = receivePrefixLength;
            this.sendPrefixLength = sendPrefixLength;
            this.receiveMessageOffset = rOffset + receivePrefixLength;
            this.permanentReceiveMessageOffset = this.receiveMessageOffset;
        }
        internal void CreateNewDataHolder()
        {
            theDataHolder = new DataHolder();
        }
        internal void PrepareOutgoingData()
        {
            Int32 lengthOfCurrentOutgoingMessage = theDataHolder.dataMessageReceived.Length;

            Byte[] arrayOfBytesInPrefix = BitConverter.GetBytes(lengthOfCurrentOutgoingMessage);

            dataToSend = new Byte[sendPrefixLength + lengthOfCurrentOutgoingMessage];

            Buffer.BlockCopy(arrayOfBytesInPrefix, 0, dataToSend, 0, sendPrefixLength);
            //Buffer.BlockCopy(idByteArray, 0, theUserToken.dataToSend, theUserToken.sendPrefixLength, idByteArray.Length);
            //The message that the client sent is already in a byte array, in DataHolder.
            Buffer.BlockCopy(theDataHolder.dataMessageReceived, 0, dataToSend, sendPrefixLength, theDataHolder.dataMessageReceived.Length);

            sendBytesRemainingCount = sendPrefixLength + lengthOfCurrentOutgoingMessage;
            bytesSentAlreadyCount = 0;
            CreateNewDataHolder();
        }
        public void Reset()
        {
            this.receivedPrefixBytesDoneCount = 0;
            this.receivedMessageBytesDoneCount = 0;
            this.recPrefixBytesDoneThisOp = 0;
            this.receiveMessageOffset = this.permanentReceiveMessageOffset;
        }
    }
}
