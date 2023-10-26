using System;

namespace WzUXRibbonTest.Data
{
    public interface IRibbonStateData : IDisposable
    {
        bool IsLoading { get; }
        bool IsLoaded { get; }
        void Save();
        void Load();
        void Reset();
    }
}
