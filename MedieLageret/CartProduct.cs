using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedieLageret
{
    public class CartProduct
    {
        #region fields
        private int id;
        private string name;
        private decimal price;
        private int amount;
        private string image;
        #endregion

        #region properties
        public int Id { get { return this.id; } set { this.id = value; } }
        public string Name { get { return this.name; } set { this.name = value; } }
        public decimal Price { get { return this.price; } set { this.price = value; } }
        public int Amount { get { return this.amount; } set { this.amount = value; } }
        public decimal TotalPrice { get { return this.price * this.amount; } }
        public string Image { get { return this.image; } }
        public decimal Vat { get { return this.price * (decimal)0.20; } }
        #endregion

        #region constructors

        public CartProduct() { }
        public CartProduct(int id, string name, decimal price, int amount, string image)
        {
            this.id = id;
            this.name = name;
            this.price = price;
            this.amount = amount;
            this.image = image;
        }

        #endregion

        #region methods
        #endregion
    }
}
