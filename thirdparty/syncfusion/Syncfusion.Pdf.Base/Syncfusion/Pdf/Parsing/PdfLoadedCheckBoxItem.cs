// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PdfLoadedCheckBoxItem
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

public class PdfLoadedCheckBoxItem : PdfLoadedStateItem
{
  public PdfCheckBoxStyle Style
  {
    get => this.Field.ObtainStyle();
    set
    {
      this.Field.AssignStyle(value);
      if (this.Field.Form.NeedAppearances)
        return;
      this.Field.Changed = true;
      this.Field.FieldChanged = true;
    }
  }

  internal PdfLoadedCheckBoxItem(PdfLoadedStyledField field, int index, PdfDictionary dictionary)
    : base(field, index, dictionary)
  {
  }

  private void SetCheckedStatus(bool value)
  {
    bool flag = value;
    string itemValue = PdfLoadedStateField.GetItemValue(this.Dictionary, this.CrossTable);
    if (flag)
    {
      (this.Parent as PdfLoadedCheckBoxField).UncheckOthers((PdfLoadedStateItem) this, itemValue, value);
      this.Parent.Dictionary.SetName("V", itemValue);
      this.Dictionary.SetProperty("AS", (IPdfPrimitive) new PdfName(itemValue));
    }
    else
    {
      PdfName pdfName = PdfCrossTable.Dereference(this.Parent.Dictionary["V"]) as PdfName;
      if (pdfName != (PdfName) null && itemValue == pdfName.Value)
        this.Parent.Dictionary.Remove("V");
      this.Dictionary.SetProperty("AS", (IPdfPrimitive) new PdfName("Off"));
    }
    this.Parent.Changed = true;
  }

  internal PdfLoadedCheckBoxItem Clone() => (PdfLoadedCheckBoxItem) this.MemberwiseClone();
}
