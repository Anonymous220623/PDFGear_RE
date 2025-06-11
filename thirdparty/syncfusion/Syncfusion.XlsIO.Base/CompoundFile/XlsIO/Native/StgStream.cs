// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.XlsIO.Native.StgStream
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.CompoundFile.XlsIO.Native;

public class StgStream : Stream, IDisposable
{
  public const STGM DEF_STORE_READONLY = STGM.STGM_SHARE_DENY_WRITE;
  public const STGM DEF_STREAM_READONLY = STGM.STGM_SHARE_EXCLUSIVE;
  public const STGM DEF_STREAM_CREATE = STGM.STGM_READWRITE | STGM.STGM_SHARE_EXCLUSIVE | STGM.STGM_CREATE;
  public const STGM DEF_READWRITE = STGM.STGM_READWRITE | STGM.STGM_SHARE_EXCLUSIVE;
  private const int DEF_BUFFER_SIZE = 32768 /*0x8000*/;
  public const STGM DEF_STORAGE_READONLY = STGM.STGM_SHARE_DENY_NONE | STGM.STGM_DIRECT_SWMR;
  private IStream m_stream;
  private IStorage m_storage;
  private bool m_bIsDisposed;
  private bool m_bCanRead;
  private bool m_bCanWrite;
  private bool m_bCanSeek;
  private bool m_bIsTransacted;
  private long m_lLength = -1;
  private List<string> m_arrStreams = new List<string>();
  private List<string> m_arrStorages = new List<string>();
  private string m_strFileName;
  private string m_strStreamName;
  private string m_strStorageName;
  private STGM m_modeStorage;
  private STGM m_modeStream;
  private long m_lPosition;
  private ILockBytes m_lockBytes;

  public override bool CanRead => this.m_bCanRead;

  public override bool CanSeek => this.m_bCanSeek;

  public override bool CanWrite => this.m_bCanWrite;

  public bool IsTransacted => this.m_bIsTransacted;

  public override long Length => this.m_lLength;

  public override long Position
  {
    get => this.m_lPosition;
    set => this.Seek(value, SeekOrigin.Begin);
  }

  public string[] Streams => this.m_arrStreams.ToArray();

  public string[] Storages => this.m_arrStorages.ToArray();

  [CLSCompliant(false)]
  public IStorage COMStorage => this.m_storage;

  [CLSCompliant(false)]
  public IStream COMStream => this.m_stream;

  public string StreamName => this.m_strStreamName;

  public string StorageName => this.m_strStorageName;

  [CLSCompliant(false)]
  public ILockBytes LockBytes => this.m_lockBytes;

  public string FileName => this.m_strFileName;

  public override void Close()
  {
    base.Close();
    if (this.m_stream != null)
    {
      this.m_stream.Commit(0U);
      Marshal.FinalReleaseComObject((object) this.m_stream);
      GC.SuppressFinalize((object) this.m_stream);
    }
    this.m_stream = (IStream) null;
    this.m_strStreamName = (string) null;
  }

  public override void Flush()
  {
    this.Commit(STGC.STGC_DEFAULT);
    if (this.m_lockBytes == null)
      return;
    this.m_lockBytes.Flush();
  }

  public virtual void Commit(STGC code)
  {
    if ((this.m_modeStream & STGM.STGM_TRANSACTED) == STGM.STGM_TRANSACTED)
    {
      this.CheckStream();
      if (this.m_stream.Commit((uint) code) != 0)
        throw new ExternalException("Commit Operation failed");
    }
    this.m_storage.Commit((uint) code);
    this.m_storage.Commit((uint) code);
  }

  public virtual void Revert()
  {
    if ((this.m_modeStream & STGM.STGM_TRANSACTED) != STGM.STGM_TRANSACTED)
      return;
    this.CheckStream();
    int errorCode = this.m_stream.Revert();
    if (errorCode != 0)
      throw new ExternalException("Revert Operation failed", errorCode);
  }

