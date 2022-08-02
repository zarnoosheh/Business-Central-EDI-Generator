using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business_Central_EDI.Model
{
    public class Company
    {
        public Guid id { get; set; }
        public string systemVersion { get; set; }
        public string name { get; set; }
        public string displayName { get; set; }
        public string businessProfileId { get; set; }
        public DateTime systemCreatedAt { get; set; }
        public string systemCreatedBy { get; set; }
        public DateTime systemModifiedAt { get; set; }
        public string systemModifiedBy { get; set; }
    }

    public class Customer
    {
        public Guid id { get; set; }
        public string number { get; set; }
        public string displayName { get; set; }
        public string type { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string postalCode { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public string website { get; set; }
        public bool taxLiable { get; set; }
        public Guid taxAreaId { get; set; }
        public string taxRegistrationNumber { get; set; }
        public Guid currencyId { get; set; }
        public string currencyCode { get; set; }
        public Guid paymentTermsId { get; set; }
        public Guid shipmentMethodId { get; set; }
        public Guid paymentMethodId { get; set; }
        public string blocked { get; set; }

    }

    public class Items
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("itemCategoryId")]
        public Guid ItemCategoryId { get; set; }

        [JsonProperty("itemCategoryCode")]
        public string ItemCategoryCode { get; set; }

        [JsonProperty("blocked")]
        public bool Blocked { get; set; }

        [JsonProperty("gtin")]
        public string Gtin { get; set; }

        [JsonProperty("inventory")]
        public long Inventory { get; set; }

        [JsonProperty("unitPrice")]
        public double UnitPrice { get; set; }

        [JsonProperty("priceIncludesTax")]
        public bool PriceIncludesTax { get; set; }

        [JsonProperty("unitCost")]
        public double UnitCost { get; set; }

        [JsonProperty("taxGroupId")]
        public Guid TaxGroupId { get; set; }

        [JsonProperty("taxGroupCode")]
        public string TaxGroupCode { get; set; }

        [JsonProperty("baseUnitOfMeasureId")]
        public Guid BaseUnitOfMeasureId { get; set; }

        [JsonProperty("baseUnitOfMeasureCode")]
        public string BaseUnitOfMeasureCode { get; set; }

        [JsonProperty("generalProductPostingGroupId")]
        public Guid GeneralProductPostingGroupId { get; set; }

        [JsonProperty("generalProductPostingGroupCode")]
        public string GeneralProductPostingGroupCode { get; set; }

        [JsonProperty("inventoryPostingGroupId")]
        public Guid InventoryPostingGroupId { get; set; }

        [JsonProperty("inventoryPostingGroupCode")]
        public string InventoryPostingGroupCode { get; set; }

        [JsonProperty("lastModifiedDateTime")]
        public DateTimeOffset LastModifiedDateTime { get; set; }
    }
    public class SalesOrder
    {
        public Guid id { get; set; }
        public string number { get; set; }
        public string externalDocumentNumber { get; set; }
        public DateTime? orderDate { get; set; }
        public DateTime? postingDate { get; set; }
        public Guid customerId { get; set; }
        public string customerNumber { get; set; }
        public string customerName { get; set; }
        public string billToName { get; set; }
        public Guid billToCustomerId { get; set; }
        public string billToCustomerNumber { get; set; }
        public string shipToName { get; set; }
        public string shipToContact { get; set; }
        public string sellToAddressLine1 { get; set; }
        public string sellToAddressLine2 { get; set; }
        public string sellToCity { get; set; }
        public string sellToCountry { get; set; }
        public string sellToState { get; set; }
        public string sellToPostCode { get; set; }
        public string billToAddressLine1 { get; set; }
        public string billToAddressLine2 { get; set; }
        public string billToCity { get; set; }
        public string billToCountry { get; set; }
        public string billToState { get; set; }
        public string billToPostCode { get; set; }
        public string shipToAddressLine1 { get; set; }
        public string shipToAddressLine2 { get; set; }
        public string shipToCity { get; set; }
        public string shipToCountry { get; set; }
        public string shipToState { get; set; }
        public string shipToPostCode { get; set; }
        public Guid currencyId { get; set; }
        public string currencyCode { get; set; }
        public Boolean? pricesIncludeTax { get; set; }
        public Guid paymentTermsId { get; set; }
        public Guid shipmentMethodId { get; set; }
        public string salesperson { get; set; }
        public Boolean? partialShipping { get; set; }
        public DateTime? requestedDeliveryDate { get; set; }
        public Decimal discountAmount { get; set; }
        public Boolean? discountAppliedBeforeTax { get; set; }
        public Decimal totalAmountExcludingTax { get; set; }
        public Decimal totalTaxAmount { get; set; }
        public Decimal totalAmountIncludingTax { get; set; }
        public Boolean? fullyShipped { get; set; }
        public string status { get; set; }
        public DateTime? lastModifiedDateTime { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
    }
    public class SalesOrderLine
    {
        public Guid id { get; set; }
        public Guid documentId { get; set; }
        public int sequence { get; set; }
        public Guid itemId { get; set; }
        public Guid accountId { get; set; }
        public string lineType { get; set; }
        public string lineObjectNumber { get; set; }
        public string description { get; set; }
        public Guid unitOfMeasureId { get; set; }
        public string unitOfMeasureCode { get; set; }
        public Decimal quantity { get; set; }
        public Decimal unitPrice { get; set; }
        public Decimal discountAmount { get; set; }
        public Decimal discountPercent { get; set; }
        public bool discountAppliedBeforeTax { get; set; }
        public Decimal amountExcludingTax { get; set; }
        public string taxCode { get; set; }
        public Decimal taxPercent { get; set; }
        public Decimal totalTaxAmount { get; set; }
        public Decimal amountIncludingTax { get; set; }
        public Decimal invoiceDiscountAllocation { get; set; }
        public Decimal netAmount { get; set; }
        public Decimal netTaxAmount { get; set; }
        public Decimal netAmountIncludingTax { get; set; }
        public DateTime shipmentDate { get; set; }
        public Decimal shippedQuantity { get; set; }
        public Decimal invoicedQuantity { get; set; }
        public Decimal invoiceQuantity { get; set; }
        public Decimal shipQuantity { get; set; }
        public Guid itemVariantId { get; set; }
    }

    

    public class Credentials
    {
        public string UserId { get; set; } = "Admin";
        public string AccessKey { get; set; } = "SuDnxx3WQILp7RqvZo1BeJqWGmrjmuuokCN8G9fH/08=";
    }
    public class APIEndpoint
    {
        public string companyId { get; set; } = "your company ID here";
        public string apiPublisher { get; set; } = "api";
        public string apiGroup { get; set; } = "";
        public string apiVersion { get; set; } = "v2.0";
        public string apiEndpoint { get; set; } = "";
        public string sandBoxName { get; set; } = "sandbox";

        public String getURI()
        {
            String uri = "";
           

            
            if (apiEndpoint == "")
            {
                uri= (String.Format("http://bc.dadekavanco.com:7048/BC190/{0}/{1}/companies",
                  apiPublisher, apiVersion));
            }
            else
            {
                //if (apiGroup == "")
                    uri = (String.Format("http://bc.dadekavanco.com:7048/BC190/{0}/{1}/companies({2})/{3}",
                          apiPublisher, apiVersion, companyId, apiEndpoint));
                //else
                //    uri = (String.Format("https://api.businesscentral.dynamics.com/v2.0/{0}/{6}/{1}/{2}/{3}/companies({4})/{5}",
                //    tenantId, apiPublisher, apiGroup, apiVersion, companyId, apiEndpoint, sandBoxName));
            }
            return uri;
        }
    }



    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Error
    {
        public string code { get; set; }
        public string message { get; set; }
    }

    public class ErrorModel
    {
        public Error error { get; set; }
    }


}
