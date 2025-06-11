// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.TabCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class TabCollection : XDLSSerializableCollection
{
  private byte m_bFlags;

  public Tab this[int index] => (Tab) this.InnerList[index];

  internal bool CancelOnChangeEvent
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal TabCollection(WordDocument document)
    : base(document, (OwnerHolder) null)
  {
  }

  internal TabCollection(WordDocument document, FormatBase owner)
    : this(document)
  {
    this.SetOwner((OwnerHolder) owner);
  }

  public Tab AddTab() => this.AddTab(0.0f, TabJustification.Left, TabLeader.NoLeader);

  public Tab AddTab(float position, TabJustification justification, TabLeader leader)
  {
    Tab tab = new Tab((IWordDocument) this.Document, position, justification, leader);
    this.InnerList.Add((object) tab);
    tab.SetOwner((OwnerHolder) this);
    this.OnChange();
    return tab;
  }

  public Tab AddTab(float position)
  {
    return this.AddTab(position, TabJustification.Left, TabLeader.NoLeader);
  }

  public void Clear()
  {
    this.InnerList.Clear();
    this.OnChange();
  }

  public void RemoveAt(int index)
  {
    this.InnerList.RemoveAt(index);
    this.OnChange();
  }

  public void RemoveByTabPosition(double position)
  {
    int index = 0;
    while (index < this.Count)
    {
      if ((double) this[index].Position == position)
        this.InnerList.Remove((object) this[index]);
      else
        ++index;
    }
    this.OnChange();
  }

  internal void AddTab(Tab tab)
  {
    this.InnerList.Add((object) tab);
    tab.SetOwner((OwnerHolder) this);
    this.OnChange();
  }

  protected override OwnerHolder CreateItem(IXDLSContentReader reader)
  {
    return (OwnerHolder) new Tab((IWordDocument) this.Document);
  }

  protected override string GetTagItemName() => "Tab";

  internal void OnChange()
  {
    if (this.CancelOnChangeEvent || this.OwnerBase == null || !(this.OwnerBase is WParagraphFormat))
      return;
    (this.OwnerBase as WParagraphFormat).ChangeTabs(this);
  }

  internal bool Compare(TabCollection tabs)
  {
    if (this.Count != tabs.Count)
      return false;
    int index = 0;
    foreach (Tab tab1 in (CollectionImpl) this)
    {
      Tab tab2 = tabs[index];
      if (tab1 != null && tab2 != null)
      {
        if (!tab1.Compare(tab2))
          return false;
        ++index;
      }
      else if (tab1 != null && tab2 == null || tab1 == null && tab2 != null)
        return false;
    }
    return true;
  }

  internal void UpdateSourceFormatting(TabCollection tabs)
  {
    for (int index = 0; index < this.Count; ++index)
    {
      Tab tab = this[index];
      tabs.InnerList.Insert(index, (object) new Tab((IWordDocument) tabs.Document)
      {
        Position = 0.0f,
        DeletePosition = (tab.Position * 20f)
      });
    }
  }

  internal void UpdateTabs(TabCollection tabs)
  {
    if (tabs == null)
      tabs = new TabCollection(this.Document);
    for (int index = 0; index < this.Count; ++index)
      tabs.AddTab(this[index].Clone());
  }

  internal bool HasTabPosition(float tabPosition)
  {
    if (this.Count == 0)
      return false;
    for (int index = 0; index < this.Count; ++index)
    {
      if ((double) this[index].Position == (double) tabPosition)
        return true;
    }
    return false;
  }

  internal void SortTabs()
  {
    for (int index1 = 1; index1 < this.Count; ++index1)
    {
      for (int index2 = 0; index2 < this.Count - 1; ++index2)
      {
        Tab tab1 = this[index2];
        Tab tab2 = this[index2 + 1];
        if (((double) tab1.Position != 0.0 ? (double) tab1.Position : ((double) tab1.DeletePosition != 0.0 ? (double) tab1.DeletePosition : 0.0)) > ((double) tab2.Position != 0.0 ? (double) tab2.Position : ((double) tab2.DeletePosition != 0.0 ? (double) tab2.DeletePosition : 0.0)))
        {
          Tab tab3 = this[index2];
          this.InnerList[index2] = (object) this[index2 + 1];
          this.InnerList[index2 + 1] = (object) tab3;
        }
      }
    }
    this.OnChange();
  }
}
