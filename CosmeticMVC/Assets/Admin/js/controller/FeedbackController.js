$(document).ready(function () {
    var feed = {
        init: function () {
            feed.registerEvents();
        },
        registerEvents: function () {
            $('.btn-active').off('click').on('click', function (e) {
                e.preventDefault();
                var btn = $(this);
                var id = btn.data('id');
                $.ajax({
                    url: "/Admin/Feedback/ChangeStatus",
                    data: { id: id },
                    dataType: "json",
                    type: "POST",
                    success: function (response) {
                        console.log(response);
                        if (response.status == true) {
                            btn.text('Đã duyệt');
                        }
                        else {
                            btn.text('Chưa xem');
                        }
                    }
                });
            });
        }
    }
    feed.init();
});