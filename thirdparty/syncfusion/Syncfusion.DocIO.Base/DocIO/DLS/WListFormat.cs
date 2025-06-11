// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WListFormat
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WListFormat : FormatBase
{
  internal const int ListLevelNumberKey = 0;
  private const int ListTypeKey = 1;
  internal const int CustomStyleNameKey = 2;
  private const int RestartKey = 3;
  internal const int LfoStyleNameKey = 4;
  internal const int DEF_START_LISTID = 1720085641;
  [ThreadStatic]
  private static string m_currentStyleName;
  [ThreadStatic]
  private static int m_currLevelNumber;
  private byte m_bFlags;

  public int ListLevelNumber
  {
    get => (int) this[0];
    set
    {
      this[0] = value <= 8 && value >= 0 ? (object) value : throw new ArgumentException("List level must be less 8 and greater then 0");
      WListFormat.m_currLevelNumber = value;
    }
  }

  public ListType ListType => (ListType) this[1];

  public bool RestartNumbering
  {
    get => (bool) this[3];
    set
    {
      if (value && this.CurrentListStyle != null)
      {
        ListStyle style = this.CurrentListStyle.Clone() as ListStyle;
        style.Name = this.CurrentListStyle.ListType == ListType.Bulleted ? "Bulleted_" + Guid.NewGuid().ToString() : "Numbered_" + Guid.NewGuid().ToString();
        style.SetNewListID(this.Document);
        this.m_doc.ListStyles.Add(style);
        this[2] = (object) style.Name;
        WListFormat.m_currentStyleName = style.Name;
        this[1] = (object) style.ListType;
      }
      this[3] = (object) value;
    }
  }

  public string CustomStyleName => (string) this[2];

  public ListStyle CurrentListStyle
  {
    get
    {
      return (string) this[2] != string.Empty ? this.Document.ListStyles.FindByName(this.CustomStyleName) : (ListStyle) null;
    }
  }

  public WListLevel CurrentListLevel
  {
    get
    {
      if ((string) this[2] == string.Empty)
        return (WListLevel) null;
      return this.ListLevelNumber >= this.CurrentListStyle.Levels.Count ? (WListLevel) null : this.CurrentListStyle.Levels[this.ListLevelNumber];
    }
  }

  internal string LFOStyleName
  {
    get => (string) this[4];
    set => this[4] = (object) value;
  }

  internal WParagraph OwnerParagraph => (WParagraph) this.OwnerBase;

  internal bool IsListRemoved
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal bool IsEmptyList
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  public WListFormat(IWParagraph owner)
    : base((IWordDocument) owner.Document, (Entity) owner)
  {
  }

  public WListFormat(WordDocument doc, WParagraphStyle owner)
    : base((IWordDocument) doc)
  {
    this.SetOwner((OwnerHolder) owner);
  }

  internal WListFormat(WordDocument doc, WNumberingStyle owner)
    : base((IWordDocument) doc)
  {
    this.SetOwner((OwnerHolder) owner);
  }

  internal WListFormat(WordDocument doc, WTableStyle owner)
    : base((IWordDocument) doc)
  {
    this.SetOwner((OwnerHolder) owner);
  }

  protected override object GetDefValue(int key)
  {
    switch (key)
    {
      case 0:
        return (object) 0;
      case 1:
        return (object) ListType.NoList;
      case 2:
        return (object) string.Empty;
      case 3:
        return (object) false;
      case 4:
        return (object) null;
      default:
        throw new ArgumentException("key has invalid value");
    }
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    if (this.HasKey(0))
      writer.WriteValue("LevelNumber", this.ListLevelNumber);
    if (this.HasKey(2))
      writer.WriteValue("Name", this.CustomStyleName);
    if (this.HasKey(1))
      writer.WriteValue("ListType", (Enum) this.ListType);
    if (!this.HasKey(4))
      return;
    writer.WriteValue("LfoStyleName", this.LFOStyleName);
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    if (reader.HasAttribute("LfoStyleName"))
      this.LFOStyleName = reader.ReadString("LfoStyleName");
    if (reader.HasAttribute("LevelNumber"))
      this[0] = (object) reader.ReadInt("LevelNumber");
    if (reader.HasAttribute("Name"))
      this[2] = (object) reader.ReadString("Name");
    if (!reader.HasAttribute("ListType"))
      return;
    this[1] = (object) (ListType) reader.ReadEnum("ListType", typeof (ListType));
  }

  private void UpdateStyleNameAndType(ListStyle destinationListStyle)
  {
    this[2] = (object) destinationListStyle.Name;
    WListFormat.m_currentStyleName = destinationListStyle.Name;
    this[1] = (object) destinationListStyle.ListType;
  }

  private bool IsListStyleAdded(WordDocument destDocument, string name, ref string newStyleName)
  {
    if (string.IsNullOrEmpty(destDocument.Settings.DuplicateListStyleNames))
      return false;
    string[] strArray = destDocument.Settings.DuplicateListStyleNames.Split(',');
    for (int index = 0; index + 1 < strArray.Length; index += 2)
    {
      if (strArray[index] == name)
      {
        newStyleName = strArray[index + 1];
        return true;
      }
    }
    return false;
  }

  private void AddListStyleToDestination(WordDocument doc, ListStyle destinationListStyle)
  {
    string styleName = destinationListStyle.Name;
    while (destinationListStyle.IsSameListNameOrIDExists(doc.ListStyles, 0L, styleName))
      styleName = destinationListStyle.ListType == ListType.Bulleted ? "Bulleted_" + Guid.NewGuid().ToString() : "Numbered_" + Guid.NewGuid().ToString();
    Settings settings = doc.Settings;
    settings.DuplicateListStyleNames = $"{settings.DuplicateListStyleNames}{destinationListStyle.Name},{styleName},";
    if (styleName != destinationListStyle.Name)
    {
      destinationListStyle.Name = styleName;
      this.UpdateStyleNameAndType(destinationListStyle);
    }
    doc.ListStyles.Add(destinationListStyle);
  }

  internal void CloneListRelationsTo(WordDocument doc, string styleName)
  {
    if (this.ListType != ListType.NoList && this.CurrentListStyle != null)
    {
      ListStyle currentListStyle = this.CurrentListStyle;
      ListStyle listStyle = doc.ListStyles.GetEquivalentStyle(currentListStyle);
      if (currentListStyle != null)
      {
        if (listStyle == null || (doc.ImportOptions & ImportOptions.ListRestartNumbering) != (ImportOptions) 0 && listStyle.ListType == ListType.Numbered)
        {
          string empty = string.Empty;
          if (listStyle == null)
          {
            listStyle = (ListStyle) currentListStyle.Clone();
            if (doc.ListStyles.HasSameListId(listStyle))
              listStyle.SetNewListID(doc);
          }
          else
          {
            listStyle = (ListStyle) currentListStyle.Clone();
            listStyle.SetNewListID(doc);
          }
          if (this.IsListStyleAdded(doc, listStyle.Name, ref empty))
          {
            listStyle = doc.ListStyles.FindByName(empty);
            this.UpdateStyleNameAndType(listStyle);
          }
          else
            this.AddListStyleToDestination(doc, listStyle);
        }
        else
          this.UpdateStyleNameAndType(listStyle);
        if (styleName != null && listStyle != null && this.ListLevelNumber < listStyle.Levels.Count && listStyle.Levels[this.ListLevelNumber] != null)
          listStyle.Levels[this.ListLevelNumber].ParaStyleName = styleName;
      }
    }
    if (this.LFOStyleName == null)
      return;
    ListOverrideStyle byName = this.Document.ListOverrides.FindByName(this.LFOStyleName);
    if (byName == null)
      return;
    ListOverrideStyle equivalentStyle = doc.ListOverrides.GetEquivalentStyle(byName);
    if (equivalentStyle == null)
      doc.ListOverrides.Add((ListOverrideStyle) byName.Clone());
    else
      this.LFOStyleName = equivalentStyle.Name;
  }

  internal void ImportListFormat(WListFormat srcListFormat)
  {
    this.ImportContainer((FormatBase) srcListFormat);
    if (srcListFormat.ListType != ListType.NoList)
    {
      ListStyle currentListStyle = srcListFormat.CurrentListStyle;
      if (currentListStyle != null && this.Document.ListStyles.FindByName(currentListStyle.Name) == null)
        this.Document.ListStyles.Add((ListStyle) currentListStyle.Clone());
    }
    if (srcListFormat.LFOStyleName == null || srcListFormat.Document == null || srcListFormat.Document.ListOverrides == null || this.Document.ListOverrides.FindByName(srcListFormat.LFOStyleName) != null)
      return;
    ListOverrideStyle byName = srcListFormat.Document.ListOverrides.FindByName(srcListFormat.LFOStyleName);
    if (byName == null)
      return;
    this.Document.ListOverrides.Add((ListOverrideStyle) byName.Clone());
  }

  public void IncreaseIndentLevel()
  {
    if (WListFormat.m_currLevelNumber == 8)
      throw new ArgumentException("List level must be less 8 and greater then 0");
    this[0] = (object) ++WListFormat.m_currLevelNumber;
  }

  public void DecreaseIndentLevel()
  {
    if (WListFormat.m_currLevelNumber == 0)
      throw new ArgumentException("List level must be less 8 and greater then 0");
    this[0] = (object) --WListFormat.m_currLevelNumber;
  }

  public void ContinueListNumbering()
  {
    this.ApplyStyle(WListFormat.m_currentStyleName);
    this.ListLevelNumber = WListFormat.m_currLevelNumber;
  }

  public void ApplyStyle(string styleName)
  {
    this[2] = (object) styleName;
    WListFormat.m_currentStyleName = styleName;
    if (!string.IsNullOrEmpty(WListFormat.m_currentStyleName))
    {
      Style byName = (Style) this.Document.Styles.FindByName(this.Document.ListStyles.FindByName(styleName).StyleLink);
      if (byName != null && byName.IsCustom && this.OwnerBase is WParagraph && byName.RangeCollection.Contains((Entity) this.OwnerParagraph))
        byName.RangeCollection.Remove((Entity) this.OwnerParagraph);
    }
    ListStyle listStyle = (ListStyle) null;
    if (!string.IsNullOrEmpty(styleName))
    {
      listStyle = this.Document.ListStyles.FindByName(styleName);
      if (this.OwnerBase is WParagraph)
      {
        Style byName = (Style) this.Document.Styles.FindByName(listStyle.StyleLink);
        if (byName != null && byName.IsCustom)
          byName.RangeCollection.Add((Entity) this.OwnerParagraph);
      }
    }
    if (listStyle == null)
      return;
    this[1] = (object) listStyle.ListType;
  }

  public void ApplyDefBulletStyle()
  {
    if (this.Document.ListStyles.FindByName("Bulleted") == null)
      this.CreateDefListStyles(ListType.Bulleted);
    this.ApplyStyle("Bulleted");
  }

  public void ApplyDefNumberedStyle()
  {
    if (this.Document.ListStyles.FindByName("Numbered") == null)
      this.CreateDefListStyles(ListType.Numbered);
    this.ApplyStyle("Numbered");
  }

  public void RemoveList()
  {
    this[2] = (object) string.Empty;
    WListFormat.m_currentStyleName = string.Empty;
    this[1] = (object) ListType.NoList;
    this.IsListRemoved = true;
  }

  public override void ClearFormatting()
  {
    if (this.m_propertiesHash != null)
      this.m_propertiesHash.Clear();
    if (this.m_sprms != null)
      this.m_sprms.Clear();
    this.RemoveList();
  }

  private void CreateDefListStyles(ListType listType)
  {
    if (listType == ListType.Numbered)
      this.Document.ListStyles.Add(new ListStyle((IWordDocument) this.Document, ListType.Numbered)
      {
        Name = "Numbered",
        ListType = ListType.Numbered
      });
    else
      this.Document.ListStyles.Add(new ListStyle((IWordDocument) this.Document, ListType.Bulleted)
      {
        Name = "Bulleted",
        ListType = ListType.Bulleted
      });
  }

  internal bool Compare(WListFormat listFormat)
  {
    if (this.ListLevelNumber != listFormat.ListLevelNumber)
      return false;
    if (listFormat.ListType != ListType.NoList && this.ListType != ListType.NoList)
    {
      ListStyle currentListStyle = listFormat.CurrentListStyle;
      if (currentListStyle != null && !this.Document.ListStyles.HasEquivalentStyle(currentListStyle))
        return false;
    }
    if (listFormat.ListType == ListType.NoList && this.ListType != ListType.NoList || listFormat.ListType != ListType.NoList && this.ListType == ListType.NoList)
      return false;
    ListOverrideStyle byName = this.Document.ListOverrides.FindByName(this.LFOStyleName);
    return byName == null || listFormat.Document.ListOverrides.HasEquivalentStyle(byName);
  }
}
