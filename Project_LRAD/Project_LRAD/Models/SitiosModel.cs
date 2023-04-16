using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project_LRAD.Models
{
    public class SitiosModel
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        [MaxLength(100)]
        public string Nomsitio { get; set; }

        public decimal latitud { get; set; }

        public decimal longitud { get; set; }

        [MaxLength(100)]
        public string pais { get; set; }

        [MaxLength(500)]
        public string nota { get; set; }

        public String foto { get; set; }
    }
}
