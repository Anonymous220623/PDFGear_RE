// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.XlsIO.Net.FAT
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.CompoundFile.XlsIO.Net;

internal class FAT
{
  private List<int> m_lstFatChains = new List<int>();
  private List<int> m_freeSectors = new List<int>();
  private ushort m_usSectorShift;
  private Stream m_stream;
  private int m_iHeaderSize;

  public int SectorSize => 1 << (int) this.m_usSectorShift;

  public FAT(Stream parentStream, ushort sectorShift, int headerSize)
  {
    this.m_stream = parentStream;
    this.m_usSectorShift = sectorShift;
    this.m_iHeaderSize = headerSize;
  }

  public FAT(Stream parentStream, ushort sectorShift, Stream fatStreamToParse, int headerSize)
  {
    this.m_usSectorShift = sectorShift;
    this.m_iHeaderSize = headerSize;
    this.m_stream = parentStream;
    fatStreamToParse.Position = 0L;
    byte[] buffer = new byte[4];
    while (fatStreamToParse.Read(buffer, 0, 4) > 0)
      this.m_lstFatChains.Add(BitConverter.ToInt32(buffer, 0));
  }

  public FAT(Syncfusion.CompoundFile.XlsIO.Net.CompoundFile file, Stream stream, DIF dif, FileHeader header)
  {
    if (file == null)
      throw new ArgumentNullException(nameof (file));
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    this.m_stream = file.BaseStream;
    List<int> sectorIds = dif.SectorIds;
    int sectorSize = header.SectorSize;
    this.m_usSectorShift = header.SectorShift;
    byte[] numArray1 = new byte[sectorSize];
    int[] numArray2 = new int[sectorSize >> 2];
    this.m_iHeaderSize = 512 /*0x0200*/;
    int index = 0;
    for (int count = sectorIds.Count; index < count; ++index)
    {
      int sectorIndex = sectorIds[index];
      if (sectorIndex >= 0)
      {
        file.ReadSector(numArray1, 0, sectorIndex, header);
        Buffer.BlockCopy((Array) numArray1, 0, (Array) numArray2, 0, sectorSize);
        this.m_lstFatChains.AddRange((IEnumerable<int>) numArray2);
      }
    }
  }

