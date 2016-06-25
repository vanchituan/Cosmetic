var client = function () {
    return this.init();
}

client.prototype = {
    init: function () {
        Common.loadProvince('');
        this.registerEvents();
    },
    registerEvents: function () {
        $('#btnSave').off('click').on('click', function () {
            $.ajax({
                url: '/User/Update',
                type: 'POST',
                dataType: 'json',
                data: {
                    clientname: $('#txtUsername').val(),
                    password: $('#Password').val(),
                    name: $('#txtName').val(),
                    phone: $('#txtPhone').val(),
                    email: $('#txtEmail').val(),
                    address: $('#txtAddress').val(),
                    photoPath: $('#txtImage').val()
                },
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = '/';
                    }
                }
            });
        });

        $('#btnCreate').off('click').on('click', function () {
            if ($('#status').html() == 'Mã xác nhận chính xác') {
                Common.client.FuncCreate();
            }
            else {
                alert('Nhập lại mã xác nhận');
            }
        });

        $('#txtUsername').off('change').on('change', function () {
            $.ajax({
                type: 'POST',
                url: '/User/CheckUsername',
                dataType: 'json',
                data: {
                    clientName: $('#txtUsername').val()
                },
                success: function (res) {
                    if (res.status == true) {
                        $('#divResult').html('Tên tài khoản hợp lệ');
                        $('#divResult').css('color', 'green');
                        $('#txtUsername').css('border', '1px solid green');
                    }
                    else {
                        $('#divResult').html('Tên tài khoản đã tồn tại');
                        $('#divResult').css('color', 'red');
                        $('#txtUsername').css('border', '1px solid red');
                        $('#txtUsername').focus();
                    }
                }
            });
        });     //end clientname change

        $("#CaptchaCode").off('change').on('change', function (e) {
            $('#status').attr('class', 'inProgress');
            $('#status').text('Checking...');

            // get client-side Captcha object instance
            var captchaObj = $("#CaptchaCode").get(0).Captcha;

            // gather data required for Captcha validation
            var params = {}
            params.CaptchaId = captchaObj.Id;
            params.InstanceId = captchaObj.InstanceId;
            params.UserInput = $("#CaptchaCode").val();

            // make asynchronous Captcha validation request
            $.getJSON('/User/CheckCaptcha', params, function (result) {
                if (true === result) {
                    $('#status').attr('class', 'correct');
                    $('#status').text('Mã xác nhận chính xác');
                    $('#CaptchaCode').css('border', '1px solid green')
                }
                else {
                    $('#status').attr('class', 'incorrect');
                    $('#status').text('Mã xác nhận sai!!!');
                    $('#CaptchaCode').css('border', '1px solid red')
                    return false;
                    // always change Captcha code if validation fails
                    captchaObj.ReloadImage();
                }
            });
            e.preventDefault();
        });

        $('#ProvinceId').off('change').on('change', function () {
            var id = $(this).val();
            if (id != '') {
                Common.loadDistrict(parseInt(id), '');
                //Common.user.SubmitForm();
            }
            else {
                $('#DistrictId').html('');
            }
        });

        $('#DistrictId').off('change').on('change', function () {
            var districtId = $(this).val();
            var provinceId = $('#ProvinceId').val();
            if (districtId != '' && provinceId != '') {
                Common.loadPrecinct(parseInt(provinceId), parseInt(districtId), '');
                //Common.user.SubmitForm();
            }
            else {
                $('#DistrictId').html('');
            }
        });
    },


    FuncCreate: function () {
        var user = {
            UserName: $('#txtUsername').val(),
            Password: $('#Password').val(),
            Name: $('#txtName').val(),
            Email: $('#txtEmail').val(),
            Address : $('#txtAddress').val(),
            Phone: $('#txtPhone').val(),
            Image: $('#txtImage').val(),
            ProvinceId: $('#ProvinceId').val(),
            DistrictId: $('#DistrictId').val(),
            PrecinctId: $('#PrecinctId').val()
        }

        $.ajax({
            url: '/User/Register',
            type: 'POST',
            dataType: 'json',
            data: user,
            success: function (response) {
                if (response.status == true) {
                    window.location.href = '/xac-nhan';
                }
            }
        });
    },

    AsyncConfirmYesNo: function (title) {
        var $confirm = $("#modalConfirmYesNo");
        $confirm.modal('show');
        $("#lblTitleConfirmYesNo").html(title);
    }
}
var client = client;
client.constructor = client;
Common.client = new client();
