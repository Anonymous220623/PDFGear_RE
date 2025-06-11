// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfInteractiveForms
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.BasicTypes;
using System;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>
/// Encapsulates a fields for gathering information interactively from the user.
/// </summary>
/// <remarks>
/// <para>
/// A PDF document may contain any number of fields appearing on any combination of
/// pages, all of which make up a single, global interactive form spanning the entire
/// document.
/// </para>
/// <para>
/// Each field in a document’s interactive form is defined by a <see cref="P:Patagames.Pdf.Net.PdfInteractiveForms.Fields" /> property.
/// A field may also include one or more widget annotations that define its appearance on the page.
/// </para>
/// <note type="note">
/// Interactive forms should not be confused with form XObjects.
/// Despite the similarity of names, the two are different, unrelated types of objects.
/// </note>
/// </remarks>
public class PdfInteractiveForms
{
  private PdfTypeDictionary _dictionary;
  internal PdfForms _fillforms;

  /// <summary>
  /// Gets the <see cref="T:Patagames.Pdf.Net.PdfForms" /> object containing acroforms.
  /// </summary>
  public PdfForms FillForms => this._fillforms;

  /// <summary>Gets or sets form's notification state</summary>
  internal bool IsNotify => true;

  /// <summary>
  /// Gets the Pdfium SDK handle that the interactive forms is bound to
  /// </summary>
  public IntPtr Handle { get; private set; }

  /// <summary>
  /// Gets collection of fields for gathering information interactively from the user.
  /// </summary>
  public PdfFieldCollections Fields { get; private set; }

  /// <summary>Gets collection of controls.</summary>
  public PdfControlCollections Controls { get; private set; }

  /// <summary>
  /// Gets the collection of fonts in an acroform resource dictionary.
  /// </summary>
  public PdfFontCollection Fonts { get; private set; }

  /// <summary>Determines that the document contains XFA Forms</summary>
  /// <remarks>
  /// PDF 1.5 introduces support for interactive forms based on the Adobe XML Forms Architecture (XFA). The XFA entry in the interactive forms dictionary specifies an XFA resource, which is an XML stream that contains the
  /// form information. The format of an XFA resource is described in the XML Data Package (XDP) Specification
  /// </remarks>
  public bool HasXFAForm => Pdfium.FPDFInterForm_HasXFAForm(this.Handle);

  /// <summary>Gets the default font for Acro forms</summary>
  public PdfFont DefaultFont
  {
    get
    {
      if (this.Handle == IntPtr.Zero)
        return (PdfFont) null;
      IntPtr defaultFormFont = Pdfium.FPDFInterForm_GetDefaultFormFont(this.Handle);
      return defaultFormFont == IntPtr.Zero ? (PdfFont) null : new PdfFont(defaultFormFont);
    }
  }

  /// <summary>Gets AcroForm's dictionary</summary>
  public PdfTypeDictionary Dictionary
  {
    get
    {
      if (this.Handle == IntPtr.Zero)
        return (PdfTypeDictionary) null;
      IntPtr formDict = Pdfium.FPDFInterForm_GetFormDict(this.Handle);
      if (formDict == IntPtr.Zero)
        return (PdfTypeDictionary) null;
      if (this._dictionary == null || this._dictionary.IsDisposed || this._dictionary.Handle != formDict)
        this._dictionary = new PdfTypeDictionary(formDict);
      return this._dictionary;
    }
  }

  /// <summary>
  /// Initializes a new instance of the PdfInteractiveForms class.
  /// </summary>
  internal PdfInteractiveForms(PdfForms forms)
  {
    this._fillforms = forms;
    this.Handle = Pdfium.FORM_GetInterForm(forms.Handle);
    if (this.Handle == IntPtr.Zero)
      throw Pdfium.ProcessLastError();
    this.Controls = new PdfControlCollections(this);
    this.Fields = new PdfFieldCollections(this);
    this.Fonts = new PdfFontCollection(this);
  }

  /// <summary>
  /// Determines that specified Field is a valid Field from the hierarchy of documents fields
  /// </summary>
  /// <param name="field">Field objects.</param>
  /// <returns>True if the specified Field is present in the list of fields of the document or False otherwise.</returns>
  public bool IsValidField(PdfField field)
  {
    return Pdfium.FPDFInterForm_IsValidFormField(this.Handle, field.Handle);
  }

  /// <summary>Determines that specified Control is a valid Control</summary>
  /// <param name="control">Control object.</param>
  /// <returns>True if the specified Field is present in the list of fields of the document or False otherwise.</returns>
  public bool IsValidControl(PdfControl control)
  {
    return Pdfium.FPDFInterForm_IsValidFormControl(this.Handle, control.Handle);
  }

  /// <summary>Gets collection of controls for specified page.</summary>
  /// <param name="page">PDF page object.</param>
  /// <returns>Collection of controls for specified page .</returns>
  public PdfControlCollections GetPageControls(PdfPage page)
  {
    return new PdfControlCollections(this, page);
  }

