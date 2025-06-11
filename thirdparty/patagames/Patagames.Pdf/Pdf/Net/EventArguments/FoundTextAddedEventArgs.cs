// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.EventArguments.FoundTextAddedEventArgs
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Net.EventArguments;

/// <summary>
/// Represents the class that contain event data for <see cref="E:Patagames.Pdf.Net.PdfSearch.FoundTextAdded" /> event.
/// </summary>
public class FoundTextAddedEventArgs : EventArgs
{
  /// <summary>Gets found text which was added into search result.</summary>
  public PdfSearch.FoundText FoundText;

  /// <summary>
  /// Initialize the new instance of PdfSearchFoundTextAddedEventArgs class
  /// </summary>
  /// <param name="foundText">Found text structure</param>
  public FoundTextAddedEventArgs(PdfSearch.FoundText foundText) => this.FoundText = foundText;
}
