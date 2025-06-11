// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ListOverrideLevelCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class ListOverrideLevelCollection : XDLSSerializableCollection
{
  private Dictionary<short, short> m_levelIndex;

  public OverrideLevelFormat this[int levelNumber]
  {
    get => (OverrideLevelFormat) this.InnerList[(int) this.LevelIndex[(short) levelNumber]];
  }

  private ListOverrideStyle OwnerStyle => this.OwnerBase as ListOverrideStyle;

  internal Dictionary<short, short> LevelIndex
  {
    get
    {
      if (this.m_levelIndex == null)
        this.m_levelIndex = new Dictionary<short, short>();
      return this.m_levelIndex;
    }
    set => this.m_levelIndex = value;
  }

  internal ListOverrideLevelCollection(WordDocument doc)
    : base(doc, (OwnerHolder) doc)
  {
  }

  internal int Add(int levelNumber, OverrideLevelFormat lfoLevel)
  {
    lfoLevel.SetOwner((OwnerHolder) this.OwnerStyle);
    short num = (short) this.InnerList.Add((object) lfoLevel);
    short key = (short) levelNumber;
    if (this.LevelIndex.ContainsKey(key))
      this.LevelIndex[key] = num;
    else
      this.LevelIndex.Add(key, num);
    return (int) num;
  }

  internal int GetLevelNumber(OverrideLevelFormat levelFormat)
  {
    int num = this.InnerList.IndexOf((object) levelFormat);
    int levelNumber = num;
    foreach (KeyValuePair<short, short> keyValuePair in this.LevelIndex)
    {
      if ((int) keyValuePair.Value == num)
      {
        levelNumber = (int) keyValuePair.Key;
        break;
      }
    }
    return levelNumber;
  }

  internal bool HasOverrideLevel(int levelNumber)
  {
    return this.LevelIndex.Count > 0 && this.LevelIndex.ContainsKey((short) levelNumber);
  }

  internal override void CloneToImpl(CollectionImpl collection)
  {
    base.CloneToImpl(collection);
    foreach (KeyValuePair<short, short> keyValuePair in this.LevelIndex)
      (collection as ListOverrideLevelCollection).LevelIndex.Add(keyValuePair.Key, keyValuePair.Value);
  }

  protected override OwnerHolder CreateItem(IXDLSContentReader reader)
  {
    return (OwnerHolder) new OverrideLevelFormat(this.Document);
  }

  protected override string GetTagItemName() => "override-level";

  internal override void Close()
  {
    base.Close();
    if (this.m_levelIndex == null)
      return;
    this.m_levelIndex.Clear();
    this.m_levelIndex = (Dictionary<short, short>) null;
  }

  internal bool Compare(ListOverrideLevelCollection listOverrideLevels)
  {
    if (this.LevelIndex.Count != listOverrideLevels.LevelIndex.Count)
      return false;
    foreach (KeyValuePair<short, short> keyValuePair in this.LevelIndex)
    {
      if (!listOverrideLevels.LevelIndex.ContainsKey(keyValuePair.Key) || !listOverrideLevels[(int) keyValuePair.Key].Compare(this[(int) keyValuePair.Key]))
        return false;
    }
    return true;
  }
}
