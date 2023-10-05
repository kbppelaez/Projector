// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var temp;

//adding a person to the project
function AddEmployee(urlAction) {
    var projId = document.getElementById("projectId").value;
    var persId = document.getElementById("addPerson").value;

    if (persId == 0) {
        document.getElementById("selectError").innerHTML = "Please select an employee."
    } else {
        document.getElementById("selectError").innerHTML = "";
        var data = {
            "projectId": projId,
            "personId": persId
        };

        $.ajax({
            url: urlAction,
            contentType: 'application/json; charset=utf-8',
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            dataType: 'json',
            type: 'POST',
            headers: {
                "Accepts": "application/json"
            },
            data: JSON.stringify(data),
            success: function (response) {
                //temp = response;
                if (response.status == "SUCCESS") {
                    document.getElementById("Assigned").innerHTML = response.assignedView;
                    document.getElementById("Unassigned").innerHTML = response.unassignedView;
                } else {
                    document.getElementById("selectError").innerHTML = response.error;
                }
            },
            error: function (response) {
                document.getElementById("selectError").innerHTML = "An error occured."
                //temp = response;
                //alert(response);
            }
        });
    }
}

function RemoveEmployee(urlAction, persId) {
    var projId = document.getElementById("projectId").value;

    if (persId == 0) {
        document.getElementById("removeError").innerHTML = "Please select an employee."
    } else {
        document.getElementById("removeError").innerHTML = "";
        var data = {
            "projectId": projId,
            "personId": persId
        };

        $.ajax({
            url: urlAction,
            contentType: 'application/json; charset=utf-8',
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            dataType: 'json',
            type: 'POST',
            headers: {
                "Accepts": "application/json"
            },
            data: JSON.stringify(data),
            success: function (response) {
                //temp = response;
                if (response.status == "FAIL") {
                    document.getElementById("removeError").innerHTML = response.error;
                } else if (response.status == "SUCCESS") {
                    document.getElementById("Assigned").innerHTML = response.assignedView;
                    document.getElementById("Unassigned").innerHTML = response.unassignedView;
                }
            },
            error: function (response) {
                document.getElementById("removeError").innerHTML = "An error occured."
                temp = response;
                //alert(response);
            }
        }).done(function (result) {
            if (result.status == "REDIRECT") {
                window.location.href = result.redirectUrl;
            }
        });
    }
}