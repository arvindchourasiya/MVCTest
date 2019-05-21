using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;
using Yahoo.Yui.Compressor;

namespace TestMVC
{
    public class CustomHttpHandler : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }

        public CustomHttpHandler(RequestContext requestContext)
        {
            this.RequestContext = requestContext;
        }

        public bool IsReusable
        {
            get { return true; }
        }
        public void ProcessRequest(HttpContext context)
        {
#if DEBUG
            Console.WriteLine("Mode=Debug");
#else
            try
            {
                context.Response.ContentType = "text/css";
                StringBuilder sb = new StringBuilder();
                    string iecss = context.Server.MapPath("~/Content/ie.css");
                    string chromecss = context.Server.MapPath("~/Content/chrome.css");
                    string firefoxcss = context.Server.MapPath("~/Content/firefox.css");
                    sb.Append(File.ReadAllText(iecss));
                    sb.Append(File.ReadAllText(chromecss));
                    sb.Append(File.ReadAllText(firefoxcss));
                    CssCompressor csscomp = new CssCompressor();
                    string compressedCss = csscomp.Compress(sb.ToString());
                    context.Response.Write(compressedCss);
                }
                catch (Exception ex)
                {
                    return;
                }
                context.Response.ContentType = "application/javascript";
                if (context.Request.Browser.Browser.Equals("Chrome", StringComparison.CurrentCultureIgnoreCase))
                {
                    context.Response.TransmitFile(context.Request.PhysicalApplicationPath + "\\Scripts\\chrome.js");
                    context.Response.End();
                }
                else if (context.Request.Browser.Browser.Equals("mozilla", StringComparison.CurrentCultureIgnoreCase))
                {
                    context.Response.TransmitFile(context.Request.PhysicalApplicationPath + "\\Scripts\\firefox.js");
                    context.Response.End();
                }
                else
                {
                    context.Response.TransmitFile(context.Request.PhysicalApplicationPath + "\\Scripts\\ie.js");
                    context.Response.End();
                }
#endif
        }
    }
}