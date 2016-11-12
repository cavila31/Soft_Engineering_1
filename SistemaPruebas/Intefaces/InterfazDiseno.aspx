<%@ Page EnableEventValidation="false" Title="Diseño" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InterfazDiseno.aspx.cs" Inherits="SistemaPruebas.Intefaces.InterfazDiseno" Async="true" %>


<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

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
    <script type="text/javascript">
        function HideLabel() {
            $('#errorGen').fadeIn();
            $('#errorGen').fadeOut(5000);
        };
    </script>

    <script type="text/javascript">
    $(document).ready(function () {
        $("[id$=A3]").addClass("active");
    });
</script>

    <legend style="margin-top:45px"><h2>Módulo de Diseño</h2></legend>

    
    <link rel="stylesheet" type="text/css" media="screen"
        href="http://tarruda.github.com/bootstrap-datetimepicker/assets/css/bootstrap-datetimepicker.min.css">
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css">
    <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script src="//code.jquery.com/ui/1.11.4/jquery-ui.js"></script>
    <link rel="stylesheet" href="/resources/demos/style.css">
<pages>
      <controls>
       
 <add tagPrefix="asp" namespace="System.Web.UI" 
assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, 
PublicKeyToken=31bf3856ad364e35"/>
        <add namespace="AjaxControlToolkit" assembly="AjaxControlToolkit" 
            tagPrefix="ajaxToolkit"/>
      </controls>
    </pages>

    <div id="errorGen" style="display: none">
        <asp:Label runat="server" ID="EtiqErrorGen"></asp:Label>
    </div>
    <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
        <p class="text-danger">
            <asp:Literal runat="server" ID="FailureText" />
        </p>
    </asp:PlaceHolder>

    <div class="form-group">
            <div class="col-md-offset-9 col-md-12">
        <div class="btn-group">
            <asp:Button runat="server" ID="Insertar" Text="Insertar" CssClass="btn btn-default" OnClick="insertarClick" CausesValidation="false" />
            <asp:Button runat="server" ID="Modificar" Text="Modificar" CssClass="btn btn-default" OnClick="modificarClick" CausesValidation="false" />
            <asp:Button runat="server" ID="Eliminar" Text="Eliminar" CssClass="btn btn-default" OnClick="eliminarClick" CausesValidation="false" />
        </div>
    </div>
    </div>

    <asp:Panel runat="server" ID="panelModalEliminar" CssClass="modalPopup" Style="display:none">
         <legend style="margin-top:15px"><h5>¿Desea eliminar este diseño?</h5></legend>
        <div aria-pressed="true">
            <asp:Button runat="server" ID="aceptarModalEliminar" Text="Eliminar" OnClick="aceptarModal_ClickEliminar" CssClass="btn btn-primary" Style="align-self: center; margin-left: 16px; margin-right: 11px; margin-bottom: 20px" CausesValidation="false" />
            <asp:Button runat="server" ID="cancelarModalEliminar" Text="Cancelar" OnClick="cancelarModal_ClickEliminar" CssClass="btn btn-default" Style="border-color: #fe6c4f; color: #fe5e3e; align-self: center; margin-left: 11px; margin-right: 6px; margin-bottom: 20px" CausesValidation="false" />
        </div>
    </asp:Panel>
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" BackgroundCssClass="modalBackground" OnCancelScript="cancelarModalEliminar" OnOkScript="aceptarModalEliminar" TargetControlID="Eliminar" PopupControlID="panelModalEliminar"></ajaxToolkit:ModalPopupExtender>

<hr style="margin:50px;">
<div class="well">

 <legend><h5>Seleccione un Proyecto</h5></legend>
    <div class="row">
        <div class="col-md-8">
            <div class="form-horizontal">

                <div class="form-group">
                    <asp:Label runat="server" ItemStyle-Wrap="False" CssClass="col-md-4 control-label">Proyecto asociado:</asp:Label>
                    <div class="col-md-5">
                        <asp:DropDownList runat="server" ID="proyectoAsociado" Style="width: 250px; margin-bottom:20px" CssClass="form-control" OnSelectedIndexChanged="proyectoAsociado_SelectedIndexChanged" AutoPostBack="True">
                            <asp:ListItem Value="1">Seleccionar</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label runat="server" ID="labelSeleccioneProyecto" class="text-danger" Visible="false">Seleccione primero un Proyecto</asp:Label>
                    </div>
                </div>

            </div>
        </div>
    </div>
    

