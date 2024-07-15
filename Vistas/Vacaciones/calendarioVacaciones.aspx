<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="calendarioVacaciones.aspx.cs" Inherits="ControlEmpresarial.Vistas.CalendarioVacaciones" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Calendario de Vacaciones</title>
    <link href="https://fonts.googleapis.com/css2?family=Montserrat:wght@400;700&display=swap" rel="stylesheet">
    <link href="~/Estilos/app.css" type="text/css" rel="stylesheet">
    <style>
        .divisor-forma-personalizado {
        position: relative; /* Cambiar a relative para mantener su posición, no mover*/
        width: 100%;
        overflow: hidden;
        line-height: 0;
        transform: rotate(180deg);
        z-index: 1;
        margin-top: -300px;
     }

    .divisor-forma-personalizado svg {
        display: block;
        width: calc(100% + 1.3px);
        height: 405px;
     }

    .divisor-forma-personalizado .relleno-forma {
       fill: #5E58F8;
    }

        @media (min-width: 768px) and (max-width: 1023px) {
            .divisor-forma-personalizado svg {
                width: calc(100% + 1.3px);
                height: 500px;
            }
        }

         header nav ul {
 list-style-type: none;
 padding: 0;
 margin: 0;
 display: flex;
 gap: 20px; /* Adjust gap as needed for spacing */
 }

header nav ul li {
   position: relative;
}

 header nav ul li a {
     text-decoration: none;
     padding: 10px;
     display: block;
     color: #000; /* Change color as needed */
     }

            /* Styling for submenu */
header nav ul li.has-submenu .submenu {
  display: none;
  position: absolute;
  top: 100%;
  left: 0;
  background-color: white;
  box-shadow: 0 8px 16px rgba(0, 0, 0, 0.2);
  z-index: 1;
  min-width: 200px;
   }

header nav ul li.has-submenu:hover .submenu {
    display: block;
    color:#5E58F8;
}

header nav ul li.has-submenu .submenu li {
    padding: 0;
}

header nav ul li.has-submenu .submenu li a {
    padding: 10px;
    color: black;
    white-space: nowrap;
    display: block;
}
header nav ul li.has-submenu .submenu li a:hover {
    color: #5E58F8; /*color texto*/
}
    </style>
</head>
<body>
    <form id="form1" runat="server">
         <header>
    <div class="cabecera-izquierda">
        <h1>Colaborador</h1>
        <p>Christian Barquero</p>
    </div>
                <nav>
    <ul>
        <li class="has-submenu">
            <a href="#">Horas Extras</a>
            <ul class="submenu">
                <li><a href="#">Solicitar Horas Extras</a></li>
            </ul>
        </li>
        <li class="has-submenu">
            <a href="#">Permisos</a>
            <ul class="submenu">
                <li><a href="../Permisos/PermisosColaborador.aspx">Solicitar Permiso</a></li>
            </ul>
        </li>
        <li class="has-submenu">
            <a href="#">Inconsistencias</a>
            <ul class="submenu">
                <li><a href="#">Justificar Inconsistencia.</a></li>
            </ul>
        </li>
        <li class="has-submenu">
            <a href="#">Reposiciones</a>
            <ul class="submenu">
                <li><a href="#">Revisar Reposiciones</a></li>
            </ul>
        </li>
        <li class="has-submenu">
            <a href="#">Vacaciones</a>
            <ul class="submenu">
                <li><a href="../Vacaciones/solicitarVacacionColaborador.aspx">Solicitar Vacaciones</a></li>
                <li><a href="../Vacaciones/calendarioVacaciones.aspx">Calendario de Vacaciones</a></li>
            </ul>
        </li>
        <li class="has-submenu">
            <a href="#">Actividades</a>
            <ul class="submenu">
                <li><a href="../Control de Actividades/ControlActividadesColaborador.aspx">Registrar Actividades</a></li>
            </ul>
        </li>
        <li class="has-submenu">
            <a href="#">Marcas</a>
            <ul class="submenu">
                <li><a href="../Marcas/marcas.aspx">Registre su hora de marca</a></li>
            </ul>
        </li>
    </ul>
