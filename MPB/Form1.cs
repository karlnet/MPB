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
using System.IO.Ports;

using System.IO;

namespace MPB
{
    

    public partial class Form1 : Form
    {

        List<Button> Bs = new List<Button>();
        List<RadioButton> Rs = new List<RadioButton>();
     
        TCPManager tcpManager;
        MPBManager mpbManager;

        Camera c;

        public Form1()
        {
            InitializeComponent();
           
            Rs.Add(radioButton1);
            Rs.Add(radioButton2);
            Rs.Add(radioButton3);
            Rs.Add(radioButton4);
            Rs.Add(radioButton5);
            Rs.Add(radioButton6);

            Bs.Add(btnSwitch);
            Bs.Add(btnReset);
           
        }

        // ///////////////////////////////////////////////////////////////////////////////

          public void MPBAddEvect(MyMBP m) {

            m.newTempValue += TempUpdate;
            m.newRelayValue += RelayUpdate;            
            m.newMPBValue += MPBNew;
            m.UpdateMPBValue += MPBUpdate;
            m.newImageValue += ImageUpdate;
            m.newStateValue += StateUpdate;
        }
          private void StateUpdate(object sender, EventArgs e)
          {
              MyMBP m = (MyMBP)sender;
              string mpbIPADDRESS = m.MPBIPAddress;
              switch (m.CurrentMPBState)
              {
                  case MPBState.KeepAlive:
                      lvDevice.DisableItems(m.ListViewNO.ToString(), 1);
                      tbMsg.WriteText(mpbIPADDRESS + " 连接成功，数据未上报。");
                      break;
                  case MPBState.Connected:
                      lvDevice.DisableItems(m.ListViewNO.ToString(), 2);
                      tbMsg.WriteText(mpbIPADDRESS + " 连接成功。");
                      //if (MPBManager.currentMPB.Contains(mpbIPADDRESS))
                      //{
                      //    gbBar1.None(false);
                      //}
                      break;
                  case MPBState.DisConnected:
                     
                      tbMsg.WriteText(mpbIPADDRESS + " 连接断开。");
                      lvDevice.DisableItems(m.ListViewNO.ToString(), 3);
                      if (MPBManager.currentMPB.Contains(mpbIPADDRESS))
                      {
                       
                          DisControls(null,true);
                          CloseCurrentLocalConnection();
                      }
                      break;
              }

          }
        private void RelayUpdate(object sender, EventArgs e) 
        {
            MyMBP m = (MyMBP)sender;
            if (MPBManager.currentMPB.Count == 1 && MPBManager.currentMPB.Contains(m.MPBIPAddress))
            {
                RadioButtonSet(m);
            }
        }
        private void TempUpdate(object sender, EventArgs e)
        {
            MyMBP m = (MyMBP)sender;
            if (MPBManager.currentMPB.Count == 1 && MPBManager.currentMPB.Contains(m.MPBIPAddress))
            {
                tbMAC.WriteText(m.MAC);
                tbTemp.WriteText(String.Format("{0} °C", m.Temp) );
            }
        }
        private void ImageUpdate(object sender, EventArgs e)
        {
            MyMBP m = (MyMBP)sender;
            if (m.IsNewCameraImage) {
                
                m.IsNewCameraImage = false;
                pictureBox1.Image = Image.FromStream(new MemoryStream(m.CameraImageBuff, 5, m.CameraImageBuffLength));
            }
           
        }
        private void MPBNew(object sender, EventArgs e)
        {
            MyMBP m = (MyMBP)sender;
            lvDevice.AddRow(m.ListViewNO.ToString(), InvocationMethod.Asynchronous, m.MPBIPAddress, "0.0.0.0", "R", m.Temp.ToString());
            if (m.CurrentMPBState == MPBState.KeepAlive) { 
                    //
            }
        }
        private void MPBUpdate(object sender, EventArgs e) 
        {
            MyMBP m = (MyMBP)sender;
            lvDevice.UpdateRow(m.ListViewNO.ToString(), InvocationMethod.Asynchronous, m.MPBIPAddress, "0.0.0.0", "R", m.Temp.ToString());
        }
        //private void DisConnectSet(object ip)
        //{

        //    string mpbIPADDRESS = (string)ip;

        //    tbMsg.WriteText(mpbIPADDRESS + " 连接断开.");

