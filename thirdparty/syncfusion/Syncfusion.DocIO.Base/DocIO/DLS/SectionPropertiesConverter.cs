// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.SectionPropertiesConverter
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

internal class SectionPropertiesConverter
{
  public static void Export(WordReader reader, WSection destination, bool parseAll)
  {
    SectionProperties sectionProperties = reader.SectionProperties;
    List<SinglePropertyModifierRecord> modifiers = sectionProperties.Sprms.Modifiers;
    WPageSetup pageSetup = destination.PageSetup;
    List<SinglePropertyModifierRecord> propertyModifierRecordList = new List<SinglePropertyModifierRecord>();
    SectionPropertiesConverter.UpdatePageOrientation(sectionProperties.Sprms, pageSetup);
    if (sectionProperties.Sprms.Contain(12857))
    {
      destination.SectionFormat.IsFormattingChange = true;
      destination.PageSetup.IsFormattingChange = true;
      destination.PageSetup.PageNumbers.IsFormattingChange = true;
      destination.PageSetup.Margins.IsFormattingChange = true;
      destination.PageSetup.Borders.IsFormattingChange = true;
      destination.Document.SetDefaultSectionFormatting(destination);
      for (int index = sectionProperties.Sprms.Modifiers.Count - 1; index > 0; --index)
      {
        SinglePropertyModifierRecord modifier = sectionProperties.Sprms.Modifiers[index];
        int typedOptions = modifier.TypedOptions;
        if (modifier.TypedOptions != 12857)
        {
          propertyModifierRecordList.Add(modifier);
          sectionProperties.Sprms.Modifiers.Remove(modifier);
        }
        else
          break;
      }
      destination.PageSetup.PageNumberStyle = (PageNumberStyle) sectionProperties.PageNfc;
      destination.PageSetup.RestartPageNumbering = sectionProperties.PageRestart;
      destination.PageSetup.PageNumbers.ChapterPageSeparator = (ChapterPageSeparatorType) sectionProperties.ChapterPageSeparator;
      destination.PageSetup.PageNumbers.HeadingLevelForChapter = (HeadingLevel) sectionProperties.HeadingLevelForChapter;
      if (sectionProperties.PageRestart)
        destination.PageSetup.PageStartingNumber = (int) sectionProperties.PageStartAt;
    }
    SectionPropertiesConverter.ImportSecSprmToFormat(sectionProperties, modifiers, destination, reader.SttbfRMarkAuthorNames, pageSetup);
    if (propertyModifierRecordList.Count > 0)
    {
      sectionProperties.Sprms.Modifiers.AddRange((IEnumerable<SinglePropertyModifierRecord>) propertyModifierRecordList);
      destination.Document.SetDefaultSectionFormatting(destination);
      SectionPropertiesConverter.ImportSecSprmToFormat(sectionProperties, propertyModifierRecordList, destination, reader.SttbfRMarkAuthorNames, pageSetup);
    }
    destination.PageSetup.EqualColumnWidth = sectionProperties.Columns.ColumnsEvenlySpaced;
    if (sectionProperties.Sprms.Contain(12857))
    {
      destination.PageSetup.IsFormattingChange = true;
      destination.PageSetup.EqualColumnWidth = sectionProperties.OldColumns.ColumnsEvenlySpaced;
      destination.PageSetup.IsFormattingChange = false;
    }
    destination.PageSetup.PageNumberStyle = (PageNumberStyle) sectionProperties.PageNfc;
    destination.PageSetup.RestartPageNumbering = sectionProperties.PageRestart;
    destination.PageSetup.PageNumbers.ChapterPageSeparator = (ChapterPageSeparatorType) sectionProperties.ChapterPageSeparator;
    destination.PageSetup.PageNumbers.HeadingLevelForChapter = (HeadingLevel) sectionProperties.HeadingLevelForChapter;
    if (sectionProperties.PageRestart)
      destination.PageSetup.PageStartingNumber = (int) sectionProperties.PageStartAt;
    int index1 = 0;
    for (int count = sectionProperties.Columns.Count; index1 < count; ++index1)
    {
      ColumnDescriptor column1 = sectionProperties.Columns[index1];
      if (column1.Width != (ushort) 0)
      {
        Column column2 = destination.AddColumn(0.0f, 0.0f, true);
        column2.Width = (float) column1.Width / 20f;
        column2.Space = (float) column1.Space / 20f;
      }
    }
    destination.SectionFormat.SectFormattingColumnCollection = new ColumnCollection(destination);
    int index2 = 0;
    for (int count = sectionProperties.OldColumns.Count; index2 < count; ++index2)
    {
      ColumnDescriptor oldColumn = sectionProperties.OldColumns[index2];
      if (oldColumn.Width != (ushort) 0)
      {
        Column column = new Column((IWordDocument) destination.Document);
        column.IsFormattingChange = true;
        column.Width = 0.0f;
        column.Space = 0.0f;
        destination.SectionFormat.SectFormattingColumnCollection.Add(column, true);
        column.Width = (float) oldColumn.Width / 20f;
        column.Space = (float) oldColumn.Space / 20f;
        column.IsFormattingChange = false;
      }
    }
    if (!parseAll)
      return;
    SinglePropertyModifierArray copiableSprm = sectionProperties.GetCopiableSprm();
    destination.DataArray = new byte[copiableSprm.Length];
    copiableSprm.Save(destination.DataArray, 0);
  }

