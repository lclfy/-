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
    public partial class homeScreen : Form
    {
        //用户名
        private string _userName;
        public string userName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
            }
        }
        //用户类型(0为用户，1为职工，2为管理员)
        private int _userType;
        public int userType
        {
            get
            {
                return _userType;
            }
            set
            {
                _userType = value;
            }
        }
        public bool formStateForTrackingNumber;//通过一个bool值判断当前是否可以添加新的运单号，因为一单只能生成一个运单号。
        public string trackingNumber;//运单号
        public DateTime trackingDate;//货物受理内当前的提交时间
        //关于受理状态的解释，自定义的受理状态int acceptState为一个数字，当其为0的时候，表示货物受理添加完毕，即将进行进货检验。当其为1的时候，表示进货检验受理完毕，即将进行核算制票，当其为2时，则表示核算制票已经完毕，运单处理完毕。


        public homeScreen(string username, int tempUserType)
        {//加载页面需要传入用户名和用户类型，并根据用户类型初始化界面
            this._userName = username;
            this.userType = tempUserType;
            InitializeComponent();
            this.initLists();
            //设置初始化状态下的货物受理提交按钮
            submitButton.Tag = "0";
            //如果是普通用户进入，则禁用部分功能，所有用户进入之后都将显示货物列表
            if (userType != 2)
            {//如果不是管理员，就不能对货物信息执行删除操作
                deleteButton_incomingCheck.Hide();
            }
            if (userType == 0)
            {//是普通用户
                homePageControl.SelectedTab = homePageControl.TabPages[1];
                //禁用计划受理
                homePageControl.TabPages[0].Parent = null;
                //禁用按钮
                editButton_incomingCheck.Hide();
                editButton_CaculationPrice.Hide();
                deleteButton_incomingCheck.Hide();


            }
            else
            {
                homePageControl.SelectedTab = homePageControl.TabPages[2];
            }

            //禁用一些按钮
            deleteMonthPlanButton.Enabled = false;
            editMonthPlanButton.Enabled = false;
            deleteOriginalListButton.Enabled = false;
            editOriginalListButton.Enabled = false;
            deleteButton_incomingCheck.Enabled = false;
            stopSearchButton.Enabled = false;


        }

        private void homePageControl_Selected(object sender, TabControlEventArgs e)
        {//对顶栏选中项目更改之后的控制，这里控制的是主动切换到非货物受理页面和切换到货物受理页面时候，分别执行的动作
            if (userType == 0)
            {//是用户
                if (homePageControl.SelectedTab == homePageControl.TabPages[0])
                {//被切换到货物受理界面
                    trackingTimeLabel.Text = DateTime.Now.ToShortDateString();
                    trackingTimeLabel2.Text = trackingTimeLabel.Text;

                }
                else
                {//否则就给出提示，提示填写的东西可能丢失
                    if (goodsNameText.Text.Length != 0 ||
                        goodsCountText.Text.Length != 0 ||
                        goodsPackageText.Text.Length != 0 ||
                        goodsPriceText.Text.Length != 0 ||
                        goodsWeightText.Text.Length != 0 ||
                        startingStationList.Text.Length != 0 ||
                        destinationStationList.Text.Length != 0 ||
                        destinationText.Text.Length != 0 ||
                        shipperNameText.Text.Length != 0 ||
                        shipperAddressText.Text.Length != 0 ||
                        shipperPhoneNumText.Text.Length != 0 ||
                        receiverNameText.Text.Length != 0 ||
                        receiverAddressText.Text.Length != 0 ||
                        receiverPhoneNumText.Text.Length != 0 ||
                        goodsListView.Items.Count != 0)
                    {//只要填了一个项目 就询问用户是否要离开页面
                        trackingTimeLabel.Text = DateTime.Now.ToShortDateString();
                        trackingTimeLabel2.Text = trackingTimeLabel.Text;
                        DialogResult dr;
                        dr = MessageBox.Show("是否确认停止添加货物信息？已填写的信息会丢失（查看原有信息则不受影响）", "确定", MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        if (dr == DialogResult.Yes)
                        {//清空所有内容
                            goodsNameText.Text = "";
                            goodsCountText.Text = "";
                            goodsPackageText.Text = "";
                            goodsPriceText.Text = "";
                            goodsWeightText.Text = "";
                            startingStationList.Text = "";
                            destinationStationList.Text = "";
                            destinationText.Text = "";
                            shipperNameText.Text = "";
                            shipperAddressText.Text = "";
                            shipperPhoneNumText.Text = "";
                            receiverNameText.Text = "";
                            receiverAddressText.Text = "";
                            receiverPhoneNumText.Text = "";
                            vehicleNumberInPickingText.Text = "";
                            destinationStationInPickingText.Text = "";
                            receiverNameInPickingText.Text = "";
                            shipperNameInPickingText.Text = "";
                            trackingDaysLabel.Text = "";
                            trackingDaysText.Text = "";
                            startingStationInPickingText.Text = "";
                            destinationStationInPickingText.Text = "";
                            //调整readonly属性
                            //创建出来的货物受理页面不能够编辑
                            goodsNameText.ReadOnly = false;
                            goodsCountText.ReadOnly = false;
                            goodsPackageText.ReadOnly = false;
                            goodsPriceText.ReadOnly = false;
                            goodsWeightText.ReadOnly = false;
                            destinationText.ReadOnly = false;
                            shipperNameText.ReadOnly = false;
                            shipperAddressText.ReadOnly = false;
                            shipperPhoneNumText.ReadOnly = false;
                            receiverNameText.ReadOnly = false;
                            receiverAddressText.ReadOnly = false;
                            receiverPhoneNumText.ReadOnly = false;
                            //本来是打印按钮，现在是完成按钮
                            submitButton.Tag = 0;
                            submitButton.Text = "完成";
                            //删除表格里面的元素
                            for (int k = 0; k < goodsListView.Items.Count;)
                            {
                                goodsListView.Items.RemoveAt(0);
                            }
                            
                            for (int k = 0; k < pickingListView.Items.Count;)
                            {
                                pickingListView.Items.RemoveAt(0);
                            }
                            
                        }
                        else
                        {
                            homePageControl.SelectedTab = homePageControl.TabPages[0];
                        }
                    }

                }
            }
            else
            {
                if (homePageControl.SelectedTab == homePageControl.TabPages[1])
                {//被切换到货物受理界面
                    trackingTimeLabel.Text = DateTime.Now.ToShortDateString();
                    trackingTimeLabel2.Text = trackingTimeLabel.Text;

                }
                else
                {//否则就给出提示，提示填写的东西可能丢失
                    if (goodsNameText.Text.Length != 0 ||
                        goodsCountText.Text.Length != 0 ||
                        goodsPackageText.Text.Length != 0 ||
                        goodsPriceText.Text.Length != 0 ||
                        goodsWeightText.Text.Length != 0 ||
                        startingStationList.Text.Length != 0 ||
                        destinationStationList.Text.Length != 0 ||
                        destinationText.Text.Length != 0 ||
                        shipperNameText.Text.Length != 0 ||
                        shipperAddressText.Text.Length != 0 ||
                        shipperPhoneNumText.Text.Length != 0 ||
                        receiverNameText.Text.Length != 0 ||
                        receiverAddressText.Text.Length != 0 ||
                        receiverPhoneNumText.Text.Length != 0 ||
                        goodsListView.Items.Count != 0)
                    {//只要填了一个项目 就询问用户是否要离开页面
                        trackingTimeLabel.Text = DateTime.Now.ToShortDateString();
                        trackingTimeLabel2.Text = trackingTimeLabel.Text;
                        DialogResult dr;
                        dr = MessageBox.Show("是否确认停止添加货物信息？已填写的信息会丢失", "确定", MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        if (dr == DialogResult.Yes)
                        {//清空所有内容
                            goodsNameText.Text = "";
                            goodsCountText.Text = "";
                            goodsPackageText.Text = "";
                            goodsPriceText.Text = "";
                            goodsWeightText.Text = "";
                            startingStationList.Text = "";
                            destinationStationList.Text = "";
                            destinationText.Text = "";
                            shipperNameText.Text = "";
                            shipperAddressText.Text = "";
                            shipperPhoneNumText.Text = "";
                            receiverNameText.Text = "";
                            receiverAddressText.Text = "";
                            receiverPhoneNumText.Text = "";
                            vehicleNumberInPickingText.Text = "";
                            destinationStationInPickingText.Text = "";
                            receiverNameInPickingText.Text = "";
                            shipperNameInPickingText.Text = "";
                            trackingDaysLabel.Text = "";
                            trackingDaysText.Text = "";
                            startingStationInPickingText.Text = "";
                            destinationStationInPickingText.Text = "";
                            //调整readonly属性
                            //创建出来的货物受理页面不能够编辑
                            goodsNameText.ReadOnly = false;
                            goodsCountText.ReadOnly = false;
                            goodsPackageText.ReadOnly = false;
                            goodsPriceText.ReadOnly = false;
                            goodsWeightText.ReadOnly = false;
                            destinationText.ReadOnly = false;
                            shipperNameText.ReadOnly = false;
                            shipperAddressText.ReadOnly = false;
                            shipperPhoneNumText.ReadOnly = false;
                            receiverNameText.ReadOnly = false;
                            receiverAddressText.ReadOnly = false;
                            receiverPhoneNumText.ReadOnly = false;
                            //本来是打印按钮，现在是完成按钮
                            submitButton.Tag = 0;
                            submitButton.Text = "完成";
                            //删除表格里面的元素
                            for (int k = 0; k < goodsListView.Items.Count;)
                            {
                                goodsListView.Items.RemoveAt(0);
                            }
                            
                            for (int k = 0; k < pickingListView.Items.Count;)
                            {
                                pickingListView.Items.RemoveAt(0);
                            }
                            
                        }

                    }
                }
            }


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void homeScreen_Load(object sender, EventArgs e)
        {

        }

        public void initLists()
        {
            //服务列表里面的按钮，初始状态不能用
            editButton_CaculationPrice.Enabled = false;
            editButton_incomingCheck.Enabled = false;
            //进货检验+核算制票表初始化
            ServiceCheckListView.View = View.Details;
            string[] ServiceCheckListTitles;
            if (userType == 0)
            {//用户的话，不显示该运单属于哪个用户
                ServiceCheckListTitles = new string[] { "运单号", "经由", "寄送人姓名", "收货人姓名" };
            }
            else
            {
                ServiceCheckListTitles = new string[] { "运单号", "经由", "寄送人姓名", "收货人姓名", "提交用户名" };
            }

            for (int i = 0; i < ServiceCheckListTitles.Count(); i++)
            {
                ColumnHeader ch = new ColumnHeader();
                ch.Text = ServiceCheckListTitles[i];   //设置列标题 
                ch.Width = 120;
                this.ServiceCheckListView.Columns.Add(ch);    //将列头添加到ListView控件。
            }
            //初始化信息
            initServiceCheckListView();

            //经由->发送吨表格初始化
            analysisListView.View = View.Details;
            analysisListView.ShowGroups = false;
            string[] analysisListViewTitle = new string[] { "经由", "吨数" };
            for (int i = 0; i < analysisListViewTitle.Count(); i++)
            {
                ColumnHeader ch = new ColumnHeader();
                ch.Text = analysisListViewTitle[i];   //设置列标题 
                ch.Width = 105;
                this.analysisListView.Columns.Add(ch);    //将列头添加到ListView控件。
            }
            //初始化信息
            initAnalysisListView();

            //货物受理中的货物列表初始化
            goodsListView.View = View.Details;
            goodsListView.ShowGroups = false;
            string[] goodsListTitles = new string[] { "货物名称", "件数", "包装", "货物价格", "托运人确定重量(kg)", "铁路确定重量(kg)", "计费重量", "运价号", "运价率", "运费" };
            for (int i = 0; i < goodsListTitles.Count(); i++)
            {
                ColumnHeader ch = new ColumnHeader();
                ch.Text = goodsListTitles[i];   //设置列标题 
                if (i == 4 || i == 5)
                {
                    ch.Width = 150;    //设置列宽度 
                }
                else if (i == 1 || i == 3)
                {
                    ch.Width = 80;
                }
                else
                {
                    ch.Width = 100;
                }
                this.goodsListView.Columns.Add(ch);    //将列头添加到ListView控件。

            }

            //领货凭证中的货物列表初始化
            pickingListView.View = View.Details;
            pickingListView.ShowGroups = false;
            string[] pickingListTitle = new string[] { "货物名称", "件数", "重量" };
            for (int i = 0; i < pickingListTitle.Count(); i++)
            {
                ColumnHeader ch = new ColumnHeader();
                ch.Text = pickingListTitle[i];   //设置列标题 
                ch.Width = 70;
                this.pickingListView.Columns.Add(ch);    //将列头添加到ListView控件。

            }

            //计划受理中的表初始化


            //货物品类/重量
            goodsTypeAndWeightListView.View = View.Details;
            goodsTypeAndWeightListView.ShowGroups = false;
            string[] goodsTypeAndWeightListTitles = new string[] { "货物名称", "重量" };
            for (int i = 0; i < goodsTypeAndWeightListTitles.Count(); i++)
            {
                ColumnHeader ch = new ColumnHeader();
                ch.Text = goodsTypeAndWeightListTitles[i];   //设置列标题 
                ch.Width = 180;
                this.goodsTypeAndWeightListView.Columns.Add(ch);    //将列头添加到ListView控件。
            }


            //原题编辑列表（和统计分析）
            originalEditListView.View = View.Details;
            originalEditListView.ShowGroups = false;
            string[] originalEditListTitles = new string[] { "计划编码", "发出类型", "车种车号", "货物品类", "货物重量", "发站", "到站", "车型", "托运人", "托运人地址", "托运人电话", "收货人", "收货人地址", "收货人电话", "经办人", "受理时间" };
            for (int i = 0; i < originalEditListTitles.Count(); i++)
            {
                ColumnHeader ch = new ColumnHeader();
                ch.Text = originalEditListTitles[i];   //设置列标题 
                ch.Width = 100;
                this.originalEditListView.Columns.Add(ch);    //将列头添加到ListView控件。
            }
            this.initAcceptancePlanList(1);

            //月计划编制表
            MonthPlanPreparationListView.View = View.Details;
            MonthPlanPreparationListView.ShowGroups = false;
            string[] MonthPlanPreparationListTitles = new string[] { "计划编码", "货物品类", "计划日均", "计划一车净载重", "计划车数", "计划发送量" };
            for (int i = 0; i < MonthPlanPreparationListTitles.Count(); i++)
            {
                ColumnHeader ch = new ColumnHeader();
                ch.Text = MonthPlanPreparationListTitles[i];   //设置列标题 
                if (i == 3)
                {
                    ch.Width = 140;
                }
                else
                {
                    ch.Width = 100;
                }

                this.MonthPlanPreparationListView.Columns.Add(ch);    //将列头添加到ListView控件。
            }
            this.initAcceptancePlanList(2);

        }



        /*
        ========================
        ========================
       =========货物受理==========
        ========================
        ========================
        */

        /*
        ========================
        ========================
       =========生成单号==========
        ========================
        ========================
        */

        public void getTrackingNumber()
        {
            if (formStateForTrackingNumber == false)
            {//为false即生成一个运单号
             //产生以日期+序号为单位的运单号
                string nowDate = DateTime.Now.ToString("yyyyMMdd");

                //查找数据库中有多少当天发送的
                //启用数据库
                string str = @"Data Source=.;Initial Catalog=User;Integrated Security=True";
                SqlConnection sqlCnt = new SqlConnection(str);
                sqlCnt.Open();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = sqlCnt;
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "select * from GoodsAcceptance where trackingNumber LIKE'%" + nowDate + "%'";
                DataSet dataSetForGoods = new DataSet();
                SqlDataAdapter daG = new SqlDataAdapter(sqlCmd.CommandText, sqlCnt);
                daG.Fill(dataSetForGoods);
                //如果表内没有数据，编号就从01开始
                if (dataSetForGoods.Tables[0].Rows.Count == 0)
                {
                    nowDate = nowDate + "01";
                }
                else
                {
                    int count = 0;
                    foreach (DataRow row in dataSetForGoods.Tables[0].Rows)
                    {//tempNumber用来获取最后一个添加的单号，以此单号为准+1

                        if (count == dataSetForGoods.Tables[0].Rows.Count - 1)
                        {
                            int tempNumber = int.Parse(row[11].ToString().TrimEnd().Remove(0, 8));
                            tempNumber++;
                            if (tempNumber >= 10)
                            {//如果号数大于10 ，就不能在前面加0了
                                nowDate = nowDate + tempNumber.ToString();
                            }
                            else
                            {
                                nowDate = nowDate + "0" + tempNumber.ToString();
                            }
                        }
                        count++;
                    }
                }


                sqlCnt.Close();
                //赋值
                trackingNumber = nowDate;
            }
            else
            {
                return;
            }
        }

        //提交按钮

        private void submitButton_Click(object sender, EventArgs e)
        {
         //启用数据库
         if(int.Parse(submitButton.Tag.ToString()) == 0)
            {//提交货物受理信息的按钮
                string str = @"Data Source=.;Initial Catalog=User;Integrated Security=True";
                SqlConnection sqlCnt = new SqlConnection(str);
                sqlCnt.Open();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = sqlCnt;
                sqlCmd.CommandType = CommandType.Text;

                {//对元素进行判空，如果不空，则添加到数据库内，如果为空，则提示没有填写完整（备注可以为空）
                    if (startingStationList.Text.Length != 0 &&
                        destinationStationList.Text.Length != 0 &&
                        destinationText.Text.Length != 0 &&
                        shipperNameText.Text.Length != 0 &&
                        shipperAddressText.Text.Length != 0 &&
                        shipperPhoneNumText.Text.Length != 0 &&
                        receiverNameText.Text.Length != 0 &&
                        receiverAddressText.Text.Length != 0 &&
                       receiverPhoneNumText.Text.Length != 0 &&
                       goodsListView.Items.Count != 0)
                    {
                        //询问用户是否添加
                        DialogResult dr;
                        dr = MessageBox.Show("是否确认添加该货物受理信息？", "确定", MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        if (dr == DialogResult.Yes)
                        {//如果用户同意添加

                            //创建一个新的运单号（自动判断有没有切换页面）
                            this.getTrackingNumber();

                            //把除了goodsListView之外的数据添加到数据库
                            sqlCmd.CommandText = "insert into GoodsAcceptance (userName,startingStation,destinationList,destinationText,GoodsAcceptance_shipperName,GoodsAcceptance_shipperAddress,GoodsAcceptance_shipperPhoneNum,GoodsAcceptance_receiverName,GoodsAcceptance_receiverAddress,GoodsAcceptance_receiverPhoneNum,goodsAcceptanceTip,trackingNumber,acceptState,trackingTime) values('" + userName + "','" + startingStationList.Text + "','" + destinationStationList.Text + "','" + destinationText.Text + "','" + shipperNameText.Text + "','" + shipperAddressText.Text + "','" + shipperPhoneNumText.Text + "','" + receiverNameText.Text + "','" + receiverAddressText.Text + "','" + receiverPhoneNumText.Text + "','" + tip1Text.Text + "','" + trackingNumber + "','0','" + trackingTimeLabel.Text + "')";
                            sqlCmd.ExecuteNonQuery();

                            //添加goodsListView中的内容
                            for (int k = 0; k < goodsListView.Items.Count; k++)
                            {
                                //在数据库中添加goodsListView的所有信息
                                sqlCmd.CommandText = "insert into Goods (userName,trackingNumber,goodsName,goodsCount,goodsPackage,goodsPrice,goodsWeightByShipper) values ('" + userName + "','" + trackingNumber + "','" + goodsListView.Items[k].SubItems[0].Text + "','" + int.Parse(goodsListView.Items[k].SubItems[1].Text) + "','" + goodsListView.Items[k].SubItems[2].Text + "','" + int.Parse(goodsListView.Items[k].SubItems[3].Text) + "','" + int.Parse(goodsListView.Items[k].SubItems[4].Text) + "')";
                                sqlCmd.ExecuteNonQuery();
                            }

                            //在服务列表中添加该项目
                            initServiceCheckListView();


                            //将所有填写的东西置空
                            startingStationList.Text = "";
                            destinationStationList.Text = "";
                            destinationText.Text = "";
                            shipperNameText.Text = "";
                            shipperAddressText.Text = "";
                            shipperPhoneNumText.Text = "";
                            receiverNameText.Text = "";
                            receiverAddressText.Text = "";
                            receiverPhoneNumText.Text = "";
                            tip1Text.Text = "";
                            this.resetInputTextField();
                            int j = goodsListView.Items.Count;
                            //删除表格里面的元素
                            for (int k = 0; k < goodsListView.Items.Count;)
                            {
                                goodsListView.Items.RemoveAt(0);
                            }
                            //切换到进货检验的列表(要根据用户类型判断跳到第几页)
                            if (userType == 0)
                            {
                                homePageControl.SelectedTab = homePageControl.TabPages[1];
                                dr = MessageBox.Show("提交成功，将等待工作人员进行受理。", "提示", MessageBoxButtons.YesNo,
    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            }
                            else
                            {
                                homePageControl.SelectedTab = homePageControl.TabPages[2];
                                //打开进货检验页面并将之前整个存储的模型传输给它
                                CargoDetails form = new CargoDetails(trackingNumber, userName);
                                form.Owner = this;
                                form.ShowDialog();
                            }


                            //可以继续生成新运单号
                            formStateForTrackingNumber = false;

                            //关闭数据库并直接结束方法
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
                //如果没有进行运算，则说明有信息不完整，给出提示框，关闭数据库，结束方法。
                MessageBox.Show("请填写所有信息后提交", "提示");
                sqlCnt.Close();
                return;
            }
            else
            {
                //对元素进行判空
                for (int i = 0; i < goodsListView.Items.Count; i++)
                {//判断运价号是不是填好了
                    if (i != goodsListView.Items.Count - 1 && goodsListView.Items[i].SubItems[6].Text.Length == 0 && goodsListView.Items[i].SubItems[7].Text.Length == 0)
                    {
                        MessageBox.Show("请等待职工受理完成后打印", "提示");
                        return;
                    }
                }
                //对界面元素进行判空
                if (vehicleNumberText.Text.Length != 0 &&
                   truckScaleText.Text.Length != 0 &&
                   sealNumberText.Text.Length != 0 &&
                   railwayTarpNumberText.Text.Length != 0 &&
                   containerNumberText.Text.Length != 0)
                {
                    if (userType == 0)
                    {
                        PrintTicket form = new PrintTicket(ServiceCheckListView.SelectedItems[0].SubItems[0].Text.TrimEnd(), userName);
                        form.Owner = this;
                        form.Show();
                    }

                    else
                    {
                        PrintTicket form = new PrintTicket(ServiceCheckListView.SelectedItems[0].SubItems[0].Text.TrimEnd(), ServiceCheckListView.SelectedItems[0].SubItems[4].Text.TrimEnd());
                        form.Owner = this;
                        form.Show();
                    }



                }
                else
                {
                    MessageBox.Show("请等待职工受理完成后打印", "提示", MessageBoxButtons.YesNo,
                             MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
            }
 
        }




        //以下为货物详情的列表控制

        public void resetInputTextField()
        {
            goodsNameText.Text = "";
            goodsCountText.Text = "";
            goodsPackageText.Text = "";
            goodsPriceText.Text = "";
            goodsWeightText.Text = "";
        }

        private void addEditGoodsDetailButton_Click(object sender, EventArgs e)
        {//为货物信息添加/修改数据
            ListViewItem lvi = new ListViewItem("1", 0);

            if (addAndEditGoodsDetailButton.Tag == null)
            {//将添加按钮的Tag设为初始值0
                addAndEditGoodsDetailButton.Tag = 0;
            }

            if ((int)addAndEditGoodsDetailButton.Tag == 0)
            {//Tag为0，说明为添加模式，Tag为1,则为修改模式（人为设定）
                //             ====添加====
                //取出货物模型
                Goods _goodsList = new Goods();
                for (int i = 0; i < 4; i++)
                {
                    lvi.SubItems.Add("");
                }
                //对所有的元素进行判空
                if (goodsNameText.Text.Length != 0 &&
                    goodsCountText.Text.Length != 0 &&
                    goodsPackageText.Text.Length != 0 &&
                    goodsPriceText.Text.Length != 0 &&
                    goodsWeightText.Text.Length != 0)
                {
                    lvi.SubItems[0].Text = goodsNameText.Text;
                    lvi.SubItems[1].Text = goodsCountText.Text;
                    lvi.SubItems[2].Text = goodsPackageText.Text;
                    lvi.SubItems[3].Text = goodsPriceText.Text;
                    lvi.SubItems[4].Text = goodsWeightText.Text;
                    //在表中添加该行信息
                    goodsListView.Items.Add(lvi);

                    //清空输入框文字
                    goodsNameText.Text = "";
                    goodsCountText.Text = "";
                    goodsPackageText.Text = "";
                    goodsPriceText.Text = "";
                    goodsWeightText.Text = "";

                    _goodsList = null;

                }
                else
                {
                    MessageBox.Show("请输入所有信息", "提示");
                }
            }
            else if ((int)addAndEditGoodsDetailButton.Tag == 1)
            {//             ====修改====

                if (goodsNameText.Text.Length != 0 &&
                     goodsCountText.Text.Length != 0 &&
                     goodsPackageText.Text.Length != 0 &&
                     goodsPriceText.Text.Length != 0 &&
                     goodsWeightText.Text.Length != 0)
                {//对所有元素进行判空
                    goodsListView.SelectedItems[0].SubItems[0].Text = goodsNameText.Text;
                    goodsListView.SelectedItems[0].SubItems[1].Text = goodsCountText.Text;
                    goodsListView.SelectedItems[0].SubItems[2].Text = goodsPackageText.Text;
                    goodsListView.SelectedItems[0].SubItems[3].Text = goodsPriceText.Text;
                    goodsListView.SelectedItems[0].SubItems[4].Text = goodsWeightText.Text;


                    //清空输入框文字
                    goodsNameText.Text = "";
                    goodsCountText.Text = "";
                    goodsPackageText.Text = "";
                    goodsPriceText.Text = "";
                    goodsWeightText.Text = "";

                    //将添加按钮Tag为0 改为修改按钮，Tag为1
                    addAndEditGoodsDetailButton.Tag = 0;
                    addAndEditGoodsDetailButton.Text = "添加";
                    //将清空按钮Tag为0 改为返回按钮，Tag为1
                    resetAndReturnGoodsDetailButton.Tag = 0;
                    resetAndReturnGoodsDetailButton.Text = "清空";
                    //禁用删除按钮
                    deleteGoodsDetailButton.Enabled = false;
                }
                else
                {
                    MessageBox.Show("请输入所有信息", "提示");
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

        private void goodsListView_SelectedIndexChanged(object sender, EventArgs e)
        {//用于把表中的数据选出来放到对话框中，同时更改“添加”按钮为“修改”按钮，"清空"按钮为"切回"按钮的方法
            if (goodsListView.SelectedItems.Count == 0)
            {
                //如果选出了表外，就恢复添加模式
                this.resetInputTextField();
                //将添加按钮Tag为0 改为修改按钮，Tag为1
                addAndEditGoodsDetailButton.Tag = 0;
                addAndEditGoodsDetailButton.Text = "添加";
                //将清空按钮Tag为0 改为返回按钮，Tag为1
                resetAndReturnGoodsDetailButton.Tag = 0;
                resetAndReturnGoodsDetailButton.Text = "清空";
                //禁用删除按钮
                deleteGoodsDetailButton.Enabled = false;
                return;
            }

            goodsNameText.Text = goodsListView.SelectedItems[0].SubItems[0].Text;
            goodsCountText.Text = goodsListView.SelectedItems[0].SubItems[1].Text;
            goodsPackageText.Text = goodsListView.SelectedItems[0].SubItems[2].Text;
            goodsPriceText.Text = goodsListView.SelectedItems[0].SubItems[3].Text;
            goodsWeightText.Text = goodsListView.SelectedItems[0].SubItems[4].Text;

            //将添加按钮Tag为0 改为修改按钮，Tag为1
            addAndEditGoodsDetailButton.Tag = 1;
            addAndEditGoodsDetailButton.Text = "修改";
            //将清空按钮Tag为0 改为返回按钮，Tag为1
            resetAndReturnGoodsDetailButton.Tag = 1;
            resetAndReturnGoodsDetailButton.Text = "返回添加";
            //启用删除按钮
            deleteGoodsDetailButton.Enabled = true;


        }

        private void editGoodsDetailButton_Click(object sender, EventArgs e)
        {//清空文本框数据，或者从修改模式切换回添加模式(Tag=0为添加模式，为清空数据。Tag为1为修改模式，为切换回添加模式。
            this.resetInputTextField();
            if (resetAndReturnGoodsDetailButton.Tag == null)
            {//将清空按钮的Tag设为初始值0
                resetAndReturnGoodsDetailButton.Tag = 0;
            }
            if ((int)resetAndReturnGoodsDetailButton.Tag == 1)
            {
                //将添加按钮Tag为0 改为修改按钮，Tag为1
                addAndEditGoodsDetailButton.Tag = 0;
                addAndEditGoodsDetailButton.Text = "添加";
                //将清空按钮Tag为0 改为返回按钮，Tag为1
                resetAndReturnGoodsDetailButton.Tag = 0;
                resetAndReturnGoodsDetailButton.Text = "清空";
            }

        }

        private void deleteGoodsDetailButton_Click(object sender, EventArgs e)
        {//删除货物列表的操作
            this.resetInputTextField();
            //删除表内数据
            goodsListView.Items[goodsListView.SelectedItems[0].Index].Remove();

        }

        /*
========================
========================
====货物受理内容的获取====
==以及获取前添加该单日期==
========================
*/
        public void initGoodsAcceptanceList(string _trackingNumber, string userName)
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
            sqlCmd.CommandText = "select * from GoodsAcceptance where trackingNumber = '" + _trackingNumber + "' and userName = '" + userName + "'";
            DataSet dataSetForGoodsAcceptance = new DataSet();
            SqlDataAdapter daGA = new SqlDataAdapter(sqlCmd.CommandText, sqlCnt);
            daGA.Fill(dataSetForGoodsAcceptance);
            //把货物数据从数据库上导入然后存储起来
            sqlCmd.CommandText = "select * from Goods where trackingNumber = '" + _trackingNumber + "' and userName = '" + userName + "'";
            DataSet dataSetForGoods = new DataSet();
            SqlDataAdapter daG = new SqlDataAdapter(sqlCmd.CommandText, sqlCnt);
            daG.Fill(dataSetForGoods);
            foreach (DataRow dataRow in dataSetForGoodsAcceptance.Tables[0].Rows)   //添加运单的数据 
            {
                //为界面上的元素赋值
                vehicleNumberText.Text = dataRow[12].ToString().TrimEnd();//车种编号
                vehicleNumberInPickingText.Text = vehicleNumberText.Text;
                truckScaleText.Text = dataRow[13].ToString().TrimEnd();//货车标重
                throughText.Text = dataRow[15].ToString().TrimEnd();//经由
                transportMileageText.Text = dataRow[18].ToString().TrimEnd();//运价里程
                railwayTarpNumberText.Text = dataRow[14].ToString().TrimEnd();//铁路篷布号码
                containerNumberText.Text = dataRow[16].ToString().TrimEnd();//集装箱号码
                sealNumberText.Text = dataRow[17].ToString().TrimEnd();//施封号码
                shipperNameText.Text = dataRow[4].ToString().TrimEnd();//寄送人
                shipperNameInPickingText.Text = shipperNameText.Text;
                shipperAddressText.Text = dataRow[5].ToString().TrimEnd();//住址
                shipperPhoneNumText.Text = dataRow[6].ToString().TrimEnd();//电话
                receiverNameText.Text = dataRow[7].ToString().TrimEnd();//收货人
                receiverNameInPickingText.Text = receiverNameText.Text;
                receiverAddressText.Text = dataRow[8].ToString().TrimEnd();//住址
                receiverPhoneNumText.Text = dataRow[9].ToString().TrimEnd();//电话
                startingStationList.Text = dataRow[1].ToString().TrimEnd();//始发站
                startingStationInPickingText.Text = startingStationList.Text;
                destinationStationList.Text = dataRow[2].ToString().TrimEnd();//终点站
                destinationStationInPickingText.Text = destinationStationList.Text;
                trackingTimeLabel.Text = dataRow[29].ToString().TrimEnd();//发送时间
                trackingTimeLabel2.Text = trackingTimeLabel.Text;
                trackingDaysLabel.Text = dataRow[28].ToString().TrimEnd() + "天";//运送天数
                trackingDaysText.Text = trackingDaysLabel.Text;


            }
            //先更新主表
            this.goodsListView.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度 
            foreach (DataRow dataRow in dataSetForGoods.Tables[0].Rows)   //添加货物的数据 
            {
                ListViewItem lvi = new ListViewItem();
                //第1列 货物名称
                lvi.SubItems[0].Text = dataRow[2].ToString().TrimEnd();
                //第2列 货物数量
                lvi.SubItems.Add(dataRow[3].ToString().TrimEnd());
                //第3列 货物包装
                lvi.SubItems.Add(dataRow[4].ToString().TrimEnd());
                //第4列 货物重量
                lvi.SubItems.Add(dataRow[5].ToString().TrimEnd());
                //第5列 寄送人确定重量
                lvi.SubItems.Add(dataRow[6].ToString().TrimEnd());
                //第6列 铁路确定重量
                lvi.SubItems.Add(dataRow[7].ToString().TrimEnd());
                //第7列 计费重量
                lvi.SubItems.Add(dataRow[8].ToString().TrimEnd());
                //第8列 运价号
                lvi.SubItems.Add(dataRow[9].ToString().TrimEnd());
                //第9列 运价率
                lvi.SubItems.Add(dataRow[10].ToString().TrimEnd());
                //计算运费
                if (lvi.SubItems[6].Text.Length != 0)
                {//这个方法传入两个值，一个是表中已经有的运价号，通过运价号找运价率，一个是正在添加的项目，用来计算运费
                    lvi.SubItems.Add(this.refreshFreight(int.Parse(lvi.SubItems[6].Text), lvi).TrimEnd());
                }
                else
                {
                    lvi.SubItems.Add(dataRow[11].ToString().TrimEnd());
                }

                this.goodsListView.Items.Add(lvi);
                ImageList imgList = new ImageList();
                imgList.ImageSize = new Size(1, 20);// 设置行高 20 //分别是宽和高 
                goodsListView.SmallImageList = imgList; //这里设置listView的SmallImageList ,用imgList将其撑大 

                //更新副表
                this.pickingListView.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度 
                ListViewItem lvi2 = new ListViewItem();
                //第1列 货物名称
                lvi2.SubItems[0].Text = dataRow[2].ToString().TrimEnd();
                //第2列 货物数量
                lvi2.SubItems.Add(dataRow[100].ToString().TrimEnd());
                //第3列 铁路确定重量
                lvi2.SubItems.Add(dataRow[7].ToString().TrimEnd());

                this.pickingListView.Items.Add(lvi2);
                ImageList imgList1 = new ImageList();
                imgList1.ImageSize = new Size(1, 20);// 设置行高 20 //分别是宽和高 
                pickingListView.SmallImageList = imgList1; //这里设置listView的SmallImageList ,用imgList将其撑大 
                this.pickingListView.EndUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度 
            }
            //添加合计货物信息（空）
            ListViewItem lvi1 = new ListViewItem();
            lvi1.SubItems[0].Text = "合计";
            lvi1.SubItems.Add("");
            lvi1.SubItems.Add("");
            lvi1.SubItems.Add("");
            lvi1.SubItems.Add("");
            lvi1.SubItems.Add("");
            lvi1.SubItems.Add("");
            lvi1.SubItems.Add("");
            lvi1.SubItems.Add("");
            this.goodsListView.Items.Add(lvi1);
            //刷新总计货物列表
            this.addEditGoodsForTotalGoods();
            this.goodsListView.EndUpdate();  //结束数据处理，UI界面一次性绘制。

            //初始化运单号
            trackingNumberLabel.Text = _trackingNumber.ToString();
            this.goodsListView.EndUpdate();  //结束数据处理，UI界面一次性绘制。

            sqlCnt.Close();

        }

        //对总计货物的添加信息，并且完成总运费的计算填写
        private void addEditGoodsForTotalGoods()
        {
            //实例化一个货物模型，用来存储合计货物中的各项信息。
            Goods totalGoods = new Goods();
            for (int i = 0; i < goodsListView.Items.Count - 1; i++)
            {//为总计货物添加数据
             //判空，并且判断总计货物是不是已经添加了之前的（重量，总计）信息
                if (goodsListView.Items[i].SubItems[1].Text.Length != 0 &&
                    goodsListView.Items[i].SubItems[2].Text.Length != 0 &&
                    goodsListView.Items[i].SubItems[3].Text.Length != 0 &&
                    goodsListView.Items[i].SubItems[4].Text.Length != 0)
                {//刷新前几行
                    totalGoods.goodsCount += int.Parse(goodsListView.Items[i].SubItems[1].Text.TrimEnd());
                    totalGoods.goodsWeightByShipper += int.Parse(goodsListView.Items[i].SubItems[2].Text.TrimEnd());
                    totalGoods.goodsWeightByRail += int.Parse(goodsListView.Items[i].SubItems[3].Text.TrimEnd());
                    totalGoods.chargedWeight += int.Parse(goodsListView.Items[i].SubItems[4].Text.TrimEnd());

                }
                if (goodsListView.Items[i].SubItems[8].Text.Length != 0)
                {//刷新总运费
                    totalGoods.goodsFreight += float.Parse(goodsListView.Items[i].SubItems[8].Text.TrimEnd());
                }



            }

            //添加合计货物信息
            //第二列 货物数量
            goodsListView.Items[goodsListView.Items.Count - 1].SubItems[1].Text = totalGoods.goodsCount.ToString();
            //第三列 寄送人确定重量
            goodsListView.Items[goodsListView.Items.Count - 1].SubItems[2].Text = totalGoods.goodsWeightByShipper.ToString();
            //第四列 铁路确定重量
            goodsListView.Items[goodsListView.Items.Count - 1].SubItems[3].Text = totalGoods.goodsWeightByRail.ToString();
            //第五列 计费重量
            goodsListView.Items[goodsListView.Items.Count - 1].SubItems[4].Text = totalGoods.chargedWeight.ToString();

            totalGoods = null;
        }

        private string refreshFreight(int tariffNumber, ListViewItem item)
        {//在编辑模式下获取运费
         //判断运价率
            switch (tariffNumber)
            {
                default: return "";
                case 0://机械冷藏车
                    return ((12.5 + 0.085 * float.Parse(transportMileageText.Text)) * float.Parse(item.SubItems[4].Text)).ToString();
                case 1:
                    return ((5.4 + 0.0276 * float.Parse(transportMileageText.Text)) * float.Parse(item.SubItems[4].Text)).ToString();
                case 2:
                    return ((6.1 + 0.0310 * float.Parse(transportMileageText.Text)) * float.Parse(item.SubItems[4].Text)).ToString();
                case 3:
                    return ((7.0 + 0.0352 * float.Parse(transportMileageText.Text)) * float.Parse(item.SubItems[4].Text)).ToString();
                case 4:
                    return ((8.6 + 0.0395 * float.Parse(transportMileageText.Text)) * float.Parse(item.SubItems[4].Text)).ToString();
                case 5:
                    return ((9.2 + 0.0459 * float.Parse(transportMileageText.Text)) * float.Parse(item.SubItems[4].Text)).ToString();
                case 6:
                    return ((13.1 + 0.0655 * float.Parse(transportMileageText.Text)) * float.Parse(item.SubItems[4].Text)).ToString();
                case 7:
                    return ((0.2795 * float.Parse(transportMileageText.Text)) * float.Parse(item.SubItems[4].Text)).ToString();
            }
        }





        /*
        ========================
        ========================
       ======进货检验+核算制票=======
        =========服务列表==========
        ========================
        */

        private void ServiceCheckListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {//双击表时候产生的动作
            if (ServiceCheckListView.SelectedItems.Count != 0)
            {
                //新建数据库
                string str = @"Data Source=.;Initial Catalog=User;Integrated Security=True";
                SqlConnection sqlCnt = new SqlConnection(str);
                sqlCnt.Open();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = sqlCnt;
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "select userName from GoodsAcceptance where trackingNumber = '" + ServiceCheckListView.SelectedItems[0].SubItems[0].Text + "'";
                DataSet ds1 = new DataSet();
                SqlDataAdapter da1 = new SqlDataAdapter(sqlCmd.CommandText, sqlCnt);
                da1.Fill(ds1);
                //本来是完成按钮，现在是打印按钮
                submitButton.Tag = 1;
                submitButton.Text = "打印";
                foreach (DataRow dr1 in ds1.Tables[0].Rows)
                {
                    this.initGoodsAcceptanceList(ServiceCheckListView.SelectedItems[0].SubItems[0].Text, dr1[0].ToString());

                }
                //针对该条点击的信息重新制造货物受理界面
                //创建出来的货物受理页面不能够编辑
                goodsNameText.ReadOnly = true;
                goodsCountText.ReadOnly = true;
                goodsPackageText.ReadOnly = true;
                goodsPriceText.ReadOnly = true;
                goodsWeightText.ReadOnly = true;
                destinationText.ReadOnly = true;
                shipperNameText.ReadOnly = true;
                shipperAddressText.ReadOnly = true;
                shipperPhoneNumText.ReadOnly = true;
                receiverNameText.ReadOnly = true;
                receiverAddressText.ReadOnly = true;
                receiverPhoneNumText.ReadOnly = true;

                if (userType == 0)
                {
                    homePageControl.SelectedTab = homePageControl.TabPages[0];
                }
                else
                {
                    homePageControl.SelectedTab = homePageControl.TabPages[1];
                }
                sqlCnt.Close();
            }

        }

        //表的更新（通过之前货物受理填写的信息更新）
        public void initServiceCheckListView(int searching = 0)
        {//新增searching参数，当参数为1时，进入查询模式
            this.ServiceCheckListView.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度 
            //先清空之前所有表内容
            for (int i = 0; i < ServiceCheckListView.Items.Count;)
            {
                ServiceCheckListView.Items.RemoveAt(0);
            }

            //要获取所有之前添加过的运单号，需要一个表
            List<string> allTrackingNumbers = new List<string>();
            //启用数据库
            string str = @"Data Source=.;Initial Catalog=User;Integrated Security=True";
            SqlConnection sqlCnt = new SqlConnection(str);
            sqlCnt.Open();
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlCnt;
            sqlCmd.CommandType = CommandType.Text;
            DataSet dataSet = new DataSet();
            if (userType == 0)
            {//是用户
                sqlCmd.CommandText = "select trackingNumber from GoodsAcceptance where userName = '" + _userName + "'";
                DataSet ds1 = new DataSet();
                SqlDataAdapter da1 = new SqlDataAdapter(sqlCmd.CommandText, sqlCnt);
                da1.Fill(ds1);
                if (ds1.Tables[0].Rows.Count == 0)
                {//如果之前没有添加任何信息，就直接退出方法
                    sqlCnt.Close();
                    this.initAnalysisListView();
                    return;
                }
                foreach (DataRow dr1 in ds1.Tables[0].Rows)
                {
                    allTrackingNumbers.Add(dr1[0].ToString());
                    sqlCmd.CommandText = "select * from GoodsAcceptance where trackingNumber = '" + allTrackingNumbers.Last() + "' and userName = '" + _userName + "'";
                    SqlDataAdapter da = new SqlDataAdapter(sqlCmd.CommandText, sqlCnt);
                    da.Fill(dataSet);
                }
            }
            else
            {
                sqlCmd.CommandText = "select trackingNumber from GoodsAcceptance";
                DataSet ds1 = new DataSet();
                SqlDataAdapter da1 = new SqlDataAdapter(sqlCmd.CommandText, sqlCnt);
                da1.Fill(ds1);
                if (ds1.Tables[0].Rows.Count == 0)
                {//如果之前没有添加任何信息，就直接退出方法
                    sqlCnt.Close();
                    this.initAnalysisListView();
                    return;
                }
                foreach (DataRow dr1 in ds1.Tables[0].Rows)
                {
                    allTrackingNumbers.Add(dr1[0].ToString());
                    sqlCmd.CommandText = "select * from GoodsAcceptance where trackingNumber = '" + allTrackingNumbers.Last() + "' ";
                    SqlDataAdapter da = new SqlDataAdapter(sqlCmd.CommandText, sqlCnt);
                    da.Fill(dataSet);
                }
            }

            if (searching == 1)
            {//查询模式
                ListViewGroup notEdit;
                ListViewGroup accepted;
                ListViewGroup completed;
                ListViewGroup searched;
                ListViewGroup searchedByGoodsName;
                if (ServiceCheckListView.Groups.Count == 0)
                {//如果没有分组再创建分组 否则不创建分组
                    //创建listViewItem，并且把它们分组
                    notEdit = new ListViewGroup("已完成货物受理");
                    accepted = new ListViewGroup("已完成进货检验");
                    completed = new ListViewGroup("已完成核算制票");
                    searched = new ListViewGroup("运单号查询结果");
                    searchedByGoodsName = new ListViewGroup("货物品名中包含该关键词的运单结果");
                    ServiceCheckListView.Groups.Add(notEdit);
                    ServiceCheckListView.Groups.Add(accepted);
                    ServiceCheckListView.Groups.Add(completed);
                    ServiceCheckListView.Groups.Add(searched);
                    ServiceCheckListView.Groups.Add(searchedByGoodsName);
                }
                else
                {
                    notEdit = ServiceCheckListView.Groups[0];
                    accepted = ServiceCheckListView.Groups[1];
                    completed = ServiceCheckListView.Groups[2];
                    searched = ServiceCheckListView.Groups[3];
                    searchedByGoodsName = ServiceCheckListView.Groups[4];
                }
                if (userType == 0)
                {
                    sqlCmd.CommandText = "select * from GoodsAcceptance where userName = '" + _userName + "' and (trackingNumber LIKE'%" + searchText.Text + "%' )";
                }
                else
                {
                    sqlCmd.CommandText = "select * from GoodsAcceptance where trackingNumber LIKE'%" + searchText.Text + "%' ";
                }

                DataSet dsSearch = new DataSet();
                SqlDataAdapter daSearch = new SqlDataAdapter(sqlCmd.CommandText, sqlCnt);
                daSearch.Fill(dsSearch);
                //先从运单号中找 如果没有再在货物里面找
                if (dsSearch.Tables[0].Rows.Count != 0)
                {

                    foreach (DataRow row in dsSearch.Tables[0].Rows)
                    {
                        if (userType == 0)
                        {
                            ListViewItem item = new ListViewItem(new string[]
                                 {row[11].ToString(),
                                                        row[14].ToString().TrimEnd(),
                                                        row[4].ToString().TrimEnd(),
                                                        row[7].ToString().TrimEnd(),
                                 }, 0, searched);
                            this.ServiceCheckListView.Items.Add(item);
                        }
                        else
                        {
                            ListViewItem item = new ListViewItem(new string[]
                                 {row[11].ToString(),
                                                        row[14].ToString().TrimEnd(),
                                                        row[4].ToString().TrimEnd(),
                                                        row[7].ToString().TrimEnd(),
                                                        row[0].ToString().TrimEnd()
                                 }, 0, searched);
                            this.ServiceCheckListView.Items.Add(item);
                        }


                    }
                }
                else
                {
                    sqlCmd.CommandText = "select trackingNumber from Goods where userName = '" + _userName + "' and goodsName = '" + searchText.Text + "' ";
                    DataSet dsSearchFromGoods = new DataSet();
                    SqlDataAdapter daSearchFromGoods = new SqlDataAdapter(sqlCmd.CommandText, sqlCnt);
                    daSearchFromGoods.Fill(dsSearchFromGoods);
                    //先从Goods表中找出符合名字条件的条目的运单号
                    foreach (DataRow row in dsSearchFromGoods.Tables[0].Rows)
                    {//在从GoodsAcceptance表中找相应的运单信息
                        sqlCmd.CommandText = "select * from GoodsAcceptance where userName = '" + _userName + "' and trackingNumber ='" + row[0].ToString() + "' ";
                        DataSet dsSearchFromGoods2 = new DataSet();
                        SqlDataAdapter daSearchFromGoods2 = new SqlDataAdapter(sqlCmd.CommandText, sqlCnt);
                        daSearchFromGoods2.Fill(dsSearchFromGoods2);

                        foreach (DataRow row1 in dsSearchFromGoods2.Tables[0].Rows)
                        {
                            if (userType == 0)
                            {
                                ListViewItem item = new ListViewItem(new string[]
   {row1[11].ToString(),
                                            row1[14].ToString().TrimEnd(),
                                            row1[4].ToString().TrimEnd(),
                                           row1[7].ToString().TrimEnd(),
      }, 0, searchedByGoodsName);
                                this.ServiceCheckListView.Items.Add(item);
                            }
                            else
                            {
                                ListViewItem item = new ListViewItem(new string[]
   {row1[11].ToString(),
                                            row1[14].ToString().TrimEnd(),
                                            row1[4].ToString().TrimEnd(),
                                           row1[7].ToString().TrimEnd(),
                                           row[0].ToString().TrimEnd()
      }, 0, searchedByGoodsName);
                                this.ServiceCheckListView.Items.Add(item);
                            }

                        }

                    }
                }

            }
            else
            {//列表模式
                foreach (DataRow dataRow in dataSet.Tables[0].Rows)   //从之前货物受理的表中添加数据 （没有则数据为空）
                {
                    //把货物分为三列，完成货物受理，完成进货检验，完成核算制票(另有两组为查询时使用。1为根据运单号查询，2为根据货物品名查询)
                    ListViewGroup notEdit;
                    ListViewGroup accepted;
                    ListViewGroup completed;
                    ListViewGroup searched;
                    ListViewGroup searchedByGoodsName;
                    if (ServiceCheckListView.Groups.Count == 0)
                    {//如果没有分组再创建分组 否则不创建分组
                     //创建listViewItem，并且把它们分组
                        notEdit = new ListViewGroup("已完成货物受理");
                        accepted = new ListViewGroup("已完成进货检验");
                        completed = new ListViewGroup("已完成核算制票");
                        searched = new ListViewGroup("运单号查询结果");
                        searchedByGoodsName = new ListViewGroup("货物品名中包含该关键词的运单结果");
                        ServiceCheckListView.Groups.Add(notEdit);
                        ServiceCheckListView.Groups.Add(accepted);
                        ServiceCheckListView.Groups.Add(completed);
                        ServiceCheckListView.Groups.Add(searched);
                        ServiceCheckListView.Groups.Add(searchedByGoodsName);
                    }
                    else
                    {
                        notEdit = ServiceCheckListView.Groups[0];
                        accepted = ServiceCheckListView.Groups[1];
                        completed = ServiceCheckListView.Groups[2];
                        searched = ServiceCheckListView.Groups[3];
                    }

                    //第一列（是0的话就是完成了货物受理，1的话就是完成了进货检验，2的话就是完成了核算制票,如果searching为1，则说明当前处于查询模式
                    if (int.Parse(dataRow[19].ToString()) == 0)
                    {                //第1列（运单号）
                                     //第2列（经由）
                                     //第3列（寄送人）
                                     //第4列（收货人）
                        if (userType == 0)
                        {
                            ListViewItem item = new ListViewItem(new string[]
                    {dataRow[11].ToString(),
                     dataRow[14].ToString().TrimEnd(),
                     dataRow[4].ToString().TrimEnd(),
                     dataRow[7].ToString().TrimEnd(),

                    }, 0, notEdit);
                            this.ServiceCheckListView.Items.Add(item);
                        }
                        else
                        {
                            ListViewItem item = new ListViewItem(new string[]
                    {dataRow[11].ToString(),
                     dataRow[14].ToString().TrimEnd(),
                     dataRow[4].ToString().TrimEnd(),
                     dataRow[7].ToString().TrimEnd(),
                     dataRow[0].ToString().TrimEnd()
                    }, 0, notEdit);
                            this.ServiceCheckListView.Items.Add(item);
                        }


                    }
                    else if (int.Parse(dataRow[19].ToString()) == 1)
                    {
                        if (userType == 0)
                        {
                            ListViewItem item = new ListViewItem(new string[]
                    {dataRow[11].ToString(),
                     dataRow[14].ToString().TrimEnd(),
                     dataRow[4].ToString().TrimEnd(),
                     dataRow[7].ToString().TrimEnd(),

                    }, 0, accepted);
                            this.ServiceCheckListView.Items.Add(item);
                        }
                        else
                        {
                            ListViewItem item = new ListViewItem(new string[]
                    {dataRow[11].ToString(),
                     dataRow[14].ToString().TrimEnd(),
                     dataRow[4].ToString().TrimEnd(),
                     dataRow[7].ToString().TrimEnd(),
                     dataRow[0].ToString().TrimEnd()
                    }, 0, accepted);
                            this.ServiceCheckListView.Items.Add(item);
                        }

                    }
                    else if (int.Parse(dataRow[19].ToString()) == 2)
                    {
                        if (userType == 0)
                        {
                            ListViewItem item = new ListViewItem(new string[]
                    {dataRow[11].ToString(),
                     dataRow[14].ToString().TrimEnd(),
                     dataRow[4].ToString().TrimEnd(),
                     dataRow[7].ToString().TrimEnd(),

                    }, 0, completed);
                            this.ServiceCheckListView.Items.Add(item);
                        }
                        else
                        {
                            ListViewItem item = new ListViewItem(new string[]
                    {dataRow[11].ToString(),
                     dataRow[14].ToString().TrimEnd(),
                     dataRow[4].ToString().TrimEnd(),
                     dataRow[7].ToString().TrimEnd(),
                     dataRow[0].ToString().TrimEnd()
                    }, 0, completed);
                            this.ServiceCheckListView.Items.Add(item);
                        }

                    }
                    ImageList imgList = new ImageList();
                    imgList.ImageSize = new Size(1, 20);// 设置行高 20 //分别是宽和高 
                    ServiceCheckListView.SmallImageList = imgList; //这里设置listView的SmallImageList ,用imgList将其撑大 
                }
            }


            this.ServiceCheckListView.EndUpdate();  //结束数据处理，UI界面一次性绘制。
            //关闭数据库
            sqlCnt.Close();
            //刷新统计分析表
            this.initAnalysisListView();
        }

        private void incomingCheckListView_SelectedIndexChanged(object sender, EventArgs e)
        {//表中选中的行，如果没有选中，则编辑和删除按钮不能使用
            if (ServiceCheckListView.SelectedItems.Count == 0)
            {
                editButton_CaculationPrice.Enabled = false;
                editButton_incomingCheck.Enabled = false;
                deleteButton_incomingCheck.Enabled = false;

            }
            else
            {
                deleteButton_incomingCheck.Enabled = true;
                editButton_incomingCheck.Enabled = true;
                if (ServiceCheckListView.SelectedItems[0].SubItems[1].Text.Length != 0)
                {//没有进行进货检验的货物，不能被核算制票
                    editButton_CaculationPrice.Enabled = true;
                }
            }
        }

        //服务列表编辑按钮的作用
        private void editButton_incomingCheck_Click(object sender, EventArgs e)
        {//为了找到编辑的那一行，需要利用表格的参数

            CargoDetails form = new CargoDetails(ServiceCheckListView.SelectedItems[0].SubItems[0].Text.TrimEnd(), ServiceCheckListView.SelectedItems[0].SubItems[4].Text.TrimEnd());
            form.Owner = this;
            form.ShowDialog();

        }

        //服务列表中删除按钮的作用
        private void deleteButton_incomingCheck_Click(object sender, EventArgs e)
        {
            //询问用户是否删除
            DialogResult dr;
            dr = MessageBox.Show("是否确认删除？", "提醒", MessageBoxButtons.YesNo,
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
                //先删Goods表的，才能删GoodsAcceptance表的
                sqlCmd.CommandText = "delete from Goods where userName = '" + ServiceCheckListView.SelectedItems[0].SubItems[4].Text.TrimEnd() + "' and trackingNumber = '" + ServiceCheckListView.SelectedItems[0].SubItems[0].Text + "'";
                sqlCmd.ExecuteNonQuery();
                sqlCmd.CommandText = "delete from GoodsAcceptance where userName = '" + ServiceCheckListView.SelectedItems[0].SubItems[4].Text.TrimEnd() + "' and trackingNumber = '" + ServiceCheckListView.SelectedItems[0].SubItems[0].Text + "'";
                sqlCmd.ExecuteNonQuery();


                //找到选出的行并且删掉它
                ServiceCheckListView.Items.RemoveAt(ServiceCheckListView.SelectedItems[0].Index);
                sqlCnt.Close();

                //刷新列表
                this.initServiceCheckListView();
            }

        }

        //核算制票表编辑按钮的作用
        private void editButton_CaculationPrice_Click(object sender, EventArgs e)
        {//为了找到编辑的那一行，需要利用表格的参数

            AddTicketPriceCount form = new AddTicketPriceCount(ServiceCheckListView.SelectedItems[0].SubItems[0].Text.TrimEnd(), ServiceCheckListView.SelectedItems[0].SubItems[4].Text.TrimEnd());
            form.Owner = this;
            form.ShowDialog();

        }


        private void searchButton_Click(object sender, EventArgs e)
        {//服务信息的查询
            if (searchText.Text.Length != 0)
            {
                stopSearchButton.Enabled = true;
                this.initServiceCheckListView(1);
            }
        }

        private void stopSearchButton_Click(object sender, EventArgs e)
        {//服务信息中回到所有列表
            this.initServiceCheckListView();

        }




        /*
========================
========================
======进货检验+核算制票=======
=========统计分析==========
========================
*/

        private void initAnalysisListView()
        {//统计分析表初始化数据
         //先清空之前所有表内容
            for (int l = 0; l < analysisListView.Items.Count;)
            {
                analysisListView.Items.RemoveAt(0);
            }
            //启用数据库
            string[] through = new string[100];//一个经由字符串，由始发站和终到站拼接成的字符串数组
            float[] goodsWeightCount = new float[200];//当前运单下所有货物的总计重量
            string[] tempTrackingNumber = new string[100];//临时的运单号
            string str = @"Data Source=.;Initial Catalog=User;Integrated Security=True";
            SqlConnection sqlCnt = new SqlConnection(str);
            sqlCnt.Open();
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlCnt;
            sqlCmd.CommandType = CommandType.Text;
            //先把所有运单找出来
            if(userType == 0)
            {
                sqlCmd.CommandText = "select trackingNumber,startingStation,destinationList from GoodsAcceptance where userName = '" + userName + "' ";
            }
            else
            {
                sqlCmd.CommandText = "select trackingNumber,startingStation,destinationList from GoodsAcceptance";
            }

            DataSet dataSet = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(sqlCmd.CommandText, sqlCnt);
            da.Fill(dataSet);
            int i = 0;//用来计数
            this.analysisListView.BeginUpdate();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {//把每一个运单里下属所有货物的重量合并起来
                if(userType == 0)
                {
                    sqlCmd.CommandText = "select goodsWeightByRail from Goods where userName = '" + userName + "' and trackingNumber = '" + row[0].ToString() + "' ";
                }
                else
                {
                    sqlCmd.CommandText = "select goodsWeightByRail from Goods where trackingNumber = '" + row[0].ToString() + "' ";
                }

                DataSet dsForGoodsWeight = new DataSet();
                SqlDataAdapter daForGoodsWeight = new SqlDataAdapter(sqlCmd.CommandText, sqlCnt);
                daForGoodsWeight.Fill(dsForGoodsWeight);
                foreach (DataRow row1 in dsForGoodsWeight.Tables[0].Rows)
                {//寻找每一个货物的铁路确定重量，并把它们加起来除以1000，以得到发送吨
                    if (row1[0].ToString().Length != 0)
                    {
                        if (!row1[0].ToString().Equals(0))
                        {//数值为0不添加
                            goodsWeightCount[i] += float.Parse(row1[0].ToString()) / 1000;
                            through[i] = row[1].ToString().TrimEnd() + "-" + row[2].ToString().TrimEnd();
                            i++;
                        }
                    }

                }
            }

            //随时将数据先填写到表格(如果一行都没有就不添加)
            if (through[0] != null)
            {
                for (int q = 0; q < i; q++)
                {
                    ListViewItem item = new ListViewItem();
                    item.SubItems[0].Text = through[q].TrimEnd();
                    item.SubItems.Add(goodsWeightCount[q].ToString().TrimEnd());
                    this.analysisListView.Items.Add(item);
                    ImageList imgList = new ImageList();
                    imgList.ImageSize = new Size(1, 20);// 设置行高 20 //分别是宽和高 
                    analysisListView.SmallImageList = imgList; //这里设置listView的SmallImageList ,用imgList将其撑大 
                }
            }
            this.analysisListView.EndUpdate();

            //开始查重
            for (int j = 0; j < analysisListView.Items.Count; j++)
            {
                for (int k = 0; k < analysisListView.Items.Count;)
                {
                    if (k != j)
                    {//被判断行和传入行相等的时候跳过方法
                        if (analysisListView.Items[k].SubItems[0].Text.Equals(analysisListView.Items[j].SubItems[0].Text))
                        {//如果选中行和第i行的值相等，就相加他们的发送数
                            analysisListView.Items[j].SubItems[1].Text = (float.Parse(analysisListView.Items[j].SubItems[1].Text) + float.Parse(analysisListView.Items[k].SubItems[1].Text)).ToString();
                            //删除后面的那一行
                            analysisListView.Items.RemoveAt(k);
                            //为了避免被删掉的一行下面一行不被查到，要限制i不能自增
                            goto A;
                        }
                        k++;
                    }
                    k++;
                A:;
                }
            }

            sqlCnt.Close();
        }






        /*
        ========================
        ========================
         =========计划受理=========
        ========================
        ========================
        */
        public void initAcceptancePlanList(int planType, int sortBy = 0)
        {//要初始化计划受理的表，可能需要刷新某个表，或所有的表。（原题的编辑+统计分析（标签为1），月计划编制表+品类重量+发车数量（标签为2））
         //启用数据库
            string str = @"Data Source=.;Initial Catalog=User;Integrated Security=True";
            SqlConnection sqlCnt = new SqlConnection(str);
            sqlCnt.Open();
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlCnt;
            sqlCmd.CommandType = CommandType.Text;
            if (planType == 1)
            {//更新原题的编辑表
                for (int i = 0; i < originalEditListView.Items.Count;)
                {//先清空原来的
                    originalEditListView.Items.RemoveAt(0);
                }
            }
            else if (planType == 2)
            {//更新月计划受理表
                for (int i = 0; i < MonthPlanPreparationListView.Items.Count;)
                {//先清空原来的
                    MonthPlanPreparationListView.Items.RemoveAt(0);
                }
            }
            if (sortBy == 0)
            {//加入sortBy主要是为了在统计分析里面针对某项数据进行全排序，因为排序利用SQL语句中的order by
                sqlCmd.CommandText = "select * from AcceptancePlan where userName = '" + userName + "' and planType = '" + planType + "'";
            }
            else if (sortBy == 1)
            {//0为不排序，1为对车种车号进行排序，2为对货物品类进行排序，3为对发站进行排序，4为对到站进行排序
                sqlCmd.CommandText = "select * from AcceptancePlan where userName = '" + userName + "' and planType = '1' order by vehicleTypeAndNumber";
            }
            else if (sortBy == 2)
            {
                sqlCmd.CommandText = "select * from AcceptancePlan where userName = '" + userName + "' and planType = '1' order by goodsCategory";
            }
            else if (sortBy == 3)
            {
                sqlCmd.CommandText = "select * from AcceptancePlan where userName = '" + userName + "' and planType = '1' order by startingStation";
            }
            else if (sortBy == 4)
            {
                sqlCmd.CommandText = "select * from AcceptancePlan where userName = '" + userName + "' and planType = '1' order by destinationStation";
            }


            sqlCmd.ExecuteNonQuery();
            DataSet ds1 = new DataSet();
            SqlDataAdapter da1 = new SqlDataAdapter(sqlCmd.CommandText, sqlCnt);
            da1.Fill(ds1);
            if (ds1.Tables[0].Rows.Count == 0)
            {//如果之前没有添加任何信息，就直接退出方法
                sqlCnt.Close();
                return;
            }
            foreach (DataRow dataRow in ds1.Tables[0].Rows)
            {//获取数据

                if (int.Parse(dataRow[2].ToString()) == 1)
                {//如果是原题的编辑，那么就更新原题的编辑数据
                    this.originalEditListView.BeginUpdate();
                    ListViewItem item = new ListViewItem();
                    item.SubItems[0].Text = dataRow[1].ToString().TrimEnd();
                    item.SubItems.Add(dataRow[3].ToString().TrimEnd());
                    item.SubItems.Add(dataRow[4].ToString().TrimEnd());
                    item.SubItems.Add(dataRow[5].ToString().TrimEnd());
                    item.SubItems.Add(dataRow[21].ToString().TrimEnd());
                    item.SubItems.Add(dataRow[6].ToString().TrimEnd());
                    item.SubItems.Add(dataRow[7].ToString().TrimEnd());
                    item.SubItems.Add(dataRow[8].ToString().TrimEnd());
                    item.SubItems.Add(dataRow[9].ToString().TrimEnd());
                    item.SubItems.Add(dataRow[10].ToString().TrimEnd());
                    item.SubItems.Add(dataRow[11].ToString().TrimEnd());
                    item.SubItems.Add(dataRow[12].ToString().TrimEnd());
                    item.SubItems.Add(dataRow[13].ToString().TrimEnd());
                    item.SubItems.Add(dataRow[14].ToString().TrimEnd());
                    item.SubItems.Add(dataRow[15].ToString().TrimEnd());
                    item.SubItems.Add(dataRow[16].ToString().TrimEnd());
                    this.originalEditListView.Items.Add(item);
                    ImageList imgList = new ImageList();
                    imgList.ImageSize = new Size(1, 20);// 设置行高 20 //分别是宽和高 
                    originalEditListView.SmallImageList = imgList; //这里设置listView的SmallImageList ,用imgList将其撑大 
                    this.originalEditListView.EndUpdate();
                    //刷新货物品类和重量表
                    this.refreshAcceptancePlan_GoodsTypeAndWeightList();
                }
                else if (int.Parse(dataRow[2].ToString()) == 2)
                {//如果是月计划编制表，那么就更新月计划编制表的数据
                    this.MonthPlanPreparationListView.BeginUpdate();
                    ListViewItem item = new ListViewItem();
                    item.SubItems[0].Text = dataRow[1].ToString().TrimEnd();
                    item.SubItems.Add(dataRow[5].ToString().TrimEnd());
                    item.SubItems.Add(dataRow[17].ToString().TrimEnd());
                    item.SubItems.Add(dataRow[18].ToString().TrimEnd());
                    item.SubItems.Add(dataRow[19].ToString().TrimEnd());
                    item.SubItems.Add(dataRow[20].ToString().TrimEnd());
                    this.MonthPlanPreparationListView.Items.Add(item);
                    ImageList imgList = new ImageList();
                    imgList.ImageSize = new Size(1, 20);// 设置行高 20 //分别是宽和高 
                    MonthPlanPreparationListView.SmallImageList = imgList; //这里设置listView的SmallImageList ,用imgList将其撑大 
                    this.MonthPlanPreparationListView.EndUpdate();
                    //在此处更新每日计划发车数
                    this.refreshVehiclesCountLabel();
                }
                sqlCnt.Close();

            }

        }

        private void addOriginalListButton_Click(object sender, EventArgs e)
        {//添加原题编辑的按钮
            int planID = this.getPlanID();
            OriginalTitleEditor form = new OriginalTitleEditor(planID, 0, userName);
            form.Owner = this;
            form.ShowDialog();
        }

        private void addMonthPlanButton_Click_1(object sender, EventArgs e)
        {//添加月计划编制的按钮
            int planID = this.getPlanID();
            MonthPlanPreparation form = new MonthPlanPreparation(planID, 0, userName);
            form.Owner = this;
            form.ShowDialog();
        }

        private void editOriginalListButton_Click(object sender, EventArgs e)
        {//[编辑]原
            int planID = int.Parse(originalEditListView.SelectedItems[0].Text);
            OriginalTitleEditor form = new OriginalTitleEditor(planID, 1, userName);
            form.Owner = this;
            form.ShowDialog();
        }

        private void editMonthPlanButton_Click(object sender, EventArgs e)
        {//[编辑]月计划
            int planID = int.Parse(MonthPlanPreparationListView.SelectedItems[0].Text);
            MonthPlanPreparation form = new MonthPlanPreparation(planID, 1, userName);
            form.Owner = this;
            form.ShowDialog();
        }



        private void deleteMonthPlanButton_Click(object sender, EventArgs e)
        {//删除月计划编制列表的信息
         //询问用户是否删除
            DialogResult dr;
            dr = MessageBox.Show("是否确认删除？", "提醒", MessageBoxButtons.YesNo,
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
                //删除的SQL语句
                sqlCmd.CommandText = "delete from AcceptancePlan where userName = '" + userName + "' and planID = '" + MonthPlanPreparationListView.SelectedItems[0].SubItems[0].Text + "'";
                sqlCmd.ExecuteNonQuery();


                //找到选出的行并且删掉它
                MonthPlanPreparationListView.Items.RemoveAt(MonthPlanPreparationListView.SelectedItems[0].Index);
                sqlCnt.Close();

                //刷新列表
                this.initAcceptancePlanList(2);
            }
        }

        private void deleteOriginalListButton_Click(object sender, EventArgs e)
        {//删除原题编辑列表的信息
         //询问用户是否删除
            DialogResult dr;
            dr = MessageBox.Show("是否确认删除？", "提醒", MessageBoxButtons.YesNo,
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
                //删除的SQL语句
                sqlCmd.CommandText = "delete from AcceptancePlan where userName = '" + userName + "' and planID = '" + originalEditListView.SelectedItems[0].SubItems[0].Text + "'";
                sqlCmd.ExecuteNonQuery();


                //找到选出的行并且删掉它
                originalEditListView.Items.RemoveAt(originalEditListView.SelectedItems[0].Index);
                sqlCnt.Close();
                //刷新货物品类重量表
                this.refreshAcceptancePlan_GoodsTypeAndWeightList();

                //刷新列表
                this.initAcceptancePlanList(1);
            }

        }

        private void refreshVehiclesCountLabel()
        {//刷新上方的大label，显示当日发车数量
            int vehiclesCount = 0;
            for (int i = 0; i < MonthPlanPreparationListView.Items.Count; i++)
            {
                vehiclesCount += int.Parse(MonthPlanPreparationListView.Items[i].SubItems[2].Text);
            }
            vehiclesCountLabel.Text = vehiclesCount.ToString();
            if (MonthPlanPreparationListView.Items.Count == 0)
            {
                vehiclesCountLabel.Text = "无";
            }

        }

        private void refreshAcceptancePlan_GoodsTypeAndWeightList()
        {//刷新货物品类重量表
            for (int i = 0; i < goodsTypeAndWeightListView.Items.Count;)
            {//先清空原来的
                goodsTypeAndWeightListView.Items.RemoveAt(0);
            }
            for (int i = 0; i < originalEditListView.Items.Count; i++)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = originalEditListView.Items[i].SubItems[3].Text;
                lvi.SubItems.Add(originalEditListView.Items[i].SubItems[4].Text);
                this.goodsTypeAndWeightListView.Items.Add(lvi);
            }

        }

        private int getPlanID()
        {
            //随机产生四位数的计划标志
            Random rad = new Random();//实例化随机数产生器rad；
            return rad.Next(1000, 10000);//用rad生成大于等于1000，小于等于9999的随机数；
        }

        private void MonthPlanPreparationListView_SelectedIndexChanged(object sender, EventArgs e)
        {//月计划编制表的值改变
            if (MonthPlanPreparationListView.SelectedItems.Count == 0)
            {
                deleteMonthPlanButton.Enabled = false;
                editMonthPlanButton.Enabled = false;
            }
            else
            {
                deleteMonthPlanButton.Enabled = true;
                editMonthPlanButton.Enabled = true;
            }
        }

        private void originalEditListView_SelectedIndexChanged(object sender, EventArgs e)
        {//原题编辑的值改变
            if (originalEditListView.SelectedItems.Count == 0)
            {
                deleteOriginalListButton.Enabled = false;
                editOriginalListButton.Enabled = false;
            }
            else
            {
                deleteOriginalListButton.Enabled = true;
                editOriginalListButton.Enabled = true;
            }
        }



        //在统计分析中进行排序的四个按钮
        private void vehicleTypeAndNumberButton_Click(object sender, EventArgs e)
        {//根据车种车号排序
            //直接用镶嵌在初始化表方法中的SQL排序方法进行排序
            this.initAcceptancePlanList(1, 1);

        }

        private void goodsCategoryButton_Click(object sender, EventArgs e)
        {//根据货物品类排序
         //直接用镶嵌在初始化表方法中的SQL排序方法进行排序
            this.initAcceptancePlanList(1, 2);
        }

        private void startStationButton_Click(object sender, EventArgs e)
        {//根据发站排序
         //直接用镶嵌在初始化表方法中的SQL排序方法进行排序
            this.initAcceptancePlanList(1, 3);
        }

        private void destinationStationButton_Click(object sender, EventArgs e)
        {//根据到站排序
         //直接用镶嵌在初始化表方法中的SQL排序方法进行排序
            this.initAcceptancePlanList(1, 4);
        }

        private void destinationStationList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


    }
}
