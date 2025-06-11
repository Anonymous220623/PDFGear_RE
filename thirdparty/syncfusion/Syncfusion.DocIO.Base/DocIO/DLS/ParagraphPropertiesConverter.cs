// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ParagraphPropertiesConverter
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

internal class ParagraphPropertiesConverter
{
  private static readonly object m_threadLocker = new object();

  internal static void ExportBorder(BorderCode srcBorder, Border destBorder)
  {
    destBorder.BorderType = (BorderStyle) srcBorder.BorderType;
    destBorder.Color = srcBorder.LineColorExt;
    destBorder.LineWidth = (float) srcBorder.LineWidth / 8f;
    destBorder.Space = (float) srcBorder.Space;
    destBorder.Shadow = srcBorder.Shadow;
  }

  internal static void ExportBorder(
    SinglePropertyModifierArray sprms,
    SinglePropertyModifierRecord oldSprm,
    int newSprmOption,
    Border border,
    WParagraphFormat paragraphFormat)
  {
    if (paragraphFormat.IsFormattingChange)
    {
      if (sprms.GetOldSprm(newSprmOption, 9828) != null)
        return;
      ParagraphPropertiesConverter.ExportBorder(paragraphFormat.GetBorder(oldSprm), border);
    }
    else
    {
      if (sprms.GetNewSprm(newSprmOption, 9828) != null)
        return;
      ParagraphPropertiesConverter.ExportBorder(paragraphFormat.GetBorder(oldSprm), border);
    }
  }

  internal static void ExportBorder(
    SinglePropertyModifierRecord sprm,
    Border border,
    WParagraphFormat paragraphFormat)
  {
    ParagraphPropertiesConverter.ExportBorder(paragraphFormat.GetBorder(sprm), border);
  }

  internal static void ExportBorder(Border source, Border destBorder)
  {
    source.BorderType = destBorder.BorderType;
    source.Color = destBorder.Color;
    source.LineWidth = destBorder.LineWidth;
    source.Space = destBorder.Space;
    source.Shadow = destBorder.Shadow;
  }

  internal static void ImportBorder(BorderCode destBorder, Border srcBorder)
  {
    if (!srcBorder.IsDefault)
    {
      if (srcBorder.BorderType == BorderStyle.Cleared)
      {
        destBorder.LineColor = (byte) 0;
        destBorder.LineColorExt = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
        destBorder.BorderType = byte.MaxValue;
        destBorder.LineWidth = byte.MaxValue;
      }
      else
      {
        destBorder.LineColor = (byte) WordColor.ConvertColorToId(srcBorder.Color);
        destBorder.LineColorExt = srcBorder.Color;
        destBorder.BorderType = (byte) srcBorder.BorderType;
        destBorder.LineWidth = (byte) Math.Round((double) srcBorder.LineWidth * 8.0);
      }
      destBorder.Space = (byte) Math.Round((double) srcBorder.Space);
      destBorder.Shadow = srcBorder.Shadow;
    }
    else
    {
      destBorder.LineColor = (byte) 0;
      destBorder.LineColorExt = Color.Empty;
      destBorder.BorderType = (byte) 0;
      destBorder.LineWidth = (byte) 0;
      destBorder.Space = (byte) 0;
      destBorder.Shadow = false;
    }
  }

  internal static void ExportTabs(TabsInfo info, TabCollection tabs)
  {
    if (info.TabDeletePositions != null)
    {
      for (int index = 0; index < info.TabDeletePositions.Length; ++index)
        tabs.AddTab().DeletePosition = (float) info.TabDeletePositions[index];
    }
    for (int index = 0; index < (int) info.TabCount; ++index)
    {
      TabDescriptor tabDescriptor = info.Descriptors[index] ?? new TabDescriptor((byte) 0);
      Tab tab = tabs.AddTab();
      tab.Position = (float) info.TabPositions[index] / 20f;
      tab.Justification = tabDescriptor.Justification;
      tab.TabLeader = tabDescriptor.TabLeader;
    }
  }

