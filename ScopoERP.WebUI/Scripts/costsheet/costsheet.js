var __ = console.log;
var formIndexing = function () {
    for (var i = 0; i < $('.costsheet tbody tr').length; i++) {
        $('.costsheet tbody').children().eq(i).children().children()
        .each(function () {
            if (this.name) {
                this.name = this.name.replace(/\[(\d+)\]/, function (str, p1) {
                    return '[' + i + ']';
                });
            }
        });
    }
};
var calculatePrice = function ($this) {
    var findArea = $this.parent().siblings().andSelf();
    var consumption = parseFloat(findArea.find('.consumption').val());
    var conversion = parseFloat(findArea.find('.conversionQuantity').val());
    var wastage = parseFloat(findArea.find('.wastage').val());
    var unitPrice = parseFloat(findArea.find('.unitPrice').val());

    var actualConsumption = consumption / conversion;
    wastage = consumption * wastage / (100 * conversion);
    var totalRawMaterials = actualConsumption + wastage;

    findArea.find('.actualConsumption').val(actualConsumption.toFixed(10));
    findArea.find('.totalRawMaterials').val(totalRawMaterials.toFixed(10));
    findArea.find('.totalCost').val((totalRawMaterials * unitPrice).toFixed(4));
};
var claculateGrandTotalPrice = function () {
    var grandTotalCost = 0;
    $('.totalCost').each(function () {
        grandTotalCost += parseFloat($(this).val());
        $('#grandTotalCost').val(grandTotalCost);
        //__($('#grandTotalCost'));
        //__(grandTotalCost);
    });
};

