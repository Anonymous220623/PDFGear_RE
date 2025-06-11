// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.XlsIO.Net.DirectoryEntry
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.CompoundFile.XlsIO.Net;

public class DirectoryEntry
{
  public const int SizeInFile = 128 /*0x80*/;
  private const int StreamNameSize = 64 /*0x40*/;
  private string m_strName;
  private DirectoryEntry.EntryType m_entryType;
  private byte m_color = 1;
  private int m_leftId = -1;
  private int m_rightId = -1;
  private int m_childId = -1;
  private Guid m_storageGuid;
  private int m_iStorageFlags;
  private DateTime m_dateCreate;
  private DateTime m_dateModify;
  private int m_iStartSector = -2;
  private uint m_uiSize;
  private int m_iReserved;
  private int m_iEntryId;
  public int LastSector = -1;
  public int LastOffset = -1;

  public string Name
  {
    get => this.m_strName;
    set => this.m_strName = value;
  }

  public DirectoryEntry.EntryType Type
  {
    get => this.m_entryType;
    set => this.m_entryType = value;
  }

  public byte Color
  {
    get => this.m_color;
    set => this.m_color = value;
  }

  public int LeftId
  {
    get => this.m_leftId;
    set => this.m_leftId = value;
  }

  public int RightId
  {
    get => this.m_rightId;
    set => this.m_rightId = value;
  }

  public int ChildId
  {
    get => this.m_childId;
    set => this.m_childId = value;
  }

  public Guid StorageGuid
  {
    get => this.m_storageGuid;
    set => this.m_storageGuid = value;
  }

  public int StorageFlags
  {
    get => this.m_iStorageFlags;
    set => this.m_iStorageFlags = value;
  }

  public DateTime DateCreate
  {
    get => this.m_dateCreate;
    set => this.m_dateCreate = value;
  }

  public DateTime DateModify
  {
    get => this.m_dateModify;
    set => this.m_dateModify = value;
  }

  public int StartSector
  {
    get => this.m_iStartSector;
    set => this.m_iStartSector = value;
  }

  public uint Size
  {
    get => this.m_uiSize;
    set => this.m_uiSize = value;
  }

  public int Reserved
  {
    get => this.m_iReserved;
    set
    {
    }
  }

  public int EntryId
  {
    get => this.m_iEntryId;
    internal set => this.m_iEntryId = value;
  }

  public DirectoryEntry(string name, DirectoryEntry.EntryType type, int entryId)
  {
    this.m_strName = name;
    this.m_entryType = type;
    this.m_iEntryId = entryId;
    this.m_dateModify = this.m_dateCreate = DateTime.Now;
  }

  public DirectoryEntry(byte[] data, int offset, int entryId)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    if (offset < 0 || offset >= data.Length)
      throw new ArgumentOutOfRangeException(nameof (offset));
    this.m_iEntryId = entryId;
    int uint16 = (int) BitConverter.ToUInt16(data, offset + 64 /*0x40*/);
    this.m_strName = Encoding.Unicode.GetString(data, offset, uint16);
    int length = this.m_strName.Length;
    if (length > 0 && this.m_strName[length - 1] == char.MinValue)
      this.m_strName = this.m_strName.Substring(0, length - 1);
    offset += 66;
    this.m_entryType = (DirectoryEntry.EntryType) data[offset];
    ++offset;
    this.m_color = data[offset];
    ++offset;
    this.m_leftId = BitConverter.ToInt32(data, offset);
    offset += 4;
    this.m_rightId = BitConverter.ToInt32(data, offset);
    offset += 4;
    this.m_childId = BitConverter.ToInt32(data, offset);
    offset += 4;
    byte[] numArray = new byte[16 /*0x10*/];
    Buffer.BlockCopy((Array) data, offset, (Array) numArray, 0, 16 /*0x10*/);
    this.m_storageGuid = new Guid(numArray);
    offset += 16 /*0x10*/;
    this.m_iStorageFlags = BitConverter.ToInt32(data, offset);
    offset += 4;
    long int64_1 = BitConverter.ToInt64(data, offset);
    long fileTime = DateTime.MaxValue.ToFileTime();
    if (int64_1 >= 0L && int64_1 <= fileTime)
      this.m_dateCreate = DateTime.FromFileTime(int64_1);
    offset += 8;
    long int64_2 = BitConverter.ToInt64(data, offset);
    if (int64_2 >= 0L && int64_2 <= fileTime)
      this.m_dateModify = DateTime.FromFileTime(int64_2);
    offset += 8;
    this.m_iStartSector = BitConverter.ToInt32(data, offset);
    offset += 4;
    this.m_uiSize = BitConverter.ToUInt32(data, offset);
    offset += 4;
    this.m_iReserved = BitConverter.ToInt32(data, offset);
    offset += 4;
  }

  public void Write(Stream stream)
  {
    long num = stream != null ? stream.Position : throw new ArgumentNullException(nameof (stream));
    if (this.m_entryType == DirectoryEntry.EntryType.Invalid)
      this.m_leftId = this.m_rightId = this.m_childId = -1;
    byte[] bytes1 = Encoding.Unicode.GetBytes(this.m_strName);
    stream.Write(bytes1, 0, bytes1.Length);
    stream.WriteByte((byte) 0);
    stream.WriteByte((byte) 0);
    stream.Position = num + 64L /*0x40*/;
    byte[] bytes2 = BitConverter.GetBytes((short) (bytes1.Length + 2));
    stream.Write(bytes2, 0, 2);
    stream.WriteByte((byte) this.m_entryType);
    stream.WriteByte(this.m_color);
    byte[] bytes3 = BitConverter.GetBytes(this.m_leftId);
    stream.Write(bytes3, 0, 4);
    byte[] bytes4 = BitConverter.GetBytes(this.m_rightId);
    stream.Write(bytes4, 0, 4);
    byte[] bytes5 = BitConverter.GetBytes(this.m_childId);
    stream.Write(bytes5, 0, 4);
    if (this.Type == DirectoryEntry.EntryType.Root && this.m_storageGuid.CompareTo(Guid.Empty) == 0)
      this.m_storageGuid = new Guid("00020820-0000-0000-c000-000000000046");
    byte[] byteArray = this.m_storageGuid.ToByteArray();
    stream.Write(byteArray, 0, byteArray.Length);
    byte[] bytes6 = BitConverter.GetBytes(this.m_iStorageFlags);
    stream.Write(bytes6, 0, 4);
    byte[] bytes7 = BitConverter.GetBytes(0L);
    stream.Write(bytes7, 0, bytes7.Length);
    byte[] bytes8 = BitConverter.GetBytes(0L);
    stream.Write(bytes8, 0, bytes8.Length);
    byte[] bytes9 = BitConverter.GetBytes(this.m_iStartSector);
    stream.Write(bytes9, 0, 4);
    byte[] bytes10 = BitConverter.GetBytes(this.m_uiSize);
    stream.Write(bytes10, 0, 4);
    byte[] bytes11 = BitConverter.GetBytes(this.m_iReserved);
    stream.Write(bytes11, 0, 4);
  }

  public enum EntryType
  {
    Invalid,
    Storage,
    Stream,
    LockBytes,
    Property,
    Root,
  }
}
