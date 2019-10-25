using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class asp_DealerPrograms : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
      MyIFrame.Attributes["src"] = "http://reports.pompstire.com/Dealers/gbweb01/index.html";

    }
}