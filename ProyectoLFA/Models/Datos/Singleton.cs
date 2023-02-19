using System.Collections.Generic;

namespace ProyectoLFA.Models.Datos
{
    public class Singleton
    {
        private static Singleton _instance = null;
        public static Singleton Instance
        {
            get
            {
                if (_instance == null) _instance = new Singleton();
                return _instance;
            }
        }
       public List<string> Texto = new List<string>();
    }
}
