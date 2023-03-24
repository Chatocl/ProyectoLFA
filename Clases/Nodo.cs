using System;
using System.Collections.Generic;
using System.Text;

namespace Clases
{
    public class Node
    {
        public Node Left{ get; set; }
        public Node Right { get; set; }
        public string Valor { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
        public bool Anulable { get; set; }

        public Node(){ }
        public Node(string Val) 
        { 
            this.Valor = Val;
            Left = null;
            Right = null;

        }
    }
}