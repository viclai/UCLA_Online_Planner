using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.SqlClient;
using System.Configuration;

namespace UCLA_Student_Planner
{
    public partial class logoff : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userID"] != null)
            {
                try
                {
                    SqlConnection con =
                        new SqlConnection(ConfigurationManager.ConnectionStrings["AppHConnection"].ConnectionString);
                    con.Open();

                    using (SqlCommand cmd =
                                new SqlCommand("SELECT Username From Users WHERE ID = @uid;", con))
                    {
                        cmd.Parameters.AddWithValue("@uid", Session["userID"]);
                        string username = cmd.ExecuteScalar().ToString();
                        if (username == "guest")
                        {
                            cmd.CommandText = "DELETE FROM DayEntries WHERE [User ID] = @uid;";
                            cmd.ExecuteNonQuery();

                            cmd.CommandText = "DELETE FROM WeekEntries WHERE [User ID] = @uid;";
                            cmd.ExecuteNonQuery();

                            cmd.CommandText = "DELETE FROM Users WHERE ID = @uid;";
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = "Log Out Error: ";
                    msg += ex.Message;
                    System.Diagnostics.Debug.WriteLine(msg);
                }
            }
        }
    }
}