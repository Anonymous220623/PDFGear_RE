// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.TopBottomWrapper
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class TopBottomWrapper : ITopBottom, IOptimizedUpdate
{
  private TopBottomImpl m_wrapped;
  private ConditionalFormatWrapper m_format;

  public ExcelCFTopBottomType Type
  {
    get => this.m_wrapped.Type;
    set
    {
      this.BeginUpdate();
      this.m_wrapped.Type = value;
      this.EndUpdate();
    }
  }

  public bool Percent
  {
    get => this.m_wrapped.Percent;
    set
    {
      this.BeginUpdate();
      this.m_wrapped.Percent = value;
      this.EndUpdate();
    }
  }

  public int Rank
  {
    get => this.m_wrapped.Rank;
    set
    {
      this.BeginUpdate();
      this.m_wrapped.Rank = value;
      this.EndUpdate();
    }
  }

  public void BeginUpdate()
  {
    this.m_format.BeginUpdate();
    this.m_wrapped = this.m_format.GetCondition().TopBottom as TopBottomImpl;
  }

  public void EndUpdate() => this.m_format.EndUpdate();

  public TopBottomWrapper(TopBottomImpl top10, ConditionalFormatWrapper format)
  {
    this.m_wrapped = top10;
    this.m_format = format;
  }
}
