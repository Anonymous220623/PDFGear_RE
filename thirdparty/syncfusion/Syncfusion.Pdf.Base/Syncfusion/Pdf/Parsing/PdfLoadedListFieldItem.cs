// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PdfLoadedListFieldItem
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

public class PdfLoadedListFieldItem : PdfLoadedFieldItem
{
  internal PdfLoadedListFieldItem(PdfLoadedStyledField field, int index, PdfDictionary dictionary)
    : base(field, index, dictionary)
  {
  }

  internal PdfLoadedListFieldItem Clone() => (PdfLoadedListFieldItem) this.MemberwiseClone();
}
