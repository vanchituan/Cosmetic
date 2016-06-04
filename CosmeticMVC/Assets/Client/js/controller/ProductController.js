var remain = '';
var product = {
    init: function () {
        product.registerEvents();
    },
    registerEvents: function () {
        $('body').on('click', '#btnLoadMore', function (e) {
            debugger;
            var href = $(this).attr('href');
            var hrefLength = href.length;
            var pageCurrent = href.charAt(hrefLength - 1);
            var page = parseInt(pageCurrent) + 1;
            var str = '/Product/Index?page=' + page;
            $(this).attr('href', str);
            //product.FuncXProduct(page, function (res) {
            //    remain = res.data;
            //});
            product.FuncXProduct(page, product.FuncCallBack);
            $.ajax({
                url: str,
                success: function (result) {
                    $('#btnLoadMore').html('Xem thêm ' + remain + ' sản phẩm');
                    $('#listProduct').html(result);
                },
                beforeSend: function () {
                    $('#btnLoadMore').html('<span class="glyphicon glyphicon-refresh glyphicon-refresh-animate"></span> Đang tải..');
                }
            });
            e.preventDefault();
        });

        
    },
    FuncXProduct: function (page, callback) {
        $.ajax({
            type: 'POST',
            url: '/Product/Remain',
            dataType: 'json',
            data: { page: page },
            success: function (res) {
                callback(res);
            }
        });
    },
    FuncCallBack: function (res) {
        if (res.data == 0) {
            $('#btnLoadMore').hide('slow');
        }
        else {
            remain = res.data;
        }
    },
}
product.init();