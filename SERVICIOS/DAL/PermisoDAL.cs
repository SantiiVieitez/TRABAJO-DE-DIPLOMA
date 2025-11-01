using SERVICIOS.Domain;
using dao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PermisoDAL
    {
        AuthDB dao;
        DataSet ds;
        public PermisoDAL()
        {
            dao = new AuthDB();
            ds = new DataSet();
        }

        public void AgregarFamilia(Familia pFamilia)
        {
            string query = $"insert into Permiso values ('{pFamilia.Nombre}','{pFamilia.Compuesto}','{pFamilia.EsRol}')";
            dao.ExecuteNonQuery(query);
        }
        public void BorrarFamilia(Familia pFamilia)
        {
            foreach(Permiso p in pFamilia.Permisos)
            {
                BorrarPermisoFamilia(pFamilia, p);
            }
            string query = $"delete from PermisoPermisos where NombreSimple_Compuesto = '{pFamilia.Nombre}'";
            dao.ExecuteNonQuery(query);
                   query = $"delete from Permiso where Nombre = '{pFamilia.Nombre}'";
            dao.ExecuteNonQuery(query);
        }
        public void GuardarPermisoEnFamilia(Familia pFamilia, Permiso pPermiso)
        {
            string query = $"INSERT INTO PermisoPermisos (NombreBase, NombreSimple_Compuesto) VALUES ('{pFamilia.Nombre}', '{pPermiso.Nombre}')";
            dao.ExecuteNonQuery(query);
        }
        public void BorrarPermisoFamilia(Familia pFamilia,Permiso pPermiso)
        {
            string query = $"delete from PermisoPermisos where NombreBase = '{pFamilia.Nombre}' AND NombreSimple_Compuesto = '{pPermiso.Nombre}'";
            dao.ExecuteNonQuery(query);
        }
        public List<Permiso> RetornarPermisos()
        {
            string query = "select * from Permiso";
            DataSet ds = dao.ExecuteDataSet(query);
            List<Permiso> lp = new List<Permiso>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                bool esFamilia = Convert.ToBoolean(dr["Tipo"]);

                if (esFamilia)
                {
                    lp.Add(new Familia(dr));
                }
                else
                {
                    lp.Add(new PermisoSimple(dr));
                }
            }
            return lp;
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
            string query = $"select * from PermisoPermisos where NombreBase = '{familia}'";
            DataSet ds = dao.ExecuteDataSet(query);

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
        public List<Permiso>RetornarPerfiles()
        {
            List<Permiso> aux = RetornarPermisos();
            List<Permiso> Perfiles = new List<Permiso>();
            foreach(Permiso p in aux)
            {
                if(p.Compuesto == true && p.EsRol == true)
                {
                    Perfiles.Add(p);
                }
            }
            return Perfiles;
        }
        public Familia RetornarPerfilUsuario(string Nombre)
        {
            DataSet ds2 = new DataSet();
            string query = $"select * from Permiso where Nombre = '{Nombre}'";
            ds2 = dao.ExecuteDataSet(query);
            Familia aux = new Familia(ds2.Tables[0].Rows[0]);
            aux.Permisos = RetornarPermisosFamilia(aux.Nombre);
            return aux;
        }
    }
}
