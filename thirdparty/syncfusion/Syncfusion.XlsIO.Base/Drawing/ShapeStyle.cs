// Decompiled with JetBrains decompiler
// Type: Syncfusion.Drawing.ShapeStyle
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO;
using Syncfusion.XlsIO.Implementation.XmlReaders;
using Syncfusion.XlsIO.Implementation.XmlReaders.Shapes;
using System.IO;
using System.Xml;

#nullable disable
namespace Syncfusion.Drawing;

internal class ShapeStyle
{
  private string attribute;
  private ShapeImplExt shape;
  private string nameSpace;
  private StyleEntryModifierEnum m_styleElementMod;
  private StyleOrFontReference m_lnRefStyleEntry;
  private double m_lineWidthScale = -1.0;
  private StyleOrFontReference m_effectRefStyleEntry;
  private StyleOrFontReference m_fillRefStyleEntry;
  private StyleOrFontReference m_fontRefstyleEntry;
  private StyleEntryShapeProperties m_shapeProperties;
  private TextSettings m_defaultParagraphRunProperties;
  private TextBodyPropertiesHolder m_textBodyProperties;

  internal StyleEntryModifierEnum StyleElementMod
  {
    get => this.m_styleElementMod;
    set => this.m_styleElementMod = value;
  }

  internal StyleOrFontReference LineRefStyleEntry
  {
    get => this.m_lnRefStyleEntry;
    set => this.m_lnRefStyleEntry = value;
  }

  internal double LineWidthScale
  {
    get => this.m_lineWidthScale;
    set => this.m_lineWidthScale = value;
  }

  internal StyleOrFontReference EffectRefStyleEntry
  {
    get => this.m_effectRefStyleEntry;
    set => this.m_effectRefStyleEntry = value;
  }

  internal StyleOrFontReference FillRefStyleEntry
  {
    get => this.m_fillRefStyleEntry;
    set => this.m_fillRefStyleEntry = value;
  }

  internal StyleOrFontReference FontRefstyleEntry
  {
    get => this.m_fontRefstyleEntry;
    set => this.m_fontRefstyleEntry = value;
  }

  internal StyleEntryShapeProperties ShapeProperties
  {
    get => this.m_shapeProperties;
    set => this.m_shapeProperties = value;
  }

  internal TextSettings DefaultRunParagraphProperties
  {
    get => this.m_defaultParagraphRunProperties;
    set => this.m_defaultParagraphRunProperties = value;
  }

  internal TextBodyPropertiesHolder TextBodyProperties
  {
    get => this.m_textBodyProperties;
    set => this.m_textBodyProperties = value;
  }

  internal ShapeStyle()
  {
  }

  public ShapeStyle(ShapeImplExt shape, string arrtibute)
  {
    this.shape = shape;
    this.attribute = arrtibute;
    this.nameSpace = this.attribute == "xdr" ? "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing" : "http://schemas.openxmlformats.org/drawingml/2006/chartDrawing";
  }

  internal ShapeStyle(
    string attributeValue,
    string nameSpaceValue,
    StyleEntryModifierEnum enumValue)
  {
    this.shape = (ShapeImplExt) null;
    this.attribute = attributeValue;
    this.nameSpace = nameSpaceValue;
    this.m_styleElementMod = enumValue;
  }

  private void SerializeStyleOrFontReference(
    XmlWriter writer,
    StyleOrFontReference styleEntry,
    string styleEntryName,
    bool isFontReference)
  {
    string prefix = this.shape != null ? "a" : this.attribute;
    string ns = this.shape != null ? "http://schemas.openxmlformats.org/drawingml/2006/main" : this.nameSpace;
    writer.WriteStartElement(prefix, styleEntryName, ns);
    if (isFontReference)
    {
      if (styleEntry.Index < 3 && styleEntry.Index > -1)
        writer.WriteAttributeString("idx", ((FontCollectionIndex) styleEntry.Index).ToString());
      else
        writer.WriteAttributeString("idx", FontCollectionIndex.none.ToString());
    }
    else
      writer.WriteAttributeString("idx", styleEntry.Index.ToString());
    this.SerializeColorSettings(writer, styleEntry.ColorModelType, styleEntry.ColorValue, styleEntry.LumModValue, styleEntry.LumOffValue1, styleEntry.LumOffValue2, styleEntry.ShadeValue);
    writer.WriteEndElement();
  }

