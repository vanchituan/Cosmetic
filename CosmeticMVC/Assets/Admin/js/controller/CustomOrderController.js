var customOrder = function () {
    return this.init();
}

customOrder.prototype = {
    init: function () {

        this.UnBindModalCustomer();
        this.registerEvents();
    },
    registerEvents: function () {
        $('tr.trUser').off('dblclick').on('dblclick', function () {
            var self = $(this);
            var name = $(this).find('td:eq(0)').html();
            var mobile = $(this).find('td:eq(1)').html();
            var fullAddress = $(this).find('td:eq(2)').html();
            var address = $(this).data('address');
            var id = $(this).data('id');
            var email = $(this).data('email');
            var promotionId = $(this).data('promotionid');
            $.post('/Admin/User/GetDiscount', { promotionId: promotionId }, function (res) {
                if ($('input[type=radio]:eq(2)').is(':checked')) {// in profile
                    var provinceId = self.data('provinceid');
                    var districtId = self.data('districtid');
                    var precinctId = self.data('precinctid');
                    $('#ProvinceId').val(provinceId);
                    $('#DistrictId').val(districtId);
                    $('#PrecinctId').val(precinctId);
                    $('#txtAddress').val(fullAddress);
                    $('#ShipAddress').val(address);
                }
                $('#txtCustomerName').val(name);
                $('#Discount').val(res);
                $('#CustomerId').val(id);
                $('#ShipEmail').val(email);
                $('#ShipMobile').val(mobile);
                $("#modalConfirmYesNo").modal("hide");
            })
        });

        //add products to cart
        $('tr.trProduct').off('dblclick').on('dblclick', function () {
            var productName = $(this).find('td:eq(1)').html();
            var flagExist = true;
            var target;

            $('#tbdCart tr').each(function () {
                //this product existed in cart
                if (productName == $(this).find('td:eq(1)').html()) {
                    target = $(this).find('td:eq(3)');
                    Common.customOrder.IncreaseQuantity(target);
                    flagExist = false;
                }
            })

            //no this product in cart
            if (flagExist) {
                var price = $(this).find('td:eq(2)').html();
                var html = '';
                //template with empty tag but have a frame, now bind data into this frame
                var template = $('#tbody-template').html();

                html += Mustache.render(template, {
                    ProductId: $(this).find('td:first').html(),
                    Name: productName,
                    Price: price,
                    Quantity: 1
                });

                $('#tbdCart').append(html);
                Common.customOrder.IterateThroughPrice();
                Common.customOrder.BindOrderNumber();
                Common.customOrder.registerEvents();
            }

        });

        //increase quantity
        $('.btnAdd').off('click').on('click', function () {
            var target = $(this).parents('tr').find('td:eq(3)');
            Common.customOrder.IncreaseQuantity(target);
        });

        // decrease quantity
        $('.btnMinus').off('click').on('click', function () {
            $target = $(this).parents('tr').find('td:eq(3)');
            var currentQuantity = $target.html();
            if (currentQuantity > 1) {
                var newQuantity = parseInt(currentQuantity) - 1;
                $target.html(newQuantity);
                Common.customOrder.IterateThroughPrice();
            }
            var hid = $(this).parents('tr').find('td:last input[type=hidden]').val();
            console.log(hid);
        });

        //remove a row
        $('.btnRemove').off('click').on('click', function () {
            $(this).parents('tr').remove();
            Common.customOrder.IterateThroughPrice();
            Common.customOrder.BindOrderNumber();
        });

        $('#CategoryId').off('change').on('change', function () {
            $("#frmProduct").submit();

        });

        $('#btnSubmit').off('click').on('click', function () {
            if ($('#tbdCart tr').length == 0) {// no product in cart
                alert('Chưa có sản phẩm nào trong giỏ hàng!!');
                return false;
            }
            else {
                var orderDetail = [];
                $('#tbdCart tr').each(function () {
                    orderDetail.push({
                        ProductId: $(this).data('productid'),
                        Quantity: $(this).find('td:eq(3)').html()
                    });
                })

                var amount = $('#Total').val();
                var discount = ($('#Discount').val()) / 100;
                var total = amount - (amount * discount);
                $('#CreatedDate').val(moment().format('MMM D, YYYY'));
                $('#Total').val(total);
                //console.log('amount : ' + amount);
                //console.log('discount : ' + discount);
                //console.log('total after promotion : ' + total);

                var order = $('#frmOrder').serialize();
                $.ajax({
                    url: '/Admin/Order/CreateOrder',
                    type: 'POST',
                    data: order + '&orderDetail=' + JSON.stringify(orderDetail),
                    success: function () {
                        $('#strongPrice').html(0);
                        Common.customOrder.clearFrmOrder();
                        $('#tbdCart tr').remove();
                        Common.product.Refresh();
                    },
                    error: function () {
                        alert('Có lỗi gì đó rồi ông bạn..');
                    }
                });
            }
        });

        $('#btnAddressDetailExpand').off('click').on('click', function () {
            var target = "#modal-address-detail";

            var $confirm = $(target);
            Common.loadProvince(target + ' ');
            $confirm.modal('show');
            $(target+' #ProvinceId').off('change').on('change', function () {

                var id = $(this).val();
                if (id != '') {
                    Common.loadDistrict(parseInt(id), target +' ');
                }
                else {
                    $('#DistrictId').html('');
                }
            });

            $(target + ' #DistrictId').off('change').on('change', function () {
                var districtId = $(this).val();
                var provinceId = $(target+' #ProvinceId').val();
                if (districtId != '' && provinceId != '') {
                    Common.loadPrecinct(parseInt(provinceId), parseInt(districtId),target + ' ');
                }
                else {
                    $('#DistrictId').html('');
                }
            });

            $(target + ' #PrecinctId').off('change');

            $('#btnYes-address-detail').off('click').on('click', function () {
                var address = $('#addressField').val();
                var province = $(target + ' #ProvinceId option:selected').text();
                var district = $(target + ' #DistrictId option:selected').text();
                var precinct = $(target + ' #PrecinctId option:selected').text();
                var provinceId = $(target + ' #ProvinceId').val();
                var districtId = $(target + ' #DistrictId').val();
                var precinctId = $(target + ' #PrecinctId').val();
                $('#ProvinceId').val(provinceId);
                $('#DistrictId').val(districtId);
                $('#PrecinctId').val(precinctId);
                var result = address + ', ' + precinct + ', ' + district + ', ' + province;
                $('#txtAddress').val(result);
                $('#ShipAddress').val(address)
                $("#modal-address-detail").modal("hide");
            });

            $('#btnNo-address-detail').off('click').on('click', function () {
                $("#modal-address-detail").modal("hide");
            });
        });

        //Không tích lũy (khách lẽ)
        $('input[type=radio]:first').off('change').on('change', function () {
            if ($(this).is(':checked')) {
                Common.customOrder.clearFrmOrder();
                Common.customOrder.UnBindModalCustomer();
            }
        });

        //khách quen
        $('input[type=radio]:eq(1)').off('change').on('change', function (e) {
            if ($(this).is(':checked')) {
                Common.customOrder.clearFrmOrder();
                Common.customOrder.regEventsForModalCustomer(e);
            }
        });


        //như trong profile
        $('input[type=radio]:last').off('change').on('change', function (e) {
            if ($(this).is(':checked')) {
                Common.customOrder.clearFrmOrder();
                Common.customOrder.regEventsForModalCustomer(e);
            }
        });
    },

    IterateThroughPrice: function () {
        var total = 0;
        $('#tbdCart tr').each(function () {
            $target = $(this).find('td:eq(2)');
            total += parseInt(Common.ReplaceFormatCurrency($target.html()) * $target.next().html());
        })
        $('#strongPrice').html(Common.FormatCurrency(total));
        Common.customOrder.BindTotal();
    },

    BindOrderNumber: function () {
        var count = 1;
        $('#tbdCart tr').each(function () {
            $(this).find('td:first').html(count);
            count++;
        })
    },

    IncreaseQuantity: function (target) {
        //target = $(that).parents('tr').find('td:eq(3)');
        var currentQuantity = target.html();
        var newQuantity = parseInt(currentQuantity) + 1;
        //update quantity
        target.html(newQuantity);
        Common.customOrder.IterateThroughPrice();
    },

    BindTotal: function () {
        var total = Common.ReplaceFormatCurrency($('#strongPrice').html());
        $('#Total').val(total);
    },

    UnBindModalCustomer: function () {
        $('#txtCustomerName').unbind('keypress');
        $('#btnModalCustomer').unbind('click');
        $('#txtCustomerName').attr('autofocus', true).attr('placeholder', 'Nhập tên khách hàng..');
    },

    regEventsForModalCustomer: function (e) {
        $('#txtCustomerName').attr('autofocus', true).attr('placeholder', 'Nhấn phím enter..');
        $('#txtCustomerName').off('keypress').on('keypress', function (e) {
            if (e.which == 13 || Common.Empty(e)) {
                Common.customOrder.ShowCustomerDialog();
            }
        });

        $('#btnModalCustomer').off('click').on('click', function () {
            Common.customOrder.ShowCustomerDialog();
        });
    },

    ShowCustomerDialog: function (e) {
        var $confirm = $("#modalConfirmYesNo");
        $confirm.modal('show');
        Common.loadProvince();
        $("#lblTitleConfirmYesNo").html('Chọn khách hàng đang đặt hàng');
        $("#btnNo-modal-customer").off('click').click(function () {
            $("#modalConfirmYesNo").modal("hide");
        });

    },

    clearFrmOrder: function () {
        $('#frmOrder').each(function () {
            this.reset();
        })
        $('#CustomerId').val('');
        $('#Total').val('');
        $('#CreatedDate').val('');
        $('#ProvinceId').val('');
        $('#DistrictId').val('');
        $('#PrecinctId').val('');
    },
}

var customOrder = customOrder;
customOrder.constructor = customOrder;
Common.customOrder = new customOrder();
