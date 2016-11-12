<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InterfazCasosDePrueba.aspx.cs" Inherits="SistemaPruebas.Intefaces.CasosDePrueba" Async="true"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<script type="text/javascript">
        function HideLabel() {
            var seconds = 5;           
            setTimeout(function () {

                $('#'+'<%=EtiqMensajeOperacion.ClientID %>').fadeOut('5000');
            }, 2000);           
    };
</script>

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



    <legend style="margin-top:45px"><h2>Módulo de Casos de Prueba</h2></legend>

<asp:Label runat="server" CssClass="text-danger" ID="EtiqMensajeOperacion" >.</asp:Label>
<asp:Label runat="server" CssClass="text-danger" ID="EtiqErrorInsertar" >*Ha habido problemas para agregar este caso de prueba al sistema. Por favor vuelva a intentarlo.</asp:Label>
<asp:Label runat="server" CssClass="text-danger" ID="EtiqErrorConsultar" >*Ha habido problemas para consultar este caso de prueba. Por favor vuelva a intentarlo mas tarde.</asp:Label>
<asp:Label runat="server" CssClass="text-danger" ID="EtiqErrorModificar" >*Ha habido problemas para modificar este caso de prueba. Por favor vuelva a intentarlo.</asp:Label>
<asp:Label runat="server" CssClass="text-danger" ID="EtiqErrorEliminar" >*Ha habido problemas para eliminar este caso de prueba del sistema. Por favor vuelva a intentarlo.</asp:Label>

<div class="form-group">
    <div class="col-md-offset-9 col-md-12">
        <div class="btn-group">
        <asp:Button runat="server" Text="Insertar" CssClass="btn btn-default" ID="BotonCPInsertar" OnClick="BotonCPInsertar_Click" CausesValidation="false"/>
        <asp:Button runat="server" Text="Modificar" CssClass="btn btn-default" ID="BotonCPModificar" OnClick="BotonCPModificar_Click" CausesValidation="false"/>
        <asp:Button runat="server" Text="Eliminar" CssClass="btn btn-default" ID="BotonCPEliminar" OnClick="BotonCPEliminar_Click" CausesValidation="false"/>
    </div>
    </div>
</div>

<hr style="margin:50px;">

<div class="panel panel-primary">
  <div class="panel-heading">
    <h3 class="panel-title">Resumen sobre el Diseño</h3>
  </div>
  <div class="panel-body">
  <div class ="row" >
    <div class="form-horizontal">
	
           <div class ="form-group">
               <div class ="col-md-offset-1 col-md-1">
			<asp:Label ID="Proyecto1" Font-Bold="true" runat="server" CssClass="control-label"  Text="Proyecto: "></asp:Label>
                </div>
                <div class ="col-sm-2">
						<asp:Label ID="Proyecto" runat="server" CssClass="control-label" Text=""></asp:Label>	
                </div>
            <div class ="col-md-3">
                <asp:Label ID="Proposito1" Font-Bold="true" runat="server" CssClass="control-label" style="width:120px" Text ="Propósito del Diseño: "></asp:Label>
                </div>

                <div class ="col-md-5">
                <asp:Label ID="Proposito" runat="server" CssClass="control-label" Text =""></asp:Label>
                </div>

                <div class ="col-md-offset-1 col-sm-1">
				<asp:Label ID="Nivel1" Font-Bold="true" runat="server" CssClass="control-label" Text="Nivel: "></asp:Label>
                </div>
               <div class="col-sm-2">
                <asp:Label ID="Nivel" runat="server" CssClass="control-label" Text=""></asp:Label>
            </div>
                <div class="col-sm-3">
               <asp:Label ID="Tecnica1" Font-Bold="true" runat="server" CssClass="control-label" Text="Técnica: "></asp:Label>
            </div>
            <div class="col-md-5">
                <asp:Label ID="Tecnica" runat="server" CssClass="control-label" Text=""></asp:Label>
            </div>
        <div class="col-md-offset-1 col-sm-2">
            <asp:Label ID="Requerimientos1" Font-Bold="true" runat="server" CssClass="control-label" Text="Requerimientos: "></asp:Label>
        </div>
        <div class="col-sm-9">
		    <asp:Label ID="Requerimientos"  runat="server" CssClass="control-label" Text=""></asp:Label>
        </div>
        
           </div>

                <div class="col-md-offset-10 col-md-12">
	<asp:Button runat="server" Text="Regresar a Diseño" OnClick="regresarADiseno" CssClass="btn btn-primary" ID="Regresar" CausesValidation="false"/>
        </div>
      </div>


        <div style="clear:both"></div>
		</div>	
    </div>	
    </div>
    <div class="well">
    
 <legend style="margin-top:45px; margin-bottom: 35px"><h4>Información de Casos de Prueba</h4></legend>
  
