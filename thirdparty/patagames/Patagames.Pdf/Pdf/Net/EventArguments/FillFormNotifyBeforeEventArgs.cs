// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.EventArguments.FillFormNotifyBeforeEventArgs
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Net.EventArguments;

/// <summary>
/// Represents the class that contain event data for form notifications
/// </summary>
public class FillFormNotifyBeforeEventArgs : EventArgs
{
  /// <summary>
  /// Gets <see cref="T:Patagames.Pdf.Net.PdfField" /> that fires the event.
  /// </summary>
  public PdfField Field { get; private set; }

  /// <summary>Gets field value</summary>
  public string Value { get; private set; }

  /// <summary>
  /// Gets or sets a flag indicating whether the action should be canceled
  /// </summary>
  public bool IsCancel { get; set; }

  /// <summary>Construct FillFormNotifyBeforeEventArgs object.</summary>
  /// <param name="Field">A PDF form</param>
  /// <param name="Value">Field's value</param>
  public FillFormNotifyBeforeEventArgs(PdfField Field, string Value)
  {
    this.Field = Field;
    this.Value = Value;
  }
}
