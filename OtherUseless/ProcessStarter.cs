// Decompiled with JetBrains decompiler
// Type: �ᚁ��S�ᚹ�ΥB���.��Kᛇ�DᚄHᚒ�ᚈ�
// Assembly: Ledger Live, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5DC7CF87-871B-48BD-97F4-4117271260D2
// Assembly location: C:\Users\hps\Desktop\ledger\new\ledger.exe

using System.Diagnostics;

namespace Ledger.otherUseless
{

    internal class ProcessStarter // was \uFFFD\uFFFDKᛇ\uFFFDDᚄHᚒ\uFFFDᚈ\uFFFD unicode
    {
        private readonly ProcessStartConfig _config; // was _442

        public ProcessStarter(ProcessStartConfig config) // was \uFFFD\uFFFDKᛇ\uFFFDDᚄHᚒ\uFFFDᚈ\uFFFD unicode, was ProcessStartConfig _497
        {
            this._config = config;
        }

        internal void Start()
        {
            Process.Start(new ProcessStartInfo()
            {
                CreateNoWindow = this._config.createNoWindow, // was _313
                UseShellExecute = this._config.useShellExecute, // was _620
                FileName = this._config.fileName, // was _376
                Arguments = this._config.arguments // was _680
            });
        }
    }
}