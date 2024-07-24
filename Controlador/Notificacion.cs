using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlEmpresarial.Controlador
{
    public class Notificacion
    {
        public int IdNotificacion { get; set; }
        public int IdEnviador { get; set; }
        public int IdRecibidor { get; set; }
        public string Titulo { get; set; }
        public string Motivo { get; set; }
        public DateTime Fecha { get; set; }
        public string EnviadorNombre { get; set; }
        public string EnviadorApellidos { get; set; }
    }
}