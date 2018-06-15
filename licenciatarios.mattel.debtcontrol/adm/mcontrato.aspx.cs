using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using DebtControl.Conn;
using DebtControl.Method;
using DebtControl.Model;

namespace licenciatarios.mattel.debtcontrol.adm
{
  public partial class mcontrato : System.Web.UI.Page
  {
    Web oWeb = new Web();
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        num_contrato.Value = oWeb.GetData("numcontrato");

        DBConn oConn = new DBConn();
        if (oConn.Open())
        {
          cContratos oContratos = new cContratos(ref oConn);
          oContratos.NumContrato = num_contrato.Value;
          DataTable dtContrato = oContratos.GetWithDeudor();
          if (dtContrato != null)
          {
            if (dtContrato.Rows.Count > 0)
            {
              lblLicenciatario.Text = dtContrato.Rows[0]["licenciatario"].ToString();
              lblNoContrato.Text = dtContrato.Rows[0]["no_contrato"].ToString();
              no_contrato.Value = dtContrato.Rows[0]["no_contrato"].ToString();
              lblTipoContrato.Text = ((dtContrato.Rows[0]["tipo_contrato"].ToString() == "F") ? "FULL" : "DRAFT");
              lblFechaInicio.Text = DateTime.Parse(dtContrato.Rows[0]["fech_inicio"].ToString()).ToString("dd-MM-yyyy");
              lblFechaTermino.Text = DateTime.Parse(dtContrato.Rows[0]["fech_termino"].ToString()).ToString("dd-MM-yyyy");
              lblEstado.Text = ((dtContrato.Rows[0]["aprobado"].ToString() == "S") ? "Aprobado" : ((dtContrato.Rows[0]["aprobado"].ToString() == "T")? "Terminado" : "No Aprobado"));

              if (dtContrato.Rows[0]["aprobado"].ToString() == "N")
              {
                divTerminar.Visible = false;
                divAbrir.Visible = false;
              }

              if (dtContrato.Rows[0]["aprobado"].ToString() == "S")
              {
                divAprobar.Visible = false;
                divAbrir.Visible = false;
                divTerminar.Visible = true;
              }

              if (dtContrato.Rows[0]["aprobado"].ToString() == "T")
              {
                divAprobar.Visible = false;
                divTerminar.Visible = false;
                divAbrir.Visible = true;
              }

              if (!string.IsNullOrEmpty(dtContrato.Rows[0]["pdfcontrato"].ToString()))
              {
                PdfContrato.Visible = true;
                hddpdfcontrato.Value = dtContrato.Rows[0]["pdfcontrato"].ToString();
              }
              else
                NoPdfContrato.Visible = true;
            }
          }
          dtContrato = null;
        }
        oConn.Close();
      }
    }

    protected void rdAdvance_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cAdvanceContrato AdvanceContrato = new cAdvanceContrato(ref oConn);
        AdvanceContrato.NumContrato = num_contrato.Value;
        rdAdvance.DataSource = AdvanceContrato.GetForInvoce();
      }
      oConn.Close();
    }

    protected void rdRoyaltyBDI_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cRoyaltyContrato RoyaltyContrato = new cRoyaltyContrato(ref oConn);
        RoyaltyContrato.NumContrato = num_contrato.Value;
        rdRoyaltyBDI.DataSource = RoyaltyContrato.GetByExcel();
      }
      oConn.Close();
    }

    protected void rdMinimo_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cMinimoContrato MinimoContrato = new cMinimoContrato(ref oConn);
        MinimoContrato.NumContrato = num_contrato.Value;
        MinimoContrato.bOrder = true;
        rdMinimo.DataSource = MinimoContrato.Get();
      }
      oConn.Close();
    }

    protected void btnAprobar_Click(object sender, EventArgs e)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cContratos Contratos = new cContratos(ref oConn);
        Contratos.NumContrato = num_contrato.Value;
        Contratos.Aprobado = true;
        Contratos.Accion = "EDITAR";
        Contratos.Put();

        if (string.IsNullOrEmpty(Contratos.Error))
        {
          divAprobar.Visible = false;
          divAlert.Visible = true;
          lblEstado.Text = "Aprobado";
        }

      }
      oConn.Close();
    }

    protected void btnVolver_Click(object sender, EventArgs e)
    {
      Response.Redirect("madmcontratos.aspx");
    }

    protected void lnkButton_Click(object sender, EventArgs e)
    {
      string sPath = string.Empty;

      DBConn oConn = new DBConn();
      if (oConn.Open()) {
        cCliente oCliente = new cCliente(ref oConn);
        oCliente.NkeyCliente = "79";
        DataTable dt = oCliente.Get();
        if (dt != null) {
          sPath = dt.Rows[0]["pathsdocscaneados"].ToString();
        }
        dt = null;
      }
      oConn.Close();

      

      sPath = sPath + "Mattel Europa\\ContratosWeb\\" + hddpdfcontrato.Value;
      Response.AppendHeader("Content-Disposition", "attachment; filename=" + hddpdfcontrato.Value);

      // Write the file to the Response
      const int bufferLength = 10000;
      byte[] buffer = new Byte[bufferLength];
      int length = 0;
      Stream download = null;
      try
      {
        download = new FileStream(sPath, FileMode.Open, FileAccess.Read);
        do
        {
          if (Response.IsClientConnected)
          {
            length = download.Read(buffer, 0, bufferLength);
            Response.OutputStream.Write(buffer, 0, length);
            buffer = new Byte[bufferLength];
          }
          else
          {
            length = -1;
          }
        }
        while (length > 0);
        Response.Flush();
        Response.End();
      }
      finally
      {
        if (download != null)
          download.Close();
      }
    }

    protected void btnTerminar_Click(object sender, EventArgs e)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cContratos Contratos = new cContratos(ref oConn);
        Contratos.NumContrato = num_contrato.Value;
        Contratos.Terminado = true;
        Contratos.Accion = "EDITAR";
        Contratos.Put();

        if (string.IsNullOrEmpty(Contratos.Error))
        {
          divAprobar.Visible = false;
          divTerminar.Visible = false;
          divAlertTermino.Visible = true;
          lblEstado.Text = "Terminado";
        }

      }
      oConn.Close();
    }

    protected void btnAbrir_Click(object sender, EventArgs e)
    {
      DBConn oConn = new DBConn();
      if (oConn.Open())
      {
        cContratos Contratos = new cContratos(ref oConn);
        Contratos.NumContrato = num_contrato.Value;
        Contratos.Aprobado = true;
        Contratos.Accion = "EDITAR";
        Contratos.Put();

        if (string.IsNullOrEmpty(Contratos.Error))
        {
          divAprobar.Visible = false;
          divAbrir.Visible = false;
          divAlertAbrir.Visible = true;
          lblEstado.Text = "Aprobado";
        }

      }
      oConn.Close();
    }
  }
}