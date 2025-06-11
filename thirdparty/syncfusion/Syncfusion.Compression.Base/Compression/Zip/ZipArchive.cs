// Decompiled with JetBrains decompiler
// Type: Syncfusion.Compression.Zip.ZipArchive
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.Compression.Zip;

public class ZipArchive : IDisposable
{
  private List<ZipArchiveItem> m_arrItems = new List<ZipArchiveItem>();
  private Dictionary<string, ZipArchiveItem> m_dicItems = new Dictionary<string, ZipArchiveItem>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
  private IFileNamePreprocessor m_fileNamePreprocessor;
  private bool m_bCheckCrc = true;
  private Syncfusion.Compression.CompressionLevel m_defaultLevel = Syncfusion.Compression.CompressionLevel.Best;
  private bool m_netCompression;
  private string m_password;
  private EncryptionAlgorithm m_encryptType;
  public ZipArchive.CompressorCreator CreateCompressor;

  public ZipArchiveItem this[int index]
  {
    get
    {
      if (index < 0 || index > this.m_arrItems.Count)
        throw new ArgumentOutOfRangeException(nameof (index));
      return this.m_arrItems[index];
    }
  }

  public ZipArchiveItem this[string itemName]
  {
    get
    {
      ZipArchiveItem zipArchiveItem;
      this.m_dicItems.TryGetValue(itemName, out zipArchiveItem);
      return zipArchiveItem;
    }
  }

  public int Count => this.m_arrItems == null ? 0 : this.m_arrItems.Count;

  public ZipArchiveItem[] Items
  {
    get
    {
      return this.m_arrItems != null ? this.m_arrItems.ToArray() : throw new ArgumentOutOfRangeException(nameof (Items));
    }
  }

  public IFileNamePreprocessor FileNamePreprocessor
  {
    get => this.m_fileNamePreprocessor;
    set => this.m_fileNamePreprocessor = value;
  }

  public Syncfusion.Compression.CompressionLevel DefaultCompressionLevel
  {
    get => this.m_defaultLevel;
    set => this.m_defaultLevel = value;
  }

  public bool CheckCrc
  {
    get => this.m_bCheckCrc;
    set => this.m_bCheckCrc = value;
  }

  public bool UseNetCompression
  {
    get => this.m_netCompression;
    set => this.m_netCompression = value;
  }

  public EncryptionAlgorithm EncryptionAlgorithm
  {
    get => this.m_encryptType;
    internal set => this.m_encryptType = value;
  }

  internal string Password
  {
    get => this.m_password;
    set => this.m_password = value;
  }