  internal static void SprmsToFormat(
    SinglePropertyModifierArray sprms,
    WParagraphFormat paragraphFormat,
    Dictionary<int, string> authorNames,
    WordStyleSheet styleSheet)
  {
    if (sprms == null)
      return;
    lock (ParagraphPropertiesConverter.m_threadLocker)
    {
      if (sprms.Contain(9828))
        paragraphFormat.IsFormattingChange = true;
      Borders borders = (Borders) null;
      foreach (SinglePropertyModifierRecord sprm in sprms)
      {
        switch (sprm.Options)
        {
          case 9219:
            if (sprms[9313] == null)
            {
              paragraphFormat.UpdateJustification(sprms, sprm);
              continue;
            }
            continue;
          case 9221:
          case 9730:
            paragraphFormat.SetPropertyValue(6, (object) sprm.BoolValue);
            continue;
          case 9222:
            paragraphFormat.SetPropertyValue(10, (object) sprm.BoolValue);
            continue;
          case 9223:
            paragraphFormat.SetPropertyValue(12, (object) sprm.BoolValue);
            continue;
          case 9228:
            paragraphFormat.SuppressLineNumbers = sprm.BoolValue;
            continue;
          case 9251:
            paragraphFormat.WrapFrameAround = (FrameWrapMode) sprm.ByteValue;
            continue;
          case 9258:
            paragraphFormat.SuppressAutoHyphens = sprm.BoolValue;
            continue;
          case 9264:
            paragraphFormat.LockFrameAnchor = sprm.BoolValue;
            continue;
          case 9265:
            paragraphFormat.WidowControl = sprm.BoolValue;
            continue;
          case 9267:
            paragraphFormat.Kinsoku = sprm.BoolValue;
            continue;
          case 9268:
            paragraphFormat.WordWrap = sprm.BoolValue;
            continue;
          case 9269:
            paragraphFormat.OverflowPunctuation = sprm.BoolValue;
            continue;
          case 9270:
            paragraphFormat.TopLinePunctuation = sprm.BoolValue;
            continue;
          case 9271:
            paragraphFormat.AutoSpaceDE = sprm.BoolValue;
            continue;
          case 9272:
            paragraphFormat.AutoSpaceDN = sprm.BoolValue;
            continue;
          case 9281:
            paragraphFormat.UpdateBiDi(sprm.BoolValue);
            ParagraphPropertiesConverter.UpdateDirectParagraphFormatting(paragraphFormat, sprms);
            continue;
          case 9283:
            if (paragraphFormat.m_unParsedSprms == null)
              paragraphFormat.m_unParsedSprms = new SinglePropertyModifierArray();
            paragraphFormat.m_unParsedSprms.Add(sprm);
            continue;
          case 9287:
            paragraphFormat.SnapToGrid = sprm.BoolValue;
            continue;
          case 9288:
            paragraphFormat.AdjustRightIndent = sprm.BoolValue;
            continue;
          case 9306:
          case 9307:
            paragraphFormat.SpaceBeforeAuto = sprm.ByteValue == (byte) 1;
            continue;
          case 9308:
            paragraphFormat.SpaceAfterAuto = sprm.ByteValue == (byte) 1;
            continue;
          case 9313:
            paragraphFormat.LogicalJustification = (HorizontalAlignment) sprms[9313].ByteValue;
            continue;
          case 9314:
            paragraphFormat.SuppressOverlap = sprm.ByteValue != (byte) 1;
            continue;
          case 9325:
            paragraphFormat.ContextualSpacing = sprm.BoolValue;
            continue;
          case 9328:
            paragraphFormat.MirrorIndents = sprm.BoolValue;
            continue;
          case 9329:
            paragraphFormat.TextboxTightWrap = (TextboxTightWrapOptions) sprm.ByteValue;
            continue;
          case 9755:
            byte byteValue1 = sprm.ByteValue;
            byte num1 = (byte) ((int) byteValue1 >> 4 & 3);
            byte num2 = (byte) ((uint) byteValue1 >> 6);
            paragraphFormat.FrameVerticalPos = num1;
            paragraphFormat.FrameHorizontalPos = num2;
            continue;
          case 9792:
            paragraphFormat.OutlineLevel = (OutlineLevel) sprm.ByteValue;
            continue;
          case 9828:
            paragraphFormat.IsFormattingChange = false;
            paragraphFormat.IsChangedFormat = true;
            continue;
          case 17451:
            paragraphFormat.FrameHeight = (float) sprm.ShortValue / 20f;
            continue;
          case 17452:
            byte byteValue2 = sprm.ByteValue;
            paragraphFormat.DropCap = (DropCapType) ((int) byteValue2 & 7);
            paragraphFormat.DropCapLines = (int) byteValue2 >> 3;
            continue;
          case 17453:
            if (!sprms.Contain(50765))
            {
              ShadingDescriptor shading = CharacterPropertiesConverter.GetShading(sprm);
              paragraphFormat.BackColor = shading.BackColor;
              paragraphFormat.TextureStyle = shading.Pattern;
              paragraphFormat.ForeColor = shading.ForeColor;
              continue;
            }
            continue;
          case 17465:
            paragraphFormat.BaseLineAlignment = (BaseLineAlignment) sprm.ByteValue;
            continue;
          case 17466:
            paragraphFormat.TextDirection = (byte) ((uint) sprm.ByteValue & 7U);
            continue;
          case 17493:
            paragraphFormat.RightIndentChars = (float) sprm.ShortValue / 100f;
            continue;
          case 17494:
            paragraphFormat.LeftIndentChars = (float) sprm.ShortValue / 100f;
            continue;
          case 17495:
            paragraphFormat.FirstLineIndentChars = (float) sprm.ShortValue / 100f;
            continue;
          case 17496:
            paragraphFormat.BeforeLines = (float) sprm.ShortValue / 100f;
            continue;
          case 17497:
            paragraphFormat.AfterLines = (float) sprm.ShortValue / 100f;
            continue;
          case 17920:
            if (paragraphFormat.IsFormattingChange)
            {
              ushort ushortValue = sprm.UshortValue;
              if (styleSheet != null && styleSheet.StyleNames.ContainsKey((int) ushortValue))
              {
                paragraphFormat.SetPropertyValue(47, (object) styleSheet.StyleNames[(int) ushortValue]);
                continue;
              }
              continue;
            }
            continue;
          case 17936:
          case 33809:
          case 33888:
            paragraphFormat.SetPropertyValue(5, (object) (float) ((double) sprm.ShortValue / 20.0));
            continue;
          case 25618:
            LineSpacingDescriptor spacingDescriptor = new LineSpacingDescriptor();
            byte[] byteArray = sprm.ByteArray;
            if (byteArray != null)
              spacingDescriptor.Parse(byteArray);
            paragraphFormat.SetPropertyValue(52, (object) (float) ((double) spacingDescriptor.LineSpacing / 20.0));
            paragraphFormat.LineSpacingRule = spacingDescriptor.LineSpacingRule;
            continue;
          case 25636:
            paragraphFormat.UpdateOldFormatBorders(ref borders);
            borders = paragraphFormat.IsFormattingChange ? borders : paragraphFormat.Borders;
            ParagraphPropertiesConverter.ExportBorder(sprms, sprm, 50766, borders.Top, paragraphFormat);
            continue;
          case 25637:
            paragraphFormat.UpdateOldFormatBorders(ref borders);
            borders = paragraphFormat.IsFormattingChange ? borders : paragraphFormat.Borders;
            ParagraphPropertiesConverter.ExportBorder(sprms, sprm, 50767, borders.Left, paragraphFormat);
            continue;
          case 25638:
            paragraphFormat.UpdateOldFormatBorders(ref borders);
            borders = paragraphFormat.IsFormattingChange ? borders : paragraphFormat.Borders;
            ParagraphPropertiesConverter.ExportBorder(sprms, sprm, 50768, borders.Bottom, paragraphFormat);
            continue;
          case 25639:
            paragraphFormat.UpdateOldFormatBorders(ref borders);
            borders = paragraphFormat.IsFormattingChange ? borders : paragraphFormat.Borders;
            ParagraphPropertiesConverter.ExportBorder(sprms, sprm, 50769, borders.Right, paragraphFormat);
            continue;
          case 25640:
            paragraphFormat.UpdateOldFormatBorders(ref borders);
            borders = paragraphFormat.IsFormattingChange ? borders : paragraphFormat.Borders;
            ParagraphPropertiesConverter.ExportBorder(sprms, sprm, 50770, borders.Horizontal, paragraphFormat);
            continue;
          case 26153:
            paragraphFormat.UpdateOldFormatBorders(ref borders);
            borders = paragraphFormat.IsFormattingChange ? borders : paragraphFormat.Borders;
            ParagraphPropertiesConverter.ExportBorder(sprms, sprm, 50771, borders.Vertical, paragraphFormat);
            continue;
          case 33806:
            if (paragraphFormat.Document.WordVersion <= (ushort) 193)
            {
              paragraphFormat.SetPropertyValue(3, (object) (float) ((double) sprm.ShortValue / 20.0));
              continue;
            }
            if (paragraphFormat.Document.WordVersion <= (ushort) 217 && !sprms.Contain(33885))
            {
              if (paragraphFormat.Bidi || sprms.Contain(9281) && sprms[9281].BoolValue)
              {
                paragraphFormat.SetPropertyValue(2, (object) (float) ((double) sprm.ShortValue / 20.0));
                continue;
              }
              paragraphFormat.SetPropertyValue(3, (object) (float) ((double) sprm.ShortValue / 20.0));
              continue;
            }
            continue;
          case 33807:
            if (paragraphFormat.Document.WordVersion <= (ushort) 193)
            {
              paragraphFormat.SetPropertyValue(2, (object) (float) ((double) sprm.ShortValue / 20.0));
              continue;
            }
            if (paragraphFormat.Document.WordVersion <= (ushort) 217 && !sprms.Contain(33886))
            {
              if (paragraphFormat.Bidi || sprms.Contain(9281) && sprms[9281].BoolValue)
              {
                paragraphFormat.SetPropertyValue(3, (object) (float) ((double) sprm.ShortValue / 20.0));
                continue;
              }
              paragraphFormat.SetPropertyValue(2, (object) (float) ((double) sprm.ShortValue / 20.0));
              continue;
            }
            continue;
          case 33816:
            paragraphFormat.FrameX = paragraphFormat.IsFrameXAlign((float) sprm.ShortValue) ? (float) sprm.ShortValue : (float) sprm.ShortValue / 20f;
            continue;
          case 33817:
            paragraphFormat.FrameY = paragraphFormat.IsFrameYAlign((float) sprm.ShortValue) ? (float) sprm.ShortValue : (float) sprm.ShortValue / 20f;
            continue;
          case 33818:
            paragraphFormat.FrameWidth = (float) sprm.ShortValue / 20f;
            continue;
          case 33838:
            paragraphFormat.FrameVerticalDistanceFromText = (float) sprm.ShortValue / 20f;
            continue;
          case 33839:
            paragraphFormat.FrameHorizontalDistanceFromText = (float) sprm.ShortValue / 20f;
            continue;
          case 33885:
            paragraphFormat.SetPropertyValue(3, (object) (float) ((double) sprm.ShortValue / 20.0));
            continue;
          case 33886:
            paragraphFormat.SetPropertyValue(2, (object) (float) ((double) sprm.ShortValue / 20.0));
            continue;
          case 42003:
            if (sprm.ShortValue == (short) -1)
            {
              paragraphFormat.SetPropertyValue(8, (object) (float) ((double) sprm.ShortValue / 20.0));
              continue;
            }
            paragraphFormat.SetPropertyValue(8, (object) (float) ((double) sprm.UshortValue / 20.0));
            continue;
          case 42004:
            if (sprm.ShortValue == (short) -1)
            {
              paragraphFormat.SetPropertyValue(9, (object) (float) ((double) sprm.ShortValue / 20.0));
              continue;
            }
            paragraphFormat.SetPropertyValue(9, (object) (float) ((double) sprm.UshortValue / 20.0));
            continue;
          case 50701:
          case 50709:
            if (!paragraphFormat.HasValue(30))
            {
              paragraphFormat.UpdateTabs(sprms);
              continue;
            }
            continue;
          case 50757:
            if (paragraphFormat.m_unParsedSprms == null)
              paragraphFormat.m_unParsedSprms = new SinglePropertyModifierArray();
            paragraphFormat.m_unParsedSprms.Add(sprm);
            continue;
          case 50765:
            ShadingDescriptor shading1 = CharacterPropertiesConverter.GetShading(sprm);
            paragraphFormat.BackColor = shading1.BackColor;
            paragraphFormat.TextureStyle = shading1.Pattern;
            paragraphFormat.ForeColor = shading1.ForeColor;
            continue;
          case 50766:
            paragraphFormat.UpdateOldFormatBorders(ref borders);
            borders = paragraphFormat.IsFormattingChange ? borders : paragraphFormat.Borders;
            ParagraphPropertiesConverter.ExportBorder(sprm, borders.Top, paragraphFormat);
            if (borders.Top.BorderType == BorderStyle.Cleared)
            {
              borders.Top.PropertiesHash.Clear();
              continue;
            }
            continue;
          case 50767:
            paragraphFormat.UpdateOldFormatBorders(ref borders);
            borders = paragraphFormat.IsFormattingChange ? borders : paragraphFormat.Borders;
            ParagraphPropertiesConverter.ExportBorder(sprm, borders.Left, paragraphFormat);
            if (borders.Left.BorderType == BorderStyle.Cleared)
            {
              borders.Left.PropertiesHash.Clear();
              continue;
            }
            continue;
          case 50768:
            paragraphFormat.UpdateOldFormatBorders(ref borders);
            borders = paragraphFormat.IsFormattingChange ? borders : paragraphFormat.Borders;
            ParagraphPropertiesConverter.ExportBorder(sprm, borders.Bottom, paragraphFormat);
            if (borders.Bottom.BorderType == BorderStyle.Cleared)
            {
              borders.Bottom.PropertiesHash.Clear();
              continue;
            }
            continue;
          case 50769:
            paragraphFormat.UpdateOldFormatBorders(ref borders);
            borders = paragraphFormat.IsFormattingChange ? borders : paragraphFormat.Borders;
            ParagraphPropertiesConverter.ExportBorder(sprm, borders.Right, paragraphFormat);
            if (borders.Right.BorderType == BorderStyle.Cleared)
            {
              borders.Right.PropertiesHash.Clear();
              continue;
            }
            continue;
          case 50770:
            paragraphFormat.UpdateOldFormatBorders(ref borders);
            borders = paragraphFormat.IsFormattingChange ? borders : paragraphFormat.Borders;
            ParagraphPropertiesConverter.ExportBorder(sprm, borders.Horizontal, paragraphFormat);
            if (borders.Horizontal.BorderType == BorderStyle.Cleared)
            {
              borders.Horizontal.PropertiesHash.Clear();
              continue;
            }
            continue;
          case 50771:
            paragraphFormat.UpdateOldFormatBorders(ref borders);
            borders = paragraphFormat.IsFormattingChange ? borders : paragraphFormat.Borders;
            ParagraphPropertiesConverter.ExportBorder(sprm, borders.Vertical, paragraphFormat);
            if (borders.Vertical.BorderType == BorderStyle.Cleared)
            {
              borders.Vertical.PropertiesHash.Clear();
              continue;
            }
            continue;
          case 50799:
            short int16 = BitConverter.ToInt16(sprm.ByteArray, 1);
            if (authorNames != null && authorNames.Count > 0 && authorNames.ContainsKey((int) int16))
              paragraphFormat.FormatChangeAuthorName = authorNames[(int) int16];
            DateTime dateTime = paragraphFormat.ParseDTTM(BitConverter.ToInt32(sprm.ByteArray, 3));
            if (dateTime.Year < 1900)
              dateTime = new DateTime(1900, 1, 1, 0, 0, 0);
            paragraphFormat.FormatChangeDateTime = dateTime;
            continue;
          default:
            continue;
        }
      }
      if (sprms.Contain(9828) && sprms.GetNewSprm(17920, 9828) != null || paragraphFormat.OldPropertiesHash.Count <= 0 || paragraphFormat.PropertiesHash.Count <= 0)
        return;
      foreach (KeyValuePair<int, object> keyValuePair in paragraphFormat.OldPropertiesHash)
      {
        if (keyValuePair.Key == 20 && paragraphFormat.PropertiesHash.ContainsKey(20))
          ParagraphPropertiesConverter.CopyBorders((Borders) keyValuePair.Value, paragraphFormat);
        if (!paragraphFormat.PropertiesHash.ContainsKey(keyValuePair.Key))
          paragraphFormat.PropertiesHash.Add(keyValuePair.Key, keyValuePair.Value);
      }
    }
  }