  private static void ImportSecSprmToFormat(
    SectionProperties source,
    List<SinglePropertyModifierRecord> modifiers,
    WSection destination,
    Dictionary<int, string> authorNames,
    WPageSetup pageSetup)
  {
    Borders destBorders = (Borders) null;
    int count = modifiers.Count;
    for (int index = 0; index < count; ++index)
    {
      SinglePropertyModifierRecord modifier = modifiers[index];
      switch (modifier.TypedOptions)
      {
        case 12294:
          destination.ProtectForm = !source.ProtectForm;
          break;
        case 12297:
          destination.BreakCode = (SectionBreakCode) source.BreakCode;
          break;
        case 12298:
          pageSetup.DifferentFirstPage = source.TitlePage;
          break;
        case 12307:
          bool flag = true;
          if (source.HasOptions(20501) && source.LineNumberingMode != LineNumberingMode.None)
          {
            pageSetup.SetPageSetupProperty("LineNumberingMode", (object) source.LineNumberingMode);
            if (source.Sprms.Contain(12857))
            {
              if (source.LineNumberingStep != (ushort) 0)
                pageSetup.SetPageSetupProperty("LineNumberingStep", (object) (int) source.LineNumberingStep);
              if (source.LineNumberingStartValue != (short) 0)
                pageSetup.SetPageSetupProperty("LineNumberingStartValue", (object) (int) source.LineNumberingStartValue);
              if (source.LineNumberingDistanceFromText != (short) 0)
                pageSetup.SetPageSetupProperty("LineNumberingDistanceFromText", (object) (float) ((double) source.LineNumberingDistanceFromText / 20.0));
              flag = false;
            }
          }
          if (flag)
          {
            if (source.LineNumberingStep != (ushort) 0)
              pageSetup.SetPageSetupProperty("LineNumberingStep", (object) (int) source.LineNumberingStep);
            if (source.LineNumberingStartValue != (short) 0)
              pageSetup.SetPageSetupProperty("LineNumberingStartValue", (object) (int) source.LineNumberingStartValue);
            if (source.LineNumberingDistanceFromText != (short) 0)
            {
              pageSetup.SetPageSetupProperty("LineNumberingDistanceFromText", (object) (float) ((double) source.LineNumberingDistanceFromText / 20.0));
              break;
            }
            break;
          }
          break;
        case 12313:
          pageSetup.DrawLinesBetweenCols = source.DrawLinesBetweenCols;
          break;
        case 12314:
          pageSetup.VerticalAlignment = (PageAlignment) source.VerticalAlignment;
          break;
        case 12317:
          pageSetup.Orientation = (PageOrientation) source.Orientation;
          break;
        case 12347:
          destination.PageSetup.FootnotePosition = (FootnotePosition) source.FootnotePosition;
          break;
        case 12348:
          destination.PageSetup.RestartIndexForFootnotes = (FootnoteRestartIndex) source.RestartIndexForFootnotes;
          break;
        case 12350:
          destination.PageSetup.RestartIndexForEndnote = (EndnoteRestartIndex) source.RestartIndexForEndnote;
          break;
        case 12840:
          pageSetup.Bidi = source.Bidi;
          break;
        case 12857:
          destination.SectionFormat.IsFormattingChange = false;
          destination.PageSetup.IsFormattingChange = false;
          destination.PageSetup.PageNumbers.IsFormattingChange = false;
          destination.PageSetup.Margins.IsFormattingChange = false;
          destination.PageSetup.Borders.IsFormattingChange = false;
          destination.SectionFormat.IsChangedFormat = true;
          break;
        case 20487:
          pageSetup.FirstPageTray = (PrinterPaperTray) source.FirstPageTray;
          break;
        case 20488:
          pageSetup.OtherPagesTray = (PrinterPaperTray) source.OtherPagesTray;
          break;
        case 20501:
          pageSetup.SetPageSetupProperty("LineNumberingStep", (object) (int) source.LineNumberingStep);
          break;
        case 20530:
          pageSetup.PitchType = (GridPitchType) source.PitchType;
          break;
        case 20531:
          destination.TextDirection = (DocTextDirection) source.TextDirection;
          break;
        case 20543:
          destination.PageSetup.InitialFootnoteNumber = (int) source.InitialFootnoteNumber;
          break;
        case 20544:
          destination.PageSetup.FootnoteNumberFormat = destination.Document.WordVersion > (ushort) 217 ? (FootEndNoteNumberFormat) source.FootnoteNumberFormat : destination.Document.FootnoteNumberFormat;
          break;
        case 20545:
          destination.PageSetup.InitialEndnoteNumber = (int) source.InitialEndnoteNumber;
          break;
        case 20546:
          destination.PageSetup.EndnoteNumberFormat = destination.Document.WordVersion > (ushort) 217 ? (FootEndNoteNumberFormat) source.EndnoteNumberFormat : destination.Document.EndnoteNumberFormat;
          break;
        case 21039:
          pageSetup.PageBordersApplyType = source.PageBorderApply;
          pageSetup.IsFrontPageBorder = source.PageBorderIsInFront;
          pageSetup.PageBorderOffsetFrom = source.PageBorderOffsetFrom;
          break;
        case 28715:
          destBorders = SectionPropertiesConverter.GetDestBorders(destBorders, pageSetup);
          SectionPropertiesConverter.ExportBorder(source.GetBorder(modifier), destBorders.Top);
          break;
        case 28716:
          destBorders = SectionPropertiesConverter.GetDestBorders(destBorders, pageSetup);
          SectionPropertiesConverter.ExportBorder(source.GetBorder(modifier), destBorders.Left);
          break;
        case 28717:
          destBorders = SectionPropertiesConverter.GetDestBorders(destBorders, pageSetup);
          SectionPropertiesConverter.ExportBorder(source.GetBorder(modifier), destBorders.Bottom);
          break;
        case 28718:
          destBorders = SectionPropertiesConverter.GetDestBorders(destBorders, pageSetup);
          SectionPropertiesConverter.ExportBorder(source.GetBorder(modifier), destBorders.Right);
          break;
        case 36899:
          pageSetup.Margins.Top = (float) source.TopMargin / 20f;
          break;
        case 36900:
          pageSetup.Margins.Bottom = (float) source.BottomMargin / 20f;
          break;
        case 36913:
          pageSetup.LinePitch = (float) source.LinePitch / 20f;
          break;
        case 45079:
          pageSetup.SetPageSetupProperty("HeaderDistance", (object) (float) ((double) source.HeaderHeight / 20.0));
          break;
        case 45080:
          pageSetup.SetPageSetupProperty("FooterDistance", (object) (float) ((double) source.FooterHeight / 20.0));
          break;
        case 45087:
          pageSetup.PageSize = new SizeF((float) source.PageWidth / 20f, pageSetup.IsFormattingChange ? (pageSetup.OldPropertiesHash.ContainsKey(5) ? ((SizeF) pageSetup.OldPropertiesHash[5]).Height : pageSetup.PageSize.Height) : pageSetup.PageSize.Height);
          break;
        case 45088:
          pageSetup.PageSize = new SizeF(pageSetup.IsFormattingChange ? (pageSetup.OldPropertiesHash.ContainsKey(5) ? ((SizeF) pageSetup.OldPropertiesHash[5]).Width : pageSetup.PageSize.Width) : pageSetup.PageSize.Width, (float) source.PageHeight / 20f);
          break;
        case 45089:
          pageSetup.Margins.Left = (float) source.LeftMargin / 20f;
          break;
        case 45090:
          pageSetup.Margins.Right = (float) source.RightMargin / 20f;
          break;
        case 45093:
          pageSetup.Margins.Gutter = (float) source.Gutter / 20f;
          break;
        case 53812:
          destBorders = SectionPropertiesConverter.GetDestBorders(destBorders, pageSetup);
          SectionPropertiesConverter.ExportBorder(source.GetBorder(modifier), destBorders.Top);
          break;
        case 53813:
          destBorders = SectionPropertiesConverter.GetDestBorders(destBorders, pageSetup);
          SectionPropertiesConverter.ExportBorder(source.GetBorder(modifier), destBorders.Left);
          break;
        case 53814:
          destBorders = SectionPropertiesConverter.GetDestBorders(destBorders, pageSetup);
          SectionPropertiesConverter.ExportBorder(source.GetBorder(modifier), destBorders.Bottom);
          break;
        case 53815:
          destBorders = SectionPropertiesConverter.GetDestBorders(destBorders, pageSetup);
          SectionPropertiesConverter.ExportBorder(source.GetBorder(modifier), destBorders.Right);
          break;
        case 53827:
          short int16 = BitConverter.ToInt16(modifier.ByteArray, 1);
          if (authorNames != null && authorNames.Count > 0 && authorNames.ContainsKey((int) int16))
            destination.SectionFormat.FormatChangeAuthorName = authorNames[(int) int16];
          DateTime dateTime = destination.SectionFormat.ParseDTTM(BitConverter.ToInt32(modifier.ByteArray, 3));
          if (dateTime.Year < 1900)
            dateTime = new DateTime(1900, 1, 1, 0, 0, 0);
          destination.SectionFormat.FormatChangeDateTime = dateTime;
          break;
      }
    }
  }

