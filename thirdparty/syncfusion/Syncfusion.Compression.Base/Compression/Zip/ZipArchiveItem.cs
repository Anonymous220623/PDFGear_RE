// Decompiled with JetBrains decompiler
// Type: Syncfusion.Compression.Zip.ZipArchiveItem
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;
using System.IO;
using System.IO.Compression;
using System.Text;

#nullable disable
namespace Syncfusion.Compression.Zip;

public class ZipArchiveItem : IDisposable
{
  private const int MaxAnsiCode = 255 /*0xFF*/;
  private string m_strItemName;
  private CompressionMethod m_compressionMethod = CompressionMethod.Deflated;
  private Syncfusion.Compression.CompressionLevel m_compressionLevel = Syncfusion.Compression.CompressionLevel.Normal;
  private uint m_uiCrc32;
  private Stream m_streamData;
  private long m_lCompressedSize;
  private long m_lOriginalSize;
  private bool m_bControlStream;
  private bool m_bCompressed;
  private long m_lCrcPosition;
  private int m_iLocalHeaderOffset;
  private GeneralPurposeBitFlags m_options;
  private int m_iExternalAttributes;
  private bool m_bCheckCrc;
  private bool m_bOptimizedDecompress;
  private ZipArchive m_archive;
  private byte[] m_actualCompression;
  private DateTime? m_lastModfied;

  public string ItemName
  {
    get => this.m_strItemName;
    set
    {
      this.m_strItemName = value != null && value.Length != 0 ? value : throw new ArgumentOutOfRangeException(nameof (ItemName));
    }
  }

  public CompressionMethod CompressionMethod
  {
    get => this.m_compressionMethod;
    set => this.m_compressionMethod = value;
  }

  public Syncfusion.Compression.CompressionLevel CompressionLevel
  {
    get => this.m_compressionLevel;
    set
    {
      if (this.m_compressionLevel == value)
        return;
      if (this.m_bCompressed)
        this.DecompressData();
      this.m_compressionLevel = value;
    }
  }

  [CLSCompliant(false)]
  public uint Crc32 => this.m_uiCrc32;

  public Stream DataStream
  {
    get
    {
      if (this.m_bCompressed)
        this.DecompressData();
      return this.m_streamData;
    }
  }

  public long CompressedSize => this.m_lCompressedSize;

  public long OriginalSize => this.m_lOriginalSize;

  public bool ControlStream => this.m_bControlStream;

  public bool Compressed => this.m_bCompressed;

  public FileAttributes ExternalAttributes
  {
    get => (FileAttributes) this.m_iExternalAttributes;
    set => this.m_iExternalAttributes = (int) value;
  }

  public bool OptimizedDecompress
  {
    get => this.m_bOptimizedDecompress;
    set => this.m_bOptimizedDecompress = value;
  }

  public static int OemCodePage => 437;

  private static int LatinOemCode => 850;

  internal DateTime? LastModified
  {
    get => this.m_lastModfied;
    set => this.m_lastModfied = value;
  }

  internal ZipArchiveItem(ZipArchive archive) => this.m_archive = archive;

  public ZipArchiveItem(
    ZipArchive archive,
    string itemName,
    Stream streamData,
    bool controlStream,
    FileAttributes attributes)
    : this(archive)
  {
    this.m_strItemName = itemName;
    this.m_bControlStream = controlStream;
    this.m_streamData = streamData;
    this.m_iExternalAttributes = (int) attributes;
  }

  public void Update(ZippedContentStream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (this.m_streamData != null && this.m_bControlStream)
      this.m_streamData.Close();
    this.m_streamData = stream.ZippedContent;
    this.m_lCompressedSize = this.m_streamData.Length;
    this.m_lOriginalSize = stream.UnzippedSize;
    this.m_bCompressed = true;
    this.m_uiCrc32 = stream.Crc32;
    this.m_bControlStream = false;
  }

