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
    public partial class forgetPassword : Form
    {
        //用于存储输入的用户名
        string userName;

        public forgetPassword()
        {
            InitializeComponent();
        }

        private void forgetPassword_Load(object sender, EventArgs e)
        {
            //设置初始状态（输入用户名）下的标签
            nameLabel.Tag = 0;
        }

        private void userNameText_TextChanged(object sender, EventArgs e)
        {

        }

        private void doneButton_Click(object sender, EventArgs e)
        {

            //启用数据库
            string str = @"Data Source=.;Initial Catalog=User;Integrated Security=True";
            SqlConnection sqlCnt = new SqlConnection(str);
            sqlCnt.Open();
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlCnt;
            sqlCmd.CommandType = CommandType.Text;
            //利用label的状态Tag来判断目前是输入用户名还是输入新密码，0为输入用户名，1为输入密码
            if((int)nameLabel.Tag == 0)
            {
                //判断输入的用户是否存在
                if (userNameText.Text.Length != 0)
                {//判空
                    sqlCmd.CommandText = "SELECT * FROM [User] WHERE userName='" + userNameText.Text + "'";
                    SqlDataReader dataReader;
                    dataReader = sqlCmd.ExecuteReader();
                    if (dataReader.Read())
                    {//成功
                        dataReader.Close();

                        //更改标签文字
                        nameLabel.Text = "请输入新密码";
                        //将之前输入的用户名存储起来以便之后更改密码用
                        userName = userNameText.Text;
                        //把文本框改为密码模式
                        userNameText.PasswordChar = '*';
                        userNameText.Text = "";
                        nameLabel.Tag = 1;

                    }
                    else
                    {//没有改用户名
                        dataReader.Close();
                        MessageBox.Show("未查询到用户名，请确认后重试", "提示", MessageBoxButtons.OK,
    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                }
                else//输入的是空的
                {
                    MessageBox.Show("请输入用户名", "提示", MessageBoxButtons.OK,
                                 MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
            }
            else
            {
                sqlCmd.CommandText = "update [User] set passWord = '" + userNameText.Text + "' where userName = '"+ userName +"'";
                sqlCmd.ExecuteNonQuery();
                this.Close();
                MessageBox.Show("修改成功", "提示", MessageBoxButtons.OK,
             MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

            }
            sqlCnt.Close();

        }
    }
}
