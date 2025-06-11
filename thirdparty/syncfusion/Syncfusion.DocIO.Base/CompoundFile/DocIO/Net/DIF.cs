// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.DocIO.Net.DIF
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.CompoundFile.DocIO.Net;

internal class DIF
{
  public const int SectorsInHeader = 109;
  private List<int> m_arrSectorID;
  private List<int> m_arrDifSectors = new List<int>();

  public DIF() => this.m_arrSectorID = new List<int>();

  public DIF(Stream stream, FileHeader header)
  {
    int difNumber = header.DifNumber;
    int sectorSize = header.SectorSize;
    ushort sectorShift1 = header.SectorShift;
    this.m_arrSectorID = new List<int>(109 + difNumber * (sectorSize - 4) / 4);
    this.m_arrSectorID.AddRange((IEnumerable<int>) header.FatStart);
    if (difNumber <= 0)
      return;
    int sectorIndex = header.DifStart;
    int sectorShift2 = (int) header.SectorShift;
    byte[] numArray1 = new byte[sectorSize];
    int[] numArray2 = new int[sectorSize / 4 - 1];
    for (; sectorIndex >= 0; sectorIndex = BitConverter.ToInt32(numArray1, sectorSize - 4))
    {
      long sectorOffset = Syncfusion.CompoundFile.DocIO.Net.CompoundFile.GetSectorOffset(sectorIndex, sectorShift1);
      this.m_arrDifSectors.Add(sectorIndex);
      stream.Position = sectorOffset;
      stream.Read(numArray1, 0, sectorSize);
      Buffer.BlockCopy((Array) numArray1, 0, (Array) numArray2, 0, sectorSize - 4);
      this.m_arrSectorID.AddRange((IEnumerable<int>) numArray2);
    }
  }

  public List<int> SectorIds => this.m_arrSectorID;

  internal void Write(Stream stream, FileHeader header)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    int count1 = this.m_arrSectorID.Count;
    int[] fatStart = header.FatStart;
    int index1;
    for (index1 = 0; index1 < count1 && index1 < 109; ++index1)
      fatStart[index1] = this.m_arrSectorID[index1];
    for (; index1 < 109; ++index1)
      fatStart[index1] = -1;
    if (this.m_arrDifSectors.Count > 0)
    {
      header.DifStart = this.m_arrDifSectors[0];
      header.DifNumber = this.m_arrDifSectors.Count;
    }
    byte[] numArray = new byte[header.SectorSize];
    int sectorSize = header.SectorSize;
    int num1 = sectorSize / 4 - 1;
    int index2 = 0;
    for (int count2 = this.m_arrDifSectors.Count; index2 < count2; ++index2)
    {
      int arrDifSector = this.m_arrDifSectors[index2];
      long sectorOffset = header.GetSectorOffset(arrDifSector);
      int num2 = 0;
      int dstOffset = 0;
      while (num2 < num1)
      {
        Buffer.BlockCopy((Array) BitConverter.GetBytes(index1 < count1 ? this.m_arrSectorID[index1] : -1), 0, (Array) numArray, dstOffset, 4);
        ++num2;
        ++index1;
        dstOffset += 4;
      }
      Buffer.BlockCopy((Array) BitConverter.GetBytes(index2 == count2 - 1 ? -2 : this.m_arrDifSectors[index2 + 1]), 0, (Array) numArray, sectorSize - 4, 4);
      stream.Position = sectorOffset;
      stream.Write(numArray, 0, sectorSize);
    }
  }

  internal void AllocateSectors(int fatSectorsRequired, FAT fat)
  {
    int num = fatSectorsRequired - 109;
    if (num <= 0)
      return;
    this.AllocateDifSectors((int) Math.Ceiling((double) (num * 4) / (double) (fat.SectorSize - 4)), fat);
  }

  private void AllocateDifSectors(int additionalSectors, FAT fat)
  {
    int count = this.m_arrDifSectors.Count;
    if (count == additionalSectors)
      return;
    if (count > additionalSectors)
      this.RemoveLastSectors(count - additionalSectors, fat);
    else
      this.AddDifSectors(additionalSectors - count, fat);
  }

  private void RemoveLastSectors(int sectorCount, FAT fat)
  {
    if (sectorCount < 0)
      throw new ArgumentOutOfRangeException(nameof (sectorCount));
    if (sectorCount == 0)
      return;
    if (fat == null)
      throw new ArgumentNullException(nameof (fat));
    int num = 0;
    int index = this.m_arrDifSectors.Count - 1;
    while (num < sectorCount)
    {
      int arrDifSector = this.m_arrDifSectors[index];
      fat.FreeSector(arrDifSector);
      ++num;
      --index;
    }
    this.m_arrDifSectors.RemoveRange(this.m_arrDifSectors.Count - sectorCount, sectorCount);
    throw new NotImplementedException();
  }

  private void AddDifSectors(int sectorCount, FAT fat)
  {
    if (sectorCount < 0)
      throw new ArgumentOutOfRangeException(nameof (sectorCount));
    if (fat == null)
      throw new ArgumentNullException(nameof (fat));
    for (int index = 0; index < sectorCount; ++index)
      this.m_arrDifSectors.Add(fat.AllocateSector(-4));
  }
}
