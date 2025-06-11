// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.BeforeClearGroupFieldEventArgs
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class BeforeClearGroupFieldEventArgs
{
  private WordDocument m_doc;
  private IWMergeField m_currentMergeField;
  private string m_groupName;
  private bool m_clearGroup = true;
  private bool m_fieldHasMappedInDataSource;
  private string[] m_fieldNames;
  private IEnumerable m_alternateValues;

  internal WordDocument Doc => this.m_doc;

  public string GroupName => this.m_groupName;

  public bool HasMappedGroupInDataSource => this.m_fieldHasMappedInDataSource;

  public bool ClearGroup
  {
    get => this.m_clearGroup;
    set => this.m_clearGroup = value;
  }

  public string[] FieldNames => this.m_fieldNames;

  public IEnumerable AlternateValues
  {
    get => this.m_alternateValues;
    set
    {
      this.m_alternateValues = value;
      string[] strArray = this.m_groupName.Split(':');
      this.Doc.MailMerge.ExecuteGroup(new MailMergeDataTable(strArray[strArray.Length - 1], this.m_alternateValues));
    }
  }

  public BeforeClearGroupFieldEventArgs(
    WordDocument doc,
    string groupName,
    IWMergeField field,
    bool fieldHasMappedInDataSource,
    string[] fieldNames)
  {
    this.m_doc = doc;
    this.m_groupName = groupName;
    this.m_fieldHasMappedInDataSource = fieldHasMappedInDataSource;
    this.m_currentMergeField = field;
    this.m_fieldNames = fieldNames;
    this.m_clearGroup = this.Doc.MailMerge.ClearFields;
  }
}
