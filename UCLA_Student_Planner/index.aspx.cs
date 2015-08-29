using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Net;
using System.Text.RegularExpressions;

namespace UCLA_Student_Planner
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /* Handle the case when user clicks 'Back' on "Session Expired" page */
            //Response.Cache.SetNoStore();
            //Response.Cache.AppendCacheExtension("no-cache");
            //Response.Expires = -1;

            checkSession();
            loadDayEntries();
            loadWeekEntries();
            loadEvents();   
        }

        private void checkSession()
        {
            if (Session["userID"] == null)
                Response.Redirect("SessionExpired.aspx", true);
        }

        private void loadEvents()
        {
            string academYearStart = "", academYearEnd = "";

            int breakI = 0;
            int[] breaks = new int[2]; // [0]: winter, [1]: spring
            KeyValuePair<string, int>[] weekContent = new KeyValuePair<string, int>[] 
            {
                new KeyValuePair<string, int>("FALL QUARTER - ZERO WEEK", 1), 
                new KeyValuePair<string, int>("FALL QUARTER - WEEK ", 10),
                new KeyValuePair<string, int>("FINALS WEEK", 1),
                new KeyValuePair<string, int>("WINTER BREAK", -1), // Unknown
                new KeyValuePair<string, int>("WINTER QUARTER - WEEK ", 10),
                new KeyValuePair<string, int>("FINALS WEEK", 1),
                new KeyValuePair<string, int>("SPRING BREAK", -1), // Unknown
                new KeyValuePair<string, int>("SPRING QUARTER - WEEK ", 10),
                new KeyValuePair<string, int>("FINALS WEEK", 1),
                new KeyValuePair<string, int>("SUMMER", 0), // Unknown
                new KeyValuePair<string, int>("FALL QUARTER - ZERO WEEK", 1),
            };

            WebClient client = new WebClient();
            string curYear = DateTime.Now.Year.ToString();
            string curMonth = DateTime.Now.Month.ToString();
            string curAcademYear = curSchoolYear(curMonth, curYear);
            startEndDates.Value += curAcademYear + "|";
            string htmlCurAcademYear =
                client.DownloadString("http://www.registrar.ucla.edu/calendar/acadcal" +
                curAcademYear.Substring(2, 2) + ".htm");

            string pattern1 =
                "<tr\\s+align=\"left\">\\s*" +
                "<td\\s+(class=\"bold\"\\s+)?(bgcolor=\"#FAE9F7\"\\s+)?width=\"200\">[^<]+</td>\\s*" +
                "<td\\s+(class=\"bold\"\\s+)?(bgcolor=\"#FAE9F7\"\\s+)?width=\"340\">(<span class=\"red\">)?[^<]+(</span>)?</td>\\s*" +
                "</tr>";
            Regex rgx1 = new Regex(pattern1);
            bool isStart = true;
            DateTime[] breakDates = new DateTime[2] {new DateTime(1970, 1, 1), new DateTime (1970, 1, 1)};
            /* Load events (including start date) into hidden fields. */
            foreach (Match match in rgx1.Matches(htmlCurAcademYear))
            {
                string subpattern = ">[A-Z][^<]+<";
                Regex innerRgx = new Regex(subpattern);
                string evt = innerRgx.Matches(match.Value)[0].ToString();
                int evtLen = evt.Length;
                string date = innerRgx.Matches(match.Value)[1].ToString();
                int dateLen = date.Length;

                eventsToDates.Value += evt.Substring(1, evtLen - 2) + "|" + date.Substring(1, dateLen - 2) + ";";
                if (evt.Substring(1, evtLen - 2) == "Quarter ends") // For calulcating winter/spring break intervals
                {
                    string[] dateContent = date.Substring(1, dateLen - 2).Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    int month = monthNo(dateContent[1]);
                    int year;
                    if (month >= 9)
                        year = Convert.ToInt32(curAcademYear);
                    else
                        year = Convert.ToInt32(curAcademYear) + 1;
                    int dateNo = Convert.ToInt32(dateContent[2]);
                    breakDates[0] = new DateTime(year, month, dateNo);
                }
                if (isStart && evt.Substring(1, evtLen - 2) == "Quarter begins")
                {
                    startEndDates.Value += date.Substring(1, dateLen - 2) + "|";
                    academYearStart = date.Substring(1, dateLen - 2);
                }
                if (isStart && evt.Substring(1, evtLen - 2) == "Instruction begins")
                    isStart = false;
                else if (!isStart && evt.Substring(1, evtLen - 2) == "Instruction begins") // For calulcating winter/spring break intervals
                {
                    string[] dateContent = date.Substring(1, dateLen - 2).Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    int month = monthNo(dateContent[1]);
                    int year;
                    if (month >= 9)
                        year = Convert.ToInt32(curAcademYear);
                    else
                        year = Convert.ToInt32(curAcademYear) + 1;
                    int dateNo = Convert.ToInt32(dateContent[2]);
                    breakDates[1] = new DateTime(year, month, dateNo);

                    breaks[breakI] = nWeeks(breakDates[0], breakDates[1]);
                    breakI++;
                }
            }
            breakI = 0;

            /* Load end date into hidden field. */
            string nextAcademYear = (Convert.ToInt32(curAcademYear) + 1).ToString();
            WebClient client2 = new WebClient();
            string htmlnextAcademYear =
                client2.DownloadString("http://www.registrar.ucla.edu/calendar/acadcal" +
                nextAcademYear.Substring(2, 2) + ".htm");
            string pattern2 =
                "<tr\\s+align=\"left\">\\s*" +
                "<td\\s+(class=\"bold\"\\s+)?(bgcolor=\"#FAE9F7\"\\s+)?width=\"200\">Quarter begins</td>\\s*" +
                "<td\\s+(class=\"bold\"\\s+)?(bgcolor=\"#FAE9F7\"\\s+)?width=\"340\">(<span class=\"red\">)?[^<]+(</span>)?</td>\\s*" +
                "</tr>";
            Regex rgx2 = new Regex(pattern2);
            string datePattern = "Monday, [^<]+";
            Regex subRgx = new Regex(datePattern);
            string endDate = subRgx.Match(rgx2.Match(htmlnextAcademYear).ToString()).ToString();
            startEndDates.Value += endDate;
            academYearEnd = endDate;

            /* Calculate number of weeks of summer */
            string[] academYearStartContent = academYearStart.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int startMonth = monthNo(academYearStartContent[1]);
            int startYear = Convert.ToInt32(curAcademYear);
            int startDateNo = Convert.ToInt32(academYearStartContent[2]);
            breakDates[0] = new DateTime(startYear, startMonth, startDateNo);

            string[] academYearEndContent = academYearEnd.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int endMonth = monthNo(academYearEndContent[1]);
            int endYear = Convert.ToInt32(curAcademYear) + 1;
            int endDateNo = Convert.ToInt32(academYearEndContent[2]);
            breakDates[1] = new DateTime(endYear, endMonth, endDateNo + 7);
            int totalWeeks = nWeeks(breakDates[0], breakDates[1]);

            int weeksTaken = 0;
            int summerIndex = -1; ;
            for (int i = 0; i < weekContent.Length; i++)
            {
                if (weekContent[i].Key == "SUMMER")
                    summerIndex = i;
                if (weekContent[i].Value == -1)
                {
                    weekContent[i] = new KeyValuePair<string,int>(weekContent[i].Key, breaks[breakI]);
                    breakI++;
                }
                weeksTaken += weekContent[i].Value;
            }
            weekContent[summerIndex] = new KeyValuePair<string, int>(weekContent[summerIndex].Key, totalWeeks - weeksTaken);

            /* Load week content into hidden field. */
            for (int i = 0; i < weekContent.Length; i++)
            {
                if (weekContent[i].Value == 10)
                {
                    for (int j = 1; j <= 10; j++)
                        weeks.Value += weekContent[i].Key + j.ToString() + "|";
                }
                else
                {
                    for (int j = 1; j <= weekContent[i].Value; j++)
                        weeks.Value += weekContent[i].Key + "|";
                }
            }
        }

        private int monthNo(string monthName)
        {
            switch (monthName)
            {
                case "January": return 1;
                case "February": return 2;
                case "March": return 3;
                case "April": return 4;
                case "May": return 5;
                case "June": return 6;
                case "July": return 7;
                case "August": return 8;
                case "September": return 9;
                case "October": return 10;
                case "November": return 11;
                case "December": return 12;
                default: return -1;
            }
        }

        // Precondition: 'd2' is a later date than 'd1'; 'd1' and 'd2' 
        //                represented as (yyyy, mm, dd)
        private int nWeeks(DateTime d1, DateTime d2) 
        {
            TimeSpan t = d2 - d1;
            return t.Days / 7;
        }

        private string curSchoolYear(string month, string year)
        {
            string curAcademYear = year;
            switch (month)
            {
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                    curAcademYear = (Convert.ToInt32(curAcademYear) - 1).ToString();
                    break;
                case "9": // Assumption: Academic year always starts in September
                    WebClient client = new WebClient();
                    string reply = 
                        client.DownloadString("http://www.registrar.ucla.edu/calendar/acadcal" + 
                        year.Substring(2, 2) + ".htm");
                    string pattern = 
                        "<tr\\s+align=\"left\">\\s*" + 
                        "<td\\s+(class=\"bold\"\\s+)?(bgcolor=\"#FAE9F7\"\\s+)?width=\"200\">Quarter begins</td>\\s*" +
                        "<td\\s+(class=\"bold\"\\s+)?(bgcolor=\"#FAE9F7\"\\s+)?width=\"340\">[^<]+</td>\\s*" +
                        "</tr>";
                    Regex rgx = new Regex(pattern);
                    Match match = rgx.Match(reply);

                    string subpattern = "September \\d+";
                    Regex rgx2 = new Regex(subpattern);
                    Match date = rgx2.Match(match.Value);
                    int dateLen = date.Value.Length;
                    string now = DateTime.Now.Date.ToString("d");
                    int firstSlash = now.IndexOf('/', 0);
                    int secondSlash = now.IndexOf('/', firstSlash + 1);
                    string parsedDate = now.Substring(firstSlash + 1, secondSlash - firstSlash - 1);

                    int test = Convert.ToInt32(" 2");
                    if (Convert.ToInt32(parsedDate) < Convert.ToInt32(date.Value.Substring(dateLen - 2, 2)))
                        curAcademYear = (Convert.ToInt32(curAcademYear) - 1).ToString();
                    break;
                case "10":
                case "11":
                case "12":
                    break;
            }
            return curAcademYear;
        }

        private int getUserID()
        {
            int userID;
            if (Request.Form["userID"] == null)
                userID = Int32.Parse(uid.Value);
            else
            {
                userID = Int32.Parse(Request.Form["userID"]);
                uid.Value = userID.ToString();
            }

            // Place user ID in Session
            if (Session["userID"] == null)
                Session["userID"] = userID;

            return userID;
        }

        private void loadDayEntries()
        {
            int userID = getUserID();
            
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString =
                    "Data Source=(localdb)\\v11.0;Initial Catalog=StudentEntries;Integrated Security=True";
                con.Open();

                using (SqlCommand cmd = 
                    new SqlCommand("SELECT [Date of Entry], Entry FROM DayEntries WHERE [User ID] = @userid;", con))
                {
                    cmd.Parameters.AddWithValue("@userid", userID);
                    using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            string dateEntry = reader["Date of Entry"].ToString();
                            int firstSlash = dateEntry.IndexOf('/', 0);
                            string month = dateEntry.Substring(0, firstSlash);
                            if (month.Length == 1)
                                month = "0" + month;
                            int secondSlash = dateEntry.IndexOf('/', firstSlash + 1);
                            string date = dateEntry.Substring(firstSlash + 1, secondSlash - firstSlash - 1);
                            if (date.Length == 1)
                                date = "0" + date;
                            string year = dateEntry.Substring(secondSlash + 1, 4);

                            string id = year + month + date;
                            string entry = reader["Entry"].ToString();

                            datesToEntries.Value += id + "|" + entry + ";";
                        }
                        reader.Close();
                    }
                }
            }
            catch(Exception ex)
            {
                string msg = "Error: ";
                msg += ex.Message;
                System.Diagnostics.Debug.WriteLine(msg);
            }
        }

        private void loadWeekEntries()
        {
            int userID = getUserID();
            
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString =
                    "Data Source=(localdb)\\v11.0;Initial Catalog=StudentEntries;Integrated Security=True";
                con.Open();

                using (SqlCommand cmd =
                    new SqlCommand("SELECT [Week of Entry], Entry FROM WeekEntries WHERE [User ID] = @userid;", con))
                {
                    cmd.Parameters.AddWithValue("@userid", userID);
                    using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            string week = reader["Week of Entry"].ToString();
                            string entry = reader["Entry"].ToString();

                            weeksToEntries.Value += week + "|" + entry + ";";
                        }
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = "Error: ";
                msg += ex.Message;
                System.Diagnostics.Debug.WriteLine(msg);
            }
        }
    }
}