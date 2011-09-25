using System;

namespace Gi7.Service.Request.Base
{
    public interface IRequest
    {
        String Uri { get; }

        OverrideSettings OverrideSettings { get; }
    }
}
