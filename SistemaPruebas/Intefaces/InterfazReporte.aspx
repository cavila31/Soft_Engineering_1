<%@ Page EnableEventValidation="false" Title="Generar Reportes" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InterfazReporte.aspx.cs" Inherits="SistemaPruebas.Intefaces.InterfazReporte" Async="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <legend style="margin-top: 45px">
        <h2>Generar Reportes</h2>
    </legend>

    <style type="text/css">
        .modalBackground {
            background-color: black;
            opacity: 0.5;
            filter: alpha(opacity=50);
        }

        .modalPopup {
  position: relative;
  background-color: #ffffff;
  border: 1px solid #999999;
  border: 1px solid transparent;
  border-radius: 5px;
  -webkit-background-clip: padding-box;
          background-clip: padding-box;
  outline: 0;
  padding: 45px;

   -webkit-box-shadow: 0 5px 15px rgba(0, 0, 0, 0.5);
    box-shadow: 0 5px 15px rgba(0, 0, 0, 0.5);
    width:460px;           
        }

                .errorDiv {
            display: none;
        }
    </style>

    <link rel="stylesheet" type="text/css" media="screen"
        href="http://tarruda.github.com/bootstrap-datetimepicker/assets/css/bootstrap-datetimepicker.min.css">
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css">
    <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script src="//code.jquery.com/ui/1.11.4/jquery-ui.js"></script>
    <link rel="stylesheet" href="/resources/demos/style.css">

    <script type="text/javascript">
        function HideLabel() {
            var seconds = 5;
            setTimeout(function () {
                $('#' + '<%=EtiqErrorGR.ClientID %>').fadeOut('5000');
            }, 2000);
        };
</script>

        <script type="text/javascript">
    $(document).ready(function () {
        $("[id$=A4]").addClass("active");
    });
</script>
    <asp:Label ID="EtiqErrorGR" runat="server" Text=" "></asp:Label>
    <div style="margin-top: 45px; margin-bottom: 20px" class="well">


         <legend><h5>Selección del origen</h5></legend>

                <div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-horizontal">
                                <asp:GridView ID="GridPP" runat="server" OnSelectedIndexChanged="PP_SelectedIndexChanged" OnPageIndexChanging="PP_OnPageIndexChanging"
                                    OnRowDataBound="PP_OnRowDataBound" CellPadding="10" margin-left="auto" CssClass="GridView" HorizontalAlign="Center" AllowRowSelect="true"
                                    AllowPaging="true" PageSize="5" HeaderStyle-BackColor="#48cfae" RowStyle-BackColor="White" PagerStyle-BackColor="White" HeaderStyle-ForeColor="#ffffff" BorderColor="#cdcdcd" border-radius="15px">
                                </asp:GridView>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <asp:GridView ID="GridMod" runat="server" OnSelectedIndexChanged="Mod_SelectedIndexChanged" OnPageIndexChanging="Mod_OnPageIndexChanging"
                                OnRowDataBound="Mod_OnRowDataBound" CellPadding="10" margin-left="auto" CssClass="GridView" HorizontalAlign="Center" AllowRowSelect="true"
                                AllowPaging="true" PageSize="5" HeaderStyle-BackColor="#48cfae" RowStyle-BackColor="White" PagerStyle-BackColor="White" HeaderStyle-ForeColor="#ffffff" BorderColor="#cdcdcd" border-radius="15px">
                            </asp:GridView>
                        </div>
                        <div class="col-md-4">
                            <asp:GridView ID="GridReq" runat="server" OnSelectedIndexChanged="Req_SelectedIndexChanged" OnPageIndexChanging="Req_OnPageIndexChanging"
                                OnRowDataBound="Req_OnRowDataBound" CellPadding="10" margin-left="auto" CssClass="GridView" HorizontalAlign="Center" AllowRowSelect="true"
                                AllowPaging="true" PageSize="5" HeaderStyle-BackColor="#48cfae" RowStyle-BackColor="White" PagerStyle-BackColor="White" HeaderStyle-ForeColor="#ffffff" BorderColor="#cdcdcd" border-radius="15px">
                            </asp:GridView>
                        </div>
                    </div>
                </div>



