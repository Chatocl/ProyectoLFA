using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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

        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult Privacy()
        {
            Repuesta repuesta = new Repuesta();
            return View(repuesta);
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
                throw;
            }
           
            return View("Privacy",repuesta);
        }
        public ActionResult Analisis_Lexico()
        {
            List<int> ResTexto = new List<int>();
            List<string> Tokens = new List<string>();
            int a=0;
            Repuesta repuesta = new Repuesta();
            repuesta.RepuestaId = "";

            try
            {
                (ResTexto,Tokens) = Singleton.Instance.Analizar.Analizar_Texto(Singleton.Instance.Texto);
                if (ResTexto[0] == -1) 
                {
                    repuesta.RepuestaId = "No se han encontrado errores.";
                    Singleton.Instance.Tokens = Tokens;
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
                throw;
            }
           
            return View("Privacy", repuesta);
        }
    }
}
