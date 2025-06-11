// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.WorksheetCustomProperty
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Parser;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using System;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class WorksheetCustomProperty : ICustomProperty, ICloneable
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
