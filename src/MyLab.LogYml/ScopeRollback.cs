using System;
using System.Collections.Generic;

namespace MyLab.LogYml
{
    class ScopeRollback : IDisposable
    {
        private readonly List<object> _scopes;
        private readonly object _item;

        public ScopeRollback(List<object> scopes, object item)
        {
            _scopes = scopes;
            _item = item;
        }

        public void Dispose()
        {
            _scopes.Remove(_item);
        }
    }
}