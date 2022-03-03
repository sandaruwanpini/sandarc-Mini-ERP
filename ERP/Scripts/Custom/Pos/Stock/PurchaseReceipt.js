var dTablePurchaseReceipt = null;
var stockTypeDropdownValue = null;

$(document).ready(function () {
    Manager.GetPurchaseReceipt(0);
    Manager.loadSupplier();
    Manager.loadBranch();
    Manager.loadStockType();
    Manager.addNewDTableRow();

    $("#btnSave").prop("disabled", true);

    $("#Inv_OtherDiscount").keyup(function () {
        Manager.netAmtCalculation();
        $("#btnSave").prop("disabled", true);
    });

    $("#Inv_OtherCharges").keyup(function () {
        Manager.netAmtCalculation();
        $("#btnSave").prop("disabled", true);
    });

    $("#btnRefresh").click(function() {
        $("#btnSave").prop("disabled", false);
        Manager.netAmtCalculation();
    });

    $("#btnCancel").click(function () {
        $("#btnSave").prop("disabled", true);
        Manager.resetForm();

    });

    $("#btnSave").click(function() {
        Manager.savePurchaseReceipt();
    });
    $("#btnFind").click(function() {
        Manager.BranchTransfarStockReceive();
    });

    if ($("#CompanyInvNo").val() != "") {
        setTimeout(function() {
        Manager.BranchTransfarStockReceive();
        }, 500);
    }

});




$(document).on("click", ".dtBtnAddNewRow", function () {
    var inputs = $(this).parents('tr').find('.dtInputs');
    if ($(inputs[4]).val() != "" && $(inputs[0]).val() != "" && $(inputs[4]).val() != "0" && $(inputs[2]).val() != null && $(inputs[3]).val() != null && $(inputs[5]).val() != null) {
        Manager.addNewDTableRow();
        $item = $(this);
        var nextInput = $item.parents('tr').next().find('.dtInputs');
        nextInput[0].focus();
    } else {
        inputs[0].focus();
    }
});

$(document).on('click', '.dtBtnRemoveRow', function () {
    dTablePurchaseReceipt.row($(this).parents('tr')).remove().draw();
});


$(document).on('change', '.dtInputs', function (key) {
    var inputs = $(this).parents('tr').find('.dtInputs');
    Manager.calculateProductAmount(inputs);
});

$(document).on('keyup', '.dtInputs', function (key) {
    var inputs = $(this).parents('tr').find('.dtInputs');
    Manager.calculateProductAmount(inputs);
});

$(document).on('keydown', '.dtInputs', function (key) {
        var inputs = $(this).parents('tr').find('.dtInputs');
        var idx = inputs.index(this);
        var $item = $(this);
        Manager.dTableEventManager(key, inputs, idx, $item);
        Manager.calculateProductAmount(inputs);
});



$(document).on('click', '.dtInputs', function () {
    $(this).focus();
    $(this).select();
});




