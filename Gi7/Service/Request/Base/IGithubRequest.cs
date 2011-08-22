using System;
using System.ComponentModel;

namespace Gi7.Service.Request.Base
{
    public interface IGithubRequest<T> : INotifyPropertyChanged
        where T : new()
    {
        String Uri { get; }

        OverrideSettings OverrideSettings { get; }
    }
}
