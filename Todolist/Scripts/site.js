var _checkCoincidencesUrl;
var _dataTablesUrl;

function initUrls(checkCoincidencesUrl, dataTablesUrl) {
    _checkCoincidencesUrl = checkCoincidencesUrl;
    _dataTablesUrl = dataTablesUrl;
}

$(document).ready(function () {
    $('#Datatables').DataTable({
        "processing": true,
        "serverSide": true,
        "searching": false,
        "ordering": true,
        "select": true,
        "ajax": {
            "type": 'POST',
            "url": _dataTablesUrl,
            "contentType": 'application/json; charset=utf-8',
            'data': function (data) {
                return data = JSON.stringify(data);
            }
        },
        "paging": true,
        "lengthMenu": [[3, 6, 10, -1], [3, 6, 10, "All"]],
        "columns": [
            { data: "TaskDescription", title: "Description" },
            { data: "EnrollmentDate", title: "Date" },
            { data: "Approved", title: "Status"  },
            { data: "Empty" }
        ]
    });
});

$(document).on("click", "#AddTask", function () {
    $.get($(this).data("url"), function (data) {
        $("#DialogContent").html(data);
        $("#ModDialog").modal("show");
    });
});

$(document).on("click", ".ajaxLink", function (e) {
    e.preventDefault();
    $.get(this.href, function (data) {
        $("#DialogContent").html(data);
        $("#ModDialog").modal("show");
    });
});

$(document).on("input", "#TaskDescription", function (e) {
    checkCoincidences(_checkCoincidencesUrl);
});

function onSuccess(result) {
    onAjaxRequest(result);
}

function onFailure() {
    $("#Results").html("Request failed!");
}

function refreshPartialContent() {
    $('#Datatables').DataTable().ajax.reload(null, false);
}

function onAjaxRequest(result) {
    if (result.EnableSuccess) {
        alertBootstrap(result.SuccessMsg);
        $("#ModDialog").modal("hide");
        refreshPartialContent();
    }
    if (result.EnableError) {
        $("#Results").html(result.ErrorMsg);
        lightBorderError();
    }
}

function alertBootstrap(mess) {
    $("#InfoMessage").text(mess);
    $("#Alert").removeClass("hide");
    window.setTimeout(function () {
        $("#Alert").addClass("hide");
    }, 3000);
}

function lightBorderError() {
    $("#ErrorMsg").removeClass("has-success has-error").addClass("has-error");
}

function checkCoincidences() {
    var description = $("#TaskDescription").val();
    var todolistId = $("#TodolistId").val();
    $.ajax({
        url: _checkCoincidencesUrl,
        type: "POST",
        data: { taskDescription: description, taskId: todolistId },
        success: function (result) {
            if (result.EnableSuccess) {
                $("#Results").html(result.SuccessMsg);
            }
            if (result.EnableError) {
                $("#Results").html(result.ErrorMsg);
            }
        },
        error: function () {
            $("#Results").html("Request failed!");
        }
    });
}



