// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.EventArguments.LocalTimeEventArgs
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Net.EventArguments;

/// <summary>
/// Represents the class that contain event data for <see cref="E:Patagames.Pdf.Net.PdfForms.LocalTime" /> event
/// </summary>
public class LocalTimeEventArgs : EventArgs
{
  /// <summary>Application should sets current time to this property</summary>
  public DateTime Time { get; set; }
}
