using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Clases
{
    public class Analizar
    {
        /// <summary>
        /// Analizardor de un texto con las condiciones del manual
        /// </summary>
        /// <param name="Texto"></param>
        /// <returns> una lista de enteros donde -1 es si todo esta correcto y -2 si algo no esta correcto mas la fila donde se encuentra el fallo</returns>
        public (List<int>,List<String>) Analizar_Texto(List<string> Texto) 
        {
            int a = 0;
            bool paso = false;
            bool token = true;
            List<int> Verificado = new List<int>();
            List<string> Tokens = new List<string>();
            string patron_SETS = @"^\s*(\w+)\s*=\s*(('\w+'|CHR\((\d+)\))((\s*\.\.\s*)|(\s*\+?\s*))?)*\s*$";
            string patronTokens1 = @"^\s*TOKEN\s*\d+\s*=\s*(((('.')|(\w*\s*(\*|\+|\?|\|)?))\s*))*$";
            string patronTokens2 = @"^\s*TOKEN\s*\d+\s*=\s*((\w*\s*(\((\w*\s*(\*|\+|\?|\|)?\s*)*\)\s*(\*|\+|\?|\|)?)\s*)*)\s*$";
            string patronTokens3 = @"^\s*TOKEN\s*\d+\s*=\s*(((\s*\{\s*((\w*\s*(\((\w*\s*(\*|\+|\?|\|)?\s*)*\)\s*(\*|\+|\?|\|)?)\s*)*)\s*\}\s*)*)|((\w*\s*(\((\w*\s*(\*|\+|\?|\|)?\s*)*\)\s*(\*|\+|\?|\|)?)\s*)*))*\s*$";
            string patron_actions = @"^\s*(\d+)\s*\=\s*'([A-Z]+)'\s*$";
            string patron_error = @"^\s*ERROR\s*=\s*([0-9]+)$";

            for ( a = 0; a < Texto.Count(); a++)
            {
                if (Regex.IsMatch(Texto[a], @"^\s*SETS\s*$"))
                {
                    a++;
                    while (Regex.IsMatch(Texto[a], patron_SETS))
                    {
                        a++;
                    }
                    if (Regex.IsMatch(Texto[a], @"^\s*TOKENS\s*$")) 
                    {
                        a++;
                        while (token)
                        {
                            
                            if (Regex.IsMatch(Texto[a], patronTokens1))
                            {
                                token = true;
                                Tokens.Add(Texto[a]);
                            }
                            else if (Regex.IsMatch(Texto[a], patronTokens2))
                            {
                                token = true;
                                Tokens.Add(Texto[a]);
                            }
                            else if (Regex.IsMatch(Texto[a], patronTokens3))
                            {
                                token = true;
                                Tokens.Add(Texto[a]);
                            }
                            else
                            {
                                token = false;
                                break;
                            }
                            a++;
                        }
                        if (Regex.IsMatch(Texto[a], @"^\s*ACTIONS\s*$"))
                        {
                            a++;
                            if (Regex.IsMatch(Texto[a], @"^\s*RESERVADAS\(\)\s*$"))
                            {
                                a++;
                                if (Regex.IsMatch(Texto[a], @"^\s*{\s*$"))
                                {
                                    a++;
                                    while (Regex.IsMatch(Texto[a], patron_actions))
                                    {
                                        a++;
                                    }
                                    if (Regex.IsMatch(Texto[a], @"^\s*}\s*$"))
                                    {
                                        a++;
                                        if (Regex.IsMatch(Texto[a], patron_error))
                                        {
                                            while (a<Texto.Count())
                                            {
                                                if (Regex.IsMatch(Texto[a], patron_error))
                                                {
                                                    paso = true;
                                                    a++;
                                                }
                                                else
                                                {
                                                    paso = false;
                                                    break;
                                                }
                                            }
                                            if (paso)
                                            {
                                                Verificado.Add(-1); 
                                                break;
                                            }
                                            else
                                            {
                                                Verificado.Add(-2);
                                                Verificado.Add(a);
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            Verificado.Add(-2);
                                            Verificado.Add(a);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        Verificado.Add(-2);
                                        Verificado.Add(a);
                                        break;
                                    }
                                }
                                else 
                                {
                                    Verificado.Add(-2);
                                    Verificado.Add(a);
                                    break;
                                }
                            }
                            else
                            {
                                Verificado.Add(-2);
                                Verificado.Add(a);
                                break;
                            }
                        }
                        else 
                        {
                            Verificado.Add(-2);
                            Verificado.Add(a);
                            break;
                        }
                    }
                    else
                    {
                        Verificado.Add(-2);
                        Verificado.Add(a);
                        break;
                    }
                }
                else
                {
                    if (Regex.IsMatch(Texto[a], @"^\s*TOKENS\s*$"))
                    {
                        a++;
                        while (token)
                        {
                            
                            if (Regex.IsMatch(Texto[a], patronTokens1))
                            {
                                token = true;
                            }
                            else if (Regex.IsMatch(Texto[a], patronTokens2))
                            {
                                token = true;
                            }
                            else if (Regex.IsMatch(Texto[a], patronTokens3))
                            {
                                token = true;
                            }
                            else
                            {
                                token = false;
                                break;
                            }
                            a++;
                        }
                        if (Regex.IsMatch(Texto[a], @"^\s*ACTIONS\s*$"))
                        {
                            a++;
                            if (Regex.IsMatch(Texto[a], @"^\s*RESERVADAS\(\)\s*$"))
                            {
                                a++;
                                if (Regex.IsMatch(Texto[a], @"^\s*{\s*$"))
                                {
                                    a++;
                                    while (Regex.IsMatch(Texto[a], patron_actions))
                                    {
                                        a++;
                                    }
                                    if (Regex.IsMatch(Texto[a], @"^\s*}\s*$"))
                                    {
                                        a++;
                                        if (Regex.IsMatch(Texto[a], patron_error))
                                        {
                                            while (a < Texto.Count())
                                            {
                                                if (Regex.IsMatch(Texto[a], patron_error))
                                                {
                                                    paso = true;
                                                    a++;
                                                }
                                                else
                                                {
                                                    paso = false;
                                                    break;
                                                }
                                            }
                                            if (paso)
                                            {
                                                Verificado.Add(-1);
                                                break;
                                            }
                                            else
                                            {
                                                Verificado.Add(-2);
                                                Verificado.Add(a);
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            Verificado.Add(-2);
                                            Verificado.Add(a);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        Verificado.Add(-2);
                                        Verificado.Add(a);
                                        break;
                                    }
                                }
                                else
                                {
                                    Verificado.Add(-2);
                                    Verificado.Add(a);
                                    break;
                                }
                            }
                            else
                            {
                                Verificado.Add(-2);
                                Verificado.Add(a);
                                break;
                            }
                        }
                        else
                        {
                            Verificado.Add(-2);
                            Verificado.Add(a);
                            break;
                        }
                    }
                    else
                    {
                        Verificado.Add(-2);
                        Verificado.Add(a);
                        break;
                    }
                }
            }
            return (Verificado,Tokens);
        
        }

    }
}
