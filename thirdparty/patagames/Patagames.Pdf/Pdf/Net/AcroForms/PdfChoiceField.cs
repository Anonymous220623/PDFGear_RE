// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.AcroForms.PdfChoiceField
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Exceptions;
using System;

#nullable disable
namespace Patagames.Pdf.Net.AcroForms;

/// <summary>
/// Represents a choice field that contains several text items, one or more of which may be selected as the field value.
/// </summary>
public abstract class PdfChoiceField : PdfField
{
  /// <summary>
  /// Gets a collection of field's items to be presented to the user.
  /// </summary>
  public ChoiceFieldItemsCollection Items { get; private set; }

  /// <summary>
  /// Gets or sets a flag indicating whether the field’s <see cref="P:Patagames.Pdf.Net.AcroForms.PdfChoiceField.Items" /> should be sorted alphabetically.
  /// </summary>
  /// <value>
  /// If true, the field’s items should be sorted alphabetically.
  /// This flag is intended for use by form authoring tools, not by PDF viewer applications.
  /// Viewers should simply display the field’s items in the order in which they occur in the <see cref="P:Patagames.Pdf.Net.AcroForms.PdfChoiceField.Items" /> collection.
  /// </value>
  public bool Sort
  {
    get => (this.FlagsEx & FieldFlagsEx.Sort) == FieldFlagsEx.Sort;
    set
    {
      if (value)
        this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create((int) (this.FlagsEx | FieldFlagsEx.Sort));
      else
        this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create((int) (this.FlagsEx & ~FieldFlagsEx.Sort));
    }
  }

  /// <summary>
  /// Gets or sets a flag indicating whether the new value is committed as soon as a selection is made with the pointing device.
  /// </summary>
  /// <value>
  /// If true, the new value is committed as soon as a selection is made with the pointing device.
  /// This option enables applications to perform an action once a selection is made,
  /// without requiring the user to exit the field. If false, the new value is not committed until the user exits the field.
  /// </value>
  public bool CommitOnSelChange
  {
    get => (this.FlagsEx & FieldFlagsEx.DoNotScroll) == FieldFlagsEx.CommitOnSelChange;
    set
    {
      if (value)
        this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create((int) (this.FlagsEx | FieldFlagsEx.CommitOnSelChange));
      else
        this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create((int) (this.FlagsEx & ~FieldFlagsEx.CommitOnSelChange));
    }
  }

  /// <summary>Gets selected items</summary>
  /// <remarks>
  /// For choice fields that allow multiple
  /// selection (MultiSelect flag set), an array of integers, sorted in ascending order, representing
  /// the zero-based indices in the <see cref="P:Patagames.Pdf.Net.AcroForms.PdfChoiceField.Items" /> collection of the currently selected items. This
  /// entry is required when two or more elements in the <see cref="P:Patagames.Pdf.Net.AcroForms.PdfChoiceField.Items" /> collection have different names but
  /// the same export value.
  /// </remarks>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.DomException">Generated for newly created fields that have not yet been added to the DOM.</exception>
  public int[] SelectedItems
  {
    get
    {
      int length = !(this.Handle == IntPtr.Zero) ? Pdfium.FPDFFormField_CountSelectedItems(this.Handle) : throw new DomException((PdfField) this);
      if (length < 0)
        length = 0;
      int[] selectedItems = new int[length];
      for (int index = 0; index < length; ++index)
        selectedItems[index] = Pdfium.FPDFFormField_GetSelectedIndex(this.Handle, index);
      return selectedItems;
    }
  }

  /// <summary>
  /// Get the Combobox's or Listbox's index of item selected by default.
  /// </summary>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.DomException">Generated for newly created fields that have not yet been added to the DOM.</exception>
  public int DefaultSelectedItem
  {
    get
    {
      return !(this.Handle == IntPtr.Zero) ? Pdfium.FPDFFormField_GetDefaultSelectedItem(this.Handle) : throw new DomException((PdfField) this);
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
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.AcroForms.PdfChoiceField" /> class.
  /// </summary>
  /// <param name="forms">Interactive forms.</param>
  /// <param name="handle">The field's handle.</param>
  internal PdfChoiceField(PdfInteractiveForms forms, IntPtr handle)
    : base(forms, handle)
  {
    this.Items = new ChoiceFieldItemsCollection((PdfField) this);
  }

  /// <summary>The base constructor for creating a new choice field</summary>
  /// <param name="forms">Interactive forms.</param>
  /// <param name="name">The partial field name. Cannot contain a period.</param>
  /// <param name="parent">The parent field. Only non-terminal fields are accepted.</param>
  /// <param name="defFont">Default font used for all controls assigned with this field.</param>
  /// <param name="fontSize">Default font's size.</param>
  /// <param name="color">The default text color used when no text color is specified in the control.</param>
  /// <param name="alignment">A code specifying the form of justification to be used in displaying the text.</param>
  public PdfChoiceField(
    PdfInteractiveForms forms,
    string name = null,
    PdfField parent = null,
    PdfFont defFont = null,
    float fontSize = 0.0f,
    FS_COLOR? color = null,
    JustifyTypes alignment = JustifyTypes.Left)
    : base(forms, name, parent, defFont, fontSize, color)
  {
    this.Dictionary["FT"] = (PdfTypeBase) PdfTypeName.Create("Ch");
    this.Items = new ChoiceFieldItemsCollection((PdfField) this);
    this.Alignment = alignment;
  }

  /// <summary>Clear field's selection</summary>
  /// <returns>True for success or False otherwise</returns>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.DomException">Generated for newly created fields that have not yet been added to the DOM.</exception>
  public bool ClearSelection()
  {
    return !(this.Handle == IntPtr.Zero) ? Pdfium.FPDFFormField_ClearSelection(this.Handle, this.IsNotify) : throw new DomException((PdfField) this);
  }

  /// <summary>Select or deselect Combobox or Listbox item</summary>
  /// <param name="index">Zero based index of item in Combobox or Listbox.</param>
  /// <param name="select">True for select, False otherwise</param>
  /// <returns>True for success or False otherwise</returns>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.DomException">Generated for newly created fields that have not yet been added to the DOM.</exception>
  public bool SelectItem(int index, bool select)
  {
    if (this.Handle == IntPtr.Zero)
      throw new DomException((PdfField) this);
    return Pdfium.FPDFFormField_SetItemSelection(this.Handle, index, select, this.IsNotify);
  }

  /// <summary>
  /// Determines that the Combobox's or Listbox's item specified by index is selected by Default
  /// </summary>
  /// <param name="index">Zero based index of item in Combobox or Listbox.</param>
  /// <returns>True if item selected, False otherwise</returns>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.DomException">Generated for newly created fields that have not yet been added to the DOM.</exception>
  public bool IsItemDefaultSelected(int index)
  {
    return !(this.Handle == IntPtr.Zero) ? Pdfium.FPDFFormField_IsItemDefaultSelected(this.Handle, index) : throw new DomException((PdfField) this);
  }

  /// <summary>
  /// Determines that the Combobox's or Listbox's item specified by index is selected
  /// </summary>
  /// <param name="index">Zero based index of item in Combobox or Listbox.</param>
  /// <returns>True if item selected, False otherwise</returns>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.DomException">Generated for newly created fields that have not yet been added to the DOM.</exception>
  public bool IsItemSelected(int index)
  {
    return !(this.Handle == IntPtr.Zero) ? Pdfium.FPDFFormField_IsItemSelected(this.Handle, index) : throw new DomException((PdfField) this);
  }
}
