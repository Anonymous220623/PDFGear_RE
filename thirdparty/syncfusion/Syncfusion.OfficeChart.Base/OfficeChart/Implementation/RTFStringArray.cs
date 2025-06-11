// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.RTFStringArray
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class RTFStringArray : 
  IRTFWrapper,
  IDisposable,
  IRichTextString,
  IParentApplication,
  IOptimizedUpdate
{
  private IRange m_range;
  private string m_rtfText;

  private RTFStringArray()
  {
  }

  public RTFStringArray(IRange range)
  {
    this.m_range = range != null ? range : throw new ArgumentNullException(nameof (range));
  }

  public IOfficeFont GetFont(int iPosition)
  {
    IRange[] cells = this.m_range.Cells;
    int length = cells.Length;
    if (length == 0)
      return (IOfficeFont) null;
    if (!cells[0].HasRichText)
      return (IOfficeFont) null;
    IOfficeFont font = cells[0].RichText.GetFont(iPosition);
    for (int index = 1; index < length; ++index)
    {
      if (!cells[index].HasRichText)
        return (IOfficeFont) null;
      if (font != cells[index].RichText.GetFont(iPosition))
        return (IOfficeFont) null;
    }
    return font;
  }

  public void SetFont(int iStartPos, int iEndPos, IOfficeFont font)
  {
    IRange[] cells = this.m_range.Cells;
    int index = 0;
    for (int length = cells.Length; index < length; ++index)
      cells[index].RichText.SetFont(iStartPos, iEndPos, font);
  }

  public void ClearFormatting()
  {
    IRange[] cells = this.m_range.Cells;
    int index = 0;
    for (int length = cells.Length; index < length; ++index)
    {
      if (cells[index].HasRichText)
        cells[index].RichText.ClearFormatting();
    }
  }

  public void Append(string text, IOfficeFont font)
  {
    IRange[] cells = this.m_range.Cells;
    int index = 0;
    for (int length = cells.Length; index < length; ++index)
    {
      if (cells[index].HasRichText)
        cells[index].RichText.Append(text, font);
    }
  }

  public string Text
  {
    get
    {
      IRange[] cells = this.m_range.Cells;
      int length = cells.Length;
      if (length == 0)
        return (string) null;
      string text = cells[0].Text;
      if (text != null)
      {
        for (int index = 1; index < length; ++index)
        {
          if (text != cells[index].Text)
          {
            text = (string) null;
            break;
          }
        }
      }
      return text;
    }
    set
    {
      IRange[] cells = this.m_range.Cells;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
        cells[index].RichText.Text = value;
    }
  }

  public string RtfText
  {
    get
    {
      IRange[] cells = this.m_range.Cells;
      int length = cells.Length;
      if (length == 0)
        return (string) null;
      if (!cells[0].HasRichText)
        return (string) null;
      this.m_rtfText = cells[0].RichText.RtfText;
      for (int index = 1; index < length; ++index)
      {
        if (!cells[index].HasRichText)
          return (string) null;
        if (this.m_rtfText != cells[index].RichText.RtfText)
          return (string) null;
      }
      return this.m_rtfText;
    }
    set => this.m_rtfText = value;
  }

  public bool IsFormatted
  {
    get
    {
      IRange[] cells = this.m_range.Cells;
      int length = cells.Length;
      if (length == 0)
        return false;
      for (int index = 0; index < length; ++index)
      {
        if (!cells[index].HasRichText || !cells[index].RichText.IsFormatted)
          return false;
      }
      return true;
    }
  }

  public IApplication Application => (this.m_range as RangeImpl).Application;

  public object Parent => (object) this.m_range;

  public void BeginUpdate()
  {
  }

  public void EndUpdate()
  {
  }

  public void Dispose()
  {
  }

  public void Clear()
  {
    IRange[] cells = this.m_range.Cells;
    int index = 0;
    for (int length = cells.Length; index < length; ++index)
    {
      if (cells[index].HasRichText)
        ((RichTextString) cells[index].RichText).Clear();
    }
  }
}
