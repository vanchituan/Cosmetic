var contact = {
    init: function () {
        contact.registerEvents();
        google.maps.event.addDomListener(window, 'load', contact.initMap());
    },
    registerEvents: function () {
        $('#btnSend').off('click').on('click', function (e) {
            var name = $('#txtName').val();
            var mobile = $('#txtPhone').val();
            var address = $('#txtAddress').val();
            var email = $('#txtEmail').val();
            var content = $('#txtContent').val();

            $.ajax({
                url: '/Contact/Send',
                type: 'POST',
                dataType: 'json',
                data: {
                    name: name,
                    mobile: mobile,
                    address: address,
                    email: email,
                    content: content
                },
                success: function (res) {
                    if (res.status == true) {
                        alert('gui thanh cong');
                        window.location.href = '/';
                    }
                }
            });
            e.preventDefault();
        });
    },      // end function regEvents

    resetForm: function () {
        $('#txtName').val('');
        $('#txtPhone').val('');
        $('#txtAddress').val('');
        $('#txtEmail').val('');
        $('#txtContent').val('');
    },


    // GOOGLE MAP

    // This example displays a marker at the center of Australia.
    // When the user clicks the marker, an info window opens.
    //10.791919, 106.708327
    initMap: function () {
        var uluru = { lat: 10.791919, lng: 106.708327 };
        var map = new google.maps.Map(document.getElementById('mapCanvas'), {
            zoom: 18,
            center: uluru
        });
        var contentString = '@Html.Raw(Model.Content)';
        var infowindow = new google.maps.InfoWindow({
            content: contentString
        });
        var marker = new google.maps.Marker({
            position: uluru,
            map: map,
            title: 'Địa chỉ'
        });
        marker.addListener('click', function () {
            infowindow.open(map, marker);
        });
    }
}
contact.init();