  internal static void UpdateDirectParagraphFormatting(
    WParagraphFormat paragraphFormat,
    SinglePropertyModifierArray sprms)
  {
    if (sprms[9313] != null)
    {
      paragraphFormat.LogicalJustification = (HorizontalAlignment) sprms[9313].ByteValue;
    }
    else
    {
      if (sprms[9219] == null)
        return;
      paragraphFormat.UpdateJustification(sprms, sprms[9219]);
    }
  }

  internal static void CopyBorders(Borders borders, WParagraphFormat paragraphFormat)
  {
    Borders borders1 = paragraphFormat.Borders;
    if (borders.Top.IsBorderDefined && !borders1.Top.IsBorderDefined)
      ParagraphPropertiesConverter.ExportBorder(borders1.Top, borders.Top);
    if (borders.Bottom.IsBorderDefined && !borders1.Bottom.IsBorderDefined)
      ParagraphPropertiesConverter.ExportBorder(borders1.Bottom, borders.Bottom);
    if (borders.Right.IsBorderDefined && !borders1.Right.IsBorderDefined)
      ParagraphPropertiesConverter.ExportBorder(borders1.Right, borders.Right);
    if (borders.Left.IsBorderDefined && !borders1.Left.IsBorderDefined)
      ParagraphPropertiesConverter.ExportBorder(borders1.Left, borders.Left);
    if (borders.Horizontal.IsBorderDefined && !borders1.Horizontal.IsBorderDefined)
      ParagraphPropertiesConverter.ExportBorder(borders1.Horizontal, borders.Horizontal);
    if (!borders.Vertical.IsBorderDefined || borders1.Vertical.IsBorderDefined)
      return;
    ParagraphPropertiesConverter.ExportBorder(borders1.Vertical, borders.Vertical);
  }