<div class="panel panel-primary">
  <div class="panel-heading">
    <h3 class="panel-title">Requerimientos a Probar</h3>
  </div>
  <div class="panel-body">

<div class="row">
            <div class="col-md-offset-1 col-md-7">
                <div class="form-group">
                    <asp:Label runat="server" CssClass="control-label">Requerimientos Disponibles</asp:Label>
                </div>
            </div>

            <div class="col-md-4">
                <div class="form-group">
                    <asp:Label runat="server" CssClass="control-label">Requerimientos en Diseño</asp:Label>
                </div>
            </div>

        </div>        
           
<div class="row">
            <div class="col-md-5">
                <div class="form-group">
                    <asp:GridView ID="gridNoAsociados" runat="server"
                        CellPadding="10" padding-left="20px" margin-left="20px"
                        OnSelectedIndexChanged="OnSelectedIndexChangedNoAsoc"
                        OnRowDataBound="OnRowDataBoundNoAsoc" CssClass="GridView" HorizontalAlign="Left"
                        AllowPaging="true" OnPageIndexChanging="OnPageIndexChangingNoAsoc" PageSize="5"
                        HeaderStyle-BackColor="#48cfae" HeaderStyle-ForeColor="#ffffff" BorderColor="#cdcdcd" border-radius="15px"
                        AutoPostBack="true" RowStyle-BackColor="White" PagerStyle-BackColor="White" Width="100%">
                    </asp:GridView>                    
                </div>
            </div>

            <div class="col-md-offset-2 col-md-5">
                <div class="form-group">
                    <asp:GridView ID="gridAsociados" runat="server" margin-right="auto"
                        CellPadding="10" padding-right="20px"
                        margin-left="auto" OnSelectedIndexChanged="OnSelectedIndexChangedAsoc"
                        OnRowDataBound="OnRowDataBoundAsoc" CssClass="GridView" HorizontalAlign="Left"
                        AllowPaging="true" OnPageIndexChanging="OnPageIndexChangingAsoc" PageSize="5"
                        HeaderStyle-BackColor="#48cfae" HeaderStyle-ForeColor="#ffffff" BorderColor="#cdcdcd" border-radius="15px"
                        AutoPostBack="true" RowStyle-BackColor="White" PagerStyle-BackColor="White" Width="100%">
                    </asp:GridView>                    
                </div>
            </div>

        </div>     
   
        <div class="col-md-offset-10 col-md-12">
            <asp:Button runat="server" ID="iraRequerimientoBtn" Text="Ir a Requerimiento" CssClass="btn btn-primary" OnClick="irAReq" CausesValidation="false" Style="margin-top: 20px" />
        </div>

  </div>
