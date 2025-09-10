


    var editingRow = null;

    function loadTableData() {
        $.ajax({
            url: 'GetInventoryHeadList',
            type: 'GET',
            success: function (data) {
                console.log(data);

                var tableBody = $("#myTable tbody");
                tableBody.empty(); // Clear existing rows
                data.forEach(function (item) {

                    var item1 = item.mid;
                    console.log(item.mname);
                    console.log(item1);
                    console.log(item["Mid"]);
                    var row = `<tr>
                        <td data-id="${item.mid}">${item.mid}</td>
                        <td>${item.mname}</td>
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
        var itemHeader = $("#ItemHeader").val();

        if (itemHeader) {

            debugger;
            var data = { Mname: itemHeader }; // Adjust as necessary

            if (editingRow) {
                // Update existing row
                var id = $(editingRow).find("td:eq(0)").text();
                var Name = itemHeader;//$(editingRow).find("td:eq(1)").text();
                var UpdateDate = { Mid: id, Mname: Name };
              

                $.ajax({
                    url: 'UpdateInventoryHead',
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
                    url: 'AddInventoryHead',
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
        var itemId = $(editingRow).find("td:eq(0)").data('id');
        var itemHeader = $(editingRow).find("td:eq(1)").text();

        $("#ItemHeader").val(itemHeader);
        $("#addButton").text("Update").removeClass('btn-primary').addClass('btn-warning');
    });

   
    $(document).ready(function() {
      /*  loadTableData();*/
    });

