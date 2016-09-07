using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MedieLageret
{
    public partial class Product : System.Web.UI.Page
    {
        private int _productId = 0;
        private Database db = new Database();
        private Cart cart;
       
        protected void Page_Load  (object sender, EventArgs e)
        {
            cart = new Cart();

            //Check if category id is a valid integer.
            if (!int.TryParse(Request.QueryString["produkt"], out this._productId))
            {
                throw new HttpException(404, "Produktet id er ikke gyldigt!");
            }

            DataTable product = db.GetMany("Products", "id", _productId);

            if(product.Rows.Count == 0)
            {
                throw new HttpException(404, "Produktet findes ikke!");
            }

            FormView_product.DataSource = product;
            FormView_product.DataBind();
        }

        protected void LinkButton_buy_Command(object s, CommandEventArgs e)
        {
            int produktId = 0;
            if (!int.TryParse(e.CommandArgument.ToString(), out produktId))
            {
                throw new HttpException(404, "Produkt id er ikke gyldigt!");
            }

            DataRow product = db.GetOne("Products", "id", produktId);

            cart.AddToCart(
                produktId,
                product["name"].ToString(),
                (decimal)product["price"],
                1,
                product["image"].ToString()
            );

            CartView cartView = (CartView)Master.FindControl("CartView");
            cartView.Refresh();

        }
    }
}