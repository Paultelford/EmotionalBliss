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
/// 

[Serializable]
public class EBCart
{
    private voucher Voucher = new voucher("", false, 0, false); //Default voucher to nothing(no voucher)
    private shipping ShippingCosts = new shipping(0, 0); //Default shiping charged to 0's
    private string _currencyCode = "";
    private string _currencySign = "";
    private string _purchaseOrderNo = "";
    private string _shopCountry = "";
    private decimal _CouponVat = 0;
    private bool _isPercent = false;
    public Hashtable _Items = new Hashtable();

    public ICollection Items
    {
        get { return _Items.Values; }
    }
    public void emptyBasket()
    {
        _Items = new Hashtable();
        Voucher = new voucher("", false, 0, false);
        ShippingCosts = new shipping(0, 0);
        _CouponVat = 0;
    }

    
    public bool VoucherIsPercent {
        get { return Voucher.VoucherIsPercent; }
    }

    public string ShopCountry
    {
        get { return _shopCountry; }
        set { _shopCountry = value; }
    }
    public string CurrencyCode
    {
        get { return _currencyCode; }
        set { _currencyCode = value; }
    }
    public string CurrencySign
    {
        get { return _currencySign; }
        set { _currencySign = value; }
    }
    public string PurchaseOrderNo
    {
        set { _purchaseOrderNo = value; }
        get { return _purchaseOrderNo; }
    }
    public void removeVoucher()
    {
        Voucher = new voucher("", false, 0, false);
    }
    public string VoucherNumber
    {
        get { return Voucher.VoucherNumber; }
    }
    public bool VoucherIsCoupon
    {
        get { return Voucher.VoucherIsCoupon; }
    }
    public decimal VoucherCredit
    {
        get { return -Voucher.VoucherCredit; }
    }
    public decimal Shipping
    {
        get { return ShippingCosts.ShippingCost; }
    }
    public decimal ShippingVatRate
    {
        get { return ShippingCosts.ShippingVatRate; }
    }
    public decimal ShippingVat 
    {
        get { return ShippingTotal - Shipping; }
    }
    public decimal ShippingTotal
    {
        get { return ShippingCosts.ShippingTotal; }
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
    public decimal TotalInc
    {
        get
        {
            decimal discountVat = 0;
            decimal sum = 0;
            foreach (cartItem item in _Items.Values)
            {
                //Returns total inc VAT
                decimal price = decimal.Parse(item.PriceIncDis.ToString("#.00"));
                decimal priceInc = price + (price * (item.Vat / 100));
                priceInc = decimal.Parse(priceInc.ToString("#.00"));
                sum += priceInc * item.Qty;
            }
            if (Voucher.VoucherNumber != "")
            {
                if (Voucher.VoucherIsCoupon)
                {
                    //Distributor created coupon, remove voucher amount before vat is applied
                    //Step 1) Make sure the voucher discount is not greater than the sum of the VAT Items
                    decimal vatItems = 0;
                    decimal nonVatItems = 0;
                    decimal vatRate = 0;
                    decimal vat = 0;
                    decimal voucherCredit = 0;
                    foreach (cartItem item in _Items.Values)
                    {
                        if (item.Vat > 0)
                        {
                            vatItems += item.PriceIncDis * item.Qty;
                            vatRate = item.Vat;
                        }
                        else
                        {
                            nonVatItems += item.PriceIncDis * item.Qty;
                        }
                    }

                    if (Voucher.VoucherIsPercent)
                    {
                        voucherCredit = ((vatItems + nonVatItems) / 100) * Voucher.VoucherCredit;
                    }
                    else
                    {
                        voucherCredit = Voucher.VoucherCredit;
                    }

                    //Step 2) Calculate new vat
                    vat = vatRate > 0 ? ((vatItems - voucherCredit) / 100) * vatRate : 0;
                    vat = decimal.Parse(vat.ToString("#.00"));
                    _CouponVat = vat;
                    //Step 3) Calculate new order totals
                    sum = (vatItems + nonVatItems + vat) - voucherCredit;

                }
                else
                {
                    if (Voucher.VoucherIsPercent)
                    {
                        var amount = (sum / 100) * Voucher.VoucherCredit;
                        sum -= amount;
                    }
                    else
                    {
                        //Customer bought voucher, just deduct the voucher amount from the basket total (inc vat) amount
                        sum -= Voucher.VoucherCredit;
                    }
                }
            }
            sum += ShippingCosts.ShippingTotal;
            return decimal.Parse(sum.ToString("#.00"));
        }
    }
    public decimal GoodsIncVat
    {
        get
        {
            decimal tot = 0;
            decimal unitPriceIncVat = 0;
            foreach (cartItem item in _Items.Values)
            {
                unitPriceIncVat = (item.PriceIncDis + ((item.PriceIncDis / 100) * item.Vat));
                //unitPriceIncVat = decimal.Parse(unitPriceIncVat.ToString("#.00"));
                tot += unitPriceIncVat * item.Qty;
            }
            return decimal.Parse(tot.ToString("#.00"));
        }
    }
    public decimal GoodsVat
    {
        get
        {
            //Must take coupons into consideration.  
            decimal vat = 0;
            decimal vatTotal = 0;
            decimal voucherDiscountRemaining = Voucher.VoucherCredit;
            foreach (cartItem item in _Items.Values)
                if (item.Vat > 0)
                {
                    //vat = ((item.PriceIncDis / 100) * item.Vat) * item.Qty;
                    if (Voucher.VoucherIsCoupon)
                    {
                        //Calculate the vat, remembering to remove the discount beforehand (remove voucherCredit from each item in basket until there is no voucherCredit remaining)
                        for (int i = 0; i < item.Qty; i++)
                        {
                            if (voucherDiscountRemaining > 0)
                            {
                                if (voucherDiscountRemaining >= item.PriceIncDis)
                                    voucherDiscountRemaining -= item.PriceIncDis;
                                else
                                {
                                    vat += ((item.PriceIncDis - voucherDiscountRemaining) / 100) * item.Vat;
                                    vat = decimal.Parse(vat.ToString("#.00"));
                                    voucherDiscountRemaining = 0;
                                }
                            }
                            else
                            {
                                //All discount has gone, calculate items as normal
                                vat += (item.PriceIncDis / 100) * item.Vat;
                                vat = decimal.Parse(vat.ToString("#.00"));
                            }
                        }
                    }else{
                        vat = ((item.PriceIncDis / 100) * item.Vat);
                        vat = decimal.Parse(vat.ToString("#.00")) * item.Qty;
                    }
                    vatTotal += vat;
                }
            //vatTotal = decimal.Parse(vatTotal.ToString("#.00"));
            return decimal.Parse(vatTotal.ToString("#.00"));
        }
    }
    public decimal CouponVat
    {
        get { return _CouponVat; }
    }
    public int TotalWeight
    {
        get
        {
            int tWeight = 0;
            foreach (cartItem item in _Items.Values)
                tWeight += item.Weight * item.Qty;
            return tWeight;
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
    public void AddItem(string ID, string Name, decimal Price, decimal Discount, decimal PriceIncDiscount, decimal Vat, int DistBuyingID, int Weight, string ProductCode)
    {
        AddItem(ID, Name, Price, Discount, PriceIncDiscount, Vat, DistBuyingID, Weight, "", ProductCode);
    }
    public void AddItem(string ID, string Name, decimal Price, decimal Discount, decimal PriceIncDiscount, decimal Vat, int DistBuyingID, int Weight, string VoucherNumber, string ProductCode)
    {
        cartItem item = (cartItem)_Items[ID];
        if (item == null)
            _Items.Add(ID, new cartItem(ID, Name, Price, Discount, PriceIncDiscount, Vat, DistBuyingID, Weight, VoucherNumber, ProductCode));
        else
        {
            item.Qty++;
            _Items[ID] = item;
        }
    }
    // Add voucher
    public void AddVoucher(string Number, bool IsCoupon, decimal Credit, bool isPercent)
    {
        Voucher = new voucher(Number, IsCoupon, Credit, isPercent);
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
            if (Qty == 0)
            {
                RemoveItem(ID);
            }
            else
            {
                item.Qty = Qty;
                _Items[ID] = item;
            }
        }
    }
    public void UpdateBasketImage(string ID, string imageName)
    {
        cartItem item = (cartItem)_Items[ID];
        if (item == null)
            return;
        else
        {
            item.BasketImageName = imageName;
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
    private string _BasketImageName="";
    private decimal _Price;
    private decimal _Discount;
    private decimal _PriceIncDis;
    private int _Qty = 1;
    private decimal _Vat;
    private int _DistBuyingID;
    private string _VoucherNumber;
    private int _Weight;
    private string _ProductCode;

    public string ID
    {
        get { return _ID; }
    }
    public string Name
    {
        get { return _Name; }
    }
    public string BasketImageName
    {
        get { return _BasketImageName; }
        set { _BasketImageName = value; }
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
    public int Weight
    {
        get { return _Weight; }
    }
    public string ProductCode
    {
        get { return _ProductCode; }
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
            decimal price = decimal.Parse(_PriceIncDis.ToString("#.00"));
            decimal priceInc = price + (price * (_Vat / 100));
            //priceInc = decimal.Parse(priceInc.ToString("#.00"));
            priceInc = priceInc * _Qty;
            return decimal.Parse(priceInc.ToString("#.00"));
        }
    }
    public decimal itemPriceInc
    {
        get
        {
            //Returns the unit price including vat
            decimal price = decimal.Parse(_PriceIncDis.ToString("#.00"));
            decimal priceInc = price + (price * (_Vat / 100));
            return decimal.Parse(priceInc.ToString("#.00"));
        }
    }
    public int RowWeight
    {
        get { return _Weight * _Qty; }
    }
    public string VoucherNumber
    {
        get { return _VoucherNumber; }
    }
    public int DistBuyingID
    {
        get { return _DistBuyingID; }
    }
    public cartItem(string ID, string Name, decimal Price, decimal Discount, decimal PriceIncDis, decimal Vat, int DistBuyingID, int Weight, string VoucherNumber, string ProductCode)
    {
        _ID = ID;
        _Name = Name;
        _Price = Price;
        _Discount = Discount;
        _PriceIncDis = PriceIncDis;
        _Vat = Vat;
        _DistBuyingID = DistBuyingID;
        _VoucherNumber = VoucherNumber;
        _Weight = Weight;
        _ProductCode = ProductCode;
    }
}
[Serializable]
public class voucher
{
    private string _VoucherNumber;
    private bool _VoucherIsCoupon;
    private decimal _VoucherCredit;
    private bool _VoucherIsPercent;
    public string VoucherNumber
    {
        get { return _VoucherNumber; }
    }
    public bool VoucherIsCoupon
    {
        get { return _VoucherIsCoupon; }
    }
    public bool VoucherIsPercent {
        get { return _VoucherIsPercent; }
    }

    public decimal VoucherCredit
    {
        get { return _VoucherCredit; }
    }
    public voucher(string VoucherNumber, bool VoucherIsCoupon, decimal VoucherCredit, bool VoucherIsPercent)
    {
        _VoucherNumber = VoucherNumber;
        _VoucherIsCoupon = VoucherIsCoupon;
        _VoucherCredit = VoucherCredit;
        _VoucherIsPercent = VoucherIsPercent;
    }
}
[Serializable]
public class shipping
{
    private decimal _shippingCost;
    private decimal _shippingVatRate;
    private decimal _shippingTotal;
    public decimal ShippingCost
    {
        get { return _shippingCost; }
    }
    public decimal ShippingVatRate
    {
        get { return _shippingVatRate; }
    }
    public decimal ShippingTotal
    {
        get { return _shippingTotal; }
    }
    public shipping(decimal cost, decimal vatRate)
    {
        _shippingCost = cost;
        _shippingVatRate = vatRate;
        _shippingTotal = cost * (1 + (vatRate / 100));
    }
}
