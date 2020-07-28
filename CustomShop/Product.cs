using System;
using System.Collections.Generic;
using System.Text;

namespace CustomShop
{
    class Product
    {
        private int vendorCode;
        private string name;
        private int price;
        private string description;

        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }
        public int VendorCode
        {
            get
            {
                return vendorCode;
            }
            set
            {
                vendorCode = value;
            }
        }
        public int Price
        {
            get
            {
                return price;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                this.name = value;
            }
        }
        public Product(string Name, int Price)
        {
            this.Name = Name;
            this.price = Price;

            Random rnd = new Random();
            VendorCode = rnd.Next();
        }
    }
}
