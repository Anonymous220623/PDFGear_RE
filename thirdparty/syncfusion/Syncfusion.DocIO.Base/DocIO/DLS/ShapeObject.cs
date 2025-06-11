// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ShapeObject
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;
using Syncfusion.DocIO.Rendering;
using Syncfusion.Layouting;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class ShapeObject : ParagraphItem, ILeafWidget, IWidget
{
  private FileShapeAddress m_fspa;
  private WTextBoxCollection m_textBoxColl;
  private byte m_bFlags;

  public override EntityType EntityType => EntityType.Shape;

  internal FileShapeAddress FSPA
  {
    get => this.m_fspa;
    set => this.m_fspa = value;
  }

  internal WTextBoxCollection AutoShapeTextCollection => this.m_textBoxColl;

  internal bool IsHeaderAutoShape
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal bool AllowInCell
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
  }

  internal WCharacterFormat CharacterFormat => this.m_charFormat;

  internal ShapeObject(IWordDocument doc)
    : base((WordDocument) doc)
  {
    this.m_fspa = new FileShapeAddress();
    this.m_textBoxColl = new WTextBoxCollection(doc);
    this.m_charFormat = new WCharacterFormat(doc, (Entity) this);
  }

  internal override void AddSelf()
  {
    foreach (Entity autoShapeText in (CollectionImpl) this.AutoShapeTextCollection)
      autoShapeText.AddSelf();
  }

  internal override void AttachToParagraph(WParagraph owner, int itemPos)
  {
    base.AttachToParagraph(owner, itemPos);
  }

  internal override void CloneRelationsTo(WordDocument doc, OwnerHolder nextOwner)
  {
    base.CloneRelationsTo(doc, nextOwner);
    foreach (WTextBox autoShapeText in (CollectionImpl) this.AutoShapeTextCollection)
    {
      foreach (Entity childEntity in (CollectionImpl) autoShapeText.ChildEntities)
      {
        childEntity.CloneRelationsTo(doc, nextOwner);
        childEntity.SetOwner((OwnerHolder) doc);
      }
    }
    this.Document.CloneShapeEscher(doc, (IParagraphItem) this);
    this.IsCloned = false;
  }

  protected override object CloneImpl()
  {
    ShapeObject shapeObject = (ShapeObject) base.CloneImpl();
    shapeObject.m_textBoxColl = new WTextBoxCollection((IWordDocument) this.Document);
    this.m_textBoxColl.CloneTo((EntityCollection) shapeObject.m_textBoxColl);
    if (this.FSPA != null)
      shapeObject.m_fspa = this.FSPA.Clone();
    shapeObject.IsCloned = true;
    return (object) shapeObject;
  }

  protected override void CreateLayoutInfo()
  {
    this.m_layoutInfo = (ILayoutInfo) new LayoutInfo(ChildrenLayoutDirection.Horizontal);
    if (!(this is InlineShapeObject))
      this.m_layoutInfo.IsSkipBottomAlign = true;
    this.m_layoutInfo.IsClipped = true;
  }

  internal override void InitLayoutInfo(Entity entity, ref bool isLastTOCEntry)
  {
    this.m_layoutInfo = (ILayoutInfo) null;
    if (this.m_textBoxColl == null || this.m_textBoxColl.Count <= 0)
      return;
    foreach (Entity entity1 in (CollectionImpl) this.m_textBoxColl)
    {
      entity1.InitLayoutInfo(entity, ref isLastTOCEntry);
      if (isLastTOCEntry)
        break;
    }
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    writer.WriteValue("type", (Enum) ParagraphItemType.ShapeObject);
    writer.WriteValue("ShapeID", this.m_fspa.Spid);
    writer.WriteValue("IsBelowText", this.m_fspa.IsBelowText);
    writer.WriteValue("HorizontalOrigin", (Enum) this.m_fspa.RelHrzPos);
    writer.WriteValue("VerticalOrigin", (Enum) this.m_fspa.RelVrtPos);
    writer.WriteValue("WrappingStyle", (Enum) this.m_fspa.TextWrappingStyle);
    writer.WriteValue("WrappingType", (Enum) this.m_fspa.TextWrappingType);
    writer.WriteValue("HorizontalPosition", this.m_fspa.XaLeft);
    writer.WriteValue("VerticalPosition", this.m_fspa.YaTop);
    writer.WriteValue("TxbxCount", this.m_fspa.TxbxCount);
    writer.WriteValue("Height", this.m_fspa.Height);
    writer.WriteValue("Width", this.m_fspa.Width);
    writer.WriteValue("IsHeader", this.IsHeaderAutoShape);
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    if (reader.HasAttribute("ShapeID"))
      this.m_fspa.Spid = reader.ReadInt("ShapeID");
    if (reader.HasAttribute("IsBelowText"))
      this.m_fspa.IsBelowText = reader.ReadBoolean("IsBelowText");
    if (reader.HasAttribute("HorizontalOrigin"))
      this.m_fspa.RelHrzPos = (HorizontalOrigin) reader.ReadEnum("HorizontalOrigin", typeof (HorizontalOrigin));
    if (reader.HasAttribute("VerticalOrigin"))
      this.m_fspa.RelVrtPos = (VerticalOrigin) reader.ReadEnum("VerticalOrigin", typeof (VerticalOrigin));
    if (reader.HasAttribute("WrappingStyle"))
      this.m_fspa.TextWrappingStyle = (TextWrappingStyle) reader.ReadEnum("WrappingStyle", typeof (TextWrappingStyle));
    if (reader.HasAttribute("WrappingType"))
      this.m_fspa.TextWrappingType = (TextWrappingType) reader.ReadEnum("WrappingType", typeof (TextWrappingType));
    if (reader.HasAttribute("HorizontalPosition"))
      this.m_fspa.XaLeft = reader.ReadInt("HorizontalPosition");
    if (reader.HasAttribute("VerticalPosition"))
      this.m_fspa.YaTop = reader.ReadInt("VerticalPosition");
    if (reader.HasAttribute("TxbxCount"))
      this.m_fspa.TxbxCount = reader.ReadInt("TxbxCount");
    if (reader.HasAttribute("Height"))
      this.m_fspa.Height = reader.ReadInt("Height");
    if (reader.HasAttribute("Width"))
      this.m_fspa.Width = reader.ReadInt("Width");
    if (!reader.HasAttribute("IsHeader"))
      return;
    this.IsHeaderAutoShape = reader.ReadBoolean("IsHeader");
  }

  protected override void InitXDLSHolder()
  {
    base.InitXDLSHolder();
    this.XDLSHolder.AddElement("textboxes", (object) this.m_textBoxColl);
    this.XDLSHolder.AddElement("character-format", (object) this.m_charFormat);
  }

  internal override void Close()
  {
    base.Close();
    this.m_fspa = (FileShapeAddress) null;
    if (this.m_textBoxColl == null)
      return;
    this.m_textBoxColl.Close();
    this.m_textBoxColl = (WTextBoxCollection) null;
  }

  SizeF ILeafWidget.Measure(DrawingContext dc)
  {
    float height = 0.0f;
    if (this.OwnerParagraph.ChildEntities.Count == 1 && this != null && !((IWidget) this).LayoutInfo.IsSkip)
    {
      this.m_layoutInfo.IsClipped = false;
      height = this.OwnerParagraph.m_layoutInfo.Size.Height;
    }
    return new SizeF(0.0f, height);
  }
}
