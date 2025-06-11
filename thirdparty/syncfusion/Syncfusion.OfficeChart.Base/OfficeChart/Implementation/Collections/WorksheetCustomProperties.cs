// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Collections.WorksheetCustomProperties
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Parser.Biff_Records;
using System;
using System.Collections;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Collections;

internal class WorksheetCustomProperties : 
  TypedSortedListEx<string, ICustomProperty>,
  IWorksheetCustomProperties
{
  public WorksheetCustomProperties()
  {
  }

  public WorksheetCustomProperties(IList m_arrRecords, int iCustomPropertyPos)
  {
    int num = m_arrRecords != null ? m_arrRecords.Count : throw new ArgumentNullException(nameof (m_arrRecords));
    if (iCustomPropertyPos < 0 || iCustomPropertyPos >= num)
      throw new ArgumentOutOfRangeException(nameof (iCustomPropertyPos));
    for (; iCustomPropertyPos < num && m_arrRecords[iCustomPropertyPos] is CustomPropertyRecord mArrRecord; ++iCustomPropertyPos)
      this.Add(mArrRecord);
  }

  public ICustomProperty this[int index] => this.GetByIndex(index);

  public new ICustomProperty this[string strName] => this.GetByName(strName);

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      (this.GetByIndex(index) as WorksheetCustomProperty).Serialize(records);
  }

  public ICustomProperty Add(string strName)
  {
    return this.Add((ICustomProperty) new WorksheetCustomProperty(strName));
  }

  public ICustomProperty Add(ICustomProperty property)
  {
    this.Add(property.Name, property);
    return property;
  }

  [CLSCompliant(false)]
  public void Add(CustomPropertyRecord property)
  {
    if (property == null)
      throw new ArgumentNullException(nameof (property));
    this.Add((ICustomProperty) new WorksheetCustomProperty(property));
  }
}
