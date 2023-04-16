using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Project_LRAD.Models;
using SQLite;

namespace Project_LRAD.Controller
{
    public class ContactosController
    {
        readonly SQLiteAsyncConnection connection;

        public ContactosController(string dpath)
        {
            connection = new SQLiteAsyncConnection(dpath);
            connection.CreateTableAsync<ContactosModel>().Wait();

        }

        public Task<int> saveContacto(ContactosModel contactosmodel)
        {
            if (contactosmodel.id != 0)
                return connection.UpdateAsync(contactosmodel);
            else
                return connection.InsertAsync(contactosmodel);
        }

        /// <summary>
        /// Recuperar todos los contactos
        /// </summary>
        /// <returns></returns>

        public Task<List<ContactosModel>> getlistContacto()
        {
            return connection.Table<ContactosModel>().ToListAsync();
        }

        /// <summary>
        /// Recuperar alumnos por id
        /// </summary>
        /// <param name="cid">Id del contacto que se requiere</param>
        /// <returns></returns>

        public Task<ContactosModel> GetContacto(int cid)
        {
            return connection.Table<ContactosModel>()
                .Where(a => a.id == cid)
                // .ElementAtAsync(2);
                .FirstOrDefaultAsync();
        }

        public Task DeleteContactos(ContactosModel contactosmodel)
        {
            return connection.DeleteAsync(contactosmodel);
        }
    }
}
