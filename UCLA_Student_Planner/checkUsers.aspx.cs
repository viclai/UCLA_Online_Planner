using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.SqlClient;

namespace UCLA_Student_Planner
{
    public partial class checkUsers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string userIn = Request.Form["user"];
            string passIn = Request.Form["pass"];

            Response.Write(check(userIn, passIn));

            Response.End();
        }

        private string check(string username, string password)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString =
                    "Data Source=(localdb)\\v11.0;Initial Catalog=StudentEntries;Integrated Security=True";
                con.Open();

                if (username == "guest" && password == "guest") // Temporary guest account
                {
                    // TODO: Check for other guests logged on.
                    try
                    {
                        using (SqlCommand cmd =
                            new SqlCommand("INSERT INTO Users (Username, Password) VALUES (@u, @p);", con))
                        {
                            cmd.Parameters.AddWithValue("@u", username);
                            cmd.Parameters.AddWithValue("@p", password);
                            cmd.ExecuteNonQuery();

                            cmd.CommandText = "SELECT ID FROM Users WHERE Username = @u";
                            string userID = cmd.ExecuteScalar().ToString();
                            Session["userID"] = Int32.Parse(userID);
                            return userID;
                        }
                    }
                    catch (Exception ex)
                    {
                        string msg = "Creating Guest Error: ";
                        msg += ex.Message;
                        System.Diagnostics.Debug.WriteLine(msg);
                        return "0";
                    }
                }

                using (SqlCommand cmd =
                        new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username = @username AND Password = @password", con))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    int isValid = (int)cmd.ExecuteScalar();
                    if (isValid == 1)
                    {
                        cmd.CommandText = "SELECT ID FROM Users WHERE Username = @username AND Password = @password";
                        string userID = cmd.ExecuteScalar().ToString();
                        Session["userID"] = Int32.Parse(userID);

                        return userID;
                    }
                    else
                        return "0";
                }
            }
            catch (Exception ex)
            {
                string msg = "Check Error: ";
                msg += ex.Message;
                System.Diagnostics.Debug.WriteLine(msg);
                return "0";
            }
        }
    }
}