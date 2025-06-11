// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WDropDownItem
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WDropDownItem(IWordDocument doc) : XDLSSerializableBase((WordDocument) doc, (Entity) null)
{
  private string m_text = string.Empty;

  public string Text
  {
    get => this.m_text;
    set => this.m_text = value;
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    writer.WriteValue("itemText", this.m_text);
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    if (!reader.HasAttribute("itemText"))
      return;
    this.m_text = reader.ReadString("itemText");
  }

  internal WDropDownItem Clone() => (WDropDownItem) this.CloneImpl();
}
