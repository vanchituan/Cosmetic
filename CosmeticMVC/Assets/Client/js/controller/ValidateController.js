var validator = {
    init: function () {
        validator.registerEvents();
        validator.Validation();
    },
    registerEvents: function () {
        
    },
    Validation: function () {
        $("form:not(:first)").validate({
            rules: {
                Password: "required",
                ConfirmPassword: {
                    required: true,
                    equalTo: "#Password"
                },
                Email: {
                    required: true,
                    email: true
                }
            },
            messages: {
                ConfirmPassword: 'Xác nhận mật khẩu chưa đúng !!',
                Email: 'Định dạng email chưa đúng !!'
            }
        });
    }

}
validator.init();