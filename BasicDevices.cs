﻿//#define DISABLE_SHOW_MESSAGE

#if (DISABLE_SHOW_MESSAGE)
#warning Message is disabled.
#endif

using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HiwinRobot
{
    public interface IDevice
    {
        /// <summary>
        /// Connect state. <br/>
        /// • true: Connected. <br/>
        /// • false: Disconnected or unknown. <br/>
        /// </summary>
        bool Connected { get; }

        /// <summary>
        /// Connect.<br/>
        /// </summary>
        /// <returns>
        /// true: Connection successful.<br/>
        /// false: Connection unsuccessful.
        /// </returns>
        bool Connect();

        /// <summary>
        /// Disconnect.<br/>
        /// </summary>
        /// <returns>
        /// true: Disconnection successful.<br/>
        /// false: Disconnection unsuccessful.
        /// </returns>
        bool Disconnect();
    }

    public interface ISerialPortDevice : IDevice
    {
        SerialPort SerialPort { get; set; }
    }

    public class SerialPortDevice : ISerialPortDevice
    {
        private IMessage ErrorMessage = new ErrorMessage();

        public SerialPortDevice(SerialPort serialPort)
        {
            // XXX 此處沒有使用深層複製，需注意指標(pointer)的問題。
            SerialPort = serialPort;
        }

        public SerialPortDevice(string comPort)
        {
            SerialPort = new SerialPort(comPort);
        }

        public bool Connected { get; private set; } = false;

        public SerialPort SerialPort { get; set; }

        public virtual bool Connect()
        {
            if (!SerialPort.IsOpen)
            {
                try
                {
                    SerialPort.Open();
                    Thread.Sleep(50);
                    if (SerialPort.IsOpen)
                    {
                        Connected = true;
                        return true;
                    }
                    else
                    {
                        Connected = false;
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage.Show("無法進行連線。\r\n請檢查COM Port等設定。", ex);
                    return false;
                }
            }
            else
            {
                Connected = true;
                return true;
            }
        }

        public virtual bool Disconnect()
        {
            if (SerialPort.IsOpen)
            {
                try
                {
                    SerialPort.Close();
                    Thread.Sleep(50);
                    if (SerialPort.IsOpen)
                    {
                        Connected = true;
                        return false;
                    }
                    else
                    {
                        Connected = false;
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage.Show("無法進行斷線。\r\n請檢查COM Port等設定。", ex);
                    return false;
                }
            }
            else
            {
                Connected = false;
                return true;
            }
        }
    }
}