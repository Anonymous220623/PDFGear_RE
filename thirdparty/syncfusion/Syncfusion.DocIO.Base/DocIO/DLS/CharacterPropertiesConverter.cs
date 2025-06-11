// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.CharacterPropertiesConverter
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class CharacterPropertiesConverter
{
  private static List<int> m_incorrectOptions;
  private static readonly object m_threadLocker = new object();
  private static List<string> m_authorNames;

  private static List<int> IncorrectOptions
  {
    get
    {
      if (CharacterPropertiesConverter.m_incorrectOptions == null)
      {
        CharacterPropertiesConverter.m_incorrectOptions = new List<int>();
        CharacterPropertiesConverter.m_incorrectOptions.Add(0);
        CharacterPropertiesConverter.m_incorrectOptions.Add(10);
        CharacterPropertiesConverter.m_incorrectOptions.Add(15);
        CharacterPropertiesConverter.m_incorrectOptions.Add(16 /*0x10*/);
        CharacterPropertiesConverter.m_incorrectOptions.Add(17);
        CharacterPropertiesConverter.m_incorrectOptions.Add(30);
        CharacterPropertiesConverter.m_incorrectOptions.Add(31 /*0x1F*/);
        CharacterPropertiesConverter.m_incorrectOptions.Add(32 /*0x20*/);
        CharacterPropertiesConverter.m_incorrectOptions.Add(33);
        CharacterPropertiesConverter.m_incorrectOptions.Add(35);
        CharacterPropertiesConverter.m_incorrectOptions.Add(36);
        CharacterPropertiesConverter.m_incorrectOptions.Add(40);
        CharacterPropertiesConverter.m_incorrectOptions.Add(42);
        CharacterPropertiesConverter.m_incorrectOptions.Add(43);
        CharacterPropertiesConverter.m_incorrectOptions.Add(44);
        CharacterPropertiesConverter.m_incorrectOptions.Add(45);
        CharacterPropertiesConverter.m_incorrectOptions.Add(46);
        CharacterPropertiesConverter.m_incorrectOptions.Add(50);
        CharacterPropertiesConverter.m_incorrectOptions.Add(256 /*0x0100*/);
        CharacterPropertiesConverter.m_incorrectOptions.Add(512 /*0x0200*/);
        CharacterPropertiesConverter.m_incorrectOptions.Add(2304 /*0x0900*/);
        CharacterPropertiesConverter.m_incorrectOptions.Add(2560 /*0x0A00*/);
        CharacterPropertiesConverter.m_incorrectOptions.Add(3328 /*0x0D00*/);
        CharacterPropertiesConverter.m_incorrectOptions.Add(3968);
        CharacterPropertiesConverter.m_incorrectOptions.Add(4096 /*0x1000*/);
        CharacterPropertiesConverter.m_incorrectOptions.Add(4104);
        CharacterPropertiesConverter.m_incorrectOptions.Add(4185);
        CharacterPropertiesConverter.m_incorrectOptions.Add(4460);
        CharacterPropertiesConverter.m_incorrectOptions.Add(4468);
        CharacterPropertiesConverter.m_incorrectOptions.Add(6912);
        CharacterPropertiesConverter.m_incorrectOptions.Add(7914);
        CharacterPropertiesConverter.m_incorrectOptions.Add(8163);
        CharacterPropertiesConverter.m_incorrectOptions.Add(8169);
        CharacterPropertiesConverter.m_incorrectOptions.Add(8207);
        CharacterPropertiesConverter.m_incorrectOptions.Add(8325);
        CharacterPropertiesConverter.m_incorrectOptions.Add(8482);
        CharacterPropertiesConverter.m_incorrectOptions.Add(8520);
        CharacterPropertiesConverter.m_incorrectOptions.Add(8933);
        CharacterPropertiesConverter.m_incorrectOptions.Add(8939);
        CharacterPropertiesConverter.m_incorrectOptions.Add(8960);
        CharacterPropertiesConverter.m_incorrectOptions.Add(9080);
        CharacterPropertiesConverter.m_incorrectOptions.Add(9088);
        CharacterPropertiesConverter.m_incorrectOptions.Add(9212);
        CharacterPropertiesConverter.m_incorrectOptions.Add(9253);
        CharacterPropertiesConverter.m_incorrectOptions.Add(9367);
        CharacterPropertiesConverter.m_incorrectOptions.Add(9728);
        CharacterPropertiesConverter.m_incorrectOptions.Add(9984);
        CharacterPropertiesConverter.m_incorrectOptions.Add(10446);
        CharacterPropertiesConverter.m_incorrectOptions.Add(10752);
        CharacterPropertiesConverter.m_incorrectOptions.Add(10789);
        CharacterPropertiesConverter.m_incorrectOptions.Add(10876);
        CharacterPropertiesConverter.m_incorrectOptions.Add(11015);
        CharacterPropertiesConverter.m_incorrectOptions.Add(11061);
        CharacterPropertiesConverter.m_incorrectOptions.Add(11176);
        CharacterPropertiesConverter.m_incorrectOptions.Add(11493);
        CharacterPropertiesConverter.m_incorrectOptions.Add(11603);
        CharacterPropertiesConverter.m_incorrectOptions.Add(11776);
        CharacterPropertiesConverter.m_incorrectOptions.Add(12897);
        CharacterPropertiesConverter.m_incorrectOptions.Add(12929);
        CharacterPropertiesConverter.m_incorrectOptions.Add(13028);
        CharacterPropertiesConverter.m_incorrectOptions.Add(13036);
        CharacterPropertiesConverter.m_incorrectOptions.Add(13063);
        CharacterPropertiesConverter.m_incorrectOptions.Add(13287);
        CharacterPropertiesConverter.m_incorrectOptions.Add(13298);
        CharacterPropertiesConverter.m_incorrectOptions.Add(13328);
        CharacterPropertiesConverter.m_incorrectOptions.Add(13824);
        CharacterPropertiesConverter.m_incorrectOptions.Add(17408);
        CharacterPropertiesConverter.m_incorrectOptions.Add(19968);
        CharacterPropertiesConverter.m_incorrectOptions.Add(20224);
        CharacterPropertiesConverter.m_incorrectOptions.Add(21504);
        CharacterPropertiesConverter.m_incorrectOptions.Add(21760);
        CharacterPropertiesConverter.m_incorrectOptions.Add(22016);
        CharacterPropertiesConverter.m_incorrectOptions.Add(24064);
        CharacterPropertiesConverter.m_incorrectOptions.Add(24320);
        CharacterPropertiesConverter.m_incorrectOptions.Add(26624);
        CharacterPropertiesConverter.m_incorrectOptions.Add(27136);
        CharacterPropertiesConverter.m_incorrectOptions.Add(27904);
        CharacterPropertiesConverter.m_incorrectOptions.Add(29952);
        CharacterPropertiesConverter.m_incorrectOptions.Add(30976);
        CharacterPropertiesConverter.m_incorrectOptions.Add(31488);
        CharacterPropertiesConverter.m_incorrectOptions.Add(32000);
        CharacterPropertiesConverter.m_incorrectOptions.Add(32280);
        CharacterPropertiesConverter.m_incorrectOptions.Add(32768 /*0x8000*/);
        CharacterPropertiesConverter.m_incorrectOptions.Add(33024);
        CharacterPropertiesConverter.m_incorrectOptions.Add(33536);
        CharacterPropertiesConverter.m_incorrectOptions.Add(34816);
        CharacterPropertiesConverter.m_incorrectOptions.Add(35328);
        CharacterPropertiesConverter.m_incorrectOptions.Add(35584);
        CharacterPropertiesConverter.m_incorrectOptions.Add(35840);
        CharacterPropertiesConverter.m_incorrectOptions.Add(38656);
        CharacterPropertiesConverter.m_incorrectOptions.Add(40192);
        CharacterPropertiesConverter.m_incorrectOptions.Add(40960 /*0xA000*/);
        CharacterPropertiesConverter.m_incorrectOptions.Add(41984);
        CharacterPropertiesConverter.m_incorrectOptions.Add(42240);
        CharacterPropertiesConverter.m_incorrectOptions.Add(42496);
        CharacterPropertiesConverter.m_incorrectOptions.Add(43008);
        CharacterPropertiesConverter.m_incorrectOptions.Add(43520);
        CharacterPropertiesConverter.m_incorrectOptions.Add(43776);
        CharacterPropertiesConverter.m_incorrectOptions.Add(44032);
        CharacterPropertiesConverter.m_incorrectOptions.Add(44288);
        CharacterPropertiesConverter.m_incorrectOptions.Add(45568);
        CharacterPropertiesConverter.m_incorrectOptions.Add(45824);
        CharacterPropertiesConverter.m_incorrectOptions.Add(46080);
        CharacterPropertiesConverter.m_incorrectOptions.Add(46336);
        CharacterPropertiesConverter.m_incorrectOptions.Add(47104);
        CharacterPropertiesConverter.m_incorrectOptions.Add(47360);
        CharacterPropertiesConverter.m_incorrectOptions.Add(47616);
        CharacterPropertiesConverter.m_incorrectOptions.Add(47872);
        CharacterPropertiesConverter.m_incorrectOptions.Add(48128);
        CharacterPropertiesConverter.m_incorrectOptions.Add(49408);
        CharacterPropertiesConverter.m_incorrectOptions.Add(52992);
        CharacterPropertiesConverter.m_incorrectOptions.Add(53504);
        CharacterPropertiesConverter.m_incorrectOptions.Add(58112);
        CharacterPropertiesConverter.m_incorrectOptions.Add(58368);
        CharacterPropertiesConverter.m_incorrectOptions.Add(58880);
        CharacterPropertiesConverter.m_incorrectOptions.Add(59904);
        CharacterPropertiesConverter.m_incorrectOptions.Add(60160);
        CharacterPropertiesConverter.m_incorrectOptions.Add(60672);
        CharacterPropertiesConverter.m_incorrectOptions.Add(61696);
        CharacterPropertiesConverter.m_incorrectOptions.Add(64000);
        CharacterPropertiesConverter.m_incorrectOptions.Add(64256);
      }
      return CharacterPropertiesConverter.m_incorrectOptions;
    }
  }

  internal static List<string> AuthorNames
  {
    get
    {
      if (CharacterPropertiesConverter.m_authorNames == null)
      {
        CharacterPropertiesConverter.m_authorNames = new List<string>();
        CharacterPropertiesConverter.m_authorNames.Add("Unknown");
      }
      return CharacterPropertiesConverter.m_authorNames;
    }
  }

  public static void SprmsToFormat(IWordReaderBase reader, WCharacterFormat format)
  {
    lock (CharacterPropertiesConverter.m_threadLocker)
      CharacterPropertiesConverter.SprmsToFormat(reader.CHPX.PropertyModifiers, format, reader.StyleSheet, reader.SttbfRMarkAuthorNames, true);
  }

  internal static void SprmsToFormat(
    SinglePropertyModifierArray CHPModifierArray,
    WCharacterFormat characterFormat,
    WordStyleSheet styleSheet,
    Dictionary<int, string> authorNames,
    bool isNewPropertyHash)
  {
    lock (CharacterPropertiesConverter.m_threadLocker)
    {
      characterFormat.IsDocReading = true;
      if (CHPModifierArray == null)
        return;
      if (isNewPropertyHash)
      {
        characterFormat.PropertiesHash.Clear();
        characterFormat.OldPropertiesHash.Clear();
      }
      if (CHPModifierArray.Contain(10883))
        characterFormat.IsFormattingChange = true;
      bool flag = false;
      foreach (SinglePropertyModifierRecord chpModifier in CHPModifierArray)
      {
        switch (chpModifier.Options)
        {
          case 2048 /*0x0800*/:
            characterFormat.SetPropertyValue(104, (object) chpModifier.ByteValue);
            continue;
          case 2049:
            characterFormat.SetPropertyValue(103, (object) chpModifier.ByteValue);
            continue;
          case 2050:
            characterFormat.SetPropertyValue(109, (object) chpModifier.ByteValue);
            continue;
          case 2065:
            characterFormat.SetPropertyValue(92, (object) chpModifier.ByteValue);
            continue;
          case 2072:
            characterFormat.SetPropertyValue(24, (object) chpModifier.ByteValue);
            continue;
          case 2101:
            characterFormat.SetPropertyValue(4, (object) chpModifier.ByteValue);
            continue;
          case 2102:
            characterFormat.SetPropertyValue(5, (object) chpModifier.ByteValue);
            continue;
          case 2103:
            characterFormat.SetPropertyValue(6, (object) chpModifier.ByteValue);
            continue;
          case 2104:
            characterFormat.SetPropertyValue(71, (object) chpModifier.ByteValue);
            continue;
          case 2105:
            characterFormat.SetPropertyValue(50, (object) chpModifier.ByteValue);
            continue;
          case 2106:
            characterFormat.SetPropertyValue(55, (object) chpModifier.ByteValue);
            continue;
          case 2107:
            characterFormat.SetPropertyValue(54, (object) chpModifier.ByteValue);
            continue;
          case 2108:
            characterFormat.SetPropertyValue(53, (object) chpModifier.ByteValue);
            continue;
          case 2132:
            characterFormat.SetPropertyValue(52, (object) chpModifier.ByteValue);
            continue;
          case 2133:
            characterFormat.SetPropertyValue(106, (object) chpModifier.ByteValue);
            continue;
          case 2136:
            characterFormat.SetPropertyValue(51, (object) chpModifier.ByteValue);
            continue;
          case 2138:
            characterFormat.SetPropertyValue(58, (object) chpModifier.ByteValue);
            continue;
          case 2140:
            characterFormat.SetPropertyValue(59, (object) chpModifier.ByteValue);
            continue;
          case 2141:
            characterFormat.SetPropertyValue(60, (object) chpModifier.ByteValue);
            continue;
          case 2152:
            characterFormat.SetPropertyValue(81, (object) chpModifier.ByteValue);
            continue;
          case 2165:
            characterFormat.NoProof = chpModifier.BoolValue;
            continue;
          case 2178:
            characterFormat.ComplexScript = chpModifier.BoolValue;
            continue;
          case 10329:
            characterFormat.TextEffect = (TextEffect) CHPModifierArray.GetByte(10329, (byte) 0);
            continue;
          case 10351:
            characterFormat.IdctHint = (FontHintType) chpModifier.ByteValue;
            continue;
          case 10361:
            characterFormat.BreakClear = (BreakClearType) chpModifier.ByteValue;
            continue;
          case 10764:
            characterFormat.HighlightColor = WordColor.ColorsArray[(int) chpModifier.ByteValue];
            continue;
          case 10804:
            characterFormat.EmphasisType = (EmphasisType) chpModifier.ByteValue;
            continue;
          case 10814:
            characterFormat.UnderlineStyle = (UnderlineStyle) chpModifier.ByteValue;
            continue;
          case 10818:
          case 19040:
            if (CHPModifierArray.GetUInt(26736, uint.MaxValue) == uint.MaxValue)
            {
              characterFormat.TextColor = WordColor.ConvertIdToColor((int) CHPModifierArray.GetByte(10818, (byte) 0));
              continue;
            }
            continue;
          case 10824:
            characterFormat.SubSuperScript = (SubSuperScript) chpModifier.ByteValue;
            continue;
          case 10835:
            characterFormat.SetPropertyValue(14, (object) chpModifier.ByteValue);
            continue;
          case 10883:
            characterFormat.IsFormattingChange = false;
            characterFormat.IsChangedFormat = true;
            characterFormat.SetPropertyValue(105, (object) true);
            continue;
          case 18436:
            short key1 = CHPModifierArray.GetShort(18436, (short) 0);
            if (authorNames != null && authorNames.Count > 0 && authorNames.ContainsKey((int) key1))
            {
              characterFormat.AuthorName = authorNames[(int) key1];
              continue;
            }
            continue;
          case 18501:
            characterFormat.SetPropertyValue(17, (object) (float) ((double) chpModifier.ShortValue / 2.0));
            continue;
          case 18507:
            characterFormat.Kern = (float) chpModifier.UshortValue / 2f;
            continue;
          case 18514:
            characterFormat.Scaling = (float) CHPModifierArray.GetUShort((int) chpModifier.Options, (ushort) 0);
            continue;
          case 18527:
            short num = CHPModifierArray.GetShort(18527, (short) 0);
            characterFormat.LocaleIdBidi = num == (short) 1024 /*0x0400*/ ? (short) 1025 : num;
            continue;
          case 18531:
            short key2 = CHPModifierArray.GetShort(18531, (short) 0);
            if (authorNames != null && authorNames.Count > 0 && authorNames.ContainsKey((int) key2))
            {
              characterFormat.AuthorName = authorNames[(int) key2];
              continue;
            }
            continue;
          case 18534:
            if (!CHPModifierArray.Contain(51825))
            {
              ShadingDescriptor shading = CharacterPropertiesConverter.GetShading(chpModifier);
              characterFormat.TextBackgroundColor = shading.BackColor;
              characterFormat.ForeColor = shading.ForeColor;
              characterFormat.TextureStyle = shading.Pattern;
              continue;
            }
            continue;
          case 18541:
            characterFormat.LocaleIdASCII = chpModifier.ShortValue;
            continue;
          case 18542:
            characterFormat.LocaleIdFarEast = chpModifier.ShortValue;
            continue;
          case 18547:
            characterFormat.LocaleIdASCII = chpModifier.ShortValue;
            continue;
          case 18548:
            characterFormat.LocaleIdFarEast = chpModifier.ShortValue;
            continue;
          case 18568:
            characterFormat.ListHasPicture = chpModifier.BoolValue;
            continue;
          case 18992:
            if (!flag && !(characterFormat.OwnerBase is Style) && styleSheet != null)
            {
              if (styleSheet.StyleNames.ContainsKey((int) chpModifier.UshortValue))
              {
                characterFormat.CharStyleName = styleSheet.StyleNames[(int) chpModifier.UshortValue];
                continue;
              }
              if (styleSheet.StyleNames.Count > 0)
              {
                characterFormat.CharStyleName = styleSheet.StyleNames[0];
                continue;
              }
              continue;
            }
            continue;
          case 19011:
            characterFormat.SetPropertyValue(3, (object) (float) ((double) chpModifier.UshortValue / 2.0));
            continue;
          case 19023:
            int ushortValue1 = (int) chpModifier.UshortValue;
            if (styleSheet != null && ushortValue1 < styleSheet.FontNamesList.Count)
            {
              string fontNames = styleSheet.FontNamesList[ushortValue1];
              characterFormat.SetPropertyValue(68, (object) fontNames);
              characterFormat.SetPropertyValue(2, (object) fontNames);
              continue;
            }
            continue;
          case 19024:
            int ushortValue2 = (int) chpModifier.UshortValue;
            if (styleSheet != null && ushortValue2 < styleSheet.FontNamesList.Count)
            {
              characterFormat.SetPropertyValue(69, (object) styleSheet.FontNamesList[ushortValue2]);
              continue;
            }
            continue;
          case 19025:
            int ushortValue3 = (int) chpModifier.UshortValue;
            if (styleSheet != null && ushortValue3 < styleSheet.FontNamesList.Count)
            {
              characterFormat.SetPropertyValue(70, (object) styleSheet.FontNamesList[ushortValue3]);
              continue;
            }
            continue;
          case 19038:
            if (styleSheet != null)
            {
              characterFormat.FontNameBidi = styleSheet.FontNamesList.Count == 0 ? string.Empty : ((int) chpModifier.UshortValue < styleSheet.FontNamesList.Count ? styleSheet.FontNamesList[(int) chpModifier.UshortValue] : string.Empty);
              continue;
            }
            continue;
          case 19041:
            characterFormat.SetPropertyValue(62, (object) (float) ((int) chpModifier.UshortValue / 2));
            continue;
          case 26629:
          case 26724:
            DateTime dateTime1 = characterFormat.ParseDTTM(chpModifier.IntValue);
            if (dateTime1.Year < 1900)
              dateTime1 = new DateTime(1900, 1, 1, 0, 0, 0);
            characterFormat.RevDateTime = dateTime1;
            continue;
          case 26725:
            if (!CHPModifierArray.Contain(51826))
            {
              ParagraphPropertiesConverter.ExportBorder(new BorderCode(CHPModifierArray.GetByteArray(26725), 0), (Border) characterFormat.GetPropertyValue(67));
              continue;
            }
            continue;
          case 26736:
            uint uintValue = chpModifier.UIntValue;
            characterFormat.TextColor = uintValue != uint.MaxValue ? WordColor.ConvertRGBToColor(uintValue) : WordColor.ConvertIdToColor((int) CHPModifierArray.GetByte(10818, (byte) 0));
            continue;
          case 26743:
            characterFormat.UnderlineColor = WordColor.ConvertRGBToColor(chpModifier.UIntValue);
            continue;
          case 26759:
            SinglePropertyModifierRecord propertyModifierRecord = characterFormat.IsFormattingChange ? CHPModifierArray.GetOldSprm(18568, 10883) : CHPModifierArray.GetNewSprm(18568, 10883);
            if (propertyModifierRecord != null && propertyModifierRecord.BoolValue)
            {
              characterFormat.ListPictureIndex = chpModifier.IntValue;
              continue;
            }
            continue;
          case 34880:
            characterFormat.SetPropertyValue(18, (object) (float) ((double) CHPModifierArray.GetShort(34880, (short) 0) / 20.0));
            continue;
          case 51761:
            if (chpModifier.ByteArray.Length > 0 && chpModifier.ByteArray[0] == (byte) 0 && CHPModifierArray.Contain(18992) && !(characterFormat.OwnerBase is Style))
            {
              ushort ushortValue4 = CHPModifierArray[18992].UshortValue;
              ushort uint16_1 = BitConverter.ToUInt16(chpModifier.ByteArray, 1);
              ushort uint16_2 = BitConverter.ToUInt16(chpModifier.ByteArray, 3);
              if ((int) uint16_2 >= (int) uint16_1 && (int) ushortValue4 >= (int) uint16_1 && (int) ushortValue4 <= (int) uint16_2)
              {
                int startIndex = 5;
                ushort length = (ushort) ((int) uint16_2 - (int) uint16_1 + 1);
                ushort[] numArray = new ushort[(int) length];
                for (int index = 0; index < (int) length && startIndex + 1 < chpModifier.ByteArray.Length; ++index)
                {
                  numArray[index] = BitConverter.ToUInt16(chpModifier.ByteArray, startIndex);
                  startIndex += 2;
                }
                if ((int) ushortValue4 - (int) uint16_1 < numArray.Length)
                  ushortValue4 = numArray[(int) ushortValue4 - (int) uint16_1];
                if (styleSheet != null)
                {
                  if (styleSheet.StyleNames.ContainsKey((int) ushortValue4))
                    characterFormat.CharStyleName = styleSheet.StyleNames[(int) ushortValue4];
                  else if (styleSheet.StyleNames.Count > 0)
                    characterFormat.CharStyleName = styleSheet.StyleNames[0];
                }
                flag = true;
                continue;
              }
              continue;
            }
            continue;
          case 51799:
            if (!CHPModifierArray.Contain(51849))
            {
              short int16 = BitConverter.ToInt16(chpModifier.ByteArray, 1);
              if (authorNames != null && authorNames.Count > 0 && authorNames.ContainsKey((int) int16))
                characterFormat.FormatChangeAuthorName = authorNames[(int) int16];
              DateTime dateTime2 = characterFormat.ParseDTTM(BitConverter.ToInt32(chpModifier.ByteArray, 3));
              if (dateTime2.Year < 1900)
                dateTime2 = new DateTime(1900, 1, 1, 0, 0, 0);
              characterFormat.FormatChangeDateTime = dateTime2;
              continue;
            }
            continue;
          case 51825:
            ShadingDescriptor shading1 = CharacterPropertiesConverter.GetShading(chpModifier);
            characterFormat.TextBackgroundColor = shading1.BackColor;
            characterFormat.ForeColor = shading1.ForeColor;
            characterFormat.TextureStyle = shading1.Pattern;
            continue;
          case 51826:
            byte[] byteArray = CHPModifierArray.GetByteArray(51826);
            BorderCode srcBorder = new BorderCode();
            srcBorder.ParseNewBrc(byteArray, 0);
            ParagraphPropertiesConverter.ExportBorder(srcBorder, (Border) characterFormat.GetPropertyValue(67));
            continue;
          case 51830:
            if (chpModifier.ByteArray.Length > 0)
              characterFormat.FitTextWidth = BitConverter.ToInt32(chpModifier.ByteArray, 0) / 20;
            if (chpModifier.ByteArray.Length > 7)
            {
              characterFormat.FitTextID = BitConverter.ToInt32(chpModifier.ByteArray, 4);
              continue;
            }
            continue;
          case 51832:
            CFELayout cfeLayout = new CFELayout();
            cfeLayout.UpdateCFELayout((ushort) BitConverter.ToInt16(chpModifier.ByteArray, 0), BitConverter.ToInt32(chpModifier.ByteArray, 2));
            characterFormat.CFELayout = cfeLayout;
            continue;
          case 51849:
            short int16_1 = BitConverter.ToInt16(chpModifier.ByteArray, 1);
            if (authorNames != null && authorNames.Count > 0 && authorNames.ContainsKey((int) int16_1))
              characterFormat.FormatChangeAuthorName = authorNames[(int) int16_1];
            DateTime dateTime3 = characterFormat.ParseDTTM(BitConverter.ToInt32(chpModifier.ByteArray, 3));
            if (dateTime3.Year < 1900)
              dateTime3 = new DateTime(1900, 1, 1, 0, 0, 0);
            characterFormat.FormatChangeDateTime = dateTime3;
            continue;
          default:
            continue;
        }
      }
      if ((!CHPModifierArray.Contain(10883) || CHPModifierArray.GetNewSprm(18992, 10883) == null) && characterFormat.OldPropertiesHash.Count > 0 && characterFormat.PropertiesHash.Count > 0)
      {
        foreach (KeyValuePair<int, object> keyValuePair in characterFormat.OldPropertiesHash)
        {
          if (!characterFormat.PropertiesHash.ContainsKey(keyValuePair.Key))
            characterFormat.PropertiesHash.Add(keyValuePair.Key, keyValuePair.Value);
        }
      }
      if (!characterFormat.HasValue(103) && !characterFormat.HasValue(104))
        return;
      characterFormat.Document.SetTriggerElement(ref characterFormat.Document.m_notSupportedElementFlag, 30);
    }
  }

  internal static void FormatToSprms(
    WCharacterFormat characterFormat,
    SinglePropertyModifierArray sprms,
    WordStyleSheet styleSheet)
  {
    lock (CharacterPropertiesConverter.m_threadLocker)
    {
      sprms.Clear();
      Dictionary<int, object> propertyHash = new Dictionary<int, object>();
      if (characterFormat.PropertiesHash.Count > 0)
        propertyHash = new Dictionary<int, object>((IDictionary<int, object>) characterFormat.PropertiesHash);
      if (characterFormat.OldPropertiesHash.Count > 0)
      {
        foreach (KeyValuePair<int, object> keyValuePair in new Dictionary<int, object>((IDictionary<int, object>) characterFormat.OldPropertiesHash))
        {
          CharacterPropertiesConverter.FormatToSprms(keyValuePair.Key, keyValuePair.Value, sprms, characterFormat, styleSheet);
          if (propertyHash.ContainsKey(keyValuePair.Key) && propertyHash[keyValuePair.Key] == keyValuePair.Value)
            propertyHash.Remove(keyValuePair.Key);
        }
      }
      if (propertyHash.Count > 0)
        CharacterPropertiesConverter.UpdateFontSprms(propertyHash, sprms, characterFormat, styleSheet);
      if (propertyHash.Count > 0 && propertyHash.ContainsKey(105))
      {
        CharacterPropertiesConverter.FormatToSprms(105, propertyHash[105], sprms, characterFormat, styleSheet);
        propertyHash.Remove(105);
      }
      if (propertyHash.Count > 0)
      {
        SinglePropertyModifierArray sprms1 = new SinglePropertyModifierArray();
        foreach (KeyValuePair<int, object> keyValuePair in propertyHash)
          CharacterPropertiesConverter.FormatToSprms(keyValuePair.Key, keyValuePair.Value, sprms1, characterFormat, styleSheet);
        CharacterPropertiesConverter.UpdateFontSprms(propertyHash, sprms1, characterFormat, styleSheet);
        for (int sprmIndex = 0; sprmIndex < sprms1.Count; ++sprmIndex)
          sprms.Add(sprms1.GetSprmByIndex(sprmIndex).Clone());
        sprms1.Clear();
      }
      sprms.SortSprms();
    }
  }

  internal static void FormatToSprms(
    int propKey,
    object value,
    SinglePropertyModifierArray sprms,
    WCharacterFormat charFormat,
    WordStyleSheet styleSheet)
  {
    lock (CharacterPropertiesConverter.m_threadLocker)
    {
      int sprmOption = charFormat.GetSprmOption(propKey);
      switch (propKey)
      {
        case 1:
          uint rgb1 = WordColor.ConvertColorToRGB((Color) value);
          sprms.SetByteValue(sprmOption, (byte) WordColor.ConvertRGBToId(rgb1));
          sprms.SetUIntValue(26736, rgb1);
          break;
        case 3:
          sprms.SetUShortValue(sprmOption, (ushort) ((double) (float) value * 2.0));
          break;
        case 4:
        case 5:
        case 6:
        case 24:
        case 50:
        case 51:
        case 52:
        case 53:
        case 54:
        case 55:
        case 58:
        case 59:
        case 60:
        case 71:
        case 76:
        case 81:
        case 92:
        case 99:
        case 103:
        case 104:
        case 106:
        case 109:
          byte num1;
          if (charFormat.IsDocReading)
          {
            try
            {
              num1 = CharacterPropertiesConverter.GetByteValue(value);
            }
            catch
            {
              num1 = charFormat.GetBoolComplexValue(propKey, charFormat.GetBoolPropertyValue((short) propKey));
            }
          }
          else
            num1 = charFormat.GetBoolComplexValue(propKey, charFormat.GetBoolPropertyValue((short) propKey));
          sprms.SetByteValue(sprmOption, num1);
          break;
        case 7:
          sprms.SetByteValue(sprmOption, (byte) (UnderlineStyle) value);
          break;
        case 8:
          Entity ownerBase1 = charFormat.OwnerBase as Entity;
          List<Revision> revisionList1 = charFormat.OwnerBase is WParagraph || charFormat.OwnerBase is WTableCell ? charFormat.Revisions : (charFormat.OwnerBase is WTableRow ? (charFormat.OwnerBase as WTableRow).RowFormat.Revisions : ownerBase1?.RevisionsInternal);
          if (revisionList1 == null || revisionList1.Count <= 0)
            break;
          using (List<Revision>.Enumerator enumerator = revisionList1.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              Revision current = enumerator.Current;
              if (!CharacterPropertiesConverter.AuthorNames.Contains(current.Author))
                CharacterPropertiesConverter.AuthorNames.Add(current.Author);
              if (current.RevisionType == RevisionType.Insertions || current.RevisionType == RevisionType.MoveTo)
                sprms.SetShortValue(18436, (short) CharacterPropertiesConverter.AuthorNames.IndexOf(current.Author));
              else if (current.RevisionType == RevisionType.Deletions || current.RevisionType == RevisionType.MoveFrom)
                sprms.SetShortValue(18531, (short) CharacterPropertiesConverter.AuthorNames.IndexOf(current.Author));
            }
            break;
          }
        case 9:
        case 77:
        case 78:
          if (sprms.Contain(18534))
            break;
          ShadingDescriptor shadingDescriptor = new ShadingDescriptor();
          if (charFormat.HasKey(9))
            shadingDescriptor.BackColor = charFormat.TextBackgroundColor;
          if (charFormat.HasKey(77))
            shadingDescriptor.ForeColor = charFormat.ForeColor;
          if (charFormat.HasKey(78))
            shadingDescriptor.Pattern = charFormat.TextureStyle;
          sprms.SetShortValue(18534, shadingDescriptor.Save());
          sprms.SetByteArrayValue(51825, shadingDescriptor.SaveNewShd());
          break;
        case 10:
          sprms.SetByteValue(sprmOption, (byte) (SubSuperScript) value);
          break;
        case 11:
          Entity ownerBase2 = charFormat.OwnerBase as Entity;
          List<Revision> revisionList2 = charFormat.OwnerBase is WParagraph || charFormat.OwnerBase is WTableCell ? charFormat.Revisions : (charFormat.OwnerBase is WTableRow ? (charFormat.OwnerBase as WTableRow).RowFormat.Revisions : ownerBase2?.RevisionsInternal);
          if (revisionList2 == null || revisionList2.Count <= 0)
            break;
          using (List<Revision>.Enumerator enumerator = revisionList2.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              Revision current = enumerator.Current;
              if (current.RevisionType == RevisionType.Insertions || current.RevisionType == RevisionType.MoveTo)
                sprms.SetIntValue(26629, charFormat.GetDTTMIntValue(current.Date));
              else if (current.RevisionType == RevisionType.Deletions || current.RevisionType == RevisionType.MoveFrom)
                sprms.SetIntValue(26724, charFormat.GetDTTMIntValue(current.Date));
            }
            break;
          }
        case 12:
        case 15:
          if (sprms.Contain(51849) || !charFormat.IsChangedFormat)
            break;
          byte[] dst1 = new byte[7];
          dst1[0] = (byte) 1;
          if (!CharacterPropertiesConverter.AuthorNames.Contains(charFormat.FormatChangeAuthorName))
            CharacterPropertiesConverter.AuthorNames.Add(charFormat.FormatChangeAuthorName);
          byte[] bytes1 = BitConverter.GetBytes((short) CharacterPropertiesConverter.AuthorNames.IndexOf(charFormat.FormatChangeAuthorName));
          byte[] src = new byte[4];
          if (charFormat.HasValue(15))
            src = BitConverter.GetBytes(charFormat.GetDTTMIntValue(charFormat.FormatChangeDateTime));
          Buffer.BlockCopy((Array) bytes1, 0, (Array) dst1, 1, 2);
          Buffer.BlockCopy((Array) src, 0, (Array) dst1, 3, 4);
          sprms.SetByteArrayValue(51799, dst1);
          sprms.SetByteArrayValue(51849, dst1);
          break;
        case 13:
          sprms.SetByteArrayValue(sprmOption, (value as CFELayout).GetCFELayoutBytes());
          break;
        case 14:
          if (charFormat.DoubleStrike)
            sprms.SetBoolValue(charFormat.GetSprmOption(6), false);
          byte num2;
          if (charFormat.IsDocReading)
          {
            try
            {
              num2 = CharacterPropertiesConverter.GetByteValue(value);
            }
            catch
            {
              num2 = charFormat.GetBoolComplexValue(propKey, charFormat.GetBoolPropertyValue((short) 14));
            }
          }
          else
            num2 = charFormat.GetBoolComplexValue(propKey, charFormat.GetBoolPropertyValue((short) 14));
          sprms.SetByteValue(sprmOption, num2);
          break;
        case 16 /*0x10*/:
        case 19:
          if (sprms.Contain(51830))
            break;
          byte[] dst2 = new byte[8];
          byte[] bytes2 = BitConverter.GetBytes(charFormat.FitTextWidth * 20);
          byte[] bytes3 = BitConverter.GetBytes(charFormat.FitTextID);
          Buffer.BlockCopy((Array) bytes2, 0, (Array) dst2, 0, 4);
          Buffer.BlockCopy((Array) bytes3, 0, (Array) dst2, 4, 4);
          sprms.SetByteArrayValue(51830, dst2);
          break;
        case 17:
          sprms.SetShortValue(sprmOption, (short) ((double) (float) value * 2.0));
          break;
        case 18:
          sprms.SetShortValue(sprmOption, (short) ((double) (float) value * 20.0));
          break;
        case 62:
          sprms.SetUShortValue(sprmOption, (ushort) ((double) (float) value * 2.0));
          break;
        case 63 /*0x3F*/:
          sprms.SetByteValue(sprmOption, (byte) WordColor.ConvertColorToId((Color) value));
          break;
        case 67:
          if (!charFormat.Border.IsBorderDefined)
            break;
          BorderCode destBorder1 = new BorderCode();
          ParagraphPropertiesConverter.ImportBorder(destBorder1, (Border) value);
          byte[] arr1 = new byte[4];
          destBorder1.SaveBytes(arr1, 0);
          sprms.SetByteArrayValue(sprmOption, arr1);
          BorderCode destBorder2 = new BorderCode();
          ParagraphPropertiesConverter.ImportBorder(destBorder2, (Border) value);
          byte[] arr2 = new byte[8];
          destBorder2.SaveNewBrc(arr2, 0);
          sprms.SetByteArrayValue(51826, arr2);
          break;
        case 72:
          sprms.SetByteValue(sprmOption, (byte) (FontHintType) value);
          break;
        case 73:
          sprms.SetShortValue(sprmOption, (short) value);
          sprms.SetShortValue(18547, (short) value);
          break;
        case 74:
          sprms.SetShortValue(sprmOption, (short) value);
          sprms.SetShortValue(18548, (short) value);
          break;
        case 75:
          sprms.SetShortValue(sprmOption, (short) value);
          break;
        case 79:
          sprms.SetByteValue(sprmOption, (byte) (EmphasisType) value);
          break;
        case 80 /*0x50*/:
          sprms.SetByteValue(sprmOption, (byte) (TextEffect) value);
          break;
        case 90:
          uint rgb2 = WordColor.ConvertColorToRGB((Color) value);
          sprms.SetUIntValue(26743, rgb2);
          break;
        case 91:
          if (value == null)
            break;
          short index = (short) styleSheet.StyleNameToIndex(value.ToString());
          if (index <= (short) -1)
            break;
          byte[] bytes4 = BitConverter.GetBytes(index);
          sprms.SetByteArrayValue(18992, bytes4);
          break;
        case 105:
          sprms.SetBoolValue(10883, (bool) value);
          break;
        case 107:
          sprms.SetIntValue(sprmOption, (int) value);
          break;
        case 108:
          sprms.SetBoolValue(sprmOption, (bool) value);
          break;
        case 125:
          sprms.SetUShortValue(sprmOption, (ushort) ((double) (float) value * 2.0));
          break;
        case 126:
          sprms.SetByteValue(sprmOption, (byte) (BreakClearType) value);
          break;
        case (int) sbyte.MaxValue:
          sprms.SetUShortValue(sprmOption, (ushort) (float) value);
          break;
      }
    }
  }

  private static byte GetByteValue(object value)
  {
    return value is ToggleOperand ? (byte) value : (byte) 0;
  }

  internal static void UpdateFontSprms(
    Dictionary<int, object> propertyHash,
    SinglePropertyModifierArray sprms,
    WCharacterFormat charFormat,
    WordStyleSheet styleSheet)
  {
    string fontName1 = charFormat.FontName;
    if (propertyHash.ContainsKey(2))
    {
      fontName1 = propertyHash[2] as string;
      CharacterPropertiesConverter.UpdateFontSprms(2, propertyHash[2], sprms, charFormat, styleSheet);
    }
    if (propertyHash.ContainsKey(68))
    {
      string fontName2 = propertyHash[68] as string;
      if (charFormat.IsThemeFont(fontName2))
        fontName2 = fontName1;
      CharacterPropertiesConverter.UpdateFontSprms(68, (object) fontName2, sprms, charFormat, styleSheet);
    }
    if (propertyHash.ContainsKey(69))
    {
      string fontName3 = propertyHash[69] as string;
      if (charFormat.IsThemeFont(fontName3))
        fontName3 = fontName1;
      CharacterPropertiesConverter.UpdateFontSprms(69, (object) fontName3, sprms, charFormat, styleSheet);
    }
    if (propertyHash.ContainsKey(70))
    {
      string fontName4 = propertyHash[70] as string;
      if (charFormat.IsThemeFont(fontName4))
        fontName4 = fontName1;
      CharacterPropertiesConverter.UpdateFontSprms(70, (object) fontName4, sprms, charFormat, styleSheet);
    }
    if (!propertyHash.ContainsKey(61))
      return;
    string fontName5 = propertyHash[61] as string;
    if (charFormat.IsThemeFont(fontName5))
      fontName5 = fontName1;
    CharacterPropertiesConverter.UpdateFontSprms(61, (object) fontName5, sprms, charFormat, styleSheet);
  }

  internal static void UpdateFontSprms(
    int propKey,
    object value,
    SinglePropertyModifierArray sprms,
    WCharacterFormat charFormat,
    WordStyleSheet styleSheet)
  {
    lock (CharacterPropertiesConverter.m_threadLocker)
    {
      charFormat.GetSprmOption(propKey);
      switch (propKey)
      {
        case 2:
          int index1 = styleSheet.FontNameToIndex((string) value);
          if (index1 >= 0)
          {
            sprms.SetUShortValue(19023, (ushort) index1);
            sprms.SetUShortValue(19024, (ushort) index1);
            sprms.SetUShortValue(19025, (ushort) index1);
            break;
          }
          ushort count = (ushort) styleSheet.FontNamesList.Count;
          sprms.SetUShortValue(19023, count);
          sprms.SetUShortValue(19024, count);
          sprms.SetUShortValue(19025, count);
          styleSheet.UpdateFontName((string) value);
          break;
        case 61:
          int index2 = styleSheet.FontNameToIndex((string) value);
          if (index2 >= 0)
          {
            sprms.SetUShortValue(19038, (ushort) index2);
            break;
          }
          sprms.SetUShortValue(19038, (ushort) styleSheet.FontNamesList.Count);
          styleSheet.UpdateFontName((string) value);
          break;
        case 68:
          int index3 = styleSheet.FontNameToIndex((string) value);
          if (index3 >= 0)
          {
            sprms.SetUShortValue(19023, (ushort) index3);
            break;
          }
          sprms.SetUShortValue(19023, (ushort) styleSheet.FontNamesList.Count);
          styleSheet.UpdateFontName((string) value);
          break;
        case 69:
          int index4 = styleSheet.FontNameToIndex((string) value);
          if (index4 >= 0)
          {
            sprms.SetUShortValue(19024, (ushort) index4);
            break;
          }
          sprms.SetUShortValue(19024, (ushort) styleSheet.FontNamesList.Count);
          styleSheet.UpdateFontName((string) value);
          break;
        case 70:
          int index5 = styleSheet.FontNameToIndex((string) value);
          if (index5 >= 0)
          {
            sprms.SetUShortValue(19025, (ushort) index5);
            break;
          }
          sprms.SetUShortValue(19025, (ushort) styleSheet.FontNamesList.Count);
          styleSheet.UpdateFontName((string) value);
          break;
      }
    }
  }

  internal static ShadingDescriptor GetShading(SinglePropertyModifierRecord record)
  {
    byte[] byteArray = record.ByteArray;
    ShadingDescriptor shading;
    if (byteArray.Length == 2)
    {
      shading = new ShadingDescriptor(record.ShortValue);
    }
    else
    {
      shading = new ShadingDescriptor();
      shading.ReadNewShd(byteArray, 0);
    }
    return shading;
  }

  internal static void Close()
  {
    if (CharacterPropertiesConverter.m_incorrectOptions != null)
    {
      CharacterPropertiesConverter.m_incorrectOptions.Clear();
      CharacterPropertiesConverter.m_incorrectOptions = (List<int>) null;
    }
    if (CharacterPropertiesConverter.m_authorNames == null)
      return;
    CharacterPropertiesConverter.m_authorNames.Clear();
    CharacterPropertiesConverter.m_authorNames = (List<string>) null;
  }
}
