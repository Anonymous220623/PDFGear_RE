// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WFieldMark
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using Syncfusion.Layouting;
using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WFieldMark : ParagraphItem
{
  private FieldMarkType m_fldMarkType;
  private WField m_parentField;

  public override EntityType EntityType => EntityType.FieldMark;

  public WCharacterFormat CharacterFormat => this.m_charFormat;

  public FieldMarkType Type
  {
    get => this.m_fldMarkType;
    set => this.m_fldMarkType = value;
  }

  internal WField ParentField
  {
    get => this.m_parentField;
    set => this.m_parentField = value;
  }

  internal WFieldMark(IWordDocument doc)
    : base((WordDocument) doc)
  {
    this.m_charFormat = new WCharacterFormat(doc, (Entity) this);
  }

  protected internal WFieldMark(WFieldMark fieldMark, IWordDocument doc)
    : this(doc)
  {
    this.Type = fieldMark.Type;
    this.m_charFormat.ImportContainer((FormatBase) fieldMark.CharacterFormat);
    this.m_charFormat.CopyProperties((FormatBase) fieldMark.CharacterFormat);
  }

  internal WFieldMark(IWordDocument doc, FieldMarkType type)
    : this(doc)
  {
    this.m_fldMarkType = type;
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    if (!reader.HasAttribute("FieldMarkType"))
      return;
    this.m_fldMarkType = (FieldMarkType) reader.ReadEnum("FieldMarkType", typeof (FieldMarkType));
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    writer.WriteValue("type", (Enum) ParagraphItemType.FieldMark);
    writer.WriteValue("FieldMarkType", (Enum) this.m_fldMarkType);
  }

  protected override void InitXDLSHolder()
  {
    this.XDLSHolder.AddElement("character-format", (object) this.m_charFormat);
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
