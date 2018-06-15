using System;
using System.Web.UI;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.IO;
using DebtControl.Conn;
using DebtControl.Method;
using DebtControl.Model;
using Telerik.Web.UI;
using ClosedXML.Excel;

namespace licenciatarios.mattel.debtcontrol.adm
{
  public partial class logeventos : System.Web.UI.Page
  {
    Web oWeb = new Web();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
      obgrilla.Visible = true;
      RadGrid1.Rebind();
    }

    protected void rdLog_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cDeudor oDeudor = new cDeudor(ref oConn);
        if (!string.IsNullOrEmpty(txtBuscarUsuario.Text))
          oDeudor.SNombre = txtBuscarUsuario.Text;
        rdLog.DataSource = oDeudor.Get();
      }
      oConn.Close();
    }

    protected void BtnBuscarLicenciatarios_Click(object sender, EventArgs e)
    {
      rdLog.Rebind();
    }

    protected void rdLog_ItemCreated(object sender, GridItemEventArgs e)
    {
      if (e.Item is GridDataItem)
      {
        GridDataItem item = (GridDataItem)e.Item;
        LinkButton lb = (LinkButton)item["SelectRecord"].Controls[0];
        lb.Attributes.Add("onclick", "onSelectClient('" + item.ItemIndex + "');");
      }
    }

    protected void RadGrid1_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cLogEventos oLogEventos = new cLogEventos(ref oConn);
        oLogEventos.NkeyDeudor = hdd_nkey_deudor.Value;
        oLogEventos.NoContrato = txt_no_contrato.Text;
        oLogEventos.CodFlujo = dropdownflujo.SelectedValue;

        if ((RadDatePicker1.SelectedDate != null) && (RadDatePicker2.SelectedDate != null))
        {
          oLogEventos.FchDesdeLog = RadDatePicker1.SelectedDate.Value.ToString("yyyyMMdd") + " 00:00:00";
          oLogEventos.FchHastaLog = RadDatePicker2.SelectedDate.Value.ToString("yyyyMMdd") + " 23:59:59";
        }

        RadGrid1.DataSource = oLogEventos.Get();
      }
      oConn.Close();
    }

    protected void RadGrid1_ItemCommand(object source, GridCommandEventArgs e)
    {

      if (e.CommandName == "DownFile")
      {
        string sNomArchivo = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["arch_log"].ToString();
        string sIndRepArch = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ind_reporsitorio_arch"].ToString();

        Response.Redirect("downloadfile.ashx?sNomArchivo=" + sNomArchivo + "&sIndRepArch=" + sIndRepArch);
      }
      else if (e.CommandName == RadGrid.ExportToExcelCommandName)
      {
        string sNkeyDeudor = hdd_nkey_deudor.Value;
        string sNoContrato = txt_no_contrato.Text;
        string sCodFlujo = dropdownflujo.SelectedValue;
        string sRadDatePicker1 = string.Empty;
        string sRadDatePicker2 = string.Empty;

        if ((RadDatePicker1.SelectedDate != null)&&(RadDatePicker2.SelectedDate != null)){
          sRadDatePicker1 = RadDatePicker1.SelectedDate.Value.ToString("yyyyMMdd") + " 00:00:00";
          sRadDatePicker2 = RadDatePicker2.SelectedDate.Value.ToString("yyyyMMdd") + " 23:59:59";
        }

        Response.Redirect("downloadlog.ashx?sNkeyDeudor=" + sNkeyDeudor + "&sNoContrato=" + sNoContrato + "&sCodFlujo=" + sCodFlujo + "&sRadDatePicker1=" + sRadDatePicker1 + "&sRadDatePicker2=" + sRadDatePicker2);
      }

    }

    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {

      string sNomArchivo = string.Empty;
      string sIndRepArch = string.Empty;
      if (e.Item is GridDataItem)
      {
        if (string.IsNullOrEmpty(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["arch_log"].ToString()))
        {
          GridDataItem item = (GridDataItem)e.Item;


          ImageButton oButton = (ImageButton)item["BtnVerArchivo"].FindControl("btnDownFile");
          oButton.Visible = false;

          //DataRowView row = (DataRowView)e.Item.DataItem;

          //if (!string.IsNullOrEmpty(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["arch_log"].ToString()))
          //{
          //  sNomArchivo = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["cod_registro"].ToString();
          //  sIndRepArch = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ind_reporsitorio_arch"].ToString();

          //  GridTemplateColumn oGridTemplateColumn = 


          //}
        }
      }
    }

    
  }
}
