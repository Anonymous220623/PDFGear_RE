// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.SectionProperties
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class SectionProperties
{
  private SectionPropertyException m_sepx;
  private ColumnArray m_columnsArray;
  private ColumnArray m_oldColumnsArray;
  private bool m_stickProperties = true;

  internal SectionProperties()
  {
    this.m_sepx = new SectionPropertyException();
    this.m_columnsArray = new ColumnArray(this.Sprms);
    this.m_oldColumnsArray = new ColumnArray(this.Sprms);
  }

  internal SectionProperties(SectionPropertyException sepx)
  {
    this.m_sepx = sepx;
    this.m_columnsArray = new ColumnArray(this.Sprms);
    this.m_columnsArray.ReadColumnsProperties(true);
    this.m_oldColumnsArray = new ColumnArray(this.Sprms);
    this.m_oldColumnsArray.ReadColumnsProperties(false);
  }

  internal bool StickProperties
  {
    get => this.m_stickProperties;
    set => this.m_stickProperties = value;
  }

  internal SinglePropertyModifierArray Sprms => this.m_sepx.Properties;

  internal short HeaderHeight
  {
    get => this.Sprms.GetShort(45079, (short) -1);
    set
    {
      if (value != (short) -1)
        this.Sprms.SetShortValue(45079, value);
      else
        this.Sprms.SetShortValue(45079, (short) 720);
    }
  }

  internal short FooterHeight
  {
    get => this.Sprms.GetShort(45080, (short) -1);
    set
    {
      if (value != (short) -1)
        this.Sprms.SetShortValue(45080, value);
      else
        this.Sprms.SetShortValue(45080, (short) 720);
    }
  }

  internal bool TitlePage
  {
    get => this.Sprms.GetBoolean(12298, false);
    set
    {
      if (!value)
        return;
      this.Sprms.SetBoolValue(12298, value);
    }
  }

  internal byte BreakCode
  {
    get => this.Sprms.GetByte(12297, (byte) 2);
    set => this.Sprms.SetByteValue(12297, value);
  }

  internal byte TextDirection
  {
    get => this.Sprms.GetByte(20531, (byte) 0);
    set => this.Sprms.SetByteValue(20531, value);
  }

  internal ColumnArray Columns => this.m_columnsArray;

  internal ColumnArray OldColumns => this.m_oldColumnsArray;

  internal short BottomMargin
  {
    get => this.Sprms.GetShort(36900, (short) -1);
    set
    {
      if (value == (short) -1)
        return;
      this.Sprms.SetShortValue(36900, value);
    }
  }

  internal short TopMargin
  {
    get => this.Sprms.GetShort(36899, (short) -1);
    set
    {
      if (value == (short) -1)
        return;
      this.Sprms.SetShortValue(36899, value);
    }
  }

  internal short LeftMargin
  {
    get => this.Sprms.GetShort(45089, (short) -1);
    set
    {
      if (value == (short) -1)
        return;
      this.Sprms.SetShortValue(45089, value);
    }
  }

  internal short RightMargin
  {
    get => this.Sprms.GetShort(45090, (short) -1);
    set
    {
      if (value == (short) -1)
        return;
      this.Sprms.SetShortValue(45090, value);
    }
  }

  internal byte Orientation
  {
    get => this.Sprms.GetByte(12317, (byte) 0);
    set => this.Sprms.SetByteValue(12317, value);
  }

  internal ushort PageHeight
  {
    get => this.Sprms.GetUShort(45088, (ushort) 0);
    set
    {
      if (value <= (ushort) 0)
        return;
      this.Sprms.SetUShortValue(45088, value);
    }
  }

  internal ushort PageWidth
  {
    get => this.Sprms.GetUShort(45087, (ushort) 0);
    set
    {
      if (value <= (ushort) 0)
        return;
      this.Sprms.SetUShortValue(45087, value);
    }
  }

  internal ushort FirstPageTray
  {
    get => this.Sprms.GetUShort(20487, (ushort) 0);
    set => this.Sprms.SetUShortValue(20487, value);
  }

  internal ushort OtherPagesTray
  {
    get => this.Sprms.GetUShort(20488, (ushort) 0);
    set => this.Sprms.SetUShortValue(20488, value);
  }

  internal byte VerticalAlignment
  {
    get => this.Sprms.GetByte(12314, (byte) 0);
    set
    {
      if (value < (byte) 0)
        return;
      this.Sprms.SetByteValue(12314, value);
    }
  }

  internal short Gutter
  {
    get => this.Sprms.GetShort(45093, (short) 0);
    set => this.Sprms.SetShortValue(45093, value);
  }

  internal bool Bidi
  {
    get => this.Sprms.GetBoolean(12840, false);
    set => this.Sprms.SetBoolValue(12840, value);
  }

  internal byte PageNfc
  {
    get => this.Sprms.GetByte(12302, (byte) 0);
    set
    {
      if (value < (byte) 0)
        return;
      this.Sprms.SetByteValue(12302, value);
    }
  }

  internal ushort PageStartAt
  {
    get => this.Sprms.GetUShort(20508, (ushort) 0);
    set
    {
      if (value < (ushort) 0)
        return;
      this.Sprms.SetUShortValue(20508, value);
    }
  }

  internal bool PageRestart
  {
    get => this.Sprms.GetBoolean(12305, false);
    set => this.Sprms.SetBoolValue(12305, value);
  }

  internal ushort LinePitch
  {
    get => this.Sprms.GetUShort(36913, (ushort) 0);
    set
    {
      if (value < (ushort) 0)
        return;
      this.Sprms.SetUShortValue(36913, value);
    }
  }

  internal ushort PitchType
  {
    get => this.Sprms.GetUShort(20530, (ushort) 0);
    set
    {
      if (value <= (ushort) 0 || value >= (ushort) 4)
        return;
      this.Sprms.SetUShortValue(20530, value);
    }
  }

  internal bool DrawLinesBetweenCols
  {
    get => this.Sprms.GetBoolean(12313, false);
    set
    {
      if (!value)
        return;
      this.Sprms.SetBoolValue(12313, value);
    }
  }

  internal bool ProtectForm
  {
    get => this.Sprms.GetBoolean(12294, false);
    set
    {
      if (value)
        this.Sprms.SetBoolValue(12294, false);
      else
        this.Sprms.SetBoolValue(12294, true);
    }
  }

  internal bool IsChangedFormat
  {
    get => this.Sprms.GetBoolean(12857, false);
    set
    {
      if (value)
        this.Sprms.SetBoolValue(12857, true);
      else
        this.Sprms.SetBoolValue(12857, false);
    }
  }

  internal byte ChapterPageSeparator
  {
    get => this.Sprms.GetByte(12288 /*0x3000*/, (byte) 0);
    set => this.Sprms.SetByteValue(12288 /*0x3000*/, value);
  }

  internal byte HeadingLevelForChapter
  {
    get => this.Sprms.GetByte(12289, (byte) 0);
    set => this.Sprms.SetByteValue(12289, value);
  }

  internal LineNumberingMode LineNumberingMode
  {
    get => (LineNumberingMode) this.Sprms.GetByte(12307, (byte) 0);
    set => this.Sprms.SetByteValue(12307, (byte) value);
  }

  internal ushort LineNumberingStep
  {
    get => this.Sprms.GetUShort(20501, (ushort) 0);
    set => this.Sprms.SetUShortValue(20501, value);
  }

  internal short LineNumberingStartValue
  {
    get => (short) ((int) this.Sprms.GetShort(20507, (short) 0) + 1);
    set => this.Sprms.SetShortValue(20507, (short) ((int) value - 1));
  }

  internal short LineNumberingDistanceFromText
  {
    get => this.Sprms.GetShort(36886, (short) 0);
    set => this.Sprms.SetShortValue(36886, value);
  }

  internal BorderCode LeftBorder
  {
    get
    {
      byte[] byteArray = this.Sprms.GetByteArray(28716);
      return byteArray != null ? new BorderCode(byteArray, 0) : new BorderCode();
    }
    set
    {
      byte[] arr = new byte[4];
      value.SaveBytes(arr, 0);
      this.Sprms.SetByteArrayValue(28716, arr);
    }
  }

  internal BorderCode TopBorder
  {
    get
    {
      byte[] byteArray = this.Sprms.GetByteArray(28715);
      return byteArray != null ? new BorderCode(byteArray, 0) : new BorderCode();
    }
    set
    {
      byte[] arr = new byte[4];
      value.SaveBytes(arr, 0);
      this.Sprms.SetByteArrayValue(28715, arr);
    }
  }

  internal BorderCode RightBorder
  {
    get
    {
      byte[] byteArray = this.Sprms.GetByteArray(28718);
      return byteArray != null ? new BorderCode(byteArray, 0) : new BorderCode();
    }
    set
    {
      byte[] arr = new byte[4];
      value.SaveBytes(arr, 0);
      this.Sprms.SetByteArrayValue(28718, arr);
    }
  }

  internal BorderCode BottomBorder
  {
    get
    {
      byte[] byteArray = this.Sprms.GetByteArray(28717);
      return byteArray != null ? new BorderCode(byteArray, 0) : new BorderCode();
    }
    set
    {
      byte[] arr = new byte[4];
      value.SaveBytes(arr, 0);
      this.Sprms.SetByteArrayValue(28717, arr);
    }
  }

  internal BorderCode LeftBorderNew
  {
    get
    {
      byte[] byteArray = this.Sprms.GetByteArray(53813);
      if (byteArray == null)
        return new BorderCode();
      BorderCode leftBorderNew = new BorderCode();
      leftBorderNew.ParseNewBrc(byteArray, 0);
      return leftBorderNew;
    }
    set
    {
      byte[] arr = new byte[8];
      value.SaveNewBrc(arr, 0);
      this.Sprms.SetByteArrayValue(53813, arr);
    }
  }

  internal BorderCode TopBorderNew
  {
    get
    {
      byte[] byteArray = this.Sprms.GetByteArray(53812);
      if (byteArray == null)
        return new BorderCode();
      BorderCode topBorderNew = new BorderCode();
      topBorderNew.ParseNewBrc(byteArray, 0);
      return topBorderNew;
    }
    set
    {
      byte[] arr = new byte[8];
      value.SaveNewBrc(arr, 0);
      this.Sprms.SetByteArrayValue(53812, arr);
    }
  }

  internal BorderCode RightBorderNew
  {
    get
    {
      byte[] byteArray = this.Sprms.GetByteArray(53815);
      if (byteArray == null)
        return new BorderCode();
      BorderCode rightBorderNew = new BorderCode();
      rightBorderNew.ParseNewBrc(byteArray, 0);
      return rightBorderNew;
    }
    set
    {
      byte[] arr = new byte[8];
      value.SaveNewBrc(arr, 0);
      this.Sprms.SetByteArrayValue(53815, arr);
    }
  }

  internal BorderCode BottomBorderNew
  {
    get
    {
      byte[] byteArray = this.Sprms.GetByteArray(53814);
      if (byteArray == null)
        return new BorderCode();
      BorderCode bottomBorderNew = new BorderCode();
      bottomBorderNew.ParseNewBrc(byteArray, 0);
      return bottomBorderNew;
    }
    set
    {
      byte[] arr = new byte[8];
      value.SaveNewBrc(arr, 0);
      this.Sprms.SetByteArrayValue(53814, arr);
    }
  }

  internal PageBordersApplyType PageBorderApply
  {
    get => (PageBordersApplyType) ((int) this.Sprms.GetShort(21039, (short) 0) & 7);
    set
    {
      short num = (short) (((int) this.Sprms.GetShort(21039, (short) 0) & 65528) + (int) (short) value);
      if (num == (short) 0)
        return;
      this.Sprms.SetShortValue(21039, num);
    }
  }

  internal bool PageBorderIsInFront
  {
    get => ((int) this.Sprms.GetShort(21039, (short) 0) & 24) >> 3 != 1;
    set
    {
      short num = (short) (((int) this.Sprms.GetShort(21039, (short) 0) & 65511) + (int) (short) ((!value ? 1 : 0) << 3));
      if (num == (short) 0)
        return;
      this.Sprms.SetShortValue(21039, num);
    }
  }

  internal PageBorderOffsetFrom PageBorderOffsetFrom
  {
    get
    {
      return (PageBorderOffsetFrom) (((int) this.Sprms.GetShort(21039, (short) 0) & 224 /*0xE0*/) >> 5);
    }
    set
    {
      short num = (short) (((int) this.Sprms.GetShort(21039, (short) 0) & 65311) + (int) (short) ((int) (short) value << 5));
      if (num == (short) 0)
        return;
      this.Sprms.SetShortValue(21039, num);
    }
  }

  internal ushort ColumnsCount
  {
    get => (ushort) ((uint) this.Sprms.GetUShort(20491, (ushort) 0) + 1U);
    set => this.Sprms.SetUShortValue(20491, (ushort) ((uint) value - 1U));
  }

  internal ushort EndnoteNumberFormat
  {
    get => this.Sprms.GetUShort(20546, (ushort) 2);
    set => this.Sprms.SetUShortValue(20546, value);
  }

  internal ushort FootnoteNumberFormat
  {
    get => this.Sprms.GetUShort(20544, (ushort) 0);
    set => this.Sprms.SetUShortValue(20544, value);
  }

  internal byte RestartIndexForEndnote
  {
    get => this.Sprms.GetByte(12350, (byte) 0);
    set => this.Sprms.SetByteValue(12350, value);
  }

  internal byte RestartIndexForFootnotes
  {
    get => this.Sprms.GetByte(12348, (byte) 0);
    set => this.Sprms.SetByteValue(12348, value);
  }

  internal byte FootnotePosition
  {
    get => this.Sprms.GetByte(12347, (byte) 1);
    set => this.Sprms.SetByteValue(12347, value);
  }

  internal ushort InitialFootnoteNumber
  {
    get => (ushort) ((uint) this.Sprms.GetUShort(20543, (ushort) 0) + 1U);
    set => this.Sprms.SetUShortValue(20543, (ushort) ((uint) value - 1U));
  }

  internal ushort InitialEndnoteNumber
  {
    get => (ushort) ((uint) this.Sprms.GetUShort(20545, (ushort) 0) + 1U);
    set => this.Sprms.SetUShortValue(20545, (ushort) ((uint) value - 1U));
  }

  internal SectionPropertyException CloneSepx()
  {
    SectionPropertyException sepx = this.m_sepx;
    this.m_sepx = new SectionPropertyException();
    if (this.StickProperties)
    {
      int sprmIndex = 0;
      for (int count = sepx.Properties.Count; sprmIndex < count; ++sprmIndex)
      {
        SinglePropertyModifierRecord modifier = sepx.Properties.GetSprmByIndex(sprmIndex).Clone();
        if (modifier != null)
          this.m_sepx.Properties.Add(modifier);
      }
    }
    return sepx;
  }

  internal bool HasOptions(int options) => this.Sprms[options] != null;

  internal SinglePropertyModifierArray GetCopiableSprm()
  {
    SinglePropertyModifierArray copiableSprm = new SinglePropertyModifierArray();
    int count = this.Sprms.Modifiers.Count;
    for (int sprmIndex = 0; sprmIndex < count; ++sprmIndex)
    {
      SinglePropertyModifierRecord sprmByIndex = this.Sprms.GetSprmByIndex(sprmIndex);
      switch (sprmByIndex.TypedOptions)
      {
        case 0:
        case 12314:
        case 12317:
        case 20491:
        case 36899:
        case 36900:
        case 45079:
        case 45080:
        case 45087:
        case 45089:
        case 45090:
        case 61955:
          continue;
        default:
          copiableSprm.Modifiers.Add(sprmByIndex);
          continue;
      }
    }
    return copiableSprm;
  }

  internal BorderCode GetBorder(SinglePropertyModifierRecord record)
  {
    byte[] byteArray = record.ByteArray;
    BorderCode border;
    if (byteArray.Length == 4)
    {
      border = new BorderCode(record.ByteArray, 0);
    }
    else
    {
      border = new BorderCode();
      border.ParseNewBrc(byteArray, 0);
    }
    return border;
  }
}
