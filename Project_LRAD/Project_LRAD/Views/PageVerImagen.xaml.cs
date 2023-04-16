using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace Project_LRAD.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageVerImagen : ContentPage
    {
        string FORM = "";

        int id;
        public PageVerImagen()
        {
            InitializeComponent();
        }

        public PageVerImagen(int Id, string fORM)
        {
            InitializeComponent();
            cargar(Id, fORM);
            FORM = fORM;
            id= Id;
        }


        public async void cargar(int Id,string FORMU)
        {
           // FORM = FORMU;

            byte[] Base64Stream = null;
            string base64Imagen = null; 

            if (FORMU == "CONTACTO")
            {
                var contacto = await App.DBContactos.GetContacto(Id);
                if (contacto != null)
                {
                    nombre.Text = contacto.nombre;

                    base64Imagen = contacto.foto;
                    Base64Stream = Convert.FromBase64String(base64Imagen);
                    v_image.Source = ImageSource.FromStream(() => new MemoryStream(Base64Stream));
                }
            }
            else if (FORMU == "SITIO")
            {
                var sitio = await App.DBSitios.GetSitio(Id);
                nombre.Text = sitio.Nomsitio;

                base64Imagen = sitio.foto;
                Base64Stream = Convert.FromBase64String(base64Imagen);
                v_image.Source = ImageSource.FromStream(() => new MemoryStream(Base64Stream));
            }
            
        }  
       
    }
}