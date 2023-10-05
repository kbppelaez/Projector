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
        $.ajax({
            url: urlAction,
            type: 'POST',
            data: {
                "projectId": projId,
                "personId": persId
            },
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
                alert(response);
            }
        });
    }
}