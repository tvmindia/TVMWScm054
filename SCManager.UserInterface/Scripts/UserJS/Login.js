var appAddress = window.location.protocol + "//" + window.location.host + "/";   //Retrieving browser Url 
function LoginSuccess(data, status, xhr) {
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            if (JsonResult.Record != "false") {
                window.location = appAddress + "Dashboard";
            }
            else {
                $('.card').append('<span class="logfailed">Login Failed</span>');

            }


            break;
        case "ERROR":

            break;
        default:

            break;
    }

}

function GetDataFromServer(page, formData) {
    debugger;
    var jsonResult = {};
    $.ajax({

        type: "GET",
        url: appAddress + page,
        data: formData,
        
        async: false,
        cache: false,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            jsonResult = data;
        },
        error: function (jqXHR, textStatus, errorThrown) {
            debugger;
           // notyAlert('error', errorThrown + ',' + textStatus + ',' + jqXHR.statusText);
        },
        

    });
    return jsonResult;
}

function EmailValidation() {
    debugger;
    var Email = $('#txtEmail').val();
    var ptag = document.getElementById('lblerror');
    var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    if (emailReg.test(Email)) {
        //if (Email.match(/@/)) {
        var data = { "EmailID": Email };
        var ds = {};      
    

        ds = GetDataFromServer("Account/EmailValidation/", data)
       
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Message.Status === "1") {

            ptag.style.color = 'green';
            ptag.style.fontFamily = 'monaco';
            ptag.style.paddingLeft = "7px";
            ptag.innerHTML = "Email is Valid Continue";
            $('#Sendinggif').hide();
            return false;
        }
        if (ds.Message.Status == "0") {

            ptag.style.color = 'red';
            ptag.style.fontFamily = 'monaco';
            ptag.style.paddingLeft = "7px";
            ptag.innerHTML = "Email is Invalid";

            return false;
        }
    }
    else {
        ptag.innerHTML = " ";
        $('#Sendinggif').show();
    }
    return false;
}

function UpdatePassword() {

    var UsrID = $('#HdnUserID').val();
    var Passwd = $('#txtPassword').val();
    var CPasswd = $('#txtConfirmPassword').val();

    if (Passwd === CPasswd) {

        var ds = {};
          
        var data = {"ID":UsrID,"password":CPasswd};
        ds = GetDataFromServer("Account/UpdatePassword/", data);
        debugger;

        if (ds != '') {
            ds = JSON.parse(ds);
        }
       

        if (ds.Message.Status == "1") {
            $('#NewPassword').remove();
            Succes();
        }
        else {
            var ptag = document.getElementById('lblerror');
            ptag.style.color = 'red';
            ptag.style.fontFamily = 'monaco';
            ptag.style.paddingLeft = "5px";
            ptag.innerHTML = table.d;
        }
    }
    else {
        var ptag = document.getElementById('lblerror');
        ptag.style.color = 'red';
        ptag.style.fontFamily = 'monaco';
        ptag.style.paddingLeft = "5px";
        ptag.innerHTML = 'Passwords MissMatch';
    }
    setTimeout(function () {
        window.location.reload();
    }, 20000);

}

function Succes() {
    var LoginDIv = $('#ForgorPassDiv');
    var html = ('<div class="content" id="ChangedPassword">'
        + '<h2>Password Changed Successfully..</h2>'
        + '<div class="clearfix"></div>'
        + '<h3></h3><p><a onclick="ReloadWindow();" style="color:blue;cursor:pointer;">Click Here</a> to Login</p></div');
    LoginDIv.append(html);
}
function ReloadWindow()
{
    window.location.reload();
}

function ForgotPassword() {
    debugger;
    $('#LoginBoxDiv').hide();
    $("#ForgorPassDiv").show();
    var LoginDIv = $("#ForgorPassDiv");
   // $('#loginRowFluid').removeClass("content");
    var html = ('<div class="content" id="EmailBox">'
        + '<h2>Enter your Email</h2>'
        + '<div class="" title="Email">'
        + '<input class="form-control form-control-solid placeholder-no-fix" name="Email" id="txtEmail" type="Email" onkeyup="return EmailValidation();"  autocomplete="off" placeholder="Email"/>'
        + '</div>'
        + '<div><img src="../Content/images/ring.gif" style="border:0;margin-left:100px;;max-width:24%;height:auto;vertical-align:middle;display:none;" id="Sendinggif"></div>'
        + '<div class="button-login">'
        + '<a href="#" onclick="SendVerificationCode();" class="btn btn-primary loginbtn">Continue</a>'
        + '</div>'
        + '<div class="clearfix"></div>'
        + '<h3></h3><p id="lblerror"></p></div');
    LoginDIv.append(html);

}

