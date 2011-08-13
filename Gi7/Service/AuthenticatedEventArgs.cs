using System;

namespace Gi7.Service
{
    public class AuthenticatedEventArgs : EventArgs
    {
        public bool IsAuthenticated { get; private set; }

        public AuthenticatedEventArgs(bool isAuthenticated)
        {
            IsAuthenticated = isAuthenticated;
        }
    }
}
