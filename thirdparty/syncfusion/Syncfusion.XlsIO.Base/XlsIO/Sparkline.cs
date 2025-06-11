// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Sparkline
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO;

public class Sparkline : ISparkline
{
  private IRange m_dataRange;
  private IRange m_referenceRange;
  private ISparklines m_parent;

  public IRange DataRange
  {
    get => this.m_dataRange;
    set
    {
      if (value != null && value.LastRow - value.Row == 1 && value.LastColumn - value.Column == 1)
        throw new ArgumentOutOfRangeException(nameof (DataRange), "The range should not exceed single row.");
      this.m_dataRange = value;
    }
  }

  public IRange ReferenceRange
  {
    get => this.m_referenceRange;
    set
    {
      if (value.Rows.Length != 1 || value.Columns.Length != 1)
        throw new ArgumentOutOfRangeException(nameof (ReferenceRange), "Location reference is not valid because the cells are not all in the same column or row.");
      this.m_referenceRange = value;
    }
  }

  public int Column => this.m_dataRange.Column;

  public int Row => this.m_dataRange.Row;

  internal ISparklines Parent
  {
    get => this.m_parent;
    set => this.m_parent = value;
  }
}
