<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="UCLA_Student_Planner.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Planning Your Life at UCLA</title>
    <link rel="stylesheet" href="CSS/login.css" />

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

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <script>
        function quickLogin() {
            var xmlhttp;
            if (window.XMLHttpRequest) { // code for IE7+, Firefox, Chrome, Opera, Safari
                xmlhttp = new XMLHttpRequest();
            }
            else { // code for IE6, IE5
                xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
            }

            xmlhttp.onreadystatechange = function () {
                if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
                    var rtn = xmlhttp.responseText;
                    if (rtn != "0") {
                        var fm = document.createElement("form");
                        fm.setAttribute("action", "index.aspx");
                        fm.setAttribute("method", "post");

                        var userID = document.createElement("input");
                        userID.name = "userID";
                        userID.type = "number";
                        userID.value = rtn;
                        fm.appendChild(userID);

                        var next = document.createElement("input");
                        next.type = "submit";
                        next.value = "next";
                        fm.appendChild(next);

                        document.body.appendChild(fm);

                        fm.submit();
                    }
                    else {
                        $("#signInError").show();
                        document.getElementById("signinButton").value = "Sign In";
                    }
                    $("#loadingImg").hide();
                }
            }

            try {
                var user = document.getElementById("userText").value;
                var pass = document.getElementById("passwordText").value;

                $("#loadingImg").show();
                document.getElementById("signinButton").value = "Signing in...";
                
                xmlhttp.open("POST", "checkUsers.aspx", true);
                xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
                var input = "user=" + user + "&pass=" + pass;
                xmlhttp.send(input);
            }
            catch (err) {
                alert("quickLogin err: " + err.message);
            }

            return false;
        }
    </script>
</head>

<body>
    <div id="hover" runat="server"></div>
    <div id="signUpPopup" runat="server">
        Sorry, signing up for a planner is <br />currently not available.
        <br /><br />
        <button id="UA_Close" type="button">CLOSE</button>
    </div>
    <div id="logPopup" runat="server">
        <div id="close">X</div>
        <h2>LOG IN</h2>
        <hr />
        <form id="form1" runat="server" method="post">
            <div>
                <asp:TextBox ID="userText" runat="server" Placeholder="Username" CssClass="roundBox" AutoCompleteType="Disabled"></asp:TextBox>
                <br />
                <asp:TextBox ID="passwordText" runat="server" Placeholder="Password" TextMode="Password" CssClass="roundBox">
                </asp:TextBox>
                <br />
                <asp:Button ID="signinButton" runat="server" Text="Sign In" OnClick="signinButton_Click" OnClientClick="return quickLogin();" />
            </div>
            <img src="Images/ajax-loader.gif" id="loadingImg" />
            <label id="signInError" runat="server">The username or password entered is incorrect.</label>
        </form>
    </div>

    <div id="topBar">
        <a href="login.aspx"><asp:Image ID="uclaLogo" runat="server" 
            ImageUrl="Images/ucla_planner_logo.gif" ImageAlign="Left" Height="25%" Width="25%" /></a>
        <div>
            <button id="signUpButton" type="button" onclick="showSignup();">Sign up</button>
            <button id="loginButton" type="button" onclick="showLogin();">Log in</button>
        </div>
        <div id="dateWeek" runat="server"></div>
    </div>
    <br />
    <br />
    <div class="tabular">
        <div class="transBox">
            <h1 id="linksTitle">BRUIN LINKS</h1>
            <ul>
                <li><a href="http://www.newstudents.ucla.edu/" target="_blank">New Student Orientation</a></li>
                <li><a href="http://happenings.ucla.edu/" target="_blank">UCLA Happenings</a></li>  
                <li><a href="http://shop.uclastore.com/" target="_blank">UCLA Store</a></li>
                <li><a href="https://secure.bruincard.ucla.edu/bcw/web/Home.aspx" target="_blank">Bruin Card</a></li>
                <li><a href="http://maps.ucla.edu/campus/" target="_blank"
                    title="Find out where exactly your classes are located on campus!">Campus Interactive Map</a></li>
                <li><a href="https://main.transportation.ucla.edu/" target="_blank"
                    title="Commute to and from campus!">Transportation</a></li>
                <li><a href="http://dailybruin.com/" target="_blank"
                    title="Check out what's happening at UCLA!">Daily Bruin</a></li>
            </ul>
            <h2>DINING</h2>
            <ul>
                <li><a href="http://menu.ha.ucla.edu/foodpro/" target="_blank"
                    title="Check out daily menus of your favorite dining halls on the Hill!">Dining Hall Menus</a></li>
                <li><a href="http://asucla.ucla.edu/restaurants/" target="_blank"
                    title="Find restaurants on campus!">On-Campus Food</a></li>
            </ul>
            <h2>ACADEMICS</h2>
            <ul>
                <li><a href="http://my.ucla.edu/" target="_blank">MyUCLA</a></li>
                <li><a href="https://ccle.ucla.edu/" target="_blank">CCLE</a></li>
                <li><a href="http://registrar.ucla.edu/Calendars/Overview" target="_blank"
                    title="Check out when school is in session!">Calendars</a></li>
                <li><a href="https://sa.ucla.edu/ro/Public/SOC" target="_blank">Schedule of Classes</a></li>
                <li><a href="http://www.courserail.com/" target="_blank"
                    title="Find your classes!">Course Rail</a></li>
                <li><a href="http://classscanner.com/" target="_blank" 
                    title="Get alerts on courses in high demand!">Class Scanner</a></li>
                <li><a href="http://www.bruinwalk.com/" target="_blank" 
                    title="Check our reviews and grade distributions of professors!">Bruinwalk</a></li>
            </ul>
        </div>
        <div id="empty">
            <br /><br /><br />
            <asp:Image ID="plannerPic" runat="server" ImageUrl="Images/planner_pic.gif" 
                Height="50%" Width="50%" ImageAlign="AbsMiddle" />
        </div>
        <div id="pgTitleSect">
            <br /><br /><br /><br /><br /><br /><br /><br /><br />
            <h1 id="pgTitle">The Guide to Life at <br /><span id="uclaText">UCLA</span></h1>
        </div>
    </div>

    <div id="copyInfo">
        Copyright &copy; 2015, Victor Lai
    </div>
</body>

<script>
    $("#UA_Close").click(function () {
        $("#hover").fadeOut();
        $("#signUpPopup").fadeOut();
    });

    function showSignup() {
        $("#hover").show();
        $("#signUpPopup").show();

        $("#hover").click(function () {
            $(this).fadeOut();
            $("#signUpPopup").fadeOut();
        });
    }

    function showLogin() {
        $("#hover").show();
        $("#logPopup").show();
        $("#userText").focus();

        $("#hover").click(function () {
            $(this).fadeOut();
            $("#logPopup").fadeOut();
        });

        $("#close").click(function () {
            $("#hover").fadeOut();
            $("#logPopup").fadeOut();
            document.getElementById("userText").value = "";
            document.getElementById("passwordText").value = "";
            $("#signInError").hide();
        });
    }
</script>

</html>
