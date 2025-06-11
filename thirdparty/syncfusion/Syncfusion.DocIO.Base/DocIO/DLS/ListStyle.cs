// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ListStyle
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class ListStyle : XDLSSerializableBase, IStyle
{
  private const int DEF_MULTIPLIER = 72;
  internal const string DEF_BULLLET_FIRST = "\uF0B7";
  internal const string DEF_BULLLET_SECOND = "o";
  internal const string DEF_BULLLET_THIRD = "\uF0A7";
  private ListLevelCollection m_levels;
  private ListType m_listType;
  private string m_name;
  private string m_baseLstStyle;
  private byte m_bFlags;
  private long m_listId = 1720085641;
  private string m_styleLink;

  internal long ListID
  {
    get => this.m_listId;
    set => this.m_listId = value;
  }

  internal string StyleLink
  {
    get => this.m_styleLink;
    set => this.m_styleLink = value;
  }

  public string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  public ListType ListType
  {
    get => this.m_listType;
    set => this.m_listType = value;
  }

  public ListLevelCollection Levels => this.m_levels;

  public StyleType StyleType => StyleType.OtherStyle;

  internal bool IsHybrid
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal bool IsSimple
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  internal bool IsBuiltInStyle
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
  }

  internal string BaseListStyleName
  {
    get => this.m_baseLstStyle;
    set => this.m_baseLstStyle = value;
  }

  public ListStyle(IWordDocument doc, ListType listType)
    : this((WordDocument) doc)
  {
    this.m_listType = listType;
    this.CreateDefListLevels(listType);
  }

  internal ListStyle(WordDocument doc, ListType listType, bool isOneLevelList)
    : this(doc)
  {
    this.m_listType = listType;
    this.CreateEmptyListLevels(isOneLevelList);
  }

  internal ListStyle(WordDocument doc)
    : base(doc, (Entity) doc)
  {
    this.m_levels = new ListLevelCollection(this);
    this.m_levels.SetOwner((OwnerHolder) this);
    this.SetNewListID(doc);
  }

  public static ListStyle CreateEmptyListStyle(
    IWordDocument doc,
    ListType listType,
    bool isOneLevelList)
  {
    return new ListStyle((WordDocument) doc, listType, isOneLevelList);
  }

  public IStyle Clone() => this.CloneImpl() as IStyle;

  void IStyle.Close() => this.Close();

  internal new void Close()
  {
    if (this.m_levels == null || this.m_levels.Count == 0)
    {
      this.m_levels = (ListLevelCollection) null;
    }
    else
    {
      int count = this.m_levels.Count;
      WListLevel wlistLevel = (WListLevel) null;
      for (int index = 0; index < count; ++index)
      {
        this.m_levels[index].Close();
        wlistLevel = (WListLevel) null;
      }
      this.m_levels.Close();
      this.m_levels = (ListLevelCollection) null;
    }
  }

  protected override object CloneImpl()
  {
    ListStyle owner = (ListStyle) base.CloneImpl();
    owner.m_levels = new ListLevelCollection(owner);
    this.m_levels.CloneToImpl((CollectionImpl) owner.m_levels);
    return (object) owner;
  }

  protected override void InitXDLSHolder()
  {
    base.InitXDLSHolder();
    this.XDLSHolder.AddElement("levels", (object) this.Levels);
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    writer.WriteValue("Name", this.Name);
    writer.WriteValue("ListType", (Enum) this.ListType);
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    this.m_name = reader.ReadString("Name");
    this.ListType = (ListType) reader.ReadEnum("ListType", typeof (ListType));
  }

  internal void CreateDefListLevels(ListType listType)
  {
    this.Levels.Clear();
    this.Document.CreateListLevelImpl(this);
    if (listType == ListType.Bulleted)
    {
      for (float num = 0.5f; (double) num < 4.5; num += 1.5f)
      {
        this.Levels.Add(WListLevel.CreateDefBulletLvl((float) (int) (72.0 * (double) num), "\uF0B7", this));
        this.Levels.Add(WListLevel.CreateDefBulletLvl((float) (int) (72.0 * ((double) num + 0.5)), "o", this));
        this.Levels.Add(WListLevel.CreateDefBulletLvl((float) (int) (72.0 * ((double) num + 1.0)), "\uF0A7", this));
      }
    }
    else
    {
      int num1 = 0;
      for (float num2 = 0.5f; (double) num2 < 4.5; num2 += 1.5f)
      {
        ListLevelCollection levels1 = this.Levels;
        int dxLeft1 = (int) (72.0 * (double) num2);
        int levelNumber1 = num1;
        int num3 = levelNumber1 + 1;
        WListLevel defNumberLvl1 = WListLevel.CreateDefNumberLvl(dxLeft1, levelNumber1, ListPatternType.Arabic, ListNumberAlignment.Left, this);
        levels1.Add(defNumberLvl1);
        ListLevelCollection levels2 = this.Levels;
        int dxLeft2 = (int) (72.0 * ((double) num2 + 0.5));
        int levelNumber2 = num3;
        int num4 = levelNumber2 + 1;
        WListLevel defNumberLvl2 = WListLevel.CreateDefNumberLvl(dxLeft2, levelNumber2, ListPatternType.LowLetter, ListNumberAlignment.Right, this);
        levels2.Add(defNumberLvl2);
        ListLevelCollection levels3 = this.Levels;
        int dxLeft3 = (int) (72.0 * ((double) num2 + 1.0));
        int levelNumber3 = num4;
        num1 = levelNumber3 + 1;
        WListLevel defNumberLvl3 = WListLevel.CreateDefNumberLvl(dxLeft3, levelNumber3, ListPatternType.LowRoman, ListNumberAlignment.Left, this);
        levels3.Add(defNumberLvl3);
      }
    }
  }

  public WListLevel GetNearLevel(int levelNumber)
  {
    if (levelNumber < 0)
      throw new ArgumentOutOfRangeException("number", "Value can not be less than 0");
    if (levelNumber > this.Levels.Count - 1)
      levelNumber = this.Levels.Count - 1;
    return this.Levels[levelNumber];
  }

  internal void CreateEmptyListLevels(bool isOneLevelList)
  {
    int num = isOneLevelList ? 1 : 9;
    for (int index = 0; index < num; ++index)
      this.Levels.Add(this.Document.CreateListLevelImpl(this));
  }

  internal override void CloneRelationsTo(WordDocument doc, OwnerHolder nextOwner)
  {
    if (doc == this.Document)
      return;
    this.SetOwner((OwnerHolder) doc);
    this.Levels.SetOwner((OwnerHolder) this);
    for (int index = 0; index < this.Levels.Count; ++index)
    {
      this.Levels[index].SetOwner((OwnerHolder) this);
      this.Levels[index].CharacterFormat.SetOwner((OwnerHolder) this.Levels[index]);
      this.Levels[index].ParagraphFormat.SetOwner((OwnerHolder) this.Levels[index]);
      if (this.Levels[index].PicBullet != null)
        this.Levels[index].PicBullet.CloneRelationsTo(this.Document, (OwnerHolder) this.Levels[index]);
    }
  }

  internal bool Compare(ListStyle listStyle)
  {
    return this.ListID == listStyle.ListID && this.IsHybrid == listStyle.IsHybrid && this.IsSimple == listStyle.IsSimple && this.ListType == listStyle.ListType && (this.Levels == null || listStyle.Levels == null || this.Levels.Compare(listStyle.Levels));
  }

  internal bool IsSameListNameOrIDExists(
    ListStyleCollection docListStyles,
    long listID,
    string styleName)
  {
    bool flag = string.IsNullOrEmpty(styleName);
    foreach (ListStyle docListStyle in (CollectionImpl) docListStyles)
    {
      if ((flag ? (listID == docListStyle.ListID ? 1 : 0) : (styleName == docListStyle.Name ? 1 : 0)) != 0)
        return true;
    }
    return false;
  }

  internal void SetNewName(WordDocument doc)
  {
    while (this.IsSameListNameOrIDExists(doc.ListStyles, 0L, this.Name))
      this.Name = this.ListType == ListType.Bulleted ? "Bulleted_" + Guid.NewGuid().ToString() : "Numbered_" + Guid.NewGuid().ToString();
  }

  internal void SetNewListID(WordDocument destDocument)
  {
    Random random = new Random();
    long listID = (long) random.Next();
    if (destDocument.m_listStyles != null)
    {
      while (this.IsSameListNameOrIDExists(destDocument.m_listStyles, listID, (string) null))
        listID = (long) random.Next();
    }
    this.ListID = listID;
  }

  void IStyle.Remove()
  {
  }
}
