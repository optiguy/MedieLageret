using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MedieLageret
{
    public partial class CartView : System.Web.UI.UserControl
    {
        private Cart cart;

        protected void Page_Load(object sender, EventArgs e)
        {
            cart = new Cart();
            if (!IsPostBack)
            {
                Refresh();
            }
        }

        protected void Rpt_cartview_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "plus")
            {
                cart.addAmountOnProduct(id, 1);
            }
            else if (e.CommandName == "minus")
            {
                cart.reduceAmountOnProduct(id, 1);
            }
            else if (e.CommandName == "delete")
            {
                cart.removeProduct(id);
            }
            Refresh();
        }

        protected void Btn_emptyCart_Click(object sender, EventArgs e)
        {
            cart.removeAllProducts();
            Refresh();
        }

        public void Refresh()
        {
            cart = new Cart();
            Rpt_cartview.DataSource = cart.Items;
            Rpt_cartview.DataBind();
        }
    }
}