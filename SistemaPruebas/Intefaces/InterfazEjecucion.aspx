<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InterfazEjecucion.aspx.cs" Inherits="SistemaPruebas.Intefaces.InterfazEjecucion" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
 
<script type="text/javascript">
        $(document).ready(function () {
            $("[id$=A5]").addClass("active");
        });
</script>

<legend style="margin-top:45px"><h2><%: Title %>Módulo Ejecución de Prueba</h2></legend>

<asp:Label runat="server" CssClass="text-danger" ID="EtiqMensajeOperacion" Visible ="false" ></asp:Label>

<asp:Panel ID= "BotonesPrincipales" runat="server" CssClass="form-group">
    <div class="col-md-offset-9 col-md-12">
        <div class="btn-group">
        <asp:Button runat="server" Text="Insertar" CssClass="btn btn-default"  ID="BotonEPInsertar"    CausesValidation="false" OnClick="BotonEPInsertar_Click"/>
        <asp:Button runat="server" Text="Modificar" CssClass="btn btn-default" ID="BotonEPModificar"   CausesValidation ="false" OnClick="BotonEPModificar_Click"/>
        <asp:Button runat="server" Text="Eliminar" CssClass="btn btn-default"  ID="BotonEPEliminar"    CausesValidation="false" OnClick="BotonEPEliminar_Click"/>
    </div>
    </div>
</asp:Panel>
<hr style="margin:50px;">

  <div class="panel panel-primary">
  <div class="panel-heading">
    <h3 class="panel-title">Selección del origen</h3>
  </div>
  <div class="panel-body">
     <div class ="row">
        <div class ="col-md-2" style="text-align:center">
            <asp:Label ID="LabelProyecto" runat="server" Text="Proyecto:" CssClass = "col-md-2 control-label"></asp:Label>
        </div>
        <div class ="col-md-4">
            <asp:DropDownList ID="DropDownProyecto" runat="server" CssClass ="form-control"  style="width:250px" OnSelectedIndexChanged="DropDownProyecto_SelectedIndexChanged" AutoPostBack="true" >
                <asp:ListItem Text ="Seleccionar" Value =1/>
            </asp:DropDownList>
        </div>
        <div class ="col-md-2">
             <asp:Label ID="LabelDiseno" runat="server" Text="Diseño:" CssClass = "col-md-2 control-label"></asp:Label>
        </div>
        <div class ="col-md-4">
              <asp:DropDownList ID="DropDownDiseno" runat="server" CssClass ="form-control" style="width:250px" AutoPostBack ="true" OnSelectedIndexChanged="DropDownDiseno_SelectedIndexChanged" >
                 <asp:ListItem Text ="Seleccionar" Value =1/>
              </asp:DropDownList>
        </div>
    </div>  
  </div>
</div> 

