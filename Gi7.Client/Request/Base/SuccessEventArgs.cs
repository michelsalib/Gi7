using System;

namespace Gi7.Client.Request.Base
{
    public class SuccessEventArgs<T> : EventArgs
    {
        public T NewResult { get; set; }
    }
}