var Manager = {

    BranchTransfarStockReceive() {
        var jsonParam = { invoiceNo: $("#CompanyInvNo").val() };
        var serviceURL = "/PosStock/GetStockTransfarInvoiceItem/";
        JsManager.SendJson(serviceURL, jsonParam, onSuccess, onFailed);

        function onSuccess(jsonData) {
            if (jsonData.Status==1 && jsonData.Result.toString().length > 0) {
                dTablePurchaseReceipt.rows().remove();

                $.each(jsonData.Result.ProductList, function (i, v) {
                    if (i >= 0) {
                        Manager.addNewDTableRow();
                    }
                        
                    var inputs = $($("#dTablePurchaseReceipt").find('tr')[i+1]).find('.dtInputs');
                    Manager.loadProductBatch(v.Code, inputs);
                    Manager.loadUomMaster(v.Code, inputs);
                    $(inputs[0]).val(v.Code);
                    $(inputs[1]).val(v.Name);
                    $(inputs[2]).val(v.PosProductBatchId);
                    $(inputs[4]).val(v.Qty);
                    $(inputs[5]).html(stockTypeDropdownValue);
                    $(inputs[6]).val(((v.SchDiscount/(v.Qty * v.PurchaseRate)) * 100).toFixed(2));
                    $(inputs[8]).val(v.PurchaseRate);

                    Manager.calculateProductAmount(inputs);
                });
                var lessDis = jsonData.Result.Invoice.Discount.toFixed(2);
                $("#Inv_OtherDiscount").val(lessDis);
                $("#NetPayable").val(jsonData.Result.Invoice.ReceivedAmount);
                $("#InvDate").val(JsManager.DMYToMDY(jsonData.Result.Invoice.InvDate));
                Manager.netAmtCalculation();
                $('.dTableQty').prop('readonly', true);
                $('.dTableProductCode ').prop('readonly', true);
                

            } else if (jsonData.Status == "InvoiceReceived") {
                Message.Warning("The invoice already received.");
            } else if (jsonData.Status == "InvalidInvoice") {
                Message.Warning("Invoice not found!");
            }
        }

        function onFailed(error) {
                inputs[0].focus();
                inputs[0].select();
                $(inputs[0]).val('');
                //  window.alert(error.statusText);
            }
        
    },

    resetForm:function() {
        dTablePurchaseReceipt.clear().row.add({
            'ProductCode': '',
            'ProductName': '',
            'BatchName': '',
            'PosUomMasterId': '',
            'Qty': '',
            'PosStockTypeId': '',
            'DiscountPer': '0.00',
            'Discount': '0.00',
            'PurchaseRate': '0.00',
            'Amount': '0.00',
            'PurchaseTax': '0.00'
        }).draw();
        $("#CompanyInvNo").val('');
        $("#InvReferenceNo").val('');
        $("#NetPayable").val('');
        $("#Inv_TotalAmount").val(0);
        $("#Inv_OtherCharges").val(0);
        $("#Inv_NetValue").val(0);
        $("#Inv_LessDiscount").val(0);
        $("#Inv_OtherDiscount").val(0);
        $("#Inv_PurchaseTax").val(0);
    },
    savePurchaseReceipt: function () {
        var obj = [];
        obj.push({ 'Company Invoice No': $("#CompanyInvNo").val() });
        obj.push({ 'Invoice Date': $("#InvDate").val() });
        obj.push({ 'Inv. Receiv eDate': $("#InvReceiveDate").val() });
        obj.push({ 'Net Payable': $("#NetPayable").val() });
        if (parseFloat($("#Inv_TotalAmount").val()) < 1) {
            Message.Warning("Select minimum one product.");
            return;
        }
        if (JsManager.validate(obj)) {

            var posStockDetail = [];
            var objStock = new Object();
            objStock.InvReferenceNo = $("#InvReferenceNo").val();
            objStock.CompanyInvNo = $("#CompanyInvNo").val();
            objStock.PosSupplierId = $("#PosSupplierId").val();
            objStock.PosBranchId = $("#PosBranchId").val();
            objStock.InvDate = $("#InvDate").val();
            objStock.InvReceiveDate = $("#InvReceiveDate").val();
            objStock.NetPayable = $("#NetPayable").val();
            objStock.OtherDiscount = $("#Inv_OtherDiscount").val();
            objStock.OtherCharges = $("#Inv_OtherCharges").val();
            objStock.Remarks = $("#Inv_Remarks").val();

            $.each($('#dTablePurchaseReceipt tbody tr'), function (rowIdx, val) {
                var objStockDetails = new Object();
                var qty = parseFloat($(val).find(".dTableQty ").val()) * parseFloat($(val).find(".dTablePosUomMasterId").find(':selected').data('conversion'));
                if (!isNaN(qty) && qty > 0) {
                    objStockDetails.ProductCode = $(val).find(".dTableProductCode").val();
                    objStockDetails.PosProductBatchId = $(val).find(".dTableBatchName").val();
                    objStockDetails.Qty = qty;
                    objStockDetails.PosStockTypeId = $(val).find(".dTablePosStockTypeId").val();
                    objStockDetails.Discount = $(val).find(".dTableDiscount").val();
                    objStockDetails.PurchaseTax = $(val).find(".dTablePurchaseTax").val();
                    posStockDetail.push(objStockDetails);
                }
            });
            objStock.PosStockDetail = posStockDetail;
            if (Message.Prompt()) {
                var jsonParam = { vmStock: JSON.stringify(objStock) }
                var serviceURL = "/PosStock/InsertPurchaseReceipt/";
                JsManager.SendJson(serviceURL, jsonParam, onSuccess, onFailed);
            }


            function onSuccess(jsonData) {
                if (jsonData == "0") {
                    Message.Error("save");
                } else {
                    Message.Success("save");
                    Manager.resetForm();
                    $('.dTableQty').prop('readonly', true);
                    $('.dTableProductCode ').prop('readonly', true);
                }
            }

            function onFailed(xhr, status, err) {
                Message.Exception(xhr);
            }
        }
    },
    netAmtCalculation: function () {
        var productAmount = 0;
        var productDiscount = 0;
        var productPurchaseTax = 0;
        var productOtherDiscount = 0;
        var productOtherCharges = 0;
        $.each($('#dTablePurchaseReceipt tbody tr'), function (rowIdx, val) { 
            var tmpAmt = parseFloat($(val).find(".dTablePurchaseAmount").val());
            if (!isNaN(tmpAmt)) {
                productAmount += tmpAmt;
            } else {
                productAmount += 0;
            }
            var tmpPurc = parseFloat($(val).find(".dTablePurchaseTax").val());
            if (!isNaN(tmpPurc)) {
                productPurchaseTax += tmpPurc;
            } else {
                productPurchaseTax += 0;
            }
            var tmpDisc = parseFloat($(val).find(".dTableDiscount").val());
            if (!isNaN(tmpDisc)) {
                productDiscount += tmpDisc;
            } else {
                productDiscount += 0;
            }
        });
        $("#Inv_LessDiscount").val(productDiscount);
        $("#Inv_PurchaseTax").val(productPurchaseTax);
        $("#Inv_TotalAmount").val(productAmount);
        var othrDisc = parseFloat($("#Inv_OtherDiscount").val());
        if (!isNaN(othrDisc)) {
            productOtherDiscount += othrDisc;
        } else {
            productOtherDiscount += 0;
        }
        var otherCharge = parseFloat($("#Inv_OtherCharges").val());
        if (!isNaN(otherCharge)) {
            productOtherCharges += otherCharge;
        } else {
            productOtherCharges += 0;
        }
        $("#Inv_NetValue").val((productAmount + productPurchaseTax+productOtherCharges) - productOtherDiscount);

    },
    calculateProductAmount: function (inputs) {
        $("#btnSave").prop("disabled", true);
        $(inputs[8]).val($(inputs[2]).find(':selected').data('purchaserate'));
        var pRate = parseFloat($(inputs[8]).val());
        var pQty = parseFloat($(inputs[4]).val());
        var conversionFactor = parseFloat($(inputs[3]).find(':selected').data('conversion'));
        var amt = (conversionFactor * pQty) * pRate;

        var pDiscPer = parseFloat($(inputs[6]).val());
        var pDiscAmt = parseFloat($(inputs[7]).val());
        var discAmt = 0;
        var calByDisPer = 0;
        $(inputs[7]).val(discAmt);
        if (!isNaN(pDiscPer)) {
            discAmt = (amt * pDiscPer) / 100;
            $(inputs[7]).val(discAmt);
            calByDisPer = 1;
            $(inputs[9]).val(amt - discAmt);
        }
        
        $(inputs[9]).val(amt - discAmt);
        Manager.netAmtCalculation();
    },


    GetProductDetailsForPurchaseReceipt: function(pCode, inputs) {
        var jsonParam = { productCode: pCode };
        var serviceURL = "/PosStock/GetProductDetailsForPurchaseReceipt/";
        JsManager.SendJson(serviceURL, jsonParam, onSuccess, onFailed);

        function onSuccess(jsonData) {
            Manager.loadProductBatch(jsonData.Code, inputs);
            Manager.loadUomMaster(jsonData.Code, inputs);
            $(inputs[5]).html(stockTypeDropdownValue);
            $(inputs[0]).val(jsonData.Code);
            $(inputs[1]).val(jsonData.Name);
            $(inputs[2]).val(jsonData.PosProductBatchId);
            $(inputs[3]).val(jsonData.PosUomMasterId);
            $(inputs[8]).val(jsonData.PurchaseRate);
        }

        function onFailed(error) {
            inputs[0].focus();
            inputs[0].select();
            $(inputs[0]).val('');
        }
    },
    loadProductBatch: function(pCode, inputs) {
        var jsonParam = { productCode: pCode };
        var serviceURL = "/PosStock/GetBatchByProductCode/";
        JsManager.SendJsonAsyncON(serviceURL, jsonParam, onSuccess, onFailed);

        function onSuccess(jsonData) {
            var cbmOptions = "";
            $.each(jsonData, function() {
                cbmOptions += '<option data-purchaserate=' + this.PurchaseRate + ' value=\"' + this.Id + '\">' + this.Name + '</option>';
            });
            $(inputs[2]).html(cbmOptions);

        }

        function onFailed(error) {
            window.alert(error.statusText);
        }
    },
    loadUomMaster: function(pCode, inputs) {
        var jsonParam = { productCode: pCode };
        var serviceURL = "/PosDropDown/GetProductUom/";
        JsManager.SendJsonAsyncON(serviceURL, jsonParam, onSuccess, onFailed);

        function onSuccess(jsonData) {
            var cbmOptions = "";
            $.each(jsonData, function() {
                cbmOptions += '<option data-uomgroupid="' + this.PosUomGroupId + '" data-conversion="' + this.ConversionFactor + '" value="' + this.Id + '">' + this.Name + '</option>';
            });
        var isBasedUom = jsonData.filter(w => w.IsBaseUom == true)[0];
            $(inputs[3]).html(cbmOptions);
            $(inputs[3]).val(isBasedUom.Id);
        }

        function onFailed(error) {
            window.alert(error.statusText);
        }
    },
    loadStockType: function() {
        var jsonParam = '';
        var serviceURL = "/PosDropDown/GetStockType/";
        JsManager.SendJson(serviceURL, jsonParam, onSuccess, onFailed);

        function onSuccess(jsonData) {
            var cbmOptions = "";
            $.each(jsonData, function() {
                cbmOptions += '<option value=\"' + this.Id + '\">' + this.Name + '</option>';
            });
            $('.dTablePosStockTypeId').html(cbmOptions);
            stockTypeDropdownValue = cbmOptions;
        }

        function onFailed(error) {
            window.alert(error.statusText);
        }
    },
    loadBranch: function() {
        var jsonParam = '';
        var serviceURL = "/PosDropDown/GetBranch/";
        JsManager.SendJson(serviceURL, jsonParam, onSuccess, onFailed);

        function onSuccess(jsonData) {
            var objProgram = jsonData;
            JsManager.PopulateCombo('#PosBranchId', objProgram);
        }

        function onFailed(error) {
            window.alert(error.statusText);
        }
    },
   


    loadSupplier: function() {
        var jsonParam = '';
        var serviceURL = "/PosDropDown/GetSupplier/";
        JsManager.SendJson(serviceURL, jsonParam, onSuccess, onFailed);

        function onSuccess(jsonData) {
            var objProgram = jsonData;
            JsManager.PopulateCombo('#PosSupplierId', objProgram);
        }

        function onFailed(error) {
            window.alert(error.statusText);
        }
    },

    addNewDTableRow: function() {
        dTablePurchaseReceipt.row.add({
            'ProductCode': '',
            'ProductName': '',
            'BatchName': '',
            'PosUomMasterId': '',
            'Qty': '',
            'PosStockTypeId': '',
            'DiscountPer': '0.00',
            'Discount': '0.00',
            'PurchaseRate': '0.00',
            'Amount': '0.00',
            'PurchaseTax': '0.00'
        }).draw();
    },


    

    GetPurchaseReceipt: function(ref) {
        var jsonParam = { companyInvNo: 0 };
        var serviceURL = "/PosStock/GetPurchaseReceipt/";
        JsManager.SendJsonAsyncON(serviceURL, jsonParam, onSuccess, onFailed);

        function onSuccess(jsonData) {
            Manager.LoadDataTablePurchaseReceipt(jsonData, ref);
        }

        function onFailed(xhr, status, err) {
            Message.Exception(xhr);
        }
    },

    LoadDataTablePurchaseReceipt: function(userdata, isRef) {
        if (isRef == "0") {
            dTablePurchaseReceipt = $('#dTablePurchaseReceipt').DataTable({
                dom: 'B<"tableToolbar">rt',
                initComplete: function () {
                    dTableManager.Border("#dTablePurchaseReceipt", 300);
                },
                buttons: [
                ],
                scrollY: "300px",
                scrollX: true,
                scrollCollapse: true,
                lengthMenu: [[-1], ["All"]],
                columnDefs: [
                    { visible: false, targets: [] },
                ],
                columns: [
                    {
                        data: '',
                        name: 'SL',
                        orderable: false,
                        title: '',
                        width:10,
                        render: function(data, type, row, meta) {
                            return '<div class="dataTableSerialNumberDiv">' + (meta.row + 1) + '</div>';
                        }
                    },
                    {
                        data: 'ProductCode',
                        name: 'ProductCode',
                        orderable: false,
                        title: 'Code *....',
                        align: 'left',
                        width: 90,
                        render: function(data, type, row, meta) {
                            return "<input type='text' id='row-" + meta.row + "-ProductCode' name='row-" + meta.row + "-ProductCode'  value='" + data + "' class='form-control input-sm dTableProductCode dtInputs' placeholder='Product Code'/>";
                        }
                    },
                    {
                        data: 'ProductName',
                        name: 'ProductName',
                        title: 'Product Name',
                        align: 'left',
                        width: 250,
                        orderable: false,
                        render: function(data, type, row, meta) {
                            return "<input type='text' id='row-" + meta.row + "-ProductName' name='row-" + meta.row + "-ProductName' value='" + data + "' class='form-control dtInputBackground input-sm dTableProductName dtInputs' placeholder='Product Name' readonly='true'/>";
                        }
                    },
                    {
                        data: 'BatchName',
                        name: 'BatchName',
                        title: 'Batch/Size *',
                        orderable: false,
                        width: 90,
                        render: function(data, type, row, meta) {
                            return "<select id='row-" + meta.row + "-BatchName' name='row-" + meta.row + "-BatchName'  value='" + data + "' class='form-control input-sm dTableBatchName dtInputs'></select>";
                        }
                    },
                    {
                        data: 'PosUomMasterId',
                        name: 'PosUomMasterId',
                        title: 'Pack *',
                        width: 80,
                        orderable: false,
                        render: function(data, type, row, meta) {
                            return "<select id='row-" + meta.row + "-PosUomMasterId' name='row-" + meta.row + "-PosUomMasterId' value='" + data + "' class='form-control input-sm dTablePosUomMasterId dtInputs'></select>";
                        }
                    },
                    {
                        data: 'Qty',
                        name: 'Qty',
                        title: 'Received Qty *',
                        width: 100,
                        orderable: false,
                        render: function(data, type, row, meta) {
                            return "<input type='number' id='row-" + meta.row + "-Qty' name='row-" + meta.row + "-Qty' value='" + data + "' class='form-control input-sm dtInputTextAlign dTableQty dtInputs' placeholder='Qty' min='0'/>";
                        }
                    },
                    {
                        data: 'PosStockTypeId',
                        name: 'PosStockTypeId',
                        title: 'Stock Type *',
                        width: 80,
                        orderable: false,
                        render: function(data, type, row, meta) {
                            return "<select id='row-" + meta.row + "-PosStockTypeId' name='row-" + meta.row + "-PosStockTypeId' value='" + data + "' class='form-control input-sm dTablePosStockTypeId dtInputs'></select>";
                        }
                    },
                    {
                        data: 'DiscountPer',
                        name: 'DiscountPer',
                        title: 'Discount %',
                        width: 70,
                        orderable: false,
                        render: function(data, type, row, meta) {
                            return "<input type='number' id='row-" + meta.row + "-DiscountPer' name='row-" + meta.row + "-DiscountPer'  value='" + data + "' class='form-control input-sm dtInputTextAlign dTableDiscountPer dtInputs' placeholder='Discount %'  min='0'/>";
                        }
                    },
                    {
                        data: 'Discount',
                        name: 'Discount',
                        title: 'Discount Amt.',
                        width: 100,
                        orderable: false,
                        render: function(data, type, row, meta) {
                            return "<input type='number' id='row-" + meta.row + "-Discount' name='row-" + meta.row + "-Discount'  value='" + data + "' class='form-control input-sm dtInputTextAlign dTableDiscount dtInputs' placeholder='Discount Amount'  min='0'/>";
                        }
                    },
                    {
                        data: 'PurchaseRate',
                        name: 'PurchaseRate',
                        title: 'Rate',
                        width: 100,
                        orderable: false,
                        render: function(data, type, row, meta) {
                            return "<input type='number' id='row-" + meta.row + "-PurchaseRate' name='row-" + meta.row + "-PurchaseRate'  value='" + data + "' class='form-control input-sm dtInputTextAlign dtInputBackground dTablePurchaseRate dtInputs' placeholder='Rate' readonly='true'  min='0'/>";
                        }
                    },
                    {
                        data: '',
                        name: 'Amount',
                        title: 'Amount',
                        width: 100,
                        orderable: false,
                        render: function(data, type, row, meta) {
                            return "<input type='number' id='row-" + meta.row + "-Amount' name='row-" + meta.row + "-Amount'  value='" + data + "' class='form-control input-sm dtInputTextAlign dtInputBackground dTablePurchaseAmount dtInputs' placeholder='Amount' readonly='true' min='0'/>";
                        }
                    },
                    {
                        data: 'PurchaseTax',
                        name: 'PurchaseTax',
                        title: 'Purchase Tax',
                        width: 100,
                        orderable: false,
                        render: function(data, type, row, meta) {
                            return "<input type='number' id='row-" + meta.row + "-PurchaseTax' name='row-" + meta.row + "-PurchaseTax' value='" + data + "' class='form-control input-sm dtInputTextAlign dTablePurchaseTax dtInputs' placeholder='Purchase Tax'  min='0'/>";
                        }
                    },
                    {
                        name: 'Option',
                        title: 'Option',
                        width: 20,
                        orderable: false,
                        render: function(data, type, row) {
                            return "<div style='min-width:90px;'><input type='button' value='Ad.' class='dtBtnAddNewRow btn-blue' style='float:left;' /><input type='button' value='Rv.' class='dtBtnRemoveRow btn-danger' style='float:left;'/></div>";
                        }
                    }
                ],
                fixedColumns: true,
                data: userdata,
                rowsGroup: null
            });

        } else {
            dTablePurchaseReceipt.clear().rows.add(userdata).draw();

        }

    },
    dTableEventManager: function(key, inputs, idx, $item) {
        //for F4 product hot search
        if (key.keyCode == 115 && idx == 0) {
        } else if (key.keyCode == 13) {
            if (idx == 0) {
                if ($(inputs[0]).val() != "" && $(inputs[0]).val() != "0") {
                    Manager.GetProductDetailsForPurchaseReceipt($(inputs[0]).val(), inputs);
                    inputs[idx + 4].focus();
                    inputs[idx + 4].select();
                } else {
                    inputs[0].focus();
                    inputs[0].select();
                }
            } else if (idx == 6) {
                if ($(inputs[4]).val() != "" && $(inputs[0]).val() != "" && $(inputs[4]).val() != "0" && $(inputs[2]).val() != null && $(inputs[3]).val() != null && $(inputs[5]).val() != null) {
                    Manager.addNewDTableRow();
                    var nextInput = $item.parents('tr').next().find('.dtInputs');
                    nextInput[0].focus();
                } else {
                    inputs[0].focus();
                    inputs[0].select();
                }
            } else if (idx == 10) {
                if ($(inputs[4]).val() != "" && $(inputs[0]).val() != "" && $(inputs[4]).val() != "0" && $(inputs[2]).val() != null && $(inputs[3]).val() != null && $(inputs[5]).val() != null) {
                    Manager.addNewDTableRow();
                    var nextInput2 = $item.parents('tr').next().find('.dtInputs');
                    nextInput2[0].focus();
                } else {
                    inputs[0].focus();
                    inputs[0].select();
                }
            } else {
                inputs[idx + 1].focus();
                if (!$(inputs[idx + 1]).is('select')) {
                    inputs[idx + 1].select();
                }

            }
        } else if (key.keyCode == 37 && idx != 0) {
            inputs[idx - 1].focus();
        } else if (key.keyCode == 38) {
            key.preventDefault();
            var uPArrowInputs = $item.parents('tr').prev().find('.dtInputs');
            if (uPArrowInputs[idx] != undefined)
            uPArrowInputs[idx].focus();
        } else if (key.keyCode == 39) {
            inputs[idx + 1].focus();
        } else if (key.keyCode == 40) {
            key.preventDefault();
            var downArrowInputs = $item.parents('tr').next().find('.dtInputs');
            if (downArrowInputs[idx] != undefined)
            downArrowInputs[idx].focus();
        }
    }
}



