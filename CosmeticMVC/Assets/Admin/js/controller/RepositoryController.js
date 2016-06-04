var product = {
    init: function () {
        product.PrependCategory();
        product.registerEvents();
    },
    registerEvents: function () {
        $('#txtQuantity').keypress(function (evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode != 46 && charCode > 31
              && (charCode < 48 || charCode > 57))
                return false;
            return true;
        });

        $('#ddlCategory').off('change').on('change', function () {
            $.ajax({
                url: '/Admin/Product/LoadProductsByCateId',
                type: 'POST',
                dataType: 'json',
                data: {
                    cateID: $(this).val()
                },
                success: function (response) {
                    var list = response;
                    var html = '<option disabled selected> --- Chọn sản phẩm để nhập hàng --- </option>'
                    $(list).each(function () {
                        html += '<option value=' + this.Id + '>' + this.Name + '</option>';
                    });
                    $('#ddlProduct').html(html);
                }
            })
        });
        $('#ddlProduct').off('change').on('change', function () {
            $.ajax({
                url: '/Admin/Product/GetProductById',
                type: 'POST',
                dataType: 'json',
                data: {
                    productId: $(this).val()
                },
                success: function (response) {
                    var product = response;
                    var html = '';
                    html += '<tr>';
                    html += '<td>' + product.Name + '</td>';
                    html += '<td>' + product.Quantity + '</td>';
                    html += '</tr>';

                    $('.table tbody').html(html);
                    $('.table-responsive').show();
                }
            });
        });

        $('#btnOK').off('click').on('click', function () {
            $.ajax({
                url: '/Admin/Product/UpdateWareHouse',
                type: 'POST',
                dataType: 'json',
                data: {
                    productId: $('#ddlProduct').val(),
                    quantity : $('#txtQuantity').val()
                },
                success: function (response) {
                    var product = response.product;
                    var html = '';
                    html += '<tr>';
                    html += '<td>' + product.Name + '</td>';
                    html += '<td>' + product.Quantity + '</td>';
                    html += '</tr>';

                    $('.table tbody').html(html);
                    $('.table-responsive').show();
                }
            });
        });
    },
    PrependCategory: function () {
        var html = '<option selected disabled> --- Chọn loại sản phẩm --- </option>'
        $('#ddlCategory').prepend(html);
    }
}
product.init();0