<div class="panel panel-primary" style="margin-top:20px">
  <div class="panel-heading">
    <h3 class="panel-title">Información General</h3>
  </div>
  <div class="panel-body">


           <div class ="form-group" style="margin-bottom:20px">
		   
		        <div class="col-md10">
                    <asp:Label ID="Label1" runat="server" Text=" "></asp:Label>
                </div>
		   
               <div class ="col-md-4">
			<asp:Label ID="proyectoSeleccionadoLabel" Font-Bold="true" runat="server" CssClass="control-label"  Text="Proyecto seleccionado: "></asp:Label>
                </div>
                <div class ="col-sm-8">
						<asp:Label ID="proyectoSeleccionado" runat="server" Text=""></asp:Label>
                </div>
						
               <div class ="col-md-4">
			<asp:Label ID="moduloSeleccionadoLabel" Font-Bold="true" runat="server" CssClass="control-label"  Text="Módulo seleccionado: "></asp:Label>
                </div>
                <div class ="col-sm-8">
					<asp:Label ID="modSeleccionado" runat="server" Text=""></asp:Label>
                </div>						
						
				 <div class ="col-md-4">
			<asp:Label ID="reqSeleccionadoLabel" Font-Bold="true" runat="server" CssClass="control-label"  Text="Requerimiento seleccionado: "></asp:Label>
                </div>
                <div class ="col-sm-8">
					<asp:Label ID="reqSeleccionado" runat="server" Text=""></asp:Label>
                </div>		
</div>		
      <br />
      <br />
      <br />
          <div class="row">
                <div class="col-md-4">
                    <div class="form-horizontal">
                        &nbsp;<asp:CheckBox ID="CheckBoxNombreProyecto" runat="server" Text="Nombre sistema."/>
                    </div>
                </div>
                <div class="col-md-4">
                    &nbsp;<asp:CheckBox ID="CheckBoxNombModulo" runat="server" Text="Nombre módulo."/>
                </div>
                <div class="col-md-4">
                    &nbsp;<asp:CheckBox ID="CheckBoxNombReq" runat="server" Text="Nombre requerimiento."/>
                </div>
            </div>

  </div>
</div>

        <div class="panel panel-primary">
            <div class="panel-heading">
                <h3 class="panel-title">Información sobre Proyecto</h3>
            </div>
            <div class="panel-body">
            <div class="col-md3">
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-horizontal">
                            &nbsp;<asp:CheckBox ID="CheckBoxFechAsignacionProyecto" runat="server" Text="Fecha de asignación." />
                        </div>
                    </div>
                    <div class="col-md-4">
                        &nbsp;<asp:CheckBox ID="CheckBoxOficinaProyecto" runat="server" Text="Datos de oficina usuaria." />
                    </div>
                    <div class="col-md-4">
                        &nbsp;<asp:CheckBox ID="CheckBoxResponsableProyecto" runat="server" Text="Líder." />
                    </div>
                </div>
            </div>

            <div class="col-md3">
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-horizontal">
                            &nbsp;<asp:CheckBox ID="CheckBoxObjetivoProyecto" runat="server" Text="Objetivo general." />
                        </div>
                    </div>
                    <div class="col-md-4">
                        &nbsp;<asp:CheckBox ID="CheckBoxEstadoProyecto" runat="server" Text="Estado" />
                    </div>
                    <div class="col-md-4">
                        &nbsp;<asp:CheckBox ID="CheckBoxMiembrosProyecto" runat="server" Text="Miembros de equipo asociados." />
                    </div>
                </div>

            </div>
            </div>


            </div>

<div class="panel panel-primary">
  <div class="panel-heading">
    <h3 class="panel-title">Información sobre Requerimientos </h3>
  </div>
  <div class="panel-body">

            <div class="col-md3">
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-horizontal">
                            &nbsp;<asp:CheckBox ID="CheckBoxExitos" runat="server" Text="Cantidad éxitos." />
                        </div>
                    </div>
                    <div class="col-md-4">
                        &nbsp;<asp:CheckBox ID="CheckBoxTipoNoConf" runat="server" Text="Cantidad pendientes"  />
                    </div>
                    <div class="col-md-4">
                        &nbsp;<asp:CheckBox ID="CheckBoxCantNoConf" runat="server" Text="Tipos de no conformidades." />
                    </div>
                </div>
            </div>

  </div>
