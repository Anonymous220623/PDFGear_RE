// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ListOverrideStyle
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class ListOverrideStyle : Style
{
  private ListOverrideLevelCollection m_overrideLevels;
  internal int m_res1;
  internal int m_res2;
  internal string listStyleName;
  internal int m_unused1;
  internal int m_unused2;
  private long m_listID = 1720085641;

  public override StyleType StyleType => StyleType.OtherStyle;

  internal ListOverrideLevelCollection OverrideLevels => this.m_overrideLevels;

  internal long ListID
  {
    get => this.m_listID;
    set => this.m_listID = value;
  }

  internal ListOverrideStyle(WordDocument doc)
    : base(doc)
  {
    this.m_overrideLevels = new ListOverrideLevelCollection(doc);
    this.m_overrideLevels.SetOwner((OwnerHolder) this);
  }

  public override IStyle Clone() => (IStyle) this.CloneImpl();

  protected override object CloneImpl()
  {
    ListOverrideStyle owner = (ListOverrideStyle) base.CloneImpl();
    owner.m_overrideLevels = new ListOverrideLevelCollection(this.Document);
    owner.m_overrideLevels.SetOwner((OwnerHolder) owner);
    this.m_overrideLevels.CloneToImpl((CollectionImpl) owner.m_overrideLevels);
    return (object) owner;
  }

  internal override void CloneRelationsTo(WordDocument doc, OwnerHolder nextOwner)
  {
    if (doc == this.Document)
      return;
    this.SetOwner((OwnerHolder) doc);
    this.OverrideLevels.SetOwner((OwnerHolder) this);
    foreach (OverrideLevelFormat overrideLevel in (CollectionImpl) this.OverrideLevels)
    {
      overrideLevel.SetOwner((OwnerHolder) this);
      overrideLevel.OverrideListLevel.SetOwner((OwnerHolder) overrideLevel);
      overrideLevel.OverrideListLevel.CharacterFormat.SetOwner((OwnerHolder) overrideLevel.OverrideListLevel);
      overrideLevel.OverrideListLevel.ParagraphFormat.SetOwner((OwnerHolder) overrideLevel.OverrideListLevel);
      if (overrideLevel.OverrideListLevel.PicBullet != null)
        overrideLevel.OverrideListLevel.PicBullet.CloneRelationsTo(this.Document, (OwnerHolder) overrideLevel.OverrideListLevel);
    }
  }

  internal override void Close()
  {
    base.Close();
    if (this.m_overrideLevels == null || this.m_overrideLevels.Count == 0)
    {
      this.m_overrideLevels = (ListOverrideLevelCollection) null;
    }
    else
    {
      OverrideLevelFormat overrideLevelFormat = (OverrideLevelFormat) null;
      int count = this.m_overrideLevels.Count;
      foreach (KeyValuePair<short, short> keyValuePair in this.m_overrideLevels.LevelIndex)
      {
        this.m_overrideLevels[(int) keyValuePair.Key].Close();
        overrideLevelFormat = (OverrideLevelFormat) null;
      }
      this.m_overrideLevels.Close();
      this.m_overrideLevels = (ListOverrideLevelCollection) null;
    }
  }

  protected override void InitXDLSHolder()
  {
    base.InitXDLSHolder();
    this.XDLSHolder.AddElement("override-levels", (object) this.m_overrideLevels);
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    if (this.m_res1 != 0)
      writer.WriteValue("Res1", this.m_res1);
    if (this.m_res2 != 0)
      writer.WriteValue("Res2", this.m_res2);
    if (this.m_unused1 != 0)
      writer.WriteValue("Unused1", this.m_unused1);
    if (this.m_unused2 == 0)
      return;
    writer.WriteValue("Unused2", this.m_unused2);
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    if (reader.HasAttribute("Res1"))
      this.m_res1 = reader.ReadInt("Res1");
    if (reader.HasAttribute("Res2"))
      this.m_res2 = reader.ReadInt("Res2");
    if (reader.HasAttribute("Unused1"))
      this.m_unused1 = reader.ReadInt("Unused1");
    if (!reader.HasAttribute("Unused2"))
      return;
    this.m_unused2 = reader.ReadInt("Unused2");
  }

  internal bool Compare(ListOverrideStyle listOverrideStyle)
  {
    if (this.ListID != listOverrideStyle.ListID || this.IsCustom != listOverrideStyle.IsCustom || this.IsPrimaryStyle != listOverrideStyle.IsPrimaryStyle || this.IsSemiHidden != listOverrideStyle.IsSemiHidden || this.LinkStyle != listOverrideStyle.LinkStyle || this.UnhideWhenUsed != listOverrideStyle.UnhideWhenUsed)
      return false;
    if (this.OverrideLevels != null && listOverrideStyle.OverrideLevels != null)
    {
      if (!this.OverrideLevels.Compare(listOverrideStyle.OverrideLevels))
        return false;
    }
    else if (this.OverrideLevels != null && listOverrideStyle.OverrideLevels == null || this.OverrideLevels == null && listOverrideStyle.OverrideLevels != null)
      return false;
    return this.IsEquivalentListStyle(listOverrideStyle.listStyleName, this.listStyleName, listOverrideStyle.Document);
  }

  private bool IsEquivalentListStyle(
    string sourceListStyleName,
    string destListStyleName,
    WordDocument doc)
  {
    ListStyle listStyle1 = (ListStyle) null;
    if (doc != null)
      listStyle1 = doc.ListStyles.FindByName(sourceListStyleName);
    ListStyle listStyle2 = (ListStyle) null;
    if (this.Document != null)
      listStyle2 = this.Document.ListStyles.FindByName(destListStyleName);
    return listStyle1 == null && listStyle2 == null || listStyle1 != null && listStyle2 != null && listStyle1.Compare(listStyle2);
  }
}
