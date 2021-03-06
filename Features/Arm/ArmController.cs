﻿//#define DISABLE_SHOW_MESSAGE
#define USE_SDK_RELATIVE
#define USE_MOTION_STATE_WAIT

#if (DISABLE_SHOW_MESSAGE)
#warning Message is disabled.
#endif

using SDKHrobot;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace Features
{
    #region - 列舉 enum -

    /// <summary>
    /// 坐標類型。
    /// </summary>
    public enum CoordinateType
    {
        /// <summary>
        /// 絕對坐標。
        /// </summary>
        Absolute,

        /// <summary>
        /// 相對坐標。
        /// </summary>
        Relative,

        /// <summary>
        /// 未知的類型。
        /// </summary>
        Unknown
    }

    /// <summary>
    /// 運動類型。
    /// </summary>
    public enum MotionType
    {
        /// <summary>
        /// 直線運動。
        /// </summary>
        Linear,

        /// <summary>
        /// 點對點運動。
        /// </summary>
        PointToPoint,

        /// <summary>
        /// 圓弧運動。
        /// </summary>
        Circle,

        /// <summary>
        /// 未知的類型。
        /// </summary>
        Unknown
    }

    /// <summary>
    /// 位置類型。
    /// </summary>
    public enum PositionType
    {
        /// <summary>
        /// 笛卡爾。
        /// </summary>
        Descartes,

        /// <summary>
        /// 關節。
        /// </summary>
        Joint,

        /// <summary>
        /// 未知的類型。
        /// </summary>
        Unknown
    }

    /// <summary>
    /// 手臂平滑模式。
    /// </summary>
    public enum SmoothType
    {
        /// <summary>
        /// 關閉平滑功能。
        /// </summary>
        Disable = 0,

        /// <summary>
        /// 貝茲曲線平滑百分比。
        /// </summary>
        BezierCurveSmoothPercent = 1,

        /// <summary>
        /// 貝茲曲線平滑半徑。
        /// </summary>
        BezierCurveSmoothRadius = 2,

        /// <summary>
        /// 依兩線段速度平滑。
        /// </summary>
        TwoLinesSpeedSmooth = 3
    }

    #endregion - 列舉 enum -

    /// <summary>
    /// 上銀機械手臂控制介面。
    /// </summary>
    public interface IArmController : IDevice
    {
        /// <summary>
        /// 手臂ID。
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// 手臂IP。
        /// </summary>
        string Ip { get; set; }

        #region - Default Position -

        /// <summary>
        /// 笛卡爾原點絕對坐標。
        /// </summary>
        double[] DescartesHomePosition { get; }

        /// <summary>
        /// 關節原點絕對坐標。
        /// </summary>
        double[] JointHomePosition { get; }

        #endregion - Default Position -

        #region - Speed and Acceleration -

        /// <summary>
        /// 整體加速度比例。<br/>
        /// 正常數值爲 1 ~ 100，-1 代表取得數值時出錯。
        /// </summary>
        int Acceleration { get; set; }

        /// <summary>
        /// 整體速度比例。<br/>
        /// 正常數值爲 1 ~ 100，-1 代表取得數值時出錯。
        /// </summary>
        int Speed { get; set; }

        #endregion - Speed and Acceleration -

        #region - Motion -

        /// <summary>
        /// 回到指定座標系的原點。預設爲笛卡爾。
        /// </summary>
        /// <param name="positionType"></param>
        void Homing(PositionType positionType = PositionType.Descartes,
                    bool waitForMotion = true);

        /// <summary>
        /// 進行直線運動。<br/>
        /// ● targetPosition：目標位置。<br/>
        /// ● positionType：位置類型，笛卡爾或關節。預設爲笛卡爾。<br/>
        /// ● coordinateType：坐標類型，絕對坐標或相對坐標。預設為絕對坐標。<br/>
        /// ● smoothType：平滑模式類型。預設為依兩線段速度平滑。<br/>
        /// ● smoothValue：平滑值。預設為50。<br/>
        /// ● waitForMotion：是否等待動作完成。預設為true。
        /// </summary>
        /// <param name="targetPosition"></param>
        /// <param name="positionType"></param>
        /// <param name="coordinateType"></param>
        /// <param name="smoothType"></param>
        /// <param name="smoothValue"></param>
        /// <param name="waitForMotion"></param>
        void MoveLinear(double[] targetPosition,
                        PositionType positionType = PositionType.Descartes,
                        CoordinateType coordinateType = CoordinateType.Absolute,
                        SmoothType smoothType = SmoothType.TwoLinesSpeedSmooth,
                        double smoothValue = 50,
                        bool waitForMotion = true);

        /// <summary>
        /// 進行點對點運動。<br/>
        /// ● targetPosition：目標位置。<br/>
        /// ● positionType：位置類型，笛卡爾或關節。預設爲笛卡爾。<br/>
        /// ● coordinateType：坐標類型，絕對坐標或相對坐標。預設為絕對坐標。<br/>
        /// ● smoothType：平滑模式類型。預設為依兩線段速度平滑。<br/>
        /// ● waitForMotion：是否等待動作完成。預設為true。
        /// </summary>
        /// <param name="targetPosition"></param>
        /// <param name="positionType"></param>
        /// <param name="coordinateType"></param>
        /// <param name="smoothType"></param>
        /// <param name="waitForMotion"></param>
        void MovePointToPoint(double[] targetPosition,
                              PositionType positionType = PositionType.Descartes,
                              CoordinateType coordinateType = CoordinateType.Absolute,
                              SmoothType smoothType = SmoothType.TwoLinesSpeedSmooth,
                              bool waitForMotion = true);

        #endregion - Motion -

        #region - Others -

        /// <summary>
        /// 清除警報。
        /// </summary>
        void ClearAlarm();

        /// <summary>
        /// 取得手臂目前的位置座標數值。<br/>
        /// 須在引數選擇位置類型是笛卡爾還是關節。預設爲笛卡爾。
        /// </summary>
        /// <param name="positionType"></param>
        /// <returns>目前的手臂位置座標數值。</returns>
        double[] GetPosition(PositionType positionType = PositionType.Descartes);

        #endregion - Others -
    }

    /// <summary>
    /// 上銀機械手臂控制實作。
    /// </summary>
    public class ArmController : IArmController
    {
        public ArmController(string armIp, IMessage message)
        {
            Ip = armIp;
            Id = 0;
            Message = message;

#if (!USE_MOTION_STATE_WAIT)
            InitTimer();
#endif
        }

        public int Id { get; set; }
        public string Ip { get; set; }

        #region - Default Position -

        public double[] DescartesHomePosition { get; } = { 0, 368, 294, 180, 0, 90 };
        public double[] JointHomePosition { get; } = { 0, 0, 0, 0, 0, 0 };

        #endregion - Default Position -

        #region - Speed and Acceleration -

        public int Acceleration
        {
            get
            {
                int acc;
                if (Connected)
                {
                    acc = HRobot.get_acc_dec_ratio(Id);
                    if (acc == -1)
                    {
                        Message.Show("取得手臂加速度時出錯。", LoggingLevel.Error);
                    }
                }
                else
                {
                    acc = -1;
                    Message.Show("手臂未連線。", LoggingLevel.Info);
                }
                return acc;
            }

            set
            {
                if (value > 100 || value < 1)
                {
                    Message.Show("手臂加速度應為1% ~ 100%之間。", LoggingLevel.Info);
                }
                else
                {
                    if (Connected)
                    {
                        int retuenCode = HRobot.set_acc_dec_ratio(Id, value);

                        // 執行HRobot.set_acc_dec_ratio時會固定回傳錯誤代碼4000。
                        IsErrorAndHandler(retuenCode, 4000);
                    }
                    else
                    {
                        Message.Show("手臂未連線。", LoggingLevel.Info);
                    }
                }
            }
        }

        public int Speed
        {
            get
            {
                int speed;
                if (Connected)
                {
                    speed = HRobot.get_override_ratio(Id);
                    if (speed == -1)
                    {
                        Message.Show("取得手臂速度時出錯。", LoggingLevel.Error);
                    }
                }
                else
                {
                    speed = -1;
                    Message.Show("手臂未連線。", LoggingLevel.Info);
                }
                return speed;
            }

            set
            {
                if (value > 100 || value < 1)
                {
                    Message.Show("手臂速度應為1% ~ 100%之間。", LoggingLevel.Info);
                }
                else
                {
                    if (Connected)
                    {
                        int retuenCode = HRobot.set_override_ratio(Id, value);
                        IsErrorAndHandler(retuenCode);
                    }
                    else
                    {
                        Message.Show("手臂未連線。", LoggingLevel.Info);
                    }
                }
            }
        }

        #endregion - Speed and Acceleration -

        #region - Motion -

        public void Homing(PositionType positionType = PositionType.Descartes,
                           bool waitForMotion = true)
        {
            Message.Log($"Arm-Homing. {positionType}", LoggingLevel.Trace);
            int returnCode;
            switch (positionType)
            {
                case PositionType.Descartes:
                    returnCode = HRobot.ptp_pos(Id, (int)SmoothType.Disable, DescartesHomePosition);
                    if ((returnCode >= 0) && waitForMotion)
                    {
                        WaitForMotionComplete(DescartesHomePosition, positionType);
                    }
                    break;

                case PositionType.Joint:
                    returnCode = HRobot.ptp_axis(Id, (int)SmoothType.Disable, JointHomePosition);
                    if ((returnCode >= 0) && waitForMotion)
                    {
                        WaitForMotionComplete(JointHomePosition, positionType);
                    }
                    break;

                default:
                    ShowUnknownPositionType();
                    return;
            }
        }

        public void MoveLinear(double[] targetPosition,
                               PositionType positionType = PositionType.Descartes,
                               CoordinateType coordinateType = CoordinateType.Absolute,
                               SmoothType smoothType = SmoothType.TwoLinesSpeedSmooth,
                               double smoothValue = 50,
                               bool waitForMotion = true)
        {
            Message.Log($"Arm-Linear: {GetTextPositin(targetPosition)}. {positionType}",
                        LoggingLevel.Trace);
            int retuenCode = 0;

#if (USE_SDK_RELATIVE)
            if (coordinateType == CoordinateType.Absolute)
            {
                switch (positionType)
                {
                    case PositionType.Descartes:
                        retuenCode = HRobot.lin_pos(Id, (int)smoothType, smoothValue, targetPosition);
                        break;

                    case PositionType.Joint:
                        retuenCode = HRobot.lin_axis(Id, (int)smoothType, smoothValue, targetPosition);
                        break;

                    default:
                        ShowUnknownPositionType();
                        return;
                }
            }
            else if (coordinateType == CoordinateType.Relative)
            {
                switch (positionType)
                {
                    case PositionType.Descartes:
                        retuenCode = HRobot.lin_rel_pos(Id, (int)smoothType, smoothValue, targetPosition);
                        break;

                    case PositionType.Joint:
                        retuenCode = HRobot.lin_rel_axis(Id, (int)smoothType, smoothValue, targetPosition);
                        break;

                    default:
                        ShowUnknownPositionType();
                        return;
                }
            }
#else
            if (coordinateType == CoordinateType.relative)
            {
                targetPosition = ConvertRelativeToAdsolute(targetPosition, positionType);
            }

            switch (positionType)
            {
                case PositionType.descartes:
                    retuenCode = HRobot.lin_pos(DeviceID, (int)smoothType, smoothValue, targetPosition);
                    break;

                case PositionType.joint:
                    retuenCode = HRobot.lin_axis(DeviceID, (int)smoothType, smoothValue, targetPosition);
                    break;

                default:
                    ShowUnknownPositionType();
                    return;
            }
#endif

            if (!IsErrorAndHandler(retuenCode) && waitForMotion)
            {
                WaitForMotionComplete(targetPosition, positionType);
            }
        }

        public void MovePointToPoint(double[] targetPosition,
                                     PositionType positionType = PositionType.Descartes,
                                     CoordinateType coordinateType = CoordinateType.Absolute,
                                     SmoothType smoothType = SmoothType.TwoLinesSpeedSmooth,
                                     bool waitForMotion = true)
        {
            Message.Log($"Arm-PointToPoint: {GetTextPositin(targetPosition)}. {positionType}",
                        LoggingLevel.Trace);
            int retuenCode = 0;
            int smoothTypeCode = (smoothType == SmoothType.TwoLinesSpeedSmooth) ? 1 : 0;

#if (USE_SDK_RELATIVE)
            if (coordinateType == CoordinateType.Absolute)
            {
                switch (positionType)
                {
                    case PositionType.Descartes:
                        retuenCode = HRobot.ptp_pos(Id, smoothTypeCode, targetPosition);
                        break;

                    case PositionType.Joint:
                        retuenCode = HRobot.ptp_axis(Id, smoothTypeCode, targetPosition);
                        break;

                    default:
                        ShowUnknownPositionType();
                        return;
                }
            }
            else if (coordinateType == CoordinateType.Relative)
            {
                switch (positionType)
                {
                    case PositionType.Descartes:
                        retuenCode = HRobot.ptp_rel_pos(Id, smoothTypeCode, targetPosition);
                        break;

                    case PositionType.Joint:
                        retuenCode = HRobot.ptp_rel_axis(Id, smoothTypeCode, targetPosition);
                        break;

                    default:
#if (!DISABLE_SHOW_MESSAGE)
                        ShowUnknownPositionType();
#endif
                        return;
                }
            }
#else
            if (coordinateType == CoordinateType.relative)
            {
                targetPosition = ConvertRelativeToAdsolute(targetPosition, positionType);
            }

            switch (positionType)
            {
                case PositionType.descartes:
                    retuenCode = HRobot.ptp_pos(DeviceID, smoothTypeCode, targetPosition);
                    break;

                case PositionType.joint:
                    retuenCode = HRobot.ptp_axis(DeviceID, smoothTypeCode, targetPosition);
                    break;

                default:
                    ShowUnknownPositionType();
                    retuen;
            }
#endif

            if (!IsErrorAndHandler(retuenCode) && waitForMotion)
            {
                WaitForMotionComplete(targetPosition, positionType);
            }
        }

        /// <summary>
        /// 將相對坐標以目前位置轉為絕對坐標。
        /// </summary>
        /// <param name="relativePosition"></param>
        /// <param name="positionType"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private double[] ConvertRelativeToAdsolute(double[] relativePosition,
                                                   PositionType positionType)
        {
            double[] position = GetPosition(positionType);
            for (int i = 0; i < 6; i++)
            {
                position[i] += relativePosition[i];
            }
            return position;
        }

        private string GetTextPositin(double[] position)
        {
            string stringPos = "\"";
            foreach (double val in position)
            {
                stringPos += val.ToString() + ",";
            }
            stringPos = stringPos.TrimEnd(new char[] { ' ', ',' });
            stringPos += "\"";
            return stringPos;
        }

        /// <summary>
        /// 等待手臂運動完成。
        /// </summary>
        /// <param name="targetPosition"></param>
        /// <param name="positionType"></param>
        private void WaitForMotionComplete(double[] targetPosition, PositionType positionType)
        {
#if (USE_MOTION_STATE_WAIT)
            while (true)
            {
                // motion_state = 1: Idle.
                if (HRobot.get_motion_state(Id) != 1)
                {
                    Thread.Sleep(200);
                }
                else
                {
                    break;
                }
            }
#else
            double[] nowPosition = new double[6];

            ActionTimer.Enabled = true;

            // For無限回圈。
            for (; ; )
            {
                if (TimeCheck % 1 == 0 && positionType == PositionType.Descartes)
                {
                    foreach (int k in nowPosition)
                    {
                        // 取得目前的笛卡爾坐標。
                        HRobot.get_current_position(Id, nowPosition);
                    }

                    if (Math.Abs(targetPosition[0] - nowPosition[0]) < 0.01 &&
                        Math.Abs(targetPosition[1] - nowPosition[1]) < 0.01 &&
                        Math.Abs(targetPosition[2] - nowPosition[2]) < 0.01 &&
                        Math.Abs(Math.Abs(targetPosition[3]) - Math.Abs(nowPosition[3])) < 0.01 &&
                        Math.Abs(Math.Abs(targetPosition[4]) - Math.Abs(nowPosition[4])) < 0.01 &&
                        Math.Abs(Math.Abs(targetPosition[5]) - Math.Abs(nowPosition[5])) < 0.01)
                    {
                        // 跳出For無限回圈。
                        break;
                    }
                }
                else if (TimeCheck % 1 == 0 && positionType == PositionType.Joint)
                {
                    foreach (int k in nowPosition)
                    {
                        // 取得目前的關節坐標。
                        HRobot.get_current_joint(Id, nowPosition);
                    }

                    if (Math.Abs(targetPosition[0] - nowPosition[0]) < 0.01 &&
                        Math.Abs(targetPosition[1] - nowPosition[1]) < 0.01 &&
                        Math.Abs(targetPosition[2] - nowPosition[2]) < 0.01 &&
                        Math.Abs(targetPosition[3] - nowPosition[3]) < 0.01 &&
                        Math.Abs(targetPosition[4] - nowPosition[4]) < 0.01 &&
                        Math.Abs(targetPosition[5] - nowPosition[5]) < 0.01)
                    {
                        // 跳出For無限回圈。
                        break;
                    }
                }
            }

            ActionTimer.Enabled = false;
            TimeCheck = 0;
#endif
        }

        #endregion - Motion -

        #region - Connect and Disconnect -

        /// <summary>
        /// 此 delegate 必須要是 static，否則手臂動作有可能會出現問題。
        /// </summary>
        private static HRobot.CallBackFun CallBackFun;

        public bool Connected { get; private set; } = false;

        public bool Connect()
        {
            //接收控制器回傳訊息
            CallBackFun = new HRobot.CallBackFun(EventFun);

            //連線設定。測試連線設定:("127.0.0.1", 1, CallBackFun);
            Id = HRobot.open_connection(Ip, 1, CallBackFun);
            Thread.Sleep(500);

            //0 ~ 65535為有效裝置ID
            if (Id >= 0 && Id <= 65535)
            {
                int alarmState;
                int motorState;
                int connectionLevel;

                //清除錯誤
                alarmState = HRobot.clear_alarm(Id);

                //錯誤代碼300代表沒有警報，無法清除警報
                if (alarmState == 300)
                {
                    alarmState = 0;
                }

                //設定控制器: 1為啟動,0為關閉
                HRobot.set_motor_state(Id, 1);
                Thread.Sleep(500);

                //回傳控制器狀態
                motorState = HRobot.get_motor_state(Id);

                connectionLevel = HRobot.get_connection_level(Id);

                string text = string.Format("連線成功!\r\n" +
                                            "手臂ID: {0}\r\n" +
                                            "連線等級: {1}\r\n" +
                                            "控制器狀態: {2}\r\n" +
                                            "錯誤代碼: {3}\r\n",
                                            Id,
                                            (connectionLevel == 0) ? "觀測者" : "操作者",
                                            (motorState == 0) ? "關閉" : "開啟",
                                            alarmState);

                Message.Show(text, "連線", MessageBoxButtons.OK, MessageBoxIcon.None);

                Connected = true;
                return true;
            }
            else
            {
                string message;

                switch (Id)
                {
                    case -1:
                        message = "-1：連線失敗。";
                        break;

                    case -2:
                        message = "-2：回傳機制創建失敗。";
                        break;

                    case -3:
                        message = "-3：無法連線至Robot。";
                        break;

                    case -4:
                        message = "-4：版本不相符。";
                        break;

                    default:
                        message = $"未知的錯誤代碼： {Id}";
                        break;
                }

                Message.Show($"無法連線!\r\n{message}", LoggingLevel.Error);

                Connected = false;
                return false;
            }
        }

        public bool Disconnect()
        {
            int alarmState;
            int motorState;

            //設定控制器: 1為啟動,0為關閉
            HRobot.set_motor_state(Id, 0);
            Thread.Sleep(500);

            //將所有錯誤代碼清除
            alarmState = HRobot.clear_alarm(Id);

            //錯誤代碼300代表沒有警報，無法清除警報
            if (alarmState == 300)
            {
                alarmState = 0;
            }

            //回傳控制器狀態
            motorState = HRobot.get_motor_state(Id);

            //關閉手臂連線
            HRobot.disconnect(Id);

            string text = string.Format("斷線成功!\r\n" +
                                        "控制器狀態: {0}\r\n" +
                                        "錯誤代碼: {1}\r\n",
                                        (motorState == 0) ? "關閉" : "開啟",
                                        alarmState);

            Message.Show(text, "斷線", MessageBoxButtons.OK, MessageBoxIcon.None);

            Connected = false;
            return true;
        }

        /// <summary>
        /// 控制器回傳。
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="rlt"></param>
        /// <param name="Msg"></param>
        /// <param name="len"></param>
        private static void EventFun(UInt16 cmd, UInt16 rlt, ref UInt16 Msg, int len)
        {
            Console.WriteLine("Command: " + cmd + " Resault: " + rlt);

            switch (cmd)
            {
                case 4011:
                    if (rlt != 0)
                    {
#if (!DISABLE_SHOW_MESSAGE)
                        // XXX 此處不受訊息控制器影響。
                        MessageBox.Show("Update fail. " + rlt,
                                        "HRSS update callback",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning,
                                        MessageBoxDefaultButton.Button1,
                                        MessageBoxOptions.DefaultDesktopOnly);
#endif
                    }
                    break;
            }
        }

        #endregion - Connect and Disconnect -

        #region - Timer -

        private Timer ActionTimer = null;
        private int TimeCheck = 0;

        /// <summary>
        /// 初始化計時器。
        /// </summary>
        private void InitTimer()
        {
            ActionTimer = new Timer { Interval = 50, Enabled = false };
            ActionTimer.Tick += (s, e) => { ++TimeCheck; };
        }

        #endregion - Timer -

        #region - Message -

        private IMessage Message { get; set; }

        /// <summary>
        /// 如果出現錯誤，顯示錯誤代碼。
        /// </summary>
        /// <param name="code"></param>
        /// <param name="successCode"></param>
        /// <param name="ignoreCode"></param>
        /// <returns>
        /// 是否出現錯誤。<br/>
        /// ● true：出現錯誤。<br/>
        /// ● false：沒有出現錯誤。
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsErrorAndHandler(int code, int ignoreCode = 0, int successCode = 0)
        {
            if (code == successCode || code == ignoreCode)
            {
                // Successful.
                return false;
            }
            else
            {
                // Not successful.
                Message.Show($"上銀機械手臂控制錯誤。\r\n錯誤代碼：{code}", LoggingLevel.Error);
                return true;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ShowUnknownPositionType()
        {
            Message.Show($"錯誤的位置類型。\r\n" +
                         $"位置類型應為：{PositionType.Descartes} 或是 {PositionType.Joint}",
                         LoggingLevel.Warn);
        }

        #endregion - Message -

        #region - Others -

        public void ClearAlarm()
        {
            int retuenCode = HRobot.clear_alarm(Id);

            // 錯誤代碼300代表沒有警報，無法清除警報
            IsErrorAndHandler(retuenCode, 300);
        }

        public double[] GetPosition(PositionType type = PositionType.Descartes)
        {
            double[] position = new double[6];
            int retuenCode = -1;

            foreach (int k in position)
            {
                if (type == PositionType.Descartes)
                {
                    retuenCode = HRobot.get_current_position(Id, position);
                }
                else if (type == PositionType.Joint)
                {
                    retuenCode = HRobot.get_current_joint(Id, position);
                }
                else
                {
                    ShowUnknownPositionType();
                    return position;
                }
            }
            IsErrorAndHandler(retuenCode);
            return position;
        }

        #endregion - Others -
    }
}