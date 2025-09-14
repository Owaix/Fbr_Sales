using System;
using System.Collections.Generic;

public class ErrorDetail
{
    public string Code { get; set; }
    public string Description { get; set; }

    public ErrorDetail(string code, string description)
    {
        Code = code;
        Description = description;
    }
}

public class FBRErrorCodes
{
    List<ErrorDetail> errorList = new List<ErrorDetail>();
    public string GetErrorByCode(string code)
    {
        var error = errorList.FirstOrDefault(e => e.Code == code);
        if (error != null)
        {
            return error.Description;
        }
        return "";
    }
    public FBRErrorCodes()
    {
        errorList = new List<ErrorDetail>
        {
            new ErrorDetail("0001", "Seller not registered for sales tax, please provide valid registration/NTN."),
            new ErrorDetail("0002", "Buyer Registration Number or NTN is not in proper format, please provide buyer registration number in 13 digits or NTN in 7 or 9 digits."),
            new ErrorDetail("0003", "Invoice type is not valid or empty, please provide valid invoice type."),
            new ErrorDetail("0005", "Invoice date is not in proper format, please provide invoice date in 'YYYY-MM-DD' format. For example: 2025-05-25."),
            new ErrorDetail("0006", "Sales invoice does not exist against STWH."),
            new ErrorDetail("0007", "Selected invoice type is not associated with proper registration number, please select actual invoice type."),
            new ErrorDetail("0008", "ST withheld at source is not equal to zero or sales tax, please enter ST withheld at source zero or equal to sales tax."),
            new ErrorDetail("0009", "Buyer Registration Number cannot be empty, please provide proper buyer registration number."),
            new ErrorDetail("0010", "Buyer Name cannot be empty, please provide valid buyer name."),
            new ErrorDetail("0012", "Buyer Registration type cannot be empty, please provide valid Buyer Registration type."),
            new ErrorDetail("0011", "Invoice type cannot be empty, please provide valid invoice type."),
            new ErrorDetail("0013", "Sale type cannot be empty/null, please provide valid sale type."),
            new ErrorDetail("0018", "Sales Tax/FED cannot be empty, please provide valid Sales Tax/FED."),
            new ErrorDetail("0019", "HS Code cannot be empty, please provide valid HS Code."),
            new ErrorDetail("0020", "Rate field cannot be empty, please provide Rate."),
            new ErrorDetail("0021", "Value of Sales Excl. ST /Quantity cannot be empty, please provide valid Value of Sales Excl. ST /Quantity."),
            new ErrorDetail("0022", "ST withheld at Source or STS Withheld cannot be empty, please provide valid ST withheld at Source or STS Withheld."),
            new ErrorDetail("0023", "Sales Tax cannot be empty, please provide valid Sales Tax."),
            new ErrorDetail("0024", "Sales Tax withheld cannot be empty, please provide valid Sales Tax withheld."),
            new ErrorDetail("0026", "Invoice Reference No. is mandatory requirement for debit/credit note. Please provide valid Invoice Reference No."),
            new ErrorDetail("0027", "Reason is mandatory requirement for debit/credit note. Please provide valid reason for debit/credit note."),
            new ErrorDetail("0028", "Reason is selected as 'Others'. Please provide valid remarks against this reason."),
            new ErrorDetail("0029", "Debit/Credit note date should be equal or greater from original invoice date."),
            new ErrorDetail("0030", "Unregistered distributer type not allowed before system cut of date."),
            new ErrorDetail("0031", "Sales Tax is not mentioned, please provide Sales Tax."),
            new ErrorDetail("0032", "User is not FTN holder, STWH can only be created for GOV/FTN Holders without sales invoice."),
            new ErrorDetail("0034", "Debit/Credit note can only be added within 180 days of original invoice date."),
            new ErrorDetail("0035", "Note Date must be greater or equal to original invoice date."),
            new ErrorDetail("0036", "Credit Note Value of Sale must be less or equal to the value of Sale in original invoice."),
            new ErrorDetail("0037", "Credit Note Value of ST Withheld must be less or equal to the value of ST Withheld in original invoice."),
            new ErrorDetail("0039", "For registered users, STWH invoice fields must be same as sale invoice."),
            new ErrorDetail("0041", "Invoice number cannot be empty, please provide invoice number."),
            new ErrorDetail("0042", "Invoice date cannot be empty, please provide invoice date."),
            new ErrorDetail("0043", "Invoice date is not valid, please provide valid invoice date."),
            new ErrorDetail("0044", "HS Code cannot be empty, please provide HS Code."),
            new ErrorDetail("0046", "Rate cannot be empty, please provide valid rate as per selected Sales Type."),
            new ErrorDetail("0050", "For sale type 'Cotton ginners', Sales Tax Withheld must be equal to Sales Tax or zero."),
            new ErrorDetail("0052", "HS Code that does not match with provided sale type, please provide valid HS Code against sale type."),
            new ErrorDetail("0053", "Buyer Registration Type is invalid, please provide valid Buyer Registration Type."),
            new ErrorDetail("0055", "Sales tax withheld cannot be empty or invalid format. Please provide valid sales tax withheld."),
            new ErrorDetail("0056", "Buyer does not exist in steel sector."),
            new ErrorDetail("0057", "Reference invoice for debit/credit note does not exist. Please provide valid Invoice Reference No."),
            new ErrorDetail("0058", "Buyer and Seller Registration number are same, this type of invoice is not allowed."),
            new ErrorDetail("0064", "Credit note is already added to an invoice."),
            new ErrorDetail("0067", "Sales Tax value of Debit Note is greater than original invoice's sales tax."),
            new ErrorDetail("0068", "Sales Tax value of Credit Note is less than original invoice's sales tax according to the rate."),
            new ErrorDetail("0070", "User is not registered, STWH is allowed only for registered user."),
            new ErrorDetail("0071", "Credit note allowed to add only for specific users."),
            new ErrorDetail("0073", "Sale Origination Province of Supplier cannot be empty, please provide valid Sale Origination Province of Supplier."),
            new ErrorDetail("0074", "Destination of Supply cannot be empty, please provide valid Destination of Supply."),
            new ErrorDetail("0077", "SRO/Schedule Number cannot be empty, please provide valid SRO/Schedule Number."),
            new ErrorDetail("0078", "Item serial number cannot be empty, please provide valid item serial number."),
            new ErrorDetail("0079", "If sales value is greater than 20,000 then rate 5% is not allowed."),
            new ErrorDetail("0080", "Further Tax cannot be empty, please provide valid Further Tax."),
            new ErrorDetail("0081", "'Input Credit not Allowed' cannot be empty, please provide ‘Input Credit not Allowed'."),
            new ErrorDetail("0082", "The Seller is not registered for sales tax. Please provide a valid registration/NTN."),
            new ErrorDetail("0083", "Seller Reg No. doesn’t match. Please provide valid Seller Registration Number."),
            new ErrorDetail("0085", "Total Value of Sales is not provided, please provide valid Total Value of Sales (In case of PFAD only)."),
            new ErrorDetail("0086", "You are not an EFS license holder who has imported Compressor Scrap in the last 12 months."),
            new ErrorDetail("0087", "Petroleum Levy rates not configured properly. Please update levy rates properly."),
            new ErrorDetail("0088", "Invoice number is not valid, please provide valid invoice number in alphanumeric format. For example: Inv-001."),
            new ErrorDetail("0089", "FED Charged cannot be empty, please provide valid FED Charged."),
            new ErrorDetail("0090", "Fixed / notified value or Retail Price cannot be empty, please provide valid Fixed / notified value or Retail Price."),
            new ErrorDetail("0091", "Extra tax must be empty."),
            new ErrorDetail("0092", "Purchase type cannot be empty, please provide valid purchase type."),
            new ErrorDetail("0093", "Selected Sale is not allowed to Manufacturer. Please select proper sale type."),
            new ErrorDetail("0095", "Extra Tax cannot be empty, please provide valid extra tax."),
            new ErrorDetail("0096", "For provided HS Code, only KWH UOM is allowed."),
            new ErrorDetail("0097", "Please provide UOM in KG."),
            new ErrorDetail("0098", "Quantity / Electricity Unit cannot be empty, please provide valid Quantity / Electricity Unit."),
            new ErrorDetail("0099", "UOM is not valid. UOM must be according to given HS Code."),
            new ErrorDetail("0100", "Registered user cannot add sale invoice. Only cotton ginner sale type is allowed for registered users."),
            new ErrorDetail("0101", "Sale type is not selected properly, please use Toll Manufacturing Sale Type for Steel Sector."),
            new ErrorDetail("0102", "The calculated sales tax not calculated as per 3rd schedule calculation formula."),
            new ErrorDetail("0103", "Calculated tax not matched for potassium chlorate. Calculated value doesn’t match according to potassium chlorate for sales potassium invoices."),
            new ErrorDetail("0104", "Calculated percentage of sales tax not matched. Calculation must be correct with respect to provided rate."),
            new ErrorDetail("0105", "The calculated sales tax for the quantity is incorrect."),
            new ErrorDetail("0106", "The Buyer is not registered for sales tax. Please provide a valid registration/NTN."),
            new ErrorDetail("0107", "Buyer Reg No. doesn’t match. Please provide valid Buyer Registration Number."),
            new ErrorDetail("0108", "Seller Reg No. is not valid. Please provide valid Seller Registration Number/NTN."),
            new ErrorDetail("0109", "Invoice type is not selected properly, please select proper invoice type."),
            new ErrorDetail("0111", "Purchase type is not selected properly, please provide proper purchase type."),
            new ErrorDetail("0113", "Date is not in proper format, please provide date in 'YYYY-MM-DD' format. For example: 2025-05-25."),
            new ErrorDetail("0300", "Discount Value is not valid at item 1 | Total Value is not valid at item 1 | Fed Payable Value is not valid at item 1 | Extra Tax Value is not valid at item 1 | Further Tax Value is not valid at item 1 | SalesTaxWithheldAtSource Value is not valid at item 1 | Quantity Value is not valid at item 1."),
            new ErrorDetail("0401", "Unauthorized access: Provided seller registration number is not 13 digits (CNIC) or 7 digits (NTN) or the authorized token does not exist against seller registration number."),
            new ErrorDetail("0402", "Unauthorized access: Provided buyer registration number is not 13 digits (CNIC) or 7 digits (NTN) or the authorized token does not exist against buyer registration number.")
        };
    }

}