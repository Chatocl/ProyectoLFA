using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.FileIO;
using ProyectoLFA.Models;
using ProyectoLFA.Models.Datos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProyectoLFA.Controllers
{
    public class HomeController : Controller
    {
        private IHostingEnvironment Environment;
        public HomeController(IHostingEnvironment _environment)
        {
            Environment = _environment;

        }
        public IActionResult GenerarAutomata() 
        { 
            return View();
        }

        public IActionResult Automata()
        {
            string temp = "";
            List<string> ActionsTokens = new List<string>();  
            for (int i = 0; i < Singleton.Instance.Actions.Count(); i++)
            {
                ActionsTokens.Add(Singleton.Instance.Actions[i].Substring(0,1));
                temp = Regex.Replace(Singleton.Instance.Actions[i],@"^\s*(\d+)\s*\=\s*'","");
                temp = Regex.Replace(temp, @"'$", "");
                Singleton.Instance.Actions[i]=temp;
            }
            Singleton.Instance.Scanner.crearArchivo(Singleton.Instance.Sets,Singleton.Instance.Actions,ActionsTokens,Singleton.Instance.TablaTrancisiones,Singleton.Instance.Transiciones,Singleton.Instance.Simbolos);
            return View("GenerarAutomata");
        }
        public IActionResult Index()
        {
            
            return View();
        }
        public IActionResult First()
        {

            return View();
        }
        public IActionResult Follow()
        {
            List<string> ImprimirFollow = new List<string>();
            for (int i = 1; i < Singleton.Instance.TablaFollow.Keys.Count; i++)
            {
                string valor = "";
                for (int a = 0; a < Singleton.Instance.TablaFollow[i].Count; a++)
                {
                    valor += Singleton.Instance.TablaFollow[i][a] + ",";
                }
                ImprimirFollow.Add(valor.ToString());
            }
           
            return View(ImprimirFollow);
        }

        public IActionResult Privacy()
        {
            Repuesta repuesta = new Repuesta();
            return View(repuesta);
        }

        public IActionResult Tabla()
        {
            try
            {
                List<string> list = new List<string>();
                string patron = @"^\s*TOKEN\s*\d+\s*=\s*";
                string resultado = "";
                string temp = "";
                Clases.ArbolExpresiones arbolExpresiones = new Clases.ArbolExpresiones();
           
                for (int a = 0; a < Singleton.Instance.Tokens.Count(); a++)
                {
                    if (a < Singleton.Instance.Tokens.Count() - 1)
                    {
                        resultado += "(" + Regex.Replace(Singleton.Instance.Tokens[a], patron, "") + ") |";
                    }
                    else
                    {
                        temp = Regex.Replace(Singleton.Instance.Tokens[a], patron, "");
                        temp = Regex.Replace(temp, @"\s*{\s*RESERVADAS\(\s*\)\s*}\s*", "");
                        resultado += temp;
                    }
                }

                while (resultado.Length > 0)
                {
                    Match match = Regex.Match(resultado, @"(^\w+|^'.'|^\*|^\+|^\?|^\||^\(|^\))");
                    if (match.Value != "")
                    {
                        list.Add(match.Value);
                    }
                    resultado = resultado.Remove(0, match.Value.Length);
                    if (resultado.Length > 0)
                    {
                        if (resultado[0] == ' ')
                        {
                            resultado = resultado.Remove(0, 1);
                        }
                    }

                }

                for (int i = 1; i < list.Count; i++)
                {
                    if (i < list.Count - 1)
                    {
                        string temp2 = list[i];
                        string temp3 = list[i + 1];
                        string temp4 = "";

                        if (i != 0)
                        {
                            temp4 = list[i - 1];
                            if (temp2 == ")" && arbolExpresiones.EsSimbolo(temp3))
                            {
                                list.Insert(i, ".");
                                i++;
                            }
                            else if (arbolExpresiones.EsSimbolo(temp2) && arbolExpresiones.EsSimbolo(temp3))
                            {
                                list.Insert(i + 1, ".");
                                i++;
                            }
                            else if (arbolExpresiones.EsSimbolo(temp2) && temp3 == "(")
                            {
                                list.Insert(i + 1, ".");
                                i++;
                            }

                        }

                    }
                }
                list.Insert(0, "(");
                list.Add(")");
                list.Add(".");
                list.Add("#");
                Singleton.Instance.Arbol = arbolExpresiones.ContruirArbol(list);
                arbolExpresiones.PostOrder(Singleton.Instance.Arbol);
                arbolExpresiones.Terminales(Singleton.Instance.Arbol,Singleton.Instance.TablaFollow);
                arbolExpresiones.CalcularFollow(Singleton.Instance.Arbol, Singleton.Instance.TablaFollow);
                Singleton.Instance.ListaImprimir = arbolExpresiones.GetList(Singleton.Instance.Arbol);
                Singleton.Instance.Simbolos = arbolExpresiones.ObSimbolosValor();
                (Singleton.Instance.Transiciones, Singleton.Instance.TablaTrancisiones) = arbolExpresiones.TablaTrancisiones(Singleton.Instance.Arbol, Singleton.Instance.TablaFollow);
                List<FirstLast> Imprimir = new List<FirstLast>();
                int pos = 0;
                for (int i = 0; i < Singleton.Instance.ListaImprimir.Count; i++)
                {
                    var newSimbolo = new Models.Datos.FirstLast()
                    {
                        Id = (i + 1),
                        simbolo = Singleton.Instance.ListaImprimir[i].Valor,
                        first = Singleton.Instance.ListaImprimir[i].First,
                        last = Singleton.Instance.ListaImprimir[i].Last,
                        Nullable = Singleton.Instance.ListaImprimir[i].Anulable
                    };
                    Imprimir.Add(newSimbolo);
                }
               
                return View(Imprimir);
            }
            catch (Exception)
            {

                
                
            }
            return View(null);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public ActionResult CargarArchivo(IFormFile File)
        {
            // Lectura del archivo y eliminación de los intros y espacios sin contenido textual 
            Repuesta repuesta = new Repuesta();
            try
            {
                Singleton.Instance.Texto_Completo.Clear();
                Singleton.Instance.Texto.Clear();
                if (File != null)
                {
                    string path = Path.Combine(this.Environment.WebRootPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string FileName = Path.GetFileName(File.FileName);
                    string FilePath = Path.Combine(path, FileName);
                    using (FileStream stream = new FileStream(FilePath, FileMode.Create))
                    {
                        File.CopyTo(stream);
                    }
                    using (TextFieldParser txtFile = new TextFieldParser(FilePath))
                    {
                        txtFile.CommentTokens = new string[] { "#" };
                        txtFile.SetDelimiters(new string[] { "," });
                        txtFile.HasFieldsEnclosedInQuotes = true;
                        Regex regex = new Regex("[a-zA-Z\\{\\}]");

                        while (!txtFile.EndOfData)
                        {
                            string data = txtFile.ReadLine();
                            if (data != null)
                            {
                                Singleton.Instance.Texto_Completo.Add(data);
                                if (regex.IsMatch(data))
                                {
                                    Singleton.Instance.Texto.Add(data);
                                }
                            }

                        }
                        repuesta.RepuestaId = "Ingreso del archivo correcto. Presione \"Analizar Texto\" para continuar ";
                    }
                }
            }
            catch (Exception)
            {
                repuesta.RepuestaId = "Ingreso un archivo incorrecto o vacio";
                
            }
           
            return View("Privacy",repuesta);
        }
        public ActionResult Analisis_Lexico()
        {
            List<int> ResTexto = new List<int>();
            List<string> Tokens = new List<string>(); 
            List<string> Actions = new List<string>();
            int a=0;
            Repuesta repuesta = new Repuesta();
            repuesta.RepuestaId = "";

            try
            {
                (ResTexto,Tokens,Actions,Singleton.Instance.Sets) = Singleton.Instance.Analizar.Analizar_Texto(Singleton.Instance.Texto);
                if (ResTexto[0] == -1) 
                {
                    repuesta.RepuestaId = "No se han encontrado errores.";
                    Singleton.Instance.Tokens = Tokens;
                    Singleton.Instance.Actions = Actions;
                }
                else
                {
                    for (a = 0; a < Singleton.Instance.Texto_Completo.Count(); a++)
                    {
                        if (Singleton.Instance.Texto_Completo[a].Contains(Singleton.Instance.Texto[ResTexto[1]]))
                        {
                            break;
                        }
                    }
                    repuesta.RepuestaId = "Se ha encontrado un error en la fila " + (a+1) +" siendo esta: " + Singleton.Instance.Texto_Completo[a];
                }
            }
            catch (Exception)
            {
                repuesta.RepuestaId = "Ingreso un archivo incorrecto o vacio";
               
            }
           
            return View("Privacy", repuesta);
        }
    }
}