  public void Update(Stream newDataStream, bool controlStream)
  {
    if (this.m_streamData != null && this.m_bControlStream && this.m_streamData != newDataStream)
      this.m_streamData.Close();
    this.m_bControlStream = controlStream;
    this.m_streamData = newDataStream;
    this.ResetFlags();
    this.m_lOriginalSize = newDataStream != null ? newDataStream.Length : 0L;
  }

  public void ResetFlags()
  {
    this.m_lCompressedSize = 0L;
    this.m_lOriginalSize = 0L;
    this.m_bCompressed = false;
    this.m_uiCrc32 = 0U;
  }

  internal void Write(Stream outputStream)
  {
    if (this.m_streamData == null || this.m_streamData.Length == 0L)
    {
      this.m_compressionLevel = Syncfusion.Compression.CompressionLevel.NoCompression;
      this.m_compressionMethod = CompressionMethod.Stored;
    }
    this.WriteHeader(outputStream);
    this.WriteZippedContent(outputStream);
    if (this.m_archive.Password != null && this.m_streamData != null)
    {
      byte[] plainData = new byte[this.m_lCompressedSize];
      outputStream.Position = (long) (int) (outputStream.Position - this.m_lCompressedSize);
      for (int index = 0; (long) index < this.m_lCompressedSize; ++index)
        plainData[index] = (byte) outputStream.ReadByte();
      byte[] numArray = this.Encrypt(plainData);
      outputStream.Position = (long) (int) (outputStream.Position - this.m_lCompressedSize);
      for (int index = 0; index < numArray.Length; ++index)
        outputStream.WriteByte(numArray[index]);
      this.m_lCompressedSize = (long) numArray.Length;
    }
    if (this.m_uiCrc32 == 0U)
      this.m_bCheckCrc = false;
    if (this.m_uiCrc32 == 0U && this.m_archive.Password == null && this.DataStream != null)
    {
      long length1 = this.DataStream.Length;
      byte[] buffer = new byte[4096 /*0x1000*/];
      Stream dataStream = this.DataStream;
      while (length1 > 0L)
      {
        int length2 = dataStream.Read(buffer, 0, 4096 /*0x1000*/);
        length1 -= (long) length2;
        this.m_uiCrc32 = ZipCrc32.ComputeCrc(buffer, 0, length2, this.m_uiCrc32);
      }
      this.m_bCheckCrc = true;
    }
    this.WriteFooter(outputStream);
  }

  internal void Close()
  {
    if (this.m_streamData == null)
      return;
    if (this.m_bControlStream)
      this.m_streamData.Close();
    this.m_streamData = (Stream) null;
    this.m_strItemName = (string) null;
  }

