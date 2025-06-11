// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.InlineShapeObject
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;
using System;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class InlineShapeObject : ShapeObject
{
  private PICF m_inlinePictDesc;
  private MsofbtBSE m_curBSE;
  private MsofbtSpContainer m_shapeContainer;
  private int m_oleContainerId = -1;
  private byte[] m_unparsedData;
  private GradientFill m_lineGradient;
  private byte m_bFlags;

  internal bool IsHorizontalRule
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  internal GradientFill LineGradient
  {
    get
    {
      if (this.m_lineGradient == null)
        this.m_lineGradient = new GradientFill();
      return this.m_lineGradient;
    }
  }

  internal PICF PictureDescriptor
  {
    get => this.m_inlinePictDesc;
    set => this.m_inlinePictDesc = value;
  }

  internal MsofbtSpContainer ShapeContainer
  {
    get => this.m_shapeContainer;
    set => this.m_shapeContainer = value;
  }

  internal bool IsOLE
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal int OLEContainerId
  {
    get => this.m_oleContainerId;
    set => this.m_oleContainerId = value;
  }

  internal byte[] UnparsedData
  {
    get => this.m_unparsedData;
    set => this.m_unparsedData = value;
  }

  internal InlineShapeObject(IWordDocument doc)
    : base(doc)
  {
    this.m_curBSE = new MsofbtBSE(doc as WordDocument);
    this.m_shapeContainer = new MsofbtSpContainer(doc as WordDocument);
    this.m_inlinePictDesc = new PICF();
  }

  protected override object CloneImpl()
  {
    InlineShapeObject inlineShapeObject = (InlineShapeObject) base.CloneImpl();
    inlineShapeObject.m_inlinePictDesc = this.PictureDescriptor.Clone();
    if (this.ShapeContainer != null)
      inlineShapeObject.ShapeContainer = (MsofbtSpContainer) this.ShapeContainer.Clone();
    inlineShapeObject.IsCloned = true;
    return (object) inlineShapeObject;
  }

  internal LineDashing GetDashStyle(BorderStyle borderStyle, ref TextBoxLineStyle lineStyle)
  {
    LineDashing dashStyle = LineDashing.Solid;
    lineStyle = TextBoxLineStyle.Simple;
    switch (borderStyle)
    {
      case BorderStyle.Double:
      case BorderStyle.DoubleWave:
        lineStyle = TextBoxLineStyle.Double;
        break;
      case BorderStyle.Dot:
      case BorderStyle.DashSmallGap:
        dashStyle = LineDashing.DotGEL;
        break;
      case BorderStyle.DashLargeGap:
        dashStyle = LineDashing.DashGEL;
        break;
      case BorderStyle.DotDash:
        dashStyle = LineDashing.DashDotGEL;
        break;
      case BorderStyle.DotDotDash:
        dashStyle = LineDashing.LongDashDotDotGEL;
        break;
      case BorderStyle.Triple:
      case BorderStyle.ThinThickThinSmallGap:
      case BorderStyle.ThickThickThinMediumGap:
      case BorderStyle.ThinThickThinLargeGap:
        lineStyle = TextBoxLineStyle.Triple;
        break;
      case BorderStyle.ThinThickSmallGap:
      case BorderStyle.ThinThickMediumGap:
      case BorderStyle.ThinThickLargeGap:
      case BorderStyle.Inset:
        lineStyle = TextBoxLineStyle.ThinThick;
        break;
      case BorderStyle.ThinThinSmallGap:
      case BorderStyle.ThickThinMediumGap:
      case BorderStyle.ThickThinLargeGap:
      case BorderStyle.Outset:
        lineStyle = TextBoxLineStyle.ThickThin;
        break;
    }
    return dashStyle;
  }

  internal BorderStyle GetBorderStyle(LineDashing dashStyle, TextBoxLineStyle lineStyle)
  {
    BorderStyle borderStyle = BorderStyle.None;
    switch (dashStyle)
    {
      case LineDashing.Solid:
        switch (lineStyle)
        {
          case TextBoxLineStyle.Simple:
            break;
          case TextBoxLineStyle.Double:
            borderStyle = BorderStyle.Double;
            break;
          case TextBoxLineStyle.ThickThin:
            borderStyle = BorderStyle.ThickThinMediumGap;
            break;
          case TextBoxLineStyle.ThinThick:
            borderStyle = BorderStyle.ThinThickMediumGap;
            break;
          case TextBoxLineStyle.Triple:
            borderStyle = BorderStyle.ThickThickThinMediumGap;
            break;
          default:
            borderStyle = BorderStyle.Single;
            break;
        }
        break;
      case LineDashing.Dash:
      case LineDashing.DashGEL:
      case LineDashing.LongDashGEL:
        borderStyle = BorderStyle.DashLargeGap;
        break;
      case LineDashing.Dot:
      case LineDashing.DotGEL:
        borderStyle = BorderStyle.Dot;
        break;
      case LineDashing.DashDot:
      case LineDashing.LongDashDotGEL:
        borderStyle = BorderStyle.DotDash;
        break;
      case LineDashing.DashDotDot:
      case LineDashing.LongDashDotDotGEL:
        borderStyle = BorderStyle.DotDotDash;
        break;
      case LineDashing.DashDotGEL:
        borderStyle = BorderStyle.DotDash;
        break;
      default:
        borderStyle = BorderStyle.Single;
        break;
    }
    return borderStyle;
  }

  internal void ConvertToInlineShape()
  {
    uint num1 = 0;
    if (this.ShapeContainer.ShapeOptions.Properties.ContainsKey(459))
    {
      num1 = this.ShapeContainer.GetPropertyValue(459);
      this.ShapeContainer.ShapeOptions.Properties.Remove(459);
    }
    uint num2 = (uint) Math.Round((double) num1 / 12700.0 * 8.0);
    this.PictureDescriptor.BorderLeft.LineWidth = (byte) num2;
    this.PictureDescriptor.BorderTop.LineWidth = (byte) num2;
    this.PictureDescriptor.BorderRight.LineWidth = (byte) num2;
    this.PictureDescriptor.BorderBottom.LineWidth = (byte) num2;
    BorderStyle borderStyle = BorderStyle.None;
    if (this.ShapeContainer.ShapeOptions.Properties.ContainsKey(461))
    {
      TextBoxLineStyle propertyValue = (TextBoxLineStyle) this.ShapeContainer.GetPropertyValue(461);
      borderStyle = this.GetBorderStyle(LineDashing.Solid, propertyValue);
      if (propertyValue == TextBoxLineStyle.Simple)
        borderStyle = BorderStyle.Single;
      this.ShapeContainer.ShapeOptions.Properties.Remove(461);
    }
    if (this.ShapeContainer.ShapeOptions.Properties.ContainsKey(462))
    {
      LineDashing propertyValue = (LineDashing) this.ShapeContainer.GetPropertyValue(462);
      borderStyle = this.GetBorderStyle(propertyValue, TextBoxLineStyle.Simple);
      if (propertyValue == LineDashing.Solid && borderStyle == BorderStyle.None)
        borderStyle = BorderStyle.Single;
      this.ShapeContainer.ShapeOptions.Properties.Remove(462);
    }
    if (borderStyle != BorderStyle.None)
    {
      this.PictureDescriptor.BorderLeft.BorderType = (byte) borderStyle;
      this.PictureDescriptor.BorderTop.BorderType = (byte) borderStyle;
      this.PictureDescriptor.BorderRight.BorderType = (byte) borderStyle;
      this.PictureDescriptor.BorderBottom.BorderType = (byte) borderStyle;
    }
    if (this.ShapeContainer.ShapeOptions.Properties.ContainsKey(448))
    {
      uint propertyValue = this.ShapeContainer.GetPropertyValue(448);
      int id = WordColor.ConvertColorToId(WordColor.ConvertRGBToColor(propertyValue));
      this.PictureDescriptor.BorderLeft.LineColor = (byte) id;
      this.PictureDescriptor.BorderTop.LineColor = (byte) id;
      this.PictureDescriptor.BorderRight.LineColor = (byte) id;
      this.PictureDescriptor.BorderBottom.LineColor = (byte) id;
      this.ShapeContainer.ShapePosition.SetPropertyValue(924, propertyValue);
      this.ShapeContainer.ShapePosition.SetPropertyValue(923, propertyValue);
      this.ShapeContainer.ShapePosition.SetPropertyValue(926, propertyValue);
      this.ShapeContainer.ShapePosition.SetPropertyValue(925, propertyValue);
      this.ShapeContainer.ShapeOptions.Properties.Remove(448);
    }
    if (!this.ShapeContainer.ShapeOptions.LineProperties.HasDefined)
      return;
    this.ShapeContainer.ShapeOptions.Properties.Remove(511 /*0x01FF*/);
  }

  internal void ConvertToShape()
  {
    this.ShapeContainer.ShapeOptions.SetPropertyValue(459, (uint) Math.Round((double) this.PictureDescriptor.BorderLeft.LineWidth / 8.0 * 12700.0));
    Color color = this.PictureDescriptor.BorderLeft.LineColorExt;
    if (this.ShapeContainer.ShapePosition.Properties.ContainsKey(924))
    {
      color = WordColor.ConvertRGBToColor(this.ShapeContainer.ShapePosition.GetPropertyValue(924));
      this.ShapeContainer.ShapePosition.Properties.Remove(924);
    }
    this.ShapeContainer.ShapeOptions.SetPropertyValue(448, WordColor.ConvertColorToRGB(color));
    TextBoxLineStyle lineStyle = TextBoxLineStyle.Simple;
    LineDashing dashStyle = this.GetDashStyle((BorderStyle) this.PictureDescriptor.BorderLeft.BorderType, ref lineStyle);
    this.ShapeContainer.ShapeOptions.SetPropertyValue(461, (uint) lineStyle);
    this.ShapeContainer.ShapeOptions.SetPropertyValue(462, (uint) dashStyle);
    if (this.ShapeContainer.ShapePosition.Properties.ContainsKey(923))
      this.ShapeContainer.ShapePosition.Properties.Remove(923);
    if (this.ShapeContainer.ShapePosition.Properties.ContainsKey(926))
      this.ShapeContainer.ShapePosition.Properties.Remove(926);
    if (this.ShapeContainer.ShapePosition.Properties.ContainsKey(925))
      this.ShapeContainer.ShapePosition.Properties.Remove(925);
    this.PictureDescriptor.brcLeft = new BorderCode();
    this.PictureDescriptor.brcTop = new BorderCode();
    this.PictureDescriptor.brcRight = new BorderCode();
    this.PictureDescriptor.brcBottom = new BorderCode();
  }

  internal void GetEffectExtent(double borderWidth, ref long leftTop, ref long rightBottom)
  {
    int num = (int) (borderWidth / 1.5);
    if (borderWidth % 1.5 >= 1.0)
      num += (int) (borderWidth % 1.5);
    if (num == 0)
      num = 1;
    leftTop = (long) ((double) num * 1.5 * 12700.0);
    rightBottom = 0L;
    if (num <= 1)
      return;
    rightBottom = (long) ((double) (num - 1) * 1.5 * 12700.0);
  }

  protected override void WriteXmlContent(IXDLSContentWriter writer)
  {
    base.WriteXmlContent(writer);
    if (this.m_inlinePictDesc != null)
    {
      MemoryStream memoryStream = new MemoryStream();
      this.m_inlinePictDesc.Write((Stream) memoryStream);
      byte[] array = memoryStream.ToArray();
      memoryStream.Close();
      writer.WriteChildBinaryElement("PictureDescriptor", array);
    }
    if (this.ShapeContainer == null)
      return;
    MemoryStream memoryStream1 = new MemoryStream();
    this.ShapeContainer.WriteMsofbhWithRecord((Stream) memoryStream1);
    byte[] array1 = memoryStream1.ToArray();
    memoryStream1.Close();
    writer.WriteChildBinaryElement("ShapeContainer", array1);
    this.m_curBSE = this.ShapeContainer.Bse;
    if (this.m_curBSE == null)
      return;
    MemoryStream memoryStream2 = new MemoryStream();
    this.m_curBSE.Write((Stream) memoryStream2);
    byte[] array2 = memoryStream2.ToArray();
    writer.WriteChildBinaryElement("ShapeBlip", array2);
    MemoryStream memoryStream3 = new MemoryStream();
    this.m_curBSE.WriteMsofbhWithRecord((Stream) memoryStream3);
    byte[] array3 = memoryStream3.ToArray();
    writer.WriteChildBinaryElement("ShapeFbse", array3);
    memoryStream2.Close();
    memoryStream3.Close();
  }

  protected override bool ReadXmlContent(IXDLSContentReader reader)
  {
    bool flag = base.ReadXmlContent(reader);
    if (reader.TagName == "PictureDescriptor")
    {
      byte[] buffer = reader.ReadChildBinaryElement();
      if (buffer.Length != 0)
      {
        MemoryStream input = new MemoryStream(buffer, 0, buffer.Length);
        this.m_inlinePictDesc.Read(new BinaryReader((Stream) input));
        input.Close();
      }
    }
    if (reader.TagName == "ShapeContainer")
    {
      byte[] buffer = reader.ReadChildBinaryElement();
      if (buffer.Length != 0)
      {
        MemoryStream memoryStream = new MemoryStream(buffer, 0, buffer.Length);
        this.ShapeContainer = new MsofbtSpContainer(this.Document);
        memoryStream.Position = 0L;
        this.ShapeContainer.ReadMsofbhWithRecord((Stream) memoryStream);
        memoryStream.Close();
        flag = true;
      }
    }
    if (reader.TagName == "ShapeBlip")
    {
      byte[] buffer = reader.ReadChildBinaryElement();
      MemoryStream memoryStream = new MemoryStream(buffer, 0, buffer.Length);
      MsofbtBSE msofbtBse = new MsofbtBSE(this.Document);
      msofbtBse.Read((Stream) memoryStream);
      this.m_curBSE = msofbtBse;
    }
    if (reader.TagName == "ShapeFbse")
    {
      byte[] buffer = reader.ReadChildBinaryElement();
      this.m_curBSE.ReadMsofbhWithRecord((Stream) new MemoryStream(buffer, 0, buffer.Length));
      this.ShapeContainer.Bse = new MsofbtBSE(this.Document);
      this.ShapeContainer.Bse = this.m_curBSE;
    }
    return flag;
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    if (reader.HasAttribute("IsOLE"))
      this.IsOLE = reader.ReadBoolean("IsOLE");
    if (!reader.HasAttribute("OLEContainerId"))
      return;
    this.m_oleContainerId = reader.ReadInt("OLEContainerId");
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    writer.WriteValue("type", (Enum) ParagraphItemType.InlineShapeObject);
    if (!this.IsOLE)
      return;
    writer.WriteValue("IsOLE", this.IsOLE);
    writer.WriteValue("OLEContainerId", this.OLEContainerId);
  }

  internal override void CloneRelationsTo(WordDocument doc, OwnerHolder nextOwner)
  {
    base.CloneRelationsTo(doc, nextOwner);
    if (this.ShapeContainer == null)
      return;
    this.ShapeContainer.CloneRelationsTo(doc);
  }

  internal override void Close()
  {
    this.m_inlinePictDesc = (PICF) null;
    if (this.m_curBSE != null)
    {
      this.m_curBSE.Close();
      this.m_curBSE = (MsofbtBSE) null;
    }
    if (this.m_shapeContainer != null)
    {
      this.m_shapeContainer.Close();
      this.m_shapeContainer = (MsofbtSpContainer) null;
    }
    this.m_unparsedData = (byte[]) null;
    if (this.m_lineGradient != null)
    {
      this.m_lineGradient.Close();
      this.m_lineGradient = (GradientFill) null;
    }
    base.Close();
  }
}
