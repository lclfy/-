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
    public partial class 添加职工信息 : Form
    {
        public 添加职工信息()
        {
            InitializeComponent();
            //禁用删除按钮
            this.deleteStaffButton.Enabled = false;
            StaffListView.View = View.Details;
            string[] ListTitles = new string[] { "用户名", "密码" };
            for (int i = 0; i < ListTitles.Count(); i++)
            {
                ColumnHeader ch = new ColumnHeader();
                ch.Text = ListTitles[i];   //设置列标题 
                ch.Width = 150;
                this.StaffListView.Columns.Add(ch);    //将列头添加到ListView控件。
            }
            this.initUI();
        }

        public void initUI()
        {
            //目的是把数据库里面能填进界面里面的东西都填进去
            //启用数据库
            string str = @"Data Source=.;Initial Catalog=User;Integrated Security=True";
            SqlConnection sqlCnt = new SqlConnection(str);
            sqlCnt.Open();
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlCnt;
            sqlCmd.CommandType = CommandType.Text;
            //把运单数据从数据库上导入然后存储起来
            sqlCmd.CommandText = "select * from [User] where userType = '1'";
            DataSet dataSet = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(sqlCmd.CommandText, sqlCnt);
            da.Fill(dataSet);
            //清空之前所有的信息
            for (int k = 0; k < StaffListView.Items.Count;)
            {
                StaffListView.Items.RemoveAt(0);
            }
            this.StaffListView.BeginUpdate();

            foreach (DataRow dataRow in dataSet.Tables[0].Rows)   //添加货物的数据 
            {
                ListViewItem lvi = new ListViewItem();
                //第一列 用户名
                lvi.SubItems[0].Text = dataRow[0].ToString().TrimEnd();
                //第二列 密码
                lvi.SubItems.Add(dataRow[1].ToString().TrimEnd());

                this.StaffListView.Items.Add(lvi);
                ImageList imgList = new ImageList();
                imgList.ImageSize = new Size(1, 20);// 设置行高 20 //分别是宽和高 
                StaffListView.SmallImageList = imgList; //这里设置listView的SmallImageList ,用imgList将其撑大 
            }
            this.StaffListView.EndUpdate();  //结束数据处理，UI界面一次性绘制。

            sqlCnt.Close();

        }

        private void addStaffButton_Click(object sender, EventArgs e)
        {//添加信息
         //注册按钮
         //启用数据库
            string str = @"Data Source=.;Initial Catalog=User;Integrated Security=True";
            SqlConnection sqlCnt = new SqlConnection(str);
            sqlCnt.Open();
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlCnt;
            sqlCmd.CommandType = CommandType.Text;
            //判断输入的用户是否存在，存在则禁止注册。
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
                    sqlCmd.CommandText = "insert into [User] values('" + userNameText.Text + "','" + passwordText.Text + "','1')";
                    sqlCmd.ExecuteNonQuery();
                    MessageBox.Show("注册成功。", "提示", MessageBoxButtons.OK,
             MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    this.initUI();
                    userNameText.Text = "";
                    passwordText.Text = "";
                }
            }
            else//输入的是空的
            {
                MessageBox.Show("请输入用户名与密码", "提示", MessageBoxButtons.OK,
                             MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            sqlCnt.Close();


        }

        private void StaffListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(StaffListView.SelectedItems.Count != 0)
            {//如果有选中的行，那么删除按钮就是开放状态，反之关闭
                this.deleteStaffButton.Enabled = true;
            }
            else
            {
                this.deleteStaffButton.Enabled = true;
            }
        }

        private void deleteStaffButton_Click(object sender, EventArgs e)
        {
            //询问是否确定
            DialogResult dr;
            dr = MessageBox.Show("是否确认删除？将删除此用户的所有信息且不可恢复。", "确定", MessageBoxButtons.YesNo,
                     MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            if (dr == DialogResult.Yes)
            {
                //启用数据库
                string str = @"Data Source=.;Initial Catalog=User;Integrated Security=True";
                SqlConnection sqlCnt = new SqlConnection(str);
                sqlCnt.Open();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = sqlCnt;
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "delete FROM [User] WHERE userName='" + StaffListView.SelectedItems[0].SubItems[0].Text + "' ";
                sqlCmd.ExecuteNonQuery();
                sqlCmd.CommandText = "delete FROM GoodsAcceptance WHERE userName='" + StaffListView.SelectedItems[0].SubItems[0].Text + "' ";
                sqlCmd.ExecuteNonQuery();
                sqlCmd.CommandText = "delete FROM Goods WHERE userName='" + StaffListView.SelectedItems[0].SubItems[0].Text + "' ";
                sqlCmd.ExecuteNonQuery();
                sqlCmd.CommandText = "delete FROM AcceptancePlan WHERE userName='" + StaffListView.SelectedItems[0].SubItems[0].Text + "' ";
                sqlCmd.ExecuteNonQuery();
                sqlCnt.Close();
                this.initUI();
            }




        }
    }
}
