using SERVICIOS.Domain;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DAL
{
    public class PermisoDAL : BaseAuthDAL<Permiso>
    {
        private static PermisoDAL _instance;
        public static PermisoDAL Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PermisoDAL();
                }
                return _instance;
            }
        }

        private PermisoDAL() : base("Permiso", "Nombre")
        {
        }

        protected override Permiso MapearEntidad(DataRow dr)
        {
            bool esFamilia = Convert.ToBoolean(dr["Tipo"]);

            if (esFamilia)
            {
                return new Familia(dr);
            }
            else
            {
                return new PermisoSimple(dr);
            }
        }

        public override void Agregar(Permiso pPermiso)
        {
            string query = "insert into Permiso values (@Nombre, @Compuesto, @EsRol)";
            var parameters = new Dictionary<string, object>
            {
                { "@Nombre", pPermiso.Nombre },
                { "@Compuesto", pPermiso.Compuesto },
                { "@EsRol", pPermiso.EsRol }
            };
            dao.ExecuteNonQuery(query, parameters);
        }

        public override void Borrar(Permiso pPermiso)
        {
            if (pPermiso is Familia pFamilia)
            {
                foreach (Permiso p in pFamilia.Permisos)
                {
                    BorrarPermisoFamilia(pFamilia, p);
                }
            }

            string query = "delete from PermisoPermisos where NombreSimple_Compuesto = @Nombre";
            var parameters = new Dictionary<string, object>
            {
                { "@Nombre", pPermiso.Nombre }
            };
            dao.ExecuteNonQuery(query, parameters);

            query = "delete from Permiso where Nombre = @Nombre";
            dao.ExecuteNonQuery(query, parameters);
        }

        public override void Modificar(Permiso pPermiso)
        {
            string query = "update Permiso set Compuesto = @Compuesto, EsRol = @EsRol where Nombre = @Nombre";
            var parameters = new Dictionary<string, object>
            {
                { "@Nombre", pPermiso.Nombre },
                { "@Compuesto", pPermiso.Compuesto },
                { "@EsRol", pPermiso.EsRol }
            };
            dao.ExecuteNonQuery(query, parameters);
        }

        public void GuardarPermisoEnFamilia(Familia pFamilia, Permiso pPermiso)
        {
            string query = "INSERT INTO PermisoPermisos (NombreBase, NombreSimple_Compuesto) VALUES (@NombreBase, @NombreHijo)";
            var parameters = new Dictionary<string, object>
            {
                { "@NombreBase", pFamilia.Nombre },
                { "@NombreHijo", pPermiso.Nombre }
            };
            dao.ExecuteNonQuery(query, parameters);
        }

        public void BorrarPermisoFamilia(Familia pFamilia, Permiso pPermiso)
        {
            string query = "delete from PermisoPermisos where NombreBase = @NombreBase AND NombreSimple_Compuesto = @NombreHijo";
            var parameters = new Dictionary<string, object>
            {
                { "@NombreBase", pFamilia.Nombre },
                { "@NombreHijo", pPermiso.Nombre }
            };
            dao.ExecuteNonQuery(query, parameters);
        }

        public List<Permiso> RetornarPermisos()
        {
            return ObtenerTodos();
        }

        public List<Permiso> RetornarPermisosFamilia(string pFamilia)
        {
            List<Permiso> Permisos = RetornarPermisos();
            List<Permiso> PermisosFamilia = new List<Permiso>();
            LlenarPermisosFamilia(Permisos, pFamilia, PermisosFamilia);
            return PermisosFamilia;
        }

        private void LlenarPermisosFamilia(List<Permiso> todosLosPermisos, string familia, List<Permiso> permisosFamilia)
        {
            string query = "select * from PermisoPermisos where NombreBase = @Familia";
            var parameters = new Dictionary<string, object>
            {
                { "@Familia", familia }
            };

            DataSet ds = dao.ExecuteDataSet(query, parameters);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                var permiso = todosLosPermisos.Find(c => c.Nombre == dr[1].ToString());
                if (permiso != null)
                {
                    if (permiso is Familia subFamilia)
                    {
                        List<Permiso> subPermisos = new List<Permiso>();
                        LlenarPermisosFamilia(todosLosPermisos, subFamilia.Nombre, subPermisos);
                        subFamilia.Permisos = subPermisos;
                        permisosFamilia.Add(subFamilia);
                    }
                    else
                    {
                        permisosFamilia.Add(permiso);
                    }
                }
            }
        }

        public List<Permiso> RetornarPerfiles()
        {
            List<Permiso> aux = RetornarPermisos();
            List<Permiso> Perfiles = new List<Permiso>();
            foreach (Permiso p in aux)
            {
                if (p.Compuesto == true && p.EsRol == true)
                {
                    Perfiles.Add(p);
                }
            }
            return Perfiles;
        }

        public Familia RetornarPerfilUsuario(string Nombre)
        {
            string query = "select * from Permiso where Nombre = @Nombre";
            var parameters = new Dictionary<string, object>
            {
                { "@Nombre", Nombre }
            };
            DataSet ds2 = dao.ExecuteDataSet(query, parameters);
            Familia aux = new Familia(ds2.Tables[0].Rows[0]);
            aux.Permisos = RetornarPermisosFamilia(aux.Nombre);
            return aux;
        }
    }
}