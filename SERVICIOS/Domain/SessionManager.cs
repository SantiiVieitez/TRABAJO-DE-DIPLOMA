using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SERVICIOS.Domain;

namespace SERVICIOS.Domain
{
    public class SessionManager
    {
        private static object _lock = new object();
        private static SessionManager _session;

        public DateTime FechaInicio { get; set; }
        public Usuarios Usuario { get; set; }
        public Idioma idioma { get; set; }
        public List<iObserver> Observadores { get; set; } = new List<iObserver>();

        public static SessionManager GetInstance
        {
            get
            {
                if (_session == null) throw new Exception("Sesion no Iniciada / Session not Started");

                return _session;
            }
        }
        public static void Login(Usuarios usuario)
        {

            lock (_lock)
            {
                if (_session == null)
                {
                    _session = new SessionManager();
                    _session.Usuario = usuario;
                    _session.FechaInicio = DateTime.Now;
                    _session.idioma = new Idioma(usuario.Idioma);

                }
                else
                {
                    throw new Exception("Sesión ya iniciada / Session already started");
                }
            }
        }
        public static void Logout()
        {
            lock (_lock)
            {
                if (_session != null)
                {
                    _session = null;
                }
            }
        }

        public void CambiarIdioma(Idioma nuevoIdioma)
        {
            this.idioma = nuevoIdioma;
            Notificar();
        }

        // Método para suscribir un observador
        public void SuscribirObservador(iObserver observador)
        {
            Observadores.Add(observador);
        }

        // Método para notificar a los observadores
        private void Notificar()
        {
            foreach (var observador in Observadores)
            {
                observador.ActualizarIdioma(this.idioma);
            }
        }
        public void DesuscribirObservador(iObserver observador)
        {
            Observadores.Remove(observador);
        }


        private SessionManager()
        {

        }
    }
}
