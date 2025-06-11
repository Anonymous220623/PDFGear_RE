// Decompiled with JetBrains decompiler
// Type: TextSearchItem
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
public class TextSearchItem
{
  internal string SearchWord { get; set; }

  internal TextSearchOptions SearchOption { get; set; }

  public TextSearchItem(string searchWord, TextSearchOptions searchOption)
  {
    this.SearchWord = searchWord;
    this.SearchOption = searchOption;
  }
}
