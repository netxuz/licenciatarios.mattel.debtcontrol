using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DebtControl.Conn;
using DebtControl.Method;
using DebtControl.Model;

namespace licenciatarios.mattel.debtcontrol.adm
{
  public partial class mparamemail : System.Web.UI.Page
  {
    Web oWeb = new Web();
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
        getDataEmail(rdCmbEmails.SelectedValue);
    }

    private void getDataEmail(string pTipoEmail)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cParamEmail oParamEmail = new cParamEmail(ref oConn);
        oParamEmail.TipoEmail = pTipoEmail;
        DataTable dParamEmail = oParamEmail.Get();
        if (dParamEmail != null)
          if (dParamEmail.Rows.Count > 0)
          {
            hdd_accion.Value = "EDITAR";
            txtNomEmail.Text = dParamEmail.Rows[0]["nom_email"].ToString();
            txtAsunto.Text = dParamEmail.Rows[0]["asunto_email"].ToString();
            rdCuerpoEmail.Content = dParamEmail.Rows[0]["cuerpo_email"].ToString();
          }
          else
          {
            hdd_accion.Value = "CREAR";
            txtNomEmail.Text = string.Empty;
            txtAsunto.Text = string.Empty;
            rdCuerpoEmail.Content = string.Empty;
          }
        dParamEmail = null;

        oConn.Close();
      }

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cParamEmail oParamEmail = new cParamEmail(ref oConn);
        oParamEmail.TipoEmail = rdCmbEmails.SelectedValue;
        oParamEmail.NomEmail = txtNomEmail.Text;
        oParamEmail.AsuntoEmail = txtAsunto.Text;
        oParamEmail.CuerpoEmail = rdCuerpoEmail.Content;
        oParamEmail.Accion = hdd_accion.Value;
        oParamEmail.Put();
        
        oConn.Close();
      }
    }

    protected void rdCmbEmails_SelectedIndexChanged(object o, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
      getDataEmail(rdCmbEmails.SelectedValue);
    }
  }
}