// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.ListProperties
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

internal class ListProperties
{
  private ListInfo m_listInfo;
  private ParagraphPropertyException m_papx;
  private Dictionary<string, short> m_overrideStyles = new Dictionary<string, short>();
  private Dictionary<string, short> m_styles = new Dictionary<string, short>();

  internal ListProperties(ListInfo listInfo, ParagraphPropertyException papx)
  {
    this.m_listInfo = listInfo;
    this.m_papx = papx;
  }

  internal Dictionary<string, short> StyleListIndexes => this.m_styles;

  internal void Close()
  {
    this.m_listInfo = (ListInfo) null;
    this.m_papx.Close();
    if (this.m_overrideStyles != null)
    {
      this.m_overrideStyles.Clear();
      this.m_overrideStyles = (Dictionary<string, short>) null;
    }
    if (this.m_styles == null)
      return;
    this.m_styles.Clear();
    this.m_styles = (Dictionary<string, short>) null;
  }

  internal void UpdatePAPX(ParagraphPropertyException papx) => this.m_papx = papx;

  internal void ContinueCurrentList(
    ListData listData,
    WListFormat listFormat,
    WordStyleSheet styleSheet)
  {
    string lfoStyleName = listFormat.LFOStyleName;
    if (lfoStyleName != null)
    {
      string key = lfoStyleName + listFormat.CustomStyleName;
      if (this.m_overrideStyles.ContainsKey(key))
      {
        this.m_papx.PropertyModifiers.SetShortValue(17931, this.m_overrideStyles[key]);
      }
      else
      {
        this.m_papx.PropertyModifiers.SetShortValue(17931, this.m_listInfo.ApplyLFO(listData, listFormat, styleSheet));
        this.m_overrideStyles.Add(key, this.m_papx.PropertyModifiers.GetShort(17931, (short) -1));
      }
    }
    else if (this.m_styles.ContainsKey(listFormat.CustomStyleName))
      this.m_papx.PropertyModifiers.SetShortValue(17931, this.m_styles[listFormat.CustomStyleName]);
    this.m_papx.PropertyModifiers.SetByteValue(9738, (byte) listFormat.ListLevelNumber);
  }

  internal int ApplyList(
    ListData listData,
    WListFormat listFormat,
    WordStyleSheet styleSheet,
    bool applyToPap)
  {
    short num = this.m_listInfo.ApplyList(listData, listFormat, styleSheet);
    if (applyToPap)
    {
      this.m_papx.PropertyModifiers.SetShortValue(17931, num);
      this.m_papx.PropertyModifiers.SetByteValue(9738, (byte) listFormat.ListLevelNumber);
    }
    if (listFormat.LFOStyleName != null)
    {
      string key = listFormat.LFOStyleName + listFormat.CustomStyleName;
      if (!this.m_overrideStyles.ContainsKey(key))
        this.m_overrideStyles.Add(key, num);
    }
    if (!this.m_styles.ContainsKey(listFormat.CustomStyleName))
      this.m_styles.Add(listFormat.CustomStyleName, num);
    else
      this.m_styles[listFormat.CustomStyleName] = num;
    return (int) num;
  }

  internal int ApplyBaseStyleList(
    ListData listData,
    WListFormat listFormat,
    WordStyleSheet styleSheet)
  {
    short num = this.m_listInfo.ApplyList(listData, listFormat, styleSheet);
    this.m_styles.Add(listFormat.CustomStyleName, num);
    return (int) num;
  }
}
