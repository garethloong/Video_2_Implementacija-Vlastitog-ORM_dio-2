using ConsoleApp1.Helper;
using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Data
{
    class VirtualDB
    {
        // private set prevents users of your class from changing the property’s value, but it could still be changed from within your class
        public static FTable<Student> Studenti { get; private set; }   
        public static FTable<Fakultet> Fakulteti { get; private set; }
        public static FTable<Smjer> Smjerovi { get; private set; }
        public static FTable<AkademskaGodina> AkademskeGodine { get; private set; }   
        public static FTable<UpisGodine> UpisiGodine { get; private set; }

        static VirtualDB()      // static constructor
        {
            Studenti = new FTable<Student>();
            Fakulteti = new FTable<Fakultet>();
            Smjerovi = new FTable<Smjer>();
            AkademskeGodine = new FTable<AkademskaGodina>();
            UpisiGodine = new FTable<UpisGodine>();
        }
    }
}
