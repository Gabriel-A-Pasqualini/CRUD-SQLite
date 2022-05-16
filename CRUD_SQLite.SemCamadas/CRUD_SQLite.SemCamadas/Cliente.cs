using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_SQLite.SemCamadas
{
    public class Cliente
    {
        public int Id { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public int idade { get; set; }
    }

}
