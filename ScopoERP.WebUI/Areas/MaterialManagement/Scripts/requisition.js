$(document).ready(function () {
    $('#RequisitionDate').on('change', function () {
        $.getJSON('', { jodID: $('#JobID').val(), supplierID: $(this).val() }, function (res) {

        })
    })
})