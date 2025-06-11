// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.AcroForms.PdfTextBoxField
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.BasicTypes;
using System;

#nullable disable
namespace Patagames.Pdf.Net.AcroForms;

/// <summary>
/// Represents a text field that is a box or space in which the user can enter text from the keyboard.
/// </summary>
public class PdfTextBoxField : PdfField
{
  /// <summary>
  /// Gets or sets a flag indicating whether the text field is multiline.
  /// </summary>
  /// <value>
  /// If true, the field can contain multiple lines of text; if false, the field’s text is restricted to a single line.
  /// </value>
  public bool Multiline
  {
    get => (this.FlagsEx & FieldFlagsEx.Multiline) == FieldFlagsEx.Multiline;
    set
    {
      if (value)
        this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create((int) (this.FlagsEx | FieldFlagsEx.Multiline));
      else
        this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create((int) (this.FlagsEx & ~FieldFlagsEx.Multiline));
    }
  }

  /// <summary>
  /// Gets or sets a flag indicating whether the text field is intended for entering a secure password.
  /// </summary>
  /// <value>
  /// If true, the field is intended for entering a secure password that should not be echoed visibly to the screen.
  /// Characters typed from the keyboard should instead be echoed in some unreadable form, such as asterisks or bullet characters.
  /// </value>
  public bool Password
  {
    get => (this.FlagsEx & FieldFlagsEx.Password) == FieldFlagsEx.Password;
    set
    {
      if (value)
        this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create((int) (this.FlagsEx | FieldFlagsEx.Password));
      else
        this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create((int) (this.FlagsEx & ~FieldFlagsEx.Password));
    }
  }

  /// <summary>
  /// Gets or sets a flag indicating whether the text entered in the field represents the pathname of a file
  /// </summary>
  /// <value>
  /// If true, the text entered in the field represents the pathname of a file whose contents are to be submitted as the value of the field.
  /// </value>
  public bool FileSelect
  {
    get => (this.FlagsEx & FieldFlagsEx.FileSelect) == FieldFlagsEx.FileSelect;
    set
    {
      if (value)
        this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create((int) (this.FlagsEx | FieldFlagsEx.FileSelect));
      else
        this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create((int) (this.FlagsEx & ~FieldFlagsEx.FileSelect));
    }
  }

  /// <summary>
  /// Gets or sets a flag indicating whether the text entered in the field should be spell-checked or not.
  /// </summary>
  /// <value>If true, text entered in the field is not spell-checked.</value>
  public bool DoNotSpellCheck
  {
    get => (this.FlagsEx & FieldFlagsEx.DoNotSpellCheck) == FieldFlagsEx.DoNotSpellCheck;
    set
    {
      if (value)
        this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create((int) (this.FlagsEx | FieldFlagsEx.DoNotSpellCheck));
      else
        this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create((int) (this.FlagsEx & ~FieldFlagsEx.DoNotSpellCheck));
    }
  }

  /// <summary>
  /// Gets or sets a flag indicating whether the text entered in the field can be scrolled or not.
  /// </summary>
  /// <value>
  /// If true, the field does not scroll (horizontally for single-line fields, vertically for multiple-line fields)
  /// to accommodate more text than fits within its annotation rectangle.
  /// Once the field is full, no further text is accepted.
  /// </value>
  public bool DoNotScroll
  {
    get => (this.FlagsEx & FieldFlagsEx.DoNotScroll) == FieldFlagsEx.DoNotScroll;
    set
    {
      if (value)
        this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create((int) (this.FlagsEx | FieldFlagsEx.DoNotScroll));
      else
        this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create((int) (this.FlagsEx & ~FieldFlagsEx.DoNotScroll));
    }
  }

  /// <summary>
  /// Gets or sets a flag indicating whether the text field contains a rich text string.
  /// </summary>
  /// <value>
  /// If true, the <see cref="P:Patagames.Pdf.Net.PdfField.Value" /> of this field should be represented as a flat (non-formatted) text version of the <see cref="P:Patagames.Pdf.Net.AcroForms.PdfTextBoxField.RichText" /> property.
  /// If the field has a value, the <see cref="P:Patagames.Pdf.Net.AcroForms.PdfTextBoxField.RichText" /> propertry  specifies the rich text string.
  /// </value>
  public bool IsRichText
  {
    get => (this.FlagsEx & FieldFlagsEx.RadiosInUnison) == FieldFlagsEx.RadiosInUnison;
    set
    {
      if (value)
        this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create((int) (this.FlagsEx | FieldFlagsEx.RadiosInUnison));
      else
        this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create((int) (this.FlagsEx & ~FieldFlagsEx.RadiosInUnison));
    }
  }

