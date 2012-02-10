using System;

namespace Gi7.Client.Request.Base
{
    public interface IRequest
    {
        String Uri { get; }

        OverrideSettings OverrideSettings { get; }
    }
}