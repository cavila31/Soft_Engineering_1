<%@ Page Title="Gestión de Proyectos" EnableEventValidation="false"  Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InterfazProyecto.aspx.cs" Inherits="SistemaPruebas.Intefaces.InterfazProyecto" Async="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <legend style="margin-top:45px"><h2>Módulo de Gestión de Proyectos</h2></legend>
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

                $('#'+'<%=EtiqErrorLlaves.ClientID %>').fadeOut('5000');
            }, 2000);           
    };
</script>
  
            <div>
                <asp:Label runat="server" CssClass="text-danger" ID="EtiqErrorLlaves" Font-Size="Large" Visible="False"></asp:Label>

            </div>    

    
               
                <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                    <p class="text-danger">
                        <asp:Literal runat="server" ID="FailureText" />
                    </p>
                </asp:PlaceHolder>

    <div class="form-group">
            <div class="col-md-offset-9 col-md-12">
        <div class="btn-group">
            <asp:Button runat="server" ID="Insertar" Text="Insertar" CssClass="btn btn-default" OnClick="Insertar_button" CausesValidation="false" />
            <asp:Button runat="server" ID="Modificar" Text="Modificar" CssClass="btn btn-default" OnClick="Modificar_Click"  CausesValidation="false"/>
            <asp:Button runat="server" ID="Eliminar" Text="Eliminar" CssClass="btn btn-default" OnClick="Eliminar_Click" CausesValidation="false"/>
        </div>
    </div>
    </div>

    <script type="text/javascript">
    $(document).ready(function () {
        $("[id$=A1]").addClass("active");
    });
</script>


<hr style="margin:50px;">
<div class="well">

 <legend><h5>Información del Proyecto</h5></legend>

    <div class="row">
        <div class="col-md-7">

            <div class="form-horizontal">

                <div class="form-group">
                    <div class="col-md-4">
                    <asp:Label runat="server" ID="nombre_label" CssClass=" control-label">Nombre del Proyecto*</asp:Label>
                      </div>
                    <div class="col-md-1">
                        <asp:TextBox runat="server" ID="nombre_proyecto" style="width:250px;height:36px;" CssClass="form-control" onkeypress="solo_letras(event)" OnTextChanged="nombre_proyecto_TextChanged" placeholder="Sólo letras."/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Campo requerido" ControlToValidate="nombre_proyecto" ForeColor="Salmon"></asp:RequiredFieldValidator>
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
                        <div id="errorNombreSistema" style="display:none">
                            <asp:Label runat="server" ID="errorNombreSistLbl" text="Sólo se permite el ingreso de letras" ForeColor="Salmon"></asp:Label>
                        </div>                                            
                    </div>
                </div>

                <div class="form-group">
                     <div class="col-md-4">
                    <asp:Label runat="server" columns="3" CssClass="control-label" >Objetivo General*</asp:Label>
                         </div>
                    <div class="col-md-2">
                        <asp:TextBox runat="server" ID="obj_general" style="width:250px; height:90px" CssClass="form-control" TextMode="multiline" onkeypress="solo_letrasYNumeros(event)" placeHolder="Sólo letras."/>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Campo requerido" ControlToValidate="obj_general" ForeColor="Salmon"></asp:RequiredFieldValidator>
                        <script type="text/javascript">
                            function solo_letrasYNumeros(evt) {
                                if ((evt.charCode < 65 || evt.charCode > 90) && (evt.charCode < 97 || evt.charCode > 122)) {
                                    if ((evt.keyCode != 32) && (evt.charCode != 32) && (evt.charCode != 46) && (evt.charCode != 44) && (evt.keyCode != 13) && (evt.keyCode != 37) && (evt.keyCode != 39) && (evt.keyCode != 8) && (evt.keyCode != 83)) {
                                        if ($('#errorObjSistema').css('display') == 'none') {
                                            //alert(evt.charCode);
                                            $('#errorObjSistema').fadeIn();
                                            $('#errorObjSistema').fadeOut(6000);
                                        }
                                        if (window.event)//IE
                                            evt.returnValue = false;
                                        else//Firefox
                                            evt.preventDefault();

                                    }
                                }
                            }
                        </script>
                        <div id="errorObjSistema" style="display:none">
                            <asp:Label runat="server" ID="errorObjSistemalbl" text="Este campo sólo acepta letras" ForeColor="Salmon"></asp:Label>
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-md-4">
                    <asp:Label runat="server" CssClass="col-md-2 control-label">Estado</asp:Label>
                        </div>
                    <div class="col-md-2">
                        <asp:DropDownList runat="server" ID="estado" style="width:250px" CssClass="form-control">
                            <asp:ListItem Value="1">Pendiente</asp:ListItem>
                            <asp:ListItem Value="2">Asignado</asp:ListItem>
                            <asp:ListItem Value="3">En Ejecución</asp:ListItem>
                            <asp:ListItem Value="4">Finalizado</asp:ListItem>
                            <asp:ListItem Value="5">Cerrado</asp:ListItem>
                        </asp:DropDownList>

                    </div>
                </div>

                <div class="form-group">
                     <div class="col-md-4">
                    <asp:Label runat="server" CssClass="control-label">Fecha de Asignación</asp:Label>
                         </div>
                    <div class="col-md-2" runat="server">
                        <asp:TextBox runat="server" id="txt_date" Style="width: 250px; height: 36px" CssClass="form-control" placeholder="De click para seleccionar fecha"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txt_date" PopupButtonID="txt_date"/>
                    </div>
                    
                         
                </div>

                                <div class="form-group">
                                    <div class="col-md-4">
                     <asp:Label runat="server" id="LiderLbl" Text="Lider del proyecto" CssClass="control-label"></asp:label>
                                        </div>
                        <div class="col-md-2">
                     <asp:DropDownList ID="LiderProyecto" runat="server" style="width:250px" CssClass="form-control"></asp:DropDownList>                         
                            </div>
                 </div>

            </div>
        </div>


        
        <div class="col-md-5">

            <div class="form-horizontal">

