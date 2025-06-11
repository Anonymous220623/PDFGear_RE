// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.MessageBoxHelper.ModernMessageBoxCloser
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

#nullable disable
namespace CommomLib.Commom.MessageBoxHelper;

public class ModernMessageBoxCloser
{
  internal CommomLib.Controls.ModernMessageBox Owner { get; set; }

  public void Close(bool? result)
  {
    if (this.Owner == null || !this.Owner.IsVisible)
      return;
    this.Owner.DialogResult = result;
  }
}
