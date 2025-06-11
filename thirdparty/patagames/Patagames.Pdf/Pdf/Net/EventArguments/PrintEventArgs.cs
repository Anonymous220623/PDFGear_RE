// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.EventArguments.PrintEventArgs
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Net.EventArguments;

/// <summary>
/// Represents the class that contain event data for <see cref="E:Patagames.Pdf.Net.PdfForms.Print" /> event
/// </summary>
public class PrintEventArgs : EventArgs
{
  /// <summary>
  /// If true, will cause a UI to be presented to the user to obtain printing information and confirm the action.
  /// </summary>
  public bool bUI { get; private set; }

  /// <summary>
  /// A 0-based index that defines the start of an inclusive range of pages.
  /// </summary>
  public int nStart { get; private set; }

  /// <summary>
  /// A 0-based index that defines the end of an inclusive page range.
  /// </summary>
  public int nEnd { get; private set; }

  /// <summary>
  /// If true, suppresses the cancel dialog box while the document is printing. The default is false.
  /// </summary>
  public bool bSilent { get; private set; }

  /// <summary>
  /// If true, the page is shrunk (if necessary) to fit within the imageable area of the printed page.
  /// </summary>
  public bool bShrinkToFit { get; private set; }

  /// <summary>If true, print pages as an image.</summary>
  public bool bPrintAsImage { get; private set; }

  /// <summary>If true, print from nEnd to nStart.</summary>
  public bool bReverse { get; private set; }

  /// <summary>If true (the default), annotations are printed.</summary>
  public bool bAnnotations { get; private set; }

  /// <summary>Construct PrintEventArgs object</summary>
  /// <param name="bUI">If true, will cause a UI to be presented to the user to obtain printing information and confirm the action.</param>
  /// <param name="nStart">A 0-based index that defines the start of an inclusive range of pages.</param>
  /// <param name="nEnd">A 0-based index that defines the end of an inclusive page range.</param>
  /// <param name="bSilent">If true, suppresses the cancel dialog box while the document is printing. The default is false.</param>
  /// <param name="bShrinkToFit">If true, the page is shrunk (if necessary) to fit within the imageable area of the printed page.</param>
  /// <param name="bPrintAsImage">If true, print pages as an image.</param>
  /// <param name="bReverse">If true, print from nEnd to nStart.</param>
  /// <param name="bAnnotations">If true (the default), annotations are printed.</param>
  public PrintEventArgs(
    bool bUI,
    int nStart,
    int nEnd,
    bool bSilent,
    bool bShrinkToFit,
    bool bPrintAsImage,
    bool bReverse,
    bool bAnnotations)
  {
    this.bUI = bUI;
    this.nStart = nStart;
    this.nEnd = nEnd;
    this.bSilent = bSilent;
    this.bShrinkToFit = bShrinkToFit;
    this.bPrintAsImage = bPrintAsImage;
    this.bReverse = bReverse;
    this.bAnnotations = bAnnotations;
  }
}
