// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PdfLoadedRadioButtonListField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

public class PdfLoadedRadioButtonListField : PdfLoadedStateField
{
  private const string CHECK_SYMBOL = "l";

  public PdfLoadedRadioButtonItemCollection Items
  {
    get => base.Items as PdfLoadedRadioButtonItemCollection;
    internal set => this.Items = (PdfLoadedStateItemCollection) value;
  }

  public int SelectedIndex
  {
    get => this.ObtainSelectedIndex();
    set => this.AssignSelectedIndex(value);
  }

  public string SelectedValue
  {
    get
    {
      int selectedIndex = this.SelectedIndex;
      return selectedIndex > -1 ? this.Items[selectedIndex].Value : (string) null;
    }
    set => this.AssignSelectedValue(value);
  }

  public PdfLoadedRadioButtonItem SelectedItem
  {
    get
    {
      int selectedIndex = this.SelectedIndex;
      PdfLoadedRadioButtonItem selectedItem = (PdfLoadedRadioButtonItem) null;
      if (selectedIndex > -1)
        selectedItem = this.Items[selectedIndex];
      return selectedItem;
    }
  }

  public string Value
  {
    get => this.Items[this.DefaultIndex].Value;
    set => this.Items[this.DefaultIndex].Value = value;
  }

  internal PdfLoadedRadioButtonListField(PdfDictionary dictionary, PdfCrossTable crossTable)
    : base(dictionary, crossTable, (PdfLoadedStateItemCollection) new PdfLoadedRadioButtonItemCollection())
  {
    if (this.Items == null || this.Items.Count <= 0)
      return;
    this.RetrieveOptionValue();
  }

  internal PdfLoadedRadioButtonItem GetItem(string value)
  {
    foreach (PdfLoadedRadioButtonItem loadedRadioButtonItem in (PdfCollection) this.Items)
    {
      if (loadedRadioButtonItem.Value == value || loadedRadioButtonItem.OptionValue == value)
        return loadedRadioButtonItem;
    }
    return (PdfLoadedRadioButtonItem) null;
  }

  internal override PdfLoadedStateItem GetItem(int index, PdfDictionary itemDictionary)
  {
    return (PdfLoadedStateItem) new PdfLoadedRadioButtonItem((PdfLoadedStyledField) this, index, itemDictionary);
  }

  private int ObtainSelectedIndex()
  {
    int selectedIndex = -1;
    PdfLoadedRadioButtonItemCollection items = this.Items;
    int index = 0;
    for (int count = items.Count; index < count; ++index)
    {
      PdfLoadedRadioButtonItem loadedRadioButtonItem = items[index];
      PdfDictionary dictionary = loadedRadioButtonItem.Dictionary;
      IPdfPrimitive pdfPrimitive = PdfLoadedField.SearchInParents(dictionary, this.CrossTable, "V");
      PdfName pdfName1 = pdfPrimitive as PdfName;
      PdfString pdfString = (PdfString) null;
      if (pdfName1 == (PdfName) null)
        pdfString = pdfPrimitive as PdfString;
      if (dictionary.ContainsKey("AS") && (pdfName1 != (PdfName) null || pdfString != null))
      {
        PdfName pdfName2 = this.CrossTable.GetObject(dictionary["AS"]) as PdfName;
        if (pdfName2.Value.ToLower() != "off")
        {
          if (pdfName1 != (PdfName) null && pdfName1.Value.ToLower() != "off")
          {
            if (pdfName2.Value == pdfName1.Value || loadedRadioButtonItem.OptionValue == pdfName1.Value)
            {
              selectedIndex = index;
              break;
            }
            break;
          }
          if (pdfString != null && pdfString.Value.ToLower() != "off")
          {
            if (pdfName2.Value == pdfString.Value || loadedRadioButtonItem.OptionValue == pdfString.Value)
            {
              selectedIndex = index;
              break;
            }
            break;
          }
        }
      }
    }
    return selectedIndex;
  }

  private void AssignSelectedIndex(int value)
  {
    if (this.SelectedIndex == value)
      return;
    PdfLoadedRadioButtonItem child = this.Items[value];
    this.UncheckOthers((PdfLoadedStateItem) child, PdfLoadedStateField.GetItemValue(child.Dictionary, this.CrossTable), true);
    child.Checked = true;
    string name = PdfName.EncodeName(child.Value);
    this.Dictionary.SetName("V", name);
    this.Dictionary.SetName("DV", name);
  }

  private void AssignSelectedValue(string value)
  {
    PdfLoadedRadioButtonItem child = value != null ? this.GetItem(PdfName.DecodeName(value)) : throw new ArgumentNullException("SelectedValue");
    if (child != null)
    {
      this.UncheckOthers((PdfLoadedStateItem) child, PdfLoadedStateField.GetItemValue(child.Dictionary, this.CrossTable), true);
      child.Checked = true;
      if (!this.isAcrobat)
      {
        this.Dictionary.SetName("V", child.Value);
        this.Dictionary.SetName("DV", child.Value);
      }
      else
      {
        this.Dictionary.SetName("V", value);
        this.Dictionary.SetName("DV", value);
      }
    }
    else
    {
      if (!(value == "Off"))
        return;
      this.UncheckOthers((PdfLoadedStateItem) null, value, true);
    }
  }