  internal void WriteFileHeader(Stream stream)
  {
    stream.Write(BitConverter.GetBytes(33639248), 0, 4);
    stream.Write(BitConverter.GetBytes((short) 45), 0, 2);
    if (this.m_compressionMethod == (CompressionMethod.PPMd | CompressionMethod.Shrunk))
      stream.Write(BitConverter.GetBytes((short) 51), 0, 2);
    else
      stream.Write(BitConverter.GetBytes((short) 20), 0, 2);
    if (this.m_compressionMethod == (CompressionMethod.PPMd | CompressionMethod.Shrunk) && this.m_archive.Password != null)
      this.m_options = (GeneralPurposeBitFlags) 1;
    stream.Write(BitConverter.GetBytes((short) this.m_options), 0, 2);
    stream.Write(BitConverter.GetBytes((short) this.m_compressionMethod), 0, 2);
    int num = !this.LastModified.HasValue ? this.ConvertDateTime(DateTime.Now) : this.ConvertDateTime(this.LastModified.Value);
    byte[] buffer = new byte[4]
    {
      (byte) (num & (int) byte.MaxValue),
      (byte) ((num & 65280) >> 8),
      (byte) ((num & 16711680 /*0xFF0000*/) >> 16 /*0x10*/),
      (byte) (((long) num & 4278190080L /*0xFF000000*/) >> 24)
    };
    stream.Write(buffer, 0, 4);
    stream.Write(BitConverter.GetBytes(this.m_uiCrc32), 0, 4);
    stream.Write(BitConverter.GetBytes((int) this.m_lCompressedSize), 0, 4);
    stream.Write(BitConverter.GetBytes((int) this.m_lOriginalSize), 0, 4);
    Encoding encoding = (this.m_options & GeneralPurposeBitFlags.Unicode) != (GeneralPurposeBitFlags) 0 ? Encoding.UTF8 : Encoding.GetEncoding(ZipArchiveItem.OemCodePage);
    if (this.m_options != GeneralPurposeBitFlags.Unicode && this.CheckForLatin(this.m_strItemName))
      encoding = Encoding.GetEncoding(ZipArchiveItem.LatinOemCode);
    byte[] bytes = encoding.GetBytes(this.m_strItemName);
    int byteCount = encoding.GetByteCount(this.m_strItemName);
    stream.Write(BitConverter.GetBytes((short) byteCount), 0, 2);
    if (this.m_compressionMethod == (CompressionMethod.PPMd | CompressionMethod.Shrunk))
      stream.WriteByte((byte) 47);
    else
      stream.WriteByte((byte) 0);
    stream.WriteByte((byte) 0);
    stream.WriteByte((byte) 0);
    stream.WriteByte((byte) 0);
    stream.WriteByte((byte) 0);
    stream.WriteByte((byte) 0);
    stream.WriteByte((byte) 0);
    stream.WriteByte((byte) 0);
    stream.Write(BitConverter.GetBytes(this.m_iExternalAttributes), 0, 4);
    stream.Write(BitConverter.GetBytes(this.m_iLocalHeaderOffset), 0, 4);
    stream.Write(bytes, 0, bytes.Length);
    if (this.m_compressionMethod != (CompressionMethod.PPMd | CompressionMethod.Shrunk) || this.m_archive.Password == null)
      return;
    stream.Position += 36L;
    this.WriteEncryptionHeader(stream);
  }

  private int ConvertDateTime(DateTime time)
  {
    time = time.ToLocalTime();
    return (int) (ushort) (time.Day & 31 /*0x1F*/ | time.Month << 5 & 480 | time.Year - 1980 << 9 & 65024) << 16 /*0x10*/ | (int) (ushort) (time.Second / 2 & 31 /*0x1F*/ | time.Minute << 5 & 2016 | time.Hour << 11 & 63488);
  }

  private DateTime ConvertToDateTime(int dateTimeValue)
  {
    uint num1 = (uint) (dateTimeValue >> 16 /*0x10*/);
    uint num2 = (uint) ((ulong) dateTimeValue & (ulong) num1);
    ushort second = (ushort) (((int) num2 & 31 /*0x1F*/) * 2);
    ushort minute = (ushort) ((num2 & 2016U) >> 5);
    ushort hour = (ushort) ((num2 & 63488U) >> 11);
    ushort day = (ushort) (num1 & 31U /*0x1F*/);
    ushort month = (ushort) ((num1 & 480U) >> 5);
    ushort year = (ushort) (((num1 & 65024U) >> 9) + 1980U);
    return year <= (ushort) 0 || year > (ushort) 9999 || month <= (ushort) 0 || month > (ushort) 12 || day <= (ushort) 0 || !this.CheckValidDate(year, month, day) || hour < (ushort) 0 || hour > (ushort) 24 || minute < (ushort) 0 || minute > (ushort) 60 || second < (ushort) 0 || second > (ushort) 60 ? DateTime.Now : new DateTime((int) year, (int) month, (int) day, (int) hour, (int) minute, (int) second);
  }

  internal bool CheckValidDate(ushort year, ushort month, ushort day)
  {
    switch (month)
    {
      case 1:
        return day <= (ushort) 31 /*0x1F*/;
      case 2:
        return (int) year % 4 == 0 && (int) year % 100 != 0 || (int) year % 400 == 0 ? day <= (ushort) 29 : day <= (ushort) 28;
      case 3:
        return day <= (ushort) 31 /*0x1F*/;
      case 4:
        return day <= (ushort) 30;
      case 5:
        return day <= (ushort) 31 /*0x1F*/;
      case 6:
        return day <= (ushort) 30;
      case 7:
        return day <= (ushort) 31 /*0x1F*/;
      case 8:
        return day <= (ushort) 31 /*0x1F*/;
      case 9:
        return day <= (ushort) 30;
      case 10:
        return day <= (ushort) 31 /*0x1F*/;
      case 11:
        return day <= (ushort) 30;
      case 12:
        return day <= (ushort) 31 /*0x1F*/;
      default:
        return false;
    }
  }