  private static void UpdatePageOrientation(SinglePropertyModifierArray sprms, WPageSetup pageSetup)
  {
    if (sprms == null || sprms.Contain(12317) || pageSetup.Orientation == PageOrientation.Portrait)
      return;
    pageSetup.Orientation = PageOrientation.Portrait;
  }

  public static void Import(SectionProperties destination, WSection source)
  {
    List<SinglePropertyModifierRecord> collection = new List<SinglePropertyModifierRecord>();
    if (source.SectionFormat.IsChangedFormat)
    {
      SectionPropertiesConverter.OldFormatToSprms(destination, source);
      for (int index = 0; index < destination.Sprms.Modifiers.Count - 1; index = index - 1 + 1)
      {
        SinglePropertyModifierRecord modifier = destination.Sprms.Modifiers[index];
        int typedOptions = modifier.TypedOptions;
        if (modifier.TypedOptions == 12857)
        {
          collection.Add(modifier);
          destination.Sprms.Modifiers.Remove(modifier);
          break;
        }
        collection.Add(modifier);
        destination.Sprms.Modifiers.Remove(modifier);
      }
    }
    SectionPropertiesConverter.Import(destination, source, source.SectionFormat.IsChangedFormat);
    if (collection.Count <= 0)
      return;
    collection.AddRange((IEnumerable<SinglePropertyModifierRecord>) destination.Sprms.Modifiers);
    destination.Sprms.Modifiers.Clear();
    destination.Sprms.Modifiers.AddRange((IEnumerable<SinglePropertyModifierRecord>) collection);
  }

