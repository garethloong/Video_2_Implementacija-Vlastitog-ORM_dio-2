using ConsoleApp1.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Models
{
    class AkademskaGodina : IEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string Opis { get; set; }
    }
}
