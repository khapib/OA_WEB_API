using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// ERP系統 - 系統共通功能資訊
/// </summary>
namespace OA_WEB_API.Models.ERP
{
    #region 接收ERP回傳狀態

    /// <summary>
    /// 接收ERP回傳狀態
    /// </summary>
    public class ErpResponseState
    {

        public string data { get; set; }

        /// <summary>狀態</summary>
        public string code { get; set; }

        /// <summary>信息</summary>
        public string msg { get; set; }

    }

    #endregion
}