// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.OverrideLevelFormat
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class OverrideLevelFormat : XDLSSerializableBase
{
  private int m_startAt;
  private byte m_bFlags;
  private WListLevel m_lfoLevel;
  internal int m_reserved1;
  internal int m_reserved2;
  internal int m_reserved3;

  internal OverrideLevelFormat(WordDocument doc)
    : base(doc, (Entity) null)
  {
    this.m_lfoLevel = new WListLevel(this.Document);
    this.m_lfoLevel.SetOwner((OwnerHolder) this);
  }

  internal bool OverrideStartAtValue
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal bool OverrideFormatting
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  internal int StartAt
  {
    get => this.m_startAt;
    set => this.m_startAt = value;
  }

  internal WListLevel OverrideListLevel
  {
    get => this.m_lfoLevel;
    set => this.m_lfoLevel = value;
  }

  protected override void InitXDLSHolder()
  {
    base.InitXDLSHolder();
    this.XDLSHolder.AddElement("level-override", (object) this.m_lfoLevel);
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    if (this.OverrideStartAtValue)
    {
      writer.WriteValue("ChangeStartAt", this.OverrideStartAtValue);
      writer.WriteValue("StartAt", this.m_startAt);
    }
    if (this.OverrideFormatting)
      writer.WriteValue("ChangeFormat", this.OverrideFormatting);
    if (this.m_reserved1 != 0)
      writer.WriteValue("Reserved1", this.m_reserved1);
    if (this.m_reserved2 != 0)
      writer.WriteValue("Reserved2", this.m_reserved2);
    if (this.m_reserved3 == 0)
      return;
    writer.WriteValue("Reserved3", this.m_reserved3);
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    if (reader.HasAttribute("ChangeFormat"))
      this.OverrideFormatting = reader.ReadBoolean("ChangeFormat");
    if (reader.HasAttribute("ChangeStartAt"))
      this.OverrideStartAtValue = reader.ReadBoolean("ChangeStartAt");
    if (reader.HasAttribute("StartAt"))
      this.m_startAt = reader.ReadInt("StartAt");
    if (reader.HasAttribute("Reserved1"))
      this.m_reserved1 = reader.ReadInt("Reserved1");
    if (reader.HasAttribute("Reserved2"))
      this.m_reserved2 = reader.ReadInt("Reserved2");
    if (!reader.HasAttribute("Reserved3"))
      return;
    this.m_reserved3 = reader.ReadInt("Reserved3");
  }

  protected override object CloneImpl()
  {
    OverrideLevelFormat owner = (OverrideLevelFormat) base.CloneImpl();
    owner.OverrideListLevel = this.OverrideListLevel.Clone();
    owner.OverrideListLevel.SetOwner((OwnerHolder) owner);
    return (object) owner;
  }

  internal new void Close()
  {
    if (this.m_lfoLevel != null)
    {
      this.m_lfoLevel.Close();
      this.m_lfoLevel = (WListLevel) null;
    }
    base.Close();
  }

  internal bool Compare(OverrideLevelFormat overrideLevelFormat)
  {
    if (this.OverrideFormatting != overrideLevelFormat.OverrideFormatting || this.OverrideStartAtValue != overrideLevelFormat.OverrideStartAtValue || this.StartAt != overrideLevelFormat.StartAt)
      return false;
    if (this.OverrideListLevel != null && overrideLevelFormat.OverrideListLevel != null)
    {
      if (this.OverrideListLevel.CharacterFormat != null && this.OverrideListLevel.CharacterFormat != null)
      {
        if (!this.OverrideListLevel.CharacterFormat.Compare(overrideLevelFormat.OverrideListLevel.CharacterFormat))
          return false;
      }
      else if (this.OverrideListLevel.CharacterFormat == null && this.OverrideListLevel.CharacterFormat != null || this.OverrideListLevel.CharacterFormat != null && this.OverrideListLevel.CharacterFormat == null)
        return false;
      if (this.OverrideListLevel.ParagraphFormat != null && this.OverrideListLevel.ParagraphFormat != null)
      {
        if (!this.OverrideListLevel.ParagraphFormat.Compare(overrideLevelFormat.OverrideListLevel.ParagraphFormat))
          return false;
      }
      else if (this.OverrideListLevel.ParagraphFormat == null && this.OverrideListLevel.ParagraphFormat != null || this.OverrideListLevel.ParagraphFormat != null && this.OverrideListLevel.ParagraphFormat == null)
        return false;
    }
    else if (this.OverrideListLevel == null && overrideLevelFormat.OverrideListLevel != null || this.OverrideListLevel != null && overrideLevelFormat.OverrideListLevel == null)
      return false;
    return true;
  }
}
