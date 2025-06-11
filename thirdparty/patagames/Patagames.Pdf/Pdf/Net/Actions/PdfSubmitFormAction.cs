// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Actions.PdfSubmitFormAction
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Wrappers;
using Patagames.Pdf.Properties;
using System;

#nullable disable
namespace Patagames.Pdf.Net.Actions;

/// <summary>
/// A submit-form action transmits the names and values of selected interactive form fields to a specified
/// uniform resource locator(URL), presumably the address of a Web server that will process them and send back a response.
/// </summary>
public class PdfSubmitFormAction : PdfAction
{
  private PdfIndirectList _list;
  private PdfFileSpecification _fileSpec;
  private PdfField[] _fields;
  private string[] _names;

  /// <summary>
  /// Gets or sets a file specification described the file associated with this action.
  /// </summary>
  private PdfFileSpecification _fileSpecification
  {
    get
    {
      if (!this.Dictionary.ContainsKey("F"))
        return (PdfFileSpecification) null;
      if ((PdfWrapper) this._fileSpec == (PdfWrapper) null || this._fileSpec.Dictionary.IsDisposed)
        this._fileSpec = this.Dictionary["F"].Is<PdfTypeDictionary>() ? new PdfFileSpecification(this.Document, this.Dictionary["F"]) : (PdfFileSpecification) null;
      return this._fileSpec;
    }
    set
    {
      if ((PdfWrapper) value == (PdfWrapper) null && this.Dictionary.ContainsKey("F"))
        this.Dictionary.Remove("F");
      else if ((PdfWrapper) value != (PdfWrapper) null)
      {
        if (value.Dictionary.ObjectNumber == 0 && Pdfium.FPDFOBJ_GetParentObj(value.Dictionary.Handle) != IntPtr.Zero)
          throw new ArgumentException(string.Format(Error.err0067, (object) "file specification", (object) "object"));
        this._list.Add((PdfTypeBase) value.Dictionary);
        this.Dictionary.SetIndirectAt("F", this._list, (PdfTypeBase) value.Dictionary);
      }
      this._fileSpec = value;
    }
  }

  /// <summary>
  /// Gets or sets the uniform resource locator (URL) of the script at the Web server that will process the submission.
  /// </summary>
  public string Url
  {
    get
    {
      return !((PdfWrapper) this._fileSpecification != (PdfWrapper) null) ? (string) null : this._fileSpecification.FileName;
    }
    set
    {
      if (value == null)
      {
        this._fileSpecification = (PdfFileSpecification) null;
      }
      else
      {
        if ((PdfWrapper) this._fileSpecification == (PdfWrapper) null)
          this._fileSpecification = new PdfFileSpecification(this.Document);
        this._fileSpecification.IsUrl = true;
        this._fileSpecification.FileName = value;
      }
    }
  }

  /// <summary>
  ///  Gets or sets a set of flags specifying various characteristics of the action. See <see cref="T:Patagames.Pdf.Enums.SubmitFormFlags" />.
  /// </summary>
  public SubmitFormFlags Flags
  {
    get
    {
      return !this.Dictionary.ContainsKey(nameof (Flags)) ? SubmitFormFlags.None : (SubmitFormFlags) this.Dictionary[nameof (Flags)].As<PdfTypeNumber>().IntValue;
    }
    set
    {
      if (value == SubmitFormFlags.None && this.Dictionary.ContainsKey(nameof (Flags)))
      {
        this.Dictionary.Remove(nameof (Flags));
      }
      else
      {
        if (value == SubmitFormFlags.None)
          return;
        this.Dictionary[nameof (Flags)] = (PdfTypeBase) PdfTypeNumber.Create((int) value);
      }
    }
  }