function SendVerificationCode() {
    debugger;
    $('#Sendinggif').show();
    var Email = $('#txtEmail').val();
    var ds = {};
    var table = {};
    var data = { "Email": Email };
    ds = GetDataFromServer("Account/VerificationCodeEmit/", data);
    debugger;

    if (ds != '') {
        ds = JSON.parse(ds);
    }
   // table = JSON.parse(ds.Message);
    if (ds.Message == "true") {
        $('#EmailBox').remove();
        MatchVetification(Email);
    }
    if (ds.Message == "Error")
    {
        var ptag = document.getElementById('lblerror');
        ptag.style.color = 'red';
        ptag.style.fontFamily = 'monaco';
        ptag.style.paddingLeft = "5px";
        ptag.innerHTML = 'Mail Sending Failed.Try Again !';
    }
    if (ds.Message == "false") {
        var ptag = document.getElementById('lblerror');
        ptag.style.color = 'red';
        ptag.style.fontFamily = 'monaco';
        ptag.style.paddingLeft = "5px";
        ptag.innerHTML = 'The Email You Entered Is  InValid !';
    }
    return ds.Message;
}
function MatchVetification(EmailAddr) {
    var HdnMail = document.createElement('input');
    HdnMail.setAttribute("type", "hidden");
    HdnMail.setAttribute("id", "HdnEmail");
    var LoginDIv = $('#ForgorPassDiv');
    var html = ('<div class="content" id="VerifyBox">'
        + '<h2>Enter Verification Code</h2>'
        + '<div class="" title="Verification">'
        + '<input class="form-control form-control-solid placeholder-no-fix" name="VerificationCode" id="txtVerifyCode" type="password" autocomplete="off" placeholder="Verification Code"/>'
        + '</div><div style="font-family:monaco;padding-left:10px;font-size:14px;color:rosybrown"> ✉ CHECK Email For Verification Code</div>'
        + '<div class="button-login">'
        + '<button type="" id="btnlogin" onclick="VerifyCodeNow()" class="btn btn-primary loginbtn">Verify</button>'
        + '</div>'
        + '<div class="clearfix"></div>'
        + '<h3></h3><p id="lblerror"></p></div');
    LoginDIv.append(html);
    LoginDIv.append(HdnMail);
    $('#HdnEmail').val(EmailAddr);
}

function VerifyCodeNow() {
    debugger;
    var Email = $('#HdnEmail').val();
    var VerifCode = $('#txtVerifyCode');
    var VerificationCod = VerifCode[0].value;
    var ds = {};
    var table = {};
    var data = { "Email": Email ,"verificationCode":VerificationCod};
    ds = GetDataFromServer("Account/VerifyCode/", data);
    debugger;

    if (ds != '') {
        ds = JSON.parse(ds);
    }
   
    if (ds.Message.Message === "True") {
        $('#VerifyBox').remove();
        EnterPassword(ds.Message.ID);
    }
    if (ds.Message.Message === "False") {
        var ptag = document.getElementById('lblerror');
        ptag.style.color = 'red';
        ptag.style.fontFamily = 'monaco';
        ptag.style.paddingLeft = "5px";
        ptag.innerHTML = 'The Verification Code Missmatch !';
    }
    else {
        if (ds.Message.Message !== 'False' && ds.Message.Message !== 'True') {
            var ptag = document.getElementById('lblerror');
            ptag.style.color = 'red';
            ptag.style.fontFamily = 'monaco';
            ptag.style.paddingLeft = "5px";
            ptag.innerHTML = ds.Message.Message + '!';
        }

    }
}
function EnterPassword(UsrID) {

    var HdnUserID = document.createElement('input');
    HdnUserID.setAttribute("type", "hidden");
    HdnUserID.setAttribute("id", "HdnUserID");
    var LoginDIv = $('#ForgorPassDiv');
    var html = ('<div class="content" id="NewPassword">'
        + '<h2>Enter New Password</h2>'
        + '<div class="" title="NewPassword">'
        + '<input class="form-control form-control-solid placeholder-no-fix" name="NPAss" id="txtPassword" type="password" autocomplete="off" placeholder="New Password"/>'
        + '</div>'
        + '<div class="" title="NewPassword">'
        + '<input class="form-control form-control-solid placeholder-no-fix" name="CPass" id="txtConfirmPassword" type="password" autocomplete="off" placeholder="Confirm Password"/>'
        + '</div>'
        + '<div class="button-login">'
        + '<button type="submit" id="btnlogin" onclick="UpdatePassword()" class="btn btn-primary loginbtn">Change Password</button>'
        + '</div>'
        + '<div class="clearfix"></div>'
        + '<h3></h3><p id="lblerror"></p></div');
    LoginDIv.append(html);
    LoginDIv.append(HdnUserID);
    $('#HdnUserID').val(UsrID);
}