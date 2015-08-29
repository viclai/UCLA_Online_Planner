using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UCLA_Student_Planner
{
    public partial class SessionCheck : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // May need to use function to check for new session. Below not working?
            if (Session["userID"] == null)
                Response.Write("1");
            else
                Response.Write("0");
            Response.End();
        }
    }
}