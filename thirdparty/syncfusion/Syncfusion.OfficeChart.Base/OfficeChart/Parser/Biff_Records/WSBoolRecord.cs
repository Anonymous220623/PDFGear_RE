// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.WSBoolRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.WSBool)]
internal class WSBoolRecord : BiffRecordRaw
{
  public const ushort DisplayGutsMask = 3072 /*0x0C00*/;
  public const int DisplayGutsStartBit = 10;
  private const int DEF_RECORD_SIZE = 2;
  [BiffRecordPos(0, 2)]
  private WSBoolRecord.OptionFlags m_options = (WSBoolRecord.OptionFlags) 1217;

  public bool IsAutoBreaks
  {
    get => (this.m_options & WSBoolRecord.OptionFlags.AutoBreaks) != (WSBoolRecord.OptionFlags) 0;
    set
    {
      if (value)
        this.m_options |= WSBoolRecord.OptionFlags.AutoBreaks;
      else
        this.m_options &= ~WSBoolRecord.OptionFlags.AutoBreaks;
    }
  }

  public bool IsDialog
  {
    get => (this.m_options & WSBoolRecord.OptionFlags.Dialog) != (WSBoolRecord.OptionFlags) 0;
    set
    {
      if (value)
        this.m_options |= WSBoolRecord.OptionFlags.Dialog;
      else
        this.m_options &= ~WSBoolRecord.OptionFlags.Dialog;
    }
  }

  public bool IsApplyStyles
  {
    get => (this.m_options & WSBoolRecord.OptionFlags.ApplyStyles) != (WSBoolRecord.OptionFlags) 0;
    set
    {
      if (value)
        this.m_options |= WSBoolRecord.OptionFlags.ApplyStyles;
      else
        this.m_options &= ~WSBoolRecord.OptionFlags.ApplyStyles;
    }
  }

  public bool IsRowSumsBelow
  {
    get => (this.m_options & WSBoolRecord.OptionFlags.RowSumsBelow) != (WSBoolRecord.OptionFlags) 0;
    set
    {
      if (value)
        this.m_options |= WSBoolRecord.OptionFlags.RowSumsBelow;
      else
        this.m_options &= ~WSBoolRecord.OptionFlags.RowSumsBelow;
    }
  }

  public bool IsRowSumsRight
  {
    get => (this.m_options & WSBoolRecord.OptionFlags.RowSumsRight) != (WSBoolRecord.OptionFlags) 0;
    set
    {
      if (value)
        this.m_options |= WSBoolRecord.OptionFlags.RowSumsRight;
      else
        this.m_options &= ~WSBoolRecord.OptionFlags.RowSumsRight;
    }
  }

  public bool IsFitToPage
  {
    get => (this.m_options & WSBoolRecord.OptionFlags.FitToPage) != (WSBoolRecord.OptionFlags) 0;
    set
    {
      if (value)
        this.m_options |= WSBoolRecord.OptionFlags.FitToPage;
      else
        this.m_options &= ~WSBoolRecord.OptionFlags.FitToPage;
    }
  }

  public ushort DisplayGuts
  {
    get
    {
      return (ushort) ((uint) BiffRecordRaw.GetUInt16BitsByMask((ushort) this.m_options, (ushort) 3072 /*0x0C00*/) >> 10);
    }
    set
    {
      if (value > (ushort) 3)
        throw new ArgumentOutOfRangeException();
      ushort options = (ushort) this.m_options;
      BiffRecordRaw.SetUInt16BitsByMask(ref options, (ushort) 3072 /*0x0C00*/, (ushort) ((uint) value << 10));
      this.m_options = (WSBoolRecord.OptionFlags) options;
    }
  }

  public bool IsAlternateExpression
  {
    get
    {
      return (this.m_options & WSBoolRecord.OptionFlags.AlternateExpression) != (WSBoolRecord.OptionFlags) 0;
    }
    set
    {
      if (value)
        this.m_options |= WSBoolRecord.OptionFlags.AlternateExpression;
      else
        this.m_options &= ~WSBoolRecord.OptionFlags.AlternateExpression;
    }
  }

  public bool IsAlternateFormula
  {
    get
    {
      return (this.m_options & WSBoolRecord.OptionFlags.AlternateFormula) != (WSBoolRecord.OptionFlags) 0;
    }
    set
    {
      if (value)
        this.m_options |= WSBoolRecord.OptionFlags.AlternateFormula;
      else
        this.m_options &= ~WSBoolRecord.OptionFlags.AlternateFormula;
    }
  }

  public override int MinimumRecordSize => 2;

  public override int MaximumRecordSize => 2;

  public override int MaximumMemorySize => 2;

  public WSBoolRecord()
  {
  }

  public WSBoolRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public WSBoolRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_options = (WSBoolRecord.OptionFlags) provider.ReadUInt16(iOffset);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_iLength = this.MinimumRecordSize;
    provider.WriteUInt16(iOffset, (ushort) this.m_options);
  }

  [Flags]
  private enum OptionFlags : ushort
  {
    AutoBreaks = 1,
    Dialog = 16, // 0x0010
    ApplyStyles = 32, // 0x0020
    RowSumsBelow = 64, // 0x0040
    RowSumsRight = 128, // 0x0080
    FitToPage = 256, // 0x0100
    AlternateExpression = 16384, // 0x4000
    AlternateFormula = 32768, // 0x8000
  }
}
