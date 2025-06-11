// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.MessageBoxHelper.ModernMessageBoxUIOverrides
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

#nullable disable
namespace CommomLib.Commom.MessageBoxHelper;

public class ModernMessageBoxUIOverrides
{
  public ModernMessageBoxUIOverrides() => this.HighlightPrimaryButton = true;

  public bool IsButtonsReversed { get; set; }

  public bool HighlightPrimaryButton { get; set; }

  public object OKButtonContent { get; set; }

  public object CancelButtonContent { get; set; }

  public object YesButtonContent { get; set; }

  public object NoButtonContent { get; set; }

  public bool HideButtons { get; set; }
}
