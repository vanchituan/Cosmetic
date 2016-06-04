var product = function () {
    return this.init();
}
product.prototype = {
    init: function () {
        this.InitDatePicker();
        this.registerEvents();
    },
    registerEvents: function () {
        var IsPaging = false;
        $('.btnStatus').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: "/Admin/Product/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        btn.text('Đang bán');
                    }
                    else {
                        btn.text('Ngừng k.doanh');
                    }
                }
            });
        });

        $('.txtPrice, .txtQuantity').off('keypress keyup')
        .keypress(function (e) {
            Common.ValidateDecimal(e, this);
        })
        .keyup(function () {
            var money = $(this).val();
            //if (!Common.Empty(money)) {
            money = Common.ReplaceFormatCurrency(money);
            $(this).val(Common.FormatCurrency(money));
            $(this).next().val(Common.ReplaceFormatCurrency(money));
            //}
        });

        $(':reset').off('click').on('click', function () {
            window.location.reload();
        });

        $('#CategoryId').off('change').on('change', function () {
            var form = $("#frmProduct");
            //form.attr("data-ajax", "false");
            form.submit();
            
            //form.attr("data-ajax", "true");
        });

        $('#Status').off('change').on('change', function () {
            Common.product.SubmitForm();
        });

        $('#ddlItemsPerPage').off('change').on('change', function () {
            $('#PageSize').val($(this).val());
            Common.product.SubmitForm();
        });
    },

    OrderBy: function (name, sort) {
        debugger;
        $("#frmProduct").find('#OrderBy').val(name);
        $("#frmProduct").find('#SortBy').val(sort);
        Common.product.SubmitForm();
    },

    SubmitForm: function () {
        $("#frmProduct").submit();
        //if (!Common.Empty(Common.user)) {
        //    Common.customOrder.registerEvents();
        //}
    },

    SuccessForm: function () {
        Common.ShowLoading(false);

        var itemsPerPage = $('#PageSize').val();
        $('#ddlItemsPerPage option').each(function (index, element) {
            if ($(element).val() == itemsPerPage) {
                $(element).prop('selected', true);
            }
        })

        Common.product.registerEvents();
        if (!Common.Empty(Common.customOrder)) {
            Common.customOrder.registerEvents();
        }
    },

    Paging: function (page) {
        Common.product.SetPage(page);
        Common.product.IsPaging = true;
        Common.product.Refresh();
    },

    BeforeSend: function () {
        Common.ShowLoading(true);
        if (Common.product.IsPaging) {
            Common.product.SetPage(1);
        }
    },

    SetPage: function (page) {
        $("#frmProduct").find("input[name='PageCurrent']").val(page);
    },

    Refresh: function (func) {
        Common.product.SubmitForm();
    },

    InitDatePicker: function () {
        $('.txtDate').datepicker({
            dateFormat: "dd/mm/yy",
        });
    },
};

var product = product;
product.constructor = product;
Common.product = new product();
