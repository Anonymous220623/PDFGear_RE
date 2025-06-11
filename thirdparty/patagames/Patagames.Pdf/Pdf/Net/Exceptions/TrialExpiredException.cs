// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Exceptions.TrialExpiredException
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Properties;

#nullable disable
namespace Patagames.Pdf.Net.Exceptions;

/// <summary>
/// You are currently evaluating the library and the trial period is expired.
/// </summary>
public class TrialExpiredException : PdfiumException
{
  /// <summary>
  /// Initializes a new instance of the TrialExpiredException class.
  /// </summary>
  public TrialExpiredException()
    : base(1003U, Error.err0059)
  {
  }
}
