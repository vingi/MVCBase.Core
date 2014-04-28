using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace MVCBase.Core.SqlInject
{
    public class SqlstrAny : IHttpModule
    {
        public void Init(HttpApplication application)
        {
            application.BeginRequest += (new
            EventHandler(this.Application_BeginRequest));
        }
        private void Application_BeginRequest(Object source, EventArgs e)
        {
            ProcessRequest pr = new ProcessRequest();
            pr.StartProcessRequest();
        }
        public void Dispose()
        {
        }
    }

    public class ProcessRequest
    {
        private static string SqlStr = System.Configuration.ConfigurationManager.AppSettings["SqlInject"].ToString();
        private static string sqlErrorPage = System.Configuration.ConfigurationManager.AppSettings["SQLInjectErrPage"].ToString();
        ///
        /// 用来识别是否是流的方式传输
        ///
        ///
        ///
        bool IsUploadRequest(HttpRequest request)
        {
            return StringStartsWithAnotherIgnoreCase(request.ContentType, "multipart/form-data");
        }
        ///
        /// 比较内容类型
        ///
        ///
        ///
        ///
        private static bool StringStartsWithAnotherIgnoreCase(string s1, string s2)
        {
            return (string.Compare(s1, 0, s2, 0, s2.Length, true, System.Globalization.CultureInfo.InvariantCulture) == 0);
        }

        //SQL注入式攻击代码分析
        #region SQL注入式攻击代码分析
        ///
        /// 处理用户提交的请求
        ///
        public void StartProcessRequest()
        {
            HttpRequest Request = System.Web.HttpContext.Current.Request;
            HttpResponse Response = System.Web.HttpContext.Current.Response;
            try
            {
                string getkeys = "";
                if (IsUploadRequest(Request)) return; //如果是流传递就退出
                //字符串参数
                if (Request.QueryString != null)
                {
                    for (int i = 0; i < Request.QueryString.Count; i++)
                    {
                        getkeys = Request.QueryString.Keys[i];
                        if (!ProcessSqlStr(Request.QueryString[getkeys]))
                        {
                            Response.Redirect(sqlErrorPage + "?errmsg=QueryString中含有非法字符串&amp;sqlprocess=true");
                            Response.End();
                        }
                    }
                }
                //form参数
                if (Request.Form != null)
                {
                    for (int i = 0; i < Request.Form.Count; i++)
                    {
                        getkeys = Request.Form.Keys[i];
                        if (!ProcessSqlStr(Request.Form[getkeys]))
                        {
                            Response.Redirect(sqlErrorPage + "?errmsg=Form中含有非法字符串&amp;sqlprocess=true");
                            Response.End();
                        }
                    }
                }
                //cookie参数
                if (Request.Cookies != null)
                {
                    for (int i = 0; i < Request.Cookies.Count; i++)
                    {
                        getkeys = Request.Cookies.Keys[i];
                        if (!ProcessSqlStr(Request.Cookies[getkeys].Value))
                        {
                            Response.Redirect(sqlErrorPage + "?errmsg=Cookie中含有非法字符串&amp;sqlprocess=true");
                            Response.End();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                // 错误处理: 处理用户提交信息!
                if (!(ex is HttpRequestValidationException))
                {
                    Response.Clear();
                    Response.Write("CustomErrorPage配置错误");
                    Response.End();
                }
            }
        }

        ///
        /// 分析用户请求是否正常
        ///
        /// 传入用户提交数据
        /// 返回是否含有SQL注入式攻击代码
        private bool ProcessSqlStr(string Str)
        {
            bool ReturnValue = true;
            try
            {
                if (Str != "")
                {
                    string[] anySqlStr = SqlStr.Split('|');
                    foreach (string ss in anySqlStr)
                    {
                        if (Str.IndexOf(ss) >= 0)
                        {
                            ReturnValue = false;
                            break;
                        }
                    }
                }
            }
            catch
            {
                ReturnValue = false;
            }
            return ReturnValue;
        }
        #endregion
    }
}
