using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace Clases
{
    public class Scanner
    {
        public void crearArchivo(List<string> sets, List<string> actions, List<string> actionsTokens, List<string[]> TablaTrancisiones,List<string> Trancisiones, List<string> Simbolos ) 
        {
            string nombreProgram = "AUTOMATA";
            List<string> lineasDeCodigo = new List<string>();
            lineasDeCodigo.Add("import java.util.Scanner;");
            lineasDeCodigo.Add("public class " + nombreProgram + " {");
            lineasDeCodigo.Add("public static void main(String[] args) {");
            lineasDeCodigo.Add("System.out.println(\"Ingrese el programa\");");
            lineasDeCodigo.Add("Scanner in = new Scanner(System.in);");
            lineasDeCodigo.Add("String program = in.nextLine() + \" \";");
            lineasDeCodigo.Add("int index = 0;");
            lineasDeCodigo.Add("int actual_state = 0;");
            // esto le toca a fer 


            
            
            // esto le toca a fer 
            lineasDeCodigo.Add("}"); // Fin Main 



            //Trabajando los set
            

            lineasDeCodigo.Add("static String identify_SET(char lexeme) {"); //Inicia funcion de SETS
            lineasDeCodigo.Add("int lexeme_value = (int)lexeme;");
            for (int i = 0; i < sets.Count; i++)
            {
                string nombreSet = sets[i].Split('=')[0].Trim(); ;
                string valorSet = sets[i].Split('=')[1].Trim();

                //Verifico los conjuntos de sets
                string[] conjuntosSet = valorSet.Split('+');

                for (int j = 0; j < conjuntosSet.Length; j++)
                {

                    if (conjuntosSet[j].Contains(".."))
                    { //Es un conjunto
                        conjuntosSet[j] = conjuntosSet[j].Replace("..", "$");
                        string[] limites = conjuntosSet[j].Split('$');

                        lineasDeCodigo.Add("int " + nombreSet + j + "_INFERIOR = (int)" + limites[0] + ";");
                        lineasDeCodigo.Add("int " + nombreSet + j + "_SUPERIOR = (int)" + limites[1] + ";");

                        lineasDeCodigo.Add("if (lexeme_value >= " + nombreSet + j + "_INFERIOR  && lexeme_value <= " + nombreSet + j + "_SUPERIOR)");
                        lineasDeCodigo.Add("return \"" + nombreSet + "\";");

                    }
                    else //Es valor unico 
                    {
                        lineasDeCodigo.Add("int " + nombreSet + j + "_ONLY = (int)" + conjuntosSet[j] + ";");

                        lineasDeCodigo.Add("if (lexeme_value == " + nombreSet + j + "_ONLY)");
                        lineasDeCodigo.Add("return \"" + nombreSet + "\";");

                    }

                }


            }

            lineasDeCodigo.Add("return \"\";");

            lineasDeCodigo.Add("}"); //Finaliza funcion sets
            lineasDeCodigo.Add("static String RESERVADAS(String command) {"); //Inicia funcion de actions
            for (int i = 0; i < actions.Count; i++)
            {
                lineasDeCodigo.Add("if (command.equalsIgnoreCase(\""+actions[i]+"\"))");
                lineasDeCodigo.Add("return \"TOKEN "+actionsTokens[i]+"\";");
            }
            lineasDeCodigo.Add("return \"TOKEN " + Convert.ToString(Convert.ToInt32(actionsTokens[0]) - 1) + "\";");
            lineasDeCodigo.Add("}");//Finaliza funcion actions 
            lineasDeCodigo.Add("}"); //Finaliza el programa

            // Create an array of strings to write to the file
            string[] lines = lineasDeCodigo.ToArray();

            // Write the array of strings to the file

           File.WriteAllLines(nombreProgram + ".java", lines);

        }
    }
}
