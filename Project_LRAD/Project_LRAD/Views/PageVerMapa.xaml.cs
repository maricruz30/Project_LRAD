using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Project_LRAD.Views;
using Plugin.Media.Abstractions;


namespace Project_LRAD.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageVerMapa : ContentPage
    {
        double latd, longt; //para latitud y longitud
        string idd, sitioo, fotoo, paiss, notaa;
        

        public PageVerMapa()
        {
            InitializeComponent();
            cmap();
        }

        public PageVerMapa( double latitud, double longitud)
        {
            InitializeComponent();
            cmapa2(latitud,longitud);

           
        }

        public PageVerMapa(string id, string foto,string sitio, double latitud, double longit, string pais, string nota)
        {
            latd = latitud;
            longt = longit;
            idd = id;
            fotoo= foto;
            paiss= pais;
            notaa=nota;
            sitioo = sitio;
            InitializeComponent();
            cmapa2(latd,longt);

            ubication.IsVisible= true;
            btnCambiarU.IsVisible= true;

        }


        public async void cmapa2(double latitud, double longitud)
        {
        
            try
            {
                    var posicion = new Position(latitud,longitud);
                    mapa.MoveToRegion(MapSpan.FromCenterAndRadius(posicion, Distance.FromKilometers(1)));
                mapa.MoveToRegion(MapSpan.FromCenterAndRadius(posicion, Distance.FromKilometers(8)));
                ubication.IsVisible= false;
                    btnCambiarU.IsVisible= false;
            }
            catch(Exception ex) { 
            
            
            }
        }

        private async void cmap()
        {
            try
            {
                var location = await Geolocation.GetLocationAsync(); //localizacion activa

                if (location != null)
                {
                    var posicion = new Position(location.Latitude, location.Longitude);
                    mapa.MoveToRegion(MapSpan. FromCenterAndRadius(posicion, Distance.FromKilometers(1)));

                   
                    latd = posicion.Latitude;
                    longt = posicion.Longitude;

                    //var po = posicion.;
                   // await DisplayAlert("AVISO", po, "OK");

                    ubication.Text = "Latitud: " + posicion.Latitude.ToString() + "\n" + "Longitud: " + posicion.Longitude.ToString();

                }
              
            }
            catch (Exception ex)
            {

            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            mapa.MapClicked += HandleMapClicked;
        }

        private void btnCambiarU_Clicked(object sender, EventArgs e)
        {
           try 
            {
               // string[] sit = { idd, fotoo,sitioo, latd.ToString(), longt.ToString(), paiss, notaa }; //para latitud y longitud}
                var addSitios = new Views.PageAddSitios(latd,longt);
                Navigation.PushAsync(addSitios);
            }
            catch (Exception ex)
            {
                DisplayAlert("AVISO", "ERROR " + ex, "OK");
            }
        }

        private void HandleMapClicked(object sender, MapClickedEventArgs e)
        {
            var postion = e.Position;
            latd = postion.Latitude;
            longt = postion.Longitude;

            ubication.Text= "Latitud: " +  postion.Latitude.ToString() + "\n" + "Longitud: " + postion.Longitude.ToString();
           

        }
    }
}