function BillPrintOfStockTransfer(jsonData) {

    var productInfoRow = "";
    var slCount = 1;
    $.each(jsonData.ProductList, function(ind, val) {
        productInfoRow += '<tr>' +
            '<td>' + slCount + '</td>' +
            '<td>' + val.Name + '</td>' +
            '<td class="tr">' + val.UnitPrice + '</td>' +
            '<td class="tr">' + val.Qty.toFixed(3) + '</td>' +
            '<td class="tr">' + (val.UnitPrice * val.Qty).toFixed(2) + '</td>' +
            '<td class="tr">' + (val.SchDiscount).toFixed(2) + '</td>' +
            '<td class="tr">' + ((val.UnitPrice * val.Qty) - val.SchDiscount).toFixed(2) + '</td>' +
            '</tr>';
        slCount++;
    });
    var invoiceNo = jsonData.Invoice.InvDate.replace(new RegExp("-", 'gi'), '') + '-' + jsonData.Invoice.InvoiceNumber;
    var printWindow = window.open('', '', 'width=900,height=600');
    printWindow.document.write(
        '<!DOCTYPE html>' +
        '<html>' +
        '<head>' +
        '<title>Bill Ptint Invoice-' + invoiceNo + '</title>' +
        '</head>' +
        '<body style="width:100%;">' +
        '<style type="text/css">' +
        '*{' +
        'margin: 0;' +
        'font-family: helvetica;' +
        '		}' +
        '.fL{' +
        '			font-size: 20px;' +
        '		}' +
        '.fM{' +
        '			font-size: 16px;' +
        '		}' +
        '.fs{' +
        '			font-size: 13px;' +
        '		}' +
        '.flL{' +
        'float:left;' +
        '		}' +
        '.flR{' +
        'float:right;' +
        '		}' +
        '.tl{' +
        '			text-align: left;' +
        '		}' +
        '.tc{' +
        '			text-align: center;' +
        '		}' +
        '.tr{' +
        '			text-align: right;' +
        '		}' +
        '.fwb{' +
        'font-weight: bold;' +
        '		}' +
        '.mt5{' +
        'margin-top: 5px;' +
        '		}' +
        '.mtb10{' +
        'margin: 10px 0;' +
        '		}' +
        '.w50{' +
        'width: 50%;' +
        '		}' +
        '.w100{' +
        'width: 100%;' +
        '		}' +
        '#wrap{' +
        'width:793px;' +
        'line-height: 1;' +
        'padding: 20px 10px;' +
        'margin: 0 auto' +
        '		}' +
        '.bdrB{' +
        'border-top: 0;' +
        'border-left: 0;' +
        'border-right: 0;' +
        'border-bottom: 2px solid #000;' +
        'border-style: dashed;' +
        '		}' +
        '</style>' +
        '<div id="wrap">' +
        '<div class="flL w100 tc fL fwb">' + jsonData.Company + '</div>' +
        '<div class="flL w100 tc fM fwb mt5">' + jsonData.Branch + '</div>' +
        '<div class="flL w100 fs tc mt5">' + jsonData.BranchAddress + '</div>' +
        '<div class="flL w100 fs tc mt5"><b>Vat Reg No:</b> ' + jsonData.VatRegNo + ' <b>&nbsp &nbsp Phone:</b> ' + jsonData.Phone + '</div>' +
        '<div class="flL w100 bdrB mtb10"></div>' +
        '<div class="flL w100 fs "><b>Branch/Office:</b> ' + jsonData.Invoice.CustomerName + ' </div>' +
        '<div class="flL w100 "></div>' +
        '<div class="flL fs mt5"><b>Invoice No:</b> ' + invoiceNo + ' </div><div class="flR fs mt5"><b>Date:</b> ' + jsonData.Invoice.InvDate + ' </div>' +
        '<div class="flL w100 "></div>' +
        '<div class="flL fs mt5"><b>User:</b> ' + jsonData.User + ' </div><div class="flR fs mt5"><b>Terminal No:</b> ' + jsonData.TerminalNo + ' </div>' +
        '<div class="flL w100 bdrB mtb10"></div>' +
        '<table class="w100 fs">' +
        '<thead>' +
        '<tr>' +
        '					<th class="tl">SL</th>' +
        '<th class="tl">Items Description</th>' +
        '<th class="tr">MRP</th>' +
        '<th class="tr">Qty</th>' +
        '<th class="tr">Total SR</th>' +
        '<th class="tr">Discount SR</th>' +
        '<th class="tr">NetPay SR</th>' +
        '</tr>' +
        '</thead>' +
        '<tbody>' +
        '<tr>' +
        '					<td colspan="7">' +
        '<div class="flL w100 bdrB mtb10"></div>' +
        '</td>' +
        '</tr>' +
        productInfoRow +
        '<tr>' +
        '					<td colspan="7">' +
        '<div class="flL w100 bdrB mtb10"></div>' +
        '</td>' +
        '</tr>' +
        '<tr>' +
        '<td colspan="5" class="fwb tr">' +
        'Sub Total SR :' +
        '</td>' +
        '<td colspan="2" class="fwb tr">' +
        jsonData.Invoice.MrpTotal +
        '</td>' +
        '</tr>' +
        '<tr>' +

        '<td colspan="5" class="fwb tr">' +
        'SD SR :' +
        '</td>' +
        '<td colspan="2" class="fwb tr">' +
        jsonData.Invoice.SdTotal +
        '</td>' +
        '</tr>' +
        '<tr>' +

        '<td colspan="5" class="fwb tr">' +
        '( + ) Vat SR :' +
        '</td>' +
        '<td colspan="2" class="fwb tr">' +
        jsonData.Invoice.VatTotal +
        '</td>' +
        '</tr>' +
        '<tr>' +

        '<td colspan="5" class="fwb tr">' +
        '( - ) Discount SR :' +
        '</td>' +
        '<td colspan="2" class="fwb tr">' +
        (jsonData.Invoice.Discount- jsonData.Invoice.PerProductWiseDiscount).toFixed(2) +
        '</td>' +
        '</tr>' +
        '<tr>' +
        '<td colspan="5" class="fwb tr">' +
        '						' +
        '</td>' +
        '<td colspan="2" class="fwb tr">' +
        '<div class="flL w100 bdrB"></div>' +
        '</td>' +
        '</tr>' +
        '<tr>' +
        '<td colspan="5" class="fwb tr">' +
        'Total Amount SR :' +
        '</td>' +
        '<td colspan="2" class="fwb tr">' +
        jsonData.Invoice.TotalAmt +
        '</td>' +
        '</tr>' +
        '<tr>' +
        '<td colspan="5" class="fwb tr">' +
        '( +/- ) Rounding SR : ' +
        '</td>' +
        '<td colspan="2" class="fwb tr">' +
        jsonData.RoundOffvalue +
        '</td>' +
        '</tr>' +
        '<tr>' +
        '<td colspan="5" class="fwb tr">' +
        'Net Payable SR :' +
        '</td>' +
        '<td colspan="2" class="fwb tr">' +
        jsonData.Invoice.PaidableAmount +
        '</td>' +
        '</tr>' +
        '<tr>' +
        '<td colspan="5" class="fwb tr">'
        + ((jsonData.PaidBy != "") ? jsonData.PaidBy : "Paid Amount") + ' :' +
        '</td>' +
        '<td colspan="2" class="fwb tr">' +
        jsonData.Invoice.ReceivedAmount +
        '</td>' +
        '</tr>' +
        '<tr>' +
        '<td colspan="5" class="fwb tr">' +
        'Change Amount SR :' +
        '</td>' +
        '<td colspan="2" class="fwb tr">' +
        jsonData.ChangeAmt +
        '</td>' +
        '</tr>' +
        '<tr>' +
        '<td colspan="5" class="fwb tr">' +
        'Due Amount SR :' +
        '</td>' +
        '<td colspan="2" class="fwb tr">' +
        jsonData.Invoice.DueAmount +
        '</td>' +
        '</tr>				' +
        '</tbody>' +
        '</table>' +
        '<div class="flL w100 bdrB mtb10"></div>' +
        '<div class="flL w100 tc fs">' + jsonData.FooterText +
        '</div>' +
        '<div class="flL w100 tc fs fwb mt5">Printed Date & Time: ' + dateFormat(new Date(), "dd-mm-yyyy HH:MM:ss") + '</div>' +
        '<div class="flL w100 tc fs fwb mtb10"> ' + jsonData.PoweredBy + '</div>' +
        '</div>' +
        '</body>' +
        '</html>'
    );

    printWindow.document.close();
    printWindow.focus();
    printWindow.print();
    printWindow.close();

}


