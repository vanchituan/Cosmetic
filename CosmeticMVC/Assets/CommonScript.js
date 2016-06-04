var Common = function () {
    return this.init();

}

Common.prototype = {
    init: function () {

        //this.registerEvents();
    },
    registerEvents: function () {

    },

    ValidateDecimal: function (event, target) {
        var keyCode = event.charCode || event.keyCode || event.which;
        if (keyCode >= 48 && keyCode <= 57) {
            var text = $(target).val();
        } else {
            if (!Common.Empty(String.fromCharCode(keyCode)) && !Common.Keys[keyCode]) {
                event.preventDefault();
            }
        }
    },

    Keys: {
        8: true,
        35: true,
        36: true,
        37: true,
        39: true,
    },
    ReplaceFormatCurrency: function (num) {
        if (!Common.Empty(num)) {
            var str = num.toString();
            var decPoint = ",";
            if (str.indexOf(decPoint) >= 0) {
                str = str.replace(new RegExp(decPoint, "g"), "");
                return str;
            }
        }
        return num;
    },

    //format number(money)
    FormatCurrency: function (num) {
        if (!Common.Empty(num) && num != null) {
            num = Common.ReplaceFormatCurrency(num);
            var str = Math.round(num).toString();
            var parts = false;
            var output = [];
            var i = 1;
            var sub = "";
            var formatted = null;
            if (str.indexOf(".") > 0) {
                parts = str.split(".");
                str = parts[0];
            }
            if (str.indexOf("-") != -1) {
                sub = str.substr(0, 1);
                str = str.substr(1);
            }
            str = str.split("").reverse();
            for (var j = 0, len = str.length; j < len; j++) {
                if (str[j] != ",") {
                    output.push(str[j]);
                    if (i % 3 == 0 && j < (len - 1)) {
                        output.push(",");
                    }
                    i++;
                }
            }
            formatted = output.reverse().join("");
            if (!Common.Empty(sub)) {
                return (sub + formatted + ((parts) ? "." + parts[1].substr(0, 2) : ""));
            }
            else {
                return (formatted + ((parts) ? "." + parts[1].substr(0, 2) : ""));
            }
        }
        return 0;
    },

    ShowLoading: function (isShow) {
        if (isShow) {
            $(".loading").show();
        } else {
            $(".loading").hide();
        }
    },

    Empty: function (target) {
        if (target == null || target == undefined || target == "" || target.length == 0 || jQuery.trim(target).length == 0)
            return true;
        return false;
    },

    GetStadardDate: function (date) {
        var from = date.split('/');
        var d = from[0];
        var m = from[1];
        var y = from[2];
        var date = m + '/' + d + '/' + y;

        return date;
    },

    loadProvince: function (parentSelector) {
        $.ajax({
            url: '/User/LoadProvince',
            type: "POST",
            dataType: "json",
            success: function (response) {
                if (response.status == true) {
                    var html = '<option value="">--Chọn tỉnh thành--</option>';
                    var data = response.data;
                    $(data).each(function () {        //$.each(data, function (i, item) {
                        html += '<option value="' + this.ProvinceId + '">' + this.ProvinceName + '</option>'
                    });
                    $(parentSelector + '' + '#ProvinceId').html(html);
                }
            }
        })
    },

    loadDistrict: function (id, parentSelector) {

        $.ajax({
            url: '/User/LoadDistrict',
            type: "POST",
            data: { provinceId: id },
            dataType: "json",
            success: function (response) {
                if (response.status == true) {
                    var html = '<option value="">--Chọn quận huyện--</option>';
                    var data = response.data;
                    $(data).each(function () {      //$.each(data, function (i, item) {
                        html += '<option value="' + this.DistrictId + '">' + this.DistrictName + '</option>'
                    });
                    $(parentSelector + '' + '#DistrictId').html(html);
                }
            }
        })
    },

    loadPrecinct: function (provinceId, districtId, parentSelector) {
        $.ajax({
            url: '/User/LoadPrecincts',
            type: "POST",
            data: {
                provinceId: provinceId,
                districtId: districtId
            },
            dataType: "json",
            success: function (response) {

                var html = '<option value="">--Chọn phường xã--</option>';
                $(response).each(function () {      //$.each(data, function (i, item) {
                    html += '<option value="' + this.PrecinctId + '">' + this.PrecinctName + '</option>'
                });
                $(parentSelector + '' + '#PrecinctId').html(html);
            }
        })
    },

};
this.Common = window.Common = new Common();