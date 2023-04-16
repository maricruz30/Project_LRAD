using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Media;
using Xamarin.Essentials;
using System.Xml.Linq;
using System.Diagnostics;
using Project_LRAD.Models;

namespace Project_LRAD.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageAddContact : ContentPage
    {
        //variable global
        Plugin.Media.Abstractions.MediaFile fileFoto = null;
        Stream stream2;
        string MODO = "";
        string metodous = "";


        private String ImagetoBase64()
        {
            if (fileFoto != null)
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    Stream stream = fileFoto.GetStream();
                    stream.CopyTo(memory);
                    byte[] bytefoto = memory.ToArray();
                    string fotoBase64 = Convert.ToBase64String(bytefoto);
                    return fotoBase64;
                }
            }
            return null;
        }
        public PageAddContact()
        {
            InitializeComponent();
            MODO = "Crear";
           
        }

        public PageAddContact(int IdUsuario)
        {
            InitializeComponent();
            MODO = "Actualizar";
            cargar(IdUsuario);
            
        }
       


        public  async void cargar(int IdContact)
        {
            var contacto = await App.DBContactos.GetContacto(IdContact);
            if (contacto != null)
            {
                txtIdContact.Text = contacto.id.ToString();
                txtNombre.Text = contacto.nombre;
                txtTelefono.Text= contacto.telefono.ToString();
                txtEdad.Text= contacto.edad.ToString();
                txtNota.Text = contacto.nota;
                cmbPais.SelectedItem=contacto.pais;

                string base64Imagen = contacto.foto;
                metodous = contacto.foto;
                byte[] Base64Stream = Convert.FromBase64String(base64Imagen);
                foto.Source = ImageSource.FromStream(() => new MemoryStream(Base64Stream));
            }
        }

        private async void btnFoto_Clicked(object sender, EventArgs e)
        {
            var resp = await DisplayAlert("?", "¿ACCEDER A?", "CAMARA", "ARCHIVO");

            if (resp == true)
            {
                fileFoto = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    Directory = "MiApp",
                    Name = "foto.jpg"
                });

                if (fileFoto != null)
                {
                    foto.Source = ImageSource.FromStream(() => { return fileFoto.GetStream(); }); //espresion lamba
                    stream2 = null;
                }
            }
            else
            {
                try
                {
                    var file = await FilePicker.PickAsync(
                        new PickOptions
                        {
                            FileTypes = FilePickerFileType.Images
                        });

                    if (file == null)
                        return;

                    var stream = await file.OpenReadAsync();
                    stream2 = await file.OpenReadAsync();
                    foto.Source = ImageSource.FromStream(() => stream);  
                }
                catch (Exception ex)
                {

                }
            }
            
        }
        private String Image2toBase64()
        {


            if (stream2 != null)
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    //Stream stream = fileFoto.GetStream();
                    stream2.CopyTo(memory);
                    byte[] bytefoto = memory.ToArray();
                    string fotoBase64 = Convert.ToBase64String(bytefoto);
                    //txtNota.Text = fotoBase64.ToString();
                    return fotoBase64;
                }
            }
            return null;
        }

        
        private async void btnAgregarF_Clicked(object sender, EventArgs e)
        {
           

            try
            {
                var file = await FilePicker.PickAsync(
                    new PickOptions 
                    {
                        FileTypes = FilePickerFileType.Images
                    });

                if (file == null)
                    return;

               // info.Text = file.FileName;

                // str64= file.FileName;

                var stream = await file.OpenReadAsync();
                foto.Source=ImageSource.FromStream(() => stream);

            }
            catch (Exception ex)
            {

            }
            

        }

        private async void btnSalvar_Clicked(object sender, EventArgs e)
        {
            if (txtNombre.Text == null)
            {
                await DisplayAlert("ALERTA", "DEBES ESCRIBIR UN NOMBRE", "OK");
                txtNombre.Focus();
            }
            else if (txtTelefono.Text == null)
            {
                await DisplayAlert("ALERTA", "DEBES ESCRIBIR UN TELEFONO", "OK");
                txtTelefono.Focus();
            }
            else if (txtEdad.Text == null)
            {
                await DisplayAlert("ALERTA", "DEBES ESCRIBIR UNA EDAD", "OK");
                txtEdad.Focus();
            }
            else if (txtNota.Text == null)
            {
                await DisplayAlert("ALERTA", "DEBES ESCRIBIR UNA NOTA", "OK");
                txtEdad.Focus();
            }
            else if (cmbPais.SelectedItem == null)
            {
                await DisplayAlert("ALERTA", "DEBES SELECCIONAR UN PAIS", "OK");
            }
            else if (foto.Source == null)
            {
                await DisplayAlert("ALERTA", "DEBES AGREGAR UNA FOTO", "OK");
            }
            else
            {
               // var metodous = "";

                if (stream2 != null)
                {
                    metodous = Image2toBase64();
                }
                else if (fileFoto==null && stream2==null)
                {
                    //metodous = metodous;
                }
                else
                {
                    metodous = ImagetoBase64();
                }
                

                if (MODO== "Crear") 
                {
                    var user = new Models.ContactosModel
                    {
                        nombre = txtNombre.Text,
                        telefono = Convert.ToInt32(txtTelefono.Text),
                        edad = Convert.ToInt32(txtEdad.Text),
                        pais = cmbPais.SelectedItem.ToString(),
                        nota = txtNota.Text,
                        foto = metodous,

                    };

                    if (await App.DBContactos.saveContacto(user) > 0)
                        await DisplayAlert("AVISO", "CONTACTO GUARDADO CORRECTAMENTE!", "OK");

                    else
                        await DisplayAlert("AVISO", "ERROR", "OK");
                }
                else if (MODO== "Actualizar")
                {
                    var user = new Models.ContactosModel
                    {
                        id = Convert.ToInt32(txtIdContact.Text),
                        nombre = txtNombre.Text,
                        telefono = Convert.ToInt32(txtTelefono.Text),
                        edad = Convert.ToInt32(txtEdad.Text),
                        pais = cmbPais.SelectedItem.ToString(),
                        nota = txtNota.Text,
                        foto = metodous


                    };

                    if (await App.DBContactos.saveContacto(user) > 0)
                        await DisplayAlert("AVISO", "CONTACTO ACTUALIZO CORRECTAMENTE!", "OK");

                    //por aqui deberia tener un metodo que limpie

                    else
                        await DisplayAlert("AVISO", "ERROR", "OK");

                }
            }    
        }

       
    }
}