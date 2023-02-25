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

    }
}
