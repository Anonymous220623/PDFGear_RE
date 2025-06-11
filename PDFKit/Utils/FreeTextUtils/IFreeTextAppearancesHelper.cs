// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.FreeTextUtils.IFreeTextAppearancesHelper
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf.Net.Annotations;
using System.Threading.Tasks;
using System.Windows.Controls;

#nullable disable
namespace PDFKit.Utils.FreeTextUtils;

internal interface IFreeTextAppearancesHelper
{
  void RegenerateAppearancesWithRichText(PdfFreeTextAnnotation annot, bool force);

  Task RegenerateAppearancesWithRichTextAsync(PdfFreeTextAnnotation annot);

  Task RegenerateAppearancesWithRichTextAsync(PdfFreeTextAnnotation annot, RichTextBox rtb);
}
