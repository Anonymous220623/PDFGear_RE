// Decompiled with JetBrains decompiler
// Type: pdfconverter.Models.CompressItemCollection
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using System.Collections.ObjectModel;

#nullable disable
namespace pdfconverter.Models;

public class CompressItemCollection : ObservableCollection<CompressItem>
{
  protected override void ClearItems() => base.ClearItems();
}
