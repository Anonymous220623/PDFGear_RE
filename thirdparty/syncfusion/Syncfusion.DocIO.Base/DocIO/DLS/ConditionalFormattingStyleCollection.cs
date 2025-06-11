// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ConditionalFormattingStyleCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class ConditionalFormattingStyleCollection : CollectionImpl
{
  private IWordDocument m_doc;

  internal IWordDocument Document => this.m_doc;

  public ConditionalFormattingStyle this[ConditionalFormattingType formattingType]
  {
    get
    {
      ConditionalFormattingStyle conditionalFormattingStyle = (ConditionalFormattingStyle) null;
      for (int index = 0; index < this.InnerList.Count; ++index)
      {
        if ((this.InnerList[index] as ConditionalFormattingStyle).ConditionalFormattingType == formattingType)
        {
          conditionalFormattingStyle = this.InnerList[index] as ConditionalFormattingStyle;
          break;
        }
      }
      return conditionalFormattingStyle;
    }
  }

  internal ConditionalFormattingStyleCollection(WordDocument doc)
    : base(doc, (OwnerHolder) doc, 12)
  {
    this.m_doc = (IWordDocument) doc;
  }

  public ConditionalFormattingStyle Add(
    ConditionalFormattingType conditionalFormattingType)
  {
    if (this.InnerList.Count > 0)
    {
      int count = this.InnerList.Count;
      bool flag = false;
      for (int index = 0; index < count; ++index)
      {
        if ((this.InnerList[index] as ConditionalFormattingStyle).ConditionalFormattingType == conditionalFormattingType)
        {
          flag = true;
          break;
        }
        flag = false;
      }
      if (flag)
        throw new ArgumentException("The given style already exist in the collcetion");
      ConditionalFormattingStyle conditionalFormattingStyle = new ConditionalFormattingStyle(conditionalFormattingType, this.Document);
      this.InnerList.Add((object) conditionalFormattingStyle);
      return conditionalFormattingStyle;
    }
    ConditionalFormattingStyle conditionalFormattingStyle1 = new ConditionalFormattingStyle(conditionalFormattingType, this.Document);
    this.InnerList.Add((object) conditionalFormattingStyle1);
    return conditionalFormattingStyle1;
  }

  public void Remove(
    ConditionalFormattingStyle conditionalFormattingStyle)
  {
    this.InnerList.Remove((object) conditionalFormattingStyle);
  }
}
