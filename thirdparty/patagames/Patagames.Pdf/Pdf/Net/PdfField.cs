// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfField
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.AcroForms;
using Patagames.Pdf.Net.Actions;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Exceptions;
using Patagames.Pdf.Net.Wrappers;
using Patagames.Pdf.Properties;
using System;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>
/// Represents a field for gathering information interactively from the user.
/// </summary>
public class PdfField
{
  private PdfInteractiveForms _forms;
  private PdfTypeDictionary _dictionary;
  private PdfFieldLevelActions _aactions;

  /// <summary>
  /// Gets an Interactive Forms object that contains this field.
  /// </summary>
  public PdfInteractiveForms InterForms => this._forms;

  /// <summary>Gets form's notification state</summary>
  internal bool IsNotify => this._forms.IsNotify;

  /// <summary>
  /// Gets the Pdfium SDK handle that the field is bound to.
  /// </summary>
  public IntPtr Handle { get; private set; }

  /// <summary>Gets collection of controls assigned to this field.</summary>
  public PdfControlCollections Controls { get; private set; }

  /// <summary>Gets the fully qualified field name.</summary>
  /// <remarks>
  /// <para>
  /// The fully qualified field name is not explicitly defined but is constructed from the partial field names of the field and all of its
  /// ancestors. For a field with no parent, the partial and fully qualified names are the same. For a field that is the child of another field, the fully qualified name is
  /// formed by appending the child field’s partial name to the parent’s fully qualified name, separated by a period (.): parent’s_full_name.child’s_partial_name
  /// </para>
  /// </remarks>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.DomException">Generated for newly created fields that have not yet been added to the DOM.</exception>
  public string FullName
  {
    get
    {
      return !(this.Handle == IntPtr.Zero) ? Pdfium.FPDFFormField_GetFullName(this.Handle) : throw new DomException(this);
    }
  }

  /// <summary>Gets or sets alternate field name.</summary>
  /// <remarks> <para>An alternate field name to be used in place of the actual field name wherever the field must be identified in the user
  /// interface (such as in error or status messages referring to the field). This text is also useful when
  /// extracting the document’s contents in support of accessibility to users with
  /// disabilities or for other purposes.</para>
  /// <para>Partial field names cannot contain a period.</para>
  /// </remarks>
  public string AlternateName
  {
    get
    {
      if (this.Handle != IntPtr.Zero)
        return Pdfium.FPDFFormField_GetAlternateName(this.Handle);
      PdfTypeBase fieldAttribute = this.GetFieldAttribute("TU");
      return fieldAttribute == null || !fieldAttribute.Is<PdfTypeString>() ? (string) null : fieldAttribute.As<PdfTypeString>().UnicodeString;
    }
    set
    {
      if (value == null && this.Dictionary.ContainsKey("TU"))
      {
        this.Dictionary.Remove("TU");
      }
      else
      {
        if (value == null)
          return;
        this.Dictionary["TU"] = (PdfTypeBase) PdfTypeString.Create(value, true);
      }
    }
  }

  /// <summary>Gets or sets mapping field name.</summary>
  /// <remarks><para>The mapping name to be used when exporting interactive form field data from the document.</para>
  /// <para>Partial field names cannot contain a period.</para>
  /// </remarks>
  public string MappingName
  {
    get
    {
      if (this.Handle != IntPtr.Zero)
        return Pdfium.FPDFFormField_GetMappingName(this.Handle);
      PdfTypeBase fieldAttribute = this.GetFieldAttribute("TM");
      return fieldAttribute == null || !fieldAttribute.Is<PdfTypeString>() ? (string) null : fieldAttribute.As<PdfTypeString>().UnicodeString;
    }
    set
    {
      if (value == null && this.Dictionary.ContainsKey("TM"))
      {
        this.Dictionary.Remove("TM");
      }
      else
      {
        if (value == null)
          return;
        this.Dictionary["TM"] = (PdfTypeBase) PdfTypeString.Create(value, true);
      }
    }
  }

