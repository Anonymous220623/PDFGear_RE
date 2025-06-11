// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.TablePropertiesConverter
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class TablePropertiesConverter
{
  private static readonly object m_threadLocker = new object();
  private static byte[] m_cellShadings = (byte[]) null;
  private static byte[] m_cellShadings_1st = (byte[]) null;
  private static byte[] m_cellShadings_2nd = (byte[]) null;
  private static byte[] m_cellShadings_3rd = (byte[]) null;
  private static Dictionary<short, byte[]> m_cellBorders = (Dictionary<short, byte[]>) null;
  private static Dictionary<short, byte[]> m_cellTCGRF = (Dictionary<short, byte[]>) null;
  private static Dictionary<short, byte[]> m_cellWidth = (Dictionary<short, byte[]>) null;
  private static byte[] m_topBorderCV = (byte[]) null;
  private static byte[] m_leftBorderCV = (byte[]) null;
  private static byte[] m_rightBorderCV = (byte[]) null;
  private static byte[] m_bottomBorderCV = (byte[]) null;
  private static byte[] m_cellBorderType = (byte[]) null;

  internal static void SprmsToFormat(IWordReaderBase reader, RowFormat rowFormat)
  {
    lock (TablePropertiesConverter.m_threadLocker)
      TablePropertiesConverter.SprmsToFormat(reader.PAPX.PropertyModifiers, rowFormat, reader.StyleSheet, reader.SttbfRMarkAuthorNames, reader, true);
  }

  internal static void SprmsToFormat(
    SinglePropertyModifierArray sprms,
    RowFormat rowFormat,
    WordStyleSheet styleSheet,
    Dictionary<int, string> authorNames,
    IWordReaderBase reader,
    bool isNewPropertyHash)
  {
    lock (TablePropertiesConverter.m_threadLocker)
    {
      if (sprms == null)
        return;
      RowFormat.TablePositioning tablePositioning1 = (RowFormat.TablePositioning) null;
      if (sprms.Contain(13928))
      {
        rowFormat.IsFormattingChange = true;
        tablePositioning1 = new RowFormat.TablePositioning(rowFormat);
      }
      WTableRow ownerRow = rowFormat.OwnerRow;
      short[] xCenterArray = (short[]) null;
      int cellCount = 0;
      int num1 = 22;
      RowFormat.TablePositioning positioning = rowFormat.Positioning;
      Spacings source = (Spacings) null;
      Dictionary<int, Spacings> cellSpacings = (Dictionary<int, Spacings>) null;
      bool flag = false;
      IEnumerator enumerator = sprms.GetEnumerator();
      try
      {
label_142:
        while (enumerator.MoveNext())
        {
          SinglePropertyModifierRecord current = (SinglePropertyModifierRecord) enumerator.Current;
          switch (current.Options)
          {
            case 13315:
              if (rowFormat.Document.WordVersion <= (ushort) 217)
              {
                rowFormat.SetPropertyValue(106, (object) !current.BoolValue);
                continue;
              }
              continue;
            case 13316:
              rowFormat.SetPropertyValue(107, (object) current.BoolValue);
              continue;
            case 13413:
              RowFormat.TablePositioning tablePositioning2 = tablePositioning1 ?? positioning;
              tablePositioning2.AllowOverlap = !current.BoolValue;
              if (rowFormat.IsFormattingChange && !flag)
              {
                rowFormat.SetPropertyValue(120, (object) tablePositioning2);
                flag = true;
                continue;
              }
              continue;
            case 13414:
              if (rowFormat.Document.WordVersion > (ushort) 217)
              {
                rowFormat.SetPropertyValue(106, (object) !current.BoolValue);
                continue;
              }
              continue;
            case 13837:
              RowFormat.TablePositioning tablePositioning3 = tablePositioning1 ?? positioning;
              HorizontalRelation horizontalRelation = HorizontalRelation.Column;
              switch ((int) current.ByteValue >> 6 & 3)
              {
                case 1:
                  horizontalRelation = HorizontalRelation.Margin;
                  break;
                case 2:
                  horizontalRelation = HorizontalRelation.Page;
                  break;
              }
              tablePositioning3.HorizRelationTo = horizontalRelation;
              VerticalRelation verticalRelation = VerticalRelation.Margin;
              switch ((int) current.ByteValue >> 4 & 3)
              {
                case 1:
                  verticalRelation = VerticalRelation.Page;
                  break;
                case 2:
                  verticalRelation = VerticalRelation.Paragraph;
                  break;
              }
              tablePositioning3.VertRelationTo = verticalRelation;
              if (rowFormat.IsFormattingChange && !flag)
              {
                rowFormat.SetPropertyValue(120, (object) tablePositioning3);
                flag = true;
                continue;
              }
              continue;
            case 13845:
              rowFormat.SetPropertyValue(103, (object) current.BoolValue);
              continue;
            case 13928:
              if (source != null)
              {
                Paddings destination = new Paddings();
                TablePropertiesConverter.ExportPaddings(source, destination);
                rowFormat.SetPropertyValue(3, (object) destination);
              }
              source = (Spacings) null;
              if (cellSpacings != null && cellSpacings.Count > 0)
                TablePropertiesConverter.UpdateCellPaddings(cellSpacings, ownerRow, rowFormat.IsFormattingChange);
              cellSpacings = (Dictionary<int, Spacings>) null;
              if (!rowFormat.OldPropertiesHash.ContainsKey(53) && xCenterArray != null)
                TablePropertiesConverter.UpdateLeftIndent(sprms.GetOldSprm(26185, 13928), ownerRow, xCenterArray[0], rowFormat.IsFormattingChange);
              tablePositioning1 = (RowFormat.TablePositioning) null;
              flag = false;
              rowFormat.IsFormattingChange = false;
              continue;
            case 21504:
              if (rowFormat.Document.WordVersion <= (ushort) 217)
              {
                rowFormat.SetPropertyValue(105, (object) (ParagraphJustify) current.ShortValue);
                continue;
              }
              continue;
            case 21642:
              if (sprms.HasSprm(22116) && sprms[22116].BoolValue && current.ShortValue == (short) 2 && (!sprms.HasSprm(22027) || !sprms[22027].BoolValue))
              {
                rowFormat.SetPropertyValue(105, (object) ParagraphJustify.Left);
                continue;
              }
              rowFormat.SetPropertyValue(105, (object) (ParagraphJustify) current.ShortValue);
              continue;
            case 22027:
              ownerRow.RowFormat.SetPropertyValue(104, (object) current.BoolValue);
              continue;
            case 22074:
              if (rowFormat.m_unParsedSprms == null)
                rowFormat.m_unParsedSprms = new SinglePropertyModifierArray();
              rowFormat.m_unParsedSprms.Add(current);
              continue;
            case 22116:
              if (rowFormat.m_unParsedSprms == null)
                rowFormat.m_unParsedSprms = new SinglePropertyModifierArray();
              rowFormat.m_unParsedSprms.Add(current);
              continue;
            case 25707:
              if (rowFormat.m_unParsedSprms == null)
                rowFormat.m_unParsedSprms = new SinglePropertyModifierArray();
              rowFormat.m_unParsedSprms.Add(current);
              continue;
            case 29706:
              if (rowFormat.m_unParsedSprms == null)
                rowFormat.m_unParsedSprms = new SinglePropertyModifierArray();
              rowFormat.m_unParsedSprms.Add(current);
              continue;
            case 29801:
              if (rowFormat.m_unParsedSprms == null)
                rowFormat.m_unParsedSprms = new SinglePropertyModifierArray();
              rowFormat.m_unParsedSprms.Add(current);
              continue;
            case 37895:
              float num2 = (float) current.ShortValue / 20f;
              if ((double) num2 < 0.0)
              {
                ownerRow.HeightType = TableRowHeightType.Exactly;
                num2 = Math.Abs(num2);
              }
              rowFormat.Height = num2;
              continue;
            case 37902:
              RowFormat.TablePositioning tablePositioning4 = tablePositioning1 ?? positioning;
              short shortValue1 = current.ShortValue;
              tablePositioning4.HorizPosition = shortValue1 == (short) -4 || shortValue1 == (short) -8 || shortValue1 == (short) -12 || shortValue1 == (short) -16 ? (float) shortValue1 : (float) shortValue1 / 20f;
              if (rowFormat.IsFormattingChange && !flag)
              {
                rowFormat.SetPropertyValue(120, (object) tablePositioning4);
                flag = true;
                continue;
              }
              continue;
            case 37903:
              RowFormat.TablePositioning tablePositioning5 = tablePositioning1 ?? positioning;
              short shortValue2 = current.ShortValue;
              tablePositioning5.VertPosition = shortValue2 == (short) -4 || shortValue2 == (short) -8 || shortValue2 == (short) -12 || shortValue2 == (short) -16 || shortValue2 == (short) -20 ? (float) shortValue2 : (float) shortValue2 / 20f;
              if (rowFormat.IsFormattingChange && !flag)
              {
                rowFormat.SetPropertyValue(120, (object) tablePositioning5);
                flag = true;
                continue;
              }
              continue;
            case 37904:
              RowFormat.TablePositioning tablePositioning6 = tablePositioning1 ?? positioning;
              tablePositioning6.DistanceFromLeft = (float) current.ShortValue / 20f;
              if (rowFormat.IsFormattingChange && !flag)
              {
                rowFormat.SetPropertyValue(120, (object) tablePositioning6);
                flag = true;
                continue;
              }
              continue;
            case 37905:
              RowFormat.TablePositioning tablePositioning7 = tablePositioning1 ?? positioning;
              tablePositioning7.DistanceFromTop = (float) current.ShortValue / 20f;
              if (rowFormat.IsFormattingChange && !flag)
              {
                rowFormat.SetPropertyValue(120, (object) tablePositioning7);
                flag = true;
                continue;
              }
              continue;
            case 37918:
              RowFormat.TablePositioning tablePositioning8 = tablePositioning1 ?? positioning;
              tablePositioning8.DistanceFromRight = (float) current.ShortValue / 20f;
              if (rowFormat.IsFormattingChange && !flag)
              {
                rowFormat.SetPropertyValue(120, (object) tablePositioning8);
                flag = true;
                continue;
              }
              continue;
            case 37919:
              RowFormat.TablePositioning tablePositioning9 = tablePositioning1 ?? positioning;
              tablePositioning9.DistanceFromBottom = (float) current.ShortValue / 20f;
              if (rowFormat.IsFormattingChange && !flag)
              {
                rowFormat.SetPropertyValue(120, (object) tablePositioning9);
                flag = true;
                continue;
              }
              continue;
            case 38401:
              if (rowFormat.m_unParsedSprms == null)
                rowFormat.m_unParsedSprms = new SinglePropertyModifierArray();
              rowFormat.m_unParsedSprms.Add(current);
              continue;
            case 54789:
              TablePropertiesConverter.ExportTableRowBorders(sprms, rowFormat, current);
              continue;
            case 54792:
              TablePropertiesConverter.UpdateTableCellDefinition(current, ownerRow, ref cellCount, ref xCenterArray, reader);
              continue;
            case 54793:
              byte[] byteArray1 = current.ByteArray;
              int index1 = 0;
              int startIndex = 0;
              while (true)
              {
                if (index1 < ownerRow.Cells.Count && byteArray1.Length > startIndex)
                {
                  WTableCell cell = ownerRow.Cells[index1];
                  ShadingDescriptor shdDesc = new ShadingDescriptor(BitConverter.ToInt16(byteArray1, startIndex));
                  cell.CellFormat.IsFormattingChange = ownerRow.RowFormat.IsFormattingChange;
                  TablePropertiesConverter.SetShadingValues(cell.CellFormat, shdDesc);
                  startIndex += 2;
                  ++index1;
                }
                else
                  goto label_142;
              }
            case 54796:
              TablePropertiesConverter.UpdateCellShading(num1 * 2, 63 /*0x3F*/, current.ByteArray, ownerRow);
              continue;
            case 54802:
              TablePropertiesConverter.UpdateCellShading(0, num1, current.ByteArray, ownerRow);
              continue;
            case 54803:
              Syncfusion.DocIO.ReaderWriter.Biff_Records.TableBorders tableBorders = new Syncfusion.DocIO.ReaderWriter.Biff_Records.TableBorders();
              TablePropertiesConverter.ExportTableRowBorders(current.ByteArray, tableBorders, false);
              if (rowFormat.IsFormattingChange)
              {
                Borders destBorders = new Borders();
                TablePropertiesConverter.ExportBorders(tableBorders, destBorders);
                rowFormat.SetPropertyValue(1, (object) destBorders);
                continue;
              }
              TablePropertiesConverter.ExportBorders(tableBorders, rowFormat.Borders);
              continue;
            case 54806:
              TablePropertiesConverter.UpdateCellShading(num1, num1 * 2, current.ByteArray, ownerRow);
              continue;
            case 54810:
              uint[] colors1 = TablePropertiesConverter.GetColors(current, ownerRow.Cells.Count);
              if (colors1 != null && colors1.Length == ownerRow.Cells.Count)
              {
                for (int index2 = 0; index2 < ownerRow.Cells.Count; ++index2)
                {
                  Borders cellBorders = TablePropertiesConverter.GetCellBorders(ownerRow.Cells[index2], rowFormat.IsFormattingChange);
                  if (colors1[index2] != 4278190080U /*0xFF000000*/)
                    cellBorders.Top.Color = WordColor.ConvertRGBToColor(colors1[index2]);
                }
                continue;
              }
              continue;
            case 54811:
              uint[] colors2 = TablePropertiesConverter.GetColors(current, ownerRow.Cells.Count);
              if (colors2 != null && colors2.Length == ownerRow.Cells.Count)
              {
                for (int index3 = 0; index3 < ownerRow.Cells.Count; ++index3)
                {
                  Borders cellBorders = TablePropertiesConverter.GetCellBorders(ownerRow.Cells[index3], rowFormat.IsFormattingChange);
                  if (colors2[index3] != 4278190080U /*0xFF000000*/)
                    cellBorders.Left.Color = WordColor.ConvertRGBToColor(colors2[index3]);
                }
                continue;
              }
              continue;
            case 54812:
              uint[] colors3 = TablePropertiesConverter.GetColors(current, ownerRow.Cells.Count);
              if (colors3 != null && colors3.Length == ownerRow.Cells.Count)
              {
                for (int index4 = 0; index4 < ownerRow.Cells.Count; ++index4)
                {
                  Borders cellBorders = TablePropertiesConverter.GetCellBorders(ownerRow.Cells[index4], rowFormat.IsFormattingChange);
                  if (colors3[index4] != 4278190080U /*0xFF000000*/)
                    cellBorders.Bottom.Color = WordColor.ConvertRGBToColor(colors3[index4]);
                }
                continue;
              }
              continue;
            case 54813:
              uint[] colors4 = TablePropertiesConverter.GetColors(current, ownerRow.Cells.Count);
              if (colors4 != null && colors4.Length == ownerRow.Cells.Count)
              {
                for (int index5 = 0; index5 < ownerRow.Cells.Count; ++index5)
                {
                  Borders cellBorders = TablePropertiesConverter.GetCellBorders(ownerRow.Cells[index5], rowFormat.IsFormattingChange);
                  if (colors4[index5] != 4278190080U /*0xFF000000*/)
                    cellBorders.Right.Color = WordColor.ConvertRGBToColor(colors4[index5]);
                }
                continue;
              }
              continue;
            case 54834:
              if (cellSpacings == null)
                cellSpacings = new Dictionary<int, Spacings>();
              byte key1 = current.ByteArray[0];
              byte num3 = current.ByteArray[1];
              if ((int) key1 < ownerRow.Cells.Count && (int) num3 >= (int) key1 && (int) num3 <= ownerRow.Cells.Count)
              {
                if (cellSpacings.ContainsKey((int) key1))
                {
                  cellSpacings[(int) key1].Parse(current);
                }
                else
                {
                  Spacings spacings = new Spacings(current);
                  cellSpacings.Add((int) key1, spacings.Clone());
                }
                if ((int) key1 + 1 != (int) num3)
                {
                  for (int key2 = (int) key1 + 1; key2 < (int) num3; ++key2)
                    cellSpacings[key2] = cellSpacings[(int) key1].Clone();
                  continue;
                }
                continue;
              }
              continue;
            case 54835:
              byte[] byteArray2 = current.ByteArray;
              if (byteArray2 != null && byteArray2.Length == 6 && (byteArray2[3] == (byte) 3 || byteArray2[3] == (byte) 19) && byteArray2[2] == (byte) 15)
              {
                float num4 = (float) BitConverter.ToUInt16(byteArray2, 4) / 20f;
                if ((double) num4 >= 0.0)
                {
                  rowFormat.SetPropertyValue(52, (object) num4);
                  continue;
                }
                continue;
              }
              continue;
            case 54836:
              if (source == null)
              {
                source = new Spacings(current);
                continue;
              }
              source.Parse(current);
              continue;
            case 54880:
              ShadingDescriptor shadingDescriptor = new ShadingDescriptor();
              shadingDescriptor.ReadNewShd(current.ByteArray, 0);
              rowFormat.SetPropertyValue(111, (object) shadingDescriptor.ForeColor);
              rowFormat.SetPropertyValue(108, (object) shadingDescriptor.BackColor);
              rowFormat.SetPropertyValue(110, (object) shadingDescriptor.Pattern);
              continue;
            case 54882:
              byte[] byteArray3 = current.ByteArray;
              if (byteArray3.Length % 4 == 0)
              {
                int index6 = 0;
                int index7 = 0;
                while (true)
                {
                  if (index7 < ownerRow.Cells.Count && index6 + 3 < byteArray3.Length)
                  {
                    WTableCell cell = ownerRow.Cells[index7];
                    Borders borders = rowFormat.IsFormattingChange ? (Borders) cell.CellFormat.OldPropertiesHash[1] : (Borders) cell.CellFormat.PropertiesHash[1];
                    if (borders != null)
                    {
                      TablePropertiesConverter.ApplyBorderStyle(borders.Top, byteArray3[index6]);
                      TablePropertiesConverter.ApplyBorderStyle(borders.Left, byteArray3[index6 + 1]);
                      TablePropertiesConverter.ApplyBorderStyle(borders.Bottom, byteArray3[index6 + 2]);
                      TablePropertiesConverter.ApplyBorderStyle(borders.Right, byteArray3[index6 + 3]);
                    }
                    index6 += 4;
                    ++index7;
                  }
                  else
                    goto label_142;
                }
              }
              else
                continue;
            case 54887:
              rowFormat.IsChangedFormat = true;
              short int16 = BitConverter.ToInt16(current.ByteArray, 1);
              if (authorNames != null && authorNames.Count > 0 && authorNames.ContainsKey((int) int16))
                rowFormat.FormatChangeAuthorName = authorNames[(int) int16];
              DateTime dateTime = rowFormat.ParseDTTM(BitConverter.ToInt32(current.ByteArray, 3));
              if (dateTime.Year < 1900)
                dateTime = new DateTime(1900, 1, 1, 0, 0, 0);
              rowFormat.FormatChangeDateTime = dateTime;
              continue;
            case 54896:
              if ((rowFormat.IsFormattingChange ? sprms.GetOldSprm(54802, 13928) : sprms.GetNewSprm(54802, 13928)) == null)
              {
                TablePropertiesConverter.UpdateCellShading(0, num1, current.ByteArray, ownerRow);
                continue;
              }
              continue;
            case 54897:
              if ((rowFormat.IsFormattingChange ? sprms.GetOldSprm(54806, 13928) : sprms.GetNewSprm(54806, 13928)) == null)
              {
                TablePropertiesConverter.UpdateCellShading(num1, num1 * 2, current.ByteArray, ownerRow);
                continue;
              }
              continue;
            case 54898:
              if ((rowFormat.IsFormattingChange ? sprms.GetOldSprm(54796, 13928) : sprms.GetNewSprm(54796, 13928)) == null)
              {
                TablePropertiesConverter.UpdateCellShading(num1 * 2, 63 /*0x3F*/, current.ByteArray, ownerRow);
                continue;
              }
              continue;
            case 62996:
              TablePropertiesConverter.UpdatePreferredWidthInfo(current, rowFormat, 11);
              continue;
            case 62999:
              TablePropertiesConverter.UpdatePreferredWidthInfo(current, rowFormat, 13);
              continue;
            case 63000:
              TablePropertiesConverter.UpdatePreferredWidthInfo(current, rowFormat, 15);
              continue;
            case 63073:
              rowFormat.SetPropertyValue(53, (object) (float) ((double) BitConverter.ToInt16(current.ByteArray, 1) / 20.0));
              continue;
            default:
              continue;
          }
        }
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
      if (sprms.GetNewSprm(54803, 13928) == null && sprms.GetNewSprm(54789, 13928) == null)
        TablePropertiesConverter.ExportBorders(new Syncfusion.DocIO.ReaderWriter.Biff_Records.TableBorders(), rowFormat.Borders);
      if (source != null && !source.IsEmpty)
        TablePropertiesConverter.ExportPaddings(source, rowFormat.Paddings);
      else if (sprms.Contain(54836))
        TablePropertiesConverter.ExportDefaultPaddings(rowFormat.Paddings);
      rowFormat.SkipDefaultPadding = true;
      if (cellSpacings != null && cellSpacings.Count > 0)
        TablePropertiesConverter.UpdateCellPaddings(cellSpacings, ownerRow);
      if (!rowFormat.PropertiesHash.ContainsKey(53) && xCenterArray != null)
        TablePropertiesConverter.UpdateLeftIndent(sprms.GetNewSprm(26185, 13928), ownerRow, xCenterArray[0], rowFormat.IsFormattingChange);
      TablePropertiesConverter.UpdateRowFormatPropertyHash(rowFormat);
      foreach (WTableCell cell in (CollectionImpl) ownerRow.Cells)
        TablePropertiesConverter.UpdateCellFormatPropertyHash(cell.CellFormat);
    }
  }

  private static void ApplyBorderStyle(Border border, byte borderTypeValue)
  {
    if ((double) border.LineWidth == 0.0 && border.Color == Color.Empty && border.BorderType == BorderStyle.None)
      return;
    border.BorderType = (BorderStyle) borderTypeValue;
  }

  private static void UpdateRowFormatPropertyHash(RowFormat rowFormat)
  {
    if (rowFormat.OldPropertiesHash.Count <= 0 || rowFormat.PropertiesHash.Count <= 0)
      return;
    foreach (KeyValuePair<int, object> keyValuePair in rowFormat.OldPropertiesHash)
    {
      if (keyValuePair.Key == 1 && rowFormat.PropertiesHash.ContainsKey(1))
      {
        Borders borders1 = (Borders) keyValuePair.Value;
        Borders borders2 = rowFormat.Borders;
        if (borders1.Top.IsBorderDefined && !borders2.Top.IsBorderDefined)
          ParagraphPropertiesConverter.ExportBorder(borders2.Top, borders1.Top);
        if (borders1.Bottom.IsBorderDefined && !borders2.Bottom.IsBorderDefined)
          ParagraphPropertiesConverter.ExportBorder(borders2.Bottom, borders1.Bottom);
        if (borders1.Right.IsBorderDefined && !borders2.Right.IsBorderDefined)
          ParagraphPropertiesConverter.ExportBorder(borders2.Right, borders1.Right);
        if (borders1.Left.IsBorderDefined && !borders2.Left.IsBorderDefined)
          ParagraphPropertiesConverter.ExportBorder(borders2.Left, borders1.Left);
        if (borders1.Horizontal.IsBorderDefined && !borders2.Horizontal.IsBorderDefined)
          ParagraphPropertiesConverter.ExportBorder(borders2.Horizontal, borders1.Horizontal);
        if (borders1.Vertical.IsBorderDefined && !borders2.Vertical.IsBorderDefined)
          ParagraphPropertiesConverter.ExportBorder(borders2.Vertical, borders1.Vertical);
      }
      if (keyValuePair.Key == 120 && rowFormat.PropertiesHash.ContainsKey(120))
        TablePropertiesConverter.UpdatePositioning((RowFormat.TablePositioning) keyValuePair.Value, rowFormat.Positioning);
      if (keyValuePair.Key == 3 && rowFormat.PropertiesHash.ContainsKey(3))
        TablePropertiesConverter.UpdatePaddings((Paddings) keyValuePair.Value, rowFormat.Paddings);
      if (!rowFormat.PropertiesHash.ContainsKey(keyValuePair.Key))
        rowFormat.PropertiesHash.Add(keyValuePair.Key, keyValuePair.Value);
    }
  }

  private static void UpdateCellFormatPropertyHash(CellFormat cellFormat)
  {
    if (cellFormat.OldPropertiesHash.Count <= 0 || cellFormat.PropertiesHash.Count <= 0)
      return;
    foreach (KeyValuePair<int, object> keyValuePair in cellFormat.OldPropertiesHash)
    {
      if (keyValuePair.Key == 1 && cellFormat.PropertiesHash.ContainsKey(1))
      {
        Borders borders1 = (Borders) keyValuePair.Value;
        Borders borders2 = cellFormat.Borders;
        if (borders1.Top.IsBorderDefined && !borders2.Top.IsBorderDefined)
          ParagraphPropertiesConverter.ExportBorder(borders2.Top, borders1.Top);
        if (borders1.Bottom.IsBorderDefined && !borders2.Bottom.IsBorderDefined)
          ParagraphPropertiesConverter.ExportBorder(borders2.Bottom, borders1.Bottom);
        if (borders1.Right.IsBorderDefined && !borders2.Right.IsBorderDefined)
          ParagraphPropertiesConverter.ExportBorder(borders2.Right, borders1.Right);
        if (borders1.Left.IsBorderDefined && !borders2.Left.IsBorderDefined)
          ParagraphPropertiesConverter.ExportBorder(borders2.Left, borders1.Left);
        if (borders1.Horizontal.IsBorderDefined && !borders2.Horizontal.IsBorderDefined)
          ParagraphPropertiesConverter.ExportBorder(borders2.Horizontal, borders1.Horizontal);
        if (borders1.Vertical.IsBorderDefined && !borders2.Vertical.IsBorderDefined)
          ParagraphPropertiesConverter.ExportBorder(borders2.Vertical, borders1.Vertical);
      }
      if (keyValuePair.Key == 3 && cellFormat.PropertiesHash.ContainsKey(3))
        TablePropertiesConverter.UpdatePaddings((Paddings) keyValuePair.Value, cellFormat.Paddings);
      if (!cellFormat.PropertiesHash.ContainsKey(keyValuePair.Key))
        cellFormat.PropertiesHash.Add(keyValuePair.Key, keyValuePair.Value);
    }
  }

  private static void UpdatePositioning(
    RowFormat.TablePositioning positioningOld,
    RowFormat.TablePositioning positioning)
  {
    if (positioningOld.HasKey(70) && !positioning.HasKey(70))
      positioning.AllowOverlap = positioningOld.AllowOverlap;
    if (positioningOld.HasKey(67) && !positioning.HasKey(67))
      positioning.DistanceFromBottom = positioningOld.DistanceFromBottom;
    if (positioningOld.HasKey(68) && !positioning.HasKey(68))
      positioning.DistanceFromLeft = positioningOld.DistanceFromLeft;
    if (positioningOld.HasKey(69) && !positioning.HasKey(69))
      positioning.DistanceFromRight = positioningOld.DistanceFromRight;
    if (positioningOld.HasKey(66) && !positioning.HasKey(66))
      positioning.DistanceFromTop = positioningOld.DistanceFromTop;
    if (positioningOld.HasKey(62) && !positioning.HasKey(62))
      positioning.HorizPositionAbs = positioningOld.HorizPositionAbs;
    if (positioningOld.HasKey(64 /*0x40*/) && !positioning.HasKey(64 /*0x40*/))
      positioning.HorizRelationTo = positioningOld.HorizRelationTo;
    if (positioningOld.HasKey(63 /*0x3F*/) && !positioning.HasKey(63 /*0x3F*/))
      positioning.VertPositionAbs = positioningOld.VertPositionAbs;
    if (!positioningOld.HasKey(65) || positioning.HasKey(65))
      return;
    positioning.VertRelationTo = positioningOld.VertRelationTo;
  }

  private static void UpdatePaddings(Paddings paddingsOld, Paddings paddings)
  {
    if (paddingsOld.HasKey(3) && !paddings.HasKey(3))
      paddings.Bottom = paddingsOld.Bottom;
    if (paddingsOld.HasKey(1) && !paddings.HasKey(1))
      paddings.Left = paddingsOld.Left;
    if (paddingsOld.HasKey(4) && !paddings.HasKey(4))
      paddings.Right = paddingsOld.Right;
    if (!paddingsOld.HasKey(2) || paddings.HasKey(2))
      return;
    paddings.Top = paddingsOld.Top;
  }

  private static void UpdateCellPaddings(
    Dictionary<int, Spacings> cellSpacings,
    WTableRow tableRow,
    bool isChangedFormat)
  {
    foreach (KeyValuePair<int, Spacings> cellSpacing in cellSpacings)
    {
      Paddings destination = new Paddings();
      TablePropertiesConverter.ExportPaddings(cellSpacing.Value, destination);
      WTableCell cell = tableRow.Cells[cellSpacing.Key];
      cell.CellFormat.IsFormattingChange = isChangedFormat;
      cell.CellFormat.SetPropertyValue(3, (object) destination);
    }
    cellSpacings.Clear();
  }

  private static void UpdateCellPaddings(Dictionary<int, Spacings> cellSpacings, WTableRow tableRow)
  {
    foreach (KeyValuePair<int, Spacings> cellSpacing in cellSpacings)
    {
      WTableCell cell = tableRow.Cells[cellSpacing.Key];
      cell.CellFormat.SamePaddingsAsTable = false;
      TablePropertiesConverter.ExportPaddings(cellSpacing.Value, cell.CellFormat.Paddings);
    }
    cellSpacings.Clear();
  }

  private static void UpdateLeftIndent(
    SinglePropertyModifierRecord sprmTNestingLevel,
    WTableRow tableRow,
    short xCenterArrayStart,
    bool isChangedFormat)
  {
    short num1 = 0;
    Dictionary<int, object> dictionary1 = isChangedFormat ? tableRow.RowFormat.OldPropertiesHash : tableRow.RowFormat.PropertiesHash;
    if (sprmTNestingLevel != null && sprmTNestingLevel.IntValue == 1)
    {
      Dictionary<int, object> dictionary2 = isChangedFormat ? tableRow.Cells[0].CellFormat.OldPropertiesHash : tableRow.Cells[0].CellFormat.PropertiesHash;
      Paddings paddings1 = dictionary2.ContainsKey(3) ? dictionary2[3] as Paddings : (Paddings) null;
      if (paddings1 != null && (double) paddings1.Left != -1.0 && (double) paddings1.Right != -1.0 && (double) paddings1.Top != -1.0 && (double) paddings1.Bottom != -1.0)
      {
        num1 = (short) (paddings1.Left * 20f);
      }
      else
      {
        Paddings paddings2 = dictionary1.ContainsKey(3) ? dictionary1[3] as Paddings : (Paddings) null;
        if (paddings2 != null && (double) paddings2.Left != -1.0 && (double) paddings2.Right != -1.0 && (double) paddings2.Top != -1.0 && (double) paddings2.Bottom != -1.0)
          num1 = (short) (paddings2.Left * 20f);
      }
    }
    short num2 = 0;
    float num3 = (dictionary1.ContainsKey(52) ? (float) dictionary1[52] : 0.0f) * 20f;
    if ((double) num3 > 0.0)
    {
      num2 += (short) ((double) num3 * 2.0);
      Borders borders = dictionary1.ContainsKey(1) ? (Borders) dictionary1[1] : (Borders) null;
      if (borders != null)
        num2 += (short) (float) Math.Round((double) borders.Left.LineWidth / 8.0 * 20.0);
    }
    short num4 = (short) ((int) xCenterArrayStart + (int) num1 + (int) num2);
    tableRow.RowFormat.SetPropertyValue(53, (object) (float) ((double) num4 / 20.0));
  }

  private static uint[] GetColors(SinglePropertyModifierRecord sprm, int cellCount)
  {
    uint[] colors = (uint[]) null;
    byte[] byteArray = sprm.ByteArray;
    if (byteArray != null && byteArray.Length == cellCount * 4 && cellCount > 0)
    {
      colors = new uint[cellCount];
      for (int index = 0; index < cellCount; ++index)
      {
        uint uint32 = BitConverter.ToUInt32(byteArray, index * 4);
        colors[index] = uint32;
      }
    }
    return colors;
  }

  private static Borders GetCellBorders(WTableCell cell, bool isFormattingChange)
  {
    Borders cellBorders = (Borders) null;
    if (isFormattingChange && cell.CellFormat.OldPropertiesHash.ContainsKey(1))
      cellBorders = cell.CellFormat.OldPropertiesHash[1] as Borders;
    else if (cell.CellFormat.PropertiesHash.ContainsKey(1))
      cellBorders = cell.CellFormat.PropertiesHash[1] as Borders;
    return cellBorders;
  }

  private static void UpdateCellShading(
    int startCellIndex,
    int endCellIndex,
    byte[] buf,
    WTableRow tableRow)
  {
    int offset = 0;
    for (int index = startCellIndex; index < endCellIndex && index < tableRow.Cells.Count && buf.Length > offset; ++index)
    {
      WTableCell cell = tableRow.Cells[index];
      ShadingDescriptor shdDesc = new ShadingDescriptor();
      shdDesc.ReadNewShd(buf, offset);
      cell.CellFormat.IsFormattingChange = tableRow.RowFormat.IsFormattingChange;
      TablePropertiesConverter.SetShadingValues(cell.CellFormat, shdDesc);
      offset += 10;
    }
  }

  private static void UpdatePreferredWidthInfo(
    SinglePropertyModifierRecord sprm,
    RowFormat rowFormat,
    int formatKey)
  {
    byte[] byteArray = sprm.ByteArray;
    rowFormat.SetPropertyValue(formatKey, (object) (FtsWidth) byteArray[0]);
    if (byteArray[0] == (byte) 2)
      rowFormat.SetPropertyValue(formatKey + 1, (object) (float) ((double) BitConverter.ToInt16(byteArray, 1) / 50.0));
    else
      rowFormat.SetPropertyValue(formatKey + 1, (object) (float) ((double) BitConverter.ToInt16(byteArray, 1) / 20.0));
  }

  private static void SetShadingValues(CellFormat cellFormat, ShadingDescriptor shdDesc)
  {
    cellFormat.SetPropertyValue(5, (object) shdDesc.ForeColor);
    cellFormat.SetPropertyValue(4, (object) shdDesc.BackColor);
    cellFormat.SetPropertyValue(7, (object) shdDesc.Pattern);
  }

  private static void UpdateTableCellDefinition(
    SinglePropertyModifierRecord sprm,
    WTableRow tableRow,
    ref int cellCount,
    ref short[] xCenterArray,
    IWordReaderBase reader)
  {
    byte[] byteArray = sprm.ByteArray;
    cellCount = (int) byteArray[0];
    byte[] src = cellCount >= 1 ? new byte[(cellCount + 1) * 2] : throw new ArgumentException($" Number of cells {(object) cellCount} must be greater than 1");
    int startPos = 1 + 2 * (cellCount + 1);
    int num1 = 1 + 2 * (cellCount + 1);
    for (int index = 1; index < num1; ++index)
      src[index - 1] = byteArray[index];
    xCenterArray = new short[cellCount + 1];
    Buffer.BlockCopy((Array) src, 0, (Array) xCenterArray, 0, src.Length);
    if (reader != null && reader.TableRowWidthStack.Count > 0)
    {
      Dictionary<WTableRow, short> dictionary = reader.TableRowWidthStack.Peek();
      short num2 = xCenterArray[xCenterArray.Length - 1];
      if (!dictionary.ContainsKey(tableRow))
        dictionary.Add(tableRow, num2);
      if (reader.MaximumTableRowWidth != null && reader.MaximumTableRowWidth.Count > 0 && (int) reader.MaximumTableRowWidth[reader.MaximumTableRowWidth.Count - 1] < (int) num2)
        reader.MaximumTableRowWidth[reader.MaximumTableRowWidth.Count - 1] = num2;
    }
    int index1 = 0;
    int num3 = 20;
    for (; index1 < cellCount && index1 < tableRow.Cells.Count; ++index1)
    {
      WTableCell cell = tableRow.Cells[index1];
      cell.CellFormat.IsFormattingChange = tableRow.RowFormat.IsFormattingChange;
      if (index1 > xCenterArray.Length - 2)
        throw new ArgumentOutOfRangeException("cellIndex");
      cell.CellFormat.SetPropertyValue(12, (object) (float) ((double) ((int) xCenterArray[index1 + 1] - (int) xCenterArray[index1]) / 20.0));
      if (startPos + num3 > byteArray.Length)
        TablePropertiesConverter.UpdateTCGRF(cell.CellFormat, (short) 0);
      else
        TablePropertiesConverter.UpdateTC80(cell, byteArray, startPos);
      startPos += num3;
    }
  }

  private static void UpdateTC80(WTableCell tableCell, byte[] sprmData, int startPos)
  {
    short int16 = BitConverter.ToInt16(sprmData, startPos);
    TablePropertiesConverter.UpdateTCGRF(tableCell.CellFormat, int16);
    startPos += 2;
    ushort uint16 = BitConverter.ToUInt16(sprmData, startPos);
    switch ((FtsWidth) ((int) int16 >> 9 & 7))
    {
      case FtsWidth.Percentage:
        tableCell.CellFormat.SetPropertyValue(14, (object) (float) ((double) uint16 / 50.0));
        break;
      case FtsWidth.Point:
        tableCell.CellFormat.SetPropertyValue(14, (object) (float) ((double) uint16 / 20.0));
        break;
    }
    startPos += 2;
    Borders borders1 = new Borders();
    Borders borders2 = tableCell.CellFormat.IsFormattingChange ? borders1 : tableCell.CellFormat.Borders;
    TablePropertiesConverter.BRCToBorder(new BorderStructure(sprmData, startPos), borders2.Top);
    startPos += 4;
    TablePropertiesConverter.BRCToBorder(new BorderStructure(sprmData, startPos), borders2.Left);
    startPos += 4;
    TablePropertiesConverter.BRCToBorder(new BorderStructure(sprmData, startPos), borders2.Bottom);
    startPos += 4;
    TablePropertiesConverter.BRCToBorder(new BorderStructure(sprmData, startPos), borders2.Right);
    if (!tableCell.CellFormat.IsFormattingChange)
      return;
    tableCell.CellFormat.SetPropertyValue(1, (object) borders2);
  }

  private static void UpdateTCGRF(CellFormat cellFormat, short TCGRF)
  {
    cellFormat.SetPropertyValue(8, (object) TablePropertiesConverter.GetCellMerge((int) TCGRF & 3, true));
    TextDirection textDirection = TextDirection.Horizontal;
    switch ((int) TCGRF >> 2 & 7)
    {
      case 1:
        textDirection = TextDirection.VerticalTopToBottom;
        break;
      case 3:
        textDirection = TextDirection.VerticalBottomToTop;
        break;
      case 4:
        textDirection = TextDirection.HorizontalFarEast;
        break;
      case 5:
        textDirection = TextDirection.VerticalFarEast;
        break;
      case 7:
        textDirection = TextDirection.Vertical;
        break;
    }
    cellFormat.SetPropertyValue(11, (object) textDirection);
    cellFormat.SetPropertyValue(6, (object) TablePropertiesConverter.GetCellMerge((int) TCGRF >> 5 & 3, false));
    cellFormat.SetPropertyValue(2, (object) (VerticalAlignment) ((int) TCGRF >> 7 & 3));
    cellFormat.SetPropertyValue(13, (object) (FtsWidth) ((int) TCGRF >> 9 & 7));
    cellFormat.SetPropertyValue(10, (object) (((int) TCGRF >> 12 & 1) == 1));
    cellFormat.SetPropertyValue(9, (object) (((int) TCGRF >> 13 & 1) == 0));
  }

  private static CellMerge GetCellMerge(int val, bool isHorizontalCellMerge)
  {
    CellMerge cellMerge = CellMerge.None;
    switch (val)
    {
      case 1:
        cellMerge = isHorizontalCellMerge ? CellMerge.Start : CellMerge.Continue;
        break;
      case 2:
      case 3:
        cellMerge = isHorizontalCellMerge ? CellMerge.Continue : CellMerge.Start;
        break;
    }
    return cellMerge;
  }

  private static void ExportTableRowBorders(
    SinglePropertyModifierArray sprms,
    RowFormat rowFormat,
    SinglePropertyModifierRecord sprm)
  {
    if (rowFormat.IsFormattingChange)
    {
      if (sprms.GetOldSprm(54803, 13928) != null)
        return;
      byte[] byteArray = sprm.ByteArray;
      Syncfusion.DocIO.ReaderWriter.Biff_Records.TableBorders tableBorders = new Syncfusion.DocIO.ReaderWriter.Biff_Records.TableBorders();
      TablePropertiesConverter.ExportTableRowBorders(byteArray, tableBorders, true);
      Borders destBorders = new Borders();
      destBorders.IsFormattingChange = true;
      TablePropertiesConverter.ExportBorders(tableBorders, destBorders);
      rowFormat.SetPropertyValue(1, (object) destBorders);
    }
    else
    {
      if (sprms.GetNewSprm(54803, 13928) != null)
        return;
      byte[] byteArray = sprm.ByteArray;
      Syncfusion.DocIO.ReaderWriter.Biff_Records.TableBorders tableBorders = new Syncfusion.DocIO.ReaderWriter.Biff_Records.TableBorders();
      TablePropertiesConverter.ExportTableRowBorders(byteArray, tableBorders, true);
      TablePropertiesConverter.ExportBorders(tableBorders, rowFormat.Borders);
    }
  }

  private static void ExportBorders(Syncfusion.DocIO.ReaderWriter.Biff_Records.TableBorders srcBorders, Borders destBorders)
  {
    TablePropertiesConverter.BRCToBorder(srcBorders.LeftBorder, destBorders.Left);
    TablePropertiesConverter.BRCToBorder(srcBorders.RightBorder, destBorders.Right);
    TablePropertiesConverter.BRCToBorder(srcBorders.TopBorder, destBorders.Top);
    TablePropertiesConverter.BRCToBorder(srcBorders.BottomBorder, destBorders.Bottom);
    TablePropertiesConverter.BRCToBorder(srcBorders.HorizontalBorder, destBorders.Horizontal);
    TablePropertiesConverter.BRCToBorder(srcBorders.VerticalBorder, destBorders.Vertical);
  }

  private static void ExportTableRowBorders(byte[] buf, Syncfusion.DocIO.ReaderWriter.Biff_Records.TableBorders tableBorders, bool isOldSprm)
  {
    if (isOldSprm)
    {
      for (int index = 0; index < 6; ++index)
      {
        tableBorders[index] = new BorderCode();
        tableBorders[index].Parse(buf, index * 4);
      }
    }
    else
    {
      for (int index = 0; index < 6; ++index)
      {
        tableBorders[index] = new BorderCode();
        tableBorders[index].ParseNewBrc(buf, index * 8);
      }
    }
  }

  internal static void FormatToSprms(
    WTableRow tableRow,
    SinglePropertyModifierArray sprms,
    WordStyleSheet styleSheet)
  {
    lock (TablePropertiesConverter.m_threadLocker)
    {
      Dictionary<int, object> dictionary = new Dictionary<int, object>();
      RowFormat rowFormat = tableRow.RowFormat;
      if (!rowFormat.PropertiesHash.ContainsKey(1))
      {
        Borders borders = rowFormat.Borders;
      }
      if (!rowFormat.SkipDefaultPadding)
        rowFormat.CheckDefPadding();
      if (rowFormat.PropertiesHash.Count > 0)
        dictionary = new Dictionary<int, object>((IDictionary<int, object>) rowFormat.PropertiesHash);
      if (rowFormat.OldPropertiesHash.Count > 0)
      {
        foreach (KeyValuePair<int, object> keyValuePair in new Dictionary<int, object>((IDictionary<int, object>) rowFormat.OldPropertiesHash))
        {
          TablePropertiesConverter.FormatToSprms(keyValuePair.Key, keyValuePair.Value, sprms, rowFormat, styleSheet, true);
          if (dictionary.ContainsKey(keyValuePair.Key) && dictionary[keyValuePair.Key] == keyValuePair.Value)
            dictionary.Remove(keyValuePair.Key);
        }
      }
      int count = tableRow.Cells.Count;
      if (count > 0)
        TablePropertiesConverter.InitCollection(count);
      for (int index = 0; index < count; ++index)
      {
        CellFormat cellFormat = tableRow.Cells[index].CellFormat;
        if (cellFormat.OldPropertiesHash.Count > 0)
        {
          Dictionary<int, object> propertyHash = new Dictionary<int, object>((IDictionary<int, object>) cellFormat.OldPropertiesHash);
          ushort tcgrf = 0;
          foreach (KeyValuePair<int, object> keyValuePair in propertyHash)
          {
            TablePropertiesConverter.FormatToSprms(keyValuePair.Key, keyValuePair.Value, sprms, cellFormat, true, index, ref tcgrf);
            if (cellFormat.PropertiesHash.ContainsKey(keyValuePair.Key) && cellFormat.PropertiesHash[keyValuePair.Key] == keyValuePair.Value)
              cellFormat.PropertiesHash.Remove(keyValuePair.Key);
          }
          TablePropertiesConverter.SetShading(propertyHash, index, false);
          TablePropertiesConverter.m_cellTCGRF[(short) index] = BitConverter.GetBytes(tcgrf);
        }
      }
      TablePropertiesConverter.UpdateCellsInformation(sprms);
      if (rowFormat.OldPropertiesHash.Count > 0)
      {
        TablePropertiesConverter.WriteTableCellProps(sprms, tableRow, true);
        TablePropertiesConverter.WriteDxaGapHalf(sprms, tableRow, true);
        sprms.SetBoolValue(13928, true);
      }
      SinglePropertyModifierArray sprms1 = new SinglePropertyModifierArray();
      if (dictionary.Count > 0)
      {
        foreach (KeyValuePair<int, object> keyValuePair in dictionary)
          TablePropertiesConverter.FormatToSprms(keyValuePair.Key, keyValuePair.Value, sprms1, rowFormat, styleSheet, false);
        if (sprms1[63073] == null)
        {
          byte[] dst = new byte[3]
          {
            (byte) 3,
            (byte) 0,
            (byte) 0
          };
          Buffer.BlockCopy((Array) BitConverter.GetBytes((short) Math.Round((double) rowFormat.LeftIndent * 20.0)), 0, (Array) dst, 1, 2);
          sprms1.SetByteArrayValue(63073, dst);
        }
        if (sprms1[62996] == null)
        {
          byte[] dst = new byte[3];
          FtsWidth widthType = rowFormat.OwnerRow.OwnerTable.TableFormat.PreferredWidth.WidthType;
          float width = rowFormat.OwnerRow.OwnerTable.TableFormat.PreferredWidth.Width;
          dst[0] = (byte) widthType;
          short num = 0;
          if (widthType == FtsWidth.Percentage)
            num = (short) Math.Round((double) width * 50.0);
          else if (widthType == FtsWidth.Point)
            num = (short) Math.Round((double) width * 20.0);
          if (widthType > FtsWidth.Auto)
            Buffer.BlockCopy((Array) BitConverter.GetBytes(num), 0, (Array) dst, 1, 2);
          sprms1.SetByteArrayValue(62996, dst);
        }
        if (rowFormat.m_unParsedSprms != null && rowFormat.m_unParsedSprms.Count > 0)
        {
          foreach (SinglePropertyModifierRecord unParsedSprm in rowFormat.m_unParsedSprms)
          {
            if (unParsedSprm.OptionType != WordSprmOptionType.sprmPTableProps)
              sprms1.Add(unParsedSprm);
          }
        }
      }
      if (count > 0)
        TablePropertiesConverter.InitCollection(count);
      for (int index = 0; index < count; ++index)
      {
        CellFormat cellFormat = tableRow.Cells[index].CellFormat;
        Dictionary<int, object> propertyHash = new Dictionary<int, object>((IDictionary<int, object>) cellFormat.PropertiesHash);
        if (propertyHash.Count > 0)
        {
          ushort tcgrf = 0;
          foreach (KeyValuePair<int, object> keyValuePair in propertyHash)
            TablePropertiesConverter.FormatToSprms(keyValuePair.Key, keyValuePair.Value, sprms1, cellFormat, false, index, ref tcgrf);
          TablePropertiesConverter.m_cellTCGRF[(short) index] = BitConverter.GetBytes(tcgrf);
        }
        TablePropertiesConverter.SetShading(propertyHash, index, false);
      }
      if (tableRow.Cells.Count > 0)
      {
        TablePropertiesConverter.UpdateCellsInformation(sprms);
        TablePropertiesConverter.WriteTableCellProps(sprms1, tableRow, false);
        TablePropertiesConverter.WriteDxaGapHalf(sprms, tableRow, false);
      }
      for (int sprmIndex = 0; sprmIndex < sprms1.Count; ++sprmIndex)
        sprms.Add(sprms1.GetSprmByIndex(sprmIndex).Clone());
      sprms1.Clear();
      sprms.SortSprms();
    }
  }

  private static void WriteDxaGapHalf(
    SinglePropertyModifierArray sprms,
    WTableRow tableRow,
    bool isOldFormat)
  {
    short num1 = 0;
    short num2 = 0;
    WTableCell cell = tableRow.Cells[0];
    Paddings paddings1 = isOldFormat ? (Paddings) cell.CellFormat.GetKeyValue(cell.CellFormat.OldPropertiesHash, 3) : (Paddings) cell.CellFormat.GetKeyValue(cell.CellFormat.PropertiesHash, 3);
    Paddings paddings2 = isOldFormat ? (Paddings) tableRow.RowFormat.GetKeyValue(tableRow.RowFormat.OldPropertiesHash, 3) : (Paddings) tableRow.RowFormat.GetKeyValue(tableRow.RowFormat.PropertiesHash, 3);
    if (paddings1 != null && !paddings1.IsEmpty)
    {
      num1 = (short) ((double) paddings1.Left * 20.0);
      num2 = (short) ((double) paddings1.Right * 20.0);
    }
    else if (paddings2 != null && !paddings2.IsEmpty)
    {
      num1 = (short) ((double) paddings2.Left * 20.0);
      num2 = (short) ((double) paddings2.Right * 20.0);
    }
    byte[] numArray = new byte[2];
    byte[] bytes = BitConverter.GetBytes((short) (((int) num1 + (int) num2) / 2));
    sprms.SetByteArrayValue(38402, bytes);
  }

  private static void WriteTableCellProps(
    SinglePropertyModifierArray sprms,
    WTableRow row,
    bool isOldFormat)
  {
    int count = row.Cells.Count;
    byte[] dst1 = new byte[count * 22 + 3];
    short[] numArray = new short[count + 1];
    TablePropertiesConverter.UpdateXCenterArray(numArray, row, isOldFormat);
    byte[] dst2 = new byte[numArray.Length * 2 + 2];
    int dstOffset1 = 1 + 2 * (count + 1);
    TablePropertiesConverter.UpdateCenterArray(sprms, numArray, row, isOldFormat);
    Buffer.BlockCopy((Array) numArray, 0, (Array) dst2, 0, numArray.Length * 2);
    dst1[0] = (byte) count;
    dst2.CopyTo((Array) dst1, 1);
    for (short key = 0; (int) key < count; ++key)
    {
      if (TablePropertiesConverter.m_cellTCGRF.ContainsKey(key))
        Buffer.BlockCopy((Array) TablePropertiesConverter.m_cellTCGRF[key], 0, (Array) dst1, dstOffset1, 2);
      int dstOffset2 = dstOffset1 + 2;
      if (TablePropertiesConverter.m_cellWidth.ContainsKey(key))
        Buffer.BlockCopy((Array) TablePropertiesConverter.m_cellWidth[key], 0, (Array) dst1, dstOffset2, 2);
      int dstOffset3 = dstOffset2 + 2;
      if (TablePropertiesConverter.m_cellBorders.ContainsKey(key))
        Buffer.BlockCopy((Array) TablePropertiesConverter.m_cellBorders[key], 0, (Array) dst1, dstOffset3, 16 /*0x10*/);
      dstOffset1 = dstOffset3 + 16 /*0x10*/;
    }
    sprms.SetByteArrayValue(54792, dst1);
    TablePropertiesConverter.m_cellTCGRF.Clear();
    TablePropertiesConverter.m_cellTCGRF = (Dictionary<short, byte[]>) null;
    TablePropertiesConverter.m_cellWidth.Clear();
    TablePropertiesConverter.m_cellWidth = (Dictionary<short, byte[]>) null;
    TablePropertiesConverter.m_cellBorders.Clear();
    TablePropertiesConverter.m_cellBorders = (Dictionary<short, byte[]>) null;
  }

  private static void UpdateXCenterArray(short[] m_xCenterArray, WTableRow row, bool isOldFormat)
  {
    bool flag = false;
    WTableColumnCollection tableGrid = row.OwnerTable.TableGrid;
    for (int index = 0; index < row.Cells.Count; ++index)
    {
      if (index > m_xCenterArray.Length - 2)
        throw new ArgumentOutOfRangeException("cellIndex");
      WTableCell cell = row.Cells[index];
      Dictionary<int, object> dictionary = isOldFormat ? cell.CellFormat.OldPropertiesHash : cell.CellFormat.PropertiesHash;
      float a = dictionary.ContainsKey(12) ? (float) dictionary[12] : 0.0f;
      if ((double) a > 1638.0)
      {
        short num = (short) Math.Round((double) a);
        m_xCenterArray[index + 1] = (short) ((int) num + (int) m_xCenterArray[index]);
      }
      else if (((double) a == 0.0 || row.Cells[index].GridSpan > (short) 1) && tableGrid.Count != 0)
      {
        short num1 = 0;
        short num2 = (short) ((int) row.Cells[index].GridColumnStartIndex + (int) row.Cells[index].GridSpan);
        short columnStartIndex = row.Cells[index].GridColumnStartIndex;
        if (columnStartIndex > (short) 0 && (int) columnStartIndex < tableGrid.Count && num2 > (short) 0 && (int) num2 < tableGrid.Count)
        {
          if (row.Cells[index].GridSpan != (short) 1)
          {
            if (tableGrid.Count > (int) num2)
            {
              num1 = (short) Math.Round(columnStartIndex == (short) 0 ? (double) tableGrid[(int) num2 - 1].EndOffset : (double) tableGrid[(int) num2 - 1].EndOffset - (double) tableGrid[(int) columnStartIndex - 1].EndOffset);
              flag = true;
            }
          }
          else if (flag)
          {
            if (tableGrid.Count > (int) num2)
              num1 = (short) Math.Round(columnStartIndex == (short) 0 ? (double) tableGrid[(int) columnStartIndex].EndOffset : (double) tableGrid[(int) num2 - 1].EndOffset - (double) tableGrid[(int) columnStartIndex - 1].EndOffset);
          }
          else if (tableGrid.Count > (int) num2)
            num1 = (short) Math.Round(columnStartIndex == (short) 0 ? (double) tableGrid[(int) columnStartIndex].EndOffset : (double) tableGrid[(int) num2 - 1].EndOffset - (double) tableGrid[(int) columnStartIndex - 1].EndOffset);
        }
        else
          num1 = (short) Math.Round((double) a * 20.0);
        m_xCenterArray[index + 1] = (short) ((int) num1 + (int) m_xCenterArray[index]);
      }
      else
      {
        short num = (short) Math.Round((double) a * 20.0);
        m_xCenterArray[index + 1] = (short) ((int) num + (int) m_xCenterArray[index]);
      }
    }
  }

  private static void UpdateCenterArray(
    SinglePropertyModifierArray sprms,
    short[] m_xCenterArray,
    WTableRow tableRow,
    bool isOldFormat)
  {
    WTable ownerTable = tableRow.OwnerTable;
    short mXCenter = m_xCenterArray[0];
    short num1 = 0;
    byte[] byteArray1 = sprms.GetByteArray(63073);
    if (byteArray1 != null && byteArray1.Length == 3)
      num1 = BitConverter.ToInt16(byteArray1, 1);
    short num2 = num1;
    if (sprms.GetInt(26185, 1) == 1)
    {
      short num3 = 0;
      if (sprms.TryGetSprm(62999) != null)
      {
        byte[] byteArray2 = sprms.GetByteArray(62999);
        if (byteArray2 != null && byteArray2.Length == 3)
          num3 = BitConverter.ToInt16(byteArray2, 1);
      }
      Paddings paddings1 = (Paddings) null;
      Paddings paddings2 = (Paddings) null;
      if (ownerTable != null && ownerTable.Rows.Count > 0)
      {
        if (ownerTable.Rows[0].Cells.Count > 0)
        {
          Dictionary<int, object> dictionary = isOldFormat ? ownerTable.Rows[0].Cells[0].CellFormat.OldPropertiesHash : ownerTable.Rows[0].Cells[0].CellFormat.PropertiesHash;
          if (dictionary != null && dictionary.ContainsKey(3))
            paddings1 = dictionary[3] as Paddings;
        }
        Dictionary<int, object> dictionary1 = isOldFormat ? ownerTable.Rows[0].RowFormat.OldPropertiesHash : ownerTable.Rows[0].RowFormat.PropertiesHash;
        if (dictionary1 != null && dictionary1.ContainsKey(3))
          paddings2 = dictionary1[3] as Paddings;
      }
      if (paddings1 != null && paddings1.HasKey(1))
        num2 = (short) ((int) (short) ((int) num2 - (int) (short) ((double) paddings1.Left * 20.0)) + (int) num3);
      else if (paddings2 != null && paddings2.HasKey(1))
        num2 = (short) ((int) (short) ((int) num2 - (int) (short) ((double) paddings2.Left * 20.0)) + (int) num3);
      else
        num2 += num3;
    }
    byte[] byteArray3 = sprms.GetByteArray(54835);
    ushort num4 = 0;
    if (byteArray3 != null && byteArray3.Length == 6 && (byteArray3[3] == (byte) 3 || byteArray3[3] == (byte) 19) && byteArray3[2] == (byte) 15)
      num4 = BitConverter.ToUInt16(byteArray3, 4);
    if (num4 > (ushort) 0)
    {
      num2 -= (short) ((int) num4 * 2);
      byte[] byteArray4 = sprms.GetByteArray(54803);
      bool flag = false;
      if (byteArray4 == null)
      {
        byteArray4 = sprms.GetByteArray(54789);
        flag = true;
      }
      if (byteArray4 != null)
      {
        Syncfusion.DocIO.ReaderWriter.Biff_Records.TableBorders tableBorders = new Syncfusion.DocIO.ReaderWriter.Biff_Records.TableBorders();
        for (int index = 0; index < 2; ++index)
        {
          tableBorders[index] = new BorderCode();
          if (flag)
            tableBorders[index].Parse(byteArray4, index * 4);
          else
            tableBorders[index].ParseNewBrc(byteArray4, index * 8);
        }
        num2 -= (short) Math.Round((double) tableBorders.LeftBorder.LineWidth / 8.0 * 20.0);
      }
    }
    if ((int) mXCenter == (int) num2)
      return;
    m_xCenterArray[0] = num2;
    for (int index = 1; index < m_xCenterArray.Length; ++index)
      m_xCenterArray[index] += (short) ((int) num2 - (int) mXCenter);
  }

  private static void InitCollection(int cellCount)
  {
    TablePropertiesConverter.m_cellShadings = new byte[cellCount * 2];
    TablePropertiesConverter.m_cellTCGRF = new Dictionary<short, byte[]>(cellCount);
    TablePropertiesConverter.m_cellWidth = new Dictionary<short, byte[]>(cellCount);
    TablePropertiesConverter.m_cellBorders = new Dictionary<short, byte[]>(cellCount);
    TablePropertiesConverter.m_topBorderCV = new byte[cellCount * 4];
    TablePropertiesConverter.m_leftBorderCV = new byte[cellCount * 4];
    TablePropertiesConverter.m_rightBorderCV = new byte[cellCount * 4];
    TablePropertiesConverter.m_bottomBorderCV = new byte[cellCount * 4];
    TablePropertiesConverter.UpdateDefaultBorderCV(cellCount);
    if (cellCount < 23)
    {
      TablePropertiesConverter.m_cellShadings_1st = new byte[cellCount * 10];
      TablePropertiesConverter.UpdateDefaultShadingBytes(TablePropertiesConverter.m_cellShadings_1st, cellCount);
    }
    else if (cellCount < 45)
    {
      TablePropertiesConverter.m_cellShadings_1st = new byte[220];
      TablePropertiesConverter.UpdateDefaultShadingBytes(TablePropertiesConverter.m_cellShadings_1st, 22);
      TablePropertiesConverter.m_cellShadings_2nd = new byte[(cellCount - 22) * 10];
      TablePropertiesConverter.UpdateDefaultShadingBytes(TablePropertiesConverter.m_cellShadings_2nd, cellCount - 22);
    }
    else
    {
      TablePropertiesConverter.m_cellShadings_1st = new byte[220];
      TablePropertiesConverter.UpdateDefaultShadingBytes(TablePropertiesConverter.m_cellShadings_1st, 22);
      TablePropertiesConverter.m_cellShadings_2nd = new byte[220];
      TablePropertiesConverter.UpdateDefaultShadingBytes(TablePropertiesConverter.m_cellShadings_2nd, 22);
      TablePropertiesConverter.m_cellShadings_3rd = new byte[(cellCount - 44) * 10];
      TablePropertiesConverter.UpdateDefaultShadingBytes(TablePropertiesConverter.m_cellShadings_3rd, cellCount - 44);
    }
  }

  private static void UpdateDefaultBorderCV(int cellCount)
  {
    byte[] src = new byte[4]
    {
      (byte) 0,
      (byte) 0,
      (byte) 0,
      byte.MaxValue
    };
    int num = 0;
    int dstOffset = 0;
    for (; num < cellCount; ++num)
    {
      Buffer.BlockCopy((Array) src, 0, (Array) TablePropertiesConverter.m_topBorderCV, dstOffset, 4);
      Buffer.BlockCopy((Array) src, 0, (Array) TablePropertiesConverter.m_bottomBorderCV, dstOffset, 4);
      Buffer.BlockCopy((Array) src, 0, (Array) TablePropertiesConverter.m_leftBorderCV, dstOffset, 4);
      Buffer.BlockCopy((Array) src, 0, (Array) TablePropertiesConverter.m_rightBorderCV, dstOffset, 4);
      dstOffset += 4;
    }
  }

  private static void UpdateDefaultShadingBytes(byte[] operand, int cellCount)
  {
    byte[] src = new byte[4]
    {
      (byte) 0,
      (byte) 0,
      (byte) 0,
      byte.MaxValue
    };
    int num = 0;
    int dstOffset1 = 0;
    for (; num < cellCount; ++num)
    {
      Buffer.BlockCopy((Array) src, 0, (Array) operand, dstOffset1, 4);
      int dstOffset2 = dstOffset1 + 4;
      Buffer.BlockCopy((Array) src, 0, (Array) operand, dstOffset2, 4);
      dstOffset1 = dstOffset2 + 4 + 2;
    }
  }

  private static void UpdateCellsInformation(SinglePropertyModifierArray sprms)
  {
    if (TablePropertiesConverter.m_cellShadings != null)
    {
      sprms.SetByteArrayValue(54793, TablePropertiesConverter.m_cellShadings.Clone() as byte[]);
      Array.Clear((Array) TablePropertiesConverter.m_cellShadings, 0, TablePropertiesConverter.m_cellShadings.Length);
      TablePropertiesConverter.m_cellShadings = (byte[]) null;
    }
    if (TablePropertiesConverter.m_cellShadings_1st != null)
    {
      sprms.SetByteArrayValue(54896, TablePropertiesConverter.m_cellShadings_1st.Clone() as byte[]);
      sprms.SetByteArrayValue(54802, TablePropertiesConverter.m_cellShadings_1st.Clone() as byte[]);
      Array.Clear((Array) TablePropertiesConverter.m_cellShadings_1st, 0, TablePropertiesConverter.m_cellShadings_1st.Length);
      TablePropertiesConverter.m_cellShadings_1st = (byte[]) null;
    }
    if (TablePropertiesConverter.m_cellShadings_2nd != null)
    {
      sprms.SetByteArrayValue(54897, TablePropertiesConverter.m_cellShadings_2nd.Clone() as byte[]);
      sprms.SetByteArrayValue(54806, TablePropertiesConverter.m_cellShadings_2nd.Clone() as byte[]);
      Array.Clear((Array) TablePropertiesConverter.m_cellShadings_2nd, 0, TablePropertiesConverter.m_cellShadings_2nd.Length);
      TablePropertiesConverter.m_cellShadings_2nd = (byte[]) null;
    }
    if (TablePropertiesConverter.m_cellShadings_3rd != null)
    {
      sprms.SetByteArrayValue(54898, TablePropertiesConverter.m_cellShadings_3rd.Clone() as byte[]);
      sprms.SetByteArrayValue(54796, TablePropertiesConverter.m_cellShadings_3rd.Clone() as byte[]);
      Array.Clear((Array) TablePropertiesConverter.m_cellShadings_3rd, 0, TablePropertiesConverter.m_cellShadings_3rd.Length);
      TablePropertiesConverter.m_cellShadings_3rd = (byte[]) null;
    }
    if (TablePropertiesConverter.m_topBorderCV != null)
    {
      sprms.SetByteArrayValue(54810, TablePropertiesConverter.m_topBorderCV.Clone() as byte[]);
      Array.Clear((Array) TablePropertiesConverter.m_topBorderCV, 0, TablePropertiesConverter.m_topBorderCV.Length);
      TablePropertiesConverter.m_topBorderCV = (byte[]) null;
    }
    if (TablePropertiesConverter.m_leftBorderCV != null)
    {
      sprms.SetByteArrayValue(54811, TablePropertiesConverter.m_leftBorderCV.Clone() as byte[]);
      Array.Clear((Array) TablePropertiesConverter.m_leftBorderCV, 0, TablePropertiesConverter.m_leftBorderCV.Length);
      TablePropertiesConverter.m_leftBorderCV = (byte[]) null;
    }
    if (TablePropertiesConverter.m_bottomBorderCV != null)
    {
      sprms.SetByteArrayValue(54812, TablePropertiesConverter.m_bottomBorderCV.Clone() as byte[]);
      Array.Clear((Array) TablePropertiesConverter.m_bottomBorderCV, 0, TablePropertiesConverter.m_bottomBorderCV.Length);
      TablePropertiesConverter.m_bottomBorderCV = (byte[]) null;
    }
    if (TablePropertiesConverter.m_rightBorderCV != null)
    {
      sprms.SetByteArrayValue(54813, TablePropertiesConverter.m_rightBorderCV.Clone() as byte[]);
      Array.Clear((Array) TablePropertiesConverter.m_rightBorderCV, 0, TablePropertiesConverter.m_rightBorderCV.Length);
      TablePropertiesConverter.m_rightBorderCV = (byte[]) null;
    }
    if (TablePropertiesConverter.m_cellBorderType == null)
      return;
    sprms.SetByteArrayValue(54882, TablePropertiesConverter.m_cellBorderType.Clone() as byte[]);
    Array.Clear((Array) TablePropertiesConverter.m_cellBorderType, 0, TablePropertiesConverter.m_cellBorderType.Length);
    TablePropertiesConverter.m_cellBorderType = (byte[]) null;
  }

  internal static void FormatToSprms(
    int key,
    object value,
    SinglePropertyModifierArray sprms,
    RowFormat rowFormat,
    WordStyleSheet styleSheet,
    bool isOldFormat)
  {
    lock (TablePropertiesConverter.m_threadLocker)
    {
      switch (key)
      {
        case 1:
          TablePropertiesConverter.SetTableBorders(sprms, (Borders) value);
          break;
        case 2:
          short num1 = (short) Math.Round((double) (float) value * 20.0);
          short num2 = rowFormat.OwnerRow.HeightType == TableRowHeightType.AtLeast ? num1 : -num1;
          sprms.SetShortValue(37895, num2);
          break;
        case 3:
          TablePropertiesConverter.SetPaddings(sprms, 54836, (Paddings) value, 0);
          break;
        case 11:
        case 12:
          TablePropertiesConverter.SetPreferredWidthInfo(11, 12, 62996, sprms, rowFormat, isOldFormat);
          break;
        case 13:
        case 14:
          TablePropertiesConverter.SetPreferredWidthInfo(13, 14, 62999, sprms, rowFormat, isOldFormat);
          break;
        case 15:
        case 16 /*0x10*/:
          TablePropertiesConverter.SetPreferredWidthInfo(15, 16 /*0x10*/, 63000, sprms, rowFormat, isOldFormat);
          break;
        case 52:
          if ((double) (float) value < 0.0)
            break;
          byte[] dst1 = new byte[6]
          {
            (byte) 0,
            (byte) 1,
            (byte) 15,
            (byte) 3,
            (byte) 0,
            (byte) 0
          };
          Buffer.BlockCopy((Array) BitConverter.GetBytes((short) ((double) (float) value * 20.0)), 0, (Array) dst1, 4, 2);
          sprms.SetByteArrayValue(54835, dst1);
          break;
        case 53:
          byte[] dst2 = new byte[3]
          {
            (byte) 3,
            (byte) 0,
            (byte) 0
          };
          Buffer.BlockCopy((Array) BitConverter.GetBytes((short) Math.Round((double) (float) value * 20.0)), 0, (Array) dst2, 1, 2);
          sprms.SetByteArrayValue(63073, dst2);
          break;
        case 103:
          sprms.SetBoolValue(13845, (bool) value);
          break;
        case 104:
          sprms.SetBoolValue(22027, (bool) value);
          break;
        case 105:
          sprms.SetShortValue(21642, (short) (ParagraphJustify) value);
          sprms.SetShortValue(21504, (short) (ParagraphJustify) value);
          break;
        case 106:
          sprms.SetBoolValue(13315, !(bool) value);
          sprms.SetBoolValue(13414, !(bool) value);
          break;
        case 107:
          sprms.SetBoolValue(13316, (bool) value);
          break;
        case 108:
        case 110:
        case 111:
          TablePropertiesConverter.SetTableShading(sprms, 54880, rowFormat, rowFormat.IsFormattingChange);
          break;
        case 120:
          TablePropertiesConverter.SetTablePositioning((RowFormat.TablePositioning) value, sprms);
          break;
        case 123:
        case 124:
          if (sprms.Contain(51849) || !rowFormat.IsChangedFormat)
            break;
          byte[] dst3 = new byte[7];
          dst3[0] = (byte) 1;
          if (!CharacterPropertiesConverter.AuthorNames.Contains(rowFormat.FormatChangeAuthorName))
            CharacterPropertiesConverter.AuthorNames.Add(rowFormat.FormatChangeAuthorName);
          byte[] bytes = BitConverter.GetBytes((short) CharacterPropertiesConverter.AuthorNames.IndexOf(rowFormat.FormatChangeAuthorName));
          byte[] src = new byte[4];
          if (rowFormat.HasValue(15))
            src = BitConverter.GetBytes(rowFormat.GetDTTMIntValue(rowFormat.FormatChangeDateTime));
          Buffer.BlockCopy((Array) bytes, 0, (Array) dst3, 1, 2);
          Buffer.BlockCopy((Array) src, 0, (Array) dst3, 3, 4);
          sprms.SetByteArrayValue(54887, dst3);
          break;
      }
    }
  }

  private static void FormatToSprms(
    int key,
    object value,
    SinglePropertyModifierArray sprms,
    CellFormat cellFormat,
    bool isOldFormat,
    int cellIndex,
    ref ushort tcgrf)
  {
    Dictionary<int, object> dictionary = isOldFormat ? cellFormat.OldPropertiesHash : cellFormat.PropertiesHash;
    lock (TablePropertiesConverter.m_threadLocker)
    {
      switch (key)
      {
        case 1:
          TablePropertiesConverter.SetCellBorders((Borders) value, cellIndex, cellFormat);
          break;
        case 2:
          tcgrf = (ushort) ((int) tcgrf & 65151 | (int) (byte) (VerticalAlignment) value << 7);
          break;
        case 3:
          if (cellFormat.SamePaddingsAsTable)
            break;
          TablePropertiesConverter.SetPaddings(sprms, 54834, (Paddings) value, cellIndex);
          break;
        case 6:
          switch ((CellMerge) value)
          {
            case CellMerge.Start:
              tcgrf = (ushort) ((int) tcgrf & 65439 | 96 /*0x60*/);
              return;
            case CellMerge.Continue:
              tcgrf = (ushort) ((int) tcgrf & 65439 | 32 /*0x20*/);
              return;
            default:
              return;
          }
        case 8:
          switch ((CellMerge) value)
          {
            case CellMerge.Start:
              tcgrf = (ushort) ((int) tcgrf & 65532 | 1);
              return;
            case CellMerge.Continue:
              tcgrf = (ushort) ((int) tcgrf & 65532 | 2);
              return;
            default:
              return;
          }
        case 9:
          tcgrf = (ushort) ((int) tcgrf & 57343 /*0xDFFF*/ | ((bool) value ? 0 : 8192 /*0x2000*/));
          break;
        case 10:
          tcgrf = (ushort) ((int) tcgrf & 61439 /*0xEFFF*/ | ((bool) value ? 1 : 0));
          break;
        case 11:
          switch ((TextDirection) value)
          {
            case TextDirection.VerticalFarEast:
              tcgrf = (ushort) ((int) tcgrf & 65507 | 20);
              return;
            case TextDirection.VerticalBottomToTop:
              tcgrf = (ushort) ((int) tcgrf & 65507 | 12);
              return;
            case TextDirection.VerticalTopToBottom:
              tcgrf = (ushort) ((int) tcgrf & 65507 | 4);
              return;
            case TextDirection.HorizontalFarEast:
              tcgrf = (ushort) ((int) tcgrf & 65507 | 16 /*0x10*/);
              return;
            case TextDirection.Vertical:
              tcgrf = (ushort) ((int) tcgrf & 65507 | 28);
              return;
            default:
              return;
          }
        case 13:
          tcgrf = (ushort) ((int) tcgrf & 61951 | (int) (byte) (FtsWidth) value << 9);
          break;
        case 14:
          if (!dictionary.ContainsKey(13))
            break;
          ushort num = 0;
          switch ((FtsWidth) dictionary[13])
          {
            case FtsWidth.Percentage:
              num = (ushort) Math.Round((double) (float) value * 50.0);
              break;
            case FtsWidth.Point:
              num = (ushort) Math.Round((double) (float) value * 20.0);
              break;
          }
          TablePropertiesConverter.m_cellWidth[(short) cellIndex] = BitConverter.GetBytes(num);
          break;
      }
    }
  }

  private static void SetShading(
    Dictionary<int, object> propertyHash,
    int cellIndex,
    bool isRowFormat)
  {
    int key1;
    int key2;
    int key3;
    if (isRowFormat)
    {
      key1 = 108;
      key2 = 111;
      key3 = 110;
    }
    else
    {
      key1 = 4;
      key2 = 5;
      key3 = 7;
    }
    ShadingDescriptor shadingDescriptor = new ShadingDescriptor();
    if (propertyHash.ContainsKey(key1))
      shadingDescriptor.BackColor = (Color) propertyHash[key1];
    if (propertyHash.ContainsKey(key2))
      shadingDescriptor.ForeColor = (Color) propertyHash[key2];
    if (propertyHash.ContainsKey(key3))
      shadingDescriptor.Pattern = (TextureStyle) propertyHash[key3];
    Buffer.BlockCopy((Array) BitConverter.GetBytes(shadingDescriptor.Save()), 0, (Array) TablePropertiesConverter.m_cellShadings, cellIndex * 2, 2);
    if (cellIndex < 22)
      Buffer.BlockCopy((Array) shadingDescriptor.SaveNewShd(), 0, (Array) TablePropertiesConverter.m_cellShadings_1st, cellIndex * 10, 10);
    else if (cellIndex < 44)
    {
      Buffer.BlockCopy((Array) shadingDescriptor.SaveNewShd(), 0, (Array) TablePropertiesConverter.m_cellShadings_2nd, (cellIndex - 22) * 10, 10);
    }
    else
    {
      if (cellIndex >= 63 /*0x3F*/)
        return;
      Buffer.BlockCopy((Array) shadingDescriptor.SaveNewShd(), 0, (Array) TablePropertiesConverter.m_cellShadings_3rd, (cellIndex - 44) * 10, 10);
    }
  }

  private static void SetTableBorders(SinglePropertyModifierArray sprms, Borders borders)
  {
    Syncfusion.DocIO.ReaderWriter.Biff_Records.TableBorders tableBorders = new Syncfusion.DocIO.ReaderWriter.Biff_Records.TableBorders();
    TablePropertiesConverter.BorderToBRC(borders.Top, tableBorders.TopBorder);
    TablePropertiesConverter.BorderToBRC(borders.Left, tableBorders.LeftBorder);
    TablePropertiesConverter.BorderToBRC(borders.Bottom, tableBorders.BottomBorder);
    TablePropertiesConverter.BorderToBRC(borders.Right, tableBorders.RightBorder);
    TablePropertiesConverter.BorderToBRC(borders.Horizontal, tableBorders.HorizontalBorder);
    TablePropertiesConverter.BorderToBRC(borders.Vertical, tableBorders.VerticalBorder);
    byte[] arr1 = new byte[24];
    for (int index = 0; index < 6; ++index)
      tableBorders[index].SaveBytes(arr1, index * 4);
    sprms.SetByteArrayValue(54789, arr1);
    byte[] arr2 = new byte[48 /*0x30*/];
    for (int index = 0; index < 6; ++index)
      tableBorders[index].SaveNewBrc(arr2, index * 8);
    sprms.SetByteArrayValue(54803, arr2);
  }

  private static void SetCellBorders(Borders borders, int cellIndex, CellFormat cellFormat)
  {
    int index1 = cellIndex * 4;
    BorderStructure[] borderStructureArray = new BorderStructure[4];
    for (int index2 = 0; index2 < borderStructureArray.Length; ++index2)
      borderStructureArray[index2] = new BorderStructure();
    if (borders.Left.BorderType == BorderStyle.Thick)
      borders.Left.BorderType = BorderStyle.Single;
    if (borders.Right.BorderType == BorderStyle.Thick)
      borders.Right.BorderType = BorderStyle.Single;
    if (borders.Top.BorderType == BorderStyle.Thick)
      borders.Top.BorderType = BorderStyle.Single;
    if (borders.Bottom.BorderType == BorderStyle.Thick)
      borders.Bottom.BorderType = BorderStyle.Single;
    if (borders.Left.BorderType != BorderStyle.None || borders.Right.BorderType != BorderStyle.None || borders.Top.BorderType != BorderStyle.None || borders.Bottom.BorderType != BorderStyle.None)
    {
      if (borders.Top.BorderType != BorderStyle.None || borders.Top.HasNoneStyle)
        TablePropertiesConverter.BorderToBRC(borders.Top, borderStructureArray[0]);
      if (borders.Left.BorderType != BorderStyle.None || borders.Left.HasNoneStyle)
        TablePropertiesConverter.BorderToBRC(borders.Left, borderStructureArray[1]);
      if (borders.Bottom.BorderType != BorderStyle.None || borders.Bottom.HasNoneStyle)
        TablePropertiesConverter.BorderToBRC(borders.Bottom, borderStructureArray[2]);
      if (borders.Right.BorderType != BorderStyle.None || borders.Right.HasNoneStyle)
        TablePropertiesConverter.BorderToBRC(borders.Right, borderStructureArray[3]);
      TablePropertiesConverter.SetCellBorderColors(borders, index1);
    }
    else
    {
      if (borders.Left.HasNoneStyle && borders.Left.BorderType == BorderStyle.None && !borders.Left.Color.IsEmpty && borders.Left.Color != Color.Black)
        Buffer.BlockCopy((Array) BitConverter.GetBytes(WordColor.ConvertColorToRGB(Color.White)), 0, (Array) TablePropertiesConverter.m_leftBorderCV, index1, 4);
      if (borders.Right.HasNoneStyle && borders.Right.BorderType == BorderStyle.None && !borders.Right.Color.IsEmpty && borders.Right.Color != Color.Black)
        Buffer.BlockCopy((Array) BitConverter.GetBytes(WordColor.ConvertColorToRGB(Color.White)), 0, (Array) TablePropertiesConverter.m_rightBorderCV, index1, 4);
      if (borders.Top.HasNoneStyle && borders.Top.BorderType == BorderStyle.None && !borders.Top.Color.IsEmpty && borders.Top.Color != Color.Black)
        Buffer.BlockCopy((Array) BitConverter.GetBytes(WordColor.ConvertColorToRGB(Color.White)), 0, (Array) TablePropertiesConverter.m_topBorderCV, index1, 4);
      if (borders.Bottom.HasNoneStyle && borders.Bottom.BorderType == BorderStyle.None && !borders.Bottom.Color.IsEmpty && borders.Bottom.Color != Color.Black)
        Buffer.BlockCopy((Array) BitConverter.GetBytes(WordColor.ConvertColorToRGB(Color.White)), 0, (Array) TablePropertiesConverter.m_bottomBorderCV, index1, 4);
    }
    byte[] arr = new byte[16 /*0x10*/];
    for (int index3 = 0; index3 < 4; ++index3)
      borderStructureArray[index3].Save(arr, index3 * 4);
    TablePropertiesConverter.m_cellBorders[(short) cellIndex] = arr;
    if ((borders.Left.BorderType == BorderStyle.None ? (borders.Left.HasNoneStyle ? 1 : 0) : 1) == 0 && (borders.Top.BorderType == BorderStyle.None ? (borders.Top.HasNoneStyle ? 1 : 0) : 1) == 0 && (borders.Right.BorderType == BorderStyle.None ? (borders.Right.HasNoneStyle ? 1 : 0) : 1) == 0 && (borders.Bottom.BorderType == BorderStyle.None ? (borders.Bottom.HasNoneStyle ? 1 : 0) : 1) == 0)
      return;
    if (TablePropertiesConverter.m_cellBorderType == null)
      TablePropertiesConverter.m_cellBorderType = new byte[(cellFormat.OwnerBase.OwnerBase as WTableRow).Cells.Count * 4];
    TablePropertiesConverter.m_cellBorderType[index1] = (byte) borders.Top.BorderType;
    TablePropertiesConverter.m_cellBorderType[index1 + 1] = (byte) borders.Left.BorderType;
    TablePropertiesConverter.m_cellBorderType[index1 + 2] = (byte) borders.Bottom.BorderType;
    TablePropertiesConverter.m_cellBorderType[index1 + 3] = (byte) borders.Right.BorderType;
  }

  private static void SetCellBorderColors(Borders borders, int startPos)
  {
    if (borders.Top.HasKey(1))
      Buffer.BlockCopy((Array) BitConverter.GetBytes(WordColor.ConvertColorToRGB(borders.Top.Color)), 0, (Array) TablePropertiesConverter.m_topBorderCV, startPos, 4);
    if (borders.Left.HasKey(1))
      Buffer.BlockCopy((Array) BitConverter.GetBytes(WordColor.ConvertColorToRGB(borders.Left.Color)), 0, (Array) TablePropertiesConverter.m_leftBorderCV, startPos, 4);
    if (borders.Bottom.HasKey(1))
      Buffer.BlockCopy((Array) BitConverter.GetBytes(WordColor.ConvertColorToRGB(borders.Bottom.Color)), 0, (Array) TablePropertiesConverter.m_bottomBorderCV, startPos, 4);
    if (!borders.Right.HasKey(1))
      return;
    Buffer.BlockCopy((Array) BitConverter.GetBytes(WordColor.ConvertColorToRGB(borders.Right.Color)), 0, (Array) TablePropertiesConverter.m_rightBorderCV, startPos, 4);
  }

  private static void SetPreferredWidthInfo(
    int widthTypeKey,
    int widthKey,
    int sprmOption,
    SinglePropertyModifierArray sprms,
    RowFormat rowFormat,
    bool isOldFormat)
  {
    if (sprms[sprmOption] != null)
      return;
    byte[] dst = new byte[3];
    FtsWidth ftsWidth = isOldFormat ? (FtsWidth) rowFormat.OldPropertiesHash[widthTypeKey] : (FtsWidth) rowFormat.PropertiesHash[widthTypeKey];
    dst[0] = (byte) ftsWidth;
    float num1 = isOldFormat ? (float) rowFormat.GetKeyValue(rowFormat.OldPropertiesHash, widthKey) : (float) rowFormat.GetKeyValue(rowFormat.PropertiesHash, widthKey);
    short num2 = 0;
    if (ftsWidth == FtsWidth.Percentage)
      num2 = (short) Math.Round((double) num1 * 50.0);
    else if (ftsWidth == FtsWidth.Point)
      num2 = (short) Math.Round((double) num1 * 20.0);
    if (ftsWidth > FtsWidth.Auto)
      Buffer.BlockCopy((Array) BitConverter.GetBytes(num2), 0, (Array) dst, 1, 2);
    sprms.SetByteArrayValue(sprmOption, dst);
  }

  internal static void SetTablePositioning(
    RowFormat.TablePositioning tablePositioning,
    SinglePropertyModifierArray sprms)
  {
    if (!tablePositioning.m_ownerRowFormat.WrapTextAround)
      return;
    sprms.SetByteValue(13837, (byte) (((int) (byte) tablePositioning.HorizRelationTo << 2 | (int) (byte) tablePositioning.VertRelationTo) << 4));
    short num1 = tablePositioning.HorizPositionAbs != HorizontalPosition.Left ? (short) tablePositioning.HorizPositionAbs : (short) (tablePositioning.HorizPosition * 20f);
    sprms.SetShortValue(37902, num1);
    short num2 = tablePositioning.VertPositionAbs != VerticalPosition.None ? (short) tablePositioning.VertPositionAbs : (short) (tablePositioning.VertPosition * 20f);
    sprms.SetShortValue(37903, num2);
    if ((double) tablePositioning.DistanceFromLeft != 0.0)
      sprms.SetShortValue(37904, (short) (tablePositioning.DistanceFromLeft * 20f));
    if ((double) tablePositioning.DistanceFromTop != 0.0)
      sprms.SetShortValue(37905, (short) (tablePositioning.DistanceFromTop * 20f));
    if ((double) tablePositioning.DistanceFromRight != 0.0)
      sprms.SetShortValue(37918, (short) (tablePositioning.DistanceFromRight * 20f));
    if ((double) tablePositioning.DistanceFromBottom != 0.0)
      sprms.SetShortValue(37919, (short) (tablePositioning.DistanceFromBottom * 20f));
    if (tablePositioning.AllowOverlap)
      return;
    sprms.SetBoolValue(13413, !tablePositioning.AllowOverlap);
  }

  private static void SetPaddings(
    SinglePropertyModifierArray sprms,
    int options,
    Paddings paddings,
    int cellIndex)
  {
    Spacings destination = new Spacings();
    TablePropertiesConverter.ImportPaddings(destination, paddings);
    if (destination.IsEmpty)
      return;
    destination.CellNumber = cellIndex;
    destination.Save(sprms, options, cellIndex);
  }

  private static void SetTableShading(
    SinglePropertyModifierArray sprms,
    int options,
    RowFormat rowFormat,
    bool isChangedFormat)
  {
    if (sprms.Contain(54880))
      return;
    Dictionary<int, object> dictionary = isChangedFormat ? rowFormat.OldPropertiesHash : rowFormat.PropertiesHash;
    ShadingDescriptor shadingDescriptor = new ShadingDescriptor();
    if (dictionary.ContainsKey(108))
      shadingDescriptor.BackColor = (Color) dictionary[108];
    if (dictionary.ContainsKey(111))
      shadingDescriptor.ForeColor = (Color) dictionary[111];
    if (dictionary.ContainsKey(110))
      shadingDescriptor.Pattern = (TextureStyle) dictionary[110];
    sprms.SetByteArrayValue(options, shadingDescriptor.SaveNewShd());
  }

  private static void BorderToBRC(Border border, BorderCode brc)
  {
    if (border.BorderType == BorderStyle.Cleared)
    {
      border.Color = Color.Empty;
      border.LineWidth = 0.0f;
    }
    else if (border.BorderType == BorderStyle.None && !border.HasNoneStyle)
      border.BorderType = BorderStyle.Single;
    else if (border.BorderType == BorderStyle.Hairline)
      border.BorderType = BorderStyle.Single;
    if (border.IsDefault)
      return;
    if (border.BorderType == BorderStyle.Cleared)
    {
      brc.BorderType = byte.MaxValue;
      brc.LineColor = (byte) 0;
      brc.LineColorExt = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      brc.LineWidth = byte.MaxValue;
    }
    else if (border.BorderType != BorderStyle.None)
    {
      brc.BorderType = (byte) border.BorderType;
      brc.LineColor = (byte) WordColor.ConvertColorToId(border.Color);
      brc.LineColorExt = border.Color;
      brc.LineWidth = (byte) ((double) border.LineWidth * 8.0);
    }
    brc.Shadow = border.Shadow;
  }

  private static void BorderToBRC(Border border, BorderStructure brc)
  {
    if (border.BorderType == BorderStyle.Hairline)
      border.BorderType = BorderStyle.Single;
    if (!border.IsDefault && border.BorderType == BorderStyle.Cleared)
    {
      brc.BorderType = byte.MaxValue;
      brc.LineColor = byte.MaxValue;
      brc.LineWidth = byte.MaxValue;
      brc.Props = byte.MaxValue;
    }
    else
    {
      if (border.IsDefault || border.BorderType == BorderStyle.None && !border.HasNoneStyle)
        return;
      if (brc.BorderType == byte.MaxValue && (byte) border.BorderType != byte.MaxValue)
        brc.Props = (byte) 0;
      brc.BorderType = (byte) border.BorderType;
      brc.LineWidth = (byte) ((double) border.LineWidth * 8.0);
      brc.Shadow = border.Shadow;
      brc.LineColor = (byte) WordColor.ColorToId(border.Color);
    }
  }

  private static void BRCToBorder(BorderCode brc, Border border)
  {
    Color lineColorExt = brc.LineColorExt;
    float lineWidth = (float) brc.LineWidth / 8f;
    BorderStyle borderType = (BorderStyle) brc.BorderType;
    bool shadow = brc.Shadow;
    border.InitFormatting(lineColorExt, lineWidth, borderType, shadow);
  }

  private static void BRCToBorder(BorderStructure brc, Border border)
  {
    if (TablePropertiesConverter.IsEmpty(brc))
      return;
    if (!brc.IsClear)
    {
      Color color = WordColor.IdToColor((int) brc.LineColor);
      float lineWidth = (float) brc.LineWidth / 8f;
      BorderStyle borderType = (BorderStyle) brc.BorderType;
      bool shadow = brc.Shadow;
      border.InitFormatting(color, lineWidth, borderType, shadow);
      if (border.BorderType != BorderStyle.None)
        return;
      border.HasNoneStyle = true;
    }
    else
    {
      border.BorderType = BorderStyle.Cleared;
      border.HasNoneStyle = false;
    }
  }

  private static bool IsEmpty(BorderStructure brc)
  {
    return brc.LineColor == (byte) 0 && brc.LineWidth == (byte) 0 && brc.BorderType == (byte) 0;
  }

  public static void ImportPaddings(Spacings destination, Paddings source)
  {
    destination.Left = (short) Math.Round((double) source.Left * 20.0);
    destination.Right = (short) Math.Round((double) source.Right * 20.0);
    destination.Top = (short) Math.Round((double) source.Top * 20.0);
    destination.Bottom = (short) Math.Round((double) source.Bottom * 20.0);
  }

  public static void ExportPaddings(Spacings source, Paddings destination)
  {
    if (source == null)
      return;
    if (source.Left >= (short) 0)
      destination.Left = (float) source.Left / 20f;
    if (source.Right >= (short) 0)
      destination.Right = (float) source.Right / 20f;
    if (source.Top >= (short) 0)
      destination.Top = (float) source.Top / 20f;
    if (source.Bottom < (short) 0)
      return;
    destination.Bottom = (float) source.Bottom / 20f;
  }

  internal static void ExportDefaultPaddings(Paddings destination)
  {
    destination.Left = 0.0f;
    destination.Right = 0.0f;
    destination.Top = 0.0f;
    destination.Bottom = 0.0f;
  }
}
