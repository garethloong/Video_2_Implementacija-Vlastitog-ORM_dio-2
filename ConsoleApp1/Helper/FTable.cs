using ConsoleApp1.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Helper
{
    // Genericka klasa koja radi sa svim tipovima iz Modela - na ovaj nacin se sve informacije o strukturi baze podataka cuvaju na jednom mjestu (u Modelu),
    // te propertiji klasa iz Modela predstavljaju kolone istoimenih tabela, a spomenute klase predstavljaju tabele u DB.
    internal class FTable<T> where T : class, IEntity
    {
        // C# Reflection: Type is the class that contains metadata about some other class
        public Type tip { get; set; }

        public FTable() // constructor must be public so we can  create objects outside of this class
        {
            // Podaci = new List<T>();

            // T is generic parameter of the class FTable
            // C# Reflection: Type is the class that contains metadata about some other class
            tip = typeof(T);
        }

        public List<T> GetAll()
        {
            //List<T> notDeleted = new List<T>();
            //foreach (var item in Podaci)
            //{
            //    if (!item.IsDeleted)
            //        notDeleted.Add(item);
            //}
            //return notDeleted;

            List<T> result = new List<T>();

            /* Block "using" automatically calls function Dispose() on it's finish.
             * Object in the block "using" must implement interface IDisposable which 
             * obligate that function Dispose() must be implemented. */
            SqlConnection k = Connection.getInstance();

            // type.Name contains the name of class T e.g. "select * from Student"
            string sql = "select * from " + tip.Name;
            SqlCommand cmd = new SqlCommand(sql, k);

            // Executes command cmd which is of type Query (select)
            SqlDataReader rdr = cmd.ExecuteReader();

            // Function Read() moves the pointer to the record.
            // If Read() returns "false" that means that pointer has came to the end i.e. it does not point at any record.
            while (rdr.Read())
            {
                // Function NapraviT() creates an instance of class T, then takes value from the current record (DB row), and then sets
                // values of the current record to the instance of type T.
                T newT = NapraviT(rdr);
                result.Add(newT);
            }

            return result;
        }

        private T NapraviT(SqlDataReader rdr)
        {
            // Creates an instance of the class T
            T newT = Activator.CreateInstance(typeof(T)) as T;

            // C# Reflection: "type.GetProperties()" returns an array of objects of type PropertyInfo (e.g. Id, IsDeleted, Ime, Prezime, BrojIndexa)
            // C# Reflection: "PropertyInfo" holds metadata about class's property e.g. public string BrojIndexa { get; set; }
            foreach (PropertyInfo p in tip.GetProperties())
            {
                if (p.PropertyType == typeof(T) || p.PropertyType == typeof(int) || p.PropertyType == typeof(bool) || p.PropertyType == typeof(string))
                {
                    // "p.Name" is the name of the property e.g. BrojIndexa in the class Student.
                    // rdr represents the current record on which row pointer points at.
                    // rdr[nesto] represents access to the column "nesto", i.e. it's accessing a cell from the current row.
                    object v = rdr[p.Name]; // Non of the rows in the record (p.Name) must not be null in the DB (if it is, the program will throw an exception).
                    // sets the value "v" (e.g. IB231047) to a property (e.g. BrojIndexa) of the object "newT" of type T (e.g. Student)
                    p.SetValue(newT, v);
                }
            }

            return newT;
        }

        public T GetById(int ID)
        {
            //foreach (T item in Podaci)
            //{
            //    if (item.Id == ID)      // in order access Id, we need to set constraint on a class FTable that only IEntity interface or types that implement this interface can be used as the type T
            //        return item;
            //}
            //
            //return null;    // Cannot return null for the T type because T could be a struct (which is not nullable), so we have to set constraint on a class FTable that T must be a class

            SqlConnection k = new SqlConnection();

            // "select * from Student where Id = 4"
            string sql = "select * from " + tip.Name + " where id = " + ID; // prone to SQL injection
            string sql1 = "select * from @tip.Name + where id = @ID";   // immune to SQL injection
            SqlCommand cmd = new SqlCommand(sql, k);

            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                T newT = NapraviT(rdr);
                return newT;
            }

            return null;
        }

        // private int identity = 0;
        public void Insert(T t)
        {
            //t.Id = ++identity;
            //Podaci.Add(t);

            string kolone = "";
            foreach (PropertyInfo p in tip.GetProperties())
            {
                // Because the Identity (Auto-increment) is turned on for the column Id in the table, we need to skip this column
                if (p.Name == "Id")
                    continue;
                if (kolone.Length > 0)
                    kolone += ",";
                kolone += p.Name;
            }

            string vrijednosti = "";
            foreach (PropertyInfo p in tip.GetProperties())
            {
                if (p.Name == "Id")
                    continue;
                if (vrijednosti.Length > 0)
                    vrijednosti += ",";
                object v = p.GetValue(t);   // Type object is used because p.GetValue(t) returns property value "p" of the object "t" and type of "p" can be int, bool, string or some other type.
                vrijednosti += "'" + v + "'";
            }

            // "insert into Student(Ime, Prezime, BrojIndexa, IsDeleted) values ('A', 'B', 'IB231047', 'False')"
            string sql = "insert into " + tip.Name + "(" + kolone + ") values (" + vrijednosti + ")";
            SqlConnection k = Connection.getInstance();
            SqlCommand cmd = new SqlCommand(sql, k);
            cmd.ExecuteNonQuery();
        }

        public void Update(T t)
        {
            //Delete(t);
            //Insert(t);

            // Update() is similar to Insert()
        }

        public void Delete(int id)
        {
            //T t = GetById(id);
            //Podaci.Remove(t);

            SqlConnection conn = Connection.getInstance();

            Type type = typeof(T);
            string sql = "delete from " + type.Name + " where id = " + id;
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.ExecuteNonQuery();  // kad se treba pozvati komanda koja ne vraca nikakav rezultat, dovoljno je pozvati ExecuteNonQuery()

            conn.Close();
        }
        //public void Delete(T t)
        //{
        //    Podaci.Remove(t);
        //}
    }
}
