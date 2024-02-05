using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace OA_WEB_API.Models.BPMPro
{
    /// <summary>
    /// 繳款單(查詢條件)
    /// </summary>
    public class PaymentOrderQueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }
    }

    /// <summary>
    /// 繳款單
    /// </summary>
    public class PaymentOrderViewModel
    {
        /// <summary>申請人資訊</summary>
        public ApplicantInfo APPLICANT_INFO { get; set; }

        /// <summary>繳款單 表頭資訊</summary>
        public PaymentOrderTitle PAYMENT_ORDER_TITLE { get; set; }

        /// <summary>繳款單 表單內容 設定</summary>
        public PaymentOrderConfig PAYMENT_ORDER_CONFIG { get; set; }

        /// <summary>表單關聯</summary>
        public IList<AssociatedFormConfig> ASSOCIATED_FORM_CONFIG { get; set; }
    }

    /// <summary>
    /// 繳款單 表頭資訊
    /// </summary>
    public class PaymentOrderTitle : ImplementHeader
    {
        /// <summary>ERP 工作流程標題名稱</summary>
        public string FLOW_NAME { get; set; }
    }

    /// <summary>
    /// 繳款單 表單內容 設定
    /// </summary>
    public class PaymentOrderConfig : DOM_TWD_Bank
    {
        /// <summary>是否過財務協理</summary>
        public string IS_CFO { get; set; }

        /// <summary>金額</summary>
        public double AMOUNT { get; set; }

        /// <summary>幣別</summary>
        public string CURRENCY { get; set; }

        /// <summary>備註</summary>
        public string NOTE { get; set; }

        /// <summary>財務審核人員編號</summary>
        public string FINANC_AUDIT_ID_1 { get; set; }

        /// <summary>財務審核人員姓名</summary>
        public string FINANC_AUDIT_NAME_1 { get; set; }

        /// <summary>財務覆核人員編號</summary>
        public string FINANC_AUDIT_ID_2 { get; set; }

        /// <summary>財務覆核人員姓名</summary>
        public string FINANC_AUDIT_NAME_2 { get; set; }

        /// <summary>
        /// 支付方式：
        /// CS.現金
        /// PT_AC.薪轉帳戶
        /// OR.其他[X]
        /// DT.其他帳戶(需自行負擔手續費)
        /// DD.支票
        /// </summary>
        public string PAY_METHOD { get; set; }

        /// <summary>
        /// 帳戶類別：
        /// A.廠商
        /// B.個人
        /// PayMethod：
        /// CS.(現金)、
        /// PT_AC.(薪轉帳戶)
        /// 帳戶類別 固定都會是B。
        /// </summary>
        public string ACCOUNT_CATEGORY { get; set; }

        /// <summary>支付對象</summary>
        public string PAYMENT_OBJECT { get; set; }
    }

}