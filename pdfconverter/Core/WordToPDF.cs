// Decompiled with JetBrains decompiler
// Type: pdfconverter.Core.WordToPDF
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.OfficeChart;
using Syncfusion.OfficeChartToImageConverter;
using Syncfusion.Pdf;
using System.IO;

#nullable disable
namespace pdfconverter.Core;

public class WordToPDF : IMyInterface
{
  private string SourcePath;

  public string OutFolder { get; set; }

  public WordToPDF(string path) => this.SourcePath = path;

  public void ToPDF()
  {
    WordDocument wordDocument = new WordDocument(this.SourcePath, FormatType.Docx)
    {
      ChartToImageConverter = (IOfficeChartToImageConverter) new ChartToImageConverter()
    };
    wordDocument.ChartToImageConverter.ScalingMode = ScalingMode.Normal;
    PdfDocument pdf = new Syncfusion.DocToPDFConverter.DocToPDFConverter()
    {
      Settings = {
        ImageQuality = 100,
        ImageResolution = 640,
        OptimizeIdenticalImages = true
      }
    }.ConvertToPDF(wordDocument);
    pdf.Save(Path.Combine(this.OutFolder, Path.GetFileNameWithoutExtension(this.SourcePath)));
    pdf.Close(true);
    pdf.Dispose();
    wordDocument.Close();
    wordDocument.Dispose();
  }
}
