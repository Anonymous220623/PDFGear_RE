// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.CellDataImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser.Biff_Records;
using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class CellDataImpl
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
