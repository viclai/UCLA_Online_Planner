var datesToPages = {};
var startingPg = 1;

function addEntry(parId) {
    var sb = document.getElementById("sidebar");

    if (sb.getElementsByTagName("textarea").length == 0) {
        var heading = document.createElement("h2");
        heading.setAttribute("id", "head");
        heading.innerHTML = "New Entry";
        sb.appendChild(heading);

        var newForm = document.createElement("form");
        
        var dateIn = document.createElement("input");
        dateIn.setAttribute("id", "id_date");
        dateIn.setAttribute("name", "entr_date");
        dateIn.setAttribute("type", "text");
        dateIn.setAttribute("value", parId);
        newForm.appendChild(dateIn);

        var textBox = document.createElement("textarea");
        if (!isNaN(parId)) {
            var year = parId.substring(0, 4);
            var month = parId.substring(4, 6);
            var date = parId.substring(6, 8);
            textBox.setAttribute("placeholder",
                "Please enter an entry here for " + month + "/" + date + "/" +
                year + ". Separate entries by line.");
        }
        else {
            var weekStr = parId;
            var n = weekStr.indexOf("("); // Extract week number at end
            weekStr = weekStr.substring(0, n - 1);
            textBox.setAttribute("placeholder",
                "Please enter a weekly note here for " + weekStr + ".");
        }
        textBox.id = "entr_content";
        textBox.setAttribute("name", "entr");
        textBox.setAttribute("rows", "20");
        textBox.setAttribute("cols", "30");
        var origState = document.getElementById(parId).innerHTML;
        var isBlank;
        if (document.getElementById(parId).innerHTML == "")
            isBlank = true;
        else
            isBlank = false;
        var keyupFunc = "typeAndShow('" + parId + "', " + textBox.id + ", " +
            isBlank + ");";
        console.log(keyupFunc);
        textBox.setAttribute("onkeyUp", keyupFunc);
        newForm.appendChild(textBox);

        var b = document.createElement("br");
        newForm.appendChild(b);

        var sub = document.createElement("input");
        sub.setAttribute("type", "submit");
        sub.setAttribute("value", "Submit");
        if (!isNaN(parId))
            sub.setAttribute("onclick", "return addDayEntry();");
        else
            sub.setAttribute("onclick", "return addWeekEntry();");
        newForm.appendChild(sub);

        var cancel = document.createElement("button");
        cancel.setAttribute("type", "button");
        cancel.id = "cancAdd";
        var restoreClick = "cancel('" + parId + "', \"" +
            origState + "\");";
        console.log(restoreClick);
        cancel.setAttribute("onclick", restoreClick);
        cancel.innerHTML = "Cancel";
        newForm.appendChild(cancel);

        var hasText = document.createElement("input");
        hasText.type = "text";
        hasText.id = "hasText";
        hasText.value = isBlank;
        newForm.appendChild(hasText);

        sb.appendChild(newForm);
        $("#id_date").hide();
        $("#hasText").hide();
        textBox.focus();
    }
}

function editEntry(parId) {
    var sb = document.getElementById("sidebar");

    if (document.getElementById(parId).innerHTML != "" &&
        sb.getElementsByTagName("textarea").length == 0) {
        var heading = document.createElement("h2");
        heading.setAttribute("id", "edit_head");
        heading.innerHTML = "Edit Entry";
        sb.appendChild(heading);

        var newForm = document.createElement("form");

        var dateIn = document.createElement("input")
        dateIn.setAttribute("id", "edit_id_date");
        dateIn.setAttribute("name", "edit_entr_date");
        dateIn.setAttribute("type", "text");
        dateIn.setAttribute("value", parId);
        newForm.appendChild(dateIn);

        var textBox = document.createElement("textarea");
        textBox.value = document.getElementById(parId).innerHTML;
        textBox.value = textBox.value.replace(/<br>/g, "\n"); // Handle more
                                                              // than 1 entry 
                                                              // with <br>'s.
        textBox.id = "edit_entr_content";
        textBox.setAttribute("name", "entr");
        textBox.setAttribute("rows", "20");
        textBox.setAttribute("cols", "30");
        textBox.setAttribute("onfocus",
            "var temp_value=this.value; this.value=''; this.value=temp_value");
        textBox.setAttribute("onkeyUp",
            "document.getElementById('" + parId + "').innerHTML = this.value");
        newForm.appendChild(textBox);

        var b = document.createElement("br");
        newForm.appendChild(b);

        var sub = document.createElement("input");
        sub.setAttribute("type", "submit");
        sub.setAttribute("value", "Submit");
        if (!isNaN(parId))
            sub.setAttribute("onclick", "return editDayEntry();");
        else
            sub.setAttribute("onclick", "return editWeekEntry();");
        newForm.appendChild(sub);

        var cancel = document.createElement("button");
        cancel.setAttribute("type", "button");
        var origState = document.getElementById(parId).innerHTML;
        var restoreClick = "cancel('" + parId + "', " + "\"" + origState + "\");";
        console.log(restoreClick);
        cancel.setAttribute("onclick", restoreClick);
        cancel.innerHTML = "Cancel";
        cancel.id = "cancEdit";
        newForm.appendChild(cancel);

        sb.appendChild(newForm);
        $("#edit_id_date").hide();
        textBox.focus();
    }
}

