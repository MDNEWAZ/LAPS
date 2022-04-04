$(document).ready(function() {
    //$('#blueTheme').onmouseover(function () {
    //    $('#blueTheme').css('width', '10px');
    //});
});





var ThemeManager = {
    IconWith:function () {
        //$('#blueTheme').css('height', '30px');
    },
// { text: "Default", value: "default" },
//{ text: "Green", value: "metro" },
//{ text: "Gray", value: "gray" },
//{ text: "Blue", value: "blueopal" },
//{ text: "Contrast", value: "highcontrast" },
//{ text: "Metro Black", value: "metroblack" },
//{ text: "Silver", value: "silver" },
//{ text: "Yellow", value: "yellow" },
//{ text: "Orange", value: "orange" },
//{ text: "Black", value: "black" }
    blueTheme: function() {
       
        themeHelper.themeUpdateForUser('blueopal');
      

    },
    greenTheme: function () {

        themeHelper.themeUpdateForUser('metro');//metro means Green theme

    },
    contrastTheme: function () {

        themeHelper.themeUpdateForUser('highcontrast');
    },
    metroblackTheme: function () {

        themeHelper.themeUpdateForUser('metroblack'); 

    },
    silverTheme: function () {

        themeHelper.themeUpdateForUser('silver');

    },
    orangeTheme: function () {

        themeHelper.themeUpdateForUser('orange'); 
    },
   
    blankTheme: function () {

        themeHelper.themeUpdateForUser('default');

    },
    
    themesPanelBar:function () {
        //$('#themeSlider').removeClass('displayNone').toggle(2000);
        //$('#themesPanelBar').addClass('displayNone');
        
    }
    
};
var themeHelper = {
    
    themeUpdateForUser: function (themeName) {
       
        {
            var serviceUrL = '../Users/UpdateTheme';
            var strthemeName = "themeName=" + themeName;
            AjaxManager.SendJson(serviceUrL, strthemeName, onSuccess, onFailed);
        }
       

        function onSuccess(jsonData) {
            if (jsonData == "Success") {
                window.location.reload();
            } else {

                AjaxManager.MsgBox('error', 'center', 'Error', "Theme not found" ,
                    [{
                        addClass: 'btn btn-primary',
                        text: 'Ok',
                        onClick: function ($noty) {
                            $noty.close();
                        }
                    }]);
            }
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

    }
};

