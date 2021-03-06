﻿using ControlPlatformLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using WorldGeneralLib.SerialPorts;
using System.Windows.Forms;
namespace FullyAutomaticLaserJetCoder.MainTask
{
    public class RunClass
    {
        private static TaskGroup m_WeldingTaskGroup = new TaskGroup();
        private static RunClass RunC;
        //  public DateSave ProductionDatA = DateSave.Instance();
        List<Date_save> user_Date_Save =new  List<Date_save>();
        public static RunClass Instance()
        {
            if (RunC == null)
            {
                RunC = new RunClass();
            }
            return RunC;
        }
        KeyValuePair<string, string> StepName = new KeyValuePair<string, string>();
        public List<KeyValuePair<string, KeyValuePair<string, string>>> RunItem_List = new List<KeyValuePair<string, KeyValuePair<string, string>>>();
        public List<KeyValuePair<string, string>> RunItem_List1111 = new List<KeyValuePair<string, string>>();
        public string WeldPlat_Str_Name = "运动平台";
        public string stfPath = System.Environment.CurrentDirectory + "\\FlowDocument\\";
        public AxisRun AxisR = AxisRun.Instance();
        public ClinderRun ClinderR = ClinderRun.Instance();
     //   public static ComeOut_process ComeOut_pro;
        public Method Meth = Method.Instance();

        CancellationTokenSource TaskCancelSource = new CancellationTokenSource();
        CancellationToken CancelToken;
        ManualResetEvent ResetEvent = new ManualResetEvent(true);
        public int delayCheckTime = 6000;
        public int RunMark = 0;
        public List<string[]> WeldListDate = new List<string[]>();
        string[] WeldListDateMes = new string[100];
        public RunClass()
        {
            for (int i=0; i < 30; i++)
            {
                Date_save Date_ = new Date_save();
                user_Date_Save.Add(Date_);
            }
           

            // ReadCode(stfPath);
        }