  internal static void FormatToSprms(
    WParagraphFormat paragraphFormat,
    SinglePropertyModifierArray sprms,
    WordStyleSheet styleSheet)
  {
    lock (ParagraphPropertiesConverter.m_threadLocker)
    {
      sprms.Clear();
      Dictionary<int, object> propertyHash1 = new Dictionary<int, object>();
      if (paragraphFormat.PropertiesHash.Count > 0)
        propertyHash1 = new Dictionary<int, object>((IDictionary<int, object>) paragraphFormat.PropertiesHash);
      if (paragraphFormat.OldPropertiesHash.Count > 0)
      {
        Dictionary<int, object> propertyHash2 = new Dictionary<int, object>((IDictionary<int, object>) paragraphFormat.OldPropertiesHash);
        foreach (KeyValuePair<int, object> keyValuePair in propertyHash2)
        {
          ParagraphPropertiesConverter.FormatToSprms(keyValuePair.Key, keyValuePair.Value, sprms, paragraphFormat, styleSheet, propertyHash2);
          if (propertyHash1.ContainsKey(keyValuePair.Key) && propertyHash1[keyValuePair.Key] == keyValuePair.Value)
            propertyHash1.Remove(keyValuePair.Key);
        }
      }
      if (propertyHash1.ContainsKey(65))
      {
        ParagraphPropertiesConverter.FormatToSprms(65, propertyHash1[65], sprms, paragraphFormat, styleSheet, propertyHash1);
        propertyHash1.Remove(65);
      }
      if (propertyHash1.ContainsKey(47))
        propertyHash1.Remove(47);
      if (propertyHash1.Count <= 0)
        return;
      SinglePropertyModifierArray sprms1 = new SinglePropertyModifierArray();
      foreach (KeyValuePair<int, object> keyValuePair in propertyHash1)
        ParagraphPropertiesConverter.FormatToSprms(keyValuePair.Key, keyValuePair.Value, sprms1, paragraphFormat, styleSheet, propertyHash1);
      for (int sprmIndex = 0; sprmIndex < sprms1.Count; ++sprmIndex)
        sprms.Add(sprms1.GetSprmByIndex(sprmIndex).Clone());
      sprms1.Clear();
    }
  }

