using System;
using System.Collections.Generic;
using System.Text;

namespace RMES.Framework
{
    public class Result<T> : Result
    {
        public T Body { get; set; }
    }
}
