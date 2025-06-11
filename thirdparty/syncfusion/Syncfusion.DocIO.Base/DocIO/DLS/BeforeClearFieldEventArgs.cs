// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.BeforeClearFieldEventArgs
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class BeforeClearFieldEventArgs
{
  private WordDocument m_doc;
  private IWMergeField m_currentMergeField;
  private string m_groupName;
  private object m_fieldValue;
  private int m_rowIndex;
  private bool m_clearField = true;
  private bool m_fieldHasMappedInDataSource;

  internal WordDocument Doc => this.m_doc;

  public string FieldName
  {
    get
    {
      return this.m_currentMergeField.Prefix != string.Empty ? $"{this.m_currentMergeField.Prefix}:{this.m_currentMergeField.FieldName}" : this.m_currentMergeField.FieldName;
    }
  }

  public object FieldValue
  {
    get => this.m_fieldValue;
    set => this.m_fieldValue = value;
  }

  public string GroupName => this.m_groupName;

  public bool HasMappedFieldInDataSource => this.m_fieldHasMappedInDataSource;

  public int RowIndex => this.m_rowIndex;

  public bool ClearField
  {
    get => this.m_clearField;
    set => this.m_clearField = value;
  }

  public IWMergeField CurrentMergeField => this.m_currentMergeField;

  public BeforeClearFieldEventArgs(
    WordDocument doc,
    string groupName,
    int rowIndex,
    IWMergeField field,
    bool fieldHasMappedInDataSource,
    object value)
  {
    this.m_doc = doc;
    this.m_currentMergeField = field;
    this.m_fieldValue = value;
    this.m_groupName = groupName;
    this.m_rowIndex = rowIndex;
    this.m_fieldHasMappedInDataSource = fieldHasMappedInDataSource;
    this.m_clearField = this.Doc.MailMerge.ClearFields;
  }
}
