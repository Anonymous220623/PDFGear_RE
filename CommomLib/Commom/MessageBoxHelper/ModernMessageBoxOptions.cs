// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.MessageBoxHelper.ModernMessageBoxOptions
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System.Globalization;
using System.Windows;

#nullable disable
namespace CommomLib.Commom.MessageBoxHelper;

public class ModernMessageBoxOptions
{
  public ModernMessageBoxOptions()
  {
    this.Caption = "";
    this.MessageBoxContent = (object) "";
    this.Button = MessageBoxButton.OK;
    this.DefaultResult = MessageBoxResult.None;
    this.UIOverrides = new ModernMessageBoxUIOverrides();
  }

  public Window Owner { get; set; }

  public string Caption { get; set; }

  public object MessageBoxContent { get; set; }

  public MessageBoxButton Button { get; set; }

  public MessageBoxResult DefaultResult { get; set; }

  public ModernMessageBoxUIOverrides UIOverrides { get; }

  public CultureInfo CultureInfo { get; set; }
}
