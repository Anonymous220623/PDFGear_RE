// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Border
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class Border(FormatBase parent, int baseKey) : FormatBase(parent, baseKey)
{
  public const int ColorKey = 1;
  internal const int BorderTypeKey = 2;
  internal const int LineWidthKey = 3;
  protected const int SpaceKey = 4;
  protected const int ShadowKey = 5;
  protected const int HasNoneStyleKey = 6;
  private Border.BorderPositions m_borderPosition;
  private byte m_bFlags;

  internal bool IsRead
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal bool IsHTMLRead
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  internal Border.BorderPositions BorderPosition
  {
    get => this.m_borderPosition;
    set => this.m_borderPosition = value;
  }

  public Color Color
  {
    get => (Color) this[1];
    set => this[1] = (object) value;
  }

  public float LineWidth
  {
    get => (float) this[3];
    set
    {
      this[3] = (object) value;
      if ((double) value == 0.0 && !this.IsRead)
      {
        if (this.BorderType != BorderStyle.None && this.BorderType != BorderStyle.Cleared)
          this.BorderType = BorderStyle.None;
      }
      else if (!this.IsRead && this.BorderType == BorderStyle.None && !this.IsHTMLRead)
        this.BorderType = BorderStyle.Single;
      if (this.IsRead)
        return;
      this.UpdateTableCells();
    }
  }

  public BorderStyle BorderType
  {
    get => (BorderStyle) this[2];
    set => this.SetBorderStyle(value);
  }

  public float Space
  {
    get => (float) this[4];
    set
    {
      this[4] = (object) (float) (byte) ((uint) (byte) Math.Round((double) value) & 31U /*0x1F*/);
    }
  }

  public bool Shadow
  {
    get => (bool) this[5];
    set
    {
      this[5] = (object) value;
      if (this.IsRead)
        return;
      this.UpdateTableCells();
    }
  }

  internal bool HasNoneStyle
  {
    get => (bool) this[6];
    set => this[6] = (object) value;
  }

  internal bool IsBorderDefined
  {
    get
    {
      if (this.BorderType != BorderStyle.None)
        return true;
      return this.HasNoneStyle && this.HasKey(6);
    }
  }

  private Border OwnerBorder()
  {
    if (this.OwnerBase is Borders ownerBase && ownerBase.CurrentRow != null)
    {
      Borders borders = ownerBase.CurrentRow.RowFormat.Borders;
      switch (this.BorderPosition)
      {
        case Border.BorderPositions.Left:
          return borders.Left;
        case Border.BorderPositions.Top:
          return borders.Top;
        case Border.BorderPositions.Right:
          return borders.Right;
        case Border.BorderPositions.Bottom:
          return borders.Bottom;
      }
    }
    return (Border) null;
  }

  internal void CopyBorderFormatting(Border sourceBorder)
  {
    this[2] = (object) sourceBorder.BorderType;
    this[3] = (object) sourceBorder.LineWidth;
    this[1] = (object) sourceBorder.Color;
    this[5] = (object) sourceBorder.Shadow;
    if (sourceBorder.BorderType == BorderStyle.Cleared)
      return;
    this[6] = (object) sourceBorder.HasNoneStyle;
  }

  private void UpdateTableCells()
  {
    if (this.OwnerBase == null || !(this.OwnerBase is Borders))
      return;
    Borders ownerBase = this.OwnerBase as Borders;
    if (ownerBase.CurrentRow == null)
      return;
    WTable ownerTable = ownerBase.CurrentRow.OwnerTable;
    if (ownerTable == null)
      return;
    int rowIndex = ownerBase.CurrentRow.GetRowIndex();
    if (ownerBase.CurrentCell == null)
    {
      this.UpdateAruondRow(ownerTable, rowIndex);
    }
    else
    {
      int curCellIndex = ownerBase.CurrentCell.CellFormat.CurCellIndex;
      if (!this.HasNoneStyle && this.BorderType == BorderStyle.None)
        return;
      this.UpdateAroundCell(ownerTable, ownerBase, rowIndex, curCellIndex);
    }
  }

  internal float GetLineWidthValue()
  {
    switch (this.BorderType)
    {
      case BorderStyle.None:
      case BorderStyle.Hairline:
      case BorderStyle.Cleared:
        return 0.0f;
      case BorderStyle.Single:
      case BorderStyle.Thick:
      case BorderStyle.Dot:
      case BorderStyle.DashLargeGap:
      case BorderStyle.DotDash:
      case BorderStyle.DotDotDash:
      case BorderStyle.DashSmallGap:
        return this.LineWidth;
      case BorderStyle.Double:
      case BorderStyle.Triple:
      case BorderStyle.ThinThickSmallGap:
      case BorderStyle.ThinThinSmallGap:
      case BorderStyle.ThinThickThinSmallGap:
      case BorderStyle.ThinThickMediumGap:
      case BorderStyle.ThickThinMediumGap:
      case BorderStyle.ThickThickThinMediumGap:
      case BorderStyle.ThinThickLargeGap:
      case BorderStyle.ThickThinLargeGap:
      case BorderStyle.ThinThickThinLargeGap:
      case BorderStyle.Emboss3D:
      case BorderStyle.Engrave3D:
        float[] borderLineWidthArray = this.GetBorderLineWidthArray(this.BorderType, this.LineWidth);
        float lineWidthValue = 0.0f;
        foreach (float num in borderLineWidthArray)
          lineWidthValue += num;
        return lineWidthValue;
      case BorderStyle.Wave:
        return (double) this.LineWidth != 1.5 ? 2.5f : 3f;
      case BorderStyle.DoubleWave:
        return 6.75f;
      case BorderStyle.DashDotStroker:
      case BorderStyle.Outset:
        return this.LineWidth;
      default:
        return this.LineWidth;
    }
  }

  internal float GetLineWeight()
  {
    int lineNumber = this.GetLineNumber();
    float lineWeight = 1f;
    switch (this.BorderType)
    {
      case BorderStyle.Single:
      case BorderStyle.Thick:
      case BorderStyle.Double:
      case BorderStyle.DotDash:
      case BorderStyle.DotDotDash:
      case BorderStyle.Triple:
      case BorderStyle.ThinThickSmallGap:
      case BorderStyle.ThinThinSmallGap:
      case BorderStyle.ThinThickThinSmallGap:
      case BorderStyle.ThinThickMediumGap:
      case BorderStyle.ThickThinMediumGap:
      case BorderStyle.ThickThickThinMediumGap:
      case BorderStyle.ThinThickLargeGap:
      case BorderStyle.ThickThinLargeGap:
      case BorderStyle.ThinThickThinLargeGap:
      case BorderStyle.Wave:
      case BorderStyle.DoubleWave:
      case BorderStyle.DashSmallGap:
      case BorderStyle.DashDotStroker:
      case BorderStyle.Emboss3D:
      case BorderStyle.Engrave3D:
      case BorderStyle.Outset:
      case BorderStyle.Inset:
        lineWeight = this.LineWidth * (float) lineNumber;
        break;
      case BorderStyle.Dot:
      case BorderStyle.DashLargeGap:
        lineWeight = 1f;
        break;
    }
    return lineWeight;
  }

  private int GetLineNumber()
  {
    int lineNumber = 0;
    switch (this.BorderType)
    {
      case BorderStyle.Single:
        lineNumber = 1;
        break;
      case BorderStyle.Thick:
        lineNumber = 2;
        break;
      case BorderStyle.Double:
        lineNumber = 3;
        break;
      case BorderStyle.DotDash:
        lineNumber = 8;
        break;
      case BorderStyle.DotDotDash:
        lineNumber = 9;
        break;
      case BorderStyle.Triple:
        lineNumber = 10;
        break;
      case BorderStyle.ThinThickSmallGap:
        lineNumber = 11;
        break;
      case BorderStyle.ThinThinSmallGap:
        lineNumber = 12;
        break;
      case BorderStyle.ThinThickThinSmallGap:
        lineNumber = 13;
        break;
      case BorderStyle.ThinThickMediumGap:
        lineNumber = 14;
        break;
      case BorderStyle.ThickThinMediumGap:
        lineNumber = 15;
        break;
      case BorderStyle.ThickThickThinMediumGap:
        lineNumber = 16 /*0x10*/;
        break;
      case BorderStyle.ThinThickLargeGap:
        lineNumber = 17;
        break;
      case BorderStyle.ThickThinLargeGap:
        lineNumber = 18;
        break;
      case BorderStyle.ThinThickThinLargeGap:
        lineNumber = 19;
        break;
      case BorderStyle.Wave:
        lineNumber = 20;
        break;
      case BorderStyle.DoubleWave:
        lineNumber = 21;
        break;
      case BorderStyle.DashSmallGap:
        lineNumber = 22;
        break;
      case BorderStyle.DashDotStroker:
        lineNumber = 23;
        break;
      case BorderStyle.Emboss3D:
        lineNumber = 24;
        break;
      case BorderStyle.Engrave3D:
        lineNumber = 25;
        break;
      case BorderStyle.Outset:
        lineNumber = 26;
        break;
      case BorderStyle.Inset:
        lineNumber = 27;
        break;
    }
    return lineNumber;
  }

  internal int GetStylePriority()
  {
    int stylePriority = 0;
    switch (this.BorderType)
    {
      case BorderStyle.Single:
        stylePriority = 3;
        break;
      case BorderStyle.Thick:
        stylePriority = 4;
        break;
      case BorderStyle.Double:
        stylePriority = 5;
        break;
      case BorderStyle.Dot:
        stylePriority = 1;
        break;
      case BorderStyle.DashLargeGap:
        stylePriority = 2;
        break;
      case BorderStyle.DotDash:
        stylePriority = 6;
        break;
      case BorderStyle.DotDotDash:
        stylePriority = 7;
        break;
      case BorderStyle.Triple:
        stylePriority = 8;
        break;
      case BorderStyle.ThinThickSmallGap:
        stylePriority = 9;
        break;
      case BorderStyle.ThinThinSmallGap:
        stylePriority = 10;
        break;
      case BorderStyle.ThinThickThinSmallGap:
        stylePriority = 11;
        break;
      case BorderStyle.ThinThickMediumGap:
        stylePriority = 12;
        break;
      case BorderStyle.ThickThinMediumGap:
        stylePriority = 13;
        break;
      case BorderStyle.ThickThickThinMediumGap:
        stylePriority = 14;
        break;
      case BorderStyle.ThinThickLargeGap:
        stylePriority = 15;
        break;
      case BorderStyle.ThickThinLargeGap:
        stylePriority = 16 /*0x10*/;
        break;
      case BorderStyle.ThinThickThinLargeGap:
        stylePriority = 17;
        break;
      case BorderStyle.Wave:
        stylePriority = 18;
        break;
      case BorderStyle.DoubleWave:
        stylePriority = 19;
        break;
      case BorderStyle.DashSmallGap:
        stylePriority = 20;
        break;
      case BorderStyle.DashDotStroker:
        stylePriority = 21;
        break;
      case BorderStyle.Emboss3D:
        stylePriority = 22;
        break;
      case BorderStyle.Engrave3D:
        stylePriority = 23;
        break;
      case BorderStyle.Outset:
        stylePriority = 24;
        break;
      case BorderStyle.Inset:
        stylePriority = 25;
        break;
    }
    return stylePriority;
  }

  private float[] GetBorderLineWidthArray(BorderStyle borderType, float lineWidth)
  {
    float[] borderLineWidthArray = new float[1]{ lineWidth };
    switch (borderType)
    {
      case BorderStyle.Double:
        borderLineWidthArray = new float[3]{ 1f, 1f, 1f };
        break;
      case BorderStyle.Triple:
        borderLineWidthArray = new float[5]
        {
          1f,
          1f,
          1f,
          1f,
          1f
        };
        break;
      case BorderStyle.ThinThickSmallGap:
        borderLineWidthArray = new float[3]
        {
          1f,
          -0.75f,
          -0.75f
        };
        break;
      case BorderStyle.ThinThinSmallGap:
        borderLineWidthArray = new float[3]
        {
          -0.75f,
          -0.75f,
          1f
        };
        break;
      case BorderStyle.ThinThickThinSmallGap:
        borderLineWidthArray = new float[5]
        {
          -0.75f,
          -0.75f,
          1f,
          -0.75f,
          -0.75f
        };
        break;
      case BorderStyle.ThinThickMediumGap:
        borderLineWidthArray = new float[3]
        {
          1f,
          0.5f,
          0.5f
        };
        break;
      case BorderStyle.ThickThinMediumGap:
        borderLineWidthArray = new float[3]
        {
          0.5f,
          0.5f,
          1f
        };
        break;
      case BorderStyle.ThickThickThinMediumGap:
        borderLineWidthArray = new float[5]
        {
          0.5f,
          0.5f,
          1f,
          0.5f,
          0.5f
        };
        break;
      case BorderStyle.ThinThickLargeGap:
        borderLineWidthArray = new float[3]
        {
          -1.5f,
          1f,
          -0.75f
        };
        break;
      case BorderStyle.ThickThinLargeGap:
        borderLineWidthArray = new float[3]
        {
          -0.75f,
          1f,
          -1.5f
        };
        break;
      case BorderStyle.ThinThickThinLargeGap:
        borderLineWidthArray = new float[5]
        {
          -0.75f,
          1f,
          -1.5f,
          1f,
          -0.75f
        };
        break;
      case BorderStyle.Emboss3D:
      case BorderStyle.Engrave3D:
        borderLineWidthArray = new float[5]
        {
          0.25f,
          0.0f,
          1f,
          0.0f,
          0.25f
        };
        break;
    }
    if (borderLineWidthArray.Length == 1)
      return new float[1]{ lineWidth };
    for (int index = 0; index < borderLineWidthArray.Length; ++index)
    {
      if ((double) borderLineWidthArray[index] >= 0.0)
        borderLineWidthArray[index] *= lineWidth;
      else
        borderLineWidthArray[index] = Math.Abs(borderLineWidthArray[index]);
    }
    return borderLineWidthArray;
  }

  private void UpdateAroundCell(WTable table, Borders borders, int rowIndex, int cellIndex)
  {
    if (rowIndex == -1)
      return;
    switch (this.BorderPosition)
    {
      case Border.BorderPositions.Left:
        if (cellIndex <= 0 || cellIndex - 1 >= table.Rows[rowIndex].Cells.Count)
          break;
        WTableCell wtableCell1 = table[rowIndex, cellIndex - 1];
        if (!wtableCell1.CellFormat.Borders.Right.HasNoneStyle)
          break;
        wtableCell1.CellFormat.Borders.Right.CopyBorderFormatting(this);
        break;
      case Border.BorderPositions.Top:
        if (rowIndex <= 0 || table.Rows[rowIndex - 1].Cells.Count <= cellIndex)
          break;
        WTableCell wtableCell2 = table[rowIndex - 1, cellIndex];
        if (!wtableCell2.CellFormat.Borders.Bottom.HasNoneStyle)
          break;
        wtableCell2.CellFormat.Borders.Bottom.CopyBorderFormatting(this);
        break;
      case Border.BorderPositions.Right:
        if (cellIndex + 1 >= table.Rows[rowIndex].Cells.Count)
          break;
        WTableCell wtableCell3 = table[rowIndex, cellIndex + 1];
        if (!wtableCell3.CellFormat.Borders.Left.HasNoneStyle)
          break;
        wtableCell3.CellFormat.Borders.Left.CopyBorderFormatting(this);
        break;
      case Border.BorderPositions.Bottom:
        if (rowIndex + 1 >= table.Rows.Count || table.Rows[rowIndex + 1].Cells.Count <= cellIndex)
          break;
        WTableCell wtableCell4 = table[rowIndex + 1, cellIndex];
        if (!wtableCell4.CellFormat.Borders.Top.HasNoneStyle)
          break;
        wtableCell4.CellFormat.Borders.Top.CopyBorderFormatting(this);
        break;
    }
  }

  private void UpdateAruondRow(WTable table, int rowIndex)
  {
    switch (this.BorderPosition)
    {
      case Border.BorderPositions.Top:
        if (rowIndex == 0)
          break;
        break;
      case Border.BorderPositions.Bottom:
        int num = table.Rows.Count - 1;
        break;
    }
  }

  private void SetBorderStyle(BorderStyle value)
  {
    if (value == BorderStyle.None || value == BorderStyle.Cleared)
    {
      if ((double) this.LineWidth != 0.0)
        this.LineWidth = 0.0f;
      this.HasNoneStyle = value == BorderStyle.None;
    }
    else if ((BorderStyle) this[2] == BorderStyle.None && value != BorderStyle.Cleared && !this.IsHTMLRead)
    {
      if ((double) this.LineWidth == 0.0)
        this.LineWidth = 0.5f;
      if (this.Color == Color.Empty)
        this.Color = Color.Black;
      this.HasNoneStyle = false;
    }
    if (!this.IsHTMLRead && ((BorderStyle) this[2] == BorderStyle.None || (BorderStyle) this[2] == BorderStyle.Cleared) && value != BorderStyle.Cleared && value != BorderStyle.None)
    {
      if ((double) this.LineWidth == 0.0)
        this.LineWidth = 0.5f;
      if (this.Color == Color.Empty || this.Color == Color.White || this.Color.Name.ToLower() == "ffffff")
        this.Color = Color.Black;
    }
    this[2] = (object) value;
  }

  internal void SetDefaultProperties()
  {
    this.PropertiesHash.Add(this.GetFullKey(3), (object) 0.0f);
    this.PropertiesHash.Add(this.GetFullKey(2), (object) BorderStyle.None);
    this.PropertiesHash.Add(this.GetFullKey(5), (object) false);
    this.PropertiesHash.Add(this.GetFullKey(1), (object) Color.Empty);
    this.PropertiesHash.Add(this.GetFullKey(4), (object) 0.0f);
  }

  internal bool Compare(Border border)
  {
    return this.BorderPosition == border.BorderPosition && this.Compare(this.GetFullKey(2), (FormatBase) border) && this.Compare(this.GetFullKey(1), (FormatBase) border) && this.Compare(this.GetFullKey(3), (FormatBase) border) && this.Compare(this.GetFullKey(5), (FormatBase) border) && this.Compare(this.GetFullKey(4), (FormatBase) border) && this.Compare(this.GetFullKey(6), (FormatBase) border);
  }

  public void InitFormatting(Color color, float lineWidth, BorderStyle borderType, bool shadow)
  {
    this[1] = (object) color;
    this[3] = (object) lineWidth;
    this[2] = (object) borderType;
    this[5] = (object) shadow;
    if (borderType == BorderStyle.None)
      this[6] = (object) true;
    else
      this[6] = (object) false;
  }

  protected override object GetDefValue(int key)
  {
    switch (key)
    {
      case 1:
        return (object) Color.Empty;
      case 2:
        return (object) BorderStyle.None;
      case 3:
        return (object) 0.0f;
      case 4:
        return (object) 0.0f;
      case 5:
        return (object) false;
      case 6:
        return (object) false;
      default:
        throw new ArgumentException("key has invalid value");
    }
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    if (this.HasKey(1) && !this.Color.IsEmpty)
      writer.WriteValue("Color", this.Color);
    if (this.HasKey(3))
      writer.WriteValue("LineWidth", this.LineWidth);
    if (this.HasKey(2))
      writer.WriteValue("BorderType", (Enum) this.BorderType);
    if (this.HasKey(4))
      writer.WriteValue("Space", this.Space);
    if (!this.HasKey(5))
      return;
    writer.WriteValue("Shadow", this.Shadow);
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    if (reader.HasAttribute("Color"))
      this.Color = reader.ReadColor("Color");
    if (reader.HasAttribute("LineWidth"))
      this.LineWidth = reader.ReadFloat("LineWidth");
    if (reader.HasAttribute("BorderType"))
      this.BorderType = (BorderStyle) reader.ReadEnum("BorderType", typeof (BorderStyle));
    if (reader.HasAttribute("Space"))
      this.Space = reader.ReadFloat("Space");
    if (!reader.HasAttribute("Shadow"))
      return;
    this.Shadow = reader.ReadBoolean("Shadow");
  }

  protected override void InitXDLSHolder()
  {
    if (!this.IsDefault)
      return;
    this.XDLSHolder.SkipMe = true;
  }

  protected override void OnChange(FormatBase format, int propertyKey)
  {
    base.OnChange(format, propertyKey);
  }

  internal override void ApplyBase(FormatBase baseFormat) => base.ApplyBase(baseFormat);

  internal void UpdateSourceFormatting(Border border)
  {
    if (border.BorderPosition != this.BorderPosition)
      border.BorderPosition = this.BorderPosition;
    if (border.BorderType != this.BorderType)
      border.BorderType = this.BorderType;
    if (border.Color != this.Color)
      border.Color = this.Color;
    if (border.HasNoneStyle != this.HasNoneStyle)
      border.HasNoneStyle = this.HasNoneStyle;
    if (border.IsRead != this.IsRead)
      border.IsRead = this.IsRead;
    if ((double) border.LineWidth != (double) this.LineWidth)
      border.LineWidth = this.LineWidth;
    if (border.Shadow != this.Shadow)
      border.Shadow = this.Shadow;
    if ((double) border.Space == (double) this.Space)
      return;
    border.Space = this.Space;
  }

  internal enum BorderPositions
  {
    Left = 0,
    Top = 1,
    Right = 2,
    Bottom = 3,
    Vertical = 5,
    Horizontal = 6,
    DiagonalDown = 7,
    DiagonalUp = 8,
  }
}
