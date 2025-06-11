// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.ScriptLayouter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class ScriptLayouter
{
  private Dictionary<ScriptTags, string[]> m_opentTypeScriptTags = new Dictionary<ScriptTags, string[]>();
  private int[] arabicIsolatedCode = new int[7]
  {
    1575,
    1577,
    1583,
    1584,
    1585,
    1586,
    1608
  };

  internal ScriptLayouter()
  {
    this.m_opentTypeScriptTags.Add(ScriptTags.Arabic, new string[1]
    {
      "arab"
    });
    this.m_opentTypeScriptTags.Add(ScriptTags.Bengali, new string[2]
    {
      "bng2",
      "beng"
    });
    this.m_opentTypeScriptTags.Add(ScriptTags.Devanagari, new string[2]
    {
      "dev2",
      "deva"
    });
    this.m_opentTypeScriptTags.Add(ScriptTags.Gurmukhi, new string[2]
    {
      "gur2",
      "guru"
    });
    this.m_opentTypeScriptTags.Add(ScriptTags.Gujarati, new string[2]
    {
      "gjr2",
      "gujr"
    });
    this.m_opentTypeScriptTags.Add(ScriptTags.Kannada, new string[2]
    {
      "knd2",
      "knda"
    });
    this.m_opentTypeScriptTags.Add(ScriptTags.Khmer, new string[1]
    {
      "khmr"
    });
    this.m_opentTypeScriptTags.Add(ScriptTags.Latin, new string[1]
    {
      "latn"
    });
    this.m_opentTypeScriptTags.Add(ScriptTags.Malayalam, new string[2]
    {
      "mlm2",
      "mlym"
    });
    this.m_opentTypeScriptTags.Add(ScriptTags.Oriya, new string[2]
    {
      "ory2",
      "orya"
    });
    this.m_opentTypeScriptTags.Add(ScriptTags.Tamil, new string[2]
    {
      "tml2",
      "taml"
    });
    this.m_opentTypeScriptTags.Add(ScriptTags.Telugu, new string[2]
    {
      "tel2",
      "telu"
    });
    this.m_opentTypeScriptTags.Add(ScriptTags.Thai, new string[1]
    {
      "thai"
    });
  }

  private IList<IndicGlyphInfoList> GetIndicCharacterGroup(OtfGlyphInfoList glyphList)
  {
    IndicCharacterClassifier characterClassifier = new IndicCharacterClassifier();
    string clustersPattern = characterClassifier.GetClustersPattern(glyphList);
    Match match = new Regex(characterClassifier.Pattern).Match('X'.ToString() + clustersPattern);
    IList<IndicGlyphInfoList> indicCharacterGroup = (IList<IndicGlyphInfoList>) new List<IndicGlyphInfoList>();
    for (; match.Success; match = match.NextMatch())
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = glyphList.Start + match.Index - 1; index < glyphList.Start + (match.Index + match.Length) - 1 && stringBuilder != null; ++index)
      {
        OtfGlyphInfo glyph = glyphList.Glyphs[index];
        if (glyph.CharCode > -1)
          stringBuilder.Append(char.ConvertFromUtf32(glyph.CharCode));
        else
          stringBuilder = (StringBuilder) null;
      }
      indicCharacterGroup.Add(new IndicGlyphInfoList(glyphList, glyphList.Start + match.Index - 1, glyphList.Start + (match.Index + match.Length) - 1, stringBuilder?.ToString()));
    }
    return indicCharacterGroup;
  }

  internal bool DoLayout(
    UnicodeTrueTypeFont font,
    OtfGlyphInfoList glyphInfoList,
    ScriptTags script)
  {
    GSUBTable gsub = font.TtfReader.GSUB;
    string[] strArray = (string[]) null;
    this.m_opentTypeScriptTags.TryGetValue(script, out strArray);
    if (strArray == null || gsub == null)
      return false;
    string otfScriptTag = (string) null;
    LanguageRecord languageRecord = (LanguageRecord) null;
    for (int index = 0; index < strArray.Length; ++index)
    {
      languageRecord = gsub.LanguageRecord(strArray[index]);
      if (languageRecord != null)
      {
        otfScriptTag = strArray[index];
        break;
      }
    }
    if (languageRecord == null)
      return false;
    bool flag = false;
    switch (script)
    {
      case ScriptTags.Arabic:
        return this.DoArabicScriptShape(font, glyphInfoList, gsub, otfScriptTag, languageRecord);
      case ScriptTags.Bengali:
      case ScriptTags.Devanagari:
      case ScriptTags.Gujarati:
      case ScriptTags.Gurmukhi:
      case ScriptTags.Khmer:
      case ScriptTags.Kannada:
      case ScriptTags.Malayalam:
      case ScriptTags.Oriya:
      case ScriptTags.Tamil:
      case ScriptTags.Telugu:
        return this.DoIndicScriptShape(font, glyphInfoList, script, gsub, otfScriptTag, languageRecord);
      case ScriptTags.Thai:
        return this.DoThaiScriptShape(font, glyphInfoList, gsub, otfScriptTag, languageRecord);
      default:
        return flag;
    }
  }

  private bool DoThaiScriptShape(
    UnicodeTrueTypeFont font,
    OtfGlyphInfoList glyphList,
    GSUBTable gsubTable,
    string otfScriptTag,
    LanguageRecord languageRecord)
  {
    glyphList.IsThaiShaping = true;
    IDictionary<string, IList<LookupTable>> lookupTables = this.GetLookupTables(font, gsubTable, otfScriptTag, languageRecord);
    bool flag = this.ReplaceGlyphs(this.GetLookupTable(lookupTables, "ccmp"), glyphList);
    if (this.ReplaceGlyphs(this.GetLookupTable(lookupTables, "mark"), glyphList))
      flag = true;
    if (this.ReplaceGlyphs(this.GetLookupTable(lookupTables, "mkmk"), glyphList))
      flag = true;
    glyphList.IsThaiShaping = false;
    return flag;
  }

  private bool DoArabicScriptShape(
    UnicodeTrueTypeFont font,
    OtfGlyphInfoList glyphList,
    GSUBTable gsubTable,
    string otfScriptTag,
    LanguageRecord languageRecord)
  {
    bool flag = false;
    IDictionary<string, IList<LookupTable>> lookupTables = this.GetLookupTables(font, gsubTable, otfScriptTag, languageRecord);
    IList<LookupTable> lookupTable1 = this.GetLookupTable(lookupTables, "init");
    IList<LookupTable> lookupTable2 = this.GetLookupTable(lookupTables, "medi");
    IList<LookupTable> lookupTable3 = this.GetLookupTable(lookupTables, "fina");
    IList<LookupTable> lookupTable4 = this.GetLookupTable(lookupTables, "rlig");
    IList<int> words = this.BreakArabicLineIntoWords(glyphList, lookupTable1, lookupTable2, lookupTable3, font.TtfReader.GDEF);
    if (this.DoInitMediFinaShaping(glyphList, lookupTable1, lookupTable2, lookupTable3, font.TtfReader.GDEF, words))
      flag = true;
    int start = glyphList.Start;
    int end = glyphList.End;
    int num1 = 0;
    for (int index = 0; index < words.Count; index += 2)
    {
      int num2 = words[index] + num1;
      int num3 = words[index + 1] + num1;
      glyphList.Start = num2;
      glyphList.End = num3;
      if (this.DoLigatureFeature(glyphList, lookupTable4))
        flag = true;
      num1 += glyphList.End - num3;
      words[index] = glyphList.Start;
      words[index + 1] = glyphList.End;
    }
    int num4 = end + num1;
    string[] strArray = new string[3]
    {
      "curs",
      "mark",
      "mkmk"
    };
    for (int index = 0; index < words.Count; index += 2)
    {
      glyphList.Start = words[index];
      glyphList.End = words[index + 1];
      foreach (string tag in strArray)
      {
        if (this.ReplaceGlyphs(this.GetLookupTable(lookupTables, tag), glyphList))
          flag = true;
      }
    }
    glyphList.Start = start;
    glyphList.End = num4;
    return flag;
  }

  private IList<LookupTable> GetLookupTable(
    IDictionary<string, IList<LookupTable>> lookupTables,
    string tag)
  {
    IList<LookupTable> lookupTable;
    lookupTables.TryGetValue(tag, out lookupTable);
    return lookupTable;
  }

  private ScriptFeature[] GetScriptFeatures(int type)
  {
    List<ScriptFeature> scriptFeatureList = new List<ScriptFeature>();
    switch (type)
    {
      case 1:
        scriptFeatureList.Add(new ScriptFeature("nukt", true, 1));
        scriptFeatureList.Add(new ScriptFeature("akhn", true, 2));
        scriptFeatureList.Add(new ScriptFeature("rphf", false, 4));
        scriptFeatureList.Add(new ScriptFeature("rkrf", true, 8));
        scriptFeatureList.Add(new ScriptFeature("pref", false, 16 /*0x10*/));
        scriptFeatureList.Add(new ScriptFeature("blwf", false, 32 /*0x20*/));
        scriptFeatureList.Add(new ScriptFeature("abvf", false, 64 /*0x40*/));
        scriptFeatureList.Add(new ScriptFeature("half", false, 128 /*0x80*/));
        scriptFeatureList.Add(new ScriptFeature("pstf", false, 256 /*0x0100*/));
        scriptFeatureList.Add(new ScriptFeature("vatu", true, 512 /*0x0200*/));
        scriptFeatureList.Add(new ScriptFeature("cjct", true, 1024 /*0x0400*/));
        scriptFeatureList.Add(new ScriptFeature("cfar", false, 2048 /*0x0800*/));
        break;
      case 2:
        scriptFeatureList.Add(new ScriptFeature("init", false, 4096 /*0x1000*/));
        scriptFeatureList.Add(new ScriptFeature("pres", true, 8192 /*0x2000*/));
        scriptFeatureList.Add(new ScriptFeature("abvs", true, 16384 /*0x4000*/));
        scriptFeatureList.Add(new ScriptFeature("blws", true, 32768 /*0x8000*/));
        scriptFeatureList.Add(new ScriptFeature("psts", true, 65536 /*0x010000*/));
        scriptFeatureList.Add(new ScriptFeature("haln", true, 131072 /*0x020000*/));
        break;
      case 3:
        scriptFeatureList.Add(new ScriptFeature("dist", true, 262144 /*0x040000*/));
        scriptFeatureList.Add(new ScriptFeature("abvm", true, 524288 /*0x080000*/));
        scriptFeatureList.Add(new ScriptFeature("blwm", true, 1048576 /*0x100000*/));
        break;
    }
    return scriptFeatureList.ToArray();
  }

  private bool DoIndicScriptShape(
    UnicodeTrueTypeFont font,
    OtfGlyphInfoList glyphList,
    ScriptTags script,
    GSUBTable gsubTable,
    string otfScriptTag,
    LanguageRecord gsubLanguageRecord)
  {
    bool flag = false;
    this.NormalizeGlyphList(font, glyphList);
    IndicScriptLayouter indicScriptLayouter = new IndicScriptLayouter();
    indicScriptLayouter.ReplaceDefaultGlyphs(font, glyphList);
    IList<IndicGlyphInfoList> indicCharacterGroup = this.GetIndicCharacterGroup(glyphList);
    IDictionary<string, IList<LookupTable>> lookupTables = this.GetLookupTables(font, gsubTable, otfScriptTag, gsubLanguageRecord);
    if (indicCharacterGroup != null && indicCharacterGroup.Count > 0)
    {
      ScriptFeature[] scriptFeatures1 = this.GetScriptFeatures(1);
      ScriptFeature[] scriptFeatures2 = this.GetScriptFeatures(2);
      ScriptFeature[] scriptFeatures3 = this.GetScriptFeatures(3);
      IndicScript indicScript = this.GetIndicScript(script);
      bool oldScript = indicScript.OldVersion && !otfScriptTag.EndsWith("2");
      IList<LookupTable> lookupTable1 = this.GetLookupTable(lookupTables, "locl");
      IList<LookupTable> lookupTable2 = this.GetLookupTable(lookupTables, "ccmp");
      IList<LookupTable> lookupTable3 = this.GetLookupTable(lookupTables, "blwf");
      IList<LookupTable> lookupTable4 = this.GetLookupTable(lookupTables, "pstf");
      IList<LookupTable> lookupTable5 = this.GetLookupTable(lookupTables, "pref");
      IList<LookupTable> lookupTable6 = this.GetLookupTable(lookupTables, "rphf");
      for (int index = 0; index < indicCharacterGroup.Count; ++index)
      {
        IndicGlyphInfoList indicGlyphInfoList1 = indicCharacterGroup[index];
        this.ReplaceGlyphs(lookupTable1, (OtfGlyphInfoList) indicGlyphInfoList1);
        this.ReplaceGlyphs(lookupTable2, (OtfGlyphInfoList) indicGlyphInfoList1);
        indicScriptLayouter.SetPosition(font, indicGlyphInfoList1, indicScript, lookupTable3, lookupTable4, lookupTable5);
        indicScriptLayouter.Reorder(indicGlyphInfoList1, indicScript, lookupTable6, lookupTable5, oldScript, script);
        this.ApplyScriptFeatures(scriptFeatures1, indicGlyphInfoList1, lookupTables);
        indicScriptLayouter.Reorder(indicGlyphInfoList1, indicScript, script);
        IndicGlyphInfoList indicGlyphInfoList2 = this.ApplyScriptFeatures(indicGlyphInfoList1, scriptFeatures2, lookupTables);
        indicCharacterGroup[index] = indicGlyphInfoList2;
        this.ReplaceGlyphs(this.GetLookupTable(lookupTables, "calt"), (OtfGlyphInfoList) indicGlyphInfoList2);
        this.ReplaceGlyphs(this.GetLookupTable(lookupTables, "clig"), (OtfGlyphInfoList) indicGlyphInfoList2);
        this.ApplyScriptFeatures(scriptFeatures3, indicGlyphInfoList2, lookupTables);
      }
      OtfGlyphInfoList glyphList1 = new OtfGlyphInfoList();
      for (int index = 0; index < glyphList.Start; ++index)
      {
        glyphList1.Glyphs.Add(glyphList.Glyphs[index]);
        glyphList1.Text.Add((string) null);
      }
      int num = glyphList.Start;
      foreach (IndicGlyphInfoList indicGlyphInfoList in (IEnumerable<IndicGlyphInfoList>) indicCharacterGroup)
      {
        if (indicGlyphInfoList.GlyphInfoStart > num)
        {
          for (int index = num; index < indicGlyphInfoList.GlyphInfoStart; ++index)
          {
            glyphList1.Glyphs.Add(glyphList.Glyphs[index]);
            glyphList1.Text.Add((string) null);
          }
        }
        int count = glyphList1.Glyphs.Count;
        for (int index = 0; index < indicGlyphInfoList.Glyphs.Count; ++index)
        {
          glyphList1.Glyphs.Add(indicGlyphInfoList.Glyphs[index]);
          glyphList1.Text.Add((string) null);
        }
        glyphList1.SetText(count, glyphList1.Glyphs.Count, indicGlyphInfoList.GetText());
        num = indicGlyphInfoList.GlyphInfoEnd;
      }
      for (int index = num; index < glyphList.End; ++index)
      {
        glyphList1.Glyphs.Add(glyphList.Glyphs[index]);
        glyphList1.Text.Add((string) null);
      }
      for (int end = glyphList.End; end < glyphList.Glyphs.Count; ++end)
      {
        glyphList1.Glyphs.Add(glyphList.Glyphs[end]);
        glyphList1.Text.Add((string) null);
      }
      glyphList1.End = glyphList1.Glyphs.Count;
      glyphList.ReplaceContent(glyphList1);
      flag = true;
    }
    return flag;
  }

  private void ApplyScriptFeatures(
    ScriptFeature[] scriptFeatures,
    IndicGlyphInfoList indicGlyphInfo,
    IDictionary<string, IList<LookupTable>> lookupTables)
  {
    for (int index = 0; index < scriptFeatures.Length; ++index)
      this.ReplaceGlyphs(this.GetLookupTable(lookupTables, scriptFeatures[index].Name), scriptFeatures[index], indicGlyphInfo);
  }

  private IDictionary<string, IList<LookupTable>> GetLookupTables(
    UnicodeTrueTypeFont font,
    GSUBTable gsubTable,
    string otfScriptTag,
    LanguageRecord languageRecord)
  {
    IDictionary<string, IList<LookupTable>> lookupTables = (IDictionary<string, IList<LookupTable>>) new Dictionary<string, IList<LookupTable>>();
    foreach (int record1 in languageRecord.Records)
    {
      FeatureRecord record2 = gsubTable.OTFeature.Records[record1];
      IList<LookupTable> lookups = gsubTable.GetLookups(new FeatureRecord[1]
      {
        record2
      });
      if (lookupTables.ContainsKey(record2.Tag))
        lookupTables[record2.Tag] = lookups;
      else
        lookupTables.Add(record2.Tag, lookups);
    }
    GPOSTable gpos = font.TtfReader.GPOS;
    if (gpos != null)
    {
      LanguageRecord languageRecord1 = gpos.LanguageRecord(otfScriptTag);
      if (languageRecord1 != null)
      {
        foreach (int record3 in languageRecord1.Records)
        {
          FeatureRecord record4 = gpos.OTFeature.Records[record3];
          IList<LookupTable> lookups = gpos.GetLookups(new FeatureRecord[1]
          {
            record4
          });
          if (lookupTables.ContainsKey(record4.Tag))
            lookupTables[record4.Tag] = lookups;
          else
            lookupTables.Add(record4.Tag, lookups);
        }
      }
    }
    return lookupTables;
  }

  private void NormalizeGlyphList(UnicodeTrueTypeFont font, OtfGlyphInfoList glyphList)
  {
    for (int start = glyphList.Start; start < glyphList.End; ++start)
    {
      OtfGlyphInfo glyph = glyphList.Glyphs[start];
      string str = (string) null;
      if (glyph.CharCode > -1)
        str = char.ConvertFromUtf32(glyph.CharCode);
      else if (glyph.Characters != null)
        str = new string(glyph.Characters);
      if (str != null)
      {
        string s = str.Normalize(NormalizationForm.FormD);
        if (!str.Equals(s))
        {
          glyphList.Index = start;
          int index1 = 0;
          List<int> intList = new List<int>();
          while (index1 < s.Length)
          {
            if (char.IsSurrogatePair(s, index1))
            {
              intList.Add(char.ConvertToUtf32(s, index1));
              index1 += 2;
            }
            else
            {
              intList.Add((int) s[index1]);
              ++index1;
            }
          }
          int[] array = intList.ToArray();
          int[] glyphs = new int[array.Length];
          for (int index2 = 0; index2 < array.Length; ++index2)
          {
            TtfGlyphInfo ttfGlyphInfo = font.TtfReader.ReadGlyph(array[index2], false);
            glyphs[index2] = ttfGlyphInfo.Index;
          }
          glyphList.CombineAlternateGlyphs((OtfTable) font.TtfReader.GSUB, glyphs);
        }
      }
    }
  }

  private bool ReplaceGlyphs(IList<LookupTable> lookupTables, OtfGlyphInfoList glyphList)
  {
    bool flag = false;
    if (lookupTables != null)
    {
      foreach (LookupTable lookupTable in (IEnumerable<LookupTable>) lookupTables)
      {
        if (lookupTable != null && lookupTable.ReplaceGlyphs(glyphList))
          flag = true;
      }
    }
    return flag;
  }

  private bool ReplaceGlyphsOne(IList<LookupTable> lookupTables, OtfGlyphInfoList glyphList)
  {
    bool flag = false;
    if (lookupTables != null)
    {
      foreach (LookupTable lookupTable in (IEnumerable<LookupTable>) lookupTables)
      {
        if (lookupTable != null && lookupTable.ReplaceGlyph(glyphList))
          flag = true;
      }
    }
    return flag;
  }

  private bool ReplaceGlyphs(
    IList<LookupTable> lookupTables,
    ScriptFeature scriptFeature,
    IndicGlyphInfoList glyphInfoList)
  {
    bool flag = false;
    if (lookupTables != null)
    {
      foreach (LookupTable lookupTable in (IEnumerable<LookupTable>) lookupTables)
      {
        if (lookupTable != null)
        {
          if (scriptFeature.IsComplete)
          {
            if (lookupTable.ReplaceGlyphs((OtfGlyphInfoList) glyphInfoList))
              flag = true;
          }
          else
          {
            int end = glyphInfoList.End;
            int start;
            int index = start = glyphInfoList.Start;
            while (index < end)
            {
              while (index < end && (glyphInfoList[index].Mask & scriptFeature.Mask) == 0)
                ++index;
              if (index < end)
              {
                int num = index + 1;
                while (num < end && (glyphInfoList[index].Mask & scriptFeature.Mask) != 0)
                  ++num;
                glyphInfoList.Start = index;
                glyphInfoList.End = num;
                if (lookupTable.ReplaceGlyphs((OtfGlyphInfoList) glyphInfoList))
                  flag = true;
                end += glyphInfoList.End - num;
                index = glyphInfoList.End;
                glyphInfoList.Start = start;
                glyphInfoList.End = end;
              }
            }
          }
        }
      }
    }
    return flag;
  }

  private IndicGlyphInfoList ApplyScriptFeatures(
    IndicGlyphInfoList indicGlyphInfoList,
    ScriptFeature[] scriptFeatures,
    IDictionary<string, IList<LookupTable>> lookupTables)
  {
    List<IndicGlyphItem> indicGlyphItemList1 = new List<IndicGlyphItem>();
    List<IndicGlyphItem> indicGlyphItemList2 = new List<IndicGlyphItem>();
    indicGlyphItemList1.Add(new IndicGlyphItem(indicGlyphInfoList, 0));
    IndicGlyphItem indicGlyphItem1 = (IndicGlyphItem) null;
    while (indicGlyphItemList1.Count > 0)
    {
      IndicGlyphItem indicGlyphItem2 = indicGlyphItemList1[0];
      indicGlyphItemList1.RemoveAt(0);
      IndicGlyphInfoList glyphList = indicGlyphItem2.GlyphList;
      bool flag = false;
      for (int index = 0; index < scriptFeatures.Length; ++index)
      {
        ScriptFeature scriptFeature = scriptFeatures[index];
        IndicGlyphInfoList glyphInfoList = (IndicGlyphInfoList) glyphList.SubSet(glyphList.Start, glyphList.End);
        IList<LookupTable> lookupTables1 = (IList<LookupTable>) null;
        lookupTables.TryGetValue(scriptFeature.Name, out lookupTables1);
        if (this.ReplaceGlyphs(lookupTables1, scriptFeature, glyphInfoList))
        {
          flag = true;
          IndicGlyphItem indicGlyphItem3 = new IndicGlyphItem(glyphInfoList, indicGlyphItem2.Position + 1);
          if (!indicGlyphItemList2.Contains(indicGlyphItem3))
          {
            indicGlyphItemList2.Add(indicGlyphItem3);
            if (!indicGlyphItemList1.Contains(indicGlyphItem3))
              indicGlyphItemList1.Add(indicGlyphItem3);
          }
        }
      }
      if (!flag && (indicGlyphItem1 == null || indicGlyphItem1.Position < indicGlyphItem2.Position || indicGlyphItem1.Position == indicGlyphItem2.Position && indicGlyphItem1.Length > indicGlyphItem2.Length))
        indicGlyphItem1 = indicGlyphItem2;
    }
    return indicGlyphItem1 != null ? indicGlyphItem1.GlyphList : indicGlyphInfoList;
  }

  private IndicScript GetIndicScript(ScriptTags script)
  {
    IndicScript indicScript = new IndicScript();
    switch (script)
    {
      case ScriptTags.Bengali:
        indicScript.OldVersion = true;
        indicScript.InitialChar = 2509;
        indicScript.Position = 2;
        indicScript.RephPosition = 9;
        indicScript.RephMode = 0;
        indicScript.BlwfMode = 0;
        indicScript.Length = 2;
        break;
      case ScriptTags.Devanagari:
        indicScript.OldVersion = true;
        indicScript.InitialChar = 2381;
        indicScript.Position = 2;
        indicScript.RephPosition = 10;
        indicScript.RephMode = 0;
        indicScript.BlwfMode = 0;
        indicScript.Length = 2;
        break;
      case ScriptTags.Gujarati:
        indicScript.OldVersion = true;
        indicScript.InitialChar = 2765;
        indicScript.Position = 2;
        indicScript.RephPosition = 10;
        indicScript.RephMode = 0;
        indicScript.BlwfMode = 0;
        indicScript.Length = 2;
        break;
      case ScriptTags.Gurmukhi:
        indicScript.OldVersion = true;
        indicScript.InitialChar = 2637;
        indicScript.Position = 2;
        indicScript.RephPosition = 7;
        indicScript.RephMode = 0;
        indicScript.BlwfMode = 0;
        indicScript.Length = 2;
        break;
      case ScriptTags.Khmer:
        indicScript.OldVersion = true;
        indicScript.InitialChar = 6098;
        indicScript.Position = 0;
        indicScript.RephPosition = 1;
        indicScript.RephMode = 2;
        indicScript.BlwfMode = 0;
        indicScript.Length = 2;
        break;
      case ScriptTags.Kannada:
        indicScript.OldVersion = true;
        indicScript.InitialChar = 3277;
        indicScript.Position = 2;
        indicScript.RephPosition = 12;
        indicScript.RephMode = 0;
        indicScript.BlwfMode = 1;
        indicScript.Length = 2;
        break;
      case ScriptTags.Malayalam:
        indicScript.OldVersion = true;
        indicScript.InitialChar = 3405;
        indicScript.Position = 2;
        indicScript.RephPosition = 5;
        indicScript.RephMode = 3;
        indicScript.BlwfMode = 0;
        indicScript.Length = 2;
        break;
      case ScriptTags.Oriya:
        indicScript.OldVersion = true;
        indicScript.InitialChar = 2893;
        indicScript.Position = 2;
        indicScript.RephPosition = 5;
        indicScript.RephMode = 0;
        indicScript.BlwfMode = 0;
        indicScript.Length = 2;
        break;
      case ScriptTags.Tamil:
        indicScript.OldVersion = true;
        indicScript.InitialChar = 3021;
        indicScript.Position = 2;
        indicScript.RephPosition = 12;
        indicScript.RephMode = 0;
        indicScript.BlwfMode = 0;
        indicScript.Length = 2;
        break;
      case ScriptTags.Telugu:
        indicScript.OldVersion = true;
        indicScript.InitialChar = 3149;
        indicScript.Position = 2;
        indicScript.RephPosition = 12;
        indicScript.RephMode = 1;
        indicScript.BlwfMode = 1;
        indicScript.Length = 2;
        break;
      default:
        throw new Exception("Invalid script");
    }
    return indicScript;
  }

  private IList<int> BreakArabicLineIntoWords(
    OtfGlyphInfoList glyphList,
    IList<LookupTable> initial,
    IList<LookupTable> medial,
    IList<LookupTable> terminal,
    GDEFTable gDEFTable)
  {
    IList<int> intList = (IList<int>) new List<int>();
    bool flag1 = false;
    char ch = 'ـ';
    bool flag2 = false;
    LanguageUtil languageUtil = new LanguageUtil();
    for (int index = 0; index < glyphList.Glyphs.Count; ++index)
    {
      OtfGlyphInfo glyph = glyphList.Glyphs[index];
      bool flag3 = gDEFTable.GlyphCdefTable.IsMark(glyph.Index);
      bool flag4 = index + 1 < glyphList.End && gDEFTable.GlyphCdefTable.IsMark(glyphList.Glyphs[index + 1].Index);
      bool flag5 = ScriptTags.Arabic.Equals((object) languageUtil.GetLanguage((char) glyph.CharCode)) | flag3 || (int) ch == glyph.CharCode;
      if (flag1)
      {
        bool flag6 = !flag3 && !this.IsMedialLetter(new string(glyph.Characters));
        if (!flag4 && flag6 | flag2 || !flag5)
        {
          if (!flag5)
            intList.Add(index);
          else
            intList.Add(index + 1);
          flag1 = false;
          flag2 = false;
        }
        else if (!flag2)
          flag2 = flag6;
      }
      else if (flag5 && this.IsMedialLetter(new string(glyph.Characters)))
      {
        intList.Add(index);
        flag1 = true;
      }
    }
    if (intList.Count % 2 != 0)
      intList.Add(glyphList.Glyphs.Count);
    return intList;
  }

  private bool IsMedialLetter(string word)
  {
    foreach (int num in word.Normalize(NormalizationForm.FormKD))
    {
      if (Array.BinarySearch<int>(this.arabicIsolatedCode, num) >= 0)
        return false;
    }
    return true;
  }

  private bool DoInitMediFinaShaping(
    OtfGlyphInfoList glyphLine,
    IList<LookupTable> initial,
    IList<LookupTable> medial,
    IList<LookupTable> final,
    GDEFTable gDEFTable,
    IList<int> words)
  {
    if (initial == null || medial == null || final == null)
      return false;
    bool flag = false;
    for (int index1 = 0; index1 < words.Count; index1 += 2)
    {
      int word1 = words[index1];
      int word2 = words[index1 + 1];
      IList<int> intList = (IList<int>) new List<int>();
      for (int index2 = word1; index2 < word2; ++index2)
      {
        if (!gDEFTable.GlyphCdefTable.IsMark(glyphLine.Glyphs[index2].Index))
          intList.Add(index2);
      }
      if (intList.Count > 1)
      {
        glyphLine.Index = intList[0];
        if (this.ReplaceGlyphsOne(initial, glyphLine))
          flag = true;
      }
      for (int index3 = 1; index3 < intList.Count - 1; ++index3)
      {
        glyphLine.Index = intList[index3];
        if (this.ReplaceGlyphsOne(medial, glyphLine))
          flag = true;
      }
      if (intList.Count > 1)
      {
        glyphLine.Index = intList[intList.Count - 1];
        if (this.ReplaceGlyphsOne(final, glyphLine))
          flag = true;
      }
    }
    return flag;
  }

  private bool DoLigatureFeature(OtfGlyphInfoList glyphLine, IList<LookupTable> ligature)
  {
    bool flag = false;
    if (glyphLine != null && ligature != null)
    {
      foreach (LookupTable lookupTable in (IEnumerable<LookupTable>) ligature)
      {
        if (lookupTable != null && lookupTable.ReplaceGlyphs(glyphLine))
          flag = true;
      }
    }
    return flag;
  }
}
