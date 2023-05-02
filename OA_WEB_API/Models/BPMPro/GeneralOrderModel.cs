using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// 會簽管理系統 - 行政採購申請單
/// </summary>
namespace OA_WEB_API.Models.BPMPro
{
    /// <summary>
    /// 行政採購申請單(查詢條件)
    /// </summary>
    public class GeneralOrderQueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }
    }

    /// <summary>
    /// 行政採購申請單
    /// </summary>
    public class GeneralOrderViewModel
    {
        /// <summary>申請人資訊</summary>
        public ApplicantInfo APPLICANT_INFO { get; set; }

        /// <summary>行政採購申請 表頭資訊</summary>
        public GeneralOrderTitle GENERAL_ORDER_TITLE { get; set; }

        /// <summary>行政採購申請 表單內容 設定</summary>
        public GeneralOrderConfig GENERAL_ORDER_CONFIG { get; set; }

        /// <summary>行政採購申請 採購明細 設定</summary>
        public IList<GeneralOrderDetailsConfig> GENERAL_ORDER_DETAILS_CONFIG { get; set; }

        /// <summary>行政採購申請 付款辦法 設定</summary>
        public IList<GeneralOrderPaymentsConfig> GENERAL_ORDER_PAYMENTS_CONFIG { get; set; }

        /// <summary>行政採購申請 使用預算 設定</summary>
        public IList<GeneralOrderBudgetsConfig> GENERAL_ORDER_BUDGETS_CONFIG { get; set; }

        /// <summary>行政採購申請 驗收項目 設定</summary>
        public IList<GeneralOrderAcceptancesConfig> GENERAL_ORDER_ACPT_CONFIG { get; set; }

        /// <summary>表單關聯</summary>
        public IList<AssociatedFormConfig> ASSOCIATED_FORM_CONFIG { get; set; }
    }

    /// <summary>
    /// 行政採購申請 表頭資訊
    /// </summary>
    public class GeneralOrderTitle : ImplementHeader
    {
        /// <summary>編輯註記</summary>
        public string EDIT_FLAG { get; set; }

        /// <summary>異動原單系統編號</summary>
        public string GROUP_ID { get; set; }

        /// <summary>異動原單BPM 表單單號</summary>
        public string GROUP_BPM_FORM_NO { get; set; }

        /// <summary>異動原單BPM 表單路徑</summary>
        public string GROUP_PATH { get; set; }

        /// <summary>母表單系統編號</summary>
        public string PARENT_ID { get; set; }

        /// <summary>母表單BPM 表單單號</summary>
        public string PARENT_BPM_FORM_NO { get; set; } 
        
        /// <summary>表單操作</summary>
        public string FORM_ACTION { get; set; }    

        /// <summary>ERP 工作流程標題名稱</summary>
        public string FLOW_NAME { get; set; }
    }

    /// <summary>
    /// 行政採購申請 表單內容 設定
    /// </summary>
    public class GeneralOrderConfig
    {
        /// <summary>說明</summary>
        public string DESCRIPTION { get; set; }

        /// <summary>是否過副總</summary>
        public string IS_VICE_PRESIDENT { get; set; }

        /// <summary>是否入資產</summary>
        public string IS_ASSEST { get; set; }

        /// <summary>交易類型</summary>
        public string TXN_TYPE { get; set; }

        /// <summary>幣別</summary>
        public string CURRENCY { get; set; }

        /// <summary>預計匯率</summary>
        public double PRE_RATE { get; set; }

        /// <summary>計價方式</summary>
        public string PRICING_METHOD { get; set; }

        /// <summary>營業稅/[稅率]租稅協定</summary>
        public double TAX_RATE { get; set; }

        /// <summary>廠商編號</summary>
        public string SUP_NO { get; set; }

        /// <summary>廠商名稱</summary>
        public string SUP_NAME { get; set; }

        /// <summary>登記證號類別</summary>
        public string REG_KIND { get; set; }

        /// <summary>登記證號/統編</summary>
        public string REG_NO { get; set; }

        /// <summary>負責人</summary>
        public string OWNER_NAME { get; set; }

        /// <summary>負責人電話</summary>
        public string OWNER_TEL { get; set; }

        /// <summary>總付款期數</summary>
        public int PAYMENT_PERIOD_TOTAL { get; set; }

        /// <summary>合計未稅金額(採購明細)/NET總額(採購明細)</summary>
        public double DTL_NET_TOTAL { get; set; }

        /// <summary>合計未稅金額_台幣(採購明細)/NET總額_台幣(採購明細)</summary>
        public int DTL_NET_TOTAL_TWD { get; set; }

        /// <summary>合計含稅總額(採購明細)/GROSS總額(採購明細)</summary>
        public double DTL_GROSS_TOTAL { get; set; }

        /// <summary>合計含稅總額_台幣(採購明細)/GROSS總額_台幣(採購明細)</summary>
        public int DTL_GROSS_TOTAL_TWD { get; set; }

        /// <summary>折扣額度</summary>
        public int DISCOUNT_PRICE { get; set; }

        /// <summary>合計(採購明細)</summary>
        public double DTL_ORDER_TOTAL { get; set; }

        /// <summary>合計_台幣(採購明細)</summary>
        public int DTL_ORDER_TOTAL_TWD { get; set; }

        /// <summary>不可異動標住(付款辦法)</summary>
        public string PYMT_LOCK_PERIOD { get; set; }

        /// <summary>總稅額(付款辦法)</summary>
        public double PYMT_TAX_TOTAL { get; set; }

        /// <summary>合計未稅金額(付款辦法)/NET總額(付款辦法)</summary>
        public double PYMT_NET_TOTAL { get; set; }

        /// <summary>合計含稅總額(付款辦法)/GROSS總額(付款辦法)</summary>
        public double PYMT_GROSS_TOTAL { get; set; }

        /// <summary>合計含稅總額(換算)(付款辦法)/合計GROSS價(換算)(付款辦法)</summary>
        public int PYMT_GROSS_TOTAL_CONV { get; set; }

        /// <summary>合計使用預算(所需預算)(付款辦法)</summary>
        public int PYMT_USE_BUDGET_TOTAL { get; set; }
    }

    /// <summary>
    /// 行政採購申請 採購明細 設定
    /// </summary>
    public class GeneralOrderDetailsConfig
    {
        /// <summary>商品代碼</summary>
        public string SUP_PROD_A_NO { get; set; }

        /// <summary>商品名稱</summary>
        public string ITEM_NAME { get; set; }

        /// <summary>型號</summary>
        public string MODEL { get; set; }

        /// <summary>規格</summary>
        public string SPECIFICATIONS { get; set; }

        /// <summary>數量</summary>
        public int QUANTITY { get; set; }

        /// <summary>單位</summary>
        public string UNIT { get; set; }

        /// <summary>未稅單價/NET單價</summary>
        public double NET { get; set; }

        /// <summary>未稅單價_台幣/NET單價_台幣</summary>
        public int NET_TWD { get; set; }

        /// <summary>含稅單價/GROSS單價</summary>
        public double GROSS { get; set; }

        /// <summary>含稅單價_台幣/GROSS單價_台幣</summary>
        public int GROSS_TWD { get; set; }

        /// <summary>未稅小計/NET小計</summary>
        public double NET_SUM { get; set; }

        /// <summary>未稅小計_台幣/NET小計_台幣</summary>
        public int NET_SUM_TWD { get; set; }

        /// <summary>含稅小計/GROSS小計</summary>
        public double GROSS_SUM { get; set; }

        /// <summary>含稅小計_台幣/GROSS小計_台幣</summary>
        public int GROSS_SUM_TWD { get; set; }

        /// <summary>所屬專案 ERP 單號</summary>
        public string PROJECT_FORM_NO { get; set; }

        /// <summary>所屬專案名稱</summary>
        public string PROJECT_NAME { get; set; }

        /// <summary>所屬專案描述</summary>
        public string PROJECT_NICKNAME { get; set; }

        /// <summary>所屬專案起案年度</summary>
        public string PROJECT_USE_YEAR { get; set; }

        /// <summary>備註</summary>
        public string NOTE { get; set; }
    }

    /// <summary>
    /// 行政採購申請 付款辦法 設定
    /// </summary>
    public class GeneralOrderPaymentsConfig
    {
        /// <summary>期別</summary>
        public int PERIOD { get; set; }

        /// <summary>付款項目</summary>
        public string PROJECT { get; set; }

        /// <summary>付款條件</summary>
        public string TERMS { get; set; }

        /// <summary>
        /// 付款方式編號：
        /// DD.票據
        /// FF.電匯
        /// CS.現金
        /// </summary>
        public string METHOD_ID { get; set; }

        /// <summary>稅額</summary>
        public double TAX { get; set; }

        /// <summary>未稅金額/NET單價</summary>
        public double NET { get; set; }

        /// <summary>含稅總額/GROSS單價</summary>
        public double GROSS { get; set; }

        /// <summary>當期預計匯率</summary>
        public double PRE_RATE { get; set; }

        /// <summary>含稅總額(換算)/GROSS價(換算)</summary>
        public int GROSS_CONV { get; set; }

        /// <summary>使用預算金額</summary>
        public int USE_BUDGET { get; set; }
    }

    /// <summary>
    /// 行政採購申請 使用預算 設定
    /// </summary>
    public class GeneralOrderBudgetsConfig
    {        
        /// <summary>期別</summary>
        public int PERIOD { get; set; }

        /// <summary>預算 ERP唯一碼</summary>        
        public string FORM_NO { get; set; }

        /// <summary>預算編列年度</summary>
        public string CREATE_YEAR { get; set; }

        /// <summary>預算名稱</summary>
        public string NAME { get; set; }

        /// <summary>所屬部門</summary>
        public string OWNER_DEPT { get; set; }

        /// <summary>預算總額</summary>
        public int TOTAL { get; set; }

        /// <summary>可用預算金額</summary>
        public int AVAILABLE_BUDGET_AMOUNT { get; set; }

        /// <summary>使用預算金額</summary>
        public int USE_BUDGET_AMOUNT { get; set; }
    }

    /// <summary>
    /// 行政採購申請 驗收項目 設定
    /// </summary>
    public class GeneralOrderAcceptancesConfig
    {
        /// <summary>期別</summary>
        public int PERIOD { get; set; }
                
        /// <summary>商品代碼</summary>
        public string SUP_PROD_A_NO { get; set; }

        /// <summary>商品名稱</summary>
        public string ITEM_NAME { get; set; }

        /// <summary>型號</summary>
        public string MODEL { get; set; }

        /// <summary>規格</summary>
        public string SPECIFICATIONS { get; set; }

        /// <summary>數量</summary>
        public int QUANTITY { get; set; }

        /// <summary>單位</summary>
        public string UNIT { get; set; }

        /// <summary>商品備註</summary>
        public string NOTE { get; set;}
    }
}