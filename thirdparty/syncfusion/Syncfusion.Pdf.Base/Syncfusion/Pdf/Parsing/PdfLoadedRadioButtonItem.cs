// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PdfLoadedRadioButtonItem
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

public class PdfLoadedRadioButtonItem : PdfLoadedStateItem
{
  private string m_optionValue;

  public string Value
  {
    get => this.GetItemValue();
    set => this.SetItemValue(value);
  }

  public string OptionValue
  {
    get => this.m_optionValue;
    internal set => this.m_optionValue = value;
  }

  public bool Selected
  {
    get => this.Parent.Items.IndexOf(this) == this.Parent.SelectedIndex;
    set
    {
      if (!value)
        return;
      this.Parent.SelectedIndex = this.Parent.Items.IndexOf(this);
    }
  }

  internal PdfLoadedRadioButtonListField Parent => base.Parent as PdfLoadedRadioButtonListField;

  internal PdfLoadedRadioButtonItem(
    PdfLoadedStyledField field,
    int index,
    PdfDictionary dictionary)
    : base(field, index, dictionary)
  {
  }

  private string GetItemValue()
  {
    string itemValue = string.Empty;
    if (this.Dictionary.ContainsKey("AS"))
    {
      PdfName pdfName = this.CrossTable.GetObject(this.Dictionary["AS"]) as PdfName;
      if (pdfName != (PdfName) null && pdfName.Value != "Off")
        itemValue = PdfName.DecodeName(pdfName.Value);
    }
    if (itemValue == string.Empty && this.Dictionary.ContainsKey("AP"))
    {
      PdfDictionary pdfDictionary1 = this.CrossTable.GetObject(this.Dictionary["AP"]) as PdfDictionary;
      if (pdfDictionary1.ContainsKey("N"))
      {
        PdfDictionary pdfDictionary2 = this.CrossTable.GetObject((IPdfPrimitive) this.CrossTable.GetReference(pdfDictionary1["N"])) as PdfDictionary;
        List<object> objectList = new List<object>();
        foreach (PdfName key in pdfDictionary2.Keys)
          objectList.Add((object) key);
        int index = 0;
        for (int count = objectList.Count; index < count; ++index)
        {
          PdfName pdfName = objectList[index] as PdfName;
          if (pdfName.Value != "Off")
          {
            itemValue = PdfName.DecodeName(pdfName.Value);
            break;
          }
        }
      }
    }
    return itemValue;
  }

  private void SetItemValue(string value)
  {
    string str = value;
    if (this.Dictionary.ContainsKey("AP"))
    {
      PdfDictionary pdfDictionary1 = this.CrossTable.GetObject(this.Dictionary["AP"]) as PdfDictionary;
      if (pdfDictionary1.ContainsKey("N"))
      {
        PdfDictionary pdfDictionary2 = this.CrossTable.GetObject((IPdfPrimitive) this.CrossTable.GetReference(pdfDictionary1["N"])) as PdfDictionary;
        string itemValue = this.GetItemValue();
        if (pdfDictionary2.ContainsKey(itemValue))
        {
          PdfReference reference = this.CrossTable.GetReference(pdfDictionary2[itemValue]);
          pdfDictionary2.Remove(this.Value);
          pdfDictionary2.SetProperty(str, (IPdfPrimitive) new PdfReferenceHolder(reference, this.CrossTable));
        }
      }
    }
    if (str == this.Parent.SelectedValue)
      this.Dictionary.SetName("AS", str);
    else
      this.Dictionary.SetName("AS", "Off");
  }

  internal PdfLoadedRadioButtonItem Clone() => (PdfLoadedRadioButtonItem) this.MemberwiseClone();
}
