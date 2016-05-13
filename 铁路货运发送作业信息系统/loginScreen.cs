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
    public partial class loginScreen : Form
    {
        public loginScreen()
        {
            InitializeComponent();
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void registerButton_Click(object sender, EventArgs e)
        {//注册按钮
            //启用数据库
            string str = @"Data Source=.;Initial Catalog=User;Integrated Security=True";
            SqlConnection sqlCnt = new SqlConnection(str);
            sqlCnt.Open();
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlCnt;
            sqlCmd.CommandType = CommandType.Text;
            //判断输入的用户密码是否存在，存在则禁止注册。
            if (userNameText.Text.Length != 0 &&
    passwordText.Text.Length != 0)
            {//判空
                sqlCmd.CommandText = "SELECT * FROM [User] WHERE userName='" + userNameText.Text + "'";
                SqlDataReader dataReader;
                dataReader = sqlCmd.ExecuteReader();
                if (dataReader.Read())
                {
                    dataReader.Close();
                    MessageBox.Show("该用户名已被占用", "提示", MessageBoxButtons.OK,
             MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
                else
                {
                    dataReader.Close();
                    sqlCmd.CommandText = "insert into [User] values('" + userNameText.Text + "','" + passwordText.Text + "','0')";
                    sqlCmd.ExecuteNonQuery();
                    MessageBox.Show("注册成功，将以用户身份自动登录。", "提示", MessageBoxButtons.OK,
             MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    //跳转至主页面
                    homeScreen form2 = new homeScreen(userNameText.Text,0);
                    form2.Show();
                    this.Hide();
                }
            }
            else//输入的是空的
            {
                MessageBox.Show("请输入用户名与密码后点击此按钮注册", "提示", MessageBoxButtons.OK,
                             MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            sqlCnt.Close();

        }

        private void loginButton_Click(object sender, EventArgs e)
        {//登录按钮
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
                sqlCmd.CommandText = "SELECT * FROM [User] WHERE userName='" + userNameText.Text + "' AND passWord = '"+ passwordText.Text +"'";
                SqlDataReader dataReader;
                dataReader = sqlCmd.ExecuteReader();
                if (dataReader.Read())
                {
                    //判断是用户还是职工
                    int userType = dataReader.GetInt32(dataReader.GetOrdinal("userType"));
                    dataReader.Close();
                    if(userType == 0)
                    {//是用户
                        homeScreen form1 = new homeScreen(userNameText.Text,0);
                        form1.Show();
                        this.Hide();
                    }else if(userType == 1)
                    {//是职工
                        //跳转至主页面
                        homeScreen form2 = new homeScreen(userNameText.Text, 1);
                        form2.Show();
                        this.Hide();
                    }
                    else
                    {//是管理员
                        homeScreen form3 = new homeScreen(userNameText.Text, 2);
                        form3.Show();
                        this.Hide();
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

        private void forgetPasswordButton_Click(object sender, EventArgs e)
        {//忘记密码界面
            forgetPassword form = new forgetPassword();
            form.ShowDialog();
        }

        private void staffRegisterButton_Click(object sender, EventArgs e)
        {
            staffRegister form1 = new staffRegister();
            form1.Show();
        }
    }
}
