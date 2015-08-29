using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.SqlClient;

namespace UCLA_Student_Planner
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userID"] == null)
            {
                Response.Write("-1");
                Response.End();
            }
            string type = Request.Form["type"];
            string entryDate = Request.Form["entr_date"];
            int userID = Int32.Parse(Request.Form["uIdentif"]);
            string entryText = Request.Form["entr"].Replace("\n", ";");
            string[] entries = entryText.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            Response.Write(addEntries(type, entries, entryDate, userID));
            Response.End();
        }

        private string addEntries(string type, string[] entries, string entryDate, int userID)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString =
                    "Data Source=(localdb)\\v11.0;Initial Catalog=StudentEntries;Integrated Security=True";
                con.Open();

                if (type == "D")
                {
                    foreach (string entry in entries)
                    {
                        using (SqlCommand cmd =
                            new SqlCommand("INSERT INTO DayEntries ([Date of Entry], Entry, [User ID]) VALUES (@date, @entry, @uid);",
                                con))
                        {
                            cmd.Parameters.AddWithValue("@date", entryDate);
                            cmd.Parameters.AddWithValue("@entry", entry);
                            cmd.Parameters.AddWithValue("@uid", userID);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                else if (type == "W")
                {
                    foreach (string entry in entries)
                    {
                        using (SqlCommand cmd =
                            new SqlCommand("INSERT INTO WeekEntries ([Week of Entry], Entry, [User ID]) VALUES (@week, @entry, @uid);",
                                con))
                        {
                            cmd.Parameters.AddWithValue("@week", entryDate);
                            cmd.Parameters.AddWithValue("@entry", entry);
                            cmd.Parameters.AddWithValue("@uid", userID);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                else // 'type' may be null
                {
                    con.Close();
                    return "0";
                }
                con.Close();
                return "1";
            }
            catch (Exception ex)
            {
                string msg = "Insert Error: ";
                msg += ex.Message;
                System.Diagnostics.Debug.WriteLine(msg);
                return "0";
            }
        }
    }
}