        //    if (MPBManager.currentMPB.Contains(mpbIPADDRESS))
        //    {
        //        gbMPB2.None(true);
        //        CloseCurrentLocalConnection();
        //    }

        //}

      //// -----------------------------------------------------------------------------

        private void RadioButtonSet(MyMBP m)
        {
            uint index = m.Relay;
            //Rs.ForEach(rb => rb.CheckedChanged -= RadioCheckedChanged);
            for (int i = 0; i < Rs.Count / 2; i++)
            {
                Rs[i * 2].CheckedChanged -= RadioCheckedChanged;
                if ((index & 0x01) == 0x01)
                    Rs[i * 2].ON();
                else
                    Rs[i * 2 + 1].ON();
                Rs[i * 2].CheckedChanged += RadioCheckedChanged;
                index = index >> 0x01;
            }
            //Rs.ForEach(rb => rb.CheckedChanged += RadioCheckedChanged);
        }

        private void CloseCurrentLocalConnection()
        {
            string tmp;

            if (!MPBManager.currentMPB.IsEmpty)
            {
                MPBManager.currentMPB.ToList().ForEach(key =>
                {
                    //MPBManager.MPBs[key].DisConnectMPB();
                    MPBManager.currentMPB.TryTake(out tmp);
                });
            }
        }

        /// ///////////////////////////////////////////////////////////////////////////////////
        
        private void Form1_Load(object sender, EventArgs e)
        {
            DisControls(null,true);
         
            mpbManager = new MPBManager(this);
            tcpManager = new TCPManager(mpbManager);
            
            c = new Camera(serialPort1);
        }
        private void DisControls(MyMBP mm ,bool b) {

            if (b)
            {
                gbBar1.None(true);
                tbIP.WriteText("0.0.0.0");
                tbMAC.WriteText("00:00:00:00:00:00");
                tbTemp.WriteText("0");
               
                Bs.ForEach(bt => bt.None(true));
            }
            else
            {
                gbBar1.None(false);
                tbIP.Text = mm.MPBIPAddress;
                tbMAC.Text = mm.MAC;
                tbTemp.Text = String.Format("{0} °C", mm.Temp);
                RadioButtonSet(mm);
                Bs.ForEach(bt => bt.None(false));
            }
        }
        private void lvDevice_SelectedIndexChanged(object sender, EventArgs e)
        {

            DisControls(null, true);
            gbMPB2.None(false);
            CloseCurrentLocalConnection();

            string currentIP = lvDevice.Items[lvDevice.SelectedIndices[0]].SubItems[1].Text.ToString();
            string currentIndex = lvDevice.Items[lvDevice.SelectedIndices[0]].Text;

            if (lvDevice.SelectedIndices.Count == 1)
            {
                if (MPBManager.MPBs[currentIP].CurrentMPBState == MPBState.Connected)
                {
                   
                    gbBar1.Text = "媒体播放器:(" + currentIndex + "#)";

                    DisControls(MPBManager.MPBs[currentIP],false);
                    //tbMsg.Text = "正在连接" + currentIP + " ...";

                    //try
                    //{
                    //    MPBManager.MPBs[currentIP].ConnectMPB();
                    //}
                    //catch (SocketException se)
                    //{

                    //    tbMsg.Text = "连接" + currentIP + " 失败..." + se.ToString();
                    //}
                    tbMsg.Text = "连接" + currentIP + " 成功。";
                    MPBManager.currentMPB.Add(currentIP);
                }
                else if (MPBManager.MPBs[currentIP].CurrentMPBState == MPBState.KeepAlive)
                {
                    tbMsg.Text = "连接" + currentIP + " 成功，状态未上传。";
                }
                else
                {
                    tbMsg.Text = "连接" + currentIP + " 失败。";
                }
            }

            if (lvDevice.SelectedIndices.Count > 1)
            {
                StringBuilder tmpS = new StringBuilder("媒体播放器:(");
                foreach (int x in lvDevice.SelectedIndices)
                    tmpS.Append((x + 1).ToString() + "#,");
                tmpS.Remove(tmpS.Length - 1, 1);
                tmpS.Append(")");
                gbBar1.Text = tmpS.ToString();
                tmpS.Length = 0;

                tbIP.Text = "";
                tbMAC.Text = "";
                tbTemp.Text = "";

                Rs.ForEach(r => { r.Checked = false; });

                StringBuilder sb = new StringBuilder();
                foreach (int x in lvDevice.SelectedIndices)
                {

                    string currentIP2 = lvDevice.Items[lvDevice.SelectedIndices[x]].SubItems[1].Text.ToString();
                    tbMsg.Text = "正在连接" + currentIP2 + " ...";
                    sb = sb.Append(currentIP2 + " , ");
                    //try
                    //{
                    //    MPBManager.MPBs[currentIP2].ConnectMPB();
                    //}
                    //catch (SocketException se)
                    //{

                    //    tbMsg.Text = "连接" + currentIP2 + " 失败..." + se.ToString();
                    //}
                    MPBManager.currentMPB.Add(currentIP2);
                }
                sb.Length -= 2;
                string s=null;
                if (null != (s = MPBManager.currentMPB.ToList().Find(ip => MPBManager.MPBs[ip].CurrentMPBState != MPBState.Connected)))
                {

                    tbMsg.Text = "连接" + s + " 失败。";
                }
                else {
                    gbBar1.None(false);
                    tbMsg.Text = "连接 " + sb.ToString() + " 成功。";
                    sb.Length = 0;
                }

            }
        }
        private void btn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int index = Bs.IndexOf(btn) + 3;

