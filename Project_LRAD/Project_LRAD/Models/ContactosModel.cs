using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project_LRAD.Models
{
    public class ContactosModel
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        [MaxLength(100)]
        public string nombre { get; set; }


        [MaxLength(8)]
        public int telefono { get; set; }

        [MaxLength(3)]
        public int edad { get; set; }

        [MaxLength(100)]
        public string pais { get; set; }

        [MaxLength(500)]
        public string nota { get; set; }

        public String foto { get; set; }


      
    }
}
