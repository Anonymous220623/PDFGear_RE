// Decompiled with JetBrains decompiler
// Type: CommomLib.Controls.LaunchHyperlink
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Diagnostics;
using System.Windows.Documents;

#nullable disable
namespace CommomLib.Controls;

internal class LaunchHyperlink : Hyperlink
{
  protected override void OnClick()
  {
    base.OnClick();
    Uri navigateUri = this.NavigateUri;
    if (!(navigateUri != (Uri) null))
      return;
    try
    {
      Process.Start(navigateUri.OriginalString);
    }
    catch
    {
    }
  }
}
