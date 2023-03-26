using System;
using System.Collections.Generic;
using System.Text;

namespace Clases
{
    public class ArbolExpresiones : Node
    {

        private Stack<string> TokenStack = new Stack<string>();
        private Stack<Node> TreeStack = new Stack<Node>();
        private Dictionary<string, int> precedencia = new Dictionary<string, int>();

        public ArbolExpresiones()
        {
            precedencia.Add("|", 1);
            precedencia.Add(".", 2);
            precedencia.Add("*", 3);
            precedencia.Add("+", 3);
            precedencia.Add("?", 3);
        }
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

    }

}
