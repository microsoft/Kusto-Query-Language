using System;
using System.Collections;
using System.Collections.Generic;

namespace Kusto.Language.Utils
{
    public struct CancellationToken
    {
#if BRIDGE
        private readonly Func<bool> _fnCanceled;

        public CancellationToken(Func<bool> fnCanceled = null)
        {
            _fnCanceled = fnCanceled;
        }

        public void ThrowIfCancellationRequested()
        {
            if (_fnCanceled != null && _fnCanceled())
                throw new OperationCanceledException();
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