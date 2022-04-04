var notificationManager = {
    //Count//
    GetNotiCount: function () {
        var notificationCount = "";
        var jsonParam = "";
        var serviceUrl = "../Notification/NoticeCountForNotification/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            notificationCount = jsonData;
        }
        function onFailed(error) {
        }
        return notificationCount;
    },

    GetNotiSMSSentCount: function () {
        var smsSentNotificationCount = "";
        var jsonParam = "";
        var serviceUrl = "../Notification/SmsSentNoticeCountForNotification/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            smsSentNotificationCount = jsonData;
        }
        function onFailed(error) {
        }
        return smsSentNotificationCount;
    },

    GetNotiSMSFailCount: function () {
        var smsFailNotificationCount = "";
        var jsonParam = "";
        var serviceUrl = "../Notification/SmsFailNoticeCountForNotification/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            smsFailNotificationCount = jsonData;
        }
        function onFailed(error) {
        }
        return smsFailNotificationCount;
    },

    GetUnRecognizeCollSmsNoticeCount: function () {
        var smsFailNotificationCount = "";
        var jsonParam = "";
        var serviceUrl = "../Notification/UnRecognizeCollSmsNoticeCountForNotification/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);

        function onSuccess(jsonData) {
            smsFailNotificationCount = jsonData;
        }

        function onFailed(error) {
        }

        return smsFailNotificationCount;
    },

    GetUnRecognizeSaleNoticeCount: function () {
        var smsFailNotificationCount = "";
        var jsonParam = "";
        var serviceUrl = "../Notification/UnRecognizeSaleNoticeCountForNotification/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            smsFailNotificationCount = jsonData;
        }
        function onFailed(error) {
        }
        return smsFailNotificationCount;
    },

    GetUnRecognizeSmsNoticeCount: function () {
        var smsFailNotificationCount = "";
        var jsonParam = "";
        var serviceUrl = "../Notification/UnRecognizeSmsNoticeCountForNotification/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            smsFailNotificationCount = jsonData;
        }
        function onFailed(error) {
        }
        return smsFailNotificationCount;
    },

    //Count End//


    countTotalUnreadMsg: function () {
        var msgCount = 0;
        var jsonParam = "";
        var serviceUrl = "../Message/GetTotalUnreadMessage/";
        AjaxManager.SendJson(serviceUrl, jsonParam, onSuccess, onFailed);
        function onSuccess(jsonData) {
            msgCount = jsonData;
        }
        function onFailed(error) {
        }
        return msgCount;
    },

    //GET Noti data
    GetNotificationData: function () {
        var notificationData = "";
        var jsonParam = "";
        var serviceUrl = "../Notification/NoticeForNotification/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            notificationData = jsonData;
        }
        function onFailed(error) {
        }
        return notificationData;
    },

    GetSMSSentNotificationData: function () {
        var notificationData = "";
        var jsonParam = "";
        var serviceUrl = "../Notification/SmsSentNoticeForNotification/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            notificationData = jsonData;
        }
        function onFailed(error) {
        }
        return notificationData;
    },

    GetSMSFailNotificationData: function () {
        var notificationData = "";
        var jsonParam = "";
        var serviceUrl = "../Notification/SmsFailNoticeForNotification/";
        AjaxManager.GetJsonResult(serviceUrl, jsonParam, false, false, onSuccess, onFailed);
        function onSuccess(jsonData) {
            notificationData = jsonData;
        }
        function onFailed(error) {
        }
        return notificationData;
    },

    //End Noti Data


    markAsReadByNotificationId: function (smsNotificationId) {
        var jsonParam = 'smsNotificationId=' + smsNotificationId;
        var serviceUrl = "../Notification/MarkAsReadBySmsNotificationId/";
        AjaxManager.SendJson(serviceUrl, jsonParam, onSuccess, onFailed);
        function onSuccess(jsonData) {//I return Success or Error for further use
  
            notificationHelper.SMSSentNotificationCount();
            notificationHelper.loadSMSSentNotifications();
            notificationHelper.SMSFailNotificationCount();
            notificationHelper.loadSMSFailNotifications();
            //notificationHelper.UnRecognizeNotificationCount();
            //notificationHelper.loadUnRecognizedNotifications();
        }
        function onFailed(error) {
            AjaxManager.MsgBox('error', 'center', 'Error', error.statusText,
                [{
                    addClass: 'btn btn-primary',
                    text: 'Ok',
                    onClick: function ($noty) {
                        $noty.close();
                    }
                }]);
        }
    },
};

