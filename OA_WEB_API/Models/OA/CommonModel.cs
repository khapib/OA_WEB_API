using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// OA系統 - 系統共通功能資訊
/// </summary>
namespace OA_WEB_API.Models.OA
{
    #region 接收OA回傳狀態

    /// <summary>
    /// 接收OA回傳狀態
    /// </summary>
    public class OAResponseState
    {
        /// <summary>OA系統編號</summary>
        public string OA_MASTER_NO { get; set; }

        /// <summary>OA表單編號</summary>
        public string OA_FORM_NO { get; set; }

        /// <summary>OA狀態</summary>
        public string OA_FORM_ACTION { get; set; }

        /// <summary>BPM表單單號</summary>
        public string BPM_FORM_NO { get; set; }

    }

    #endregion
}