        public bool parse = false;//暂停标志位
        List<string> ListStr = new List<string>();
        public bool RunClass_IsFinish = false;
      // public bool Stop = false;//急停
     // public bool IsStop = false;//停止
        public bool GoOnRun = false;//继续运行标志位
        public void ReadCode(string path)// Read  code
        {
            List<string> ReadListStr = new List<string>();
            StreamReader sr = new StreamReader(path, Encoding.Default);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                ReadListStr.Add(line);
            }
            for (int i = 1; i < ReadListStr.Count; i++)
            {
                string[] Code = ReadListStr[i].Split(',');
                if (Code[0].Contains("轴运动"))
                {
                    RunItem_List.Add(new KeyValuePair<string, KeyValuePair<string, string>>(Code[0], new KeyValuePair<string, string>(Code[1], Code[2])));
                  //  RunItem_List1111.Add(new KeyValuePair<string, string>(Code[1], ""));
                  //  ListStr.Add(Code[1]);

                }
                else
                {
                    RunItem_List.Add(new KeyValuePair<string, KeyValuePair<string, string>>(Code[0], new KeyValuePair<string, string>(Code[1], Code[2])));
                   // RunItem_List1111.Add(new KeyValuePair<string, string>(Code[1], Code[2]));
                    //ListStr.Add(Code[1]);
                }
            }
            //for (int i = 0; i < RunItem_List.Count; i++)
            //{
            //    string sda = RunItem_List[i].Key;
            //  //  sssf = RunItem_List[i].Value;
            //    //  sssf.Key
            //}
        }
        public Thread Run_OneCase = null;
        public void runTask(string path)
        {
            RunClass_IsFinish = false;
           // bool runFinish = false;
            RunItem_List.Clear();
            ReadCode(path);// Read  code
            Run_OneCase = new Thread(Run);
            Run_OneCase.IsBackground = true;
            Run_OneCase.Start();
            //Task tskExecute = new Task(() =>
            //{
            //    Run();
            //    while (true)
            //    {
            //        if (RunClass_IsFinish == true)
            //        {
            //            runFinish = true;
            //            break;
            //        }

            //    }
            //});
            //tskExecute.Start();
        }
        public bool StartRun = false;
        public void Run()
        {
            DateSave.Instance().Production.WeldDate.Clear();
            HighDate.Clear();
            CamerDate.Clear();
            WeldListDate.Clear();
            StartRun = true;
            GoOnRun = false;//继续运行标志位     
            RunClass_IsFinish = false;
            CamerFlow = 0;
            AutoWeld = 0;
            AutoHigh = 0;
            for (int i = 0; i < RunItem_List.Count; i++)
            {
                RunMark = i;
                while (true)
                {
                    if (DateSave.Instance().Production.IsStop == true)
                    {
                        i = 2000;

                        GoOnRun = false;
                        parse = false;//运动超时报警
                      //  ClinderR.Stop = true;
                        //AxisR.Stop = true;
                       // Meth.Stop = true;
                        break;
                    }
                   // if (DateSave.Instance().Production.EStop == true)
                    if (DateSave.Instance().Production.EStop == true)
                    {
                        i = 2000;
                    
                        GoOnRun = false;
                        parse = false;//运动超时报警
                        //ClinderR.Stop = true;
                       // AxisR.Stop = true;
                       // Meth.Stop = true;
                        break;
                 
                    }
                    //if (DateSave.Instance().Production.Door_Enable == true)
                    //{
                    //    parse = true;

                    //    GoOnRun = true;//继续运行标志位

                    //}
                    //else 
                    //{
                    //    if (IOManage.INPUT("DOOR").On)
                    //    {
                    //        Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[门打开]," + "软件暂停运行");
                    //        parse = true;
                    //    }
                    //    else if (IOManage.INPUT("DOOR").Off)
                    //    {
                    //        Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[门关闭]," + "软件继续运行");
                    //        GoOnRun = true;//继续运行标志位
                    //    }
                    //}
                    //else if(IOManage.INPUT("DOOR").On)              
                    if (parse == true)//暂停标志位
                    {
                        if (GoOnRun == true)//继续运行标志位
                        {
                            parse = false;
                            GoOnRun = false;//继续运行标志位
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                if (DateSave.Instance().Production.EStop == false&& DateSave.Instance().Production.IsStop == false)
                {
                    string RunStep = RunItem_List[i].Key;
                    StepName = RunItem_List[i].Value;
                    for (int j = 0; j < 3; j++)
                    {
                        if (DateSave.Instance().Production.IsStop == true)
                        {
                            j = 5;
                            GoOnRun = false;
                            parse = false;//运动超时报警
                         //   ClinderR.Stop = true;
                          //  AxisR.Stop = true;
                          //  Meth.Stop = true;
                            break;
                        }
                        if (DateSave.Instance().Production.EStop == true)
                        {
                            GoOnRun = false;
                            parse = false;//运动超时报警
                          //  ClinderR.Stop = true;
                         //   AxisR.Stop = true;
                         //   Meth.Stop = true;
                            j = 5;
                            break;
                        }
                        //   Weld_Log.Instance().jp_writeLogWithLevel(LOG_LEVEL.LEVEL_3, "[IO检测]," + sda + "_" + sssf.Key + "_" + sssf.Value);
                        bool currentRunStatus = Run_Switch(RunStep, StepName.Key, StepName.Value);
                      
                        if (currentRunStatus == false)
                        {
                            if (j >= 2)
                            {
                                parse = true;//运动超时报警
                                BIZZ(RunStep, StepName.Key, StepName.Value);                          
                               // GoOnRun = false;//继续运行标志位                    
                                i = i - 1;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }



            }
            Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[运行完成],");

            //IsStop = false;
          //  Stop = false;
          //  AxisR.IsStop = false;
         //   AxisR.Stop = false;
         //   ClinderR.IsStop = false;
         //   ClinderR.Stop = false;
          //  Meth.IsStop = false;
         //   Meth.Stop = false;
            RunClass_IsFinish = true;
            StartRun = false;
            DateSave.Instance().Production.IsStop = false;
            //    Thread.Sleep(100);
            Run_OneCase.Abort();
        }
        public void BIZZ(string NAME, string ERR,string value)
        {
            MessageAlarmForm AlarmForm = new MessageAlarmForm();
            Thread Bizz = new Thread(BIZZRun);
            Bizz.IsBackground = true;
            Bizz.Start();
            AlarmForm.InputBox(NAME, ERR, value);
      
            Bizz.Abort();
            Bizz.Join();
            GoOnRun = true;
            if (!MainModule.FormMain.bAuto)
            {
                if (Bizz.IsAlive==true)
                {
                    Bizz.Abort();
                    Bizz.Join();
                }
                Meth.OutPut_One_Run("三色灯黄", "true");
                Meth.OutPut_One_Run("BIZZ", "false");
                Meth.OutPut_One_Run("三色灯红", "false");
                Meth.OutPut_One_Run("三色灯绿", "false");



                Meth.OutPut_One_Run("三色灯黄", "true");
                Meth.OutPut_One_Run("BIZZ", "false");
                Meth.OutPut_One_Run("三色灯红", "false");
                Meth.OutPut_One_Run("三色灯绿", "false");
            }
            else
            {
                if (Bizz.IsAlive == true)
                {
                    Bizz.Abort();
                    Bizz.Join();
                }
                Meth.OutPut_One_Run("BIZZ", "false");
                Meth.OutPut_One_Run("三色灯红", "false");
                Meth.OutPut_One_Run("三色灯绿", "true");


                Meth.OutPut_One_Run("BIZZ", "false");
                Meth.OutPut_One_Run("三色灯红", "false");
                Meth.OutPut_One_Run("三色灯绿", "true");

            }
        }
        public void BIZZRun()
        {
            Meth.OutPut_One_Run("三色灯红", "true");
            Meth.OutPut_One_Run("三色灯绿", "false");
            while (true)
            {
                Meth.OutPut_One_Run("BIZZ", "true");
                Thread.Sleep(1000);
                Meth.OutPut_One_Run("BIZZ", "false");
                Thread.Sleep(1000);

            }

        }
        KeyValuePair<double, double> Point_1;
        public List<double> HighDate = new List<double>();
        public List<KeyValuePair<double, double>> CamerDate = new List<KeyValuePair<double, double>>();
        List<KeyValuePair<double, double>> CamerDateNeed_Date = new List<KeyValuePair<double, double>>();
        public int CamerFlow;
        public int AutoHigh;
        public int AutoWeld;
        public int DelayCamer = 0;
        public bool Run_Switch(string str, string str1, string CheckSta)
        {
          
            if (MainModule.FormMain.bAuto)
            {
                Meth.OutPut_One_Run("BIZZ", "false");
                Meth.OutPut_One_Run("三色灯红", "false");
                Meth.OutPut_One_Run("三色灯绿", "true");

            }
         
            //  GetTimerStart();
            string[] str_camer_checkNeed = new string[5];
            string[] Offset = new string[5];
            double OffsetX = 0;
            double OffsetY = 0;
            // HighDate.Add(0.0);
            bool currentRunStatus = false;
            if (str.Contains("气缸运动"))
            {
                str = str1;
            }
            else if (str.Contains("拍照定位"))
            {
               
                str = str1;
            }
            else if (str.Contains("焊接定位"))
            {
                str = str1;
            }
            else if (str.Contains("调高定位"))
            {
                str = str1;
            }
            else if (str.Contains("焊接定位"))
            {
                str = str1;
            }
            switch (str)
            {
                case "工位记忆":
                    try
                    {
                        Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[工位有无料记忆]," + "工位有无料记忆");
                        if (CheckSta.ToLower() == "true")
                        {
                            currentRunStatus = true;
                            DateSave.Instance().Production.StationMaterial = true;

                        }
                        else
                        {
                            currentRunStatus = true;
                            DateSave.Instance().Production.StationMaterial = false;
                        }
                    }
                    catch
                    {
                        currentRunStatus = false;
                    }
                    break;
                case "延时":
                    try
                    {
                       int Time= int.Parse(CheckSta.Replace(" ", ""));
                   
                        currentRunStatus = Meth.Delay(Time);
                    }
                    catch
                    {
                        currentRunStatus = false;
                    }               
                    break;
                case "IO检测":
                    delayCheckTime = 6000;
                    m_WeldingTaskGroup.AddRunMessage("[IO检测]," + str1);
                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[IO检测]," + str1);
                    currentRunStatus = Meth.WaitINPut_Check(str1, CheckSta, delayCheckTime);//检测一个输入               
                    break;
                case "IO检测等待":
                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[IO检测等待]," + str1);
                    delayCheckTime = 60000000;
                    currentRunStatus = Meth.WaitINPut_Check(str1, CheckSta, delayCheckTime);//检测一个输入               
                    break;
                case "IO输出":
                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[IO输出]," + str1);
                    currentRunStatus = Meth.OutPut_One_Run(str1, CheckSta);//一个输出
                    break;
                case "发送偏距":
                    currentRunStatus = true;
                    string[] OFFSET = CheckSta.Split(';');
                    string XX = OFFSET[0];
                    string YY = OFFSET[1];
                    string SendDate = "Offset;" + XX + ";" + YY + ";" + "0;";
                    Socket_server.Instance().sendDataToMac(SendDate);
                    break;
                case "单轴运动":
                    delayCheckTime = 6000;
                    int Asix = 0;
                     if(CheckSta=="0")
                    {
                        Asix = 0;
                    }
                    if (CheckSta == "1") { Asix = 1; }
                    if (CheckSta == "2")
                    {
                        Asix = 2;

                    }
                    if (CheckSta == "3") { Asix = 3; }
                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[单轴运动]," + str);
                    currentRunStatus = AxisR.Asix_one_Run(WeldPlat_Str_Name, str1, Asix, delayCheckTime);//0 x//1 y //2  z//3 u
                    break;
                case "拍照Z轴":
                    delayCheckTime = 6000;
                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[拍照Z轴]");
                    currentRunStatus = AxisR.Asix_z_Auto_High(WeldPlat_Str_Name, "拍照Z轴", 2, 90, -20, 5, 5, delayCheckTime);
                    break;
                case "焊接Z轴":
                    delayCheckTime = 6000;
                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[焊接Z轴]");
                    currentRunStatus = AxisR.Asix_z_Auto_High(WeldPlat_Str_Name, "焊接Z轴", 2, 90, -20, 5, 5, delayCheckTime);
                    break;
                case "双轴运动":
                    //     currentRunStatus = AxisR.Asix_Two_Run();
                    break;
                case "工装板顶升气缸上升"://工装板顶升气缸上升
                    m_WeldingTaskGroup.AddRunMessage("[工装板顶升气缸上升]," + str1);
                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[工装板顶升气缸上升]");
                    currentRunStatus = ClinderR.Tooling_Plate_Up(CheckSta);
                    break;
                case "工装板顶升气缸下降"://工装板顶升气缸下降
                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[工装板顶升气缸下降]");
                    currentRunStatus = ClinderR.Tooling_Plate_Down(CheckSta);
                    break;
                case "Y轴前模组定位气缸伸出":
                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[Y轴前模组定位气缸伸出]");
                    currentRunStatus = ClinderR.Y_Axis_befor_Clinder_Location_out(CheckSta);
                    break;
                case "Y轴前模组定位气缸缩回":
                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[Y轴前模组定位气缸缩回]");
                    currentRunStatus = ClinderR.Y_Axis_befor_Clinder_Location_in(CheckSta);
                    break;
                case "Y轴后模组定位气缸伸出":
                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[Y轴后模组定位气缸伸出]");
                    currentRunStatus = ClinderR.Y_Axis_After_Clinder_Location_in(CheckSta);
                    break;
                case "Y轴后模组定位气缸缩回":
                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[Y轴后模组定位气缸缩回]");
                    currentRunStatus = ClinderR.Y_Axis_After_Clinder_Location_out(CheckSta);
                    break;
                case "Y轴后模组顶升气缸上升":
                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[Y轴后模组顶升气缸上升]");
                    currentRunStatus = ClinderR.Y_Axis_After_Clinder_up(CheckSta);
                    break;
                case "Y轴后模组顶升气缸下降":
                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[Y轴后模组顶升气缸下降]");
                    currentRunStatus = ClinderR.Y_Axis_After_Clinder_down(CheckSta);
                    break;
                case "Y轴前模组顶升气缸上升":
                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[Y轴前模组顶升气缸上升]");
                    currentRunStatus = ClinderR.Y_Axis_befor_Clinder_up(CheckSta);
                    break;
                case "Y轴前模组顶升气缸下降":
                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[Y轴前模组顶升气缸下降]");
                    currentRunStatus = ClinderR.Y_Axis_befor_Clinder_down(CheckSta);
                    break;
                case "铜嘴压板气缸上升":
                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[铜嘴压板气缸上升]");
                    currentRunStatus = ClinderR.Copper_Mouth_Up(CheckSta);
                    break;
                case "铜嘴压板气缸下降":
                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[铜嘴压板气缸下降]");
                    currentRunStatus = ClinderR.Copper_Mouth_Down(CheckSta);
                    break;
                case "工装板阻挡气缸上升":
                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[工装板阻挡气缸上升]");
                    currentRunStatus = ClinderR.BTooling_PlateStop_Up(CheckSta);
                    break;
                case "工装板阻挡气缸下降":
                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[工装板阻挡气缸下降]");
                    currentRunStatus = ClinderR.Tooling_PlateStop_Down(CheckSta);
                    break;
                case "定位板气缸伸出":
                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[定位板气缸伸出]");
                    currentRunStatus = ClinderR.Batter_Board_Up(CheckSta);
                    break;
                case "定位板气缸缩回":
                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[定位板气缸缩回]");
                    currentRunStatus = ClinderR.Batter_Board_Down(CheckSta);
                    break;
                case "X轴右定位气缸伸出":
                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[X轴右定位伸出]");
                    currentRunStatus = ClinderR.X_Axis_right_Clinder_Location_out(CheckSta);
                    break;
                case "X轴右定位气缸缩回":
                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[X轴右定位气缸缩回]");
                    currentRunStatus = ClinderR.X_Axis_right_Clinder_Location_in(CheckSta);
                    break;
                case "X轴左定位气缸伸出":
                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[X轴左定位气缸伸出]");
                    currentRunStatus = ClinderR.X_Axis_left_Clinder_Location_out(CheckSta);
                    break;
                case "X轴左定位气缸缩回":
                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[X轴左定位气缸缩回]");
                    currentRunStatus = ClinderR.X_Axis_left_Clinder_Location_in(CheckSta);
                    break;
                case "Z轴挡板伸出":
                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[Z轴挡板伸出]");
                    currentRunStatus = ClinderR.Z_Baffle_Out(CheckSta);
                    break;
                case "Z轴挡板缩回":
                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[Z轴挡板缩回]");
                    currentRunStatus = ClinderR.Z_Baffle_In(CheckSta);
                    break;
                case "测台阶气缸上升":
                    break;
                case "测台阶气缸下降":
                    break;
                case "调高":
                    delayCheckTime = 6000;
                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[调高点基准]");
                    currentRunStatus = AxisR.Asix_z_Auto_High(WeldPlat_Str_Name, "调高点基准", 2, 90, -20, 5, 5, delayCheckTime);
                    break;
                case "光闸校验":
                    delayCheckTime = 6000;
                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[光闸校验]");
                    Thread.Sleep(500);
                    bool bOnOUTPUT = IOManage.OUTPUT("激光互锁输出").GetOn();//输出获取

                    bool bOnINPUT = IOManage.INPUT("激光互锁输入").GetOn();//输入获取


                    if (bOnOUTPUT==true&& bOnINPUT==true)
                    {                     
                        Meth.OutPut_One_Run("激光互锁输出", "false");//一个输出
                        Thread.Sleep(100);
                    }
                    while (true)
                    {
                        Meth.OutPut_One_Run("激光互锁输出", "false");//一个输出
                        Thread.Sleep(100);
                        bOnINPUT = IOManage.INPUT("激光互锁输入").GetOn();//输入获取
                        if (bOnINPUT==false)
                        {
                     
                            currentRunStatus = true;
                            break;
                        }
                        if (DateSave.Instance().Production.IsStop == true)
                        {
                            currentRunStatus = true;
                            break;
                        }
                        if (DateSave.Instance().Production.EStop == true)
                        {
                            currentRunStatus = true;
                            break;
                        }
                       
                    }

                    break;
                case "扫码":
                    string value ="T";                 
                    SerialPortDataManage.m_SerilPorts["扫码枪"].GetData(ref value);
                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[扫码]");
                    if (value !=""&& !value.Contains("ERROR"))
                    {
                        Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[扫码]:"+ value);
                        currentRunStatus = true;
                        DateSave.Instance().Production.DataReceivedstrSN = value;
                        mes.Instance().DataReceivedstrSN = value;  
                    }
                    else
                    {
                        Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[扫码]:扫码失败");
                        currentRunStatus = false;

                    }
                    break;
                case "MES过站验证":
                    string ds1 = mes.Instance().DataReceivedstrSN;
                    if (mesIsOk(ds1) == "OK")
                    {
                        Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[扫码]:验证成功");
                        currentRunStatus = true;
                    }
                    else
                    {
                        Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[扫码]:验证失败");
                        currentRunStatus = false;
                    }
                    break;
                case "MES获取SN":
                  string ds=  mes.Instance().DataReceivedstrSN;
                    mes.Instance().DataReceivedstrSN = GetSN(mes.Instance().DataReceivedstrSN.Replace("\r\n", ""));
                    string ds12 = mes.Instance().DataReceivedstrSN;
                    if (ds12 != "" && !ds12.Contains("FAIL"))
                    {
                        Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[MES获取SN]:MES获取SN成功");
                        currentRunStatus = true;
                    }
                    else
                    {
                        Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[MES获取SN]:MES获取SN失败");
                        currentRunStatus = false;

                    }
                    break;
                case "MES上传":
                    string result = "";
                    string ds2 = mes.Instance().DataReceivedstrSN;

                    result = OfflineUploadData(ds2, 9);
                    if (result == "OK")
                    {
                        Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[MES上传]:MES上传成功");
                        currentRunStatus = true;
                    }
                    else
                    {
                        Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[MES上传]:MES上传失败");
                        currentRunStatus = false;
                    }
                    break;
                case "拍照点":
                    delayCheckTime = 6000;
                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[" + str1 + "]:");
                    currentRunStatus = Meth.Asix_Line_Run("运动平台", str1, 60000);
                    currentRunStatus = AxisR.Asix_one_Run(WeldPlat_Str_Name, str1, 2, delayCheckTime);//0 x//1 y //2  z//3 u

                   // currentRunStatus = AxisR.Asix_z_Auto_High("运动平台", str1, HighDate[CamerFlow], DateSave.Instance().Production.SaveHigh_Top, DateSave.Instance().Production.SaveHigh_Low, DateSave.Instance().Production.AutoZ_High_Top, DateSave.Instance().Production.AutoZ_High_Low, 6000);

                    // CamerDate.Add(new KeyValuePair<double, double>(0.0, 0.0));
                    str_camer_checkNeed = CheckSta.Split(';');
                    try
                    {
                        DelayCamer = int.Parse(str_camer_checkNeed[2]);
                       
                        double d_Exprosure= Convert.ToDouble(str_camer_checkNeed[3]);
                        Thread.Sleep(DelayCamer);
                        CamerDateNeed_Date = CamerDateNeed(str_camer_checkNeed[0], str_camer_checkNeed[1], d_Exprosure, mes.Instance().DataReceivedstrSN.Replace("\r\n", ""));
                        if (CamerDateNeed_Date.Count == 0)
                        {
                            currentRunStatus = false;
                        }
                        else if (CamerDateNeed_Date.Count == 1)
                        {
                            //  CamerDate.Add(new KeyValuePair<double, double>(0.0, 0.0));
                            for (int i = 0; i < CamerDateNeed_Date.Count; i++)
                            {
                              
                                Point_1 = new KeyValuePair<double, double>(CamerDateNeed_Date[i].Key, CamerDateNeed_Date[i].Value);
                                if (Point_1.Value == 0.0 && Point_1.Key == 0.0)
                                {
                                    currentRunStatus = false;

                                }
                                else
                                {
                                    CamerFlow++;

                                    currentRunStatus = true;
                                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[" + str1 + "]拍照数据:" + Point_1);
                                    CamerDate.Add(Point_1);
                               
                                }
                            
                            }
                            //  CamerFlow= CamerFlow +2;
                      
                        }
                        else if (CamerDateNeed_Date.Count == 2)
                        {
                            for (int i = 0; i < CamerDateNeed_Date.Count; i++)
                            {
                                Point_1 = new KeyValuePair<double, double>(CamerDateNeed_Date[i].Key, CamerDateNeed_Date[i].Value);
                                if (Point_1.Value == 0.0 && Point_1.Key == 0.0)
                                {
                                    currentRunStatus = false;

                                }
                                else
                                {
                                    CamerDate.Add(Point_1);
                                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[" + str1 + "]拍照数据:" + Point_1);
                                    CamerFlow = CamerFlow + 2;
                                    currentRunStatus = true;
                                }
                             
                            }
                           
                        
                        }
                        if (CamerDateNeed_Date.Count == 0)
                        {
                            currentRunStatus = false;
                        }

                    }
                    catch { }
               
                    break;             
             
                case "焊接点":
          
                    Offset = CheckSta.Split(';');
                    OffsetX = 0;
                    OffsetY = 0;
                    if (Offset.Length > 0)
                    {
                        OffsetX = Convert.ToDouble(Offset[0]);
                        OffsetY = Convert.ToDouble(Offset[1]);
                        DelayCamer =int.Parse(Offset[2]);
                    }
                    if (AutoWeld>HighDate.Count)
                    {
                        MessageBox.Show("流程或数据错误,请停止运行，请检查流程后，重新开始");
                        currentRunStatus = false;
                        break;
                    }
                    //激光器就绪
                    //    焊接报警指示
                    delayCheckTime = 6000;
                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[焊接 " + str1 + ":点坐标]");                 
                    DateSave.Instance().Production.TheCurrentpoint = TheCurrentpoint(str1);                
                    currentRunStatus = Meth.Weld_Asix_Line_Run("运动平台", str1, delayCheckTime, DateSave.Instance().Production.X_Setover, DateSave.Instance().Production.Y_Setover, OffsetX, OffsetY);
                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[" + str1 + "]:" + HighDate[AutoWeld]);
                    currentRunStatus = AxisR.Asix_z_Auto_High("运动平台", str1, HighDate[AutoWeld] - DateSave.Instance().Production.From_Focus, DateSave.Instance().Production.SaveHigh_Top, DateSave.Instance().Production.SaveHigh_Low, DateSave.Instance().Production.AutoZ_High_Top, DateSave.Instance().Production.AutoZ_High_Low, 6000);
                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "["+ str1+"]:" + "开始焊接");
                    Thread.Sleep(DelayCamer);
                    if (CamerDate[AutoWeld].Key == 0.0&& CamerDate[AutoWeld].Value == 0.0)
                    {
                        WeldListDateMes[0] = DateSave.Instance().Production.Weld_Num.ToString(); //波形号
                         WeldListDateMes[1] = DateSave.Instance().Production.Weld_Speed.ToString();//速度
                        WeldListDateMes[2] = DateSave.Instance().Production.Z_AxialDatum.ToString();//基准高度
                        WeldListDateMes[3] = DateSave.Instance().Production.WeldPower.ToString();//最大功率
                        WeldListDateMes[4] = "0";//反馈功率
                        WeldListDateMes[5] = HighDate[AutoWeld].ToString();//焊接高度
                        WeldListDateMes[6] = DateSave.Instance().Production.From_Focus.ToString();//离焦量
                        WeldListDateMes[7] = DateSave.Instance().Production.Weld_R.ToString();//焊接半径            
                        WeldListDate.Add(WeldListDateMes);
                        AutoWeld++;
                        currentRunStatus = true;
                    }
                    else
                    {
                        currentRunStatus = Weld_Check(CamerDate[AutoWeld].Key, CamerDate[AutoWeld].Value);//开始焊接及检测焊接完成
                        if (currentRunStatus==true)
                        {
                           
                            WeldListDateMes[0] = DateSave.Instance().Production.Weld_Num.ToString(); //波形号
                            WeldListDateMes[1] = DateSave.Instance().Production.Weld_Speed.ToString();//速度
                            WeldListDateMes[2] = DateSave.Instance().Production.Z_AxialDatum.ToString();//基准高度
                            WeldListDateMes[3] = DateSave.Instance().Production.WeldPower.ToString();//最大功率
                            DateSave.Instance().Production.TheCurrentpoint = 0;
                            try { WeldListDateMes[4] =DateSave.Instance().Production.WeldDate.Average().ToString() ;//反馈功率                          
                            }
                            catch
                            {
                                WeldListDateMes[4] ="2000";//反馈功率 

                            }
                           
                            WeldListDateMes[5] = HighDate[AutoWeld].ToString();//焊接高度
                            WeldListDateMes[6] = DateSave.Instance().Production.From_Focus.ToString();//离焦量
                            WeldListDateMes[7] = DateSave.Instance().Production.Weld_R.ToString();//焊接半径            
                            WeldListDate.Add(WeldListDateMes);
                            AutoWeld++;

                        }
                    }
                    break;
              
                case "调高点":
                    Offset = CheckSta.Split(';');
                    OffsetX = 0;
                    OffsetY = 0;
                    if (Offset.Length > 0)
                    {
                        OffsetX = Convert.ToDouble(Offset[0]);
                        OffsetY = Convert.ToDouble(Offset[1]);
                        DelayCamer = int.Parse(Offset[2]);
                    }                 
                    delayCheckTime = 6000;
                    double PX = OffsetX- CamerDate[AutoHigh].Key ;
                    double PY = OffsetY-CamerDate[AutoHigh].Value;
                    Weld_Log.Instance().Enqueue(LOG_LEVEL.LEVEL_3, "[调高 " + str1 + ":点坐标]");
                    currentRunStatus = Meth.Weld_Asix_Line_Run("运动平台", str1, delayCheckTime, DateSave.Instance().Production.HeightOffset_X, DateSave.Instance().Production.HeightOffset_Y, PX, PY);            
                    Thread.Sleep(DelayCamer);
                    if (调高数据() > 0)
                    {
                        AutoHigh++;
                        HighDate.Add(调高数据());
                        currentRunStatus = true;
                    }
                    else
                    {
                        currentRunStatus = false;
                    }
                    break;
  
            }
            return currentRunStatus;
        }

        private static int CheckUpDateLock = 0;
        private static int CheckUpDateLock1 = 0;
        private static System.Timers.Timer CheckUpdatetimer = new System.Timers.Timer();
        private static System.Timers.Timer CheckUpdatetimerWeldFinish = new System.Timers.Timer();
        private static object LockObject = new Object();
        private static object LockObject1 = new Object();
        public void GetTimerStart()
        {
            // 循环间隔时间(10分钟)
            CheckUpdatetimer.Interval = 100;
            // 允许Timer执行
            CheckUpdatetimer.Enabled = true;
            // 定义回调
            CheckUpdatetimer.Elapsed += new ElapsedEventHandler(CheckUpdatetimer_Elapsed);
            // 定义多次循环
            CheckUpdatetimer.AutoReset = true;
            CheckUpdatetimer.Start();
            //   CheckUpdatetimer.Stop();



            CheckUpdatetimerWeldFinish.Interval = 1;
            // 允许Timer执行
            CheckUpdatetimerWeldFinish.Enabled = true;
            // 定义回调
            CheckUpdatetimerWeldFinish.Elapsed += new ElapsedEventHandler(CheckUpWeldFinish);
            // 定义多次循环
            CheckUpdatetimerWeldFinish.AutoReset = true;
            
        }
        int CountFinish = 0;
        public void CheckUpWeldFinish(object sender, ElapsedEventArgs e)
        {
            // 加锁检查更新锁
            lock (LockObject1)
            {
                if (CheckUpDateLock1 == 0) CheckUpDateLock1 = 1;
                else return;
            }

            //More code goes here.
            ////具体实现功能的方法
            //if (IOManage.INPUT("文档状态").On&& CountFinish==0)
            //{
            //    CountFinish++;
               
            //}
            //if (IOManage.INPUT("文档状态").Off&& CountFinish > 0)
            //{
            //    CountFinish = 0;
            //    Thread.Sleep(10);
            //    WeldFinishSta = "WeldFinish";
            //    // break;
            //}
            // 解锁更新检查锁
            lock (LockObject1)
            {
                CheckUpDateLock1 = 0;
            }
        }
        public void CheckUpdatetimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // 加锁检查更新锁
            lock (LockObject)
            {
                if (CheckUpDateLock == 0) CheckUpDateLock = 1;
                else return;
            }

            //More code goes here.
            //具体实现功能的方法
            //if (IOManage.INPUT("文档状态").On)
            //{
            //    if (IOManage.INPUT("文档状态").Off)
            //    {
            //        Thread.Sleep(10);
            //        WeldFinishSta = "WeldFinish";
            //       // break;
            //    }
            //}

            //if (IOManage.INPUT("文档状态").On && CountFinish == 0 && sendFinish > 0)
            //{
            //    CountFinish++;

            //}
            //if (IOManage.INPUT("文档状态").Off && CountFinish > 0)
            //{
            //    //  CountFinish = 0;
            //    Thread.Sleep(1);
            //    WeldFinishSta = "WeldFinish";
            //    // break;
            //}
            // 解锁更新检查锁
            lock (LockObject)
            {
                CheckUpDateLock = 0;
            }
        }
        public double HighDate_Need()
        {
            double High = 0.0;


            return High;

        }
        public string WeldFinishSta = "";
        Thread Weld = null;
        public bool   Weld_Check(double X,double Y)
        {
          return   weld_Finish(X, Y);
            //Weld = new Thread(weld_Finish);
            //Weld.IsBackground = true;
            //Weld.Start();
        }


        int sendFinish = 0;
        public bool  weld_Finish(double X, double Y)
        {
            CountFinish = 0;
            sendFinish = 0;
            string XX = X.ToString();
            string YY = Y.ToString();
            int stime = 6000;
            bool sta = false;
            DateTime starttime = DateTime.Now;
            WeldFinishSta = "";
     
            if (DateSave.Instance().Production.Empty_run == true)
            {
                WeldFinishSta = "WeldFinish";
              //  IOManage.OUTPUT("脱机文件0触发").SetOutBit(false);
                IOManage.OUTPUT("开始焊接机").SetOutBit(false);
            }
            else
            {
                sendFinish = 0;
                //IOManage.OUTPUT("脱机文件0触发").SetOutBit(true);
                //Thread.Sleep(300);
                IOManage.OUTPUT("开始焊接机").SetOutBit(true);
            
                Socket_server.Instance().recvDate = "";
                //while (true)
                //{
                //    if (Socket_server.Instance().recvDate.Contains("T"))
                //    {
                //        break;
                //    }
                //}
                //string SendDate = "Offset;" + XX + ";" + YY + ";" + "0;";
                //Socket_server.Instance().sendDataToMac(SendDate);
               // Task Task1 = sendOffset(X,Y);
                //CountFinish = 0;
                //sendFinish = 0;
                //CheckUpdatetimerWeldFinish.Start();
               // Task Task = WeldFinish();
            }
            while (true)
            {
                DateTime endtime = DateTime.Now;
                TimeSpan spantime = endtime - starttime;

                if (Socket_server.Instance().recvDate.Contains("T")&& sendFinish==0)
                {
                    sendFinish++;
                    string SendDate = "Offset;" + XX + ";" + YY + ";" + "0;";
                    Socket_server.Instance().sendDataToMac(SendDate);
                    Socket_server.Instance().recvDate = "";
                   // sta = false;
                   //  break;
                }

                if (IOManage.INPUT("文档状态").On && CountFinish == 0 && sendFinish > 0)
                {
                    CountFinish++;

                }
                if (IOManage.INPUT("文档状态").Off && CountFinish > 0)
                {
                    //  CountFinish = 0;
                    Thread.Sleep(1);
                    WeldFinishSta = "WeldFinish";
                    // break;
                }
                if (WeldFinishSta == "WeldFinish")
                {
                   // CheckUpdatetimerWeldFinish.Stop();
                    sta = true;
                    break;
                }

                if (spantime.TotalMilliseconds > stime)
                {
                    CheckUpdatetimerWeldFinish.Stop();
                    sta = false;
                    break;
                }
                if (DateSave.Instance().Production.IsStop == true)
                {
                    CheckUpdatetimerWeldFinish.Stop();
                    sta = true;
                    break;
                }
                if (DateSave.Instance().Production.EStop == true)
                {
                    CheckUpdatetimerWeldFinish.Stop();
                    sta = true;
                    break;
                }
            }
           // IOManage.OUTPUT("脱机文件0触发").SetOutBit(false);
            IOManage.OUTPUT("开始焊接机").SetOutBit(false);
            Thread.Sleep(300);
            return sta;
        }
        public bool WeldSta = false;
        public bool WeldStaIng = false;
        public async Task sendOffset(double X, double Y)
        {
            await Task.Run(() =>
            {
                IOManage.OUTPUT("脱机文件0触发").SetOutBit(true);
                Thread.Sleep(300);
                IOManage.OUTPUT("开始焊接机").SetOutBit(true);
                string XX = X.ToString();
                string YY = Y.ToString();
                Socket_server.Instance().recvDate = "";
                while (true)
                {
                    if (Socket_server.Instance().recvDate.Contains("T"))
                    {
                        break;
                    }
                }
                string SendDate = "Offset;" + XX + ";" + YY + ";" + "0;";
                Socket_server.Instance().sendDataToMac(SendDate);
                return;
            });

         
        }
        public async Task WeldFinish()
        {
            WeldFinishSta = "";
            //  Task Tast
            await Task.Run(() =>
            {
                while (true)
                {
                    if (IOManage.INPUT("文档状态").On)
                    {
                        break;
                    }
                }
                while (true)
                {
                    if (IOManage.INPUT("文档状态").Off)
                    {
                        Thread.Sleep(100);
                        WeldFinishSta = "WeldFinish";
                        break;
                    }
                }
                return;
            });
           // return;
        }
        public double 调高数据()
        {
            double high = 0.0;
            double BaselineSimulation = DateSave.Instance().Production.BaselineSimulation;//、、 获取基准模拟量  DateSave.Instance().Production.BaselineSimulation
            double date = 0.0;
            Thread.Sleep(200);
            TableManage.TableDriver("运动平台")._GetAdc(1, out date);//当前模拟量
            double ad12 = DateSave.Instance().Production.Z_AxialDatum;//获取Z基准坐标
            double sf = BaselineSimulation - date;
            if (sf > 0)
            {
                double s = Math.Abs(sf);
                double z = s / DateSave.Instance().Production.High_Date;
                if (DateSave.Instance().Production.AutoZ_High_Top > z && DateSave.Instance().Production.AutoZ_High_Low < z)
                {                
                }
                else
                {
                    return 0.0;
                }
                double CurrentZA = TableManage.TableDriver("运动平台").CurrentZ;//当前Z基准坐标
                double NeedCurrentZA = CurrentZA - z;
                high = NeedCurrentZA;
                if (DateSave.Instance().Production.SaveHigh_Top> high&& DateSave.Instance().Production.SaveHigh_Low< high)
                {
                   
                }
                else
                {
                    return 0.0;
                }
            }
            else
            {
                double s = Math.Abs(sf);
                double z = s / DateSave.Instance().Production.High_Date;
                if (DateSave.Instance().Production.AutoZ_High_Top > z && DateSave.Instance().Production.AutoZ_High_Low < z)
                {
                }
                else
                {
                    return 0.0;
                }
                double CurrentZA = TableManage.TableDriver("运动平台").CurrentZ;
                double NeedCurrentZA = CurrentZA + z;
                high = NeedCurrentZA;
                if (DateSave.Instance().Production.SaveHigh_Top > high && DateSave.Instance().Production.SaveHigh_Low < high)
                {

                }
                else
                {
                    return 0.0;
                }
            }
            return high;
        }

        public List<KeyValuePair<double, double>> CamerDateNeed(string needGetR, string NeedCheckR, double d_Exprosure,string sn)
        {
            List<KeyValuePair<double, double>> CamerDate = new List<KeyValuePair<double, double>>();
            KeyValuePair<double, double> Point1;
            KeyValuePair<double, double> Point2;
            List<LocationCircle.ResultClass> resultCirclr;
            resultCirclr = new List<LocationCircle.ResultClass>();
            string err = "";
            bool df = false;
            int needGetR_check = 0;
            if (needGetR == "2")
            {
                needGetR_check = 2;
            }
            else
            {
                needGetR_check = 1;
            }
            int NeedCheckR_check = 0;
            if (NeedCheckR == "2")
            {
                NeedCheckR_check = 2;
            }
            else
            {
                NeedCheckR_check = 1;
            }
            if (Program.form.VisionLocation(sn,needGetR_check, NeedCheckR_check, d_Exprosure, ref resultCirclr))
            {
                for (int i = 0; i < resultCirclr.Count; i++)
                {
                    Point1 = new KeyValuePair<double, double>(resultCirclr[i].CenterPoint.X, resultCirclr[i].CenterPoint.Y);
                    CamerDate.Add(Point1);

                }

            }
            else
            {
                

            }
           ///  Program.form.VisionLocation(needGetR_check, NeedCheckR_check, ref resultCirclr);
         //   Program.form.TestVision(ref resultCirclr, df, needGetR_check, NeedCheckR_check, out err);
           
            //double X = resultCirclr[0].CenterPoint.X;
            //double Y = resultCirclr[0].CenterPoint.Y;
            //double CirclrC = resultCirclr[0].Radius;
            //double X1 = resultCirclr[1].CenterPoint.X;
            //double Y1 = resultCirclr[1].CenterPoint.Y;
            //double CirclrC1 = resultCirclr[1].Radius;
            //Point1 = new KeyValuePair<double, double>(resultCirclr[0].CenterPoint.X, resultCirclr[0].CenterPoint.Y);
            //Point2 = new KeyValuePair<double, double>(resultCirclr[1].CenterPoint.X, resultCirclr[1].CenterPoint.Y);
            //CamerDate.Add(Point1);
            //CamerDate.Add(Point2);
            return CamerDate;
        }

        public string mesIsOk(string sn)//过站验证
        {
            string sta = "";
            //  string ISOK=LoginF.MES.GroupTest();       
            string ISOK = mes.Instance().GroupTest(sn.Replace("\r\n",""), mes.Instance().userCode, mes.Instance().deviceCode);//过站验证
          // string ISOK = mes.CellToolingPlate(sn);
            if (ISOK != "")
            {
                sta = ISOK;
            }
            else
            {
                sta = ISOK;
            }
            return sta;
        }
        public string GetSN(string sn)//过站验证
        {
            string sta = "";
            //  string ISOK=LoginF.MES.GroupTest();       
            //  string ISOK = mes.GroupTest(sn, mes.userCode, mes.deviceCode);//过站验证
            string ISOK = mes.Instance().CellToolingPlate(sn);
            if (ISOK != "")
            {
                sta = ISOK;
            }
            else
            {
                sta = ISOK;
            }
            return sta;
        }
        public string WipTest(string sn)//过站
        {
            string sta = "";
            mes.Instance().WipTest(sn,"PASS");
            return sta;
        }
        public string OfflineUploadData(string sn,int count1)//数据上传
        {
            string sta = "";
            for (int i=0;i< WeldListDate.Count;i++)
            {
                mes.Instance().WipTest(sn.Replace("\r\n", ""), "PASS", mes.Instance().userCode, mes.Instance().deviceCode, "", "");//上传数据
                string []MesDate=  WeldListDate[i];
                string MesStr = "|波形号:" + MesDate[0]
                                      + "|速度:" + MesDate[1]
                                      + "|加速度:" + "5" + "|基准高度:" + MesDate[2] + "|最大功率:" + MesDate[3] + "|反馈功率:" + MesDate[4] + "|焊接高度:" + MesDate[5] + "|离焦量:" + MesDate[6] + "|焊接半径:" + MesDate[7] + "|";            
                sta = mes.Instance().OfflineUploadData(sn.Replace("\r\n", ""), i.ToString(), "weld", "PASS", "", MesStr);
            } 
            sta = "OK";
            return sta;
        }
        public int TheCurrentpoint(string str)
        {
            int theCurrentpoint;
            int i= str.IndexOf("#");
            if (str.Substring(i - 2, 1) == "1" || str.Substring(i - 2, 1) == "2" || str.Substring(i - 2, 1) == "3")
            {
                theCurrentpoint = Convert.ToInt32(str.Substring(i - 2, 2));
            }
            else
            {
                theCurrentpoint = Convert.ToInt32(str.Substring(i - 1, 1));
            }
            return theCurrentpoint;
        }
    }

     public  class Date_save
     {

        public double CamerDateX = 0.0;
        public double CamerDateY = 0.0;
        public double HighDateY = 0.0;
    }
    
  
}