  /// <summary>Resets forms to their default values.</summary>
  /// <returns>True for successfull, false if action was canceled.</returns>
  public bool ResetForm() => Pdfium.FPDFInterForm_ResetForm(this.Handle, this.IsNotify);

  /// <summary>Fix page fields</summary>
  /// <param name="page">PDF page object.</param>
  /// <remarks>
  /// This method method reload all widget annotations on a page. You may need this if  you are changing the widget annotation through its dictionary.
  /// </remarks>
  public void FixPageFields(PdfPage page)
  {
    Pdfium.FPDFInterForm_FixPageFields(this.Handle, page.Handle);
  }

  /// <summary>Export fields into Forms Data Format (FDF) document</summary>
  /// <param name="pathToPdf">Path to the source file: the PDF document file that this FDF file was exported from.</param>
  /// <param name="isSimpleSpec">True for simple file specification, false otherwise</param>
  /// <returns>New FDF document</returns>
  /// <remarks>
  /// A PDF file can refer to the contents of another file by using a file specification (PDF 1.1), which can take either of two forms:
  /// <list type="bullet">
  /// <item>
  /// A simple file specification gives just the name of the target file in a standard format, independent of the naming conventions of any particular file system. It
  /// can take the form of either a string or a dictionary
  /// </item>
  /// <item>
  /// A full file specification includes information related to one or more specific file systems. It can only be represented as a dictionary.
  /// </item>
  /// </list>
  /// </remarks>
  public FdfDocument ExportToFdf(string pathToPdf, bool isSimpleSpec = false)
  {
    IntPtr fdf = Pdfium.FPDFInterForm_ExportToFDF(this.Handle, pathToPdf, isSimpleSpec);
    return fdf == IntPtr.Zero ? (FdfDocument) null : FdfDocument.FromHandle(fdf);
  }

  /// <summary>Import fields from Forms Data Format (FDF) document</summary>
  /// <param name="fdfDocument">A FDF Doument to import from.</param>
  /// <returns>true for successful, false if action was canceled</returns>
  public bool ImportFromFdf(FdfDocument fdfDocument)
  {
    return Pdfium.FPDFInterForm_ImportFromFDF(this.Handle, fdfDocument.Handle, true);
  }

  /// <summary>Gets the widget with the specified dictionary.</summary>
  /// <param name="dict">Widget annotaion's dictionary</param>
  /// <returns>Found widget or null if nothing found.</returns>
  public PdfControl GetControlByDict(PdfTypeDictionary dict)
  {
    return dict == null ? (PdfControl) null : this.Controls.GetByHandle(Pdfium.FPDFInterForm_GetControlByDict(this.Handle, dict.Handle));
  }

  /// <summary>Gets the Field with the specified dictionary.</summary>
  /// <param name="dict">Field's dictionary</param>
  /// <returns>Found field or null if nothing found.</returns>
  public PdfField GetFieldByDict(PdfTypeDictionary dict)
  {
    return dict == null ? (PdfField) null : this.Fields.GetByHandle(Pdfium.FPDFInterForm_GetFieldByDict(this.Handle, dict.Handle));
  }

  /// <summary>
  /// Reloads acroforms. Should be called after adding new fields and controls in order for them to appear in the corresponding collections.
  /// </summary>
  public void ReloadForms() => Pdfium.FPDFInterForm_ReloadForm(this.Handle);

  /// <summary>
  /// Set the font to the DA (default appearance) entry of acroforms dictionary.
  /// </summary>
  /// <param name="font">The font to set. if null Helvetica is used.</param>
  /// <param name="fontSize">The <paramref name="font" /> size. 0 means autosize.</param>
  /// <param name="color">The <paramref name="font" /> color. If null <see cref="P:Patagames.Pdf.FS_COLOR.Black" /> is used.</param>
  public void SetDefaultAppearance(PdfFont font = null, float fontSize = 0.0f, FS_COLOR? color = null)
  {
    if (font == null)
      font = PdfFont.CreateStock(this._fillforms.Document, FontStockNames.Helvetica);
    string str1 = this.Fonts.Add(font);
    if (!((str1 ?? "").Trim() != ""))
      return;
    if (this.Dictionary == null)
      Pdfium.FPDFInterForm_InitEmptyFormDict(this.Handle);
    string str2 = $"{"/" + str1} {fontSize.ToString("0.00").TrimEnd('0').TrimEnd('.')} Tf";
    string[] strArray = new string[8];
    strArray[0] = str2;
    strArray[1] = " ";
    FS_COLOR fsColor = color ?? FS_COLOR.Black;
    float num = (float) fsColor.R / (float) byte.MaxValue;
    strArray[2] = num.ToString();
    strArray[3] = " ";
    fsColor = color ?? FS_COLOR.Black;
    num = (float) fsColor.G / (float) byte.MaxValue;
    strArray[4] = num.ToString();
    strArray[5] = " ";
    fsColor = color ?? FS_COLOR.Black;
    num = (float) fsColor.B / (float) byte.MaxValue;
    strArray[6] = num.ToString();
    strArray[7] = " rg";
    this.Dictionary["DA"] = (PdfTypeBase) PdfTypeString.Create(string.Concat(strArray).Trim());
  }

