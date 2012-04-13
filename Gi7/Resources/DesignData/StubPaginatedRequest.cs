using System.Collections.Generic;

namespace Gi7.Resources.DesignData
{
    public class StubPaginatedRequest<T>
    {
        public IEnumerable<T> Result { get; set;}
    }
}
