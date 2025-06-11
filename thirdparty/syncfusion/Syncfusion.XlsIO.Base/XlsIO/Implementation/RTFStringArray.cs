// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.RTFStringArray
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;
using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class RTFStringArray : 
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

  public IFont GetFont(int iPosition)
  {
    IRange[] cells = this.m_range.Cells;
    int length = cells.Length;
    if (length == 0)
      return (IFont) null;
    if (!cells[0].HasRichText)
      return (IFont) null;
    IFont font = cells[0].RichText.GetFont(iPosition);
    for (int index = 1; index < length; ++index)
    {
      if (!cells[index].HasRichText)
        return (IFont) null;
      if (font != cells[index].RichText.GetFont(iPosition))
        return (IFont) null;
    }
    return font;
  }

  public void SetFont(int iStartPos, int iEndPos, IFont font)
  {
    IRange[] cells = this.m_range.Cells;
    int index = 0;
    for (int length = cells.Length; index < length; ++index)
    {
      if (!this.m_range.IsMerged || index < 1)
        cells[index].RichText.SetFont(iStartPos, iEndPos, font);
      else
        cells[index].Value = (string) null;
    }
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

  public void Append(string text, IFont font)
  {
    IRange[] cells = this.m_range.Cells;
    int index = 0;
    for (int length = cells.Length; index < length; ++index)
      cells[index].RichText.Append(text, font);
  }

  internal IRange RtfRange
  {
    get => this.m_range;
    set => this.m_range = value;
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

  public IApplication Application => this.m_range.Application;

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
