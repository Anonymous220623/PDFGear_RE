// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Tables.PdfRowCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Tables;

public class PdfRowCollection : PdfCollection
{
  public PdfRow this[int index] => this.List[index] as PdfRow;

  internal PdfRowCollection()
  {
  }

  public void Add(PdfRow row) => this.List.Add((object) row);

  public void Add(object[] values) => this.List.Add((object) new PdfRow(values));
}
