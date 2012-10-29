using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDE.IpCamClientServer.Server.Core.Profiling
{
    class ProfilerScene: IDisposable
    {
        private readonly long _usedMemory;
        private long _currentMemory;

        public ProfilerScene()
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            _usedMemory = GC.GetTotalMemory(false);
        }

        public void Dispose()
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            _currentMemory = GC.GetTotalMemory(false);
            if (_currentMemory != _usedMemory)
            {
                throw new InvalidOperationException(string.Format("Memory usage changed: now {0}, was {1}", _currentMemory, _usedMemory));
            }
        }
    }
}
