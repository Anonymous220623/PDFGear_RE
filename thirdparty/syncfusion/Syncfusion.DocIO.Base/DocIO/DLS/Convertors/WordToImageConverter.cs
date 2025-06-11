// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Convertors.WordToImageConverter
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.Rendering;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS.Convertors;

internal class WordToImageConverter
{
  public WordToImageConverter(
    int pageIndex,
    int pageCount,
    Stream stream,
    ImageFormat imageFormat)
  {
    if (imageFormat != ImageFormat.Emf || stream != null)
      return;
    stream = (Stream) new MemoryStream();
  }

  public WordToImageConverter()
  {
  }

  public WordToImageConverter(
    int pageIndex,
    int pageCount,
    string fileName,
    ImageFormat imageFormat)
  {
  }

  public Image[] ConvertToImage(WordDocument document, ImageType imageType)
  {
    document.RevisionOptions.ShowMarkup = RevisionType.None;
    document.RevisionOptions.CommentDisplayMode = CommentDisplayMode.Hide;
    return this.ConvertToImage(document, imageType, (MemoryStream) null);
  }

  public Image[] ConvertToImage(WordDocument document, ImageType imageType, MemoryStream stream)
  {
    document.RevisionOptions.ShowMarkup = RevisionType.None;
    document.RevisionOptions.CommentDisplayMode = CommentDisplayMode.Hide;
    DocumentLayouter documentLayouter = new DocumentLayouter();
    documentLayouter.Layout((IWordDocument) document);
    return documentLayouter.DrawToImage(0, -1, imageType, stream);
  }

  public Stream ConvertToImage(int pageIndex, WordDocument document, ImageFormat imageFormat)
  {
    document.RevisionOptions.ShowMarkup = RevisionType.None;
    document.RevisionOptions.CommentDisplayMode = CommentDisplayMode.Hide;
    MemoryStream image1 = new MemoryStream();
    DocumentLayouter documentLayouter = new DocumentLayouter();
    documentLayouter.Layout((IWordDocument) document);
    imageFormat = imageFormat != ImageFormat.Emf || !DocumentLayouter.IsAzureCompatible ? imageFormat : ImageFormat.Bmp;
    if (imageFormat == ImageFormat.Emf && !DocumentLayouter.IsAzureCompatible)
      return documentLayouter.DrawToStream(pageIndex, 1, ImageType.Metafile, (MemoryStream) null);
    Image[] image2 = documentLayouter.DrawToImage(pageIndex, 1, ImageType.Metafile, (MemoryStream) null);
    if (image2 == null || image2[0] == null)
      return (Stream) null;
    image2[0].Save((Stream) image1, imageFormat);
    return (Stream) image1;
  }

  public Image[] ConvertToImage(
    int pageIndex,
    int noOfPages,
    WordDocument document,
    ImageType imageType)
  {
    document.RevisionOptions.ShowMarkup = RevisionType.None;
    document.RevisionOptions.CommentDisplayMode = CommentDisplayMode.Hide;
    DocumentLayouter documentLayouter = new DocumentLayouter();
    documentLayouter.Layout((IWordDocument) document);
    return documentLayouter.DrawToImage(pageIndex, noOfPages, imageType, (MemoryStream) null);
  }
}
