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
    public partial class OriginalTitleEditor : Form
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

        public OriginalTitleEditor(int tempPlanID,int tempEditStatus, string tempUserName)
        {
            userName = tempUserName;
            editStatus = tempEditStatus;
            planID = tempPlanID;
            InitializeComponent();
            this.initUI();
        }

        public void initUI()
        {
            //目的是在编辑模式下把数据库里面能填进界面里面的东西都填进去
            //启用数据库
            if(editStatus == 1)
            {
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
                    sentTypeList.Text = dataRow[3].ToString().TrimEnd();
                    vehicleTypeAndNumberText.Text = dataRow[4].ToString().TrimEnd();
                    GoodsCategoryText.Text = dataRow[5].ToString().TrimEnd();
                    startingStationList.Text = dataRow[6].ToString().TrimEnd();
                    destinationStationList.Text = dataRow[7].ToString().TrimEnd();
                    vehicleTypeText.Text = dataRow[8].ToString().TrimEnd();
                    AcceptancePlan__shipperName_Text.Text = dataRow[9].ToString().TrimEnd();
                    AcceptancePlan__shipperAddress_Text.Text = dataRow[10].ToString().TrimEnd();

                    AcceptancePlan__shipperPhoneNum_Text.Text = dataRow[11].ToString().TrimEnd();
                    AcceptancePlan__receiverName_Text.Text = dataRow[12].ToString().TrimEnd();
                    AcceptancePlan__receiverAddress_Text.Text = dataRow[13].ToString().TrimEnd();
                    AcceptancePlan__receiverPhoneNum_Text.Text = dataRow[14].ToString().TrimEnd();
                    agentText.Text = dataRow[15].ToString().TrimEnd();
                    acceptanceTimePicker.Value = DateTime.Parse(dataRow[16].ToString());
                    goodsWeightText.Text = dataRow[21].ToString().TrimEnd();
                }

                sqlCnt.Close();
            }


        }

        private void doneButton_Click(object sender, EventArgs e)
        {//提交信息
            //启用数据库
            string str = @"Data Source=.;Initial Catalog=User;Integrated Security=True";
            SqlConnection sqlCnt = new SqlConnection(str);
            sqlCnt.Open();
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlCnt;
            sqlCmd.CommandType = CommandType.Text;
            if (sentTypeList.Text.Length != 0 &&
                startingStationList.Text.Length != 0 &&
                startingStationList.Text.Length != 0 &&
                 AcceptancePlan__shipperName_Text.Text.Length != 0 &&
                 AcceptancePlan__shipperAddress_Text.Text.Length != 0 &&
                 AcceptancePlan__shipperPhoneNum_Text.Text.Length != 0 &&
                 AcceptancePlan__receiverName_Text.Text.Length != 0 &&
                 AcceptancePlan__receiverAddress_Text.Text.Length != 0 &&
                 AcceptancePlan__receiverPhoneNum_Text.Text.Length != 0 &&
                 vehicleTypeAndNumberText.Text.Length != 0 &&
                 vehicleTypeText.Text.Length != 0 &&
                  agentText.Text.Length != 0 &&
                  goodsWeightText.Text.Length != 0)
            {
                //询问用户是否添加
                DialogResult dr;
                dr = MessageBox.Show("是否确认添加该信息？", "确定", MessageBoxButtons.YesNo,
                         MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                if (dr == DialogResult.Yes)
                {//如果用户同意添加
                    
                    //增加了planType类型，为1则说明是原题的编辑，为2则是月计划受理。二者公用同一个数据库
                    //使用editStatus判断是添加还是修改
                    if(editStatus == 0)
                    {//添加
                        sqlCmd.CommandText = "insert into AcceptancePlan (userName,planType,planID,sentType,vehicleTypeAndNumber,vehicleType,AcceptancePlan_shipperName,AcceptancePlan_shipperAddress,AcceptancePlan_shipperPhoneNum,AcceptancePlan_receiverName,AcceptancePlan_receiverAddress,AcceptancePlan_receiverPhoneNum,agent,acceptanceTime,goodsCategory,startingStation,destinationStation,goodsWeight) values('" + userName+"','1','" + planID + "','" + sentTypeList.Text + "','" + vehicleTypeAndNumberText.Text + "','" + vehicleTypeText.Text + "','" + AcceptancePlan__shipperName_Text.Text + "','" + AcceptancePlan__shipperAddress_Text.Text + "','" + AcceptancePlan__shipperPhoneNum_Text.Text + "','" + AcceptancePlan__receiverName_Text.Text + "','" + AcceptancePlan__receiverAddress_Text.Text + "','" + AcceptancePlan__receiverPhoneNum_Text.Text + "','" + agentText.Text + "','" + acceptanceTimePicker.Value.ToShortDateString() + "','" + GoodsCategoryText.Text + "','" + startingStationList.Text + "','" + destinationStationList.Text + "','"+goodsWeightText.Text+"')";
                        sqlCmd.ExecuteNonQuery();
                    }else if(editStatus == 1)
                    {
                        sqlCmd.CommandText = "update AcceptancePlan set sentType = '"+sentTypeList.Text+ "',AcceptancePlan_receiverName ='" + AcceptancePlan__receiverName_Text.Text + "',AcceptancePlan_receiverAddress ='" + AcceptancePlan__receiverAddress_Text.Text + "' ,AcceptancePlan_receiverPhoneNum ='" + AcceptancePlan__receiverPhoneNum_Text.Text + "', agent = '" + agentText.Text + "' , startingStation = '" + startingStationList.Text + "', destinationStation = '" + destinationStationList.Text + "' ,goodsWeight = '" + goodsWeightText.Text + "',AcceptancePlan_shipperPhoneNum ='" + AcceptancePlan__shipperPhoneNum_Text.Text + "',AcceptancePlan_shipperAddress ='" + AcceptancePlan__shipperAddress_Text.Text + "',AcceptancePlan_shipperName ='" + AcceptancePlan__shipperName_Text.Text + "', vehicleType = '" + vehicleTypeText.Text + "', vehicleTypeAndNumber = '" + vehicleTypeAndNumberText.Text + "',acceptanceTime = '" + acceptanceTimePicker.Value.ToShortDateString() + "' where planID = '" + planID + "' and userName = '" + _userName + "'";
                        sqlCmd.ExecuteNonQuery();
                    }


                    //在主页面的列表中添加该项目
                    homeScreen form = (homeScreen)this.Owner;
                    form.initAcceptancePlanList(1);

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



    }
}
