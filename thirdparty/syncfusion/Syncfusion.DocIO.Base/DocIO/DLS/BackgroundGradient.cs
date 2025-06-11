// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.BackgroundGradient
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class BackgroundGradient : XDLSSerializableBase
{
  internal const uint DEF_VERTICAL_ANGLE = 4289069056;
  internal const uint DEF_DIAGONALUP_ANGLE = 4286119936;
  internal const uint DEF_DIAGONALDOWN_ANGLE = 4292018176;
  internal const uint DEF_SHADEUP_VARIANT = 100;
  internal const uint DEF_SHADEOUT_VARIANT = 4294967246;
  internal const uint DEF_SHADEMIDDLE_VARIANT = 50;
  private BackgroundFillType m_fillType;
  private Color m_fillColor = Color.White;
  private Color m_fillBackColor = Color.White;
  private GradientShadingStyle m_shadingStyle;
  private GradientShadingVariant m_shadingVariant;
  private EscherClass m_escher;

  public Color Color1
  {
    get => this.m_fillColor;
    set => this.m_fillColor = value;
  }

  public Color Color2
  {
    get => this.m_fillBackColor;
    set => this.m_fillBackColor = value;
  }

  public GradientShadingStyle ShadingStyle
  {
    get => this.m_shadingStyle;
    set => this.m_shadingStyle = value;
  }

  public GradientShadingVariant ShadingVariant
  {
    get => this.m_shadingVariant;
    set => this.m_shadingVariant = value;
  }

  public BackgroundGradient()
    : base((WordDocument) null, (Entity) null)
  {
    this.m_fillColor = Color.White;
    this.m_fillBackColor = Color.Black;
  }

  internal BackgroundGradient(WordDocument doc, MsofbtSpContainer container)
    : base(doc, (Entity) null)
  {
    this.m_escher = doc.Escher;
    this.GetGradientData(container);
  }

  public BackgroundGradient Clone() => (BackgroundGradient) this.CloneImpl();

  internal new void CloneRelationsTo(WordDocument doc, OwnerHolder nextOwner)
  {
    base.CloneRelationsTo(doc, nextOwner);
  }

  private void GetGradientData(MsofbtSpContainer container)
  {
    this.m_fillType = container.GetBackgroundFillType();
    this.m_fillColor = container.GetBackgroundColor(false);
    this.m_fillBackColor = container.GetBackgroundColor(true);
    this.m_shadingStyle = container.GetGradientShadingStyle(this.m_fillType);
    this.m_shadingVariant = container.GetGradientShadingVariant(this.m_shadingStyle);
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    if (this.m_fillColor != Color.White)
      writer.WriteValue("FillColor", this.m_fillColor);
    if (this.m_fillBackColor != Color.White)
      writer.WriteValue("FillBackgroundColor", this.m_fillBackColor);
    if (this.m_shadingStyle != GradientShadingStyle.Horizontal)
      writer.WriteValue("GradientShadingStyle", (Enum) this.m_shadingStyle);
    if (this.m_shadingVariant == GradientShadingVariant.ShadingUp)
      return;
    writer.WriteValue("GradientShadingVariant", (Enum) this.m_shadingVariant);
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    if (reader.HasAttribute("FillColor"))
      this.m_fillColor = reader.ReadColor("FillColor");
    if (reader.HasAttribute("FillBackgroundColor"))
      this.m_fillBackColor = reader.ReadColor("FillBackgroundColor");
    if (reader.HasAttribute("GradientShadingStyle"))
      this.m_shadingStyle = (GradientShadingStyle) reader.ReadEnum("GradientShadingStyle", typeof (GradientShadingStyle));
    if (!reader.HasAttribute("GradientShadingVariant"))
      return;
    this.m_shadingVariant = (GradientShadingVariant) reader.ReadEnum("GradientShadingVariant", typeof (GradientShadingVariant));
  }

  internal override void Close()
  {
    base.Close();
    if (this.m_escher == null)
      return;
    this.m_escher.Close();
    this.m_escher = (EscherClass) null;
  }
}