  internal override void Draw()
  {
    base.Draw();
    PdfArray kids = this.Kids;
    if (kids != null)
    {
      for (int index = 0; index < kids.Count; ++index)
      {
        PdfLoadedRadioButtonItem loadedRadioButtonItem = this.Items[index];
        PdfCheckFieldState state = loadedRadioButtonItem.Checked ? PdfCheckFieldState.Checked : PdfCheckFieldState.Unchecked;
        if (loadedRadioButtonItem.Page != null)
          this.DrawStateItem(loadedRadioButtonItem.Page.Graphics, state, (PdfLoadedStateItem) loadedRadioButtonItem);
      }
    }
    else
      this.DrawStateItem(this.Page.Graphics, this.SelectedIndex == this.DefaultIndex ? PdfCheckFieldState.Checked : PdfCheckFieldState.Unchecked, (PdfLoadedStateItem) null);
  }

  internal override void BeginSave()
  {
    base.BeginSave();
    PdfArray kids = this.Kids;
    if (kids != null)
    {
      for (int index = 0; index < kids.Count; ++index)
        this.ApplyAppearance(this.CrossTable.GetObject(kids[index]) as PdfDictionary, (PdfLoadedStateItem) this.Items[index]);
    }
    else
      this.ApplyAppearance(this.GetWidgetAnnotation(this.Dictionary, this.CrossTable), (PdfLoadedStateItem) null);
  }

  internal new PdfField Clone(PdfDictionary dictionary, PdfPage page)
  {
    PdfCrossTable crossTable = page.Section.ParentDocument.CrossTable;
    PdfLoadedRadioButtonListField radioButtonListField = new PdfLoadedRadioButtonListField(dictionary, crossTable);
    radioButtonListField.Page = (PdfPageBase) page;
    radioButtonListField.SetName(this.GetFieldName());
    radioButtonListField.Widget.Dictionary = this.Widget.Dictionary.Clone(crossTable) as PdfDictionary;
    return (PdfField) radioButtonListField;
  }

  internal new PdfLoadedStyledField Clone()
  {
    PdfLoadedRadioButtonListField field = this.MemberwiseClone() as PdfLoadedRadioButtonListField;
    field.Dictionary = this.Dictionary.Clone(this.CrossTable) as PdfDictionary;
    field.Widget.Dictionary = this.Widget.Dictionary.Clone(this.CrossTable) as PdfDictionary;
    field.Items = new PdfLoadedRadioButtonItemCollection();
    for (int index = 0; index < this.Items.Count; ++index)
    {
      PdfLoadedRadioButtonItem loadedRadioButtonItem = new PdfLoadedRadioButtonItem((PdfLoadedStyledField) field, index, this.Items[index].Dictionary);
      field.Items.Add(loadedRadioButtonItem);
    }
    return (PdfLoadedStyledField) field;
  }

  internal override PdfLoadedFieldItem CreateLoadedItem(PdfDictionary dictionary)
  {
    base.CreateLoadedItem(dictionary);
    PdfLoadedRadioButtonItem loadedItem = (PdfLoadedRadioButtonItem) null;
    if (this.Items != null)
    {
      loadedItem = new PdfLoadedRadioButtonItem((PdfLoadedStyledField) this, this.Items.Count, dictionary);
      this.Items.Add(loadedItem);
    }
    if (this.Kids == null)
      this.Dictionary["Kids"] = (IPdfPrimitive) new PdfArray();
    this.Kids.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) dictionary));
    return (PdfLoadedFieldItem) loadedItem;
  }

  private void RetrieveOptionValue()
  {
    if (!this.Dictionary.ContainsKey("Opt"))
      return;
    IPdfPrimitive pdfPrimitive = this.Dictionary["Opt"];
    if (!(((object) (pdfPrimitive as PdfReferenceHolder) != null ? (pdfPrimitive as PdfReferenceHolder).Object : pdfPrimitive) is PdfArray pdfArray))
      return;
    int num = pdfArray.Count <= this.Items.Count ? pdfArray.Count : this.Items.Count;
    for (int index = 0; index < num; ++index)
    {
      if (((object) (pdfArray[index] as PdfReferenceHolder) != null ? (pdfArray[index] as PdfReferenceHolder).Object : pdfArray[index]) is PdfString pdfString)
        this.Items[index].OptionValue = pdfString.Value;
    }
  }

  public new void RemoveAt(int index)
  {
    if (this.Items == null || this.Items.Count == 0)
      return;
    this.Remove(this.Items[index]);
  }

  public void Remove(PdfLoadedRadioButtonItem item)
  {
    int index = item != null ? this.Items.Remove(item) : throw new NullReferenceException(nameof (item));
    if (!(this.Dictionary["Kids"] is PdfArray pdfArray))
      return;
    pdfArray.RemoveAt(index);
    pdfArray.MarkChanged();
  }
}