  public override long Seek(long offset, SeekOrigin origin)
  {
    this.CheckStream();
    long plibNewPosition;
    int errorCode = this.m_stream.Seek(offset, origin, out plibNewPosition);
    if (errorCode != 0)
      throw new ExternalException("Seek Operation failed.", errorCode);
    this.m_lPosition = plibNewPosition;
    return this.m_lPosition;
  }

  public override void SetLength(long value)
  {
    this.CheckStream();
    int errorCode = this.m_stream.SetSize((ulong) value);
    if (errorCode != 0)
      throw new ExternalException("SetLength Operation failed", errorCode);
    this.m_lLength = value;
  }

  public override int Read(byte[] buffer, int offset, int count)
  {
    if (buffer == null)
      throw new ArgumentNullException(nameof (buffer));
    if (offset < 0)
      throw new ArgumentOutOfRangeException(nameof (offset));
    if (count < 0)
      throw new ArgumentOutOfRangeException(nameof (count));
    if (buffer.Length - offset < count)
      throw new ArgumentException("Invalid offest or count values.");
    this.CheckStream();
    if (this.m_lLength == this.m_lPosition)
      return 0;
    uint pcbRead = 0;
    uint cb = (uint) count;
    byte[] numArray = offset > 0 ? new byte[count] : buffer;
    int errorCode = this.m_stream.Read(numArray, cb, ref pcbRead);
    if (errorCode != 0)
      throw new ExternalException("Read Operation failed", errorCode);
    if (offset > 0)
      Buffer.BlockCopy((Array) numArray, 0, (Array) buffer, offset, (int) pcbRead);
    this.m_lPosition += (long) pcbRead;
    return (int) pcbRead;
  }

  public override void Write(byte[] buffer, int offset, int count)
  {
    if (!this.CanWrite)
      throw new ArgumentException("Stream in ReadOnly mode. Wrong Operation.");
    if (buffer == null)
      throw new ArgumentNullException(nameof (buffer));
    if (offset < 0)
      throw new ArgumentOutOfRangeException(nameof (offset));
    if (count < 0)
      throw new ArgumentOutOfRangeException(nameof (count));
    if (buffer.Length - offset < count)
      throw new ArgumentException("Invalid offest or count values.");
    this.CheckStream();
    uint cb = (uint) count;
    uint pcbWritten = 0;
    byte[] numArray = offset > 0 ? new byte[count] : buffer;
    if (offset > 0)
      Array.Copy((Array) buffer, offset, (Array) numArray, 0, count);
    int errorCode = this.m_stream.Write(numArray, cb, ref pcbWritten);
    if (errorCode != 0)
      throw new ExternalException("Write Operation failed", errorCode);
    this.m_lPosition += (long) pcbWritten;
    if (this.m_lLength >= this.m_lPosition)
      return;
    this.m_lLength = this.m_lPosition;
  }

  private StgStream()
  {
  }

  public StgStream(string fileName, STGM flags)
  {
    if (fileName == null)
      throw new ArgumentNullException(nameof (fileName));
    int errorCode = API.StgOpenStorage(fileName, IntPtr.Zero, flags, IntPtr.Zero, 0U, out this.m_storage);
    switch (errorCode)
    {
      case -2147287008 /*0x80030020*/:
      case -2147287007:
        throw new LockShareViolationException();
      case 0:
        this.CalculateSubItemsNames();
        this.m_strFileName = fileName;
        this.m_modeStorage = flags;
        break;
      default:
        throw new ExternalException("Cannot open storage. File Name is: " + fileName, errorCode);
    }
  }

  public StgStream(string fileName, STGM storeFlags, string streamName, STGM streamFlags)
    : this(fileName, storeFlags)
  {
    this.OpenStream(streamName, streamFlags);
  }

  public StgStream(string fileName, string streamName)
    : this(fileName, STGM.STGM_SHARE_DENY_WRITE, streamName, STGM.STGM_SHARE_EXCLUSIVE)
  {
  }

  public StgStream(string fileName)
    : this(fileName, STGM.STGM_SHARE_DENY_WRITE)
  {
  }

  public StgStream(StgStream storage, string streamName)
    : this(storage, streamName, STGM.STGM_SHARE_EXCLUSIVE)
  {
  }