  internal static void FormatToSprms(
    int propKey,
    object value,
    SinglePropertyModifierArray sprms,
    WParagraphFormat paragraphFormat,
    WordStyleSheet styleSheet,
    Dictionary<int, object> propertyHash)
  {
    lock (ParagraphPropertiesConverter.m_threadLocker)
    {
      int sprmOption = paragraphFormat.GetSprmOption(propKey);
      switch (propKey)
      {
        case 0:
          sprms.SetByteValue(9313, (byte) value);
          break;
        case 2:
          short num1 = (short) Math.Round((double) (float) value * 20.0);
          sprms.SetShortValue(33886, num1);
          if (!paragraphFormat.Bidi)
          {
            sprms.SetShortValue(33807, num1);
            break;
          }
          sprms.SetShortValue(33806, num1);
          break;
        case 3:
          short num2 = (short) Math.Round((double) (float) value * 20.0);
          sprms.SetShortValue(33885, num2);
          if (!paragraphFormat.Bidi)
          {
            sprms.SetShortValue(33806, num2);
            break;
          }
          sprms.SetShortValue(33807, num2);
          break;
        case 5:
          short num3 = (short) Math.Round((double) (float) value * 20.0);
          sprms.SetShortValue(33809, num3);
          sprms.SetShortValue(33888, num3);
          break;
        case 6:
        case 10:
        case 11:
        case 12:
        case 31 /*0x1F*/:
        case 35:
        case 38:
        case 39:
        case 40:
        case 41:
        case 42:
        case 65:
        case 75:
        case 78:
        case 80 /*0x50*/:
        case 81:
        case 82:
        case 89:
        case 92:
          sprms.SetBoolValue(sprmOption, (bool) value);
          break;
        case 8:
        case 9:
          ushort num4 = (ushort) Math.Round((double) (float) value * 20.0);
          sprms.SetUShortValue(sprmOption, num4);
          break;
        case 20:
          ParagraphPropertiesConverter.ImportBorders((Borders) value, sprms);
          break;
        case 21:
        case 32 /*0x20*/:
        case 33:
          ParagraphPropertiesConverter.ImportShading(propertyHash, sprms);
          break;
        case 30:
          ParagraphPropertiesConverter.ImportTabs((TabCollection) value, sprms);
          break;
        case 34:
          sprms.SetUShortValue(sprmOption, (ushort) (BaseLineAlignment) value);
          break;
        case 36:
          byte num5 = (bool) value ? (byte) 0 : (byte) 1;
          sprms.SetByteValue(sprmOption, num5);
          break;
        case 37:
          sprms.SetByteValue(sprmOption, (byte) (TextboxTightWrapOptions) value);
          break;
        case 43:
        case 44:
          sprms.SetShortValue(sprmOption, (short) ((int) (byte) paragraphFormat.DropCapLines << 3 | (int) (byte) paragraphFormat.DropCap));
          break;
        case 45:
        case 46:
          if (sprms.Contain(50799) || !paragraphFormat.IsChangedFormat)
            break;
          byte[] dst = new byte[7];
          dst[0] = (byte) 1;
          if (!CharacterPropertiesConverter.AuthorNames.Contains(paragraphFormat.FormatChangeAuthorName))
            CharacterPropertiesConverter.AuthorNames.Add(paragraphFormat.FormatChangeAuthorName);
          byte[] bytes1 = BitConverter.GetBytes((short) CharacterPropertiesConverter.AuthorNames.IndexOf(paragraphFormat.FormatChangeAuthorName));
          byte[] bytes2 = BitConverter.GetBytes(paragraphFormat.GetDTTMIntValue(paragraphFormat.FormatChangeDateTime));
          Buffer.BlockCopy((Array) bytes1, 0, (Array) dst, 1, 2);
          Buffer.BlockCopy((Array) bytes2, 0, (Array) dst, 3, 4);
          sprms.SetByteArrayValue(50799, dst);
          break;
        case 47:
          int index = styleSheet.StyleNameToIndex((string) value, false);
          if (index <= -1)
            break;
          sprms.SetByteArrayValue(17920, BitConverter.GetBytes((ushort) index));
          break;
        case 48 /*0x30*/:
          byte[] bytes3 = BitConverter.GetBytes((short) (0 & 65528 | (int) (byte) value));
          sprms.SetByteArrayValue(sprmOption, bytes3);
          break;
        case 52:
          sprms.SetByteArrayValue(25618, new LineSpacingDescriptor()
          {
            LineSpacing = ((short) Math.Round((double) (float) value * 20.0)),
            LineSpacingRule = paragraphFormat.LineSpacingRule
          }.Save());
          break;
        case 54:
          byte num6 = (bool) value ? (byte) 1 : (byte) 0;
          sprms.SetByteValue(sprmOption, num6);
          if (num6 != (byte) 1)
            break;
          sprms.SetUShortValue(42003, (ushort) 100);
          break;
        case 55:
          byte num7 = (bool) value ? (byte) 1 : (byte) 0;
          sprms.SetByteValue(sprmOption, num7);
          if (num7 != (byte) 1)
            break;
          sprms.SetUShortValue(42004, (ushort) 100);
          break;
        case 56:
          sprms.SetByteValue(sprmOption, (byte) value);
          break;
        case 71:
        case 72:
          byte num8 = 3;
          if (paragraphFormat.HasValue(71))
            num8 = paragraphFormat.FrameHorizontalPos;
          byte num9 = 3;
          if (paragraphFormat.HasValue(72))
            num9 = paragraphFormat.FrameVerticalPos;
          if (sprms[9755] != null)
            break;
          sprms.SetByteValue(9755, (byte) (((int) num8 << 2 | (int) num9) << 4));
          break;
        case 73:
        case 74:
          if ((double) (float) value == Math.Floor((double) (float) value) && (propKey == 73 ? (paragraphFormat.IsFrameXAlign((float) value) ? 1 : 0) : (paragraphFormat.IsFrameYAlign((float) value) ? 1 : 0)) != 0)
          {
            sprms.SetShortValue(sprmOption, Convert.ToInt16(value));
            break;
          }
          sprms.SetShortValue(sprmOption, (short) (float) Math.Round((double) (float) value * 20.0));
          break;
        case 76:
        case 77:
        case 83:
        case 84:
          sprms.SetShortValue(sprmOption, (short) Math.Round((double) (float) value * 20.0));
          break;
        case 85:
          sprms.SetShortValue(sprmOption, (short) Math.Round((double) (float) value * 100.0));
          break;
        case 86:
          sprms.SetShortValue(sprmOption, (short) Math.Round((double) (float) value * 100.0));
          break;
        case 87:
          sprms.SetShortValue(sprmOption, (short) Math.Round((double) (float) value * 100.0));
          break;
        case 88:
          sprms.SetByteValue(sprmOption, (byte) (FrameWrapMode) value);
          break;
        case 90:
          sprms.SetShortValue(sprmOption, (short) Math.Round((double) (float) value * 100.0));
          break;
        case 91:
          sprms.SetShortValue(sprmOption, (short) Math.Round((double) (float) value * 100.0));
          break;
      }
    }
  }

