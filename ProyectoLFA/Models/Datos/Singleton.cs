﻿using System.Collections.Generic;

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
       public List<string> Texto_Completo = new List<string>();
       public List<string> Sets = new List<string>();
       public List<string> Tokens = new List<string>();
       public List<string> Simbolos = new List<string>();
       public List<string> Actions = new List<string>();  
       public Clases.Analizar Analizar = new Clases.Analizar();
       public Clases.Node Arbol = new Clases.Node();
       public Clases.Scanner Scanner=new Clases.Scanner();
       public Dictionary<int, List<string>> TablaFollow = new Dictionary<int, List<string>>();
       public List<string> Transiciones = new List<string>();
       public List<string[]> TablaTrancisiones = new List<string[]>();
       public List<Clases.Node> ListaImprimir = new List<Clases.Node>();
    }
}
