// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PdfLoadedCheckBoxField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using Syncfusion.Pdf.Xfa;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

public class PdfLoadedCheckBoxField : PdfLoadedStateField
{
  private const string CHECK_SYMBOL = "4";

  public bool Checked
  {
    get
    {
      bool flag = false;
      if (this.Items.Count > 0)
        flag = this.Items[this.DefaultIndex].Checked;
      return flag;
    }
    set
    {
      if ((FieldFlags.ReadOnly & this.Flags) != FieldFlags.Default)
        return;
      if (this.Items.Count > 0)
        this.Items[this.DefaultIndex].Checked = value;
      else
        this.SetCheckedStatus(value);
      ((PdfField) this).Form.SetAppearanceDictionary = true;
      if (!this.Form.m_enableXfaFormfill || !(this.Form as PdfLoadedForm).IsXFAForm || !((this.Form as PdfLoadedForm).GetXfaField(this.Name.Replace("\\", string.Empty)) is PdfLoadedXfaCheckBoxField xfaField))
        return;
      xfaField.IsChecked = this.Checked;
    }
  }

  public PdfColor BackColor
  {
    get => this.GetBackColor();
    set
    {
      this.AssignBackColor(value);
      if (this.Form.NeedAppearances)
        return;
      this.Changed = true;
      this.FieldChanged = true;
    }
  }

  public new PdfColor ForeColor
  {
    get => base.ForeColor;
    set
    {
      base.ForeColor = value;
      if (this.Form.NeedAppearances)
        return;
      this.Changed = true;
      this.FieldChanged = true;
    }
  }

  public new PdfCheckBoxStyle Style
  {
    get => base.Style;
    set
    {
      if (base.Style != value)
        this.FieldChanged = true;
      base.Style = value;
      if (this.Form.NeedAppearances)
        return;
      this.Changed = true;
    }
  }

  public PdfLoadedCheckBoxItemCollection Items
  {
    get => base.Items as PdfLoadedCheckBoxItemCollection;
    internal set => this.Items = (PdfLoadedStateItemCollection) value;
  }

  internal PdfLoadedCheckBoxField(PdfDictionary dictionary, PdfCrossTable crossTable)
    : base(dictionary, crossTable, (PdfLoadedStateItemCollection) new PdfLoadedCheckBoxItemCollection())
  {
  }

  internal override PdfLoadedStateItem GetItem(int index, PdfDictionary itemDictionary)
  {
    return (PdfLoadedStateItem) new PdfLoadedCheckBoxItem((PdfLoadedStyledField) this, index, itemDictionary);
  }

  internal override void Draw()
  {
    base.Draw();
    PdfArray kids = this.Kids;
    if (kids != null)
    {
      for (int index = 0; index < kids.Count; ++index)
      {
        PdfLoadedCheckBoxItem loadedCheckBoxItem = this.Items[index];
        PdfCheckFieldState state = loadedCheckBoxItem.Checked ? PdfCheckFieldState.Checked : PdfCheckFieldState.Unchecked;
        if (loadedCheckBoxItem.Page != null)
          this.DrawStateItem(loadedCheckBoxItem.Page.Graphics, state, (PdfLoadedStateItem) loadedCheckBoxItem);
      }
    }
    else
      this.DrawStateItem(this.Page.Graphics, this.Checked ? PdfCheckFieldState.Checked : PdfCheckFieldState.Unchecked, (PdfLoadedStateItem) null);
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
    PdfLoadedCheckBoxField loadedCheckBoxField = new PdfLoadedCheckBoxField(dictionary, crossTable);
    loadedCheckBoxField.Page = (PdfPageBase) page;
    loadedCheckBoxField.SetName(this.GetFieldName());
    loadedCheckBoxField.Widget.Dictionary = this.Widget.Dictionary.Clone(crossTable) as PdfDictionary;
    return (PdfField) loadedCheckBoxField;
  }

