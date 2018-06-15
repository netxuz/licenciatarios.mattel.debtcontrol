using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using DebtControl.Method;

namespace licenciatarios.mattel.debtcontrol
{
  public partial class testmail : System.Web.UI.Page
  {
    
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void BtngEnviar_Click(object sender, EventArgs e)
    {
      StringBuilder sHtml = new StringBuilder();
      EmailingTest oEmailing = new EmailingTest();
      oEmailing.Host = txt_host.Text;
      oEmailing.From = txt_de.Text;
      oEmailing.UserSmtp = txt_login.Text;
      oEmailing.PwdSmtp = txt_pwd.Text;
      oEmailing.Address = txt_para.Text;
      oEmailing.Subject = "PRUEBA DE ENVIO DE CORREO.";
      oEmailing.Body = sHtml.Append(txtcomentarios.Text);

      if (oEmailing.EmailSend())
      {
        Label5.Text = "Correo enviado correctamente...";
      }
      else {
        Label5.Text = "El correo no pudo enviarse correctamente: " + oEmailing.Error;
      }
    }
  }
}