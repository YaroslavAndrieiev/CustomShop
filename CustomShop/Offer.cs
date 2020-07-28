using System;
using System.Collections.Generic;
using System.Text;

namespace CustomShop
{
    enum OfferState
    {
        New,
        Canseled,
        Completed,
        Received,
        Paid,
        Sent
    }
    class Offer
    {
        private int offerNumber;
        private List<Product> productsList;
        private OfferState state;
        private User<string> user;

        public OfferState State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
            }
        }

        public List<Product> ProductsList
        {
            get
            {
                return productsList;
            }
        }

        public User<string> User
        {
            get
            {
                return user;
            }
        }

        public int OfferNumber
        {
            get
            {
                return offerNumber;
            }
        }

        public Offer(Product NewProduct,User<string> User)
        {
            productsList = new List<Product>();
            ProductsList.Add(NewProduct);

            Random rnd = new Random();
            offerNumber = rnd.Next();
            state = OfferState.New;
            this.user = User;
        }

        public void Add(Product NewProduct)
        {
            ProductsList.Add(NewProduct);
        }

        public int GetSum()
        {
            int ResSum = 0;
            foreach(Product item in ProductsList)
            {
                ResSum += item.Price;
            }
            return ResSum;
        }

    }
}