  internal new PdfLoadedStyledField Clone()
  {
    PdfLoadedCheckBoxField field = this.MemberwiseClone() as PdfLoadedCheckBoxField;
    field.Dictionary = this.Dictionary.Clone(this.CrossTable) as PdfDictionary;
    field.Widget.Dictionary = this.Widget.Dictionary.Clone(this.CrossTable) as PdfDictionary;
    field.Items = new PdfLoadedCheckBoxItemCollection();
    for (int index = 0; index < this.Items.Count; ++index)
    {
      PdfLoadedCheckBoxItem loadedCheckBoxItem = new PdfLoadedCheckBoxItem((PdfLoadedStyledField) field, index, this.Items[index].Dictionary);
      field.Items.Add(loadedCheckBoxItem);
    }
    return (PdfLoadedStyledField) field;
  }

  internal override PdfLoadedFieldItem CreateLoadedItem(PdfDictionary dictionary)
  {
    base.CreateLoadedItem(dictionary);
    PdfLoadedCheckBoxItem loadedItem = (PdfLoadedCheckBoxItem) null;
    if (this.Items != null)
    {
      loadedItem = new PdfLoadedCheckBoxItem((PdfLoadedStyledField) this, this.Items.Count, dictionary);
      this.Items.Add(loadedItem);
    }
    if (this.Kids == null)
      this.Dictionary["Kids"] = (IPdfPrimitive) new PdfArray();
    this.Kids.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) dictionary));
    return (PdfLoadedFieldItem) loadedItem;
  }

  public new void RemoveAt(int index)
  {
    if (this.Items == null || this.Items.Count == 0)
      return;
    this.Remove(this.Items[index]);
  }

  public void Remove(PdfLoadedCheckBoxItem item)
  {
    int index = item != null ? this.Items.Remove(item) : throw new NullReferenceException(nameof (item));
    if (!(this.Dictionary["Kids"] is PdfArray pdfArray))
      return;
    pdfArray.RemoveAt(index);
    pdfArray.MarkChanged();
  }

  public bool TryGetFieldItem(string exportValue, out PdfLoadedCheckBoxItem field)
  {
    field = (PdfLoadedCheckBoxItem) null;
    if (this.Kids != null || this.Items != null)
    {
      for (int index1 = 0; index1 < this.Items.Count; ++index1)
      {
        PdfLoadedCheckBoxItem loadedCheckBoxItem = this.Items[index1];
        if (loadedCheckBoxItem != null && loadedCheckBoxItem.Dictionary != null && loadedCheckBoxItem.Dictionary.ContainsKey("AS"))
        {
          PdfName pdfName1 = PdfCrossTable.Dereference(loadedCheckBoxItem.Dictionary["AS"]) as PdfName;
          if (loadedCheckBoxItem.Dictionary.ContainsKey("AP"))
          {
            PdfDictionary pdfDictionary1 = PdfCrossTable.Dereference(loadedCheckBoxItem.Dictionary["AP"]) as PdfDictionary;
            if (pdfDictionary1 != null && pdfDictionary1.ContainsKey("N") && PdfCrossTable.Dereference(pdfDictionary1["N"]) is PdfDictionary pdfDictionary2)
            {
              List<object> objectList = new List<object>();
              foreach (PdfName key in pdfDictionary2.Keys)
                objectList.Add((object) key);
              int index2 = 0;
              for (int count = objectList.Count; index2 < count; ++index2)
              {
                PdfName pdfName2 = objectList[index2] as PdfName;
                if (pdfName2 != (PdfName) null && pdfName2.Value != "Off" && pdfName2.Value == exportValue)
                {
                  field = loadedCheckBoxItem;
                  return true;
                }
              }
            }
          }
          else if (pdfName1 != (PdfName) null && pdfName1.Value != null && pdfName1.Value == exportValue)
          {
            field = loadedCheckBoxItem;
            return true;
          }
        }
      }
    }
    return false;
  }
}
