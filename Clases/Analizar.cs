using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Clases
{
    public class Analizar
    {
        public List<int> Analizar_Sets(List<string> sets, List<string> Texto)
        {
            int a = 0;
            List<int> Verificado = new List<int>();
            if (sets.Count() == 0 )
            {
                if (Texto[0] == "sets")
                {
                    Verificado.Add(-2);
                    Verificado.Add(1);
                    return Verificado;
                }
                else
                {
                    Verificado.Add(-1);
                    return Verificado;
                }
                
            }
            else
            {
                if (sets.Count() > 1)
                {
                    string patron = @"^\s*(\w+)\s*=\s*(('[^']+'|CHR\((\d+)\))..('[^']+'|CHR\((\d+)\))(\s*\+)?)*\s*$";
                    bool paso=false;
                    for (a = 1; a < sets.Count(); a++)
                    {
                        if (Regex.IsMatch(sets[a],patron))
                        {
                           paso=true;
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
                        return Verificado;
                    }
                    else
                    {
                        Verificado.Add(-2);
                        Verificado.Add(a+1);
                        return Verificado;
                    }
                }
                else
                {
                    Verificado.Add(-2);
                    Verificado.Add(2);
                    return Verificado;
                }
               
            }
            
        }
        public List<int> Analizar_Tokens(List<string> tokens, List<string> Texto) 
        { 
            return new List<int>();
        }

        public List<int> Analizar_Actions(List<string> actions, List<string> Texto)
        {
            int a = 0;
            List<int> Verificado = new List<int>();
            if (actions.Count() == 0)
            {
                Verificado.Add(-2);
                return Verificado;
            }
            else
            {
                if (Texto[0] == "ACTIONS")
                {
                    Verificado.Add(-1);
                    if (Texto[1] == "RESERVADAS()")
                    {
                        Verificado.Add(-1);
                        if (Texto[3] == "{")
                        {
                            Verificado.Add(-1);
                            string patron_actions = @"\s*([0-9]+[0-9]+)\s+\=\s+'([A-Z]+)'";
                            bool paso_actions = false;
                            for (a = 1; a < actions.Count()-1; a++)
                            {
                                if (Regex.IsMatch(actions[a], patron_actions))
                                {
                                    paso_actions = true;
                                }
                                else
                                {
                                    paso_actions = false;
                                    break;
                                }
                            }
                            if (paso_actions)
                            {
                                Verificado.Add(-1);
                                if (Texto[actions.Count()] == "}")
                                {
                                    Verificado.Add(-1);
                                    return Verificado;
                                }
                                else
                                {
                                    Verificado.Add(-2);
                                    return Verificado;
                                }
                            }
                            else
                            {
                                Verificado.Add(-2);
                                Verificado.Add(a + 1);
                                return Verificado;
                            }
                        }
                        else
                        {
                            Verificado.Add(-2);
                            return Verificado;
                        }
                    }
                    else
                    {
                        Verificado.Add(-2);
                        return Verificado;
                    }
                }
                else
                {
                    Verificado.Add(-2);
                    return Verificado;
                }
            }
        }
        public List<int> Analizar_Error(List<string> error, List<string> Texto)
        {
            return new List<int>();
        }

    }
}
