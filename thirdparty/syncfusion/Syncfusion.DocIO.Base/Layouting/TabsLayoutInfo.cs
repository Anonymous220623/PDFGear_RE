// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.TabsLayoutInfo
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Layouting;

internal class TabsLayoutInfo(ChildrenLayoutDirection childLayoutDirection) : LayoutInfo(childLayoutDirection)
{
  protected double m_defaultTabWidth;
  protected double m_pageMarginLeft;
  protected double m_pageMarginRight;
  internal List<TabsLayoutInfo.LayoutTab> m_list = new List<TabsLayoutInfo.LayoutTab>();
  internal TabsLayoutInfo.LayoutTab m_currTab = new TabsLayoutInfo.LayoutTab();
  private float m_tabWidth;
  private bool m_isTabWidthUpdatedBasedOnIndent;

  public double DefaultTabWidth => this.m_defaultTabWidth;

  internal double PageMarginLeft
  {
    get => this.m_pageMarginLeft;
    set => this.m_pageMarginLeft = value;
  }

  internal double PageMarginRight
  {
    get => this.m_pageMarginRight;
    set => this.m_pageMarginRight = value;
  }

  internal bool IsTabWidthUpdatedBasedOnIndent
  {
    get => this.m_isTabWidthUpdatedBasedOnIndent;
    set => this.m_isTabWidthUpdatedBasedOnIndent = value;
  }

  internal float TabWidth
  {
    get => this.m_tabWidth;
    set => this.m_tabWidth = value;
  }

  public TabLeader CurrTabLeader => this.m_currTab.TabLeader;

  public TabJustification CurrTabJustification => this.m_currTab.Justification;

  internal List<TabsLayoutInfo.LayoutTab> LayoutTabList => this.m_list;

  public double GetNextTabPosition(double position)
  {
    double num1 = position;
    bool flag1 = false;
    if (this.m_list.Count > 0)
    {
      for (int index = this.m_list.Count - 1; index > -1; --index)
      {
        if (Math.Round((double) this.m_list[index].Position, 2) <= Math.Round(position, 2) && (index <= 0 || (double) this.m_list[index - 1].Position <= Math.Round(position, 2)))
        {
          if (index != this.m_list.Count - 1)
          {
            if (this.m_list[index + 1].Justification != TabJustification.Bar)
            {
              num1 = (double) this.m_list[index + 1].Position;
              this.m_currTab = this.m_list[index + 1];
            }
            else
              num1 = position;
          }
          flag1 = true;
          break;
        }
      }
      if (!flag1 && this.m_list[0].Justification != TabJustification.Bar)
      {
        num1 = (double) this.m_list[0].Position;
        this.m_currTab = this.m_list[0];
      }
    }
    bool flag2 = false;
    if (num1 == position)
    {
      this.m_currTab = new TabsLayoutInfo.LayoutTab();
      flag2 = true;
      if (this.DefaultTabWidth > 0.0)
      {
        float num2 = (float) Math.Round(position * 100.0 % (this.DefaultTabWidth * 100.0) / 100.0, 2);
        num1 = ((position - (double) num2) / this.DefaultTabWidth + 1.0) * this.DefaultTabWidth;
      }
    }
    if (Math.Round(num1, 1) == Math.Round(position, 1))
      return this.DefaultTabWidth;
    return Math.Round(num1 - position, 1) > Math.Round(this.DefaultTabWidth, 1) && flag2 ? (num1 - position) % this.DefaultTabWidth : num1 - position;
  }

  public void AddTab(float position, TabJustification justification, TabLeader leader)
  {
    this.m_list.Add(new TabsLayoutInfo.LayoutTab(position, justification, leader));
  }

  internal void SortParagraphTabsCollection(
    WParagraphFormat paragraphFormat,
    TabCollection listTabCollection,
    int tabLevelIndex)
  {
    Dictionary<int, TabCollection> tabCollection = new Dictionary<int, TabCollection>();
    int key = 0;
    int count = 0;
    bool flag = true;
    for (WParagraphFormat wparagraphFormat = paragraphFormat; wparagraphFormat != null; wparagraphFormat = wparagraphFormat.BaseFormat as WParagraphFormat)
    {
      if (wparagraphFormat.Tabs.Count > 0)
      {
        if (count < wparagraphFormat.Tabs.Count)
          count = wparagraphFormat.Tabs.Count;
        tabCollection.Add(key, wparagraphFormat.Tabs);
        ++key;
      }
      if (listTabCollection != null && flag && key == tabLevelIndex && listTabCollection.Count > 0)
      {
        if (count < listTabCollection.Count)
          count = listTabCollection.Count;
        tabCollection.Add(key, listTabCollection);
        flag = false;
        ++key;
      }
    }
    this.UpdateTabs(tabCollection, count);
    tabCollection.Clear();
  }

