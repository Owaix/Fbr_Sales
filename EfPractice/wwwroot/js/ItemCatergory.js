


    var editingRow = null;

    function loadTableData() {
        $.ajax({
            url: 'GetCatergoryRegistrarion',
            type: 'GET',
            success: function (data) {
                console.log(data);

                var tableBody = $("#myTable tbody");
                tableBody.empty(); // Clear existing rows
                data.forEach(function (item) {

                   
                    var row = `<tr>
                        <td data-id="${item.cid}">${item.cid}</td>
                        <td>${item.name}</td>
                        <td data-id="${item.mid}">${item.mid}</td>
                        <td>${item.mn}</td>
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

        if (itemHeader) {
            // Get the selected category ID
            var selectedCategoryId = $('#ItemHead').val();
            // Get the item header value
            var itemHeader = $('#ItemCatergory').val();
            debugger;
            var data = { Mid: selectedCategoryId, Name: itemHeader }; // Adjust as necessary

            if (editingRow) {
                // Update existing row
                var id = $(editingRow).find("td:eq(0)").text();
                var Name = itemHeader;//$(editingRow).find("td:eq(1)").text();
              
                var CatergoryID = $(editingRow).find("td:eq(0)").text();
                var CatergoryName = $("#ItemCatergory").val();
                var HeaderId = $("#ItemHead").val();

                var UpdateDate = { Mid: HeaderId, Cid: CatergoryID, Name: CatergoryName };

                $.ajax({
                    url: 'UPDATECatergoryRegistrarion',
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
                    url: 'ADDCatergoryRegistrarion',
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
            $("#ItemHeader").val('');
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
        var CatergoryID = $(editingRow).find("td:eq(0)").text();
        var CatergoryName = $(editingRow).find("td:eq(1)").text();
        var HeaderId = $(editingRow).find("td:eq(2)").text();

$("#ItemHead").val(HeaderId);
$("#ItemCatergory").val(CatergoryName);
        $("#addButton").text("Update").removeClass('btn-primary').addClass('btn-warning');
    });

   
    $(document).ready(function() {
        loadTableData();
    });

