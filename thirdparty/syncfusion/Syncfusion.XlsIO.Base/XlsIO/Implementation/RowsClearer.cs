// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.RowsClearer
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Interfaces;
using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

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
