using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Application = Xamarin.Forms.Application;


namespace Project_LRAD
{
    public partial class App : Application
    {
        static Controller.ContactosController dbcontactos;
        static Controller.SitiosController dbsitios;

        public static Controller.ContactosController DBContactos
        {
            get
            {
                if (dbcontactos == null)
                {
                    var dbpath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    var dbname = "Contactos.db3"; //instancia sqlite
                    dbcontactos = new Controller.ContactosController(Path.Combine(dbpath, dbname));
                }

                return dbcontactos;
            }

        }

        public static Controller.SitiosController DBSitios
        {
            get
            {
                if (dbsitios == null)
                {
                    var dbpath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    var dbname = "Sitios.db3"; //instancia sqlite
                    dbsitios = new Controller.SitiosController(Path.Combine(dbpath, dbname));
                }

                return dbsitios;
            }

        }

        //public static object DBContactos { get; internal set; }
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Views.PageInitial());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