  /// <summary>Gets or sets a comb flag.</summary>
  /// <value>
  /// Meaningful only if the <see cref="P:Patagames.Pdf.Net.AcroForms.PdfTextBoxField.MaxLen" /> property is not 0
  /// and if the <see cref="P:Patagames.Pdf.Net.AcroForms.PdfTextBoxField.Multiline" />, <see cref="P:Patagames.Pdf.Net.AcroForms.PdfTextBoxField.Password" />, and <see cref="P:Patagames.Pdf.Net.AcroForms.PdfTextBoxField.FileSelect" />
  /// properies are false. If true, the field is automatically divided into as many equally spaced positions,
  /// or combs, as the value of <see cref="P:Patagames.Pdf.Net.AcroForms.PdfTextBoxField.MaxLen" />, and the text is laid out into those combs.
  /// </value>
  public bool Comb
  {
    get => (this.FlagsEx & FieldFlagsEx.Comb) == FieldFlagsEx.Comb;
    set
    {
      if (value)
        this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create((int) (this.FlagsEx | FieldFlagsEx.Comb));
      else
        this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create((int) (this.FlagsEx & ~FieldFlagsEx.Comb));
    }
  }

  /// <summary>
  /// Gets or sets the maximum length of the field’s text, in characters.
  /// </summary>
  public int MaxLen
  {
    get
    {
      if (this.Handle != IntPtr.Zero)
        return Pdfium.FPDFFormField_GetMaxLen(this.Handle);
      if (this.Dictionary != null)
      {
        PdfTypeBase fieldAttribute = this.GetFieldAttribute(nameof (MaxLen));
        if (fieldAttribute != null && fieldAttribute.Is<PdfTypeNumber>())
          return fieldAttribute.As<PdfTypeNumber>().IntValue;
      }
      return 0;
    }
    set
    {
      if (value < 0 && this.Dictionary.ContainsKey(nameof (MaxLen)))
      {
        this.Dictionary.Remove(nameof (MaxLen));
      }
      else
      {
        if (value < 0)
          return;
        this.Dictionary[nameof (MaxLen)] = (PdfTypeBase) PdfTypeNumber.Create(value);
      }
    }
  }

  /// <summary>Gets a rich text string</summary>
  /// <remarks>
  /// Beginning with PDF 1.5, the text contents of variable text form fields, as well as
  /// markup annotations, can include formatting (style) information. These rich text
  /// strings are fully-formed XML documents that conform to the rich text conventions
  /// specified for the XML Forms Architecture (XFA) specification, which is itself
  /// a subset of the XHTML 1.0 specification, augmented with a restricted set of
  /// CSS2 style attributes.
  /// </remarks>
  public string RichText
  {
    get
    {
      if (this.Handle != IntPtr.Zero)
        return Pdfium.FPDFFormField_GetRichTextString(this.Handle);
      PdfTypeBase fieldAttribute = this.GetFieldAttribute("RV");
      return fieldAttribute == null || !fieldAttribute.Is<PdfTypeString>() ? (string) null : fieldAttribute.As<PdfTypeString>().UnicodeString;
    }
  }

  /// <summary>
  /// Gets or sets a code specifying the form of justification to be used in displaying the text.
  /// </summary>
  public JustifyTypes Alignment
  {
    get
    {
      PdfTypeBase fieldAttribute = this.GetFieldAttribute("Q");
      if (fieldAttribute == null || !fieldAttribute.Is<PdfTypeNumber>())
        return JustifyTypes.Left;
      int alignment = fieldAttribute.As<PdfTypeNumber>().IntValue;
      if (alignment < 0 || alignment > 2)
        alignment = 0;
      return (JustifyTypes) alignment;
    }
    set
    {
      if (value == JustifyTypes.Left && this.Dictionary.ContainsKey("Q"))
      {
        this.Dictionary.Remove("Q");
      }
      else
      {
        if (value == JustifyTypes.Left)
          return;
        this.Dictionary["Q"] = (PdfTypeBase) PdfTypeNumber.Create((int) value);
      }
    }
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.AcroForms.PdfTextBoxField" /> class.
  /// </summary>
  /// <param name="forms">Interactive forms.</param>
  /// <param name="handle">The field's handle.</param>
  internal PdfTextBoxField(PdfInteractiveForms forms, IntPtr handle)
    : base(forms, handle)
  {
  }

  /// <summary>
  /// Create new textbox field and add it into interactive forms.
  /// </summary>
  /// <param name="forms">Interactive forms.</param>
  /// <param name="name">The partial field name. Cannot contain a period.</param>
  /// <param name="parent">The parent field. Only non-terminal fields are accepted.</param>
  /// <param name="defFont">Default font used for all controls assigned with this field.</param>
  /// <param name="fontSize">Default font's size.</param>
  /// <param name="color">The default text color used when no text color is specified in the control.</param>
  /// <param name="alignment">A code specifying the form of justification to be used in displaying the text.</param>
  public PdfTextBoxField(
    PdfInteractiveForms forms,
    string name = null,
    PdfField parent = null,
    PdfFont defFont = null,
    float fontSize = 0.0f,
    FS_COLOR? color = null,
    JustifyTypes alignment = JustifyTypes.Left)
    : base(forms, name, parent, defFont, fontSize, color)
  {
    this.Dictionary["FT"] = (PdfTypeBase) PdfTypeName.Create("Tx");
    this.Alignment = alignment;
  }
}
