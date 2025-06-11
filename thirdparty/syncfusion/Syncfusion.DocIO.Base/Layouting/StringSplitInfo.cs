// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.StringSplitInfo
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.Layouting;

internal class StringSplitInfo
{
  private int m_firstPos;
  private int m_lastPos;

  public int FirstPos => this.m_firstPos;

  public int LastPos => this.m_lastPos;

  public int Length => this.LastPos - this.FirstPos + 1;

  private StringSplitInfo()
  {
  }

  public StringSplitInfo(int firstPos, int lastPos)
  {
    if (firstPos < 0)
      throw new ArgumentException(nameof (firstPos));
    this.m_lastPos = firstPos <= lastPos ? lastPos : throw new ArgumentException(nameof (lastPos));
    this.m_firstPos = firstPos;
  }

  public void Check(int length)
  {
    if (this.m_firstPos < 0 || this.m_firstPos > length)
      throw new ArgumentOutOfRangeException("SplitInfo.FirstPos");
    if (this.m_lastPos < this.m_firstPos || this.m_lastPos > length)
      throw new ArgumentOutOfRangeException("SplitInfo.LastPos");
  }

  public void Extend(StringSplitInfo strSplitInfo)
  {
    this.m_firstPos += strSplitInfo.FirstPos;
    this.m_lastPos += strSplitInfo.FirstPos;
  }

  public StringSplitInfo GetSplitFirstPart(int position)
  {
    return new StringSplitInfo(this.m_firstPos, this.m_firstPos + position - 1);
  }

  public StringSplitInfo GetSplitSecondPart(int position)
  {
    return new StringSplitInfo(this.m_firstPos + position, this.m_lastPos);
  }

  public string GetSubstring(string text)
  {
    return text.Substring(this.m_firstPos, this.m_lastPos - this.m_firstPos + 1);
  }
}
