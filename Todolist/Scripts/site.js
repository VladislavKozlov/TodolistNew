var _partialContentUrl;
var _checkCoincidencesUrl;
var _dataPaginationUrl;

function initUrls(partialContentUrl, checkCoincidencesUrl, dataPaginationUrl) {
    _partialContentUrl = partialContentUrl;
    _checkCoincidencesUrl = checkCoincidencesUrl;
    _dataPaginationUrl = dataPaginationUrl;
}

$(document).ready(function () {
    $('#Datatables').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": _dataPaginationUrl,
        "paging": true,
        "lengthMenu": [[3, 6, 10, -1], [3, 6, 10, "All"]],
        "columns": [
            { data: "Description" },
            { data: "Date" },
            { data: "Status" },
            { data: "Empty" }
        ]
    });
});

$(document).on("click", "#AddTask", function (e) {
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

//$(document).on("click", ".sort", function (e) {
//  var descending = $(this).data("descending");
//  var sortColumn = $(this).data("column");
//  var data = { sortColumn: sortColumn, descending: descending };
//  $(this).data("descending", !descending);
//  $.ajax({
//     url: _partialContentUrl,
//     type: "GET",
//    data: data,
//    success: function (result) {
//        $("#PartialContent").html(result);
//    },
//   error: function () {
//       $("#PartialContent").html("Запрос не выполнен!");
//   }
// });
//});

$(document).on("input", "#TaskDescription", function (e) {
    checkCoincidences(_checkCoincidencesUrl);
});

function onSuccess(result) {
    onAjaxRequest(result);
}

function onFailure() {
    $("#Results").html("Запрос не выполнен!");
}

function refreshPartialContent(url) {
    $.get(url, null, function (data) {
        $("#PartialContent").html(data);
    });
}

function onAjaxRequest(result) {
    if (result.EnableSuccess) {
        alertBootstrap(result.SuccessMsg);
        $("#ModDialog").modal("hide");
        refreshPartialContent(_partialContentUrl);
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
            $("#Results").html("Запрос не выполнен!");
        }
    });
}



