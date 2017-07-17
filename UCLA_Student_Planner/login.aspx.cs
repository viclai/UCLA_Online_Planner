using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Net;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace UCLA_Student_Planner
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            loadDateAndWeek();
        }

        private void loadDateAndWeek()
        {
            WebClient client = new WebClient();
            /*try
            {
                string str = client.DownloadString("http://my.ucla.edu/");

                dateWeek.InnerHtml += extractWeek(str) + " | ";
                dateWeek.InnerHtml += extractDate(str);
            }
            catch (WebException e)
            {
                System.Diagnostics.Trace.TraceInformation("loadDateAndWeek\n");
                System.Diagnostics.Trace.TraceInformation("Error message:\n" + e.Message);
                System.Diagnostics.Trace.TraceInformation("\nStack trace:\n" + e.StackTrace);
                System.Diagnostics.Trace.TraceInformation("\nTarget site:\n" + e.TargetSite);
            }*/

            //DateTime today = DateTime.Now;
            //dateWeek.InnerHtml += today.ToString("D");
        }

        private string extractWeek(string urlstring)
        {
            string pattern =
                "<li\\s+id=\"nav-li-logout\">\\s*[^<]+\\s*</li>";
            Regex rgx = new Regex(pattern);
            string subpattern = ">(\\r\\n)?\\s*[a-zA-Z0-9\\- ]+(\\r\\n)?\\s*<";
            Regex innerRgx = new Regex(subpattern);
            string cont = innerRgx.Matches(rgx.Matches(urlstring)[0].Value)[0].ToString();
            Regex sRgx = new Regex("\\s+");
            cont = sRgx.Replace(cont, " ");
            string contSub = cont.Substring(2, cont.Length - 4); // UCLA Week
            return contSub;
        }

        private string extractDate(string urlstring)
        {
            string pattern =
                "<li\\s+id=\"nav-li-date\">\\s*<span class=\"hide-small\">[^<]+\\s*</span>";
            Regex rgx = new Regex(pattern);
            string subpattern = ">[a-zA-Z0-9, ]+<";
            Regex innerRgx = new Regex(subpattern);
            string cont = innerRgx.Matches(rgx.Matches(urlstring)[0].Value)[0].ToString();
            string date = cont.Substring(1, cont.Length - 2);
            return date;
        }

        protected void signinButton_Click(object sender, EventArgs e)
        {
            
        }
    }
}