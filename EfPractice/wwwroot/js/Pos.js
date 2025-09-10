$(document).ready(function () {
    var selectedRow = null;

    // --- Helpers ---
    function calculateRowTotal(row) {
        var price = parseFloat(row.find("td:eq(3)").text()) || 0;
        var qty = parseInt(row.find("td:eq(4)").text()) || 0;
        var discount = parseFloat(row.find("td:eq(5)").text()) || 0;
        var subtotal = (price * qty) - discount;
        if (subtotal < 0) subtotal = 0;
        row.find("td:eq(6)").text(subtotal.toFixed(2));
    }

    function updateTotals() {
        var netAmount = 0, totalQty = 0;
        $("#myTable tbody tr").each(function () {
            var qty = parseInt($(this).find("td:eq(4)").text()) || 0;
            var rowTotal = parseFloat($(this).find("td:eq(6)").text()) || 0;
            totalQty += qty;
            netAmount += rowTotal;
        });

        var taxRate = parseFloat($("#taxRate").val()) || 0;
        var tax = (netAmount * taxRate / 100);
        var grand = netAmount + tax;

        $("#totalQuantity").text(totalQty);
        $("#netAmount").text(netAmount.toFixed(2));
        $("#salesTax").text(tax.toFixed(2));
        $("#grandTotal").text(grand.toFixed(2));
    }

    function renumberRows() {
        $("#myTable tbody tr").each(function (index) {
            $(this).find("td:eq(0)").text(index + 1);
        });
    }

    // --- Add Row ---
    function addRow() {
        var newRow = `<tr>
      <td></td>
      <td contenteditable="true">Category</td>
      <td contenteditable="true">Item</td>
      <td contenteditable="true">0</td>
      <td contenteditable="true">1</td>
      <td contenteditable="true">0</td>
      <td>0.00</td>
    </tr>`;
        $("#myTable tbody").append(newRow);
        renumberRows();
        updateTotals();

        var lastRow = $("#myTable tbody tr").last();
        lastRow.find("td:eq(1)").focus();
    }
    $("#addRow").click(addRow);

    // --- Delete Row ---
    function deleteRow() {
        if (selectedRow) {
            selectedRow.remove();
            selectedRow = null;
            renumberRows();
            updateTotals();
        } else {
            Swal.fire("Select a row first!", "", "warning");
        }
    }
    $("#deleteRow").click(deleteRow);

    // --- Update Row ---
    function updateRow(row) {
        calculateRowTotal(row);
        updateTotals();
        Swal.fire("Updated!", "Row recalculated successfully.", "success");
    }

    // --- Row Selection ---
    $("#myTable").on("click", "tr", function () {
        $("#myTable tr").removeClass("selected-row");
        $(this).addClass("selected-row");
        selectedRow = $(this);
    });

    // --- Inline Editing ---
    $("#myTable").on("input", "td[contenteditable='true']", function () {
        var row = $(this).closest("tr");
        calculateRowTotal(row);
        updateTotals();
    });

    // --- Excel-like Navigation ---
    $("#myTable").on("focus", "td[contenteditable='true']", function () {
        $("td").removeClass("active-cell");
        $(this).addClass("active-cell");
        selectedRow = $(this).closest("tr");
    });

    $("#myTable").on("keydown", "td[contenteditable='true']", function (e) {
        var cell = $(this);
        var row = cell.closest("tr");
        var rowIndex = row.index();
        var colIndex = cell.index();
        var totalRows = $("#myTable tbody tr").length;
        var totalCols = row.find("td").length;

        function focusCell(r, c) {
            var targetRow = $("#myTable tbody tr").eq(r);
            var targetCell = targetRow.find("td").eq(c);
            if (targetCell.attr("contenteditable")) {
                targetCell.focus();
            }
        }

        if (e.key === "ArrowRight") {
            e.preventDefault();
            if (colIndex < totalCols - 2) {
                focusCell(rowIndex, colIndex + 1);
            } else if (rowIndex + 1 < totalRows) {
                focusCell(rowIndex + 1, 1);
            } else {
                addRow();
            }
        }

        if (e.key === "ArrowLeft") {
            e.preventDefault();
            if (colIndex > 1) {
                focusCell(rowIndex, colIndex - 1);
            } else if (rowIndex > 0) {
                focusCell(rowIndex - 1, totalCols - 2);
            }
        }

        if (e.key === "ArrowDown") {
            e.preventDefault();
            if (rowIndex + 1 < totalRows) focusCell(rowIndex + 1, colIndex);
        }

        if (e.key === "ArrowUp") {
            e.preventDefault();
            if (rowIndex > 0) focusCell(rowIndex - 1, colIndex);
        }

        if (e.key === "Enter") {
            e.preventDefault();
            updateRow(row);
        }
    });

    // --- Shortcuts ---
    $(document).keydown(function (e) {
        if (e.ctrlKey && e.key.toLowerCase() === "a") {
            e.preventDefault();
            addRow();
        }
        if (e.key === "Delete") {
            e.preventDefault();
            deleteRow();
        }
        if (e.ctrlKey && e.key.toLowerCase() === "s") {
            e.preventDefault();
            $("#saveButton").click();
        }
    });

    // --- Save Button ---
    $("#saveButton").click(function () {
        var data = [];
        $("#myTable tbody tr").each(function () {
            var row = $(this);
            data.push({
                rowNo: row.find("td:eq(0)").text(),
                category: row.find("td:eq(1)").text(),
                item: row.find("td:eq(2)").text(),
                price: row.find("td:eq(3)").text(),
                quantity: row.find("td:eq(4)").text(),
                discount: row.find("td:eq(5)").text(),
                total: row.find("td:eq(6)").text()
            });
        });
        console.log(data);
        Swal.fire("Data Saved!", "Your table data has been saved.", "success");
    });

    // Start with one row
    addRow();
});
