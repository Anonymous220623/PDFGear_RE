// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Watermark
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using Syncfusion.Layouting;
using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class Watermark : ParagraphItem
{
  private WatermarkType m_type;
  private int m_orderIndex = int.MaxValue;
  private int m_spid = -1;

  public override EntityType EntityType => EntityType.Undefined;

  public WatermarkType Type => this.m_type;

  internal int OrderIndex
  {
    get
    {
      if (this.m_orderIndex == int.MaxValue && this.Document != null && !this.Document.IsOpening && this.Document.Escher != null)
      {
        int shapeOrderIndex = this.Document.Escher.GetShapeOrderIndex(this.ShapeId);
        if (shapeOrderIndex != -1)
          this.m_orderIndex = shapeOrderIndex;
      }
      return this.m_orderIndex;
    }
    set => this.m_orderIndex = value;
  }

  internal int ShapeId
  {
    get => this.m_spid;
    set => this.m_spid = value;
  }

  internal Watermark(WatermarkType type)
    : base((WordDocument) null)
  {
    this.m_type = type;
  }

  internal Watermark(WordDocument doc, WatermarkType type)
    : base(doc)
  {
    this.m_type = type;
  }

  internal override void RemoveSelf()
  {
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    writer.WriteValue("WatermarkType", (Enum) this.m_type);
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    if (!reader.HasAttribute("WatermarkType"))
      return;
    this.m_type = (WatermarkType) reader.ReadEnum("WatermarkType", typeof (WatermarkType));
  }

  protected override void CreateLayoutInfo() => this.m_layoutInfo = (ILayoutInfo) new LayoutInfo();

  internal override void InitLayoutInfo(Entity entity, ref bool isLastTOCEntry)
  {
    this.m_layoutInfo = (ILayoutInfo) null;
    if (this != entity)
      return;
    isLastTOCEntry = true;
  }
}
