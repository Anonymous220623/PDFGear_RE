// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.CellDataImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Parser.Biff_Records;
using System;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class CellDataImpl
{
  private RangeImpl m_range;
  private ICellPositionFormat m_record;

  public RangeImpl Range
  {
    get => this.m_range;
    set => this.m_range = value;
  }

  [CLSCompliant(false)]
  public ICellPositionFormat Record
  {
    get => this.m_record;
    set => this.m_record = value;
  }
}
