using Ledger.ColorPicker;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Ledger.Animations
{
    [ToolboxBitmap(typeof(ColorDialog))]
    public class ColorPickerComponent : Component
    {
        private ColorPickerForm pickerForm;
        private Color selectedColor;
        private bool enableThemeChangeButton = true;
        private ThemeOptions currentTheme = ThemeOptions.Light;
        private IContainer components;

        public Color Color
        {
            get => selectedColor;
            set => selectedColor = value;
        }

        [Description("Allows the user to toggle the theme between Light and Dark with a button.")]
        public bool EnableThemeChangeButton
        {
            get => enableThemeChangeButton;
            set
            {
                enableThemeChangeButton = value;
                pickerForm?.ToggleThemeSwitchButton(value);
            }
        }

        public ColorPickerComponent() => InitializeComponent();

        public bool IsShowingDialog => pickerForm != null && pickerForm.Visible;

        public ColorPickerComponent(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        public Task<DialogResult> ShowDialog()
        {
            ColorPickerStateMachine stateMachine = new ColorPickerStateMachine();
            stateMachine.Builder = AsyncTaskMethodBuilder<DialogResult>.Create();
            stateMachine.Instance = this;
            stateMachine.State = -1;
            stateMachine.Builder.Start(ref stateMachine);
            return stateMachine.Builder.Task;
        }

        // The actual async dialog logic
        private async Task<DialogResult> ShowDialogAsync()
        {
            if (pickerForm != null && !pickerForm.IsDisposed)
            {
                pickerForm.Dispose();
            }

            pickerForm = new ColorPickerForm(selectedColor, 8);
            pickerForm.Theme = currentTheme == ThemeOptions.Dark
                ? ColorPickerForm.ThemeMode.Dark
                : ColorPickerForm.ThemeMode.Light;

            pickerForm.ToggleThemeSwitchButton(enableThemeChangeButton);

            DialogResult result = DialogResult.Cancel;

            // Show the dialog modally
            await Task.Run(() =>
            {
                if (pickerForm.InvokeRequired)
                {
                    pickerForm.Invoke(new Action(() =>
                    {
                        result = pickerForm.ShowDialog();
                    }));
                }
                else
                {
                    result = pickerForm.ShowDialog();
                }
            });

            // If user clicked OK, update the selected color
            if (result == DialogResult.OK)
            {
                selectedColor = pickerForm.ColorVal;
            }

            return result;
        }

        public ThemeOptions Theme
        {
            get => currentTheme;
            set => currentTheme = value;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();

                if (pickerForm != null && !pickerForm.IsDisposed)
                    pickerForm.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent() => components = new Container();

        // State machine struct for async dialog
        private struct ColorPickerStateMachine : IAsyncStateMachine
        {
            public int State;
            public AsyncTaskMethodBuilder<DialogResult> Builder;
            public ColorPickerComponent Instance;
            private TaskAwaiter<DialogResult> awaiter;

            public void MoveNext()
            {
                int state = State;
                DialogResult result;

                try
                {
                    if (state != 0)
                    {
                        // Start the async operation
                        awaiter = Instance.ShowDialogAsync().GetAwaiter();

                        if (!awaiter.IsCompleted)
                        {
                            State = 0;
                            Builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
                            return;
                        }
                    }
                    else
                    {
                        // Resume after await
                        State = -1;
                    }

                    // Get the result
                    result = awaiter.GetResult();
                }
                catch (Exception ex)
                {
                    State = -2;
                    Builder.SetException(ex);
                    return;
                }

                State = -2;
                Builder.SetResult(result);
            }

            public void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                Builder.SetStateMachine(stateMachine);
            }
        }
    }

    public enum ThemeOptions
    {
        Dark,
        Light,
    }
}