  private void UpdateTabs(Dictionary<int, TabCollection> tabCollection, int count)
  {
    int[] levelIndexes = new int[tabCollection.Count];
    int currLevelIndex = 0;
    Dictionary<float, int> delPosition = new Dictionary<float, int>();
    List<int> tabLevels = new List<int>();
    for (int index1 = 0; index1 < count; ++index1)
    {
      Tab tab1 = (Tab) null;
      int key1 = 0;
      for (int key2 = 0; key2 < tabCollection.Count; ++key2)
      {
        if (index1 < tabCollection[key2].Count && levelIndexes[key2] <= index1 && (tab1 == null || (double) tab1.Position < (double) tabCollection[key2][index1].Position))
        {
          tab1 = tabCollection[key2][index1];
          key1 = key2;
        }
      }
      bool flag1 = false;
      while (!flag1 && tab1 != null && levelIndexes[key1] <= index1)
      {
        bool flag2 = false;
        Tab tab2 = (Tab) null;
        List<Tab> tabList = new List<Tab>();
        List<int> intList = new List<int>();
        for (int key3 = 0; key3 < tabCollection.Count; ++key3)
        {
          TabCollection tab3 = tabCollection[key3];
          if (key3 != key1 && levelIndexes[key3] < tab3.Count && (double) tabCollection[key1][index1].Position > (double) tab3[levelIndexes[key3]].Position)
          {
            tabList.Add(tab3[levelIndexes[key3]]);
            intList.Add(key3);
            flag2 = true;
          }
        }
        if (tabList.Count > 0)
        {
          for (int index2 = 1; index2 < tabList.Count; ++index2)
          {
            if ((double) tabList[0].Position > (double) tabList[index2].Position)
            {
              tabList[0] = tabList[index2];
              intList[0] = intList[index2];
            }
          }
          tab2 = tabList[0];
          ++levelIndexes[intList[0]];
          currLevelIndex = intList[0];
        }
        if (!flag2)
        {
          tab2 = tabCollection[key1][index1];
          while (levelIndexes[key1] < index1 + 1)
            ++levelIndexes[key1];
          flag1 = true;
          currLevelIndex = key1;
        }
        if (tab2 != null)
          this.UpdateTabsCollection(tab2, currLevelIndex, levelIndexes, delPosition, tabLevels, tabCollection);
      }
    }
    this.ClearDeleteTabPositions(delPosition, tabLevels);
    delPosition.Clear();
    tabLevels.Clear();
  }

  private void ClearDeleteTabPositions(Dictionary<float, int> delPosition, List<int> tabLevels)
  {
    if (this.m_list.Count <= 0 || delPosition.Count <= 0)
      return;
    List<float> floatList = new List<float>((IEnumerable<float>) delPosition.Keys);
    for (int index = 0; index < this.m_list.Count; ++index)
    {
      if (floatList.Contains((float) Math.Truncate((double) this.m_list[index].Position)) && delPosition[(float) Math.Truncate((double) this.m_list[index].Position)] < tabLevels[index])
      {
        floatList.Remove((float) Math.Truncate((double) this.m_list[index].Position));
        this.m_list.RemoveAt(index);
        tabLevels.RemoveAt(index);
        --index;
        if (floatList.Count == 0)
          break;
      }
    }
  }

  private void UpdateTabsCollection(
    Tab tab,
    int currLevelIndex,
    int[] levelIndexes,
    Dictionary<float, int> delPosition,
    List<int> tabLevels,
    Dictionary<int, TabCollection> tabCollection)
  {
    bool flag = false;
    int index1 = 0;
    if (this.m_list.Count != 0)
    {
      for (int index2 = 0; index2 < this.m_list.Count; ++index2)
      {
        int index3 = levelIndexes[currLevelIndex] - 1;
        if (index3 < tabCollection[tabLevels[index2]].Count && currLevelIndex != tabLevels[index2] && Math.Truncate((double) tabCollection[tabLevels[index2]][index3].Position) == Math.Truncate((double) tabCollection[currLevelIndex][index3].Position))
        {
          flag = true;
          index1 = index2;
          break;
        }
      }
    }
    if (((double) tab.Position != 0.0 || (double) tab.DeletePosition == 0.0) && !flag)
    {
      this.AddTab((double) tab.Position != 0.0 ? tab.Position : tab.DeletePosition / 20f, (TabJustification) tab.Justification, (TabLeader) tab.TabLeader);
      tabLevels.Add(currLevelIndex);
    }
    else if ((double) tab.DeletePosition != 0.0 && !delPosition.ContainsKey((float) Math.Truncate((double) tab.DeletePosition / 20.0)))
      delPosition.Add((float) Math.Truncate((double) tab.DeletePosition / 20.0), currLevelIndex);
    if (!flag || tabLevels[index1] <= currLevelIndex)
      return;
    this.m_list[index1].Justification = (TabJustification) tab.Justification;
    this.m_list[index1].Position = (double) tab.Position != 0.0 ? tab.Position : tab.DeletePosition / 20f;
    this.m_list[index1].TabLeader = (TabLeader) tab.TabLeader;
  }

  internal class LayoutTab
  {
    private TabJustification m_jc;
    private TabLeader m_tlc;
    private float m_tabPosition;

    public TabJustification Justification
    {
      get => this.m_jc;
      set
      {
        if (value == this.m_jc)
          return;
        this.m_jc = value;
      }
    }

    public TabLeader TabLeader
    {
      get => this.m_tlc;
      set
      {
        if (value == this.m_tlc)
          return;
        this.m_tlc = value;
      }
    }

    public float Position
    {
      get => this.m_tabPosition;
      set
      {
        if ((double) value == (double) this.m_tabPosition)
          return;
        this.m_tabPosition = value;
      }
    }

    internal LayoutTab()
      : this(0.0f, TabJustification.Left, TabLeader.NoLeader)
    {
    }

    internal LayoutTab(float position, TabJustification justification, TabLeader leader)
    {
      this.m_tabPosition = position;
      this.m_jc = justification;
      this.m_tlc = leader;
    }
  }
}
