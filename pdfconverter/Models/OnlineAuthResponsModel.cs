// Decompiled with JetBrains decompiler
// Type: pdfconverter.Models.OnlineAuthResponsModel
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

#nullable disable
namespace pdfconverter.Models;

public class OnlineAuthResponsModel
{
  public bool Success { get; set; }

  public string Token { get; set; } = string.Empty;

  public string Message { get; set; } = string.Empty;
}
