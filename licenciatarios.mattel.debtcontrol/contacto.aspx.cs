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
  public partial class contacto : System.Web.UI.Page
  {
    bool bEmailOk;
    Web oWeb = new Web();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void BtngEnviar_Click(object sender, EventArgs e)
    {
      if (bEmailOk)
      {
        
        string sTxtNombres = txtnombres.Text;
        string sTxtCelular = txtcelular.Text;
        string sTxtEmail = txtemail.Text;
        string sTxtComentarios = txtcomentarios.Text;

        StringBuilder sMensaje = new StringBuilder();
        sMensaje.Append("<html>");
        sMensaje.Append("<body>");
        sMensaje.Append("Nombrse : ").Append(sTxtNombres).Append("<br>");
        sMensaje.Append("Celular : ").Append(sTxtCelular).Append("<br>");
        sMensaje.Append("Email : ").Append(sTxtEmail).Append("<br>");
        sMensaje.Append("Comentario : ").Append(sTxtComentarios).Append("<br>");
        sMensaje.Append("</body>");
        sMensaje.Append("</html>");

        Emailing oEmailing = new Emailing();
        oEmailing.FromName = Application["NameSender"].ToString();
        oEmailing.From = Application["EmailSender"].ToString();
        oEmailing.Address = Application["EmailSender"].ToString();
        oEmailing.Subject = "Mensaje de contáctenos de Licenciatarios DebtControl";
        oEmailing.Body = sMensaje;

        StringBuilder js = new StringBuilder();
        js.Append("function LgRespuesta() {");
        if (oEmailing.EmailSend())
        {
          js.Append(" window.radalert('El mensaje fue enviado exitosamente.', 400, 100,''); ");
        }
        else
        {
          js.Append(" window.radalert('El mensaje no pudo ser enviado, intente más tarde.', 400, 100,''); ");
        }
        js.Append(" Sys.Application.remove_load(LgRespuesta); ");
        js.Append("};");
        js.Append("Sys.Application.add_load(LgRespuesta);");
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "LgRespuesta", js.ToString(), true);

        txtnombres.Text = string.Empty;
        txtcelular.Text = string.Empty;
        txtemail.Text = string.Empty;
        txtcomentarios.Text = string.Empty;

      }
      else
      {
        StringBuilder js = new StringBuilder();
        js.Append("function LgRespuesta() {");
        js.Append(" window.radalert('El email ingresado no es valido, por favor vuelva a ingresarlo.', 400, 100,'Atención'); ");
        js.Append(" Sys.Application.remove_load(LgRespuesta); ");
        js.Append("};");
        js.Append("Sys.Application.add_load(LgRespuesta);");
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "LgRespuesta", js.ToString(), true);
      }
    }

    protected void valtxtEmailVal_ServerValidate(object source, ServerValidateEventArgs args)
    {
      try
      {
        bEmailOk = args.IsValid = oWeb.ValidaMail(args.Value);
      }
      catch
      {
        bEmailOk = args.IsValid = false;
      }
    }
  }
}
