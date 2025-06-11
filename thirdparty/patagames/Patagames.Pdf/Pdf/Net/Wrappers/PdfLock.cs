// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Wrappers.PdfLock
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Exceptions;
using Patagames.Pdf.Properties;
using System;

#nullable disable
namespace Patagames.Pdf.Net.Wrappers;

/// <summary>
/// A signature field lock dictionary that specifies a set of form fields to be locked when this signature field is signed.
/// </summary>
public class PdfLock : PdfWrapper
{
  private PdfField[] _fields;
  private string[] _names;
  private PdfInteractiveForms _inteforms;

  /// <summary>
  /// Gets or sets a value which, in conjunction with <see cref="P:Patagames.Pdf.Net.Wrappers.PdfLock.Fields" />, indicates the set of fields that should be locked.
  /// </summary>
  public SignatureActions Action
  {
    get
    {
      if (!this.IsExists(nameof (Action)))
        return SignatureActions.All;
      SignatureActions result;
      if (Pdfium.GetEnumDescription<SignatureActions>(this.Dictionary[nameof (Action)].As<PdfTypeName>().Value, out result))
        return result;
      throw new PdfParserException(string.Format(Error.err0045, (object) nameof (Action)));
    }
    set
    {
      string enumDescription = Pdfium.GetEnumDescription((Enum) value);
      this.Dictionary[nameof (Action)] = !((enumDescription ?? "").Trim() == "") ? (PdfTypeBase) PdfTypeName.Create(enumDescription) : throw new ArgumentException(string.Format(Error.err0047, (object) nameof (Action), (object) "are SignatureActions.All, SignatureActions.Include, and SignatureActions.Exclude"));
    }
  }

  /// <summary>
  /// Gets or sets an array identifying which fields to include or which to exclude,
  /// depending on the setting of the <see cref="P:Patagames.Pdf.Net.Wrappers.PdfLock.Action" /> property.
  /// </summary>
  /// <value>Required if the value of <see cref="P:Patagames.Pdf.Net.Wrappers.PdfLock.Action" /> is <see cref="F:Patagames.Pdf.Enums.SignatureActions.Include" /> or <see cref="F:Patagames.Pdf.Enums.SignatureActions.Exclude" />.</value>
  public PdfField[] Fields
  {
    get
    {
      if (!this.Dictionary.ContainsKey(nameof (Fields)))
        return (PdfField[]) null;
      if (!this.Dictionary[nameof (Fields)].Is<PdfTypeArray>())
        return (PdfField[]) null;
      if (this._fields == null)
        this.CreateInternalArrays();
      return this._fields;
    }
    set
    {
      if ((value == null || value.Length == 0) && this.Dictionary.ContainsKey(nameof (Fields)))
        this.Dictionary.Remove(nameof (Fields));
      else if (value != null && value.Length != 0)
      {
        PdfTypeArray pdfTypeArray = (PdfTypeArray) null;
        for (int index = 0; index < value.Length; ++index)
        {
          if (value[index] != null)
          {
            if (pdfTypeArray == null)
              pdfTypeArray = PdfTypeArray.Create();
            pdfTypeArray.Add((PdfTypeBase) PdfTypeString.Create(value[index].FullName, true));
          }
        }
        if (pdfTypeArray != null)
          this.Dictionary[nameof (Fields)] = (PdfTypeBase) pdfTypeArray;
      }
      this._fields = value;
      this._names = (string[]) null;
    }
  }

  /// <summary>
  /// Gets or sets an array of text strings containing field <see cref="P:Patagames.Pdf.Net.PdfField.FullName" />.
  /// </summary>
  /// <value>Required if the value of <see cref="P:Patagames.Pdf.Net.Wrappers.PdfLock.Action" /> is <see cref="F:Patagames.Pdf.Enums.SignatureActions.Include" /> or <see cref="F:Patagames.Pdf.Enums.SignatureActions.Exclude" />.</value>
  public string[] Names
  {
    get
    {
      if (!this.Dictionary.ContainsKey("Fields"))
        return (string[]) null;
      if (!this.Dictionary["Fields"].Is<PdfTypeArray>())
        return (string[]) null;
      if (this._names == null)
        this.CreateInternalArrays();
      return this._names;
    }
    set
    {
      if ((value == null || value.Length == 0) && this.Dictionary.ContainsKey("Fields"))
        this.Dictionary.Remove("Fields");
      else if (value != null && value.Length != 0)
      {
        PdfTypeArray pdfTypeArray = (PdfTypeArray) null;
        for (int index = 0; index < value.Length; ++index)
        {
          if (value[index] != null)
          {
            if (pdfTypeArray == null)
              pdfTypeArray = PdfTypeArray.Create();
            pdfTypeArray.Add((PdfTypeBase) PdfTypeString.Create(value[index], true));
          }
        }
        if (pdfTypeArray != null)
          this.Dictionary["Fields"] = (PdfTypeBase) pdfTypeArray;
      }
      this._fields = (PdfField[]) null;
      this._names = value;
    }
  }

  /// <summary>
  /// Creates new instance of <see cref="T:Patagames.Pdf.Net.Wrappers.PdfLock" />.
  /// </summary>
  public PdfLock(PdfInteractiveForms interfomrs)
  {
    this.Dictionary["Type"] = (PdfTypeBase) PdfTypeName.Create("SigFieldLock");
    this._inteforms = interfomrs;
  }

  /// <summary>
  /// Creates a new instance of <see cref="T:Patagames.Pdf.Net.Wrappers.PdfLock" /> and initialize it with specified dictionary
  /// </summary>
  /// <param name="interfomrs">Interactive forms.</param>
  /// <param name="dictionary">The dictionary or indirect dictionary</param>
  public PdfLock(PdfInteractiveForms interfomrs, PdfTypeBase dictionary)
    : base(dictionary)
  {
    this._inteforms = interfomrs;
  }

  private void CreateInternalArrays()
  {
    PdfTypeArray pdfTypeArray = this.Dictionary["Fields"].As<PdfTypeArray>();
    this._fields = new PdfField[pdfTypeArray.Count];
    this._names = new string[pdfTypeArray.Count];
    for (int index = 0; index < this._fields.Length; ++index)
    {
      if (pdfTypeArray[index].Is<PdfTypeDictionary>())
      {
        this._fields[index] = this._inteforms.Fields.GetFieldByDict(pdfTypeArray[index].As<PdfTypeDictionary>());
        this._names[index] = this._fields[index] != null ? this._fields[index].FullName : (string) null;
      }
      else if (pdfTypeArray[index].Is<PdfTypeString>())
      {
        this._names[index] = pdfTypeArray[index].As<PdfTypeString>().UnicodeString;
        IntPtr field = Pdfium.FPDFInterForm_GetField(this._inteforms.Handle, 0, this._names[index]);
        this._fields[index] = this._inteforms.Fields.GetByHandle(field);
      }
    }
  }
}
