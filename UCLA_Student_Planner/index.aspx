<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="UCLA_Student_Planner.index" %>

<!DOCTYPE html>
<html lang="en">

<head runat="server">
    <title>UCLA School Planner</title>
    <meta name="viewport" content="width = 1050, user-scalable = no" />
    <script src="Extras/jquery.min.1.7.js"></script>
    <script src="Extras/modernizr.2.5.3.min.js"></script>
    <script src="lib/hash.js"></script>
    <script src="Scripts/pages.js"></script>
    <script src="Scripts/autoLogoff.js"></script>

    <link rel="shortcut icon" href="Favicon/favicon.ico" />
    <link rel="icon" sizes="16x16 32x32 64x64" href="Favicon/favicon.ico" />
    <link rel="icon" type="image/png" sizes="196x196" href="Favicon/favicon-192.png" />
    <link rel="icon" type="image/png" sizes="160x160" href="Favicon/favicon-160.png" />
    <link rel="icon" type="image/png" sizes="96x96" href="Favicon/favicon-96.png" />
    <link rel="icon" type="image/png" sizes="64x64" href="Favicon/favicon-64.png" />
    <link rel="icon" type="image/png" sizes="32x32" href="Favicon/favicon-32.png" />
    <link rel="icon" type="image/png" sizes="16x16" href="Favicon/favicon-16.png" />
    <link rel="apple-touch-icon" href="Favicon/favicon-57.png" />
    <link rel="apple-touch-icon" sizes="114x114" href="Favicon/favicon-114.png" />
    <link rel="apple-touch-icon" sizes="72x72" href="Favicon/favicon-72.png" />
    <link rel="apple-touch-icon" sizes="144x144" href="Favicon/favicon-144.png" />
    <link rel="apple-touch-icon" sizes="60x60" href="Favicon/favicon-60.png" />
    <link rel="apple-touch-icon" sizes="120x120" href="Favicon/favicon-120.png" />
    <link rel="apple-touch-icon" sizes="76x76" href="Favicon/favicon-76.png" />
    <link rel="apple-touch-icon" sizes="152x152" href="Favicon/favicon-152.png" />
    <link rel="apple-touch-icon" sizes="180x180" href="Favicon/favicon-180.png" />
    <meta name="msapplication-TileColor" content="#FFFFFF" />
    <meta name="msapplication-TileImage" content="Favicon/favicon-144.png" />
    <meta name="msapplication-config" content="Favicon/browserconfig.xml" />

    <script src="Scripts/ajax.js"></script>
</head>

<body>
    <div id="tools">
        <button type="button" id="signOutButton" onclick="logOut();">LOG OUT</button>
        <button type="button" id="tipsButton" onclick="showTips();">TIPS</button>
    </div>
    <table>
        <tr>
            <td id="bookSect">

                <div class="flipbook-viewport">
                    <div class="container">
                        <div class="flipbook" id="book">
                            <div class="hard"> <!-- Page 1 -->
                                <br /><br /> 
                                <h1> UCLA Student Planner </h1>
                                <img src="Images/ucla.jpg" width="300" height="256">
                                <br><br>
                                <a href="http://my.ucla.edu" target="_blank">
                                    <img src="Images/MyUCLA.jpg">
                                </a>
                                <a href="http://www.registrar.ucla.edu/" target="_blank">
                                    <img src="Images/registrar.gif">
                                </a>
                            </div>
                        </div>
                    </div>
                </div>

            </td>

            <td id="sidebar"></td>

        </tr>
    </table>
    <input type="hidden" id="uid" name="uIdentif" runat="server" />
    <input type="hidden" id="datesToEntries" runat="server" />
    <input type="hidden" id="weeksToEntries" runat="server" />
    <input type="hidden" id="eventsToDates" runat="server" />
    <input type="hidden" id="startEndDates" runat="server" />
    <input type="hidden" id="weeks" runat="server" />

    <div id="hover" runat="server"></div>
    <div id="Error_Page_Flip" runat="server">
        <div id="errorTab">
            <div id="errorSect">
                <img src="Images/error_symbol.gif" />
            </div>
            <div id="errorMsg">
                ERROR: Please finish adding/editing your entry 
                <br />before turning the page.
            </div>
        </div>
    </div>
    <div id="tips">
        <h1 id="tipsHead">Welcome to UCLA Online Student Planner!</h1>
        We are continuing to improve our UI and add more features to this planner.
        Here are a few things you can try out.
        <ul>
            <li>Flip through the pages by using the LEFT and RIGHT arrow keys or by clicking the corners of the pages.</li>
            <li>Add/edit entries for each day or week. Entry submissions will be on the very right.</li>
        </ul>
    </div>
</body>

<script>
    function logOut() {
        if (document.getElementById("sidebar").getElementsByTagName("textarea").length != 0) {
            alert("You have not submitted your entry yet.  Please click 'Submit' or 'Cancel' before logging out.");
        }
        else
            window.location.href = 'logoff.aspx';
    }

    function showTips() {
        $("#hover").show();
        $("#tips").show();

        $("#hover").click(function () {
            $(this).fadeOut();
            $("#tips").fadeOut();
        });
    }
</script>

</html>