function typeAndShow(parId, object, isBlank) {
    var par = document.getElementById(parId);
    if (isBlank)
        par.innerHTML = object.value;
}

function cancel(id, origText) {
    clearSidebar();
    restoreOrigState(id, origText);
}

function restoreOrigState(id, origText) {
    document.getElementById(id).innerHTML = origText;
}

function clearSidebar() {
    $('#sidebar').empty();
}

function loadApp() {
    var flipbook = document.getElementsByClassName("flipbook"); // There should
                                                                // only be 1.

    var days = ["SUN", "MON", "TUES", "WED", "THURS", "FRI", "SAT"];
    var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug",
        "Sep", "Oct", "Nov", "Dec"];

    var weekStrips = document.getElementById("weeks").value;
    var arrWeekStrips = weekStrips.split("|");
    var weekIndex = 0;

    var startEndDates = document.getElementById("startEndDates").value;
    var arrStartEndDates = startEndDates.split("|"); // [0]: current year
                                                     // [1]: start date
                                                     // [2]: end date
    var space1 = arrStartEndDates[1].indexOf(" ");
    var startDateLen = arrStartEndDates[1].length;
    var space2 = arrStartEndDates[2].indexOf(" ");
    var endDateLen = arrStartEndDates[2].length;

    /* Months and days are indices of 'months' and 'days', respectively. */
    var curMonth =
        indexOfMonth(arrStartEndDates[1].substring(space1 + 1, space1 + 4));
    var curDate =
        parseInt(arrStartEndDates[1].substring(startDateLen - 2,
        startDateLen));
    var curYear = parseInt(arrStartEndDates[0]);
    var curDay =
        indexOfDay(arrStartEndDates[1].substring(0, 3).toUpperCase());
    console.log("Start month: " + curMonth + ", Start date: " + curDate +
        ", Start year: " + curYear + ", Start day: " + curDay);

    var endMonth =
        indexOfMonth(arrStartEndDates[2].substring(space2 + 1, space2 + 4));
    var endDate = // Include Zero Week of next year
        parseInt(arrStartEndDates[2].substring(endDateLen - 2,
        endDateLen)) + 7;
    if (endDate > 30) {
        endDate = endDate % 30;
        endMonth += 1;
    }
    var endYear = parseInt(arrStartEndDates[0]) + 1;
    var endDay = indexOfDay(arrStartEndDates[2].substring(0, 3).toUpperCase());
    console.log("End month: " + endMonth + ", End date: " + endDate +
        ", End year: " + endYear + ", End day: " + endDay);

    /* Get today's date */
    var d = new Date();
    var tdyMonth = d.getMonth();
    var tdyDay = d.getDay();
    var tdyDate = d.getDate();
    var tdyYear = d.getFullYear();

    var weekCount = 1;
    var pg = 1;

    /* Cover page (page 1) is already in HTML - so no need to load */

    /* BEGIN loading main pages */
    
    while (curDate !== endDate || curMonth !== endMonth ||
            curYear !== endYear || curDay !== endDay) {

        if (curDay === 1 || curDay === 4) { // New page
            pg++;
            var newPg = document.createElement("div");
            newPg.setAttribute("class", "p" + pg);
            flipbook[0].appendChild(newPg);
            
            var pageContain = document.createElement("div");
            pageContain.setAttribute("ignore", "1");
            if (curDay === 1) { // Left page       
                pageContain.setAttribute("class", "pgContain");
                newPg.appendChild(pageContain);

                var strip = document.createElement("div");
                strip.setAttribute("class", "stripLeft");
                var weekHead = document.createElement("h1");
                var weekContent =
                    document.createTextNode(arrWeekStrips[weekIndex]);
                weekHead.appendChild(weekContent);
                strip.appendChild(weekHead);
                pageContain.appendChild(strip);
                //weekIndex++;

                var pageContent = document.createElement("div");
                pageContent.setAttribute("class", "pgContent");
                pageContain.appendChild(pageContent);
            }
            else { // Right page (curDay === 4)
                pageContain.setAttribute("class", "rightPg");
                newPg.appendChild(pageContain);
            }
        }

        // Hash date to page number
        var dateStr =
            curMonth.toString() + curDate.toString() + curYear.toString();
        console.log(dateStr);
        datesToPages[dateStr] = pg;

        if (tdyDate === curDate && tdyMonth === curMonth &&
                tdyYear === curYear && tdyDay === curDay) {
            startingPg = pg;
        }

        var page;
        var curPage = document.getElementsByClassName("p" + pg)[0];
        if (isLeftPage(curDay))
            page = curPage.firstChild.firstChild.nextElementSibling;
        else
            page = curPage.firstChild;

        if (curDay === 1) {
            // Insert "Weekly Notes"
            var sectionW = document.createElement("div");
            sectionW.setAttribute("ignore", "1");

            var heading = document.createElement("h2");
            heading.innerHTML = "WEEKLY NOTES";
            sectionW.appendChild(heading);

            var box = document.createElement("p");
            box.setAttribute("class", "weekly-notes");
            var id = arrWeekStrips[weekIndex] + " (Week " + weekCount + ")";
            weekIndex++;
            box.setAttribute("id", id);
            sectionW.appendChild(box);

            var btnsDiv = document.createElement("div");
            sectionW.appendChild(btnsDiv);

            var addButton = document.createElement("button");
            addButton.setAttribute("type", "button");
            addButton.setAttribute("class", "button-center");
            var onclick = "addEntry" + "(" + "\"" + id + "\"" + ")";
            addButton.setAttribute("onclick", onclick);
            addButton.innerHTML = "Add";
            btnsDiv.appendChild(addButton);

            var editButton = document.createElement("button");
            editButton.setAttribute("type", "button");
            editButton.setAttribute("class", "button-center");
            var onclickEdit = "editEntry" + "(" + "\"" + id + "\"" + ")";
            editButton.setAttribute("onclick", onclickEdit);
            editButton.innerHTML = "Edit";
            btnsDiv.appendChild(editButton);

            page.appendChild(sectionW);
            weekCount++;
        }

        // Load the current date's entry area here.
        var sectionD = document.createElement("div");
        sectionD.setAttribute("ignore", "1");

        var tbl = document.createElement("table");
        sectionD.appendChild(tbl);
        var row = document.createElement("tr");
        tbl.appendChild(row);

        var leftCol = document.createElement("td");
        leftCol.setAttribute("align", "left");
        var rightCol = document.createElement("td");
        rightCol.setAttribute("align", "right");

        //var eventButton = document.createElement("button");
        //eventButton.setAttribute("type", "button");
        //eventButton.setAttribute("class", "events-center");
        //eventButton.innerHTML = "Events";

        var date = document.createElement("h2"); // TODO: create onhover/
                                                 // onclick for events.
        date.setAttribute("class", "date");
        date.innerHTML = days[curDay] + "<br>" + curDate + "<br>" +
            months[curMonth];

        var mth = incrementMonth(curMonth) + "";
        if (mth === "0")
            mth = "12";
        if (mth.length == 1)
            mth = "0" + mth;
        var dateTemp = curDate + "";
        if (dateTemp.length == 1)
            dateTemp = "0" + dateTemp;
        var id = curYear + mth + dateTemp;

        var box = document.createElement("p");
        box.setAttribute("class", "entry-box");
        box.setAttribute("id", id);

        var btnsDiv = document.createElement("div");
        btnsDiv.setAttribute("class", "buttonCtrls");

        var addButton = document.createElement("button");
        addButton.setAttribute("type", "button");
        addButton.setAttribute("class", "button-center");
        var onclickAdd = "addEntry" + "(" + "\"" + id + "\"" + ")";
        addButton.setAttribute("onclick", onclickAdd);
        addButton.innerHTML = "Add";
        btnsDiv.appendChild(addButton);

        var editButton = document.createElement("button");
        editButton.setAttribute("type", "button");
        editButton.setAttribute("class", "button-center");
        var onclickEdit = "editEntry" + "(" + "\"" + id + "\"" + ")";
        editButton.setAttribute("onclick", onclickEdit);
        editButton.innerHTML = "Edit";
        btnsDiv.appendChild(editButton);

        var b1 = document.createElement("br");

        if (curDay === 1 || curDay === 2 || curDay === 3) {
            leftCol.setAttribute("width", "40%");
            rightCol.setAttribute("width", "60%");

            leftCol.appendChild(b1);
            //leftCol.appendChild(eventButton);
            leftCol.appendChild(date);

            rightCol.appendChild(box);
            rightCol.appendChild(btnsDiv);
        }
        else {
            leftCol.setAttribute("width", "60%");
            rightCol.setAttribute("width", "40%");

            leftCol.appendChild(box);
            leftCol.appendChild(btnsDiv);

            rightCol.appendChild(b1);
            //rightCol.appendChild(eventButton);
            rightCol.appendChild(date);
        }
        row.appendChild(leftCol);
        row.appendChild(rightCol);
        page.appendChild(sectionD);

        // Increment
        var prevDate = curDate;
        curDay = incrementDay(curDay);
        curDate = incrementDate(curDate, curMonth, curYear);
        curYear = (curMonth === 11 && prevDate === 31) ?
			incrementYear(curYear) : curYear;
        curMonth = (curDate === 1) ? incrementMonth(curMonth) : curMonth;
    }
    /* END loading main pages */
    console.log("Total pages: " + pg - 1);

    /* Load back cover here. */
    var lastPg = document.createElement("div");
    lastPg.setAttribute("class", "hard");
    var b1 = document.createElement("br");
    var b2 = document.createElement("br");
    lastPg.appendChild(b1);
    lastPg.appendChild(b2);
    var par = document.createElement("p");
    par.setAttribute("id", "last");
    par.innerHTML = "Copyright &copy; 2015, Victor Lai";
    lastPg.appendChild(par);
    flipbook[0].appendChild(lastPg);

    /* Load day entries from database (take data from hidden input field) */
    var str = document.getElementById("datesToEntries");
    var data = str.value;
    var rows = data.split(";");

    for (var i = 0; i < rows.length - 1; i++) {
        var cols = rows[i].split("|");
        var entryDate = document.getElementById(cols[0]);
        if (entryDate == null)
            continue;
        else {
            if (entryDate.innerHTML == "")
                entryDate.innerHTML = cols[1];
            else 
                entryDate.innerHTML += "<br>" + cols[1];
        }
    }

    /* Load week entries from database (take data from hidden input field) */
    var weekEntrs = document.getElementById("weeksToEntries").value;
    var weekRows = weekEntrs.split(";");

    for (var i = 0; i < weekRows.length - 1; i++) {
        var cols = weekRows[i].split("|");
        var entryDate = document.getElementById(cols[0]);
        if (entryDate == null)
            continue;
        else {
            if (entryDate.innerHTML == "")
                entryDate.innerHTML = cols[1];
            else
                entryDate.innerHTML += "<br>" + cols[1];
        }
    }

    loadBook(startingPg);
}

