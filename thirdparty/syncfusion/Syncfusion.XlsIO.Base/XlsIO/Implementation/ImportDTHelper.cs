// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.ImportDTHelper
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Data;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class ImportDTHelper
{
  private bool m_isFieldNameShown;
  private DataTable m_dataTable;
  private int m_firstColumn;
  private int m_firstRow;
  private int m_dateStyleIndex;
  private bool m_isLoading;

  internal bool IsLoading
  {
    get => this.m_isLoading;
    set => this.m_isLoading = value;
  }

  internal int FirstColumn => this.m_firstColumn;

  internal DataTable DataTable => this.m_dataTable;

  internal int FirstRow => this.m_firstRow;

  internal int DateStyleIndex => this.m_dateStyleIndex;

  internal bool IsFieldNameShown => this.m_isFieldNameShown;

  internal ImportDTHelper(
    DataTable dt,
    int firstRow,
    int firstColumn,
    int dateStyleIndex,
    bool isFieldNameShown)
  {
    this.m_dataTable = dt;
    this.m_firstColumn = firstColumn;
    this.m_firstRow = firstRow;
    this.m_dateStyleIndex = dateStyleIndex;
    this.m_isFieldNameShown = isFieldNameShown;
  }
}
