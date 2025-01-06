
$(document).ready(function () {
    loadData();
});

//Function load data   
function loadData() {
    $.ajax({
        url: "/Customer/List",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td hidden>' + item.CustomerID + '</td>';
                html += '<td>' + item.CustomerCode + '</td>';
                html += '<td>' + item.CustomerName + '</td>';
                html += '<td><a href="#" onclick="return getbyID(' + item.CustomerID + ')">Edit</a> | <a href="#" onclick="Delele(' + item.CustomerID + ')">Delete</a></td>';
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
    var cusObj = {
        CustomerID: $('#cusid').val(),
        CustomerCode: $('#cuscode').val(),
        CustomerName: $('#cusname').val()
    };
    $.ajax({
        url: "/Customer/Add",
        data: JSON.stringify(cusObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData();
            $('#cusModal').modal('hide');
            location.reload();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//Function for getting the data from customer ID 
function getbyID(cusID) {
    $.ajax({
        url: "/Customer/getbyID/" + cusID,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#cusid').val(result.CustomerID);
            $('#cuscode').val(result.CustomerCode);
            $('#cusname').val(result.CustomerName);

            $('#cusModal').modal('show');
            $('#btnUpdate').show();
            $('#btnAdd').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

//function for updating customers's record  
function Update() {
    var cusObj = {
        CustomerID: $('#cusid').val(),
        CustomerCode: $('#cuscode').val(),
        CustomerName: $('#cusname').val(),
    };
    $.ajax({
        url: "/Customer/Update",
        data: JSON.stringify(cusObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData();
            $('#cusModal').modal('hide');
            $('#cusid').val("");
            $('#cuscode').val("");
            $('#cusname').val("");
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//function for deleting customer's record  
function Delele(ID) {
    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        $.ajax({
            url: "/Customer/Delete/" + ID,
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
    $('#cusid').val("");
    $('#cuscode').val("");
    $('#cusname').val("");
}
