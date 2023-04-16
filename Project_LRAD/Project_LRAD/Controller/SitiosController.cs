using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Project_LRAD.Models;
using SQLite;

namespace Project_LRAD.Controller
{
    
    public class SitiosController
    {
        readonly SQLiteAsyncConnection connection;
        public SitiosController(string dpath)
        {
            connection = new SQLiteAsyncConnection(dpath);
            connection.CreateTableAsync<SitiosModel>().Wait();

        }

        public Task<int> saveSitio(SitiosModel sitiosModel)
        {
            if (sitiosModel.id != 0)
                return connection.UpdateAsync(sitiosModel);
            else
                return connection.InsertAsync(sitiosModel);
        }

        /// <summary>
        /// Recuperar todos los contactos
        /// </summary>
        /// <returns></returns>

        public Task<List<SitiosModel>> getlistSitio()
        {
            return connection.Table<SitiosModel>().ToListAsync();
        }

        /// <summary>
        /// Recuperar alumnos por id
        /// </summary>
        /// <param name="sid">Id del contacto que se requiere</param>
        /// <returns></returns>

        public Task<SitiosModel> GetSitio(int sid)
        {
            return connection.Table<SitiosModel>()
                .Where(a => a.id == sid)
                // .ElementAtAsync(2);
                .FirstOrDefaultAsync();
        }

        public Task DeleteSitios(SitiosModel sitiosModel)
        {
            return connection.DeleteAsync(sitiosModel);
        }
    }
}
