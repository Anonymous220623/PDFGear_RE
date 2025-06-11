// Decompiled with JetBrains decompiler
// Type: pdfconverter.Models.ConnectModel
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

#nullable disable
namespace pdfconverter.Models;

public class ConnectModel
{
  public string appVersion { get; set; }

  public string utcTimestamp { get; set; }

  public string convertType { get; set; }

  public string uuid { get; set; }

  public int convertCountToday { get; set; }

  public string fileName { get; set; }

  public long fileSize { get; set; }

  public int pageCount { get; set; }

  public int pageFrom { get; set; }

  public int pageTo { get; set; }

  public bool needOcr { get; set; }

  public string OcrLang { get; set; }
}
