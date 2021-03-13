using Denver.Common;
using Denver.Facade.Common;
using Denver.PCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Denver.NextUI
{
    public partial class DenverUI : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnPartsPage_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Parts.aspx");
        }

        protected void btnPersMngmt_Click(object sender, EventArgs e)
        {
            CommonFacade commonFacade = new CommonFacade();
            WebUser user=commonFacade.GetCurrentUser();
            Session[SessionKeys.CurrentWebUser] = user;
            Response.Redirect("~/PersonManagement.aspx?User"+user.FullName+"&token="+user.Token+"&registirationNumber="+user.RegistrationNumber);
        }
    }
}