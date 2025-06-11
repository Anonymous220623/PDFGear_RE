// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PdfLoadedChoiceField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using Syncfusion.Pdf.Xfa;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

public class PdfLoadedChoiceField : PdfLoadedStyledField
{
  public PdfLoadedListItemCollection Values => this.GetListItemCollection();

  public int[] SelectedIndex
  {
    get => this.ObtainSelectedIndex();
    set => this.AssignSelectedIndex(value);
  }

  public string[] SelectedValue
  {
    get => this.ObtainSelectedValue();
    set => this.AssignSelectedValue(value);
  }

  public PdfLoadedListItemCollection SelectedItem
  {
    get
    {
      PdfLoadedListItemCollection selectedItem = new PdfLoadedListItemCollection(this);
      foreach (int index in this.SelectedIndex)
      {
        if (index > -1 && this.Values.Count > 0 && this.Values.Count > index)
        {
          PdfLoadedListItem pdfLoadedListItem = this.Values[index];
          selectedItem.AddItem(pdfLoadedListItem);
        }
      }
      return selectedItem;
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

  internal PdfLoadedChoiceField(PdfDictionary dictionary, PdfCrossTable crossTable)
    : base(dictionary, crossTable)
  {
  }

  protected int[] ObtainSelectedIndex()
  {
    List<int> intList = new List<int>();
    if (this.Dictionary.ContainsKey("I"))
    {
      if (this.CrossTable.GetObject(this.Dictionary["I"]) is PdfArray pdfArray)
      {
        if (pdfArray.Count > 0)
        {
          for (int index = 0; index < pdfArray.Count; ++index)
          {
            PdfNumber pdfNumber = this.CrossTable.GetObject(pdfArray[index]) as PdfNumber;
            intList.Add(pdfNumber.IntValue);
          }
        }
      }
      else if (this.CrossTable.GetObject(this.Dictionary["I"]) is PdfNumber pdfNumber1)
        intList.Add(pdfNumber1.IntValue);
    }
    if (intList.Count == 0)
      intList.Add(-1);
    return intList.ToArray();
  }

  protected void AssignSelectedIndex(int[] value)
  {
    if (value.Length == 0 || value.Length > this.Values.Count)
      throw new ArgumentOutOfRangeException("SelectedIndex");
    foreach (int num in value)
    {
      if (num >= this.Values.Count)
        throw new ArgumentOutOfRangeException("SelectedIndex");
    }
    if (this.ReadOnly)
      return;
    System.Array.Sort<int>(value);
    this.Dictionary.SetProperty("I", (IPdfPrimitive) new PdfArray(value));
    List<string> stringList = new List<string>();
    foreach (int index in value)
    {
      if (index >= 0)
        stringList.Add(this.Values[index].Value);
    }
    if (stringList.Count > 0)
      this.AssignSelectedValue(stringList.ToArray());
    this.Changed = true;
  }

  protected string[] ObtainSelectedValue()
  {
    List<string> stringList = new List<string>();
    if (this.Dictionary.ContainsKey("V"))
    {
      IPdfPrimitive pdfPrimitive = this.CrossTable.GetObject(this.Dictionary["V"]);
      if (pdfPrimitive is PdfString)
      {
        stringList.Add((pdfPrimitive as PdfString).Value);
      }
      else
      {
        PdfArray pdfArray = pdfPrimitive as PdfArray;
        for (int index = 0; index < pdfArray.Count; ++index)
        {
          PdfString pdfString = pdfArray[index] as PdfString;
          stringList.Add(pdfString.Value);
        }
      }
    }
    else
    {
      foreach (int index in this.SelectedIndex)
      {
        if (index > -1)
          stringList.Add(this.Values[index].Value);
      }
    }
    return stringList.ToArray();
  }

  protected void AssignSelectedValue(string[] values)
  {
    List<int> intList = new List<int>();
    PdfLoadedListItemCollection values1 = this.Values;
    if (!this.ReadOnly)
    {
      foreach (string str in values)
      {
        bool flag = false;
        for (int index = 0; index < values1.Count; ++index)
        {
          if (values1[index].Value == str || values1[index].Text == str)
          {
            flag = true;
            intList.Add(index);
          }
        }
        if (!flag && this is PdfLoadedComboBoxField && !(this as PdfLoadedComboBoxField).Editable)
          throw new ArgumentOutOfRangeException("index");
      }
      if (values.Length > 1 && !(this as PdfLoadedListBoxField).MultiSelect)
      {
        intList.RemoveRange(1, intList.Count - 1);
        values = new string[1]{ values1[intList[0]].Value };
      }
      if (intList.Count != 0)
      {
        intList.Sort();
        this.Dictionary.SetProperty("I", (IPdfPrimitive) new PdfArray(intList.ToArray()));
      }
      else
        this.Dictionary.Remove("I");
    }
    if (this.Dictionary.ContainsKey("V"))
    {
      IPdfPrimitive pdfPrimitive = this.CrossTable.GetObject(this.Dictionary["V"]);
      switch (pdfPrimitive)
      {
        case null:
        case PdfString _:
          if (this is PdfLoadedListBoxField)
          {
            PdfArray primitive = new PdfArray();
            foreach (string str in values)
              primitive.Add((IPdfPrimitive) new PdfString(str));
            this.Dictionary.SetProperty("V", (IPdfPrimitive) primitive);
            break;
          }
          this.Dictionary["V"] = (IPdfPrimitive) new PdfString(values[0].ToString());
          break;
        default:
          PdfArray primitive1 = pdfPrimitive as PdfArray;
          primitive1.Clear();
          foreach (string str in values)
            primitive1.Add((IPdfPrimitive) new PdfString(str));
          this.Dictionary.SetProperty("V", (IPdfPrimitive) primitive1);
          break;
      }
    }
    else if (this is PdfLoadedComboBoxField)
    {
      this.Dictionary.SetString("V", values[0]);
    }
    else
    {
      PdfArray primitive = new PdfArray();
      foreach (string str in values)
        primitive.Add((IPdfPrimitive) new PdfString(str));
      this.Dictionary.SetProperty("V", (IPdfPrimitive) primitive);
    }
    this.Changed = true;
    if (!this.Form.m_enableXfaFormfill || !(this.Form as PdfLoadedForm).IsXFAForm || values.Length <= 0)
      return;
    PdfLoadedXfaField xfaField = (this.Form as PdfLoadedForm).GetXfaField(this.Name.Replace("\\", string.Empty));
    switch (xfaField)
    {
      case PdfLoadedXfaListBoxField _:
        if (!(xfaField is PdfLoadedXfaListBoxField loadedXfaListBoxField) || values1.Count < values.Length)
          break;
        loadedXfaListBoxField.SelectedItems = values;
        break;
      case PdfLoadedXfaComboBoxField _:
        if (xfaField is PdfLoadedXfaComboBoxField xfaComboBoxField && xfaComboBoxField.Items != null && xfaComboBoxField.Items.Contains(values[0]))
        {
          xfaComboBoxField.SelectedValue = values[0];
          break;
        }
        if (xfaComboBoxField == null || xfaComboBoxField.HiddenItems == null || !xfaComboBoxField.HiddenItems.Contains(values[0]))
          break;
        xfaComboBoxField.SelectedValue = values[0];
        break;
    }
  }

  internal PdfLoadedListItemCollection GetListItemCollection()
  {
    PdfLoadedListItemCollection listItemCollection = new PdfLoadedListItemCollection(this);
    if (!(PdfLoadedField.GetValue(this.Dictionary, this.CrossTable, "Opt", true) is PdfArray pdfArray3) && this.Dictionary.ContainsKey("Kids"))
    {
      PdfArray pdfArray1 = PdfCrossTable.Dereference(this.Dictionary["Kids"]) as PdfArray;
      int index = 0;
      while (index < pdfArray1.Count && !(PdfLoadedField.GetValue(PdfCrossTable.Dereference(pdfArray1[index]) as PdfDictionary, this.CrossTable, "Opt", true) is PdfArray pdfArray3))
        ++index;
    }
    if (pdfArray3 != null)
    {
      int index = 0;
      for (int count = pdfArray3.Count; index < count; ++index)
      {
        IPdfPrimitive pdfPrimitive = this.CrossTable.GetObject(pdfArray3[index]);
        PdfLoadedListItem pdfLoadedListItem;
        if (pdfPrimitive is PdfString)
        {
          pdfLoadedListItem = new PdfLoadedListItem((pdfPrimitive as PdfString).Value, (string) null, this, this.CrossTable);
        }
        else
        {
          PdfArray pdfArray4 = pdfPrimitive as PdfArray;
          PdfString pdfString = this.CrossTable.GetObject(pdfArray4[0]) as PdfString;
          pdfLoadedListItem = new PdfLoadedListItem((this.CrossTable.GetObject(pdfArray4[1]) as PdfString).Value, pdfString.Value, this, this.CrossTable);
        }
        listItemCollection.AddItem(pdfLoadedListItem);
      }
    }
    return listItemCollection;
  }
}
