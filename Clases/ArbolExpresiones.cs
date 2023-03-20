using System;
using System.Collections.Generic;
using System.Text;

namespace Clases
{
    public class ArbolExpresiones<T>:Nodo<T> where T : IComparable<T>
    {
        private Nodo<T> temp = new Nodo<T>();
    }
}
