using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace MyKTV_Management_Studio
{
    public partial class FrmLogin : OfficeForm
    {
        public FrmLogin()
        {
            InitializeComponent();
            this.EnableGlass = false;
        }

        private IAdministratorService adminservice = new AdministratorServiceImpl();
        private Dictionary<string, Administrator> admins;

        private bool ValidateAdminInfo(string username, string pwd)
        {

            if (adminservice.GetAdministrator(username, pwd) == null)
            {
                this.lblErrorInfo.Text = PARAMS.ERRORMSG_MISMATCH_UID_PWD;
                return false;
            }
            else
            {
                return true;
            }
        }

        private void BindAutoCompleteSourceData()
        {            
            admins = AdminInfoUtil.GetAdminInfoByFile(PARAMS.BINFilePath[0]);
            this.cbUsername.AutoCompleteMode = AutoCompleteMode.Suggest;
            foreach (string name in AdminInfoUtil.GetAdminNames(admins))
            {
                this.cbUsername.AutoCompleteCustomSource.Add(name);
            }
            this.cbUsername.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private void FrmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {           
            Environment.Exit(0);
        }      

        private void FrmLogin_Load(object sender, EventArgs e)
        {            
            this.cbUsername.Focus();            
            BindAutoCompleteSourceData();
            foreach (string name in AdminInfoUtil.GetAdminNames(admins))
            {
                this.cbUsername.Items.Add(name);
            }
            if (this.cbUsername.Items.Count > 0)
            {
                this.cbUsername.SelectedIndex = this.cbUsername.Items.Count - 1;
            }
            if (admins.ContainsKey(this.cbUsername.Text.Trim()))
            {
                this.txtPassword.Text = AdminInfoUtil.GetPwdByAdminName(this.cbUsername.Text.Trim(), admins);
                this.chbIsRememberPWD.Checked = true;
            }
            if (this.txtPassword.Text == string.Empty)
            {
                this.chbIsRememberPWD.Checked = false;
            }
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.cbUsername.Text))
            {
                this.lblErrorInfo.Text = PARAMS.ERRORMSG_NULL_UIDFIELD;
                this.cbUsername.Focus();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtPassword.Text.Trim()))
            {
                this.lblErrorInfo.Text = PARAMS.ERRORMSG_NULL_PWDFIELD;
            }
            else
            {
                if (ValidateAdminInfo(this.cbUsername.Text.Trim(), this.txtPassword.Text.Trim()))
                {
                    if (admins.ContainsKey(this.cbUsername.Text.Trim()))
                    {
                        admins.Remove(this.cbUsername.Text.Trim());
                    }
                    if (this.chbIsRememberPWD.Checked)
                    {
                        AdminInfoUtil.SaveInfoToFile(this.cbUsername.Text.Trim(), this.txtPassword.Text.Trim(), admins, PARAMS.BINFilePath[0]);
                    }
                    else if (!chbIsRememberPWD.Checked)
                    {
                        AdminInfoUtil.SaveInfoToFile(this.cbUsername.Text.Trim(), string.Empty, admins, PARAMS.BINFilePath[0]);
                    }
                    if (adminservice.SetAdminSatusByAdminName(this.cbUsername.Text.Trim(), 1))
                    {
                        PARAMS.AdminName = this.cbUsername.Text.Trim();
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show(PARAMS.ERRORMSG_LOGINERROR, PARAMS.TITLE_ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void cbUsername_TextChanged(object sender, EventArgs e)
        {
            this.lblErrorInfo.Text = string.Empty;
            this.txtPassword.Text = string.Empty;
            this.chbIsRememberPWD.Checked = false;
            if (admins.ContainsKey(this.cbUsername.Text.Trim()))
            {
                this.txtPassword.Text = AdminInfoUtil.GetPwdByAdminName(this.cbUsername.Text.Trim(), admins);
                this.chbIsRememberPWD.Checked = true;
                if (this.txtPassword.Text == string.Empty)
                {
                    this.chbIsRememberPWD.Checked = false;
                }
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            this.lblErrorInfo.Text = string.Empty;
        }

        private void cbUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (string.IsNullOrEmpty(this.cbUsername.Text))
                {
                    this.lblErrorInfo.Text = PARAMS.ERRORMSG_NULL_UIDFIELD;
                    this.cbUsername.Focus();
                }
            }
            if (e.KeyChar == 8)
            {
                this.txtPassword.Text = string.Empty;
                this.chbIsRememberPWD.Checked = false;
            }
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (string.IsNullOrEmpty(this.cbUsername.Text) && string.IsNullOrEmpty(this.txtPassword.Text))
                {
                    this.lblErrorInfo.Text = PARAMS.ERRORMSG_NULL_UIDFIELD;
                    this.cbUsername.Focus();
                }
                if (string.IsNullOrEmpty(this.txtPassword.Text))
                {
                    this.lblErrorInfo.Text = PARAMS.ERRORMSG_NULL_PWDFIELD;
                    this.txtPassword.Focus();
                }
                else
                {
                    if (ValidateAdminInfo(this.cbUsername.Text.Trim(), this.txtPassword.Text.Trim()))
                    {
                        if (admins.ContainsKey(this.cbUsername.Text.Trim()))
                        {
                            admins.Remove(this.cbUsername.Text.Trim());
                        }
                        if (this.chbIsRememberPWD.Checked)
                        {
                            AdminInfoUtil.SaveInfoToFile(this.cbUsername.Text.Trim(), this.txtPassword.Text.Trim(), admins, PARAMS.BINFilePath[0]);
                        }
                        else if (!chbIsRememberPWD.Checked)
                        {
                            AdminInfoUtil.SaveInfoToFile(this.cbUsername.Text.Trim(), string.Empty, admins, PARAMS.BINFilePath[0]);
                        }
                        if (adminservice.SetAdminSatusByAdminName(this.cbUsername.Text.Trim(), 1))
                        {
                            PARAMS.AdminName = this.cbUsername.Text.Trim();
                            this.DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            MessageBox.Show(PARAMS.ERRORMSG_LOGINERROR, PARAMS.TITLE_ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            if (e.KeyChar == 8)
            {
                this.cbUsername.SelectedText = string.Empty;
                this.txtPassword.Text = string.Empty;
                this.chbIsRememberPWD.Checked = false;
            }
        }

        private void cbUsername_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (admins.ContainsKey(this.cbUsername.Text.Trim()))
            {
                this.txtPassword.Text = AdminInfoUtil.GetPwdByAdminName(this.cbUsername.Text.Trim(), admins);
                this.chbIsRememberPWD.Checked = true;
                if (this.txtPassword.Text == string.Empty)
                {
                    this.chbIsRememberPWD.Checked = false;
                }
            }
        }
    }
}