<div class="panel panel-primary" style="margin-right:20px">

      <div class="panel-heading">
    <h3 class="panel-title">Información de la oficina usuaria</h3>
  </div>

  <div class="panel-body">
                    <div class="form-group">
                    <asp:Label runat="server" CssClass="col-md-4 control-label">Nombre de la oficina</asp:Label>
                    <div class="col-md-6">
                        <asp:TextBox runat="server" ID="of_rep" style="width:250px;height:36px" CssClass="form-control" onkeypress="solo_letras2(event)" MaxLength="17" placeholder="Sólo recibe letras y espacios"/>
                        <script type="text/javascript">
                            function solo_letras2(evt) {
                                if ((evt.charCode < 65 || evt.charCode > 90) && (evt.charCode < 97 || evt.charCode > 122)) {
                                    if ((evt.keyCode != 32) && (evt.charCode != 32) && (evt.charCode != 46) && (evt.keyCode != 13) && (evt.keyCode != 37) && (evt.keyCode != 39) && (evt.keyCode != 8) && (evt.keyCode != 83) && (evt.charCode != 44)) {
                                        //alert(evt.charCode);
                                        if ($('#errorNombreOficina').css('display') == 'none') {
                                            $('#errorNombreOficina').fadeIn();
                                            $('#errorNombreOficina').fadeOut(6000);
                                        }
                                        if (window.event)//IE
                                            evt.returnValue = false;
                                        else//Firefox
                                            evt.preventDefault();
                                    }
                                }
                            }
                        </script>
                        <div id="errorNombreOficina" style="display:none">
                            <asp:Label runat="server" ID="errorNombreOficinaLbl" text="Sólo se permite el ingreso de letras" ForeColor="Salmon"></asp:Label>
                        </div>
                    </div>
                </div>

                <div class="form-group">

                    <div class="col-md-4">
                        <asp:label runat="server" style="margin-left:15px" id="tel1Label" text="Teléfono 1:"></asp:label>
                    </div>
                    <div class="col-md-6">
                        <asp:TextBox runat="server" ID="tel_rep" style="width:250px;height:36px;margin-bottom:10px" CssClass="form-control" onkeyDown="check_txt(this,event,8)" placeholder="Formato: 00000000"/>
                        <asp:RegularExpressionValidator Display ="Dynamic" ControlToValidate="tel_rep" ID="RegularExpressionValidator3" ValidationExpression = "^(\d{8})|()$" runat="server" 
                            foreColor="Salmon" ErrorMessage="Debe digitar 8 números."></asp:RegularExpressionValidator>
                        <script type="text/javascript">
                            function check_txt(textBox, e, length) {
                                if (!checkSpecialKeys(e)) {
                                    if ($('#errorTel1').css('display') == 'none') {
                                        $('#errorTel1').fadeIn();
                                        $('#errorTel1').fadeOut(6000);
                                    }
                                    if (window.event)//IE
                                        e.returnValue = false;
                                    else//Firefox
                                        e.preventDefault();
                                }
                                else
                                    $('#errorTel1').fadeOut();
                            }
                            function checkSpecialKeys(e) {
                                if ((e.keyCode < 48 || e.keyCode > 57) && ((e.keyCode < 96 || e.keyCode > 105)) && e.keyCode != 8 && e.keyCode != 127 && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 13)
                                    return false;
                                else
                                    return true;
                            }
                        </script>
                        <div id="errorTel1" Style="display:none">
                            <asp:label runat="server" ID="errorTel1Txt" visible="true" Enabled="true" Text="Este campo sólo recibe números" ForeColor="Salmon" ></asp:label>
                        </div>
                        </div>
                    <div class="col-md-4">
                        <asp:label runat="server" id="tel2Label" text="Teléfono 2:" style="margin-top:20px; margin-left:15px"></asp:label>
                        </div>
                    <div class="col-md-6">
                        <asp:TextBox runat="server" ID="tel_rep2" style="width:250px;height:36px" CssClass="form-control" onkeypress="check_txt2(this,event,8)" MaxLength="8" CausesValidation="True" placeholder="Complete primero el Tél. 1"/>
                        <asp:RegularExpressionValidator Display = "Dynamic"  ControlToValidate = "tel_rep2" ID="RegularExpressionValidator1" ValidationExpression = "^(\d{8})|()$" runat="server" ErrorMessage="Debe digitar 8 números." ForeColor="Salmon"></asp:RegularExpressionValidator>
                        <script type="text/javascript">
                            function check_txt2(textBox, e, length) {
                                if (document.getElementById('<%=tel_rep.ClientID%>').value.length != 8) {
                                    if ($('#errorTel2').css('display') == 'none') {
                                        //document.getElementById("errorTel2Lbl").innerHTML = "Primero complete el campo de télefono 1";
                                        document.getElementById('<%=errorTel2Lbl.ClientID%>').innerHTML = "Primero complete el campo de télefono 1";
                                        $('#errorTel2').fadeIn();
                                        $('#errorTel2').fadeOut(5000);
                                    }
                                    if (window.event)//IE
                                        e.returnValue = false;
                                    else//Firefox
                                        e.preventDefault();
                                }
                                else {
                                    if ((e.charCode < 48 || e.charCode > 57) && (e.keyCode < 96 || e.keyCode > 105)) {
                                        if ((e.keyCode != 37) && (e.keyCode != 39) && (e.keyCode != 8) && (e.keyCode != 83) && (e.keyCode != 46) && (e.keyCode != 13)) {
                                            //alert(e.charCode);
                                            if ($('#errorTel2').css('display') == 'none') {
                                                document.getElementById('<%=errorTel2Lbl.ClientID%>').innerHTML = "Este campo sólo recibe números";
                                                $('#errorTel2').fadeIn();
                                                $('#errorTel2').fadeOut(5000);
                                            }
                                            if (window.event)//IE
                                                e.returnValue = false;
                                            else//Firefox
                                                e.preventDefault();
                                        }

                                    }
                                }
                            }
                        </script>
                        <div id="errorTel2" Style="display:none">
                            <asp:label runat="server" ID="errorTel2Lbl" visible="true" Enabled="true" Text="Este campo sólo recibe números" ForeColor="Salmon" ></asp:label>
                        </div>
                    </div>                
                </div>                                
                <div class="form-group" style="margin-top:40px">
                    <asp:Label runat="server" CssClass="col-md-4 control-label">Nombre del representante</asp:Label>
                    <div class="col-md-6">
                        <asp:TextBox runat="server" style="width:250px;height:36px"  ID="nombre_rep" CssClass="form-control" onkeypress="solo_letras3(event)" MaxLength="30" placeHolder="Sólo recibe letras y espacios"/>
                        <script type="text/javascript">
                            function solo_letras3(evt) {
                                if ((evt.charCode < 65 || evt.charCode > 90) && (evt.charCode < 97 || evt.charCode > 122)) {
                                    if ((evt.keyCode != 32) && (evt.charCode != 32) && (evt.charCode != 46) && (evt.charCode != 44) && (evt.keyCode != 13) && (evt.keyCode != 37) && (evt.keyCode != 39) && (evt.keyCode != 8) && (evt.keyCode != 83) && (evt.charCode != 44)) {
                                        //alert(evt.charCode);
                                        if ($('#errorNombreUsuario').css('display') == 'none') {
                                            $('#errorNombreUsuario').fadeIn();
                                            $('#errorNombreUsuario').fadeOut(5000);
                                        }
                                        if (window.event)//IE
                                            evt.returnValue = false;
                                        else//Firefox
                                            evt.preventDefault();
                                    }
                                }
                            }
                        </script>
                        <div id="errorNombreUsuario" style="display:none">
                            <asp:Label runat="server" ID="errorNombreUsuarioLbl" text="Sólo se permite el ingreso de letras y espacios" ForeColor="Salmon"></asp:Label>
                        </div>
                    </div>
                </div>
  </div>
