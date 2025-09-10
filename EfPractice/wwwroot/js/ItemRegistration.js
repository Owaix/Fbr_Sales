


var editingRow = null;

function loadTableData() {
    $.ajax({
        url: 'GetItemRegistration',
        type: 'GET',
        success: function (data) {
            console.log(data);

            var tableBody = $("#myTable tbody");
            tableBody.empty(); // Clear existing rows
            data.forEach(function (item) {

                console.log("Add item");
                console.log(item);

                var row = `<tr>
                      

                          <td>${item.ic}</td>
                            <td>${item.icName}</td>
                              <td>${item.itcode}</td>
                                <td>${item.itname}</td>
                                  <td>desc</td>
                                    <td>${item.prate}</td>
                                      <td>${item.rate}</td>
                                        <td>${item.unit}</td>
                                          <td>${item.unit}</td>
                                            <td>${item.weight}</td>
                                               <td>${item.openAmt}</td>
                                                 <td>${item.openAmt}</td>



                     
                        <td>
                            <button type="button" class="btn btn-info editRow">Edit</button>
                         
                        </td>
                    </tr>`;
                tableBody.append(row);
            });
        }
    });
}
$("#addButton").click(function () {
    var itemHeader = $("#ItemCatergory").val();
    debugger;
    if (itemHeader) {

        var ItemCatergory = $('#ItemCatergory').val();
        var ItemName = $('#ItemName').val();
        var ItemDesc = $('#ItemDesc').val();
        var ItemPurchase = $('#ItemPurchase').val();
        var ItemSale = $('#ItemSale').val();
        var ItemUnit = $('#ItemUnit').val();
        var ItemOpeningQuantity = $('#ItemOpeningQuantity').val();
        var ItemOpeningAmt = $('#ItemOpeningAmt').val();




        var data = { Ic: ItemCatergory, Itname: ItemName, Desc: ItemDesc, Prate: ItemPurchase, Rate: ItemSale, UNIT: ItemUnit, WEIGHT: ItemOpeningQuantity, AMT: ItemOpeningAmt }; // Adjust as necessary

        if (editingRow) {
            // Update existing row
            var id = $(editingRow).find("td:eq(2)").text();
            var Name = itemHeader;//$(editingRow).find("td:eq(1)").text();

            var ID = $(editingRow).find("td:eq(2)").text();
      

            var UpdateDate = { Ic: ItemCatergory, Itname: ItemName, Desc: ItemDesc, Prate: ItemPurchase, Rate: ItemSale, UNIT: ItemUnit, WEIGHT: ItemOpeningQuantity, AMT: ItemOpeningAmt, ITCODE: ID }; // Adjust as necessary


            $.ajax({
                url: 'UpdateItemRegistration',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(UpdateDate),
                success: function (response) {
                    if (response.success) {
                        loadTableData(); // Reload table data
                        $("#addButton").text("Add").removeClass('btn-warning').addClass('btn-primary');
                        editingRow = null;

                        Swal.fire({
                            title: 'Updated!',
                            text: response.message,
                            icon: 'success',
                            confirmButtonText: 'OK'
                        });
                    } else {
                        Swal.fire({
                            title: 'Error!',
                            text: response.message,
                            icon: 'error',
                            confirmButtonText: 'OK'
                        });
                    }
                }
            });
        } else {
            // Add new row
            $.ajax({
                url: 'AddItemRegistration',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(data),
                success: function (response) {
                    if (response.success) {
                        loadTableData(); // Reload table data

                        Swal.fire({
                            title: 'Added!',
                            text: response.message,
                            icon: 'success',
                            confirmButtonText: 'OK'
                        });
                    } else {
                        Swal.fire({
                            title: 'Error!',
                            text: response.message,
                            icon: 'error',
                            confirmButtonText: 'OK'
                        });
                    }
                }
            });
        }

        // Clear input fields
        $("#ItemCatergory").val('');
        $("#ItemName").val('');
        $("#ItemDesc").val('');
        $("#ItemPurchase").val('');
        $("#ItemSale").val('');
        $("#ItemUnit").val('');
        $("#ItemOpeningQuantity").val('');
        $("#ItemOpeningAmt").val('');
        $("#ItemHead").val('');
       


    } else {
        Swal.fire({
            title: 'Error!',
            text: 'Please fill all fields.',
            icon: 'error',
            confirmButtonText: 'OK'
        });
    }
});

// Edit row
$("#myTable").on("click", ".editRow", function () {
    editingRow = $(this).closest("tr");
    var ItemCatergory = $(editingRow).find("td:eq(0)").text();
    var ItemName = $(editingRow).find("td:eq(3)").text();
    var ItemDesc = $(editingRow).find("td:eq(4)").text();
    var ItemPurchase = $(editingRow).find("td:eq(5)").text();
    var ItemSale = $(editingRow).find("td:eq(6)").text();
    var ItemUnit = $(editingRow).find("td:eq(7)").text();
    var ItemOpeningQuantity = $(editingRow).find("td:eq(9)").text();
    var ItemOpeningAmt = $(editingRow).find("td:eq(10)").text();
    var ItemHead = $(editingRow).find("td:eq(11)").text();



    $("#ItemCatergory").val(ItemCatergory);
    $("#ItemName").val(ItemName);
    $("#ItemDesc").val(ItemDesc);
    $("#ItemPurchase").val(ItemPurchase);
    $("#ItemOpeningQuantity").val(ItemOpeningQuantity);
    $("#ItemUnit").val(ItemUnit);
    $("#ItemOpeningAmt").val(ItemOpeningAmt);
    $("#ItemSale").val(ItemSale);
    $("#ItemHead").val(ItemHead);

    $("#addButton").text("Update").removeClass('btn-primary').addClass('btn-warning');
});


$(document).ready(function () {
    loadTableData();
});

