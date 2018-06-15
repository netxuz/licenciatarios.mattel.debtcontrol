using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using DebtControl.Conn;
using DebtControl.Method;
using DebtControl.Model;
using Telerik.Web.UI;

namespace licenciatarios.mattel.debtcontrol
{
  public partial class ingreso_ventas : System.Web.UI.Page
  {
    Web oWeb = new Web();
    Usuario oUsuario;
    
    protected void Page_Load(object sender, EventArgs e)
    {
      Session["Error"] = string.Empty;
      oWeb.ValidaSession();
      oUsuario = oWeb.GetObjUsuario();
      if (!IsPostBack)
      {
        DBConn oConn = new DBConn();
        if (oConn.Open())
        {
          cContratos oContratos = new cContratos(ref oConn);
          oContratos.NkeyDeudor = oUsuario.NKeyUsuario;
          DataTable dtContrato = oContratos.Get();
          if (dtContrato != null)
          {
            if (dtContrato.Rows.Count > 0)
            {
              foreach (DataRow oRow in dtContrato.Rows)
              {
                cmbox_contrato.Items.Add(new ListItem(oRow["no_contrato"].ToString(), oRow["num_contrato"].ToString()));
              }
            }
          }
          dtContrato = null;
        }
        oConn.Close();
      }
    }

    protected void cmbox_contrato_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (cmbox_contrato.SelectedValue != "0")
      {
        DBConn oConn = new DBConn();
        if (oConn.Open())
        {
          cReporteVenta oReporteVenta = new cReporteVenta(ref oConn);
          oReporteVenta.NumContrato = cmbox_contrato.SelectedValue;
          oReporteVenta.Facturado = "N";
          oReporteVenta.EstReporte = "P";
          oReporteVenta.OrderMes = true;
          DataTable dtReporteVenta = oReporteVenta.Get();

          if (dtReporteVenta != null)
          {
            if (dtReporteVenta.Rows.Count > 0)
            {
              ddlmesventa.Items.Clear();
              oWeb.getMesesDdlist(ddlmesventa, int.Parse(dtReporteVenta.Rows[0]["mes_reporte"].ToString()));
              ViewState["ano_reporte"] = dtReporteVenta.Rows[0]["ano_reporte"].ToString();

              //DateTime dFechFact = DateTime.Parse("01/" + dtReporteVenta.Rows[0]["mes_reporte"].ToString() + "/" + dtReporteVenta.Rows[0]["ano_reporte"].ToString());
              //DateTime dFechNext = dFechFact.AddMonths(1);
              //oWeb.getMesesDdlist(ddlmesventa, dFechNext.Month);
              //ViewState["ano_reporte"] = dFechNext.Year.ToString();
            }
            else
            {
              oReporteVenta.Facturado = "F";
              oReporteVenta.EstReporte = "C";
              oReporteVenta.OrderMes = false;
              oReporteVenta.OrderDesc = true;
              DataTable dtRepVenta = oReporteVenta.Get();
              if (dtRepVenta != null)
              {
                if (dtRepVenta.Rows.Count > 0)
                {
                  DateTime dFechFact = DateTime.Parse("01/" + dtRepVenta.Rows[0]["mes_reporte"].ToString() + "/" + dtRepVenta.Rows[0]["ano_reporte"].ToString());
                  DateTime dFechNext = dFechFact.AddMonths(1);
                  ddlmesventa.Items.Clear();
                  oWeb.getMesesDdlist(ddlmesventa, dFechNext.Month);
                  ViewState["ano_reporte"] = dFechNext.Year.ToString();
                }
                else
                {
                  oReporteVenta.Facturado = "N";
                  oReporteVenta.EstReporte = "L";
                  oReporteVenta.OrderMes = false;
                  oReporteVenta.OrderDesc = true;
                  DataTable dtVenta = oReporteVenta.Get();
                  if (dtVenta != null)
                  {
                    if (dtVenta.Rows.Count > 0)
                    {
                      DateTime dFechFact = DateTime.Parse("01/" + dtVenta.Rows[0]["mes_reporte"].ToString() + "/" + dtVenta.Rows[0]["ano_reporte"].ToString());
                      DateTime dFechNext = dFechFact.AddMonths(1);
                      ddlmesventa.Items.Clear();
                      oWeb.getMesesDdlist(ddlmesventa, dFechNext.Month);
                      ViewState["ano_reporte"] = dFechNext.Year.ToString();
                    }
                    else
                    {
                      oReporteVenta.Facturado = "N";
                      oReporteVenta.EstReporte = "C";
                      oReporteVenta.OrderMes = false;
                      oReporteVenta.OrderDesc = true;
                      DataTable dtVenta2 = oReporteVenta.Get();
                      if (dtVenta2 != null)
                      {
                        if (dtVenta2.Rows.Count > 0)
                        {
                          DateTime dFechFact = DateTime.Parse("01/" + dtVenta2.Rows[0]["mes_reporte"].ToString() + "/" + dtVenta2.Rows[0]["ano_reporte"].ToString());
                          DateTime dFechNext = dFechFact.AddMonths(1);
                          ddlmesventa.Items.Clear();
                          oWeb.getMesesDdlist(ddlmesventa, dFechNext.Month);
                          ViewState["ano_reporte"] = dFechNext.Year.ToString();
                        }
                        else
                        {
                          ddlmesventa.Items.Clear();
                          //ViewState["ano_reporte"] = "2015";
                          //oWeb.getMesesDdlist(ddlmesventa, 10);
                          cContratos oContratos = new cContratos(ref oConn);
                          oContratos.NumContrato = cmbox_contrato.SelectedValue;
                          DataTable dtContrato = oContratos.Get();
                          if (dtContrato != null)
                          {
                            if (dtContrato.Rows.Count > 0)
                            {
                              ViewState["ano_reporte"] = DateTime.Parse(dtContrato.Rows[0]["fech_inicio"].ToString()).Year.ToString();
                              oWeb.getMesesDdlist(ddlmesventa, DateTime.Parse(dtContrato.Rows[0]["fech_inicio"].ToString()).Month);
                            }
                          }
                          dtContrato = null;
                        }
                      }
                      dtVenta2 = null;
                    }
                  }
                  dtVenta = null;
                }
              }
              dtRepVenta = null;
            }
          }
          dtReporteVenta = null;
        }
        oConn.Close();
      }
      else
      {
        ddlmesventa.Items.Clear();
        ddlmesventa.Items.Add(new ListItem("", "0"));
        ViewState["ano_reporte"] = string.Empty;
        rdDetalleVenta.Rebind();
      }
    }

    protected void ddlmesventa_SelectedIndexChanged(object sender, EventArgs e)
    {
      rdDetalleVenta.Rebind();
    }

