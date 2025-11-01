using BLL;
using SERVICIOS.BLL;
using SERVICIOS.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SERVICIOS;
using System.Xml.Linq;

namespace PROYECTO_INGENIERIA_DE_SOFTWARE
{
    public partial class FormPerfiles : Form,iObserver
    {
        PermisoBLL permisoBLL;
        BitacoraEventosBLL bitacoraBLL;
        List<Familia> ListaFamilias;
        List<Familia> ListaPerfiles;
        string Idioma;
        public FormPerfiles()
        {
            InitializeComponent();
            SessionManager.GetInstance.SuscribirObservador(this);
            permisoBLL = new PermisoBLL();
            ListaFamilias = new List<Familia>();
            ListaPerfiles = new List<Familia>();
            bitacoraBLL = new BitacoraEventosBLL();
            Mostrar();
            MostrarPermisosFamilia();
            MostrarPermisosPerfil();
            ActualizarIdioma(SessionManager.GetInstance.idioma);
        }
        
        public void Mostrar()
        {
            // Limpiar listas y fuentes de datos
            ListaFamilias.Clear();
            ListaPerfiles.Clear();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = permisoBLL.RetornarPermisos();

            // Iterar sobre los permisos retornados
            foreach (Permiso p in permisoBLL.RetornarPermisos())
            {
                if (p is Familia familia && !p.EsRol)
                {
                    // Si el permiso es una familia y no es un rol, añadir a ListaFamilias
                    ListaFamilias.Add(familia);
                }
                else if (p is Familia rolFamilia && p.EsRol)
                {
                    // Si el permiso es una familia y es un rol, añadir a ListaPerfiles y comboBox2
                    ListaPerfiles.Add(rolFamilia);
                }
            }

            // Actualizar las fuentes de datos de los DataGridViews
            dataGridView2.DataSource = null;
            dataGridView2.DataSource = ListaFamilias;
            dataGridView3.DataSource = null;
            dataGridView3.DataSource = ListaPerfiles;
        }
        public void MostrarPermisosFamilia()
        {
            Familia aux2 = new Familia();
            aux2.Nombre = "NingunaFamilia";
            List<Familia> dataSource = new List<Familia>();
            dataSource.AddRange(ListaFamilias);
            dataSource.Add(aux2);

            comboBox1.DataSource = null;
            comboBox1.DataSource = dataSource;
        }
        public void MostrarPermisosPerfil()
        {
            Familia aux2 = new Familia();
            aux2.Nombre = "NingunPerfil";
            List<Familia> dataSource = new List<Familia>();
            dataSource.AddRange(ListaPerfiles);
            dataSource.Add(aux2);

            comboBox2.DataSource = null;
            comboBox2.DataSource = dataSource;
        }
        private void btnAgregarFamilias_Click(object sender, EventArgs e)
        {

            try
            {
                if (ListaFamilias.Find(c => c.Nombre == textBox1.Text) != null)
                {
                    throw new Exception(new IdiomaBLL().Traducir(Idioma, "ExcepcionNombreFamilia"));
                }
                else
                {
                    Familia aux = new Familia();
                    aux.Nombre = textBox1.Text;
                    aux.EsRol = false;
                    permisoBLL.AgregarFamilia(aux);
                    Mostrar();
                    MostrarPermisosFamilia();
                }
                
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            
        }
        private void btnAgregarPerfil_Click(object sender, EventArgs e)
        {

            try
            {
                if (ListaPerfiles.Find(c => c.Nombre == textBox2.Text) != null)
                {
                    throw new Exception(new IdiomaBLL().Traducir(Idioma, "ExcepcionNombrePerfil"));
                }
                else
                {
                    Familia aux = new Familia();
                    aux.Nombre = textBox2.Text;
                    aux.EsRol = true;
                    permisoBLL.AgregarFamilia(aux);
                    Mostrar();
                    MostrarPermisosPerfil();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            
            
            
        }
        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "NingunaFamilia")
            {
                Mostrar();
            }
            else
            {
                Familia aux = comboBox1.SelectedItem as Familia;
                aux.Permisos = permisoBLL.RetornarPermisosFamilia(aux.Nombre);
                dataGridView2.DataSource = null;
                dataGridView2.DataSource = aux.Permisos;
            }
            
        }

        private void btnAgregarPermisoFamilia_Click(object sender, EventArgs e)
        {

            if (radioButton1.Checked)
            {
                Familia pFamilia = dataGridView2.SelectedRows[0].DataBoundItem as Familia;
                pFamilia.Permisos = permisoBLL.RetornarPermisosFamilia(pFamilia.Nombre);

                foreach (DataGridViewRow dr in dataGridView1.SelectedRows)
                {
                    Permiso aux = dr.DataBoundItem as Permiso;
                    if (aux != null)
                    {
                        try
                        {
                            permisoBLL.AgregarPermisoFamilia(pFamilia, aux);

                            BitacoraEvento bitacoraEvento = new BitacoraEvento();
                            bitacoraEvento.Usuario = SessionManager.GetInstance.Usuario.Login;
                            bitacoraEvento.Fecha = DateTime.Now;
                            bitacoraEvento.Modulo = "Administrador";
                            bitacoraEvento.Evento = "Agrego/Modifico un Permiso de una Familia";
                            bitacoraEvento.Criticidad = 1;
                            bitacoraBLL.Registrar(bitacoraEvento);

                        }
                        catch (Exception ex)
                        {
                            // Manejo de excepción si el permiso ya existe
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

                Mostrar();
            }
            else
            {
                Familia pPerfil = dataGridView3.SelectedRows[0].DataBoundItem as Familia;
                pPerfil.Permisos = permisoBLL.RetornarPermisosFamilia(pPerfil.Nombre);

                foreach (DataGridViewRow dr in dataGridView1.SelectedRows)
                {
                    Permiso aux = dr.DataBoundItem as Permiso;
                    if(aux is Familia)
                    {
                        (aux as Familia).Permisos = permisoBLL.RetornarPermisosFamilia((aux as Familia).Nombre);
                    }
                    if (aux != null)
                    {
                        try
                        {
                            permisoBLL.AgregarPermisoFamilia(pPerfil, aux);
                            BitacoraEvento bitacoraEvento = new BitacoraEvento();
                            bitacoraEvento.Usuario = SessionManager.GetInstance.Usuario.Login;
                            bitacoraEvento.Fecha = DateTime.Now;
                            bitacoraEvento.Modulo = "Administrador";
                            bitacoraEvento.Evento = "Agrego/Modifico un Permiso de un Perfil";
                            bitacoraEvento.Criticidad = 1;
                            bitacoraBLL.Registrar(bitacoraEvento);
                        }
                        catch (Exception ex)
                        {
                            // Manejo de excepción si el permiso ya existe
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                Familia pFamilia = comboBox1.SelectedItem as Familia;
                Permiso pPermiso = dataGridView2.SelectedRows[0].DataBoundItem as Permiso;
                permisoBLL.BorrarPermisoFamilia(pFamilia, pPermiso);

                BitacoraEvento bitacoraEvento = new BitacoraEvento();
                bitacoraEvento.Usuario = SessionManager.GetInstance.Usuario.Login;
                bitacoraEvento.Fecha = DateTime.Now;
                bitacoraEvento.Modulo = "Administrador";
                bitacoraEvento.Evento = "Borro Permiso de una Familia";
                bitacoraEvento.Criticidad = 1;
                bitacoraBLL.Registrar(bitacoraEvento);

                Mostrar();
            }
            else
            {
                Familia pPerfil = comboBox2.SelectedItem as Familia;
                Permiso pPermiso = dataGridView3.SelectedRows[0].DataBoundItem as Permiso;
                permisoBLL.BorrarPermisoFamilia(pPerfil, pPermiso);

                BitacoraEvento bitacoraEvento = new BitacoraEvento();
                bitacoraEvento.Usuario = SessionManager.GetInstance.Usuario.Login;
                bitacoraEvento.Fecha = DateTime.Now;
                bitacoraEvento.Modulo = "Administrador";
                bitacoraEvento.Evento = "Borro un Permiso de un Perfil";
                bitacoraEvento.Criticidad = 1;
                bitacoraBLL.Registrar(bitacoraEvento);

                Mostrar();
            }
        }

        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem.ToString() == "NingunPerfil")
            {
                Mostrar();
            }
            else
            {
                Familia aux = comboBox2.SelectedItem as Familia;
                aux.Permisos = permisoBLL.RetornarPermisosFamilia(aux.Nombre);
                dataGridView3.DataSource = null;
                dataGridView3.DataSource = aux.Permisos;
            }
        }

        private void btnBorrarFamilia_Click(object sender, EventArgs e)
        {
            Familia aux = dataGridView2.SelectedRows[0].DataBoundItem as Familia;
            aux.Permisos = permisoBLL.RetornarPermisosFamilia(aux.Nombre);
            permisoBLL.BorrarFamilia(aux);
            Mostrar();
            MostrarPermisosFamilia();
            MostrarPermisosPerfil();
        }

        private void btnBorrarPerfil_Click(object sender, EventArgs e)
        {
            Familia aux = dataGridView3.SelectedRows[0].DataBoundItem as Familia;
            aux.Permisos = permisoBLL.RetornarPermisosFamilia(aux.Nombre);
            permisoBLL.BorrarFamilia(aux);
            Mostrar();
            MostrarPermisosFamilia();
            MostrarPermisosPerfil();
        }

        public void ActualizarIdioma(Idioma idioma)
        {
            
            if (idioma.Nombre == "Español")
            {
                Idioma = "FormPerfilesEspañol";
            }
            else
            {
                Idioma = "FormPerfilesEnglish";
            }
            label1.Text = new IdiomaBLL().Traducir(Idioma, "label1");
            label2.Text = new IdiomaBLL().Traducir(Idioma, "label2");
            label3.Text = new IdiomaBLL().Traducir(Idioma, "label3");
            label4.Text = new IdiomaBLL().Traducir(Idioma, "label4");
            radioButton1.Text = new IdiomaBLL().Traducir(Idioma, "radiobuttonFamilia");
            radioButton2.Text = new IdiomaBLL().Traducir(Idioma, "radiobuttonPerfil");
            btnAgregarFamilias.Text = new IdiomaBLL().Traducir(Idioma, "btnAgregarFamilia");
            btnAgregarPerfil.Text = new IdiomaBLL().Traducir(Idioma, "btnAgregarPerfil");
            btnBorrarFamilia.Text = new IdiomaBLL().Traducir(Idioma, "btnEliminarFamilia");
            btnBorrarPerfil.Text = new IdiomaBLL().Traducir(Idioma, "btnEliminarPerfil");
        }
    }
}
