$(document).ready(function () {
    $("#AjaxForm").validate({
        errorClass: 'text-danger',
        errorElement: 'div',
        errorPlacement: function (error, e) {
            e.parents('.form-group').append(error);
        },
        highlight: function (e) {

            $(e).closest('.form-group').removeClass('has-success has-error').addClass('has-error');
            $(e).closest('.help-block').remove();
        },
        success: function (e) {
            e.closest('.form-group').removeClass('has-success has-error');
            e.closest('.help-block').remove();
        },
        rules: {
            "TaskDescription": {
                required: true,
                maxlength: 100
            }
        },
        messages: {
            "TaskDescription": {
                required: "Данное поле должно быть заполнено!",
                maxlength: "Описание задачи не может быть больше 100 символов!"
            }
        }
    });
});
