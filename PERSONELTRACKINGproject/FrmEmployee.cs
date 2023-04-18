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
using DAL.DTO;
using System.IO;    

namespace PERSONELTRACKINGproject
{
    public partial class FrmEmployee : Form
    {
        public FrmEmployee()
        {
            InitializeComponent();
        }
        string fileName = ""; 
        bool combofull = false;
        EmployeeDTO dto = new EmployeeDTO();
        public EmployeeDetailDTO detail = new EmployeeDetailDTO();  
        public bool isUpdate = false;
        string imagepath = "";
        private void FrmEmployee_Load(object sender, EventArgs e)
        {
            dto = EmployeeBLL.GetAll();
            cmbDepartment.DataSource = dto.Departments;
            cmbDepartment.DisplayMember = "DepartmentName";
            cmbDepartment.ValueMember = "ID";
            cmbPosition.DataSource = dto.Positions;
            cmbPosition.DisplayMember = "PositionName";
            cmbPosition.ValueMember = "ID";
            cmbDepartment.SelectedIndex = -1;
            cmbPosition.SelectedIndex = -1;
            combofull = true;
            if (isUpdate)
            {
                txtName.Text = detail.Name;
                txtSurname.Text = detail.Surname;
                txtUserNo.Text = detail.UserNo.ToString();
                txtPassword.Text = detail.Password;
                chAdmin.Checked = Convert.ToBoolean(detail.isAdmin);
                txtAdress.Text = detail.Adress;
                dateTimePicker1.Value = Convert.ToDateTime(detail.BirthDay);
                cmbDepartment.SelectedValue = detail.DepartmentID;
                cmbDepartment.SelectedValue = detail.PositionID;
                txtSalary.Text = detail.Salary.ToString();
                imagepath = Application.StartupPath + "\\images\\" + detail.ImagePath;
                txtImagePath.Text = imagepath;
                pictureBox1.ImageLocation = imagepath;
                if(!Convert.ToBoolean(UserStatic.isAdmin))
                {
                    chAdmin.Enabled = false;
                    txtUserNo.Enabled = false;
                    txtSalary.Enabled = false;
                    cmbDepartment.Enabled = false;
                    cmbPosition.Enabled = false;

                }
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtUserNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = General.isNumber(e);
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = General.isNumber(e);
        }

        private void cmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combofull)
            {
                int departmentID = Convert.ToInt32(cmbDepartment.SelectedValue);
                cmbPosition.DataSource = dto.Positions.Where(x => x.DepartmentID == departmentID).ToList();
            }

        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Load(openFileDialog1.FileName);
                txtImagePath.Text = openFileDialog1.FileName;
                string unique = Guid.NewGuid().ToString();
                fileName += unique + openFileDialog1.SafeFileName;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtUserNo.Text.Trim() == "")
            {
                MessageBox.Show("User no is Empty");
            }
            


            else if (txtPassword.Text.Trim() == "")
            {
                MessageBox.Show("Password is Empty");
            }
            else if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("Name is Empty");
            }
            else if (txtSurname.Text.Trim() == "")
            {
                MessageBox.Show("Surname is Empty");
            }
            else if (txtSalary.Text.Trim() == "")
            {
                MessageBox.Show("Salary is Empty");
            }
            else if (cmbDepartment.SelectedIndex == -1)
            {
                MessageBox.Show("Select a Department");
            }
            else if (cmbPosition.SelectedIndex == -1)
            {
                MessageBox.Show("Select a Position");
            }
            else
            {
                if(!isUpdate)
                {
                     if (!EmployeeBLL.isUnique(Convert.ToInt32(txtUserNo.Text)))
                    {
                        MessageBox.Show("This user no is already used by another employee please change");
                    } else { 
                    EMPLOYEE employee = new EMPLOYEE();
                    employee.UserNo = Convert.ToInt32(txtUserNo.Text);
                    employee.Password = txtPassword.Text;
                    employee.isAdmin = chAdmin.Checked;
                    employee.Name = txtName.Text;
                    employee.Surname = txtSurname.Text;
                    employee.Salary = Convert.ToInt32(txtSalary.Text);
                    employee.DepartmentID = Convert.ToInt32(cmbDepartment.SelectedValue);
                    employee.PositionID = Convert.ToInt32(cmbPosition.SelectedValue);
                    employee.Adress = txtAdress.Text;
                    employee.Birthday = dateTimePicker1.Value;
                    employee.ImagePath = fileName;
                    EmployeeBLL.AddEmployee(employee);
                    File.Copy(txtImagePath.Text, @"images\\" + fileName);
                    MessageBox.Show("Employee was added");
                    txtUserNo.Clear();
                    txtPassword.Clear();
                    chAdmin.Checked = false;
                    txtName.Clear();
                    txtSurname.Clear();
                    txtSalary.Clear();
                    txtAdress.Clear();
                    txtImagePath.Clear();
                    pictureBox1.Image = null;
                    combofull = false;
                    cmbDepartment.SelectedIndex = 1;
                    cmbPosition.DataSource = dto.Positions;
                    cmbPosition.SelectedIndex = -1;
                    combofull = true;
                    dateTimePicker1.Value = DateTime.Today;
                    }
                }
               else
                {
                    DialogResult result = MessageBox.Show("Are you Sure?", "Warning!", MessageBoxButtons.YesNo);
                    if (result== DialogResult.Yes)
                    {
                        EMPLOYEE employee = new EMPLOYEE();
                        if (txtImagePath.Text != imagepath)
                        {
                            if (File.Exists(@"images\\" + detail.ImagePath))
                                File.Delete(@"images\\" + detail.ImagePath);
                            File.Copy(txtImagePath.Text, @"images\\" + fileName);
                            employee.ImagePath = fileName;

                        }
                        else
                        {
                            employee.ImagePath = detail.ImagePath;
                            employee.ID = detail.EmployeeID;
                            employee.UserNo = Convert.ToInt32(txtUserNo.Text);
                            employee.Name = txtName.Text;
                            employee.Surname = txtSurname.Text;
                            employee.isAdmin= chAdmin.Checked;
                            employee.Password = txtPassword.Text;
                            employee.Adress= txtAdress.Text;
                            employee.Birthday = dateTimePicker1.Value;
                            employee.DepartmentID = Convert.ToInt32(cmbDepartment.SelectedValue);
                            employee.PositionID = Convert.ToInt32(cmbPosition.SelectedValue);
                            employee.Salary = Convert.ToInt32(txtSalary.Text);
                            EmployeeBLL.UpdateEmployee(employee);
                            MessageBox.Show("Emloyee Was Updated");
                            this.Close();
                        }
                    }
                }


            }

        }
        bool isUnique = false;  
        private void btnCheck_Click(object sender, EventArgs e)
        {
            if (txtUserNo.Text.Trim() == "")
            {
                MessageBox.Show("User no is Empty");
            } else
            {
                isUnique= EmployeeBLL.isUnique(Convert.ToInt32(txtUserNo.Text));    
                if (!isUnique)
                {
                    MessageBox.Show("This user no is already used by another employee please change");
                } else
                {
                    MessageBox.Show("This user no is usable");
                }

            }
        }
    }


}
