using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MedieLageret
{
    public partial class Default : System.Web.UI.Page
    {

        private Database db = new Database();

        protected void Page_Load(object sender, EventArgs e)
        { 
            Repeater_categories.DataSource = db.GetAll("Categories");
            Repeater_categories.DataBind();
        }
    }
}