</div>
        <div class="row">

            <div class="col-md-offset-7">
                 <div class="col-md-6">
                        &nbsp;<asp:CheckBox ID="selTodos" runat="server" text="Seleccionar todos" OnCheckedChanged="selTodos_CheckedChanged" AutoPostBack="True" />
                    </div>
                    <div class="col-md-6">
                        &nbsp;<asp:CheckBox ID="deselTodos" runat="server" text="Deseleccionar todos" OnCheckedChanged="deselTodos_CheckedChanged" AutoPostBack="True" />
                    </div>                                               
            </div>
        </div>
<div class="row">

            <div class="col-md-offset-10 col-md-12">
                <asp:Button runat="server" Text="Generar Reporte" CssClass="btn btn-primary" ID="Generar" OnClick="BotonGE_Click" CausesValidation="false" />
            </div>
</div>

        </div>        
 


   
   <div class="panel panel-default"  style="margin-top: 20px; margin-bottom:0px; padding-bottom:0">
  <div class="panel-body">

       <legend style="margin-top: 10px"><h5>Previsualización del Reporte</h5></legend>
           <div style="overflow-x:scroll; padding:17px; margin-bottom: 20px">           
            <div class="row">
                <asp:GridView ID="preGrid" runat="server" CellPadding="20" margin-left="auto" CssClass="GridView" HorizontalAlign="Center" RowStyle-BackColor="White" AllowRowSelect="false"
                    HeaderStyle-BackColor="#eeeeee" HeaderStyle-ForeColor="#333333" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" RowStyle-HorizontalAlign="Center" RowStyle-VerticalAlign="Top" BorderColor="#cdcdcd" border-radius="15px" AutoGenerateColumns="true" >
                </asp:GridView>
            </div>
</div>
    <div class="row">
                <div class="form-group">
                    <div class="col-md-offset-2 col-md-2">
                    <asp:Label runat="server" CssClass="control-label">Tipo de Archivo</asp:Label>
                        </div>
                    <div class="col-md-3">
					 <asp:DropDownList ID="DDLTipoArchivo" runat="server" style="width:250px" CssClass="form-control"></asp:DropDownList>
                    </div>
    <div class="col-md-4">
          <asp:Button runat="server" Text="Descargar" CssClass="btn btn-primary" ID="Button1" OnClick="BotonDescGR_Click" CausesValidation="false" />
          <asp:Button runat="server" Text="Cancelar" CssClass="btn btn-default" ID="Button2" style="border-color:#fe6c4f;color:#fe5e3e" CausesValidation="false" />
    </div>					
           </div>

  </div>

</div>    

       </div> 

    <div class="row">
         <div class="col-md-10">
              <div class="Row">
                   <asp:GridView ID="GridGR" runat="server" OnSelectedIndexChanged="Reporte_SelectedIndexChanged" OnPageIndexChanging="Reporte_OnPageIndexChanging"
                        OnRowDataBound="Reporte_OnRowDataBound" CellPadding="10" margin-left="auto" CssClass="GridView" HorizontalAlign="Center" AllowRowSelect="false"
                        AllowPaging="true" PageSize="5" HeaderStyle-BackColor="#eeeeee" HeaderStyle-ForeColor="#333333" BorderColor="#cdcdcd" border-radius="15px">
                   </asp:GridView>
              </div>
        </div>
    </div>

    <asp:Panel runat="server" ID="panelModal2" CssClass="modalPopup" Style="display:none"> 
            <legend style="margin-top:15px"><h5>¿Desea cancelar la operación?</h5></legend>
            <div aria-pressed="true">
                <asp:button runat="server" ID="siModalCancelar" Text="Si" OnClick="cancelarModal_Click" CssClass="btn btn-primary" 
				style="align-self: center;margin-left:8px;margin-right:11px;margin-bottom:20px;  width:85px"/>
                <asp:button runat="server" ID="noModalCancelar" Text="No" OnClick="siModalCancelar_Click" CssClass="btn btn-default" 
				style="border-color:#fe6c4f;color:#fe5e3e;align-self:center;margin-left:11px;margin-right:6px;margin-bottom:20px;  width:85px"/>           
            </div>
        </asp:Panel>

        <ajaxToolkit:ModalPopupExtender ID="ModalCancelar" runat="server" BackgroundCssClass="modalBackground" PopupControlID="panelModal2" TargetControlID="Button2" OnCancelScript="noModalCancelar" OnOkScript="siModalCancelar" BehaviorID="ModalCancelar">
        </ajaxToolkit:ModalPopupExtender>
    <%--</div>--%>
</asp:Content>
