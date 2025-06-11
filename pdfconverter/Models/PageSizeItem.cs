// Decompiled with JetBrains decompiler
// Type: pdfconverter.Models.PageSizeItem
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using Microsoft.Toolkit.Mvvm.ComponentModel;

#nullable disable
namespace pdfconverter.Models;

public class PageSizeItem : ObservableObject
{
  private string capital;
  private PDFPageSize pdfPageSize;

  public PageSizeItem(string cap, PDFPageSize pagesize)
  {
    this.capital = cap;
    this.pdfPageSize = pagesize;
  }

  public PDFPageSize PDFPageSize => this.pdfPageSize;

  public string Capital => this.capital;

  public override string ToString() => this.Capital;
}
