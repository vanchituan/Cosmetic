var cart = {
    init: function () {
        Common.loadProvince('');
        cart.regEvents();
    },
    regEvents: function () {
        $('#btnOr').off('click').on('click', function () {
            cart.CheckQuantity();
        });

        $('#continue_btn').off('click').on('click', function () {
            window.location.href = "/";
        });

        $('#btnShowModal').off('click').on('click', function () {
            cart.CheckQuantity();
            cart.AsyncConfirmYesNo(
                            "Xác nhận ",
                            "Chưa đăng nhập, bạn muốn mua hàng không tích lũy !!",
                           cart.MyYesFunction,
                            cart.MyNoFunction
                        );
        });

        //cập nhật giỏ hàng
        $('#update_btn').off('click').on('click', function () {
            if (!cart.CheckQuantity()) {
                return false;
            }
            else {
                cart.FuncUpdate();
            }
        });     //end btn on off

        // xóa toàn bộ giỏ hàng
        $('#deleteAll_btn').off('click').on('click', function () {
            $.ajax({
                url: '/Cart/DeleteAll',
                dataType: "json",
                type: 'POST',
                success: function (res) {
                    if (res.status = true) {        // cập nhật thành công
                        window.location.href = "/gio-hang";
                    }
                }
            });
        });     //end btn on off

        // xóa 1 item
        $('.delete_btn').off('click').on('click', function (e) {
            e.preventDefault();
            $.ajax({
                data: { id: $(this).data('id') },
                url: '/Cart/Delete',
                dataType: "json",
                type: 'POST',
                success: function (res) {
                    if (res.status = true) {        // cập nhật thành công
                        window.location.href = "/gio-hang";
                    }
                }
            });
        });     //end btn on off

        $('.quantity_txt, #txtPhone').keypress(function (evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode != 46 && charCode > 31
              && (charCode < 48 || charCode > 57))
                return false;
            return true;
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

        $('input[name="paymentMethod"]').off('click').on('click', function () {
            if ($(this).val() == 'NL') {
                $('.boxContent').hide();
                $('#nganluongContent').show();
            }
            else if ($(this).val() == 'ATM_ONLINE') {
                $('.boxContent').hide();
                $('#bankContent').show();
            }
            else {
                $('.boxContent').hide();
            }
        });

        $('#btnCreatedOrder').off('click').on('click', function () {
            var orderVm = {
                ShipName: $('#ShipName').val(),
                ShipMobile: $('#ShipMobile').val(),
                ShipEmail: $('#ShipEmail').val(),
                ShipAddress: $('#ShipAddress').val(),
                ProvinceId: $('#ProvinceId').val(),
                DistrictId: $('#DistrictId').val(),
                PrecinctId: $('#PrecinctId').val(),
                PaymentMethod: $('input[name="paymentMethod"]:checked').val(),
                BankCode: $('input[groupname="bankcode"]:checked').prop('id'),
            }

            $.ajax({
                url: '/Cart/Payment',
                type: 'POST',
                data: orderVm,
                success: function (response) {
                    debugger;
                    if (response.status && response.viaNganLuong) {
                        if (response.urlCheckout != undefined && response.urlCheckout != '') {
                            window.location.href = response.urlCheckout;
                        }
                        else {
                            $('#divMessage').html('Cảm ơn bạn đã đặt hàng thành công. Chúng tôi sẽ liên hệ sớm nhất.');
                        }
                    }
                    else if (!response.status && response.viaNganLuong) {
                        $('#divMessage').show();
                        $('#divMessage').text(response.message);
                    }
                    else {
                        window.location.href = '/hoan-thanh';
                    }
                }
            })
        });

    },//end regEvent
    CheckQuantity: function () {
        var result = true;
        $('.quantity_txt').each(function () {
            if ($(this).val() == "") {
                alert('Bạn chưa nhập số lượng');
                result = false;
            }
        });
        return result;
    },

    FuncUpdate: function () {
        var cartList = new Array();
        $('.quantity_txt').each(function () {
            cartList.push(
                { //đẩy dữ liệu vào 1 mảng 
                    Quantity: $(this).val(),
                    Product: {
                        ID: $(this).data('id')//id của sản phẩm này
                    }
                });     //end push

        }); //end each
        $.ajax({
            url: '/Cart/Update',
            data: { cartModel: JSON.stringify(cartList) },
            dataType: "json",
            type: 'POST',
            success: function (res) {
                if (res.status = true) {        // cập nhật thành công
                    window.location.href = "/gio-hang";
                }
            }
        });
    },

    MyYesFunction: function () {
        window.location.href = "/thanh-toan";
    },

    MyNoFunction: function () {
        window.location.href = "/dang-nhap";
    },

    AsyncConfirmYesNo: function (title, msg, yesFn, noFn) {
        var $confirm = $("#modalConfirmYesNo");
        $confirm.modal('show');
        $("#lblTitleConfirmYesNo").html(title);
        $("#lblMsgConfirmYesNo").html(msg);
        $("#btnYesConfirmYesNo").off('click').click(function () {
            yesFn();
            $confirm.modal("hide");
        });
        $("#btnNoConfirmYesNo").off('click').click(function () {
            noFn();
            $confirm.modal("hide");
        });
    }
}
cart.init();