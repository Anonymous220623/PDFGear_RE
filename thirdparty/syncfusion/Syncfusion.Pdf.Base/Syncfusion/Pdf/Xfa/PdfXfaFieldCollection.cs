// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfXfaFieldCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Xfa;

public class PdfXfaFieldCollection : PdfCollection
{
  private int subFormCount = 1;

  public PdfXfaField this[int offset] => this.List[offset] as PdfXfaField;

  public new int Count => this.List.Count;

  public void Add(PdfXfaField field)
  {
    switch (field)
    {
      case PdfXfaRadioButtonField _:
        throw new PdfException("Can't add single radio button, need to add the radio button in group (PdfXfaRadioButtonGroup).");
      case PdfXfaForm _:
        this.List.Add((field as PdfXfaForm).Clone());
        break;
      default:
        this.List.Add((object) field);
        break;
    }
  }

  public void Remove(PdfXfaField field)
  {
    if (!this.List.Contains((object) field))
      return;
    this.List.Remove((object) field);
  }

  public void RemoveAt(int index) => this.List.RemoveAt(index);

  public void Clear() => this.List.Clear();

  public void insert(int index, PdfXfaField field)
  {
    switch (field)
    {
      case PdfXfaRadioButtonField _:
        throw new PdfException("Can't insert single radio button, need to add the radio button in group (PdfXfaRadioButtonGroup).");
      case PdfXfaForm _:
        this.List.Insert(index, (field as PdfXfaForm).Clone());
        break;
      default:
        this.List.Insert(index, (object) field);
        break;
    }
  }

  public int IndexOf(PdfXfaField field) => this.List.IndexOf((object) field);

  internal object Clone()
  {
    PdfXfaFieldCollection xfaFieldCollection = new PdfXfaFieldCollection();
    foreach (PdfXfaField pdfXfaField in this.List)
    {
      switch (pdfXfaField)
      {
        case PdfXfaForm _:
          xfaFieldCollection.Add((pdfXfaField as PdfXfaForm).Clone() as PdfXfaField);
          continue;
        case PdfXfaRectangleField _:
          xfaFieldCollection.Add((pdfXfaField as PdfXfaRectangleField).Clone() as PdfXfaField);
          continue;
        case PdfXfaCircleField _:
          xfaFieldCollection.Add((pdfXfaField as PdfXfaCircleField).Clone() as PdfXfaField);
          continue;
        case PdfXfaTextBoxField _:
          xfaFieldCollection.Add((pdfXfaField as PdfXfaTextBoxField).Clone() as PdfXfaField);
          continue;
        case PdfXfaNumericField _:
          xfaFieldCollection.Add((pdfXfaField as PdfXfaNumericField).Clone() as PdfXfaField);
          continue;
        case PdfXfaButtonField _:
          xfaFieldCollection.Add((pdfXfaField as PdfXfaButtonField).Clone() as PdfXfaField);
          continue;
        case PdfXfaCheckBoxField _:
          xfaFieldCollection.Add((pdfXfaField as PdfXfaCheckBoxField).Clone() as PdfXfaField);
          continue;
        case PdfXfaDateTimeField _:
          xfaFieldCollection.Add((pdfXfaField as PdfXfaDateTimeField).Clone() as PdfXfaField);
          continue;
        case PdfXfaComboBoxField _:
          xfaFieldCollection.Add((pdfXfaField as PdfXfaComboBoxField).Clone() as PdfXfaField);
          continue;
        case PdfXfaListBoxField _:
          xfaFieldCollection.Add((pdfXfaField as PdfXfaListBoxField).Clone() as PdfXfaField);
          continue;
        case PdfXfaImage _:
          xfaFieldCollection.Add((pdfXfaField as PdfXfaImage).Clone() as PdfXfaField);
          continue;
        case PdfXfaLine _:
          xfaFieldCollection.Add((pdfXfaField as PdfXfaLine).Clone() as PdfXfaField);
          continue;
        case PdfXfaTextElement _:
          xfaFieldCollection.Add((pdfXfaField as PdfXfaTextElement).Clone() as PdfXfaField);
          continue;
        case PdfXfaRadioButtonGroup _:
          xfaFieldCollection.Add((pdfXfaField as PdfXfaRadioButtonGroup).Clone() as PdfXfaField);
          continue;
        default:
          continue;
      }
    }
    return (object) xfaFieldCollection;
  }
}
