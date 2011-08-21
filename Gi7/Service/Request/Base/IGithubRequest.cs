using System;

namespace Gi7.Service.Request.Base
{
    public interface IGithubRequest<T>
        where T : new()
    {
        String Uri { get; }

        OverrideSettings OverrideSettings { get; }
    }
}