</nav>
    <div class="cabecera-derecha">
        <button class="boton-notificacion">
            <img src="../../Imagenes/notificacion.gif" alt="Notificación"/>
        </button>
    </div>
</header>
 <main>
        <div class="container">
            <h2>Calendario de Vacaciones</h2>
            <div class="calendar-header">
                <button type="button" onclick="prevMonth()">&#8249;</button>
                <h3 id="monthYear"></h3>
                <button type="button" onclick="nextMonth()">&#8250;</button>
            </div>
            <div class="calendar" id="calendar">
                <asp:PlaceHolder ID="calendarPlaceholder" runat="server"></asp:PlaceHolder>
            </div>
            <div class="calendar-footer">
                <span>&#9632; = Días Libres</span>
            </div>
        </div>
        </main>
        <div class="divisor-forma-personalizado">
        <svg data-name="Layer 1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1200 120" preserveAspectRatio="none">
            <path d="M321.39,56.44c58-10.79,114.16-30.13,172-41.86,82.39-16.72,168.19-17.73,250.45-.39C823.78,31,906.67,72,985.66,92.83c70.05,18.48,146.53,26.09,214.34,3V0H0V27.35A600.21,600.21,0,0,0,321.39,56.44Z" class="relleno-forma"></path>
        </svg>
    </div>
</form>
<footer class="footer">
    <div>
        <h3>About</h3>
        <p>"ActivitySync proporciona una solución integral para la gestión eficiente de actividades dentro de tu empresa. Desde la 
            marcación de inconsistencias hasta el registro de entradas y salidas de empleados, así como el seguimiento detallado de 
            sus actividades diarias, nuestro software está diseñado para optimizar la productividad y mejorar la organización empresarial."</p>
    </div>
    <div>
        <h3>Tags</h3>
        <ul class="tag-list">
            <li><a href="#">Horas Extras</a></li>
            <li><a href="#">Permisos</a></li>
            <li><a href="#">Inconsistencias</a></li>
            <li><a href="#">Reposiciones</a></li>
            <li><a href="#">Vacaciones</a></li>
            <li><a href="#">Actividades</a></li>
            <li><a href="#">Marcas</a></li>
        </ul>
    </div>
    <div>
        <h3>Contactenos</h3>
        <p>ActivitySync ofrece servicios al cliente cuales estén interesados a nuestra gestión de actividades</p>
        <p>Contactenos:</p>
        <ul>
            <li>Email: support@activitysync.com</li>
            <li>Phone: +1-800-123-4567</li>
        </ul>
    </div>
</footer>
</body>
    <script>
        const calendarEl = document.getElementById('calendar');
        const monthYearEl = document.getElementById('monthYear');
        let currentDate = new Date();

        const freeDays = [9, 18, 26]; // Example free days for the month

        function renderCalendar(date) {
            calendarEl.innerHTML = '';
            const month = date.getMonth();
            const year = date.getFullYear();
            monthYearEl.textContent = date.toLocaleDateString('es-ES', { month: 'long', year: 'numeric' });

            const firstDay = new Date(year, month, 1).getDay();
            const lastDate = new Date(year, month + 1, 0).getDate();

            const daysOfWeek = ['Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa', 'Su'];
            daysOfWeek.forEach(day => {
                const headerEl = document.createElement('div');
                headerEl.textContent = day;
                headerEl.classList.add('header');
                calendarEl.appendChild(headerEl);
            });

            const offset = firstDay === 0 ? 6 : firstDay - 1;
            for (let i = 0; i < offset; i++) {
                const emptyEl = document.createElement('div');
                calendarEl.appendChild(emptyEl);
            }

            for (let day = 1; day <= lastDate; day++) {
                const dayEl = document.createElement('div');
                dayEl.textContent = day;
                dayEl.classList.add('day');
                if (freeDays.includes(day)) {
                    dayEl.classList.add('free-day');
                }
                calendarEl.appendChild(dayEl);
            }
        }

        function prevMonth() {
            currentDate.setMonth(currentDate.getMonth() - 1);
            renderCalendar(currentDate);
        }

        function nextMonth() {
            currentDate.setMonth(currentDate.getMonth() + 1);
            renderCalendar(currentDate);
        }

        renderCalendar(currentDate);
</script>
</html>
