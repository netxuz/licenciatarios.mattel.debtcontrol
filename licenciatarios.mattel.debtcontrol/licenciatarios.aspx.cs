using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Telerik.Web.UI;
using DebtControl.Method;

namespace licenciatarios.mattel.debtcontrol
{
  public partial class licenciatarios : System.Web.UI.Page
  {
    Web oWeb = new Web();
    Usuario oUsuario;
    protected void Page_Load(object sender, EventArgs e)
    {
      oWeb.ValidaSession();
      oUsuario = oWeb.GetObjUsuario();
      if (!IsPostBack)
      {
        lblNombreUsuario.Text = "Bienvenido " + oUsuario.Nombres;
        if (oUsuario.Tipo == "1")
        {
          rdMenu.Items.Add(new RadMenuItem("Ingreso de ventas"));
          rdMenu.Items.Add(new RadMenuItem("Comprobante pago de impuesto"));
          rdMenu.Items.Add(new RadMenuItem("Reporte ventas"));
          rdMenu.Items.Add(new RadMenuItem("Status de facturas"));
          rdMenu.Items.Add(new RadMenuItem("Mínimo garantizado"));
          rdMenu.Items.Add(new RadMenuItem("Aging de deuda"));

          StringBuilder sStyle = new StringBuilder();
          sStyle.Append("<style>");
          sStyle.Append(" #rdMenu { ");
          sStyle.Append(" position:absolute!important; ");
          sStyle.Append(" bottom:5px!important; ");
          sStyle.Append("} ");
          sStyle.Append("</style>");
          this.Page.Header.Controls.Add(new LiteralControl(sStyle.ToString()));
          RadPaneTop.Height = 80;
        }
        else
        {
          RadMenuItem oItem = new RadMenuItem();

          switch (oUsuario.VistaMenu)
          {
            case "T":
              oItem.Text = "Finanzas";
              oItem.Selected = true;
              rdMenu.Items.Add(oItem);

              fnctSelected("Finanzas");

              rdMenu.Items.Add(new RadMenuItem("Reportes"));
              rdSubMenu.Visible = true;
              break;
            case "F":
              oItem.Text = "Finanzas";
              oItem.Selected = true;
              rdMenu.Items.Add(oItem);

              fnctSelected("Finanzas");
              rdSubMenu.Visible = true;
              break;
            case "R":
              oItem.Text = "Reportes";
              oItem.Selected = true;
              rdMenu.Items.Add(oItem);

              fnctSelected("Reportes");
              rdSubMenu.Visible = true;
              break;
          }
        }
      }
    }

    protected void fnctSelected(string tData)
    {

      switch (tData)
      {
        case "Ingreso de ventas":
          RadPaneDown.ContentUrl = "ingreso_ventas.aspx";
          break;
        //case "Aprobación de Facturas":
        //  RadPaneDown.ContentUrl = "aprobacion_facturas.aspx";
        //  break;
        case "Comprobante pago de impuesto":
          RadPaneDown.ContentUrl = "comprobante-pago-impuesto.aspx";
          break;
        case "Reporte ventas":
          RadPaneDown.ContentUrl = "reporte_ventas.aspx";
          break;
        case "Status de facturas":
          RadPaneDown.ContentUrl = "status-facturas.aspx";
          break;
        case "Mínimo garantizado":
          RadPaneDown.ContentUrl = "avance-minimo-garantizado.aspx";
          break;
        case "Aging de deuda":
          RadPaneDown.ContentUrl = "aging_licenciatario.aspx";
          break;
        case "Finanzas":
          rdSubMenu.Items.Clear();
          rdSubMenu.Items.Add(new RadMenuItem("Facturas Advance"));
          rdSubMenu.Items.Add(new RadMenuItem("Facturas Q"));
          rdSubMenu.Items.Add(new RadMenuItem("Facturas Short Fall"));
          rdSubMenu.Items.Add(new RadMenuItem("Reporting Regional"));
          break;
        case "Reportes":
          rdSubMenu.Items.Clear();
          rdSubMenu.Items.Add(new RadMenuItem("Reporte de venta"));
          rdSubMenu.Items.Add(new RadMenuItem("Status de facturas"));
          rdSubMenu.Items.Add(new RadMenuItem("Mínimo garantizado"));
          rdSubMenu.Items.Add(new RadMenuItem("Aging de deuda"));
          rdSubMenu.Items.Add(new RadMenuItem("Resumen Contrato"));
          rdSubMenu.Items.Add(new RadMenuItem("Administración de contratos"));
          break;
      }
    }

    protected void rdMenu_ItemClick(object sender, Telerik.Web.UI.RadMenuEventArgs e)
    {
      fnctSelected(e.Item.Text);
    }

    protected void rdSubMenu_ItemClick(object sender, Telerik.Web.UI.RadMenuEventArgs e)
    {
      switch (e.Item.Text)
      {
        case "Facturas Advance":
          RadPaneDown.ContentUrl = "contratos_afacturar_advance.aspx";
          break;
        case "Facturas Q":
          RadPaneDown.ContentUrl = "clientes_afacturar.aspx";
          break;
        case "Facturas Short Fall":
          RadPaneDown.ContentUrl = "aprobar_facturas_short_fall.aspx";
          break;
        case "Reporte de venta":
          RadPaneDown.ContentUrl = "reporte_ventas_finanzas.aspx";
          break;
        case "Status de facturas":
          RadPaneDown.ContentUrl = "status_facturas_finanzas.aspx";
          break;
        case "Mínimo garantizado":
          RadPaneDown.ContentUrl = "avance_minimo_garantizado_cliente.aspx";
          break;
        case "Aging de deuda":
          RadPaneDown.ContentUrl = "aging_finanzas.aspx";
          break;
        case "Resumen Contrato":
          RadPaneDown.ContentUrl = "resumen_contrato.aspx";
          break;
        case "Reporting Regional":
          RadPaneDown.ContentUrl = "reporting_regional.aspx";
          break;
        case "Administración de contratos":
          RadPaneDown.ContentUrl = "\\adm\\madmcontratos.aspx";
          break;

      }
    }

  }
}