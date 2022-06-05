// Chalan Create
var formIndexing = function () {
    for (var i = 0; i < $('#shipment-table tbody tr').length; i++) {
        $('#shipment-table tbody').children().eq(i).children().children()
            .each(function () {
                if (this.name) {
                    this.name = this.name.replace(/\[(\d+)\]/, function (str, p1) {
                        return '[' + i + ']';
                    });
                }
            });
    }
};

$(document).ready(function () {

    $("#StyleID").on('change', function () {

        console.log('changed');
        var styleID = $(this).val();
        var purchaseOrderID = $('#PurchaseOrderID');

        $.get("/Store/Chalan/GetPurchaseOrderByStyle", { styleID: styleID },
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

    $(".btn-add").on('click', function () {
        var purchaseOrderID = $('#PurchaseOrderID');
        var poNo = $("#PurchaseOrderID option:selected").text();
        var tableVale = "";
        var checkPIExistsInTable = false;

        if (purchaseOrderID.val()) {
            $('#shipment-table tbody tr .purchaseOrderIDRow').each(function () {
                if ($(this).val() == purchaseOrderID.val()) {
                    checkPIExistsInTable = true;
                }
            });
            if (checkPIExistsInTable) {
                alertify.alert("This Purchase Order is already in the grid");
                return;
            }
            $.ajax({
                url: '/Store/Chalan/GetShipmentByPurchaseOrder',
                data: { purchaseOrderID: purchaseOrderID.val() },
                success: function (res) {
                    tableVale = "<tr><td>";
                    tableVale += "<input class='purchaseOrderIDRow' type='hidden' name='ShipmentList[0].PurchaseOrderID' value='" + purchaseOrderID.val() + "' />";
                    tableVale += "<input type='text' class='form-control' name='ShipmentList[0].PONo' value='" + poNo + "' disabled='' />";
                    tableVale += "</td>";
                    tableVale += "<td><input type='text' class='form-control' name='ShipmentList[0].OrderQuantity' value='" + res.OrderQuantity + "' disabled='' /></td>";
                    tableVale += "<td><input type='text' class='form-control' name='ShipmentList[0].ChalanQuantity'  required/></td>";
                    tableVale += "<td><input type='text' class='form-control' name='ShipmentList[0].CBM'  /></td>";
                    tableVale += "<td><input type='text' class='form-control' name='ShipmentList[0].CartoonQuantity'  /></td>";
                    tableVale += "<td><input type='text' class='form-control' name='ShipmentList[0].FactoryName' value='" + res.FactoryName + "' disabled='' /></td>";
                    tableVale += "<td><a href='javascript:void(0);' id='btnRemove' class=\"btn btn-danger\"><i class='fa fa-trash-o fa-2x'></i></a></td>";
                    tableVale += "</tr>";

                    $('#shipment-table tbody').append(tableVale);
                }
            }).promise().done(function () {
                formIndexing();
            });
        } else {
            alertify.alert("Please select Purchase Order");
        }
    });

    $('body').on('click', '#btnRemove',function () {        
        $(this).parent().parent().remove();
        formIndexing();
    });
})


