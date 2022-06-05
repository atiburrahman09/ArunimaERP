Date.prototype.addDays = function (days) {
    var dat = new Date(this.valueOf())
    dat.setDate(dat.getDate() + days);
    return dat;
};

function getDates(startDate, stopDate) {
    var dateArray = new Array();
    var currentDate = startDate;
    while (currentDate <= stopDate) {
        dateArray.push(dateFormat(new Date(currentDate)));
        currentDate = currentDate.addDays(1);
    }
    return dateArray;
};

function formatDate(date) {
    var m_names = new Array("Jan", "Feb", "Mar",
                            "Apr", "May", "Jun", "Jul", "Aug", "Sep",
                            "Oct", "Nov", "Dec");

    var d = new Date(date);
    var date = d.getDate();
    var month = d.getMonth();
    var year = d.getFullYear();

    return date + "-" + m_names[month] + "-" + year;
};

function checkValidPlan(productionPlanningID, purchaseOrderID, startDate, floorLineID) {
    var ret = false;
    $.ajax({
        async: false,
        url: "/production/productionplan/CheckValidPlan",
        data: {
            "productionPlanningID": productionPlanningID,
            "purchaseOrderID": purchaseOrderID,
            "startDate": startDate,
            "floorLineID": floorLineID
        },
        success: function (res) {
            if (res === true)
                ret = true;
        }
    });
    return ret;
};

function reschedulePlan(productionPlanningID, startDate, endDate, floorLineID, isResize) {

    $.blockUI({ message: "<b>Please Wait...</b>" });
    $.ajax({
        url: "/production/productionplan/ReschedulePlan",
        type: 'GET',
        data: {
            "productionPlanningID": productionPlanningID,
            "startDate": startDate,
            "endDate": endDate,
            "floorLineID": floorLineID,
            "isResize": isResize
        },
        success: function (res) {
            if (res == true) {
                $('#productionPlanCalendar').fullCalendar('refetchEvents');
                $.unblockUI();
            } else {
                revertFunc();
                $.unblockUI();
            }
        },
        error: function (err) {
            revertFunc();
            $.unblockUI();
        }
    });
};

