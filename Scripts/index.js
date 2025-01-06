
$(document).ready(function () {
    loadProductData();
    loadProductData2();
    loadCustomerData();
    loadOrder();
});

//Load product code to dropdown   
function loadProductData() {
    $.ajax({
        url: "/Home/ListProductData",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            for (var i = 0; i < result.length; i++)
            {
                var opt = new Option(result[i].ProductCode, result[i].ProductCode);
                $('#productcode').append(opt);
            }  
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}


//Load product code to dropdown 2   
function loadProductData2() {
    $.ajax({
        url: "/Home/ListProductData",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            for (var i = 0; i < result.length; i++) {
                var opt = new Option(result[i].ProductCode, result[i].ProductCode);
                $('#productcode2').append(opt);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//Load customer code to dropdown   
function loadCustomerData() {
    $.ajax({
        url: "/Home/ListCustomerData",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            for (var j = 0; j < result.length; j++) {
                var opt = new Option(result[j].CustomerCode, result[j].CustomerID);
                $('#customercode').append(opt);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//Get customer name by customer id
function getCusName()
{
    var selectedId = $('#customercode').val();
    $.ajax({
        url: "/Home/getCustomerName",
        data:{'CustomerID' : selectedId },
        type: "get",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#cusname').text(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//Get unit price by product code
function getUnitPrice() {
    var selectedId = $('#productcode2').val();
    $.ajax({
        url: "/Home/getUnitPrice",
        data: { 'ProductCode': selectedId },
        type: "get",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#unitprice2').val(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}


function getUnitPrice2() {
    var selectedId = $('#productcode').val();
    $.ajax({
        url: "/Home/getUnitPrice",
        data: { 'ProductCode': selectedId },
        type: "get",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#unitprice').val(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//Calculate total
function getTotal() {
    var qty = $('#proqty2').val();
    var uprice = $('#unitprice2').val();
    var tot = qty * uprice;
    $('#total2').val(tot);
    $('#subtot').val(tot);
    $('#nettot').val(tot);
}


function getTotal2() {
    var qty = $('#proqty').val();
    var uprice = $('#unitprice').val();
    var tot = qty * uprice;
    $('#total').val(tot);
}


//Add order 
function Add() {
    var TableData = new Array();

    $('#mytable tr').each(function (row, tr) {
        TableData[row] = {
            "ProductCode": $(tr).find('td:eq(1)').text(),
            "Qty": $(tr).find('td:eq(2)').text(),
            "ItemTotal": $(tr).find('td:eq(4)').text()
        }
    });

    var ordObj = {
        Data:TableData,
        NetTotal: $('#nettot').val(),
        SubTotal: $('#subtot').val(),
        DiscountPrice: $('#distot').val(),
        CustomerID: $('#customercode').val()
    };
    $.ajax({
        url: "/Home/AddOrder",
        data: JSON.stringify(ordObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//Discount function
function addDiscount()
{
    var dis = $('#distot').val();
    var subtot = $('#subtot').val();
    var tot = subtot - dis;
    $('#nettot').val(tot);
}

//Function for clearing the textboxes  
function clearTextBox() {
    $('#procode').val("");
    $('#proqty').val("");
}

//Load order data
function loadOrder() {
    $.ajax({
        url: "/Home/ListOrder",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.OrderNo + '</td>';
                html += '<td>' + item.ProductCode + '</td>';
                html += '<td>' + item.Qty + '</td>';
                html += '<td>' + item.UnitPrice + '</td>';
                html += '<td>' + item.ItemTotal + '</td>';
                html += '<td><a href="#" onclick="return getbyID(' + item.OrderNo + ')">Edit</a> | <a href="#" onclick="Delele(' + item.OrderNo + ')">Delete</a></td>';
                html += '</tr>';
            });
            $('.tbody').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//Function for getting the data from order ID 
function getbyID(ordID) {
    $.ajax({
        url: "/Home/getbyID/" + ordID,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#orderno').val(result.OrderNo);
            $('#productcode').val(result.ProductCode);
            $('#proqty').val(result.Qty);
            $('#unitprice').val(result.UnitPrice);

            $('#ordModal').modal('show');
            $('#btnUpdate').show();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

//function for updating order's  
function Update() {
    var ordObj = {
        OrderNo: $('#orderno').val(),
        ProductCode: $('#productcode').val(),
        Qty: $('#proqty').val(),
        ItemTotal: $('#total').val(),
        UnitPrice: $('#unitprice').val(),
    };
    $.ajax({
        url: "/Home/Update",
        data: JSON.stringify(ordObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadOrder();
            $('#ordModal').modal('hide');
            $('#productcode').val("");
            $('#proqty').val("");
            $('#total').val("");
            $('#unitprice').val("");
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//Function for deleting order's record  
function Delele(ID)
{
    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        $.ajax({
            url: "/Home/Delete/" + ID,
            type: "POST",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {
                loadOrder();
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
}

//Add Record to grid
function addorderrecord() {
    var productcode = $('#productcode2').val();
    var proqty = $('#proqty2').val();
    var unitprice = $('#unitprice2').val();
    var total = $('#total2').val();
    var html = '';
        html += '<tr>';
        html += '<td hidden>' + productcode + '</td>';
        html += '<td>' + productcode + '</td>';
        html += '<td>' + proqty + '</td>';
        html += '<td>' + unitprice + '</td>';
        html += '<td>' + total + '</td>';
        html += '<td><a href="#" onclick="DeleteRow();" id="del">Delete</a></td>';
        html += '</tr>';
        $('#order').append(html);

        var tot = proqty * unitprice;
        $('#total').val(tot);

        var table = document.getElementById("mytable"), subt = 0;
        for (var i = 1; i < table.rows.length; i++) {
            subt = subt + parseInt(table.rows[i].cells[4].innerHTML);
        }

        $('#subtot').val(subt);
        $('#nettot').val(subt);
        $('#productcode2').val("");
        $('#proqty2').val("");
        
}

function DeleteRow() {
    var tid = "";
    //$('#mytable tr').click(function (event) {
    //    tid = $(this).attr('id');
    //});
    $("#del").click(function () {
        tid = $(this).attr('id');
        alert(tid);
        $('#' + tid).remove();
    });
}
