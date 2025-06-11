// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.DocIO.Net.CompoundFile
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

#nullable disable
namespace Syncfusion.CompoundFile.DocIO.Net;

public class CompoundFile : ICompoundFile, IDisposable
{
  private const string RootEntryName = "Root Entry";
  private Stream m_stream;
  private FileHeader m_header;
  private FAT m_fat;
  private DIF m_dif;
  private Directory m_directory;
  private CompoundStorage m_root;
  private Stream m_shortStream;
  private Stream m_miniFatStream;
  private FAT m_miniFat;
  private bool m_bDirectMode;

  internal FileHeader Header => this.m_header;

  public Directory Directory => this.m_directory;

  public ICompoundStorage Root => (ICompoundStorage) this.m_root;

  internal DIF DIF => this.m_dif;

  internal FAT Fat => this.m_fat;

  internal Stream BaseStream => this.m_stream;

  internal bool DirectMode
  {
    get => this.m_bDirectMode;
    set => this.m_bDirectMode = value;
  }

  public static void Main()
  {
    Syncfusion.CompoundFile.DocIO.Net.CompoundFile compoundFile = new Syncfusion.CompoundFile.DocIO.Net.CompoundFile();
    string path = "D:\\testfile\\";
    using (FileStream fileStream = new FileStream("test.bin", FileMode.Open, FileAccess.Read, FileShare.Read))
    {
      compoundFile.Open((Stream) fileStream);
      ICompoundStorage root = compoundFile.Root;
      Syncfusion.CompoundFile.DocIO.Net.CompoundFile.WriteDirectory(path, compoundFile.Directory);
      Syncfusion.CompoundFile.DocIO.Net.CompoundFile.WriteStorage(path, root);
    }
  }

  private static void WriteDirectory(string path, Directory directory)
  {
    using (StreamWriter streamWriter = new StreamWriter(Path.Combine(path, "directory.txt")))
    {
      List<DirectoryEntry> entries = directory.Entries;
      int index = 0;
      for (int count = entries.Count; index < count; ++index)
      {
        DirectoryEntry directoryEntry = entries[index];
        streamWriter.WriteLine(new string('-', 20));
        streamWriter.WriteLine("EntryId: {0}", (object) directoryEntry.EntryId);
        streamWriter.WriteLine("Name: {0}, EntryType: {1}", (object) directoryEntry.Name, (object) directoryEntry.Type);
        streamWriter.WriteLine("Left: {0}, Right: {1}, Child: {2}", (object) directoryEntry.LeftId, (object) directoryEntry.RightId, (object) directoryEntry.ChildId);
        streamWriter.WriteLine("Guid: {0}, DateCreate: {1}, DateModify: {2}", (object) directoryEntry.StorageGuid, (object) directoryEntry.DateCreate, (object) directoryEntry.DateModify);
        streamWriter.WriteLine("StartSector: {0}, Size: {1}", (object) directoryEntry.StartSector, (object) directoryEntry.Size);
      }
    }
  }

  private static void WriteStorage(string path, ICompoundStorage storage)
  {
    foreach (string stream in storage.Streams)
      Syncfusion.CompoundFile.DocIO.Net.CompoundFile.WriteStream(path, stream, storage);
    foreach (string storage1 in storage.Storages)
    {
      string path1 = Path.Combine(path, storage1);
      System.IO.Directory.CreateDirectory(path1);
      using (ICompoundStorage storage2 = storage.OpenStorage(storage1))
        Syncfusion.CompoundFile.DocIO.Net.CompoundFile.WriteStorage(path1, storage2);
    }
  }

  private static void WriteStream(string path, string streamName, ICompoundStorage storage)
  {
    byte[] buffer = new byte[32768 /*0x8000*/];
    using (Stream stream = (Stream) storage.OpenStream(streamName))
    {
      if (streamName[0] < ' ')
        streamName = streamName.Substring(1);
      using (FileStream fileStream = new FileStream(Path.Combine(path, streamName), FileMode.Create, FileAccess.Write, FileShare.None))
      {
        int count;
        while ((count = stream.Read(buffer, 0, 32768 /*0x8000*/)) > 0)
          fileStream.Write(buffer, 0, count);
      }
    }
  }