  /// <summary>Gets or sets a default style string</summary>
  /// <remarks>
  /// Beginning with PDF 1.5, the text contents of variable text form fields, as well as
  /// markup annotations, can include formatting (style) information. These rich text
  /// strings are fully-formed XML documents that conform to the rich text conventions
  /// specified for the XML Forms Architecture (XFA) specification, which is itself
  /// a subset of the XHTML 1.0 specification, augmented with a restricted set of
  /// CSS2 style attributes.
  /// </remarks>
  public string DefaultStyle
  {
    get
    {
      if (this.Handle != IntPtr.Zero)
        return Pdfium.FPDFFormField_GetDefaultStyle(this.Handle);
      PdfTypeBase fieldAttribute = this.GetFieldAttribute("DS");
      return fieldAttribute == null || !fieldAttribute.Is<PdfTypeString>() ? (string) null : fieldAttribute.As<PdfTypeString>().UnicodeString;
    }
    set
    {
      if (value == null && this.Dictionary.ContainsKey("DS"))
      {
        this.Dictionary.Remove("DS");
      }
      else
      {
        if (value == null)
          return;
        this.Dictionary["DS"] = (PdfTypeBase) PdfTypeString.Create(value, true);
      }
    }
  }

  /// <summary>Gets or Sets field's value</summary>
  /// <remarks>
  /// Make sure the field does not have input focus before setting the value. If the field has input focus, setting the value has no effect.
  /// In order to remove the input focus, call the <see cref="M:Patagames.Pdf.Net.PdfForms.ForceToKillFocus" /> method.
  /// </remarks>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.DomException">Generated for newly created fields that have not yet been added to the DOM.</exception>
  public string Value
  {
    get
    {
      return !(this.Handle == IntPtr.Zero) ? Pdfium.FPDFFormField_GetValue(this.Handle) : throw new DomException(this);
    }
    set
    {
      if (this.Handle == IntPtr.Zero)
        throw new DomException(this);
      Pdfium.FPDFFormField_SetValue(this.Handle, value, this.IsNotify);
    }
  }

  /// <summary>Gets or sets the default value of the field.</summary>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.DomException">Generated for newly created fields that have not yet been added to the DOM.</exception>
  public string DefaultValue
  {
    get
    {
      return !(this.Handle == IntPtr.Zero) ? Pdfium.FPDFFormField_GetDefaultValue(this.Handle) : throw new DomException(this);
    }
    set
    {
      if (this.Handle == IntPtr.Zero)
        throw new DomException(this);
      Pdfium.FPDFFormField_SetDefaultValue(this.Handle, value, this.IsNotify);
    }
  }

  /// <summary>Gets the type of field</summary>
  public FormFieldTypesEx Type
  {
    get
    {
      if (this.Handle != IntPtr.Zero)
        return Pdfium.FPDFFormField_GetType(this.Handle);
      switch (this)
      {
        case PdfCheckBoxField _:
          return FormFieldTypesEx.CheckBox;
        case PdfRadioButtonField _:
          return FormFieldTypesEx.RadioButton;
        case PdfPushButtonField _:
          return FormFieldTypesEx.PushButton;
        case PdfListBoxField _:
          return FormFieldTypesEx.ListBox;
        case PdfComboBoxField _:
          return FormFieldTypesEx.ComboBox;
        case PdfTextBoxField _ when (this as PdfTextBoxField).IsRichText:
          return FormFieldTypesEx.RichText;
        case PdfTextBoxField _ when (this as PdfTextBoxField).FileSelect:
          return FormFieldTypesEx.File;
        case PdfTextBoxField _:
          return FormFieldTypesEx.Text;
        case PdfSignatureField _:
          return FormFieldTypesEx.Sign;
        default:
          return FormFieldTypesEx.Unknown;
      }
    }
  }

  /// <summary>Gets the type of field</summary>
  public FormFieldTypes FieldType
  {
    get
    {
      if (this.Handle != IntPtr.Zero)
        return Pdfium.FPDFFormField_GetFieldType(this.Handle);
      switch (this)
      {
        case PdfCheckBoxField _:
          return FormFieldTypes.FPDF_FORMFIELD_CHECKBOX;
        case PdfRadioButtonField _:
          return FormFieldTypes.FPDF_FORMFIELD_RADIOBUTTON;
        case PdfPushButtonField _:
          return FormFieldTypes.FPDF_FORMFIELD_PUSHBUTTON;
        case PdfListBoxField _:
          return FormFieldTypes.FPDF_FORMFIELD_LISTBOX;
        case PdfComboBoxField _:
          return FormFieldTypes.FPDF_FORMFIELD_COMBOBOX;
        case PdfTextBoxField _:
          return FormFieldTypes.FPDF_FORMFIELD_TEXTFIELD;
        case PdfSignatureField _:
          return FormFieldTypes.FPDF_FORMFIELD_SIGNATURE;
        default:
          return FormFieldTypes.FPDF_FORMFIELD_UNKNOWN;
      }
    }
  }

