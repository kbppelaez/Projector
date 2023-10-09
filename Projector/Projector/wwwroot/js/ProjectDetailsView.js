(function ($, scope) {

    function ToggleAddButton(form) {
        $("#addPersonToProjectButton", form).attr('disabled', false);
    }

    function ReInitializeAddButton() {
        var button = document.getElementById("addPersonToProjectButton");
        if (button) {
            button.addEventListener("click", AddPersonToProject);
        }
    }
    function PromptSelectError(form, msg) {
        $('#selectError', form).html(msg);
    }

    function SendAddPersonRequest(form, data) {
        ToggleAddButton(form);
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
                temp = response;
                if (response.status) {
                    PromptSelectError(form, response.responseText);
                } else {
                    $("#Assignments").html(response);
                    ReInitializeAddButton();
                }
            },
            error: function (response) {
                temp = response.responseText;
                form = $('#addPersonToProjectForm');
                PromptSelectError(form, temp);
            }
        })
        .done(function (e) {
            ToggleAddButton($('#addPersonToProjectForm'))
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

    function initializeAddPersonToProject() {
        document.getElementById("addPersonToProjectButton").addEventListener("click", AddPersonToProject);
    }

    function initializeView() {
        initializeAddPersonToProject();
        //initializeRemovePersonFromProject();
    }

    var vm = {
        initialize: initializeView
    };

    scope.projectDetailsView = vm;

})(jQuery, window);