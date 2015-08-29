<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SessionExpired.aspx.cs" Inherits="UCLA_Student_Planner.SessionExpired" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Session Expired</title>

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
        $(function() {
            /*
             * this swallows backspace keys on any non-input element.
             * stops backspace -> back
             */
            var rx = /INPUT|SELECT|TEXTAREA/i;

            $(document).bind("keydown keypress", function (e) {
                if (e.which == 8) { // 8 == backspace
                    if (!rx.test(e.target.tagName) || e.target.disabled || e.target.readOnly) {
                        e.preventDefault();
                    }
                }
            });
        });
        
        window.onbeforeunload = function () {
            //window.location.replace = "index.aspx";
        }
    </script>
</head>
<body>
    <h1>Session Expired</h1>
    Your session expired.  Please click <a href="login.aspx">here</a> to log in again.
</body>
</html>