  public CompoundFile()
  {
    this.m_stream = (Stream) new MemoryStream();
    this.InitializeVariables();
  }

  public CompoundFile(Stream stream) => this.Open(stream);

  public CompoundFile(string fileName, bool create)
  {
    if (!create)
    {
      using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
        this.Open((Stream) fileStream);
    }
    else
    {
      this.m_stream = (Stream) new MemoryStream();
      this.InitializeVariables();
    }
  }

  public void Open(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    stream.Position = 0L;
    long position = stream.Position;
    int num = (int) (stream.Length - position);
    MemoryStream memoryStream = new MemoryStream(num);
    memoryStream.SetLength((long) num);
    stream.Read(memoryStream.GetBuffer(), 0, num);
    memoryStream.Position = 0L;
    this.m_stream = (Stream) memoryStream;
    this.m_header = new FileHeader(this.m_stream);
    this.m_dif = new DIF(this.m_stream, this.m_header);
    this.m_fat = new FAT(this, this.m_stream, this.m_dif, this.m_header);
    this.m_directory = new Directory(this.m_fat.GetStream(this.m_stream, this.m_header.DirectorySectorStart, this));
    DirectoryEntry entry = this.m_directory.Entries[0];
    this.m_root = new CompoundStorage(this, entry);
    int startSector = entry.StartSector;
    if (startSector < 0)
      return;
    this.m_shortStream = (Stream) new MemoryStream(this.m_fat.GetStream(this.m_stream, startSector, this));
    this.m_miniFatStream = (Stream) new MemoryStream(this.m_fat.GetStream(this.m_stream, this.m_header.MiniFastStart, this));
    this.m_miniFat = new FAT(this.m_shortStream, this.m_header.MiniSectorShift, this.m_miniFatStream, 0);
    this.m_fat.CloseChain(startSector);
    this.m_fat.CloseChain(this.m_header.MiniFastStart);
  }

  private void InitializeVariables()
  {
    this.m_directory = new Directory();
    this.m_root = new CompoundStorage(this, "Root Entry", 0);
    this.m_stream.SetLength(512L /*0x0200*/);
    this.m_header = new FileHeader();
    this.m_dif = new DIF();
    DirectoryEntry entry = this.m_root.Entry;
    entry.Type = DirectoryEntry.EntryType.Root;
    this.m_directory.Add(entry);
    this.m_fat = new FAT(this.m_stream, this.m_header.SectorShift, 512 /*0x0200*/);
  }

  internal void ReadSector(byte[] buffer, int offset, int sectorIndex, FileHeader header)
  {
    ushort sectorShift = header.SectorShift;
    int sectorSize = header.SectorSize;
    this.m_stream.Position = Syncfusion.CompoundFile.DocIO.Net.CompoundFile.GetSectorOffset(sectorIndex, sectorShift);
    this.m_stream.Read(buffer, offset, sectorSize);
  }

  internal Stream GetEntryStream(DirectoryEntry entry)
  {
    if (entry == null)
      throw new ArgumentNullException(nameof (entry));
    Stream entryStream = (Stream) null;
    if (entry.Type == DirectoryEntry.EntryType.Stream)
    {
      FAT fat;
      Stream stream1;
      if (this.m_miniFat != null && entry.Size < this.m_header.MiniSectorCutoff)
      {
        fat = this.m_miniFat;
        stream1 = this.m_shortStream;
      }
      else
      {
        fat = this.m_fat;
        stream1 = this.m_stream;
      }
      byte[] stream2 = fat.GetStream(stream1, entry.StartSector, this);
      entryStream = stream2 != null ? (Stream) new MemoryStream(stream2) : (Stream) new MemoryStream();
      entryStream.SetLength((long) entry.Size);
    }
    return entryStream;
  }

