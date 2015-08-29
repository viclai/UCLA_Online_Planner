using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

using System.Data.SqlClient;

namespace UCLA_Student_Planner
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {
            
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            deleteGuestAccount();
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        private void deleteGuestAccount()
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString =
                    "Data Source=(localdb)\\v11.0;Initial Catalog=StudentEntries;Integrated Security=True";
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
                string msg = "Session End Error: ";
                msg += ex.Message;
                System.Diagnostics.Debug.WriteLine(msg);
            }
        }
    }
}