  /// <summary>Gets field flags</summary>
  public FieldFlagsEx FlagsEx
  {
    get
    {
      if (this.Handle != IntPtr.Zero)
        return Pdfium.FPDFFormField_GetFieldFlags(this.Handle);
      return this.Dictionary != null && this.Dictionary.ContainsKey("Ff") && this.Dictionary["Ff"].Is<PdfTypeNumber>() ? (FieldFlagsEx) this.Dictionary["Ff"].As<PdfTypeNumber>().IntValue : (FieldFlagsEx) 0;
    }
  }

  /// <summary>Gets extended field's flags</summary>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.DomException">Generated for newly created fields that have not yet been added to the DOM.</exception>
  public FieldFlags Flags
  {
    get
    {
      return !(this.Handle == IntPtr.Zero) ? Pdfium.FPDFFormField_GetFlags(this.Handle) : throw new DomException(this);
    }
  }

  /// <summary>Gets the field's dictionary</summary>
  public PdfTypeDictionary Dictionary
  {
    get
    {
      if (this.Handle == IntPtr.Zero && this._dictionary != null)
        return this._dictionary;
      if (this.Handle == IntPtr.Zero)
        return (PdfTypeDictionary) null;
      if (this._dictionary != null && !this._dictionary.IsDisposed)
        return this._dictionary;
      IntPtr fieldDict = Pdfium.FPDFFormField_GetFieldDict(this.Handle);
      if (fieldDict == IntPtr.Zero)
        return (PdfTypeDictionary) null;
      this._dictionary = new PdfTypeDictionary(fieldDict);
      return this._dictionary;
    }
  }

  /// <summary>
  /// Gets or sets a flag indicating whether the field is readonly.
  /// </summary>
  /// <value>If true, the user may not change the value of the field.</value>
  public bool ReadOnly
  {
    get => (this.FlagsEx & FieldFlagsEx.ReadOnly) == FieldFlagsEx.ReadOnly;
    set
    {
      if (value)
        this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create((int) (this.FlagsEx | FieldFlagsEx.ReadOnly));
      else
        this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create((int) (this.FlagsEx & ~FieldFlagsEx.ReadOnly));
    }
  }

  /// <summary>
  /// Gets or sets a flag indicating whether the field must have an export value.
  /// </summary>
  /// <value>
  /// If true, the field must have a value at the time it is exported by a submit-form action.
  /// </value>
  public bool Required
  {
    get => (this.FlagsEx & FieldFlagsEx.Required) == FieldFlagsEx.Required;
    set
    {
      if (value)
        this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create((int) (this.FlagsEx | FieldFlagsEx.Required));
      else
        this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create((int) (this.FlagsEx & ~FieldFlagsEx.Required));
    }
  }

  /// <summary>
  /// Gets or sets a flag indicating whether the field must be exported by a <see cref="T:Patagames.Pdf.Net.Actions.PdfSubmitFormAction" />.
  /// </summary>
  /// <value>
  /// If true, the field must not be exported by a submit-form action.
  /// </value>
  public bool NoExport
  {
    get => (this.FlagsEx & FieldFlagsEx.NoExport) == FieldFlagsEx.NoExport;
    set
    {
      if (value)
        this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create((int) (this.FlagsEx | FieldFlagsEx.NoExport));
      else
        this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create((int) (this.FlagsEx & ~FieldFlagsEx.NoExport));
    }
  }