  internal void SetEntryStream(DirectoryEntry entry, Stream stream)
  {
    if (entry == null)
      throw new ArgumentNullException(nameof (entry));
    if (stream.Length >= (long) this.m_header.MiniSectorCutoff)
      this.SetEntryLongStream(entry, stream);
    else
      this.SetEntryShortStream(entry, stream);
    entry.Size = (uint) stream.Length;
  }

  private void SetEntryLongStream(DirectoryEntry entry, Stream stream)
  {
    int sectorShift = (int) this.m_header.SectorShift;
    int sectorSize = this.m_header.SectorSize;
    long size = (long) entry.Size;
    long length = stream.Length;
    int iAllocatedSectors = (int) Math.Ceiling((double) size / (double) sectorSize);
    int iRequiredSectors = (int) Math.Ceiling((double) length / (double) sectorSize);
    this.AllocateSectors(entry, iAllocatedSectors, iRequiredSectors, this.m_fat);
    this.WriteData(this.m_stream, entry.StartSector, stream, this.m_fat);
  }

  private void SetEntryShortStream(DirectoryEntry entry, Stream stream)
  {
    if (this.m_shortStream == null)
      this.m_shortStream = (Stream) new MemoryStream();
    if (this.m_miniFat == null)
      this.m_miniFat = new FAT(this.m_shortStream, this.m_header.MiniSectorShift, 0);
    int miniSectorShift = (int) this.m_header.MiniSectorShift;
    int sectorSize = this.m_miniFat.SectorSize;
    long size = (long) entry.Size;
    long length = stream.Length;
    int iAllocatedSectors = (int) Math.Ceiling((double) size / (double) sectorSize);
    int iRequiredSectors = (int) Math.Ceiling((double) length / (double) sectorSize);
    this.AllocateSectors(entry, iAllocatedSectors, iRequiredSectors, this.m_miniFat);
    this.WriteData(this.m_shortStream, entry.StartSector, stream, this.m_miniFat);
  }

