using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;

/// <summary>
/// Summary description for ComponentCart
/// </summary>
/// 
namespace affiliateCart
{
    [Serializable]
    public class EBAffCart
    {
        private string _purchaseOrderNo = "";
        private string _currencyCode = "";
        private shipping ShippingCosts = new shipping(0, 0); //Default shiping charged to 0's
        public ListDictionary _Items = new ListDictionary();

        public ICollection Items
        {
            get { return _Items.Values; }
        }
        public string CurrencyCode
        {
            get { return _currencyCode; }
            set { _currencyCode = value; }
        }
        public void emptyBasket()
        {
            _Items = new ListDictionary();
        }
        // The sum total of the prices
        public decimal TotalEx
        {
            get
            {
                decimal sum = 0;
                foreach (cartItem item in _Items.Values)
                {
                    //Returns total exc VAT
                    decimal price = decimal.Parse(item.PriceIncDis.ToString("#.00"));
                    sum += price * item.Qty;
                }
                return sum;
            }
        }
        public string PurchaseOrderNo
        {
            set { _purchaseOrderNo = value; }
            get { return _purchaseOrderNo; }
        }
        public decimal Shipping
        {
            get { return ShippingCosts.ShippingCost; }
        }
        public decimal ShippingVatRate
        {
            get { return ShippingCosts.ShippingVatRate; }
        }
        public decimal ShippingTotal
        {
            get { return ShippingCosts.ShippingTotal; }
        }
        public decimal TotalInc
        {
            get
            {
                decimal sum = 0;
                foreach (cartItem item in _Items.Values)
                {
                    //Returns total inc VAT
                    //decimal price = decimal.Parse(item.PriceIncDis.ToString("#.00")) * item.Qty;
                    //decimal priceInc = price + (price * (item.Vat / 100));
                    //priceInc = decimal.Parse(priceInc.ToString("#.00"));
                    //sum += priceInc;
                    decimal price = decimal.Parse(item.PriceIncDis.ToString("#.00"));
                    decimal priceInc = price + (price * (item.Vat / 100));
                    priceInc = decimal.Parse(priceInc.ToString("#.00"));
                    sum += priceInc * item.Qty;
                }
                return sum;
            }
        }
        //Return product name
        public string getName(string ID)
        {
            cartItem item = (cartItem)_Items[ID];
            if (item != null)
            {
                return item.Name;
            }
            else
            {
                return "Does not exist";
            }
        }
        // Add a new item to the shopping cart
        public void AddItem(string ID, string Name, decimal Price, decimal Discount, decimal PriceIncDiscount, decimal Vat)
        {
            cartItem item = (cartItem)_Items[ID];
            if (item == null)
                _Items.Add(ID, new cartItem(ID, Name, Price, Discount, PriceIncDiscount, Vat));
            else
            {
                item.Qty++;
                _Items[ID] = item;
            }
        }
        public void setShipping(decimal cost, decimal vatRate)
        {
            ShippingCosts = new shipping(cost, vatRate);
        }
        public void UpdateItem(int Qty, string ID)
        {
            cartItem item = (cartItem)_Items[ID];
            if (item == null)
                return;
            else
            {
                item.Qty = Qty;
                _Items[ID] = item;
            }
        }

        // Remove an item from the shopping cart
        public void RemoveItem(string ID)
        {
            cartItem item = (cartItem)_Items[ID];
            if (item == null)
                return;
            //item.Qty--;
            //if (item.Quantity == 0)
            _Items.Remove(ID);
            //else
            //_Items[ID] = item;
        }

    }

    [Serializable]
    public class cartItem
    {
        private string _ID;
        private string _Name;
        private decimal _Price;
        private decimal _Discount;
        private decimal _PriceIncDis;
        private int _Qty = 1;
        private decimal _Vat;

        public string ID
        {
            get { return _ID; }
        }
        public string Name
        {
            get { return _Name; }
        }
        public decimal Price
        {
            get { return _Price; }
        }
        public decimal Discount
        {
            get { return _Discount; }
        }
        public decimal PriceIncDis
        {
            get { return _PriceIncDis; }
        }
        public int Qty
        {
            get { return _Qty; }
            set { _Qty = value; }
        }
        public decimal Vat
        {
            get { return _Vat; }
        }
        public decimal RowPriceEx
        {
            get
            {
                //Returns the rowTotal excluding VAT
                decimal price = decimal.Parse(_PriceIncDis.ToString("#.00"));
                return price * _Qty;
            }
        }
        public decimal RowPriceInc
        {
            get
            {
                //Returns the rowTotal including VAT
                decimal price = decimal.Parse(_PriceIncDis.ToString("#.00")) * _Qty;
                decimal priceInc = price + (price * (_Vat / 100));
                priceInc = decimal.Parse(priceInc.ToString("#.00"));
                return priceInc;
            }
        }
        public cartItem(string ID, string Name, decimal Price, decimal Discount, decimal PriceIncDis, decimal Vat)
        {
            _ID = ID;
            _Name = Name;
            _Price = Price;
            _Discount = Discount;
            _PriceIncDis = PriceIncDis;
            _Vat = Vat;
        }
    }
}
