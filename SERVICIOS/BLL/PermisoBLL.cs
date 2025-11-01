using SERVICIOS.Domain;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class PermisoBLL
    {
        PermisoDAL dao;
        public PermisoBLL()
        {
            dao = new PermisoDAL();
        }
        public void AgregarFamilia(Familia pFamilia)
        {
            dao.AgregarFamilia(pFamilia);
        }
        public void BorrarFamilia(Familia pFamilia)
        {
            dao.BorrarFamilia(pFamilia);
        }
        public void AgregarPermisoFamilia(Familia familia, Permiso pPermiso)
        {
            if (familia is null || pPermiso is null)
                throw new ArgumentNullException("Familia o permiso nulo.");

            if (familia.EsRol && pPermiso.EsRol)
                throw new InvalidOperationException(
                    $"No se puede asignar el perfil '{pPermiso.Nombre}' dentro del perfil '{familia.Nombre}'. Asigne Permisos o Familias.");

            if (VerificarPermisoEnFamilia(familia, pPermiso.Nombre))
                throw new InvalidOperationException(
                    $"El permiso '{pPermiso.Nombre}' ya existe en la familia/perfil '{familia.Nombre}'.");

            // 🔁 Defensa extra: si estás agregando una subfamilia, evitá ciclos
            if (pPermiso is Familia subFamilia && VerificarPermisoEnFamilia(subFamilia, familia.Nombre))
                throw new InvalidOperationException(
                    $"Se detectó una referencia cíclica entre '{familia.Nombre}' y '{subFamilia.Nombre}'.");

            familia.Permisos.Add(pPermiso);
            dao.GuardarPermisoEnFamilia(familia, pPermiso);
        }
        public void BorrarPermisoFamilia(Familia familia, Permiso pPermiso)
        {
            dao.BorrarPermisoFamilia(familia, pPermiso);
        }
        public List<Permiso> RetornarPermisos()
        {
            return dao.RetornarPermisos();
        }
        public List<Permiso> RetornarPermisosFamilia(string pFamilia)
        {
            return dao.RetornarPermisosFamilia(pFamilia);
        }
        private bool VerificarPermisoEnFamilia(Familia familia, string nombrePermiso)
        {
            if (familia.Permisos.Find(c => c.Nombre == nombrePermiso) != null)
            {
                return true;
            }

            foreach (var permiso in familia.Permisos)
            {
                if (permiso is Familia subFamilia)
                {
                    if (VerificarPermisoEnFamilia(subFamilia, nombrePermiso))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public List<Permiso> RetornarPerfiles()
        {
            return dao.RetornarPerfiles();
        }
        public Familia RetornarPerfilUsuario(string Nombre)
        {
            return dao.RetornarPerfilUsuario(Nombre);
        }

    }
}
