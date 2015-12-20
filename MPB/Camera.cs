using System.IO.Ports;
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
    public class Camera
    {

        //复位指令与复位回复
        byte[] reset_cmd = { 0x56, 0x00, 0x26, 0x00 };
        byte[] reset_rcv = { 0x76, 0x00, 0x26, 0x00 };

        //设置临时波特率指令与回复
        byte[] bandrate_cmd = { 0x56, 0x00, 0x24, 0x03,0x01,0x0D,0xA6 };
        byte[] bandrate_rcv = { 0x76, 0x00, 0x24, 0x00, 0x00 };


        //设置压缩率指令与回复，最后1个字节为压缩率选项
        //范围是：00 - FF,一般设置是60 , 36
        byte[] set_compress_cmd = { 0x56, 0x00, 0x31, 0x05, 0x01, 0x01, 0x12, 0x04, 0x60 };
        byte[] compress_rate_rcv = { 0x76, 0x00, 0x31, 0x00, 0x00 };

        //设置图片大小指令与回复
        //photo_size_cmd最后1个字节的意义
        //0x22 - 160X120
        //0x11 - 320X240
        //0x00 - 640X480
        byte[] photo_size_cmd = { 0x56, 0x00, 0x31, 0x05, 0x04, 0x01, 0x00, 0x19, 0x11 };
        byte[] photo_size_rcv = { 0x76, 0x00, 0x31, 0x00, 0x00 };

        //清除图片缓存指令与回复
        byte[] photoBufCls_cmd = { 0x56, 0x00, 0x36, 0x01, 0x03 };
        byte[] photoBufCls_rcv = { 0x76, 0x00, 0x36, 0x00, 0x00 };

        //拍照指令与回复
        byte[] start_photo_cmd = { 0x56, 0x00, 0x36, 0x01, 0x00 };
        byte[] start_photo_rcv = { 0x76, 0x00, 0x36, 0x00, 0x00 };

        //读图片长度指令与回复
        //图片长度指令回复的前7个字节是固定的，最后2个字节表示图片的长度
        //如0xA0,0x00,10进制表示是40960,即图片长度(大小)为40K
        byte[] read_len_cmd = { 0x56, 0x00, 0x34, 0x01, 0x00 };
        byte[] read_len_rcv = { 0x76, 0x00, 0x34, 0x00, 0x04, 0x00, 0x00 };

        //读图片数据指令与回复
        //get_photo_cmd前6个字节是固定的，
        //第9,10字节是图片的起始地址
        //第13,14字节是图片的末尾地址，即本次读取的长度

        //如果是一次性读取，第9,10字节的起始地址是0x00,0x00;
        //第13,14字节是图片长度的高字节，图片长度的低字节(如0xA0,0x00)

        //如果是分次读取，每次读N字节(N必须是8的倍数)长度，
        //则起始地址最先从0x00,0x00读取N长度(即N & 0xff00, N & 0x00ff)，
        //后几次读的起始地址就是上一次读取数据的末尾地址
        byte[] get_photo_cmd = { 0x56, 0x00, 0x32, 0x0C, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF };
        byte[] get_photo_rcv = { 0x76, 0x00, 0x32, 0x00, 0x00 };

        static SerialPort sp;

        public byte[] CameraBuffer { get; set; }
        public int CameraBufferLength { get; set; }

        public int CameraBufferStart = 5;

        public Camera(SerialPort theSP)
        {
            sp = theSP;
        }

        public void CameraInit(string thePhotoSize,byte theCompressRate)
        {
            
            //设置图片尺寸，一次设置永久有效
            if (!SendPhotoSize(thePhotoSize))
                throw new Exception("\r\nsend photo size error\r\n");

            //清空图片缓存
            if (!SendPhotoBufCls())
                throw new Exception("\r\nsend stop_photo error\r\n");

            //设置图片压缩率,一次设置永久有效
            //if (!SendCompressRate(theCompressRate))
            //    throw new Exception("\r\nsend compress rate error\r\n");

            if (!SendReset())
                throw new Exception("\r\nsend reset error\r\n");
           

        }

        /****************************************************************
        函数名：send_read_len
        函数描述：读取拍照后的图片长度，即图片占用空间大小
        输入参数：无
        返回:图片的长度
        ******************************************************************/
        public void SendReadLen()
        {
           
            byte[] tmp = null;

            //发送读图片长度指令           
            tmp = sp.ReadTaskAsync(read_len_cmd, 0, read_len_cmd.Length, 9).Result;

            for (int i = 0; i < read_len_rcv.Length; i++)
                if (read_len_rcv[i] != tmp[i])
                    throw new Exception("\r\nread photo size error\r\n");

            CameraBufferLength = (int)tmp[7] << 8;//高字节
            CameraBufferLength |= tmp[8];//低字节

           
        }


        /****************************************************************
        函数名：send_get_photo
        函数描述：读取图片数据
        输入参数：读图片起始地址StaAdd, 
                  读取的长度readLen ，
                  接收数据的缓冲区buf
        返回:成功返回1，失败返回0
        FF D8 ... FF D9 是JPG的图片格式

        1.一次性读取的回复格式：76 00 32 00 00 FF D8 ... FF D9 76 00 32 00 00

        2.分次读取，每次读N字节,循环使用读取图片数据指令读取M次或者(M + 1)次读取完毕：
        如第一次执行后回复格式
        76 00 32 00 <FF D8 ... N> 76 00 32 00
        下次执行读取指令时，起始地址需要偏移N字节，即上一次的末尾地址，回复格式
        76 00 32 00 <... N> 76 00 32 00
        ......
        76 00 32 00 <... FF D9> 76 00 32 00 //lastBytes <= N

        Length = N * M 或 Length = N * M + lastBytes

        ******************************************************************/
        public  void SendGetPhoto(int staAdd, int readLen)
        {
          
            //装入起始地址高低字节
            get_photo_cmd[8] = (byte)((staAdd >> 8) & 0xff);
            get_photo_cmd[9] = (byte)(staAdd & 0xff);

            //装入末尾地址高低字节
            get_photo_cmd[13] = (byte)(readLen & 0xff);
            get_photo_cmd[12] = (byte)((readLen >> 8) & 0xff);

            CameraBuffer = sp.ReadTaskAsync(get_photo_cmd, 0, get_photo_cmd.Length, readLen+10).Result;

            //检验帧头76 00 32 00 00  ||  //检验帧尾76 00 32 00 00
            for (int i = 0; i < get_photo_rcv.Length; i++)
                if ((get_photo_rcv[i] != CameraBuffer[i])/* || (get_photo_rcv[i] != tmp[i + get_photo_rcv.Length + readLen])*/)
                    throw new Exception("\r\nread photo error @ " + i + "\r\n");
        }

        public void GetSerialPortPhoto()
        {
            SendPhotoBufCls();  // clear buff

            SendStartPhoto();   // start photo

            SendReadLen();   //read length

            SendGetPhoto(0, CameraBufferLength);   //get photo
        }

        /****************************************************************
         函数名：send_cmd
         函数描述：发送指令并识别指令返回
         输入参数：指令的首地址，指令的长度，匹配指令的首地址，需验证的个数
         返回：成功返回1,失败返回0
        ******************************************************************/
        private bool SendCmd(byte[] cmd, byte[] rev)
        {
            byte[] tmp = sp.ReadTaskAsync(cmd, 0, cmd.Length, rev.Length).Result;
            return tmp.SequenceEqual(rev);
        }
        /****************************************************************
      函数名：send_bandwidth
      函数描述：临时修改串口波特率
      输入参数：无
      返回:成功返回1 失败返回0
      ******************************************************************/
        public bool SendBandRate()
        {
            bool b = false;
            b = SendCmd(bandrate_cmd, bandrate_rcv);
            //复位后延时2秒
            Thread.Sleep(2000);
            return b;
        }
        /****************************************************************
        函数名：send_reset
        函数描述：发送复位指令复位后要延时2.5秒
        输入参数：无
        返回:成功返回1 失败返回0
        ******************************************************************/
        public bool SendReset()
        {
            bool b = false;
            b = SendCmd(reset_cmd, reset_rcv);
            //复位后延时2秒
            Thread.Sleep(2000);
            return b;
        }
        /****************************************************************
        函数名：send_stop_photo
        函数描述：清空图片缓存
        输入参数：无
        返回:成功返回1,失败返回0
        ******************************************************************/
        public bool SendPhotoBufCls()
        {
            return SendCmd(photoBufCls_cmd, photoBufCls_rcv);
        }
        /****************************************************************
        函数名：send_compress_rate
        函数描述：发送设置压缩率大少设置为80:1
        输入参数：无
        返回:成功返回1,失败返回0
        ******************************************************************/
        public bool SendCompressRate(byte theCompRatio)
        {
            bool b = false;
            set_compress_cmd[8] = theCompRatio;
            b = SendCmd(set_compress_cmd, compress_rate_rcv);
            Thread.Sleep(1000);
            return b;
        }
        /****************************************************************
        函数名：send_start_photo
        函数描述：发送开始拍照的指令
        输入参数：无
        返回:识别成功返回1 失败返回0
        ******************************************************************/
        public bool SendStartPhoto()
        {
            return SendCmd(start_photo_cmd, start_photo_rcv);
        }
        /****************************************************************
        函数名：send_photo_size
        函数描述：设置图片大小，可选择：160X120,320X240,640X480
        输入参数：无
        返回:成功返回1,失败返回0
        ******************************************************************/
        public bool SendPhotoSize(string theSize)
        {
            if (theSize == "L")
                photo_size_cmd[8] = 0x00;
            else if (theSize == "M")
                photo_size_cmd[8] = 0x11;
            else if (theSize == "S")
                photo_size_cmd[8] = 0x22;

            return SendCmd(photo_size_cmd, photo_size_rcv);
        }

    }
}
