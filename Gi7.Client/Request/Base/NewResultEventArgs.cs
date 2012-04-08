using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gi7.Client.Request.Base
{
    public class NewResultEventArgs<T> : EventArgs
    {
        public T NewResult { get; set; }
    }
}
