using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// 會簽管理系統 - 離職、留職停薪_手續表
/// </summary>
namespace OA_WEB_API.Models.BPMPro
{
    /// <summary>
    /// 離職、留職停薪_手續表(查詢條件)
    /// </summary>
    public class ResignUnpaidLeaveAgendaQueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }
    }

    /// <summary>
    /// 離職、留職停薪_流程表
    /// </summary>
    public class ResignUnpaidLeaveAgendaViewModel
    {
        /// <summary>申請人資訊</summary>
        public ApplicantInfo APPLICANT_INFO { get; set; }

        /// <summary>離職、留職停薪_手續表 表頭資訊</summary>
        public ResignUnpaidLeaveAgendaTitle RESIGN_UNPAID_LEAVE_AGENDA_TITLE { get; set; }

        /// <summary>離職、留職停薪_手續表 表單內容 設定</summary>
        public ResignUnpaidLeaveAgendaConfig RESIGN_UNPAID_LEAVE_AGENDA_CONFIG { get; set; }

        /// <summary>離職、留職停薪_手續表 事務清單 設定</summary>
        public IList<ResignUnpaidLeaveAgendaAffairsConfig> RESIGN_UNPAID_LEAVE_AGENDA_AFFS_CONFIG { get; set; }
    }

    /// <summary>
    /// 離職、留職停薪_手續表 表頭資訊
    /// </summary>
    public class ResignUnpaidLeaveAgendaTitle : HeaderTitle
    {

    }

    /// <summary>
    /// 離職、留職停薪_手續表 表單內容 設定
    /// </summary>
    public class ResignUnpaidLeaveAgendaConfig
    {
        /// <summary>用途</summary>
        public string FORM_ACTION { get; set; }

        /// <summary>離職日期</summary>
        public Nullable<DateTime> RESIGN_DATE { get; set; }

        /// <summary>申請人交接主管部門</summary>
        public string HANDOVER_SUPERVISOR_DEPT_ID { get; set; }

        /// <summary>申請人交接主管員工編號</summary>
        public string HANDOVER_SUPERVISOR_ID { get; set; }

        /// <summary>申請人交接主管工姓名</summary>
        public string HANDOVER_SUPERVISOR_NAME { get; set; }

        /// <summary>勞健團保轉出日期</summary>
        public Nullable<DateTime> C01B_DATE { get; set; }

        /// <summary>特休假尚餘的天數與時數</summary>
        public string C01C_STR_DATE_TIME { get; set; }

        /// <summary>補休假尚餘的天數與時數</summary>
        public string C01F_STR_DATE_TIME { get; set; }

        /// <summary>是否預借特休的天數與時數</summary>
        public string C01H_STR_DATE_TIME { get; set; }

        /// <summary>薪資發放狀況其他</summary>
        public string C02_OTHERS { get; set; }

        /// <summary>後續資訊作業其他</summary>
        public string C03_OTHERS { get; set; }
    }

    /// <summary>
    /// 離職、留職停薪_手續表 事務清單 設定
    /// </summary>
    public class ResignUnpaidLeaveAgendaAffairsConfig
    {
        /// <summary>承辦事項編號</summary>
        public string ITEM_ID { get; set; }

        /// <summary>承辦事項</summary>
        public string ITEM_NAME { get; set; }

        /// <summary>
        /// 是否完成：
        /// 給前端Bool，
        /// 存進DB記得轉String
        /// </summary>
        public Nullable<Boolean> IS_CONSUMMATION { get; set; }

        /// <summary>交接物品/尚未結清說明</summary>
        public string DESCRIPTION { get; set; }

        /// <summary>承辦人部門</summary>
        public string CONTACTER_DEPT_ID { get; set; }

        /// <summary>承辦人員工編號</summary>
        public string CONTACTER_ID { get; set; }

        /// <summary>承辦人員工姓名</summary>
        public string CONTACTER_NAME { get; set; }

        /// <summary>畫押日期</summary>
        public Nullable<DateTime> SIGN_DATE { get; set; }
    }

    /// <summary>
    /// 離職、留職停薪_手續表 建立新的事務清單
    /// </summary>
    public class NewAffairsConfig
    {
        /// <summary>T-SQL</summary>
        public string STR_SQL { get; set; }

        /// <summary>承辦事項：字典</summary>
        public Dictionary<string, string> DICTIONARY { get; set; }

        /// <summary>SqlParameter</summary>
        public List<SqlParameter> PARAMETER { get; set; }
    }

}