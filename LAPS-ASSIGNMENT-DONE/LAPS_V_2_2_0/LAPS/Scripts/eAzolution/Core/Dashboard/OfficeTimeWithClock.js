
var OfficeTimeWithClockManager = {
    startServerClock: function () {
        var jsonParam = "";
        var url = "../Dashboard/GetServerTime";
        AjaxManager.SendJson(url, jsonParam, onSuccess, onFailed);
        function onSuccess(jsonData) {
            var dt = jsonData;
            OfficeTimeWithClockHelper.startNewClock(dt);
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
    },
    
    getOfficeTime: function () {
        var jsonParam = "";
        var url = "../Dashboard/GetOfficeTime";

        AjaxManager.SendJson(url, jsonParam, onSuccess, onFailed);
        function onSuccess(jsonData) {
            var officeTimeDetails = "Office Time: DAY OFF";
            if (jsonData != "") {
                var officeTime = jsonData.split('^');
                var fromTime = officeTime[0].split('.');
                var toTime = officeTime[1].split('.');
                officeTimeDetails = "Office Time: " + fromTime[0] + " - " + toTime[0];

            }
            $("#lblOfficeTime").html(officeTimeDetails);
        }
        function onFailed(error) {
            window.alert(error.statusText);
        }
    },

};
var prevmin = 0;
var prevsec = 0;
var prevhrs = 0;

var OfficeTimeWithClockHelper = {
    stopAutoRefresh: function () {
        self.clearInterval(timerID);
        timerID = null;
    },
    
    startNewClock: function (dt) {

        var today = new Date(dt);
        var h = 0;
        var m = 0;
        var s = 0;
        if (prevmin == 0 && prevsec == 0 && prevhrs == 0) {
            h = today.getHours();
            m = today.getMinutes();
            s = today.getSeconds();
            // add a zero in front of numbers<10
            m = OfficeTimeWithClockHelper.checkTime(m);
            s = OfficeTimeWithClockHelper.checkTime(s);
        }
        else {
            prevsec++;
            s = prevsec;
            m = prevmin;
            h = prevhrs;
            if (s == 60) {
                prevmin++;
                m = prevmin;
                s = 0;
            }
            if (m == 60) {
                prevhrs++;
                h = prevhrs;
                m = 0;
            }
            m = OfficeTimeWithClockHelper.checkTime(m);
            s = OfficeTimeWithClockHelper.checkTime(s);


        }
        $("#lblServerTime").html("My Time: " + h + ":" + m + ":" + s);
        prevsec = s;
        prevmin = parseInt(m);
        prevhrs = parseInt(h);
        time = setTimeout('OfficeTimeWithClockHelper.startNewClock()', 1000);
    },
    
    checkTime: function (i) {
        if (i < 10) {
            i = "0" + i;
        }
        return i;
    }

};