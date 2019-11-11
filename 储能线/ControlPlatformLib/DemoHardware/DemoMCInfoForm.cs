﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlPlatformLib
{
    public partial class DemoMCInfoForm : Form
    {

        DemoMCInfo hardInfo;
        public DemoMCInfoForm(DemoMCInfo Info)
        {
            InitializeComponent();
            hardInfo = Info;
        }

        private void DemoMCInfoForm_Load(object sender, EventArgs e)
        {
            label1.Text = hardInfo.hardwareName;
        }

    }
}
