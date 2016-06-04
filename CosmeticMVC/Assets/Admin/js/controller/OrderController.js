//cục bộ 1 nè
var order = function () {
    return this.init();
}
//cục bộ 2 nè
order.prototype = {
    init: function () {
        Common.loadProvince('');
        this.registerEvents();
    },
    //mã hóa đơn dùng chung
    orderID: '',
    registerEvents: function () {
        $('.btnOrderDetail').off('click').on('click', function () {
            var that = $(this)
            Common.order.orderID = that.data('id');
            $.ajax({
                url: '/Admin/Order/GetDetail',
                type: 'POST',
                dataType: 'json',
                data: {
                    orderID: Common.order.orderID
                },
                success: function (res) {
                    var templateInfoOrder = $('#tplInfoOrder-orderDetail').html();
                    var templateCartDetail = $('#tplCartDetail-orderDetail').html();
                    var templateInfoPromotion = $('#tplInfoPromotion-orderDetail').html();
                    var strCartDetail = '';
                    var strInfoOrder = '';
                    var strInfoPromotion = '';

                    // bind dropdownlist
                    if (res.StatusId == 5) {// disable modify if status name : "Đơn hàng thành công"
                        $('.modal-dialog #StatusId').prop('disabled', true);
                        $('#btnYesConfirmYesNo').prop('disabled', true);
                    }
                    else {
                        $('#btnYesConfirmYesNo').removeAttr('disabled');
                        $('.modal-dialog #StatusId').removeAttr('disabled');
                    }

                    $('.modal-dialog #StatusId option').removeAttr('selected').each(function () {
                        if ($(this).val() == res.StatusId) {
                            $(this).prop('selected', 'selected');
                        }
                    });

                    //bind info customer
                    strInfoOrder += Mustache.render(templateInfoOrder, {
                        OrderId: Common.order.orderID,
                        CreatedDate: function () {
                            return moment(res.CreatedDate).format('DD/MM/YYYY');
                        },
                        PromotionName: res.PromotionName,
                        ShipName: res.ShipName,
                        ShipEmail: res.ShipEmail,
                        ShipAddress: res.ShipAddress,
                        ShipMobile: res.ShipMobile,
                    });
                    $('#divInfoOrder-orderDetail').html(strInfoOrder);

                    // bind order detail into tbody
                    $(res.CartList).each(function (index, element) {
                        strCartDetail += Mustache.render(templateCartDetail, {
                            OrderNumber: index + 1,
                            ProductId: element.ProductId,
                            ProductName: element.ProductName,
                            Quantity: element.Quantity,
                            Price: function () {
                                return Common.FormatCurrency(element.Price);
                            }
                        });
                    })
                    $('#divCartDetail-orderDetail').html(strCartDetail);

                    //bind info promotion into footer
                    var prototypePromotion;
                    var prototypeAmount = 0;
                    strInfoPromotion += Mustache.render(templateInfoPromotion, {
                        Amount: function () {
                            var tmpQuan;
                            var tmpAmount;
                            $('tbody#divCartDetail-orderDetail tr').each(function (index, element) {
                                tmpQuan = $(this).find('td:eq(3)').html();
                                tmpAmount = Common.ReplaceFormatCurrency($(this).find('td:last').html());
                                prototypeAmount += tmpQuan * tmpAmount;
                            })
                            return Common.FormatCurrency(prototypeAmount);
                        },
                        Discount: function () {
                            return res.Discount;
                        },
                        Promotion: function () {
                            prototypePromotion = (prototypeAmount * res.Discount / 100);
                            return Common.FormatCurrency(prototypePromotion);
                        },
                        Total: function () {
                            return Common.FormatCurrency(prototypeAmount - prototypePromotion);
                        }
                    })
                    $('#divInfoPromotion-orderDetail').html(strInfoPromotion);
                },
                complete: function () {
                    var title = "Chi tiết đơn đặt hàng " + Common.order.orderID;
                    Common.order.TemplateModal(title);
                }
            });
        });

        $('.btnPrint').off('click').on('click', function () {
            Common.order.orderID = $(this).data('id');
            $('#non-printable').hide();
            $('#printable').show();
            $.ajax({
                url: order.Url.GetDetail,
                async: false,
                type: 'POST',
                dataType: 'json',
                data: {
                    orderID: Common.order.orderID
                },
                success: function (res) {
                    var templateInfoOrder = $('#tplInfoOrder-printPreview').html();
                    var templateCartDetail = $('#tplCartDetail-printPreview').html();
                    var tenplateHeader = $('#tplHeader-printPreview').html();
                    var tenplateInfoPromotion = $('#tplInfoPromotion-printPreview').html();

                    var strHeader = '';
                    var strCartDetail = '';
                    var strInfoOrder = '';
                    var strInfoPromotion = '';
                    debugger;
                    //bind header
                    strHeader += Mustache.render(tenplateHeader, {
                        OrderId: Common.order.orderID,
                        CreatedDate: function () {
                            return moment(res.CreatedDate).format('DD/MM/YYYY');
                        }
                    })
                    $('#divHeader').html(strHeader);

                    //bind info customer
                    strInfoOrder += Mustache.render(templateInfoOrder, {
                        ShipName: res.ShipName,
                        ShipEmail: res.ShipEmail,
                        ShipAddress: res.ShipAddress,
                        ShipMobile: res.ShipMobile,
                        PromotionName: res.PromotionName
                    });
                    $('#divInfoOrder-printPreview').html(strInfoOrder);

                    // bind products into tbody (cart)
                    $(res.CartList).each(function (index, element) {
                        strCartDetail += Mustache.render(templateCartDetail, {
                            OrderNumber: index + 1,
                            ProductId: element.ProductId,
                            ProductName: element.ProductName,
                            Quantity: element.Quantity,
                            Price: function () {
                                return Common.FormatCurrency(element.Price);
                            }
                        });
                    })
                    $('#divCartDetail-printPreview').html(strCartDetail);

                    //bind info promotion into footer
                    var prototypePromotion;
                    var prototypeAmount = 0;
                    strInfoPromotion += Mustache.render(tenplateInfoPromotion, {
                        Amount: function () {
                            var tmpQuan;
                            var tmpAmount;
                            $('tbody#divCartDetail-printPreview tr').each(function (index, element) {
                                tmpQuan = $(this).find('td:eq(3)').html();
                                tmpAmount = Common.ReplaceFormatCurrency($(this).find('td:last').html());
                                prototypeAmount += tmpQuan * tmpAmount;
                            })
                            return Common.FormatCurrency(prototypeAmount);
                        },
                        Discount: function () {
                            return res.Discount;
                        },
                        Promotion: function () {
                            prototypePromotion = (prototypeAmount * res.Discount / 100);
                            return Common.FormatCurrency(prototypePromotion);
                        },
                        Total: function () {
                            return Common.FormatCurrency(prototypeAmount - prototypePromotion);
                        }
                    })
                    $('#divInfoPromotion-printPreview').html(strInfoPromotion);
                },
                complete: function () {
                    window.print();
                }
            })
            $('#printable').hide();
            $('#non-printable').show();
        });

        $('#btnExport').off('click').on('click', function () {
            var form = $("#frmOrder");
            form.attr("action", '/Admin/Order/ExportOrderList');
            form.attr("data-ajax", "false");
            form.submit();
            form.attr("action", '/Admin/Order/OrderList');
            form.attr("data-ajax", "true");
        });

        $("#FromDate").datepicker({ format: 'dd/mm/yyyy', autoclose: true }).off('changeDate').
            on('changeDate', function () {
                var from = Common.GetStadardDate($(this).val());
                $(this).val(from);
                Common.order.SubmitForm();
            });

        $('#ToDate').datepicker({ format: 'dd/mm/yyyy', autoclose: true }).off('changeDate').
            on('changeDate', function () {
                var to = Common.GetStadardDate($(this).val());
                $(this).val(to);
                console.log($(this).val());
                Common.order.SubmitForm();
            });

        $('.txtPrice, .txtQuantity').off('keypress keyup')
        .keypress(function (e) {
            Common.ValidateDecimal(e, this);
        })
        .keyup(function () {
            var money = $(this).val();
            money = Common.ReplaceFormatCurrency(money);
            $(this).val(Common.FormatCurrency(money));
            $(this).next().val(Common.ReplaceFormatCurrency(money));

        });

        $(':reset').off('click').on('click', function () {
            window.location.reload();
        });

        $('#PromotionId, .ddlStatusName, #PrecinctId').off('change').on('change', function () {
            Common.order.SubmitForm();
        });

        $('#ddlItemsPerPage').off('change').on('change', function () {
            $('#PageSize').val($(this).val());
            Common.order.SubmitForm();
        });

        $('#ProvinceId').off('change').on('change', function () {
            var id = $(this).val();
            if (id != '') {
                Common.loadDistrict(parseInt(id),'');
                Common.order.SubmitForm();
            }
            else {
                $('#DistrictId').html('');
            }
        });

        $('#DistrictId').off('change').on('change', function () {
            var districtId = $(this).val();
            var provinceId = $('#ProvinceId').val();
            if (districtId != '' && provinceId != '') {
                Common.loadPrecinct(parseInt(provinceId), parseInt(districtId),'');
                Common.order.SubmitForm();
            }
            else {
                $('#DistrictId').html('');
            }
        });
    },

    OrderBy: function (name, sort) {
        debugger;
        $("#OrderBy").val(name);
        $("#SortBy").val(sort);
        Common.order.SubmitForm();
    },

    SubmitForm: function () {
        $("#frmOrder").submit();
    },
    SuccessForm: function () {
        Common.ShowLoading(false);
        var itemsPerPage = $('#PageSize').val();
        $('#ddlItemsPerPage option').each(function (index, element) {
            if ($(element).val() == itemsPerPage) {
                $(element).prop('selected', true);
            }
        })

        Common.order.registerEvents();
    },
    Paging: function (page) {

        Common.order.SetPage(page);
        Common.order.IsPaging = true;
        Common.order.Refresh();
    },
    BeforeSend: function () {
        Common.ShowLoading(true);
        if (Common.order.IsPaging) {
            Common.order.SetPage(1);
        }
    },
    SetPage: function (page) {
        $("#frmOrder").find("input[name='PageCurrent']").val(page);
    },
    Refresh: function (func) {
        Common.order.SubmitForm();
    },

    TemplateModal: function (title) {
        var $confirm = $("#modalConfirmYesNo");
        $confirm.modal('show');
        $("#lblTitleConfirmYesNo").html(title);
        $("#btnYesConfirmYesNo").off('click').on('click', function () {
            $.post('/Admin/Order/ChangeStatus', {
                orderId: Common.order.orderID,
                selectedValue: $('.modal-dialog #StatusId').find(':selected').val()
            }, function (response) {
                //update status name in this row in table
                $('#row_' + Common.order.orderID).find(':eq(4)').html(response);
                $("#modalConfirmYesNo").modal("hide");
            });
        });

        $("#btnNoConfirmYesNo").off('click').click(function () {
            $("#modalConfirmYesNo").modal("hide");
        });
    },
};

//cục bộ 3 nè
var order = order;
order.constructor = order;
Common.order = new order();