function indexOfDay(day) {
    switch (day) {
        case "SUN": return 0;
        case "MON": return 1;
        case "TUES": return 2;
        case "WED": return 3;
        case "THURS": return 4;
        case "FRI": return 5;
        case "SAT": return 6;
        default: return -1;
    }
}

function indexOfMonth(month) {
    switch (month) {
        case "Jan": return 0;
        case "Feb": return 1;
        case "Mar": return 2;
        case "Apr": return 3;
        case "May": return 4;
        case "Jun": return 5;
        case "Jul": return 6;
        case "Aug": return 7;
        case "Sep": return 8;
        case "Oct": return 9;
        case "Nov": return 10;
        case "Dec": return 11;
        default: return -1;
    }
}

function isLeftPage(day) {
    if (day === 1 || day === 2 || day === 3)
        return true;
    else
        return false;
}

function incrementYear(year) {
    var newYear = year;
    newYear++;
    return newYear;
}

function incrementMonth(month) {
    return (month + 1) % 12;
}

function incrementDay(day) {
    return (day + 1) % 7;
}

function incrementDate(date, month, year) {
    var maxDays = nDaysInMonth(month, year);
    var newDate = date;

    if (date === maxDays) {
        return 1;
    }
    else {
        newDate++;
        return newDate;
    }
}

