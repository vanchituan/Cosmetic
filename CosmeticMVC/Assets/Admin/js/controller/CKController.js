var ck = {
    init: function () {
        ck.registerEvents();
        ck.funcReplace();

    },
    registerEvents: function () {
        $('#btnSelectImage').off('click').on('click', function () {
            var finder = new CKFinder();
            //finder.basePath = '../';
            finder.selectActionFunction = ck.SetFileField;
            finder.popup();
        });
    },

    SetFileField: function (fileUrl) {
        $('#txtImage').val(fileUrl);
    },

    funcReplace: function () {
        if ($('#Detail').length != 0) {
            CKEDITOR.replace('Detail', {
                customConfig: '/Assets/Admin/js/plugins/ckeditor/config.js',
            });
        }
    }
}
ck.init();