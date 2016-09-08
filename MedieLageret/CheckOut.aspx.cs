using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MedieLageret
{
    public partial class CheckOut : System.Web.UI.Page
    {
        Order order;
        Cart cart;
        protected void Page_Load(object sender, EventArgs e)
        {
            cart = new Cart();
        }

        protected void LinkButton_accept_and_buy_Click(object sender, EventArgs e)
        {
            Literal_message.Text = "";
            if (!CheckBox_terms.Checked)
            {
                SetAlertMessage("Du skal accepterer vore betingelser for at afgive din ordre.", "alert-info");
                return;
            }

            if (cart.Items.Count == 0)
            {
                SetAlertMessage("Du har ingen produkter i kurven.", "alert-warning");
                return;
            }

            //Gem ordre
            order = new Order(1);
            {
                order.Products = cart.Items;
                if (order.saveOrder())
                {
                    cart.Items.Clear();
                    SetAlertMessage("Din ordre er nu afsendt!", "alert-success");
                }
                else
                {
                    SetAlertMessage("<strong>Fejl</strong> Din ordre blev ikke gemt. Prøv igen eller kontakt os venligst.", "alert-danger");
                }
            }

        }

        private void SetAlertMessage(string message, string extraClass = "")
        {
            Literal_message.Text = string.Format("<div class='alert " + extraClass + "' role='alert'>{0}</div>", message);
        }
    }
}