<div class="well" style="margin-bottom:0px">
 <legend><h4>Ejecución de Prueba</h4></legend>
 <asp:Panel ID="DatosEjecucion" runat ="server">
    <div class ="row" >
       <div class="form-horizontal">
            <div class ="form-group">
                <div class="col-md-2">
                    <asp:Label ID="ResponsableEP" runat="server" CssClass = "col-md-1 control-label" >Responsable:</asp:Label>  
                </div>
                <div class ="col-md-4">
                    <asp:DropDownList ID="DropDownResponsable" runat="server" CssClass ="form-control" style="width:250px">
                        <asp:ListItem Text ="Seleccionar" Value =1/>
                    </asp:DropDownList>
                </div>
           </div>

           <div class ="form-group">
               <div class ="col-md-2">
                    <asp:Label ID="FechaEP" runat="server" CssClass = "col-md-6 control-label" >Fecha:</asp:Label> 
                </div>
                   <asp:ImageButton ID="imgPopup" ImageUrl="~/Imagenes/calendar.png" runat="server" CausesValidation="false"/>
                <div class ="col-md-2">
                   <asp:TextBox runat="server" ID="ControlFecha" CssClass="form-control" ></asp:TextBox>
                   <asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                       runat="server" ErrorMessage="Campo requerido" ControlToValidate="ControlFecha" ForeColor="Salmon"></asp:RequiredFieldValidator>
                   <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgPopup" TargetControlID="ControlFecha" />
                </div>            
           </div>

           <div class ="form-group">
                <div class="col-md-2">
                    <asp:Label ID="Incidencias" runat="server" CssClass="col-md-2 control-label" Text ="Incidencias:"></asp:Label> 
                </div>  
                <div class ="col-md-9">
                    <asp:TextBox runat="server" ID="TextBoxIncidencias" CssClass="form-control" MaxLength="300" TextMode="multiline" onkeypress="checkInput1(event)" Style="height: 90px"/>
                    <script type="text/javascript">
                        function checkInput1(e) {
                            var ok = /[A-Za-z0-9.,:; ]/.test(String.fromCharCode(e.charCode));
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
                    <div id="errorNombreSistema" style="display:none">
                            <asp:Label runat="server" ID="errorNombreSistLbl0" text="Sólo se permite el ingreso de letras, espacios y números." ForeColor="Salmon"></asp:Label>
                    </div>   
                    </div>
           </div>


      <br />

           <div class ="row">
               <asp:GridView runat ="server" ID ="gridNoConformidades" OnRowDataBound ="gridNoConformidades_RowDataBound"  AutoGenerateColumns="false"
                   CssClass ="GridView" HorizontalAlign="Center" OnRowCommand="gridNoConformidades_RowCommand" HeaderStyle-BackColor="#48cfae" RowStyle-BackColor="White"  HeaderStyle-ForeColor="#ffffff" BorderColor="#cdcdcd"  HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" RowStyle-HorizontalAlign="Center" RowStyle-VerticalAlign="Top" EditRowStyle-Height="40px">
                   <Columns>
                       <%--<asp:BoundField DataField="RowNumber" HeaderText="Row Number" Visible="false" />--%>
                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                        <ItemTemplate>
                            <asp:Button ID ="btnEliminarFila" runat="server" text="X" OnClick="btnEliminarFila_Click" CausesValidation="false" CssClass="btn btn-default" style="border-color:#fe6c4f;background-color:#fe5e3e;font-weight:bold; color:white" ></asp:Button>
                            <asp:Label runat="server" ID="lblId" Text='<%# Bind("Id") %>' Visible="false"></asp:Label>
                            <asp:Label runat="server" ID="lblIDNC" Text="0" Visible="false"></asp:Label>
                        </ItemTemplate>
                     </asp:TemplateField>
                     <asp:TemplateField HeaderText="Tipo de no conformidad">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblTipo" Visible="false" Text='<%# Eval("Tipo") %>'></asp:Label>
                            <asp:DropDownList ID="ddlTipo" runat="server" ClientIDMode="Static" CssClass="form-control" Style="width: 210px"> 
                                <asp:ListItem Text="Seleccionar" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Funcionalidad" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Validación" Value="3"></asp:ListItem>
                                <asp:ListItem Text="Opciones que no funcionan" Value="4"></asp:ListItem>
                                <asp:ListItem Text="Errores de usabilidad" Value="5"></asp:ListItem>
                                <asp:ListItem Text="Excepciones" Value="6"></asp:ListItem>
                                <asp:ListItem Text="Implementación diferente a documentación"></asp:ListItem>
                                <asp:ListItem Text="Ortografía" Value="7"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvTipo" ValidationGroup="grupo" 
                                runat="server" ErrorMessage="Campo requerido" ControlToValidate="ddlTipo" InitialValue="1" ForeColor="Salmon"></asp:RequiredFieldValidator>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Id Caso de Prueba">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblIdCaso" Visible="false" Text='<%# Eval("IdCaso") %>'></asp:Label>
                            <asp:DropDownList ID="ddlIdCaso" runat="server" ClientIDMode="Static" CssClass="form-control" Style="width: 165px"> </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvCasoPrueba" ValidationGroup="grupo" 
                                runat="server" ErrorMessage="Campo requerido" ControlToValidate="ddlIdCaso" InitialValue="Seleccionar" ForeColor="Salmon"></asp:RequiredFieldValidator>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Descripción">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblDescripcion" Visible="false"></asp:Label>
                            <asp:TextBox ID="txtDescripcion" runat="server" Style="height: 37px; width: 220px; margin-bottom:0px" Text='<%# Eval("Descripcion") %>' CssClass="form-control" TextMode="multiline" onkeypress="checkInput2(event)"  ></asp:TextBox>
                            <script type="text/javascript">
                                function checkInput2(e) {
                                    var ok = /[A-Za-z0-9.,:; ]/.test(String.fromCharCode(e.charCode));
                                    if (e.keyCode == 8) {
                                        //alert();
                                    }
                                    else if (!ok) {
                                        if ($('#errorNombreSistema1').css('display') == 'none') {
                                            $('#errorNombreSistema1').fadeIn();
                                            $('#errorNombreSistema1').fadeOut(6000);
                                        }
                                        if (window.event)//IE
                                            e.returnValue = false;
                                        else//Firefox
                                            e.preventDefault();
                                    }
                                }
                        </script>
                        <div id="errorNombreSistema1" style="display:none">
                                <asp:Label runat="server" ID="errorNombreSistLbl" text="Solo letras." ForeColor="Salmon"></asp:Label>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvDescripcion" ValidationGroup="grupo" 
                            runat="server" ErrorMessage="Campo requerido" ControlToValidate="txtDescripcion" ForeColor="Salmon"></asp:RequiredFieldValidator>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Justificación">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblJustificacion" Visible="false"></asp:Label>
                            <asp:TextBox ID="txtJustificacion" runat="server" Style="height: 37px; width: 200px"  Text='<%# Eval("Justificacion") %>' CssClass="form-control" TextMode="multiline" onkeypress="checkInput3(event)"></asp:TextBox>
                            <script type="text/javascript">
                                function checkInput3(e) {
                                    var ok = /[A-Za-z0-9.,:; ]/.test(String.fromCharCode(e.charCode));
                                    if (e.keyCode == 8) {
                                        //alert();
                                    }
                                    else if (!ok) {
                                        if ($('#errorNombreSistema2').css('display') == 'none') {
                                            $('#errorNombreSistema2').fadeIn();
                                            $('#errorNombreSistema2').fadeOut(6000);
                                        }
                                        if (window.event)//IE
                                            e.returnValue = false;
                                        else//Firefox
                                            e.preventDefault();
                                    }
                                }
                            </script>
                            <div id="errorNombreSistema2" style="display:none">
                                <asp:Label runat="server" ID="errorNombreSistLbl2" text="Solo letras." ForeColor="Salmon"></asp:Label>
                        </div> 
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Resultado"  ItemStyle-VerticalAlign="Middle">
                        <ItemTemplate>
				            <asp:Button ID="botonImagen" runat="server" Text="Imagen" CausesValidation="false" CssClass="btn btn-primary" Style="width: 90px;"/>
                            <asp:Panel runat="server" ID="panelSubirImagen" CssClass="modalPopup"> 
                                <asp:Image ID="imagenSubida" runat="server" style="max-height:400px" />
                                <asp:FileUpload id="Uploader" runat="server" />
                                <br/>
                                <div aria-pressed="true" >
                                    <asp:button runat="server" ID="subirImagen" Text="Subir" CssClass="btn btn-primary" style="margin-left:0px;margin-right:20px;margin-bottom:20px; width:85px" CausesValidation="false" OnClick="subirImagen_Click" CommandArgument="0"/>
                                    <asp:button runat="server" ID="mostrarImagen" Text="Cerrar" CssClass="btn btn-default" style="border-color:#fe6c4f;color:#fe5e3e;align-self:center;margin-left:11px;margin-right:6px;margin-bottom:20px; width:85px"     CausesValidation="false"        CommandArgument="0" />           
                                </div>
                            </asp:Panel>
                            <ajaxToolkit:ModalPopupExtender ID="modalSubir" runat="server" BackgroundCssClass="modalBackground" PopupControlID="panelSubirImagen" TargetControlID="botonImagen"></ajaxToolkit:ModalPopupExtender>
			            </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Estado">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblEstado" Visible="false" Text='<%# Eval("Estado") %>'></asp:Label>
                            <asp:DropDownList ID="ddlEstado" runat="server" ClientIDMode="Static" CssClass="form-control" Style="width: 110px">
                                <asp:ListItem Text="Seleccionar" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Satisfactorio" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Fallido" Value="3"></asp:ListItem>
                                <asp:ListItem Text="Pendiente" Value="4"></asp:ListItem>
                                <asp:ListItem Text="Cancelado" Value="5"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvEstado" ValidationGroup="grupo" 
                                runat="server" ErrorMessage="Campo requerido" ControlToValidate="ddlEstado" InitialValue="1" ForeColor="Salmon"></asp:RequiredFieldValidator>
                        </ItemTemplate>
                    </asp:TemplateField>
                   </Columns>
               </asp:GridView>
               <div class="form-group">
                    <div class="col-md-12" style="margin-left:55px">
                        <asp:Button  runat="server" 
                            Text="+" causesvalidation="true" CssClass="btn btn-default" style="border-color:#4bb648; background-color:#4bb648; font-weight:bold; color:white; font-size:15px;" ID="AgregarFIla" ValidationGroup="grupo" OnClick="AgregarFIla_Click"/>
                    </div>
              </div>
           </div>


           
       </div>

    </div>  
</asp:Panel>
</div>

<div class="form-group">
    <div class="col-md-offset-9 col-md-12" style="margin-bottom:20px; margin-top: 20px;">
        <asp:Button runat="server" style="border-color:#4bb648;color:#4bb648;"
            Text="Aceptar" causesvalidation="true" CssClass="btn btn-default" ValidationGroup="grupo" ID="BotonEPAceptar" OnClick="BotonEPAceptar_Click"/>
        <asp:Button runat="server" Text="Cancelar" style="border-color:#fe6c4f;color:#fe5e3e;" 
            CssClass="btn btn-default" ID="BotonEPCancelar" CausesValidation="false"/>
    </div>
</div>
   

    <div class="row">  
            <div class="form-group">          		
                <asp:GridView ID="gridEjecucion" runat ="server" margin-right ="auto" 		
                    CellPadding="10" 		
                    margin-left="auto" AutoGenerateColumns ="true" 		
                    CssClass ="GridView" HorizontalAlign="Center"   
                    HeaderStyle-BackColor="#48cfae" RowStyle-BackColor="White"  HeaderStyle-ForeColor="#ffffff" BorderColor="#cdcdcd"  HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" 		
                    border-radius="15px"  PagerStyle-BackColor="White" Width="90%" 		
                    AutoPostBack ="true" AllowPaging="true" PageSize="3"
                    OnRowDataBound= "OnGridEjecucionRowDataBound"
                    OnPageIndexChanging="OnGridEjecucionPageIndexChanging"	
                    OnSelectedIndexChanged= "GridEjecucion_SelectedIndexChanged">		
                </asp:GridView>			
        </div>	
        </div>

<asp:Panel runat="server" ID="cancelarPanelModal" CssClass="modalPopup"> 
    <legend style="margin-top:15px"><h5>¿Desea cancelar la operación?</h5></legend>
    <div aria-pressed="true">
        <asp:button runat="server" ID="cancelarBotonSiModal" Text="Si" CssClass="btn btn-primary" style="margin-left:8px;margin-right:20px;margin-bottom:20px; width:85px" CausesValidation="false"/>
        <asp:button runat="server" ID="cancelarBotonNoModal" Text="No" CssClass="btn btn-default" style="border-color:#fe6c4f;color:#fe5e3e;align-self:center;margin-left:11px;margin-right:6px;margin-bottom:20px; width:85px" CausesValidation="false" />           
    </div>
</asp:Panel>	

<ajaxToolkit:ModalPopupExtender ID="ModalCancelar" runat="server" BackgroundCssClass="modalBackground" PopupControlID="cancelarPanelModal" TargetControlID="BotonEPCancelar" ></ajaxToolkit:ModalPopupExtender>

<asp:Panel runat="server" ID="panelModal" CssClass="modalPopup"> 
    <asp:label runat ="server" ID="textModal" style="padding-top:20px;padding-left:11px;padding-right:11px">¿Desea eliminar este caso de prueba?</asp:label>
    <br/> <br/>
    <div aria-pressed="true">
        <asp:button runat="server" ID="aceptarModal" Text="Eliminar"  CssClass="btn btn-default" style="border-color:#4bb648;color:#4bb648;align-self:center;margin-left:16px;margin-right:11px;margin-bottom:20px" OnClick="eliminarAceptarModal" CausesValidation="false"/>
        <asp:button runat="server" ID="cancelarModal" Text="Cancelar"  CssClass="btn btn-default" style="border-color:#fe6c4f;color:#fe5e3e;align-self:center;margin-left:11px;margin-right:6px;margin-bottom:20px" CausesValidation="false"/>           
    </div>
</asp:Panel>
<ajaxToolkit:ModalPopupExtender ID="ModalEliminar" runat="server" BackgroundCssClass="modalBackground" PopupControlID="panelModal" TargetControlID="BotonEPEliminar" OnCancelScript="cancelarModal" OnOkScript="aceptarModal"></ajaxToolkit:ModalPopupExtender>


</asp:Content>
