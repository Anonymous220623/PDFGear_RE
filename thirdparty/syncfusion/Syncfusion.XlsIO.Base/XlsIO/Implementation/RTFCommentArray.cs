// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.RTFCommentArray
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;
using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class RTFCommentArray : CommonObject, IRichTextString, IOptimizedUpdate, IParentApplication
{
  private IRange m_range;
  private string m_rtfText;

  public RTFCommentArray(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParents();
  }

  public IFont GetFont(int iPosition)
  {
    IRange[] cells = this.m_range.Cells;
    bool flag = true;
    IFont font = (IFont) null;
    int index = 0;
    for (int length = cells.Length; index < length; ++index)
    {
      if (cells[index].Comment != null)
      {
        if (flag)
          font = cells[index].Comment.RichText.GetFont(iPosition);
        else if (!font.Equals((object) cells[index].Comment.RichText.GetFont(iPosition)))
          return (IFont) null;
      }
    }
    return font;
  }

  public void SetFont(int iStartPos, int iEndPos, IFont font)
  {
    IRange[] cells = this.m_range.Cells;
    int index = 0;
    for (int length = cells.Length; index < length; ++index)
    {
      if (cells[index].Comment != null)
        cells[index].Comment.RichText.SetFont(iStartPos, iEndPos, font);
    }
  }

  public void ClearFormatting()
  {
    IRange[] cells = this.m_range.Cells;
    int index = 0;
    for (int length = cells.Length; index < length; ++index)
    {
      if (cells[index].Comment != null)
        cells[index].Comment.RichText.ClearFormatting();
    }
  }

  public void Append(string text, IFont font)
  {
    IRange[] cells = this.m_range.Cells;
    int index = 0;
    for (int length = cells.Length; index < length; ++index)
    {
      if (cells[index].Comment != null)
        cells[index].Comment.RichText.Append(text, font);
    }
  }

  public void Clear()
  {
    IRange[] cells = this.m_range.Cells;
    int index = 0;
    for (int length = cells.Length; index < length; ++index)
    {
      if (cells[index].Comment != null)
        cells[index].Comment.RichText.Clear();
    }
  }

  public string Text
  {
    get
    {
      IRange[] cells = this.m_range.Cells;
      bool flag = true;
      string text = (string) null;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].Comment != null)
        {
          if (flag)
            text = cells[index].Comment.RichText.Text;
          else if (text != cells[index].Comment.RichText.Text)
            return (string) null;
        }
      }
      return text;
    }
    set
    {
      IRange[] cells = this.m_range.Cells;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
        cells[index].AddComment().RichText.Text = value;
    }
  }

  public string RtfText
  {
    get
    {
      IRange[] cells = this.m_range.Cells;
      bool flag = true;
      this.m_rtfText = (string) null;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].Comment != null)
        {
          if (flag)
            this.m_rtfText = cells[index].Comment.RichText.RtfText;
          else if (this.m_rtfText != cells[index].Comment.RichText.RtfText)
            return (string) null;
        }
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
      bool flag = true;
      bool isFormatted = false;
      int index = 0;
      for (int length = cells.Length; index < length; ++index)
      {
        if (cells[index].Comment != null)
        {
          if (flag)
            isFormatted = cells[index].Comment.RichText.IsFormatted;
          else if (isFormatted != cells[index].Comment.RichText.IsFormatted)
            return false;
        }
      }
      return isFormatted;
    }
  }

  private void SetParents()
  {
    this.m_range = this.FindParent(typeof (IRange)) as IRange;
    if (this.m_range == null)
      throw new ArgumentNullException("Can't find parent range");
  }

  public void BeginUpdate()
  {
  }

  public void EndUpdate()
  {
  }
}
