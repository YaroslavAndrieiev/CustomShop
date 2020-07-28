using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomShop
{
    class DataBase<T>
    {
        private List<Offer> offersList;
        private List<User<string>> usersList;
        private List<Product> productsList;

        public DataBase()
        {
            usersList = new List<User<string>>();
            //Example user:
            User<string> u1 = new User<string>("User", "User");
            usersList.Add(u1);
            User<string> u2 = new User<string>("dima228", "User", "Dima", "Kulikov");
            usersList.Add(u2);
            User<string> u3 = new User<string>("anuta21", "User", "Anna","Amelina");
            usersList.Add(u3);
            //Example admin:
            User<string> a1 = new User<string>("Admin","Admin", Rights.Admin);
            usersList.Add(a1);
            //Example products:
            productsList = new List<Product>();
            Product p1 = new Product("Cannondale Topstone Sora 2020", 2999);
            productsList.Add(p1);
            Product p2 = new Product("Cyclone alx 29 2020", 799);
            productsList.Add(p2);
            productsList.Add(new Product("Gt Avalanche Sport 2019", 500));
            productsList.Add(new Product("Leon tn-60 2020", 1000));
            productsList.Add(new Product("Kellys Soot 30 2020", 899));
            //Example offers:
            offersList = new List<Offer>();
            Offer o1 = new Offer(p1,u1);
            o1.State = OfferState.Completed;
            o1.Add(p2);
            Offer o2 = new Offer(p2, u2);
            o2.State = OfferState.Completed;
            offersList.Add(o1);
            offersList.Add(o2);
        }

        public List<Offer> OffersList
        {
            get
            {
                return offersList;
            }
        }

        public List<User<string>> UsersList
        {
            get
            {
                return usersList;
            }
        }

        public List<Product> ProductsList
        {
            get
            {
                return productsList;
            }
        }

        public void AddUser(User<string> NewUser)
        {
            usersList.Add(NewUser);
        }

        public void AddOffer(Offer NewOffer)
        {
            offersList.Add(NewOffer);
        }

        public Product GetProductByCode(int Code)
        {
            foreach (Product item in ProductsList)
            {
                if(item.VendorCode.Equals(Code))
                {
                    return item;
                }
            }
            return null;
        }

        public Product GetProductByNumber(int Number) //0 based
        {
            for(int i = 0; i < ProductsList.Count; i++)
            {
                if (i == Number)
                {
                    return ProductsList.ElementAt(i);
                }
            }
            return null;
        }
    }
}
