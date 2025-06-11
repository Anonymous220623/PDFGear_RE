// Decompiled with JetBrains decompiler
// Type: pdfconverter.PdfiumPdfRange
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

#nullable disable
namespace pdfconverter;

public class PdfiumPdfRange
{
  public PdfiumPdfRange(string filePath, int startPageIndex, int endPageIndex, string password = null)
  {
    this.FilePath = filePath;
    this.StartPageIndex = startPageIndex;
    this.EndPageIndex = endPageIndex;
    this.Password = password;
  }

  public string FilePath { get; }

  public int StartPageIndex { get; }

  public int EndPageIndex { get; }

  public string Password { get; }
}
