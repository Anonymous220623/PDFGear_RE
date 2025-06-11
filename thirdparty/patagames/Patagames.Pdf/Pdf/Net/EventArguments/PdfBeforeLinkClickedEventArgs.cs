// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.EventArguments.PdfBeforeLinkClickedEventArgs
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Net.EventArguments;

/// <summary>
/// Represents the class that contain event data for <see cref="E:Patagames.Pdf.Net.Controls.WinForms.PdfViewer.BeforeLinkClicked" /> event.
/// </summary>
/// <remarks>In this event handler, you can cancel the link action.</remarks>
public class PdfBeforeLinkClickedEventArgs : EventArgs
{
  /// <summary>Gets WebLink object if it was clicked, null otherwise</summary>
  public PdfWebLink WebLink { get; private set; }

  /// <summary>Gets PdfLink object if it was clicked, null otherwise</summary>
  public PdfLink Link { get; private set; }

  /// <summary>
  /// Determines whether the default action is canceled. True to cancel, false otherwise.
  /// </summary>
  public bool Cancel { get; set; }

  /// <summary>Construct new PdfBeforeLinkClickedEventArgs object</summary>
  /// <param name="WebLink">WebLink object that was clicked.</param>
  /// <param name="Link">PdfLink object that was clicked.</param>
  public PdfBeforeLinkClickedEventArgs(PdfWebLink WebLink, PdfLink Link)
  {
    this.WebLink = WebLink;
    this.Link = Link;
  }
}
