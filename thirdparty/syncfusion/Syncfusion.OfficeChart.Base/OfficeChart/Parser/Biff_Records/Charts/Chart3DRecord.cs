// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Charts.Chart3DRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Charts;

[CLSCompliant(false)]
[Biff(TBIFFRecord.Chart3D)]
internal class Chart3DRecord : BiffRecordRaw
{
  private const int DEF_RECORD_SIZE = 14;
  [BiffRecordPos(0, 2)]
  private ushort m_usRotationAngle = 20;
  [BiffRecordPos(2, 2, true)]
  private short m_sElevationAngle = 15;
  [BiffRecordPos(4, 2)]
  private ushort m_usDistance = 30;
  [BiffRecordPos(6, 2)]
  private ushort m_usHeight = 100;
  [BiffRecordPos(8, 2)]
  private ushort m_usDepth = 100;
  [BiffRecordPos(10, 2)]
  private ushort m_usGap = 150;
  [BiffRecordPos(12, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(12, 0, TFieldType.Bit)]
  private bool m_bPerspective;
  [BiffRecordPos(12, 1, TFieldType.Bit)]
  private bool m_bClustered;
  [BiffRecordPos(12, 2, TFieldType.Bit)]
  private bool m_bAutoScaling = true;
  [BiffRecordPos(12, 4, TFieldType.Bit)]
  private bool m_bReserved = true;
  [BiffRecordPos(12, 5, TFieldType.Bit)]
  private bool m_b2DWalls;
  private bool m_bDefaultElevation = true;
  private bool m_bDefaultRotation = true;

  public ushort RotationAngle
  {
    get => this.m_usRotationAngle;
    set
    {
      this.m_usRotationAngle = value >= (ushort) 0 && value <= (ushort) 360 ? value : throw new ArgumentOutOfRangeException(nameof (value), "Value cannot be less than 0 or greater than 360.");
      this.m_bDefaultRotation = false;
    }
  }

  public short ElevationAngle
  {
    get => this.m_sElevationAngle;
    set
    {
      this.m_sElevationAngle = value >= (short) -90 && value <= (short) 90 ? value : throw new ArgumentOutOfRangeException(nameof (value), "Value cannot be less than -90 or greater than 90.");
      this.m_bDefaultElevation = false;
    }
  }

  public bool IsDefaultRotation
  {
    get => this.m_bDefaultRotation;
    set => this.m_bDefaultRotation = value;
  }

  public bool IsDefaultElevation
  {
    get => this.m_bDefaultElevation;
    set => this.m_bDefaultElevation = value;
  }

  public ushort DistanceFromEye
  {
    get => this.m_usDistance;
    set
    {
      this.m_usDistance = value >= (ushort) 0 && value <= (ushort) 100 ? value : throw new ArgumentOutOfRangeException(nameof (value), "Value cannot be less than 0 or greater than 100.");
    }
  }

  public ushort Height
  {
    get => this.m_usHeight;
    set => this.m_usHeight = value;
  }

  public ushort Depth
  {
    get => this.m_usDepth;
    set
    {
      if ((int) value == (int) this.m_usDepth)
        return;
      this.m_usDepth = value;
    }
  }

  public ushort SeriesSpace
  {
    get => this.m_usGap;
    set
    {
      if ((int) value == (int) this.m_usGap)
        return;
      this.m_usGap = value;
    }
  }

  public ushort Options => this.m_usOptions;

  public bool IsPerspective
  {
    get => this.m_bPerspective;
    set
    {
      if (value == this.m_bPerspective)
        return;
      this.m_bPerspective = value;
    }
  }

  public bool IsClustered
  {
    get => this.m_bClustered;
    set
    {
      if (value == this.m_bClustered)
        return;
      this.m_bClustered = value;
    }
  }

  public bool IsAutoScaled
  {
    get => this.m_bAutoScaling;
    set
    {
      if (value == this.m_bAutoScaling)
        return;
      this.m_bAutoScaling = value;
    }
  }

  public bool Is2DWalls
  {
    get => this.m_b2DWalls;
    set
    {
      if (value == this.m_b2DWalls)
        return;
      this.m_b2DWalls = value;
    }
  }

  public Chart3DRecord()
  {
  }

  public Chart3DRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public Chart3DRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override int GetStoreSize(OfficeVersion version) => 14;

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usRotationAngle = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_sElevationAngle = provider.ReadInt16(iOffset);
    iOffset += 2;
    this.m_usDistance = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usHeight = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usDepth = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usGap = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usOptions = provider.ReadUInt16(iOffset);
    this.m_bPerspective = provider.ReadBit(iOffset, 0);
    this.m_bClustered = provider.ReadBit(iOffset, 1);
    this.m_bAutoScaling = provider.ReadBit(iOffset, 2);
    this.m_bReserved = provider.ReadBit(iOffset, 4);
    this.m_b2DWalls = provider.ReadBit(iOffset, 5);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_usOptions &= (ushort) 55;
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteUInt16(iOffset, this.m_usRotationAngle);
    iOffset += 2;
    provider.WriteInt16(iOffset, this.m_sElevationAngle);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usDistance);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usHeight);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usDepth);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usGap);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usOptions);
    provider.WriteBit(iOffset, this.m_bPerspective, 0);
    provider.WriteBit(iOffset, this.m_bClustered, 1);
    provider.WriteBit(iOffset, this.m_bAutoScaling, 2);
    provider.WriteBit(iOffset, this.m_bReserved, 4);
    provider.WriteBit(iOffset, this.m_b2DWalls, 5);
  }

  public static bool operator ==(Chart3DRecord chart3D, Chart3DRecord chart3D2)
  {
    if (object.Equals((object) chart3D, (object) null) && object.Equals((object) chart3D2, (object) null))
      return true;
    return !object.Equals((object) chart3D, (object) null) && !object.Equals((object) chart3D2, (object) null) && (int) chart3D2.m_usRotationAngle == (int) chart3D.m_usRotationAngle && (int) chart3D2.m_sElevationAngle == (int) chart3D.m_sElevationAngle && (int) chart3D2.m_usDistance == (int) chart3D.m_usDistance && (int) chart3D2.m_usHeight == (int) chart3D.m_usHeight && (int) chart3D2.m_usDepth == (int) chart3D.m_usDepth && (int) chart3D2.m_usGap == (int) chart3D.m_usGap && chart3D2.m_bPerspective == chart3D.m_bPerspective && chart3D2.m_bClustered == chart3D.m_bClustered && chart3D2.m_bAutoScaling == chart3D.m_bAutoScaling && chart3D2.m_b2DWalls == chart3D.m_b2DWalls;
  }

  public static bool operator !=(Chart3DRecord chart3D, Chart3DRecord chart3D2)
  {
    return !(chart3D == chart3D2);
  }
}