</div>
            
            </div>
        </div>

            <div class="form-group col-md-offset-9 col-md-3">
        <asp:Label runat="server" id="CamposObligarotios" Text="Campos Obligatorios*" style="color: #C0C0C0;" CssClass="control-label"></asp:label>
    </div>
    </div>
    
</div>


    <div class="form-group">
        <div id="Botones_aceptar_cancelar" class="col-md-offset-9 col-md-12">
            <asp:Button runat="server" ID="aceptar" Text="Aceptar" CssClass="btn btn-default" OnClick="aceptar_Click" style="border-color:#4bb648;color:#4bb648; align-self:center;margin-left:8px;margin-right:6px;margin-bottom:20px;" CausesValidation="true"/>
            <asp:Button runat="server" ID="cancelar" Text="Cancelar" style="border-color:#fe6c4f;color:#fe5e3e; align-self:center;margin-left:11px;margin-right:6px;margin-bottom:20px;" CssClass="btn btn-default" OnClick="cancelar_Click" CausesValidation="False" />           
            <asp:Panel runat="server" ID="cancelarPanelModal" CssClass="modalPopup"> 
                             <legend style="margin-top:15px"><h5>¿Desea cancelar la operación?</h5></legend>

        <div aria-pressed="true">
            <asp:button runat="server" ID="cancelarButtonSiModal" Text="Si" OnClick="cancelar_Click" CssClass="btn btn-primary" style="margin-left:8px;margin-right:11px;margin-bottom:20px; width:85px" CausesValidation="false"/>
            <asp:button runat="server" ID="cancelarButtonNoModal" Text="No" OnClick="no_cancelar_Click" CssClass="btn btn-default" style="border-color:#fe6c4f;color:#fe5e3e;align-self:center;margin-left:11px;margin-right:6px;margin-bottom:20px; width:85px" CausesValidation="false"/>           
       </div>
    </asp:Panel>
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" BackgroundCssClass="modalBackground" PopupControlID="cancelarPanelModal" TargetControlID="cancelar" OnCancelScript="cancelarButtonNoModal" OnOkScript="cancelarButtonSiModal">
    </ajaxToolkit:ModalPopupExtender>
        </div>
    </div>

    <div id="tablaProyectos" class="row">

        <asp:GridView ID="gridProyecto" runat="server" HeaderStyle-BackColor="#48cfae" RowStyle-BackColor="White" PagerStyle-BackColor="White" Width="90%"   HeaderStyle-ForeColor="#ffffff" CellPadding="10"  HorizontalAlign="Center" 
            AutoGenerateColumns="false" OnSelectedIndexChanged="OnSelectedIndexChanged" BorderColor="#cdcdcd" border-radius="7px" 
            AllowPaging="true" OnPageIndexChanging="OnPageIndexChanging" AllowSorting="true" PageSize="5"   OnRowDataBound ="OnRowDataBound" AutoPostBack ="true" CssClass="control-label" CausesValidation="false" CaptionAlign="Right" CellSpacing="5">
            <Columns>
                <asp:BoundField DataField="Id Proyecto" ItemStyle-Width="185px" HeaderText=" Id Proyecto"  />
                <asp:TemplateField ItemStyle-Width="200px" HeaderText=" Nombre del sistema">
                    <ItemTemplate>
                        <asp:Label ID="lblNombre" runat="server" Text='<%# Eval("Nombre del sistema") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Lider del Proyecto" ItemStyle-Width="185px" HeaderText=" Lider del proyecto"  />
                </Columns>

            </asp:GridView>
    </div>
    
    <asp:Panel runat="server" ID="panelModal" CssClass="modalPopup"> 
        <legend style="margin-top:15px"><h5>¿Desea eliminar este proyecto?</h5></legend>
        <div aria-pressed="true" style="padding-left:30px">
            <asp:button runat="server" ID="aceptarModal" Text="Eliminar" OnClick="aceptarModal_Click" CssClass="btn btn-primary" style="align-self:center;margin-left:16px;margin-right:11px;margin-bottom:20px"/>
            <asp:button runat="server" ID="cancelarModal" Text="Cancelar" OnClick="cancelarModal_Click" CssClass="btn btn-default" style="border-color:#fe6c4f;color:#fe5e3e;align-self:center;margin-left:11px;margin-right:6px;margin-bottom:20px"/>           
       </div>
    </asp:Panel>
    <ajaxToolkit:ModalPopupExtender ID="ModalEliminar" runat="server" BackgroundCssClass="modalBackground" PopupControlID="panelModal" TargetControlID="Eliminar" OnCancelScript="cancelarModal" OnOkScript="aceptarModal">
    </ajaxToolkit:ModalPopupExtender>
</asp:Content>

