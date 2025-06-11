// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.RowsClearer
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Collections;
using Syncfusion.OfficeChart.Interfaces;
using System;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class RowsClearer : IOperation
{
  private WorksheetImpl m_sheet;
  private int m_iIndex;
  private int m_iCount;

  public RowsClearer(WorksheetImpl sheet, int index, int count)
  {
    this.m_sheet = sheet != null ? sheet : throw new ArgumentNullException(nameof (sheet));
    this.m_iIndex = index;
    this.m_iCount = count;
  }

  public void Do()
  {
    ArrayListEx rows = this.m_sheet.CellRecords.Table.Rows;
    int num = 0;
    int index = this.m_iIndex - 1;
    while (num < this.m_iCount)
    {
      RowStorage rowStorage = rows[index];
      rows[index] = (RowStorage) null;
      rowStorage?.Dispose();
      ++num;
      ++index;
    }
  }
}
