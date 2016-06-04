var role = {
    init: function () {
        //role.registerEvent();
    },
    registerEvent: function () {
        $('#txtID').off('keypress').on('keypress', function () {
            $.ajax({
                type: 'POST',
                url: '/Admin/Role/CheckRole',
                dataType: 'json',
                data: {
                    Id: $('#txtID').val()
                },
                success: function (res) {
                    if (res.status == true) {
                        $('#divResult').html('Tên vai trò hợp lệ');
                        $('#divResult').css('color', 'green');
                        $('#txtID').css('border', '1px solid green');
                    }
                    else {
                        $('#divResult').html('Tên vai trò đã tồn tại');
                        $('#divResult').css('color', 'red');
                        $('#txtID').css('border', '1px solid red');
                        $('#txtID').focus();
                    }
                }
            });
        });
    }
}
role.init();
