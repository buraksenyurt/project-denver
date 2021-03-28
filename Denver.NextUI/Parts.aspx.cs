using Denver.Facade.Parts;
using Denver.PCL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Denver.NextUI
{
    public partial class Parts : System.Web.UI.Page
    {
        PartManagerFacade partManagerFacade = new PartManagerFacade();

        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet allParts = partManagerFacade.GetParts();
            grdPartList.DataSource = allParts;
            grdPartList.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            var result = partManagerFacade.AddToStock(Convert.ToInt32(txtCode.Text), Convert.ToInt32(txtNumber.Text), Convert.ToDecimal(txtPrice.Text), 0, txtName.Text, Convert.ToInt32(txtQuantity.Text), drpSupplier.SelectedValue, txtDescription.Text);
            if (result == Common.RetCode.Success)
            {
                lblSummary.Text = "Parça eklendi";
            }
            else
            {
                lblSummary.Text = "Parça eklenmesi işleminde hata oluştu";
            }
        }
    }
}