  internal void ReadCentralDirectoryData(Stream stream)
  {
    stream.Position += 4L;
    this.m_options = (GeneralPurposeBitFlags) ZipArchive.ReadInt16(stream);
    if (this.m_options == (GeneralPurposeBitFlags) 1 && this.m_archive.Password == null)
      throw new Exception("Password required");
    if (this.m_options == (GeneralPurposeBitFlags) 1)
      this.m_archive.EncryptionAlgorithm = EncryptionAlgorithm.ZipCrypto;
    this.m_compressionMethod = (CompressionMethod) ZipArchive.ReadInt16(stream);
    this.m_bCheckCrc = this.m_compressionMethod != (CompressionMethod.PPMd | CompressionMethod.Shrunk);
    this.m_bCompressed = true;
    this.LastModified = new DateTime?(this.ConvertToDateTime(ZipArchive.ReadInt32(stream)));
    this.m_uiCrc32 = (uint) ZipArchive.ReadInt32(stream);
    this.m_lCompressedSize = (long) ZipArchive.ReadInt32(stream);
    this.m_lOriginalSize = (long) ZipArchive.ReadInt32(stream);
    int count = (int) ZipArchive.ReadInt16(stream);
    int num1 = (int) ZipArchive.ReadInt16(stream);
    int num2 = (int) ZipArchive.ReadInt16(stream);
    stream.Position += 4L;
    this.m_iExternalAttributes = ZipArchive.ReadInt32(stream);
    this.m_iLocalHeaderOffset = ZipArchive.ReadInt32(stream);
    byte[] numArray = new byte[count];
    stream.Read(numArray, 0, count);
    this.m_strItemName = ((this.m_options & GeneralPurposeBitFlags.Unicode) != (GeneralPurposeBitFlags) 0 ? Encoding.UTF8 : Encoding.GetEncoding(ZipArchiveItem.OemCodePage)).GetString(numArray, 0, numArray.Length);
    this.m_strItemName = this.m_strItemName.Replace("\\", "/");
    if (this.m_options != GeneralPurposeBitFlags.Unicode)
      this.m_strItemName = this.CheckForLatin(this.m_strItemName) ? Encoding.GetEncoding(ZipArchiveItem.LatinOemCode).GetString(numArray, 0, numArray.Length).Replace("\\", "/") : this.m_strItemName;
    if (this.m_strItemName.ToLower().StartsWith("customxml"))
      this.m_strItemName = "customXml" + this.m_strItemName.Remove(0, 9);
    stream.Position += (long) (num1 + num2);
    if (this.m_options == (GeneralPurposeBitFlags) 0)
      return;
    this.m_options = (GeneralPurposeBitFlags) 0;
  }

