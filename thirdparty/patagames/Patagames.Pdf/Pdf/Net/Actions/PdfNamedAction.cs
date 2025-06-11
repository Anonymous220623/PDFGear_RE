// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Actions.PdfNamedAction
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using System;

#nullable disable
namespace Patagames.Pdf.Net.Actions;

/// <summary>Represents a named action.</summary>
public class PdfNamedAction : PdfAction
{
  /// <summary>Gets or sets the name of the action to be performed.</summary>
  public string Name
  {
    get
    {
      if (!this.Dictionary.ContainsKey("N"))
        return (string) null;
      return !this.Dictionary["N"].Is<PdfTypeName>() ? (string) null : this.Dictionary["N"].As<PdfTypeName>().Value;
    }
    set
    {
      if (value == null && this.Dictionary.ContainsKey("N"))
      {
        this.Dictionary.Remove("N");
      }
      else
      {
        if (value == null)
          return;
        this.Dictionary["N"] = (PdfTypeBase) PdfTypeName.Create(value);
      }
    }
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Actions.PdfSoundAction" /> class with the document and the destination.
  /// </summary>
  /// <param name="document">Document which contains this action.</param>
  /// <param name="name">The name of the action to be performed. Please see remarks section. </param>
  /// <remarks>
  /// List of several named actions that PDF viewer applications are expected to support:
  /// NextPage, PrevPage, FirstPage, LastPage.
  /// </remarks>
  public PdfNamedAction(PdfDocument document, string name)
    : base(document, PdfTypeDictionary.Create().Handle)
  {
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    this.Dictionary["Type"] = (PdfTypeBase) PdfTypeName.Create("Action");
    this.Dictionary["S"] = (PdfTypeBase) PdfTypeName.Create("Named");
    this.Name = name;
  }

  /// <summary>Initializes a new instance of the PdfAction class.</summary>
  /// <param name="document">Document which contains this action.</param>
  /// <param name="handle">Pdfium SDK handle that the action is bound to</param>
  public PdfNamedAction(PdfDocument document, IntPtr handle)
    : base(document, handle)
  {
  }
}
