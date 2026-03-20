using Microsoft.Maui.Dispatching;
using utilities.Interfaces;
using utilities.Platforms.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;

using CommunityToolkit.Maui;
namespace utilities.Views
{
    public partial class ColorPickerPage
    {
        private readonly IScreenColorPicker? _picker;
        private IDispatcherTimer? _timer;
        private bool _active;
        public ColorPickerPage() 
            : this(GetPickerFromServices())
        {
        }

        private static IScreenColorPicker? GetPickerFromServices()
        {
            var services = IPlatformApplication.Current?.Services;
            return services?.GetService<IScreenColorPicker>();
        }
        public ColorPickerPage(IScreenColorPicker? picker = null)
        {
            InitializeComponent();
            _picker = picker;

            ColorPreview.BackgroundColor = Colors.Transparent;
            ColorHex.Text = "#--------";
        }
        private void OnToggleEyedropper(object sender, EventArgs e)
        {
            if (_picker is null || !_picker.IsSupported)
            {
                DisplayAlert("Not supported", "It's available only in Windows.", "OK");
                return;
            }

            _active = !_active;

            if (_active)
            {
                StartSampling();
                ((Button)sender).Text = "Deactivate";
            }
            else
            {
                StopSampling();
                ((Button)sender).Text = "Activate";
            }
        }

        private void StartSampling()
        {
            _timer ??= Dispatcher.CreateTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(33);
            _timer.Tick -= OnTick;
            _timer.Tick += OnTick;
            _timer.Start();
#if WINDOWS
            if (_picker is ScreenColorPicker winPicker)
            {
                winPicker.OnGlobalClick += OnGlobalClick;
                winPicker.StartGlobalClickListener();
            }
#endif
        }

        private void OnGlobalClick()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                StopSampling();
                _active = false;

        #if WINDOWS
                if (_picker is ScreenColorPicker winPicker)
                {
                    winPicker.StopGlobalClickListener();
                    winPicker.OnGlobalClick -= OnGlobalClick;
                }
        #endif
                _active = false;
                StopSampling();
                ActivateButton.Text = "Activate";
                DisplayAlert("Color selected", ColorHex.Text, "OK");
            });
        }

        private void StopSampling()
        {
            _timer?.Stop();
        }

        private void OnTick(object? sender, EventArgs e)
        {
            if (_picker is null) return;

            var c = _picker.GetColorUnderCursor();
            ColorPickerControl.PickedColor = c;
            ColorPreview.BackgroundColor = c;

            // Hex
            var r = (int)Math.Round(c.Red * 255);
            var g = (int)Math.Round(c.Green * 255);
            var b = (int)Math.Round(c.Blue * 255);
            ColorHex.Text = $"#{r:X2}{g:X2}{b:X2}";
        }
    }
}