<div class ="row" >
    <div class="form-horizontal">
           <div class ="form-group">
               <div class ="col-md-2">
                    <asp:Label ID="id_casoPrueba" runat="server" style="margin-left:15px" CssClass="col-md-2 control-label"  Text="Id*:"></asp:Label> 
                </div>
                <div class ="col-md-4">

		    <asp:TextBox runat="server" ID="TextBoxID" CssClass="form-control" onkeypress="checkInput(event)" AutoPostBack="true" MaxLength="20"/>
		    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" style="margin-left:200px" runat="server" ErrorMessage="Campo requerido" ControlToValidate="TextBoxID" ForeColor="Salmon"></asp:RequiredFieldValidator>
            <script type="text/javascript">
                function checkInput(e) {
                    var ok = /[A-Za-z0-9-_]/.test(String.fromCharCode(e.charCode));
                    if (e.keyCode == 8) {
                        //alert();
                    }
                    else if (!ok)
                        e.preventDefault();
                }
		    </script>			
				
                </div>

           </div>

           <div class ="form-group">                                     
               <div class ="col-md-2">
		    <asp:Label ID="PropositoCP" runat="server"  style="margin-left:15px" CssClass="col-md-2 control-label" Text="Propósito*:"></asp:Label>                        
                </div>
                <div class ="col-md-4">

		    <asp:TextBox runat="server" ID="TextBoxPropositoCP" CssClass="form-control" style="vertical-align: top; width:230%"
                 onkeypress="checkInput3(event)" TextMode="multiline"/>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" style="margin-left:200px; width: 230%" ErrorMessage="Campo requerido" ControlToValidate="TextBoxPropositoCP" ForeColor="Salmon"></asp:RequiredFieldValidator>
		    <script type="text/javascript">
		        function checkInput3(e) {
		            var key = theEvent.keyCode || theEvent.which;

		            var ok = /[A-Za-z0-9.\"\(\)áéíóú ]/.test(String.fromCharCode(e.charCode));
		            if (e.keyCode == 8) {
		                //alert();
		            }
		            else if (!ok)
		                e.preventDefault();
		        }
		    </script>			
				
                </div>
               </div>

</div>
    </div>

        <div class="row">
        <div class="col-md-offset-0 col-md-5">

<div class="panel panel-primary">
  <div class="panel-heading">
    <h3 class="panel-title">Entrada de Datos</h3>
  </div>
  <div class="panel-body">


<div class ="row" >
    <div class="form-horizontal">
           <div class ="form-group">
               <div class ="col-md-2">
                    <asp:Label ID="EntradaDatosCP" runat="server" CssClass="col-md-2 control-label" Text ="Descripción:"></asp:Label>
                </div>
                <div class ="col-md-offset-1 col-md-5">

                <asp:TextBox runat="server" ID="TextBoxDescripcion" style="margin-left:15px; width:146%" onkeypress="checkInput1(event)" CssClass="form-control"/>
                    <script type="text/javascript">
                        function checkInput1(e) {
                            var ok = /[A-Za-z]/.test(String.fromCharCode(e.charCode));
                            if (e.keyCode == 8) {
                                //alert();
                            }
                            else if (!ok) {
                                if ($('#errorNombreSistema').css('display') == 'none') {
                                    $('#errorNombreSistema').fadeIn();
                                    $('#errorNombreSistema').fadeOut(6000);
                                }
                                if (window.event)//IE
                                    e.returnValue = false;
                                else//Firefox
                                    e.preventDefault();
                            }
                        }
                    </script>			
				
                </div>

           </div>		  		   
</div>

</div>		

                <div class="form-group">
                    <asp:Label ID="TiposCP" runat="server" CssClass="col-md-2 control-label" style="width: 90px; text-align: right; margin-left: 30px;" Text="Tipo:"></asp:Label>
                        <asp:DropDownList ID="TipoEntrada" runat="server"  CssClass="form-control" style="width:60%"  OnSelectedIndexChanged="TipoEntrada_SelectedIndexChanged">
                            <asp:ListItem Text ="Válido" Value =1/>
                            <asp:ListItem Text ="Inválido" Value =2/>
                            <asp:ListItem Text ="No Aplica" Value =3/>
                        </asp:DropDownList>
                </div>
                <div class="form-group">
                    <asp:Label ID="DatosCP" runat="server" CssClass="col-md-2 control-label" style="width: 90px; text-align: right; margin-left: 30px;" Text="Datos:"></asp:Label>

                    <asp:TextBox runat="server" ID="TextBoxDatos" style="width:60%" onkeypress="checkInput2(event)" CssClass="form-control"/>
                    <script type="text/javascript">
                        function checkInput2(e) {
                            var ok = /[A-Za-z0-9]/.test(String.fromCharCode(e.charCode));
                            if (e.keyCode == 8) {
                                //alert();
                            }
                            else if (!ok) {
                                if ($('#errorNombreSistema').css('display') == 'none') {
                                    $('#errorNombreSistema').fadeIn();
                                    $('#errorNombreSistema').fadeOut(6000);
                                }

                                if (window.event)//IE
                                    e.returnValue = false;
                                else//Firefox
                                    e.preventDefault();
                            }
                        }
                    </script>
                    
                </div>
                

                <div class="form-group">          
                        <div class="col-md-offset-6 col-md-12">
                            <asp:Button runat="server" Text="Agregar"                               
                                CssClass="btn btn-default" ID="AgregarEntrada" OnClick="AgregarEntrada_Click" CausesValidation="false" style="border-color:#4bb648;color:#4bb648;" />
                            <asp:Button runat="server" Text="Eliminar"  
                                CssClass="btn btn-default" ID="EliminarEntrada" OnClick="EliminarEntrada_Click" CausesValidation="false" style="border-color:#fe6c4f;color:#fe5e3e;"/>
                        </div>
                </div>

                <div class="form-group">          
                    <div class="col-md-8">
                        <asp:GridView ID="DECP" runat ="server" margin-right ="auto" 
                                    CellPadding="10" 
                                    margin-left="auto" AutoGenerateColumns ="true" 
                                    CssClass ="GridView" HorizontalAlign="Center"   
                                    HeaderStyle-BackColor="#48cfae" HeaderStyle-ForeColor="#ffffff" BorderColor="#CDCDCD" border-radius="15px"  RowStyle-BackColor="White" PagerStyle-BackColor="White"
                                    AutoPostBack ="true" OnSelectedIndexChanged="DECP_SelectedIndexChanged"
                                    AllowPaging="true" PageSize="3" OnPageIndexChanging="OnDECPPageIndexChanging" OnRowDataBound ="OnDECPRowDataBound" Width="155%">                            
                        </asp:GridView>

  

                    </div>
                </div>
                <div style="clear:both"></div>
            </div>
  </div>
</div>

           <div class ="form-group">

		        <div class="col-md-offset-1 col-md-2">
                    <asp:Label ID="ResultadoCP" runat="server" CssClass="col-md-6 control-label" Text="Resultado esperado:"></asp:Label>
                </div>
                <div class ="col-md-4">
		<asp:TextBox runat="server" ID="TextBoxResultadoCP" onkeypress="checkInput4(event)" style="width:250px;height:90px" CssClass="form-control" MaxLength="50" TextMode="multiline"/>
        <script type="text/javascript">
            function checkInput4(e) {
                var ok = /[A-Za-z.áéíóú ]/.test(String.fromCharCode(e.charCode));
                if (e.keyCode == 8) {
                    //alert();
                }
                else if (!ok) {
                    if ($('#errorNombreSistema').css('display') == 'none') {
                        $('#errorNombreSistema').fadeIn();
                        $('#errorNombreSistema').fadeOut(6000);
                    }
                    if (window.event)//IE
                        e.returnValue = false;
                    else//Firefox
                        e.preventDefault();
                }

            }
        </script>
                </div>

           </div>	

           <div class ="form-group">

		        <div class="col-md-offset-1 col-md-2">
                    <asp:Label ID="FlujoCP" runat="server" CssClass="col-md-6 control-label" style="width:20px;margin-top:20px;" Text="Flujo Central:"></asp:Label>
                </div>
                <div class ="col-md-4">
        <asp:TextBox runat="server" ID="TextBoxFlujoCentral" onkeypress="checkInput5(event)" style="margin-top:20px; width:250px; height:90px;" CssClass="form-control" MaxLength="50" TextMode="multiline"/>
        <script type="text/javascript">
            function checkInput5(e) {
                var ok = /[A-Za-z0-9.\"\(\)áéíóú +]/.test(String.fromCharCode(e.charCode));
                if (e.keyCode == 8) {
                    //alert();
                }
                else if (!ok) {
                    if ($('#errorNombreSistema').css('display') == 'none') {
                        $('#errorNombreSistema').fadeIn();
                        $('#errorNombreSistema').fadeOut(6000);
                    }
                    if (window.event)//IE
                        e.returnValue = false;
                    else//Firefox
                        e.preventDefault();
                }

            }
        </script>
                </div>

           </div>



</div>

<div class ="row" >
   <div class="form-group col-md-offset-9 col-md-12">
        <asp:Label runat="server" id="Label1" Text="Campos Obligatorios*" style="color: #C0C0C0;" CssClass="control-label"></asp:label>
    </div>
</div>

    </div>

    <div class="col-md-offset-9 col-md-12">
        <asp:Button runat="server" style="border-color:#4bb648;color:#4bb648;"
            Text="Aceptar" 
            causesvalidation="true"                              
            CssClass="btn btn-default" 
            ID="BotonCPAceptar" OnClick="BotonCPAceptar_Click" 
        />
        <asp:Button runat="server" Text="Cancelar" style="border-color:#fe6c4f;color:#fe5e3e;" 
            CssClass="btn btn-default" ID="BotonCPCancelar"  
            OnClick="BotonCPCancelar_Click" 
            CausesValidation="false"
        />
    </div>


 <div class="row">        
    <asp:GridView ID="CP" runat ="server" margin-right ="auto"
            
            CellPadding="10" 
            margin-left="auto"
            CssClass ="GridView" HorizontalAlign="Center" 
            OnRowDataBound="OnCPRowDataBound" 
            AllowPaging="true"   PageSize="5" 
            OnPageIndexChanging="OnCPPageIndexChanging"
            HeaderStyle-BackColor="#48cfae" HeaderStyle-ForeColor="#ffffff" BorderColor="#cdcdcd" border-radius="15px" 
            AutoPostBack ="true" OnSelectedIndexChanged="CP_SelectedIndexChanged" RowStyle-BackColor="White" PagerStyle-BackColor="White" Width="90%" >      
    </asp:GridView>
</div> 


<asp:Panel runat="server" ID="cancelarPanelModal" CssClass="modalPopup"> 
                             <legend style="margin-top:15px"><h5>¿Desea cancelar la operación?</h5></legend>

    <div aria-pressed="true" style="padding-left:30px">
        <asp:button runat="server" ID="cancelarBotonSiModal" Text="Si" CssClass="btn btn-primary" style="margin-left:8px;margin-right:20px;margin-bottom:11px; width:85px" CausesValidation="false" OnClick="cancelarModal_Click"/>
        <asp:button runat="server" ID="cancelarBotonNoModal" Text="No" CssClass="btn btn-default" style="border-color:#fe6c4f;color:#fe5e3e;align-self:center;margin-left:11px;margin-right:6px;margin-bottom:20px; width:85px" CausesValidation="false" />           
    </div>
</asp:Panel>
<ajaxToolkit:ModalPopupExtender ID="ModalCancelar" runat="server" BackgroundCssClass="modalBackground" PopupControlID="cancelarPanelModal" TargetControlID="BotonCPCancelar" OnCancelScript="cancelarButtonNoModal" OnOkScript="cancelarButtonSiModal"></ajaxToolkit:ModalPopupExtender>

<asp:Panel runat="server" ID="panelModal" CssClass="modalPopup"> 
    <legend style="margin-top:15px"><h5>¿Desea eliminar este caso de prueba?</h5></legend>
    <div aria-pressed="true" style="padding-left:30px">
        <asp:button runat="server" ID="aceptarModal" Text="Eliminar"  CssClass="btn btn-primary" style="align-self:center;margin-left:16px;margin-right:11px;margin-bottom:20px" OnClick="aceptarModalEliminar_Click" CausesValidation="false"/>
        <asp:button runat="server" ID="cancelarModal" Text="Cancelar"  CssClass="btn btn-default" style="border-color:#fe6c4f;color:#fe5e3e;align-self:center;margin-left:11px;margin-right:6px;margin-bottom:20px" OnClick="cancelarModal_Click" CausesValidation="false"/>           
    </div>
</asp:Panel>

<ajaxToolkit:ModalPopupExtender ID="ModalEliminar" runat="server" BackgroundCssClass="modalBackground" PopupControlID="panelModal" TargetControlID="BotonCPEliminar" OnCancelScript="cancelarModal" OnOkScript="aceptarModal"></ajaxToolkit:ModalPopupExtender>
</asp:Content>

