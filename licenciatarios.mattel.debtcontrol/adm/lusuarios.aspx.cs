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
  public partial class lusuarios : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void rdUsuarios_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        /*GridColumn oGridColumn;

        oGridColumn = rdUsuarios.MasterTableView.Columns.FindByUniqueName("NomUsuario");
        oGridColumn.HeaderText = "Nombres";

        oGridColumn = rdUsuarios.MasterTableView.Columns.FindByUniqueName("ApeUsuario");
        oGridColumn.HeaderText = "Apellidos";

        oGridColumn = rdUsuarios.MasterTableView.Columns.FindByUniqueName("EstUsuario");
        oGridColumn.HeaderText = "Estado";*/

        cSysUsuario oUsuario = new cSysUsuario(ref oConn);
        if (!string.IsNullOrEmpty(txt_buscar.Text.ToString()))
        {
          oUsuario.NomUser = txt_buscar.Text;
        }
        rdUsuarios.DataSource = oUsuario.Get();

        oConn.Close();
      }

    }

    protected void rdUsuarios_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
      if (e.Item is GridDataItem)
      {
        GridDataItem oItem = e.Item as GridDataItem;

        if ((!string.IsNullOrEmpty(oItem["EstUsuario"].Text)) && (oItem["EstUsuario"].Text != "&nbsp;"))
        {
          if (oItem["EstUsuario"].Text == "V")
            oItem["EstUsuario"].Text = "Vigente";
          else if (oItem["EstUsuario"].Text == "B")
            oItem["EstUsuario"].Text = "Bloqueado";
          else if (oItem["EstUsuario"].Text == "E")
            oItem["EstUsuario"].Text = "Eliminado";
          else
            oItem["EstUsuario"].Text = "No Vigente";
        }
      }
    }

    protected void rdUsuarios_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
    {
      switch (e.CommandName)
      {
        case "cmdEdit":
          string[] cParam = new string[2];
          cParam[0] = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["cod_user"].ToString();
          Response.Redirect(String.Format("musuario.aspx?codusuario={0}", cParam));
          break;
        case "cmdDelete":
          string pCodUsuario = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["cod_user"].ToString();
          DBConn oConn = new DBConn();
          if (oConn.Open())
          {
            cSysUserCliente oUserCliente = new cSysUserCliente(ref oConn);
            oUserCliente.CodUser = pCodUsuario;
            oUserCliente.Accion = "ELIMINAR";
            oUserCliente.Put();

            cSysUserDeudor oUserDeudor = new cSysUserDeudor(ref oConn);
            oUserDeudor.CodUser = pCodUsuario;
            oUserDeudor.Accion = "ELIMINAR";
            oUserDeudor.Put();

            cSysPerfilesUsuarios oPerfilesUsuarios = new cSysPerfilesUsuarios(ref oConn);
            oPerfilesUsuarios.CodUser = pCodUsuario;
            oPerfilesUsuarios.Accion = "ELIMINAR";
            oPerfilesUsuarios.Put();

            cSysUsuario oUsuario = new cSysUsuario(ref oConn);
            oUsuario.CodUser = pCodUsuario;
            oUsuario.Accion = "ELIMINAR";
            oUsuario.Put();

            oConn.Close();
          }
          rdUsuarios.Rebind();
          break;
      }
    }

    protected void btnCrear_Click(object sender, EventArgs e)
    {
      Response.Redirect("musuario.aspx");
    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
      rdUsuarios.Rebind();
    }
  }
}