<%@ Page Title="Iniciar sesión" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SistemaPruebas.Account.Login" Async="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <script type="text/javascript">
        function HideLabel() {
            var seconds = 5;
            setTimeout(function () {

                $('#' + '<%=EtiqErrorLlaves.ClientID %>').fadeOut('slow');
            }, 1000);
        };
</script>
<hr style="margin-top:50px; margin-bottom:40px">
    <div class="row">
        <div class="col-md-offset-3 col-md-6">
            <section id="loginForm">
                <asp:label runat="server" ID="EtiqErrorLlaves" Visible="False" Font-Size="Large"></asp:label>
                <div class="form-horizontal">
                    
                      <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                        <p class="text-danger">
                            <asp:Literal runat="server" ID="FailureText" />
                        </p>
                    </asp:PlaceHolder>
            
               <div class="well">     
                    <legend><h3>Iniciar sesión</h3></legend>
                    <p>Utilice una cuenta local para iniciar sesión.</p>                    
                      <asp:PlaceHolder runat="server" ID="PlaceHolder1" Visible="false">
                        <p class="text-danger">
                            <asp:Literal runat="server" ID="Literal1" />
                        </p>
                    </asp:PlaceHolder>



                      <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="UserName" CssClass="col-md-2 control-label">Usuario</asp:Label>
                        <div class="col-lg-offset-1 col-md-8">
                            <asp:TextBox runat="server" ID="UserName" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName"
                                CssClass="text-danger" ErrorMessage="El campo de nombre de usuario es obligatorio." />
                        </div>
                    </div>


                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="Password" CssClass="col-md-2 control-label">Contraseña</asp:Label>
                        <div class="col-lg-offset-1 col-md-8">
                            <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="text-danger" ErrorMessage="El campo de contraseña es obligatorio." />
                        </div>
                    </div>                                 
                            <asp:Button runat="server" OnClick="LogIn" Text="Iniciar sesión" CssClass="btn btn-primary btn-lg btn-block" />
                        <div class="form-group">
                        <div class="col-md-10">
                            <a href="CambiarContrasena">Cambiar Contraseña</a>
                        </div>
                    </div>
		    
        </div>
                  

                </div>
            </section>
        </div>
    </div>
</asp:Content>