</div>
 
 <legend style="margin-top:45px; margin-bottom: 35px"><h4>Información del Diseño</h4></legend>


        <div class ="row" >
       <div class="form-horizontal">
           <div class ="form-group">
               <div class ="col-md-2">
                    <asp:Label runat="server" ID="propositoLabel" CssClass="col-md-2 control-label">Propósito:</asp:Label>
                </div>
                <div class ="col-md-4">
                    <asp:TextBox runat="server" ID="propositoTxtbox" Style="width: 250px; height: 36px" CssClass="form-control" MaxLength="80"
                            onkeypress="solo_letras(event)" placeholder="Sólo letras."/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Campo requerido" ControlToValidate="propositoTxtbox" ForeColor="Salmon"></asp:RequiredFieldValidator>
                        <script type="text/javascript">
                            function solo_letras(evt) {

                                if ((evt.charCode < 65 || evt.charCode > 90) && (evt.charCode < 97 || evt.charCode > 122)) {
                                    if ((evt.keyCode != 32) && (evt.charCode != 32) && (evt.charCode != 46) && (evt.charCode != 44) && (evt.keyCode != 13) && (evt.keyCode != 37) && (evt.keyCode != 39) && (evt.keyCode != 8) && (evt.keyCode != 83)) {
                                        //alert();
                                        if ($('#errorNombreSistema').css('display') == 'none') {
                                            $('#errorNombreSistema').fadeIn();
                                            $('#errorNombreSistema').fadeOut(6000);
                                        }
                                        if (window.event)//IE
                                            evt.returnValue = false;
                                        else//Firefox
                                            evt.preventDefault();

                                    }
                                }
                            }
                        </script>
                        <div id="errorNombreSistema" style="display: none">
                            <asp:Label runat="server" ID="errorNombreSistLbl" Text="Sólo se permite el ingreso de letras" ForeColor="Salmon"></asp:Label>
                        </div>
                </div>

                <div class="col-md-2">
                    <asp:Label runat="server" CssClass="col-md-6 control-label">Ambiente:</asp:Label> 
                </div>
                <div class ="col-md-4">
                    <asp:TextBox runat="server" ID="ambienteTxtbox" Style="width: 250px; height: 130px" CssClass="form-control" MaxLength="150" TextMode="multiline"
                            onkeypress="solo_letras1(event)" placeholder="Sólo letras y espacios."/>
                        <script type="text/javascript">
                            function solo_letras1(evt) {

                                if ((evt.charCode < 65 || evt.charCode > 90) && (evt.charCode < 97 || evt.charCode > 122)) {
                                    if ((evt.keyCode != 32) && (evt.charCode != 32) && (evt.charCode != 46) && (evt.charCode != 44) && (evt.keyCode != 13) && (evt.keyCode != 37) && (evt.keyCode != 39) && (evt.keyCode != 8) && (evt.keyCode != 83)) {
                                        //alert();
                                        if ($('#errorNombreSistema1').css('display') == 'none') {
                                            $('#errorNombreSistema1').fadeIn();
                                            $('#errorNombreSistema1').fadeOut(6000);
                                        }
                                        if (window.event)//IE
                                            evt.returnValue = false;
                                        else//Firefox
                                            evt.preventDefault();

                                    }
                                }
                            }
                        </script>
                        <div id="errorNombreSistema1" style="display: none; width: 250px;">
                            <asp:Label runat="server" ID="errorNombreSistLbl1" Text="Sólo se permite el ingreso de letras y espacios" ForeColor="Salmon"></asp:Label>
                        </div>
                </div>
           </div>
		</div>

       <div class="form-horizontal">
           <div class ="form-group">

               <div class ="col-md-2">
                    <asp:Label runat="server" CssClass="col-md-2 control-label">Nivel:</asp:Label>
                </div>
                <div class ="col-md-4">

                       <asp:DropDownList runat="server" ID="Nivel" Style="width: 250px" CssClass="form-control">
                            <asp:ListItem Selected="True" Value="1">Seleccionar</asp:ListItem>
                            <asp:ListItem Value="2">Unitaria</asp:ListItem>
                            <asp:ListItem Value="3">Integración</asp:ListItem>
                            <asp:ListItem Value="4">Sistema</asp:ListItem>
                            <asp:ListItem Value="5">Aceptación</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Nivel" InitialValue="1" ErrorMessage="Campo Requerido" ForeColor="Salmon"/>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Campo requerido" ControlToValidate="procedimientoTxtbox" ForeColor="Salmon"></asp:RequiredFieldValidator>--%>
                    				
                </div>

                <div class="col-md-2">
                    <asp:Label runat="server" CssClass="col-md-6 control-label">Técnica:</asp:Label>  
                </div>
                <div class ="col-md-4">
					        <asp:DropDownList runat="server" ID="Tecnica" Style="width: 250px" CssClass="form-control">
                            <asp:ListItem Selected="True" Value="1">Seleccionar</asp:ListItem>
                            <asp:ListItem Value="2">Caja Negra</asp:ListItem>
                            <asp:ListItem Value="3">Caja Blanca</asp:ListItem>
                            <asp:ListItem Value="4">Exploratoria</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="reqTecnica" runat="server" ControlToValidate="Tecnica" InitialValue="1" ErrorMessage="Campo Requerido" ForeColor="Salmon"/>
                </div>

            </div>
        </div>


