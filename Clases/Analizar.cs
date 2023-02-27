﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Clases
{
    public class Analizar
    {
        /// <summary>
        /// Analizar de forma lexica el formato de los SETS
        /// </summary>
        /// <returns>Lista de ints indicado -1 si esta correcto o -2 mas la fila donde se encuentra el error </returns>
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

        /// <summary>
        /// Analizar de forma lexica el formato de los TOKENS
        /// </summary>
        /// <returns>Lista de ints indicado -1 si esta correcto o -2 mas la fila donde se encuentra el error </returns>
        public List<int> Analizar_Tokens(List<string> tokens, List<string> Texto) 
        {
            string patronTokens1 = @"^\s*TOKEN\s*\d+\s*=\s*(((('.')|(\w*\s*(\*|\+|\?|\|)?))\s*))*$";
            string patronTokens2 = @"^\s*TOKEN\s*\d+\s*=\s*((\w*\s*(\((\w*\s*(\*|\+|\?|\|)?\s*)*\)\s*(\*|\+|\?|\|)?)\s*))\s*$";
            string patronTokens3 = @"^\s*TOKEN\s*\d+\s*=\s*(((\s*\{\s*((\w*\s*(\((\w*\s*(\*|\+|\?|\|)?\s*)*\)\s*(\*|\+|\?|\|)?)\s*)*)\s*\}\s*)*)|((\w*\s*(\((\w*\s*(\*|\+|\?|\|)?\s*)*\)\s*(\*|\+|\?|\|)?)\s*)*))*\s*$";
            int a = 1;
            List<int> Verificado = new List<int>();

            if (tokens[0]!="TOKENS")
            {
                while (Texto[a]!=tokens[0])
                {
                    a++;
                }
                Verificado.Add(-2);
                Verificado.Add(a);
                return Verificado;
            }
            else
            {
                if (tokens.Count()>1)
                {
                    bool paso = false;
                    for ( a = 1; a < tokens.Count(); a++)
                    {
                        if (Regex.IsMatch(tokens[a], patronTokens1))
                        {
                            paso = true;
                        }
                        else if (Regex.IsMatch(tokens[a], patronTokens2))
                        {
                            paso = true;
                        }
                        else if (Regex.IsMatch(tokens[a], patronTokens3))
                        {
                            paso = true;
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
                        Verificado.Add(a);
                        return Verificado;
                    }
                }
                else
                {
                    Verificado.Add(-2);
                    Verificado.Add(a);
                    return Verificado;
                }
            }
            return Verificado;
        }
       
        /// <summary>
        /// Analizar de forma lexica el formato de los ACTIONS
        /// </summary>
        /// <returns>Lista de ints indicado -1 si esta correcto o -2 mas la fila donde se encuentra el error </returns>
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
                if (actions[0] == "ACTIONS")
                {
                    Verificado.Add(-1);
                    if (Texto[1] == "RESERVADAS()")
                    {
                        Verificado.Add(-1);
                        if (Texto[3] == "{")
                        {
                            Verificado.Add(-1);
                            string patron_actions = @"\s*([0-9]+[0-9]+)\s+\=\s+'([A-Z]+)'";
                            for (a = 4; a < actions.Count()-2; a++)
                            {
                                if (Regex.IsMatch(actions[a], patron_actions))
                                {
                                    Verificado.Add(-1);
                                }
                                else
                                {
                                    Verificado.Add(-2);
                                    return Verificado;
                                }
                            }

                            if (Texto[actions.Count()-1] == "}")
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

        /// <summary>
        /// Analizar de forma lexica el formato de los ERROR
        /// </summary>
        /// <returns>Lista de ints indicado -1 si esta correcto o -2 mas la fila donde se encuentra el error </returns>
        public List<int> Analizar_Error(List<string> error, List<string> Texto)
        {
            int a = 0;
            List<int> Verificado = new List<int>();
            string patron_error = @"ERROR\s*\=\s+([0-9]+)$";
            if (error.Count() == 0)
            {
                Verificado.Add(-2);
                return Verificado;
            }
            else
            {
                for (a=0;a<error.Count()-1;a++)
                {
                    if (Regex.IsMatch(Texto[a], patron_error))
                    {
                        Verificado.Add(-1);
                    }
                    else
                    {
                        Verificado.Add(-2);
                        return Verificado;
                    }
                 }
                return Verificado;
            }
        }

    }
}