  public StgStream(StgStream storage, string streamName, STGM streamFlags)
    : this(storage, streamName, STGM.STGM_SHARE_EXCLUSIVE, false)
  {
  }

  public StgStream(StgStream storage, string streamName, bool bCreate)
    : this(storage, streamName, bCreate ? STGM.STGM_READWRITE | STGM.STGM_SHARE_EXCLUSIVE | STGM.STGM_CREATE : STGM.STGM_SHARE_EXCLUSIVE, bCreate)
  {
  }

  public StgStream(StgStream storage, string streamName, STGM streamFlags, bool bCreate)
  {
    if (storage == null)
      throw new ArgumentNullException(nameof (storage));
    if (storage.m_storage == null)
      throw new ArgumentException("input storage must be opened");
    if (streamName == null)
      throw new ArgumentNullException(nameof (streamName));
    IntPtr iunknownForObject = Marshal.GetIUnknownForObject((object) storage.m_storage);
    this.m_storage = (IStorage) Marshal.GetObjectForIUnknown(iunknownForObject);
    Marshal.Release(iunknownForObject);
    this.m_modeStorage = storage.m_modeStorage;
    this.m_arrStorages.AddRange((IEnumerable<string>) storage.m_arrStorages);
    this.m_arrStreams.AddRange((IEnumerable<string>) storage.m_arrStreams);
    if (bCreate)
      this.CreateStream(streamName, streamFlags);
    else
      this.OpenStream(streamName, streamFlags);
  }

  public StgStream(Stream stream, STGM flags)
  {
    if (stream is StgStream)
      throw new ArgumentException("It is StgStream already.");
    int ilockBytesOnHglobal = API.CreateILockBytesOnHGlobal(IntPtr.Zero, true, out this.m_lockBytes);
    if (ilockBytesOnHglobal != 0)
      throw new ExternalException("Can't create LockBytes.", ilockBytesOnHglobal);
    int count = (int) (stream.Length - stream.Position);
    byte[] numArray = new byte[count];
    stream.Read(numArray, 0, count);
    int errorCode1 = this.m_lockBytes.WriteAt(0UL, numArray, (uint) numArray.Length, out uint _);
    if (errorCode1 != 0)
      throw new ExternalException("Can't write LockBytes.", errorCode1);
    int errorCode2 = this.m_lockBytes.Flush();
    if (errorCode2 != 0)
      throw new ExternalException("Can't flush LockBytes.", errorCode2);
    int errorCode3 = API.StgOpenStorageOnILockBytes(this.m_lockBytes, (IStorage) null, flags, 0, 0, out this.m_storage);
    if (errorCode3 != 0)
      throw new ExternalException("Can't open storage on LockBytes.", errorCode3);
    this.CalculateSubItemsNames();
    this.m_modeStorage = flags;
  }

  public StgStream(Stream stream)
    : this(stream, STGM.STGM_SHARE_EXCLUSIVE)
  {
  }

  public new void Dispose()
  {
    if (this.m_bIsDisposed)
      return;
    this.m_bIsDisposed = true;
    this.Close();
    Marshal.FinalReleaseComObject((object) this.m_storage);
    GC.SuppressFinalize((object) this.m_storage);
    if (this.m_lockBytes != null)
    {
      Marshal.FinalReleaseComObject((object) this.m_lockBytes);
      GC.SuppressFinalize((object) this.m_lockBytes);
      this.m_lockBytes = (ILockBytes) null;
    }
    this.m_storage = (IStorage) null;
    this.m_strStorageName = (string) null;
    this.m_bIsDisposed = true;
    this.m_arrStorages = (List<string>) null;
    this.m_arrStreams = (List<string>) null;
  }

  public void OpenStream(string streamName)
  {
    this.OpenStream(streamName, STGM.STGM_SHARE_EXCLUSIVE);
  }

