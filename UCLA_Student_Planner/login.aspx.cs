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
            string str = client.DownloadString("http://my.ucla.edu/");
            string pattern =
                "<li\\s+id=\"nav-li-logout\">\\s*[^<]+\\s*</li>";
            Regex rgx = new Regex(pattern);
            string subpattern = ">(\\r\\n)?[a-zA-Z0-9 ]+\\s*<";
            Regex innerRgx = new Regex(subpattern);
            string cont = innerRgx.Matches(rgx.Matches(str)[0].Value)[0].ToString();
            Regex sRgx = new Regex("\\s+");
            cont = sRgx.Replace(cont, " ");
            string contSub = cont.Substring(2, cont.Length - 4); // UCLA Week
            dateWeek.InnerHtml += contSub + " | ";

            DateTime today = DateTime.Now;
            dateWeek.InnerHtml += today.ToString("D");
        }

        protected void signinButton_Click(object sender, EventArgs e)
        {
            
        }
    }
}