  private static void OldFormatToSprms(SectionProperties destination, WSection source)
  {
    Dictionary<int, object> dictionary1 = new Dictionary<int, object>((IDictionary<int, object>) source.SectionFormat.OldPropertiesHash);
    Dictionary<int, object> dictionary2 = new Dictionary<int, object>((IDictionary<int, object>) source.PageSetup.OldPropertiesHash);
    Dictionary<int, object> dictionary3 = new Dictionary<int, object>((IDictionary<int, object>) source.PageSetup.PageNumbers.OldPropertiesHash);
    Dictionary<int, object> dictionary4 = new Dictionary<int, object>((IDictionary<int, object>) source.PageSetup.Margins.OldPropertiesHash);
    Dictionary<int, object> dictionary5 = new Dictionary<int, object>((IDictionary<int, object>) source.SectionFormat.PropertiesHash);
    Dictionary<int, object> dictionary6 = new Dictionary<int, object>((IDictionary<int, object>) source.PageSetup.PropertiesHash);
    Dictionary<int, object> dictionary7 = new Dictionary<int, object>((IDictionary<int, object>) source.PageSetup.PageNumbers.PropertiesHash);
    Dictionary<int, object> dictionary8 = new Dictionary<int, object>((IDictionary<int, object>) source.PageSetup.Margins.PropertiesHash);
    ColumnCollection columnCollection = new ColumnCollection(source);
    if (source.SectionFormat.SectFormattingColumnCollection != null)
    {
      for (int index = 0; index < source.Columns.Count; ++index)
        columnCollection.Add(source.Columns[index]);
      if (source.Columns.Count > 0)
        source.Columns.InnerList.Clear();
      for (int index = 0; index < source.SectionFormat.SectFormattingColumnCollection.Count; ++index)
      {
        Column column = new Column((IWordDocument) source.Document);
        source.Columns.Add(column);
        foreach (KeyValuePair<int, object> keyValuePair in source.SectionFormat.SectFormattingColumnCollection[index].OldPropertiesHash)
          source.Columns[index].PropertiesHash[keyValuePair.Key] = keyValuePair.Value;
      }
    }
    source.SectionFormat.PropertiesHash.Clear();
    source.SectionFormat.OldPropertiesHash.Clear();
    source.PageSetup.OldPropertiesHash.Clear();
    source.PageSetup.PropertiesHash.Clear();
    source.PageSetup.PageNumbers.OldPropertiesHash.Clear();
    source.PageSetup.PageNumbers.PropertiesHash.Clear();
    source.PageSetup.Margins.OldPropertiesHash.Clear();
    source.PageSetup.Margins.PropertiesHash.Clear();
    foreach (KeyValuePair<int, object> keyValuePair in dictionary1)
      source.SectionFormat.PropertiesHash[keyValuePair.Key] = keyValuePair.Value;
    foreach (KeyValuePair<int, object> keyValuePair in dictionary2)
      source.PageSetup.PropertiesHash[keyValuePair.Key] = keyValuePair.Value;
    foreach (KeyValuePair<int, object> keyValuePair in dictionary3)
      source.PageSetup.PageNumbers.PropertiesHash[keyValuePair.Key] = keyValuePair.Value;
    foreach (KeyValuePair<int, object> keyValuePair in dictionary4)
      source.PageSetup.Margins.PropertiesHash[keyValuePair.Key] = keyValuePair.Value;
    source.SectionFormat.IsChangedFormat = true;
    SectionPropertiesConverter.Import(destination, source, source.SectionFormat.IsChangedFormat);
    if (source.Columns.Count > 0)
      source.Columns.InnerList.Clear();
    for (int index = 0; index < columnCollection.Count; ++index)
      source.Columns.Add(columnCollection[index]);
    columnCollection.Close();
    source.SectionFormat.PropertiesHash.Clear();
    source.PageSetup.PropertiesHash.Clear();
    source.PageSetup.PageNumbers.PropertiesHash.Clear();
    source.PageSetup.Margins.PropertiesHash.Clear();
    foreach (KeyValuePair<int, object> keyValuePair in dictionary1)
      source.SectionFormat.OldPropertiesHash[keyValuePair.Key] = keyValuePair.Value;
    foreach (KeyValuePair<int, object> keyValuePair in dictionary5)
      source.SectionFormat.PropertiesHash[keyValuePair.Key] = keyValuePair.Value;
    foreach (KeyValuePair<int, object> keyValuePair in dictionary2)
      source.PageSetup.OldPropertiesHash[keyValuePair.Key] = keyValuePair.Value;
    foreach (KeyValuePair<int, object> keyValuePair in dictionary6)
      source.PageSetup.PropertiesHash[keyValuePair.Key] = keyValuePair.Value;
    foreach (KeyValuePair<int, object> keyValuePair in dictionary3)
      source.PageSetup.PageNumbers.OldPropertiesHash[keyValuePair.Key] = keyValuePair.Value;
    foreach (KeyValuePair<int, object> keyValuePair in dictionary7)
      source.PageSetup.PageNumbers.PropertiesHash[keyValuePair.Key] = keyValuePair.Value;
    foreach (KeyValuePair<int, object> keyValuePair in dictionary4)
      source.PageSetup.Margins.OldPropertiesHash[keyValuePair.Key] = keyValuePair.Value;
    foreach (KeyValuePair<int, object> keyValuePair in dictionary8)
      source.PageSetup.Margins.PropertiesHash[keyValuePair.Key] = keyValuePair.Value;
    destination.IsChangedFormat = true;
    source.SectionFormat.PropertiesHash.Remove(4);
  }