  private static void ImportTabs(TabCollection tabCollection, SinglePropertyModifierArray sprms)
  {
    bool flag = false;
    int length = 0;
    int index1 = 0;
    int index2 = 0;
    for (int index3 = 0; index3 < tabCollection.Count; ++index3)
    {
      if ((double) tabCollection[index3].DeletePosition == 0.0)
        ++length;
    }
    TabsInfo tabsInfo = new TabsInfo((byte) length);
    short[] numArray = new short[(int) (byte) tabCollection.Count - length];
    int index4 = 0;
    for (int count = tabCollection.Count; index4 < count; ++index4)
    {
      Tab tab = tabCollection[index4];
      if ((double) tab.DeletePosition != 0.0)
      {
        flag = true;
        numArray[index1] = (short) tab.DeletePosition;
        ++index1;
      }
      else if (length > 0)
      {
        tabsInfo.TabPositions[index2] = (short) Math.Round((double) tab.Position * 20.0);
        tabsInfo.Descriptors[index2] = new TabDescriptor(tab.Justification, tab.TabLeader);
        ++index2;
      }
    }
    if (flag)
      tabsInfo.TabDeletePositions = numArray;
    tabsInfo.Save(sprms, 50701);
  }

  private static void ImportShading(
    Dictionary<int, object> propertyHash,
    SinglePropertyModifierArray sprms)
  {
    ShadingDescriptor shadingDescriptor = new ShadingDescriptor();
    if (propertyHash.ContainsKey(21))
      shadingDescriptor.BackColor = (Color) propertyHash[21];
    if (propertyHash.ContainsKey(32 /*0x20*/))
      shadingDescriptor.ForeColor = (Color) propertyHash[32 /*0x20*/];
    if (propertyHash.ContainsKey(33))
      shadingDescriptor.Pattern = (TextureStyle) propertyHash[33];
    sprms.SetShortValue(17453, shadingDescriptor.Save());
    sprms.SetByteArrayValue(50765, shadingDescriptor.SaveNewShd());
  }