  [CLSCompliant(false)]
  public static long FindValueFromEnd(Stream stream, uint value, int maxCount)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (!stream.CanSeek || !stream.CanRead)
      throw new ArgumentOutOfRangeException("We need to have seekable and readable stream.");
    long length = stream.Length;
    if (length < 4L)
      return -1;
    byte[] buffer = new byte[4];
    long num1 = Math.Max(0L, length - (long) maxCount);
    long num2 = length - 1L - 4L;
    stream.Position = num2;
    stream.Read(buffer, 0, 4);
    uint num3 = BitConverter.ToUInt32(buffer, 0);
    bool flag = (int) num3 == (int) value;
    if (!flag)
    {
      while (num2 > num1)
      {
        uint num4 = num3 << 8;
        --num2;
        stream.Position = num2;
        num3 = num4 + (uint) stream.ReadByte();
        if ((int) num3 == (int) value)
        {
          flag = true;
          break;
        }
      }
    }
    return !flag ? -1L : num2;
  }

  public static int ReadInt32(Stream stream)
  {
    byte[] buffer = new byte[4];
    return stream.Read(buffer, 0, 4) == 4 ? BitConverter.ToInt32(buffer, 0) : throw new ZipException("Unable to read value at the specified position - end of stream was reached.");
  }

  public static short ReadInt16(Stream stream)
  {
    byte[] buffer = new byte[2];
    return stream.Read(buffer, 0, 2) == 2 ? BitConverter.ToInt16(buffer, 0) : throw new ZipException("Unable to read value at the specified position - end of stream was reached.");
  }

  public static ushort ReadUInt16(Stream stream)
  {
    byte[] buffer = new byte[2];
    return stream.Read(buffer, 0, 2) == 2 ? BitConverter.ToUInt16(buffer, 0) : throw new ZipException("Unable to read value at the specified position - end of stream was reached.");
  }

  internal static uint ReadUInt32(Stream stream)
  {
    byte[] buffer = new byte[4];
    return stream.Read(buffer, 0, 4) == 4 ? BitConverter.ToUInt32(buffer, 0) : throw new ZipException("Unable to read value at the specified position - end of stream was reached.");
  }

  public ZipArchive()
  {
    this.CreateCompressor = new ZipArchive.CompressorCreator(this.CreateNativeCompressor);
  }

  private Stream CreateNativeCompressor(Stream outputStream)
  {
    return this.m_netCompression ? (Stream) new NetCompressor(Syncfusion.Compression.CompressionLevel.Best, outputStream) : (Stream) new DeflateStream(outputStream, CompressionMode.Compress, true);
  }

  public ZipArchiveItem AddDirectory(string directoryName)
  {
    FileAttributes attributes = directoryName != null && directoryName.Length != 0 ? new DirectoryInfo(directoryName).Attributes : throw new ArgumentOutOfRangeException(nameof (directoryName));
    if (!Directory.Exists(directoryName))
      attributes = FileAttributes.Directory;
    if (this.m_fileNamePreprocessor != null)
      directoryName = this.m_fileNamePreprocessor.PreprocessName(directoryName);
    return this.AddItem(directoryName, (Stream) null, false, attributes);
  }

  public ZipArchiveItem AddFile(string fileName)
  {
    Stream data = (Stream) new FileStream(fileName, FileMode.Open, FileAccess.Read);
    FileInfo fileInfo = new FileInfo(fileName);
    int attributes = (int) fileInfo.Attributes;
    if (this.m_fileNamePreprocessor != null)
      fileName = this.m_fileNamePreprocessor.PreprocessName(fileName);
    fileName = Path.GetFileName(fileName);
    return this.AddItem(fileName, data, true, fileInfo);
  }

  public ZipArchiveItem AddItem(
    string itemName,
    Stream data,
    bool bControlStream,
    FileAttributes attributes)
  {
    itemName = itemName.Replace('\\', '/');
    if (itemName.IndexOf(':') != itemName.LastIndexOf(':'))
      throw new ArgumentOutOfRangeException("ZipItem name contains illegal characters.", nameof (itemName));
    if (this.m_dicItems.ContainsKey(itemName))
      throw new ArgumentOutOfRangeException($"Item {itemName} already exists in the archive");
    return this.AddItem(new ZipArchiveItem(this, itemName, data, bControlStream, attributes)
    {
      CompressionLevel = this.m_defaultLevel
    });
  }

  internal ZipArchiveItem AddItem(
    string itemName,
    Stream data,
    bool bControlStream,
    FileInfo fileInfo)
  {
    ZipArchiveItem zipArchiveItem = this.AddItem(itemName, data, bControlStream, fileInfo.Attributes);
    zipArchiveItem.LastModified = new DateTime?(fileInfo.LastWriteTime);
    return zipArchiveItem;
  }

  public ZipArchiveItem AddItem(ZipArchiveItem item)
  {
    if (item == null)
      throw new ArgumentNullException(nameof (item));
    this.m_arrItems.Add(item);
    this.m_dicItems.Add(item.ItemName, item);
    return item;
  }

  public void RemoveItem(string itemName)
  {
    int index = this.Find(itemName);
    if (index < 0)
      return;
    this.RemoveAt(index);
  }

  public void RemoveAt(int index)
  {
    if (index < 0 || index >= this.m_arrItems.Count)
      throw new ArgumentOutOfRangeException(nameof (index));
    ZipArchiveItem zipArchiveItem = this[index];
    this.m_arrItems.RemoveAt(index);
    this.m_dicItems.Remove(zipArchiveItem.ItemName);
  }

  public void Remove(Regex mask)
  {
    int index = 0;
    for (int count = this.m_arrItems.Count; index < count; ++index)
    {
      string itemName = this.m_arrItems[index].ItemName;
      if (mask.IsMatch(itemName))
      {
        this.m_arrItems.RemoveAt(index);
        this.m_dicItems.Remove(itemName);
        --index;
        --count;
      }
    }
  }

  public void UpdateItem(string itemName, Stream newDataStream, bool controlStream)
  {
    (this[itemName] ?? throw new ArgumentOutOfRangeException(nameof (itemName), "Cannot find specified item.")).Update(newDataStream, controlStream);
  }

  public void UpdateItem(
    string itemName,
    Stream newDataStream,
    bool controlStream,
    FileAttributes attributes)
  {
    ZipArchiveItem zipArchiveItem = this[itemName];
    if (zipArchiveItem != null)
      zipArchiveItem.Update(newDataStream, controlStream);
    else
      this.AddItem(itemName, newDataStream, controlStream, attributes);
  }

  public void UpdateItem(string itemName, byte[] newData)
  {
    ZipArchiveItem zipArchiveItem = this[itemName];
    if (zipArchiveItem == null)
      throw new ArgumentOutOfRangeException(nameof (itemName), "Cannot find specified item.");
    MemoryStream newDataStream = new MemoryStream(newData);
    zipArchiveItem.Update((Stream) newDataStream, true);
  }

  public void Save(string outputFileName)
  {
    if (outputFileName == null || outputFileName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (outputFileName));
    this.Save(outputFileName, false);
  }

  public void Save(string outputFileName, bool createFilePath)
  {
    if (outputFileName == null || outputFileName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (outputFileName));
    if (createFilePath)
    {
      string directoryName = Path.GetDirectoryName(Path.GetFullPath(outputFileName));
      if (!Directory.Exists(directoryName))
        Directory.CreateDirectory(directoryName);
    }
    using (FileStream fileStream = new FileStream(outputFileName, FileMode.Create, FileAccess.ReadWrite))
      this.Save((Stream) fileStream, false);
  }

  public void Save(Stream stream, bool closeStream)
  {
    if (stream == null)
      throw new ArgumentNullException();
    Stream stream1 = (Stream) null;
    if (!stream.CanSeek)
    {
      stream1 = stream;
      stream = (Stream) new MemoryStream();
    }
    stream.Position = 0L;
    int index = 0;
    for (int count = this.m_arrItems.Count; index < count; ++index)
      this.m_arrItems[index].Write(stream);
    this.WriteCentralDirectory(stream);
    if (stream1 != null)
    {
      stream.Position = 0L;
      ((MemoryStream) stream).WriteTo(stream1);
      stream.Close();
      stream = stream1;
    }
    if (!closeStream)
      return;
    stream.Close();
  }

  public void Open(string inputFileName)
  {
    if (inputFileName == null || inputFileName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (inputFileName));
    using (FileStream fileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read))
      this.Open((Stream) fileStream, false);
  }

  public void Open(Stream stream, bool closeStream)
  {
    long num1 = stream != null ? ZipArchive.FindValueFromEnd(stream, 101010256U, 65557) : throw new ArgumentNullException(nameof (stream));
    if (num1 < 0L)
      throw new ZipException("Can't locate end of central directory record. Possible wrong file format or archive is corrupt.");
    stream.Position = num1 + 12L;
    int num2 = ZipArchive.ReadInt32(stream);
    long num3 = num1 - (long) num2;
    stream.Position = num3;
    this.ReadCentralDirectoryData(stream);
    this.ExtractItems(stream);
  }

  public void Close()
  {
    int index = 0;
    for (int count = this.m_arrItems.Count; index < count; ++index)
      this.m_arrItems[index].Close();
    this.m_arrItems.Clear();
    this.m_dicItems.Clear();
    this.m_dicItems = (Dictionary<string, ZipArchiveItem>) null;
  }

  public int Find(string itemName)
  {
    int num = -1;
    ZipArchiveItem zipArchiveItem;
    if (this.m_dicItems.TryGetValue(itemName, out zipArchiveItem))
    {
      int index = 0;
      for (int count = this.m_arrItems.Count; index < count; ++index)
      {
        if (this.m_arrItems[index] == zipArchiveItem)
        {
          num = index;
          break;
        }
      }
    }
    return num;
  }

  public int Find(Regex itemRegex)
  {
    int num = -1;
    int index = 0;
    for (int count = this.m_arrItems.Count; index < count; ++index)
    {
      string itemName = this.m_arrItems[index].ItemName;
      if (itemRegex.IsMatch(itemName))
      {
        num = index;
        break;
      }
    }
    return num;
  }

  private void WriteCentralDirectory(Stream stream)
  {
    long position = stream.Position;
    int index = 0;
    for (int count = this.m_arrItems.Count; index < count; ++index)
      this.m_arrItems[index].WriteFileHeader(stream);
    this.WriteCentralDirectoryEnd(stream, position);
  }

  private void WriteCentralDirectoryEnd(Stream stream, long directoryStart)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    int num = (int) (stream.Position - directoryStart);
    stream.Write(BitConverter.GetBytes(101010256), 0, 4);
    stream.WriteByte((byte) 0);
    stream.WriteByte((byte) 0);
    stream.WriteByte((byte) 0);
    stream.WriteByte((byte) 0);
    byte[] bytes = BitConverter.GetBytes((short) this.m_arrItems.Count);
    stream.Write(bytes, 0, 2);
    stream.Write(bytes, 0, 2);
    stream.Write(BitConverter.GetBytes(num), 0, 4);
    stream.Write(BitConverter.GetBytes((int) directoryStart), 0, 4);
    stream.WriteByte((byte) 0);
    stream.WriteByte((byte) 0);
  }

  private void ReadCentralDirectoryData(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    while (ZipArchive.ReadInt32(stream) == 33639248)
    {
      ZipArchiveItem zipArchiveItem = new ZipArchiveItem(this);
      zipArchiveItem.ReadCentralDirectoryData(stream);
      this.m_arrItems.Add(zipArchiveItem);
    }
  }

  private void ExtractItems(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException();
    if (!stream.CanSeek || !stream.CanRead)
      throw new ArgumentOutOfRangeException(nameof (stream), "We need seekable and readable stream to parse items.");
    int index = 0;
    for (int count = this.m_arrItems.Count; index < count; ++index)
    {
      ZipArchiveItem arrItem = this.m_arrItems[index];
      arrItem.ReadData(stream, this.m_bCheckCrc);
      this.m_dicItems.Add(arrItem.ItemName, arrItem);
    }
  }

  public ZipArchive Clone()
  {
    ZipArchive zipArchive = (ZipArchive) this.MemberwiseClone();
    zipArchive.m_arrItems = new List<ZipArchiveItem>();
    zipArchive.m_dicItems = new Dictionary<string, ZipArchiveItem>();
    int index = 0;
    for (int count = this.m_arrItems.Count; index < count; ++index)
    {
      ZipArchiveItem zipArchiveItem = this.m_arrItems[index].Clone();
      zipArchive.AddItem(zipArchiveItem);
    }
    return zipArchive;
  }

  public void Protect(string password, EncryptionAlgorithm type)
  {
    if (string.IsNullOrEmpty(password))
      throw new ArgumentNullException(nameof (password));
    if (type == EncryptionAlgorithm.None)
      return;
    this.m_encryptType = type;
    this.m_password = password;
  }

  public void UnProtect()
  {
    this.m_password = (string) null;
    this.m_encryptType = EncryptionAlgorithm.None;
  }

  public void Open(string fileName, string password)
  {
    if (string.IsNullOrEmpty(fileName))
      throw new ArgumentNullException(nameof (fileName));
    this.m_password = !string.IsNullOrEmpty(password) ? password : throw new ArgumentNullException(nameof (password));
    this.Open(fileName);
  }

  public void Open(Stream stream, bool closeStream, string password)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    this.m_password = !string.IsNullOrEmpty(password) ? password : throw new ArgumentNullException(nameof (password));
    this.Open(stream, closeStream);
  }

  public void Dispose()
  {
    if (this.m_arrItems != null)
    {
      int index = 0;
      for (int count = this.m_arrItems.Count; index < count; ++index)
        this.m_arrItems[index].Dispose();
    }
    if (this.m_password != null)
      this.m_password = (string) null;
    GC.SuppressFinalize((object) this);
  }

  ~ZipArchive()
  {
    if (this.m_arrItems == null)
      return;
    this.Dispose();
  }

  public delegate Stream CompressorCreator(Stream outputStream);
}