function BillPrintByA4Page(jsonData) {

    var productInfoRow = "";
    var slCount = 1;
    $.each(jsonData.ProductList, function (ind, val) {
        productInfoRow += '<tr>' +
            '<td>' + slCount + '</td>' +
            '<td>' + val.Name + '</td>' +
            '<td class="tr">' + val.UnitPrice + '</td>' +
            '<td class="tr">' + val.Qty.toFixed(3) + '</td>' +
            '<td class="tr">' + (val.UnitPrice * val.Qty).toFixed(2) + '</td>' +
            '<td class="tr">' + (val.SchDiscount).toFixed(2) + '</td>' +
            '<td class="tr">' + ((val.UnitPrice * val.Qty) - val.SchDiscount).toFixed(2) + '</td>' +
            '</tr>';
        slCount++;
    });
    var invoiceNo = jsonData.Invoice.InvDate.replace(new RegExp("-", 'gi'), '') + '-' + jsonData.Invoice.InvoiceNumber;
    var printWindow = window.open('', '', 'width=900,height=600');
    printWindow.document.write(
        '<!DOCTYPE html>' +
        '<html>' +
        '<head>' +
        '<title>Bill Ptint Invoice-' + invoiceNo + '</title>' +
        '</head>' +
        '<body style="width:100%;">' +
        '<style type="text/css">' +
        '*{' +
        'margin: 0;' +
        'font-family: helvetica;' +
        '		}' +
        '.fL{' +
        '			font-size: 20px;' +
        '		}' +
        '.fM{' +
        '			font-size: 16px;' +
        '		}' +
        '.fs{' +
        '			font-size: 13px;' +
        '		}' +
        '.flL{' +
        'float:left;' +
        '		}' +
        '.flR{' +
        'float:right;' +
        '		}' +
        '.tl{' +
        '			text-align: left;' +
        '		}' +
        '.tc{' +
        '			text-align: center;' +
        '		}' +
        '.tr{' +
        '			text-align: right;' +
        '		}' +
        '.fwb{' +
        'font-weight: bold;' +
        '		}' +
        '.mt5{' +
        'margin-top: 5px;' +
        '		}' +
        '.mtb10{' +
        'margin: 10px 0;' +
        '		}' +
        '.w50{' +
        'width: 50%;' +
        '		}' +
        '.w100{' +
        'width: 100%;' +
        '		}' +
        '#wrap{' +
        'width:793px;' +
        'line-height: 1;' +
        'padding: 20px 10px;' +
        'margin: 0 auto' +
        '		}' +
        '.bdrB{' +
        'border-top: 0;' +
        'border-left: 0;' +
        'border-right: 0;' +
        'border-bottom: 2px solid #000;' +
        'border-style: dashed;' +
        '		}' +
        '</style>' +
        '<div id="wrap">' +
        '<div class="flL w100 tc fL fwb">' + jsonData.Company + '</div>' +
        '<div class="flL w100 tc fM fwb mt5">' + jsonData.Branch + '</div>' +
        '<div class="flL w100 fs tc mt5">' + jsonData.BranchAddress + '</div>' +
        '<div class="flL w100 fs tc mt5"><b>Vat Reg No:</b> ' + jsonData.VatRegNo + ' <b>&nbsp &nbsp Phone:</b> ' + jsonData.Phone + '</div>' +
        '<div class="flL w100 bdrB mtb10"></div>' +
        '<div class="flL w100 fs "><b>Customer:</b> ' + jsonData.Invoice.CustomerName + ' </div>' +
        '<div class="flL w100 "></div>' +
        '<div class="flL fs mt5"><b>Invoice No:</b> ' + invoiceNo + ' </div><div class="flR fs mt5"><b>Date:</b> ' + jsonData.Invoice.InvDate + ' </div>' +
        '<div class="flL w100 "></div>' +
        '<div class="flL fs mt5"><b>User:</b> ' + jsonData.User + ' </div><div class="flR fs mt5"><b>Terminal No:</b> ' + jsonData.TerminalNo + ' </div>' +
        '<div class="flL w100 bdrB mtb10"></div>' +
        '<table class="w100 fs">' +
        '<thead>' +
        '<tr>' +
        '					<th class="tl">SL</th>' +
        '<th class="tl">Items Description</th>' +
        '<th class="tr">MRP</th>' +
        '<th class="tr">Qty</th>' +
        '<th class="tr">Total SR</th>' +
        '<th class="tr">Discount SR</th>' +
        '<th class="tr">NetPay SR</th>' +
        '</tr>' +
        '</thead>' +
        '<tbody>' +
        '<tr>' +
        '					<td colspan="7">' +
        '<div class="flL w100 bdrB mtb10"></div>' +
        '</td>' +
        '</tr>' +
        productInfoRow +
        '<tr>' +
        '					<td colspan="7">' +
        '<div class="flL w100 bdrB mtb10"></div>' +
        '</td>' +
        '</tr>' +
        '<tr>' +
        '<td colspan="5" class="fwb tr">' +
        'Sub Total SR :' +
        '</td>' +
        '<td colspan="2" class="fwb tr">' +
        jsonData.Invoice.MrpTotal +
        '</td>' +
        '</tr>' +
        '<tr>' +

         '<td colspan="5" class="fwb tr">' +
        'SD SR :' +
        '</td>' +
        '<td colspan="2" class="fwb tr">' +
        jsonData.Invoice.SdTotal +
        '</td>' +
        '</tr>' +
        '<tr>' +

        '<td colspan="5" class="fwb tr">' +
        '( + ) Vat SR :' +
        '</td>' +
        '<td colspan="2" class="fwb tr">' +
        jsonData.Invoice.VatTotal +
        '</td>' +
        '</tr>' +
        '<tr>' +
        '<td colspan="5" class="fwb tr">' +
        '( - ) Discount SR :' +
        '</td>' +
        '<td colspan="2" class="fwb tr">' +
        (jsonData.Invoice.Discount - jsonData.Invoice.PerProductWiseDiscount).toFixed(2) +
        '</td>' +
        '</tr>' +
        '<tr>' +
        '<td colspan="5" class="fwb tr">' +
        '						' +
        '</td>' +
        '<td colspan="2" class="fwb tr">' +
        '<div class="flL w100 bdrB"></div>' +
        '</td>' +
        '</tr>' +
        '<tr>' +
        '<td colspan="5" class="fwb tr">' +
        'Total Amount SR :' +
        '</td>' +
        '<td colspan="2" class="fwb tr">' +
        jsonData.Invoice.TotalAmt +
        '</td>' +
        '</tr>' +
        '<tr>' +
        '<td colspan="5" class="fwb tr">' +
        '( +/- ) Rounding SR : ' +
        '</td>' +
        '<td colspan="2" class="fwb tr">' +
        jsonData.RoundOffvalue +
        '</td>' +
        '</tr>' +
        '<tr>' +
        '<td colspan="5" class="fwb tr">' +
        'Net Payable SR :' +
        '</td>' +
        '<td colspan="2" class="fwb tr">' +
        jsonData.Invoice.PaidableAmount +
        '</td>' +
        '</tr>' +
        '<tr>' +
        '<td colspan="5" class="fwb tr">'
        + ((jsonData.PaidBy != "") ? jsonData.PaidBy : "Paid Amount") + ' :' +
        '</td>' +
        '<td colspan="2" class="fwb tr">' +
        jsonData.Invoice.ReceivedAmount +
        '</td>' +
        '</tr>' +
        '<tr>' +
        '<td colspan="5" class="fwb tr">' +
        'Change Amount SR :' +
        '</td>' +
        '<td colspan="2" class="fwb tr">' +
        jsonData.ChangeAmt +
        '</td>' +
        '</tr>' +
        '<tr>' +
        '<td colspan="5" class="fwb tr">' +
        'Due Amount SR :' +
        '</td>' +
        '<td colspan="2" class="fwb tr">' +
        jsonData.Invoice.DueAmount +
        '</td>' +
        '</tr>				' +
        '</tbody>' +
        '</table>' +
        '<div class="flL w100 bdrB mtb10"></div>' +
        '<div class="flL w100 tc fs">' + jsonData.FooterText +
        '</div>' +
        '<div class="flL w100 tc fs fwb mt5">Printed Date & Time: ' + dateFormat(new Date(), "dd-mm-yyyy HH:MM:ss") + '</div>' +
        '<div class="flL w100 tc fs fwb mtb10"> ' + jsonData.PoweredBy + '</div>' +
        '</div>' +
        '</body>' +
        '</html>'
    );

    printWindow.document.close();
    printWindow.focus();
    printWindow.print();
    printWindow.close();

}


