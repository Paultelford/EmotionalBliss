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
/// Summary description for ComponentCart
/// </summary>
[Serializable]
public class ComponentCart
{
    public Hashtable _CartItems = new Hashtable();

	public ComponentCart()
	{
		
	}

    public ICollection CartItems
    {
        get { return _CartItems.Values; }
    }

    public void emptyBasket()
    {
        _CartItems = new Hashtable();
    }

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
    public void AddItem(int ID, string Name, decimal Cost, int Qty)
    {
        CartItem item = (CartItem)_CartItems[ID];
        if (item == null)
            _CartItems.Add(ID, new CartItem(ID, Name, Cost, Qty));
        else
        {
            item.Qty++;
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
            item.Qty = Qty;
            _CartItems[ID] = item;
        }
    }

    // Remove an item from the shopping cart
    public void RemoveItem(string ID)
    {
        CartItem item = (CartItem)_CartItems[ID];
        if (item == null)
            return;
        item.Qty--;
        //if (item.Quantity == 0)
        _CartItems.Remove(ID);
        //else
        //_CartItems[ID] = item;
    }

    [Serializable]
    public class CartItem
    {
        private int _ID;
        private string _Name;
        private decimal _Cost;
        private int _Qty;

        public string Name
        {
            get { return _Name; }
        }

        public decimal Cost
        {
            get { return _Cost; }
        }

        public int Qty
        {
            get { return _Qty; }
            set { _Qty = value; }
        }

        public CartItem(int ID, string Name, decimal Cost, int Qty)
        {
            _ID = ID;
            _Name = Name;
            _Cost = Cost;
            _Qty = Qty;
        }
    }
}
