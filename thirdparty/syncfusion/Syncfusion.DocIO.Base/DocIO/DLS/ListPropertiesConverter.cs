// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ListPropertiesConverter
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class ListPropertiesConverter
{
  [ThreadStatic]
  private static bool isFirstInvalidListID = false;
  [ThreadStatic]
  private static string m_defaultListStyleName;

  public static void Export(
    WListFormat listFormat,
    SinglePropertyModifierArray papxSprms,
    WordReaderBase reader)
  {
    if (papxSprms.Contain(9828))
      listFormat.IsFormattingChange = true;
    SinglePropertyModifierRecord papxSprm1 = papxSprms[17931];
    int formatIndex = papxSprm1 != null ? (int) papxSprm1.ShortValue : -1;
    SinglePropertyModifierRecord papxSprm2 = papxSprms[9738];
    int byteValue = papxSprm2 != null ? (int) papxSprm2.ByteValue : 0;
    ListPropertiesConverter.Export(formatIndex, byteValue, listFormat, reader);
  }

  public static void Export(
    int formatIndex,
    int levelIndex,
    WListFormat listFormat,
    WordReaderBase reader)
  {
    if (formatIndex == 0 && !listFormat.IsFormattingChange)
      listFormat.IsEmptyList = true;
    ListInfo listInfo = reader.ListInfo;
    if (formatIndex > 0 && listInfo != null && listInfo.ListFormatOverrides.Count > 0)
    {
      if (listInfo.ListFormatOverrides.Count < formatIndex)
      {
        if (!listFormat.IsFormattingChange)
          return;
        ListPropertiesConverter.UpdateNewListFormat(reader, listFormat);
      }
      else
      {
        int listId = listInfo.ListFormatOverrides[formatIndex - 1].ListID;
        ListData listFromId = listInfo.ListFormats.GetListFromId(listId);
        ListFormatOverride listFormatOverride = listInfo.ListFormatOverrides[formatIndex - 1];
        if (listFormatOverride.Levels.Count > 0)
          listFormat.LFOStyleName = ListPropertiesConverter.ExportListFormatOverrides(formatIndex, reader, listFormatOverride, listFormat);
        if (listFromId != null)
          ListPropertiesConverter.ExportListFormat(listFormat, reader, listId, listFromId, levelIndex);
        else
          ListPropertiesConverter.UpdateListStyleForInvalidListId(listFormat, listId, levelIndex);
        ListPropertiesConverter.UpdateNewListFormat(reader, listFormat);
        if (listFormatOverride.Levels.Count <= 0)
          return;
        ListOverrideStyle byName = listFormat.Document.ListOverrides.FindByName(listFormat.LFOStyleName);
        if (byName == null || listFormat.CurrentListStyle == null)
          return;
        byName.ListID = listFormat.CurrentListStyle.ListID;
        byName.listStyleName = listFormat.CurrentListStyle.Name;
      }
    }
    else
    {
      if (levelIndex <= 0 || levelIndex >= 9)
        return;
      listFormat.ListLevelNumber = levelIndex;
    }
  }

  private static void UpdateNewListFormat(WordReaderBase reader, WListFormat listFormat)
  {
    if (reader.PAPXSprms.GetBoolean(9828, false))
    {
      listFormat.IsFormattingChange = false;
      ListPropertiesConverter.ExportNewListFormat(listFormat, reader);
    }
    if (listFormat.OldPropertiesHash.Count <= 0 || listFormat.PropertiesHash.Count <= 0)
      return;
    foreach (KeyValuePair<int, object> keyValuePair in listFormat.OldPropertiesHash)
    {
      if (!listFormat.PropertiesHash.ContainsKey(keyValuePair.Key))
        listFormat.PropertiesHash.Add(keyValuePair.Key, keyValuePair.Value);
    }
  }

  private static void UpdateListStyleForInvalidListId(
    WListFormat listFormat,
    int id,
    int levelIndex)
  {
    if (!ListPropertiesConverter.isFirstInvalidListID)
    {
      ListStyle listStyle = new ListStyle(listFormat.Document);
      listStyle.CreateDefListLevels(ListType.Numbered);
      foreach (WListLevel level in (CollectionImpl) listStyle.Levels)
      {
        level.PatternType = ListPatternType.Arabic;
        level.NumberAlignment = ListNumberAlignment.Left;
        level.ParagraphFormat.SetPropertyValue(5, (object) -36f);
      }
      ListPropertiesConverter.UpdateListType(listStyle);
      ListPropertiesConverter.UpdateStyleName(listStyle);
      listFormat.Document.ListStyles.Add(listStyle);
      listFormat.ListLevelNumber = levelIndex;
      listFormat.ApplyStyle(listStyle.Name);
      ListPropertiesConverter.isFirstInvalidListID = true;
      ListPropertiesConverter.m_defaultListStyleName = listStyle.Name;
    }
    else
    {
      listFormat.Document.ListStyleNames.Add(id.ToString(), ListPropertiesConverter.m_defaultListStyleName);
      listFormat.ListLevelNumber = levelIndex;
      listFormat.ApplyStyle(listFormat.Document.ListStyles[id].Name);
      if (!AdapterListIDHolder.Instance.LfoStyleIDtoName.ContainsKey(id))
      {
        ListOverrideStyle listOverrideStyle = new ListOverrideStyle(listFormat.Document);
        listOverrideStyle.Name = "LfoStyle_" + (object) Guid.NewGuid();
        listOverrideStyle.listStyleName = listFormat.Document.ListStyleNames[id.ToString()];
        listFormat.Document.ListOverrides.Add(listOverrideStyle);
        AdapterListIDHolder.Instance.LfoStyleIDtoName.Add(id, listOverrideStyle.Name);
        for (int levelNumber = 0; levelNumber < 9; ++levelNumber)
        {
          OverrideLevelFormat lfoLevel = new OverrideLevelFormat(listFormat.Document);
          listOverrideStyle.OverrideLevels.Add(levelNumber, lfoLevel);
          lfoLevel.StartAt = 1;
          lfoLevel.OverrideStartAtValue = true;
        }
        listFormat.LFOStyleName = listOverrideStyle.listStyleName;
      }
      else
        listFormat.LFOStyleName = AdapterListIDHolder.Instance.LfoStyleIDtoName[id];
    }
  }

  private static void UpdateListType(ListStyle listStyle)
  {
    listStyle.ListType = ListType.Bulleted;
    foreach (WListLevel level in (CollectionImpl) listStyle.Levels)
    {
      if (level.PatternType != ListPatternType.Bullet)
      {
        listStyle.ListType = ListType.Numbered;
        break;
      }
    }
  }

  private static void UpdateStyleName(ListStyle listStyle)
  {
    if (listStyle.ListType == ListType.Numbered)
      listStyle.Name = "Numbered_" + Guid.NewGuid().ToString();
    else
      listStyle.Name = "Bulleted_" + Guid.NewGuid().ToString();
  }

  public static void Import(ListStyle lstStyle, ListData listFormat, WordStyleSheet styleSheet)
  {
    int num = 0;
    for (int count = lstStyle.Levels.Count; num < count; ++num)
    {
      WListLevel level = lstStyle.Levels[num];
      ListLevel docListLevel = new ListLevel();
      ListPropertiesConverter.ImportToDocListLevel(level, docListLevel, styleSheet, num);
      listFormat.Levels.Add((object) docListLevel);
    }
  }

  private static void ExportListFormat(
    WListFormat listFormat,
    WordReaderBase reader,
    int id,
    ListData listData,
    int levelIndex)
  {
    if (levelIndex > listData.Levels.Count)
      levelIndex = 0;
    if (levelIndex >= listData.Levels.Count)
      return;
    ListPropertiesConverter.CheckListCollection(id, listFormat);
    string styleName = ListPropertiesConverter.ExportListStyle(listFormat, reader, id, listData);
    listFormat.ListLevelNumber = levelIndex;
    listFormat.ApplyStyle(styleName);
  }

  private static void ExportNewListFormat(WListFormat listFormat, WordReaderBase reader)
  {
    SinglePropertyModifierRecord newSprm1 = reader.PAPX.PropertyModifiers.GetNewSprm(17931, 9828);
    int lfoIndex = newSprm1 == null ? (int) short.MaxValue : (int) newSprm1.ShortValue;
    if (lfoIndex != (int) short.MaxValue && lfoIndex > 0)
    {
      ListInfo listInfo = reader.ListInfo;
      int listId = listInfo.ListFormatOverrides[lfoIndex - 1].ListID;
      ListData listFromId = listInfo.ListFormats.GetListFromId(listId);
      ListFormatOverride listFormatOverride = listInfo.ListFormatOverrides[lfoIndex - 1];
      if (listFormatOverride.Levels.Count > 0)
        listFormat.LFOStyleName = ListPropertiesConverter.ExportListFormatOverrides(lfoIndex, reader, listFormatOverride, listFormat);
      string styleName = ListPropertiesConverter.ExportListStyle(listFormat, reader, listId, listFromId);
      listFormat.ApplyStyle(styleName);
    }
    SinglePropertyModifierRecord newSprm2 = reader.PAPX.PropertyModifiers.GetNewSprm(9738, 9828);
    int num = newSprm2 == null ? (int) byte.MaxValue : (int) newSprm2.ByteValue;
    if (num == (int) byte.MaxValue)
      return;
    listFormat.ListLevelNumber = num;
  }

  private static string ExportListStyle(
    WListFormat listFormat,
    WordReaderBase reader,
    int id,
    ListData listData)
  {
    if (AdapterListIDHolder.Instance.ListStyleIDtoName.ContainsKey(id))
      return AdapterListIDHolder.Instance.ListStyleIDtoName[id];
    bool flag = ListPropertiesConverter.IsBulletPatternType(listData.Levels);
    ListType listType = flag ? ListType.Bulleted : ListType.Numbered;
    ListStyle emptyListStyle = ListStyle.CreateEmptyListStyle((IWordDocument) listFormat.Document, listType, listData.SimpleList);
    ListPropertiesConverter.ExportToListLevelCollection(listData.Levels, emptyListStyle.Levels, reader);
    emptyListStyle.Name = flag ? "Bulleted_" + Guid.NewGuid().ToString() : "Numbered_" + Guid.NewGuid().ToString();
    AdapterListIDHolder.Instance.ListStyleIDtoName.Add(id, emptyListStyle.Name);
    emptyListStyle.ListID = (long) id;
    listFormat.Document.ListStyles.Add(emptyListStyle);
    emptyListStyle.IsHybrid = listData.IsHybridMultilevel;
    if (listData.SimpleList)
      emptyListStyle.IsSimple = true;
    return emptyListStyle.Name;
  }

  private static void ExportToListLevelCollection(
    ListLevels lstLevels,
    ListLevelCollection lstLevelCol,
    WordReaderBase reader)
  {
    int num = 0;
    for (int count = lstLevels.Count; num < count; ++num)
      ListPropertiesConverter.ExportToDLSListLevel(lstLevels[num], lstLevelCol[num], reader, num);
  }

  private static void ExportToDLSListLevel(
    ListLevel docListLevel,
    WListLevel dlsListLevel,
    WordReaderBase reader,
    int levelNumber)
  {
    dlsListLevel.PatternType = docListLevel.m_nfc;
    dlsListLevel.UsePrevLevelPattern = docListLevel.m_bPrev;
    dlsListLevel.StartAt = docListLevel.m_startAt;
    dlsListLevel.NumberAlignment = docListLevel.m_jc;
    dlsListLevel.FollowCharacter = docListLevel.m_ixchFollow;
    dlsListLevel.IsLegalStyleNumbering = docListLevel.m_bLegal;
    dlsListLevel.NoRestartByHigher = docListLevel.m_bNoRestart;
    dlsListLevel.Word6Legacy = docListLevel.m_bWord6;
    dlsListLevel.LegacySpace = docListLevel.m_dxaSpace;
    dlsListLevel.LegacyIndent = docListLevel.m_dxaIndent;
    char[] chArray = new char[2]
    {
      '\\',
      Convert.ToChar(levelNumber)
    };
    string[] strArray = docListLevel.m_str.Split(chArray);
    if (dlsListLevel.PatternType == ListPatternType.Bullet)
    {
      if (strArray.Length > 1)
      {
        dlsListLevel.BulletCharacter = strArray[0];
        dlsListLevel.BulletCharacter += strArray[1];
      }
      else
        dlsListLevel.BulletCharacter = docListLevel.m_str;
    }
    else
    {
      if (strArray.Length > 1)
      {
        dlsListLevel.NumberPrefix = strArray[0];
        dlsListLevel.NumberSuffix = strArray[1];
      }
      else if (strArray[0] == string.Empty)
      {
        dlsListLevel.NumberPrefix = dlsListLevel.NumberSuffix = (string) null;
      }
      else
      {
        dlsListLevel.NumberPrefix = strArray[0];
        dlsListLevel.NoLevelText = true;
      }
      if (dlsListLevel.PatternType == ListPatternType.None)
        dlsListLevel.BulletCharacter = docListLevel.m_str;
    }
    CharacterPropertiesConverter.SprmsToFormat(docListLevel.m_chpx.PropertyModifiers, dlsListLevel.CharacterFormat, reader.StyleSheet, reader.SttbfRMarkAuthorNames, true);
    ParagraphPropertiesConverter.SprmsToFormat(docListLevel.m_papx.PropertyModifiers, dlsListLevel.ParagraphFormat, reader.SttbfRMarkAuthorNames, reader.StyleSheet);
  }

  internal static void ImportToDocListLevel(
    WListLevel dlsListLevel,
    ListLevel docListLevel,
    WordStyleSheet styleSheet,
    int levelIndex)
  {
    docListLevel.m_nfc = dlsListLevel.PatternType;
    docListLevel.m_bPrev = dlsListLevel.UsePrevLevelPattern;
    docListLevel.m_startAt = dlsListLevel.StartAt;
    docListLevel.m_jc = dlsListLevel.NumberAlignment;
    docListLevel.m_ixchFollow = dlsListLevel.FollowCharacter;
    docListLevel.m_bLegal = dlsListLevel.IsLegalStyleNumbering;
    docListLevel.m_bNoRestart = dlsListLevel.NoRestartByHigher;
    docListLevel.m_bWord6 = dlsListLevel.Word6Legacy;
    docListLevel.m_dxaSpace = dlsListLevel.LegacySpace;
    docListLevel.m_dxaIndent = dlsListLevel.LegacyIndent;
    bool flag = false;
    if (dlsListLevel.PatternType == ListPatternType.None && dlsListLevel.BulletCharacter != null && dlsListLevel.BulletCharacter.Length > 0 && dlsListLevel.ParaStyleName == null)
      flag = true;
    if (dlsListLevel.PatternType != ListPatternType.Bullet && !flag)
    {
      char ch = Convert.ToChar(levelIndex);
      if (dlsListLevel.NumberPrefix == null && dlsListLevel.NumberSuffix == null)
      {
        docListLevel.m_str = string.Empty;
      }
      else
      {
        docListLevel.m_str = dlsListLevel.NumberPrefix;
        if (!dlsListLevel.NoLevelText)
        {
          ListLevel listLevel = docListLevel;
          listLevel.m_str = listLevel.m_str + ch.ToString() + dlsListLevel.NumberSuffix;
        }
      }
      ListPropertiesConverter.CreateCharacterOffsets(docListLevel.m_str, ref docListLevel.m_rgbxchNums);
    }
    else
      docListLevel.m_str = string.IsNullOrEmpty(dlsListLevel.BulletCharacter) ? string.Empty : dlsListLevel.BulletCharacter;
    if (docListLevel.m_chpx == null)
      docListLevel.m_chpx = new CharacterPropertyException();
    CharacterPropertiesConverter.FormatToSprms(dlsListLevel.CharacterFormat, docListLevel.m_chpx.PropertyModifiers, styleSheet);
    if (docListLevel.m_papx == null)
      docListLevel.m_papx = new ParagraphPropertyException();
    ParagraphPropertiesConverter.FormatToSprms(dlsListLevel.ParagraphFormat, docListLevel.m_papx.PropertyModifiers, styleSheet);
  }

  private static void CreateCharacterOffsets(string numStr, ref byte[] characterOffsets)
  {
    if (numStr == string.Empty)
      return;
    characterOffsets[0] = (byte) 1;
    int index1 = 0;
    for (int index2 = 0; index2 < 9; ++index2)
    {
      char[] chArray = new char[2]
      {
        '\\',
        Convert.ToChar(index2)
      };
      string[] strArray = numStr.Split(chArray);
      if (strArray.Length > 1)
      {
        byte num = (byte) (strArray[0].Length + 1);
        if (index1 <= 1 || (int) characterOffsets[index1 - 1] <= (int) num)
        {
          characterOffsets[index1] = num;
        }
        else
        {
          characterOffsets[index1] = characterOffsets[index1 - 1];
          characterOffsets[index1 - 1] = num;
        }
        ++index1;
      }
    }
  }

  private static bool IsBulletPatternType(ListLevels levels)
  {
    bool flag = true;
    int index = 0;
    for (int count = levels.Count; index < count; ++index)
    {
      if (levels[index].m_nfc != ListPatternType.Bullet)
      {
        flag = false;
        break;
      }
    }
    return flag;
  }

  private static void CheckListCollection(int listId, WListFormat listFormat)
  {
    if (!AdapterListIDHolder.Instance.ListStyleIDtoName.ContainsKey(listId))
      return;
    string name = AdapterListIDHolder.Instance.ListStyleIDtoName[listId];
    if (listFormat.Document.ListStyles.FindByName(name) != null)
      return;
    AdapterListIDHolder.Instance.ListStyleIDtoName.Remove(listId);
  }

  private static bool UseBaseListStyle(IWordReaderBase reader)
  {
    bool flag = false;
    if (reader.PAPX.PropertyModifiers[17931] == null)
      flag = true;
    return flag;
  }

  private static string ExportListFormatOverrides(
    int lfoIndex,
    WordReaderBase reader,
    ListFormatOverride lstOverride,
    WListFormat listFormat)
  {
    if (AdapterListIDHolder.Instance.LfoStyleIDtoName.ContainsKey(lfoIndex - 1))
      return AdapterListIDHolder.Instance.LfoStyleIDtoName[lfoIndex - 1];
    ListOverrideStyle listOverrideStyle = new ListOverrideStyle(listFormat.Document);
    ListPropertiesConverter.ExportListOverride(lstOverride, listOverrideStyle, reader, (IWordDocument) listFormat.Document);
    listOverrideStyle.Name = "LfoStyle_" + Guid.NewGuid().ToString();
    listFormat.Document.ListOverrides.Add(listOverrideStyle);
    AdapterListIDHolder.Instance.LfoStyleIDtoName.Add(lfoIndex - 1, listOverrideStyle.Name);
    return listOverrideStyle.Name;
  }

  internal static void ExportListOverride(
    ListFormatOverride sourceLfo,
    ListOverrideStyle listOverrideStyle,
    WordReaderBase reader,
    IWordDocument doc)
  {
    listOverrideStyle.m_res1 = sourceLfo.m_res1;
    listOverrideStyle.m_res2 = sourceLfo.m_res2;
    for (int index = 0; index < sourceLfo.Levels.Count; ++index)
    {
      ListFormatOverrideLevel level = sourceLfo.Levels[index] as ListFormatOverrideLevel;
      OverrideLevelFormat lfoLevel = new OverrideLevelFormat((WordDocument) doc);
      lfoLevel.OverrideFormatting = level.m_bFormatting;
      lfoLevel.OverrideStartAtValue = level.m_bStartAt;
      lfoLevel.StartAt = level.m_startAt;
      lfoLevel.m_reserved1 = level.m_reserved1;
      lfoLevel.m_reserved2 = level.m_reserved2;
      lfoLevel.m_reserved3 = level.m_reserved3;
      if (level.m_lvl != null && level.m_bFormatting)
        ListPropertiesConverter.ExportToDLSListLevel(level.m_lvl, lfoLevel.OverrideListLevel, reader, level.m_ilvl);
      listOverrideStyle.OverrideLevels.Add(level.m_ilvl, lfoLevel);
    }
  }

  internal static void ImportListOverride(
    ListOverrideStyle listOverrideStyle,
    ListFormatOverride lfo,
    WordStyleSheet styleSheet)
  {
    lfo.m_res1 = listOverrideStyle.m_res1;
    lfo.m_res2 = listOverrideStyle.m_res2;
    foreach (KeyValuePair<short, short> keyValuePair in listOverrideStyle.OverrideLevels.LevelIndex)
    {
      OverrideLevelFormat overrideLevel = listOverrideStyle.OverrideLevels[(int) keyValuePair.Key];
      ListFormatOverrideLevel formatOverrideLevel = new ListFormatOverrideLevel(overrideLevel.OverrideFormatting);
      formatOverrideLevel.m_ilvl = (int) keyValuePair.Key;
      formatOverrideLevel.m_bFormatting = overrideLevel.OverrideFormatting;
      formatOverrideLevel.m_bStartAt = overrideLevel.OverrideStartAtValue;
      formatOverrideLevel.m_startAt = overrideLevel.StartAt;
      formatOverrideLevel.m_reserved1 = overrideLevel.m_reserved1;
      formatOverrideLevel.m_reserved2 = overrideLevel.m_reserved2;
      formatOverrideLevel.m_reserved3 = overrideLevel.m_reserved3;
      if (overrideLevel.OverrideListLevel != null && overrideLevel.OverrideFormatting)
        ListPropertiesConverter.ImportToDocListLevel(overrideLevel.OverrideListLevel, formatOverrideLevel.m_lvl, styleSheet, (int) keyValuePair.Key);
      lfo.Levels.Add((object) formatOverrideLevel);
    }
  }
}