function nDaysInMonth(month, year) {
    // Refer to 
    // http://www.dispersiondesign.com/articles/time/number_of_days_in_a_month
    // for more info.
    var daysInMonth;

    if (month === 1) { // month is Feb
        daysInMonth = 28 + isLeapYear(year);
    }
    else {
        daysInMonth = 31 - month % 7 % 2;
    }
    return daysInMonth;
}

// Returns 1 if 'year' is a leap year; otherwise, 0
function isLeapYear(year) {
    // Refer to 
    // http://www.dispersiondesign.com/articles/time/determining_leap_years
    // for more info.
    if ((year % 4 !== 0) || ((year % 100 === 0) && (year % 400 !== 0))) {
        return 0;
    }
    else {
        return 1;
    }
}

// Jump to the page containing the specified date entered in 'jumpIn' input
function jump() {
    var date = document.getElementById("jumpIn").value; // Format: yyyy-mm-dd
    var elems = date.split("-");

    // Convert to mmddyyyy format
    var dateStr = "";
    if (elems[1][0] == "0")
        dateStr += parseInt(elems[1][1]) - 1;
    else
        dateStr += parseInt(elems[1]) - 1;
    
    if (elems[2][0] == "0")
        dateStr += elems[2][1];
    else
        dateStr += elems[2];

    dateStr += elems[0];

    if (dateStr in datesToPages) {
        var pg = datesToPages[dateStr];
        $('.flipbook').turn('page', pg);
    }
    else {
        // Print error some place
    }
    document.getElementById("jumpIn").value = "";
}

