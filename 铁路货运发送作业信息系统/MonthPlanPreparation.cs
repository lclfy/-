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
    public partial class MonthPlanPreparation : Form
    {
        //传入计划编号和用户名以便于检索
        private int _planID;
        public int planID
        {
            get { return _planID; }
            set { _planID = value; }
        }
        private string _userName;
        public string userName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        //传入一个编辑/添加参数表明是编辑还是添加（添加0，编辑1）
        private int _editStatus;
        public int editStatus
        {
            get { return _editStatus; }
            set { _editStatus = value; }
        }


        public MonthPlanPreparation(int tempPlanID,int tempEditStatus,string tempUserName)
        {
            userName = tempUserName;
            editStatus = tempEditStatus;
            planID = tempPlanID;
            InitializeComponent();
            this.initUI();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        public void initUI()
        {
            //目的是在编辑模式下把数据库里面能填进界面里面的东西都填进去
            if(editStatus == 1)
            {
                //启用数据库
                string str = @"Data Source=.;Initial Catalog=User;Integrated Security=True";
                SqlConnection sqlCnt = new SqlConnection(str);
                sqlCnt.Open();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = sqlCnt;
                sqlCmd.CommandType = CommandType.Text;
                //把运单数据从数据库上导入然后存储起来
                sqlCmd.CommandText = "select * from AcceptancePlan where planID = '" + planID + "' and userName = '" + _userName + "'";
                DataSet dataSetForAcceptancePlan = new DataSet();
                SqlDataAdapter daGA = new SqlDataAdapter(sqlCmd.CommandText, sqlCnt);
                daGA.Fill(dataSetForAcceptancePlan);
                //初始化货物详情表格
                foreach (DataRow dataRow in dataSetForAcceptancePlan.Tables[0].Rows)   //添加运单的数据 
                {
                    //为界面上的元素赋值
                    dailyPlanText.Text = dataRow[17].ToString().TrimEnd();//车种编号
                    plannedVehiclesCountText.Text = dataRow[18].ToString().TrimEnd();//货车标重
                    plannedTransmissionText.Text = dataRow[19].ToString().TrimEnd();//铁路篷布号码
                    plannedEachVehiclePayloadText.Text = dataRow[20].ToString().TrimEnd();//经由
                    goodsCategoryText.Text = dataRow[5].ToString().TrimEnd();//集装箱号码
                }

                sqlCnt.Close();
            }


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
            if (goodsCategoryText.Text.Length != 0 &&
                dailyPlanText.Text.Length != 0 &&
                plannedVehiclesCountText.Text.Length != 0 &&
                 plannedTransmissionText.Text.Length != 0 &&
                 plannedEachVehiclePayloadText.Text.Length != 0)
            {
                //询问用户是否添加
                DialogResult dr;
                dr = MessageBox.Show("是否确认添加该信息？", "确定", MessageBoxButtons.YesNo,
                         MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                if (dr == DialogResult.Yes)
                {//如果用户同意添加
                    //增加了planType类型，为1则说明是原题的编辑，为2则是月计划受理。二者公用同一个数据库

                    //通过editStatus判断是添加还是修改(0添加，1修改)
                    if(editStatus == 0)
                    {//添加
                        sqlCmd.CommandText = "insert into AcceptancePlan (userName,planType,planID,goodsCategory,dailyPlan,plannedEachVehiclePayload,plannedVehiclesCount,plannedTransmission) values('"+userName+"','2','" + planID + "','" + goodsCategoryText.Text + "','" + dailyPlanText.Text + "','" + plannedEachVehiclePayloadText.Text + "','" + plannedVehiclesCountText.Text + "','" + plannedTransmissionText.Text + "')";
                        sqlCmd.ExecuteNonQuery();
                    }
                    else if (editStatus == 1)
                    {
                        sqlCmd.CommandText = "update AcceptancePlan set goodsCategory = '" + goodsCategoryText.Text + "',  dailyPlan = '" + dailyPlanText.Text + "',plannedEachVehiclePayload = '" + plannedEachVehiclePayloadText.Text + "', plannedVehiclesCount = '"+plannedVehiclesCountText.Text+ "', plannedTransmission = '"+plannedTransmissionText.Text+"'   where planID = '" + planID + "' and userName = '" + _userName + "'";
                        sqlCmd.ExecuteNonQuery();
                    }

                    //在主页面的列表中添加该项目
                    homeScreen form = (homeScreen)this.Owner;
                    form.initAcceptancePlanList(2);

                    //关闭数据库并关闭界面
                    this.Close();
                    sqlCnt.Close();
                    return;
                }
                else
                {
                    //用户选择了取消
                    //直接结束方法
                    sqlCnt.Close();
                    return;
                }
            }


        }
        private void goodsText_KeyPress(object sender, KeyPressEventArgs e)
        {//用于控制货物数量，价格，重量和各种电话号码为数字的方法
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)13 && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }
    }
}
