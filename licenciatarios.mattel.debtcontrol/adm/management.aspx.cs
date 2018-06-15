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
  public partial class management : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void rdMenu_ItemClick(object sender, Telerik.Web.UI.RadMenuEventArgs e)
    {
      rdSubMenu.Items.Clear();
      switch (e.Item.Text)
      {
        case "CUENTAS":
          rdSubMenu.Items.Add(new RadMenuItem("USUARIOS"));
          break;
        case "CONFIGURACION":
          rdSubMenu.Items.Add(new RadMenuItem("MENSAJES"));
          rdSubMenu.Items.Add(new RadMenuItem("SERVICIO DE MENSAJERIA"));
          break;
        case "LOG":
          RadPaneDown.ContentUrl = "logeventos.aspx";
          break;
        case "ELIMINACIÓN DE FACTURAS":
          RadPaneDown.ContentUrl = "meliminafacturas.aspx";
          break;
        case "ADMINISTRACIÓN DE CONTRATOS":
          RadPaneDown.ContentUrl = "madmcontratos.aspx";
          break;
      }
      rdSubMenu.Visible = true;
      e.Item.Selected = true;
    }

    protected void rdSubMenu_ItemClick(object sender, Telerik.Web.UI.RadMenuEventArgs e)
    {
      switch (e.Item.Text)
      {
        case "USUARIOS":
          RadPaneDown.ContentUrl = "lusuarios.aspx";
          break;
        case "MENSAJES":
          RadPaneDown.ContentUrl = "mparamemail.aspx";
          break;
        case "SERVICIO DE MENSAJERIA":
          RadPaneDown.ContentUrl = "mserviciomensajeria.aspx";
          break;
      }
    }
  }
}