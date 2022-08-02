using EdiEngine;
using EdiEngine.Runtime;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Business_Central_EDI.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EdiEngine.Standards.X12_004010.Maps;
using System.Web;
using EdiEngine.Common.Definitions;
using SegmentDefinitions = EdiEngine.Standards.X12_004010.Segments;

namespace Business_Central_EDI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("** EDI to Business Central **");
            List<Company> companies = ListCompanies();
            Console.WriteLine(">> Selected Company = " + companies[0].name);

            var argsList = HttpUtility.UrlDecode(args[0]).Trim().Replace("businesscentraledi:", "").Replace(" ","").Split("|");
            
            string EDI_Type = argsList[0];

            if (args.Length == 0 || EDI_Type == "850")
            {
                Console.WriteLine("** Start EDI 850 **");
                AddSalesOrder(companies[0].id);
            }
            else if (EDI_Type == "810")
            {
                //var SelectedRow = HttpUtility.UrlDecode(argsList[1]);
                Console.WriteLine("** Start EDI 810 **");
                EDI810(companies[0].id);
            }
            else if (EDI_Type == "856")
            {
                Console.WriteLine("** Start EDI 856 **");
            }
            else if (EDI_Type == "852")
            {
                Console.WriteLine("** Start EDI 852 **");
            }

            Console.WriteLine(">> Done.");
            Console.ReadKey();
        }
        static List<Company> ListCompanies()
        {
            RestSharp.RestClient client = CreateRestClient(GetCompanyParameters());

            RestSharp.RestRequest request = GetRequest(RestSharp.Method.GET, new JObject());

            RestSharp.IRestResponse response = client.Execute(request);
            List<Company> companies = JsonConvert.DeserializeObject<List<Company>>(ConvertToDataSet(response.Content.ToString()).ToString());
            return companies;
        }
        static SalesOrder AddSalesOrder(Guid CompanyId)
        {
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\EDI\\Receive\\AMAZON\\850.edi";
            if (!File.Exists(filepath))
                return null;


            string edi = File.ReadAllText(filepath);

            EdiDataReader r = new EdiDataReader();
            EdiBatch b = r.FromString(edi);

            //Serialize the whole batch to JSON
            JsonDataWriter w1 = new JsonDataWriter();
            string json = w1.WriteToString(b);

            //OR Serialize selected EDI message to Json
            string jsonTrans = JsonConvert.SerializeObject(b.Interchanges[0].Groups[0].Transactions[0]);
            EDI_Model EDI = JsonConvert.DeserializeObject<EDI_Model>(jsonTrans);




            RestSharp.RestClient client = CreateRestClient(GetSalesOrdersParameters(CompanyId));

            string jsonString = @"{
                              customerNumber: 'C00010',
                            }";


            RestSharp.RestRequest request = GetRequest(RestSharp.Method.POST, JObject.Parse(jsonString));

            RestSharp.IRestResponse response = client.Execute(request);
            SalesOrder salesOrder = JsonConvert.DeserializeObject<SalesOrder>(response.Content);

            if (response.IsSuccessful)
            {
                Console.WriteLine(">> Sales Order Created [Number=" + salesOrder.number + "]");

                foreach (var L_PO1 in EDI.Content.Where(p => p.Name == "L_PO1"))
                {
                    var item = L_PO1.Content.First(p => p.Name == "PO1");
                    var itemL = L_PO1.Content.First(p => p.Name == "L_PID").Content.First(p => p.Name == "PID");


                    dynamic jsonObject = new JObject();
                    jsonObject.lineType = "Item";
                    jsonObject.lineObjectNumber = item.Content.Skip(10).First().E;
                    jsonObject.unitOfMeasureCode = item.Content.Skip(2).First().E;
                    jsonObject.quantity = Convert.ToDecimal(item.Content.Skip(1).First().E);
                    jsonObject.unitPrice = Convert.ToDecimal(item.Content.Skip(3).First().E);
                    jsonObject.description = itemL.Content.Skip(4).First().E;


                    RestSharp.RestClient clientSalesOrderLines = CreateRestClient(GetSalesOrderLinesParameters(CompanyId, salesOrder.id));


                    RestSharp.RestRequest requestSalesOrderLines = GetRequest(RestSharp.Method.POST, jsonObject);
                    RestSharp.IRestResponse responseSalesOrderLines = clientSalesOrderLines.Execute(requestSalesOrderLines);

                    if (responseSalesOrderLines.IsSuccessful)
                    {
                        Console.WriteLine(">> Added item to Sales Order [Item=" + jsonObject.description + "]");
                    }
                    else
                    {
                        var errorModel = JsonConvert.DeserializeObject<ErrorModel>(responseSalesOrderLines.Content);

                        Console.WriteLine(">> Error in add item to Sales Order [" + errorModel.error.message + "]" + " [Item=" + jsonObject.description + "]");
                    }
                }
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModel>(response.Content);
                Console.WriteLine(">> Error in Sales Order Create [" + errorModel.error.message + "]");
            }



            return salesOrder;
        }

        static List<SalesOrder> EDI810(Guid CompanyId)
        {
            RestSharp.RestClient client = CreateRestClient(GetSalesOrdersParameters(CompanyId));
            RestSharp.RestRequest request = GetRequest(RestSharp.Method.GET, new JObject());

            RestSharp.IRestResponse response = client.Execute(request);
            List<SalesOrder> salesOrders = JsonConvert.DeserializeObject<List<SalesOrder>>(ConvertToDataSet(response.Content.ToString()).ToString()).OrderByDescending(p => p.lastModifiedDateTime).Take(5).ToList();


            M_810 map = new M_810();
            var g = new EdiGroup("IN");

            foreach (var salesOrder in salesOrders)
            {
                EdiTrans t1 = new EdiTrans(map);

                var sDef = (MapSegment)map.Content.First(s => s.Name == "BIG");
                var seg = new EdiSegment(sDef);
                seg.Content.AddRange(new[] {
                    new EdiSimpleDataElement((MapSimpleDataElement)sDef.Content[0], salesOrder.orderDate.Value.ToString("yyyyMMdd")),
                    new EdiSimpleDataElement((MapSimpleDataElement)sDef.Content[1], salesOrder.number),
                    new EdiSimpleDataElement((MapSimpleDataElement)sDef.Content[2], null),
                    new EdiSimpleDataElement((MapSimpleDataElement)sDef.Content[4], null)
                });

                t1.Content.Add(seg);



                RestSharp.RestClient clientSalesOrderLines = CreateRestClient(GetSalesOrderLinesParameters(CompanyId, salesOrder.id));
                RestSharp.RestRequest requestSalesOrderLines = GetRequest(RestSharp.Method.GET, new JObject());
                RestSharp.IRestResponse responseSalesOrderLines = clientSalesOrderLines.Execute(requestSalesOrderLines);
                List<SalesOrderLine> salesOrderLines = JsonConvert.DeserializeObject<List<SalesOrderLine>>(ConvertToDataSet(responseSalesOrderLines.Content.ToString()).ToString());






                var lDef_L_IT = (MapLoop)map.Content.First(s => s.Name == "L_IT1");
                var sDef_IT = (MapSegment)lDef_L_IT.Content.First(s => s.Name == "IT1");
                var sDef_TXI = (MapSegment)lDef_L_IT.Content.First(s => s.Name == "TXI");

                EdiLoop IT = new EdiLoop(lDef_L_IT, null);
                t1.Content.Add(IT);

                int index = 1;
                foreach (var salesOrderLine in salesOrderLines)
                {
                    seg = new EdiSegment(sDef_IT);
                    seg.Content.Add(new EdiSimpleDataElement(sDef_IT.Content[0], index.ToString("00")));
                    seg.Content.Add(new EdiSimpleDataElement(sDef_IT.Content[0], salesOrderLine.quantity.ToString()));
                    seg.Content.Add(new EdiSimpleDataElement(sDef_IT.Content[0], salesOrderLine.unitOfMeasureCode));
                    seg.Content.Add(new EdiSimpleDataElement(sDef_IT.Content[0], salesOrderLine.unitPrice.ToString()));
                    IT.Content.Add(seg);


                    seg = new EdiSegment(sDef_TXI);
                    //seg.Content.Add(new EdiSimpleDataElement(sDef_IT.Content[0], salesOrderLine.taxCode.Replace(salesOrderLine.taxPercent.ToString(), "").ToString()));
                    seg.Content.Add(new EdiSimpleDataElement(sDef_IT.Content[0], "VA"));
                    seg.Content.Add(new EdiSimpleDataElement(sDef_IT.Content[0], salesOrderLine.totalTaxAmount.ToString()));
                    IT.Content.Add(seg);

                    index++;
                }

                var sDef_TDS = (MapSegment)map.Content.First(s => s.Name == "TDS");
                seg = new EdiSegment(sDef_TDS);
                seg.Content.Add(new EdiSimpleDataElement(sDef_IT.Content[0], salesOrder.totalAmountIncludingTax.ToString()));
                IT.Content.Add(seg);

                g.Transactions.Add(t1);
            }

            var i = new EdiInterchange();
            i.Groups.Add(g);

            EdiBatch eBatch = new EdiBatch();
            eBatch.Interchanges.Add(i);


            //Add all service segments
            EdiDataWriterSettings settings = new EdiDataWriterSettings(
                new SegmentDefinitions.ISA(), new SegmentDefinitions.IEA(),
                new SegmentDefinitions.GS(), new SegmentDefinitions.GE(),
                new SegmentDefinitions.ST(), new SegmentDefinitions.SE(),
                "ZZ", "NEXTALINK", "ZZ", "AMAZON", "GSSENDER", "GSRECEIVER", "00401", "004010", "T", 100, 200, "\r\n", "*");

            EdiDataWriter ediWriter = new EdiDataWriter(settings);
            string sEdi = ediWriter.WriteToString(eBatch);


            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\EDI\\SEND\\Invoice_810\\810.edi";
            File.WriteAllText(filepath, sEdi);

            Console.WriteLine(">> EDI 810 Created.");

            JsonDataWriter w1 = new JsonDataWriter();
            string jsonEdi = w1.WriteToString(eBatch);

            filepath = AppDomain.CurrentDomain.BaseDirectory + "\\EDI\\SEND\\Invoice_810\\810.json";
            File.WriteAllText(filepath, jsonEdi);


            return salesOrders;
        }

        private static JArray ConvertToDataSet(string result)
        {
            dynamic json = JsonConvert.DeserializeObject(result);
            _ = new JArray();
            JArray myObject = json["value"];

            return myObject;
        }

        private static RestSharp.RestRequest GetRequest(RestSharp.Method method, JObject json)
        {
            var request = new RestSharp.RestRequest(method);

            Credentials credentials = new Credentials();

            byte[] UserInfo = System.Text.ASCIIEncoding.ASCII.GetBytes(String.Format("{0}:{1}", credentials.UserId, credentials.AccessKey));

            request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(UserInfo));
            request.AddHeader("Content-Type", "application/json");
            if (json.Count > 0)
                request.AddParameter("application/json", json.ToString(), RestSharp.ParameterType.RequestBody);
            return request;
        }

        private static RestSharp.RestClient CreateRestClient(APIEndpoint endpoint)
        {
            var client = new RestSharp.RestClient(endpoint.getURI())
            {
                Timeout = -1
            };
            return client;
        }

        private static APIEndpoint GetCompanyParameters()
        {
            APIEndpoint endpoint = new APIEndpoint();
            return (endpoint);
        }
        private static APIEndpoint GetSalesOrdersParameters(Guid CompanyId)
        {
            APIEndpoint endpoint = new APIEndpoint();
            endpoint.companyId = CompanyId.ToString();
            endpoint.apiEndpoint = "salesOrders";
            return (endpoint);
        }
        private static APIEndpoint GetSalesOrderLinesParameters(Guid companyId, Guid salesOrderId)
        {
            APIEndpoint endpoint = new APIEndpoint();
            endpoint.companyId = companyId.ToString();
            endpoint.apiEndpoint = String.Format("salesOrders({0})/salesOrderLines", salesOrderId);
            return (endpoint);
        }
    }
}
