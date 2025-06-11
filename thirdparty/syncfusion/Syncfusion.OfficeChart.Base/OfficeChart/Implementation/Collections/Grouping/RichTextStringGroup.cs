// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Collections.Grouping.RichTextStringGroup
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Collections.Grouping;

internal class RichTextStringGroup : 
  CommonObject,
  IRichTextString,
  IParentApplication,
  IOptimizedUpdate
{
  private RangeGroup m_rangeGroup;
  private string m_rtfText;

  public RichTextStringGroup(IApplication application, object parent)
    : base(application, parent)
  {
    this.FindParents();
  }

  private void FindParents()
  {
    this.m_rangeGroup = this.FindParent(typeof (RangeGroup)) as RangeGroup;
    if (this.m_rangeGroup == null)
      throw new ArgumentOutOfRangeException("parent", "Can't find parent range group.");
  }

  public IRichTextString this[int index] => this.m_rangeGroup[index].RichText;

  public int Count => this.m_rangeGroup.Count;

  public IOfficeFont GetFont(int iPosition) => throw new NotImplementedException();

  public void SetFont(int iStartPos, int iEndPos, IOfficeFont font)
  {
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      this[index].SetFont(iStartPos, iEndPos, font);
  }

  public void ClearFormatting()
  {
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      this[index].ClearFormatting();
  }

  public void Append(string text, IOfficeFont font)
  {
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      this[index].Append(text, font);
  }

  public void Clear()
  {
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      this[index].Clear();
  }

  public string Text
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return (string) null;
      string text = this[0].Text;
      for (int index = 1; index < count; ++index)
      {
        if (text != this[index].Text)
          return (string) null;
      }
      return text;
    }
    set
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this[index].Text = value;
    }
  }

  public string RtfText
  {
    get
    {
      int count = this.Count;
      if (count == 0)
        return (string) null;
      this.m_rtfText = this[0].RtfText;
      for (int index = 1; index < count; ++index)
      {
        if (this.m_rtfText != this[index].RtfText)
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
      int count = this.Count;
      if (count == 0)
        return false;
      bool isFormatted = this[0].IsFormatted;
      for (int index = 1; index < count; ++index)
      {
        if (isFormatted != this[index].IsFormatted)
          return false;
      }
      return isFormatted;
    }
  }

  public void BeginUpdate()
  {
  }

  public void EndUpdate()
  {
  }
}