  private void SerializeColorSettings(
    XmlWriter writer,
    ColorModel colorModelType,
    string colorValue,
    double lumModValue,
    double lumOffValue1,
    double lumOffValue2,
    double shadeValue)
  {
    switch (colorModelType)
    {
      case ColorModel.none:
        return;
      case ColorModel.styleClr:
        writer.WriteStartElement("cs", colorModelType.ToString(), (string) null);
        break;
      default:
        writer.WriteStartElement("a", colorModelType.ToString(), (string) null);
        break;
    }
    writer.WriteAttributeString("val", colorValue);
    if (lumModValue != -1.0)
    {
      writer.WriteStartElement("a", "lumMod", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteAttributeString("val", lumModValue.ToString());
      writer.WriteEndElement();
    }
    if (lumOffValue1 != -1.0)
    {
      writer.WriteStartElement("a", "lumOff", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteAttributeString("val", lumOffValue1.ToString());
      writer.WriteEndElement();
    }
    if (lumOffValue2 != -1.0)
    {
      writer.WriteStartElement("a", "lumOff", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteAttributeString("val", lumOffValue2.ToString());
      writer.WriteEndElement();
    }
    if (shadeValue != -1.0)
    {
      writer.WriteStartElement("a", "shade", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteAttributeString("val", shadeValue.ToString());
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private void SerializeShapeProperties(XmlWriter writer)
  {
    writer.WriteStartElement(this.attribute, "spPr", this.nameSpace);
    if (((int) this.m_shapeProperties.FlagOptions & 1) == 1)
    {
      if (this.m_shapeProperties.ShapeFillType == ExcelFillType.SolidColor)
      {
        writer.WriteStartElement("a", "solidFill", "http://schemas.openxmlformats.org/drawingml/2006/main");
        this.SerializeColorSettings(writer, this.m_shapeProperties.ShapeFillColorModelType, this.m_shapeProperties.ShapeFillColorValue, this.m_shapeProperties.ShapeFillLumModValue, this.m_shapeProperties.ShapeFillLumOffValue1, this.m_shapeProperties.ShapeFillLumOffValue2, -1.0);
        writer.WriteEndElement();
      }
      else
        writer.WriteElementString("a", "noFill", "http://schemas.openxmlformats.org/drawingml/2006/main", "");
    }
    if (((int) this.m_shapeProperties.FlagOptions & 2) == 2)
    {
      writer.WriteStartElement("a", "ln", "http://schemas.openxmlformats.org/drawingml/2006/main");
      if (((int) this.m_shapeProperties.FlagOptions & 4) == 4 && this.m_shapeProperties.BorderWeight == 0.0)
      {
        writer.WriteElementString("a", "noFill", "http://schemas.openxmlformats.org/drawingml/2006/main", "");
      }
      else
      {
        if (((int) this.m_shapeProperties.FlagOptions & 4) == 4 && this.m_shapeProperties.BorderWeight != -1.0)
          writer.WriteAttributeString("w", this.m_shapeProperties.BorderWeight.ToString());
        if (((int) this.m_shapeProperties.FlagOptions & 4) == 8)
          writer.WriteAttributeString("cap", this.m_shapeProperties.LineCap.ToString());
        if (((int) this.m_shapeProperties.FlagOptions & 4) == 16 /*0x10*/)
          writer.WriteAttributeString("cmpd", this.m_shapeProperties.BorderLineStyle.ToString());
        if (((int) this.m_shapeProperties.FlagOptions & 4) == 32 /*0x20*/)
          writer.WriteAttributeString("algn", this.m_shapeProperties.IsInsetPenAlignment ? "in" : "ctr");
        writer.WriteStartElement("a", "solidFill", "http://schemas.openxmlformats.org/drawingml/2006/main");
        this.SerializeColorSettings(writer, this.m_shapeProperties.BorderFillColorModelType, this.m_shapeProperties.BorderFillColorValue, this.m_shapeProperties.BorderFillLumModValue, this.m_shapeProperties.BorderFillLumOffValue1, this.m_shapeProperties.BorderFillLumOffValue2, -1.0);
        writer.WriteEndElement();
        if (this.m_shapeProperties.BorderIsRound)
          writer.WriteElementString("a", "round", "http://schemas.openxmlformats.org/drawingml/2006/main", "");
      }
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private void SerializeDefaultRPrProperties(XmlWriter writer)
  {
    writer.WriteStartElement(this.attribute, "defRPr", this.nameSpace);
    if (this.m_defaultParagraphRunProperties.FontSize.HasValue)
    {
      XmlWriter xmlWriter = writer;
      float? fontSize = this.m_defaultParagraphRunProperties.FontSize;
      string str = (fontSize.HasValue ? new float?(fontSize.GetValueOrDefault() * 100f) : new float?()).ToString();
      xmlWriter.WriteAttributeString("sz", str);
    }
    if (this.m_defaultParagraphRunProperties.Bold.HasValue)
      writer.WriteAttributeString("b", this.m_defaultParagraphRunProperties.Bold.Value ? "1" : "0");
    if (this.m_defaultParagraphRunProperties.Baseline != -1)
      writer.WriteAttributeString("baseline", this.m_defaultParagraphRunProperties.Baseline.ToString());
    if ((double) this.m_defaultParagraphRunProperties.KerningValue != -1.0)
      writer.WriteAttributeString("kern", (this.m_defaultParagraphRunProperties.KerningValue * 100f).ToString());
    if (this.m_defaultParagraphRunProperties.SpacingValue != -1)
      writer.WriteAttributeString("spc", this.m_defaultParagraphRunProperties.SpacingValue.ToString());
    writer.WriteEndElement();
  }

  internal void Write(XmlWriter xmlTextWriter, string parentElement)
  {
    if (this.shape != null)
    {
      Stream stream;
      if (this.shape.PreservedElements.TryGetValue("Style", out stream))
      {
        if (stream == null || stream.Length <= 0L)
          return;
        stream.Position = 0L;
        ShapeParser.WriteNodeFromStream(xmlTextWriter, stream);
      }
      else
      {
        if (!this.shape.IsCreated)
          return;
        xmlTextWriter.WriteStartElement(this.attribute, parentElement, this.nameSpace);
        this.m_lnRefStyleEntry = new StyleOrFontReference(2, ColorModel.schemeClr, "accent1", -1.0, -1.0, -1.0, 50000.0);
        this.m_effectRefStyleEntry = new StyleOrFontReference(0, ColorModel.schemeClr, "accent1", -1.0, -1.0, -1.0, -1.0);
        this.m_fillRefStyleEntry = new StyleOrFontReference(1, ColorModel.schemeClr, "accent1", -1.0, -1.0, -1.0, -1.0);
        this.m_fontRefstyleEntry = new StyleOrFontReference(1, ColorModel.schemeClr, "lt1", -1.0, -1.0, -1.0, -1.0);
        this.SerializeStyleOrFontReference(xmlTextWriter, this.m_lnRefStyleEntry, "lnRef", false);
        this.SerializeStyleOrFontReference(xmlTextWriter, this.m_fillRefStyleEntry, "fillRef", false);
        this.SerializeStyleOrFontReference(xmlTextWriter, this.m_effectRefStyleEntry, "effectRef", false);
        this.SerializeStyleOrFontReference(xmlTextWriter, this.m_fontRefstyleEntry, "fontRef", true);
        xmlTextWriter.WriteEndElement();
      }
    }
    else
    {
      xmlTextWriter.WriteStartElement(this.attribute, parentElement, this.nameSpace);
      if (this.m_styleElementMod != StyleEntryModifierEnum.none)
      {
        string str = (this.m_styleElementMod & StyleEntryModifierEnum.allowNoFillOverride) == StyleEntryModifierEnum.allowNoFillOverride ? StyleEntryModifierEnum.allowNoFillOverride.ToString() : "";
        if ((this.m_styleElementMod & StyleEntryModifierEnum.allowNoLineOverride) == StyleEntryModifierEnum.allowNoLineOverride)
          str = $"{str} {StyleEntryModifierEnum.allowNoLineOverride.ToString()}";
        xmlTextWriter.WriteAttributeString("mods", str);
      }
      this.SerializeStyleOrFontReference(xmlTextWriter, this.m_lnRefStyleEntry, "lnRef", false);
      if (this.m_lineWidthScale != -1.0)
      {
        xmlTextWriter.WriteStartElement(this.attribute, "lineWidthScale", this.nameSpace);
        xmlTextWriter.WriteAttributeString("val", this.m_lineWidthScale.ToString());
        xmlTextWriter.WriteEndElement();
      }
      this.SerializeStyleOrFontReference(xmlTextWriter, this.m_fillRefStyleEntry, "fillRef", false);
      this.SerializeStyleOrFontReference(xmlTextWriter, this.m_effectRefStyleEntry, "effectRef", false);
      this.SerializeStyleOrFontReference(xmlTextWriter, this.m_fontRefstyleEntry, "fontRef", true);
      if (this.m_shapeProperties != null)
        this.SerializeShapeProperties(xmlTextWriter);
      if (this.m_defaultParagraphRunProperties != null)
        this.SerializeDefaultRPrProperties(xmlTextWriter);
      if (this.m_textBodyProperties != null)
        this.m_textBodyProperties.SerialzieTextBodyProperties(xmlTextWriter, this.attribute, this.nameSpace);
      xmlTextWriter.WriteEndElement();
    }
  }
}
