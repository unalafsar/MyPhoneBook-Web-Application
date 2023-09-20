using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rehberim
{
    public partial class _AnaSayfa : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            UpdateTime();
        }

        protected void UpdateTime()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "UpdateTimeScript",
                "updateTime();", true);
        }

    }
}