using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using System.Threading;

namespace MPB
{
    public static class CheckBoxExtensions
    {

        public static void ON(this CheckBox checkBox)
        {
            checkBox.InvokeIfNeeded(delegate
                {
                    checkBox.Checked = true;   
                }, InvocationMethod.Asynchronous);
           
        }

        public static void OFF(this CheckBox checkBox)
        {
            checkBox.InvokeIfNeeded(delegate
            {
                checkBox.Checked = false;
            }, InvocationMethod.Asynchronous);

        }
        public static void INDETERMINATE(this CheckBox checkBox)
        {
            checkBox.InvokeIfNeeded(delegate
            {               
                checkBox.CheckState = CheckState.Indeterminate;
            }, InvocationMethod.Asynchronous);

        }
        public static void None(this CheckBox checkBox, bool flag)
        {
            checkBox.InvokeIfNeeded(delegate
            {
                if (flag)
                    checkBox.Enabled = false;
                else 
                    checkBox.Enabled = true;

            }, InvocationMethod.Asynchronous);

        }
        public static void None(this Button button, bool flag)
        {
            button.InvokeIfNeeded(delegate
            {
                if (flag)
                    button.Enabled = false;
                else
                    button.Enabled = true;
              
            }, InvocationMethod.Asynchronous);

        }
        public static void None(this GroupBox groupBox, bool flag)
        {
            groupBox.InvokeIfNeeded(delegate
            {
                if (flag)
                    groupBox.Enabled = false;
                else
                    groupBox.Enabled = true;

            }, InvocationMethod.Asynchronous);

        }
        public static Task<byte[]> ReadTaskAsync(this SerialPort serialPort, byte[] sendBuff, Int32 startAdd, Int32 sendLen,Int32 readLen)
        {

            var tcs = new TaskCompletionSource<byte[]>();


            SerialDataReceivedEventHandler hander = null;
            hander = (sender, e) =>
             {
                 serialPort.DataReceived -= hander;
                 SerialPort sp = (SerialPort)sender;

                 byte[] readBuff = new byte[readLen];

                 int i = 0, j = readLen, k = 0;

                 while (i < readLen && k < 5)
                 {
                     Thread.Sleep(10);
                     if ((j = sp.BytesToRead) > 0)
                     {
                         if (j > readLen) j = readLen;
                         sp.Read(readBuff, i, j);
                         
                         i = i + j;
                     }
                     else
                     {
                         k++;
                     }
                 }
                 sp.DiscardInBuffer();
                 tcs.TrySetResult(readBuff);
             };

            serialPort.DataReceived += hander;
            serialPort.Write(sendBuff, startAdd, sendLen);

            return tcs.Task;

        }

        
    }

}
