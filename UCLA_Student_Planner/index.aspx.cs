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
using System.Configuration;

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

        private const string UCLA_CALENDAR_PAGE = "http://registrar.ucla.edu/Calendars/Annual-Academic-Calendar";

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
                new KeyValuePair<string, int>("WINTER BREAK", -1), // Unknown - get from registrar page
                new KeyValuePair<string, int>("WINTER QUARTER - WEEK ", 10),
                new KeyValuePair<string, int>("FINALS WEEK", 1),
                new KeyValuePair<string, int>("SPRING BREAK", -1), // Unknown - get from registrar page
                new KeyValuePair<string, int>("SPRING QUARTER - WEEK ", 10),
                new KeyValuePair<string, int>("FINALS WEEK", 1),
                new KeyValuePair<string, int>("SUMMER", 0), // Unknown - get from registrar page
                new KeyValuePair<string, int>("FALL QUARTER - ZERO WEEK", 1),
            };

            WebClient client = new WebClient();
            string curYear = DateTime.Now.Year.ToString();
            string curMonth = DateTime.Now.Month.ToString();

            // Get calendar from registrar page based on the current academic year.
            string curAcademYear = curSchoolYear(curMonth, curYear);
            startEndDates.Value += curAcademYear + "|";
            string htmlAcademYear = client.DownloadString(UCLA_CALENDAR_PAGE);
            string yearPattern =
                "<li\\s+(class=\"active\")?><a href=\"#(.+)\"\\s+(data-toggle=\"tab\")?>" + curAcademYear + "-\\d+</a></li>";
            Regex rgxYear = new Regex(yearPattern);
            string id = rgxYear.Match(htmlAcademYear).Groups[2].Value;

            string divPattern =
                "<div\\s+class=\".+\"\\s+id=\"" + id + "\">\\s*<div\\s+class=\".+\">\\s*" +
                "<table\\s+class=\"table\">\\s*" +
                "<tbody>\\s*" +
                "(.+)" +
                "</tbody>\\s*" +
                "</table>\\s*" +
                "</div>\\s*</div>";
            string content = Regex.Match(htmlAcademYear, divPattern, RegexOptions.Singleline).Groups[1].Value;

            string cellPattern =
                "<tr>\\s*<td>\\s*(.+)\\s*</td>\\s*<td>\\s*(.+)\\s*</td>\\s*</tr>";
            Regex rgx1 = new Regex(cellPattern);
            bool isStart = true;
            DateTime[] breakDates = new DateTime[2] {new DateTime(1970, 1, 1), new DateTime (1970, 1, 1)};
            /* Load events (including start date) into hidden fields. */
            foreach (Match match in rgx1.Matches(content))
            {
                string contentPattern = ">[A-Z][^<]+<";
                Regex innerRgx = new Regex(contentPattern);
                string evt = match.Groups[1].Value;
                string date = match.Groups[2].Value;

                eventsToDates.Value += evt + "|" + date + ";";
                if (evt == "Quarter ends") // For calulcating winter/spring break intervals
                {
                    string[] dateContent = 
                        date.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    int month = monthNo(dateContent[1]);
                    int year;
                    if (month >= 9)
                        year = Convert.ToInt32(curAcademYear);
                    else
                        year = Convert.ToInt32(curAcademYear) + 1;
                    int dateNo = Convert.ToInt32(dateContent[2]);
                    breakDates[0] = new DateTime(year, month, dateNo);
                }
                if (isStart && evt == "Quarter begins")
                {
                    startEndDates.Value += date + "|";
                    academYearStart = date;
                }
                if (isStart && evt == "Instruction begins")
                    isStart = false;
                else if (!isStart && evt == "Instruction begins") // For calulcating winter/spring break intervals
                {
                    string[] dateContent = 
                        date.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
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

            /* Load end date (by getting date of Fall Quarter beginning next year) into hidden field. */
            string nextAcademYear = (Convert.ToInt32(curAcademYear) + 1).ToString();

            yearPattern =
                "<li\\s+(class=\"active\")?><a href=\"#(.+)\"\\s+(data-toggle=\"tab\")?>" + nextAcademYear + "-\\d+</a></li>";
            rgxYear = new Regex(yearPattern);
            id = rgxYear.Match(htmlAcademYear).Groups[2].Value;

            divPattern =
                "<div\\s+class=\".+\"\\s+id=\"" + id + "\">\\s*<div\\s+class=\".+\">\\s*" +
                "<table\\s+class=\"table\">\\s*" +
                "<tbody>\\s*" +
                "(.+)" +
                "</tbody>\\s*" +
                "</table>\\s*" +
                "</div>\\s*</div>";
            content = Regex.Match(htmlAcademYear, divPattern, RegexOptions.Singleline).Groups[1].Value;

            string startPattern =
                "<td>\\s*Quarter begins.+Monday,\\s+(.+)\\s*</td>"; // ASSUMPTION: Quarter begins on a Monday
            Regex rgxStart = new Regex(startPattern);
            string endDate = Regex.Match(content, startPattern, RegexOptions.Singleline).Groups[1].Value;
            
            startEndDates.Value += endDate;
            academYearEnd = endDate;

            /* Calculate number of weeks of summer */
            string[] academYearStartContent = 
                academYearStart.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int startMonth = monthNo(academYearStartContent[1]);
            int startYear = Convert.ToInt32(curAcademYear);
            int startDateNo = Convert.ToInt32(academYearStartContent[2]);
            breakDates[0] = new DateTime(startYear, startMonth, startDateNo);

            string[] academYearEndContent = 
                academYearEnd.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int endMonth = monthNo(academYearEndContent[1]);
            int endYear = Convert.ToInt32(curAcademYear) + 1;
            int endDateNo = Convert.ToInt32(academYearEndContent[2]);
            breakDates[1] = new DateTime(endYear, endMonth, endDateNo + 7);
            int totalWeeks = nWeeks(breakDates[0], breakDates[1]);

            int weeksTaken = 0; // Number of non-summer weeks
            int summerIndex = -1;
            for (int i = 0; i < weekContent.Length; i++)
            {
                if (weekContent[i].Key == "SUMMER")
                    summerIndex = i;
                if (weekContent[i].Value == -1) // Record number of weeks for spring break and winter break
                {
                    weekContent[i] = new KeyValuePair<string,int>(weekContent[i].Key, breaks[breakI]);
                    breakI++;
                }
                weeksTaken += weekContent[i].Value;
            }
            // Calculate number of weeks of summer by taking difference of total number of weeks and non-summer weeks
            weekContent[summerIndex] = new KeyValuePair<string, int>(weekContent[summerIndex].Key, totalWeeks - weeksTaken);

            /* Load week content (rotated text of planner) into hidden field. */
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
                case "9": // ASSUMPTION: Academic year always starts in September
                    WebClient client = new WebClient();
                    string reply = client.DownloadString(UCLA_CALENDAR_PAGE);
                    string defYearPattern = 
                        "<li\\s+class=\"active\"><a href=\"#(.+)\"\\s+(data-toggle=\"tab\")?>(\\d+)-\\d+</a></li>";
                    Regex rgxDefYear = new Regex(defYearPattern);
                    string defaultYear = rgxDefYear.Match(reply).Groups[2].Value;
                    string yearPattern = 
                        "<li\\s+(class=\"active\")?><a href=\"#(.+)\"\\s+(data-toggle=\"tab\")?>" + year + "-\\d+</a></li>";
                    Regex rgxYear = new Regex(yearPattern);
                    string id = rgxYear.Match(reply).Groups[2].Value;

                    string divPattern = 
                        "<div\\s+class=\".+\"\\s+id=\"" + id + "\">\\s*<div\\s+class=\".+\">\\s*" +
                        "<table\\s+class=\"table\">\\s*" +
                        "<tbody>\\s*" +
                        "(.+)" +
                        "</tbody>\\s*" +
                        "</table>\\s*" +
                        "</div>\\s*</div>";
                    string content = Regex.Match(reply, divPattern, RegexOptions.Singleline).Groups[1].Value;

                    string startPattern =
                        "<td>\\s*Quarter begins.+September\\s+(\\d+)\\s*</td>";
                    Regex rgxStart = new Regex(startPattern);
                    string dayNum = Regex.Match(content, startPattern, RegexOptions.Singleline).Groups[1].Value;

                    string now = DateTime.Now.Date.ToString("d"); // Format: mm/dd/yyyy
                    int firstSlash = now.IndexOf('/', 0);
                    int secondSlash = now.IndexOf('/', firstSlash + 1);
                    string parsedDate = now.Substring(firstSlash + 1, secondSlash - firstSlash - 1); // Get dd from mm/dd/yyyy

                    // Compare today's date with date of Fall Quarter beginning
                    int curDate = Convert.ToInt32(parsedDate);
                    int schoolStartDate = Convert.ToInt32(dayNum);
                    if (curDate < schoolStartDate)
                        curAcademYear = defaultYear;
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
            {
                if (uid.Value == "")
                    Response.Redirect("SessionExpired.aspx", true);
                userID = Int32.Parse(uid.Value);
            }
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
                SqlConnection con =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["AppHConnection"].ConnectionString);
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
                string msg = "loadDayEntries() Error: ";
                msg += ex.Message;
                System.Diagnostics.Debug.WriteLine(msg);
            }
        }

        private void loadWeekEntries()
        {
            int userID = getUserID();
            
            try
            {
                SqlConnection con =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["AppHConnection"].ConnectionString);
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
                string msg = "loadWeekEntries() Error: ";
                msg += ex.Message;
                System.Diagnostics.Debug.WriteLine(msg);
            }
        }
    }
}