           MPBManager. currentMPB.ToList().ForEach(key =>
            {
                MPBManager.MPBs[key].RelayProcess(RelayOP.ON, index);
            });          
        }

        private void RadioCheckedChanged(object sender, EventArgs e)
        {
            RadioButton cQ = (RadioButton)sender;
            int index = Rs.IndexOf(cQ) / 2;

            foreach (string s in MPBManager.currentMPB)
            {
                RelayOP ro;

                if (Rs[2 * index].Checked)
                    ro = RelayOP.ON;
                else
                    ro = RelayOP.OFF; 

                MPBManager.MPBs[s].RelayProcess(ro, index);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (string s in MPBManager.currentMPB)
            {
                byte[] MPBCommand = { 0xBB, 0x00, 0x0C, 00, 00, 00, 04, 00, 0x0F, 00, 0xCC, 00, 00, 00 };

                MPBManager.MPBs[s].MPBMsgQueue.Add(MPBCommand);
                //MPBManager.MPBs[s].MPBMsgQueue.cCompleteAdding();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (string s in MPBManager.currentMPB)
            {
                byte[] MPBCommand = { 0xBB, 0x00, 0x0C, 00, 00, 00, 04, 00, 0x0F, 00, 0xDD, 00, 00, 00 };

                MPBManager.MPBs[s].MPBMsgQueue.Add(MPBCommand);
                //MPBManager.MPBs[s].MPBMsgQueue.CompleteAdding();
            }
        }


        
            
        private void button3_Click(object sender, EventArgs e)
        {
            byte[] image = MPBManager.MPBs[MPBManager.currentMPB.ElementAt(0)].CameraImageBuff;
            int len=MPBManager.MPBs[MPBManager.currentMPB.ElementAt(0)].CameraImageBuffLength;
            pictureBox1.Image = Image.FromStream(new MemoryStream(image, 0, len));

            //serialPort1.PortName = "COM5";
            //serialPort1.BaudRate = 115200;
            //serialPort1.ReadBufferSize = 8192 * 2 * 4;

            //serialPort1.Open();


            //c.CameraInit("M", 0x60);    // 320*240 , 压缩率 0x60

            //cts = new CancellationTokenSource();

            //Task t = Task.Run(() =>
            // {
            //     string path = @"c:\2\MyTest";
            //     string fileName = "";


            //     for (int i = 0; i < 1000; i++)
            //     {
            //         c.GetSerialPortPhoto();

            //         fileName = path + i.ToString("D3") + ".jpg";

            //         using (FileStream fs = File.Create(fileName))
            //         {
            //             fs.Write(c.CameraBuffer, c.CameraBufferStart, c.CameraBufferLength);
            //         }
            //         pictureBox1.Image = Image.FromStream(new MemoryStream(c.CameraBuffer, c.CameraBufferStart, c.CameraBufferLength));

            //         if (cts.Token.IsCancellationRequested)
            //         {
            //             serialPort1.Close();
            //             return;
            //         }
            //     }
            // }, cts.Token);

              

        }
        private CancellationTokenSource cts = new CancellationTokenSource();
        private void button4_Click(object sender, EventArgs e)
        {
            //cts.Cancel();
        }


    }
}