  /// <summary>
  /// Gets or sets the additional-actions defining the field's behavior in response to various trigger events.
  /// </summary>
  public PdfFieldLevelActions AdditionalActions
  {
    get
    {
      if (!this.Dictionary.ContainsKey("AA"))
      {
        this._aactions = (PdfFieldLevelActions) null;
        return (PdfFieldLevelActions) null;
      }
      if ((PdfWrapper) this._aactions == (PdfWrapper) null || this.Dictionary["AA"].Is<PdfTypeDictionary>() && this._aactions.Dictionary.Handle != this.Dictionary["AA"].As<PdfTypeDictionary>().Handle)
        this._aactions = new PdfFieldLevelActions(this.InterForms.FillForms.Document, (PdfTypeBase) this.Dictionary["AA"].As<PdfTypeDictionary>());
      return this._aactions;
    }
    set
    {
      if ((PdfWrapper) value == (PdfWrapper) null && this.Dictionary.ContainsKey("AA"))
        this.Dictionary.Remove("AA");
      else if ((PdfWrapper) value != (PdfWrapper) null)
      {
        if (value.Dictionary.ObjectNumber == 0 && Pdfium.FPDFOBJ_GetParentObj(value.Dictionary.Handle) != IntPtr.Zero)
          throw new ArgumentException(string.Format(Error.err0067, (object) "additional actions", (object) "object"));
        PdfIndirectList list = PdfIndirectList.FromPdfDocument(this.InterForms.FillForms.Document);
        list.Add((PdfTypeBase) value.Dictionary);
        this.Dictionary.SetIndirectAt("AA", list, (PdfTypeBase) value.Dictionary);
      }
      this._aactions = value;
    }
  }

  /// <summary>Initializes a new instance of the PdfField class.</summary>
  /// <param name="forms">Interactive forms</param>
  /// <param name="handle">Handle to the field wich will be assigned</param>
  internal PdfField(PdfInteractiveForms forms, IntPtr handle)
  {
    this._forms = forms;
    this.Handle = handle;
    this.Controls = new PdfControlCollections(this._forms, this);
  }

  /// <summary>The base constructor for creating new AcroForm field</summary>
  /// <param name="forms">Interactive forms.</param>
  /// <param name="name">The partial field name. Cannot contain a period.</param>
  /// <param name="parent">The parent field. Only non-terminal fields are accepted.</param>
  /// <param name="defFont">Default font used for all controls assigned with this field.</param>
  /// <param name="fontSize">Default font's size.</param>
  /// <param name="captionColor">The default button caption color used when the control does not specify a title color.</param>
  protected PdfField(
    PdfInteractiveForms forms,
    string name = null,
    PdfField parent = null,
    PdfFont defFont = null,
    float fontSize = 0.0f,
    FS_COLOR? captionColor = null)
  {
    this._forms = forms != null ? forms : throw new ArgumentNullException(nameof (forms));
    this.Controls = new PdfControlCollections(this._forms, this);
    if ((name ?? "").Trim() == "")
      name = Guid.NewGuid().ToString();
    name = name.Replace(".", "");
    PdfIndirectList list = PdfIndirectList.FromPdfDocument(forms.FillForms.Document);
    this._dictionary = PdfTypeDictionary.Create();
    this._dictionary["T"] = (PdfTypeBase) PdfTypeString.Create(name, true);
    list.Add((PdfTypeBase) this._dictionary);
    if (parent != null && parent.IsTerminalField())
      throw new ArgumentException(Error.err0068);
    if (parent != null)
    {
      if (!parent.Dictionary.ContainsKey("Kids"))
        parent.Dictionary["Kids"] = (PdfTypeBase) PdfTypeArray.Create();
      parent.Dictionary["Kids"].As<PdfTypeArray>().AddIndirect(list, (PdfTypeBase) this._dictionary);
      if (parent.Dictionary.ObjectNumber == 0 && Pdfium.FPDFOBJ_GetParentObj(parent.Dictionary.Handle) != IntPtr.Zero)
        throw new ArgumentException(string.Format(Error.err0067, (object) "parent field", (object) "object"));
      list.Add((PdfTypeBase) parent.Dictionary);
      this._dictionary.SetIndirectAt("Parent", list, (PdfTypeBase) parent.Dictionary);
    }
    bool isDaRequired = this.GetDefaultAppearance(checkAcroFormsDictionary: false) == "" && (this is PdfPushButtonField || this is PdfTextBoxField);
    this.SetDefaultAppearance(this._dictionary, defFont, fontSize, captionColor, isDaRequired);
  }