  public void OpenStream(string streamName, STGM streamFlags)
  {
    this.CheckStorage();
    if (streamName == null)
      throw new ArgumentNullException(nameof (streamName));
    bool flag = false;
    for (int index = 0; index < this.m_arrStreams.Count; ++index)
    {
      string arrStream = this.m_arrStreams[index];
      if (string.Compare(arrStream, streamName, true) == 0)
      {
        flag = true;
        streamName = arrStream;
        break;
      }
    }
    if (!flag)
      throw new ArgumentException("In storage cannot be found stream with specified name: " + streamName);
    if (this.m_stream != null)
      this.Close();
    int errorCode = this.m_storage.OpenStream(streamName, 0U, streamFlags, 0U, out this.m_stream);
    if (errorCode != 0)
      throw new ExternalException("Cannot open stream.", errorCode);
    this.m_lLength = this.CalculateStreamLength();
    this.m_lPosition = 0L;
    this.m_bCanWrite = (streamFlags & (STGM.STGM_WRITE | STGM.STGM_READWRITE)) > STGM.STGM_READ;
    this.m_bCanSeek = true;
    this.m_bCanRead = true;
    this.m_bIsTransacted = (streamFlags & STGM.STGM_TRANSACTED) > STGM.STGM_READ;
    this.m_strStreamName = streamName;
    this.m_modeStream = streamFlags;
  }

  public StgStream OpenSubStorage(string storageName)
  {
    return this.OpenSubStorage(storageName, STGM.STGM_SHARE_EXCLUSIVE);
  }

  public StgStream OpenSubStorage(string storageName, STGM flags)
  {
    if (storageName == null)
      throw new ArgumentNullException(nameof (storageName));
    this.CheckStorage();
    IStorage ppstg;
    int errorCode = this.m_storage.OpenStorage(storageName, IntPtr.Zero, flags, IntPtr.Zero, 0U, out ppstg);
    if (errorCode != 0)
      throw new ExternalException("Cannot open sub storage. sub storage name is: " + storageName, errorCode);
    StgStream stgStream = new StgStream();
    stgStream.m_storage = ppstg;
    stgStream.m_strStorageName = storageName;
    stgStream.CalculateSubItemsNames();
    stgStream.m_modeStorage = flags;
    return stgStream;
  }

  public StgStream CreateSubStorage(string storageName)
  {
    return this.CreateSubStorage(storageName, STGM.STGM_READWRITE | STGM.STGM_SHARE_EXCLUSIVE | STGM.STGM_CREATE);
  }

  public StgStream CreateSubStorage(string storageName, STGM flags)
  {
    if (storageName == null)
      throw new ArgumentNullException(nameof (storageName));
    this.CheckStorage();
    IStorage ppstg;
    int storage = this.m_storage.CreateStorage(storageName, flags, 0U, 0U, out ppstg);
    if (storage != 0)
      throw new ExternalException("Cannot open sub storage. sub storage name is: " + storageName, storage);
    StgStream subStorage = new StgStream();
    subStorage.m_storage = ppstg;
    subStorage.m_strStorageName = storageName;
    ppstg = (IStorage) null;
    subStorage.CalculateSubItemsNames();
    subStorage.m_modeStorage = flags;
    this.m_arrStorages.Add(storageName);
    return subStorage;
  }

  public void CreateStream(string streamName)
  {
    this.CreateStream(streamName, STGM.STGM_READWRITE | STGM.STGM_SHARE_EXCLUSIVE | STGM.STGM_CREATE);
  }

  public void CreateStream(string streamName, STGM streamFlags)
  {
    int stream = this.m_storage.CreateStream(streamName, streamFlags, 0U, 0U, ref this.m_stream);
    if (stream != 0)
      throw new ExternalException("Cannot create stream.", stream);
    this.m_strStreamName = streamName;
    this.m_arrStreams.Add(streamName);
    this.m_lPosition = 0L;
    this.m_bCanWrite = (streamFlags & (STGM.STGM_WRITE | STGM.STGM_READWRITE)) > STGM.STGM_READ;
    this.m_bCanSeek = true;
    this.m_bCanRead = true;
    this.m_bIsTransacted = (streamFlags & STGM.STGM_TRANSACTED) > STGM.STGM_READ;
    this.CalculateSubItemsNames();
  }