  private static void ImportBorders(Borders borders, SinglePropertyModifierArray sprms)
  {
    if (borders.IsDefault)
      return;
    BorderCode borderCode = new BorderCode();
    if (!borders.Top.IsDefault && borders.Top.IsBorderDefined)
    {
      ParagraphPropertiesConverter.ImportBorder(borderCode, borders.Top);
      ParagraphPropertiesConverter.SetBorderSprms(25636, 50766, sprms, borderCode);
    }
    if (!borders.Left.IsDefault && borders.Left.IsBorderDefined)
    {
      ParagraphPropertiesConverter.ImportBorder(borderCode, borders.Left);
      ParagraphPropertiesConverter.SetBorderSprms(25637, 50767, sprms, borderCode);
    }
    if (!borders.Bottom.IsDefault && borders.Bottom.IsBorderDefined)
    {
      ParagraphPropertiesConverter.ImportBorder(borderCode, borders.Bottom);
      ParagraphPropertiesConverter.SetBorderSprms(25638, 50768, sprms, borderCode);
    }
    if (!borders.Right.IsDefault && borders.Right.IsBorderDefined)
    {
      ParagraphPropertiesConverter.ImportBorder(borderCode, borders.Right);
      ParagraphPropertiesConverter.SetBorderSprms(25639, 50769, sprms, borderCode);
    }
    if (!borders.Horizontal.IsDefault && borders.Horizontal.IsBorderDefined)
    {
      ParagraphPropertiesConverter.ImportBorder(borderCode, borders.Horizontal);
      ParagraphPropertiesConverter.SetBorderSprms(25640, 50770, sprms, borderCode);
    }
    if (borders.Vertical.IsDefault || !borders.Vertical.IsBorderDefined)
      return;
    ParagraphPropertiesConverter.ImportBorder(borderCode, borders.Vertical);
    ParagraphPropertiesConverter.SetBorderSprms(26153, 50771, sprms, borderCode);
  }

  private static void SetBorderSprms(
    int oldSprmOPtion,
    int newSprmOption,
    SinglePropertyModifierArray sprms,
    BorderCode brc)
  {
    byte[] arr1 = new byte[4];
    brc.SaveBytes(arr1, 0);
    sprms.SetByteArrayValue(oldSprmOPtion, arr1);
    byte[] arr2 = new byte[8];
    brc.SaveNewBrc(arr2, 0);
    sprms.SetByteArrayValue(newSprmOption, arr2);
  }
}
