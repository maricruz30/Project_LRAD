using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Project_LRAD.Views;
using Project_LRAD.Models;
using Xamarin.Essentials;

namespace Project_LRAD.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageMostrarContacto : ContentPage
    {

        public PageMostrarContacto()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            listContact.ItemsSource = await App.DBContactos.getlistContacto();
        }

        public async void llenarDatos()
        {
            var alumnolist = await App.DBContactos.getlistContacto();

            if (alumnolist != null)
            {
                listContact.ItemsSource = alumnolist;
            }
        }

        private void btnActualizarC_Clicked(object sender, EventArgs e)
        {
            try
            {
                var obj = (ContactosModel)listContact.SelectedItem;
                if (!string.IsNullOrEmpty(obj.id.ToString()))
                {
                    int id = int.Parse(obj.id.ToString());
                    Views.PageAddContact pageAddContact = new Views.PageAddContact(id);
                    Navigation.PushAsync(pageAddContact);
                    //Views.PageInitial pageInitial = new Views.PageInitial(id);
                    this.OnDisappearing();

                }
            }
            catch(Exception ex)
            {

            }     
        }

        private async void listContact_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var resp = await DisplayAlert("¿PREGUNTA?", "¿DESEAS LLAMAR A ESTE CONTACTO?", "OK", "NO");

            if (resp == true)
            {
                var obj = (ContactosModel)e.SelectedItem;

                if (!string.IsNullOrEmpty(obj.id.ToString()))
                {
                    try
                    {
                        var contacto = await App.DBContactos.GetContacto(Convert.ToInt32(obj.id));
                        if (contacto != null)
                        {
                            int num = contacto.telefono;
                            
                            PhoneDialer.Open(num.ToString());
                            

                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("AVISO", "INTENTELO DE NUEVO", "OK");
                    }
                }
            }

            
            //PageAddContact.cargarText(ContactosModel contactosmodel);

        }

        private async void btnEliminarC_Clicked(object sender, EventArgs e)
        {
            try
            {
                var obj = (ContactosModel)listContact.SelectedItem;
                if (!string.IsNullOrEmpty(obj.id.ToString()))
                {
                    var contacto = await App.DBContactos.GetContacto(Convert.ToInt32(obj.id));
                    if (contacto != null)
                    {
                        await App.DBContactos.DeleteContactos(contacto);
                        await DisplayAlert("AVISO", "CONTACTO ELIMINADO CORRECTAMENTE!", "OK");
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

        private async void btnCompartir_Clicked(object sender, EventArgs e)
        {
            try
            {
                var obj = (ContactosModel)listContact.SelectedItem;
                if (!string.IsNullOrEmpty(obj.id.ToString()))
                {
                    var contacto = await App.DBContactos.GetContacto(Convert.ToInt32(obj.id));
                    if (contacto != null)
                    {
                        await Share.RequestAsync(new ShareTextRequest()
                        {
                            Title = "Compartir Contacto",
                            Subject = "Contacto Compartido",
                            Text = contacto.nombre + "\n" + contacto.telefono.ToString()
                        });

                    }
                    else
                    {
                        await DisplayAlert("AVISO", "ERROR", "OK");
                    }

                }
            } catch (Exception ex)
            {
              
            }   
        }

        private void btnVerIMG_Clicked(object sender, EventArgs e)
        {
            try
            {
                var obj = (ContactosModel)listContact.SelectedItem;
                if (!string.IsNullOrEmpty(obj.id.ToString()))
                {
                    int id = int.Parse(obj.id.ToString());
                    string formulario = "CONTACTO";
                    Views.PageVerImagen verImg = new Views.PageVerImagen(id, formulario);
                    Navigation.PushAsync(verImg);
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("AVISO", "SELECCIONA UN CONTACTO", "OK");
            }
        }
    }
}