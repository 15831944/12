﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlPlatformLib
{
    public class DemoInputOutputCard : HardWareBase, IInputAction, IOutputAction
    {
        private bool[] bBitInputStatus = new bool[128];
        private bool[] bBitOutputStatus = new bool[128];
        public bool GetInputBit(int iBit)
        {
            if (iBit < 128 && iBit > -1)
            {
                return bBitInputStatus[iBit];
            }
            else
            {
                return false;
            }
        }
        public bool SetOutBit(int iBit, bool bOn)
        {
            if (iBit < 128 && iBit > -1)
            {
                bBitOutputStatus[iBit] = bOn;
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool GetOutBit(int iBit)
        {
            if (iBit < 128 && iBit > -1)
            {
                return bBitOutputStatus[iBit];
            }
            else
            {
                return false;
            }

        }
        override public bool Init(HardWareInfoBase infoHardWare)
        {
            bInitOK = true;
            System.Threading.Thread threadScan = new System.Threading.Thread(ScanThreadFunction);
            threadScan.IsBackground = true;
            threadScan.Start();
            return true;
        }
        private void ScanThreadFunction()
        {
            WorldGeneralLib.HiPerfTimer timer = new WorldGeneralLib.HiPerfTimer();
            System.Threading.Thread.Sleep(1000);
            int iStep = 0;
            while (true)
            {
                System.Threading.Thread.Sleep(1);
                switch (iStep)
                {
                    case 0:
                        {
                            timer.Start();
                            iStep = 10;
                        }
                        break;
                    case 10:
                        {
                            if (timer.TimeUp(1))
                            {
                                timer.Start();
                                for (int i = 0; i < 128; i++)
                                {
                                    bBitInputStatus[i] = true;
                                }
                                iStep = 20;
                            }
                        }
                        break;
                    case 20:
                        {
                            if (timer.TimeUp(1))
                            {
                                for (int i = 0; i < 128; i++)
                                {
                                    bBitInputStatus[i] = false;
                                }
                                iStep = 0;
                            }
                        }
                        break;
                    default:
                        break;
                }

            }
        }
    }
}