// Jump to the page containing today's date
function jumpToday() {
    if (startingPg !== 1)
        $('.flipbook').turn('page', startingPg);
}

// Parameter 'pgNum' specifies the page of the book that should be opened to.
function loadBook(pgNum) {

    // Create the flipbook
    $('.flipbook').turn({
        // Width
        width: 1100,

        // Height
        height: 800,

        // Elevation
        elevation: 50,

        // Starting page
        page: pgNum,

        // Enable gradients
        gradients: true,

        // Auto center this flipbook
        autoCenter: false,

        when: {
            turning: function (event, page, view) {
                var sidebar = document.getElementById("sidebar");
                if (sidebar.getElementsByTagName("textarea").length == 0) {
                    // Update the current URI
                    Hash.go('page/' + page).update();
                }
                else {
                    // Prevent turning the page
                    event.preventDefault();
                    // Show error message
                    $("#hover").show();
                    $("#Error_Page_Flip").show();
                    $('#Error_Page_Flip').delay(2000).fadeOut('slow');
                    $('#hover').delay(2000).fadeOut('slow');
                }
            }
        }
    });

    // URIs - Format #/page/1 
    Hash.on('^page\/([0-9]*)$', {
        yep: function (path, parts) {
            var page = parts[1];

            if (page !== undefined) {
                if ($('.flipbook').turn('is'))
                    $('.flipbook').turn('page', page);
            }

        }
    });

    // Using arrow keys to turn the page
    $(document).keydown(function (e) {

        var previous = 37, next = 39;
        if ((e.target.nodeName == 'TEXTAREA' || e.target.id == 'jumpIn') &&
            (e.keyCode == previous || e.keyCode == next))
            return;
         
        switch (e.keyCode) {
            case previous:
                // left arrow
                $('.flipbook').turn('previous');
                e.preventDefault();
                break;
            case next:
                // right arrow
                $('.flipbook').turn('next');
                e.preventDefault();
                break;
        }
    });
}

// Refer to http://yepnopejs.com/ for more info.
yepnope({
    test: Modernizr.csstransforms,
    yep: ['lib/turn.js'],
    nope: ['lib/turn.html4.min.js'],
    both: ['CSS/book.css'],
    complete: loadApp
});
