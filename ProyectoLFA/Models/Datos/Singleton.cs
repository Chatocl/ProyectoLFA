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
       public List<string> txt_Sets = new List<string>();
       public List<string> txt_Tokens = new List<string>();
       public List<string> txt_Actions = new List<string>();
       public List<string> txt_Error = new List<string>();
       public Clases.Analizar Analizar = new Clases.Analizar();
        
    }
}
