// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.QueryTableImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Tables;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class QueryTableImpl
{
  private ExternalConnection m_externalConnection;
  private DataBaseProperty m_dataBaseProperty;
  private bool m_adjustColumnWidth = true;
  private bool m_isDeleted;
  private string m_name;
  private QueryTableRefresh m_queryTableRefresh;
  private GrowShrinkType m_growShrinkType;

  public QueryTableImpl(
    IApplication application,
    object parent,
    ExternalConnection ExternalConnection)
  {
    this.m_externalConnection = ExternalConnection;
    if (ExternalConnection.OLEDBConnection != null)
      this.m_dataBaseProperty = (DataBaseProperty) ExternalConnection.OLEDBConnection;
    else if (ExternalConnection.ODBCConnection != null)
      this.m_dataBaseProperty = (DataBaseProperty) ExternalConnection.ODBCConnection;
    this.m_queryTableRefresh = new QueryTableRefresh(this);
  }

  public ExternalConnection ExternalConnection => this.m_externalConnection;

  public bool RefreshOnFileOpen
  {
    get
    {
      return this.m_dataBaseProperty != null ? this.m_dataBaseProperty.RefreshOnFileOpen : throw new ArgumentNullException("Connection is not valid");
    }
    set
    {
      if (this.m_dataBaseProperty == null)
        throw new ArgumentNullException("Connection is not valid");
      this.m_dataBaseProperty.RefreshOnFileOpen = value;
    }
  }

  internal GrowShrinkType GrowShrinkType
  {
    get => this.m_growShrinkType;
    set => this.m_growShrinkType = value;
  }

  public ExcelCommandType CommandType
  {
    get
    {
      return this.m_dataBaseProperty != null ? this.m_dataBaseProperty.CommandType : throw new ArgumentNullException("Connection is not valid");
    }
    set
    {
      if (this.m_dataBaseProperty == null)
        throw new ArgumentNullException("Connection is not valid");
      this.m_dataBaseProperty.CommandType = value;
    }
  }

  public object CommandText
  {
    get
    {
      if (this.m_dataBaseProperty == null)
        throw new ArgumentNullException("Connection is Deleted");
      return this.m_dataBaseProperty.CommandText is string ? this.ConvertToASCII((object) (string) this.m_dataBaseProperty.CommandText) : this.m_dataBaseProperty.CommandText;
    }
    set
    {
      if (this.m_dataBaseProperty == null || this.ConnectionDeleted)
        throw new ArgumentNullException("Connection is not valid");
      this.m_dataBaseProperty.CommandText = value;
    }
  }

  public object ConnectionString
  {
    get
    {
      return this.m_dataBaseProperty != null ? this.m_dataBaseProperty.ConnectionString : throw new ArgumentNullException("Connection is not valid");
    }
    set
    {
      if (this.m_dataBaseProperty == null || this.ConnectionDeleted)
        throw new ArgumentNullException("Connection is not valid");
      this.m_dataBaseProperty.ConnectionString = value;
    }
  }

  public bool BackgroundQuery
  {
    get => this.m_dataBaseProperty.BackgroundQuery;
    set => this.m_dataBaseProperty.BackgroundQuery = value;
  }

  public string Name
  {
    get => this.m_name;
    set
    {
      value = value.Replace(" ", "_");
      this.m_name = value;
    }
  }

  public uint ConncetionId => this.m_externalConnection.ConncetionId;

  public bool AdjustColumnWidth
  {
    get => this.m_adjustColumnWidth;
    set => this.m_adjustColumnWidth = value;
  }

  internal bool ConnectionDeleted => this.m_externalConnection.Deleted;

  internal bool IsDeleted => this.m_isDeleted;

  internal QueryTableRefresh QueryTableRefresh
  {
    get => this.m_queryTableRefresh;
    set => this.m_queryTableRefresh = value;
  }

  public IParameters Parameters
  {
    get
    {
      return this.m_externalConnection != null ? this.m_externalConnection.Parameters : (IParameters) null;
    }
  }

  internal QueryTableImpl Clone(ListObject Obj, WorkbookImpl book, string ConnectionName)
  {
    QueryTableImpl queryTableImpl = (QueryTableImpl) this.MemberwiseClone();
    queryTableImpl.m_externalConnection = queryTableImpl.ExternalConnection.Clone(book, ConnectionName);
    return queryTableImpl;
  }

  public void Delete()
  {
    WorkbookImpl workbook = this.ExternalConnection.Range.Worksheet.Workbook as WorkbookImpl;
    IWorksheet worksheet = this.ExternalConnection.Range.Worksheet;
    if (worksheet.Names.Contains(this.Name))
      worksheet.Names.Remove(this.Name);
    else
      workbook.Names.Remove(this.Name);
    if (!this.ConnectionDeleted)
      workbook.DeleteConnection((IConnection) this.ExternalConnection);
    this.m_isDeleted = true;
    this.ExternalConnection.Dispose();
    this.m_externalConnection = (ExternalConnection) null;
    this.m_dataBaseProperty = (DataBaseProperty) null;
    this.QueryTableRefresh.QueryFields = (List<QueryTableField>) null;
    this.QueryTableRefresh = (QueryTableRefresh) null;
  }

  private object ConvertToASCII(object value)
  {
    return (object) ((string) value).Replace("_x000a_", "\n").Replace("_x000d_", "\r").Replace("_x0009_", "\t").Replace("_x0008_", "\b").Replace("_x0000_", "\0");
  }
}
