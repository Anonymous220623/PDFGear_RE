// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Actions.PdfJavaScriptAction
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using System;

#nullable disable
namespace Patagames.Pdf.Net.Actions;

/// <summary>
/// A JavaScript action causes a script to be compiled and executed by the JavaScript interpreter.
/// </summary>
public class PdfJavaScriptAction : PdfAction
{
  /// <summary>
  /// Gets or sets a text string or text stream containing the JavaScript script to be executed.
  /// </summary>
  public string JavaScript
  {
    get
    {
      if (!this.Dictionary.ContainsKey("JS"))
        return (string) null;
      if (this.Dictionary["JS"].Is<PdfTypeString>())
        return this.Dictionary["JS"].As<PdfTypeString>().UnicodeString;
      return this.Dictionary["JS"].Is<PdfTypeStream>() ? this.Dictionary["JS"].As<PdfTypeStream>().DecodedText : "";
    }
    set
    {
      if (value == null && this.Dictionary.ContainsKey("JS"))
      {
        this.Dictionary.Remove("JS");
      }
      else
      {
        if (value == null)
          return;
        this.Dictionary["JS"] = (PdfTypeBase) PdfTypeString.Create(value, true);
      }
    }
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Actions.PdfJavaScriptAction" /> class.
  /// </summary>
  /// <param name="document">Document which contains this action.</param>
  /// <param name="javaScript">The uniform resource locator (URL) of the script at the Web server that will process the submission.</param>
  public PdfJavaScriptAction(PdfDocument document, string javaScript)
    : base(document, PdfTypeDictionary.Create().Handle)
  {
    if (javaScript == null)
      throw new ArgumentNullException(nameof (javaScript));
    this.Dictionary["Type"] = (PdfTypeBase) PdfTypeName.Create("Action");
    this.Dictionary["S"] = (PdfTypeBase) PdfTypeName.Create(nameof (JavaScript));
    this.JavaScript = javaScript;
  }

  /// <summary>Initializes a new instance of the PdfAction class.</summary>
  /// <param name="document">Document which contains this action.</param>
  /// <param name="handle">Pdfium SDK handle that the action is bound to</param>
  public PdfJavaScriptAction(PdfDocument document, IntPtr handle)
    : base(document, handle)
  {
  }
}
