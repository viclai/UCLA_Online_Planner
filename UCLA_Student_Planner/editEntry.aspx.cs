using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.SqlClient;
using System.Collections;

namespace UCLA_Student_Planner
{
    public partial class editEntry : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userID"] == null)
            {
                Response.Write("-1");
                Response.End();
            }
            string type = Request.Form["type"];
            string entryDate = Request.Form["edit_entr_date"];
            int userID = Int32.Parse(Request.Form["uIdentif"]);
            string entryText = Request.Form["entr"].Replace("\n", ";");
            string[] entries = entryText.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            Response.Write(editEntries(type, entries, entryDate, userID));
            
            Response.End();
        }

        private string editEntries(string type, string[] entries, string entryDate, int userID)
        {
            int nEntries = entries.Length;
            int nOrigEntries = 0;
            KeyValuePair<string, string>[] entryMap;
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString =
                    "Data Source=(localdb)\\v11.0;Initial Catalog=StudentEntries;Integrated Security=True";
                con.Open();

                /* Get a count of how many entries of that particular date were originally in the database */
                if (type == "D")
                {
                    using (SqlCommand countCmd =
                        new SqlCommand("SELECT COUNT(*) FROM DayEntries WHERE [User ID] = @userid AND ([Date of Entry] = '" +
                            entryDate + "')", con))
                    {
                        countCmd.Parameters.AddWithValue("@userid", userID);
                        nOrigEntries = (int)countCmd.ExecuteScalar();
                        entryMap = new KeyValuePair<string, string>[nOrigEntries];
                    }

                    int i = 0;
                    using (SqlCommand cmd =
                        new SqlCommand("SELECT [Date of Entry], Entry, ID FROM DayEntries WHERE [User ID] = @uid AND ([Date of Entry] = '" +
                            entryDate + "') ORDER BY ID;", con))
                    {
                        cmd.Parameters.AddWithValue("@uid", userID);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                entryMap[i] = new KeyValuePair<string, string>(reader["ID"].ToString(), reader["Entry"].ToString());
                                i++;
                            }
                            reader.Close();
                        }
                    }

                    i = 0;
                    while (i < nEntries)
                    {
                        if (entryMap.Length > i && entries[i] != entryMap[i].Value)
                        {
                            using (SqlCommand updateCmd =
                                new SqlCommand("UPDATE DayEntries SET Entry = @newContent, [Date Modified] = GETDATE() WHERE [User ID] = @user AND ID =" +
                                    entryMap[i].Key, con))
                            {
                                updateCmd.Parameters.AddWithValue("@user", userID);
                                updateCmd.Parameters.AddWithValue("@newContent", entries[i]);
                                updateCmd.ExecuteNonQuery();
                            }
                        }
                        else if (entryMap.Length <= i) // Newly inserted entries
                        {
                            using (SqlCommand cmd =
                            new SqlCommand("INSERT INTO DayEntries ([Date of Entry], Entry, [User ID]) VALUES (@date, @entry, @userID);",
                                con))
                            {
                                cmd.Parameters.AddWithValue("@date", entryDate);
                                cmd.Parameters.AddWithValue("@entry", entries[i]);
                                cmd.Parameters.AddWithValue("@userID", userID);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        i++;
                    }
                    while (i < nOrigEntries)
                    {
                        using (SqlCommand deleteCmd =
                            new SqlCommand("DELETE FROM DayEntries WHERE [User ID] = " + userID + " AND ID =" + entryMap[i].Key, con))
                        {

                            deleteCmd.ExecuteNonQuery();
                        }
                        i++;
                    }
                }
                else if (type == "W")
                {
                    using (SqlCommand countCmd =
                        new SqlCommand("SELECT COUNT(*) FROM WeekEntries WHERE [User ID] = @userid AND ([Week of Entry] = '" +
                            entryDate + "')", con))
                    {
                        countCmd.Parameters.AddWithValue("@userid", userID);
                        nOrigEntries = (int)countCmd.ExecuteScalar();
                        entryMap = new KeyValuePair<string, string>[nOrigEntries];
                    }

                    int i = 0;
                    using (SqlCommand cmd =
                        new SqlCommand("SELECT [Week of Entry], Entry, ID FROM WeekEntries WHERE [User ID] = @uid AND ([Week of Entry] = '" +
                            entryDate + "') ORDER BY ID;", con))
                    {
                        cmd.Parameters.AddWithValue("@uid", userID);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                entryMap[i] = new KeyValuePair<string, string>(reader["ID"].ToString(), reader["Entry"].ToString());
                                i++;
                            }
                            reader.Close();
                        }
                    }

                    i = 0;
                    while (i < nEntries)
                    {
                        if (entryMap.Length > i && entries[i] != entryMap[i].Value)
                        {
                            using (SqlCommand updateCmd =
                                new SqlCommand("UPDATE WeekEntries SET Entry = @newContent, [Date Modified] = GETDATE() WHERE [User ID] = @user AND ID =" +
                                    entryMap[i].Key, con))
                            {
                                updateCmd.Parameters.AddWithValue("@user", userID);
                                updateCmd.Parameters.AddWithValue("@newContent", entries[i]);
                                updateCmd.ExecuteNonQuery();
                            }
                        }
                        else if (entryMap.Length <= i) // Newly inserted entries
                        {
                            using (SqlCommand cmd =
                            new SqlCommand("INSERT INTO WeekEntries ([Week of Entry], Entry, [User ID]) VALUES (@week, @entry, @userID);",
                                con))
                            {
                                cmd.Parameters.AddWithValue("@week", entryDate);
                                cmd.Parameters.AddWithValue("@entry", entries[i]);
                                cmd.Parameters.AddWithValue("@userID", userID);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        i++;
                    }
                    while (i < nOrigEntries)
                    {
                        using (SqlCommand deleteCmd =
                            new SqlCommand("DELETE FROM WeekEntries WHERE [User ID] = " + userID + " AND ID =" + entryMap[i].Key, con))
                        {

                            deleteCmd.ExecuteNonQuery();
                        }
                        i++;
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
                string msg = "Error: ";
                msg += ex.Message;
                System.Diagnostics.Debug.WriteLine(msg);
                return "0";
            }
        }
    }
}