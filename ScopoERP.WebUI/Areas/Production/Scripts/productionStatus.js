$('#Floor').live('change', function () {

    var floor = $(this).val();
    var line = $('#Line');

    $.get("/Production/ProductionStatus/GetLineByFloor", { floor: floor },
        function (data) {
            line.empty();
            line.append($('<option/>', {
                value: "",
                text: ""
            }));
            $.each(data, function (index, itemData) {
                line.append($('<option/>', {
                    value: itemData.ValueString,
                    text: itemData.Text
                }));
            })
        });
});

$('#StyleID').live('change', function () {

    var styleID = $(this).val();
    var purchaseOrderID = $('#PurchaseOrderID');

    $.get("/Production/ProductionStatus/GetPurchaseOrderByStyle", { styleID: styleID },
        function (data) {
            purchaseOrderID.empty();
            purchaseOrderID.append($('<option/>', {
                value: "",
                text: ""
            }));
            $.each(data, function (index, itemData) {
                purchaseOrderID.append($('<option/>', {
                    value: itemData.Value,
                    text: itemData.Text
                }));
            })
        });
});

$('#PurchaseOrderID').live('change', function () {

    $("#OrderQuantity").val("");

    $("#Cutting").parent().next("span").text(".");
    $("#TodayFinish").parent().next("span").text(".");
    $("#TodaySewing").parent().next("span").text(".");
    $("#SewingInput").parent().next("span").text(".");
    $("#SentPrintEmb").parent().next("span").text(".");
    $("#ReceivedPrintEmb").parent().next("span").text(".");
    $("#SentWash").parent().next("span").text(".");
    $("#ReceivedWash").parent().next("span").text(".");

    var purchaseOrderID = $(this).val();

    $.get("/Production/ProductionStatus/GetOrderQuantityByPurchaseOrder", { purchaseOrderID: purchaseOrderID },
        function (data) {
            $("#OrderQuantity").val(data.OrderQuantity);

            var exitDate = new Date(parseInt(data.ExitDate.substr(6)));

            console.log(exitDate);

            $("#ExitDate").val($.datepicker.formatDate('dd-M-yy', exitDate));
        });

    $.get("/Production/ProductionStatus/GetTotalProductionStatusByPurchaseOrder", { purchaseOrderID: purchaseOrderID },
        function (data) {
            $("#Cutting").parent().next("span").text(data.Cutting);
            $("#TodayFinish").parent().next("span").text(data.TodayFinish);
            $("#TodaySewing").parent().next("span").text(data.TodaySewing);
            $("#SewingInput").parent().next("span").text(data.SewingInput);
            $("#SentPrintEmb").parent().next("span").text(data.SentPrintEmb);
            $("#ReceivedPrintEmb").parent().next("span").text(data.ReceivedPrintEmb);
            $("#SentWash").parent().next("span").text(data.SentWash);
            $("#ReceivedWash").parent().next("span").text(data.ReceivedWash);
        });
});

