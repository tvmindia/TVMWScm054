var DataTables = {};
$(document).ready(function () {
    try
    {
        DataTables.UsersTable = $('#tblUsersList').DataTable(
      {
      dom: '<"pull-left"f>rt<"bottom"ip><"clear">',
      order: [],
      searching: true,
      paging: true,
      data: GetAllUsers(),
      columns: [
             { "data": "ID", "defaultContent": "<i>-</i>" },
             { "data": "serviceCenter", "defaultContent": "<i>-</i>" },
             { "data": "serviceCenter", "defaultContent": "<i>-</i>" },
             { "data": "UserName", "defaultContent": "<i>-</i>" },
             { "data": "Email", "defaultContent": "<i>-</i>" },
             { "data": "LoginName", "defaultContent": "<i>-</i>" },
             { "data": "RoleList", "defaultContent": "<i>-</i>" },
             { "data": "Active", "defaultContent": "<i>-</i>" },
             { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink" onclick="Edit(this)"><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
      ],
      columnDefs: [
          { "visible":false, "targets": [0],"searchable":false },
          { className: "text-left", "targets": [1, 2, 3, 4, 5, 6] },
          { className: "text-center", "targets": [7,8] },
          {
              "render": function (data, type, row) {
                  return data.Code
              },
              "targets": 1

          },
          {
              "render": function (data, type, row) {
                  return data.Description
              },
              "targets": 2

          },
          {
              "render": function (data, type, row) {
                 
                  return (data == true ? 'YES' : 'NO');
              },
              "targets": 7

          }
           

      ]
  });
        $('#tblUsersList tbody').on('dblclick', 'td', function () {
           
            Edit(this);
        });
    }
    catch(e)
    {
        notyAlert('error', e.message);
    }

});
function List()
{
    try {

        ChangeButtonPatchView('UserProfile', 'btnPatchBuildMember', 'Add');

    } catch (e) {
        notyAlert('error', e.message);
    }
}
function Add(id) {
    if (id != 1) {
        $('#AddTab').trigger('click');
    }
    clearfields();
    $("#userform").attr('data-ajax-begin', 'BeforePost');
 
    ChangeButtonPatchView('UserProfile', 'btnPatchBuildMember', 'Save');
}

function clearfields() {
    $("#ID").val('');
    $("#deleteId").val('');
    $("#DeleteSCCode").val('');
    $("#SCCode").val('');
    $("#UserName").val('');
    $("#Email").val('');
    $("#Password").val('');
    $("#ConfirmPassword").val('');
    $('input[name="Roles"]:checked').each(function () {
        $(this).prop('checked', false);
    });
    $("#LoginName").val('');
    $("#LoginName").removeAttr('readonly');
    $("#Active").prop('checked', false);
   
}
//function ResetForm() {
//    var validator = $("#userform").validate();
//    $('#userform').find('.field-validation-error span').each(function () {
//        validator.settings.success($(this));
//    });
//    validator.resetForm();
//}
function goBack() {
    $('#UsersTab').trigger('click');
    clearfields();
}

function Edit(currentObj) {
    //Tab Change on edit click
    $('#AddTab').trigger('click');
    
    var rowData = DataTables.UsersTable.row($(currentObj).parents('tr')).data();
    if (rowData) {
        var Result = GetUserDetailsByUser(rowData.ID, rowData.serviceCenter.Code);
        BindRolesCheckboxesBySC(rowData.serviceCenter.Code);
        FillUsers(Result);
    }
    ChangeButtonPatchView("UserProfile", "btnPatchBuildMember", "Edit"); //ControllerName,id of the container div,Name of the action
    $("#userform").removeAttr('data-ajax-begin');
}

function Save() {
    var r=[];
    $('input[name="Roles"]:checked').each(function () {
        if(this.value)
        {
            r.push(this.value);
        }
    });
    $("#RoleList").val(r);
    $("#btnInsertUpdateUser").trigger('click');
}
function Delete()
{
    $("#deleteId").val();
    $("#DeleteSCCode").val($("#SCCode").val());
    
    $("#btnuserFormDelete").trigger('click');
}

function GetUserDetailsByUser(userID,scCode)
{
    try {
        var data = { "UserID": userID, "SCCode": scCode };
        var ds = {};
        ds = GetDataFromServer("UserProfile/GetUserDetailsByUser/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Record;
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
           
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}



function FillUsers(Record)
{
    try
    {
        $("#ID").val(Record.ID);
        $("#deleteId").val(Record.ID);
        $("#SCCode").val(Record.serviceCenter.Code);
        $("#UserName").val(Record.UserName);
        if (Record.Roles)
        {
            for(var j=0;j<Record.Roles.length;j++)
            {
                $("#"+Record.Roles[j]).prop('checked', true);
            }
        }
        $("#Email").val(Record.Email);
        $("#LoginName").val(Record.LoginName);
        $("#LoginName").attr('readonly', 'true');
        switch(Record.Active)
        {
            case true:
                $("#Active").prop('checked', true);
                break;
            case false:
                $("#Active").prop('checked', false);
                break;
        }
      }
    catch(e)
    {
        notyAlert('error', e.message);
    }
}

function GetAllUsers() {
    try {
        var data = {};
        var ds = {};
        ds = GetDataFromServer("UserProfile/GetAllUsers/", data);

        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {

            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
            var emptyarr = [];
            return emptyarr;
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

function BindRolesCheckboxesBySC(scCode)
{
    try
    {
        var data = { "SCCode": scCode };
        var ds = {};
        ds = GetDataFromServer("UserProfile/GetAllRolesBySC/", data);

        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK")
        {
            if(ds.Records)
            {
                $("#divroles").html('');
                for(var i=0;i<ds.Records.length;i++)
                {
                    if (ds.Records[i].RoleName != 'SAdmin')
                    {
                        $("#divroles").append('<label class="checkbox-inline"><input id=' + ds.Records[i].RoleName + ' name="Roles" value=' + ds.Records[i].ID + ' type="checkbox"> ' + ds.Records[i].RoleName + '</label>');
                        $("#" + ds.Records[i].RoleName).prop('checked', false);
                    }
                 

                }
            }

            
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
            var emptyarr = [];
            return emptyarr;
        }
    }
    catch(e)
    {
        notyAlert('error', e.message);
    }
}


function RefreshUsersTable()
{
    DataTables.UsersTable.clear().rows.add(GetAllUsers()).draw(false);
    
}

function UserSaveSuccess(data, status) {

    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            notyAlert('success', JsonResult.Message.Status);
            RefreshUsersTable();
            //$("#ID").val(JsonResult.Record.userID);
            //$("#deleteId").val(JsonResult.Record.userID);
            //$("#Password").val('');
            //$("#ConfirmPassword").val('');
            //ChangeButtonPatchView("UserProfile", "btnPatchBuildMember", "Edit"); //ControllerName,id of the container div,Name of the action
            //$("#userform").removeAttr('data-ajax-begin');
            //$("#LoginName").attr('readonly', 'true');
            goBack();
            break;
        case "ERROR":
            notyAlert('error', JsonResult.Message);
            break;
        default:
            notyAlert('error', JsonResult.Message);
            break;
    }
}

function BeforePost()
{
    var fl = false;
   // var userid = $("#ID").val();
    var pass=$("#Password").val();
    var confm = $("#ConfirmPassword").val();
   
    if ((pass) && (confm))
        {
            fl = true;
           
    }
    else
    {
        fl = false;
        notyAlert('error', 'Password is required');
    }
   
    return fl;
}

function DeleteSuccess(data, status) {

    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            notyAlert('success', JsonResult.Message.Status);
            RefreshUsersTable();
            goBack();
           

            break;
        case "ERROR":
            notyAlert('error', JsonResult.Message);
            break;
        default:
            notyAlert('error', JsonResult.Message);
            break;
    }
}


