﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// 會簽管理系統 - 四方四隅_會簽單
/// </summary>
namespace OA_WEB_API.Models.BPMPro
{
    /// <summary>
    /// 四方四隅_會簽單(查詢條件)
    /// </summary>
    public class GPI_CountersignQueryModel
    {
        /// <summary>系統編號</summary>
        public string REQUISITION_ID { get; set; }
    }

    /// <summary>
    /// 四方四隅_會簽單
    /// </summary>
    public class GPI_CountersignViewModel
    {
        /// <summary>申請人資訊</summary>
        public ApplicantInfo APPLICANT_INFO { get; set; }

        /// <summary>四方四隅_會簽單 表頭資訊</summary>
        public GPI_CountersignTitle GPI_COUNTERSIGN_TITLE { get; set; }

        /// <summary>四方四隅_會簽單 表單內容 設定</summary>
        public GPI_CountersignConfig GPI_COUNTERSIGN_CONFIG { get; set; }

        /// <summary>四方四隅_會簽單 會簽簽核人員 設定</summary>
        public List<GPI_CountersignApproversConfig> GPI_COUNTERSIGN_APPROVERS_CONFIG { get; set; }

        /// <summary>表單關聯</summary>
        public IList<AssociatedFormConfig> ASSOCIATED_FORM_CONFIG { get; set; }

    }

    /// <summary>
    /// 四方四隅_會簽單 表頭資訊
    /// </summary>
    public class GPI_CountersignTitle : HeaderTitle
    {
        /// <summary>
        /// 級別：
        /// 調整 ApplicantInfo(申請人資訊)的，表單PRIORITY(重要性)
        /// 特急件:3
        /// 急件:2
        /// 普通件:1
        /// </summary>
        public string LEVEL_TYPE { get; set; }
    }

    /// <summary>
    /// 四方四隅_會簽單 表單內容 設定
    /// </summary>
    public class GPI_CountersignConfig
    {
        /// <summary>說明</summary>
        public string DESCRIPTION { get; set; }

        /// <summary>備註</summary>
        public string NOTE { get; set; }

        /// <summary>是否過副總</summary>
        public string IS_VICE_PRESIDENT { get; set; }
    }

    /// <summary>
    /// 四方四隅_會簽單 會簽簽核人員 設定
    /// </summary>
    public class GPI_CountersignApproversConfig : ApproversConfig
    {

    }
}