// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.CellFormat
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class CellFormat : FormatBase
{
  internal const int BordersKey = 1;
  internal const int VrAlignmentKey = 2;
  internal const int PaddingsKey = 3;
  internal const int ShadingColorKey = 4;
  internal const int ForeColorKey = 5;
  internal const int VerticalMergeKey = 6;
  internal const int TextureStyleKey = 7;
  internal const int HorizontalMergeKey = 8;
  internal const int TextWrapKey = 9;
  internal const int FitTextKey = 10;
  internal const int TextDirectionKey = 11;
  internal const int CellWidthKey = 12;
  internal const int PreferredWidthTypeKey = 13;
  internal const int PreferredWidthKey = 14;
  internal const int FormatChangeAuthorNameKey = 15;
  internal const int FormatChangeDateTimeKey = 16 /*0x10*/;
  private RowFormat m_ownerRowFormat;
  private byte m_bFlags = 2;
  private PreferredWidthInfo m_preferredWidth;
  private List<Stream> m_xmlProps;

  private bool CancelOnChange
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal bool Hidden
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
  }

  internal PreferredWidthInfo PreferredWidth
  {
    get
    {
      if (this.m_preferredWidth == null)
        this.m_preferredWidth = new PreferredWidthInfo((FormatBase) this, 13);
      return this.m_preferredWidth;
    }
  }

  public Borders Borders
  {
    get => this.GetPropertyValue(1) as Borders;
    internal set => this.SetPropertyValue(1, (object) value);
  }

  public Paddings Paddings => this.GetPropertyValue(3) as Paddings;

  public VerticalAlignment VerticalAlignment
  {
    get => (VerticalAlignment) this.GetPropertyValue(2);
    set => this.SetPropertyValue(2, (object) value);
  }

  public Color BackColor
  {
    get => (Color) this.GetPropertyValue(4);
    set => this.SetPropertyValue(4, (object) value);
  }

  public CellMerge VerticalMerge
  {
    get => (CellMerge) this.GetPropertyValue(6);
    set => this.SetPropertyValue(6, (object) value);
  }

  public CellMerge HorizontalMerge
  {
    get
    {
      CellMerge propertyValue = (CellMerge) this.GetPropertyValue(8);
      if (propertyValue == CellMerge.Continue)
        this.UpdateHorizontalMerge(ref propertyValue);
      return propertyValue;
    }
    set
    {
      if (this.Document != null && !this.Document.IsOpening && value == CellMerge.Start)
      {
        this.PreferredWidth.WidthType = FtsWidth.None;
        this.PreferredWidth.Width = 0.0f;
      }
      this.SetPropertyValue(8, (object) value);
    }
  }

  public bool TextWrap
  {
    get => (bool) this.GetPropertyValue(9);
    set => this.SetPropertyValue(9, (object) value);
  }

  public bool FitText
  {
    get => (bool) this.GetPropertyValue(10);
    set => this.SetPropertyValue(10, (object) value);
  }

  public TextDirection TextDirection
  {
    get => (TextDirection) this.GetPropertyValue(11);
    set => this.SetPropertyValue(11, (object) value);
  }

  public bool SamePaddingsAsTable
  {
    get => this.HasSamePaddingsAsTable();
    set
    {
      this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
      if (value || this.OwnerRowFormat == null)
        return;
      this.Paddings.ImportContainer((FormatBase) this.OwnerRowFormat.Paddings);
    }
  }

  internal RowFormat OwnerRowFormat => this.GetOwnerRowFormatValue();

  internal int CurCellIndex => this.GetOwnerCellIndex();

  internal float CellWidth
  {
    get => (float) this.GetPropertyValue(12);
    set => this.SetPropertyValue(12, (object) value);
  }

  internal Color ForeColor
  {
    get => (Color) this.GetPropertyValue(5);
    set => this.SetPropertyValue(5, (object) value);
  }

  internal TextureStyle TextureStyle
  {
    get => (TextureStyle) this.GetPropertyValue(7);
    set => this.SetPropertyValue(7, (object) value);
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

  internal bool HideMark
  {
    get => ((int) this.m_bFlags & 8) >> 3 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 247 | (value ? 1 : 0) << 3);
  }

  internal string FormatChangeAuthorName
  {
    get => (string) this.GetPropertyValue(15);
    set => this.SetPropertyValue(15, (object) value);
  }

  internal DateTime FormatChangeDateTime
  {
    get => (DateTime) this.GetPropertyValue(16 /*0x10*/);
    set => this.SetPropertyValue(16 /*0x10*/, (object) value);
  }

  public CellFormat() => this.Borders.SetOwner((OwnerHolder) this);

  internal object GetPropertyValue(int propertyKey) => this[propertyKey];

  internal void SetPropertyValue(int propertyKey, object value) => this[propertyKey] = value;

  private RowFormat GetOwnerRowFormatValue()
  {
    if (this.m_ownerRowFormat != null)
      return this.m_ownerRowFormat;
    if (!(this.OwnerBase is WTableCell ownerBase))
      return (RowFormat) null;
    if (!(ownerBase.Owner is WTableRow owner))
      return (RowFormat) null;
    this.m_ownerRowFormat = owner.RowFormat;
    return this.m_ownerRowFormat;
  }

  private int GetOwnerCellIndex()
  {
    return this.OwnerBase is WTableCell ownerBase ? ownerBase.GetCellIndex() : -1;
  }

  private bool HasSamePaddingsAsTable()
  {
    return ((int) this.m_bFlags & 2) >> 1 == 0 ? ((int) this.m_bFlags & 2) >> 1 != 0 : ((int) this.m_bFlags & 2) >> 1 != 0;
  }

  private void UpdateHorizontalMerge(ref CellMerge horizMerge)
  {
    if ((this.CurCellIndex <= 0 || ((WTableCell) this.OwnerBase).OwnerRow.Cells[this.CurCellIndex - 1].CellFormat.HorizontalMerge != CellMerge.None) && this.CurCellIndex != 0)
      return;
    this.HorizontalMerge = horizMerge = CellMerge.None;
  }

  internal void ClearPreferredWidthPropertyValue(int key)
  {
    if (!this.m_propertiesHash.ContainsKey(key))
      return;
    this.PreferredWidth.Width = 0.0f;
    this.PreferredWidth.WidthType = FtsWidth.None;
    this.m_propertiesHash.Remove(key);
  }

  internal override void Close()
  {
    this.m_ownerRowFormat = (RowFormat) null;
    if (this.m_preferredWidth != null)
    {
      this.m_preferredWidth.Close();
      this.m_preferredWidth = (PreferredWidthInfo) null;
    }
    if (this.Borders != null)
      this.Borders.Close();
    if (this.Paddings != null)
      this.Paddings.Close();
    base.Close();
    if (this.m_xmlProps == null)
      return;
    this.m_xmlProps.Clear();
    this.m_xmlProps = (List<Stream>) null;
  }

  internal override bool HasValue(int propertyKey) => this.HasKey(propertyKey);

  internal override void ApplyBase(FormatBase baseFormat)
  {
    base.ApplyBase(baseFormat);
    this.Borders.ApplyBase((FormatBase) (baseFormat as CellFormat).Borders);
    this.Paddings.ApplyBase((FormatBase) (baseFormat as CellFormat).Paddings);
  }

  protected internal override void EnsureComposites()
  {
    this.EnsureComposites(1);
    this.EnsureComposites(3);
  }

  protected override object GetDefValue(int key)
  {
    switch (key)
    {
      case 1:
      case 3:
        return (object) this.GetDefComposite(key);
      case 2:
        return (object) VerticalAlignment.Top;
      case 4:
        return (object) Color.Empty;
      case 5:
        return (object) Color.Empty;
      case 6:
        return (object) CellMerge.None;
      case 7:
        return (object) TextureStyle.TextureNone;
      case 8:
        return (object) CellMerge.None;
      case 9:
        return (object) true;
      case 10:
        return (object) false;
      case 11:
        return (object) TextDirection.Horizontal;
      case 12:
      case 14:
        return (object) 0.0f;
      case 13:
        return (object) FtsWidth.None;
      case 15:
        return (object) string.Empty;
      case 16 /*0x10*/:
        return (object) DateTime.MinValue;
      default:
        throw new NotImplementedException();
    }
  }

  protected override FormatBase GetDefComposite(int key)
  {
    switch (key)
    {
      case 1:
        return this.GetDefComposite(1, (FormatBase) new Borders((FormatBase) this, 1));
      case 3:
        return this.GetDefComposite(3, (FormatBase) new Paddings((FormatBase) this, 3));
      default:
        return (FormatBase) null;
    }
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    if (this.HasKey(9))
      writer.WriteValue("TextWrap", this.TextWrap);
    writer.WriteValue("SamePaddingsAsTable", ((int) this.m_bFlags & 2) >> 1 != 0);
    if (this.OwnerRowFormat.HasSprms())
      return;
    if (this.VerticalAlignment != VerticalAlignment.Top)
      writer.WriteValue("VAlignment", (Enum) this.VerticalAlignment);
    if (this.VerticalMerge != CellMerge.None)
      writer.WriteValue("VMerge", (Enum) this.VerticalMerge);
    if (this.HorizontalMerge != CellMerge.None)
      writer.WriteValue("HMerge", (Enum) this.HorizontalMerge);
    if (this.BackColor != Color.Empty)
      writer.WriteValue("ShadingColor", this.BackColor);
    if (this.FitText)
      writer.WriteValue("FitText", this.FitText);
    if (this.TextDirection == TextDirection.Horizontal)
      return;
    writer.WriteValue("TextDirection", (Enum) this.TextDirection);
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    if (reader.HasAttribute("VAlignment"))
      this.VerticalAlignment = (VerticalAlignment) reader.ReadEnum("VAlignment", typeof (VerticalAlignment));
    if (reader.HasAttribute("VMerge"))
      this.VerticalMerge = (CellMerge) reader.ReadEnum("VMerge", typeof (CellMerge));
    if (reader.HasAttribute("HMerge"))
      this.HorizontalMerge = (CellMerge) reader.ReadEnum("HMerge", typeof (CellMerge));
    if (reader.HasAttribute("ShadingColor"))
      this.BackColor = reader.ReadColor("ShadingColor");
    if (reader.HasAttribute("TextWrap"))
      this.TextWrap = reader.ReadBoolean("TextWrap");
    if (reader.HasAttribute("SamePaddingsAsTable"))
      this.SamePaddingsAsTable = reader.ReadBoolean("SamePaddingsAsTable");
    if (reader.HasAttribute("FitText"))
      this.FitText = reader.ReadBoolean("FitText");
    if (!reader.HasAttribute("TextDirection"))
      return;
    this.TextDirection = (TextDirection) reader.ReadEnum("TextDirection", typeof (TextDirection));
  }

  protected override void InitXDLSHolder()
  {
    if (this.OwnerRowFormat.HasSprms())
      return;
    this.XDLSHolder.AddElement("borders", (object) this.Borders);
    this.XDLSHolder.AddElement("Paddings", (object) this.Paddings);
  }

  protected internal new void ImportContainer(FormatBase format)
  {
    base.ImportContainer(format);
    if (!(format is CellFormat format1))
      return;
    this.ImportXmlProps(format1);
  }

  private void ImportXmlProps(CellFormat format)
  {
    if (format.m_xmlProps == null || format.m_xmlProps.Count <= 0)
      return;
    foreach (Stream xmlProp in format.XmlProps)
      this.XmlProps.Add(this.CloneStream(xmlProp));
  }

  protected override void ImportMembers(FormatBase format)
  {
    if (format is CellFormat)
    {
      this.Borders.SetOwner((OwnerHolder) this);
      this.SamePaddingsAsTable = ((CellFormat) format).SamePaddingsAsTable;
      this.CellWidth = ((CellFormat) format).CellWidth;
    }
    else
      this.ApplyParentRowFormat(format as RowFormat);
  }

  protected override void OnChange(FormatBase format, int propKey)
  {
    if (this.CancelOnChange || this.OwnerBase != null && this.OwnerBase.Document.IsOpening)
      return;
    int num = int.MinValue;
    switch (format)
    {
      case Borders _:
      case Border _:
        num = 1;
        break;
      case Paddings _:
        num = 3;
        break;
    }
  }

  internal void UpdateCellFormat(TableStyleCellProperties cellProperties)
  {
    if (cellProperties.HasValue(4))
      this[4] = (object) cellProperties.BackColor;
    if (cellProperties.HasValue(5))
      this[5] = (object) cellProperties.ForeColor;
    if (cellProperties.HasValue(7))
      this[7] = (object) cellProperties.TextureStyle;
    if (cellProperties.HasValue(9))
      this[9] = (object) cellProperties.TextWrap;
    if (cellProperties.HasValue(2))
      this[2] = (object) cellProperties.VerticalAlignment;
    if (cellProperties.BaseFormat != null)
      this.BaseFormat = cellProperties.BaseFormat;
    this.Paddings.UpdatePaddings(cellProperties.Paddings);
  }

  private void ApplyParentRowFormat(RowFormat rowFormat)
  {
    this.BackColor = rowFormat.BackColor;
    this.ImportBorderSettings(rowFormat.Borders);
  }

  private void ImportBorderSettings(Borders borders)
  {
    this.Borders.Left.BorderType = borders.Left.BorderType;
    this.Borders.Left.Color = borders.Left.Color;
    this.Borders.Left.IsDefault = borders.Left.IsDefault;
    this.Borders.Left.LineWidth = borders.Left.LineWidth;
    this.Borders.Left.Shadow = borders.Left.Shadow;
    this.Borders.Left.Space = borders.Left.Space;
    this.Borders.Right.BorderType = borders.Right.BorderType;
    this.Borders.Right.Color = borders.Right.Color;
    this.Borders.Right.IsDefault = borders.Right.IsDefault;
    this.Borders.Right.LineWidth = borders.Right.LineWidth;
    this.Borders.Right.Shadow = borders.Right.Shadow;
    this.Borders.Right.Space = borders.Right.Space;
    this.Borders.Top.BorderType = borders.Top.BorderType;
    this.Borders.Top.Color = borders.Top.Color;
    this.Borders.Top.IsDefault = borders.Top.IsDefault;
    this.Borders.Top.LineWidth = borders.Top.LineWidth;
    this.Borders.Top.Shadow = borders.Top.Shadow;
    this.Borders.Top.Space = borders.Top.Space;
    this.Borders.Bottom.BorderType = borders.Bottom.BorderType;
    this.Borders.Bottom.Color = borders.Bottom.Color;
    this.Borders.Bottom.IsDefault = borders.Bottom.IsDefault;
    this.Borders.Bottom.LineWidth = borders.Bottom.LineWidth;
    this.Borders.Bottom.Shadow = borders.Bottom.Shadow;
    this.Borders.Bottom.Space = borders.Bottom.Space;
  }
}
