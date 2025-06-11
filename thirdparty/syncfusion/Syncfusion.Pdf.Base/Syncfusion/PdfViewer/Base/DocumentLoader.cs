// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.DocumentLoader
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Parsing;
using System.IO;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal sealed class DocumentLoader
{
  private static DocumentLoader s_instance;
  private static readonly object s_lock = new object();

  public static DocumentLoader Instance
  {
    get
    {
      if (DocumentLoader.s_instance == null)
      {
        lock (DocumentLoader.s_lock)
          DocumentLoader.s_instance = new DocumentLoader();
      }
      return DocumentLoader.s_instance;
    }
  }

  public PdfLoadedDocument Load(string filePath) => new PdfLoadedDocument(filePath);

  public PdfLoadedDocument Load(string filePath, string password)
  {
    return new PdfLoadedDocument(filePath, password);
  }

  public PdfLoadedDocument Load(Stream stream) => new PdfLoadedDocument(stream);
}
