using System;
using System.Collections.Generic;
using System.Web;

namespace MVCBase.Core
{
    public class MVCCommon
    {
        /// <summary>
        /// 前台绑定checkbox
        /// </summary>
        /// <param name="istrue"></param>
        /// <returns></returns>
        public static string BindCheckBox_Html(bool istrue)
        {
            string checkstr = string.Empty;
            if (istrue)
                checkstr = "checked = \"checked\"";
            return checkstr;
        }

        /// <summary>
        /// controller接收checkbox值并转换
        /// </summary>
        /// <returns></returns>
        public static bool BindCheckBox_Entity(object obj)
        {
            bool istrue = true;
            if (obj == null)
                istrue = false;
            return istrue;
        }
    }
}