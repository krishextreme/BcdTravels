using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ledger.Animations
{
    // Placeholder for special control type
    public class SpecialControl : Control
    {
        // Special control that shouldn't have opacity animation applied
    }

    [ToolboxBitmap(typeof(TrackBar))]
    public class ControlAnimator : Component
    {
        private PaintEventHandler paintHandler;
        private Control targetControl;
        private int duration = 1000;
        private bool animateOpacity;
        private TargetOpacityState targetOpacity = TargetOpacityState.Visible;
        private int startX;
        private int startY;
        private double xDistance;
        private double yDistance;
        private double elapsedTime;
        private bool animating;
        private bool animationFinished = true;
        private byte currentControlOpacity = byte.MaxValue;
        public EventHandler AnimationEnded;
        public EventHandler AnimationStarted;
        private IContainer components;

        public ControlAnimator()
        {
            InitializeComponent();
            paintHandler = (sender, e) =>
            {
                if (animationFinished || !AnimateOpacity || currentControlOpacity > 254 || sender is SpecialControl)
                    return;

                Rectangle clientRectangle = TargetControl.ClientRectangle;
                clientRectangle.Inflate(2, 2);
                using (SolidBrush solidBrush = new SolidBrush(Color.FromArgb(currentControlOpacity, TargetControl.BackColor)))
                {
                    e.Graphics.FillRectangle(solidBrush, clientRectangle);
                }
            };
        }

        [Description("The control to animate.")]
        public Control TargetControl
        {
            get => targetControl;
            set
            {
                targetControl = value;
                if (value == null)
                    return;

                if (value is Form && DesignMode)
                {
                    MessageBox.Show("Hey, You are probably looking for 'cuiFormAnimator' to animate Form.\nYou can still use 'cuiControlAnimator' though!", "CuoreUI");
                    targetControl = null;
                }
                else
                {
                    targetControl.HandleCreated += new EventHandler(TargetControl_HandleCreated);
                }
            }
        }

        private void TargetControl_HandleCreated(object sender, EventArgs e)
        {
            if (!AnimateOnStart)
                return;

            PlayAnimation();
        }

        [Description("How long the animation should last in milliseconds. (ms)")]
        public int Duration
        {
            get => duration;
            set => duration = value;
        }

        [Description("Animates 'opacity' of the control from 0 -> 1.")]
        public bool AnimateOpacity
        {
            get => animateOpacity;
            set => animateOpacity = value;
        }

        [Description("Choose the easing type that suits the best.")]
        public Ledger.Animations.EasingType EasingType { get; set; } = Ledger.Animations.EasingType.QuadOut;

        [Description("Where the TargetControl should be moved to.")]
        public Point TargetLocation { get; set; } = Point.Empty;

        [Description("Animate control when first shown on screen.")]
        public bool AnimateOnStart { get; set; } = true;

        [Description("Either move to TargetLocation or ignore animating location.")]
        public bool AnimateLocation { get; set; } = true;

        [Description("Target opacity (0 - 255) for the control when animation completes.")]
        public TargetOpacityState TargetOpacity
        {
            get => targetOpacity;
            set => targetOpacity = value;
        }

        public Task PlayAnimation()
        {
            AnimationStateMachine stateMachine = new AnimationStateMachine();
            stateMachine.Builder = AsyncTaskMethodBuilder.Create();
            stateMachine.Instance = this;
            stateMachine.State = -1;
            stateMachine.Builder.Start(ref stateMachine);
            return stateMachine.Builder.Task;
        }

        // The actual animation logic
        private async Task PlayAnimationAsync()
        {
            if (targetControl == null || targetControl.IsDisposed)
                return;

            animationFinished = false;
            animating = true;
            AnimationStarted?.Invoke(this, EventArgs.Empty);

            // Store starting position
            startX = targetControl.Location.X;
            startY = targetControl.Location.Y;

            // Calculate distances
            if (AnimateLocation && TargetLocation != Point.Empty)
            {
                xDistance = TargetLocation.X - startX;
                yDistance = TargetLocation.Y - startY;
            }
            else
            {
                xDistance = 0;
                yDistance = 0;
            }

            // Set initial opacity if animating opacity
            byte startOpacity = currentControlOpacity;
            byte endOpacity = (byte)targetOpacity;

            if (AnimateOpacity)
            {
                currentControlOpacity = 0;
                if (targetControl.Parent != null)
                    targetControl.Parent.Paint += paintHandler;
            }

            DateTime startTime = DateTime.Now;
            elapsedTime = 0;

            while (elapsedTime < duration)
            {
                if (targetControl.IsDisposed)
                    break;

                elapsedTime = (DateTime.Now - startTime).TotalMilliseconds;
                double progress = Math.Min(elapsedTime / duration, 1.0);

                // Apply easing
                double easedProgress = ApplyEasing(progress, EasingType);

                // Update location
                if (AnimateLocation && TargetLocation != Point.Empty)
                {
                    int newX = (int)(startX + xDistance * easedProgress);
                    int newY = (int)(startY + yDistance * easedProgress);

                    if (targetControl.InvokeRequired)
                    {
                        targetControl.Invoke(new Action(() =>
                        {
                            if (!targetControl.IsDisposed)
                                targetControl.Location = new Point(newX, newY);
                        }));
                    }
                    else
                    {
                        targetControl.Location = new Point(newX, newY);
                    }
                }

                // Update opacity
                if (AnimateOpacity)
                {
                    currentControlOpacity = (byte)(startOpacity + (endOpacity - startOpacity) * easedProgress);
                    targetControl.Invalidate();
                    targetControl.Parent?.Invalidate();
                }

                await Task.Delay(16); // ~60 FPS
            }

            // Ensure final values are set
            if (AnimateLocation && TargetLocation != Point.Empty)
                targetControl.Location = TargetLocation;

            if (AnimateOpacity)
            {
                currentControlOpacity = endOpacity;
                if (targetControl.Parent != null)
                    targetControl.Parent.Paint -= paintHandler;
            }

            animationFinished = true;
            animating = false;
            AnimationEnded?.Invoke(this, EventArgs.Empty);
        }

        private double ApplyEasing(double t, Ledger.Animations.EasingType easingType)
        {
            switch (easingType)
            {
                case Ledger.Animations.EasingType.QuadOut:
                    return t * (2.0 - t);
                case Ledger.Animations.EasingType.Linear:
                    return t;
                case Ledger.Animations.EasingType.QuadIn:
                    return t * t;
                case Ledger.Animations.EasingType.CubicOut:
                    t--;
                    return t * t * t + 1.0;
                case Ledger.Animations.EasingType.CubicIn:
                    return t * t * t;
                default:
                    return t;
            }
        }

        public bool IsAnimationFinished() => animationFinished;

        private void EmergencySetLocation(int duration, bool shouldAnimateLocationNow)
        {
            EmergencyLocationStateMachine stateMachine = new EmergencyLocationStateMachine();
            stateMachine.Builder = AsyncVoidMethodBuilder.Create();
            stateMachine.Instance = this;
            stateMachine.Duration = duration;
            stateMachine.ShouldAnimateLocationNow = shouldAnimateLocationNow;
            stateMachine.State = -1;
            stateMachine.Builder.Start(ref stateMachine);
        }

        // Emergency location setting logic
        private async void EmergencySetLocationAsync(int emergencyDuration, bool shouldAnimateLocationNow)
        {
            if (targetControl == null || targetControl.IsDisposed)
                return;

            bool originalAnimateLocation = AnimateLocation;
            AnimateLocation = shouldAnimateLocationNow;

            int originalDuration = duration;
            duration = emergencyDuration;

            await PlayAnimationAsync();

            duration = originalDuration;
            AnimateLocation = originalAnimateLocation;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();

                if (targetControl != null && targetControl.Parent != null)
                    targetControl.Parent.Paint -= paintHandler;
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent() => components = new Container();

        public enum TargetOpacityState
        {
            Transparent = 0,
            Visible = 255,
        }

        // State machine for PlayAnimation
        private struct AnimationStateMachine : IAsyncStateMachine
        {
            public int State;
            public AsyncTaskMethodBuilder Builder;
            public ControlAnimator Instance;
            private TaskAwaiter awaiter;

            public void MoveNext()
            {
                int state = State;

                try
                {
                    if (state != 0)
                    {
                        awaiter = Instance.PlayAnimationAsync().GetAwaiter();

                        if (!awaiter.IsCompleted)
                        {
                            State = 0;
                            Builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
                            return;
                        }
                    }
                    else
                    {
                        State = -1;
                    }

                    awaiter.GetResult();
                }
                catch (Exception ex)
                {
                    State = -2;
                    Builder.SetException(ex);
                    return;
                }

                State = -2;
                Builder.SetResult();
            }

            public void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                Builder.SetStateMachine(stateMachine);
            }
        }

        // State machine for EmergencySetLocation
        private struct EmergencyLocationStateMachine : IAsyncStateMachine
        {
            public int State;
            public AsyncVoidMethodBuilder Builder;
            public ControlAnimator Instance;
            public int Duration;
            public bool ShouldAnimateLocationNow;

            public void MoveNext()
            {
                try
                {
                    if (State == -1)
                    {
                        Instance.EmergencySetLocationAsync(Duration, ShouldAnimateLocationNow);
                    }
                }
                catch (Exception ex)
                {
                    Builder.SetException(ex);
                    return;
                }

                Builder.SetResult();
            }

            public void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                Builder.SetStateMachine(stateMachine);
            }
        }
    }
}