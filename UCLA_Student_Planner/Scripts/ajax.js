function addDayEntry() {
    var xmlhttp;
    if (window.XMLHttpRequest) { // code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else { // code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }

    var id_date = document.getElementById("id_date").value;
    var entr_content = document.getElementById("entr_content").value;

    xmlhttp.onreadystatechange = function() {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            var rtn = xmlhttp.responseText;
            if (rtn == "1") {
                if ($("#hasText").val() == "false") {
                    document.getElementById(id_date).innerHTML += "<br />" + entr_content;
                }
                $('#sidebar').empty();
                if (document.getElementById("success") == null) {
                    var suc = document.createElement("label");
                    suc.setAttribute("class", "alert");
                    suc.innerHTML = "Successfully added!";
                    suc.id = "success";
                    var sidebar = document.getElementById("sidebar");
                    sidebar.appendChild(suc);
                }
                $('#success').delay(2000).fadeOut('slow');
            }
            else if (rtn == "-1") { // Session timeout
                window.location.href = "SessionExpired.aspx";
            }
            else {
                var test = document.getElementById("cancAdd");
                document.getElementById('cancAdd').click();
                if (document.getElementById("failure") == null) {
                    var fail = document.createElement("label");
                    fail.setAttribute("class", "alert");
                    fail.innerHTML = "Oops, an error occurred!";
                    fail.id = "failure";
                    var sidebar = document.getElementById("sidebar");
                    sidebar.appendChild(fail);
                }
                $('#failure').delay(2000).fadeOut('slow');
            }
        }
    }

    try {
        xmlhttp.open("POST", "addEntry.aspx", true);
        xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
        var userid = document.getElementById("uid").value;
        var input = "entr_date=" + id_date + "&uIdentif=" + userid + "&entr=" + entr_content + "&type=D";
        xmlhttp.send(input);
    }
    catch (err) {
        alert("addDayEntry err: " + err.message);
    }

    return false;
}

function addWeekEntry() {
    var xmlhttp;
    if (window.XMLHttpRequest) { // code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else { // code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }

    var week = document.getElementById("id_date").value;
    var entr_content = document.getElementById("entr_content").value;

    xmlhttp.onreadystatechange = function() {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            var rtn = xmlhttp.responseText;
            if (rtn == "1") {
                if ($("#hasText").val() == "false") {
                    document.getElementById(week).innerHTML += "<br />" + entr_content;
                }
                $('#sidebar').empty();
                if (document.getElementById("success") == null) {
                    var suc = document.createElement("label");
                    suc.setAttribute("class", "alert");
                    suc.innerHTML = "Successfully added!";
                    suc.id = "success";
                    var sidebar = document.getElementById("sidebar");
                    sidebar.appendChild(suc);
                }
                $('#success').delay(2000).fadeOut('slow');
            }
            else if (rtn == "-1") { // Session timeout
                window.location.href = "SessionExpired.aspx";
            }
            else {
                document.getElementById("cancAdd").click();
                if (document.getElementById("failure") == null) {
                    var fail = document.createElement("label");
                    fail.setAttribute("class", "alert");
                    fail.innerHTML = "Oops, an error occurred!";
                    fail.id = "failure";
                    var sidebar = document.getElementById("sidebar");
                    sidebar.appendChild(fail);
                }
                $('#failure').delay(2000).fadeOut('slow');
            }
        }
    }

    try {
        xmlhttp.open("POST", "addEntry.aspx", true);
        xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
        var userid = document.getElementById("uid").value;
        var input = "entr_date=" + week + "&uIdentif=" + userid + "&entr=" + entr_content + "&type=W";
        xmlhttp.send(input);
    }
    catch (err) {
        alert("addWeekEntry err: " + err.message);
    }

    return false;
}

function editDayEntry() {
    var xmlhttp;
    if (window.XMLHttpRequest) { // code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else { // code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }

    xmlhttp.onreadystatechange = function() {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            var rtn = xmlhttp.responseText;
            if (rtn == "1") {
                $('#sidebar').empty();
                if (document.getElementById("success") == null) {
                    var suc = document.createElement("label");
                    suc.setAttribute("class", "alert");
                    suc.innerHTML = "Successfully edited!";
                    suc.id = "success";
                    var sidebar = document.getElementById("sidebar");
                    sidebar.appendChild(suc);
                }
                $('#success').delay(2000).fadeOut('slow');
            }
            else if (rtn == "-1") { // Session timeout
                window.location.href = "SessionExpired.aspx";
            }
            else {
                document.getElementById("cancEdit").click();
                if (document.getElementById("failure") == null) {
                    var fail = document.createElement("label");
                    fail.setAttribute("class", "alert");
                    fail.innerHTML = "Oops, an error occurred!";
                    fail.id = "failure";
                    var sidebar = document.getElementById("sidebar");
                    sidebar.appendChild(fail);
                }
                $('#failure').delay(2000).fadeOut('slow');
            }
        }
    }

    try {
        xmlhttp.open("POST", "editEntry.aspx", true);
        xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
        var date = document.getElementById("edit_id_date").value;
        var userid = document.getElementById("uid").value;
        var content = document.getElementById("edit_entr_content").value;
        var input = "edit_entr_date=" + date + "&uIdentif=" + userid + "&entr=" + content + "&type=D";
        xmlhttp.send(input);
    }
    catch (err) {
        alert("editDayEntry err: " + err.message);
    }

    return false;
}

function editWeekEntry() {
    var xmlhttp;
    if (window.XMLHttpRequest) { // code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else { // code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }

    xmlhttp.onreadystatechange = function() {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            var rtn = xmlhttp.responseText;
            if (rtn == "1") {
                $('#sidebar').empty();
                if (document.getElementById("success") == null) {
                    var suc = document.createElement("label");
                    suc.setAttribute("class", "alert");
                    suc.innerHTML = "Successfully edited!";
                    suc.id = "success";
                    var sidebar = document.getElementById("sidebar");
                    sidebar.appendChild(suc);
                }
                $('#success').delay(2000).fadeOut('slow');
            }
            else if (rtn == "-1") { // Session timeout
                window.location.href = "SessionExpired.aspx";
            }
            else {
                document.getElementById("cancEdit").click();
                if (document.getElementById("failure") == null) {
                    var fail = document.createElement("label");
                    fail.setAttribute("class", "alert");
                    fail.innerHTML = "Oops, an error occurred!";
                    fail.id = "failure";
                    var sidebar = document.getElementById("sidebar");
                    sidebar.appendChild(fail);
                }
                $('#failure').delay(2000).fadeOut('slow');
            }
        }
    }

    try {
        xmlhttp.open("POST", "editEntry.aspx", true);
        xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
        var week = document.getElementById("edit_id_date").value;
        var userid = document.getElementById("uid").value;
        var content = document.getElementById("edit_entr_content").value;
        var input = "edit_entr_date=" + week + "&uIdentif=" + userid + "&entr=" + content + "&type=W";
        xmlhttp.send(input);
    }
    catch (err) {
        alert("editDayEntry err: " + err.message);
    }

    return false;
}
/*
$(function () {
    function checkSessionState() {
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
                if (rtn == "1") { // Session expired
                    window.location.href = "SessionExpired.aspx";
                }
            }
        }

        try {
            xmlhttp.open("POST", "SessionCheck.aspx", true);
            xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
            xmlhttp.send();
        }
        catch (err) {
            alert("checkSessionState err: " + err.message);
        }
        return false;
    }
    setInterval(checkSessionState, 30000); // 2nd argument in milliseconds.
    checkSessionState();
});
*/