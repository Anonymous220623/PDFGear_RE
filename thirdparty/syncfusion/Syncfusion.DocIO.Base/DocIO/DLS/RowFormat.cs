// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.RowFormat
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using Syncfusion.DocIO.ReaderWriter;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class RowFormat : FormatBase
{
  internal const int BordersKey = 1;
  internal const int RowHeightKey = 2;
  internal const int PaddingsKey = 3;
  internal const int PreferredWidthTypeKey = 11;
  internal const int PreferredWidthKey = 12;
  internal const int GridBeforeWidthTypeKey = 13;
  internal const int GridBeforeWidthKey = 14;
  internal const int GridAfterWidthTypeKey = 15;
  internal const int GridAfterWidthKey = 16 /*0x10*/;
  internal const int CellSpacingKey = 52;
  internal const int LeftIndentKey = 53;
  internal const int SpacingBetweenCellsKey = 102;
  internal const int IsAutoResizedCellsKey = 103;
  internal const int IsBreakAcrossPagesKey = 106;
  internal const int IsHeaderRowKey = 107;
  internal const int BidiTableKey = 104;
  internal const int RowAlignmentKey = 105;
  internal const int ShadingColorKey = 108;
  internal const int ForeColorKey = 111;
  internal const int TextureStyleKey = 110;
  internal const int PositioningKey = 120;
  internal const int DEF_BORDER_COUNT = 6;
  internal const int HiddenKey = 121;
  internal const int ChangedFormatKey = 122;
  internal const int FormatChangeAuthorNameKey = 123;
  internal const int FormatChangeDateTimeKey = 124;
  private byte m_bFlags;
  private PreferredWidthInfo m_gridBeforeWidth;
  private PreferredWidthInfo m_gridAfterWidth;
  private PreferredWidthInfo m_preferredWidth;
  private List<Stream> m_xmlProps;
  internal float AfterWidth;
  internal float BeforeWidth;

  internal PreferredWidthInfo PreferredWidth
  {
    get
    {
      if (this.m_preferredWidth == null)
        this.m_preferredWidth = new PreferredWidthInfo((FormatBase) this, 11);
      return this.m_preferredWidth;
    }
  }

  internal PreferredWidthInfo GridBeforeWidth
  {
    get
    {
      if (this.m_gridBeforeWidth == null)
        this.m_gridBeforeWidth = new PreferredWidthInfo((FormatBase) this, 13);
      return this.m_gridBeforeWidth;
    }
  }

  internal PreferredWidthInfo GridAfterWidth
  {
    get
    {
      if (this.m_gridAfterWidth == null)
        this.m_gridAfterWidth = new PreferredWidthInfo((FormatBase) this, 15);
      return this.m_gridAfterWidth;
    }
  }

  internal short GridBefore
  {
    get
    {
      return (double) this.BeforeWidth > 0.0 && this.OwnerRow != null ? this.GetGridCount(-1) : (short) -1;
    }
  }

  internal short GridAfter
  {
    get
    {
      return (double) this.AfterWidth > 0.0 && this.OwnerRow != null ? this.GetGridCount(this.OwnerRow.Cells.Count) : (short) -1;
    }
  }

  internal bool Hidden
  {
    get => (bool) this.GetPropertyValue(121);
    set => this.SetPropertyValue(121, (object) value);
  }

  public Color BackColor
  {
    get => (Color) this.GetPropertyValue(108);
    set
    {
      this.SetPropertyValue(108, (object) value);
      if (this.Document == null || this.Document.IsOpening)
        return;
      if (this.OwnerBase is WTableRow)
      {
        foreach (WTableCell cell in (CollectionImpl) (this.OwnerBase as WTableRow).Cells)
          cell.CellFormat.BackColor = value;
      }
      else
      {
        if (!(this.OwnerBase is WTable))
          return;
        foreach (WTableRow row in (CollectionImpl) (this.OwnerBase as WTable).Rows)
          row.RowFormat.BackColor = value;
      }
    }
  }

  internal Color ForeColor
  {
    get => (Color) this.GetPropertyValue(111);
    set => this.SetPropertyValue(111, (object) value);
  }

  internal TextureStyle TextureStyle
  {
    get => (TextureStyle) this.GetPropertyValue(110);
    set
    {
      this.SetPropertyValue(110, (object) value);
      if (this.Document == null || this.Document.IsOpening)
        return;
      if (this.OwnerBase is WTableRow)
      {
        foreach (WTableCell cell in (CollectionImpl) (this.OwnerBase as WTableRow).Cells)
          cell.CellFormat.TextureStyle = value;
      }
      else
      {
        if (!(this.OwnerBase is WTable))
          return;
        foreach (WTableRow row in (CollectionImpl) (this.OwnerBase as WTable).Rows)
          row.RowFormat.TextureStyle = value;
      }
    }
  }

  public Borders Borders => this.GetPropertyValue(1) as Borders;

  public Paddings Paddings => this.GetPropertyValue(3) as Paddings;

  public float CellSpacing
  {
    get => (float) this.GetPropertyValue(52);
    set => this.SetPropertyValue(52, (object) value);
  }

  public float LeftIndent
  {
    get => (float) this.GetPropertyValue(53);
    set
    {
      if (((double) value < -1080.0 || (double) value > 1080.0 || float.IsNaN(value)) && this.Document != null && !this.Document.IsOpening)
        throw new ArgumentOutOfRangeException(nameof (LeftIndent), "Table Left Indent must be between -1080 pt and 1080 pt");
      this.SetPropertyValue(53, (object) value);
    }
  }

  public bool IsAutoResized
  {
    get => (bool) this.GetPropertyValue(103);
    set => this.SetPropertyValue(103, (object) value);
  }

  public bool IsBreakAcrossPages
  {
    get => (bool) this.GetPropertyValue(106);
    set => this.SetPropertyValue(106, (object) value);
  }

  internal bool IsHeaderRow
  {
    get => (bool) this.GetPropertyValue(107);
    set => this.SetPropertyValue(107, (object) value);
  }

  public bool Bidi
  {
    get => (bool) this.GetPropertyValue(104);
    set => this.SetPropertyValue(104, (object) value);
  }

  public RowAlignment HorizontalAlignment
  {
    get => (RowAlignment) this.GetPropertyValue(105);
    set => this.SetPropertyValue(105, (object) value);
  }

  internal bool SkipDefaultPadding
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal bool IsChangedFormat
  {
    get => (bool) this.GetPropertyValue(122);
    set => this.SetPropertyValue(122, (object) value);
  }

  internal WTableRow OwnerRow => this.OwnerBase as WTableRow;

  internal float Height
  {
    get => (float) this.GetPropertyValue(2);
    set => this.SetPropertyValue(2, (object) value);
  }

  internal bool CancelOnChange
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    private set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  public bool WrapTextAround
  {
    get => this.GetTextWrapAround();
    set => this.SetTextWrapAround(value);
  }

  public RowFormat.TablePositioning Positioning
  {
    get => this.GetPropertyValue(120) as RowFormat.TablePositioning;
  }

  internal List<Stream> XmlProps
  {
    get
    {
      if (this.m_xmlProps == null)
        this.m_xmlProps = new List<Stream>();
      return this.m_xmlProps;
    }
  }

  internal string FormatChangeAuthorName
  {
    get => (string) this.GetPropertyValue(123);
    set => this.SetPropertyValue(123, (object) value);
  }

  internal DateTime FormatChangeDateTime
  {
    get => (DateTime) this.GetPropertyValue(124);
    set => this.SetPropertyValue(124, (object) value);
  }

  internal bool IsLeftIndentDefined
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
  }

  public RowFormat()
  {
    this.Paddings.SetOwner((OwnerHolder) this);
    this.Positioning.SetOwner((OwnerHolder) this);
  }

  internal RowFormat(IWordDocument doc)
    : base(doc)
  {
    this.Paddings.SetOwner((OwnerHolder) this);
    this.Positioning.SetOwner((OwnerHolder) this);
  }

  internal float GetCellSpacing()
  {
    float cellSpacing = this.CellSpacing;
    if ((double) cellSpacing == -1.0)
      cellSpacing = 0.0f;
    return cellSpacing;
  }

  internal short GetGridCount(int index)
  {
    if (this.OwnerRow == null || this.OwnerRow.OwnerTable == null)
      return -1;
    float offset = 0.0f;
    WTableColumnCollection tableGrid = this.OwnerRow.OwnerTable.TableGrid;
    float num1;
    if (index == -1)
    {
      num1 = this.BeforeWidth * 20f;
    }
    else
    {
      offset += this.BeforeWidth * 20f;
      if (index > 0)
        offset += this.GetCellOffset(index);
      num1 = index >= this.OwnerRow.Cells.Count ? this.AfterWidth * 20f : this.OwnerRow.Cells[index].Width * 20f;
    }
    int num2 = (double) offset == 0.0 ? this.GetOffsetIndex(tableGrid, offset) : this.GetOffsetIndex(tableGrid, offset) + 1;
    int num3 = this.GetOffsetIndex(tableGrid, offset + num1) + 1;
    if (index >= 0 && index < this.OwnerRow.Cells.Count)
      this.OwnerRow.Cells[index].GridColumnStartIndex = (short) num2;
    return (short) (num3 - num2);
  }

  private int GetOffsetIndex(WTableColumnCollection tableGrid, float offset)
  {
    offset = (float) Math.Round((double) offset);
    int offsetIndex;
    if (tableGrid.Contains(offset))
    {
      offsetIndex = tableGrid.IndexOf(offset);
    }
    else
    {
      for (int index = 0; index < tableGrid.Count; ++index)
      {
        if ((double) tableGrid[index].EndOffset > (double) offset)
          return index;
      }
      offsetIndex = tableGrid.Count - 1;
    }
    return offsetIndex;
  }

  private float GetCellOffset(int index)
  {
    float cellOffset = 0.0f;
    int index1 = 0;
    for (int count = this.OwnerRow.Cells.Count; index1 < count && index1 != index; ++index1)
      cellOffset += this.OwnerRow.Cells[index1].Width * 20f;
    return cellOffset;
  }

  private bool GetTextWrapAround()
  {
    if (this.HasValue(64 /*0x40*/))
      return true;
    if (this.HasValue(62))
      return (double) this.Positioning.HorizPosition != 0.0;
    if (this.HasValue(65))
    {
      if (this.Positioning.VertRelationTo == VerticalRelation.Paragraph)
        return true;
      if (this.HasValue(63 /*0x3F*/))
        return (double) this.Positioning.VertPosition != 0.0;
    }
    else if (this.HasValue(63 /*0x3F*/))
      return (double) this.Positioning.VertPosition != 0.0;
    return false;
  }

  private void SetTextWrapAround(bool value)
  {
    if (value)
    {
      this.Positioning.DistanceFromLeft = 9f;
      this.Positioning.DistanceFromRight = 9f;
      this.Positioning.VertRelationTo = VerticalRelation.Paragraph;
      this.Positioning.VertPosition = 0.0f;
    }
    else
      this.ClearAbsolutePosition();
  }

  private void ClearAbsolutePosition()
  {
    if (this.PropertiesHash.ContainsKey(68))
      this.PropertiesHash.Remove(68);
    if (this.PropertiesHash.ContainsKey(69))
      this.PropertiesHash.Remove(69);
    if (this.PropertiesHash.ContainsKey(66))
      this.PropertiesHash.Remove(66);
    if (this.PropertiesHash.ContainsKey(67))
      this.PropertiesHash.Remove(67);
    if (this.PropertiesHash.ContainsKey(64 /*0x40*/))
      this.PropertiesHash.Remove(64 /*0x40*/);
    if (this.PropertiesHash.ContainsKey(62))
      this.PropertiesHash.Remove(62);
    if (this.PropertiesHash.ContainsKey(65))
      this.PropertiesHash.Remove(65);
    if (!this.PropertiesHash.ContainsKey(63 /*0x3F*/))
      return;
    this.PropertiesHash.Remove(63 /*0x3F*/);
  }

  internal object GetPropertyValue(int propertyKey) => this[propertyKey];

  internal void SetPropertyValue(int propertyKey, object value) => this[propertyKey] = value;

  internal bool HasSprms() => this.m_sprms != null;

  private short GetRowIndent()
  {
    if (this.m_sprms == null)
      return short.MinValue;
    SinglePropertyModifierRecord sprm = this.m_sprms[63073];
    return sprm == null || sprm.ByteArray == null || sprm.ByteArray.Length != 3 ? short.MinValue : BitConverter.ToInt16(sprm.ByteArray, 1);
  }

  internal override bool HasValue(int propertyKey)
  {
    if (propertyKey != 1 && propertyKey != 3 && this.HasKey(propertyKey))
      return true;
    if (this.m_sprms == null || this.m_sprms.Count == 0)
      return false;
    int sprmOption = this.GetSprmOption(propertyKey);
    return sprmOption != int.MaxValue && this.m_sprms[sprmOption] != null;
  }

  internal override int GetSprmOption(int propertyKey)
  {
    switch (propertyKey)
    {
      case 2:
        return 37895;
      case 3:
        return 54836;
      case 11:
      case 12:
        return 62996;
      case 13:
      case 14:
        return 62999;
      case 15:
      case 16 /*0x10*/:
        return 63000;
      case 53:
        return 63073;
      case 62:
        return 37902;
      case 63 /*0x3F*/:
        return 37903;
      case 64 /*0x40*/:
      case 65:
        return 13837;
      case 66:
        return 37905;
      case 67:
        return 37919;
      case 68:
        return 37904;
      case 69:
        return 37918;
      case 106:
        return 13315;
      case 107:
        return 13316;
      case 121:
        return 54850;
      default:
        return int.MaxValue;
    }
  }

  internal override void AcceptChanges()
  {
    if (this.m_sprms == null || this.m_sprms.Length <= 0)
      return;
    this.m_sprms.RemoveValue(54887);
    base.AcceptChanges();
  }

  internal float UpdateRowBeforeAfterWidth(short gridSpan, bool isAfterWidth)
  {
    if (this.OwnerRow == null || this.OwnerRow.OwnerTable == null || this.OwnerRow.OwnerTable.TableGrid == null || this.OwnerRow.OwnerTable.TableGrid.Count == 0)
      return 0.0f;
    WTableColumnCollection tableGrid = this.OwnerRow.OwnerTable.TableGrid;
    float num1 = 0.0f;
    int num2 = isAfterWidth ? tableGrid.Count - (int) gridSpan : 0;
    int index = num2 + (int) gridSpan;
    if ((index < 0 || num2 < 0 || !isAfterWidth ? (index >= tableGrid.Count ? 0 : (num2 - 1 < tableGrid.Count ? 1 : 0)) : (index <= tableGrid.Count ? 1 : 0)) != 0)
      num1 = num2 == 0 ? (index == 0 ? tableGrid[index].EndOffset : tableGrid[index - 1].EndOffset) : tableGrid[index - 1].EndOffset - tableGrid[num2 - 1].EndOffset;
    return num1 / 20f;
  }

  internal void ClearPreferredWidthPropertyValue(int key)
  {
    if (!this.m_propertiesHash.ContainsKey(key))
      return;
    this.PreferredWidth.Width = 0.0f;
    this.PreferredWidth.WidthType = FtsWidth.None;
    this.m_propertiesHash.Remove(key);
  }

  internal override void ApplyBase(FormatBase baseFormat)
  {
    base.ApplyBase(baseFormat);
    this.Borders.ApplyBase((FormatBase) (baseFormat as RowFormat).Borders);
    this.Paddings.ApplyBase((FormatBase) (baseFormat as RowFormat).Paddings);
    this.Positioning.ApplyBase((FormatBase) (baseFormat as RowFormat).Positioning);
  }

  protected internal override void EnsureComposites()
  {
    this.EnsureComposites(1);
    this.EnsureComposites(3);
    this.EnsureComposites(120);
  }

  protected override FormatBase GetDefComposite(int key)
  {
    switch (key)
    {
      case 1:
        return this.GetDefComposite(1, (FormatBase) new Borders((FormatBase) this, 1));
      case 3:
        return this.GetDefComposite(3, (FormatBase) new Paddings((FormatBase) this, 3));
      case 120:
        return this.GetDefComposite(120, (FormatBase) new RowFormat.TablePositioning((FormatBase) this, 120));
      default:
        return (FormatBase) null;
    }
  }

  protected override object GetDefValue(int key)
  {
    switch (key)
    {
      case 2:
      case 12:
      case 14:
      case 16 /*0x10*/:
        return (object) 0.0f;
      case 11:
      case 13:
      case 15:
        return (object) FtsWidth.None;
      case 52:
        return (object) -1f;
      case 53:
        return (object) 0.0f;
      case 62:
      case 63 /*0x3F*/:
      case 66:
      case 67:
        return (object) 0.0f;
      case 64 /*0x40*/:
        return (object) HorizontalRelation.Column;
      case 65:
        return (object) VerticalRelation.Margin;
      case 68:
      case 69:
        return (object) 0.0f;
      case 70:
      case 106:
        return (object) true;
      case 102:
        return (object) 0.0f;
      case 103:
        return (object) false;
      case 104:
      case 107:
      case 121:
        return (object) false;
      case 105:
        bool flag = this.PropertiesHash.ContainsKey(62);
        if (flag && (double) this.Positioning.HorizPosition == -4.0)
          return (object) RowAlignment.Center;
        return flag && (double) this.Positioning.HorizPosition == -8.0 ? (object) RowAlignment.Right : (object) RowAlignment.Left;
      case 108:
      case 111:
        return (object) Color.Empty;
      case 110:
        return (object) TextureStyle.TextureNone;
      case 122:
        return (object) false;
      case 123:
        return (object) string.Empty;
      case 124:
        return (object) DateTime.MinValue;
      default:
        throw new NotImplementedException();
    }
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    if (reader.HasAttribute("CellSpacing"))
      this.CellSpacing = reader.ReadFloat("CellSpacing");
    if (reader.HasAttribute("LeftOffset"))
      this.LeftIndent = reader.ReadFloat("LeftOffset");
    if (reader.HasAttribute("HAlignment"))
      this.HorizontalAlignment = (RowAlignment) reader.ReadEnum("HAlignment", typeof (RowAlignment));
    if (reader.HasAttribute("IsAutoResized"))
      this.IsAutoResized = reader.ReadBoolean("IsAutoResized");
    if (reader.HasAttribute("IsBreakAcrossPages"))
      this.IsBreakAcrossPages = reader.ReadBoolean("IsBreakAcrossPages");
    if (!reader.HasAttribute("BidiTable"))
      return;
    this.Bidi = reader.ReadBoolean("BidiTable");
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    if (this.m_sprms != null)
      return;
    base.WriteXmlAttributes(writer);
    if ((double) this.CellSpacing != -1.0)
      writer.WriteValue("CellSpacing", this.CellSpacing);
    if ((double) this.LeftIndent != 0.0)
      writer.WriteValue("LeftOffset", this.LeftIndent);
    if (this.HorizontalAlignment != RowAlignment.Left)
      writer.WriteValue("HAlignment", (Enum) this.HorizontalAlignment);
    if (this.IsAutoResized)
      writer.WriteValue("IsAutoResized", this.IsAutoResized);
    if (this.IsBreakAcrossPages)
      writer.WriteValue("IsBreakAcrossPages", this.IsBreakAcrossPages);
    if (!this.Bidi)
      return;
    writer.WriteValue("BidiTable", this.Bidi);
  }

  protected override void InitXDLSHolder()
  {
    if (this.m_sprms != null)
      return;
    this.XDLSHolder.AddElement("borders", (object) this.Borders);
    this.XDLSHolder.AddElement("Paddings", (object) this.Paddings);
  }

  protected override void WriteXmlContent(IXDLSContentWriter writer)
  {
    base.WriteXmlContent(writer);
    if (this.m_sprms == null)
      return;
    byte[] arrData = new byte[this.m_sprms.Length];
    this.m_sprms.Save(arrData, 0);
    writer.WriteChildBinaryElement("internal-data", arrData);
  }

  protected override bool ReadXmlContent(IXDLSContentReader reader)
  {
    bool flag = base.ReadXmlContent(reader);
    if (reader.TagName == "internal-data")
    {
      TablePropertiesConverter.SprmsToFormat(new SinglePropertyModifierArray(reader.ReadChildBinaryElement()), this, (WordStyleSheet) null, (Dictionary<int, string>) null, (IWordReaderBase) null, false);
      flag = true;
    }
    return flag;
  }

  protected override void OnChange(FormatBase format, int propKey)
  {
    if (this.CancelOnChange || this.OwnerBase != null && this.OwnerBase.Document.IsOpening || this.OwnerBase == null || !(this.OwnerBase is WTable))
      return;
    (this.OwnerBase as WTable).UpdateFormat(format, propKey);
  }

  internal void RemoveRowSprms()
  {
    if (this.m_sprms == null)
      return;
    this.m_sprms.RemoveValue(54792);
    this.m_sprms.RemoveValue(54896);
    this.m_sprms.RemoveValue(54802);
    this.m_sprms.RemoveValue(54834);
    this.m_sprms.RemoveValue(54810);
    this.m_sprms.RemoveValue(54811);
    this.m_sprms.RemoveValue(54812);
    this.m_sprms.RemoveValue(54813);
  }

  protected internal new void ImportContainer(FormatBase format)
  {
    base.ImportContainer(format);
    if (!(format is RowFormat format1))
      return;
    this.ImportXmlProps(format1);
  }

  private void ImportXmlProps(RowFormat format)
  {
    if (format.m_xmlProps == null || format.m_xmlProps.Count <= 0)
      return;
    foreach (Stream xmlProp in format.XmlProps)
      this.XmlProps.Add(this.CloneStream(xmlProp));
  }

  protected override void ImportMembers(FormatBase format)
  {
    base.ImportMembers(format);
    if (!(format is RowFormat format1))
      return;
    if (format1.GridAfter != (short) -1)
      this.AfterWidth = format1.UpdateRowBeforeAfterWidth(format1.GridAfter, true);
    if (format1.GridBefore != (short) -1)
      this.BeforeWidth = format1.UpdateRowBeforeAfterWidth(format1.GridBefore, false);
    this.SkipDefaultPadding = format1.SkipDefaultPadding;
    this.CopyProperties((FormatBase) format1);
    this.EnsureComposites();
    this.IsDefault = false;
  }

  internal override void RemovePositioning()
  {
    if (this.m_sprms == null || this.m_sprms.Count <= 0)
      return;
    this.m_sprms.RemoveValue(13837);
    this.m_sprms.RemoveValue(37902);
    this.m_sprms.RemoveValue(37903);
    this.m_sprms.RemoveValue(37919);
    this.m_sprms.RemoveValue(37904);
    this.m_sprms.RemoveValue(37918);
    this.m_sprms.RemoveValue(37905);
  }

  internal override void Close()
  {
    base.Close();
    if (this.Borders != null)
      this.Borders.Close();
    if (this.Paddings != null)
      this.Paddings.Close();
    if (this.m_gridBeforeWidth != null)
    {
      this.m_gridBeforeWidth.Close();
      this.m_gridBeforeWidth = (PreferredWidthInfo) null;
    }
    if (this.m_gridAfterWidth != null)
    {
      this.m_gridAfterWidth.Close();
      this.m_gridAfterWidth = (PreferredWidthInfo) null;
    }
    if (this.m_preferredWidth != null)
    {
      this.m_preferredWidth.Close();
      this.m_preferredWidth = (PreferredWidthInfo) null;
    }
    if (this.m_xmlProps == null)
      return;
    this.m_xmlProps.Clear();
    this.m_xmlProps = (List<Stream>) null;
  }

  internal void CheckDefPadding()
  {
    if (!this.Paddings.HasKey(1))
      this.Paddings.Left = 5.4f;
    if (this.Paddings.HasKey(4))
      return;
    this.Paddings.Right = 5.4f;
  }

  public class TablePositioning : FormatBase
  {
    internal const int HorizPosKey = 62;
    internal const int VertPosKey = 63 /*0x3F*/;
    internal const int HorizRelKey = 64 /*0x40*/;
    internal const int VertRelKey = 65;
    internal const int DistanceFromTopKey = 66;
    internal const int DistanceFromBottomKey = 67;
    internal const int DistanceFromLeftKey = 68;
    internal const int DistanceFromRightKey = 69;
    internal const int AllowOverlapKey = 70;
    internal const float DEF_HORIZ_DISTANCE = 0.0f;
    internal RowFormat m_ownerRowFormat;

    internal bool AllowOverlap
    {
      get => (bool) this.GetPropertyValue(70);
      set => this.SetPropertyValue(70, (object) value);
    }

    public HorizontalPosition HorizPositionAbs
    {
      get
      {
        if ((double) this.HorizPosition == -4.0)
          return HorizontalPosition.Center;
        if ((double) this.HorizPosition == -8.0)
          return HorizontalPosition.Right;
        if ((double) this.HorizPosition == -12.0)
          return HorizontalPosition.Inside;
        return (double) this.HorizPosition == -16.0 ? HorizontalPosition.Outside : HorizontalPosition.Left;
      }
      set => this.HorizPosition = (float) value;
    }

    public VerticalPosition VertPositionAbs
    {
      get
      {
        if ((double) this.VertPosition == -4.0)
          return VerticalPosition.Top;
        if ((double) this.VertPosition == -8.0)
          return VerticalPosition.Center;
        if ((double) this.VertPosition == -12.0)
          return VerticalPosition.Bottom;
        if ((double) this.VertPosition == -16.0)
          return VerticalPosition.Inside;
        return (double) this.VertPosition == -20.0 ? VerticalPosition.Outside : VerticalPosition.None;
      }
      set => this.VertPosition = (float) value;
    }

    public float HorizPosition
    {
      get => (float) this.GetPropertyValue(62);
      set => this.SetPropertyValue(62, (object) value);
    }

    public float VertPosition
    {
      get => (float) this.GetPropertyValue(63 /*0x3F*/);
      set => this.SetPropertyValue(63 /*0x3F*/, (object) value);
    }

    public HorizontalRelation HorizRelationTo
    {
      get => (HorizontalRelation) this.GetPropertyValue(64 /*0x40*/);
      set => this.SetPropertyValue(64 /*0x40*/, (object) value);
    }

    public VerticalRelation VertRelationTo
    {
      get => (VerticalRelation) this.GetPropertyValue(65);
      set => this.SetPropertyValue(65, (object) value);
    }

    public float DistanceFromTop
    {
      get => (float) this.GetPropertyValue(66);
      set => this.SetPropertyValue(66, (object) value);
    }

    public float DistanceFromBottom
    {
      get => (float) this.GetPropertyValue(67);
      set => this.SetPropertyValue(67, (object) value);
    }

    public float DistanceFromLeft
    {
      get => (float) this.GetPropertyValue(68);
      set => this.SetPropertyValue(68, (object) value);
    }

    public float DistanceFromRight
    {
      get => (float) this.GetPropertyValue(69);
      set => this.SetPropertyValue(69, (object) value);
    }

    internal TablePositioning(RowFormat ownerRowFormat) => this.m_ownerRowFormat = ownerRowFormat;

    internal TablePositioning(FormatBase parent, int baseKey)
      : base(parent, baseKey)
    {
      this.m_ownerRowFormat = (RowFormat) parent;
    }

    internal object GetPropertyValue(int propertyKey)
    {
      return this.m_ownerRowFormat.GetPropertyValue(propertyKey);
    }

    private void SetPropertyValue(int propertyKey, object value)
    {
      this.m_ownerRowFormat.SetPropertyValue(propertyKey, value);
    }

    protected override object GetDefValue(int key) => this.m_ownerRowFormat.GetDefValue(key);
  }
}