var productInfos = null;
var prdFindBySystem = 0;
var isSearchOn = 0;
var productCodeId = "";

$(function () {
    fetch("/PosStock/GetProductInfoForPurchasReceipt").then(response => response.json().then(jsonData => {
        productInfos = jsonData;
    }));
});

$(document).on("keyup", ".dTableProductCode, #txtCodeOrBarcode", function (e) {
    var elInput = this;
    var elWrap = document.querySelector("#txtItemOrBarcodeAC");
    if (e.key == "F4") {
        if (isSearchOn == 0) {
            isSearchOn = 1;
            $("#chkSearchEnable").prop("checked", true);
        } else {
            isSearchOn = 0;
            $("#chkSearchEnable").prop("checked", false);
        }
        return;
    }
    if ($("#chkSearchEnable").prop("checked")) {isSearchOn = 1;} else { isSearchOn = 0; }

    var ignoreKey = [13, 37, 38, 39, 40];
    if (!ignoreKey.find(a=>a == e.keyCode)) {
        if (isSearchOn == 1) {
            $("#txtCodeOrBarcode").focus();
        }

        elWrap.innerHTML = '<table class="table table-sm table-bordered table-search" style="min-width:700px;background: #f7f7f7;margin-bottom:7px;">' +
                            '<thead class="theadPrdSrcTbl">' +
                                    '<tr style="margin:0;padding:0;">' +
                                        '<th>Product</th>' +
                                        '<th>Product Code</th>' +
                                        '<th>Product Bar Code</th>' +
                                        '<th>Batch/Size</th>' +
                                        '<th>Rate</th>' +
                                    '</tr>' +
                                '</thead>' +
                                '<tbody id="txtItemOrBarcodeACTR">' +
                                '</tbody>' +
                            '</table>';

        if (isSearchOn && elInput.value.length > 0) {
            var matchs = false;
            productInfos.forEach(function (pItem, ind) {
                var serXp = new RegExp('^' + elInput.value, "i");
                var serXI = new RegExp(elInput.value, "i");
                if (
                        pItem.Code.search(serXp) != -1 ||
                        pItem.BarCode.search(serXI) != -1 ||
                        pItem.Name.search(serXp) != -1 ||
                        pItem.BatchName.search(serXp) != -1
                ) {
                    matchs = true;
                    var elFound = '<tr><td class="txtItemOrBarcodeACITD"><button class="txtItemOrBarcodeACI" value="' + pItem.Code + '">' + pItem.Name + '</button></td>' +
                        '<td>' + pItem.Code + '</td>' +
                        '<td>' + pItem.BarCode + '</td>' +
                        '<td>' + pItem.BatchName + '</td>' +
                        '<td style="text-align:right;">' + pItem.PurchaseRate + '/-</td></tr>';
                    $(elWrap).find("#txtItemOrBarcodeACTR").append(elFound);

                }
            });

            if (matchs) {
                $(elWrap).css("display", "contents");
                $("#divSearchOn").show();
                $("#txtCodeOrBarcode").focus();
            }
        }
    }
});

