var user = function () {
    return this.init();
}

user.prototype = {
    init: function () {
        Common.loadProvince('');
        this.registerEvents();
    },
    registerEvents: function () {

        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: "/Admin/User/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        btn.text('Kích hoạt');
                    }
                    else {
                        btn.text('Khoá');
                    }
                }
            });
        });

        $('.btnChange').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: "/Admin/User/ChangeGroup",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    if (response.status == true) {
                        btn.text('MEMBER');
                    }
                    else {
                        btn.text('MOD');
                    }
                }
            });
        });

        $('#PromotionId').off('change').on('change', function () {
            Common.user.SubmitForm();
        });

        $('#UserGroupId').off('change').on('change', function () {
            Common.user.SubmitForm();
        });

        $('input[type=checkbox]').off('change').on('change', function () {
            Common.user.SubmitForm();
        });

        $('#Status, #PrecinctId').off('change').on('change', function () {
            Common.user.SubmitForm();
        });

        $('#ddlItemsPerPage').off('change').on('change', function () {
            $('#PageSize').val($(this).val());
            Common.user.SubmitForm();
        });

        $('#ProvinceId').off('change').on('change', function () {
            var id = $(this).val();
            if (id != '') {
                Common.loadDistrict(parseInt(id),'');
                Common.user.SubmitForm();
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
                Common.user.SubmitForm();
            }
            else {
                $('#DistrictId').html('');
            }
        });

    },
    OrderBy: function (name, sort) {
        debugger;
        $("#frmUser").find('#OrderBy').val(name);
        $("#frmUser").find('#SortBy').val(sort);
        Common.user.SubmitForm();
    },

    SubmitForm: function () {
        $("#frmUser").submit();
    },
    SuccessForm: function () {
        Common.ShowLoading(false);

        var itemsPerPage = $('#PageSize').val();
        $('#ddlItemsPerPage option').each(function (index, element) {
            if ($(element).val() == itemsPerPage) {
                $(element).prop('selected', true);
            }
        })

        Common.user.registerEvents();
        if (!Common.Empty(Common.customOrder)) {
            Common.customOrder.registerEvents();
        }
    },
    Paging: function (page) {
        Common.user.SetPage(page);
        Common.user.IsPaging = true;
        Common.user.Refresh();
    },
    BeforeSend: function () {
        Common.ShowLoading(true);
        if (Common.user.IsPaging) {
            Common.user.SetPage(1);
        }
    },
    SetPage: function (page) {
        $("#frmUser").find("input[name='PageCurrent']").val(page);
    },
    Refresh: function (func) {
        Common.user.SubmitForm();
    },
}
var user = user;
user.constructor = user;
Common.user = new user();
