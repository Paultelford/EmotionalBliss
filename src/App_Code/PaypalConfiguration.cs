using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace PaypalConfigurationClass.Models
{
    public static class PaypalConfiguration
    {
        //Variables for storing the clientID and clientSecret key  
        public readonly static string ClientId;
        public readonly static string ClientSecret;
        //Constructor  
        static PaypalConfiguration()
        {
            var config = GetConfig();
            ClientId = config["clientId"];
            ClientSecret = config["clientSecret"];
        }
        // getting properties from the web.config  
        public static Dictionary<string, string> GetConfig()
        {
            return PayPal.Api.ConfigManager.Instance.GetProperties();
        }
        private static string GetAccessToken()
        {
            // getting accesstocken from paypal  
            string accessToken = new OAuthTokenCredential(ClientId, ClientSecret, GetConfig()).GetAccessToken();
            return accessToken;
        }
        public static APIContext GetAPIContext()
        {
            // return apicontext object by invoking it with the accesstoken  
            APIContext apiContext = new APIContext(GetAccessToken());
            apiContext.Config = GetConfig();
            return apiContext;
        }

        private static Payment payment;
        private static Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            payment = new Payment()
            {
                id = paymentId
            };
            return payment.Execute(apiContext, paymentExecution);
        }
        public static Payment CreatePayment(APIContext apiContext, string redirectUrl, EBCart objEBCart,int CustomerOrderID)
        {
            //cartItem obj = (cartItem)objEBCart.Items;
            DataTable objDataTable = GetCustomerDetails(CustomerOrderID);
            decimal SubTotal = 0,Total=0;

            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            //Details details = new Details();
            
            //Adding Item Details like name, currency, price etc
            foreach (var item in objEBCart.Items)
            {
                cartItem obj = (cartItem)item;
                itemList.items.Add(new Item()
                {
                    name = obj.Name,
                    currency = "GBP",
                    price = obj.Price.ToString(),
                    quantity =obj.Qty.ToString(),
                    sku = "sku"
                });
                SubTotal = SubTotal + (obj.Price * obj.Qty);
            }
            var baseAddress = new BaseAddress()
            {
                //city = "New York",
                city= "London",
                postal_code = objDataTable.Rows[0]["billPostcode"].ToString(),
                line1 = objDataTable.Rows[0]["shipAdd1"].ToString(),
                line2 = objDataTable.Rows[0]["shipAdd2"].ToString() + " " + objDataTable.Rows[0]["shipAdd3"].ToString(),
                country_code = "GB",
                //state = "New York",
                state = "England",

            };
            var shippingAddress = new ShippingAddress()
            {
                city = baseAddress.city,
                postal_code = baseAddress.postal_code,
                line1 = baseAddress.line1,
                line2 = baseAddress.line2,
                country_code = baseAddress.country_code,
                recipient_name = objDataTable.Rows[0]["billName"].ToString(),
                phone = objDataTable.Rows[0]["phone"].ToString(),
            };
            itemList.shipping_address = shippingAddress;
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details
            decimal Tax =decimal.Round(((SubTotal * objEBCart.ShippingVatRate) / 100),2);
            var details = new Details()
            {
                tax =Tax.ToString(), //decimal.Round( ((SubTotal * objEBCart.ShippingVatRate) / 100),2).ToString(),
                shipping = objEBCart.Shipping.ToString(),
                subtotal = SubTotal.ToString()
            };
            Total = Total + Tax + objEBCart.Shipping + SubTotal;
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "GBP",
                total = Total.ToString(), // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            transactionList.Add(new Transaction()
            {

                description = "Transaction description",
                invoice_number = "your generated invoice number", //Generate an Invoice No  
                amount = amount,
                item_list = itemList
            });
            payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return payment.Create(apiContext);
        }
        public static DataTable GetCustomerDetails(int CustomerOrderID)
        {
            DataTable objDataTable = new DataTable();
            try
            {
                if (CustomerOrderID > 0)
                {
                    SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
                    SqlDataAdapter da = new SqlDataAdapter();
                    SqlCommand command = new SqlCommand("procShopCustomerByCustomerIDSelect", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.Int)).Value= CustomerOrderID;
                    connection.Open();
                    objDataTable = new DataTable();
                    da = new SqlDataAdapter(command);
                    da.Fill(objDataTable);
                    da.Dispose();
                    command.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
                return objDataTable;
            }
            catch (Exception)
            {
                return new DataTable();
            }
            

        }

        public static Payment PaymentWithCreditCard(APIContext apiContext, EBCart objEBCart, int CustomerOrderID)
        {
            Payment pymnt = new Payment();
            try
            {
                //create and item for which you are taking payment
                //if you need to add more items in the list
                //Then you will need to create multiple item objects or use some loop to instantiate object
                DataTable objDataTable = GetCustomerDetails(CustomerOrderID);
                decimal SubTotal = 0, Total = 0;
                List<Item> itms = new List<Item>();
                foreach (var CartData in objEBCart.Items)
                {
                    cartItem obj = (cartItem)CartData;
                    Item item = new Item();
                    item.name = obj.Name;
                    item.currency = objEBCart.ShopCountry == "us" ? "USD" : objEBCart.ShopCountry == "nl" ? "EU" : "GBP";
                    item.price = obj.Price.ToString();
                    item.quantity = obj.Qty.ToString();
                    item.sku = "sku";
                    //Now make a List of Item and add the above item to it
                    //you can create as many items as you want and add to this list
                    itms.Add(item);
                    SubTotal = SubTotal + (obj.Price * obj.Qty);
                }
                
                ItemList itemList = new ItemList();
                itemList.items = itms;

                //Address for the payment
                Address billingAddress = new Address();
                billingAddress.city = "NewYork";
                billingAddress.country_code = objEBCart.ShopCountry;///"GB";
                billingAddress.line1 = objDataTable.Rows[0]["shipAdd1"].ToString();
                billingAddress.line2 = objDataTable.Rows[0]["shipAdd2"].ToString() + " " + objDataTable.Rows[0]["shipAdd3"].ToString();
                billingAddress.postal_code = objDataTable.Rows[0]["billPostcode"].ToString();
                billingAddress.state = objEBCart.ShopCountry == "us" ? objDataTable.Rows[0]["shipAdd4"].ToString() : ""; //"NY";


                //Now Create an object of credit card and add above details to it
                //Please replace your credit card details over here which you got from paypal
                CreditCard crdtCard = new CreditCard();
                crdtCard.billing_address = billingAddress;
                crdtCard.cvv2 = objDataTable.Rows[0]["cardCv2"].ToString(); ;  //card cvv2 number
                crdtCard.expire_month = Convert.ToInt32(objDataTable.Rows[0]["cardExp"].ToString().Substring(0, 2).ToString()); //card expire date
                crdtCard.expire_year = Convert.ToInt32("20" + objDataTable.Rows[0]["cardExp"].ToString().Substring(2).ToString()); //card expire year
                crdtCard.first_name = objDataTable.Rows[0]["billName"].ToString();
                //crdtCard.last_name = "Thakur";
                crdtCard.number = objDataTable.Rows[0]["cardNo"].ToString(); //enter your credit card number here
                crdtCard.type = objDataTable.Rows[0]["cardType"].ToString().ToLower();  //credit card type here paypal allows 4 types

                // Specify details of your payment amount.
                decimal Tax = decimal.Round(((SubTotal * objEBCart.ShippingVatRate) / 100), 2);
                Details details = new Details();
                details.shipping = objEBCart.Shipping.ToString();
                details.subtotal = SubTotal.ToString();
                details.tax = Tax.ToString();

                // Specify your total payment amount and assign the details object
                Total = Total + Tax + objEBCart.Shipping + SubTotal;
                Amount amnt = new Amount();
                amnt.currency = objEBCart.ShopCountry == "us" ? "USD" : objEBCart.ShopCountry == "nl" ? "EU" : "GBP";
                // Total = shipping tax + subtotal.
                amnt.total = Total.ToString();
                amnt.details = details;

                // Now make a transaction object and assign the Amount object
                Transaction tran = new Transaction();
                tran.amount = amnt;
                tran.description = "Description about the payment amount.";
                tran.item_list = itemList;
                tran.invoice_number = "EB"+ CustomerOrderID;

                // Now, we have to make a list of transaction and add the transactions object
                // to this list. You can create one or more object as per your requirements

                List<Transaction> transactions = new List<Transaction>();
                transactions.Add(tran);

                // Now we need to specify the FundingInstrument of the Payer
                // for credit card payments, set the CreditCard which we made above

                FundingInstrument fundInstrument = new FundingInstrument();
                fundInstrument.credit_card = crdtCard;

                // The Payment creation API requires a list of FundingIntrument

                List<FundingInstrument> fundingInstrumentList = new List<FundingInstrument>();
                fundingInstrumentList.Add(fundInstrument);

                // Now create Payer object and assign the fundinginstrument list to the object
                Payer payr = new Payer();
                payr.funding_instruments = fundingInstrumentList;
                payr.payment_method = "credit_card";

                // finally create the payment object and assign the payer object & transaction list to it
                //Payment pymnt = new Payment();
                pymnt.intent = "sale";
                pymnt.payer = payr;
                pymnt.transactions = transactions;
                return pymnt.Create(apiContext);
            }
            catch (PayPal.PayPalException ex)
            {
                return null;
            }
            
        }

    }
    
}