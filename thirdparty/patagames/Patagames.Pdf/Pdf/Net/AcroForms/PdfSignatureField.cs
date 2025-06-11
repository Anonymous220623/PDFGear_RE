// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.AcroForms.PdfSignatureField
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Wrappers;
using Patagames.Pdf.Properties;
using System;

#nullable disable
namespace Patagames.Pdf.Net.AcroForms;

/// <summary>Represents a signature field.</summary>
public class PdfSignatureField : PdfField
{
  private PdfLock _lock;

  /// <summary>
  /// Gets or sets a signature field lock that specifies a set of form fields to be locked
  /// when this signature field is signed.
  /// </summary>
  public PdfLock Lock
  {
    get
    {
      if (!this.Dictionary.ContainsKey(nameof (Lock)))
        return (PdfLock) null;
      if ((PdfWrapper) this._lock == (PdfWrapper) null || this._lock.Dictionary.IsDisposed)
        this._lock = this.Dictionary[nameof (Lock)].Is<PdfTypeDictionary>() ? new PdfLock(this.InterForms, this.Dictionary[nameof (Lock)]) : (PdfLock) null;
      return this._lock;
    }
    set
    {
      if ((PdfWrapper) value == (PdfWrapper) null && this.Dictionary.ContainsKey(nameof (Lock)))
        this.Dictionary.Remove(nameof (Lock));
      else if ((PdfWrapper) value != (PdfWrapper) null)
      {
        if (value.Dictionary.ObjectNumber == 0 && Pdfium.FPDFOBJ_GetParentObj(value.Dictionary.Handle) != IntPtr.Zero)
          throw new ArgumentException(string.Format(Error.err0067, (object) "PdfLock", (object) "object"));
        PdfIndirectList list = PdfIndirectList.FromPdfDocument(this.InterForms.FillForms.Document);
        list.Add((PdfTypeBase) value.Dictionary);
        this.Dictionary.SetIndirectAt(nameof (Lock), list, (PdfTypeBase) value.Dictionary);
      }
      this._lock = value;
    }
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.AcroForms.PdfSignatureField" /> class.
  /// </summary>
  /// <param name="forms">Interactive forms.</param>
  /// <param name="handle">The field's handle.</param>
  internal PdfSignatureField(PdfInteractiveForms forms, IntPtr handle)
    : base(forms, handle)
  {
  }

  /// <summary>
  /// Create new radiobutton field and add it into interactive forms.
  /// </summary>
  /// <param name="forms">Interactive forms.</param>
  /// <param name="name">The partial field name. Cannot contain a period.</param>
  /// <param name="parent">The parent field. Only non-terminal fields are accepted.</param>
  public PdfSignatureField(PdfInteractiveForms forms, string name = null, PdfField parent = null)
    : base(forms, name, parent)
  {
    this.Dictionary["FT"] = (PdfTypeBase) PdfTypeName.Create("Sig");
  }
}