  /// <summary>
  /// Gets or sets an array identifying which fields to include in the submission or which to exclude,
  /// depending on the setting of the <see cref="F:Patagames.Pdf.Enums.SubmitFormFlags.IncludeExclude" /> flag in the <see cref="P:Patagames.Pdf.Net.Actions.PdfSubmitFormAction.Flags" /> property.
  /// </summary>
  /// <remarks>
  /// <para>If this entry is omitted, the <see cref="F:Patagames.Pdf.Enums.SubmitFormFlags.IncludeExclude" /> flag is ignored, and all
  /// fields in the document’s interactive form are submitted except those
  /// whose <see cref="F:Patagames.Pdf.Enums.FieldFlags.NoExport" /> flag is set.</para>
  /// <para>Fields with no values may also be excluded, depending on the setting of
  /// the <see cref="F:Patagames.Pdf.Enums.SubmitFormFlags.IncludeNoValueFields" /> flag.</para>
  /// </remarks>
  public PdfField[] Fields
  {
    get
    {
      if (!this.Dictionary.ContainsKey(nameof (Fields)))
        return (PdfField[]) null;
      if (!this.Dictionary[nameof (Fields)].Is<PdfTypeArray>())
        return (PdfField[]) null;
      if (this._fields == null)
      {
        if (this.Document.FormFill == null)
          throw new ArgumentNullException("Document.FormFill");
        this.CreateInternalArrays();
      }
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
            if (value[index].Dictionary.ObjectNumber == 0 && Pdfium.FPDFOBJ_GetParentObj(value[index].Dictionary.Handle) != IntPtr.Zero)
              throw new ArgumentException(string.Format(Error.err0067, (object) $"field at {index.ToString()} pos", (object) "object"));
            this._list.Add((PdfTypeBase) value[index].Dictionary);
            if (pdfTypeArray == null)
              pdfTypeArray = PdfTypeArray.Create();
            pdfTypeArray.Add((PdfTypeBase) value[index].Dictionary, this._list);
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
  /// Gets or sets an array of a text string representing the fully qualified name of a field.
  /// </summary>
  /// <remarks>
  /// <para>If this entry is omitted, the <see cref="F:Patagames.Pdf.Enums.SubmitFormFlags.IncludeExclude" /> flag is ignored, and all
  /// fields in the document’s interactive form are submitted except those
  /// whose <see cref="F:Patagames.Pdf.Enums.FieldFlags.NoExport" /> flag is set.</para>
  /// <para>Fields with no values may also be excluded, depending on the setting of
  /// the <see cref="F:Patagames.Pdf.Enums.SubmitFormFlags.IncludeNoValueFields" /> flag.</para>
  /// </remarks>
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
            pdfTypeArray.Add((PdfTypeBase) PdfTypeString.Create(value[index], true, true));
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
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Actions.PdfSubmitFormAction" /> class with the url and an array of fields.
  /// </summary>
  /// <param name="document">Document which contains this action.</param>
  /// <param name="url">The uniform resource locator (URL) of the script at the Web server that will process the submission.</param>
  /// <param name="fields">An array identifying which fields to include in the submission or which to exclude, depending on the setting of the <see cref="F:Patagames.Pdf.Enums.SubmitFormFlags.IncludeExclude" /> flag in the <see cref="P:Patagames.Pdf.Net.Actions.PdfSubmitFormAction.Flags" /> property.</param>
  public PdfSubmitFormAction(PdfDocument document, string url, PdfField[] fields = null)
    : base(document, PdfTypeDictionary.Create().Handle)
  {
    if (url == null)
      throw new ArgumentNullException(nameof (url));
    this._list = PdfIndirectList.FromPdfDocument(this.Document);
    this.Dictionary["Type"] = (PdfTypeBase) PdfTypeName.Create("Action");
    this.Dictionary["S"] = (PdfTypeBase) PdfTypeName.Create("SubmitForm");
    this.Url = url;
    this.Fields = fields;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Actions.PdfSubmitFormAction" /> class with the url and an array of field names.
  /// </summary>
  /// <param name="document">Document which contains this action.</param>
  /// <param name="url">The uniform resource locator (URL) of the script at the Web server that will process the submission.</param>
  /// <param name="names">An array of a text string representing the fully qualified name of a field.</param>
  public PdfSubmitFormAction(PdfDocument document, string url, string[] names = null)
    : base(document, PdfTypeDictionary.Create().Handle)
  {
    if (url == null)
      throw new ArgumentNullException(nameof (url));
    this._list = PdfIndirectList.FromPdfDocument(this.Document);
    this.Dictionary["Type"] = (PdfTypeBase) PdfTypeName.Create("Action");
    this.Dictionary["S"] = (PdfTypeBase) PdfTypeName.Create("SubmitForm");
    this.Url = url;
    this.Names = names;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Actions.PdfSubmitFormAction" /> class with the url and an array of field names.
  /// </summary>
  /// <param name="document">Document which contains this action.</param>
  /// <param name="url">The uniform resource locator (URL) of the script at the Web server that will process the submission.</param>
  public PdfSubmitFormAction(PdfDocument document, string url)
    : this(document, url, (string[]) null)
  {
  }

  /// <summary>Initializes a new instance of the PdfAction class.</summary>
  /// <param name="document">Document which contains this action.</param>
  /// <param name="handle">Pdfium SDK handle that the action is bound to</param>
  public PdfSubmitFormAction(PdfDocument document, IntPtr handle)
    : base(document, handle)
  {
    this._list = PdfIndirectList.FromPdfDocument(this.Document);
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
        PdfField fieldByDict = this.Document.FormFill != null ? this.Document.FormFill.InterForm.Fields.GetFieldByDict(pdfTypeArray[index].As<PdfTypeDictionary>()) : (PdfField) null;
        this._fields[index] = fieldByDict;
        this._names[index] = fieldByDict?.FullName;
      }
      else if (pdfTypeArray[index].Is<PdfTypeString>())
      {
        this._names[index] = pdfTypeArray[index].As<PdfTypeString>().UnicodeString;
        if (this.Document.FormFill != null)
        {
          IntPtr field = Pdfium.FPDFInterForm_GetField(this.Document.FormFill.InterForm.Handle, 0, this._names[index]);
          this._fields[index] = this.Document.FormFill.InterForm.Fields.GetByHandle(field);
        }
      }
    }
  }
}