$(document).ready(function () {
    var _StyleDropDown = $('#StyleDropDown');
    var _CostSheetNoDropDown = $('#CostSheetNoDropDown');
    var _CopyFromStyleDropDown = $('#CopyFromStyleDropDown');
    var _CopyFromCostSheetNoDropDown = $('#CopyFromCostSheetNoDropDown');

    _StyleDropDown.select2();
    _CostSheetNoDropDown.select2();
    _CopyFromStyleDropDown.select2();
    _CopyFromCostSheetNoDropDown.select2();

    _StyleDropDown.on('change', function () {
        toastr.info('Please Wait');
        ($(this).val() === "") ? function () {
            _CostSheetNoDropDown.empty();
            $('#costsheet-create-area').html('');
            toastr.clear();
        }() : $.get("/MaterialManagement/Costsheet/GetCosheetNoDropDown", { styleID: $(this).val() }, function (data) {
            _CostSheetNoDropDown.empty();
            _CostSheetNoDropDown.append($('<option/>', {
                value: "",
                text: ""
            }));
            $.each(data, function (index, itemData) {
                _CostSheetNoDropDown.append($('<option/>', {
                    value: itemData,
                    text: itemData
                }));
            });
        }).promise().done(function () {
            $.ajax({
                url: '/MaterialManagement/Costsheet/Create',
                type: 'GET',
                success: function (res) {
                    $('#costsheet-create-area').html(res);
                    toastr.clear();
                },
                error: function () {
                    toastr.clear();
                }
            }).promise().done(function () {
                $('.styleID').val(_StyleDropDown.val());
            });
        });
    });

    _CopyFromStyleDropDown.on('change', function () {
        if ($(this).val()) {
            $.get("/MaterialManagement/Costsheet/GetCosheetNoDropDown", { styleID: $(this).val() }, function (data) {
                _CopyFromCostSheetNoDropDown.empty();
                _CopyFromCostSheetNoDropDown.append($('<option/>', {
                    value: "",
                    text: ""
                }));
                $.each(data, function (index, itemData) {
                    _CopyFromCostSheetNoDropDown.append($('<option/>', {
                        value: itemData,
                        text: itemData
                    }));
                });
            });
        } else {
            _CopyFromCostSheetNoDropDown.empty();
        }

    });

    _CostSheetNoDropDown.on('change', function () {
        toastr.info('Please Wait');
        ($(this).val() === "") ? function () {
            $('#costsheet-create-area').html('');
            toastr.clear();
        }() : $.ajax({
            url: '/MaterialManagement/Costsheet/Create',
            type: 'GET',
            data: { costSheetNo: $(this).val() },
            success: function (res) {
                $('#costsheet-create-area').html(res);
                toastr.clear();
            },
            error: function () {
                toastr.clear();
            }
        }).promise().done(function () {
            claculateGrandTotalPrice();
        });
    });

    $('.itemCategoryID').live('change', function () {
        var _itemDropDown = $(this).parent().siblings().find('.itemID');
        //$(this).parent().siblings().find('select.itemID').select2();

        ($(this).val() === '') ? function () {
            _itemDropDown.empty();
        }() : $.get("/MaterialManagement/Costsheet/GetItemDropDown", { itemCategoryID: $(this).val() }, function (data) {
            _itemDropDown.empty();
            _itemDropDown.append($('<option/>', {
                value: "",
                text: ""
            }));
            $.each(data, function (index, itemData) {
                _itemDropDown.append($('<option/>', {
                    value: itemData.Value,
                    text: itemData.Text
                }));
            });
        });
    });

    $('.addAnotherItem').live('click', function () {
        $(this).parents('tr').after($(this).parents('tr').clone()).promise().done(function () {
            $(this).next('tr').find('input[type="text"], select').val('');
            $(this).next('tr').find('.costSheetID').val('-1');
            formIndexing();
        });
    });

    $('.removeItem').live('click', function () {
        $(this).parents('tr').remove();
        formIndexing();
    });

    $('#costsheetFormSubmit').live('click', function () {
        toastr.info('Please Wait');
        $.ajax({
            url: '/MaterialManagement/Costsheet/Create',
            type: 'POST',
            data: $('#costsheetForm').serializeArray(),
            success: function (res) {
                (res.errorMessage) ? function () {
                    toastr.clear();
                    toastr.error(res.errorMessage);
                }() : function () {
                    if ($('#CostSheetNoDropDown').val() !== res) {
                        $('<option value="' + res + '" selected>' + res + '</option>').appendTo($('#CostSheetNoDropDown')).promise().done(function () {
                            $('#CostSheetNoDropDown').trigger('change');
                        });
                    }
                    $('#CostSheetNoDropDown').trigger('change');
                    toastr.clear();
                    toastr.success("Cost Sheet is Saved Successfully");
                }();
            },
            error: function () {
                toastr.clear();
            }
        });
    });

    $('.consumption, .conversionQuantity, .wastage, .unitPrice').live({
        keyup: function () {
            calculatePrice($(this));
            claculateGrandTotalPrice();
        },
        blur: function () {
            calculatePrice($(this));
            claculateGrandTotalPrice();
        }
    });

    $('#btn-copy').on('click', function (e) {
        e.preventDefault();
        if (_StyleDropDown.val()) {
            if (_CopyFromCostSheetNoDropDown.val()) {
                toastr.info('Please Wait');
                $.ajax({
                    url: '/MaterialManagement/Costsheet/Copy',
                    type: 'POST',
                    data: {
                        'existingCostsheetNo': _CopyFromCostSheetNoDropDown.val(),
                        'toStyleID': _StyleDropDown.val()
                    },
                    success: function (res) {
                        (res === false) ? function () {
                            toastr.clear();
                            toastr.error("An unexpected error has been occured");
                        }() : function () {
                            if ($('#CostSheetNoDropDown').val() !== res) {
                                $('<option value="' + res + '" selected>' + res + '</option>').appendTo($('#CostSheetNoDropDown')).promise().done(function () {
                                    $('#CostSheetNoDropDown').trigger('change');
                                });
                            }
                            toastr.clear();
                            toastr.success("Cost Sheet is Copied Successfully");
                        }();
                    }
                });
            } else {
                toastr.error("Please select an existing costsheet");
            }
        } else {
            toastr.error("Please select a style where you want to copy the costsheet");
        }
    });

    $('#btnCostsheetExport').on('click', function (e) {
        e.preventDefault();
        if (_CostSheetNoDropDown.val()) {
            window.location.href = "/MaterialManagement/Costsheet/Export/?costsheetNo=" + _CostSheetNoDropDown.val();
        } else {
            toastr.error("Please Select a Costsheet");
        }
    });

    $('#btnCostsheetDelete').on('click', function (e) {
        e.preventDefault();
        if (_CostSheetNoDropDown.val()) {
            toastr.info('Please Wait');
            $.ajax({
                url: '/MaterialManagement/Costsheet/Delete',
                type: 'POST',
                data: { costsheetNo: _CostSheetNoDropDown.val() },
                success: function (res) {
                    (res === false) ? function () {
                        toastr.clear();
                        toastr.error("An unexpected error has been occured");
                    }() : function () {
                        _CostSheetNoDropDown.empty();
                        _StyleDropDown.trigger('change');
                        toastr.clear();
                        toastr.success("Cost Sheet is Copied Successfully");
                    }();
                }
            });
        } else {
            toastr.error("Please Select a Costsheet");
        }
    });
});