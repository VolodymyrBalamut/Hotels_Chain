﻿using Hotels.Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hotels.View
{
    public partial class FormHotel : Form
    {
        ControllerHotel controller;
        public FormHotel()
        {
            InitializeComponent();
            Dictionary<string, TextBox> textBoxs = new Dictionary<string, TextBox>();
            textBoxs.Add("Name", tbName);
            controller = new ControllerHotel(dgvHotel, bindingNavigatorFormHotel, textBoxs);
            controller.FillColumns();
        }

        private void FormHotel_Load(object sender, EventArgs e)
        {
            controller.LoadDB();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            controller.Save();
            controller.LoadDB();
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            controller.Delete();
            controller.LoadDB();
        }


    }
}
