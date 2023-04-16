using Project_LRAD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Project_LRAD.Models;


namespace Project_LRAD.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageMostrarSitio : ContentPage
    {
        public PageMostrarSitio()
        {
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            listSities.ItemsSource = await App.DBSitios.getlistSitio();
        }

        public async void llenarDatos()
        {
            var sitieslist = await App.DBSitios.getlistSitio();

            if (sitieslist != null)
            {
                listSities.ItemsSource = sitieslist;
            }
        }
        private async void btnEliminarSitio_Clicked(object sender, EventArgs e)
        {
            try
            {
                var obj = (SitiosModel)listSities.SelectedItem;
                if (!string.IsNullOrEmpty(obj.id.ToString()))
                {
                    var sitio = await App.DBSitios.GetSitio(Convert.ToInt32(obj.id));
                    if (sitio != null)
                    {
                        await App.DBSitios.DeleteSitios(sitio);
                        await DisplayAlert("AVISO", "SITIO ELIMINADO CORRECTAMENTE!", "OK");
                        llenarDatos();
                    }
                    else
                    {
                        await DisplayAlert("AVISO", "ERROR", "OK");
                    }

                }
            }
            catch (Exception ex)
            {

            }

        }

        private void btnActualizarS_Clicked(object sender, EventArgs e)
        {
            try
            {
              
                var obj = (SitiosModel)listSities.SelectedItem;
                if (!string.IsNullOrEmpty(obj.id.ToString()))
                {
                    int id = int.Parse(obj.id.ToString());
                    Views.PageAddSitios pageAddSities = new Views.PageAddSitios(id);
                    Navigation.PushAsync(pageAddSities);
                }
            }
            catch (Exception ex)
            {

            }

        }

        private async void btnCompartirS_Clicked(object sender, EventArgs e)
        {

            try
            {
                var obj = (SitiosModel)listSities.SelectedItem;
                if (!string.IsNullOrEmpty(obj.id.ToString()))
                {
                    var sitio = await App.DBSitios.GetSitio(Convert.ToInt32(obj.id));
                    if (sitio != null)
                    {
                        await Share.RequestAsync(new ShareTextRequest()
                        {
                            Title = "Compartir Sitio",
                            Subject = "Sitio Compartido",
                            Text = "Latitud: "+sitio.latitud + "\n" +
                            "Longitud: " + sitio.longitud
                        });

                    }
                    else
                    {
                        await DisplayAlert("AVISO", "ERROR", "OK");
                    }

                }
            }
            catch (Exception ex)
            {

            }

        }

        private async void listSities_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var resp = await DisplayAlert("¿PREGUNTA?", "¿DESEAS ABRIR EL MAPA?", "OK", "NO");

            if (resp == true)
            {
                var obj = (SitiosModel)e.SelectedItem;

                if (!string.IsNullOrEmpty(obj.id.ToString()))
                {
                    try
                    {
                        var sitio = await App.DBSitios.GetSitio(Convert.ToInt32(obj.id));
                        if (sitio != null)
                        {
                            double latd = Convert.ToDouble(sitio.latitud);
                            double longd = Convert.ToDouble(sitio.longitud);

                        //var n= Map.OpenAsync(latd, longd);
                            

                            Views.PageVerMapa map = new Views.PageVerMapa(latd,longd);
                            Navigation.PushAsync(map);
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("AVISO", "INTENTELO DE NUEVO", "OK");
                    } 
                }
            }
        }

        private void btnVerIMG_Clicked(object sender, EventArgs e)
        {
            try
            {
                var obj = (SitiosModel)listSities.SelectedItem;
                if (!string.IsNullOrEmpty(obj.id.ToString()))
                {
                    int id = int.Parse(obj.id.ToString());
                    string formulario = "SITIO";
                    Views.PageVerImagen verImg = new Views.PageVerImagen(id, formulario);
                    Navigation.PushAsync(verImg);
                    //Views.PageInitial pageInitial = new Views.PageInitial(id);

                }
            }
            catch (Exception ex)
            {
                DisplayAlert("AVISO", "SELECCIONA UN SITIO", "OK");
            }
        }
    }
}