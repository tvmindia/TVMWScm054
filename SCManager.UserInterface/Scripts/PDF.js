//**************************************************************************
//Author: Thomson K Varkey
//Created On: 16-Oct-2017
//This js will support the html binding and ajax call to iText sharp 
//*****************************************************************************
function DrawTable(options) {
    debugger;
    var Records = GetItemsSummary(options);
    if (Records != null) {
        for (var i = 0; i < Records.length; i++) {
            for (var j = 0; j < options.Exclude_column.length; j++)
            {
                delete Records[i][options.Exclude_column[j]];
            }            
        }
        $("#customtbl").empty();
        $("#customtbl").append('<table id="tblTechnicianPerformanceList" style="margin-top:30px;width:100%;padding-right:10px;" class="table compact" cellspacing="0">'
                            + '<thead><tr id="trTechPerform" style="height:30px"></tr></thead>'
                            + '<tbody id="tbodyPerform"></tbody>'
                        + '</table>')
        var Header = [];
        $.each(Records, function (index, Records) {
            
            if (Header.length == 0) {
                $.each(Records, function (key, value) {
                    Header.push(key);
                });

                for (var i = 0; i < Header.length; i++) {
                    Performed = 0;
                    $.each(options.Header_column_style, function (key, value) {
                            if (key == Header[i])
                            {
                                $("#trTechPerform").append('<th style="' + (value.style !== undefined ? value.style : "") + '">' + (value.custom_name !== undefined ? value.custom_name : Header[i]) + '</th>')
                                Performed = 1;
                            }
                            
                        });
                        if (Performed == 0)
                        {
                            $("#trTechPerform").append('<th>' + Header[i] + '</th>')
                        }
                    
                }
            }
            var html = "";
            $.each(Records, function (key, value) {
                PerformedCol = 0;
                $.each(options.Body_Column_style, function (keyCol, valueCol) {
                    if (keyCol == key) {
                        if (value === null)
                        {
                            html = html + '<td></td>'
                        }
                        else
                        {
                            html = html + '<td style="' + valueCol + '">' + (($.isNumeric(value)) ? roundoff(value) : value) + '</td>'
                        }                        
                        PerformedCol = 1;
                    }

                });
                if (PerformedCol == 0)
                {
                    if (value === null) {
                        html = html + '<td></td>'
                    }
                    else
                    {
                        html = html + '<td>' + ($.isNumeric(value) ? roundoff(value) : value) + '</td>'
                    }
                        
                   
                }                   

            });
            if (index % 2 === 0)
            {
                $("#tbodyPerform").append('<tr style="padding-top:1px;padding-botton:1px;height:30px;background-color:' + options.Row_color.Even + '">' + html + '</tr>')
            }
            else
            {
                $("#tbodyPerform").append('<tr style="padding-top:1px;padding-botton:1px;height:30px;background-color:' + options.Row_color.Odd + '">' + html + '</tr>')
            }
            

        });
    }

}
function GetItemsSummary(options) {
    try {
        var data = options.data;
        var ds = {};
        ds = GetDataFromServer(options.Action, data);
            if (ds != '') {
                ds = JSON.parse(ds);                 
            }
            if (ds.Result == "OK") {
                return ds.Records;
            }
            if (ds.Result == "ERROR") {
                notyAlert('error', ds.Message);
            }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}

