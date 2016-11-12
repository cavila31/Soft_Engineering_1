<%@ Page EnableEventValidation="false"  Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Creditos.aspx.cs" Inherits="SistemaPruebas.Creditos" Async="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    	<meta charset="UTF-8">
	<meta name="viewport" content="width=device-width, initial-scale=1">

    <legend style="margin-top:45px"><h2>Equipo desarrollador</h2></legend>

<br />

        <div class="jumbotron" font-family: 'Roboto', 'Helvetica Neue', Helvetica, Arial, sans-serif;">
        <h1>¿Quiénes somos?</h1>
        <p class="lead">Los Doroteos es un grupo de cinco estudiantes de computación quienes, en su tercer año de universidad (algunos un poquitín más), han iniciado en el proceso para aprender los secretos ocultos detrás del &lt;div class=""&gt;.Frecuentemente sin respuestas claras y con excesos de preguntas se han esforzado por entregar los mejores productos posibles. Obsesiv@s, detallistas y legos del JavaScript...</p>
        
    </div>

			<div class="row">
                  <p style="text-align:center">

           <asp:ImageButton id="ImageRicardo" runat="server"
           AlternateText="ImageButton 1"
           ImageAlign="middle"
           ImageUrl="Imagenes/tigre.png"
           OnClick="ricardoClick" AutoPostback ="false"/>

            <asp:ImageButton id="imageDaniel" runat="server"
           AlternateText="ImageButton 1"
           ImageAlign="middle"
           ImageUrl="Imagenes/panda.png" style="margin-left:65px; margin-right:65px"
           OnClick="danielClick" AutoPostback ="false"/>
                 
            <asp:ImageButton id="imageCaro" runat="server"
           AlternateText="ImageButton 1"
           ImageAlign="middle"
           ImageUrl="Imagenes/leon.png"
           OnClick="carolinaClick" AutoPostback ="false"/>


 <asp:Panel ID= "PanelRicardo" runat="server" Visible="false">
<div class="col-md-4">
<div class="panel panel-default" style="margin-left=20px">
  <div class="panel-body">
  <legend style="margin-top:15px"><h5>Ricardo</h5></legend>
      <p> Dícese de un cónyuge que cree, de manera ilusa que ‘todo sale’ y al final si le sale. Poco realista. Idealista. Un ser sin confianza para dejar abierto Telegram, como muestra de poca fe con sus congéneres. Es un joven proficiente en el uso de grids. Aunque no lo acepte, es ávido seguidor de Chayanne. Prueba de esto se encuentra en su cuenta de Spotify. Pronuncia la frase: “vistes” 50 veces en promedio al día. Oveja, Doroteo.</p>
  </div>
</div>
    </div>
     </asp:Panel>

 <asp:Panel ID= "PanelDaniel" runat="server" Visible="false" >
                      <div class=" col-md-offset-4 col-md-4">
<div class="panel panel-default" >
  <div class="panel-body">
   <legend style="margin-top:15px"><h5>Daniel</h5></legend>
       <p> Nacido en el reino encantado de las mariquitas, fue adoptado por una familia originaria de la bruma sagrada, quienes, exiliados en las praderas de las flores construyeron su casa y por poco su hogar. Semi institucionalizado desde corta edad (por decisión propia) y temeroso de las ovejas con piel de lobo, su infancia y adolescencia trascendió entre bolillos, máscaras, pinceles, buses y huelgas. 
Después de un medianamente largo viaje para identificarse como una mariquita -adoptada- ha logrado encontrarse, y en un loop finito espera poder discernir lo mejor posible en todo lo que, este AFND que conocemos como vida, le depare.</p>
  </div>
</div>
                          </div>
     </asp:Panel>

 <asp:Panel ID= "PanelCarolina" runat="server" Visible="false">
<div class="col-md-offset-8 col-md-4">

    <div class="panel panel-default">
  <div class="panel-body">
    <legend style="margin-top:15px"><h5>Carolina</h5></legend>
       <p> Desde unas tierras lejanas, calientes y ubicadas cerca del “mejor clima del mundo” lidera la Scrum Master todo su grupo de Doroteos. Obsesiva con los colores, las posiciones de los objetos, las combinaciones y los detalles que ha simple vista son inobservables; siempre sin descuidar el trabajo de todo su grupo. El epíteto de Scrum sólo vino a decorar una característica que la acompaña desde pequeña.</p>
  </div>
</div>

    </div>
     </asp:Panel>
                      </p>


                    <p style="text-align:center">

           <asp:ImageButton id="ImageButton1" runat="server"
           AlternateText="ImageButton 1"
           ImageAlign="middle"
           ImageUrl="Imagenes/perro.png"
           OnClick="helenaClick" AutoPostback ="false"/>

           <asp:ImageButton id="ImageButton2" runat="server"
           AlternateText="ImageButton 1"
           ImageAlign="middle" style="margin-left:65px"
           ImageUrl="Imagenes/lobo.png"
           OnClick="andreaClick" AutoPostback ="false"/>


 <asp:Panel ID= "PanelHelena" runat="server" Visible="false">
                        <div class="col-md-offset-2 col-md-4">
<div class="panel panel-default" style="margin-left=20px">
  <div class="panel-body">
  <legend style="margin-top:15px"><h5>Helena</h5></legend>
      <p>Se destaca por su amplio conocimiento sobre todo. Le encanta el desarrollo de interfaces. No confía en nadie y por eso prefiere que los trabajos tengan sólo el mínimo pues cree que no van a salir . Es realista estimando costos, tiempo, esfuerzo y neuronas en las labores programadoras. Excelente compañera de trabajo. Fabulosa trayendo positivismo al ambiente trabajador. </p>
  </div>
</div>
    </div>
</asp:Panel>

 <asp:Panel ID= "PanelAndrea" runat="server" Visible="false">
    <div class="col-md-offset-6 col-md-4">
  <div class="panel panel-default">
  <div class="panel-body">
  <legend style="margin-top:15px"><h5>Andrea</h5></legend>
      <p>Por las mañanas una persona bastante “normal ” y por las noches una heroína que lucha contra el crimen. Hija de una mortal con el dios griego Hypnos, de ahí a que siempre esté cansada. Tiene la bendicion de la diosa Adefalgia por lo que puede transformar cosas en comida, cuando no está siendo observada. Por fuera está hecha de chocolate sólido color piel pero por dentro tiene un corazon de miel. </p>
  </div>
</div>
</div>
     </asp:Panel>
       </p>
			</div>


</asp:Content>