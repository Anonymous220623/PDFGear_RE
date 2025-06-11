// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.StringValidationEventArgs
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class StringValidationEventArgs : EventArgs
{
  private bool m_bCancel;
  private bool m_bIsValidInput = true;
  private string m_message = string.Empty;
  private string m_validationString = string.Empty;

  public StringValidationEventArgs(
    bool bCancel,
    bool bIsValidInput,
    string message,
    string validationString)
  {
    this.m_bCancel = bCancel;
    this.m_bIsValidInput = bIsValidInput;
    this.m_message = message;
    this.m_validationString = validationString;
  }

  public StringValidationEventArgs(bool bIsValidInput, string message, string validationString)
    : this(false, bIsValidInput, message, validationString)
  {
  }

  public StringValidationEventArgs()
  {
  }

  public bool Cancel
  {
    get => this.m_bCancel;
    set
    {
      if (this.m_bCancel == value)
        return;
      this.m_bCancel = value;
    }
  }

  public bool IsValidInput => this.m_bIsValidInput;

  public string Message => this.m_message;

  public string ValidationString => this.m_validationString;
}
