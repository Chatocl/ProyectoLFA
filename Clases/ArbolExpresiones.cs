using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace Clases
{  
    /// <summary>
    ///  Arbol de Expreciones    
    /// </summary>
    public class ArbolExpresiones : Node
    {
        int cantTerminal = 0;
        private Stack<string> TokenStack = new Stack<string>();
        private Stack<Node> TreeStack = new Stack<Node>();
        private Dictionary<string, int> precedencia = new Dictionary<string, int>();
        public static ArbolExpresiones Instance { get; } = new ArbolExpresiones();

        /// <summary>
        /// Metodo de creacion para agregar los simbolos en un dictionario de precedencia
        /// </summary>
        public ArbolExpresiones()
        {
            precedencia.Add("|", 1);
            precedencia.Add(".", 2);
            precedencia.Add("*", 3);
            precedencia.Add("+", 3);
            precedencia.Add("?", 3);
        }
        /// <summary>
        /// Metodo para saber si un string es un simbolo o un terminal
        /// </summary>
        /// <param name="c">String a evaluar</param>
        /// <returns>Devuelve true si es un terminal y false si es un no terminal</returns>
        public bool EsSimbolo(string c)
        {
            return c != "(" && c != ")" && !EsOperador(c);
        }
        private bool EsOperador(string c)
        {
            return precedencia.ContainsKey(c);
        }
        private bool Unitario(string c)
        {
            return c == "*" || c == "+" || c == "?";
        }
        public Node ContruirArbol(List<string> Expreciones)
        {

            foreach (string tokens in Expreciones)
            {
                if (EsSimbolo(tokens))
                {
                    TreeStack.Push(new Node(tokens));
                }
                else if (tokens == "(")
                {
                    TokenStack.Push(tokens);
                }
                else if (tokens == ")")
                {
                    while (TokenStack.Count > 0 && TokenStack.Peek() != "(")
                    {
                        if (TokenStack.Count == 0)
                        {
                            throw new Exception("Error");
                        }
                        if (TreeStack.Count < 2)
                        {
                            throw new Exception("Error");
                        }
                        Node temp = new Node(TokenStack.Pop());
                        Node Der = TreeStack.Pop();
                        Node Izq = TreeStack.Pop();

                        temp.Left = Izq;
                        temp.Right = Der;
                        TreeStack.Push(temp);
                    }

                    TokenStack.Pop();
                }
                else if (EsOperador(tokens))
                {
                    if (Unitario(tokens))
                    {
                        Node node = new Node(tokens);
                        if (TreeStack.Count < 0)
                        {
                            throw new Exception("Error");
                        }
                        node.Left = TreeStack.Pop();
                        TreeStack.Push(node);
                    }
                    else if (TokenStack.Count > 0 && TokenStack.Peek() != "(" && precedencia[tokens] <= precedencia[TokenStack.Peek()])
                    {
                        Node temp = new Node(TokenStack.Pop());
                        if (TreeStack.Count < 2)
                        {
                            throw new Exception("Error");
                        }
                        Node Der = TreeStack.Pop();
                        Node Izq = TreeStack.Pop();

                        temp.Left = Izq;
                        temp.Right = Der;
                        TreeStack.Push(temp);
                    }
                    if (!Unitario(tokens))
                    {
                        TokenStack.Push(tokens);
                    }

                }
                else
                {
                    throw new Exception("Error");
                }
            }

            while (TokenStack.Count > 0)
            {
                Node temp = new Node(TokenStack.Pop());
                if (temp.Valor == "(")
                {
                    throw new Exception("Error");
                }
                if (TreeStack.Count < 2)
                {

                    throw new Exception("Error");
                }
                Node Der = TreeStack.Pop();
                Node Izq = TreeStack.Pop();

                temp.Left = Izq;
                temp.Right = Der;
                TreeStack.Push(temp);
            }

            if (TreeStack.Count != 1)
            {
                throw new Exception("Error");
            }

            return TreeStack.Pop();
        }
        public void PostOrder(Node node)
        {
            if (node == null)
            {
                return;
            }
            if (node.Left == null && node.Right == null)
            {
                cantTerminal++;
                node.First = cantTerminal.ToString();
                node.Last = cantTerminal.ToString();
                node.Anulable = false;
                return;
            }

            PostOrder(node.Left);
            PostOrder(node.Right);

            if (node.Valor == "|")
            {
                node.First = node.Left.First + "," + node.Right.First;
                node.Last = node.Left.Last + "," + node.Right.Last;
                node.Anulable = node.Left.Anulable || node.Right.Anulable;
            }
            else if (node.Valor == ".")
            {
                node.First = node.Left.First;
                if (node.Left.Anulable)
                {
                    node.First += "," + node.Right.First;
                }
                node.Last = node.Right.Last;
                if (node.Right.Anulable)
                {
                    node.Last += "," + node.Left.Last;
                }
                node.Anulable = node.Left.Anulable && node.Right.Anulable;
            }
            else if (node.Valor == "*")
            {
                node.First = node.Left.First;
                node.Last = node.Left.Last;
                node.Anulable = true;
            }
            else if (node.Valor == "+")
            {
                node.First = node.Left.First;
                node.Last = node.Left.Last;
                node.Anulable = node.Left.Anulable;
            }
            else if (node.Valor == "?")
            {
                node.First = node.Left.First + "," + node.Right.First;
                node.Last = node.Left.Last + "," + node.Right.Last;
                node.Anulable = true;
            }
        }
        public void Terminales(Node node, Dictionary<int, List<string>> diccionario)
        {
            if (node == null)
            {
                return;
            }
            if (node.Left == null && node.Right == null)
            {
                List<string> valores = new List<string>();
                diccionario.Add(Convert.ToInt32(node.First), valores);
                return;
            }
            Terminales(node.Left, diccionario);
            Terminales(node.Right, diccionario);

        }
        public void CalcularFollow(Node node, Dictionary<int, List<string>> TablaFollow)
        {
            if (node == null)
            {
                return;
            }
            CalcularFollow(node.Left, TablaFollow);
            CalcularFollow(node.Right, TablaFollow);

            if (node.Left != null || node.Right != null)
            {
                if (node.Valor == "." || node.Valor == "+")
                {
                    foreach (string L in node.Left.Last.Split(","))
                    {
                        int l = Convert.ToInt32(L);
                        foreach (string F in node.Right.First.Split(","))
                        {
                            if (!TablaFollow[l].Contains(F))
                            {
                                TablaFollow[l].Add(F);
                            }
                        }
                    }
                }
                if (node.Valor == "*")
                {
                    foreach (string L in node.Left.Last.Split(","))
                    {
                        int l = Convert.ToInt32(L);
                        foreach (string F in node.Left.First.Split(","))
                        {
                            if (!TablaFollow[l].Contains(F))
                            {
                                TablaFollow[l].Add(F);
                            }
                        }
                    }
                }
            }


        }

    }

}