  private void WriteData(Stream destination, int startSector, Stream stream, FAT fat)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    long sectorOffset = fat.GetSectorOffset(startSector);
    int sectorSize = fat.SectorSize;
    byte[] buffer = new byte[sectorSize];
    long position = stream.Position;
    stream.Position = 0L;
    int count;
    while ((count = stream.Read(buffer, 0, sectorSize)) > 0)
    {
      destination.Position = sectorOffset;
      destination.Write(buffer, 0, count);
      startSector = fat.NextSector(startSector);
      if (startSector >= 0)
        sectorOffset = fat.GetSectorOffset(startSector);
      else
        break;
    }
    stream.Position = position;
  }

  private void AllocateSectors(
    DirectoryEntry entry,
    int iAllocatedSectors,
    int iRequiredSectors,
    FAT fat)
  {
    if (iAllocatedSectors == iRequiredSectors)
      return;
    int iSector = entry.LastSector >= 0 ? entry.LastSector : entry.StartSector;
    int num = this.AllocateSectors(iSector, iAllocatedSectors, iRequiredSectors, fat);
    if (iSector >= 0)
      return;
    entry.StartSector = num;
  }

  private int AllocateSectors(int iSector, int iAllocatedSectors, int iRequiredSectors, FAT fat)
  {
    int num1 = -1;
    if (iAllocatedSectors == iRequiredSectors)
      num1 = iSector;
    else if (iAllocatedSectors < iRequiredSectors)
    {
      for (int index = iSector >= 0 ? fat.NextSector(iSector) : iSector; index >= 0; index = fat.NextSector(iSector))
        iSector = index;
      int num2 = fat.EnlargeChain(iSector, iRequiredSectors - iAllocatedSectors);
      if (iSector < 0)
        num1 = num2;
    }
    else
    {
      for (int index = 0; index < iRequiredSectors - 1; ++index)
        iSector = fat.NextSector(iSector);
      fat.CloseChain(iSector);
    }
    return num1;
  }

  [CLSCompliant(false)]
  public static long GetSectorOffset(int sectorIndex, ushort sectorShift)
  {
    return (long) ((sectorIndex << (int) sectorShift) + 512 /*0x0200*/);
  }

  [CLSCompliant(false)]
  public static long GetSectorOffset(int sectorIndex, ushort sectorShift, int headerSize)
  {
    return (long) ((sectorIndex << (int) sectorShift) + headerSize);
  }

  public static bool CheckHeader(Stream stream) => FileHeader.CheckSignature(stream);

  internal DirectoryEntry AllocateDirectoryEntry(
    string streamName,
    DirectoryEntry.EntryType entryType)
  {
    DirectoryEntry entry = new DirectoryEntry(streamName, entryType, this.m_directory.Entries.Count);
    this.m_directory.Add(entry);
    entry.DateModify = entry.DateCreate = DateTime.Now;
    return entry;
  }

  internal void RemoveItem(DirectoryEntry directoryEntry)
  {
    if (directoryEntry == null)
      throw new ArgumentNullException(nameof (directoryEntry));
    directoryEntry.Type = DirectoryEntry.EntryType.Invalid;
    if (directoryEntry.Type != DirectoryEntry.EntryType.Stream)
      return;
    this.m_fat.CloseChain(directoryEntry.StartSector);
    directoryEntry.StartSector = -1;
  }

  internal int ReadData(DirectoryEntry entry, long position, byte[] buffer, int length)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  internal void WriteData(
    DirectoryEntry entry,
    long position,
    byte[] buffer,
    int offset,
    int length)
  {
    int sectorShift = (int) this.m_header.SectorShift;
    int sectorSize1 = this.m_header.SectorSize;
    long size = (long) entry.Size;
    long val2 = position + (long) length;
    int iAllocatedSectors = (int) Math.Ceiling((double) size / (double) sectorSize1);
    int iRequiredSectors = (int) Math.Ceiling((double) val2 / (double) sectorSize1);
    if (iRequiredSectors > iAllocatedSectors)
      this.AllocateSectors(entry, iAllocatedSectors, iRequiredSectors, this.m_fat);
    int iCurrentSector = entry.StartSector;
    int sectorSize2 = this.m_fat.SectorSize;
    int iCurrentOffset = sectorSize2;
    this.GetOffsets(entry, position, ref iCurrentOffset, ref iCurrentSector);
    entry.LastSector = iCurrentSector;
    entry.LastOffset = iCurrentOffset;
    int num = (int) (position % (long) sectorSize2);
    while (length > 0)
    {
      int count = Math.Min(length, sectorSize2 - num);
      this.m_stream.Position = this.m_fat.GetSectorOffset(iCurrentSector) + (long) num;
      this.m_stream.Write(buffer, offset, count);
      num = 0;
      offset += count;
      length -= count;
      iCurrentSector = this.m_fat.NextSector(iCurrentSector);
    }
    entry.Size = (uint) Math.Max((long) entry.Size, val2);
  }

  private void GetOffsets(
    DirectoryEntry entry,
    long position,
    ref int iCurrentOffset,
    ref int iCurrentSector)
  {
    int sectorSize = this.m_fat.SectorSize;
    if (entry.LastSector >= 0)
    {
      int num1 = (int) position % sectorSize;
      long num2 = num1 > 0 ? position + (long) sectorSize - (long) num1 : position;
      if ((long) entry.LastOffset <= num2)
      {
        iCurrentOffset = entry.LastOffset;
        iCurrentSector = entry.LastSector;
      }
      else
        Debugger.Break();
    }
    while ((long) iCurrentOffset <= position)
    {
      iCurrentSector = this.m_fat.NextSector(iCurrentSector);
      iCurrentOffset += sectorSize;
    }
  }

  public ICompoundStorage RootStorage => (ICompoundStorage) this.m_root;

  public void Flush()
  {
    this.m_root.Flush();
    this.SaveMiniStream();
    this.SerializeDirectory();
    this.m_fat.Write(this.m_stream, this.m_dif, this.m_header);
    this.m_dif.Write(this.m_stream, this.m_header);
    this.m_header.Write(this.m_stream);
    this.m_stream.Position = 0L;
  }

  public void Save(Stream stream)
  {
    this.Flush();
    this.WriteStreamTo(stream);
  }

  private void WriteStreamTo(Stream destination)
  {
    if (this.m_stream is MemoryStream stream)
    {
      stream.WriteTo(destination);
    }
    else
    {
      byte[] buffer = new byte[32768 /*0x8000*/];
      int count;
      while ((count = this.m_stream.Read(buffer, 0, 32768 /*0x8000*/)) > 0)
        destination.Write(buffer, 0, count);
    }
  }

  private void SaveMiniStream()
  {
    if (this.m_shortStream == null || this.m_shortStream.Length == 0L)
      return;
    int iRequiredSectors1 = (int) Math.Ceiling((double) this.m_shortStream.Length / (double) this.m_header.SectorSize);
    DirectoryEntry entry1 = this.m_directory.Entries[0];
    int startSector1 = this.AllocateSectors(entry1.StartSector, (int) Math.Ceiling((double) entry1.Size / (double) this.m_fat.SectorSize), iRequiredSectors1, this.m_fat);
    this.WriteData(this.m_stream, startSector1, this.m_shortStream, this.m_fat);
    DirectoryEntry entry2 = this.m_directory.Entries[0];
    entry2.StartSector = startSector1;
    entry2.Size = (uint) this.m_shortStream.Length;
    MemoryStream stream = new MemoryStream();
    this.m_miniFat.WriteSimple(stream, this.m_header.SectorSize);
    int iRequiredSectors2 = (int) Math.Ceiling((double) stream.Length / (double) this.m_header.SectorSize);
    int startSector2 = this.AllocateSectors(this.m_header.MiniFastStart, this.m_header.MiniFatNumber, iRequiredSectors2, this.m_fat);
    this.WriteData(this.m_stream, startSector2, (Stream) stream, this.m_fat);
    this.m_header.MiniFastStart = startSector2;
    int miniSectorShift = (int) this.m_header.MiniSectorShift;
    this.m_header.MiniFatNumber = iRequiredSectors2;
    stream.Dispose();
  }

  private void SerializeDirectory()
  {
    MemoryStream memoryStream = new MemoryStream();
    this.m_directory.Write((Stream) memoryStream);
    int iRequiredSectors = (int) Math.Ceiling((double) memoryStream.Length / (double) this.m_header.SectorSize);
    int directorySectorStart = this.m_header.DirectorySectorStart;
    int chainLength = directorySectorStart >= 0 ? this.m_fat.GetChainLength(directorySectorStart) : 0;
    this.WriteData(this.m_stream, this.m_header.DirectorySectorStart = this.AllocateSectors(directorySectorStart, chainLength, iRequiredSectors, this.m_fat), (Stream) memoryStream, this.m_fat);
    memoryStream.Dispose();
  }

  public void Save(string fileName)
  {
    using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
      this.Save((Stream) fileStream);
  }

  public void Dispose()
  {
    if (this.m_root != null)
    {
      this.m_root.Dispose();
      this.m_root = (CompoundStorage) null;
      this.m_stream.Dispose();
      this.m_stream = (Stream) null;
      this.m_header = (FileHeader) null;
      this.m_fat = (FAT) null;
      this.m_directory = (Directory) null;
    }
    if (this.m_shortStream == null)
      return;
    this.m_shortStream.Dispose();
    this.m_shortStream = (Stream) null;
  }
}
