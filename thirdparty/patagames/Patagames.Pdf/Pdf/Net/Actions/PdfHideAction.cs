// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Actions.PdfHideAction
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Wrappers;
using Patagames.Pdf.Properties;
using System;

#nullable disable
namespace Patagames.Pdf.Net.Actions;

/// <summary>
/// Represents a hide action. A hide action hides or shows one or more annotations on the screen by setting or clearing their Hidden flags.
/// </summary>
public class PdfHideAction : PdfAction
{
  private PdfIndirectList _list;

  /// <summary>
  /// Gets or sets a flag indicating whether to hide the annotation (true) or show it (false).
  /// </summary>
  public bool Hide
  {
    get
    {
      return !this.Dictionary.ContainsKey("H") || !this.Dictionary["H"].Is<PdfTypeBoolean>() || this.Dictionary["H"].As<PdfTypeBoolean>().Value;
    }
    set => this.Dictionary["H"] = (PdfTypeBase) PdfTypeBoolean.Create(value);
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Actions.PdfHideAction" /> class with the <see cref="T:Patagames.Pdf.Net.Annotations.PdfAnnotation" />.
  /// </summary>
  /// <param name="document">Document which contains this action.</param>
  /// <param name="annotation">The annotation whose Hidden flag should be setting or clearing.</param>
  public PdfHideAction(PdfDocument document, PdfAnnotation annotation)
    : base(document, PdfTypeDictionary.Create().Handle)
  {
    if ((PdfWrapper) annotation != (PdfWrapper) null && annotation.Dictionary.ObjectNumber == 0 && Pdfium.FPDFOBJ_GetParentObj(annotation.Dictionary.Handle) != IntPtr.Zero)
      throw new ArgumentException(string.Format(Error.err0067, (object) nameof (annotation), (object) "object"));
    this._list = PdfIndirectList.FromPdfDocument(this.Document);
    this.Dictionary["Type"] = (PdfTypeBase) PdfTypeName.Create("Action");
    this.Dictionary["S"] = (PdfTypeBase) PdfTypeName.Create(nameof (Hide));
    if (!((PdfWrapper) annotation != (PdfWrapper) null))
      return;
    this._list.Add((PdfTypeBase) annotation.Dictionary);
    this.Dictionary.SetIndirectAt("T", this._list, (PdfTypeBase) annotation.Dictionary);
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Actions.PdfHideAction" /> class with the array of <see cref="T:Patagames.Pdf.Net.Annotations.PdfAnnotation" />.
  /// </summary>
  /// <param name="document">Document which contains this action.</param>
  /// <param name="annotations">The annotation whose Hidden flag should be setting or clearing.</param>
  public PdfHideAction(PdfDocument document, PdfAnnotation[] annotations)
    : base(document, PdfTypeDictionary.Create().Handle)
  {
    this._list = PdfIndirectList.FromPdfDocument(this.Document);
    this.Dictionary["Type"] = (PdfTypeBase) PdfTypeName.Create("Action");
    this.Dictionary["S"] = (PdfTypeBase) PdfTypeName.Create(nameof (Hide));
    if (annotations == null)
      return;
    PdfTypeArray pdfTypeArray = (PdfTypeArray) null;
    for (int index = 0; index < annotations.Length; ++index)
    {
      if (!((PdfWrapper) annotations[index] == (PdfWrapper) null))
      {
        if (annotations[index].Dictionary.ObjectNumber == 0 && Pdfium.FPDFOBJ_GetParentObj(annotations[index].Dictionary.Handle) != IntPtr.Zero)
          throw new ArgumentException(string.Format(Error.err0067, (object) $"annotation at {index.ToString()} pos", (object) "object"));
        this._list.Add((PdfTypeBase) annotations[index].Dictionary);
        if (pdfTypeArray == null)
          pdfTypeArray = PdfTypeArray.Create();
        pdfTypeArray.Add((PdfTypeBase) annotations[index].Dictionary, this._list);
      }
    }
    if (pdfTypeArray == null)
      return;
    this.Dictionary["T"] = (PdfTypeBase) pdfTypeArray;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Actions.PdfHideAction" /> class with fully qualified field name.
  /// </summary>
  /// <param name="document">Document which contains this action.</param>
  /// <param name="fieldName">A text string giving the fully qualified field name of an interactive form field whose associated widget annotation or annotations are to be affected.</param>
  public PdfHideAction(PdfDocument document, string fieldName)
    : base(document, PdfTypeDictionary.Create().Handle)
  {
    this._list = PdfIndirectList.FromPdfDocument(this.Document);
    this.Dictionary["Type"] = (PdfTypeBase) PdfTypeName.Create("Action");
    this.Dictionary["S"] = (PdfTypeBase) PdfTypeName.Create(nameof (Hide));
    if (fieldName == null)
      return;
    this.Dictionary["T"] = (PdfTypeBase) PdfTypeString.Create(fieldName, true, true);
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Actions.PdfHideAction" /> class with the array of <see cref="T:Patagames.Pdf.Net.Annotations.PdfAnnotation" />.
  /// </summary>
  /// <param name="document">Document which contains this action.</param>
  /// <param name="fieldNames">An array of text strings giving the fully qualified field names.</param>
  public PdfHideAction(PdfDocument document, string[] fieldNames)
    : base(document, PdfTypeDictionary.Create().Handle)
  {
    this._list = PdfIndirectList.FromPdfDocument(this.Document);
    this.Dictionary["Type"] = (PdfTypeBase) PdfTypeName.Create("Action");
    this.Dictionary["S"] = (PdfTypeBase) PdfTypeName.Create(nameof (Hide));
    if (fieldNames == null)
      return;
    PdfTypeArray pdfTypeArray = (PdfTypeArray) null;
    for (int index = 0; index < fieldNames.Length; ++index)
    {
      if (fieldNames[index] != null)
      {
        if (pdfTypeArray == null)
          pdfTypeArray = PdfTypeArray.Create();
        pdfTypeArray.Add((PdfTypeBase) PdfTypeString.Create(fieldNames[index], true, true));
      }
    }
    if (pdfTypeArray == null)
      return;
    this.Dictionary["T"] = (PdfTypeBase) pdfTypeArray;
  }

  /// <summary>Initializes a new instance of the PdfAction class.</summary>
  /// <param name="document">Document which contains this action.</param>
  /// <param name="handle">Pdfium SDK handle that the action is bound to</param>
  public PdfHideAction(PdfDocument document, IntPtr handle)
    : base(document, handle)
  {
    this._list = PdfIndirectList.FromPdfDocument(this.Document);
  }
}
