using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gi7.Service.Request.Base
{
    public class NewResultsEventArgs<T> : EventArgs
    {
        public IEnumerable<T> NewResults { get; set; }
    }
}
