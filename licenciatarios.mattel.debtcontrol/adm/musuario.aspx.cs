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
using Telerik.Web.UI;

namespace licenciatarios.mattel.debtcontrol.adm
{
  public partial class musuario : System.Web.UI.Page
  {
    Web oWeb = new Web();
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        rdCmbCliente.Items.Add(new ListItem("<< Seleccione Cliente", "0"));
        rdCmbLicencitarios.Items.Add(new ListItem("<< Seleccione Licenciatario", "0"));
        DBConn oConn = new DBConn();
        if (oConn.Open())
        {
          cCliente oCliente = new cCliente(ref oConn);
          DataTable dtCliente = oCliente.Get();
          if (dtCliente != null) {
            foreach (DataRow oRow in dtCliente.Rows)
            {
              rdCmbCliente.Items.Add(new ListItem(oRow["sNombre"].ToString(), oRow["nKey_cliente"].ToString()));
            }
          }
          dtCliente = null;

          cDeudor oDeudor = new cDeudor(ref oConn);
          DataTable dtDeudor = oDeudor.Get();
          if (dtDeudor != null)
          {
            foreach (DataRow oRow in dtDeudor.Rows)
            {
              rdCmbLicencitarios.Items.Add(new ListItem(oRow["sNombre"].ToString(), oRow["nKey_Deudor"].ToString()));
            }
          }
          dtDeudor = null;

          oConn.Close();
        }
        hdd_codusuario.Value = oWeb.GetData("codusuario");
        if (!string.IsNullOrEmpty(hdd_codusuario.Value))
          getUsuario();
      }
    }

    protected void getUsuario()
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cSysUsuario oUsuario = new cSysUsuario(ref oConn);
        oUsuario.CodUser = hdd_codusuario.Value;
        DataTable dUsuario = oUsuario.Get();
        if (dUsuario != null)
        {
          if (dUsuario.Rows.Count > 0)
          {
            txtnombre.Text = dUsuario.Rows[0]["nom_user"].ToString();
            txtapellido.Text = dUsuario.Rows[0]["ape_user"].ToString();
            txtemail.Text = dUsuario.Rows[0]["eml_user"].ToString();
            txtlogin.Text = dUsuario.Rows[0]["login_user"].ToString();
            txtpassword.Attributes.Add("value", ((dUsuario.Rows[0]["pwd_user"] == null) || (string.IsNullOrEmpty(dUsuario.Rows[0]["pwd_user"].ToString())) ? "" : oWeb.UnCrypt(dUsuario.Rows[0]["pwd_user"].ToString())));
            ddlestado.Items.FindByValue(dUsuario.Rows[0]["est_user"].ToString()).Selected = true;
            if (!string.IsNullOrEmpty(dUsuario.Rows[0]["cod_tipo"].ToString()))
              ddlTipoUsuario.Items.FindByValue(dUsuario.Rows[0]["cod_tipo"].ToString()).Selected = true;

            idAtrib.Visible = true;

            cSysPerfilesUsuarios oSysPerfilesUsuarios = new cSysPerfilesUsuarios(ref oConn);
            oSysPerfilesUsuarios.CodUser = hdd_codusuario.Value;
            DataTable dtPerfiles = oSysPerfilesUsuarios.Get();
            if (dtPerfiles != null)
            {
              foreach (DataRow oRow in dtPerfiles.Rows)
              {
                foreach (ListItem item in rdbtnlist_roles.Items)
                {
                  if (item.Value == oRow["cod_perfil"].ToString())
                  {
                    item.Selected = true;
                    if (item.Value == "4") {
                      idcliente.Visible = true;
                      cSysUserCliente oSysUserCliente = new cSysUserCliente(ref oConn);
                      oSysUserCliente.CodUser = hdd_codusuario.Value;
                      DataTable dtUserCliente = oSysUserCliente.Get();
                      if (dtUserCliente != null)
                      {
                        if (dtUserCliente.Rows.Count > 0) {
                          foreach (ListItem rdItem in rdCmbCliente.Items)
                          {
                            if (rdItem.Value == dtUserCliente.Rows[0]["nkey_cliente"].ToString())
                            {
                              rdItem.Selected = true;
                            }
                          }

                          foreach (ListItem rdItemPerfil in ddlperfil.Items) {
                            if (rdItemPerfil.Value == dtUserCliente.Rows[0]["tipo_cliente"].ToString())
                            {
                              rdItemPerfil.Selected = true;
                            }
                          }
                        }
                      }
                      dtUserCliente = null;

                    }
                    else if (item.Value == "5")
                    {
                      idLicenciatario.Visible = true;
                      cSysUserDeudor oSysUserDeudor = new cSysUserDeudor(ref oConn);
                      oSysUserDeudor.CodUser = hdd_codusuario.Value;
                      DataTable dtUserDeudor = oSysUserDeudor.Get();
                      if (dtUserDeudor != null)
                      {
                        if (dtUserDeudor.Rows.Count > 0)
                        {
                          foreach (ListItem rditem in rdCmbLicencitarios.Items)
                          {
                            if (rditem.Value == dtUserDeudor.Rows[0]["nkey_deudor"].ToString())
                            {
                              rditem.Selected = true;
                            }
                          }
                        }
                      }
                      dtUserDeudor = null;
                    }
                  }
                }
              }
            }
            dtPerfiles = null;
          }
        }
        dUsuario = null;
        oConn.Close();
      }
    }

    protected void btnGrabar_Click(object sender, EventArgs e)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        string sClave = oWeb.Crypt(txtpassword.Text);

        cSysUsuario oSysUsuario = new cSysUsuario(ref oConn);
        oSysUsuario.CodUser = hdd_codusuario.Value;
        oSysUsuario.NomUser = txtnombre.Text;
        oSysUsuario.ApeUser = txtapellido.Text;
        oSysUsuario.EmlUser = txtemail.Text;
        oSysUsuario.LoginUser = txtlogin.Text;
        oSysUsuario.PwdUser = sClave;
        oSysUsuario.EstUser = ddlestado.SelectedValue;
        oSysUsuario.CodTipoUsuario = ddlTipoUsuario.SelectedValue;
        oSysUsuario.Accion = (string.IsNullOrEmpty(hdd_codusuario.Value) ? "CREAR" : "EDITAR");
        oSysUsuario.Put();
        hdd_codusuario.Value = oSysUsuario.CodUser;
        txtpassword.Attributes.Add("value", txtpassword.Text);

        idAtrib.Visible = true;
        idcliente.Visible = false;
        idLicenciatario.Visible = false;
        cSysPerfilesUsuarios oSysPerfilesUsuarios = new cSysPerfilesUsuarios(ref oConn);
        oSysPerfilesUsuarios.CodUser = hdd_codusuario.Value;
        oSysPerfilesUsuarios.Accion = "ELIMINAR";
        oSysPerfilesUsuarios.Put();

        cSysUserCliente oSysUserCliente = new cSysUserCliente(ref oConn);
        oSysUserCliente.CodUser = hdd_codusuario.Value;
        oSysUserCliente.Accion = "ELIMINAR";
        oSysUserCliente.Put();

        cSysUserDeudor oSysUserDeudor = new cSysUserDeudor(ref oConn);
        oSysUserDeudor.CodUser = hdd_codusuario.Value;
        oSysUserDeudor.Accion = "ELIMINAR";
        oSysUserDeudor.Put();

        oSysPerfilesUsuarios.Accion = "CREAR";
        foreach (ListItem item in rdbtnlist_roles.Items)
        {
          if (item.Selected)
          {
            oSysPerfilesUsuarios.CodPerfil = item.Value;
            oSysPerfilesUsuarios.Put();

            if (item.Value == "4") {
              idcliente.Visible = true;
              if (rdCmbCliente.SelectedValue != "0") {
                oSysUserCliente.NKeyCliente = rdCmbCliente.SelectedValue;
                oSysUserCliente.TipoCliente = ddlperfil.SelectedValue;
                oSysUserCliente.Accion = "CREAR";
                oSysUserCliente.Put();
              }
            }
            else if  (item.Value == "5")
            {
              idLicenciatario.Visible = true;
              if (rdCmbLicencitarios.SelectedValue != "0")
              {
                oSysUserDeudor.NKeyDeudor = rdCmbLicencitarios.SelectedValue;
                oSysUserDeudor.Accion = "CREAR";
                oSysUserDeudor.Put();
              }
            }
          }
        }

        oConn.Close();
      }

    }
  }
}