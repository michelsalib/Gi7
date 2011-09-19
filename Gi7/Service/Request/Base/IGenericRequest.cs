using System;
using System.ComponentModel;

namespace Gi7.Service.Request.Base
{
    public interface IGenericRequest<T> : IRequest, INotifyPropertyChanged
        where T : new()
    {

    }
}
