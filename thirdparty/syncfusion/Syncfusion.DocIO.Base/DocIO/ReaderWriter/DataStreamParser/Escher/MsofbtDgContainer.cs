// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.MsofbtDgContainer
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ReaderWriter.Escher;
using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;

internal class MsofbtDgContainer : BaseContainer
{
  private ShapeDocType m_shapeDocType;

  internal MsofbtDg Dg => this.FindContainerByType(typeof (MsofbtDg)) as MsofbtDg;

  internal MsofbtSpgrContainer PatriarchGroupContainer
  {
    get => this.FindContainerByType(typeof (MsofbtSpgrContainer)) as MsofbtSpgrContainer;
  }

  internal ShapeDocType ShapeDocType
  {
    get => this.m_shapeDocType;
    set => this.m_shapeDocType = value;
  }

  internal MsofbtDgContainer(WordDocument doc)
    : base(MSOFBT.msofbtDgContainer, doc)
  {
  }

  private static void GetShapeCountAndMaxSpid(
    BaseContainer baseContainer,
    ref int shapeCount,
    ref int spidMax)
  {
    for (int index = 0; index < baseContainer.Children.Count; ++index)
    {
      BaseEscherRecord child = baseContainer.Children[index] as BaseEscherRecord;
      if (child is MsofbtSp)
      {
        MsofbtSp msofbtSp = child as MsofbtSp;
        spidMax = Math.Max(spidMax, msofbtSp.ShapeId);
        ++shapeCount;
      }
      if (child is BaseContainer)
        MsofbtDgContainer.GetShapeCountAndMaxSpid(child as BaseContainer, ref shapeCount, ref spidMax);
    }
  }

  internal void InitWriting()
  {
    int shapeCount = 0;
    int spidMax = 0;
    MsofbtDgContainer.GetShapeCountAndMaxSpid((BaseContainer) this.PatriarchGroupContainer, ref shapeCount, ref spidMax);
    this.Dg.ShapeCount = shapeCount;
    this.Dg.SpidLast = spidMax;
  }

  internal override BaseEscherRecord Clone()
  {
    MsofbtDgContainer msofbtDgContainer = (MsofbtDgContainer) base.Clone();
    msofbtDgContainer.m_shapeDocType = this.m_shapeDocType;
    msofbtDgContainer.m_doc = this.m_doc;
    return (BaseEscherRecord) msofbtDgContainer;
  }
}
