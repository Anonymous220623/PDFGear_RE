// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.MailMergeDataSet
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class MailMergeDataSet
{
  private string DEF_GROUPNAME_PROPERTY = "GroupName";
  private string DEF_SOURCEDATA_PROPERTY = "SourceData";
  private List<object> m_dataSet;

  public List<object> DataSet => this.m_dataSet;

  public MailMergeDataSet() => this.m_dataSet = new List<object>();

  public void Add(object dataTable) => this.m_dataSet.Add(dataTable);

  public void Clear()
  {
    this.m_dataSet.Clear();
    this.m_dataSet = (List<object>) null;
  }

  internal MailMergeDataTable GetDataTable(string tableName)
  {
    foreach (object data in this.m_dataSet)
    {
      Type type = data.GetType();
      string groupName = type.GetProperty(this.DEF_GROUPNAME_PROPERTY).GetValue(data, (object[]) null).ToString();
      if (!string.IsNullOrEmpty(groupName) && groupName == tableName)
      {
        IEnumerator enumerator = type.GetProperty(this.DEF_SOURCEDATA_PROPERTY).GetValue(data, (object[]) null) as IEnumerator;
        MailMergeDataTable dataTable = (MailMergeDataTable) null;
        if (enumerator != null)
          dataTable = new MailMergeDataTable(groupName, enumerator);
        return dataTable;
      }
    }
    return (MailMergeDataTable) null;
  }

  internal void RemoveDataTable(string tableName)
  {
    foreach (object data in this.m_dataSet)
    {
      string str = data.GetType().GetProperty(this.DEF_GROUPNAME_PROPERTY).GetValue(data, (object[]) null).ToString();
      if (!string.IsNullOrEmpty(str) && str == tableName)
      {
        this.m_dataSet.Remove(data);
        break;
      }
    }
  }
}
