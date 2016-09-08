using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedieLageret
{
    public class Order
    {
        private int orderId;
        private int userId;
        private DateTime createdDate;
        private List<CartProduct> products;
        private Database db = new Database();

        public int OrderId { get { return this.orderId; } }
        public int UserId { get { return this.userId; } set { this.userId = value; } }
        public DateTime CreatedDate { get { return this.createdDate; } set { this.createdDate = value; } }
        public List<CartProduct> Products { get { return this.products; } set { this.products = value; } }

        public decimal TotalValue
        {
            get
            {
                decimal total = 0;
                foreach(CartProduct product in this.products)
                {
                    total += product.Price * product.Amount;
                }
                return total;
            }
        }

        public Order(int userId)
        {
            this.userId = userId;
            this.createdDate = DateTime.Now;
        }

        public void addProduct(CartProduct item)
        {
            this.products.Add(item);
        }

        public bool saveOrder()
        {
            try
            {
                int orderId = db.Insert("Orders", new Dictionary<string, object>{
                    {"date_created", this.createdDate},
                    {"user_id", this.userId},
                    {"total_value", this.TotalValue}
                });
                try
                {
                    foreach (CartProduct product in this.products)
                    {
                        db.Insert("Order_items", new Dictionary<string, object> {
                            {"order_id", orderId}, 
                            {"product_id", product.Id},
                            {"price", product.Price},
                            {"amount", product.Amount}
                        });
                    }
                }
                catch { return false; }
                return true; //Success
            }
            catch { return false; }
        }
    }
}