  internal void ReadData(Stream stream, bool checkCrc)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    stream.Position = (long) this.m_iLocalHeaderOffset;
    this.m_bCheckCrc = checkCrc;
    this.ReadLocalHeader(stream);
    this.ReadCompressedData(stream);
  }

  private void ReadCompressedData(Stream stream)
  {
    if (this.m_lCompressedSize > 0L)
    {
      MemoryStream memoryStream = new MemoryStream();
      int lCompressedSize = (int) this.m_lCompressedSize;
      memoryStream.Capacity = lCompressedSize;
      byte[] buffer = new byte[4096 /*0x1000*/];
      int count;
      for (; lCompressedSize > 0; lCompressedSize -= count)
      {
        count = Math.Min(lCompressedSize, 4096 /*0x1000*/);
        if (stream.Read(buffer, 0, count) != count)
          throw new ZipException("End of file reached - wrong file format or file is corrupt.");
        memoryStream.Write(buffer, 0, count);
      }
      if (this.m_archive.Password != null)
      {
        byte[] numArray = new byte[memoryStream.Length];
        memoryStream = new MemoryStream(this.Decrypt(memoryStream.ToArray()));
      }
      this.m_streamData = (Stream) memoryStream;
      this.m_bControlStream = true;
    }
    else
    {
      if (this.m_lCompressedSize >= 0L)
        return;
      MemoryStream memoryStream = new MemoryStream();
      bool flag = true;
      while (flag)
      {
        int num1;
        if ((num1 = stream.ReadByte()) == 80 /*0x50*/)
        {
          --stream.Position;
          int num2 = ZipArchive.ReadInt32(stream);
          if (num2 == 33639248 || num2 == 33639248)
            flag = false;
          stream.Position -= 3L;
        }
        if (flag)
          memoryStream.WriteByte((byte) num1);
      }
      this.m_streamData = (Stream) memoryStream;
      this.m_lCompressedSize = this.m_streamData.Length;
      this.m_bControlStream = true;
    }
  }

  private void ReadLocalHeader(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (ZipArchive.ReadInt32(stream) != 67324752)
      throw new ZipException("Can't find local header signature - wrong file format or file is corrupt.");
    stream.Position += 22L;
    int num1 = (int) ZipArchive.ReadInt16(stream);
    int num2 = (int) ZipArchive.ReadUInt16(stream);
    if (this.m_compressionMethod == (CompressionMethod.PPMd | CompressionMethod.Shrunk))
    {
      stream.Position += (long) (num1 + 8);
      this.m_archive.EncryptionAlgorithm = (EncryptionAlgorithm) stream.ReadByte();
      this.m_actualCompression = new byte[2];
      stream.Read(this.m_actualCompression, 0, 2);
    }
    else if (num2 > 2)
    {
      stream.Position += (long) num1;
      if (ZipArchive.ReadInt16(stream) == (short) 23)
        throw new Exception("UnSupported");
      stream.Position += (long) (num2 - 2);
    }
    else
      stream.Position += (long) (num1 + num2);
  }

  private void DecompressData()
  {
    if (!this.m_bCompressed)
      return;
    if (this.m_compressionMethod == CompressionMethod.Deflated)
    {
      if (this.m_lOriginalSize > 0L)
      {
        this.m_streamData.Position = 0L;
        if (this.m_bOptimizedDecompress)
          this.DecompressDataMemoryOptimized();
        else
          this.DecompressDataOrdinary();
      }
      else
      {
        this.m_streamData.Position = 0L;
        if (this.m_bOptimizedDecompress)
          this.DecompressDataMemoryOptimized();
        else
          this.DecompressDataOrdinary();
      }
      this.m_bCompressed = false;
    }
    else
    {
      if (this.m_compressionMethod != CompressionMethod.Stored)
        throw new NotSupportedException($"Compression type: {this.m_compressionMethod.ToString()} is not supported");
      this.m_bCompressed = false;
    }
  }

  private void DecompressDataOld()
  {
    CompressedStreamReader compressedStreamReader = new CompressedStreamReader(this.m_streamData, true);
    MemoryStream memoryStream = new MemoryStream();
    if (this.m_lOriginalSize > 0L)
      memoryStream.Capacity = (int) this.m_lOriginalSize;
    byte[] buffer = new byte[4096 /*0x1000*/];
    int count;
    while ((count = compressedStreamReader.Read(buffer, 0, 4096 /*0x1000*/)) > 0)
      memoryStream.Write(buffer, 0, count);
    if (this.m_bControlStream)
      this.m_streamData.Close();
    this.m_lOriginalSize = memoryStream.Length;
    this.m_bControlStream = true;
    this.m_streamData = (Stream) memoryStream;
    memoryStream.SetLength(this.m_lOriginalSize);
    memoryStream.Capacity = (int) this.m_lOriginalSize;
    if (this.m_bCheckCrc)
      this.CheckCrc(memoryStream.GetBuffer());
    this.m_streamData.Position = 0L;
  }

  private void DecompressDataMemoryOptimized()
  {
    this.m_streamData = (Stream) new DeflateStream(this.m_streamData, CompressionMode.Decompress, this.m_bControlStream);
  }

  private void DecompressDataOrdinary()
  {
    DeflateStream deflateStream = new DeflateStream(this.m_streamData, CompressionMode.Decompress, true);
    MemoryStream memoryStream = new MemoryStream();
    if (this.m_lOriginalSize > 0L)
      memoryStream.Capacity = (int) this.m_lOriginalSize;
    byte[] buffer = new byte[4096 /*0x1000*/];
    bool flag = false;
    int count;
    while ((count = deflateStream.Read(buffer, 0, 4096 /*0x1000*/)) > 0)
    {
      flag = true;
      memoryStream.Write(buffer, 0, count);
    }
    deflateStream.Dispose();
    if (!flag)
    {
      this.m_streamData.Position = 0L;
      this.DecompressDataOld();
    }
    else
    {
      if (this.m_bControlStream)
        this.m_streamData.Close();
      if (this.m_lOriginalSize < 0L)
        this.m_lOriginalSize = memoryStream.Length;
      this.m_bControlStream = true;
      this.m_streamData = (Stream) memoryStream;
      memoryStream.SetLength(this.m_lOriginalSize);
      memoryStream.Capacity = (int) this.m_lOriginalSize;
      if (this.m_bCheckCrc)
        this.CheckCrc(memoryStream.GetBuffer());
      this.m_streamData.Position = 0L;
    }
  }

  private void WriteHeader(Stream outputStream)
  {
    this.m_iLocalHeaderOffset = (int) outputStream.Position;
    outputStream.Write(BitConverter.GetBytes(67324752), 0, 4);
    if (this.m_archive.Password != null)
      outputStream.Write(BitConverter.GetBytes((short) 51), 0, 2);
    else
      outputStream.Write(BitConverter.GetBytes((short) 20), 0, 2);
    if (!this.IsIBM437Encoding(this.ItemName))
      this.m_options |= GeneralPurposeBitFlags.Unicode;
    if (this.m_archive.Password != null)
    {
      this.m_options = (GeneralPurposeBitFlags) 1;
      switch (this.m_archive.EncryptionAlgorithm)
      {
        case EncryptionAlgorithm.AES128:
        case EncryptionAlgorithm.AES192:
        case EncryptionAlgorithm.AES256:
          if (this.m_streamData != null)
          {
            this.m_compressionMethod = CompressionMethod.PPMd | CompressionMethod.Shrunk;
            this.m_uiCrc32 = 0U;
            break;
          }
          break;
      }
    }
    outputStream.Write(BitConverter.GetBytes((short) this.m_options), 0, 2);
    outputStream.Write(BitConverter.GetBytes((short) this.m_compressionMethod), 0, 2);
    int num = !this.LastModified.HasValue ? this.ConvertDateTime(DateTime.Now) : this.ConvertDateTime(this.LastModified.Value);
    byte[] buffer = new byte[4]
    {
      (byte) (num & (int) byte.MaxValue),
      (byte) ((num & 65280) >> 8),
      (byte) ((num & 16711680 /*0xFF0000*/) >> 16 /*0x10*/),
      (byte) (((long) num & 4278190080L /*0xFF000000*/) >> 24)
    };
    outputStream.Write(buffer, 0, 4);
    this.m_lCrcPosition = outputStream.Position;
    outputStream.Write(BitConverter.GetBytes(this.m_uiCrc32), 0, 4);
    outputStream.Write(BitConverter.GetBytes((int) this.m_lCompressedSize), 0, 4);
    outputStream.Write(BitConverter.GetBytes((int) this.m_lOriginalSize), 0, 4);
    Encoding encoding = (this.m_options & GeneralPurposeBitFlags.Unicode) != (GeneralPurposeBitFlags) 0 ? Encoding.UTF8 : Encoding.GetEncoding(ZipArchiveItem.OemCodePage);
    if (this.m_options != GeneralPurposeBitFlags.Unicode && this.CheckForLatin(this.m_strItemName))
      encoding = Encoding.GetEncoding(ZipArchiveItem.LatinOemCode);
    int byteCount = encoding.GetByteCount(this.m_strItemName);
    outputStream.Write(BitConverter.GetBytes((short) byteCount), 0, 2);
    if (this.m_compressionMethod == (CompressionMethod.PPMd | CompressionMethod.Shrunk) && this.m_archive.Password != null)
      outputStream.WriteByte((byte) 11);
    else
      outputStream.WriteByte((byte) 0);
    outputStream.WriteByte((byte) 0);
    byte[] bytes = encoding.GetBytes(this.m_strItemName);
    outputStream.Write(bytes, 0, bytes.Length);
    if (this.m_actualCompression != null && !this.m_bCompressed && this.m_compressionMethod == (CompressionMethod.PPMd | CompressionMethod.Shrunk))
      this.m_actualCompression = BitConverter.GetBytes((short) 8);
    if (this.m_compressionMethod != (CompressionMethod.PPMd | CompressionMethod.Shrunk) || this.m_archive.Password == null)
      return;
    this.WriteEncryptionHeader(outputStream);
  }

  private void WriteZippedContent(Stream outputStream)
  {
    long length = this.m_streamData != null ? this.m_streamData.Length : 0L;
    if (length <= 0L)
      return;
    long position = outputStream.Position;
    if (this.m_bCompressed || this.m_compressionMethod == CompressionMethod.Stored)
    {
      this.m_streamData.Position = 0L;
      byte[] buffer = new byte[4096 /*0x1000*/];
      while (length > 0L)
      {
        int num = this.m_streamData.Read(buffer, 0, 4096 /*0x1000*/);
        outputStream.Write(buffer, 0, num);
        length -= (long) num;
        if (this.m_compressionMethod == CompressionMethod.Stored && this.m_uiCrc32 == 0U)
          this.m_uiCrc32 = ZipCrc32.ComputeCrc(buffer, 0, num, this.m_uiCrc32);
      }
    }
    else if (this.m_compressionMethod == CompressionMethod.Deflated || this.m_compressionMethod == (CompressionMethod.PPMd | CompressionMethod.Shrunk))
    {
      this.m_lOriginalSize = length;
      this.m_streamData.Position = 0L;
      this.m_uiCrc32 = 0U;
      byte[] buffer = new byte[4096 /*0x1000*/];
      Stream stream = this.m_archive.CreateCompressor(outputStream);
      while (length > 0L)
      {
        int num = this.m_streamData.Read(buffer, 0, 4096 /*0x1000*/);
        stream.Write(buffer, 0, num);
        length -= (long) num;
        this.m_uiCrc32 = ZipCrc32.ComputeCrc(buffer, 0, num, this.m_uiCrc32);
      }
      stream.Flush();
      stream.Close();
    }
    this.m_lCompressedSize = outputStream.Position - position;
  }

  private void WriteFooter(Stream outputStream)
  {
    long num = outputStream != null ? outputStream.Position : throw new ArgumentNullException(nameof (outputStream));
    outputStream.Position = this.m_lCrcPosition;
    outputStream.Write(BitConverter.GetBytes(this.m_uiCrc32), 0, 4);
    outputStream.Write(BitConverter.GetBytes((int) this.m_lCompressedSize), 0, 4);
    outputStream.Write(BitConverter.GetBytes((int) this.m_lOriginalSize), 0, 4);
    outputStream.Position = num;
  }

  private void CheckCrc()
  {
    this.m_streamData.Position = 0L;
    if ((int) ZipCrc32.ComputeCrc(this.m_streamData, (int) this.m_lOriginalSize) != (int) this.m_uiCrc32)
      throw new ZipException("Wrong Crc value.");
  }

  private void CheckCrc(byte[] arrData)
  {
    if ((int) ZipCrc32.ComputeCrc(arrData, 0, (int) this.m_lOriginalSize, 0U) != (int) this.m_uiCrc32)
      throw new ZipException("Wrong Crc value.");
  }

  internal ZipArchiveItem Clone()
  {
    ZipArchiveItem zipArchiveItem = (ZipArchiveItem) this.MemberwiseClone();
    zipArchiveItem.m_streamData = ZipArchiveItem.CloneStream(this.m_streamData);
    return zipArchiveItem;
  }

  public static Stream CloneStream(Stream stream)
  {
    if (stream == null)
      return (Stream) null;
    long position = stream.Position;
    MemoryStream memoryStream = new MemoryStream((int) stream.Length);
    stream.Position = 0L;
    byte[] buffer = new byte[32768 /*0x8000*/];
    int count;
    while ((count = stream.Read(buffer, 0, 32768 /*0x8000*/)) != 0)
      memoryStream.Write(buffer, 0, count);
    stream.Position = position;
    memoryStream.Position = position;
    return (Stream) memoryStream;
  }

  private bool CheckForLatin(string text)
  {
    char[] charArray = text.ToCharArray();
    for (int index = 0; index < charArray.Length; ++index)
    {
      if (charArray[index] >= char.MinValue && charArray[index] <= '\u007F' || charArray[index] >= '\u0080' && charArray[index] <= 'ÿ' || charArray[index] >= 'Ā' && charArray[index] <= 'ſ' || charArray[index] >= 'ƀ' && charArray[index] <= 'ɏ' || charArray[index] >= 'Ḁ' && charArray[index] <= 'ỿ' || charArray[index] >= 'Ⱡ' && charArray[index] <= 'Ɀ' || charArray[index] >= '꜠' && charArray[index] <= 'ꟿ')
        return true;
    }
    return false;
  }

  public void Dispose()
  {
    if (this.m_strItemName != null)
    {
      this.Close();
      this.m_strItemName = (string) null;
    }
    if (this.m_actualCompression != null)
      this.m_actualCompression = (byte[]) null;
    GC.SuppressFinalize((object) this);
  }

  ~ZipArchiveItem() => this.Dispose();

  internal void WriteEncryptionHeader(Stream stream)
  {
    byte[] encryptionHeader = SecurityConstants.AesEncryptionHeader;
    stream.Write(encryptionHeader, 0, encryptionHeader.Length);
    stream.WriteByte((byte) this.m_archive.EncryptionAlgorithm);
    if (this.m_actualCompression == null)
      this.m_actualCompression = BitConverter.GetBytes((short) 8);
    stream.Write(this.m_actualCompression, 0, this.m_actualCompression.Length);
  }

  internal byte[] Encrypt(byte[] plainData)
  {
    return this.m_archive.EncryptionAlgorithm == EncryptionAlgorithm.ZipCrypto ? new ZipCrypto(this.m_streamData, this.m_archive.Password, this.m_uiCrc32).Encrypt(plainData) : new Aes(this.m_archive.EncryptionAlgorithm, this.m_archive.Password).Encrypt(plainData);
  }

  internal byte[] Decrypt(byte[] cipherData)
  {
    if (this.m_compressionMethod != (CompressionMethod.PPMd | CompressionMethod.Shrunk))
      return new ZipCrypto(this.m_archive.Password, this.m_uiCrc32).Decrypt(cipherData);
    byte[] numArray = new Aes(this.m_archive.EncryptionAlgorithm, this.m_archive.Password).Decrypt(cipherData);
    this.m_compressionMethod = (CompressionMethod) BitConverter.ToInt16(this.m_actualCompression, 0);
    return numArray;
  }

  private bool IsIBM437Encoding(string fileName)
  {
    if (fileName == null || fileName == string.Empty)
      throw new ArgumentException(nameof (fileName));
    Encoding encoding = Encoding.GetEncoding(437);
    byte[] bytes = encoding.GetBytes(fileName.ToCharArray());
    return encoding.GetString(bytes, 0, bytes.Length) == fileName;
  }

  internal static byte[] CreateRandom(int length)
  {
    byte[] random1 = length > 0 ? new byte[length] : throw new ArgumentOutOfRangeException(nameof (length));
    Random random2 = new Random((int) DateTime.Now.Ticks);
    int maxValue = 256 /*0x0100*/;
    for (int index = 0; index < length; ++index)
      random1[index] = (byte) random2.Next(maxValue);
    return random1;
  }
}
