using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MedieLageret.Layout
{
    public partial class Category : System.Web.UI.Page
    {
        private int _categoryId = 0;
        private Database db = new Database();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            //Get category id from querystring
            int.TryParse(Request.QueryString["kategori"], out _categoryId);
            //Check if category id is a valid integer.
            if (_categoryId == 0)
            {
                throw new HttpException(404, "Kategorien id er ikke gyldigt!");
            }

            //Get sort from querystring
            string sort = "priceAsc";
            if (Request.QueryString["sort"] != null)
            {
                sort = Request.QueryString["sort"].ToString();
            }

            //Select the right items in dropdownlists
            DropDownList_category.DataBind(); //Databind the data from the database
            DropDownList_category.SelectedValue = _categoryId.ToString();
            DropDownList_sorting.SelectedValue = sort;

            SetSorting(sort); //Show products on screen

        }

        protected void Linkbutton_Filter_Click(object sender, EventArgs e)
        {
            Panel_filters.Visible = (Panel_filters.Visible == false) ? true : false;
        }

        protected void Button_search_Click(object sender, EventArgs e)
        {
            MakeUrl();
        }

        private void MakeUrl()
        {
            //Add Dropdownlist values to search url
            int.TryParse(DropDownList_category.SelectedValue.ToString(), out _categoryId);
            string sort = DropDownList_sorting.SelectedValue;
            string url = string.Format("~/Category.aspx?kategori={0}&sort={1}", _categoryId, sort);

            //Add Text search values to search url
            if (Textbox_search.Text != "")
            {
                url = string.Format(url + "&q={0}", Textbox_search.Text);
            }

            //Add min price values to search url
            if (Textbox_minPris.Text != "")
            {
                url = string.Format(url + "&minPris={0}", Textbox_minPris.Text);
            }
            //Add max price values to search url
            if (Textbox_maxPris.Text != "")
            {
                url = string.Format(url + "&maxPris={0}", Textbox_maxPris.Text);
            }

            //Add min year values to search url
            if (Textbox_minYear.Text != "")
            {
                url = string.Format(url + "&minYear={0}", Textbox_minYear.Text);
            }
            //Add max year values to search url
            if (Textbox_maxYear.Text != "")
            {
                url = string.Format(url + "&maxYear={0}", Textbox_maxYear.Text);
            }

            //Add genres values to search url
            List<int> selectedIndexes = GetCheckedCheckboxesInList(CheckBoxList_genre);
            if (selectedIndexes.Count != 0)
            {
                url = string.Format(url + "&genres={0}", string.Join("~", selectedIndexes));
            }

            Response.Redirect(url);
        }

        private List<int> GetCheckedCheckboxesInList(CheckBoxList list)
        {
            //Get all checked genres in checkboxlist
            List<int> selectedIndexes = new List<int>();
            foreach (ListItem genre in list.Items)
            {
                if (genre.Selected)
                {
                    int genreId = 0;
                    int.TryParse(genre.Value, out genreId);
                    selectedIndexes.Add(genreId);
                }
            }
            return selectedIndexes;
        }

        private void SetSorting(string sort = "priceAsc")
        {
            //Get products with the selected sorting
            switch (sort)
            {
                default:
                    GetProducts();
                    break;
                case "priceDesc":
                    GetProducts("price", false);
                    break;
                case "titleAsc":
                    GetProducts("name");
                    break;
                case "titleDesc":
                    GetProducts("name", false);
                    break;
            }
        }

        private void GetProducts(string field = "price", bool ascending = true)
        {
            Dictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("id", this._categoryId);

            string order = (ascending) ? "ASC" : "DESC";

            string sql = @"SELECT p.id, p.name, p.stock, p.[image],p.price FROM Products AS p
                        INNER JOIN Product_has_categories AS pc
                        ON p.id = pc.product_id 
                        LEFT JOIN Product_has_genres AS pg
                        ON p.id = pg.product_id
                        WHERE pc.category_id = @id";

            //Add search filter
            if (Request.QueryString["q"] != null && Request.QueryString["q"] != "")
            {
                sql += " AND (name LIKE '%' + @search + '%' OR description LIKE '%' + @search + '%')";
                parameter.Add("search", Request.QueryString["q"]);
            }

            //Add price filter
            int minPris = 0;
            int maxPris = 0;
            int.TryParse(Request.QueryString["minPris"], out minPris);
            int.TryParse(Request.QueryString["maxPris"], out maxPris);
            if (maxPris == 0)
            {
                maxPris = 9999999;
            }
            sql += string.Format(" AND price BETWEEN {0} AND {1}", minPris, maxPris);

            //Add year filter
            int minYear = 0;
            int maxYear = 0;
            int.TryParse(Request.QueryString["minYear"], out minYear);
            int.TryParse(Request.QueryString["maxYear"], out maxYear);
            if (maxYear == 0)
            {
                maxYear = 9999999;
            }
            sql += string.Format(" AND year BETWEEN {0} AND {1}", minYear, maxYear);

            //Add genre filter
            if (Request.QueryString["genres"] != null && Request.QueryString["genres"] != "")
            {
                string test = Request.QueryString["genres"];
                string genreIds = test.Replace("~", ",");
                sql += string.Format(" AND pg.genre_id IN({0})", genreIds);
            }
 
            //Add sorting
            sql += " GROUP BY p.id, p.name, p.price, p.image, p.stock ORDER BY " + field + " " + order;

            DataTable products = db.Select(sql, parameter);

            if (products.Rows.Count == 0)
            {
                Label_noProducts.Text = "Din søgning gav intet resultat!";
            }

            Repeater_Products.DataSource = products;
            Repeater_Products.DataBind();
        }

    }
}