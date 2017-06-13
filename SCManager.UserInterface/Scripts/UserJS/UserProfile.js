$(document).ready(function () {
   
});


function Save()
{
    try
    {
       
        $('#btnSaveUserprofile').trigger('click');
    }
    catch(e)
    {
        notyAlert('error', e.message);
    }
}

function validate()
{
    var fl = false;
    var cp = $("#CurrentPassword").val();
    var np=$("#NewPassword").val();
    var cfp = $("#ConfirmPassword").val();
    if(cp)
    {
       
        if((np)&&(cfp))
        {
            fl = true;
        }
        else
        {
            f1 = false;
            notyAlert('error', 'New Password and Confirm password required');
        }
        
    }
    else
    {
        if ((np) && (cfp))
        {
            fl = false;
            notyAlert('error', 'Current password required');
        }
        else
        {
            fl = true;
        }
        
    }
    return fl;
}

function userProfileSaveSuccess(data, status, xhr)
{
    try
    {
        var i = JSON.parse(data)
        switch (i.Result) {
            case "OK":
                $("#CurrentPassword").val('');
                $("#NewPassword").val('');
                $("#ConfirmPassword").val('');
                notyAlert('success', i.Message);
                break;
           case "ERROR":
                notyAlert('error', i.Message);
              
                break;
            default:
                break;
        }
    }
    catch(e)
    {
        notyAlert('error', e.message);
    }
}