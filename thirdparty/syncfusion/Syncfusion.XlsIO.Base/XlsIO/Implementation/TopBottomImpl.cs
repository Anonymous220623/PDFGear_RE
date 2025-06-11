// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.TopBottomImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class TopBottomImpl : ITopBottom
{
  private ExcelCFTopBottomType m_type;
  private bool m_bPercent;
  private int m_rank = 10;

  public TopBottomImpl Clone() => (TopBottomImpl) this.MemberwiseClone();

  public static bool operator ==(TopBottomImpl first, TopBottomImpl second)
  {
    if ((object) first == null && (object) second == null)
      return true;
    return (object) first != null && (object) second != null && first.m_type == second.m_type && first.m_bPercent == second.m_bPercent && first.m_rank == second.m_rank;
  }

  public static bool operator !=(TopBottomImpl first, TopBottomImpl second) => !(first == second);

  public ExcelCFTopBottomType Type
  {
    get => this.m_type;
    set => this.m_type = value;
  }

  public bool Percent
  {
    get => this.m_bPercent;
    set => this.m_bPercent = value;
  }

  public int Rank
  {
    get => this.m_rank;
    set
    {
      if (this.m_bPercent && (value < 1 || value > 100))
        throw new ArgumentException("Rank must be between 1 and 100");
      this.m_rank = value >= 1 && value <= 1000 ? value : throw new ArgumentException("Rank must be between 1 and 1000");
    }
  }
}