    protected void rdDetalleVenta_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
      if ((!string.IsNullOrEmpty(cmbox_contrato.SelectedValue)) && (!string.IsNullOrEmpty(ddlmesventa.SelectedValue)))
      {
        DBConn oConn = new DBConn();
        if (oConn.Open())
        {
          cDetalleVenta oDetalleVenta;

          cReporteVenta oReporteVenta = new cReporteVenta(ref oConn);
          oReporteVenta.NumContrato = cmbox_contrato.SelectedValue;
          oReporteVenta.MesReporte = ddlmesventa.SelectedValue;
          oReporteVenta.AnoReporte = ViewState["ano_reporte"].ToString();
          DataTable dtReporteVenta = oReporteVenta.Get();
          if (dtReporteVenta != null)
          {
            if (dtReporteVenta.Rows.Count > 0)
            {
              ViewState["cod_reporte_venta"] = dtReporteVenta.Rows[0]["cod_reporte_venta"].ToString();
              oDetalleVenta = new cDetalleVenta(ref oConn);
              oDetalleVenta.CodigoReporteVenta = ViewState["cod_reporte_venta"].ToString();
              rdDetalleVenta.DataSource = oDetalleVenta.Get();
            }
            else
            {
              ViewState["cod_reporte_venta"] = string.Empty;
              oReporteVenta.MesReporte = string.Empty;
              oReporteVenta.AnoReporte = string.Empty;
              oReporteVenta.OrderDesc = true;
              DataTable dtReporte = oReporteVenta.Get();
              if (dtReporte != null)
              {
                if (dtReporte.Rows.Count > 0)
                {
                  oDetalleVenta = new cDetalleVenta(ref oConn);
                  oDetalleVenta.IndLoadDatMonthAnt = true;
                  oDetalleVenta.CodigoReporteVenta = dtReporte.Rows[0]["cod_reporte_venta"].ToString();
                  rdDetalleVenta.DataSource = oDetalleVenta.Get();
                }
                else
                {
                  oDetalleVenta = new cDetalleVenta(ref oConn);
                  oDetalleVenta.CodigoReporteVenta = "0";
                  rdDetalleVenta.DataSource = oDetalleVenta.Get();
                }
              }
              dtReporte = null;
            }
          }
          dtReporteVenta = null;
        }
        oConn.Close();
      }
    }

    protected void rdDetalleVenta_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
      if ((e.Item is GridEditableItem) && (e.Item.IsInEditMode))
      {
        GridEditableItem editedItem = e.Item as GridEditableItem;
        GridEditManager editMan = editedItem.EditManager;

        //GridDropDownListColumnEditor editor1 = editMan.GetColumnEditor("marca") as GridDropDownListColumnEditor;
        //editor1.ComboBoxControl.DataTextField = "cod_marca";
        //editor1.ComboBoxControl.DataTextFormatString = "descripcion";
        //editor1.DataSource = getMarca(string.Empty, cmbox_contrato.SelectedValue);
        //editor1.DataBind();

        //GridDropDownListColumnEditor editor2 = editMan.GetColumnEditor("categoria") as GridDropDownListColumnEditor;
        //editor2.ComboBoxControl.DataTextField = "cod_categoria";
        //editor2.ComboBoxControl.DataTextFormatString = "descripcion";
        //editor2.ComboBoxControl.AutoPostBack = true;
        //editor2.ComboBoxControl.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(ComboBoxControl_SelectedIndexChanged);
        //editor2.DataSource = getCategoria(string.Empty, cmbox_contrato.SelectedValue);
        //editor2.DataBind();

        //GridTextBoxColumnEditor editor3 = editMan.GetColumnEditor("producto") as GridTextBoxColumnEditor;
        //TableCell cell1 = (TableCell)editor3.TextBoxControl.Parent;

        //RequiredFieldValidator validator1 = new RequiredFieldValidator();
        //editor3.TextBoxControl.ID = "ObValidationProducto";
        //validator1.ControlToValidate = editor3.TextBoxControl.ID;
        //validator1.ErrorMessage = "*";
        //cell1.Controls.Add(validator1);

        //GridTextBoxColumnEditor editor4 = editMan.GetColumnEditor("cliente") as GridTextBoxColumnEditor;
        //TableCell cell2 = (TableCell)editor4.TextBoxControl.Parent;

        //RequiredFieldValidator validator2 = new RequiredFieldValidator();
        //editor4.TextBoxControl.ID = "ObValidationCliente";
        //validator2.ControlToValidate = editor4.TextBoxControl.ID;
        //validator2.ErrorMessage = "*";
        //cell2.Controls.Add(validator2);

        //GridTextBoxColumnEditor editor5 = editMan.GetColumnEditor("sku") as GridTextBoxColumnEditor;
        //TableCell cell3 = (TableCell)editor5.TextBoxControl.Parent;

        //RequiredFieldValidator validator3 = new RequiredFieldValidator();
        //editor5.TextBoxControl.ID = "ObValidationSku";
        //validator3.ControlToValidate = editor5.TextBoxControl.ID;
        //validator3.ErrorMessage = "*";
        //cell3.Controls.Add(validator3);

        //GridTextBoxColumnEditor editor6 = editMan.GetColumnEditor("descripcion_producto") as GridTextBoxColumnEditor;
        //TableCell cell4 = (TableCell)editor6.TextBoxControl.Parent;

        //RequiredFieldValidator validator4 = new RequiredFieldValidator();
        //editor6.TextBoxControl.ID = "ObValidationSku";
        //validator4.ControlToValidate = editor6.TextBoxControl.ID;
        //validator4.ErrorMessage = "*";
        //cell4.Controls.Add(validator4);

      }
      else if ((e.Item is GridDataItem) && (e.Item.DataItem != null))
      {
        GridDataItem item = (GridDataItem)e.Item;
        DataRowView row = (DataRowView)e.Item.DataItem;
        /*item["marca"].Text = getMarca(row["cod_marca"].ToString(), cmbox_contrato.SelectedValue).Rows[0]["descripcion"].ToString();
        item["categoria"].Text = getCategoria(row["cod_categoria"].ToString(), cmbox_contrato.SelectedValue).Rows[0]["descripcion"].ToString();
        item["subcategoria"].Text = getSubCategoria(string.Empty, row["cod_subcategoria"].ToString(), string.Empty).Rows[0]["descripcion"].ToString();
        if (!string.IsNullOrEmpty(row["cod_royalty"].ToString()))
          item["descroyalty"].Text = getRoyalty(row["cod_royalty"].ToString()).Rows[0]["descripcion"].ToString();*/
        if (!string.IsNullOrEmpty(row["royalty"].ToString()))
          item["royalty"].Text = String.Format("{0:p0}", decimal.Parse(row["royalty"].ToString()));
        if (!string.IsNullOrEmpty(row["bdi"].ToString()))
          item["bdi"].Text = String.Format("{0:p0}", decimal.Parse(row["bdi"].ToString()));
        item["producto"].Text = row["producto"].ToString();
      }
    }

    protected void rdDetalleVenta_ItemDataBound(object sender, GridItemEventArgs e)
    {
      //if (e.Item is GridEditableItem && e.Item.IsInEditMode) //fire for both edit and insert  
      //{
      //  if (e.Item is GridEditableItem && (e.Item as GridEditableItem).IsInEditMode)
      //  {
      //    GridEditableItem editedItem = e.Item as GridEditableItem;
      //    GridEditManager editMan = editedItem.EditManager;

      //    GridDropDownListColumnEditor editor1 = editMan.GetColumnEditor("marca") as GridDropDownListColumnEditor;
      //    editor1.DataSource = getMarca(string.Empty, cmbox_contrato.SelectedValue);
      //    editor1.DataBind();
      //    //if ((editMan.GetColumnEditor("subcategoria") as GridDropDownListColumnEditor).SelectedValue == "")
      //      //editor1.SelectedValue = "0";

      //    GridDropDownListColumnEditor editor2 = editMan.GetColumnEditor("categoria") as GridDropDownListColumnEditor;
      //    editor2.ComboBoxControl.AutoPostBack = true;
      //    editor2.ComboBoxControl.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(ComboBoxControl_SelectedIndexChanged);
      //    editor2.DataSource = getCategoria(string.Empty, cmbox_contrato.SelectedValue);
      //    editor2.DataBind();
      //    //if ((editMan.GetColumnEditor("subcategoria") as GridDropDownListColumnEditor).SelectedValue == "")
      //      //editor2.SelectedValue = "0";

      //    if (e.Item.RowIndex != -1)
      //    {
      //      DataRowView row = (DataRowView)e.Item.DataItem;
      //      editor1.SelectedValue = row["cod_marca"].ToString();
      //      editor2.SelectedValue = row["cod_categoria"].ToString();

      //      GridDropDownListColumnEditor editor3 = editMan.GetColumnEditor("subcategoria") as GridDropDownListColumnEditor;
      //      editor3.DataSource = getSubCategoria(row["cod_categoria"].ToString(), string.Empty, cmbox_contrato.SelectedValue);
      //      editor3.DataBind();
      //      editor3.SelectedValue = row["cod_subcategoria"].ToString();
      //    }

      //  }
      //}
      //else if (e.Item is GridDataItem)
      if (e.Item is GridDataItem)
      {
        GridDataItem item = (GridDataItem)e.Item;
        DataRowView row = (DataRowView)e.Item.DataItem;
        //item["EditCommandColumn"].Visible = false;// to disable the entire cell 
        /*item["marca"].Text = getMarca(row["cod_marca"].ToString(), cmbox_contrato.SelectedValue).Rows[0]["descripcion"].ToString();
        item["categoria"].Text = getCategoria(row["cod_categoria"].ToString(), cmbox_contrato.SelectedValue).Rows[0]["descripcion"].ToString();
        item["subcategoria"].Text = getSubCategoria(string.Empty, row["cod_subcategoria"].ToString(), string.Empty).Rows[0]["descripcion"].ToString();
        if (!string.IsNullOrEmpty(row["cod_royalty"].ToString()))
          item["descroyalty"].Text = getRoyalty(row["cod_royalty"].ToString()).Rows[0]["descripcion"].ToString();*/
        if (!string.IsNullOrEmpty(row["royalty"].ToString()))
          item["royalty"].Text = String.Format("{0:p0}", decimal.Parse(row["royalty"].ToString()));
        if (!string.IsNullOrEmpty(row["bdi"].ToString()))
          item["bdi"].Text = String.Format("{0:p0}", decimal.Parse(row["bdi"].ToString()));
        item["producto"].Text = row["producto"].ToString();
      }
    }

    protected void ComboBoxControl_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {
      //rdDetalleVenta.Rebind();
      // Get the row we are editing
      GridEditableItem gefItem = (o as RadComboBox).NamingContainer as GridEditableItem;
      // Get the contact & requestor lists
      GridDropDownListColumnEditor gddlContact = gefItem.EditManager.GetColumnEditor("categoria") as GridDropDownListColumnEditor;
      GridDropDownListColumnEditor gddlReq = gefItem.EditManager.GetColumnEditor("subcategoria") as GridDropDownListColumnEditor;
      gddlReq.ComboBoxControl.DataTextField = "cod_subcategoria";
      gddlReq.ComboBoxControl.DataTextFormatString = "descripcion";
      gddlReq.DataSource = getSubCategoria(gddlContact.SelectedValue, string.Empty, cmbox_contrato.SelectedValue);
      gddlReq.DataBind();
      gddlReq.SelectedValue = "0";

    }

    protected DataTable getMarca(string pCodMarca, string pCodContrato)
    {
      DataTable dtMarca = null;
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cMarcas oMarcas = new cMarcas(ref oConn);
        oMarcas.CodMarca = pCodMarca;
        oMarcas.CodContrato = pCodContrato;
        dtMarca = oMarcas.GetByContrato();

      }
      oConn.Close();
      return dtMarca;
    }

    protected DataTable getMarca(string sDesripcion)
    {
      DataTable dtMarca = null;
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cMarcas oMarcas = new cMarcas(ref oConn);
        oMarcas.Descripcion = sDesripcion;
        dtMarca = oMarcas.GetByDescripcion();

      }
      oConn.Close();
      return dtMarca;
    }

    protected DataTable getCategoria(string pCodCategoria, string pCodContrato)
    {
      DataTable dtCategoria = null;
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cCategorias oCategorias = new cCategorias(ref oConn);
        oCategorias.CodContrato = pCodContrato;
        oCategorias.CodCategoria = pCodCategoria;
        dtCategoria = oCategorias.GetByCodContrato();

      }
      oConn.Close();
      return dtCategoria;
    }

    protected DataTable getCategoria(string sDescripcion)
    {
      DataTable dtCategoria = null;
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cCategorias oCategorias = new cCategorias(ref oConn);
        oCategorias.Descripcion = sDescripcion;
        dtCategoria = oCategorias.GetByDescripcion();

      }
      oConn.Close();
      return dtCategoria;
    }

    protected DataTable getSubCategoria(string pCodCategoria, string pCodSubCategoria, string pCodContrato)
    {
      DataTable dtSubCategoria = null;
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cSubCategoria oSubCategoria = new cSubCategoria(ref oConn);
        if (!string.IsNullOrEmpty(pCodCategoria))
        {
          oSubCategoria.CodCategoria = pCodCategoria;
          oSubCategoria.CodContrato = pCodContrato;
          dtSubCategoria = oSubCategoria.GetByContrato();
        } if (!string.IsNullOrEmpty(pCodSubCategoria))
        {
          oSubCategoria.CodSubCategoria = pCodSubCategoria;
          dtSubCategoria = oSubCategoria.Get();
        }
      }
      oConn.Close();
      return dtSubCategoria;
    }

    protected DataTable getSubCategoria(string sDescripcion)
    {
      DataTable dtSubCategoria = null;
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cSubCategoria oSubCategoria = new cSubCategoria(ref oConn);
        oSubCategoria.Descripcion = sDescripcion;
        dtSubCategoria = oSubCategoria.Get();
      }
      oConn.Close();
      return dtSubCategoria;
    }

    protected DataTable getRoyalty(string pCodRoyalty, string sDescripcion)
    {
      DataTable dtRoyalty = null;
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cTipoRoyalty oTipoRoyalty = new cTipoRoyalty(ref oConn);
        oTipoRoyalty.CodRoyalty = pCodRoyalty;
        oTipoRoyalty.Descripcion = sDescripcion;
        dtRoyalty = oTipoRoyalty.Get();

      }
      oConn.Close();
      return dtRoyalty;
    }

    protected void btnGuardar_Click(object sender, EventArgs e)
    {
      bool bExito = false;
      bool bCrear = false;
      if (Page.IsValid)
      {
        RadGrid grid = (this.FindControl("rdDetalleVenta") as RadGrid);

        DBConn oConn = new DBConn();
        if (oConn.Open())
        {
          if (string.IsNullOrEmpty(ViewState["cod_reporte_venta"].ToString()))
          {
            bCrear = true;
          }
          cReporteVenta oReporteVenta = new cReporteVenta(ref oConn);
          oReporteVenta.CodigoReporteVenta = ViewState["cod_reporte_venta"].ToString();
          oReporteVenta.NumContrato = cmbox_contrato.SelectedValue;
          oReporteVenta.MesReporte = ddlmesventa.SelectedValue;
          oReporteVenta.AnoReporte = ViewState["ano_reporte"].ToString();
          oReporteVenta.FechaReporte = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
          oReporteVenta.Facturado = "N";
          oReporteVenta.Periodo = oWeb.getPeriodoVenta(oWeb.getMes(int.Parse(ddlmesventa.SelectedValue)).ToUpper());
          oReporteVenta.EstReporte = "L";
          oReporteVenta.DeclaraMovimiento = (chkbox_nodeclaracion.Checked ? "N" : "S");
          oReporteVenta.Accion = (bCrear ? "CREAR" : "EDITAR");
          oReporteVenta.Put();

          ViewState["cod_reporte_venta"] = oReporteVenta.CodigoReporteVenta;

          cDetalleVenta oDetalleVenta;
          if (!chkbox_nodeclaracion.Checked)
          {
            GridDataItem item;
            for (int i = 0; i < grid.MasterTableView.Items.Count; i++)
            {
              if (grid.MasterTableView.Items[i] is GridDataItem)
              {
                item = (GridDataItem)grid.MasterTableView.Items[i];
                string pCodDetalle = (bCrear ? string.Empty : grid.MasterTableView.Items[i].GetDataKeyValue("codigo_detalle").ToString());
                oDetalleVenta = new cDetalleVenta(ref oConn);
                oDetalleVenta.CodigoReporteVenta = ViewState["cod_reporte_venta"].ToString();
                oDetalleVenta.CodigoDetalle = pCodDetalle;

                string pCodMarca = string.Empty;
                if (!string.IsNullOrEmpty((item.FindControl("lblMarca") as Label).Text))
                {
                  DataTable dtMarca = getMarca((item.FindControl("lblMarca") as Label).Text);
                  if (dtMarca != null)
                  {
                    if (dtMarca.Rows.Count > 0)
                    {
                      pCodMarca = dtMarca.Rows[0]["cod_marca"].ToString();
                    }
                  }
                  dtMarca = null;
                }

                string pCodCategoria = string.Empty;
                if (!string.IsNullOrEmpty((item.FindControl("lblCategoria") as Label).Text))
                {
                  DataTable dtCategoria = getCategoria((item.FindControl("lblCategoria") as Label).Text);
                  if (dtCategoria != null)
                  {
                    if (dtCategoria.Rows.Count > 0)
                    {
                      pCodCategoria = dtCategoria.Rows[0]["cod_categoria"].ToString();
                    }
                  }
                  dtCategoria = null;
                }

                string pCodSubCategoria = string.Empty;
                if (!string.IsNullOrEmpty((item.FindControl("lblSubCategoria") as Label).Text))
                {
                  DataTable dtSubCategoria = getSubCategoria((item.FindControl("lblSubCategoria") as Label).Text);
                  if (dtSubCategoria != null)
                  {
                    if (dtSubCategoria.Rows.Count > 0)
                    {
                      pCodSubCategoria = dtSubCategoria.Rows[0]["cod_subcategoria"].ToString();
                    }
                  }
                  dtSubCategoria = null;
                }

                if (!string.IsNullOrEmpty(pCodMarca))
                  oDetalleVenta.Marca = pCodMarca;
                if (!string.IsNullOrEmpty(pCodCategoria))
                  oDetalleVenta.Categoria = pCodCategoria;
                if (!string.IsNullOrEmpty(pCodSubCategoria))
                  oDetalleVenta.SubCategoria = pCodSubCategoria;

                oDetalleVenta.Producto = item["producto"].Text;
                oDetalleVenta.CodRoyalty = getRoyalty(string.Empty, item["descroyalty"].Text).Rows[0]["cod_royalty"].ToString();
                oDetalleVenta.Royalty = (double.Parse(item["royalty"].Text.Replace("%", "").Trim()) / 100).ToString();
                oDetalleVenta.Bdi = (double.Parse(item["bdi"].Text.Replace("%", "").Trim()) / 100).ToString();
                oDetalleVenta.Cliente = ((item["cliente"].Text == "&nbsp") ? string.Empty : item["cliente"].Text);
                oDetalleVenta.Sku = ((item["sku"].Text == "&nbsp") ? string.Empty : item["sku"].Text);
                oDetalleVenta.DescripcionProducto = item["descripcion_producto"].Text;
                oDetalleVenta.PrecioUnitarioVentaBruta = ((RadTextBox)item.FindControl("txt_lbl_PrecioUniBruto")).Text;
                oDetalleVenta.CantidadVentaBruta = ((RadTextBox)item.FindControl("txt_lblCantidadVentaBruta")).Text;
                oDetalleVenta.PrecioUnitDescueDevol = ((RadTextBox)item.FindControl("txt_lbl_PrecioUnitDescueDevol")).Text;
                oDetalleVenta.CantidadDescueDevol = ((RadTextBox)item.FindControl("txt_lblCantidadDescueDevol")).Text;
                oDetalleVenta.Accion = (bCrear ? "CREAR" : "EDITAR");
                oDetalleVenta.Put();
              }
            }
          }
          else
          {
            cRoyaltyContrato oRoyaltyContrato = new cRoyaltyContrato(ref oConn);
            oRoyaltyContrato.NumContrato = cmbox_contrato.SelectedValue;
            DataTable dtProdCont = oRoyaltyContrato.GetByInvoce();

            if (dtProdCont != null)
            {
              if (dtProdCont.Rows.Count > 0)
              {
                foreach (DataRow oRow in dtProdCont.Rows)
                {
                  oDetalleVenta = new cDetalleVenta(ref oConn);
                  oDetalleVenta.CodigoReporteVenta = ViewState["cod_reporte_venta"].ToString();
                  oDetalleVenta.Marca = (!string.IsNullOrEmpty(oRow["cod_marca"].ToString())? oRow["cod_marca"].ToString() : null);
                  oDetalleVenta.Categoria = (!string.IsNullOrEmpty(oRow["cod_categoria"].ToString()) ? oRow["cod_categoria"].ToString() : null);
                  oDetalleVenta.SubCategoria = (!string.IsNullOrEmpty(oRow["cod_subcategoria"].ToString()) ? oRow["cod_subcategoria"].ToString() : null);
                  oDetalleVenta.Producto = string.Empty;
                  oDetalleVenta.CodRoyalty = oRow["cod_royalty"].ToString();
                  oDetalleVenta.Royalty = oRow["porcentaje"].ToString();


                  cBdiContrato oBdiContrato = new cBdiContrato(ref oConn);
                  oBdiContrato.NumContrato = cmbox_contrato.SelectedValue;
                  oBdiContrato.CodMarca = oRow["cod_marca"].ToString();
                  oBdiContrato.CodCategoria = oRow["cod_categoria"].ToString();
                  oBdiContrato.CodSubCategoria = oRow["cod_subcategoria"].ToString();
                  DataTable dtDBI = oBdiContrato.Get();
                  if (dtDBI != null) {
                    if (dtDBI.Rows.Count > 0)
                    {
                      oDetalleVenta.Bdi = dtDBI.Rows[0]["porcentaje"].ToString();
                    }
                    else {
                      oDetalleVenta.Bdi = "0.0%";
                    }
                  }
                  dtDBI = null;
                  
                  oDetalleVenta.Cliente = string.Empty;
                  oDetalleVenta.Sku = string.Empty;
                  oDetalleVenta.DescripcionProducto = string.Empty;
                  oDetalleVenta.PrecioUnitarioVentaBruta = "0";
                  oDetalleVenta.CantidadVentaBruta = "0";
                  oDetalleVenta.PrecioUnitDescueDevol = "0";
                  oDetalleVenta.CantidadDescueDevol = "0";
                  oDetalleVenta.Accion = "CREAR";
                  oDetalleVenta.Put();
                }
              }
            }
            dtProdCont = null;

          }
          bExito = true;
          chkbox_declaracion.Checked = false;
          chkbox_nodeclaracion.Checked = false;
        }
        oConn.Close();

        rdDetalleVenta.Rebind();
      }

      if (bExito)
      {
        StringBuilder scriptstring = new StringBuilder();
        scriptstring.Append(" window.radalert('El reporte de venta ha sido grabado con éxito.', 330, 210); ");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "radalert", scriptstring.ToString(), true);
      }
    }

    protected void rdDetalleVenta_InsertCommand(object source, GridCommandEventArgs e)
    {
      if (e.CommandName == RadGrid.PerformInsertCommandName)
      {
        GridEditFormInsertItem item = (GridEditFormInsertItem)e.Item;

        DBConn oConn = new DBConn();
        if (oConn.Open())
        {
          if (string.IsNullOrEmpty(ViewState["cod_reporte_venta"].ToString()))
          {
            cReporteVenta oReporteVenta = new cReporteVenta(ref oConn);
            oReporteVenta.NumContrato = cmbox_contrato.SelectedValue;
            oReporteVenta.MesReporte = ddlmesventa.SelectedValue;
            oReporteVenta.AnoReporte = ViewState["ano_reporte"].ToString();
            oReporteVenta.FechaReporte = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            oReporteVenta.Facturado = "N";
            oReporteVenta.Periodo = oWeb.getPeriodoVenta(oWeb.getMes(int.Parse(ddlmesventa.SelectedValue)).ToUpper());
            oReporteVenta.EstReporte = "P";
            oReporteVenta.DeclaraMovimiento = "S";
            oReporteVenta.Accion = "CREAR";
            oReporteVenta.Put();

            ViewState["cod_reporte_venta"] = oReporteVenta.CodigoReporteVenta;
          }

          string pCodMarca = string.Empty;
          string pCodCategoria = string.Empty;
          string pCodSubCategoria = string.Empty;

          if ((item["marca"].Controls[1] as RadComboBox).SelectedValue != "0")
          {
            DataTable dtMarca = getMarca((item["marca"].Controls[1] as RadComboBox).SelectedValue, cmbox_contrato.SelectedValue);
            if (dtMarca != null)
            {
              if (dtMarca.Rows.Count > 0)
              {
                pCodMarca = dtMarca.Rows[0]["cod_marca"].ToString();
              }
            }
            dtMarca = null;
          }

          if ((!string.IsNullOrEmpty((item["categoria"].Controls[1] as RadComboBox).SelectedValue)) && ((item["categoria"].Controls[1] as RadComboBox).SelectedValue != "0"))
          {
            DataTable dtCategoria = getCategoria((item["categoria"].Controls[1] as RadComboBox).SelectedValue, cmbox_contrato.SelectedValue);
            if (dtCategoria != null)
            {
              if (dtCategoria.Rows.Count > 0)
              {
                pCodCategoria = dtCategoria.Rows[0]["cod_categoria"].ToString();
              }
            }
            dtCategoria = null;
          }

          if ((!string.IsNullOrEmpty((item["subcategoria"].Controls[1] as RadComboBox).SelectedValue)) && ((item["subcategoria"].Controls[1] as RadComboBox).SelectedValue != "0"))
          {
            DataTable dtSubCategoria = getSubCategoria(string.Empty, (item["subcategoria"].Controls[1] as RadComboBox).SelectedValue, cmbox_contrato.SelectedValue);
            if (dtSubCategoria != null)
            {
              if (dtSubCategoria.Rows.Count > 0)
              {
                pCodSubCategoria = dtSubCategoria.Rows[0]["cod_subcategoria"].ToString();
              }
            }
            dtSubCategoria = null;
          }

          cDetalleVenta oDetalleVenta = new cDetalleVenta(ref oConn);
          oDetalleVenta.CodigoReporteVenta = ViewState["cod_reporte_venta"].ToString();
          oDetalleVenta.Marca = pCodMarca;
          oDetalleVenta.Categoria = (!string.IsNullOrEmpty(pCodCategoria) ? pCodCategoria : null); ;
          oDetalleVenta.SubCategoria = (!string.IsNullOrEmpty(pCodSubCategoria) ? pCodSubCategoria : null);
          oDetalleVenta.Producto = (item["producto"].Controls[0] as TextBox).Text;

          cRoyaltyContrato oRoyaltyContrato = new cRoyaltyContrato(ref oConn);
          oRoyaltyContrato.NumContrato = cmbox_contrato.SelectedValue;
          oRoyaltyContrato.CodMarca = pCodMarca;
          oRoyaltyContrato.CodCategoria = pCodCategoria;
          oRoyaltyContrato.CodSubCategoria = pCodSubCategoria;
          DataTable dtRoyaltyContrato = oRoyaltyContrato.Get();
          oDetalleVenta.CodRoyalty = dtRoyaltyContrato.Rows[0]["cod_royalty"].ToString();
          oDetalleVenta.Royalty = dtRoyaltyContrato.Rows[0]["porcentaje"].ToString();
          dtRoyaltyContrato = null;

          cBdiContrato oBdiContrato = new cBdiContrato(ref oConn);
          oBdiContrato.NumContrato = cmbox_contrato.SelectedValue;
          oBdiContrato.CodMarca = pCodMarca;
          oBdiContrato.CodCategoria = pCodCategoria;
          oBdiContrato.CodSubCategoria = pCodSubCategoria;
          DataTable dtBdiContrato = oBdiContrato.Get();
          oDetalleVenta.Bdi = dtBdiContrato.Rows[0]["porcentaje"].ToString();
          dtBdiContrato = null;

          oDetalleVenta.Cliente = (item["cliente"].Controls[0] as TextBox).Text;
          oDetalleVenta.Sku = (item["sku"].Controls[0] as TextBox).Text;
          oDetalleVenta.DescripcionProducto = (item["descripcion_producto"].Controls[0] as TextBox).Text;
          oDetalleVenta.PrecioUnitarioVentaBruta = (item["precio_uni_venta_bruta"].FindControl("txt1") as RadTextBox).Text;
          oDetalleVenta.CantidadVentaBruta = (item["cantidad_venta_bruta"].FindControl("txt2") as RadTextBox).Text;
          oDetalleVenta.PrecioUnitDescueDevol = (item["precio_unit_descue_devol"].FindControl("txt3") as RadTextBox).Text;
          oDetalleVenta.CantidadDescueDevol = (item["cantidad_descue_devol"].FindControl("txt4") as RadTextBox).Text;
          oDetalleVenta.Accion = "CREAR";
          oDetalleVenta.Put();

        }
        oConn.Close();

        rdDetalleVenta.Rebind();
      }
    }

    protected void rdDetalleVenta_UpdateCommand(object source, GridCommandEventArgs e)
    {

    }

    protected void rdDetalleVenta_DeleteCommand(object source, GridCommandEventArgs e)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cDetalleVenta oDetalleVenta = new cDetalleVenta(ref oConn);
        oDetalleVenta.CodigoDetalle = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["codigo_detalle"].ToString();
        oDetalleVenta.Accion = "ELIMINAR";
        oDetalleVenta.Put();
      }
      oConn.Close();
      rdDetalleVenta.Rebind();
    }

    protected void rdDetalleVenta_PreRender(object sender, EventArgs e)
    {
      foreach (GridColumn col in rdDetalleVenta.MasterTableView.Columns)
      {
        if (col.UniqueName == "EditCommandColumn")
          col.Visible = false;
      }

    }

    protected void cmb_marca_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {
      GridEditableItem gefItem = (o as RadComboBox).NamingContainer as GridEditableItem;

      RadComboBox oCmbMarca = ((RadComboBox)(gefItem.EditManager.GetColumnEditor("marca")).ContainerControl.FindControl("cmb_marca"));
      oCmbMarca.DataTextField = "descripcion";
      oCmbMarca.DataValueField = "cod_marca";
      oCmbMarca.DataSource = getMarca(string.Empty, cmbox_contrato.SelectedValue);
      oCmbMarca.DataBind();
      oCmbMarca.Items.Add(new RadComboBoxItem("<< Seleccione Marca >>", "0"));
      oCmbMarca.SelectedValue = "0";

    }

    protected void cmb_categoria_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {
      GridEditableItem gefItem = (o as RadComboBox).NamingContainer as GridEditableItem;
      RadComboBox oCmbCategoria = ((RadComboBox)(gefItem.EditManager.GetColumnEditor("categoria")).ContainerControl.FindControl("cmb_categoria"));
      oCmbCategoria.DataTextField = "descripcion";
      oCmbCategoria.DataValueField = "cod_categoria";
      oCmbCategoria.DataSource = getCategoria(string.Empty, cmbox_contrato.SelectedValue);
      oCmbCategoria.DataBind();
      oCmbCategoria.Items.Add(new RadComboBoxItem("<< Todas las Categorias >>", "0"));
      oCmbCategoria.SelectedValue = "0";
    }

    protected void cmb_subcategoria_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {
      GridEditFormInsertItem gefItem = (o as RadComboBox).NamingContainer as GridEditFormInsertItem;

      RadComboBox oCmbCategoria = gefItem["categoria"].Controls[1] as RadComboBox;
      RadComboBox oCmbSubCategoria = gefItem["subcategoria"].Controls[1] as RadComboBox;
      oCmbSubCategoria.DataTextField = "descripcion";
      oCmbSubCategoria.DataValueField = "cod_subcategoria";
      oCmbSubCategoria.DataSource = getSubCategoria(e.Context["Text"].ToString(), string.Empty, cmbox_contrato.SelectedValue);
      oCmbSubCategoria.DataBind();
      oCmbSubCategoria.Items.Add(new RadComboBoxItem("<< Todas las SubCategorias >>", "0"));
      oCmbSubCategoria.SelectedValue = "0";


    }

    protected void ImgBtn_Click(object sender, ImageClickEventArgs e)
    {
      //if (!string.IsNullOrEmpty(cmbox_contrato.SelectedValue))
      //{
      //  Response.Redirect("DownloadGrid.ashx?pCodContrato=" + cmbox_contrato.SelectedValue);
      //}
      ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Download", "GotoDownloadPage(" + cmbox_contrato.SelectedValue + ");", true);
      //ScriptManager.RegisterStartupScript(this, this.GetType(), "radalert", scriptstring.ToString(), true);
    }

    protected void btnLoadExcel_Click(object sender, EventArgs e)
    {
      try
      {
        System.Threading.Thread.Sleep(2000);
        string FileName = string.Empty;
        string FilePath = string.Empty;
        string Extension = string.Empty;
        if (RadAsyncUpload1.UploadedFiles.Count > 0)
        {
          foreach (UploadedFile ofile in RadAsyncUpload1.UploadedFiles)
          {
            FileName = ofile.GetName();
            Extension = ofile.GetExtension();
            FilePath = Server.MapPath("UploadTemp/") + FileName;

            //ofile.SaveAs(FilePath);

          }
        }
        Import_To_Grid(FilePath, Extension);
      }
      catch
      {
        StringBuilder scriptstring = new StringBuilder();
        scriptstring.Append(" window.radalert('Ha habido un problema en la carga, por favor verifique el archivo y vuelva a intentarlo.', 330, 210); ");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "radalert", scriptstring.ToString(), true);
      }
    }

    private void Import_To_Grid(string FilePath, string Extension)
    {
      int iLine = 0;
      int iColumn = 0;
      string sDataError = string.Empty;
      try
      {
        bool bExito = true;
        DataTable dt = new DataTable();
        using (StreamReader sr = new StreamReader(FilePath))
        {
          sDataError = "Error en campos de encabezado del archivo.[br]";
          string[] headers = sr.ReadLine().Split(';');
          foreach (string header in headers)
          {
            iLine = 1;
            dt.Columns.Add(header.Trim());
            iLine++;
          }
          sDataError = "Error en campos de datos del archivo.[br]";
          while (!sr.EndOfStream)
          {
            iLine = 1;
            string[] rows = sr.ReadLine().Split(';');
            DataRow dr = dt.NewRow();
            for (int i = 0; i < headers.Length; i++)
            {
              iColumn = 1;
              dr[i] = rows[i].Trim();
              iColumn++;
            }
            dt.Rows.Add(dr);
            iLine++;
          }

        }
        sDataError = string.Empty;
        DBConn oConn = new DBConn();
        if (oConn.Open())
        {
          cDetalleVenta oDetalleVenta;
          if (string.IsNullOrEmpty(ViewState["cod_reporte_venta"].ToString()))
          {
            cReporteVenta oReporteVenta = new cReporteVenta(ref oConn);
            oReporteVenta.NumContrato = cmbox_contrato.SelectedValue;
            oReporteVenta.MesReporte = ddlmesventa.SelectedValue;
            oReporteVenta.AnoReporte = ViewState["ano_reporte"].ToString();
            oReporteVenta.FechaReporte = DateTime.Now.ToString("yyyy-MM-dd");
            oReporteVenta.Facturado = "N";
            oReporteVenta.Periodo = oWeb.getPeriodoVenta(oWeb.getMes(int.Parse(ddlmesventa.SelectedValue)).ToUpper());
            oReporteVenta.EstReporte = "P";
            oReporteVenta.Accion = "CREAR";
            oReporteVenta.Put();

            if (!string.IsNullOrEmpty(oReporteVenta.Error))
              bExito = false;
            else
              ViewState["cod_reporte_venta"] = oReporteVenta.CodigoReporteVenta;
          }
          else
          {
            oDetalleVenta = new cDetalleVenta(ref oConn);
            oDetalleVenta.Accion = "ELIMINAR";
            oDetalleVenta.CodigoReporteVenta = ViewState["cod_reporte_venta"].ToString();
            oDetalleVenta.Put();
          }

          if (bExito)
          {
            iLine = 2;
            //sDataError = string.Empty;
            foreach (DataRow odtRow in dt.Rows)
            {
              //if (bExito)
              string pCodMarca = string.Empty;
              string pCodCategoria = string.Empty;
              string pCodSubCategoria = string.Empty;

              //if (bExito)
              //  sDataError = "columna Marca";
              if (!string.IsNullOrEmpty(odtRow["Marca"].ToString()))
              {
                DataTable dtMarca = getMarca(odtRow["Marca"].ToString());
                if (dtMarca != null)
                {
                  if (dtMarca.Rows.Count > 0)
                  {
                    pCodMarca = dtMarca.Rows[0]["cod_marca"].ToString();
                  }
                }
                dtMarca = null;
              }

              //if (bExito)
              //  sDataError = "columna Categoria";
              if (!string.IsNullOrEmpty(odtRow["Categoria"].ToString()))
              {
                DataTable dtCategoria = getCategoria(odtRow["Categoria"].ToString());
                if (dtCategoria != null)
                {
                  if (dtCategoria.Rows.Count > 0)
                  {
                    pCodCategoria = dtCategoria.Rows[0]["cod_categoria"].ToString();
                  }
                }
                dtCategoria = null;
              }

              //if (bExito)
              //  sDataError = "columna SubCategoria";
              if (!string.IsNullOrEmpty(odtRow["SubCategoria"].ToString()))
              {
                DataTable dtSubCategoria = getSubCategoria(odtRow["SubCategoria"].ToString());
                if (dtSubCategoria != null)
                {
                  if (dtSubCategoria.Rows.Count > 0)
                  {
                    pCodSubCategoria = dtSubCategoria.Rows[0]["cod_subcategoria"].ToString();
                  }
                }
                dtSubCategoria = null;
              }

              oDetalleVenta = new cDetalleVenta(ref oConn);
              oDetalleVenta.CodigoReporteVenta = ViewState["cod_reporte_venta"].ToString();
              oDetalleVenta.Marca = pCodMarca;
              oDetalleVenta.Categoria = (!string.IsNullOrEmpty(pCodCategoria) ? pCodCategoria : null);
              oDetalleVenta.SubCategoria = (!string.IsNullOrEmpty(pCodSubCategoria) ? pCodSubCategoria : null);
              //if (bExito)
              //  sDataError = "columna Producto";
              oDetalleVenta.Producto = odtRow["Producto"].ToString();

              cRoyaltyContrato oRoyaltyContrato = new cRoyaltyContrato(ref oConn);
              oRoyaltyContrato.NumContrato = cmbox_contrato.SelectedValue;
              oRoyaltyContrato.CodMarca = pCodMarca;
              oRoyaltyContrato.CodCategoria = pCodCategoria;
              oRoyaltyContrato.CodSubCategoria = pCodSubCategoria;
              //if (odtRow["Moneda"].ToString() == "USD")
              //  oRoyaltyContrato.CodRoyalty = "3";
              //else
              //  oRoyaltyContrato.NotUSD = true;
              //if (bExito)
              //  sDataError = "columna Desc. Royalty";
              if (odtRow["Desc. Royalty"].ToString().ToUpper() == "STANDARD")
                oRoyaltyContrato.CodRoyalty = "1";
              else if (odtRow["Desc. Royalty"].ToString().ToUpper() == "DIRECT TO CONSUMER")
                oRoyaltyContrato.CodRoyalty = "2";
              else if (odtRow["Desc. Royalty"].ToString().ToUpper() == "FOB")
                oRoyaltyContrato.CodRoyalty = "3";
              else if (odtRow["Desc. Royalty"].ToString().ToUpper() == "DISTRIBUITORS")
                oRoyaltyContrato.CodRoyalty = "4";

              //if (bExito)
              //  sDataError = "oRoyaltyContrato.Get() " + odtRow["Desc. Royalty"].ToString();
              DataTable dtRoyaltyContrato = oRoyaltyContrato.Get();
              if (dtRoyaltyContrato != null)
              {
                if (dtRoyaltyContrato.Rows.Count == 0)
                {
                  bExito = false;
                  oRoyaltyContrato.Marca = odtRow["Marca"].ToString();
                  oRoyaltyContrato.AsqMarcaNull = true;
                  DataTable dtValMarca = oRoyaltyContrato.GetValData();
                  if (dtValMarca != null)
                  {
                    if (dtValMarca.Rows.Count == 0)
                    {
                      if (!string.IsNullOrEmpty(odtRow["Marca"].ToString()))
                        sDataError = sDataError + "Error en la linea " + iLine + ".La marca no corresponde al contrato.[br]";
                      else
                        sDataError = sDataError + "Error en la linea " + iLine + ".La marca esta en blanco.[br]";
                    }
                    else
                    {
                      oRoyaltyContrato.Categoria = odtRow["Categoria"].ToString();
                      oRoyaltyContrato.AsqCategoriaNull = true;
                      DataTable dtValCategoria = oRoyaltyContrato.GetValData();
                      if (dtValCategoria != null)
                      {
                        if (dtValCategoria.Rows.Count == 0)
                        {
                          if (!string.IsNullOrEmpty(odtRow["Categoria"].ToString()))
                            sDataError = sDataError + "Error en la linea " + iLine + ".La categoria no corresponde al contrato.[br]";
                          else
                            sDataError = sDataError + "Error en la linea " + iLine + ".La categoria esta en blanco.[br]";
                        }
                        else
                        {
                          oRoyaltyContrato.SubCategoria = odtRow["SubCategoria"].ToString();
                          oRoyaltyContrato.AsqSubCategoriaNull = true;
                          DataTable dtValSubCategoria = oRoyaltyContrato.Get();
                          if (dtValSubCategoria != null)
                          {
                            if (dtValSubCategoria.Rows.Count == 0) {
                              if (!string.IsNullOrEmpty(odtRow["SubCategoria"].ToString()))
                                sDataError = sDataError + "Error en la linea " + iLine + ".La subcategoria no corresponde al contrato.[br]";
                              else
                                sDataError = sDataError + "Error en la linea " + iLine + ".La subcategoria esta en blanco.[br]";
                            }
                          }
                          dtValSubCategoria = null;
                        }
                      }
                      dtValCategoria = null;
                    }
                  }
                  dtValMarca = null;

                  //sDataError = "Error al tratar de asignar el royalty según los datos ingresados. El error se encuentra en los campos asociados a los datos de Marca, Categoría, Subcategoría o Desc. Royalty.";
                }
                else {
                  oDetalleVenta.CodRoyalty = dtRoyaltyContrato.Rows[0]["cod_royalty"].ToString();
                  oDetalleVenta.Royalty = dtRoyaltyContrato.Rows[0]["porcentaje"].ToString();
                }
              }
              dtRoyaltyContrato = null;

              //if (bExito)
              //  sDataError = "oBdiContrato.Get()";
              cBdiContrato oBdiContrato = new cBdiContrato(ref oConn);
              oBdiContrato.NumContrato = cmbox_contrato.SelectedValue;
              oBdiContrato.CodMarca = pCodMarca;
              oBdiContrato.CodCategoria = pCodCategoria;
              oBdiContrato.CodSubCategoria = pCodSubCategoria;
              DataTable dtBdiContrato = oBdiContrato.Get();
              if (dtBdiContrato != null)
              {
                if (dtBdiContrato.Rows.Count == 0)
                {
                  bExito = false;
                  sDataError = sDataError + "Error en la linea " + iLine + ".Dado que no hay Marca y/o Categoría correctas no se puede calcular BDI y/o Royalty.[br]";
                }
                else {
                  oDetalleVenta.Bdi = dtBdiContrato.Rows[0]["porcentaje"].ToString();
                }
              }
              dtBdiContrato = null;

              //sDataError = "columna Cliente";
              oDetalleVenta.Cliente = odtRow["Cliente"].ToString();

              //sDataError = "columna SKU";
              oDetalleVenta.Sku = odtRow["SKU"].ToString();

              //sDataError = "error  Descripcion Producto";
              oDetalleVenta.DescripcionProducto = odtRow["Descripcion Producto"].ToString();

              if (string.IsNullOrEmpty(odtRow["Precio Unitario Venta Neta"].ToString()))
              {
                bExito = false;
                sDataError = sDataError + "Error en la linea " + iLine + ".Precio Unitario Venta Neta, viene vacio desde la planilla.[br]";
              }
              else
              {
                if (!IsNumber(odtRow["Precio Unitario Venta Neta"].ToString()))
                {
                  bExito = false;
                  sDataError = sDataError + "Error en la linea " + iLine + ".Precio Unitario Venta Neta, No es un numero.[br]";
                }
                else
                {
                  oDetalleVenta.PrecioUnitarioVentaBruta = odtRow["Precio Unitario Venta Neta"].ToString();
                }
              }

              if (string.IsNullOrEmpty(odtRow["Q Venta Neta"].ToString()))
              {
                bExito = false;
                sDataError = sDataError + "Error en la linea " + iLine + ".Q Venta Neta, viene vacio desde la planilla.[br]";
              }
              else
              {
                if (!IsNumber(odtRow["Q Venta Neta"].ToString()))
                {
                  bExito = false;
                  sDataError = sDataError + "Error en la linea " + iLine + ".Q Venta Neta, No es un numero.[br]";
                }
                else
                {
                  oDetalleVenta.CantidadVentaBruta = odtRow["Q Venta Neta"].ToString();
                }
              }

              if (string.IsNullOrEmpty(odtRow["Precio Unitario Devolucion / Descuento"].ToString()))
              {
                bExito = false;
                sDataError = sDataError + "Error en la linea " + iLine + ".Precio Unitario Devolucion / Descuento, viene vacio desde la planilla.[br]";
              }
              else
              {
                if (!IsNumber(odtRow["Precio Unitario Devolucion / Descuento"].ToString()))
                {
                  bExito = false;
                  sDataError = sDataError + "Error en la linea " + iLine + ".Precio Unitario Devolucion / Descuento, No es un numero.[br]";
                }
                else
                {
                  oDetalleVenta.PrecioUnitDescueDevol = odtRow["Precio Unitario Devolucion / Descuento"].ToString();
                }
              }

              if (string.IsNullOrEmpty(odtRow["Q Devolucion"].ToString()))
              {
                bExito = false;
                sDataError = sDataError + "Error en la linea " + iLine + ".Q Devolucion, viene vacio desde la planilla.[br]";
              }
              else
              {
                if (!IsNumber(odtRow["Q Devolucion"].ToString()))
                {
                  bExito = false;
                  sDataError = sDataError + "Error en la linea " + iLine + ".Q Devolucion, No es un numero.[br]";
                }
                else
                {
                  oDetalleVenta.CantidadDescueDevol = odtRow["Q Devolucion"].ToString();
                }
              }
              
              oDetalleVenta.Accion = "CREAR";
              oDetalleVenta.Put();

              if (!string.IsNullOrEmpty(oDetalleVenta.Error))
              {
                //sDataError = sDataError + "Error de sistema : No se pudo cargar la linea " + iLine + ". " + oDetalleVenta.Error + "[br]";
                bExito = false;
                sDataError = sDataError + "[br]";
              }

              oDetalleVenta = null;
              //if (bExito)
                iLine++;
            }
          }
        }
        oConn.Close();


        if (bExito)
        {
          StringBuilder scriptstring = new StringBuilder();
          scriptstring.Append(" window.radalert('El archivo ha sido cargado con éxito.', 330, 210); ");
          ScriptManager.RegisterStartupScript(this, this.GetType(), "radalert", scriptstring.ToString(), true);
        }
        else
        {
          if (!string.IsNullOrEmpty(ViewState["cod_reporte_venta"].ToString()))
          {
            if (oConn.Open())
            {
              cDetalleVenta oDetalleVenta = new cDetalleVenta(ref oConn);
              oDetalleVenta.CodigoReporteVenta = ViewState["cod_reporte_venta"].ToString();
              oDetalleVenta.Accion = "ELIMINAR";
              oDetalleVenta.Put();

              cReporteVenta oReporteVenta = new cReporteVenta(ref oConn);
              oReporteVenta.CodigoReporteVenta = ViewState["cod_reporte_venta"].ToString();
              oReporteVenta.Accion = "ELIMINAR";
              oReporteVenta.Put();
            }
            oConn.Close();
          }
          Session["Error"] = sDataError;
          Response.Redirect("error_ingreso_venta.aspx?sTypeError=1");
          //Response.Redirect("error_ingreso_venta.aspx?sTypeError=1&sDataError=" + sDataError);
          //StringBuilder scriptstring = new StringBuilder();
          //scriptstring.Append(" window.radalert('Archivo no cargado. Posibles problemas : El formato del archivo no es el que corresponde. Los datos ingresados no corresponden al contrato.', 330, 210); ");
          //ScriptManager.RegisterStartupScript(this, this.GetType(), "radalert", scriptstring.ToString(), true);
        }
      }
      catch (Exception e)
      {
        if (!string.IsNullOrEmpty(ViewState["cod_reporte_venta"].ToString()))
        {
          DBConn oConn = new DBConn();
          if (oConn.Open())
          {
            cDetalleVenta oDetalleVenta = new cDetalleVenta(ref oConn);
            oDetalleVenta.CodigoReporteVenta = ViewState["cod_reporte_venta"].ToString();
            oDetalleVenta.Accion = "ELIMINAR";
            oDetalleVenta.Put();

            cReporteVenta oReporteVenta = new cReporteVenta(ref oConn);
            oReporteVenta.CodigoReporteVenta = ViewState["cod_reporte_venta"].ToString();
            oReporteVenta.Accion = "ELIMINAR";
            oReporteVenta.Put();
          }
          oConn.Close();
        }
        Session["Error"] = sDataError;
        Response.Redirect("error_ingreso_venta.aspx?sTypeError=2");
        //Response.Redirect("error_ingreso_venta.aspx?sTypeError=2&sDataError=" + sDataError);
        //StringBuilder scriptstring = new StringBuilder();
        //scriptstring.Append(" window.radalert('Ha habido un problema en la carga, por favor verifique el archivo y vuelva a intentarlo.', 330, 210); ");
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "radalert", scriptstring.ToString(), true);
      }
      rdDetalleVenta.Rebind();
    }

    protected void btnContacto_Click(object sender, EventArgs e)
    {
      Response.Redirect("contacto.aspx");
    }

    protected void btnInstructivo_Click(object sender, EventArgs e)
    {
      Response.Redirect("downloadmanual.ashx");
    }

    bool IsNumber(string text)
    {
      Regex regex = new Regex(@"^[-+]?[0-9]*\,?[0-9]+$");
      //Regex regex = new Regex(@"^-?[0-9]+([,\.][0-9]*)?$");
      //Regex regex = new Regex(@"^d+$|^d+,d+$");
      return regex.IsMatch(text);
    }

  }
}