<%@ Page Title="Gestión de Recursos Humanos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InterfazRecursoHumano.aspx.cs" Inherits="SistemaPruebas.Intefaces.InterfazRecursoHumano" Async="true" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <legend style="margin-top:45px"><h2>Gestión de Recursos Humanos</h2></legend>

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

                $('#' + '<%=EtiqErrorGen.ClientID %>').fadeOut('5000');
            }, 2000);
        }
    </script>
    <script type="text/javascript">
        function solo_numeros(evt) {
            if (evt.charCode > 31 && (evt.charCode < 48 || evt.charCode > 57)) {
                //alert("Sólo se permite números");
                return false;
            }
            else
                return true;
        }
    </script>
    <script type="text/javascript">
        function solo_letras(evt) {           
            if ((evt.charCode < 65 || evt.charCode > 90) && (evt.charCode < 97 || evt.charCode > 122)) {
                if ((evt.keyCode != 32) && (evt.charCode != 32) && (evt.charCode != 46) && (evt.keyCode != 13) && (evt.keyCode != 37) && (evt.keyCode != 39)
                    && (evt.keyCode != 8) && (evt.keyCode != 83) && (evt.charCode != 44)) {                  
                    return false;
                }
                else {

                    return true;
                }
            }
            else
                return true;
        }
    </script>
    <script type="text/javascript">
        function HideLabel() {
            $('#errorGen').fadeIn();
            $('#errorGen').fadeOut(5000);
        };
</script>

<script type="text/javascript">
    $(document).ready(function () {
        $("[id$=A2]").addClass("active");
    });
</script>

    <div id="errorGen" style="display:none" >
    <asp:Label runat="server" ID="EtiqErrorGen"></asp:Label>
        </div>
    <asp:Label runat="server" AssociatedControlID="TextBoxCedulaRH" CssClass="text-danger" ID="EtiqErrorInsertar">*Ha habido problemas para agregar este recurso humano al sistema. Por favor vuelva a intentarlo.</asp:Label>
    <asp:Label runat="server" AssociatedControlID="TextBoxCedulaRH" CssClass="text-danger" ID="EtiqErrorConsultar">*Ha habido problemas para consultar este recurso humano. Por favor vuelva a intentarlo mas tarde.</asp:Label>
    <asp:Label runat="server" AssociatedControlID="TextBoxCedulaRH" CssClass="text-danger" ID="EtiqErrorLlaves">*La cédula ingresada ya pertenece a un usuario de la aplicación. Por favor ingrese otra identificación.</asp:Label>
    <asp:Label runat="server" AssociatedControlID="TextBoxCedulaRH" CssClass="text-danger" ID="EtiqErrorModificar">*Ha habido problemas para modificar este recurso humano. Por favor vuelva a intentarlo.</asp:Label>
    <asp:Label runat="server" AssociatedControlID="TextBoxCedulaRH" CssClass="text-danger" ID="EtiqErrorEliminar">*Ha habido problemas para eliminar este recurso humano del sistema. Por favor vuelva a intentarlo.</asp:Label>

    <div class="form-group">
            <div class="col-md-offset-9 col-md-12">
        <div class="btn-group">
            <asp:Button runat="server" Text="Insertar" CssClass="btn btn-default" ID="BotonRHInsertar" OnClick="BotonRHInsertar_Click" CausesValidation="False" />
            <asp:Button runat="server" Text="Modificar" CssClass="btn btn-default" ID="BotonRHModificar" OnClick="BotonRHModificar_Click" CausesValidation="False" />
            <asp:Button runat="server" Text="Eliminar" CssClass="btn btn-default" ID="BotonRHEliminar" OnClick="BotonRHEliminar_Click" CausesValidation="False" />
        </div>
    </div>
    </div>

    <hr style="margin:50px;">
<div class="well">

 <legend><h5>Información del Recurso Humano</h5></legend>
    
    <div class="row">
       
            <div class="col-md-5" style="margin-left:15px">
                <div class="form-horizontal">
              
