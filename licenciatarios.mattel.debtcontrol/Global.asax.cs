using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace licenciatarios.mattel.debtcontrol
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
          Application["SmtpServer"] = "mail.debtcontrol.cl";
          Application["EmailSender"] = "licenciatariosmattel@debtcontrol.cl";
          Application["NameSender"] = string.Empty;
          Application["PortSmtpServer"] = string.Empty;
          Application["UserSmtp"] = "dc\\licenciatariosmattel";
          Application["PwdSmtp"] = "00 Mtt 11";
          Application["setAccount"] = string.Empty;
          Application["setDomainName"] = string.Empty;
          Application["trackPageview"] = string.Empty;
          Application["cUrlSite"] = "http://licenciatariosmattel.debtcontrol.cl/";
          //Application["cUrlSite"] = "http://localhost:60627/";
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Session["USUARIO"] = string.Empty;
            Session["Administrador"] = string.Empty;
        }
    }
}