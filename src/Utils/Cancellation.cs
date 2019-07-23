using System;
using System.Collections;
using System.Collections.Generic;

namespace Kusto.Language.Utils
{
    public struct CancellationToken
    {
#if BRIDGE
        public void ThrowIfCancellationRequested()
        {
        }
#else
        System.Threading.CancellationToken token;

        private CancellationToken(System.Threading.CancellationToken token)
        {
            this.token = token;
        }

        public static implicit operator CancellationToken(System.Threading.CancellationToken token)
        {
            return new CancellationToken(token);
        }

        public void ThrowIfCancellationRequested()
        {
            this.token.ThrowIfCancellationRequested();
        }
#endif
    }
}