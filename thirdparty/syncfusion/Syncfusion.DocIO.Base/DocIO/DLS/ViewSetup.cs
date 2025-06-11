// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ViewSetup
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class ViewSetup : XDLSSerializableBase
{
  public const int DEF_ZOOMING = 100;
  private ZoomType m_zoomType;
  private int m_zoomPercent;
  private DocumentViewType m_docViewType;

  public int ZoomPercent
  {
    get => this.m_zoomPercent;
    set
    {
      this.m_zoomPercent = value >= 10 && value <= 500 ? value : throw new ArgumentOutOfRangeException("Zoom percentage must be between 10 and 500 percent.");
    }
  }

  public ZoomType ZoomType
  {
    get => this.m_zoomType;
    set => this.m_zoomType = value;
  }

  public DocumentViewType DocumentViewType
  {
    get => this.m_docViewType;
    set => this.m_docViewType = value;
  }

  public ViewSetup(IWordDocument doc)
    : base((WordDocument) doc, (Entity) null)
  {
    this.m_zoomType = ZoomType.None;
    this.m_docViewType = DocumentViewType.PrintLayout;
    this.m_zoomPercent = 100;
  }

  internal ViewSetup Clone(WordDocument doc)
  {
    ViewSetup viewSetup = (ViewSetup) this.CloneImpl();
    viewSetup.SetOwner((OwnerHolder) doc);
    return viewSetup;
  }

  internal void SetZoomPercentValue(int value)
  {
    if (value == 0)
      value = 100;
    else if (value < 10)
      value = 10;
    else if (value > 500)
      value = 500;
    if (value < 10 || value > 500)
      return;
    this.m_zoomPercent = value;
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    if (this.ZoomPercent != 100)
      writer.WriteValue("ZoomPercent", this.ZoomPercent);
    if (this.ZoomType != ZoomType.None)
      writer.WriteValue("ZoomType", (Enum) this.ZoomType);
    if (this.DocumentViewType == DocumentViewType.PrintLayout)
      return;
    writer.WriteValue("ViewType", (Enum) this.DocumentViewType);
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    if (reader.HasAttribute("ZoomPercent"))
      this.ZoomPercent = reader.ReadInt("ZoomPercent");
    if (reader.HasAttribute("ZoomType"))
      this.ZoomType = (ZoomType) reader.ReadEnum("ZoomType", typeof (ZoomType));
    if (!reader.HasAttribute("ViewType"))
      return;
    this.DocumentViewType = (DocumentViewType) reader.ReadEnum("ViewType", typeof (DocumentViewType));
  }
}