<div class="form-horizontal">
           <div class ="form-group">
               <div class ="col-md-2">
                    <asp:Label runat="server" CssClass="col-md-2 control-label">Procedimiento Utilizado:</asp:Label>
                </div>
                <div class ="col-md-5">
  				<asp:TextBox runat="server" ID="procedimientoTxtbox" Style="width: 180%; height: 90px" CssClass="form-control" MaxLength="150" TextMode="multiline"
                            onkeypress="solo_letras2(event)" placeholder="Sólo letras y espacios."/>

                        <script type="text/javascript">
                            function solo_letras2(evt) {

                                if ((evt.charCode < 65 || evt.charCode > 90) && (evt.charCode < 97 || evt.charCode > 122)) {
                                    if ((evt.keyCode != 32) && (evt.charCode != 32) && (evt.charCode != 46) && (evt.charCode != 44) && (evt.keyCode != 13) && (evt.keyCode != 37) && (evt.keyCode != 39) && (evt.keyCode != 8) && (evt.keyCode != 83)) {
                                        //alert();
                                        if ($('#errorNombreSistema2').css('display') == 'none') {
                                            $('#errorNombreSistema2').fadeIn();
                                            $('#errorNombreSistema2').fadeOut(6000);
                                        }
                                        if (window.event)//IE
                                            evt.returnValue = false;
                                        else//Firefox
                                            evt.preventDefault();

                                    }
                                }
                            }
                        </script>
                        <div id="errorNombreSistema2" style="display: none; width: 500px;">
                            <asp:Label runat="server" ID="errorNombreSistLbl2" Text="Sólo se permite el ingreso de letras y espacios" ForeColor="Salmon"></asp:Label>
						                  
                </div>
				</div>
				</div>
				<div class ="form-group">
               <div class ="col-md-2">
                    <asp:Label runat="server" CssClass="col-md-2 control-label">Criterios de Aceptación:</asp:Label>
                </div>
                <div class ="col-md-4">
                    
				                        <asp:TextBox runat="server" ID="criteriosTxtbox" Style="width:230%; height: 90px" CssClass="form-control" MaxLength="150" TextMode="multiline"
                            onkeypress="solo_letras3(event)" placeholder="Sólo letras y espacios."/>
                        <script type="text/javascript">
                            function solo_letras3(evt) {

                                if ((evt.charCode < 65 || evt.charCode > 90) && (evt.charCode < 97 || evt.charCode > 122)) {
                                    if ((evt.keyCode != 32) && (evt.charCode != 32) && (evt.charCode != 46) && (evt.charCode != 44) && (evt.keyCode != 13) && (evt.keyCode != 37) && (evt.keyCode != 39) && (evt.keyCode != 8) && (evt.keyCode != 83)) {
                                        //alert();
                                        if ($('#errorNombreSistema3').css('display') == 'none') {
                                            $('#errorNombreSistema3').fadeIn();
                                            $('#errorNombreSistema3').fadeOut(6000);
                                        }
                                        if (window.event)//IE
                                            evt.returnValue = false;
                                        else//Firefox
                                            evt.preventDefault();

                                    }
                                }
                            }
                        </script>
                        <div id="errorNombreSistema3" style="display: none; width: 500px;">
                            <asp:Label runat="server" ID="Label1" Text="Sólo se permite el ingreso de letras y espacios" ForeColor="Salmon"></asp:Label>
                        </div>	
					
					
                </div>
			</div>
