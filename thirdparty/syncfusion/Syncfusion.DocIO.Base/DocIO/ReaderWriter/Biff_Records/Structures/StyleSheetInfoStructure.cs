// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures.StyleSheetInfoStructure
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;

[CLSCompliant(false)]
[StructLayout(LayoutKind.Sequential)]
internal class StyleSheetInfoStructure : IDataStructure
{
  private const int DEF_BIT_STYLE_NAMES_WRITTEN = 0;
  private const int DEF_RECORD_SIZE = 20;
  private ushort m_usStylesCount;
  private ushort m_usSTDBaseLength;
  private ushort m_usOptions = 1;
  private ushort m_usStiMaxWhenSaved;
  private ushort m_usISTDMaxFixedWhenSaved;
  private ushort m_usBuiltInNamesVersion = 4;
  [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
  private ushort[] m_arrStandardChpStsh = new ushort[3];
  private ushort m_ftcBi;

  internal ushort StylesCount
  {
    get => this.m_usStylesCount;
    set => this.m_usStylesCount = value;
  }

  internal ushort STDBaseLength
  {
    get => this.m_usSTDBaseLength;
    set => this.m_usSTDBaseLength = value;
  }

  internal bool IsStdStyleNamesWritten
  {
    get => BaseWordRecord.GetBit((int) this.m_usOptions, 0);
    set => this.m_usOptions = (ushort) BaseWordRecord.SetBit((int) this.m_usOptions, 0, value);
  }

  internal ushort StiMaxWhenSaved
  {
    get => this.m_usStiMaxWhenSaved;
    set => this.m_usStiMaxWhenSaved = value;
  }

  internal ushort ISTDMaxFixedWhenSaved
  {
    get => this.m_usISTDMaxFixedWhenSaved;
    set => this.m_usISTDMaxFixedWhenSaved = value;
  }

  internal ushort BuiltInNamesVersion
  {
    get => this.m_usBuiltInNamesVersion;
    set => this.m_usBuiltInNamesVersion = value;
  }

  internal ushort[] StandardChpStsh
  {
    get => this.m_arrStandardChpStsh;
    set
    {
      this.m_arrStandardChpStsh = value != null && value.Length == 3 ? value : throw new ArgumentException("Trying to set wrong StandardChpStsh");
    }
  }

  internal ushort FtcBi
  {
    get => this.m_ftcBi;
    set => this.m_ftcBi = value;
  }

  public int Length => 20;

  public void Parse(byte[] arrData, int iOffset)
  {
    this.m_usStylesCount = ByteConverter.ReadUInt16(arrData, ref iOffset);
    this.m_usSTDBaseLength = ByteConverter.ReadUInt16(arrData, ref iOffset);
    this.m_usOptions = ByteConverter.ReadUInt16(arrData, ref iOffset);
    this.m_usStiMaxWhenSaved = ByteConverter.ReadUInt16(arrData, ref iOffset);
    this.m_usISTDMaxFixedWhenSaved = ByteConverter.ReadUInt16(arrData, ref iOffset);
    this.m_usBuiltInNamesVersion = ByteConverter.ReadUInt16(arrData, ref iOffset);
    for (int index = 0; index < 3; ++index)
      this.m_arrStandardChpStsh[index] = ByteConverter.ReadUInt16(arrData, ref iOffset);
    if (arrData.Length <= iOffset + 1)
      return;
    this.m_ftcBi = ByteConverter.ReadUInt16(arrData, ref iOffset);
  }

  public int Save(byte[] arrData, int iOffset)
  {
    ByteConverter.WriteUInt16(arrData, ref iOffset, this.m_usStylesCount);
    ByteConverter.WriteUInt16(arrData, ref iOffset, this.m_usSTDBaseLength);
    ByteConverter.WriteUInt16(arrData, ref iOffset, this.m_usOptions);
    ByteConverter.WriteUInt16(arrData, ref iOffset, this.m_usStiMaxWhenSaved);
    ByteConverter.WriteUInt16(arrData, ref iOffset, this.m_usISTDMaxFixedWhenSaved);
    ByteConverter.WriteUInt16(arrData, ref iOffset, this.m_usBuiltInNamesVersion);
    for (int index = 0; index < 3; ++index)
      ByteConverter.WriteUInt16(arrData, ref iOffset, this.m_arrStandardChpStsh[index]);
    ByteConverter.WriteUInt16(arrData, ref iOffset, this.m_ftcBi);
    return 20;
  }
}
