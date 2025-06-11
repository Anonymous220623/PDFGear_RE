// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WEmbedField
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class WEmbedField : WField
{
  protected internal int m_storagePicLocation;
  private byte m_bFlags;

  public override EntityType EntityType => EntityType.EmbededField;

  internal int StoragePicLocation
  {
    get => this.m_storagePicLocation;
    set => this.m_storagePicLocation = value;
  }

  internal bool IsOle2
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal WEmbedField(IWordDocument doc)
    : base(doc)
  {
    this.m_paraItemType = ParagraphItemType.EmbedField;
  }

  protected override void InitXDLSHolder() => base.InitXDLSHolder();

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    if (!reader.HasAttribute("StoragePicLocation"))
      return;
    this.m_storagePicLocation = reader.ReadInt("StoragePicLocation");
    this.IsOle2 = reader.ReadBoolean("Ole2Object");
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    if (this.m_storagePicLocation <= 0)
      return;
    writer.WriteValue("StoragePicLocation", this.m_storagePicLocation);
    writer.WriteValue("Ole2Object", this.IsOle2);
  }

  protected override object CloneImpl() => (object) (WEmbedField) base.CloneImpl();
}
