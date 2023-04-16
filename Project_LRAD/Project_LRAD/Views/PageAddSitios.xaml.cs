using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Project_LRAD.Models;
using Plugin.Media;
using Xamarin.Essentials;
using System.IO;
using System.Diagnostics.Contracts;

namespace Project_LRAD.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageAddSitios : ContentPage
    {
        Plugin.Media.Abstractions.MediaFile fileFoto = null;
        Stream stream2;
        string MODO = "";
        string metodous = "";


        public PageAddSitios()
        {
            InitializeComponent();
            MODO = "Crear";
            cargubic();
        }

        public PageAddSitios(int idSitio)
        {
            InitializeComponent();
            MODO = "Actualizar";
            cargar(idSitio);
        }

        public PageAddSitios(double latd, double longd)
        {
            InitializeComponent();
 
            txtLatitud.Text = latd.ToString();
            txtLongitud.Text = longd.ToString();

            if (txtIdSitio!= null)
            {
                MODO = "Actualizar";
            }
            else
            {
                MODO = "Crear";
            }
        }

        public PageAddSitios(string[] camp)
        {
            InitializeComponent();

            if(!string.IsNullOrEmpty(camp[0]))
            {
                txtIdSitio.Text = camp[0];
            }
            else if (!string.IsNullOrEmpty(camp[1]))
            {
                foto.Source = camp[1].ToString();
            }
            else if (!string.IsNullOrEmpty( camp[2]))
            {
                txtSitio.Text = camp[2].ToString();
            }
            else if (!string.IsNullOrEmpty(camp[3]))
            {
                txtLatitud.Text = camp[3].ToString();
            }
            else if (!string.IsNullOrEmpty(camp[4]))
            {
                txtLongitud.Text = camp[4].ToString();
            }
            else if (!string.IsNullOrEmpty(camp[5]))
            {
                cmbPais.SelectedItem = camp[5].ToString();
            }
            else if (!string.IsNullOrEmpty(camp[6]))
            {
                txtNota.Text = camp[6].ToString();
            }

            if (txtIdSitio != null)
            {
                MODO = "ACTUALIZAR";
            }
            else
            {
                MODO = "CREAR";
            }
        }

        private async void cargubic()
        {
            try
            {
                var location = await Geolocation.GetLocationAsync(); //localizacion activa

                if (location != null)
                {
                    txtLatitud.Text = Convert.ToString(location.Latitude);
                    txtLongitud.Text = Convert.ToString(location.Longitude);
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            
        }

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

        public void cargarUbicacion(double latd, double longt)
        {
            txtLatitud.Text = Convert.ToString(latd);
            txtLongitud.Text = Convert.ToString(longt);
        }

        public async void cargar(int IdSitio)
        {
            var sitio = await App.DBSitios.GetSitio(IdSitio);
            if (sitio != null)
            {
                txtIdSitio.Text = sitio.id.ToString();
                txtSitio.Text = sitio.Nomsitio.ToString();
                txtLatitud.Text = sitio.latitud.ToString();
                txtLongitud.Text = sitio.longitud.ToString();
                txtNota.Text = sitio.nota;
                cmbPais.SelectedItem = sitio.pais;
                metodous = sitio.foto;

                string base64Imagen = sitio.foto;
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
                }
            }
            else if (resp == false)
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
                    stream2= await file.OpenReadAsync();
                    foto.Source = ImageSource.FromStream(() => stream);
                

                }
                catch (Exception ex)
                {

                }
            }


            
        }

        private  async void btnSalvar_Clicked(object sender, EventArgs e)
        {
            if (txtSitio.Text == null)
            {
                await DisplayAlert("ALERTA", "DEBES ESCRIBIR UN NOMBRE DE SITIO", "OK");
                txtSitio.Focus();
            }
            else if (txtLatitud.Text == null)
            {
                await DisplayAlert("ALERTA", "DEBES ESCRIBIR UNA LATITUD", "OK");
                txtLatitud.Focus();
            }
            else if (txtLongitud.Text == null)
            {
                await DisplayAlert("ALERTA", "DEBES ESCRIBIR UNA LONGITUD", "OK");
                txtLongitud.Focus();
            }
            else if (txtNota.Text == null)
            {
                await DisplayAlert("ALERTA", "DEBES ESCRIBIR UNA NOTA", "OK");
                txtNota.Focus();
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

                if (stream2 != null)
                {
                    metodous = Image2toBase64();
                }
                else if (fileFoto == null && stream2 == null)
                {
                    //metodous = metodous;
                }
                else
                {
                    metodous = ImagetoBase64();
                }

                if (MODO == "Crear")
                {
                    var sitio = new Models.SitiosModel
                    {
                        id = Convert.ToInt32(txtIdSitio.Text),
                        Nomsitio = txtSitio.Text,
                        latitud = Convert.ToDecimal(txtLatitud.Text),
                        longitud = Convert.ToDecimal(txtLongitud.Text),
                        pais = cmbPais.SelectedItem.ToString(),
                        nota = txtNota.Text,
                        foto = metodous


                    };

                    if (await App.DBSitios.saveSitio(sitio) > 0)
                        await DisplayAlert("AVISO", "SITIO GUARDADO CORRECTAMENTE!", "OK");

                    else
                        await DisplayAlert("AVISO", "ERROR", "OK");
                }
                else if (MODO=="Actualizar")
                {
                    var sitio = new Models.SitiosModel
                    {
                        id = Convert.ToInt32(txtIdSitio.Text),
                        Nomsitio = txtSitio.Text,
                        latitud = Convert.ToDecimal(txtLatitud.Text),
                        longitud = Convert.ToDecimal(txtLongitud.Text),
                        pais = cmbPais.SelectedItem.ToString(),
                        nota = txtNota.Text,
                      foto = metodous

                    };

                    if (await App.DBSitios.saveSitio(sitio) > 0)
                        await DisplayAlert("AVISO", "EL SITIO SE ACTUALIZO CORRECTAMENTE!", "OK");


                    //por aqui deberia tener un metodo que limpie

                    else
                        await DisplayAlert("AVISO", "ERROR", "OK");

                }
            }
        }

       /* private string[] camp()
        {
           
            return campos;
        }*/

        private void btnMapa_Clicked(object sender, EventArgs e)
        {
                var pageMap = new Views.PageVerMapa();
                Navigation.PushAsync(pageMap);

        }
    }
}