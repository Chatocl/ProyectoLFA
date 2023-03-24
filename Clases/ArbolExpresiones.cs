using System;
using System.Collections.Generic;
using System.Text;

namespace Clases
{
    public class ArbolExpresiones:Node
    {

        private Stack<Node> TokenStack = new Stack<Node>();
        private Stack<Node> TreeStack = new Stack<Node>();
        private Dictionary<string, int> precedencia = new Dictionary<string, int>();

        public ArbolExpresiones()
        {
            precedencia.Add("#", 0);
            precedencia.Add("|", 1);
            precedencia.Add(".", 2);
            precedencia.Add("*", 3);
            precedencia.Add("+", 3);
            precedencia.Add("?", 3);
        }
        private bool EsSimbolo(string c)
        { 
            return c!="("&& c != ")" && EsOperador(c);
        }
        private bool EsOperador(string c)
        {
            return precedencia.ContainsKey(c);
        }
        private bool concadenacion(string c)
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
                    TokenStack.Push(new Node(tokens));
                }
                else if (tokens == ")")
                {
                    while (TokenStack.Count>0 && TokenStack.Peek().Valor!="(")
                    {
                        if (TreeStack.Count<2)
                        {
                            break; 
                        }
                        Node temp = TokenStack.Pop();
                        Node Der = TreeStack.Pop();
                        Node Izq = TreeStack.Pop();
                       
                        temp.Left = Izq;
                        temp.Right = Der;
                        TreeStack.Push(temp);
                    }
                    if (TokenStack.Count == 0)
                    {
                        throw new Exception("Error");
                    }
                    TokenStack.Pop();
                }
                else if (EsOperador(tokens))
                {
                    Node node = new Node(tokens);
                    if (concadenacion(tokens))
                    {
                        if (TreeStack.Count < 1)
                        {
                            throw new Exception("Error");
                        }
                        node.Left = TreeStack.Pop();
                    }
                    else
                    {
                        while (TokenStack.Count>0 && TokenStack.Peek().Valor!="("&& precedencia[tokens]<= precedencia[TokenStack.Peek().Valor])
                        {
                            Node operador = TokenStack.Pop();
                            if (TreeStack.Count < 2)
                            {
                                throw new Exception("Error");
                            }
                            Node Der = TreeStack.Pop();
                            Node Izq = TreeStack.Pop();

                            operador.Left = Izq;
                            operador.Right = Der;
                            TreeStack.Push(operador);
                        }
                    }
                    TokenStack.Push(node);
                }
                else
                {
                    throw new Exception("Error");
                }
            }

            while (TokenStack.Count>0)
            {
                Node operador = TokenStack.Pop();
                if (operador.Valor == "(")
                {
                    throw new Exception("Error");
                }
                if (TreeStack.Count < 2)
                {

                    throw new Exception("Error");
                }
                Node Der = TreeStack.Pop();
                Node Izq = TreeStack.Pop();

                operador.Left = Izq;
                operador.Right = Der;
                TreeStack.Push(operador);
            }

            if (TreeStack.Count != 1) 
            {
                throw new Exception("Error");
            }
         
            return TreeStack.Pop();
        }
        
    }

   
}
