using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace 铁路货运发送作业信息系统
{
    public partial class staffRegister : Form
    {
        public staffRegister()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            //登录按钮
            //启用数据库
            string str = @"Data Source=.;Initial Catalog=User;Integrated Security=True";
            SqlConnection sqlCnt = new SqlConnection(str);
            sqlCnt.Open();
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlCnt;
            sqlCmd.CommandType = CommandType.Text;
            //判断输入的用户密码是否存在，不存在则提示。
            if (userNameText.Text.Length != 0 &&
    passwordText.Text.Length != 0)
            {//判空
                sqlCmd.CommandText = "SELECT * FROM [User] WHERE userName='" + userNameText.Text + "' AND passWord = '" + passwordText.Text + "'";
                SqlDataReader dataReader;
                dataReader = sqlCmd.ExecuteReader();
                if (dataReader.Read())
                {
                    //判断是用户还是职工
                    int userType = dataReader.GetInt32(dataReader.GetOrdinal("userType"));
                    dataReader.Close();
                    if (userType == 2)
                    {//是管理员
                        添加职工信息 窗口 = new 添加职工信息();
                        窗口.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("请输入管理员账户", "提示", MessageBoxButtons.OK,
MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                }
                else
                {
                    dataReader.Close();
                    MessageBox.Show("用户名或密码错误，请检查后重新输入", "提示", MessageBoxButtons.OK,
             MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
            }
            else//输入的是空的
            {
                MessageBox.Show("请输入用户名与密码后登录", "提示", MessageBoxButtons.OK,
                             MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            sqlCnt.Close();
        }
    }
}
