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
    public partial class AddTicketPriceCount : Form
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



        public AddTicketPriceCount(string tempTrackingNumber, string tempUserName)
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
            string[] goodsListTitles = new string[] {  "货物名称", "件数", "发货人确定重量", "铁路确定重量", "计费重量", "包装", "运价号", "运价率" ,"运费"};
            for (int i = 0; i < goodsListTitles.Count(); i++)
            {
                ColumnHeader ch = new ColumnHeader();
                ch.Text = goodsListTitles[i];   //设置列标题 
                if(i == 5)
                {
                    ch.Width = 140;
                }
                else
                {
                    ch.Width = 100;
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
                GoodsAcceptance__shipperName_Text .Text= dataRow[4].ToString().TrimEnd();//寄送人
                GoodsAcceptance__shipperAddress_Text .Text= dataRow[5].ToString().TrimEnd();//住址
                GoodsAcceptance__shipperPhoneNum_Text .Text= dataRow[6].ToString().TrimEnd();//电话
                GoodsAcceptance__receiverName_Text.Text = dataRow[7].ToString().TrimEnd();//收货人
                GoodsAcceptance__receiverAddress_Text.Text = dataRow[8].ToString().TrimEnd();//住址
                GoodsAcceptance__receiverPhoneNum_Text.Text = dataRow[9].ToString().TrimEnd();//电话
                startingStationText.Text = dataRow[1].ToString().TrimEnd();//始发站
                destinationStationText.Text = dataRow[2].ToString().TrimEnd();//终点站
                loadingChargesText.Text = dataRow[23].ToString().TrimEnd();//装费
                serviceChargesText.Text = dataRow[24].ToString().TrimEnd();//取送车费
                weighageText.Text = dataRow[25].ToString().TrimEnd();//装费

                //设置文本框的只读属性
                vehicleNumberText.ReadOnly = true;
                truckScaleText.ReadOnly = true;
                throughText.ReadOnly = true;
                transportMileageText.ReadOnly = true;
                railwayTarpNumberText.ReadOnly = true;
                containerNumberText.ReadOnly = true;
                sealNumberText.ReadOnly = true;
                GoodsAcceptance__shipperName_Text.ReadOnly = true;
                GoodsAcceptance__shipperAddress_Text.ReadOnly = true;
                GoodsAcceptance__shipperPhoneNum_Text.ReadOnly = true;
                GoodsAcceptance__receiverName_Text.ReadOnly = true;
                GoodsAcceptance__receiverAddress_Text.ReadOnly = true;
                GoodsAcceptance__receiverPhoneNum_Text.ReadOnly = true;
                startingStationText.ReadOnly = true;
                destinationStationText.ReadOnly = true;

            }

            this.goodsListView.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度 
            foreach (DataRow dataRow in dataSetForGoods.Tables[0].Rows)   //添加货物的数据 
            {
                ListViewItem lvi = new ListViewItem();
                //第一列 货物名称
                lvi.SubItems[0].Text = dataRow[2].ToString().TrimEnd();
                goodsNames.Add((string)dataRow[2]);
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

            //初始状态下不可点击修改按钮
            addEditGoodsDetailButton.Enabled = false;
            this.goodsListView.EndUpdate();  //结束数据处理，UI界面一次性绘制。

            sqlCnt.Close();
        }

        private void addEditGoodsDetailButton_Click(object sender, EventArgs e)
        {//添加运价号和运价率的表格
         //修改表格信息
            if (tariffNumberText.Text.Length != 0)
            {//对填写元素进行判空
                //对表格进行修改
                goodsListView.SelectedItems[0].SubItems[6].Text = tariffNumberText.Text;
                //判断运价率
                switch(int.Parse(tariffNumberText.Text))
                    {
                        default: break;
                    case 0://机械冷藏车
                        goodsListView.SelectedItems[0].SubItems[7].Text = "0.0850";
                        goodsListView.SelectedItems[0].SubItems[8].Text = ((12.5+0.085*float.Parse(transportMileageText.Text))* float.Parse(goodsListView.SelectedItems[0].SubItems[4].Text)).ToString();
                        break;
                    case 1:
                            goodsListView.SelectedItems[0].SubItems[7].Text = "0.0276";
                        goodsListView.SelectedItems[0].SubItems[8].Text = ((5.4 + 0.0276 * float.Parse(transportMileageText.Text)) * float.Parse(goodsListView.SelectedItems[0].SubItems[4].Text)).ToString();
                        break;
                    case 2:
                        goodsListView.SelectedItems[0].SubItems[7].Text = "0.0310";
                        goodsListView.SelectedItems[0].SubItems[8].Text = ((6.1 + 0.0310 * float.Parse(transportMileageText.Text)) * float.Parse(goodsListView.SelectedItems[0].SubItems[4].Text)).ToString();
                        break;
                    case 3:
                        goodsListView.SelectedItems[0].SubItems[7].Text = "0.0352";
                        goodsListView.SelectedItems[0].SubItems[8].Text = ((7.0 + 0.0352 * float.Parse(transportMileageText.Text)) * float.Parse(goodsListView.SelectedItems[0].SubItems[4].Text)).ToString();
                        break;
                    case 4:
                        goodsListView.SelectedItems[0].SubItems[7].Text = "0.0395";
                        goodsListView.SelectedItems[0].SubItems[8].Text = ((8.6 + 0.0395 * float.Parse(transportMileageText.Text)) * float.Parse(goodsListView.SelectedItems[0].SubItems[4].Text)).ToString();
                        break;
                    case 5:
                        goodsListView.SelectedItems[0].SubItems[7].Text = "0.0459";
                        goodsListView.SelectedItems[0].SubItems[8].Text = ((9.2 + 0.0459 * float.Parse(transportMileageText.Text)) * float.Parse(goodsListView.SelectedItems[0].SubItems[4].Text)).ToString();
                        break;
                    case 6:
                        goodsListView.SelectedItems[0].SubItems[7].Text = "0.0655";
                        goodsListView.SelectedItems[0].SubItems[8].Text = ((13.1 + 0.0655 * float.Parse(transportMileageText.Text)) * float.Parse(goodsListView.SelectedItems[0].SubItems[4].Text)).ToString();
                        break;
                    case 7:
                        goodsListView.SelectedItems[0].SubItems[7].Text = "0.2795";
                        goodsListView.SelectedItems[0].SubItems[8].Text = ((0.2795 * float.Parse(transportMileageText.Text)) * float.Parse(goodsListView.SelectedItems[0].SubItems[4].Text)).ToString();
                        break;


                }
                //清空输入框文字
                tariffNumberText.Text = "";
                //刷新货物总计一行
                this.addEditGoodsForTotalGoods();
            }
            else
            {
                MessageBox.Show("请输入所有信息后点击添加", "提示");
            }

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

        private void submitButton_Click(object sender, EventArgs e)
        {//完成按钮
         //点击完成按钮
         //启用数据库
            string str = @"Data Source=.;Initial Catalog=User;Integrated Security=True";
            SqlConnection sqlCnt = new SqlConnection(str);
            sqlCnt.Open();
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlCnt;
            sqlCmd.CommandType = CommandType.Text;
            //判断点击该按钮时的受理状态（可能已经添加过核算制票为2了）

            //对元素进行判空
            for (int i = 0; i < goodsListView.Items.Count; i++)
            {//判断运价号是不是填好了
                if (i != goodsListView.Items.Count - 1 && goodsListView.Items[i].SubItems[6].Text.Length == 0 && goodsListView.Items[i].SubItems[7].Text.Length == 0)
                {
                    MessageBox.Show("请检查所有货物的运价号是否填写完毕", "提示");
                    sqlCnt.Close();
                    return;
                }
            }
            //对界面元素进行判空
            if (freightText.Text.Length != 0 &&
               loadingChargesText.Text.Length != 0 &&
               serviceChargesText.Text.Length != 0 &&
               weighageText.Text.Length != 0 &&
               totalChargesText.Text.Length != 0)
            {
                //询问用户是否添加
                DialogResult dr;
                dr = MessageBox.Show("是否确认添加该信息？", "确定", MessageBoxButtons.YesNo,
                         MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                if (dr == DialogResult.Yes)
                {
                    //在数据库中添加所有已经更新的货物信息
                    for (int k = 0; k < goodsListView.Items.Count; k++)
                    {
                        //在数据库中添加goodsListView的所有信息(货物总计不能添加进去)
                        if(k != goodsListView.Items.Count - 1)
                        {
                            sqlCmd.CommandText = "update Goods set tariffNumber = '" + goodsListView.Items[k].SubItems[6].Text + "' , trafficPriceRate = '" + goodsListView.Items[k].SubItems[7].Text + "' where userName = '" + _userName + "' and trackingNumber = '" + _trackingNumber + "' and goodsName = '" + goodsNames[k] + "' ";
                            sqlCmd.ExecuteNonQuery();
                        }

                    }
                    //在数据库中添加输入框中的所有信息(受理状态改为2)
                    sqlCmd.CommandText = "update GoodsAcceptance set freight = '" + float.Parse(freightText.Text) + "' , loadingCharges = '" + float.Parse(loadingChargesText.Text) + "', serviceCharges = '" + float.Parse(serviceChargesText.Text) + "', weighage = '" + float.Parse(weighageText.Text) + "' , totalCharges = '" + float.Parse(totalChargesText.Text) + "' ,addTicketPriceCountTip = '"+tip3Text.Text+"', acceptState = '2' where userName = '" + _userName + "' and trackingNumber = '" + _trackingNumber + "' ";
                    sqlCmd.ExecuteNonQuery();

                    //将模型回传给主页面控制器并刷新它的列表
                    homeScreen _homeScreen = (homeScreen)this.Owner;
                    _homeScreen.initServiceCheckListView();

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
                    goodsListView.Items[i].SubItems[4].Text.Length != 0 )
                {//刷新前几行
                    totalGoods.goodsCount += int.Parse(goodsListView.Items[i].SubItems[1].Text.TrimEnd());
                    totalGoods.goodsWeightByShipper += int.Parse(goodsListView.Items[i].SubItems[2].Text.TrimEnd());
                    totalGoods.goodsWeightByRail += int.Parse(goodsListView.Items[i].SubItems[3].Text.TrimEnd());
                    totalGoods.chargedWeight += int.Parse(goodsListView.Items[i].SubItems[4].Text.TrimEnd());

                }
                if(goodsListView.Items[i].SubItems[8].Text.Length != 0)
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

        private void AddTicketPriceCount_Load(object sender, EventArgs e)
        {

        }

        private void goodsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(goodsListView.SelectedItems.Count != 0)
            {
                if(goodsListView.SelectedItems[0].Index != goodsListView.Items.Count - 1) {//限制如果选中的是全部货物，就不能进行编辑
                    addEditGoodsDetailButton.Enabled = true;
                }
            }
            else
            {
                addEditGoodsDetailButton.Enabled = false;
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
        private void goodsText_KeyPress(object sender, KeyPressEventArgs e)
        {//用于控制货物数量，价格，重量和各种电话号码为数字的方法
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)13 && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }

        private void printButton_Click(object sender, EventArgs e)
        {
            //对元素进行判空
            for (int i = 0; i < goodsListView.Items.Count; i++)
            {//判断运价号是不是填好了
                if (i != goodsListView.Items.Count - 1 && goodsListView.Items[i].SubItems[6].Text.Length == 0 && goodsListView.Items[i].SubItems[7].Text.Length == 0)
                {
                    MessageBox.Show("请检查所有货物的运价号是否填写完毕", "提示");
                    return;
                }
            }
            //对界面元素进行判空
            if (freightText.Text.Length != 0 &&
               loadingChargesText.Text.Length != 0 &&
               serviceChargesText.Text.Length != 0 &&
               weighageText.Text.Length != 0 &&
               totalChargesText.Text.Length != 0)
            {
                    PrintTicket form = new PrintTicket(trackingNumber, userName);
                    form.Owner = this;
                    form.Show();

            }
            else
            {
                MessageBox.Show("请添加所有信息", "提示", MessageBoxButtons.YesNo,
                         MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }

        }
    }
}
