// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.EventArguments.PageCollectionChangedEventArgs
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Net.EventArguments;

/// <summary>
/// Represents the class that contain event data for <see cref="E:Patagames.Pdf.Net.PdfPageCollection.PageInserted" /> and
/// <see cref="E:Patagames.Pdf.Net.PdfPageCollection.PageDeleted" /> events.
/// </summary>
public class PageCollectionChangedEventArgs : EventArgs
{
  /// <summary>
  /// Gets the index of the page which was added or deleted.
  /// </summary>
  public int Index { get; private set; }

  /// <summary>
  /// Construct <see cref="T:Patagames.Pdf.Net.EventArguments.PageCollectionChangedEventArgs" /> object
  /// </summary>
  /// <param name="index">Zero-based index of the page which was added or deleted.</param>
  public PageCollectionChangedEventArgs(int index) => this.Index = index;
}