<div class="panel panel-primary">
  <div class="panel-heading">
    <h3 class="panel-title">Datos Personales</h3>
  </div>
  <div class="panel-body">
                        <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                            <p class="text-danger">
                                <asp:Literal runat="server" ID="FailureText" />
                            </p>
                        </asp:PlaceHolder>

                        <div class="form-group">
                            <div class="col-md-4">
                            <asp:Label runat="server" AssociatedControlID="TextBoxCedulaRH" CssClass="control-label" ID="Etiqueta1">Cédula*:</asp:Label>
                                 </div>
                            <div class="col-md-4">

                                <asp:TextBox runat="server" ID="TextBoxCedulaRH" Style="width: 250px" CssClass="form-control" MaxLength="10" onkeypress="check_txt(event)" placeholder="Formato: 000000000">.</asp:TextBox>
                                <%-- %><asp:Label runat="server" AssociatedControlID="TextBoxCedulaRH" CssClass="text-danger" ID="CedVal">*Por favor ingrese solo el numero de la cedula, sin guiones u otros simbolos.</asp:Label>--%>
                                <asp:RequiredFieldValidator ID="ValidaCampos"
                                    ControlToValidate="TextBoxCedulaRH"
                                    Display="Dynamic"
                                    ValidationGroup="CamposNoVacios"
                                    CssClass="text-danger"
                                    ErrorMessage="El campo de Cédula es obligatorio."
                                    runat="Server">
                                </asp:RequiredFieldValidator>
                                <script type="text/javascript">
                                    function check_txt(e) {
                                        if (!solo_numeros(e)) {
                                            if ($('#errorCedula').css('display') == 'none') {
                                                $('#errorCedula').fadeIn();
                                                $('#errorCedula').fadeOut(5000);
                                            }

                                            if (window.event)//IE
                                                e.returnValue = false;
                                            else//Firefox
                                                e.preventDefault();
                                        }                                        
                                    };
                                </script>
                                <div id="errorCedula" class="errorDiv" style="display: none">
                                    <asp:Label runat="server" CssClass="text-danger">*Este campo sólo acepta números</asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4">
                            <asp:Label runat="server" style="text-align:left" AssociatedControlID="TextBoxNombreRH" CssClass="control-label">Nombre completo*:</asp:Label>
                             </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="TextBoxNombreRH" Style="width: 250px" CssClass="form-control" MaxLength="49" onkeypress="check_txt1(event)" placeholder="Ingrese sólo letras."></asp:TextBox>
                                <%--<asp:Label runat="server" AssociatedControlID="TextBoxCedulaRH" CssClass="text-danger" ID="NombVal">*En este campo solo se permiten letras y espacios</asp:Label> --%>
                                <asp:RequiredFieldValidator ID="Requiredfieldvalidator1"
                                    ControlToValidate="TextBoxNombreRH"
                                    Display="Dynamic"
                                    ValidationGroup="CamposNoVacios"
                                    CssClass="text-danger"
                                    ErrorMessage="El campo de Nombre es obligatorio."
                                    runat="Server">
                                </asp:RequiredFieldValidator>
                                <script type="text/javascript">
                                    function check_txt1(e) {
                                        if (!solo_letras(e)) {
                                            if ($('#errorNombre').css('display') == 'none') {
                                                $('#errorNombre').fadeIn();
                                                $('#errorNombre').fadeOut(5000);
                                            }
                                            if (window.event)//IE
                                                e.returnValue = false;
                                            else//Firefox
                                                e.preventDefault();
                                        }
                                    };
                                </script>
                                <div id="errorNombre" class="errorDiv" style="display: none">
                                    <asp:Label runat="server" CssClass="text-danger">*Este campo sólo acepta Letras</asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-4">
                            <asp:Label runat="server" AssociatedControlID="TextBoxCedulaRH" CssClass="control-label">Teléfono 1:</asp:Label>
                                </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="TextBoxTel1" Style="width: 250px" CssClass="form-control" Columns="8" MaxLength="8" onkeypress="check_txt3(event)" placeholder="Formato: 00000000" />
                                <script type="text/javascript">
                                    function check_txt3(e) {
                                        if (!solo_numeros(e)) {
                                            if ($('#errorTel1').css('display') == 'none') {
                                                $('#errorTel1').fadeIn();
                                                $('#errorTel1').fadeOut(5000);
                                            }
                                            if (window.event)//IE
                                                e.returnValue = false;
                                            else//Firefox
                                                e.preventDefault();
                                        }
                                    };
                                </script>
                                <div id="errorTel1" style="display: none">
                                    <asp:Label runat="server" AssociatedControlID="TextBoxCedulaRH" CssClass="text-danger" ID="TelVal1">*Por favor ingrese un teléfono valido.</asp:Label>
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-md-4">
                            <asp:Label runat="server" AssociatedControlID="TextBoxCedulaRH" CssClass="control-label">Teléfono 2:</asp:Label>
                                </div>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="TextBoxTel2" Style="width: 250px" CssClass="form-control" MaxLength="8" onkeypress="check_txt4(event)" placeholder="Formato: 00000000" />
                                <script type="text/javascript">
                                    function check_txt4(e) {
                                        if (!solo_numeros(e)) {
                                            if ($('#errorTel2').css('display') == 'none') {
                                                $('#errorTel2').fadeIn();
                                                $('#errorTel2').fadeOut(5000);
                                            }
                                            if (window.event)//IE
                                                e.returnValue = false;
                                            else//Firefox
                                                e.preventDefault();
                                        }
                                    };
                                </script>
                                <div id="errorTel2" style="display: none">
                                    <asp:Label runat="server" AssociatedControlID="TextBoxCedulaRH" CssClass="text-danger">*Por favor ingrese un teléfono valido.</asp:Label>
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-md-4">
                            <asp:Label runat="server" AssociatedControlID="TextBoxCedulaRH" CssClass="col-md-2 control-label">Email:</asp:Label>
                                </div>
                            <div class="col-md-4" style="margin-bottom:0px">
                                <asp:TextBox runat="server" ID="TextBoxEmail" Style="width: 250px" CssClass="form-control" MaxLength="30" placeholder="Formato: xxx@yyy.com" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="*Error en el formato del email" ControlToValidate="TextBoxEmail" 
                                    CssClass="text-danger" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                <%--<asp:Label runat="server" AssociatedControlID="TextBoxCedulaRH" CssClass="text-danger" ID="EmailVal">*Por favor ingrese un email valido valido.</asp:Label> --%>
                            </div>
                        </div>
 </div>
 </div>
                                    
                </div>
            </div>
       
        
        <div class=" col-md-offset-1 col-md-5">

                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-4">
                        <asp:Label runat="server" AssociatedControlID="TextBoxCedulaRH" CssClass="control-label">Nombre de usuario*:</asp:Label>
                            </div>
                        <div class="col-md-4">
                            <asp:TextBox runat="server" ID="TextBoxUsuario" CssClass="form-control" Style="width: 270px" MaxLength="30" placeholder="Ingrese sólo letras y números" onkeypress="check_txt5(event)"/>
                            <%--<asp:Label runat="server" AssociatedControlID="TextBoxCedulaRH" CssClass="text-danger" ID="UserVal">*Por favor ingrese un usuario valido.</asp:Label>--%>
                            <asp:RequiredFieldValidator ID="Requiredfieldvalidator3"
                                ControlToValidate="TextBoxUsuario"
                                Display="Dynamic"
                                ValidationGroup="CamposNoVacios"
                                CssClass="text-danger"
                                ErrorMessage="El campo de Nombre de Usuario es obligatorio."
                                runat="Server">
                            </asp:RequiredFieldValidator>
                            <script type="text/javascript">
                                function check_txt5(e) {
                                    if (!solo_letras(e)) {
                                        if ($('#errorUsuario').css('display') == 'none') {
                                            $('#errorUsuario').fadeIn();
                                            $('#errorUsuario').fadeOut(5000);
                                        }
                                        if (window.event)//IE
                                            e.returnValue = false;
                                        else//Firefox
                                            e.preventDefault();
                                    }
                                };
                                </script>
                                <div id="errorUsuario" style="display: none">
                                    <asp:Label runat="server" AssociatedControlID="TextBoxCedulaRH" CssClass="text-danger">*Por favor ingrese un usuario valido.</asp:Label>
                                </div>
                        </div>
                    </div>

                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="TextBoxCedulaRH" CssClass="col-md-4 control-label">Contraseña*:</asp:Label>
                        <div class="col-md-8">
                            <asp:TextBox runat="server" ID="TextBoxClave" CssClass="form-control" MaxLength="12" placeholder="Ingrese sólo letras y números" onkeypress="check_txt6"/>
                            <%-- <asp:Label runat="server" AssociatedControlID="TextBoxCedulaRH" CssClass="text-danger" ID="ClaveVal">*Por favor ingrese una contraseña valida.</asp:Label> --%>
                            <asp:RequiredFieldValidator
                                ControlToValidate="TextBoxClave"
                                Display="Dynamic"
                                ValidationGroup="CamposNoVacios"
                                CssClass="text-danger"
                                ErrorMessage="El campo de Contraseña es obligatorio."
                                runat="Server">
                            </asp:RequiredFieldValidator>
                            <script type="text/javascript">
                                function check_txt6(e) {
                                    if (!solo_letras(e)) {
                                        if ($('#errorPass').css('display') == 'none') {
                                            $('#errorPass').fadeIn();
                                            $('#errorPass').fadeOut(5000);
                                        }
                                        if (window.event)//IE
                                            e.returnValue = false;
                                        else//Firefox
                                            e.preventDefault();
                                    }
                                };
                                </script>
                                <div id="errorPass" style="display: none">
                                    <asp:Label runat="server" AssociatedControlID="TextBoxCedulaRH" CssClass="text-danger">*Por favor ingrese un usuario valido.</asp:Label>
                                </div>
                        </div>
                    </div>

                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="TextBoxCedulaRH" CssClass="col-md-4 control-label">Tipo de perfil</asp:Label>
                        <div class="col-md-8">
                            <asp:DropDownList ID="PerfilAccesoComboBox" runat="server" OnSelectedIndexChanged="PerfilAccesoComboBox_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="TextBoxCedulaRH" CssClass="col-md-4 control-label">Rol</asp:Label>
                        <div class="col-md-8">
                            <asp:DropDownList ID="RolComboBox" runat="server" OnSelectedIndexChanged="RolComboBox_SelectedIndexChanged" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>

            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="TextBoxCedulaRH" CssClass="col-md-4 control-label">Proyecto Asociado</asp:Label>
                <div class="col-md-8">
                    <asp:DropDownList ID="ProyectoAsociado" runat="server" OnSelectedIndexChanged="ProyectoAsociado_SelectedIndexChanged" CssClass="form-control">
                    </asp:DropDownList>
                </div>
            </div>

        </div>
            </div>
    </div>
     <div class="row">
    <div class="form-group col-md-offset-9 col-md-12">
        <asp:Label runat="server" id="CamposObligarotios" Text="Campos Obligatorios*" style="color: #C0C0C0;" CssClass="control-label"></asp:label>
    </div>
         </div>
    </div>

    <div class="form-group">
        <div class="col-md-offset-9 col-md-12">
            <asp:Button runat="server" Style="border-color: #4bb648; color: #4bb648"
                Text="Aceptar"
                CausesValidation="True"
                ValidationGroup="CamposNoVacios"
                CssClass="btn btn-default"
                ID="BotonRHAceptar"
                OnClick="BotonRHAceptar_Click" />
            <asp:Button runat="server" Style="border-color: #4bb648; color: #4bb648"
                Text="Aceptar"
                CausesValidation="True"
                ValidationGroup="CamposNoVacios"
                CssClass="btn btn-default"
                ID="BotonRHAceptarModificar" OnClick="BotonRHAceptarModificar_Click" />
            <asp:Button runat="server" Text="Cancelar" Style="border-color: #fe6c4f; color: #fe5e3e" CssClass="btn btn-default" ID="BotonRHCancelar" OnClick="BotonRHCancelar_Click" CausesValidation="False" />
            <asp:Panel runat="server" ID="cancelarPanelModal" CssClass="modalPopup">
                         <legend style="margin-top:15px"><h5>¿Desea cancelar la operación?</h5></legend>

                <div aria-pressed="true">
                    <asp:Button runat="server" ID="cancelarButtonSiModal" Text="Si" OnClick="cancelar_Click" CssClass="btn btn-primary" Style="align-self:center;margin-left:8px;margin-right:11px;margin-bottom:20px;  width:85px" CausesValidation="false" />
                    <asp:Button runat="server" ID="cancelarButtonNoModal" Text="No" CssClass="btn btn-default" Style="border-color:#fe6c4f;color:#fe5e3e;align-self:center;margin-left:11px;margin-right:6px;margin-bottom:20px;  width:85px" CausesValidation="false" />
                </div>
            </asp:Panel>
            <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" BackgroundCssClass="modalBackground" PopupControlID="cancelarPanelModal" TargetControlID="BotonRHCancelar" OnCancelScript="cancelarButtonNoModal" OnOkScript="cancelarButtonSiModal">
            </ajaxToolkit:ModalPopupExtender>


        </div>
    </div>

    <div class="row">

        <asp:GridView ID="RH" runat="server" margin-right="auto"
            CellPadding="10"
            margin-left="auto" OnSelectedIndexChanged="RH_SelectedIndexChanged"
            OnRowDataBound="OnRowDataBound" CssClass="GridView" HorizontalAlign="Center"
            AllowPaging="true" OnPageIndexChanging="OnPageIndexChanging" PageSize="5"
            HeaderStyle-BackColor="#48cfae" HeaderStyle-ForeColor="#ffffff" BorderColor="#cdcdcd" border-radius="15px"
            AutoPostBack="true" RowStyle-BackColor="White" PagerStyle-BackColor="White" Width="90%">
        </asp:GridView>

    </div>
    <div>
        <asp:Panel runat="server" ID="panelModal" CssClass="modalPopup" Style="display:none">
            <legend style="margin-top:15px"><h5>¿Desea eliminar este Recurso Humano?</h5></legend>
            <div aria-pressed="true" style="padding-left:30px">
                <asp:Button runat="server" ID="aceptarModal" Text="Eliminar" OnClick="aceptarModal_Click" CssClass="btn btn-primary" style="align-self:center;margin-left:16px;margin-right:11px;margin-bottom:20px;  width:85px" CausesValidation="false" />
                <asp:Button runat="server" ID="cancelarModal" Text="Cancelar" OnClick="cancelarModal_Click" CssClass="btn btn-default" style="border-color:#fe6c4f;color:#fe5e3e;align-self:center;margin-left:11px;margin-right:6px;margin-bottom:20px;  width:85px" />
            </div>
        </asp:Panel>
        <ajaxToolkit:ModalPopupExtender ID="ModalEliminar" runat="server" BackgroundCssClass="modalBackground" PopupControlID="panelModal" TargetControlID="BotonRHEliminar" OnCancelScript="cancelarModal" OnOkScript="aceptarModal">
        </ajaxToolkit:ModalPopupExtender>
    </div>
</asp:Content>

    
