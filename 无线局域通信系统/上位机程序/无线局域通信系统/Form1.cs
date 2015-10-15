using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;

namespace 无线局域通信系统
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

     
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        SerialPort sp = new SerialPort();
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (button5.Text == "打开串口")
                {
                    if ( comboBox1.Text== null)
                    {
                        MessageBox.Show("请先选择串口！", "Error");
                        return;
                    }

                    sp.Close();
                    sp = new SerialPort();
                    sp.PortName = comboBox1.Text;//串口编号
                    sp.BaudRate = 9600;//波特率
                    sp.StopBits = StopBits.One ;                              //设置停止位
                    sp.DataBits = 8;                                         //设置数据位
                    sp.Parity = Parity.None ;                                 //设置串口属性
                    sp.WriteTimeout = 3000;                                   //写超时
                    sp.ReadTimeout = 3000;                                     //读超时
                    sp.Open();//打开串口
                    button5.Text = "关闭串口";
                    MessageBox.Show ( Convert.ToString(sp.PortName) + "已开启！","OK");


                }
                else
                {
                    sp.Close();
                    button5.Text = "打开串口";
                    MessageBox.Show ( Convert.ToString(sp.PortName) + "已关闭！","ERROR");
                }
            }

            
            catch (Exception)
            {

                MessageBox.Show("打开失败！" , "Error");
                return;

            }
        }



        string I1 = "1";
        string I2 = "1";
        string I3 = "1";
        string I4 = "1";
        string I5 = "1";
        string I6 = "1";
        string I7 = "1";
        string I8 = "1";
        string O1 = "0";
        string O2 = "0";
        string O3 = "0";
        string O4 = "0";
        string O5 = "0";
        string O6 = "0";
        string O7 = "0";
        string O8 = "0";
        string x = "0";
        string y = "0";
        string z = "00000000" ;   //标识位
        string a = "0";
        string b = "0";
        string con = "10101010";
       
        
        private void button1_Click(object sender, EventArgs e)                  
        {

            
            
            
            if (I1 == "1")
            {
                
                I1 = "0";
                O1 = "1";
                x = con + z + I8 + I7 + I6 + I5 + I4 + I3 + I2 + I1 + O8 + O7 + O6 + O5 + O4 + O3 + O2 + O1;
                y = I8 + I7 + I6 + I5 + I4 + I3 + I2 + I1;
                a =Convert.ToString(Convert.ToInt32(x,2),16).ToUpper();               //将二进制字符串转化为十六进制字符串
                b = Fill16(Convert.ToString(Convert.ToInt32(y, 2), 16)).ToUpper();
                textBox1.Text = a;                                        //显示要发送的数
               
                
                try
                {    
                     
                     
                     Byte[] bit=strToToHexByte(a);
                     sp.Write(bit,0,4);                                             //向串口写入 
                     Byte[] buffer = new Byte[1];
                     sp.Read(buffer, 0, 1);
                     string result = byteToHexStr(buffer);
                    if ( b==result)                                 //与单片机返回的数比较
                    {
                        MessageBox.Show(this, "发送成功", ":)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        pictureBox1.Image = Properties.Resources.on;              //改变图片
                        button1.Text = "关闭";
                    }
                    else
                    {
                        MessageBox.Show(this, "发送失败", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                 
                    
                }
                
                catch (Exception)
                {

                    MessageBox.Show(this, "打开失败！", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                   
                 
                }
              
          
            }
            else
            {
                
                I1 = "1";
                O1 = "0";
                x = con + z + I8 + I7 + I6 + I5 + I4 + I3 + I2 + I1 + O8 + O7 + O6 + O5 + O4 + O3 + O2 + O1;
                y = I8 + I7 + I6 + I5 + I4 + I3 + I2 + I1;
                a = Convert.ToString(Convert.ToInt32(x, 2), 16).ToUpper();
                b = Fill16(Convert.ToString(Convert.ToInt32(y, 2), 16)).ToUpper();
                textBox1.Text = a; 
                
                try
                {
                    Byte[] bit = strToToHexByte(a);
                    sp.Write(bit, 0, 4);                                             //向串口写入 
                    Byte[] buffer = new Byte[1];
                    sp.Read(buffer, 0, 1);
                    string result = byteToHexStr(buffer);
                    if (b == result)                                 //与单片机返回的数比较
                    {
                        MessageBox.Show(this, "发送成功", ":)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        pictureBox1.Image = Properties.Resources.off;
                        button1.Text = "打开";
                    }
                    else
                    {
                        MessageBox.Show(this, "发送失败", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                   
                }
                catch (Exception)
                {

                    MessageBox.Show(this, "打开失败！", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                 
                   
                }

                
                
            }
         
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (I2 == "1")
            {
                
                I2 = "0";
                O2 = "1";
                x = con + z + I8 + I7 + I6 + I5 + I4 + I3 + I2 + I1 + O8 + O7 + O6 + O5 + O4 + O3 + O2 + O1;
                y = I8 + I7 + I6 + I5 + I4 + I3 + I2 + I1;
                a = Convert.ToString(Convert.ToInt32(x, 2), 16).ToUpper();
                b = Fill16(Convert.ToString(Convert.ToInt32(y, 2), 16)).ToUpper();
                textBox1.Text = a;
               
                try
                {
                    Byte[] bit = strToToHexByte(a);
                    sp.Write(bit, 0, 4);                                             //向串口写入 
                    Byte[] buffer = new Byte[1];
                    sp.Read(buffer, 0, 1);
                    string result = byteToHexStr(buffer);
                    if (b == result)                                 //与单片机返回的数比较
                    {
                        MessageBox.Show(this, "发送成功", ":)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        pictureBox2.Image = Properties.Resources.on;
                        button2.Text = "关闭";
                    }
                    else
                    {
                        MessageBox.Show(this, "发送失败", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception)
                {

                    MessageBox.Show(this, "打开失败！", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


            }
            else
            {
              
                I2 = "1";
                O2 = "0";
                x = con + z + I8 + I7 + I6 + I5 + I4 + I3 + I2 + I1 + O8 + O7 + O6 + O5 + O4 + O3 + O2 + O1;
                y = I8 + I7 + I6 + I5 + I4 + I3 + I2 + I1;
                a = Convert.ToString(Convert.ToInt32(x, 2), 16).ToUpper();
                b = Fill16(Convert.ToString(Convert.ToInt32(y, 2), 16)).ToUpper();
                textBox1.Text = a;
               
                try
                {
                    Byte[] bit = strToToHexByte(a);
                    sp.Write(bit, 0, 4);                                             //向串口写入 
                    Byte[] buffer = new Byte[1];
                    sp.Read(buffer, 0, 1);
                    string result = byteToHexStr(buffer);
                    if (b == result)                                 //与单片机返回的数比较
                    {
                        MessageBox.Show(this, "发送成功", ":)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        pictureBox2.Image = Properties.Resources.off;
                        button2.Text = "打开";
                    }
                    else
                    {
                        MessageBox.Show(this, "发送失败", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception)
                {

                    MessageBox.Show(this, "打开失败！", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (I3 == "1")
            {
                
                I3 = "0";
                O3 = "1";
                x = con + z + I8 + I7 + I6 + I5 + I4 + I3 + I2 + I1 + O8 + O7 + O6 + O5 + O4 + O3 + O2 + O1;
                y = I8 + I7 + I6 + I5 + I4 + I3 + I2 + I1;
                a = Convert.ToString(Convert.ToInt32(x, 2), 16).ToUpper();
                b = Fill16(Convert.ToString(Convert.ToInt32(y, 2), 16)).ToUpper();
                textBox1.Text = a;
                
                try
                {
                    Byte[] bit = strToToHexByte(a);
                    sp.Write(bit, 0, 4);                                             //向串口写入 
                    Byte[] buffer = new Byte[1];
                    sp.Read(buffer, 0, 1);
                    string result = byteToHexStr(buffer);
                    if (b == result)                                 //与单片机返回的数比较
                    {
                        MessageBox.Show(this, "发送成功", ":)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        pictureBox3.Image = Properties.Resources.on;
                        button3.Text = "关闭";
                    }
                    else
                    {
                        MessageBox.Show(this, "发送失败", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception)
                {

                    MessageBox.Show(this, "打开失败！", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


            }
            else
            {
               
                I3 = "1";
                O3 = "0";
                x = con + z + I8 + I7 + I6 + I5 + I4 + I3 + I2 + I1 + O8 + O7 + O6 + O5 + O4 + O3 + O2 + O1;
                y = I8 + I7 + I6 + I5 + I4 + I3 + I2 + I1;
                a = Convert.ToString(Convert.ToInt32(x, 2), 16).ToUpper();
                b = Fill16(Convert.ToString(Convert.ToInt32(y, 2), 16)).ToUpper();
                textBox1.Text = a;
                
                try
                {
                    Byte[] bit = strToToHexByte(a);
                    sp.Write(bit, 0, 4);                                             //向串口写入 
                    Byte[] buffer = new Byte[1];
                    sp.Read(buffer, 0, 1);
                    string result = byteToHexStr(buffer);
                    if (b == result)                                 //与单片机返回的数比较
                    {
                        MessageBox.Show(this, "发送成功", ":)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        pictureBox3.Image = Properties.Resources.off;
                        button3.Text = "打开";
                    }
                    else
                    {
                        MessageBox.Show(this, "发送失败", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception)
                {

                    MessageBox.Show(this, "打开失败！", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (I4 == "1")
            {
               
                I4 = "0";
                O4 = "1";
                x = con + z + I8 + I7 + I6 + I5 + I4 + I3 + I2 + I1 + O8 + O7 + O6 + O5 + O4 + O3 + O2 + O1;
                y = I8 + I7 + I6 + I5 + I4 + I3 + I2 + I1;
                a = Convert.ToString(Convert.ToInt32(x, 2), 16).ToUpper();
                b = Fill16(Convert.ToString(Convert.ToInt32(y, 2), 16)).ToUpper();
                textBox1.Text = a;
               
                try
                {
                    Byte[] bit = strToToHexByte(a);
                    sp.Write(bit, 0, 4);                                             //向串口写入 
                    Byte[] buffer = new Byte[1];
                    sp.Read(buffer, 0, 1);
                    string result = byteToHexStr(buffer);
                    if (b == result)
                    {
                        MessageBox.Show(this, "发送成功", ":)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        pictureBox4.Image = Properties.Resources.on;
                        button4.Text = "关闭";
                    }
                    else
                    {
                        MessageBox.Show(this, "发送失败", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception)
                {

                    MessageBox.Show(this, "打开失败！", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


            }
            else
            {
                
                I4 = "1";
                O4 = "0";
                x = con + z + I8 + I7 + I6 + I5 + I4 + I3 + I2 + I1 + O8 + O7 + O6 + O5 + O4 + O3 + O2 + O1;
                y = I8 + I7 + I6 + I5 + I4 + I3 + I2 + I1;
                a = Convert.ToString(Convert.ToInt32(x, 2), 16).ToUpper();
                b = Fill16(Convert.ToString(Convert.ToInt32(y, 2), 16)).ToUpper();
                textBox1.Text = a;
               
                try
                {
                    Byte[] bit = strToToHexByte(a);
                    sp.Write(bit, 0, 4);                                             //向串口写入 
                    Byte[] buffer = new Byte[1];
                    sp.Read(buffer, 0, 1);
                    string result = byteToHexStr(buffer);
                    if (b == result)
                    {
                        MessageBox.Show(this, "发送成功", ":)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        pictureBox4.Image = Properties.Resources.off;
                        button4.Text = "打开";
                    }
                    else
                    {
                        MessageBox.Show(this, "发送失败", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception)
                {

                    MessageBox.Show(this, "打开失败！", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


            }
        }
        private void button11_Click(object sender, EventArgs e)
        {

            if (I5 == "1")
            {

                I5 = "0";
                O5 = "1";
                x = con + z + I8 + I7 + I6 + I5 + I4 + I3 + I2 + I1 + O8 + O7 + O6 + O5 + O4 + O3 + O2 + O1;
                y = I8 + I7 + I6 + I5 + I4 + I3 + I2 + I1;
                a = Convert.ToString(Convert.ToInt32(x, 2), 16).ToUpper();
                b = Fill16(Convert.ToString(Convert.ToInt32(y, 2), 16)).ToUpper();
                textBox1.Text = a;

                try
                {
                    Byte[] bit = strToToHexByte(a);
                    sp.Write(bit, 0, 4);                                             //向串口写入 
                    Byte[] buffer = new Byte[1];
                    sp.Read(buffer, 0, 1);
                    string result = byteToHexStr(buffer);
                    if (b == result)
                    {
                        MessageBox.Show(this, "发送成功", ":)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        pictureBox5.Image = Properties.Resources.on;
                        button11.Text = "关闭";
                    }
                    else
                    {
                        MessageBox.Show(this, "发送失败", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception)
                {

                    MessageBox.Show(this, "打开失败！", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


            }
            else
            {

                I5 = "1";
                O5 = "0";
                x = con + z + I8 + I7 + I6 + I5 + I4 + I3 + I2 + I1 + O8 + O7 + O6 + O5 + O4 + O3 + O2 + O1;
                y = I8 + I7 + I6 + I5 + I4 + I3 + I2 + I1;
                a = Convert.ToString(Convert.ToInt32(x, 2), 16).ToUpper();
                b = Fill16(Convert.ToString(Convert.ToInt32(y, 2), 16)).ToUpper();
                textBox1.Text = a;

                try
                {
                    Byte[] bit = strToToHexByte(a);
                    sp.Write(bit, 0, 4);                                             //向串口写入 
                    Byte[] buffer = new Byte[1];
                    sp.Read(buffer, 0, 1);
                    string result = byteToHexStr(buffer);
                    if (b == result)
                    {
                        MessageBox.Show(this, "发送成功", ":)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        pictureBox5.Image = Properties.Resources.off;
                        button11.Text = "打开";
                    }
                    else
                    {
                        MessageBox.Show(this, "发送失败", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception)
                {

                    MessageBox.Show(this, "打开失败！", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


            }

        }

        private void button12_Click(object sender, EventArgs e)
        {

            if (I6 == "1")
            {

                I6 = "0";
                O6 = "1";
                x = con + z + I8 + I7 + I6 + I5 + I4 + I3 + I2 + I1 + O8 + O7 + O6 + O5 + O4 + O3 + O2 + O1;
                y = I8 + I7 + I6 + I5 + I4 + I3 + I2 + I1;
                a = Convert.ToString(Convert.ToInt32(x, 2), 16).ToUpper();
                b = Fill16(Convert.ToString(Convert.ToInt32(y, 2), 16)).ToUpper();
                textBox1.Text = a;

                try
                {
                    Byte[] bit = strToToHexByte(a);
                    sp.Write(bit, 0, 4);                                             //向串口写入 
                    Byte[] buffer = new Byte[1];
                    sp.Read(buffer, 0, 1);
                    string result = byteToHexStr(buffer);
                    if (b == result)
                    {
                        MessageBox.Show(this, "发送成功", ":)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        pictureBox6.Image = Properties.Resources.on;
                        button12.Text = "关闭";
                    }
                    else
                    {
                        MessageBox.Show(this, "发送失败", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception)
                {

                    MessageBox.Show(this, "打开失败！", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


            }
            else
            {

                I6 = "1";
                O6 = "0";
                x = con + z + I8 + I7 + I6 + I5 + I4 + I3 + I2 + I1 + O8 + O7 + O6 + O5 + O4 + O3 + O2 + O1;
                y = I8 + I7 + I6 + I5 + I4 + I3 + I2 + I1;
                a = Convert.ToString(Convert.ToInt32(x, 2), 16).ToUpper();
                b = Fill16(Convert.ToString(Convert.ToInt32(y, 2), 16)).ToUpper();
                textBox1.Text = a;

                try
                {
                    Byte[] bit = strToToHexByte(a);
                    sp.Write(bit, 0, 4);                                             //向串口写入 
                    Byte[] buffer = new Byte[1];
                    sp.Read(buffer, 0, 1);
                    string result = byteToHexStr(buffer);
                    if (b == result)
                    {
                        MessageBox.Show(this, "发送成功", ":)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        pictureBox6.Image = Properties.Resources.off;
                        button12.Text = "打开";
                    }
                    else
                    {
                        MessageBox.Show(this, "发送失败", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception)
                {

                    MessageBox.Show(this, "打开失败！", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


            }

        }

        private void button13_Click(object sender, EventArgs e)
        {

            if (I7 == "1")
            {

                I7 = "0";
                O7 = "1";
                x = con + z + I8 + I7 + I6 + I5 + I4 + I3 + I2 + I1 + O8 + O7 + O6 + O5 + O4 + O3 + O2 + O1;
                y = I8 + I7 + I6 + I5 + I4 + I3 + I2 + I1;
                a = Convert.ToString(Convert.ToInt32(x, 2), 16).ToUpper();
                b = Fill16(Convert.ToString(Convert.ToInt32(y, 2), 16)).ToUpper();
                textBox1.Text = a;

                try
                {
                    Byte[] bit = strToToHexByte(a);
                    sp.Write(bit, 0, 4);                                             //向串口写入 
                    Byte[] buffer = new Byte[1];
                    sp.Read(buffer, 0, 1);
                    string result = byteToHexStr(buffer);
                    if (b == result)
                    {
                        MessageBox.Show(this, "发送成功", ":)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        pictureBox7.Image = Properties.Resources.on;
                        button13.Text = "关闭";
                    }
                    else
                    {
                        MessageBox.Show(this, "发送失败", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception)
                {

                    MessageBox.Show(this, "打开失败！", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


            }
            else
            {

                I7 = "1";
                O7 = "0";
                x = con + z + I8 + I7 + I6 + I5 + I4 + I3 + I2 + I1 + O8 + O7 + O6 + O5 + O4 + O3 + O2 + O1;
                y = I8 + I7 + I6 + I5 + I4 + I3 + I2 + I1;
                a = Convert.ToString(Convert.ToInt32(x, 2), 16).ToUpper();
                b = Fill16(Convert.ToString(Convert.ToInt32(y, 2), 16)).ToUpper();
                textBox1.Text = a;

                try
                {
                    Byte[] bit = strToToHexByte(a);
                    sp.Write(bit, 0, 4);                                             //向串口写入 
                    Byte[] buffer = new Byte[1];
                    sp.Read(buffer, 0, 1);
                    string result = byteToHexStr(buffer);
                    if (b == result)
                    {
                        MessageBox.Show(this, "发送成功", ":)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        pictureBox7.Image = Properties.Resources.off;
                        button13.Text = "打开";
                    }
                    else
                    {
                        MessageBox.Show(this, "发送失败", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception)
                {

                    MessageBox.Show(this, "打开失败！", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


            }

        }

        private void button14_Click(object sender, EventArgs e)
        {

            if (I8 == "1")
            {

                I8 = "0";
                O8 = "1";
                x = con + z + I8 + I7 + I6 + I5 + I4 + I3 + I2 + I1 + O8 + O7 + O6 + O5 + O4 + O3 + O2 + O1;
                y = I8 + I7 + I6 + I5 + I4 + I3 + I2 + I1;
                a = Convert.ToString(Convert.ToInt32(x, 2), 16).ToUpper();
                b =Fill16( Convert.ToString(Convert.ToInt32(y, 2), 16)).ToUpper();
                textBox1.Text = a;

                try
                {
                    Byte[] bit = strToToHexByte(a);
                    sp.Write(bit, 0, 4);                                             //向串口写入 
                    Byte[] buffer = new Byte[1];
                    sp.Read(buffer, 0, 1);
                    string result = byteToHexStr(buffer);
                    if (b == result)
                    {
                        MessageBox.Show(this, "发送成功", ":)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        pictureBox8.Image = Properties.Resources.on;
                        button14.Text = "关闭";
                    }
                    else
                    {
                        MessageBox.Show(this, "发送失败", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception)
                {

                    MessageBox.Show(this, "打开失败！", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


            }
            else
            {

                I8 = "1";
                O8 = "0";
                x = con + z + I8 + I7 + I6 + I5 + I4 + I3 + I2 + I1 + O8 + O7 + O6 + O5 + O4 + O3 + O2 + O1;
                y = I8 + I7 + I6 + I5 + I4 + I3 + I2 + I1;
                a = Convert.ToString(Convert.ToInt32(x, 2), 16).ToUpper();
                b = Fill16(Convert.ToString(Convert.ToInt32(y, 2), 16)).ToUpper();
                textBox1.Text = a;

                try
                {
                    Byte[] bit = strToToHexByte(a);
                    sp.Write(bit, 0, 4);                                             //向串口写入 
                    Byte[] buffer = new Byte[1];
                    sp.Read(buffer, 0, 1);
                    string result = byteToHexStr(buffer);
                    if (b == result)
                    {
                        MessageBox.Show(this, "发送成功", ":)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        pictureBox8.Image = Properties.Resources.off;
                        button14.Text = "打开";
                    }
                    else
                    {
                        MessageBox.Show(this, "发送失败", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception)
                {

                    MessageBox.Show(this, "打开失败！", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


            }

        }
        private static byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "Design by:周润良", "DGUT", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        

       

        private void button7_Click(object sender, EventArgs e)               //写入地址码
        {
            
            try
            {
                 
                 int bz=Convert.ToInt32(textBox2.Text,10);
                 if (0<=bz&&bz<=255)
                 {
                     string b0 = Fill(Convert.ToString(bz, 2));
                     string b1 = "10111011" + b0;
                     string b2 =  Convert.ToString(bz, 10);
                     string b3 = Convert.ToString(Convert.ToInt32(b1, 2), 16).ToUpper();
                     textBox1.Text = b3;
                     Byte[] bit = strToToHexByte(b3);
                     sp.Write(bit, 0, 2);                                             //向串口写入 
                     Byte[] buffer = new Byte[1];
                     sp.Read(buffer, 0, 1);
                     string result1 = byteToHexStr(buffer);
                     int result2 = Convert.ToInt32(result1,16);
                     string result3 = Convert.ToString(result2,10);
                     textBox1.Text = result3;
                     if (result3 == b2)
                     {
                         MessageBox.Show(this, "写入成功", ":)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                     }
                     else
                     {
                         MessageBox.Show(this, "写入失败", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
                     }
                 }
                else
                     MessageBox.Show(this, "请输入０~２５５的整数", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
               
            }
            catch (Exception)
            {

                MessageBox.Show(this, "写入失败", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            

           
        }

        private void button8_Click(object sender, EventArgs e)　　　　　//读出地址码
        {


                int c0 = 0xcc;
                string c1 = Convert.ToString(c0, 16).ToUpper();
                textBox1.Text = c1;
            try
            {
                Byte[] bit = strToToHexByte(c1);
                sp.Write(bit, 0, 1);                                             //向串口写入 
                Byte[] buffer = new Byte[1];
                sp.Read(buffer, 0, 1);
                string result = byteToHexStr(buffer);
                int re = Convert.ToInt32(result, 16);
                textBox3.Text  =Convert.ToString(re,10);
                textBox4.Text = Convert.ToString(re, 10);
                string c2 = Convert.ToString(re,2);
                z =  Fill(c2);
                MessageBox.Show(this, "读取成功", ":)", MessageBoxButtons.OK, MessageBoxIcon.Information); 
            }
            catch (Exception)
            {

                MessageBox.Show(this, "读取失败", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information); 
            }
        }

        private void button9_Click(object sender, EventArgs e)　　　//查询状态
        {
            try
            {
                string A1 = "1";
                string A2 = "1";
                string A3 = "1";
                string A4 = "1";
                string A5 = "1";
                string A6 = "1";
                string A7 = "1";
                string A8 = "1";
               
                string d1 = "11011101" + z;
                int d2 = Convert.ToInt32(d1,2);
                string d3 = Convert.ToString(d2, 16).ToUpper();

                textBox1.Text = d3;
                Byte[] bit = strToToHexByte(d3);
                sp.Write(bit, 0, 2); 
                Byte[] buffer = new Byte[1];
                sp.Read(buffer, 0, 1);
                string result = byteToHexStr(buffer);
                int cx = Convert.ToInt32(result, 16);
                string cxx = Fill(Convert.ToString(cx, 2));
                //textBox1.Text = cxx;
                A1 = cxx.Substring(7,1);
                A2 = cxx.Substring(6,1);
                A3 = cxx.Substring(5,1);
                A4 = cxx.Substring(4,1);
                A5 = cxx.Substring(3, 1);
                A6 = cxx.Substring(2, 1);
                A7 = cxx.Substring(1, 1);
                A8 = cxx.Substring(0, 1);
                //textBox1.Text = A8;
                if (A1 == "0")
                {
                    pictureBox1.Image = Properties.Resources.on;
                }
                else
                {
                    pictureBox1.Image = Properties.Resources.off;
                };
                if (A2 == "0")
                {
                    pictureBox2.Image = Properties.Resources.on;
                }
                else
                {
                    pictureBox2.Image = Properties.Resources.off;
                };
                if (A3 == "0")
                {
                    pictureBox3.Image = Properties.Resources.on;
                }
                else
                {
                    pictureBox3.Image = Properties.Resources.off;
                };
                if (A4 == "0")
                {
                    pictureBox4.Image = Properties.Resources.on;
                }
                else
                {
                    pictureBox4.Image = Properties.Resources.off;
                };
                if (A5 == "0")
                {
                    pictureBox5.Image = Properties.Resources.on;
                }
                else
                {
                    pictureBox5.Image = Properties.Resources.off;
                };
                if (A6 == "0")
                {
                    pictureBox6.Image = Properties.Resources.on;
                }
                else
                {
                    pictureBox6.Image = Properties.Resources.off;
                };
                if (A7 == "0")
                {
                    pictureBox7.Image = Properties.Resources.on;
                }
                else
                {
                    pictureBox7.Image = Properties.Resources.off;
                };
                if (A8 == "0")
                {
                    pictureBox8.Image = Properties.Resources.on;
                }
                else
                {
                    pictureBox8.Image = Properties.Resources.off;
                };
                MessageBox.Show(this, "查询成功", ":)", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {

                MessageBox.Show(this, "查询失败", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
           
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        public static string Fill(string pStrSource)                //字符串转换为8位二进制码表示
        {
            switch (pStrSource.Length)
            {
                case 1: return "0000000" + pStrSource;
                case 2: return "000000" + pStrSource;
                case 3: return "00000" + pStrSource;
                case 4: return "0000" + pStrSource;
                case 5: return "000" + pStrSource;
                case 6: return "00" + pStrSource;
                case 7: return "0" + pStrSource;
                default: return pStrSource;
            }
        }
        public static string Fill16(string pStrSource)                //字符串转换为2位十六进制码表示
        {
            switch (pStrSource.Length)
            {
                case 1: return "0" + pStrSource;
                default: return pStrSource;
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                int e0 = Convert.ToInt32(textBox4.Text, 10);
                string e1 = Fill(Convert.ToString(e0, 2));
                z = e1;
                MessageBox.Show(this, "更改成功", ":)", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {

                MessageBox.Show(this, "更改失败：请输入ID", ":(", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
             
                     
        }

        

       

        

        
       


        
      
    }
}