function PosBillPrintByPosPrinter(jsonData) {
    var productInfoRow = "";
    var slCount = 1;
    $.each(jsonData.ProductList, function(ind, val) {
        productInfoRow += '<tr>' +
            '<td>' + slCount + '</td>' +
            '<td>' + val.Name + '</td>' +
            '<td class="tr">' + val.UnitPrice + '</td>' +
            '<td class="tr">' + val.Qty.toFixed(3) + '</td>' +
            '<td class="tr">' + (val.UnitPrice * val.Qty).toFixed(2) + '</td>' +
            '</tr>';
        slCount++;
    });
    var invoiceNo = jsonData.Invoice.InvDate.replace(new RegExp("-", 'gi'), '') + '-' + jsonData.Invoice.InvoiceNumber;
    var printWindow = window.open('', '', 'width=900,height=600');
    printWindow.document.write(
        '<!DOCTYPE html>' +
        '<html>' +
        '<head>' +
        '<title>Bill Ptint Invoice-' + invoiceNo + '</title>' +
        '</head>' +
        '<body style="width:100%;">' +
        '<style type="text/css">' +
        '*{' +
        'margin: 0;' +
        'font-family: helvetica;' +
        '		}' +
        '.fL{' +
        '			font-size: 16px;' +
        '		}' +
        '.fM{' +
        '			font-size: 12px;' +
        '		}' +
        '.fs{' +
        '			font-size: 10px;' +
        '		}' +
        '.flL{' +
        'float:left;' +
        '		}' +
        '.flR{' +
        'float:right;' +
        '		}' +
        '.tl{' +
        '			text-align: left;' +
        '		}' +
        '.tc{' +
        '			text-align: center;' +
        '		}' +
        '.tr{' +
        '			text-align: right;' +
        '		}' +
        '.fwb{' +
        'font-weight: bold;' +
        '		}' +
        '.mt5{' +
        'margin-top: 5px;' +
        '		}' +
        '.mtb10{' +
        'margin: 10px 0;' +
        '		}' +
        '.w50{' +
        'width: 50%;' +
        '		}' +
        '.w100{' +
        'width: 100%;' +
        '		}' +
        '#wrap{' +
        'width:285px;' +
        'line-height: 1;' +
        'padding: 20px 10px;' +
        'margin: 0 auto' +
        '		}' +
        '.bdrB{' +
        'border-top: 0;' +
        'border-left: 0;' +
        'border-right: 0;' +
        'border-bottom: 2px solid #000;' +
        'border-style: dashed;' +
        '		}' +
        '</style>' +
        '<div id="wrap">' +
        '<div class="flL w100 tc fL fwb">' + jsonData.Company + '</div>' +
        '<div class="flL w100 tc fM fwb mt5">' + jsonData.Branch + '</div>' +
        '<div class="flL w100 fs tc mt5">' + jsonData.BranchAddress + '</div>' +
        '<div class="flL w50 fs mt5"><b>Vat Reg No:</b> ' + jsonData.VatRegNo + ' </div><div class="flL w50 fs mt5"> <b>Phone:</b> ' + jsonData.Phone + '</div>' +
        '<div class="flL w100 bdrB mtb10"></div>' +
        '<div class="flL w100 fs "><b>Customer:</b> ' + jsonData.Invoice.CustomerName + ' </div>' +
        '<div class="flL w100 "></div>' +
        '<div class="flL fs mt5"><b>Invoice No:</b> ' + invoiceNo + ' </div><div class="flR fs mt5"><b>Date:</b> ' + jsonData.Invoice.InvDate + ' </div>' +
        '<div class="flL w100 "></div>' +
        '<div class="flL fs mt5"><b>User:</b> ' + jsonData.User + ' </div><div class="flR fs mt5"><b>Terminal No:</b> ' + jsonData.TerminalNo + ' </div>' +
        '<div class="flL w100 bdrB mtb10"></div>' +
        '<table class="w100 fs">' +
        '<thead>' +
        '<tr>' +
        '					<th class="tl">SL</th>' +
        '<th class="tl">Items Description</th>' +
        '<th class="tr">MRP</th>' +
        '<th class="tr">Qty</th>' +
        '<th class="tr">Total SR</th>					' +
        '</tr>' +
        '</thead>' +
        '<tbody>' +
        '<tr>' +
        '					<td colspan="5">' +
        '<div class="flL w100 bdrB mtb10"></div>' +
        '</td>' +
        '</tr>' +
        productInfoRow +
        '<tr>' +
        '					<td colspan="5">' +
        '<div class="flL w100 bdrB mtb10"></div>' +
        '</td>' +
        '</tr>' +
        '<tr>' +
        '					<td colspan="3" class="fwb tr">' +
        'Sub Total SR :' +
        '</td>' +
        '<td colspan="2" class="fwb tr">' +
        jsonData.Invoice.MrpTotal +
        '</td>' +
        '</tr>' +
        '<tr>' +

        '<td colspan="3" class="fwb tr">' +
        'SD SR :' +
        '</td>' +
        '<td colspan="2" class="fwb tr">' +
        jsonData.Invoice.SdTotal +
        '</td>' +
        '</tr>' +
        '<tr>' +
        
        '<td colspan="3" class="fwb tr">' +
        '( + ) Vat SR :' +
        '</td>' +
        '<td colspan="2" class="fwb tr">' +
        jsonData.Invoice.VatTotal +
        '</td>' +
        '</tr>' +
        '<tr>' +

        '<td colspan="3" class="fwb tr">' +
        '( - ) Discount SR :' +
        '</td>' +
        '<td colspan="2" class="fwb tr">' +
        jsonData.Invoice.Discount +
        '</td>' +
        '</tr>' +
        '<tr>' +
        '					<td colspan="3" class="fwb tr">' +
        '						' +
        '</td>' +
        '<td colspan="2" class="fwb tr">' +
        '<div class="flL w100 bdrB"></div>' +
        '</td>' +
        '</tr>' +
        '<tr>' +
        '					<td colspan="3" class="fwb tr">' +
        'Total Amount SR :' +
        '</td>' +
        '<td colspan="2" class="fwb tr">' +
        jsonData.Invoice.TotalAmt +
        '</td>' +
        '</tr>' +
        '<tr>' +
        '					<td colspan="3" class="fwb tr">' +
        '( +/- ) Rounding SR : ' +
        '</td>' +
        '<td colspan="2" class="fwb tr">' +
        jsonData.RoundOffvalue +
        '</td>' +
        '</tr>' +
        '<tr>' +
        '					<td colspan="3" class="fwb tr">' +
        'Net Payable SR :' +
        '</td>' +
        '<td colspan="2" class="fwb tr">' +
        jsonData.Invoice.PaidableAmount +
        '</td>' +
        '</tr>' +
        '<tr>' +
        '					<td colspan="3" class="fwb tr">'
        + ((jsonData.PaidBy != "") ? jsonData.PaidBy : "Paid Amount") + ' :' +
        '</td>' +
        '<td colspan="2" class="fwb tr">' +
        jsonData.Invoice.ReceivedAmount +
        '</td>' +
        '</tr>' +
        '<tr>' +
        '					<td colspan="3" class="fwb tr">' +
        'Change Amount SR :' +
        '</td>' +
        '<td colspan="2" class="fwb tr">' +
        jsonData.ChangeAmt +
        '</td>' +
        '</tr>' +
        '<tr>' +
        '					<td colspan="3" class="fwb tr">' +
        'Due Amount SR :' +
        '</td>' +
        '<td colspan="2" class="fwb tr">' +
        jsonData.Invoice.DueAmount +
        '</td>' +
        '</tr>				' +
        '</tbody>' +
        '</table>' +
        '<div class="flL w100 bdrB mtb10"></div>' +
        '<div class="flL w100 tc fs">' + jsonData.FooterText +
        '</div>' +
        '<div class="flL w100 tc fs fwb mt5">Printed Date & Time: ' + dateFormat(new Date(), "dd-mm-yyyy HH:MM:ss") + '</div>' +
        '<div class="flL w100 tc fs fwb mtb10"> ' + jsonData.PoweredBy + '</div>' +
        '</div>' +
        '</body>' +
        '</html>'
    );

    printWindow.document.close();
    printWindow.focus();
    printWindow.print();
    printWindow.close();
}