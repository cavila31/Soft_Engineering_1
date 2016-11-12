<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SistemaPruebas._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

	<meta charset="UTF-8">
	<meta name="viewport" content="width=device-width, initial-scale=1">

	<link href='http://fonts.googleapis.com/css?family=Droid+Serif|Open+Sans:400,700' rel='stylesheet' type='text/css'>

	<link rel="stylesheet" href="Content/reset.css"> <!-- CSS reset -->
	<link rel="stylesheet" href="Content/style.css"> <!-- Resource style -->
	<script src="js/modernizr.js"></script> <!-- Modernizr -->


<hr style="margin:50px;">

    <div class="jumbotron" style="margin-top:60px; font-family: 'Roboto', 'Helvetica Neue', Helvetica, Arial, sans-serif;">
        <h1>Bienvenido</h1>
        <p class="lead">Sistema de Pruebas. Página de inicio</p>
        
    </div>
 <asp:Panel ID= "Timeline" runat="server" Visible="false">
<section id="cd-timeline" class="cd-container">
		<div class="cd-timeline-block">
			<div class="cd-timeline-img cd-picture">
				<img src="Imagenes\team-blanco.svg" alt="Picture">
			</div> <!-- cd-timeline-img -->

			<div class="cd-timeline-content">
				<h2>Gestión de Recursos Humanos</h2>
				<p>Este módulo administra la información personal referente a los Miembros de Equipo y Administradores.</p>
				<a href="Intefaces/InterfazRecursoHumano" class="cd-read-more">Ir a Recursos Humanos</a>
			</div> <!-- cd-timeline-content -->
		</div> <!-- cd-timeline-block -->

		<div class="cd-timeline-block">
			<div class="cd-timeline-img cd-movie">
				<img src="Imagenes/proyecto.svg" alt="Movie">
			</div> <!-- cd-timeline-img -->

			<div class="cd-timeline-content">
				<h2>Gestión de un proyecto</h2>
				<p>Este módulo permite la gestión de Proyectos a ser probados, entre sus funcionalidades fundamentales están: Ingresar, Modificar y Eliminar un Proyecto. </p>
				<a href="Intefaces/InterfazProyecto" class="cd-read-more">Ir a Proyecto</a>
			</div> <!-- cd-timeline-content -->
		</div> <!-- cd-timeline-block -->

		<div class="cd-timeline-block">
			<div class="cd-timeline-img cd-location">
				<img src="Imagenes/requerimientos.svg" alt="Picture">
			</div> <!-- cd-timeline-img -->

			<div class="cd-timeline-content">
				<h2>Módulo de Requerimientos</h2>
				<p>Este módulo provee la capacidad de agregar diversos Requerimientos a un Proyecto específico, entre sus funcionalidades fundamentales están: Ingresar, Modificar y Eliminar un Requerimiento.</p>
				<a href="Intefaces/InterfazRequerimiento" class="cd-read-more">Ir a Requerimientos</a>

			</div> <!-- cd-timeline-content -->
		</div> <!-- cd-timeline-block -->

		<div class="cd-timeline-block">
			<div class="cd-timeline-img cd-picture">
				<img src="Imagenes/diseno.svg" alt="Location">
			</div> <!-- cd-timeline-img -->

			<div class="cd-timeline-content">
				<h2>Módulo de Diseño</h2>
				<p>Este módulo facilita la administración de Diseños para un proyecto específico con ciertos Requerimientos seleccionados por el Usuario, entre sus funcionalidades fundamentales están: Ingresar, Modificar y Eliminar un Diseño.</p>
				<a href="Intefaces/InterfazDiseno" class="cd-read-more">Ir a Diseño</a>

			</div> <!-- cd-timeline-content -->
		</div> <!-- cd-timeline-block -->

		<div class="cd-timeline-block">
			
                <div class="cd-timeline-img cd-movie">
				<img src="Imagenes/CasosPrueba.svg" alt="Location">
			</div> <!-- cd-timeline-img -->

			<div class="cd-timeline-content">
				<h2>Módulo de Casos de Prueba</h2>
				<p>Este módulo brinda acceso a la administración de Casos de Prueba para un Diseño en particular, entre sus funcionalidades fundamentales están: Ingresar, Modificar y Eliminar un Caso de Prueba</p>

			</div> <!-- cd-timeline-content -->
		</div> <!-- cd-timeline-block -->

		<div class="cd-timeline-block">
			<div class="cd-timeline-img cd-location">
				<img src="Imagenes/ejecucion.svg" alt="Movie">
			</div> <!-- cd-timeline-img -->

			<div class="cd-timeline-content">
				<h2>Módulo de Ejecución de Pruebas</h2>
				<p>Este módulo provee la capacidad de administrar las Ejecuciones de Prueba para un diseño específico y sus Casos de Prueba Asociados, entre sus funcionalidades fundamentales están: Ingresar, Modificar y Eliminar una Ejecución.</p>
                <a href="Intefaces/InterfazEjecucion" class="cd-read-more">Ir a Ejecución</a>
			</div> <!-- cd-timeline-content -->
		</div> <!-- cd-timeline-block -->

		<div class="cd-timeline-block">
			<div class="cd-timeline-img cd-picture">
				<img src="Imagenes/reportes.svg" alt="Movie">
			</div> <!-- cd-timeline-img -->

			<div class="cd-timeline-content">
				<h2>Generar Reportes</h2>
				<p>Este módulo facilita la obtención de Reportes acerca de un Proyecto determinado, incluyendo diversos elementos y atributos del mismo, los cuales son seleccionados por el usuario de acuerdo a sus necesidades.</p>
                <a href="Intefaces/InterfazReporte" class="cd-read-more">Ir a Reportes</a>
			</div> <!-- cd-timeline-content -->
		</div> <!-- cd-timeline-block -->

	</section> <!-- cd-timeline -->
     </asp:Panel>
<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js"></script>
<script src="js/main.js"></script> <!-- Resource jQuery -->


</asp:Content>
