// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.TimeStampElements
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class TimeStampElements
{
  private Dictionary<DerObjectID, List<TimeStampElement>> m_elements;

  internal TimeStampElement this[DerObjectID id]
  {
    get
    {
      if (!this.m_elements.ContainsKey(id))
        return (TimeStampElement) null;
      object element = (object) this.m_elements[id];
      return element is IList ? (TimeStampElement) ((IList) element)[0] : (TimeStampElement) element;
    }
  }

  internal TimeStampElements(Asn1Set values)
  {
    this.m_elements = new Dictionary<DerObjectID, List<TimeStampElement>>(values.Count);
    for (int index = 0; index != values.Count; ++index)
    {
      TimeStampElement timeStampElement = TimeStampElement.GetTimeStampElement((object) values[index]);
      DerObjectID type = timeStampElement.Type;
      if (this.m_elements.ContainsKey(type))
      {
        object element = (object) this.m_elements[type];
        List<TimeStampElement> timeStampElementList;
        if (element is TimeStampElement)
        {
          timeStampElementList = new List<TimeStampElement>();
          timeStampElementList.Add(element as TimeStampElement);
          timeStampElementList.Add(timeStampElement);
        }
        else
        {
          timeStampElementList = (List<TimeStampElement>) element;
          timeStampElementList.Add(timeStampElement);
        }
        this.m_elements[type] = timeStampElementList;
      }
      else
        this.m_elements[type] = new List<TimeStampElement>(1)
        {
          timeStampElement
        };
    }
  }
}
