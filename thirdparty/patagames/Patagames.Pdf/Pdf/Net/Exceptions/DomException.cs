// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Exceptions.DomException
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Properties;

#nullable disable
namespace Patagames.Pdf.Net.Exceptions;

/// <summary>
/// The exception that is thrown when an element is not present in the DOM.
/// </summary>
public class DomException : PdfiumException
{
  /// <summary>Initializes a new instance of the DomException class.</summary>
  public DomException(PdfField field)
    : base(1004U, string.Format(Error.err0071, (object) DomException.GetFieldName(field)))
  {
  }

  /// <summary>Initializes a new instance of the DomException class.</summary>
  public DomException(PdfControl control)
    : base(1005U, Error.err0072)
  {
  }

  /// <summary>Initializes a new instance of the DomException class.</summary>
  public DomException(string message)
    : base(1006U, message)
  {
  }

  private static string GetFieldName(PdfField field)
  {
    return field == null || field.Dictionary == null || !field.Dictionary.ContainsKey("T") || !field.Dictionary["T"].Is<PdfTypeString>() ? "" : field.Dictionary["T"].As<PdfTypeString>().UnicodeString;
  }
}
