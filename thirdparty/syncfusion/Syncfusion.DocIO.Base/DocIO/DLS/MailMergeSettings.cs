// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.MailMergeSettings
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class MailMergeSettings
{
  internal const byte ActiveRecordKey = 0;
  internal const byte AddressFieldNameKey = 1;
  internal const byte CheckErrorsKey = 2;
  internal const byte ConnectStringKey = 3;
  internal const byte DataSourceKey = 4;
  internal const byte DataTypeKey = 5;
  internal const byte DestinationKey = 6;
  internal const byte DoNotSupressBlankLinesKey = 7;
  internal const byte HeaderSourceKey = 8;
  internal const byte LinkToQueryKey = 9;
  internal const byte MailAsAttachmentKey = 10;
  internal const byte MailSubjectKey = 11;
  internal const byte MainDocumentTypeKey = 12;
  internal const byte QueryKey = 13;
  internal const byte ViewMergedDataKey = 14;
  internal const byte ODSOSettingsKey = 15;
  private Dictionary<int, object> m_propertiesHash;

  internal int ActiveRecord
  {
    get => this.HasKey(0) ? (int) this.PropertiesHash[0] : 1;
    set => this.SetKeyValue(0, (object) value);
  }

  internal string AddressFieldName
  {
    get => this.HasKey(1) ? (string) this.PropertiesHash[1] : string.Empty;
    set => this.SetKeyValue(1, (object) value);
  }

  internal MailMergeCheckErrors CheckErrors
  {
    get
    {
      return this.HasKey(2) ? (MailMergeCheckErrors) this.PropertiesHash[2] : MailMergeCheckErrors.PauseOnError;
    }
    set => this.SetKeyValue(2, (object) value);
  }

  internal string ConnectString
  {
    get => this.HasKey(3) ? (string) this.PropertiesHash[3] : string.Empty;
    set => this.SetKeyValue(3, (object) value);
  }

  public string DataSource
  {
    get
    {
      return this.HasKey(4) ? ((string) this.PropertiesHash[4]).Replace("file:///", "") : string.Empty;
    }
    set => this.SetKeyValue(4, (object) value);
  }

  internal MailMergeDataType DataType
  {
    get => this.HasKey(5) ? (MailMergeDataType) this.PropertiesHash[5] : MailMergeDataType.Native;
    set => this.SetKeyValue(5, (object) value);
  }

  internal MailMergeDestination Destination
  {
    get
    {
      return this.HasKey(6) ? (MailMergeDestination) this.PropertiesHash[6] : MailMergeDestination.NewDocument;
    }
    set => this.SetKeyValue(6, (object) value);
  }

  internal bool DoNotSupressBlankLines
  {
    get => this.HasKey(7) && (bool) this.PropertiesHash[7];
    set => this.SetKeyValue(7, (object) value);
  }

  internal string HeaderSource
  {
    get => this.HasKey(8) ? (string) this.PropertiesHash[8] : string.Empty;
    set => this.SetKeyValue(8, (object) value);
  }

  internal bool LinkToQuery
  {
    get => this.HasKey(9) && (bool) this.PropertiesHash[9];
    set => this.SetKeyValue(9, (object) value);
  }

  internal bool MailAsAttachment
  {
    get => this.HasKey(10) && (bool) this.PropertiesHash[10];
    set => this.SetKeyValue(10, (object) value);
  }

  internal string MailSubject
  {
    get => this.HasKey(11) ? (string) this.PropertiesHash[11] : string.Empty;
    set => this.SetKeyValue(11, (object) value);
  }

  internal MailMergeMainDocumentType MainDocumentType
  {
    get
    {
      return this.HasKey(12) ? (MailMergeMainDocumentType) this.PropertiesHash[12] : MailMergeMainDocumentType.FormLetters;
    }
    set => this.SetKeyValue(12, (object) value);
  }

  internal string Query
  {
    get => this.HasKey(13) ? (string) this.PropertiesHash[13] : string.Empty;
    set => this.SetKeyValue(13, (object) value);
  }

  internal bool ViewMergedData
  {
    get => this.HasKey(14) && (bool) this.PropertiesHash[14];
    set => this.SetKeyValue(14, (object) value);
  }

  internal Stream ODSOSettings
  {
    get => this.HasKey(15) ? (Stream) this.PropertiesHash[15] : (Stream) null;
    set => this.SetKeyValue(15, (object) value);
  }

  private Dictionary<int, object> PropertiesHash
  {
    get
    {
      if (this.m_propertiesHash == null)
        this.m_propertiesHash = new Dictionary<int, object>();
      return this.m_propertiesHash;
    }
  }

  public bool HasData => this.m_propertiesHash != null && this.m_propertiesHash.Count > 0;

  internal MailMergeSettings()
  {
  }

  internal bool HasKey(int Key)
  {
    return this.m_propertiesHash != null && this.m_propertiesHash.ContainsKey(Key);
  }

  private void SetKeyValue(int propKey, object value) => this.PropertiesHash[propKey] = value;

  internal void Close()
  {
    if (this.m_propertiesHash == null)
      return;
    if (this.m_propertiesHash.ContainsKey(15))
      ((Stream) this.m_propertiesHash[15]).Dispose();
    this.m_propertiesHash.Clear();
    this.m_propertiesHash = (Dictionary<int, object>) null;
  }

  public void RemoveData() => this.Close();
}
