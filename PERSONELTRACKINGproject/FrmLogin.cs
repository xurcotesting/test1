﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using DAL;

namespace PERSONELTRACKINGproject
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void txtUserNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {

                e.Handled = General.isNumber(e);
            
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(txtUserNo.Text.Trim() == "" || txtPassword.Text.Trim() == "")
            {
                MessageBox.Show("Please fill username and password");
            } else
            {
                List<EMPLOYEE> employeelist = EmployeeBLL.GetEmployees(Convert.ToInt32(txtUserNo.Text), txtPassword.Text);
                if(employeelist.Count == 0)
                {
                    MessageBox.Show("Please control your information");
                } else
                {
                    EMPLOYEE employee = new EMPLOYEE();
                    employee = employeelist.First();    
                    UserStatic.EmployeeID= employee.ID;
                    UserStatic.UserNo = employee.UserNo;
                    UserStatic.isAdmin = Convert.ToBoolean(employee.isAdmin);   
                    FrmMain frm = new FrmMain();
                    this.Hide();
                    frm.ShowDialog();
                }
            }
        }
    }
}