  /// <summary>
  /// Create new instance of a field which type is depend on field's type.
  /// </summary>
  /// <param name="interForm">Interactive forms.</param>
  /// <param name="handle">The field's handle.</param>
  /// <returns>An instance of one of the child classes, depending on the type of the field, or an instance of the base class if there was a problem with the type definition.</returns>
  internal static PdfField Create(PdfInteractiveForms interForm, IntPtr handle)
  {
    switch (Pdfium.FPDFFormField_GetFieldType(handle))
    {
      case FormFieldTypes.FPDF_FORMFIELD_PUSHBUTTON:
        return (PdfField) new PdfPushButtonField(interForm, handle);
      case FormFieldTypes.FPDF_FORMFIELD_CHECKBOX:
        return (PdfField) new PdfCheckBoxField(interForm, handle);
      case FormFieldTypes.FPDF_FORMFIELD_RADIOBUTTON:
        return (PdfField) new PdfRadioButtonField(interForm, handle);
      case FormFieldTypes.FPDF_FORMFIELD_COMBOBOX:
        return (PdfField) new PdfComboBoxField(interForm, handle);
      case FormFieldTypes.FPDF_FORMFIELD_LISTBOX:
        return (PdfField) new PdfListBoxField(interForm, handle);
      case FormFieldTypes.FPDF_FORMFIELD_TEXTFIELD:
        return (PdfField) new PdfTextBoxField(interForm, handle);
      case FormFieldTypes.FPDF_FORMFIELD_SIGNATURE:
        return (PdfField) new PdfSignatureField(interForm, handle);
      default:
        return new PdfField(interForm, handle);
    }
  }

  internal void UpdateHandle(IntPtr handle) => this.Handle = handle;

  /// <summary>
  /// Reset the appearance of all controls assigned to this field and mark them to be rebuilt on next page load.
  /// </summary>
  public void ResetAppearance()
  {
    foreach (PdfControl control in this.Controls)
      control.ResetAppearance();
  }

  /// <summary>
  /// Delete the existing appearance stream of all controls associated with this <see cref="T:Patagames.Pdf.Net.PdfField" /> and build a new one in accordance with the properties of the field and controls.
  /// </summary>
  public void RegenerateAppearance()
  {
    foreach (PdfControl control in this.Controls)
      control.RegenerateAppearance();
  }

  /// <summary>Checks if the given field is terminal.</summary>
  /// <returns>true if the field is terminla.</returns>
  /// <remarks>A field whose children are widget annotations is called a terminal field.
  /// As a convenience, when a field has only a single associated widget annotation, the
  /// contents of the field dictionary and the annotation dictionary may be merged into a single dictionary
  /// containing entries that pertain to both a field and an annotation.
  /// (This presents no ambiguity, since the contents of the two kinds of dictionaries do not conflict.)
  /// Such a field is also terminal.</remarks>
  public bool IsTerminalField()
  {
    if (this.Dictionary == null)
      return false;
    if (this.Dictionary.ContainsKey("Subtype") && this.Dictionary["Subtype"].Is<PdfTypeName>() && this.Dictionary["Subtype"].As<PdfTypeName>().Value == "Widget")
      return true;
    if (this.Dictionary.ContainsKey("Kids") && this.Dictionary["Kids"].Is<PdfTypeArray>())
    {
      PdfTypeArray pdfTypeArray = this.Dictionary["Kids"].As<PdfTypeArray>();
      if (pdfTypeArray.Count <= 0 || !pdfTypeArray[0].Is<PdfTypeDictionary>())
        return false;
      PdfTypeDictionary pdfTypeDictionary = pdfTypeArray[0].As<PdfTypeDictionary>();
      if (pdfTypeDictionary.ContainsKey("Subtype") && pdfTypeDictionary["Subtype"].Is<PdfTypeName>() && pdfTypeDictionary["Subtype"].As<PdfTypeName>().Value == "Widget")
        return true;
    }
    return false;
  }

