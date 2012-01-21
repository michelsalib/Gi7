﻿using System;

namespace Gi7.Service
{
    public class AuthenticatedEventArgs : EventArgs
    {
        public AuthenticatedEventArgs(bool isAuthenticated)
        {
            IsAuthenticated = isAuthenticated;
        }

        public bool IsAuthenticated { get; private set; }
    }
}