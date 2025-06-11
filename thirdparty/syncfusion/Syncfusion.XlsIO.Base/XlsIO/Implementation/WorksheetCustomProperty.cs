// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.WorksheetCustomProperty
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class WorksheetCustomProperty : ICustomProperty, ICloneable
{
  private CustomPropertyRecord m_record;

  private WorksheetCustomProperty()
  {
  }

  public WorksheetCustomProperty(string strName)
  {
    this.m_record = (CustomPropertyRecord) BiffRecordFactory.GetRecord(TBIFFRecord.CustomProperty);
    this.m_record.Name = strName;
  }

  [CLSCompliant(false)]
  public WorksheetCustomProperty(CustomPropertyRecord property)
  {
    this.m_record = property != null ? property : throw new ArgumentNullException(nameof (property));
  }

  public string Name => this.m_record.Name;

  public string Value
  {
    get => this.m_record.Value;
    set => this.m_record.Value = value;
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    records.Add((IBiffStorage) this.m_record);
  }

  public object Clone()
  {
    WorksheetCustomProperty worksheetCustomProperty = (WorksheetCustomProperty) this.MemberwiseClone();
    worksheetCustomProperty.m_record = (CustomPropertyRecord) CloneUtils.CloneCloneable((ICloneable) this.m_record);
    return (object) worksheetCustomProperty;
  }
}