var notificationHelper = {
    notificationInit: function () {
        notificationHelper.UnRecognizeNotificationCount();
        notificationHelper.SMSSentNotificationCount();
        notificationHelper.SMSFailNotificationCount();

       // notificationHelper.msgCount();
        notificationHelper.friendCount();


        notificationHelper.notiHover();
        notificationHelper.notiClose();
    },

    UnRecognizeNotificationCount: function () {
        var notiUnCollSms = notificationManager.GetUnRecognizeCollSmsNoticeCount();
        var notiUnSale = notificationManager.GetUnRecognizeSaleNoticeCount();
        var notiUnSms = notificationManager.GetUnRecognizeSmsNoticeCount();
        var totalUnRecognize = notiUnCollSms + notiUnSale + notiUnSms;
        if (totalUnRecognize != 0) {
            $("#noti_Container_bubble_UnRecognize").html(totalUnRecognize);

        } else {
            $("#noti_Container_bubble_UnRecognize").hide();

        }
        // messageInformationManager.countTotalUnreadMsg();
    },

    SMSSentNotificationCount: function () {
        var notiCount = notificationManager.GetNotiSMSSentCount();
        if (notiCount != 0) {
            $("#noti_Container_bubble_SMSSent").html(notiCount);

        } else {
            $("#noti_Container_bubble_SMSSent").hide();

        }
        // messageInformationManager.countTotalUnreadMsg();
    },

    SMSFailNotificationCount: function () {
        var notiCount = notificationManager.GetNotiSMSFailCount();
        if (notiCount != 0) {
            $("#noti_Container_bubble_SMSFail").html(notiCount);

        } else {
            $("#noti_Container_bubble_SMSFail").hide();

        }
        // messageInformationManager.countTotalUnreadMsg();
    },
    msgCount: function () {

        var msgCount = notificationManager.countTotalUnreadMsg();
        if (msgCount != 0) {
            $("#msg_Container_bubble_UnRecognize").html(msgCount);

        } else {
            $("#msg_Container_bubble_UnRecognize").hide();
            $("#friend_Container_bubble").hide();
        }
    },
    friendCount: function () {

        var friendCount = 0;
        if (friendCount != 0) {
            $("#friend_Container_bubble").html(friendCount);
        } else {
            $("#friend_Container_bubble").hide();
        }
    },

    notiHover: function () {

        $("#noti_Container_main_UnRecognize").click(function () {
            notificationHelper.loadUnRecognizedNotifications();
            $(this).toggleClass("open");
            $(".notifications").removeClass("open");
            $("#notificationMenu_UnRecognize").toggleClass("open");

        });

        $("#noti_Container_main_SMSSent").click(function () {
            notificationHelper.loadSMSSentNotifications();
            $(this).toggleClass("open");
            $(".notifications").removeClass("open");
            $("#notificationMenu_SMSSent").toggleClass("open");

        });

        $("#noti_Container_main_SMSFail").click(function () {
            notificationHelper.loadSMSFailNotifications();
            $(this).toggleClass("open");
            $(".notifications").removeClass("open");
            $("#notificationMenu_SMSFail").toggleClass("open");

        });
    },

    notiClose: function () {

        $("#SmsSentNotiClose").click(function () {
            $(".notifications").removeClass("open");
        });
        $("#SmsFailNotiClose").click(function () {
            $(".notifications").removeClass("open");
        });
        $("#SmsUnRecognizeClose").click(function () {
            $(".notifications").removeClass("open");
        });
    },

    loadUnRecognizedNotifications: function () {
       
        var notiUnCollSms = notificationManager.GetUnRecognizeCollSmsNoticeCount();
        var notiUnSale = notificationManager.GetUnRecognizeSaleNoticeCount();
        var notiUnSms = notificationManager.GetUnRecognizeSmsNoticeCount();
        var customHtml = "";

        var currentdate = new Date();
        var newdate = new Date();

        newdate.setDate(newdate.getDate() - 5);

        var dd = newdate.getDate();
        var mm = newdate.getMonth() + 1;
        var y = newdate.getFullYear();

        var someFormattedDate = mm + '/' + dd + '/' + y;

        var dateFiveDaysAgo = someFormattedDate;

        if (notiUnCollSms >= 0) {
            var unRecogCollSmsTitle = "Unrecognized Collection SMS";
            var detailsUnRecogCollSms = notiUnCollSms;
            var dateFrom = kendo.toString(dateFiveDaysAgo, 'dd/MM/yyyy');
            var dateTo = kendo.toString(currentdate, 'dd/MM/yyyy');
            customHtml += '<li class="notif"> <div class="messageblock"><div class="message">' + unRecogCollSmsTitle + '<br/><span class="NotificationDetails">' + "<b>" + detailsUnRecogCollSms + "</b>" + '</span></div> <div class="messageaction"> <a onclick="notificationHelper.clickEventForNotification(' + 1 + ')" class="button tiny alert">Show</a> </div><div class="messageinfo">' + '</div></div></a> </li>';
        }
        if (notiUnSale >= 0) {
            var unRecogSaleTitle = "Unrecognized Sale";
            var detailsUnRecogSale = notiUnSale;
            var saledateFrom = kendo.toString(dateFiveDaysAgo, 'dd/MM/yyyy');
            var saledateTo = kendo.toString(currentdate, 'dd/MM/yyyy');
            customHtml += '<li class="notif"> <div class="messageblock"><div class="message">' + unRecogSaleTitle + '<br/><span class="NotificationDetails">' + "<b>" + detailsUnRecogSale + "</b>" + '</span></div> <div class="messageaction"> <a onclick="notificationHelper.clickEventForNotification(' + 2 + ')" class="button tiny alert">Show</a> </div><div class="messageinfo">' + '</div></div></a> </li>';
        }
        if (notiUnSms >= 0) {
            var unRecogSmsTitle = "Unrecognized SMS";
            var detailsUnRecogSms = notiUnSms;
            var receivedateFrom = kendo.toString(dateFiveDaysAgo, 'dd/MM/yyyy');
            var receivedateTo = kendo.toString(currentdate, 'dd/MM/yyyy');
            customHtml += '<li class="notif"> <div class="messageblock"><div class="message">' + unRecogSmsTitle + '<br/><span class="NotificationDetails">' + "<b>" + detailsUnRecogSms + "</b>" + '</span></div> <div class="messageaction"> <a onclick="notificationHelper.clickEventForNotification(' + 3 + ')" class="button tiny alert">Show</a> </div><div class="messageinfo">' + '</div></div></a> </li>';
        }
       
        $("#notifications_UnRecognize").html(customHtml);
    },

    loadSMSSentNotifications: function () {
        var notificationData = notificationManager.GetSMSSentNotificationData();
        var customHtml = "";

        if (notificationData != null) {
            for (var i = 0; i < notificationData.length; i++) {
                var details = notificationData[i].SMSText.substring(0, 100);
                var deliveryDate = kendo.toString(kendo.parseDate(notificationData[i].DeliveryDateTime, 'dd/MM/yyyy'), 'dd/MM/yyyy');
                customHtml += '<li class="notif"><div class="messageblock"><div class="message">' + "MobileNo : " + notificationData[i].MobileNumber + '<br/><span class="NotificationDetails">' + details + '...</span></div><div class="messageaction"> <a onclick="notificationHelper.clickEventForMarkAsReadNotification(' + notificationData[i].SmsNotificationId + ')" class="button tiny alert">Mark As read</a> </div> <div class="messageaction"></div><div class="messageinfo">' + deliveryDate + '</div></div></a> </li>';

            }
        }
        $("#notifications_SMSSent").html(customHtml);
    },

    loadSMSFailNotifications: function () {
        var notificationDataForFailSms = notificationManager.GetSMSFailNotificationData();
        var customHtml = "";
  
        if (notificationDataForFailSms != null) {
            for (var i = 0; i < notificationDataForFailSms.length; i++) {
                var details = notificationDataForFailSms[i].SMSText.substring(0, 100);
                var deliveryDate = kendo.toString(kendo.parseDate(notificationDataForFailSms[i].DeliveryDateTime, 'dd/MM/yyyy'), 'dd/MM/yyyy');
                customHtml += '<li class="notif"><div class="messageblock"><div class="message">' + "MobileNo : " + notificationDataForFailSms[i].MobileNumber + '<br/><span class="NotificationDetails">' + details + '...</span></div><div class="messageaction"> <a onclick="notificationHelper.clickEventForMarkAsReadNotification(' + notificationDataForFailSms[i].SmsNotificationId + ')" class="button tiny alert">Mark As read</a> </div> <div class="messageaction"></div><div class="messageinfo">' + deliveryDate + '</div></div></a> </li>';

            }
        }

        $("#notifications_SMSFail").html(customHtml);
    },

    clickEventForNotification: function (notificationUnRecogType) {
    
        if (notificationUnRecogType == 1) {
            
          //  window.location.replace("../Sms/CollectionSms");
            window.open("../Sms/CollectionSms", '_blank');
        } else if (notificationUnRecogType == 2) {
            window.open("../Sale/UnrecognizedSale", '_blank');
        } else {
            window.open("../Sms/SaleRequestBySms", '_blank');
        }
       // notificationManager.markAsReadByNotificationId(notificationData);
    },
    clickEventForMarkAsReadNotification: function (smsNotificationId) {

        notificationManager.markAsReadByNotificationId(smsNotificationId);
    },


};