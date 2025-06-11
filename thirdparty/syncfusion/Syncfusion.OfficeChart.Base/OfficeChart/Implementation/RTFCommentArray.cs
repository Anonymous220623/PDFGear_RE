// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.RTFCommentArray
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class RTFCommentArray : CommonObject, IRichTextString, IOptimizedUpdate, IParentApplication
{
  private IRange m_range;
  private string m_rtfText;

  public RTFCommentArray(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParents();
  }

  public IOfficeFont GetFont(int iPosition)
  {
    IRange[] cells = this.m_range.Cells;
    return (IOfficeFont) null;
  }

  public void SetFont(int iStartPos, int iEndPos, IOfficeFont font)
  {
    IRange[] cells = this.m_range.Cells;
  }

  public void ClearFormatting()
  {
    IRange[] cells = this.m_range.Cells;
  }

  public void Append(string text, IOfficeFont font)
  {
    IRange[] cells = this.m_range.Cells;
  }

  public void Clear()
  {
    IRange[] cells = this.m_range.Cells;
  }

  public string Text
  {
    get
    {
      IRange[] cells = this.m_range.Cells;
      return (string) null;
    }
    set
    {
      IRange[] cells = this.m_range.Cells;
    }
  }

  public string RtfText
  {
    get
    {
      IRange[] cells = this.m_range.Cells;
      this.m_rtfText = (string) null;
      return this.m_rtfText;
    }
    set => this.m_rtfText = value;
  }

  public bool IsFormatted
  {
    get
    {
      IRange[] cells = this.m_range.Cells;
      return false;
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
