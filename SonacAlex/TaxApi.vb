Imports System.Collections.Specialized
Imports System.Data
Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Security.Cryptography
Imports System.Security.Cryptography.X509Certificates
Imports System.Text
Imports System.Web
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports Org.BouncyCastle.Asn1

Imports Net.Pkcs11Interop.Common
Imports Net.Pkcs11Interop.HighLevelAPI
Imports EssCertIDv2 = Org.BouncyCastle.Asn1.Ess.EssCertIDv2
Imports SigningCertificateV2 = Org.BouncyCastle.Asn1.Ess.SigningCertificateV2

Public Class TaxApi

    'Variable
    'Public apiBaseUrl As String = "https://api.preprod.invoicing.eta.gov.eg"
    'Public idSrvBaseUrl As String = "https://id.preprod.eta.gov.eg"
    'Public clientId As String = "daea7510-39b7-4179-b5a3-80e7fb1a42c0"
    'Public ClientSecret As String = "5e65bf5e-08ef-4e39-8a7d-6025e213e6e5"
    'Public ClientSecret2 As String = "7873e142-64d1-4743-8d06-fa531745bf95"

    Public apiBaseUrl As String = "https://api.invoicing.eta.gov.eg"
    Public idSrvBaseUrl As String = "https://id.eta.gov.eg"
    Public clientId As String = "c4297a1c-03d3-4293-a299-d304a4585e74"
    Public ClientSecret As String = "3623fd8d-bc44-4b70-b958-10d0b328608d"
    Public ClientSecret2 As String = "f5b40783-2611-4a1a-aeda-ecf37d6bb2b3"

    Private DllLibPath As String = "eps2003csp11.dll"
    Private TokenPin As String = ""

    Public access_token As String = ""

    Sub New(MyTokenPin As String)
        TokenPin = MyTokenPin
        Dim dt As DataTable = bm.ExecuteAdapter("select clientId,ClientSecret,ClientSecret2 from Statics")
        If dt.Rows.Count > 0 Then
            clientId = dt.Rows(0)("clientId").ToString
            ClientSecret = dt.Rows(0)("ClientSecret").ToString
            ClientSecret2 = dt.Rows(0)("ClientSecret2").ToString
        End If

    End Sub


    Dim bm As New BasicMethods
    Private Function PostRequest(uri As Uri, jsonDataBytes As Byte(), contentType As String, method As String) As String
        Try

            ServicePointManager.Expect100Continue = True
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

            Dim response As String
            Dim request As WebRequest
            request = WebRequest.Create(uri)
            request.ContentLength = jsonDataBytes.Length
            request.ContentType = contentType
            request.Method = method
            request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " & access_token)
            Using requestStream = request.GetRequestStream
                requestStream.Write(jsonDataBytes, 0, jsonDataBytes.Length)
                requestStream.Close()

                Dim responseStream = request.GetResponse.GetResponseStream
                Using reader As New StreamReader(responseStream)
                    response = reader.ReadToEnd()
                End Using

            End Using
            Return response
        Catch ex As Exception
            bm.ShowMSG(ex.Message)
            Return ""
        End Try
    End Function

    Private Function PostRequest(uri As Uri, jsonData As String, contentType As String, method As String) As String
        Try
            Dim response As String
            Dim request As WebRequest
            request = WebRequest.Create(uri)
            request.ContentLength = jsonData.Length
            request.ContentType = contentType
            request.Method = method
            request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " & access_token)

            Using requestStream As New StreamWriter(request.GetRequestStream())
                requestStream.Write(jsonData)
            End Using


            Dim httpResponse As HttpWebResponse = request.GetResponse()
            Using streamReader As New StreamReader(httpResponse.GetResponseStream())
                Dim result = streamReader.ReadToEnd()
            End Using

            Return response
        Catch ex As Exception
            bm.ShowMSG(ex.Message)
            Return ""
        End Try
    End Function

    Private Function PutRequest(uri As String, postData As String) As String
        Dim client = New HttpClient()
        Dim response As HttpResponseMessage
        Try
            client.BaseAddress = New Uri(apiBaseUrl)
            Dim byteArray As Byte() = Encoding.UTF8.GetBytes(postData)
            Using httpContent As HttpContent = New StringContent(postData)
                httpContent.Headers.ContentType = New MediaTypeHeaderValue("application/json")
                client.DefaultRequestHeaders.Authorization = New AuthenticationHeaderValue("Bearer", access_token)
                response = client.PutAsync(uri, httpContent).Result
                Return response.Content.ReadAsStringAsync().Result
            End Using
        Catch ex As Exception
            bm.ShowMSG(ex.Message)
            Return ""
        End Try
    End Function

    Private Function GetRequest(requestUrl As String) As String
        Try
            Dim request = TryCast(WebRequest.Create(requestUrl), HttpWebRequest)
            request.Method = "GET"
            request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " & access_token)
            request.ContentLength = 0
            Dim responseContent As String = ""
            Using response = TryCast(request.GetResponse(), HttpWebResponse)
                Using reader = New System.IO.StreamReader(response.GetResponseStream())
                    responseContent = reader.ReadToEnd()
                End Using
            End Using
            Return responseContent
        Catch ex As Exception
            bm.ShowMSG(ex.Message)
            Return ""
        End Try
    End Function


    '1. Login as Taxpayer System
    Public Sub Login()
        Try
            Dim outgoingQueryString As NameValueCollection = HttpUtility.ParseQueryString(String.Empty)
            outgoingQueryString.Add("grant_type", "client_credentials")
            outgoingQueryString.Add("client_id", clientId)
            outgoingQueryString.Add("client_secret", ClientSecret)
            outgoingQueryString.Add("scope", "InvoicingAPI")
            Dim jsonDataBytes = New ASCIIEncoding().GetBytes(outgoingQueryString.ToString())
            Dim result = PostRequest(New Uri(idSrvBaseUrl & "/connect/token"), jsonDataBytes, "application/x-www-form-urlencoded", "POST")
            access_token = JsonConvert.DeserializeObject(result)("access_token")
        Catch ex As Exception
            'bm.ShowMSG(ex.Message)
        End Try
    End Sub


    '2. Get Document Types
    Public DocumentTypes
    Public Sub GetDocumentTypes()
        Try
            Dim result = GetRequest(apiBaseUrl & "/api/v1.0/documenttypes")
            DocumentTypes = JsonConvert.DeserializeObject(result)("result")
        Catch ex As Exception
            'bm.ShowMSG(ex.Message)
        End Try
    End Sub

    '3. Get Document Type
    Public DocumentType
    Public Sub GetDocumentTypes(documentTypeID As Integer)
        Try
            Dim result = GetRequest(apiBaseUrl & "/api/v1.0/documenttypes/" & documentTypeID)
            DocumentType = JsonConvert.DeserializeObject(result)
        Catch ex As Exception
            'bm.ShowMSG(ex.Message)
        End Try
    End Sub

    '4. Get Document Type Version
    Public DocumentTypeVersion
    Public Sub GetDocumentTypeVersion(documentTypeID As Integer, documentTypeVersionID As Integer)
        Try
            Dim result = GetRequest(apiBaseUrl & "/api/v1/documenttypes/" & documentTypeID & "/versions/" & documentTypeVersionID)
            DocumentTypeVersion = JsonConvert.DeserializeObject(result)
        Catch ex As Exception
            'bm.ShowMSG(ex.Message)
        End Try
    End Sub
    '5.1 Submit Documents (JSON)
    Public Sub SubmitDocuments(Flag As Integer, StoreId As Integer, InvoiceNo As Integer)

        Try
            Dim invoiceLines As String = ""
            Dim taxTotalsLines As String = ""


            Dim TaxApi_issuerDT As DataTable = bm.ExecuteAdapter("select * from TaxApi_issuer")
            Dim SalesMasterDT As DataTable = bm.ExecuteAdapter("select * from SalesMaster where Flag=" & Flag & " and StoreId=" & StoreId & " and InvoiceNo=" & InvoiceNo)
            If SalesMasterDT.Rows(0)("Temp") = 1 Then Return

            Dim SalesDetailsDT As DataTable = bm.ExecuteAdapter("select D.*,It.itemCode,It.TaxApi_UnitTypeId,It.TaxApi_UnitTypeQty,It.Name It_Name from SalesDetails D left join Items It on (D.ItemId=It.Id) where D.Flag=" & Flag & " and D.StoreId=" & StoreId & " and D.InvoiceNo=" & InvoiceNo)
            Dim receiverDT As DataTable = bm.ExecuteAdapter("select * from Customers where Id=" & SalesMasterDT.Rows(0)("ToId"))

            Dim CurrencySign As String = bm.ExecuteScalar("select Sign from Currencies where Id=(" & SalesMasterDT.Rows(0)("CurrencyId") & ")")
            If CurrencySign = "" Then
                CurrencySign = "EGP"
            End If


            'Dim SalesDetailsTaxDT As DataTable = bm.ExecuteAdapter("select It.taxType,sum(It.rate*D.Value/100)amount from SalesDetails D left join taxableItems It on (D.ItemId=It.ItemId) where D.Flag=" & Flag & " and D.StoreId=" & StoreId & " and D.InvoiceNo=" & InvoiceNo & " group by It.taxType")
            Dim SalesMasterTaxDT As DataTable = bm.ExecuteAdapter("getSalesMasterTaxes", {"Flag", "StoreId", "InvoiceNo"}, {Flag, StoreId, InvoiceNo})

            Dim totalTaxAmount As Decimal = 0
            For Each acc In SalesMasterTaxDT.Rows
                If acc("taxType").ToString <> "" Then
                    taxTotalsLines &= "{
                    ""taxType"": """ & acc("taxType") & """,
                    ""amount"": " & Dec5(Math.Abs(acc("amount"))) & "
                    },"
                    totalTaxAmount += Dec5(acc("amount"))
                End If
            Next
            If taxTotalsLines.Length > 0 Then
                taxTotalsLines = taxTotalsLines.Substring(0, taxTotalsLines.Length - 1)
            End If

            Dim documentType As String = "I"
            If Flag = 14 OrElse Flag = 34 Then
                documentType = "C"
            ElseIf SalesMasterDT.Rows(0)("IsDebit") = 1 Then
                documentType = "D"
            End If

            Dim Weight As Decimal = 0

            For Each it In SalesDetailsDT.Rows

                'Dim taxableItemsDT As DataTable = bm.ExecuteAdapter("select * from taxableItems where ItemId=" & it("ItemId"))

                Weight += it("Qty") * it("TaxApi_UnitTypeQty")

                Dim quantity As Decimal = 0
                If it("SalesPriceTypeId") = 2 Then
                    quantity = Dec5(it("PreQty"))
                Else
                    quantity = Dec5(it("Qty") / IIf(it("Flag") = 33 OrElse it("Flag") = 34, 1000, 1))
                End If

                Dim itemTotalTax As Decimal = 0
                'For Each itD In taxableItemsDT.Rows
                '    itemTotalTax += Dec5(it("Value") * itD("rate") / 100)
                'Next
                For Each acc In SalesMasterTaxDT.Rows
                    itemTotalTax += Dec5(it("Value") * acc("rate") / 100)
                Next

                invoiceLines &= "{
                    ""description"": """ & it("It_Name") & """,
                    ""itemType"": ""GS1"",
                    ""itemCode"": """ & it("itemCode") & """,
                    ""unitType"": """ & it("TaxApi_UnitTypeId") & """,
                    ""quantity"": " & quantity & ",
                    ""internalCode"": """ & it("ItemId") & """,
                    ""salesTotal"": " & Dec5(it("Value") * IIf(SalesMasterDT.Rows(0)("CurrencyId") = 1, 1, SalesMasterDT.Rows(0)("Exchange"))) & ",
                    ""total"": " & Dec5((it("Value") + itemTotalTax) * IIf(SalesMasterDT.Rows(0)("CurrencyId") = 1, 1, SalesMasterDT.Rows(0)("Exchange"))) & ",
                    ""valueDifference"": 0.00000,
                    ""totalTaxableFees"": 0.00000,
                    ""netTotal"": " & Dec5(it("Value") * IIf(SalesMasterDT.Rows(0)("CurrencyId") = 1, 1, SalesMasterDT.Rows(0)("Exchange"))) & ",
                    ""itemsDiscount"": 0.00000,
                    ""unitValue"": {
                        ""currencySold"": """ & CurrencySign & """,
                        ""amountEGP"": " & Dec5(it("Price") * IIf(SalesMasterDT.Rows(0)("CurrencyId") = 1, 1, SalesMasterDT.Rows(0)("Exchange"))) & IIf(SalesMasterDT.Rows(0)("CurrencyId") = 1, "", ",
                        ""amountsold"": " & Dec5(it("Price")) & ",
                        ""currencyexchangerate"": " & Dec5(SalesMasterDT.Rows(0)("Exchange"))) & "
                    },
                    ""discount"": {
                        ""rate"": 0.00,
                        ""amount"": 0.00000
                    },
                    ""taxableItems"": [
                        
                  "
                For Each acc In SalesMasterTaxDT.Rows
                    invoiceLines &= "{
                            ""taxType"": """ & acc("taxType") & """,
                            ""amount"": " & Dec5(it("Value") * Math.Abs(acc("rate")) / 100) & ",
                            ""subType"": """ & acc("TaxSubType") & """,
                            ""rate"": " & Dec2(IIf(acc("DESCRIPTION").ToString.Contains("(قطعيه بمقدار ثابت)"), 0, 1) * Math.Abs(acc("rate"))) & "
                        },"
                Next
                invoiceLines = invoiceLines.Substring(0, invoiceLines.Length - 1)

                invoiceLines &= "  ]
                },"

            Next
            invoiceLines = invoiceLines.Substring(0, invoiceLines.Length - 1)

            '{""documents"": [
            Dim json As String = "{
            ""issuer"": {
                ""address"": {
                    ""branchID"": """ & TaxApi_issuerDT.Rows(0)("issuer_address_branchID") & """,
                    ""country"": """ & TaxApi_issuerDT.Rows(0)("issuer_address_country") & """,
                    ""governate"": """ & TaxApi_issuerDT.Rows(0)("issuer_address_governate") & """,
                    ""regionCity"": """ & TaxApi_issuerDT.Rows(0)("issuer_address_regionCity") & """,
                    ""street"": """ & TaxApi_issuerDT.Rows(0)("issuer_address_street") & """,
                    ""buildingNumber"": """ & TaxApi_issuerDT.Rows(0)("issuer_address_buildingNumber") & """,
                    ""postalCode"": """ & TaxApi_issuerDT.Rows(0)("issuer_address_postalCode") & """,
                    ""floor"": """ & TaxApi_issuerDT.Rows(0)("issuer_address_floor") & """,
                    ""room"": """ & TaxApi_issuerDT.Rows(0)("issuer_address_room") & """,
                    ""landmark"": """ & TaxApi_issuerDT.Rows(0)("issuer_address_landmark") & """,
                    ""additionalInformation"": """ & TaxApi_issuerDT.Rows(0)("issuer_address_additionalInformation") & """
                },
                ""type"": """ & TaxApi_issuerDT.Rows(0)("issuer_type") & """,
                ""id"": """ & TaxApi_issuerDT.Rows(0)("issuer_id") & """,
                ""name"": """ & TaxApi_issuerDT.Rows(0)("issuer_name") & """
            },
            ""receiver"": {
                ""address"": {
                    ""branchID"": """ & receiverDT.Rows(0)("receiver_address_branchID") & """,
                    ""country"": """ & receiverDT.Rows(0)("receiver_address_country") & """,
                    ""governate"": """ & receiverDT.Rows(0)("receiver_address_governate") & """,
                    ""regionCity"": """ & receiverDT.Rows(0)("receiver_address_regionCity") & """,
                    ""street"": """ & receiverDT.Rows(0)("receiver_address_street") & """,
                    ""buildingNumber"": """ & receiverDT.Rows(0)("receiver_address_buildingNumber") & """,
                    ""postalCode"": """ & receiverDT.Rows(0)("receiver_address_postalCode") & """,
                    ""floor"": """ & receiverDT.Rows(0)("receiver_address_floor") & """,
                    ""room"": """ & receiverDT.Rows(0)("receiver_address_room") & """,
                    ""landmark"": """ & receiverDT.Rows(0)("receiver_address_landmark") & """,
                    ""additionalInformation"": """ & receiverDT.Rows(0)("receiver_address_additionalInformation") & """
                },
                ""type"": """ & receiverDT.Rows(0)("receiver_type") & """,
                ""id"": """ & receiverDT.Rows(0)("receiver_id") & """,
                ""name"": """ & receiverDT.Rows(0)("receiver_name") & """
            },
            ""documentType"": """ & documentType & """,
            ""documentTypeVersion"": ""1.0"",
            ""dateTimeIssued"": """ & bm.ToStrDateTimeFormated(SalesMasterDT.Rows(0)("DayDate")).Replace(" ", "T") & "Z"",
            ""taxpayerActivityCode"": """ & TaxApi_issuerDT.Rows(0)("taxpayerActivityCode") & """,
            ""internalID"": """ & SalesMasterDT.Rows(0)("DocNo") & """,
            ""purchaseOrderReference"": """ & SalesMasterDT.Rows(0)("purchaseOrderReference") & """,
            ""purchaseOrderDescription"": """",
            ""salesOrderReference"": """",
            ""salesOrderDescription"": """",
            ""proformaInvoiceNumber"": """",
            ""payment"": {
                ""bankName"": """",
                ""bankAddress"": """",
                ""bankAccountNo"": """",
                ""bankAccountIBAN"": """",
                ""swiftCode"": """",
                ""terms"": """"
            },
            ""delivery"": {
                ""approach"": """",
                ""packaging"": """",
                ""dateValidity"": """ & bm.ToStrDateTimeFormated(SalesMasterDT.Rows(0)("DayDate")).Replace(" ", "T") & "Z"",
                ""exportPort"": """",
                ""grossWeight"": " & Dec5(Weight) & ",
                ""netWeight"": " & Dec5(Weight) & ",
                ""terms"": """"
            },
            ""invoiceLines"": [
            " & invoiceLines & "                 
            ],
            ""totalDiscountAmount"": " & Dec5(0 * SalesMasterDT.Rows(0)("DiscountValue") * IIf(SalesMasterDT.Rows(0)("CurrencyId") = 1, 1, SalesMasterDT.Rows(0)("Exchange"))) & ",
            ""totalSalesAmount"": " & Dec5(SalesMasterDT.Rows(0)("Total") * IIf(SalesMasterDT.Rows(0)("CurrencyId") = 1, 1, SalesMasterDT.Rows(0)("Exchange"))) & ",
            ""netAmount"": " & Dec5((SalesMasterDT.Rows(0)("Total")) * IIf(SalesMasterDT.Rows(0)("CurrencyId") = 1, 1, SalesMasterDT.Rows(0)("Exchange"))) & ",
            ""taxTotals"": [
                " & taxTotalsLines & "
            ],
            ""totalAmount"": " & Dec5((SalesMasterDT.Rows(0)("TotalAfterDiscount")) * IIf(SalesMasterDT.Rows(0)("CurrencyId") = 1, 1, SalesMasterDT.Rows(0)("Exchange"))) & ",
            ""extraDiscountAmount"": 0.00000 ,
            ""totalItemsDiscountAmount"": " & Dec5(0 * SalesMasterDT.Rows(0)("DiscountValue") * IIf(SalesMasterDT.Rows(0)("CurrencyId") = 1, 1, SalesMasterDT.Rows(0)("Exchange"))) & "           
        }"


            Dim serialize0 As String = Serialize(JsonConvert.DeserializeObject(json))
            Dim SignWithCMS0 As String = SignWithCMS(Encoding.UTF8.GetBytes(serialize0))

            Dim jsonDataBytes = Encoding.UTF8.GetBytes("{""documents"": [" & json.Substring(0, json.Length - 1) & ",""signatures"": [{""signatureType"": ""I"",""value"": """ & SignWithCMS0 & """}]}]}")

            Dim result = PostRequest(New Uri(apiBaseUrl & "/api/v1/documentsubmissions"), jsonDataBytes, "application/json", "POST")


            Dim result0 = JsonConvert.DeserializeObject(result)
            If result0("acceptedDocuments").Count > 0 Then
                'bm.ShowMSG(result0("acceptedDocuments")(0)("internalId").ToString)
                bm.ExecuteNonQuery("delete SalesMasterETA where StoreId=" & SalesMasterDT.Rows(0)("StoreId") & " and Flag=" & SalesMasterDT.Rows(0)("Flag") & " and InvoiceNo=" & SalesMasterDT.Rows(0)("InvoiceNo") & "    insert SalesMasterETA (StoreId,Flag,InvoiceNo,uuid,longId) select " & SalesMasterDT.Rows(0)("StoreId") & "," & SalesMasterDT.Rows(0)("Flag") & "," & SalesMasterDT.Rows(0)("InvoiceNo") & ",'" & result0("acceptedDocuments")(0)("uuid") & "','" & result0("acceptedDocuments")(0)("longId") & "'")
            End If
            If result0("rejectedDocuments").Count > 0 Then
                'bm.ShowMSG(result0("rejectedDocuments")(0)("internalId").ToString)
                Dim msg As String = ""
                For i As Integer = 0 To result0("rejectedDocuments")(0)("error")("details").Count - 1
                    msg &= result0("rejectedDocuments")(0)("error")("details")(i)("propertyPath") & " - " & result0("rejectedDocuments")(0)("error")("details")(i)("message") & vbCrLf
                Next
                bm.ExecuteNonQuery("delete SalesMasterETA where StoreId=" & SalesMasterDT.Rows(0)("StoreId") & " and Flag=" & SalesMasterDT.Rows(0)("Flag") & " and InvoiceNo=" & SalesMasterDT.Rows(0)("InvoiceNo") & "    insert SalesMasterETA (StoreId,Flag,InvoiceNo,Notes) select " & SalesMasterDT.Rows(0)("StoreId") & "," & SalesMasterDT.Rows(0)("Flag") & "," & SalesMasterDT.Rows(0)("InvoiceNo") & ",' " & msg & "'")
            End If
        Catch ex As Exception
            'If ex.Message <> "Method C_Login returned CKR_USER_ALREADY_LOGGED_IN" Then
            'End If
            bm.ShowMSG("فاتورة " & InvoiceNo & vbCrLf & ex.Message)
        End Try
    End Sub

    Function Dec2(Value As Decimal)
        Return String.Format("{0:0.00}", Value)
    End Function

    Function Dec5(Value As Decimal)
        Return String.Format("{0:0.00000}", Value)
    End Function


    '6. Cancel Document
    Public Sub CancelDocument(documentUUID As String, reason As String)
        Try
            Dim result = PutRequest("api/v1.0/documents/state/" & documentUUID & "/state", "{
	            status:""cancelled"",
	            reason:""" & reason & """
            }")
            Dim result0 = JsonConvert.DeserializeObject(result)
            If result0("error")("details") Is Nothing Then
            Else
                'For Each msg In result0("error")("details")
                'bm.ShowMSG(msg("message"))
                'Next
            End If
        Catch ex As Exception
            'bm.ShowMSG(ex.Message)
        End Try
    End Sub

    '7. Reject Document
    Public Sub RejectDocument(documentUUID As String, reason As String)
        Try
            Dim result = PutRequest("api/v1.0/documents/state/" & documentUUID & "/state", "{
	            status:""rejected"",
	            reason:""" & reason & """
            }")
            Dim result0 = JsonConvert.DeserializeObject(result)
            If result0("error")("details") Is Nothing Then

            Else
                For Each msg In result0("error")("details")
                    bm.ShowMSG(msg("message"))
                Next
            End If
        Catch ex As Exception
            'bm.ShowMSG(ex.Message)
        End Try
    End Sub

    '8. Get Recent Documents
    Public Documents
    Public Sub GetRecentDocuments()
        Try

            Dim result = GetRequest(apiBaseUrl & "/api/v1.0/documenttypes")
            Documents = JsonConvert.DeserializeObject(result)("result")
        Catch ex As Exception
            'bm.ShowMSG(ex.Message)
        End Try
    End Sub

    '10. Get Package Requests
    Public Requests
    Public Sub GetPackageRequests()
        Try

            Dim result = GetRequest(apiBaseUrl & "/api/v1/documentPackages/requests?pageSize=10&pageNo=1")
            Requests = JsonConvert.DeserializeObject(result)("result")
        Catch ex As Exception
            'bm.ShowMSG(ex.Message)
        End Try
    End Sub

    '11. Get Document Package
    Public DocumentPackage
    Public Sub GetDocumentPackage(DocumentPackageUniqueID As String)
        Try

            Dim result = GetRequest(apiBaseUrl & "/api/v1/documentPackages/" & DocumentPackageUniqueID)
            DocumentPackage = JsonConvert.DeserializeObject(result)("result")
        Catch ex As Exception
            'bm.ShowMSG(ex.Message)
        End Try
    End Sub

    '12. Get Document
    Public Document
    Public Sub GetDocument(DocumentUniqueID As String)
        Try

            Dim result = GetRequest(apiBaseUrl & "/api/v1/documents/" & DocumentUniqueID & "/raw")
            Document = JsonConvert.DeserializeObject(result) '("result")
        Catch ex As Exception
            'bm.ShowMSG(ex.Message)
        End Try
    End Sub

    '13. Get Submission
    Public Submission
    Public Sub GetSubmission(SubmissionUniqueID As String)
        Try

            Dim result = GetRequest(apiBaseUrl & "/api/v1.0/documentSubmissions/" & SubmissionUniqueID & "?PageSize=1")
            Submission = JsonConvert.DeserializeObject(result)("result")
        Catch ex As Exception
            'bm.ShowMSG(ex.Message)
        End Try
    End Sub

    '15. Get Document Printout (PDF)
    Public DocumentPDF
    Public Sub GetDocumentPrintout(DocumentUniqueID As String)
        Try
            Dim MyBath As String = bm.GetNewTempName(DocumentUniqueID & ".pdf")

            Dim MyWebClient As New WebClient()
            MyWebClient.Headers.Add(HttpRequestHeader.Authorization, "Bearer " & access_token)
            MyWebClient.DownloadFile(apiBaseUrl & "/api/v1/documents/" & DocumentUniqueID & "/pdf", MyBath)
            Process.Start(MyBath)
        Catch ex As Exception
            'bm.ShowMSG(ex.Message)
        End Try
    End Sub

    '18. Get Notifications
    Public Notifications
    Public Sub GetNotifications()
        Try
            Dim result = GetRequest(apiBaseUrl & "/api/v1/notifications/taxpayer?pageSize=10&pageNo=1&dateFrom=&dateTo=&type=&language=en&status=&channel=")
            Notifications = JsonConvert.DeserializeObject(result)("result")
        Catch ex As Exception
            'bm.ShowMSG(ex.Message)
        End Try
    End Sub

    '20. Get Document Details
    Public DocumentDetails
    Public Sub GetDocumentDetails(DocumentUniqueID As String)
        Try
            Dim result = GetRequest(apiBaseUrl & "/api/v1/documents/" & DocumentUniqueID & "/details")
            If result = "" Then Return
            DocumentDetails = JsonConvert.DeserializeObject(result)("status").ToString & IIf(JsonConvert.DeserializeObject(result)("documentStatusReason").ToString = "", "", " - Canceled - " & JsonConvert.DeserializeObject(result)("documentStatusReason").ToString)
            If DocumentDetails <> "Valid" AndAlso DocumentDetails <> "Submitted" Then
                For Each r In JsonConvert.DeserializeObject(result)("validationResults")("validationSteps")
                    If r("status") = "Invalid" Then
                        For Each r2 In r("error")("innerError")
                            DocumentDetails &= vbCrLf & r2("error").ToString
                        Next
                        bm.ExecuteNonQuery("update SalesMasterETA set NeedReSubmit=1 where uuid='" & DocumentUniqueID & "'")
                    End If
                Next
            End If
            bm.ExecuteNonQuery("update SalesMasterETA set DocumentDetails='" & DocumentDetails & "' where uuid='" & DocumentUniqueID & "'")
        Catch ex As Exception
            bm.ShowMSG(ex.Message)
        End Try
    End Sub

    '21. Decline Cancel Document
    Public Sub DeclineCancelDocument(DocumentUniqueID As String)
        Try
            Dim result = PutRequest("api/v1.0/documents/state/" & DocumentUniqueID & "/decline/cancelation", "")
            Dim result0 = JsonConvert.DeserializeObject(result)
            If result0("error")("details") Is Nothing Then

            Else
                For Each msg In result0("error")("details")
                    bm.ShowMSG(msg("message"))
                Next
            End If
        Catch ex As Exception
            'bm.ShowMSG(ex.Message)
        End Try
    End Sub

    '22. Decline Rejection Document
    Public Sub DeclineRejectionDocument(DocumentUniqueID As String)
        Try
            Dim result = PutRequest("api/v1.0/documents/state/" & DocumentUniqueID & "/decline/rejection", "")
            Dim result0 = JsonConvert.DeserializeObject(result)
            If result0("error")("details") Is Nothing Then

            Else
                For Each msg In result0("error")("details")
                    bm.ShowMSG(msg("message"))
                Next
            End If
        Catch ex As Exception
            'bm.ShowMSG(ex.Message)
        End Try
    End Sub



    '23. Create EGS Code Usage
    Public Sub CreateEGSCodeUsage(codeType As String, parentCode As String, itemCode As String, codeName As String, codeNameAr As String, activeFrom As String, activeTo As String, description As String, descriptionAr As String, requestReason As String)
        Try
            Dim outgoingQueryString As NameValueCollection = HttpUtility.ParseQueryString(String.Empty)

            outgoingQueryString.Add("""items""", " [{""codeType"": """ & codeType & """,""parentCode"": """ & parentCode & """,""itemCode"": """ & itemCode & """,""codeName"": """ & codeName & """,""codeNameAr"": """ & codeNameAr & """,""activeFrom"": """ & activeFrom & """,""activeTo"": """ & activeTo & """,""description"": """ & description & """,""descriptionAr"": """ & descriptionAr & """,""requestReason"": """ & requestReason & """}]")

            Dim jsonDataBytes = New ASCIIEncoding().GetBytes(outgoingQueryString.ToString())
            Dim result = PostRequest(New Uri(apiBaseUrl & "/api/v1.0/codetypes/requests/codes"), jsonDataBytes, "application/json", "POST")

            Dim result0 = JsonConvert.DeserializeObject(result)
            If result0("ERROR") Is Nothing Then

            Else
                'bm.ShowMSG(result0("ERROR")("details")("message"))
            End If
        Catch ex As Exception
            'bm.ShowMSG(ex.Message)
        End Try
    End Sub



    '24. Search my EGS code usage requests
    Public CodeTypes
    Public Sub SearchEGSCodeUsageRequests()
        Try
            Dim result = GetRequest(apiBaseUrl & "/api/v1.0/codetypes/requests/my?Active=true&Status=Approved&PageSize=10&PageNumber=1&OrderDirections=Descending")
            CodeTypes = JsonConvert.DeserializeObject(result)("result")
        Catch ex As Exception
            'bm.ShowMSG(ex.Message)
        End Try
    End Sub




    '25. Request Code Reuse
    Public Sub RequestCodeReuse(codeType As String, itemCode As String, comment As String)
        Try
            Dim result = PutRequest("api/v1.0/codetypes/requests/codeusages", "{items: [                                     {
                        codeType:""" & codeType & """,
                        itemCode:""" & itemCode & """,
                        comment:""" & comment & """
                         }
                    ]
                }")
            Dim result0 = JsonConvert.DeserializeObject(result)
            If result0("failedItems") Is Nothing Then

            Else
                For Each msg In result0("failedItems")
                    bm.ShowMSG(msg("errors").ToString)
                Next
            End If
        Catch ex As Exception
            'bm.ShowMSG(ex.Message)
        End Try
    End Sub

    '26. Search Published Codes
    Public Sub SearchPublishedCodes(CodeType As String, ParentLevelName As String)
        Try
            Dim result = GetRequest(apiBaseUrl & "/api/v1.0/codetypes/" & CodeType & "/codes?ParentLevelName=" & ParentLevelName & "&OnlyActive=true&ActiveFrom=2019-01-01T00:00:00Z&Ps=10&Pn=1&OrdDir=Descending&CodeTypeLevelNumber=5)")
            CodeDetails = JsonConvert.DeserializeObject(result)("result")
        Catch ex As Exception
            'bm.ShowMSG(ex.Message)
        End Try
    End Sub

    '27. Get Code Details By Item Code
    Public CodeDetails
    Public Sub GetCodeDetailsByItemCode(CodeType As String, ItemCode As String)
        Try
            Dim result = GetRequest(apiBaseUrl & "/api/v1.0/codetypes/" & CodeType & "/codes/" & ItemCode)
            CodeDetails = JsonConvert.DeserializeObject(result)("result")
        Catch ex As Exception
            'bm.ShowMSG(ex.Message)
        End Try
    End Sub

    '28. Update EGS Code Usage
    Public Sub GetCodeDetailsByItemCode(CodeUsageRequestId As String)
        Try
            Dim result = PutRequest("api/v1.0/codetypes/requests/codes/" & CodeUsageRequestId, "")
            Dim result0 = JsonConvert.DeserializeObject(result)
            If result0("ERROR") Is Nothing Then

            Else
                bm.ShowMSG(result0("ERROR")("details")("message"))
            End If
        Catch ex As Exception
            'bm.ShowMSG(ex.Message)
        End Try
    End Sub

    '29. Update Code
    Public Sub UpdateCode(CodeType As String, ItemCode As String)
        Try
            Dim result = PutRequest("api/v1.0/codetypes/" & CodeType & "/codes/" & ItemCode, "")
            Dim result0 = JsonConvert.DeserializeObject(result)
            If result0("ERROR") Is Nothing Then

            Else
                bm.ShowMSG(result0("ERROR")("details")("message"))
            End If
        Catch ex As Exception
            'bm.ShowMSG(ex.Message)
        End Try
    End Sub



#Region "Serialization"
    Function Serialize(request As JObject) As String
        Dim data As String = SerializeToken(request)
        Return data
    End Function
    Function SerializeToken(request As JToken) As String
        Dim serialized As String = ""
        If (request.Parent Is Nothing) Then
            SerializeToken(request.First)
        Else
            If (request.Type = JTokenType.Property) Then
                Dim name As String = CType(request, JProperty).Name.ToUpper()
                serialized &= """" & name & """"
                For Each prop In request
                    If (prop.Type = JTokenType.Object) Then
                        serialized &= SerializeToken(prop)
                    End If
                    If (prop.Type = JTokenType.Boolean OrElse prop.Type = JTokenType.Integer) Then
                        serialized &= """" & prop.Value(Of String)() & """"
                    End If
                    If (prop.Type = JTokenType.Float) Then
                        If (name = "RATE") Then
                            serialized &= """" & Dec2(prop.Value(Of String)()) & """"
                        Else
                            serialized &= """" & Dec5(prop.Value(Of String)()) & """"
                        End If
                    End If

                    If (prop.Type = JTokenType.Date) Then
                        serialized &= """" + bm.ToStrDateTimeFormated(prop.Value(Of Date)()).Replace(" ", "T") & "Z"""
                    End If
                    If (prop.Type = JTokenType.String) Then
                        serialized &= JsonConvert.ToString(prop.Value(Of String)())
                    End If
                    If (prop.Type = JTokenType.Array) Then
                        For Each item In prop.Children()
                            If (item.HasValues) Then
                                serialized &= """" & CType(request, JProperty).Name.ToUpper() & """"
                                serialized &= SerializeToken(item)
                            Else
                                serialized &= """" & (CType(request, JProperty)).Name.ToUpper() + """"
                                serialized &= """" & item.Value(Of String)() & """"
                            End If
                        Next
                    End If

                Next
            End If
        End If
        If (request.Type = JTokenType.Object) Then
            For Each prop In request.Children()
                If (prop.Type = JTokenType.Object OrElse prop.Type = JTokenType.Property) Then
                    serialized &= SerializeToken(prop)
                End If
            Next
        End If
        Return serialized
    End Function
    Public Function SignWithCMS(data As Byte()) As String
        Dim factories As New Pkcs11InteropFactories()
        Dim pkcs11Library As IPkcs11Library = factories.Pkcs11LibraryFactory.LoadPkcs11Library(factories, DllLibPath, AppType.MultiThreaded)

        Dim slot As ISlot = pkcs11Library.GetSlotList(SlotsType.WithTokenPresent).FirstOrDefault()

        If (slot Is Nothing) Then
            bm.ShowMSG("No slots found")
            Return "No slots found"
        End If

        Dim tokenInfo As ITokenInfo = slot.GetTokenInfo()

        Dim slotInfo As ISlotInfo = slot.GetSlotInfo()


        Dim session = slot.OpenSession(SessionType.ReadWrite)
        Try
            session.Login(CKU.CKU_USER, Encoding.UTF8.GetBytes(TokenPin))
        Catch ex As Exception
        End Try

        Dim certificateSearchAttributes = New List(Of IObjectAttribute)() From {
        session.Factories.ObjectAttributeFactory.Create(CKA.CKA_CLASS, CKO.CKO_CERTIFICATE),
        session.Factories.ObjectAttributeFactory.Create(CKA.CKA_TOKEN, True),
        session.Factories.ObjectAttributeFactory.Create(CKA.CKA_CERTIFICATE_TYPE, CKC.CKC_X_509)}


        Dim certificate As Net.Pkcs11Interop.HighLevelAPI.IObjectHandle = session.FindAllObjects(certificateSearchAttributes).FirstOrDefault()

        If (certificate Is Nothing) Then
            bm.ShowMSG("No slots found")
            Return "Certificate not found"
        End If

        Dim store As New X509Store(StoreName.My, StoreLocation.CurrentUser)
        store.Open(OpenFlags.MaxAllowed)

        'find cert by thumbprint
        Dim foundCerts = store.Certificates.Find(X509FindType.FindByIssuerName, "Egypt Trust Sealing CA", True)
        If Md.IsMyVeryFruits Then
            foundCerts = store.Certificates.Find(X509FindType.FindByIssuerName, "MCDR 2019", True)
        End If

        If (foundCerts.Count = 0) Then
            bm.ShowMSG("no device detected")
            Return "no device detected"
        End If

        Dim certForSigning = foundCerts(0)
        store.Close()


        Dim content As New ContentInfo(New Oid("1.2.840.113549.1.7.5"), data)


        Dim cms As New SignedCms(content, True)


        Dim bouncyCertificate As EssCertIDv2 = New EssCertIDv2(New Org.BouncyCastle.Asn1.X509.AlgorithmIdentifier(New DerObjectIdentifier("1.2.840.113549.1.9.16.2.47")), HashBytes(certForSigning.RawData))


        Dim signerCertificateV2 As New SigningCertificateV2(New EssCertIDv2() {bouncyCertificate})


        Dim signer As New CmsSigner(certForSigning)

        signer.DigestAlgorithm = New Oid("2.16.840.1.101.3.4.2.1")



        signer.SignedAttributes.Add(New Pkcs9SigningTime(DateTime.UtcNow))
        signer.SignedAttributes.Add(New AsnEncodedData(New Oid("1.2.840.113549.1.9.16.2.47"), signerCertificateV2.GetEncoded()))

        cms.ComputeSignature(signer)

        Dim output = cms.Encode()

        Return Convert.ToBase64String(output)


    End Function
    Function HashBytes(input As Byte()) As Byte()
        Dim sha As SHA256 = SHA256.Create()
        Dim output = sha.ComputeHash(input)
        Return output
    End Function
#End Region


End Class
