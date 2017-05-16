var appAddress = window.location.protocol + "//" + window.location.host + "/";   //Retrieving browser Url 

//(function Checker() {
//    var flag = false;
//    $.ajax({
//        url: appAddress + 'Account/AreyouAlive/',
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (data) {

//            if (data.Result == "OK") {
//                switch (data.Record) {
//                    case "dead":
//                        $('.modal').modal('hide');
//                        $("#RedirectToLoginModel").modal('show');
//                        //notyAlert('success', 'dead df');
//                        flag = true;
//                        break;
//                    case "alive":
//                        flag = false;
//                        //notyAlert('success', 'alive df');
//                        break;
//                }


//            }
//            if (data.Result == "ERROR") {
//                notyAlert('error', data.Message);
//            }

//        },
//        complete: function () {
//            // Schedule the next request when the current one's complete
//            //  setTimeout(Checker, 126000);
//            if (flag != true) {
//                //setTimeout(Checker, 3000);
//                setTimeout(Checker, 126000);
//            }

//        }
//    });
//})();


$(document).ready(function () {
  
    $('input[type="date"]').datepicker({
        format: "yyyy-mm-dd",//dd-M-yyyy",
        maxViewMode: 0,
        todayBtn: "linked",
        clearBtn: true,
        autoclose: true,
        todayHighlight: true
    });
    
    //menu submenu popup on click 3rd level menus
    $('.navbar a.dropdown-toggle').on('click', function (e) {
        var $el = $(this);
        var $parent = $(this).offsetParent(".dropdown-menu");
        $(this).parent("li").toggleClass('open');

        if (!$parent.parent().hasClass('nav')) {
            $el.next().css({ "top": $el[0].offsetTop, "left": $parent.outerWidth() - 4 });
        }

        $('.nav li.open').not($(this).parents("li")).removeClass("open");

        return false;
    });
   
    $(".dropdown, .btn-group").hover(function () {
        debugger;
        var dropdownMenu = $(this).children(".dropdown-menu");
        if (dropdownMenu.is(":visible")) {
            dropdownMenu.parent().toggleClass("open");
        }
    });

    //$('input').keydown(function (e) {
    //    var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;
    //    if (key == 13) {
    //        e.preventDefault();
    //        var inputs = $(this).closest('form').find(':input:visible');
    //        inputs.eq(inputs.index(this) + 1).focus();
    //    }
    //});
   
});

function notyAlert(type, msgtxt,title) {
    var t = '';
    if (title == undefined) {
        t = type;
    }
    else {
        t = title;
    }

    swal({ title: t, text: msgtxt, type: type, timer: 6000 });
    //var n = noty({
    //    text: msgtxt,
    //    type: type,//'alert','information','error','warning','notification','success'
    //    dismissQueue: true,
    //    timeout: 3000,
    //    layout: 'center',
    //    theme: 'defaultTheme',//closeWith: ['click'],
    //    maxVisible: 5
    //});
   
}
function ConvertDateFormats(thisdate) {
    try {
        if (thisdate != null) {
            thisdate = thisdate.replace(/-/g, ' ');
            var converteddate = new Date(thisdate);
            result = ('0' + (converteddate.getMonth() + 1)).slice(-2) + ' ' + ('0' + converteddate.getDate()).slice(-2) + ' '
             + converteddate.getFullYear();
            //var result = converteddate.getDate() + '-' + (converteddate.getMonth()+1) + '-' + converteddate.getFullYear();
            return result;
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
function PostDataToServer(page, formData, callback)
{
   $.ajax({
        type: "POST",
        url: appAddress+page,
        async: true,
        data: formData,
        beforeSend: function () {
            showLoader();
        },
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            callback(data);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            notyAlert('error', errorThrown + ',' + textStatus + ',' + jqXHR.statusText);
        },
        complete:function()
        {
            hideLoader();
        }

    });
    
}


function GetDataFromServer(page, formData) {
    var jsonResult = {};
    $.ajax({
        
        type: "GET",
        url: appAddress + page,
        data: formData,
        beforeSend: function () {
            showLoader();
        },
        async: false,
        cache: false,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
         jsonResult = data;
        },
        error: function (jqXHR, textStatus, errorThrown) {
          notyAlert('error',errorThrown + ',' + textStatus + ',' + jqXHR.statusText);
        },
        complete: function () {
            hideLoader();
        }

    });
    return jsonResult;
}
function ChangeButtonPatchView(Controller,Dom, Action) {
    var data = { ActionType: Action };
    var ds = {};
    ds = GetDataFromServer(Controller + "/ChangeButtonStyle/", data);
    if (ds == "Nochange")
    {
        return;
    }
    $("#" + Dom).empty();
    $("#" + Dom).html(ds);
}

function NetworkFailure(data, status, xhr) {
    var i = JSON.parse(data)
    notyAlert('error', status);
}


//Common function for clearing input fields
function ClearFields() {
    $(':input').each(function () {

        if (this.type == 'text' || this.type == 'textarea' || this.type == 'file'|| this.type == 'search') {
            this.value = '';
        }
        else if (this.type == 'checkbox') {
            this.checked = false;
        }
        else if (this.type == 'select-one' || this.type == 'select-multiple') {
            this.value = '-1';
        }
    });

}

//------Date Formating :Return Result Eg: 01-Jan-2017--------------------//
//Passing value
//Argument Eg:"2017-03-30T00:00:00"
//Returns 30-03-2017
function ConvertJsonToDate(jsonDate) {
    try
    {
        if (jsonDate != null) {
            var currentTime = new Date(jsonDate.substr(0, 10));
            var monthNames = ["Jan", "Feb", "Mar","Apr", "May", "Jun", "Jul","Aug", "Sep", "Oct","Nov", "Dec"];
            var result = currentTime.getDate() + '-' + monthNames[currentTime.getMonth()] + '-' + currentTime.getFullYear();
            return result;
        }
    }
    catch(e)
    {
        notyAlert('error', e.message);
    }
   
}
//only number validation
function isNumber(e) {
    var unicode = e.charCode ? e.charCode : e.keyCode
    if (unicode != 8) { //if the key isn't the backspace key (which we should allow)
        if (unicode < 48 || unicode > 57) //if not a number
            return false //disable key press
    }
}


function notyConfirm(msg, functionIfSuccess,msg2,btnText) {
    var m = 'You will not be able to recover this action!'
    if (msg2 != undefined) {
        m = msg2 + '  ' + m;
    }
    if (btnText == undefined)
    {
        btnText = "Yes, delete it!";
    }
    swal({
        title: msg,
        text: m,
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: btnText,
        closeOnConfirm: false
    },
function () {
    //swal("Deleted!", "Your imaginary file has been deleted.", "success");
    eval(functionIfSuccess );
});


    //var text = '<div class="confirmbox"><span class="confirmboxHead">Delete Alert !</span><br/><br/><span class="confirmboxMsg">' + msg + '</span><br/><br/><span class="confirmboxFooter">You cannot reverse this action</span><div>'
    //var n = noty({
    //    text: text,
    //    type: 'confirm',
    //    dismissQueue: false,
    //    layout: 'center',
    //    modal: true,
    //    theme: 'defaultTheme',
    //    buttons: [
    //        {
    //            addClass: 'btn btn-primary', text: '&nbsp&nbsp;&nbsp;&nbsp;Ok&nbsp;&nbsp;&nbsp;&nbsp', onClick: function ($noty) {
    //                $noty.close();
    //                eval(functionIfSuccess + '()');

    //            }
    //        },
    //    {
    //        addClass: 'btn btn-danger', text: 'Cancel', onClick: function ($noty) {
    //            $noty.close();
    //            return false;
    //        }
    //    }
    //    ]
    //})

}


function Logout() {
    window.location = appAddress;
}

var loadStatus = 0;
 
function showLoader() {
    try {
        $(".preloader").show();
    } catch (e) {

    }
   
   
}

 
function hideLoader() {
    try {
        $('.preloader').fadeOut();
    } catch (e) {

    }
   
   
}


function roundoff(num) {
    return (Math.round(num * 100) / 100).toFixed(2);
}
function roundoff(num, opt) {
    if (num == '' && opt != undefined) { return ''; }
    return (Math.round(num * 100) / 100).toFixed(2);
}
