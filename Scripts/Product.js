
$(document).ready(function () {
    loadData();
});

//Function load data
function loadData() {
    $.ajax({
        url: "/Product/List",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td hidden>' + item.ProductID + '</td>';
                html += '<td>' + item.ProductCode + '</td>';
                html += '<td>' + item.ProductName + '</td>';
                html += '<td>' + item.Quantity + '</td>';
                html += '<td>' + item.UnitPrice + '</td>';
                html += '<td><a href="#" onclick="return getbyID(' + item.ProductID + ')">Edit</a> | <a href="#" onclick="Delele(' + item.ProductID + ')">Delete</a></td>';
                html += '</tr>';
            });
            $('.tbody').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//Function add data  
function Add() {
    var proObj = {
        ProductID: $('#proid').val(),
        ProductCode: $('#procode').val(),
        ProductName: $('#proname').val(),
        Quantity: $('#proqty').val(),
        UnitPrice: $('#prounitprice').val()
    };
    $.ajax({
        url: "/Product/Add",
        data: JSON.stringify(proObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData();
            $('#proModal').modal('hide');
            location.reload();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//Function for getting the data from product ID 
function getbyID(proID) {
    $.ajax({
        url: "/Product/getbyID/" + proID,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#proid').val(result.ProductID);
            $('#procode').val(result.ProductCode);
            $('#proname').val(result.ProductName);
            $('#proqty').val(result.Quantity);
            $('#prounitprice').val(result.UnitPrice);

            $('#proModal').modal('show');
            $('#btnUpdate').show();
            $('#btnAdd').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

//function for updating products's record  
function Update() {
    var proObj = {
        ProductID: $('#proid').val(),
        ProductCode: $('#procode').val(),
        ProductName: $('#proname').val(),
        Quantity: $('#proqty').val(),
        UnitPrice: $('#prounitprice').val()
    };
    $.ajax({
        url: "/Product/Update",
        data: JSON.stringify(proObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData();
            $('#proModal').modal('hide');
            $('#proid').val("");
            $('#procode').val("");
            $('#proname').val("");
            $('#proqty').val("");
            $('#prounitprice').val("");
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//function for deleting product's record  
function Delele(ID) {
    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        $.ajax({
            url: "/Product/Delete/" + ID,
            type: "POST",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {
                loadData();
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
}

//Function for clearing the textboxes  
function clearTextBox() {
    $('#proid').val("");
    $('#procode').val("");
    $('#proname').val("");
    $('#proqty').val("");
    $('#prounitprice').val("");
}
