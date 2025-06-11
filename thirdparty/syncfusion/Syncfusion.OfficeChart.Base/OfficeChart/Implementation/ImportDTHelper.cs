// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.ImportDTHelper
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System.Data;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class ImportDTHelper
{
  private DataTable m_dataTable;
  private int m_firstColumn;
  private int m_firstRow;
  private int m_dateStyleIndex;

  internal int FirstColumn => this.m_firstColumn;

  internal DataTable DataTable => this.m_dataTable;

  internal int FirstRow => this.m_firstRow;

  internal int DateStyleIndex => this.m_dateStyleIndex;

  internal ImportDTHelper(DataTable dt, int firstRow, int firstColumn, int dateStyleIndex)
  {
    this.m_dataTable = dt;
    this.m_firstColumn = firstColumn;
    this.m_firstRow = firstRow;
    this.m_dateStyleIndex = dateStyleIndex;
  }
}
