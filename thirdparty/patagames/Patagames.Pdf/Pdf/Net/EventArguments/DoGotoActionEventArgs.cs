// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.EventArguments.DoGotoActionEventArgs
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using System;

#nullable disable
namespace Patagames.Pdf.Net.EventArguments;

/// <summary>
/// Represents the class that contain event data for Goto action
/// </summary>
/// <remarks>
/// <para>Application must changes the view to a specified destination.</para>
/// <para>See the Destinations description of <a href="http://www.adobe.com/content/dam/Adobe/en/devnet/acrobat/pdfs/pdf_reference_1-7.pdf">PDF Reference, Version 1.7</a> in 8.2.1 for more details.</para></remarks>
/// <seealso href="http://www.adobe.com/content/dam/Adobe/en/devnet/acrobat/pdfs/pdf_reference_1-7.pdf">PDF Reference 1.7</seealso>
public class DoGotoActionEventArgs : EventArgs
{
  /// <summary>The index of the PDF page.</summary>
  public int PageIndex { get; private set; }

  /// <summary>The zoom mode for viewing page.See ZoomTypes.</summary>
  public ZoomTypes Zoom { get; private set; }

  /// <summary>The float array which carries the position info</summary>
  public float[] Positions { get; private set; }

  /// <summary>Construct DoGotoActionEventArgs object.</summary>
  /// <param name="PageIndex">The index of the PDF page.</param>
  /// <param name="Zoom">The zoom mode for viewing page.See ZoomTypes.</param>
  /// <param name="Positions">The float array which carries the position info</param>
  public DoGotoActionEventArgs(int PageIndex, ZoomTypes Zoom, float[] Positions)
  {
    this.PageIndex = PageIndex;
    this.Positions = Positions;
    this.Zoom = Zoom;
  }
}