  public void SaveILockBytesIntoStream(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (this.m_lockBytes == null)
      throw new ArgumentNullException("m_lockBytes");
    byte[] numArray = new byte[32768 /*0x8000*/];
    long ulOffset = 0;
    int errorCode;
    while (true)
    {
      uint pcbRead;
      errorCode = this.m_lockBytes.ReadAt((ulong) ulOffset, numArray, 32768U /*0x8000*/, out pcbRead);
      if (errorCode == 0)
      {
        stream.Write(numArray, 0, (int) pcbRead);
        if (pcbRead >= 32768U /*0x8000*/)
          ulOffset += 32768L /*0x8000*/;
        else
          goto label_9;
      }
      else
        break;
    }
    throw new ExternalException("Unable to read bytes from ILockBytes", errorCode);
label_9:;
  }

  public string FindStream(string strStreamName)
  {
    switch (strStreamName)
    {
      case null:
        throw new ArgumentNullException(nameof (strStreamName));
      case "":
        throw new ArgumentException("strStreamName - string cannot be empty.");
      default:
        int index = 0;
        for (int count = this.m_arrStreams.Count; index < count; ++index)
        {
          string arrStream = this.m_arrStreams[index];
          if (string.Compare(arrStream, strStreamName, true) == 0)
            return arrStream;
        }
        return (string) null;
    }
  }

  public bool ContainsStream(string strStreamName)
  {
    if (strStreamName == null || strStreamName.Length == 0)
      return false;
    int index = 0;
    for (int count = this.m_arrStreams.Count; index < count; ++index)
    {
      if (this.m_arrStreams[index].Equals(strStreamName))
        return true;
    }
    return false;
  }

  public bool ContainsStorage(string strStorageName)
  {
    if (strStorageName == null || strStorageName.Length == 0)
      return false;
    int index = 0;
    for (int count = this.m_arrStorages.Count; index < count; ++index)
    {
      if (this.m_arrStorages[index].Equals(strStorageName))
        return true;
    }
    return false;
  }

  public int RemoveElement(string elementName) => this.m_storage.DestroyElement(elementName);

  public static void CopySourceStorages(StgStream source, StgStream destination)
  {
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    if (destination == null)
      throw new ArgumentNullException(nameof (destination));
    string storageName = source.StorageName;
    if (storageName == null)
      throw new ArgumentException("Source does not contain opened sub-storage");
    using (StgStream stgStream = !destination.ContainsStorage(storageName) ? destination.CreateSubStorage(storageName) : destination.OpenSubStorage(storageName))
    {
      source.COMStorage.CopyTo(0U, IntPtr.Zero, IntPtr.Zero, stgStream.COMStorage);
      destination.COMStorage.Commit(0U);
    }
  }

  public static StgStream CreateStorage(string fileName)
  {
    IStorage ppstgOpen;
    int docfile = API.StgCreateDocfile(fileName, STGM.STGM_READWRITE | STGM.STGM_SHARE_EXCLUSIVE | STGM.STGM_CREATE, 0U, out ppstgOpen);
    switch (docfile)
    {
      case -2147287008 /*0x80030020*/:
      case -2147287007:
        throw new LockShareViolationException();
      case 0:
        return new StgStream() { m_storage = ppstgOpen };
      default:
        throw new ExternalException("Cannot create compound file.", docfile);
    }
  }

  public static StgStream CreateStorageOnILockBytes()
  {
    ILockBytes ppLkbyt;
    int ilockBytesOnHglobal = API.CreateILockBytesOnHGlobal(IntPtr.Zero, true, out ppLkbyt);
    if (ilockBytesOnHglobal != 0)
      throw new ExternalException("Can't create LockBytes.", ilockBytesOnHglobal);
    IStorage ppstgOpen;
    int docfileOnIlockBytes = API.StgCreateDocfileOnILockBytes(ppLkbyt, STGM.STGM_READWRITE | STGM.STGM_SHARE_EXCLUSIVE | STGM.STGM_CREATE, 0, out ppstgOpen);
    if (docfileOnIlockBytes != 0)
      throw new ExternalException("Can't create storage on ILockBytes.", docfileOnIlockBytes);
    return new StgStream()
    {
      m_storage = ppstgOpen,
      m_lockBytes = ppLkbyt
    };
  }

