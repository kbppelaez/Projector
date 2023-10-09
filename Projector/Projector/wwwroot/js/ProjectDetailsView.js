(function ($, scope) {

    function ToggleAddButton(form, disable) {
        $("#addPersonToProjectButton", form).attr('disabled', disable);
    }

    function ToggleRemoveButton(disable) {
        $(".RemoveButton").attr('disabled', disable);
    }

    function PromptSelectError(form, msg) {
        $('#selectError', form).html(msg);
    }

    function PromptRemoveError(id, msg) {
        if (id == -1) { // -1 is passed when all removeError spans would be given msg
            $('.RemoveError').html(msg);
            return;
        }
        $('#removeError-' + id).html(msg);
    }

    function SendAddPersonRequest(form, data) {
        ToggleAddButton(form, true);
        PromptSelectError(form, ""); //clears select error span
        $.ajax({
            url: form.attr('action'),
            contentType: 'application/json; charset=utf-8',
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden [name="__RequestVerificationToken"]').val()
                );
            },
            type: 'POST',
            headers: {
                "Accepts": "text/html",
            },
            data: JSON.stringify(data),
            success: function (response) {
                //temp = response;
                if (response.status) {
                    PromptSelectError(form, response.responseText);
                }else {
                    $("#Assignments").html(response);
                    initializeView();
                }
            },
            error: function (response) {
                //temp = response.responseText;
                form = $('#addPersonToProjectForm');
                PromptSelectError(form, response.responseText);
            }
        })
        .done(function (response) {
            ToggleAddButton($('#addPersonToProjectForm'), false);
        });
    }

    function SendRemovePersonRequest(form, data) {
        ToggleRemoveButton(true);
        PromptRemoveError(-1, ""); //set all to empty string
        var redirect = false;

        $.ajax({
            url: form.attr('action'),
            contentType: 'application/json; charset=utf-8',
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden [name="__RequestVerificationToken"]').val()
                );
            },
            type: 'POST',
            headers: {
                "Accepts": 'text/html',
            },
            data: JSON.stringify(data),
            success: function (response) {
                //temp = response;
                if (response.status) {
                    PromptRemoveError(data.personId, response.responseText);
                } else if (response.startsWith("/")) {
                    redirect = true;
                } else {
                    $("#Assignments").html(response);
                    initializeView();
                }
            },
            error: function (response) {
                //temp = response.responseText;
                form = $('#Remove-' + data.personId);
                PromptRemoveError(data.personId, response.responseText);
            }
        }).done(function (response) {
            if (redirect) {
                window.location.href = response;
            } else {
                ToggleRemoveButton(false);
            }
        });
    }

    function AddPersonToProject(e) {
        if (e.target && e.target.tagName == 'BUTTON') {
            var form = $('#addPersonToProjectForm');
            var personId = $('#addPerson', form).val();

            if (personId == 0) {
                PromptSelectError(form, "Please select an employee.");
                return;
            }

            var data = { "personId": personId, "projectId": $('#projectId', form).val() };
            SendAddPersonRequest(form, data);
        }
    }

    function RemovePersonFromProject(e) {
        if (e.target && e.target.tagName == 'BUTTON') {
            var personId = e.target.value;
            var form = $('#Remove-' + personId);
            var projectId = $('#projectId-' + personId).val();

            var data = { "personId": personId, "projectId": projectId };
            SendRemovePersonRequest(form, data);
        }
    }

    function initializeAddPersonToProject() {
        var button = document.getElementById("addPersonToProjectButton");
        if (button) {
            button.addEventListener("click", AddPersonToProject);
        }
    }

    function initializeRemovePersonFromProject() {
        var buttons = $(".RemoveButton");
        if (buttons && buttons.length > 0) {
            buttons.on("click", RemovePersonFromProject);
        }
    }

    function initializeView() {
        initializeAddPersonToProject();
        initializeRemovePersonFromProject();
    }

    var vm = {
        initialize: initializeView
    };

    scope.projectDetailsView = vm;

})(jQuery, window);