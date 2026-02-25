using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Ledger.Animations
{
    // Enum for easing types - moved outside the class to avoid ambiguity
    public enum EasingType
    {
        QuadOut,
        Linear,
        QuadIn,
        CubicOut,
        CubicIn
    }

    public class OpacityAnimator : Component
    {
        private Form targetForm;
        private double startOpacity;
        private double targetOpacity = 1.0;
        private double lastKnownOpacity;
        private byte animationsInQueue;
        private bool cancelRequested;
        private IContainer components;

        public OpacityAnimator() => InitializeComponent();

        public OpacityAnimator(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        [Category("Animating")]
        public Form TargetForm
        {
            get => targetForm;
            set
            {
                if (targetForm != null)
                    targetForm.HandleCreated -= new EventHandler(OnHandleCreated);
                targetForm = value;
                if (value == null)
                    return;
                value.HandleCreated += new EventHandler(OnHandleCreated);
            }
        }

        [Category("Animating")]
        public bool AnimateOnStart { get; set; } = true;

        [Category("Animating Opacity")]
        public double StartOpacity
        {
            get => startOpacity;
            set => startOpacity = ClampOpacity(value);
        }

        [Category("Animating Opacity")]
        public double TargetOpacity
        {
            get => targetOpacity;
            set => targetOpacity = ClampOpacity(value);
        }

        [Category("Animating")]
        public int Duration { get; set; } = 400;

        [Category("Animating")]
        public EasingType EasingType { get; set; } = EasingType.QuadOut;

        private void OnHandleCreated(object sender, EventArgs e)
        {
            if (!AnimateOnStart)
                return;

            targetForm.Opacity = StartOpacity;
            lastKnownOpacity = StartOpacity;
            targetForm.Shown += new EventHandler(OnTargetFormShown);
            targetForm.VisibleChanged += new EventHandler(OnTargetFormVisibleChanged);
        }

        private void OnTargetFormVisibleChanged(object sender, EventArgs e)
        {
            double? opacity = targetForm?.Opacity;
            double lastKnownOpacity = this.lastKnownOpacity;
            if (opacity.GetValueOrDefault() == lastKnownOpacity && opacity.HasValue)
                return;

            cancelRequested = true;
        }

        private double ClampOpacity(double targetOpacity)
        {
            if (targetOpacity < 0.0)
                return 0.0;
            return targetOpacity <= 1.0 ? targetOpacity : 1.0;
        }

        public void PlayAnimation()
        {
            targetForm.Opacity = StartOpacity;
            lastKnownOpacity = StartOpacity;
            OnTargetFormShown(this, EventArgs.Empty);
        }

        private void OnTargetFormShown(object sender, EventArgs e)
        {
            AnimationStateMachine stateMachine = new AnimationStateMachine();
            stateMachine.Builder = AsyncVoidMethodBuilder.Create();
            stateMachine.Instance = this;
            stateMachine.State = -1;
            stateMachine.Builder.Start(ref stateMachine);
        }

        // The actual animation logic
        public async void AnimateOpacityAsync()
        {
            animationsInQueue++;
            cancelRequested = false;

            if (targetForm == null || targetForm.IsDisposed)
            {
                animationsInQueue--;
                return;
            }

            double startValue = targetForm.Opacity;
            double endValue = TargetOpacity;
            int duration = Duration;
            DateTime startTime = DateTime.Now;

            while (true)
            {
                if (cancelRequested || targetForm.IsDisposed)
                    break;

                TimeSpan elapsed = DateTime.Now - startTime;
                double progress = Math.Min(elapsed.TotalMilliseconds / duration, 1.0);

                // Apply easing
                double easedProgress = ApplyEasing(progress, EasingType);
                double newOpacity = startValue + (endValue - startValue) * easedProgress;

                if (targetForm.InvokeRequired)
                {
                    targetForm.Invoke(new Action(() =>
                    {
                        if (!targetForm.IsDisposed)
                            targetForm.Opacity = newOpacity;
                    }));
                }
                else
                {
                    targetForm.Opacity = newOpacity;
                }

                lastKnownOpacity = newOpacity;

                if (progress >= 1.0)
                    break;

                await Task.Delay(16); // ~60 FPS
            }

            animationsInQueue--;
        }

        private double ApplyEasing(double t, EasingType easingType)
        {
            switch (easingType)
            {
                case EasingType.Linear:
                    return t;
                case EasingType.QuadIn:
                    return t * t;
                case EasingType.QuadOut:
                    return t * (2.0 - t);
                case EasingType.CubicIn:
                    return t * t * t;
                case EasingType.CubicOut:
                    t--;
                    return t * t * t + 1.0;
                default:
                    return t;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent() => components = new Container();

        // State machine struct for async animation
        //public struct AnimationStateMachine : IAsyncStateMachine
        //{
        //    public int State;
        //    public AsyncVoidMethodBuilder Builder;
        //    public OpacityAnimator Instance;

        //    public void MoveNext()
        //    {
        //        try
        //        {
        //            if (State == -1)
        //            {
        //                // Start the animation
        //                Instance.AnimateOpacityAsync();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Builder.SetException(ex);
        //            return;
        //        }

        //        Builder.SetResult();
        //    }

        //    public void SetStateMachine(IAsyncStateMachine stateMachine)
        //    {
        //        Builder.SetStateMachine(stateMachine);
        //    }
        //}
    }
}