  internal static void Import(SectionProperties destination, WSection source, bool format)
  {
    if ((double) source.PageSetup.PageSize.Width != 0.0)
      destination.PageWidth = (ushort) Math.Round((double) source.PageSetup.PageSize.Width * 20.0);
    if ((double) source.PageSetup.PageSize.Height != 0.0)
      destination.PageHeight = (ushort) Math.Round((double) source.PageSetup.PageSize.Height * 20.0);
    if (source.PageSetup.VerticalAlignment != PageAlignment.Top)
      destination.VerticalAlignment = (byte) source.PageSetup.VerticalAlignment;
    if (source.PageSetup.Orientation != PageOrientation.Portrait)
      destination.Orientation = (byte) source.PageSetup.Orientation;
    destination.TextDirection = (byte) source.TextDirection;
    destination.LeftMargin = (short) Math.Round((double) source.PageSetup.Margins.Left * 20.0);
    destination.RightMargin = (short) Math.Round((double) source.PageSetup.Margins.Right * 20.0);
    destination.TopMargin = (short) Math.Round((double) source.PageSetup.Margins.Top * 20.0);
    destination.BottomMargin = (short) Math.Round((double) source.PageSetup.Margins.Bottom * 20.0);
    destination.Gutter = (short) Math.Round((double) source.PageSetup.Margins.Gutter * 20.0);
    destination.HeaderHeight = (short) Math.Round((double) source.PageSetup.HeaderDistance * 20.0);
    destination.FooterHeight = (short) Math.Round((double) source.PageSetup.FooterDistance * 20.0);
    if (source.PageSetup.FirstPageTray > PrinterPaperTray.DefaultBin)
      destination.FirstPageTray = (ushort) source.PageSetup.FirstPageTray;
    if (source.PageSetup.OtherPagesTray > PrinterPaperTray.DefaultBin)
      destination.OtherPagesTray = (ushort) source.PageSetup.OtherPagesTray;
    if (source.SectionFormat.IsChangedFormat)
    {
      byte[] dst = new byte[7];
      dst[0] = (byte) 1;
      if (source.SectionFormat.HasKey(5))
        CharacterPropertiesConverter.AuthorNames.Add(source.SectionFormat.FormatChangeAuthorName);
      byte[] bytes = BitConverter.GetBytes((short) CharacterPropertiesConverter.AuthorNames.IndexOf(source.SectionFormat.FormatChangeAuthorName));
      byte[] src = new byte[4];
      if (source.SectionFormat.HasKey(6))
        src = BitConverter.GetBytes(source.SectionFormat.GetDTTMIntValue(source.SectionFormat.FormatChangeDateTime));
      Buffer.BlockCopy((Array) bytes, 0, (Array) dst, 1, 2);
      Buffer.BlockCopy((Array) src, 0, (Array) dst, 3, 4);
      destination.Sprms.SetByteArrayValue(53827, dst);
    }
    if (source.PageSetup.DifferentFirstPage)
      destination.TitlePage = source.PageSetup.DifferentFirstPage;
    if (source.PageSetup.LineNumberingStep != 0)
    {
      bool flag = true;
      if (source.PageSetup.LineNumberingMode != LineNumberingMode.None)
      {
        destination.LineNumberingMode = source.PageSetup.LineNumberingMode;
        if (destination.Sprms.Contain(12857))
        {
          if (source.PageSetup.LineNumberingStep != 0)
            destination.LineNumberingStep = (ushort) source.PageSetup.LineNumberingStep;
          if (source.PageSetup.LineNumberingStartValue != 0)
            destination.LineNumberingStartValue = (short) source.PageSetup.LineNumberingStartValue;
          if ((double) source.PageSetup.LineNumberingDistanceFromText != 0.0)
            destination.LineNumberingDistanceFromText = (short) Math.Round((double) source.PageSetup.LineNumberingDistanceFromText * 20.0);
          flag = false;
        }
      }
      if (flag)
      {
        if (source.PageSetup.LineNumberingStep != 0)
          destination.LineNumberingStep = (ushort) source.PageSetup.LineNumberingStep;
        if (source.PageSetup.LineNumberingStartValue != 0)
          destination.LineNumberingStartValue = (short) source.PageSetup.LineNumberingStartValue;
        if ((double) source.PageSetup.LineNumberingDistanceFromText != 0.0)
          destination.LineNumberingDistanceFromText = (short) Math.Round((double) source.PageSetup.LineNumberingDistanceFromText * 20.0);
      }
    }
    if (source.PageSetup.Bidi)
      destination.Bidi = source.PageSetup.Bidi;
    if (!source.PageSetup.Borders.IsDefault)
    {
      Borders borders = source.PageSetup.Borders;
      BorderCode destBorder = new BorderCode();
      if (borders.Top.IsBorderDefined)
      {
        SectionPropertiesConverter.ImportBorder(destBorder, borders.Top);
        destination.TopBorder = destBorder;
        destination.TopBorderNew = destBorder;
      }
      if (borders.Left.IsBorderDefined)
      {
        SectionPropertiesConverter.ImportBorder(destBorder, borders.Left);
        destination.LeftBorder = destBorder;
        destination.LeftBorderNew = destBorder;
      }
      if (borders.Bottom.IsBorderDefined)
      {
        SectionPropertiesConverter.ImportBorder(destBorder, borders.Bottom);
        destination.BottomBorder = destBorder;
        destination.BottomBorderNew = destBorder;
      }
      if (borders.Right.IsBorderDefined)
      {
        SectionPropertiesConverter.ImportBorder(destBorder, borders.Right);
        destination.RightBorder = destBorder;
        destination.RightBorderNew = destBorder;
      }
    }
    destination.PageBorderApply = source.PageSetup.PageBordersApplyType;
    destination.PageBorderIsInFront = source.PageSetup.IsFrontPageBorder;
    destination.PageBorderOffsetFrom = source.PageSetup.PageBorderOffsetFrom;
    destination.PageNfc = (byte) source.PageSetup.PageNumberStyle;
    destination.PageRestart = source.PageSetup.RestartPageNumbering;
    destination.PageStartAt = (ushort) source.PageSetup.PageStartingNumber;
    if (source.PageSetup.PageNumbers.HeadingLevelForChapter != HeadingLevel.None)
    {
      destination.HeadingLevelForChapter = (byte) source.PageSetup.PageNumbers.HeadingLevelForChapter;
      destination.ChapterPageSeparator = (byte) source.PageSetup.PageNumbers.ChapterPageSeparator;
    }
    else
    {
      destination.Sprms.RemoveValue(12288 /*0x3000*/);
      destination.Sprms.RemoveValue(12289);
    }
    destination.Columns.Clear();
    destination.OldColumns.Clear();
    if (source.PageSetup.FootnoteNumberFormat != FootEndNoteNumberFormat.Arabic)
      destination.FootnoteNumberFormat = (ushort) (byte) source.PageSetup.FootnoteNumberFormat;
    if (source.PageSetup.FootnotePosition == FootnotePosition.PrintImmediatelyBeneathText)
      destination.FootnotePosition = (byte) source.PageSetup.FootnotePosition;
    if (source.PageSetup.EndnoteNumberFormat != FootEndNoteNumberFormat.LowerCaseRoman)
      destination.EndnoteNumberFormat = (ushort) (byte) source.PageSetup.EndnoteNumberFormat;
    if (source.PageSetup.RestartIndexForFootnotes != FootnoteRestartIndex.DoNotRestart)
      destination.RestartIndexForFootnotes = (byte) source.PageSetup.RestartIndexForFootnotes;
    if (source.PageSetup.RestartIndexForEndnote != EndnoteRestartIndex.DoNotRestart)
      destination.RestartIndexForEndnote = (byte) source.PageSetup.RestartIndexForEndnote;
    if (source.PageSetup.InitialFootnoteNumber != 1)
      destination.InitialFootnoteNumber = (ushort) source.PageSetup.InitialFootnoteNumber;
    if (source.PageSetup.InitialEndnoteNumber != 1)
      destination.InitialEndnoteNumber = (ushort) source.PageSetup.InitialEndnoteNumber;
    if (source.Columns.Count > 0)
    {
      int index = 0;
      for (int count = source.Columns.Count; index < count; ++index)
      {
        Column column = source.Columns[index];
        ColumnDescriptor columnDescriptor = destination.Columns.AddColumn();
        columnDescriptor.Width = (ushort) Math.Round((double) column.Width * 20.0);
        columnDescriptor.Space = (ushort) Math.Round((double) column.Space * 20.0);
      }
      if (source.Columns.Count > 1 && !source.PageSetup.EqualColumnWidth)
        destination.Columns.ColumnsEvenlySpaced = false;
    }
    else
      destination.Sprms.SetShortValue(36876, (short) 720);
    if (source.DataArray != null && source.DataArray.Length < 300 && source.DataArray.Length > 0)
    {
      SinglePropertyModifierArray propertyModifierArray = new SinglePropertyModifierArray(source.DataArray, 0);
      int sprmIndex = 0;
      for (int count = propertyModifierArray.Count; sprmIndex < count; ++sprmIndex)
      {
        SinglePropertyModifierRecord sprmByIndex = propertyModifierArray.GetSprmByIndex(sprmIndex);
        if (!destination.HasOptions(sprmByIndex.TypedOptions) || destination.HasOptions(21039) || destination.HasOptions(53827))
          destination.Sprms.Modifiers.Add(sprmByIndex);
      }
    }
    destination.Sprms.SortSprms();
    destination.DrawLinesBetweenCols = source.PageSetup.DrawLinesBetweenCols;
    destination.BreakCode = (byte) source.BreakCode;
    destination.ProtectForm = source.ProtectForm;
  }

  private static void ExportBorder(BorderCode srcBorder, Border destBorder)
  {
    if (srcBorder.IsDefault)
      return;
    destBorder.Color = srcBorder.LineColorExt;
    destBorder.BorderType = (BorderStyle) srcBorder.BorderType;
    destBorder.LineWidth = (float) srcBorder.LineWidth / 8f;
    destBorder.Space = (float) srcBorder.Space;
    destBorder.Shadow = srcBorder.Shadow;
  }

  private static void ImportBorder(BorderCode destBorder, Border srcBorder)
  {
    if (!srcBorder.IsDefault)
    {
      destBorder.LineColor = (byte) WordColor.ConvertColorToId(srcBorder.Color);
      destBorder.LineColorExt = srcBorder.Color;
      destBorder.BorderType = (byte) srcBorder.BorderType;
      destBorder.LineWidth = (byte) Math.Round((double) srcBorder.LineWidth * 8.0);
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

  private static Borders GetDestBorders(Borders destBorders, WPageSetup destination)
  {
    if (destBorders == null)
      destBorders = destination.Borders;
    return destBorders;
  }
}
