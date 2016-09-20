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
        public bool interactionsAllowed
        {
            get
            {
                if (ViewState["interactionsAllowed"] == null)
                {
                    ViewState["interactionsAllowed"] = true;
                }
                return (bool)ViewState["interactionsAllowed"];
            }
            set
            {
                ViewState["interactionsAllowed"] = value;
            }
        }

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
            if (!interactionsAllowed)
            {
                return;
            }

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
            if (!interactionsAllowed)
            {
                return;
            }
            cart.removeAllProducts();
            Refresh();
        }

        public void Refresh()
        {
            cart = new Cart();
            Rpt_cartview.DataSource = cart.Items;
            Rpt_cartview.DataBind();

            //Removed buttons if interactions is disabled
            if (!interactionsAllowed)
            {
                Button empty = (Button)FindControl("Btn_emptyCart");
                empty.Visible = false;

                foreach (RepeaterItem product in Rpt_cartview.Items)
                {
                    LinkButton minus = (LinkButton)product.FindControl("Btn_minus");
                    LinkButton plus = (LinkButton)product.FindControl("Btn_plus");
                    Button delete = (Button)product.FindControl("Btn_delete");

                    minus.Visible = false;
                    plus.Visible = false;
                    delete.Visible = false;

                }
            }
        }
    }
}