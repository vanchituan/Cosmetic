var profit = function () {
    return this.init();
}

profit.prototype = {
    init: function () {
        //console.log(new Date("05-05-2016".replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3")));
        //console.log(new Date("05-05-2016".replace(/(\d{2})\/(\d{2})\/(\d{4})/, "$2/$1/$3")));

        //var fromDate = new Date($('#FromDate').val().replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3"));
        //var toDate = new Date($('#ToDate').val().replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3"));


        //this.LoadChart(this.toDate('#FromDate'), this.toDate('#ToDate'));
        //var f = new Date(parseInt($("#FromDate").val(), 10));
        //var t = new Date(parseInt($("#ToDate").val(), 10));
        //var f = $("#FromDate").val();
        //var t = $("#ToDate").val();
        //this.LoadChart(f, t);
        this.registerEvents();
        var f = $("#FromDate").val();
        var t = $("#ToDate").val();
        console.log('init date : ' + f);
        console.log('init date : ' + t);
        var fromDate = Common.GetStadardDate(f);
        var toDate = Common.GetStadardDate(t);
        this.LoadChart(fromDate, toDate);

    },

    registerEvents: function () {
        $("#FromDate").datepicker({ format: 'dd/mm/yyyy', autoclose: true }).off('changeDate').
            on('changeDate', function () {
            if (!Common.Empty($("#ToDate").val())) {
                var fromDate = $("#FromDate").val();
                var toDate = $("#ToDate").val();
                Common.profit.LoadChart(Common.GetStadardDate(fromDate), Common.GetStadardDate(toDate));
            }
        });

        $("#ToDate").datepicker({ format: 'dd/mm/yyyy', autoclose: true }).off('changeDate').
            on('changeDate', function () {
            if (!Common.Empty($("#FromDate").val())) {
                var fromDate = $("#FromDate").val();
                var toDate = $("#ToDate").val();
                Common.profit.LoadChart(fromDate, toDate);
            }
        });

        $('.trDateDetail').off('click').on('click', function () {
            var date = $(this).data('date');
            $.ajax({
                url: '/Admin/Report/GetDateDetail',
                type: 'POST',
                data: { date: date },
                success: function (res) {
                    var str = '';
                    var template = $('#tplDateDetail').html();
                    $.each(res, function (index, element) {
                        str += Mustache.render(template, {
                            InvoiceId: element.InvoiceId,
                            CreatedDate: function () {
                                return moment(element.CreatedDate).format('DD/MM/YYYY');
                            },
                            ShippedDate: function () {
                                return moment(element.ShippedDate).format('DD/MM/YYYY');
                            },
                            Total: function () {
                                return Common.FormatCurrency(element.Total);
                            },
                            Profit: function () {
                                return Common.FormatCurrency(element.Profit);
                            },
                            OrderId: element.OrderId
                        })
                    })
                    $('#tbdDateDetail').html(str);
                },
                complete: function () {
                    Common.invoice.registerEvents();
                }
            })
        })
    },

    LoadChart: function (fromDate, toDate) {
        console.log('func : ' + fromDate);
        console.log('func : ' +toDate);
        $.ajax({
            type: 'POST',
            url: '/Admin/Report/GetDataLine',
            dataType: 'json',
            beforeSend: function () {
                Common.ShowLoading(true);
            },
            data: {
                f: fromDate,
                t: toDate
            },
            success: function (result) {
                if (!Common.Empty(result)) {
                    //debugger;
                    var axes = [{
                        type: 'dateTime',
                        location: 'bottom',
                        //zoomEnabled: true,
                        labels: { stringFormat: 'dd/mm/yyyy' },
                        interval: 1,
                        intervalType: 'days'
                    }];

                    var series = [];
                    for (var i = 0; i < result.chart.length; i++) {//chart.lengh = 4
                        var randomColor = '#' + ('000000' + (Math.random() * 0xFFFFFF << 0).toString(16)).slice(-6);
                        var resul = result.chart[i];

                        var item = new Object();// pairs key, val of series
                        item.type = 'line';
                        item.title = resul.title;// clickable, bottom of chart
                        item.fillStyle = randomColor;
                        item.fillStyle = randomColor;
                        item.strokeStyle = randomColor;
                        item.nullHandling = 'connect';
                        item.axisY = resul.title;
                        item.markers = {
                            size: 5,
                            type: 'circle',
                            strokeStyle: randomColor,
                            fillStyle: randomColor,
                            lineWidth: 2
                        };


                        item.data = new Array();
                        for (var j = 0; j < resul.data.length; j++) {// resul is a item of chart eg : doanh thu sau thue
                            var resu = resul.data[j];// resu is per date of chart
                            var tempDataItem = JSON.parse(JSON.stringify(resu), function (key, value) {
                                var a;
                                if (typeof value === 'string') {
                                    a = /\/Date\((\d*)\)\//.exec(value);
                                    if (a) {
                                        var date = new Date(+a[1]);
                                        return date;
                                    }
                                }
                                return value;
                            });
                            var point = [tempDataItem.date, tempDataItem.value];
                            item.data.push(point);
                        }
                        series.push(item);
                    }//end for

                    //bind chart
                    $('#jqChart').jqChart({
                        title: {
                            text: 'Biểu đồ',
                            fillStyle: '#6495ED'
                        },
                        animation: { duration: 1 },
                        legend: { location: 'bottom' },
                        border: { strokeStyle: '#6ba851' },
                        background: {
                            type: 'linearGradient',
                            x0: 0,
                            y0: 0,
                            x1: 0,
                            y1: 1,
                            colorStops: [{ offset: 0, color: '#d2e6c9' },
                                         { offset: 1, color: 'white' }]
                        },
                        tooltips: { type: 'shared' },
                        shadows: {
                            enabled: true
                        },
                        crosshairs: {
                            enabled: true,
                            hLine: false,
                            vLine: { strokeStyle: '#cc0a0c' }
                        },
                        noDataMessage: {
                            text: 'Không có dữ liệu'
                        },
                        axes: axes,
                        series: series
                    });

                    //format tooltip
                    $('#jqChart').bind('tooltipFormat', function (e, data) {
                        if (data.length == undefined) {
                            console.log('data.length == undefined');
                            var date = new Date(data.x);
                            var dd = date.getDate();
                            var mm = date.getMonth() + 1; //January is 0!
                            var yyyy = date.getFullYear();
                            if (dd < 10) {
                                dd = '0' + dd;
                            }
                            if (mm < 10) {
                                mm = '0' + mm;
                            }
                            var color = null;
                            var title = null;
                            if (!Common.Empty(data.series)) {
                                color = data.series.fillStyle;
                                title = data.series.axisY;
                            }
                            var dformat = [dd, mm, yyyy].join('/');
                            return "<b>" + dformat + "</b><br />" +
                                "<font color='" + color + "'>" + title + "</font>" + ": <b>" +
                                Common.FormatCurrency(data.y) + " đ" + "</b><br />";
                        }
                        else {
                            //debugger;
                            var result = null;
                            if ((Object.getPrototypeOf(data) === Object.prototype) == false) {
                                for (var i = 0; i < data.length; i++) {
                                    var date = new Date(data[i].x);
                                    var dd = date.getDate();
                                    var mm = date.getMonth() + 1; //January is 0!
                                    var yyyy = date.getFullYear();
                                    if (dd < 10) {
                                        dd = '0' + dd;
                                    }
                                    if (mm < 10) {
                                        mm = '0' + mm;
                                    }
                                    var dformat = [dd, mm, yyyy].join('/');

                                    var unitarrs = data[i].series.title.split(" ");
                                    var units = unitarrs[unitarrs.length - 1];
                                    if (Common.Empty(result)) {
                                        result = "<b>" + dformat + "</b><br />" +
                                            "<font color='" + data[i].series.fillStyle + "'>" +
                                            data[i].series.axisY + "</font>" + ": <b>" +
                                            Common.FormatCurrency(data[i].y) + " đ" + "</b><br />";
                                    }
                                    else {
                                        result += "<font color='" + data[i].series.fillStyle + "'>" +
                                            data[i].series.axisY + "</font>" + ": <b>" +
                                            Common.FormatCurrency(data[i].y) + " đ" + "</b><br />";
                                    }
                                }
                            }
                            return result;
                        }
                    });//end tooltip Format

                    //bind table (detail of date)
                    var str = '';
                    var template = $('#tplDateList').html();
                    $.each(result.table, function (index, element) {
                        str += Mustache.render(template, {
                            Date: function () {
                                return moment(element.Date).format('DD/MM/YYYY');
                            },
                            Revenue: function () {
                                return Common.FormatCurrency(element.Revenue);
                            },
                            Profit: function () {
                                return Common.FormatCurrency(element.Profit);
                            }
                        })
                    })

                    $('tbody#tbdDateList').html(str);
                }
                //end if
            },
            complete: function () {
                Common.ShowLoading(false);
                Common.profit.registerEvents();
            }
        })
    },
}

var profit = profit;
profit.constructor = profit;
Common.profit = new profit();