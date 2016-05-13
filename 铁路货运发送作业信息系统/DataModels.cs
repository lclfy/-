using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 铁路货运发送作业信息系统
{
    public class User
    {
        public string userName;
        public string password;
        public List<GoodsAcceptance> goodsAcceptance = new List<GoodsAcceptance>();
        public List<AcceptancePlan> acceptancePlan = new List<AcceptancePlan>();

    }
    public class GoodsAcceptance
    {
        //货物受理
        public string startingStation;//始发站
        public string destinationStation;//终到站
        public string destinationText;//始发终到自治区
        public string GoodsAcceptance_shipperName;//托运人
        public string GoodsAcceptance_shipperAddress;//托运人住址
        public string GoodsAcceptance_shipperPhoneNum;//托运人电话
        public string GoodsAcceptance_receiverName;//收货人姓名
        public string GoodsAcceptance_receiverAddress;//收货人地址
        public string GoodsAcceptance_receiverPhoneNum;//收货人电话
        public List<Goods> goodsList = new List<Goods>();
        public string goodsAcceptanceTip;//货物受理的备注
        public int trackingNumber;//运单号
        //进货检验
        public string vehicleNumber;//车种编号
        public string truckScale;//货车标重
        public string railwayTarpNumber;//铁路篷布号码
        public string through;//经由
        public string containerNumber;//集装箱号码
        public string sealNumber;//施封号码
        public string transportMileage;//运输里程
        public int acceptState;//受理状态
        public string cargoDetailsTip;//进货检验的备注
        //核算制票
        public string freightMileage;//运价里程（核算制票）
        public Goods totalGoods;//全部货物的计算
        public float freight;//运费
        public float loadingCharges;//装卸费
        public float serviceCharges;//取送车费
        public float totalCharges;//合计费用
    }

    public class Goods
    {
        //一个货物里面包含的信息
        public string goodsName;//货物名称
        public int goodsCount;//件数
        public string goodsPackage;//货物包装
        public int goodsPrice;//货物价格
        public int goodsWeightByShipper;//发货人确定的货物重量
        public int goodsWeightByRail;//铁路确定的货物重量
        public int chargedWeight;//计费重量
        public float tariffNumber;//运价号
        public float trafficPriceRate;//运价率
        public float goodsFreight;//单个货物的运费
    }

    public struct AcceptancePlan
    {
        public int planID;//用于唯一确定某一单的单号
        //原单编辑
        public string sentType;//发出类型
        public string vehicleTypeAndNumber;//车种车号
        public string vehicleType;//车型
        public string AcceptancePlan_shipperName;//托运人
        public string AcceptancePlan_shipperAddress;//托运人住址
        public string AcceptancePlan_shipperPhoneNum;//托运人电话
        public string AcceptancePlan_receiverName;//收货人
        public string AcceptancePlan_receiverAddress;//收货人住址
        public string AcceptancePlan_receiverPhoneNum;//收货人电话
        public string agent;//经办人
        public string acceptanceTime;//受理时间
        public int vehiclesCount;//车辆总计
        public int totalPayLoad;//总计载重
        public string startingStation;
        public string destinationStation;

        //月计划
        public string goodsCategory;//货物品类
        public string dailyPlan;//计划日均
        public int plannedVehiclesCount;//计划车数
        public int plannedTransmission;//计划发送量
        public int plannedEachVehiclePayload;//计划一车净载重

    }

     public struct AnalysisData
    {//用于统计分析里面
        string through;//一个经由字符串，由始发站和终到站拼接成的字符串数组
        float goodsWeightCount;//当前运单下所有货物的总计重量
        string tempTrackingNumber;//临时的运单号
    }
}