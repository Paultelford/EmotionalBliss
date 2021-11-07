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
using System.Globalization;

/// <summary>
/// Summary description for ShoppingCart
/// </summary>
[Serializable]
public class ShoppingCart
{
    public Hashtable _CartItems = new Hashtable();

    // Return all the items from the Shopping Cart
    public ICollection CartItems
    {
        get { return _CartItems.Values; }
    }

    public void emptyBasket()
    {
        _CartItems = new Hashtable();
    }


    // The sum total of the prices
    public decimal TotalEx
    {
        get
        {
            decimal sum = 0;
            foreach (CartItem item in _CartItems.Values)
            {
                //Returns total exc VAT
                decimal price = decimal.Parse(item.PriceIncDis.ToString("#.00"));
                sum += price * item.Quantity;
            }
            return sum;
        }
    }

    public decimal TotalInc
    {
        get
        {
            decimal sum = 0;
            foreach (CartItem item in _CartItems.Values)
            {
                //Returns total inc VAT
                decimal price = decimal.Parse(item.PriceIncDis.ToString("#.00")) * item.Quantity;
                decimal priceInc = price + (price * (item.Vat / 100));
                priceInc = decimal.Parse(priceInc.ToString("#.00"));
                sum += priceInc;
            }
            return sum;
        }
    }

    //Return product name
    public string getName(string ID)
    {
        CartItem item = (CartItem)_CartItems[ID];
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
        CartItem item = (CartItem)_CartItems[ID];
        if (item == null)
            _CartItems.Add(ID, new CartItem(ID, Name, Price, Discount, PriceIncDiscount, Vat));
        else
        {
            item.Quantity++;
            _CartItems[ID] = item;
        }
    }

    public void UpdateItem(int Qty, string ID)
    {
        CartItem item = (CartItem)_CartItems[ID];
        if (item == null)
            return;
        else
        {
            if (Qty == 0)
            {
                siteInclude.debug("Deleting item");
                RemoveItem(ID);
            }
            else
            {
                item.Quantity = Qty;
                _CartItems[ID] = item;
            }
        }
    }

    // Remove an item from the shopping cart
    public void RemoveItem(string ID)
    {
        CartItem item = (CartItem)_CartItems[ID];
        if (item == null)
            return;
        item.Quantity--;
        //if (item.Quantity == 0)
        _CartItems.Remove(ID);
        //else
        //_CartItems[ID] = item;
    }

}

[Serializable]
public class CartItem
{
    private string _ID;
    private string _Name;
    private decimal _Price;
    private decimal _Discount;
    private decimal _PriceIncDis;
    private int _Quantity = 1;
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

    public decimal PriceIncDis
    {
        get { return _PriceIncDis; }
    }

    public decimal Discount
    {
        get { return _Discount / 100; }
    }

    public decimal Vat
    {
        get { return _Vat; }
    }

    public decimal RowPriceEx
    {
        get
        {
            //Returns the rowTotal including VAT
            //decimal price = decimal.Parse(_PriceIncDis.ToString("#.00"));
            //decimal priceInc = price + (price * (_Vat / 100));
            //priceInc = decimal.Parse(priceInc.ToString("#.00"));
            //return priceInc * _Quantity;
            //Returns the rowTotal excluding VAT
            decimal price = decimal.Parse(_PriceIncDis.ToString("#.00"));
            return price * _Quantity;
        }
    }
    public decimal RowPriceInc
    {
        get
        {
            //Returns the rowTotal including VAT
            decimal price = decimal.Parse(_PriceIncDis.ToString("#.00")) * _Quantity;
            decimal priceInc = price + (price * (_Vat / 100));
            priceInc = decimal.Parse(priceInc.ToString("#.00"));
            return priceInc;
        }
    }

    public int Quantity
    {
        get { return _Quantity; }
        set { _Quantity = value; }
    }

    public CartItem(string ID, string Name, decimal Price, decimal Discount, decimal PriceIncDiscount, decimal Vat)
    {
        _ID = ID;
        _Name = Name;
        _Price = Price;
        _Discount = Discount;
        _PriceIncDis = PriceIncDiscount;
        _Vat = Vat;
    }
}