  /// <summary>Resets field to it default values</summary>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.DomException">Generated for newly created fields that have not yet been added to the DOM.</exception>
  public void ResetField()
  {
    if (this.Handle == IntPtr.Zero)
      throw new DomException(this);
    Pdfium.FPDFFormField_ResetField(this.Handle, this.IsNotify);
  }

  /// <summary>Gets control's index in a field</summary>
  /// <param name="control">control object</param>
  /// <returns>Zero based index of a control in Controls collections. -1 if nothing found.</returns>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.DomException">Generated for newly created fields that have not yet been added to the DOM.</exception>
  public int GetControlIndex(PdfControl control)
  {
    if (this.Handle == IntPtr.Zero)
      throw new DomException(this);
    return Pdfium.FPDFFormField_GetControlIndex(this.Handle, control.Handle);
  }

  internal string GetDefaultAppearance(
    PdfTypeDictionary dict = null,
    int level = 0,
    bool checkAcroFormsDictionary = true)
  {
    if (level > 128 /*0x80*/)
      return "";
    if (dict == null)
      dict = this.Dictionary;
    if (dict.ContainsKey("DA") && dict["DA"].Is<PdfTypeString>())
      return dict["DA"].As<PdfTypeString>().UnicodeString;
    if (dict.ContainsKey("Parent") && dict["Parent"].Is<PdfTypeDictionary>())
      return this.GetDefaultAppearance(dict["Parent"].As<PdfTypeDictionary>(), level++);
    return checkAcroFormsDictionary && this.InterForms.Dictionary != null && this.InterForms.Dictionary.ContainsKey("DA") && this.InterForms.Dictionary["DA"].Is<PdfTypeString>() ? this.InterForms.Dictionary["DA"].As<PdfTypeString>().UnicodeString : "";
  }

  internal void SetDefaultAppearance(
    PdfTypeDictionary dict,
    PdfFont defFont,
    float fontSize,
    FS_COLOR? captionColor,
    bool isDaRequired)
  {
    if (((defFont != null ? 1 : (captionColor.HasValue ? 1 : 0)) | (isDaRequired ? 1 : 0)) == 0)
      return;
    float[] fillColor;
    string fontName;
    Pdfium.FPDFTOOLS_ParseDefaultAppearance(this.GetDefaultAppearance(), out float[] _, out fillColor, out fontName, out float _, out FS_MATRIX _);
    if (!captionColor.HasValue)
      captionColor = new FS_COLOR?(fillColor == null ? FS_COLOR.Black : new FS_COLOR(fillColor));
    if (defFont == null && fontName == null)
      defFont = PdfFont.CreateStock(this.InterForms.FillForms.Document, FontStockNames.Helvetica);
    if (defFont != null)
      fontName = this.InterForms.Fonts.Add(defFont);
    if (fontName == null)
      return;
    string[] strArray = new string[8]
    {
      $"{"/" + fontName} {fontSize.ToString("0.00").TrimEnd('0').TrimEnd('.')} Tf",
      " ",
      ((float) captionColor.Value.R / (float) byte.MaxValue).ToString(),
      " ",
      null,
      null,
      null,
      null
    };
    FS_COLOR fsColor = captionColor.Value;
    strArray[4] = ((float) fsColor.G / (float) byte.MaxValue).ToString();
    strArray[5] = " ";
    fsColor = captionColor.Value;
    strArray[6] = ((float) fsColor.B / (float) byte.MaxValue).ToString();
    strArray[7] = " rg";
    string initialVal = string.Concat(strArray).Trim();
    dict["DA"] = (PdfTypeBase) PdfTypeString.Create(initialVal);
  }

  internal PdfTypeBase GetFieldAttribute(string key, PdfTypeDictionary dict = null, int level = 0)
  {
    if (level > 128 /*0x80*/)
      return (PdfTypeBase) null;
    if (dict == null)
      dict = this.Dictionary;
    if (dict == null)
      return (PdfTypeBase) null;
    if (dict.ContainsKey(key))
      return dict[key];
    return dict.ContainsKey("Parent") && dict["Parent"].Is<PdfTypeDictionary>() ? this.GetFieldAttribute(key, dict["Parent"].As<PdfTypeDictionary>(), level++) : (PdfTypeBase) null;
  }
}
