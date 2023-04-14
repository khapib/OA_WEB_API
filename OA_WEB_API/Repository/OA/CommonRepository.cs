using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OA_WEB_API.Repository.OA
{
    /// <summary>
    /// OA系統 - 系統共通功能資訊
    /// </summary>
    public class CommonRepository
    {
        /// <summary>
        /// OA(系統)案件等級
        /// </summary>
        public static string OA_CaseLevel(string Level)
        {
            switch (Level)
            {
                case "1": return "A";
                case "2": return "A";
                case "3": return "B";
                default: return String.Empty;
            }
        }
    }
}