using WzUXRibbon.Controls;

namespace WzUXRibbonTest.Data
{
    public class RibbonStateData : IRibbonStateData
    {
        private Ribbon _ribbon = null;

        public bool IsMinimized
        {
            get;
            private set;
        }

        public bool IsLoading
        {
            get;
            private set;
        }

        public bool IsLoaded
        {
            get;
            private set;
        }

        public RibbonStateData(Ribbon ribbon)
        {
            _ribbon = ribbon;
        }

        public void Dispose()
        {
        }

        public void Load()
        {
            IsLoaded = false;
            IsLoading = true;

            _ribbon.IsMinimized = false;
            _ribbon.SelectedTabIndex = 0;

            IsLoaded = true;
            IsLoading = false;
        }

        public void Reset()
        {
        }

        public void Save()
        {
            bool isMinimized = _ribbon.IsMinimized;
            int index = _ribbon.SelectedTabIndex;
        }
    }
}
