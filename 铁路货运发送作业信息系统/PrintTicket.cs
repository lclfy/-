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
    public partial class PrintTicket : Form
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

        public PrintTicket(string tempTrackingNumber, string tempUserName)
        {
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
            sqlCmd.CommandText = "select * from Goods where trackingNumber = '" + _trackingNumber + "' and userName = '" + _userName + "'";
            DataSet dataSetForGoods = new DataSet();
            SqlDataAdapter daG = new SqlDataAdapter(sqlCmd.CommandText, sqlCnt);
            daG.Fill(dataSetForGoods);
            //初始化核算制票表格
            goodsListView.View = View.Details;
            goodsListView.ShowGroups = false;
            string[] goodsListTitles = new string[] { "货物名称", "件数", "发货人确定重量", "铁路确定重量", "计费重量", "包装", "运价号", "运价率","运费" };
            for (int i = 0; i < goodsListTitles.Count(); i++)
            {
                ColumnHeader ch = new ColumnHeader();
                ch.Text = goodsListTitles[i];   //设置列标题 
                if (i == 3 || i == 2)
                {
                    ch.Width = 100;
                }
                else if (i == 1 || i ==5 || i== 6 || i ==7)
                {
                    ch.Width = 50;
                }
                else
                {
                    ch.Width = 70;
                }
                this.goodsListView.Columns.Add(ch);    //将列头添加到ListView控件。
            }
            foreach (DataRow dataRow in dataSetForGoodsAcceptance.Tables[0].Rows)   //添加运单的数据 
            {
                //为界面上的元素赋值
                vehicleNumberText.Text = dataRow[12].ToString().TrimEnd();//车种编号
                truckScaleText.Text = dataRow[13].ToString().TrimEnd();//货车标重
                throughText.Text = dataRow[15].ToString().TrimEnd();//经由
                transportMileageText.Text = dataRow[18].ToString().TrimEnd();//运价里程
                railwayTarpNumberText.Text = dataRow[14].ToString().TrimEnd();//铁路篷布号码
                containerNumberText.Text = dataRow[16].ToString().TrimEnd();//集装箱号码
                sealNumberText.Text = dataRow[17].ToString().TrimEnd();//施封号码
                shipperNameText.Text = dataRow[4].ToString().TrimEnd();//寄送人
                shipperAddressText.Text = dataRow[5].ToString().TrimEnd();//住址
                shipperPhoneNumText.Text = dataRow[6].ToString().TrimEnd();//电话
                receiverNameText.Text = dataRow[7].ToString().TrimEnd();//收货人
                receiverAddressText.Text = dataRow[8].ToString().TrimEnd();//住址
                receiverPhoneNumText.Text = dataRow[9].ToString().TrimEnd();//电话
                startingStationText.Text = dataRow[1].ToString().TrimEnd();//始发站
                destinationStationText.Text = dataRow[2].ToString().TrimEnd();//终点站
                loadingChargesText.Text = dataRow[23].ToString().TrimEnd();//装费
                serviceChargesText.Text = dataRow[24].ToString().TrimEnd();//取送车费
                weighageText.Text = dataRow[25].ToString().TrimEnd();//装费
            }
            this.goodsListView.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度 
            foreach (DataRow dataRow in dataSetForGoods.Tables[0].Rows)   //添加货物的数据 
            {
                ListViewItem lvi = new ListViewItem();
                //第一列 货物名称
                lvi.SubItems[0].Text = dataRow[2].ToString().TrimEnd();
                //第二列 货物数量
                lvi.SubItems.Add(dataRow[3].ToString().TrimEnd());
                //第三列 寄送人确定重量
                lvi.SubItems.Add(dataRow[6].ToString().TrimEnd());
                //第四列 铁路确定重量
                lvi.SubItems.Add(dataRow[7].ToString().TrimEnd());
                //第五列 计费重量
                lvi.SubItems.Add(dataRow[8].ToString().TrimEnd());
                //第六列 包装
                lvi.SubItems.Add(dataRow[4].ToString().TrimEnd());
                //第七列 运价号
                lvi.SubItems.Add(dataRow[9].ToString().TrimEnd());
                //第八列 运价率
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
            trackingNumberLabel.Text = trackingNumber.ToString();
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
            //总运费
            freightText.Text = totalGoods.goodsFreight.ToString();

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

        private void loadingChargesText_TextChanged(object sender, EventArgs e)
        {//总运费监听方法（输入数据，总运费加起来）
            float totalCharges = 0;
            if (freightText.Text.Length != 0)
            {
                totalCharges += float.Parse(freightText.Text);
            }
            if (loadingChargesText.Text.Length != 0)
            {
                totalCharges += float.Parse(loadingChargesText.Text);
            }
            if (serviceChargesText.Text.Length != 0)
            {
                totalCharges += float.Parse(serviceChargesText.Text);
            }
            if (weighageText.Text.Length != 0)
            {
                totalCharges += float.Parse(weighageText.Text);
            }
            totalChargesText.Text = totalCharges.ToString();


        }

        private void maskedTextBox9_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void maskedTextBox8_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }
    }
}