</div>

       <div class="form-horizontal">
           <div class ="form-group">
               <div class ="col-md-2">
                    <asp:Label runat="server" CssClass="col-md-2 control-label">Responsable:</asp:Label>
                </div>
                <div class ="col-md-4">

                        <asp:DropDownList runat="server" ID="responsable" Style="width: 250px" CssClass="form-control">
                            <asp:ListItem Selected="True" Value="1">Seleccionar</asp:ListItem>
                        </asp:DropDownList>				
				
                </div>
                <div class="col-md-2">
                    <asp:Label runat="server" CssClass="col-md-6 control-label">Fecha de Diseño:</asp:Label>  
                </div>
                <div class ="col-md-4">
				<asp:TextBox runat="server" ID="txt_date" Style="width: 250px; height: 36px" CssClass="form-control" ></asp:TextBox>
                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="txt_date" TargetControlID="txt_date" />
                </div>
           </div>
		</div>

	</div>

    <div class="row">
        <div id="botonCasoPrueba"  class="col-md-offset-9 col-md-12">
            <asp:Button runat="server" ID="botonCP" Text="Ir a Casos de Prueba" Enabled="false" CssClass="btn btn-primary" Style="margin-top: 10px; margin-left: 15px;" CausesValidation="false" OnClick="irACasoPrueba" />
        </div>
    </div>

    </div>

    <div class="form-group">
        <div id="Botones_aceptar_cancelar" class="col-md-offset-9 col-md-12">
            <asp:Button runat="server" ID="aceptar" Text="Aceptar" CssClass="btn btn-default" Style="border-color: #4bb648; color: #4bb648; margin-top: 20px;" OnClick="aceptarClick" />
            <asp:Button runat="server" ID="cancelar" Text="Cancelar" Style="border-color: #fe6c4f; color: #fe5e3e; margin-top: 20px;" CssClass="btn btn-default" OnClick="cancelarClick" CausesValidation="false" />

        </div>
    </div>

    <div id="tablaDisenos" class="row">

        <asp:GridView ID="gridDisenos" runat="server" margin-right="auto"
            CellPadding="10" CellSpacing="500"
            margin-left="auto" OnSelectedIndexChanged="OnSelectedIndexChanged"
            OnRowDataBound="OnRowDataBound" CssClass="GridView" HorizontalAlign="Center"
            AllowPaging="true" OnPageIndexChanging="OnPageIndexChanging" PageSize="5"
            HeaderStyle-BackColor="#48cfae" HeaderStyle-ForeColor="#ffffff" BorderColor="#cdcdcd" border-radius="15px"
            AutoPostBack="true"  RowStyle-BackColor="White" PagerStyle-BackColor="White" Width="90%">
        </asp:GridView>
    </div>

    <asp:Panel runat="server" ID="panelModalCancelar" CssClass="modalPopup" Style="display: none">
                 <legend style="margin-top:15px"><h5>¿Desea cancelar la operación?</h5></legend>
        <div aria-pressed="true" style="padding-left:30px">
            <asp:Button runat="server" ID="aceptarModalCancelar" Text="Si" CssClass="btn btn-primary" Style="align-self: center; margin-left: 8px; margin-right: 11px; margin-bottom: 20px; width:85px" OnClick="aceptarModal_ClickCancelar" CausesValidation="false" />
            <asp:Button runat="server" ID="cancelarModalCancelar" Text="No" CssClass="btn btn-default" Style="border-color: #fe6c4f; color: #fe5e3e; align-self: center; margin-left: 11px; margin-right: 6px; margin-bottom: 20px;  width:85px" OnClick="cancelarModal_ClickCancelar" CausesValidation="false" />
        </div>
    </asp:Panel>
    <ajaxToolkit:ModalPopupExtender ID="ModalCancelar" runat="server" BackgroundCssClass="modalBackground" PopupControlID="panelModalCancelar" TargetControlID="cancelar" OnCancelScript="cancelarModalCancelar" OnOkScript="aceptarModalCancelar"></ajaxToolkit:ModalPopupExtender>

</asp:Content>
