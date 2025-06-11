// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PageRangeReader
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;

#nullable disable
namespace PDFKit.Utils;

internal struct PageRangeReader(string pageRange)
{
  private readonly string pageRange = !string.IsNullOrEmpty(pageRange) ? pageRange : throw new ArgumentException(nameof (pageRange));
  private int curIdx = 0;
  private PageRangeTokenType curType = PageRangeTokenType.Invalid;

  public bool HasMore => this.curIdx < this.pageRange.Length;

  public (PageRangeTokenType type, int value, int startIdx) GetNextToken()
  {
    int num1 = 0;
    int curIdx = this.curIdx;
    for (; this.curIdx < this.pageRange.Length; ++this.curIdx)
    {
      char ch = this.pageRange[this.curIdx];
      if (ch >= '0' && ch <= '9')
      {
        if (this.curType != PageRangeTokenType.Number)
          num1 = 0;
        if (this.curType == PageRangeTokenType.Invalid)
          this.curType = PageRangeTokenType.Number;
        num1 = num1 != 0 ? num1 * 10 + ((int) ch - 48 /*0x30*/) : (int) ch - 48 /*0x30*/;
      }
      else
      {
        int num2;
        switch (ch)
        {
          case ' ':
            if (this.curType != PageRangeTokenType.Number)
            {
              ++curIdx;
              continue;
            }
            goto label_23;
          case ',':
            num2 = 1;
            break;
          default:
            num2 = ch == '，' ? 1 : 0;
            break;
        }
        if (num2 != 0)
        {
          if (this.curType == PageRangeTokenType.Invalid)
          {
            this.curType = PageRangeTokenType.Comma;
            num1 = (int) ch;
            ++this.curIdx;
            break;
          }
          if (this.curType == PageRangeTokenType.Number || this.curType == PageRangeTokenType.Dash)
            break;
        }
        else if (ch == '-')
        {
          if (this.curType == PageRangeTokenType.Invalid)
          {
            this.curType = PageRangeTokenType.Dash;
            num1 = (int) ch;
            ++this.curIdx;
            break;
          }
          if (this.curType == PageRangeTokenType.Number || this.curType == PageRangeTokenType.Comma)
            break;
        }
        else
        {
          this.curType = PageRangeTokenType.Invalid;
          num1 = 0;
          break;
        }
      }
    }
label_23:
    PageRangeTokenType curType = this.curType;
    this.curType = PageRangeTokenType.Invalid;
    return (curType, num1, curIdx);
  }
}
