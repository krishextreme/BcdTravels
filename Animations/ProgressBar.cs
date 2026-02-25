// Decompiled with JetBrains decompiler
// Type: ProgressIndicatorComponent
// Assembly: Ledger Live, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5DC7CF87-871B-48BD-97F4-4117271260D2
// Assembly location: C:\Users\hps\Desktop\for\ghidra files\ledger\ledger.exe

using Microsoft.WindowsAPICodePack.Taskbar;
using System;
using System.ComponentModel;
using System.Windows.Forms;


namespace Ledger.Animations 
{
    public class ProgressIndicatorComponent : Component
    {
        private Form targetForm;
        private ProgressState privateState = ProgressState.Default;
        private int progressValue = 50;
        private int maxValue = 100;
        private IContainer components;

        public Form TargetForm
        {
            get => targetForm;
            set
            {
                targetForm = value;
                if (targetForm == null)
                    return;
                targetForm.Shown += new EventHandler(TargetForm_Shown);
            }
        }

        private void TargetForm_Shown(object sender, EventArgs e)
        {
            UpdateProgressValue();
            UpdateTaskbarState();
        }

        public ProgressIndicatorComponent()
        {
            InitializeComponent();
            MaxValue = 100;
        }

        public ProgressIndicatorComponent(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
            MaxValue = 100;
        }

        public ProgressState State
        {
            get => privateState;
            set
            {
                privateState = value;
                UpdateTaskbarState();
            }
        }

        public int ProgressValue
        {
            get => progressValue;
            set
            {
                progressValue = value;
                UpdateProgressValue();
            }
        }

        public int MaxValue
        {
            get => maxValue;
            set
            {
                maxValue = value;
                UpdateProgressValue();
            }
        }

        private void UpdateTaskbarState()
        {
            if (targetForm == null || DesignMode || targetForm.Handle ==IntPtr.Zero)
                return;

            TaskbarManager instance = TaskbarManager.Instance;
            switch (privateState)
            {
                case ProgressState.Progress:
                    instance.SetProgressState(TaskbarProgressBarState.Normal, targetForm.Handle);
                    break;
                case ProgressState.Paused:
                    instance.SetProgressState(TaskbarProgressBarState.Paused, targetForm.Handle);
                    break;
                case ProgressState.Error:
                    instance.SetProgressState(TaskbarProgressBarState.Error, targetForm.Handle);
                    break;
                default:
                    instance.SetProgressState(TaskbarProgressBarState.NoProgress, targetForm.Handle);
                    break;
            }
        }

        private void UpdateProgressValue()
        {
            if (targetForm == null || DesignMode || targetForm.Handle ==IntPtr.Zero)
                return;

            TaskbarManager.Instance.SetProgressValue(progressValue, maxValue, targetForm.Handle);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent() => components = new Container();

        public enum ProgressState
        {
            Progress,
            Paused,
            Error,
            Default,
        }
    }
}
