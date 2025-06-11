// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.EventArguments.FillFormNotifyAfterEventArgs
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
public class FillFormNotifyAfterEventArgs : EventArgs
{
  /// <summary>
  /// Gets <see cref="T:Patagames.Pdf.Net.PdfField" /> that fires the event.
  /// </summary>
  public PdfField Field { get; private set; }

  /// <summary>Construct FillFormNotifyAfterEventArgs object.</summary>
  /// <param name="Field">A PDF form</param>
  public FillFormNotifyAfterEventArgs(PdfField Field) => this.Field = Field;
}
