// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.PageNumbers
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class PageNumbers : FormatBase
{
  internal const int ChapterPageSeparatorKey = 51;
  internal const int HeadingLevelForChapterKey = 52;

  public ChapterPageSeparatorType ChapterPageSeparator
  {
    get => (ChapterPageSeparatorType) this.GetPropertyValue(51);
    set => this.SetPropertyValue(51, (object) value);
  }

  public HeadingLevel HeadingLevelForChapter
  {
    get => (HeadingLevel) this.GetPropertyValue(52);
    set => this.SetPropertyValue(52, (object) value);
  }

  internal PageNumbers Clone()
  {
    PageNumbers pageNumbers = new PageNumbers();
    pageNumbers.ImportContainer((FormatBase) this);
    pageNumbers.CopyProperties((FormatBase) this);
    return pageNumbers;
  }

  internal object GetPropertyValue(int propKey) => this[propKey];

  internal bool Compare(PageNumbers pageNumbers)
  {
    return this.Compare(52, (FormatBase) pageNumbers) && this.Compare(51, (FormatBase) pageNumbers);
  }

  internal void SetPropertyValue(int propKey, object value)
  {
    this[propKey] = value;
    this.OnStateChange((object) this);
  }

  public PageNumbers()
  {
    this.SetPropertyValue(51, (object) ChapterPageSeparatorType.Hyphen);
    this.SetPropertyValue(52, (object) HeadingLevel.None);
  }

  protected override object GetDefValue(int key)
  {
    switch (key)
    {
      case 51:
        return (object) ChapterPageSeparatorType.Hyphen;
      case 52:
        return (object) HeadingLevel.None;
      default:
        throw new ArgumentException("key not found");
    }
  }
}
