using System.Windows;
using WzUXRibbon.Internal.KnownBoxes;

namespace WzUXRibbon.Controls
{

    public class StartScreen : Backstage
    {
        public bool Shown
        {
            get => (bool)this.GetValue(ShownProperty);
            set => this.SetValue(ShownProperty, value ? BooleanBoxes.TrueBox : BooleanBoxes.FalseBox);
        }

        /// <summary>Identifies the <see cref="Shown"/> dependency property.</summary>
        public static readonly DependencyProperty ShownProperty =
            DependencyProperty.Register(nameof(Shown), typeof(bool), typeof(StartScreen), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null));

        static StartScreen()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StartScreen), new FrameworkPropertyMetadata(typeof(StartScreen)));

            VisibilityProperty.OverrideMetadata(typeof(StartScreen), new PropertyMetadata(OnVisibilityChanged));
        }

        private static void OnVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((StartScreen)d).UpdateIsTitleBarCollapsed();
        }

        // This is required for scenarios like in #445 where the start screen is shown, but never hidden through Hide() but made hidden by changing just it's visibility.
        private void UpdateIsTitleBarCollapsed()
        {
            if (this.IsOpen == false)
            {
                return;
            }

            var parentRibbon = GetParentRibbon(this);

            parentRibbon?.TitleBar?.SetCurrentValue(RibbonTitleBar.IsCollapsedProperty, this.Visibility == Visibility.Visible
                ? BooleanBoxes.TrueBox
                : BooleanBoxes.FalseBox);
        }

        protected override bool Show()
        {
            if (this.Shown)
            {
                return false;
            }

            this.Shown = base.Show();

            if (this.Shown)
            {
                var parentRibbon = GetParentRibbon(this);

                parentRibbon?.TitleBar?.SetCurrentValue(RibbonTitleBar.IsCollapsedProperty, BooleanBoxes.TrueBox);
            }

            return this.Shown;
        }

        protected override void Hide()
        {
            var wasShown = this.Shown;

            base.Hide();

            if (wasShown)
            {
                var parentRibbon = GetParentRibbon(this);

                parentRibbon?.TitleBar?.ClearValue(RibbonTitleBar.IsCollapsedProperty);
            }
        }
    }
}
