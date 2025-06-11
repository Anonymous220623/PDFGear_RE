// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PdfLoadedStateItem
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

public class PdfLoadedStateItem : PdfLoadedFieldItem
{
  public bool Checked
  {
    get
    {
      bool flag = false;
      PdfName pdfName1 = PdfCrossTable.Dereference(this.Dictionary["AS"]) as PdfName;
      if (pdfName1 == (PdfName) null)
      {
        PdfName pdfName2 = PdfLoadedField.GetValue(this.Parent.Dictionary, this.Parent.CrossTable, "V", false) as PdfName;
        if (pdfName2 != (PdfName) null)
          flag = pdfName2.Value == PdfLoadedStateField.GetItemValue(this.Dictionary, this.CrossTable);
      }
      else
        flag = pdfName1.Value != "Off";
      return flag;
    }
    set
    {
      if ((FieldFlags.ReadOnly & this.Field.Flags) != FieldFlags.Default || value == this.Checked)
        return;
      this.SetCheckedStatus(value);
      ((PdfField) this.Field).Form.SetAppearanceDictionary = true;
    }
  }

  public PdfColor BackColor
  {
    get => this.GetBackColor();
    set
    {
      PdfDictionary widgetAnnotation = this.Field.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
      if (widgetAnnotation != null)
      {
        if (widgetAnnotation.ContainsKey("MK"))
        {
          (this.CrossTable.GetObject(widgetAnnotation["MK"]) as PdfDictionary)["BG"] = (IPdfPrimitive) value.ToArray();
        }
        else
        {
          PdfDictionary pdfDictionary = new PdfDictionary();
          PdfArray array = value.ToArray();
          pdfDictionary["BG"] = (IPdfPrimitive) array;
          widgetAnnotation["MK"] = (IPdfPrimitive) pdfDictionary;
        }
        ((PdfField) this.Field).Form.SetAppearanceDictionary = true;
      }
      if (this.Field.Form.NeedAppearances)
        return;
      this.Field.Changed = true;
      this.Field.FieldChanged = true;
    }
  }

  public PdfColor ForeColor
  {
    get
    {
      PdfDictionary widgetAnnotation = this.Field.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
      PdfColor foreColor = new PdfColor((byte) 0, (byte) 0, (byte) 0);
      if (widgetAnnotation != null && widgetAnnotation.ContainsKey("DA"))
        foreColor = this.Field.GetForeColour((this.CrossTable.GetObject(widgetAnnotation["DA"]) as PdfString).Value);
      else if (widgetAnnotation != null && widgetAnnotation.GetValue(this.CrossTable, "DA", "Parent") is PdfString pdfString)
        foreColor = this.Field.GetForeColour(pdfString.Value);
      return foreColor;
    }
    set
    {
      PdfDictionary widgetAnnotation = this.Field.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
      float height = 0.0f;
      string str = (string) null;
      if (widgetAnnotation != null && widgetAnnotation.ContainsKey("DA"))
        str = this.Field.FontName((widgetAnnotation["DA"] as PdfString).Value, out height);
      else if (widgetAnnotation != null && this.Dictionary.ContainsKey("DA"))
        str = this.Field.FontName((this.Dictionary["DA"] as PdfString).Value, out height);
      if (str != null)
        widgetAnnotation["DA"] = (IPdfPrimitive) new PdfString(new PdfDefaultAppearance()
        {
          FontName = str,
          FontSize = height,
          ForeColor = value
        }.ToString());
      else
        widgetAnnotation["DA"] = (IPdfPrimitive) new PdfString(new PdfDefaultAppearance()
        {
          FontName = this.Font.Name,
          FontSize = this.Font.Size,
          ForeColor = value
        }.ToString());
      ((PdfField) this.Field).Form.SetAppearanceDictionary = true;
      if (this.Field.Form.NeedAppearances)
        return;
      this.Field.Changed = true;
      this.Field.FieldChanged = true;
    }
  }

  internal PdfLoadedStateItem(PdfLoadedStyledField field, int index, PdfDictionary dictionary)
    : base(field, index, dictionary)
  {
  }

  private void SetCheckedStatus(bool value)
  {
    bool flag = value;
    string name = PdfLoadedStateField.GetItemValue(this.Dictionary, this.CrossTable);
    (this.Parent as PdfLoadedStateField).UncheckOthers(this, name, value);
    if (flag)
    {
      if (name == null || name == string.Empty)
        name = "Yes";
      this.Parent.Dictionary.SetName("V", name);
      this.Dictionary.SetProperty("AS", (IPdfPrimitive) new PdfName(name));
      this.Dictionary.SetProperty("V", (IPdfPrimitive) new PdfName(name));
    }
    else
    {
      PdfName pdfName = PdfCrossTable.Dereference(this.Parent.Dictionary["V"]) as PdfName;
      if (pdfName != (PdfName) null && name == pdfName.Value)
        this.Parent.Dictionary.Remove("V");
      this.Dictionary.SetProperty("AS", (IPdfPrimitive) new PdfName("Off"));
    }
    this.Parent.Changed = true;
  }

  internal PdfColor GetBackColor()
  {
    PdfDictionary widgetAnnotation = this.Field.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
    PdfColor backColor = new PdfColor();
    if (widgetAnnotation.ContainsKey("MK"))
    {
      PdfDictionary pdfDictionary = this.CrossTable.GetObject(widgetAnnotation["MK"]) as PdfDictionary;
      if (pdfDictionary.ContainsKey("BG"))
        backColor = this.CreateColor(pdfDictionary["BG"] as PdfArray);
    }
    return backColor;
  }

  private PdfColor CreateColor(PdfArray array)
  {
    int count1 = array.Count;
    PdfColor color = PdfColor.Empty;
    float[] numArray = new float[array.Count];
    int index = 0;
    for (int count2 = array.Count; index < count2; ++index)
    {
      PdfNumber pdfNumber = this.CrossTable.GetObject(array[index]) as PdfNumber;
      numArray[index] = pdfNumber.FloatValue;
    }
    switch (count1)
    {
      case 1:
        color = (double) numArray[0] <= 0.0 || (double) numArray[0] > 1.0 ? new PdfColor((float) (byte) numArray[0]) : new PdfColor(numArray[0]);
        break;
      case 3:
        color = (double) numArray[0] > 0.0 && (double) numArray[0] <= 1.0 || (double) numArray[1] > 0.0 && (double) numArray[1] <= 1.0 || (double) numArray[2] > 0.0 && (double) numArray[2] <= 1.0 ? new PdfColor(numArray[0], numArray[1], numArray[2]) : new PdfColor((byte) numArray[0], (byte) numArray[1], (byte) numArray[2]);
        break;
      case 4:
        color = (double) numArray[0] > 0.0 && (double) numArray[0] <= 1.0 || (double) numArray[1] > 0.0 && (double) numArray[1] <= 1.0 || (double) numArray[2] > 0.0 && (double) numArray[2] <= 1.0 || (double) numArray[3] > 0.0 && (double) numArray[3] <= 1.0 ? new PdfColor(numArray[0], numArray[1], numArray[2], numArray[3]) : new PdfColor((byte) numArray[0], (byte) numArray[1], (byte) numArray[2], (byte) numArray[3]);
        break;
    }
    return color;
  }
}
