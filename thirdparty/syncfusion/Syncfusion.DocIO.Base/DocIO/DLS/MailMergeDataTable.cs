// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.MailMergeDataTable
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class MailMergeDataTable
{
  private string m_groupName;
  private IEnumerator m_sourceData;
  private int m_matchingRecordsCount;
  private string m_command;

  public string GroupName => this.m_groupName;

  public IEnumerator SourceData => this.m_sourceData;

  internal int MatchingRecordsCount
  {
    get => this.m_matchingRecordsCount;
    set => this.m_matchingRecordsCount = value;
  }

  internal string Command
  {
    get => this.m_command;
    set => this.m_command = value;
  }

  public MailMergeDataTable(string groupName, IEnumerable enumerable)
  {
    this.m_groupName = groupName;
    this.m_sourceData = enumerable.GetEnumerator();
    try
    {
      this.m_sourceData.Reset();
    }
    catch (Exception ex)
    {
      if (!(ex.Message == "The method or operation is not implemented."))
        return;
      enumerable = (IEnumerable) enumerable.Cast<object>().ToList<object>();
      this.m_sourceData = enumerable.GetEnumerator();
    }
  }

  internal MailMergeDataTable(string groupName, IEnumerator enumerator)
  {
    this.m_groupName = groupName;
    this.m_sourceData = enumerator;
  }

  internal MailMergeDataTable Select(string command)
  {
    string[] strArray = command.Split(' ');
    string str1 = strArray[0];
    string str2 = strArray[2];
    List<object> objectList = new List<object>();
    this.m_sourceData.Reset();
    while (this.m_sourceData.MoveNext())
    {
      if (this.m_sourceData.Current is IDictionary<string, object>)
      {
        this.m_sourceData.Reset();
        while (this.m_sourceData.MoveNext())
        {
          if ((this.m_sourceData.Current as IDictionary<string, object>).ContainsKey(str1) && (str2 == (this.m_sourceData.Current as IDictionary<string, object>)[str1].ToString() || strArray[1].ToLower() == "contains" && (this.m_sourceData.Current as IDictionary<string, object>)[str1].ToString().Contains(str2)))
            ++this.MatchingRecordsCount;
        }
        return new MailMergeDataTable(this.GroupName, (IEnumerator) objectList.GetEnumerator());
      }
      object obj = this.m_sourceData.Current.GetType().GetProperty(str1).GetValue(this.m_sourceData.Current, (object[]) null);
      if (str2 == obj.ToString())
        ++this.MatchingRecordsCount;
    }
    return new MailMergeDataTable(this.GroupName, (IEnumerator) objectList.GetEnumerator());
  }
}