  public byte[] GetStream(Stream stream, int firstSector, Syncfusion.CompoundFile.XlsIO.Net.CompoundFile file)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (file == null)
      throw new ArgumentNullException(nameof (file));
    if (firstSector < 0)
      return (byte[]) null;
    List<int> intList = new List<int>();
    FileHeader header = file.Header;
    for (int index = firstSector; index != -2; index = this.m_lstFatChains[index])
    {
      if (index < 0 || index >= this.m_lstFatChains.Count)
        throw new ApplicationException();
      intList.Add(index);
    }
    int count1 = intList.Count;
    byte[] buffer = new byte[count1 << (int) this.m_usSectorShift];
    int count2 = 1 << (int) this.m_usSectorShift;
    int index1 = 0;
    int offset = 0;
    while (index1 < count1)
    {
      long sectorOffset = this.GetSectorOffset(intList[index1]);
      stream.Position = sectorOffset;
      stream.Read(buffer, offset, count2);
      ++index1;
      offset += count2;
    }
    return buffer;
  }

  internal int NextSector(int sectorIndex) => this.m_lstFatChains[sectorIndex];

  internal void CloseChain(int iSector)
  {
    int lstFatChain = this.m_lstFatChains[iSector];
    this.m_lstFatChains[iSector] = -2;
    while (lstFatChain != -2)
    {
      iSector = lstFatChain;
      lstFatChain = this.m_lstFatChains[iSector];
      this.m_lstFatChains[iSector] = -1;
      this.m_freeSectors.Add(iSector);
    }
  }

  internal int EnlargeChain(int sector, int sectorCount)
  {
    if (sectorCount <= 0)
      return sector;
    int count1 = this.m_freeSectors.Count;
    int count2 = Math.Min(sectorCount, count1);
    int count3 = sectorCount - count2;
    this.AllocateFreeSectors(ref sector, count2);
    int num = this.AllocateNewSectors(ref sector, count3);
    this.m_lstFatChains[sector] = -2;
    return num;
  }

  internal void FreeSector(int sector)
  {
    int count = this.m_lstFatChains.Count;
    if (sector < 0 || sector >= count)
      throw new ArgumentOutOfRangeException(nameof (sector));
    if (sector != count - 1)
    {
      this.m_lstFatChains[sector] = -1;
      this.m_freeSectors.Add(sector);
    }
    else
    {
      this.m_lstFatChains.RemoveAt(sector);
      this.FreeLastSector();
    }
  }

  private void FreeLastSector()
  {
    this.m_stream.SetLength(Math.Max(0L, this.m_stream.Length - (long) this.SectorSize));
  }

  private int AllocateNewSectors(ref int sector, int count)
  {
    int num1 = sector;
    int index = sector;
    sector = this.AddSectors(count);
    if (num1 < 0)
      num1 = sector;
    int num2 = 0;
    while (num2 < count)
    {
      this.m_lstFatChains.Add(sector + 1);
      if (index >= 0)
        this.m_lstFatChains[index] = sector;
      index = sector;
      ++num2;
      ++sector;
    }
    --sector;
    this.m_lstFatChains[sector] = -2;
    return num1;
  }

  private int AddSectors(int count)
  {
    long length = this.m_stream.Length;
    this.m_stream.SetLength(length + (long) (count << (int) this.m_usSectorShift));
    return (int) (length - (long) this.m_iHeaderSize >> (int) this.m_usSectorShift);
  }

  private int AllocateFreeSectors(ref int sector, int count)
  {
    int num = sector;
    for (int index = 0; index < count; ++index)
    {
      int freeSector = this.m_freeSectors[index];
      if (sector >= 0)
        this.m_lstFatChains[sector] = freeSector;
      else
        num = freeSector;
      sector = freeSector;
    }
    this.m_freeSectors.RemoveRange(0, count);
    return num;
  }

  public void Write(Stream stream, DIF dif, FileHeader header)
  {
    int count = this.m_lstFatChains.Count;
    int sectorSize = header.SectorSize;
    ushort sectorShift = header.SectorShift;
    int num1 = this.SectorSize / 4 - 1;
    int num2 = (int) Math.Ceiling(((double) num1 * (double) count - 109.0) / ((double) num1 * (double) num1 - 1.0));
    header.FatSectorsNumber = num2;
    byte[] numArray = new byte[sectorSize];
    dif.AllocateSectors(num2, this);
    this.AllocateFatSectors(num2, dif);
    List<int> sectorIds = dif.SectorIds;
    int index = 0;
    int fatItemToStart = 0;
    for (; index < num2; ++index)
    {
      fatItemToStart = this.FillNextSector(fatItemToStart, numArray);
      long sectorOffset = Syncfusion.CompoundFile.XlsIO.Net.CompoundFile.GetSectorOffset(sectorIds[index], sectorShift);
      stream.Seek(sectorOffset, SeekOrigin.Begin);
      stream.Write(numArray, 0, sectorSize);
    }
  }

  private void AllocateFatSectors(int fatSectorsCount, DIF dif)
  {
    List<int> intList = dif != null ? dif.SectorIds : throw new ArgumentNullException(nameof (dif));
    int count = intList.Count;
    if (count >= fatSectorsCount)
      return;
    for (int index = count; index < fatSectorsCount; ++index)
    {
      int num = this.AllocateSector(-3);
      intList.Add(num);
    }
  }

  private int FillNextSector(int fatItemToStart, byte[] arrSector)
  {
    int count = this.m_lstFatChains.Count;
    int length = arrSector.Length;
    int dstOffset;
    for (dstOffset = 0; dstOffset < length && fatItemToStart < count; ++fatItemToStart)
    {
      Buffer.BlockCopy((Array) BitConverter.GetBytes(this.m_lstFatChains[fatItemToStart]), 0, (Array) arrSector, dstOffset, 4);
      dstOffset += 4;
    }
    if (dstOffset < length)
    {
      byte[] bytes = BitConverter.GetBytes(-1);
      for (; dstOffset < length; dstOffset += 4)
        Buffer.BlockCopy((Array) bytes, 0, (Array) arrSector, dstOffset, 4);
    }
    return fatItemToStart;
  }

  internal int AllocateSector(int sectorType)
  {
    int count = this.m_freeSectors.Count;
    int index1;
    if (count > 0)
    {
      int index2 = count - 1;
      index1 = this.m_freeSectors[index2];
      this.m_freeSectors.RemoveAt(index2);
      this.m_lstFatChains[index1] = sectorType;
    }
    else
    {
      index1 = this.AddSector();
      this.m_lstFatChains.Add(sectorType);
    }
    return index1;
  }

  internal int AddSector()
  {
    long length = this.m_stream.Length;
    int sectorSize = this.SectorSize;
    this.m_stream.SetLength(length + (long) sectorSize);
    return (int) (length - (long) this.m_iHeaderSize >> (int) this.m_usSectorShift);
  }

  internal void WriteSimple(MemoryStream stream, int sectorSize)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (sectorSize <= 0)
      throw new ArgumentOutOfRangeException(nameof (sectorSize));
    int num1 = (int) Math.Ceiling((double) (this.m_lstFatChains.Count * 4) / (double) sectorSize);
    byte[] numArray = new byte[sectorSize];
    int num2 = sectorSize / 4;
    for (int index = 0; index < num1; ++index)
    {
      this.FillNextSector(index * num2, numArray);
      stream.Write(numArray, 0, sectorSize);
    }
  }

  internal long GetSectorOffset(int sectorIndex)
  {
    return Syncfusion.CompoundFile.XlsIO.Net.CompoundFile.GetSectorOffset(sectorIndex, this.m_usSectorShift, this.m_iHeaderSize);
  }

  internal int GetChainLength(int firstSector)
  {
    int chainLength = 1;
    while ((firstSector = this.NextSector(firstSector)) >= 0)
      ++chainLength;
    return chainLength;
  }
}
