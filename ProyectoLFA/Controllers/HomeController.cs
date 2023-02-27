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
                                if (regex.IsMatch(data))
                                {
                                    Singleton.Instance.Texto.Add(data);
                                }
                            }

                        }
                    }
                    //separación por secciones 
                    for (int a = 0; a < Singleton.Instance.Texto.Count(); a++)
                    {
                        if (Singleton.Instance.Texto[a].Contains("SETS") &&  a < Singleton.Instance.Texto.Count())
                        {
                            while (!Singleton.Instance.Texto[a].Contains("TOKENS") && a < Singleton.Instance.Texto.Count())
                            {
                                Singleton.Instance.txt_Sets.Add(Singleton.Instance.Texto[a]);
                                a++;
                            }
                            a--;
                        }
                        else if (Singleton.Instance.Texto[a].Contains("TOKENS") && a < Singleton.Instance.Texto.Count())
                        {
                            while (!Singleton.Instance.Texto[a].Contains("ACTIONS") && a < Singleton.Instance.Texto.Count())
                            {
                                Singleton.Instance.txt_Tokens.Add(Singleton.Instance.Texto[a]);
                                a++;
                            }
                            a--;
                        }
                        else if (Singleton.Instance.Texto[a].Contains("ACTIONS") &&  a < Singleton.Instance.Texto.Count())
                        {
                            while (!Singleton.Instance.Texto[a].Contains("ERROR") && a < Singleton.Instance.Texto.Count())
                            {
                                Singleton.Instance.txt_Actions.Add(Singleton.Instance.Texto[a]);
                                a++;
                            }
                            a--;
                        }
                        else if (Singleton.Instance.Texto[a].Contains("ERROR") && a < Singleton.Instance.Texto.Count())
                        {
                            while (a < Singleton.Instance.Texto.Count())
                            {
                                Singleton.Instance.txt_Error.Add(Singleton.Instance.Texto[a]);
                                a++;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return View("Privacy",repuesta);
        }

        public ActionResult Analisis_Lexico()
        {
            List<int> ResTest = new List<int>();
            List<int> ResTokens = new List<int>();
            List<int> ResActions = new List<int>();
            List<int> ResError = new List<int>();
            Repuesta repuesta = new Repuesta();
            repuesta.RepuestaId += "";

            try
            {
                ResTest = Singleton.Instance.Analizar.Analizar_Sets(Singleton.Instance.txt_Sets, Singleton.Instance.Texto);
                if (ResTest[0] == -1)
                {
                    ResTokens = Singleton.Instance.Analizar.Analizar_Tokens(Singleton.Instance.txt_Tokens, Singleton.Instance.Texto);
                    if (ResTokens[0] == -1)
                    {
                        ResActions = Singleton.Instance.Analizar.Analizar_Actions(Singleton.Instance.txt_Actions, Singleton.Instance.Texto);
                        if (ResActions[0] == -1)
                        {
                            ResError = Singleton.Instance.Analizar.Analizar_Error(Singleton.Instance.txt_Error, Singleton.Instance.Texto);
                            if (ResError[0] == -1)
                            {
                                repuesta.RepuestaId += "No se han encontrado errores.";
                            }
                            else
                            {
                                repuesta.RepuestaId += "Se ha encontrado un error en la fila " +ResError[1];
                            }
                        }
                        else
                        {
                            repuesta.RepuestaId += "Se ha encontrado un error en la fila " + ResActions[1];
                        }
                    }
                    else
                    {
                        repuesta.RepuestaId += "Se ha encontrado un error en la fila " + ResTokens[1];
                    }
                }
                else
                {
                    repuesta.RepuestaId += "Se ha encontrado un error en la fila " + ResTest[1];
                }
            }
            catch (Exception)
            {

                throw;
            }
           
            return View("Privacy", repuesta);
        }
    }
}
