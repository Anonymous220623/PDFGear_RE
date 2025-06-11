// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.TimedListener
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
internal class TimedListener : TextWriterTraceListener
{
  public TimedListener(Stream stream)
    : base(stream)
  {
    this.IndentSize = 4;
  }

  public override void WriteLine(string message)
  {
    string[] strArray = message.Split('\n');
    string str1 = $"[{DateTime.Now.ToString("hh:mm:ss.ffff")}] ";
    if (this.IndentLevel > 0)
      str1 += new string(' ', this.IndentLevel * this.IndentSize);
    string str2 = new string(' ', str1.Length);
    for (int index = 0; index < strArray.Length; ++index)
    {
      message = index != 0 ? str2 + strArray[index] : str1 + strArray[0];
      this.NeedIndent = false;
      base.WriteLine(message);
    }
  }
}