$(document).ready(function () {

    /* Load PO By Style */
    $('#cboStyle').on('change', function () {
        if (!$('#cboFactory').val()) {
            $(this).val('');
            alertify.alert('<b class="alert-error-msg">Please select a factory</b>');
            return;
        }

        if (!$('#fromDate').val() || !$('#toDate').val()) {
            $(this).val('');
            alertify.alert('<b class="alert-error-msg">Please select date range</b>');
            return;
        }

        $.ajax({
            url: "/production/productionplan/GetPurchaseOrderByStyle",
            data: {
                styleID: $(this).val(),
                factoryID: $('#cboFactory').val(),
                fromDate: dateFormat($('#fromDate').val()).toString(),
                toDate: dateFormat($('#toDate').val()).toString()
            },
            success: function (res) {
                if (res.length > 0) {
                    $('#purchaseOrderContainer').html('');
                    for (var i in res) {
                        $('#purchaseOrderContainer')
                            .append(
                            '<div data-purchaseOrderID="' + res[i].Value + '" class="external-po">' + res[i].Text + '</div>'
                            );
                    }
                } else {
                    $('#purchaseOrderContainer').html('');
                    $('#cboStyle').val('');
                    alertify.alert("<b class='alert-error-msg'>There is no purchase order in this date range</b>");
                }
                
                
                /*Start of External Dragging */
                var eventObject = {
                    title: $.trim($('.external-po').text())
                };
                $('.external-po').data('eventObject', eventObject);
                $('.external-po').draggable({
                    zIndex: 999,
                    revert: true,
                    revertDuration: 0
                });
            }
        });
    });

    $('#btnSubmitSearch').on('click', function () {
        if (!$('#cboFloor').val() || !$('#fromDate').val() || !$('#toDate').val()) {
            alertify.alert('<b class="alert-error-msg">Please select floor, start date and end date</b>');
            return;
        }
        $.ajax({
            url: '/production/productionplan/GetStyleByDateRange',
            data: {
                factoryID: $('#cboFactory').val(),
                fromDate: dateFormat($('#fromDate').val()).toString(),
                toDate: dateFormat($('#toDate').val()).toString()
            },
            success: function (res) {
                $('#cboStyle').empty();
                $('#purchaseOrderContainer').html('');
                $('#cboStyle').append($('<option/>', {
                    value: "",
                    text: ""
                }));
                $.each(res, function (index, itemData) {
                    $('#cboStyle').append($('<option/>', {
                        value: itemData.Value,
                        text: itemData.Text
                    }));
                });
            }
        });
        $('#productionPlanCalendar').html('');
        var calendar = $('#productionPlanCalendar').fullCalendar({
            header: {
                left: 'prev,next today',
                center: 'title',
            },
            defaultView: 'resourceMonth',
            firstDay: 1,
            editable: true,
            selectable: false,
            droppable: true,
            drop: function (date, allDay, ev, ui, res) {
                var $this = $(this);
                $.blockUI({ message: "<b>Please Wait...</b>" });
                if (checkValidPlan(-1, $this.data('purchaseorderid'), dateFormat(new Date(date)).toString(), res.id) === true) {
                    $.ajax({
                        url: "/production/productionplan/GetCreatePopUp",
                        type: 'GET',
                        data: {
                            "purchaseOrderID": $this.data('purchaseorderid')
                        },
                        success: function (res) {
                            $.unblockUI();
                            $('<div id="productionPlanPopUp"></div>').append(res).dialog({
                                title: 'Create Production Plan',
                                show: {
                                    effect: 'fade',
                                    duration: 1000
                                },
                                hide: {
                                    effect: 'fade',
                                    duration: 1000
                                },
                                width: '30%',
                                close: function () { $(this).remove() },
                                modal: true,
                                draggable: true,
                                position: ['top', 100],
                                resizable: false,
                            });
                        }
                    });
                    var createProductionPlan = $('#createProductionPlan');
                    createProductionPlan.live('click', function () {
                        var lineCapacity = $('#LineCapacity').val();
                        var lineQuantity = $('#LineQuantity').val();
                        if (isNaN(lineCapacity) || isNaN(lineQuantity)) {
                            alertify.alert("<b class='alert-error-msg'>Sorry, Please insert valid number to line capacity or line quantity</b>");
                        } else {
                            if (lineCapacity > 0 && lineQuantity > 0) {
                                $.blockUI({ message: "<b>Please Wait...</b>" });
                                $.ajax({
                                    url: "/production/productionplan/create",
                                    type: 'GET',
                                    data: {
                                        "purchaseOrderID": $this.data('purchaseorderid'),
                                        "startDate": dateFormat(new Date(date)).toString(),
                                        "floorLineID": res.id,
                                        "lineQuantity": lineQuantity,
                                        "lineCapacity": lineCapacity
                                    },
                                    success: function (res) {
                                        $.unblockUI();
                                        createProductionPlan.die('click');
                                        $('#productionPlanPopUp').dialog("close");
                                        if (res == true) {
                                            $this.remove();
                                            $('#productionPlanCalendar').fullCalendar('refetchEvents');
                                        }
                                    }
                                });
                            } else {
                                alertify.alert("<b class='alert-error-msg'>Sorry, Zero(0) or Negetive Value as Line Quantity or Line Capacity is not accepted</b>");
                            }
                        }
                    });
                } else {
                    $.unblockUI();
                    alertify.alert('<b class="alert-error-msg">Sorry! You cant place this po here</b>');
                }
                
            },
            minTime: 8,
            maxTime: 24,
            month: $('#fromDate').val().split('/')[0] - 1,//$('#cboStartDate').val(),
            year: $('#fromDate').val().split('/')[2],//$('#txtYear').val(),
            selectHelper: true,
            resources: "/Production/ProductionPlan/GetLineByFloor?floor=" + $('#cboFloor').val(),
            events: function (start, end, callback) {
                $.ajax({
                    url: '/production/productionplan/GetProductionPlanByMonth',
                    data: {
                        month: start.getMonth() + 1, //parseInt($('#cboStartDate').val()) + 1,
                        year: start.getFullYear() //$('#txtYear').val()
                    },
                    success: function (res) {
                        callback(res);
                    }
                });
                
            },
            select: function (start, end, allDay, jsEvent, view, resource) {
                alertify.prompt('event title:', function (e, str) {
                    if (e) {
                        calendar.fullCalendar('renderEvent',
                        {
                            title: str,
                            start: start,
                            end: end,
                            allDay: allDay,
                            resource: resource.id
                        },
                        true // make the event "stick"
                    );
                    }
                });
                calendar.fullCalendar('unselect');
            },
            resourceRender: function (resource, element, view) {
                // this is triggered when the resource is rendered, just like eventRender

            },
            eventDrop: function (event, dayDelta, minuteDelta, allDay, revertFunc, jsEvent, ui, view) {
                var floorLineID = typeof (event.resource) === 'object' ? event.resource[0] : event.resource;
                if (checkValidPlan(event.ProductionPlanningID, event.PoStyleID, dateFormat(new Date(event.start)).toString(), floorLineID)) {
                    reschedulePlan(event.ProductionPlanningID, dateFormat(new Date(event.start)).toString(), dateFormat(new Date(event.end)).toString(), floorLineID, 0);
                } else {
                    revertFunc();
                    alertify.alert('<b class="alert-error-msg">Sorry! You cant place this po here</b>');
                }
            },
            eventResize: function (event, dayDelta, minuteDelta, allDay, revertFunc, jsEvent, ui, view) {
                var floorLineID = typeof (event.resource) === 'object' ? event.resource[0] : event.resource;
                reschedulePlan(event.ProductionPlanningID, dateFormat(new Date(event.start)).toString(), dateFormat(new Date(event.end)).toString(), floorLineID, 1);
            },
            eventMouseover: function (event, jsEvent) {
                //$(this).tooltip({
                //    title: 'Style No: ' + event.StyleNo
                //            + '<br />Start Date: ' + formatDate(event.start)
                //            + '<br />End Date: ' + formatDate(event.end)
                //            + '<br />Line Quantity: ' + event.Quantity
                //            + '<br />Capacity: ' + event.Capacity,
                //    placement: 'top',
                //    container: 'body',
                //    html: true,
                //});
            },
            eventClick: function (event, jsEvent, view) {
                console.log(event.start);
                console.log(event.end);
                console.log(event.ProductionPlanningID);
                //alertify.alert('event ' + event.title + ' was left clicked' + "Production Plan ID " + event.ProductionPlanningID);
            },
            eventRender: function (event, element, view) {
                element.bind('contextmenu', function () {
                    context.init({
                        fadeSpeed: 100,
                        filter: function ($obj) { },
                        above: 'auto',
                        preventDoubleContext: true,
                        compress: false
                    });
                    context.attach('.fc-event', [
                         {
                             id: 'purchaseOrderDetail',
                             text: 'Purchase Order Details',
                             action: function (e) {
                                 e.preventDefault();
                                 $.blockUI({ message: "<b>Please Wait...</b>" });
                                 $.ajax({
                                     url: '/Production/ProductionPlan/GetPurchaseOrderDetails',
                                     data: { 'purchaseOrderID': event.PoStyleID },
                                     success: function (res) {
                                         $.unblockUI();
                                         $('<div></div>').append(res).dialog({
                                             title: 'Purchase Order Details',
                                             show: {
                                                 effect: 'fade',
                                                 duration: 1000
                                             },
                                             hide: {
                                                 effect: 'fade',
                                                 duration: 1000
                                             },
                                             width: '80%',
                                             close: function () { $(this).remove() },
                                             modal: true,
                                             draggable: true,
                                             position: ['top', 100],
                                             resizable: false,
                                         });
                                     }
                                 });
                             }
                         },
                         {
                             id: 'productionStatus',
                             text: 'Production Status',
                             action: function (e) {
                                 e.preventDefault();
                                 $.blockUI({ message: "<b>Please Wait...</b>" });
                                 var xAxisDate = [];
                                 var yAxisTodaySewing = [];
                                 var yAxisCutting = [];
                                 $.ajax({
                                     url: '/Production/ProductionPlan/GetDailyProductionStatus',
                                     data: { 'purchaseOrderID': event.PoStyleID },
                                     success: function (res) {
                                         $.unblockUI();
                                         if (res.length > 0) {
                                             for (var i in res) {
                                                 xAxisDate.push(res[i].DateInString);
                                                 yAxisTodaySewing.push(res[i].TodaySewing);
                                                 yAxisCutting.push(res[i].Cutting);
                                             }
                                         }
                                     }
                                 }).promise().done(function () {
                                     $('<div id="productionStatusGraph"></div>').dialog({
                                         title: 'Daily Production Status',
                                         width: '80%',
                                         height: 400,
                                         close: function () { $(this).remove() },
                                         modal: true,
                                         draggable: true,
                                         position: ['top', 100],
                                         resizable: false,
                                     });
                                     $('#productionStatusGraph').highcharts({
                                         title: {
                                             text: 'Daily Production Status',
                                             x: -20 //center
                                         },
                                         xAxis: {
                                             categories: xAxisDate
                                         },
                                         yAxis: {
                                             title: {
                                                 text: 'Piece'
                                             },
                                             plotLines: [{
                                                 value: 0,
                                                 width: 1,
                                                 color: '#808080'
                                             }]
                                         },
                                         tooltip: {
                                             valueSuffix: ' piece'
                                         },
                                         legend: {
                                             layout: 'vertical',
                                             align: 'right',
                                             verticalAlign: 'middle',
                                             borderWidth: 0
                                         },
                                         series: [
                                             { name: 'Today Sewing', data: yAxisTodaySewing },
                                             { name: 'Cutting', data: yAxisCutting },
                                         ]
                                     });
                                 });
                                 
                             }
                         },
                         {
                             id: 'productionStatistics',
                             text: 'Production Statistics',
                             action: function (e) {
                                 e.preventDefault();
                                 $.blockUI({ message: "<b>Please Wait...</b>" });
                                 $.ajax({
                                     url: '/Production/ProductionPlan/GetProductionStatistics',
                                     data: { 'purchaseOrderID': event.PoStyleID },
                                     success: function (res) {
                                         $.unblockUI();
                                         $('<div></div>').append(res).dialog({
                                             title: 'Production Statistics',
                                             show: {
                                                 effect: 'fade',
                                                 duration: 1000
                                             },
                                             hide: {
                                                 effect: 'fade',
                                                 duration: 1000
                                             },
                                             width: '80%',
                                             close: function () { $(this).remove() },
                                             modal: true,
                                             draggable: true,
                                             position: ['top', 100],
                                             resizable: false,
                                         });
                                     }
                                 });
                             }
                         },
                         {
                             id: 'rawMaterialStatus',
                             text: 'Raw Material Status',
                             action: function (e) {
                                 e.preventDefault();
                                 $.blockUI({ message: "<b>Please Wait...</b>" });
                                 $.ajax({
                                     url: '/Production/ProductionPlan/GetRawMaterialStatus',
                                     data: { 'purchaseOrderID': event.PoStyleID },
                                     success: function (res) {
                                         $.unblockUI();
                                         $('<div></div>').append(res).dialog({
                                             title: 'Raw Material Status',
                                             show: {
                                                 effect: 'fade',
                                                 duration: 1000
                                             },
                                             hide: {
                                                 effect: 'fade',
                                                 duration: 1000
                                             },
                                             width: '80%',
                                             close: function () { $(this).remove() },
                                             modal: true,
                                             draggable: true,
                                             position: ['top', 100],
                                             resizable: false,
                                         });
                                     }
                                 });
                             }
                         },
                         {
                             id: 'splitProductionPlan',
                             text: 'Split Production Plan',
                             action: function (e) {
                                 e.preventDefault();
                                 var conf = confirm("Are you want to sure to split this plan?");
                                 if (conf) {
                                     $.blockUI({ message: "<b>Please Wait...</b>" });
                                     $.ajax({
                                         url: '/Production/ProductionPlan/SplitProductionPlan',
                                         data: { 'productionPlanningID': event.ProductionPlanningID },
                                         success: function (res) {
                                             if (res == true) {
                                                 $('#productionPlanCalendar').fullCalendar('refetchEvents');
                                             }
                                             $.unblockUI();
                                         }
                                     });
                                 }
                             }
                         },
                         {
                             id: 'removeProductionPlan',
                             text: 'Remove Production Plan',
                             action: function (e) {
                                 e.preventDefault();
                                 var conf = confirm("Are you want to sure remove Purchase Order No: " + event.PurchaseOrderNo);
                                 
                                 if (conf) {
                                     $.blockUI({ message: "<b>Please Wait...</b>" });
                                     $.ajax({
                                         url: '/Production/ProductionPlan/Delete',
                                         data: { 'productionPlanningID': event.ProductionPlanningID },
                                         success: function (res) {
                                             if (res == true) {
                                                 $('#productionPlanCalendar').fullCalendar('refetchEvents');
                                             }
                                             $.unblockUI();
                                         }
                                     });
                                 }
                             }
                         },
                    ]);
                });

            },
            windowResize: function (view) {
                calendar.fullCalendar('option', 'height', $(window).height() - 40);
            }
        });
    });
});