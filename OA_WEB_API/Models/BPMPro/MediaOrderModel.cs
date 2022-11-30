﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OA_WEB_API.Models.BPMPro
{
    /// <summary>
    /// 版權採購申請單(查詢條件)
    /// </summary>
    public class MediaOrderQueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }
    }

    /// <summary>
    /// 版權採購申請單(查詢)
    /// </summary>
    public class MediaOrderViewModel
    {
        /// <summary>申請人資訊</summary>
        public ApplicantInfo APPLICANT_INFO { get; set; }

        /// <summary>版權採購申請單 表頭資訊</summary>
        public MediaOrderTitle MEDIA_ORDER_TITLE { get; set; }

        /// <summary>版權採購申請單 表單內容 設定</summary>
        public MediaOrderConfig MEDIA_ORDER_CONFIG { get; set; }

        /// <summary>版權採購申請單 採購明細 設定</summary>
        public IList<MediaOrderDetailsConfig> MEDIA_ORDER_DTLS_CONFIG { get; set; }

        /// <summary>版權採購申請單 授權權利 設定</summary>
        public IList<MediaOrderAuthorizesConfig> MEDIA_ORDER_AUTHS_CONFIG { get; set; }

        /// <summary>版權採購申請單 額外項目 設定</summary>
        public IList<MediaOrderExtrasConfig> MEDIA_ORDER_EXS_CONFIG { get; set; }

        /// <summary>版權採購申請單 付款辦法 設定</summary>
        public IList<MediaOrderPaymentsConfig> MEDIA_ORDER_PYMTS_CONFIG { get; set; }

        /// <summary>版權採購申請單 使用預算 設定</summary>
        public IList<MediaOrderBudgetsConfig> MEDIA_ORDER_BUDGS_CONFIG { get; set; }

        /// <summary>版權採購申請單 驗收項目 設定</summary>
        public IList<MediaOrderAcceptancesConfig> MEDIA_ORDER_ACPTS_CONFIG { get; set; }

        /// <summary>表單關聯</summary>
        public IList<AssociatedFormConfig> ASSOCIATED_FORM_CONFIG { get; set; }
    }

    /// <summary>
    /// 版權採購申請單 表頭資訊
    /// </summary>
    public class MediaOrderTitle : ImplementHeader
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
    /// 版權採購申請單 表單內容 設定
    /// </summary>
    public class MediaOrderConfig
    {
        /// <summary>說明</summary>
        public string DESCRIPTION { get; set; }

        /// <summary>是否過副總</summary>
        public string IS_VICE_PRESIDENT { get; set; }

        /// <summary>幣別</summary>
        public string CURRENCY { get; set; }

        /// <summary>預計匯率</summary>
        public double PRE_RATE { get; set; }

        /// <summary>計價方式</summary>
        public string PRICING_METHOD { get; set; }

        /// <summary>營業稅/[稅率]租稅協定</summary>
        public double TAX { get; set; }

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

        /// <summary>材料費總價(採購明細)</summary>
        public double DTL_MATERIAL_TOTAL { get; set; }

        /// <summary>材料費總價_台幣(採購明細)</summary>
        public int DTL_MATERIAL_TOTAL_TWD { get; set; }

        /// <summary>合計(採購明細)</summary>
        public double DTL_ORDER_TOTAL { get; set; }

        /// <summary>合計_台幣(採購明細)</summary>
        public int DTL_ORDER_TOTAL_TWD { get; set; }

        /// <summary>額外項目總額(額外項目)</summary>
        public double EX_AMOUNT_TOTAL { get; set; }

        /// <summary>額外項目總額_台幣(額外項目)</summary>
        public int EX_AMOUNT_TOTAL_TWD { get; set; }

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
    /// 版權採購申請單 採購明細 設定
    /// </summary>
    public class MediaOrderDetailsConfig
    {
        /// <summary>行數編號</summary>
        public int DTL_ROW_NO { get; set; }

        /// <summary>商品代碼</summary>
        public string DTL_SUP_PROD_A_NO { get; set; }

        /// <summary>商品名稱</summary>
        public string DTL_ITEM_NAME { get; set; }

        /// <summary>影片類型</summary>
        public string DTL_MEDIA_TYPE { get; set; }

        /// <summary>開始集數</summary>
        public int DTL_START_EPISODE { get; set; }

        /// <summary>結束集數</summary>
        public int DTL_END_EPISODE { get; set; }

        /// <summary>總集數</summary>
        public int DTL_EPISODE_TOTAL { get; set; }

        /// <summary>每集長度</summary>
        public int DTL_EPISODE_TIME { get; set; }

        /// <summary>未稅單價/NET單價</summary>
        public double DTL_NET { get; set; }

        /// <summary>未稅單價_台幣/NET單價_台幣</summary>
        public int DTL_NET_TWD { get; set; }

        /// <summary>含稅單價/GROSS單價</summary>
        public double DTL_GROSS { get; set; }

        /// <summary>含稅單價_台幣/GROSS單價_台幣</summary>
        public int DTL_GROSS_TWD { get; set; }

        /// <summary>未稅小計/NET小計</summary>
        public double DTL_NET_SUM { get; set; }

        /// <summary>未稅小計_台幣/NET小計_台幣</summary>
        public int DTL_NET_SUM_TWD { get; set; }

        /// <summary>含稅小計/GROSS小計</summary>
        public double DTL_GROSS_SUM { get; set; }

        /// <summary>含稅小計_台幣/GROSS小計_台幣</summary>
        public int DTL_GROSS_SUM_TWD { get; set; }

        /// <summary>單集材料費</summary>
        public double DTL_MATERIAL { get; set; }

        /// <summary>明細單項小記</summary>
        public double DTL_ITEM_SUM { get; set; }

        /// <summary>所屬專案 ERP 單號</summary>
        public string DTL_PROJECT_FORM_NO { get; set; }

        /// <summary>所屬專案名稱</summary>
        public string DTL_PROJECT_NAME { get; set; }

        /// <summary>所屬專案描述</summary>
        public string DTL_PROJECT_NICKNAME { get; set; }

        /// <summary>所屬專案起案年度</summary>
        public string DTL_PROJECT_USE_YEAR { get; set; }

        /// <summary>備註</summary>
        public string DTL_NOTE { get; set; }
    }

    /// <summary>
    /// 版權採購申請單 授權權利 設定
    /// </summary>
    public class MediaOrderAuthorizesConfig
    {
        /// <summary>行數編號</summary>
        public int AUTH_ROW_NO { get; set; }

        /// <summary>商品代碼</summary>
        public string AUTH_SUP_PROD_A_NO { get; set; }

        /// <summary>商品名稱</summary>
        public string AUTH_ITEM_NAME { get; set; }

        /// <summary>洲別</summary>
        public string AUTH_CONTINENT { get; set; }

        /// <summary>國家</summary>
        public string AUTH_COUNTRY { get; set; }

        /// <summary>播放平台</summary>
        public string AUTH_PLAY_PLATFORM { get; set; }

        /// <summary>播放[標記]</summary>
        public string AUTH_PLAY { get; set; }

        /// <summary>販售[標記]</summary>
        public string AUTH_SELL { get; set; }

        /// <summary>剪後播[標記]</summary>
        public string AUTH_EDIT_TO_PLAY { get; set; }

        /// <summary>剪後售[標記]</summary>
        public string AUTH_EDIT_TO_SELL { get; set; }

        /// <summary>授權時間類型</summary>
        public string AUTH_ALLOTED_TIME_TYPE { get; set; }

        /// <summary>授權開始時間(授權時間)</summary>
        public DateTime AUTH_START_DATE { get; set; }

        /// <summary>授權結束時間(授權時間)</summary>
        public DateTime AUTH_END_DATE { get; set; }

        /// <summary>授權方式類型</summary>
        public string AUTH_FREQUENCY_TYPE { get; set; }

        /// <summary>授權播放次數(授權方式)</summary>
        public int AUTH_PLAY_FREQUENCY { get; set; }

        /// <summary>備註</summary>
        public string AUTH_NOTE { get; set; }

    }

    /// <summary>
    /// 版權採購申請單 額外項目 設定
    /// </summary>
    public class MediaOrderExtrasConfig
    {
        /// <summary>名稱</summary>
        public string EX_NAME { get; set; }

        /// <summary>金額</summary>
        public int EX_AMOUNT { get; set; }

        /// <summary>所屬專案 ERP 單號</summary>
        public string EX_PROJECT_FORM_NO { get; set; }

        /// <summary>所屬專案名稱 </summary>
        public string EX_PROJECT_NAME { get; set; }

        /// <summary>所屬專案描述</summary>
        public string EX_PROJECT_NICKNAME { get; set; }

        /// <summary>所屬專案年分</summary>
        public string EX_PROJECT_USE_YEAR { get; set; }

        /// <summary>備註</summary>
        public string EX_NOTE { get; set; }
    }

    /// <summary>
    /// 版權採購申請單 付款辦法 設定
    /// </summary>
    public class MediaOrderPaymentsConfig
    {
        /// <summary>期別</summary>
        public int PERIOD { get; set; }

        /// <summary>付款項目</summary>
        public string PYMT_PROJECT { get; set; }

        /// <summary>付款條件</summary>
        public string PYMT_TERMS { get; set; }

        /// <summary>付款方式編號</summary>
        public string PYMT_METHOD_ID { get; set; }

        /// <summary>稅額</summary>
        public double PYMT_TAX { get; set; }

        /// <summary>未稅金額/NET單價</summary>
        public double PYMT_NET { get; set; }

        /// <summary>含稅總額/GROSS單價</summary>
        public double PYMT_GROSS { get; set; }

        /// <summary>當期預計匯率</summary>
        public double PYMT_PRE_RATE { get; set; }

        /// <summary>含稅總額(換算)/GROSS價(換算)</summary>
        public int PYMT_GROSS_CONV { get; set; }

        /// <summary>使用預算金額</summary>
        public int PYMT_USE_BUDGET { get; set; }
    }

    /// <summary>
    /// 版權採購申請單 使用預算 設定
    /// </summary>
    public class MediaOrderBudgetsConfig
    {
        /// <summary>期別</summary>
        public int PERIOD { get; set; }

        /// <summary>預算 ERP唯一碼</summary>        
        public string BUDG_FORM_NO { get; set; }

        /// <summary>預算編列年度</summary>
        public string BUDG_CREATE_YEAR { get; set; }

        /// <summary>預算名稱</summary>
        public string BUDG_NAME { get; set; }

        /// <summary>所屬部門</summary>
        public string BUDG_OWNER_DEPT { get; set; }

        /// <summary>預算總額</summary>
        public int BUDG_TOTAL { get; set; }

        /// <summary>可用預算金額</summary>
        public int BUDG_AVAILABLE_BUDGET_AMOUNT { get; set; }

        /// <summary>使用預算金額</summary>
        public int BUDG_USE_BUDGET_AMOUNT { get; set; }
    }

    /// <summary>
    /// 版權採購申請單 驗收項目 設定
    /// </summary>
    public class MediaOrderAcceptancesConfig
    {
        /// <summary>期別</summary>
        public int PERIOD { get; set; }

        /// <summary>商品代碼</summary>
        public string PA_SUP_PROD_A_NO { get; set; }

        /// <summary>商品名稱</summary>
        public string PA_ITEM_NAME { get; set; }

        /// <summary>影片類型</summary>
        public string PA_MEDIA_TYPE { get; set; }

        /// <summary>開始集數</summary>
        public int PA_START_EPISODE { get; set; }

        /// <summary>結束集數</summary>
        public int PA_END_EPISODE { get; set; }

        /// <summary>總集數</summary>
        public int PA_EPISODE_TOTAL { get; set; }


        /// <summary>商品備註</summary>
        public string PA_NOTE { get; set; }
    }

}