$(document).on('keydown', "#divSearchOn", function (e) {
    if (e.target && (e.target.getAttribute("class") == 'form-control input-sm txtItemOrBarcodeACI' || e.target.getAttribute("class") == 'txtItemOrBarcodeACI active')) {
        e.stopPropagation();
        if (e.key == "ArrowUp") {
            if (e.target.parentNode.parentNode.previousSibling && e.target.parentNode.parentNode.previousSibling.querySelector("button")) {
                var preVN = e.target.parentNode.parentNode.previousSibling.querySelector("button");
                e.target.parentNode.parentNode.setAttribute("class", "");
                preVN.setAttribute("class", "txtItemOrBarcodeACI active");
                preVN.parentNode.parentNode.setAttribute("class", "txtItemOrBarcodeACTR active");
                preVN.focus();
                e.target.setAttribute("class", "txtItemOrBarcodeACI");
            }
            else {
                e.target.parentNode.parentNode.setAttribute("class", "");
                e.target.setAttribute("class", "txtItemOrBarcodeACI");
                $("#txtCodeOrBarcode").focus();
            }
        } else if (e.key == "ArrowDown") {
            if (e.target.parentNode.parentNode.nextSibling && e.target.parentNode.parentNode.nextElementSibling.querySelector("button")) {
                var nextVN = e.target.parentNode.parentNode.nextElementSibling.querySelector("button");

                var prvTr = nextVN.parentNode.parentNode.previousSibling;
                if (prvTr != null) {
                    prvTr.setAttribute("class", "txtItemOrBarcodeACI");
                    prvTr.querySelector('button').setAttribute("class", "txtItemOrBarcodeACI");
                }
                nextVN.setAttribute("class", "txtItemOrBarcodeACI active");
                nextVN.parentNode.parentNode.setAttribute("class", "txtItemOrBarcodeACTR active");
                nextVN.focus();
            }
        }
        else if (e.key == "Enter") {
            $("#txtCodeOrBarcode").val('');
            $("#txtItemOrBarcodeAC").html("");
            $("#divSearchOn").hide();
            $("#" + productCodeId).val(e.target.value);
            $("#" + productCodeId).focus();

        }
    }
    if (e.key == "Escape") {
        $("#txtCodeOrBarcode").val('');
        $("#txtCodeOrBarcode").focus();
        $("#txtItemOrBarcodeAC").html("");
        $("#divSearchOn").hide();
        isSearchOn = 0;
    }
    else if (e.key == "F2") {
        $("#txtCodeOrBarcode").val('');
        $("#txtCodeOrBarcode").focus();
    } else if (e.ctrlKey && e.shiftKey && e.keyCode == 'I'.charCodeAt(0)) {
        e.preventDefault();
        return false;
    } else if (e.ctrlKey && e.shiftKey && e.keyCode == 'C'.charCodeAt(0)) {
        e.preventDefault();
        return false;
    } else if (e.ctrlKey && e.shiftKey && e.keyCode == 'J'.charCodeAt(0)) {
        e.preventDefault();
        return false;
    } else if (e.ctrlKey && e.keyCode == 'U'.charCodeAt(0)) {
        e.preventDefault();
        return false;
    }

});

$(document).on('click', function (e) {
    if (e.target && (e.target.getAttribute("class") == 'txtItemOrBarcodeACI' || e.target.getAttribute("class") == 'txtItemOrBarcodeACI active')) {
      e.preventDefault();
        e.stopPropagation();
        $("#txtItemOrBarcodeAC").html("");
        $("#divSearchOn").hide();
        $("#" + productCodeId).val(e.target.value);
        $("#" + productCodeId).focus();
    }
});

$(document).on("focus", ".dTableProductCode", function () {
    productCodeId = this.id;
});

$(document).on("click", "#divCloseSearch", function () {

    $("#txtCodeOrBarcode").val('');
    $("#txtCodeOrBarcode").focus();
    $("#txtItemOrBarcodeAC").html("");
    $("#divSearchOn").hide();
});

