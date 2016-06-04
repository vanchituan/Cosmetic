var invoice = function () {
    return this.init();
}

invoice.prototype = {
    init: function () {
        this.registerEvents();
    },

    invoiceId: '',

    registerEvents: function () {
        $('.btnInvoiceDetail').off('click').on('click', function () {
            Common.invoice.invoiceId = $(this).data('id');
            $.ajax({
                url: '/Admin/Invoice/GetDetail',
                type: 'POST',
                dataType: 'json',
                data: {
                    invoiceId: Common.invoice.invoiceId
                },
                success: function (res) {
                    var templateInfoInvoice = $('#tplInfoInvoice-invoiceDetail').html();
                    var templateCartDetail = $('#tplCartDetail-invoiceDetail').html();
                    var templateInfoPromotion = $('#tplInfoPromotion-invoiceDetail').html();
                    var strCartDetail = '';
                    var strInfoInvoice = '';
                    var strInfoPromotion = '';

                    //bind info customer
                    strInfoInvoice += Mustache.render(templateInfoInvoice, {
                        InvoiceId: Common.invoice.invoiceId,
                        CreatedDate: function () {
                            return moment(res.CreatedDate).format('DD/MM/YYYY');
                        },
                        PromotionName: res.PromotionName,
                        ShipName: res.ShipName,
                        ShipEmail: res.ShipEmail,
                        ShipAddress: res.ShipAddress,
                        ShipMobile: res.ShipMobile,

                    });
                    $('#divInfoInvoice-invoiceDetail').html(strInfoInvoice);


                    // bind invoice detail into tbody
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
                    $('#divCartDetail-invoiceDetail').html(strCartDetail);

                    //bind info promotion into footer
                    var prototypePromotion;
                    var prototypeAmount = 0;

                    strInfoPromotion += Mustache.render(templateInfoPromotion, {
                        Amount: function () {
                            var tmpQuan;
                            var tmpAmount;
                            $('tbody#divCartDetail-invoiceDetail tr').each(function (index, element) {
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
                    $('#divInfoPromotion-invoiceDetail').html(strInfoPromotion);
                },
                complete: function () {
                    var title = "Chi tiết đơn đặt hàng " + Common.invoice.invoiceId;
                    var $confirm = $("#modalConfirmYesNo");
                    $confirm.modal('show');
                    $("#lblTitleConfirmYesNo").html(title);
                    $("#btnNoConfirmYesNo").off('click').click(function () {
                        $("#modalConfirmYesNo").modal("hide");
                    });
                }
            });
        });

        $('#ddlItemsPerPage').off('change').on('change', function () {
            $('#PageSize').val($(this).val());
            Common.invoice.SubmitForm();
        });

        $("#FromDate").datepicker({ format: 'dd/mm/yyyy', autoclose: true }).off('changeDate').
            on('changeDate', function () {
                var from = Common.GetStadardDate($(this).val());
                $(this).val(from);
                Common.invoice.SubmitForm();
            });

        $('#ToDate').datepicker({ format: 'dd/mm/yyyy', autoclose: true }).off('changeDate').
            on('changeDate', function () {
                var to = Common.GetStadardDate($(this).val());
                $(this).val(to);
                console.log($(this).val());
                Common.invoice.SubmitForm();
            });


        $('#PromotionId').off('change').on('change', function () {
            Common.invoice.SubmitForm();
        });
    },

    OrderBy: function (name, sort) {
        debugger;
        $("#OrderBy").val(name);
        $("#SortBy").val(sort);
        Common.invoice.SubmitForm();
    },

    SubmitForm: function () {
        $("#frmInvoice").submit();
    },
    SuccessForm: function () {
        Common.ShowLoading(false);

        var itemsPerPage = $('#PageSize').val();
        $('#ddlItemsPerPage option').each(function (index, element) {
            if ($(element).val() == itemsPerPage) {
                $(element).prop('selected', true);
            }
        })

        Common.invoice.registerEvents();
    },
    Paging: function (page) {

        Common.invoice.SetPage(page);
        Common.invoice.IsPaging = true;
        Common.invoice.Refresh();
    },
    BeforeSend: function () {
        Common.ShowLoading(true);
        if (Common.invoice.IsPaging) {
            Common.invoice.SetPage(1);
        }
    },
    SetPage: function (page) {
        $("#frmInvoice").find("input[name='PageCurrent']").val(page);
    },
    Refresh: function (func) {
        Common.invoice.SubmitForm();
    },

    InitDatePicker: function () {
        $('.txtDate').datepicker({
            format: 'dd/mm/yyyy'
        });
    },
}

var invoice = invoice;
invoice.constructor = invoice;
Common.invoice = new invoice();