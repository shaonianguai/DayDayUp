using System;

namespace WzUXRibbon.Internal
{
#pragma warning disable CA1063 // Implement IDisposable Correctly
    public class ScopeGuard : IDisposable
#pragma warning restore CA1063 // Implement IDisposable Correctly
    {
        private readonly Action onEntry;
        private readonly Action onDispose;

        public ScopeGuard()
        {
        }

        public ScopeGuard(Action onEntry, Action onDispose)
        {
            this.onEntry = onEntry ?? throw new ArgumentNullException(nameof(onEntry));
            this.onDispose = onDispose ?? throw new ArgumentNullException(nameof(onDispose));
        }

        public bool IsActive { get; private set; }

        public ScopeGuard Start()
        {
            if (this.IsActive)
            {
                return this;
            }

            this.IsActive = true;
            this.onEntry?.Invoke();

            return this;
        }

        /// <inheritdoc />
#pragma warning disable CA1063 // Implement IDisposable Correctly
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
        public void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
#pragma warning restore CA1063 // Implement IDisposable Correctly
        {
            var wasActive = this.IsActive;
            this.IsActive = false;

            if (wasActive)
            {
                this.onDispose?.Invoke();
            }
        }
    }
}
