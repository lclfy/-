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
    public partial class CargoDetails : Form
    {
        //传入运单号和用户名以便于检索
        private string _trackingNumber;
        public string trackingNumber
        {
            get { return _trackingNumber; }
            set { _trackingNumber = value; }
        }
        private string _userName;
        public string userName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        //将货物名称存储起来，作为更新数据库时候唯一的参照
        public List<string> goodsNames = new List<string>();

        public CargoDetails(string tempTrackingNumber , string tempUserName)
        {
            //传入用户名和运单号以便于检索
            userName = tempUserName;
            trackingNumber = tempTrackingNumber;
            InitializeComponent();
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
            sqlCmd.CommandText = "select * from GoodsAcceptance where trackingNumber = '" + _trackingNumber + "' and userName = '" + _userName + "'";
            DataSet dataSetForGoodsAcceptance = new DataSet();
            SqlDataAdapter daGA = new SqlDataAdapter(sqlCmd.CommandText, sqlCnt);
            daGA.Fill(dataSetForGoodsAcceptance);
            //把货物数据从数据库上导入然后存储起来
            sqlCmd.CommandText = "select * from Goods where trackingNumber = '"+_trackingNumber+"' and userName = '"+_userName+"'";
            DataSet dataSetForGoods = new DataSet();
            SqlDataAdapter daG = new SqlDataAdapter(sqlCmd.CommandText,sqlCnt );
            daG.Fill(dataSetForGoods);
            //初始化货物详情表格
            goodsDetailListView.View = View.Details;
            string[] goodsDetailListTitles = new string[] { "货物名称", "件数", "寄送人确定重量(kg)","铁路确定重量(kg)", "计费重量"};
            for (int i = 0; i < goodsDetailListTitles.Count(); i++)
            {
                ColumnHeader ch = new ColumnHeader();
                ch.Text = goodsDetailListTitles[i];   //设置列标题 
                if (i == 2 || i == 3)
                    ch.Width = 120;
                else if (i == 0)
                    ch.Width = 100;
                else if (i == 1)
                    ch.Width = 50;
                else if (  i == 4)
                    ch.Width = 70;
                this.goodsDetailListView.Columns.Add(ch);    //将列头添加到ListView控件。
            }

            this.goodsDetailListView.BeginUpdate();

            foreach (DataRow dataRow in dataSetForGoodsAcceptance.Tables[0].Rows)   //添加运单的数据 
            {
                //为界面上的元素赋值
                vehicleNumberText.Text = dataRow[12].ToString().TrimEnd();//车种编号
                truckScaleText.Text = dataRow[13].ToString().TrimEnd();//货车标重
                railwayTarpNumberText.Text = dataRow[14].ToString().TrimEnd();//铁路篷布号码
                throughText.Text = dataRow[15].ToString().TrimEnd();//经由
                containerNumberText.Text = dataRow[16].ToString().TrimEnd();//集装箱号码
                sealNumberText.Text = dataRow[17].ToString().TrimEnd();//施封号码
                transportMileageText.Text = dataRow[18].ToString().TrimEnd();//运价里程
                tip2Text.Text = dataRow[20].ToString().TrimEnd();//备注
            }

            foreach (DataRow dataRow in dataSetForGoods.Tables[0].Rows)   //添加货物的数据 
            {
                ListViewItem lvi = new ListViewItem();
                //第一列 货物名称
                lvi.SubItems[0].Text = dataRow[2].ToString().TrimEnd();
                goodsNames.Add(dataRow[2].ToString());
                //第二列 货物数量
                lvi.SubItems.Add(dataRow[3].ToString().TrimEnd());
                //第三列 寄送人确定重量
                lvi.SubItems.Add(dataRow[6].ToString().TrimEnd());
                    //第四列 铁路确定重量
                    lvi.SubItems.Add(dataRow[7].ToString().TrimEnd());
                    //第五列 计费重量
                    lvi.SubItems.Add(dataRow[8].ToString().TrimEnd());

                this.goodsDetailListView.Items.Add(lvi);
                ImageList imgList = new ImageList();
                imgList.ImageSize = new Size(1, 20);// 设置行高 20 //分别是宽和高 
                goodsDetailListView.SmallImageList = imgList; //这里设置listView的SmallImageList ,用imgList将其撑大 
            }
            this.goodsDetailListView.EndUpdate();  //结束数据处理，UI界面一次性绘制。

            //初始化受理状态和运单号
            acceptStateLabel.Text = "已受理";
            trackingNumberLabel.Text = trackingNumber.ToString();

            //初始状态下不可点击修改按钮
            //editIncomingDetailButton.Enabled = false;
            //初始状态下默认选择第一个项目
            if(goodsDetailListView.Items.Count != 0)
            {
                goodsDetailListView.Items[0].Selected = true;
            }


            sqlCnt.Close();

        }

        private void goodsDetailListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(goodsDetailListView.SelectedItems.Count == 0)
            {
                editIncomingDetailButton.Enabled = false;
            }
            else
            {
                goodsWeightByRailText.Text = goodsDetailListView.SelectedItems[0].SubItems[3].Text;
                chargedWeightText.Text = goodsDetailListView.SelectedItems[0].SubItems[4].Text;
                editIncomingDetailButton.Enabled = true;
            }
        }

        private void doneButton_Click(object sender, EventArgs e)
        {//点击完成按钮
         //启用数据库
            string str = @"Data Source=.;Initial Catalog=User;Integrated Security=True";
            SqlConnection sqlCnt = new SqlConnection(str);
            sqlCnt.Open();
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlCnt;
            sqlCmd.CommandType = CommandType.Text;
            //判断点击该按钮时的受理状态（可能已经添加过核算制票为2了）

            //对元素进行判空
            for (int i = 0; i < goodsDetailListView.Items.Count; i++)
            {//判断计费重量和铁路确认重量是不是填上了
                if(goodsDetailListView.Items[i].SubItems[3].Text.Length == 0 && goodsDetailListView.Items[i].SubItems[4].Text.Length == 0)
                {
                    MessageBox.Show("请检查确认重量与计费重量是否填写完毕", "提示");
                    sqlCnt.Close();
                    return;
                }
            }
            if (vehicleNumberText.Text.Length != 0 &&
                truckScaleText.Text.Length != 0 &&
                railwayTarpNumberText.Text.Length != 0 &&
                throughText.Text.Length != 0 &&
                containerNumberText.Text.Length != 0 &&
                sealNumberText.Text.Length != 0 &&
                transportMileageText.Text.Length != 0 &&
                goodsDetailListView.Items.Count != 0)
            {
                //询问用户是否添加
                DialogResult dr;
                dr = MessageBox.Show("是否确认添加该信息？", "确定", MessageBoxButtons.YesNo,
                         MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                if (dr == DialogResult.Yes)
                {
                    //在数据库中添加所有已经更新的货物信息
                    for (int k = 0; k < goodsDetailListView.Items.Count; k++)
                    {
                        //在数据库中添加goodsListView的所有信息
                        sqlCmd.CommandText = "update Goods set goodsWeightByRail = '"+ goodsDetailListView.Items[k].SubItems[3].Text + "' , chargedWeight = '"+ goodsDetailListView.Items[k].SubItems[4].Text + "' where userName = '" + _userName + "' and trackingNumber = '" + _trackingNumber + "' and goodsName = '"+goodsNames[k]+"' ";
                        sqlCmd.ExecuteNonQuery();
                    }
                    //计算运到期限（不足3天为3天）
                    float tempTrackingDays = (1 + (float.Parse(transportMileageText.Text) / 250));
                    int trackingDays;
                    if (tempTrackingDays*10 % 10 != 0)
                    {
                        tempTrackingDays++;
                        trackingDays = (int)tempTrackingDays;
                    }
                    else
                    {
                        trackingDays = (int)tempTrackingDays;
                    }
                    //在数据库中添加输入框中的所有信息(当受理状态为0时改为1)
                    sqlCmd.CommandText = "update GoodsAcceptance set vehicleNumber = '"+vehicleNumberText.Text+ "' , truckScale = '"+truckScaleText.Text+ "', railwayTarpNumber = '"+railwayTarpNumberText.Text+ "', through = '"+throughText.Text+ "' , containerNumber = '"+containerNumberText.Text+ "' , sealNumber = '"+sealNumberText.Text+ "' , transportMileage = '"+transportMileageText.Text+ "', cargoDetailsTip = '"+tip2Text.Text+ "', acceptState = '1', trackingDays = '" + trackingDays.ToString() + "'  where userName = '" + _userName+ "' and trackingNumber = '"+_trackingNumber+"' ";
                    sqlCmd.ExecuteNonQuery();

                    //将模型回传给主页面控制器并刷新它的列表
                    homeScreen _homeScreen = (homeScreen)this.Owner;
                     _homeScreen.initServiceCheckListView();



                    //跳转到核算制票阶段
                    //询问用户是否要跳转到核算制票
                    DialogResult dr1;
                    dr1 = MessageBox.Show("修改运单完成，需要进行核算制票，是否进入核算制票？", "提示", MessageBoxButtons.YesNo,
                             MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    if (dr1 == DialogResult.Yes)
                    {
                        AddTicketPriceCount form = new AddTicketPriceCount(trackingNumber,userName);
                        form.Owner = this.Owner;
                        form.Show();
                    }
                    //关闭窗体和数据库
                    sqlCnt.Close();
                    this.Close();
                }
                else
                {
                    //用户选择了取消
                    //直接结束方法和数据库
                    sqlCnt.Close();
                    return;
                }
            }
            else
            {
                sqlCnt.Close();
                MessageBox.Show("请添加所有信息", "提示", MessageBoxButtons.YesNo,
                         MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }

        private void editIncomingDetailButton_Click(object sender, EventArgs e)
        {//修改表格信息
         //             ====修改====
            if (goodsWeightByRailText.Text.Length != 0 &&
                 chargedWeightText.Text.Length != 0)
            {//对填写元素进行判空
                //对表格进行修改
                goodsDetailListView.SelectedItems[0].SubItems[3].Text = goodsWeightByRailText.Text;
                goodsDetailListView.SelectedItems[0].SubItems[4].Text = chargedWeightText.Text;
                //清空输入框文字
                goodsWeightByRailText.Text = "";
                chargedWeightText.Text = "";
                //如果填写完成的下一条仍然有数据，则光标自动移动到下一行
                if (goodsDetailListView.SelectedItems[0].Index != goodsDetailListView.Items.Count - 1)
                {
                    goodsDetailListView.Items[goodsDetailListView.SelectedItems[0].Index + 1].Selected = true;
                    goodsDetailListView.Items[goodsDetailListView.SelectedItems[0].Index].Selected = false;

                }
            }
            else
            {
                MessageBox.Show("请输入所有信息后点击添加", "提示");
            }
        }
        private void goodsText_KeyPress(object sender, KeyPressEventArgs e)
        {//用于控制输入为数字的方法
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)13 && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }
    }
}