  /// <summary>
  /// Checks if acroforms dictionary contain the valid default appearance value.
  /// </summary>
  /// <returns>False if the default appearance is missing or incorrect.</returns>
  public bool HasDefaultAppearance()
  {
    return this.GetDefaultAppearance(out FS_COLOR _, out FS_COLOR _, out PdfFont _, out float _, out FS_MATRIX _);
  }

  /// <summary>Get the default field's text size, colors and font.</summary>
  /// <param name="strokeColor">Stroke Color. Transparent if not specified/</param>
  /// <param name="fillColor">Fill color. Transparent if not specified.</param>
  /// <param name="font">Font.</param>
  /// <param name="fontSize">Font size. 0 - auto size.</param>
  /// <param name="matrix">Matrix.</param>
  /// <returns>False if the default appearance is missing or incorrect.</returns>
  public bool GetDefaultAppearance(
    out FS_COLOR strokeColor,
    out FS_COLOR fillColor,
    out PdfFont font,
    out float fontSize,
    out FS_MATRIX matrix)
  {
    strokeColor = FS_COLOR.Transparent;
    fillColor = FS_COLOR.Transparent;
    font = (PdfFont) null;
    fontSize = 0.0f;
    matrix = (FS_MATRIX) null;
    float[] strokeColor1;
    float[] fillColor1;
    string fontName;
    if (this.Dictionary == null || !this.Dictionary.ContainsKey("DA") || !this.Dictionary["DA"].Is<PdfTypeString>() || !Pdfium.FPDFTOOLS_ParseDefaultAppearance(this.Dictionary["DA"].As<PdfTypeString>().UnicodeString, out strokeColor1, out fillColor1, out fontName, out fontSize, out matrix))
      return false;
    if (strokeColor1 != null)
      strokeColor = new FS_COLOR(strokeColor1);
    if (fillColor1 != null)
      fillColor = new FS_COLOR(fillColor1);
    if (fontName != null)
    {
      if (Pdfium.FPDFInterForm_GetFormFont(this.Handle, fontName) == IntPtr.Zero)
        return false;
      font = this.Fonts[fontName];
    }
    return true;
  }

  /// <summary>
  /// Removes the first occurrence of a specific form field from the <see cref="P:Patagames.Pdf.Net.PdfInteractiveForms.Fields" />.
  /// </summary>
  /// <param name="field">The form field to remove from the <see cref="P:Patagames.Pdf.Net.PdfInteractiveForms.Fields" />.</param>
  /// <returns>true if <paramref name="field" /> was successfully removed from <see cref="P:Patagames.Pdf.Net.PdfInteractiveForms.Fields" />;
  /// otherwise, false. This method also returns false if item is not found in the  <see cref="P:Patagames.Pdf.Net.PdfInteractiveForms.Fields" /> collection.</returns>
  /// <remarks>For the changes to take effect, you need to call the <see cref="M:Patagames.Pdf.Net.PdfInteractiveForms.ReloadForms" /> method.</remarks>
  public bool RemoveField(PdfField field)
  {
    if (field == null)
      throw new ArgumentNullException(nameof (field));
    PdfTypeArray fieldsArray = (PdfTypeArray) null;
    int index = this.SearchFieldIndex(field.Dictionary, out fieldsArray);
    if (index < 0)
      return false;
    foreach (PdfControl control in field.Controls)
      control.RemoveFromDom();
    field.UpdateHandle(IntPtr.Zero);
    fieldsArray.RemoveAt(index);
    return true;
  }

  internal bool ContainsField(PdfTypeDictionary fieldDict)
  {
    return this.SearchFieldIndex(fieldDict, out PdfTypeArray _) >= 0;
  }

  internal int SearchFieldIndex(PdfTypeDictionary fieldDict, out PdfTypeArray fieldsArray)
  {
    fieldsArray = (PdfTypeArray) null;
    if (fieldDict == null)
      return -1;
    if (fieldDict.ContainsKey("Parent") && fieldDict["Parent"].Is<PdfTypeDictionary>())
    {
      PdfTypeDictionary pdfTypeDictionary = fieldDict["Parent"].As<PdfTypeDictionary>();
      if (pdfTypeDictionary.ContainsKey("Kids") && pdfTypeDictionary["Kids"].Is<PdfTypeArray>())
        fieldsArray = pdfTypeDictionary["Kids"].As<PdfTypeArray>();
    }
    if (fieldsArray == null && this.Dictionary != null && this.Dictionary.ContainsKey("Fields") && this.Dictionary["Fields"].Is<PdfTypeArray>())
      fieldsArray = this.Dictionary["Fields"].As<PdfTypeArray>();
    if (fieldsArray != null)
    {
      for (int index = 0; index < fieldsArray.Count; ++index)
      {
        IntPtr directObjectAt = Pdfium.FPDFARRAY_GetDirectObjectAt(fieldsArray.Handle, index);
        if (directObjectAt != IntPtr.Zero && Pdfium.FPDFOBJ_GetObjNum(directObjectAt) == fieldDict.ObjectNumber)
          return index;
      }
    }
    return -1;
  }
}