  private void CheckStorage()
  {
    if (this.m_storage == null)
      throw new ArgumentNullException("storage", "Storage not initialized");
  }

  private void CheckStream()
  {
    if (this.m_stream == null)
      throw new ArgumentNullException("Stream", "Stg Stream not initialized");
  }

  private long CalculateStreamLength()
  {
    this.CheckStream();
    long offset = this.Seek(0L, SeekOrigin.Current);
    long streamLength = this.Seek(0L, SeekOrigin.End);
    this.Seek(offset, SeekOrigin.Begin);
    return streamLength;
  }

  private List<string> CalculateStorageStreams()
  {
    List<string> userData = new List<string>();
    this.CalculateSubItems(new StgStream.SubItemNameEventHandler(this.ByTypeAccumulate_Streams), (object) userData);
    return userData;
  }

  private List<string> CalculateStorageSubStorages()
  {
    List<string> userData = new List<string>();
    this.CalculateSubItems(new StgStream.SubItemNameEventHandler(this.ByTypeAccumulate_Storages), (object) userData);
    return userData;
  }

  private void CalculateSubItemsNames()
  {
    this.m_arrStorages.Clear();
    this.m_arrStreams.Clear();
    this.CalculateSubItems(new StgStream.SubItemNameEventHandler(this.ByTypeAccumulate_All), (object) null);
  }

  private void ByTypeAccumulate_Streams(STATSTG item, object userData)
  {
    if (item.type != STGTY.STGTY_STREAM)
      return;
    ((List<string>) userData).Add(item.pwcsName);
  }

  private void ByTypeAccumulate_Storages(STATSTG item, object userData)
  {
    if (item.type != STGTY.STGTY_STORAGE)
      return;
    ((List<string>) userData).Add(item.pwcsName);
  }

  private void ByTypeAccumulate_All(STATSTG item, object userData)
  {
    if (item.type == STGTY.STGTY_STREAM)
    {
      if (this.m_arrStreams == null)
        return;
      this.m_arrStreams.Add(item.pwcsName);
    }
    else
    {
      if (item.type != STGTY.STGTY_STORAGE || this.m_arrStorages == null)
        return;
      this.m_arrStorages.Add(item.pwcsName);
    }
  }

  private void CalculateSubItems(StgStream.SubItemNameEventHandler caller, object userData)
  {
    if (caller == null)
      throw new ArgumentNullException(nameof (caller));
    this.CheckStorage();
    IEnumSTATSTG ppenum = (IEnumSTATSTG) null;
    int errorCode1 = this.m_storage.EnumElements(0U, IntPtr.Zero, 0U, ref ppenum);
    if (errorCode1 != 0)
      throw new ExternalException("Stream Enumeration Operation failed", errorCode1);
    int errorCode2 = ppenum != null ? ppenum.Reset() : throw new SystemException("Cannot get IEnumSTATSTG interface refernce from storage");
    if (errorCode2 != 0)
      throw new ExternalException("Stream Enumeration Operation failed", errorCode2);
    STATSTG rgelt = new STATSTG();
    uint pceltFetched = 0;
    int errorCode3;
    for (errorCode3 = ppenum.Next(1U, ref rgelt, ref pceltFetched); errorCode3 == 0 && 1U == pceltFetched; errorCode3 = ppenum.Next(1U, ref rgelt, ref pceltFetched))
      caller(rgelt, userData);
    if (errorCode3 > 1 || errorCode3 < 0)
      throw new ExternalException("Stream Enumeration Operation failed", errorCode3);
    Marshal.FinalReleaseComObject((object) ppenum);
    ppenum = (IEnumSTATSTG) null;
  }

  internal delegate void SubItemNameEventHandler(STATSTG item, object userData);
}
