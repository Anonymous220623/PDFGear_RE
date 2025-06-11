// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.XlsIO.Native.CompoundFile
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.CompoundFile.XlsIO.Native;

public class CompoundFile : ICompoundFile, IDisposable
{
  private Storage m_rootStorage;
  private ILockBytes m_lockBytes;

  public CompoundFile() => this.CreateStorageOnILockBytes();

  public CompoundFile(Stream stream) => this.Open(stream);

  public CompoundFile(string fileName, STGM options)
  {
    if (fileName == null || fileName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (fileName));
    if ((options & STGM.STGM_CREATE) == STGM.STGM_READ)
    {
      using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
        this.Open((Stream) fileStream);
    }
    else
    {
      Guid riid = new Guid("0000000b-0000-0000-C000-000000000046");
      IStorage storage;
      int storageEx = API.StgCreateStorageEx(fileName, options, STGFMT.STGFMT_DOCFILE, 0, IntPtr.Zero, IntPtr.Zero, ref riid, out storage);
      switch (storageEx)
      {
        case -2147287008 /*0x80030020*/:
        case -2147287007:
          throw new LockShareViolationException();
        case 0:
          this.m_rootStorage = new Storage(storage);
          break;
        default:
          throw new ExternalException("Cannot open storage. File Name is: " + fileName, storageEx);
      }
    }
  }

  public void Flush()
  {
    this.m_rootStorage.Flush();
    if (this.m_lockBytes == null)
      return;
    this.m_lockBytes.Flush();
  }

  private void CreateStorageOnILockBytes()
  {
    int ilockBytesOnHglobal = API.CreateILockBytesOnHGlobal(IntPtr.Zero, true, out this.m_lockBytes);
    if (ilockBytesOnHglobal != 0)
      throw new ExternalException("Can't create LockBytes.", ilockBytesOnHglobal);
    IStorage ppstgOpen;
    int docfileOnIlockBytes = API.StgCreateDocfileOnILockBytes(this.m_lockBytes, STGM.STGM_READWRITE | STGM.STGM_SHARE_EXCLUSIVE | STGM.STGM_CREATE, 0, out ppstgOpen);
    if (docfileOnIlockBytes != 0)
      throw new ExternalException("Can't create storage on ILockBytes.", docfileOnIlockBytes);
    this.m_rootStorage = new Storage(ppstgOpen);
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

  private void Open(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    API.CreateILockBytesOnHGlobal(IntPtr.Zero, true, out this.m_lockBytes);
    int count = (int) (stream.Length - stream.Position);
    byte[] numArray = new byte[count];
    stream.Read(numArray, 0, count);
    this.m_lockBytes.WriteAt(0UL, numArray, (uint) numArray.Length, out uint _);
    this.m_lockBytes.Flush();
    IStorage ppstgOpen;
    API.StgOpenStorageOnILockBytes(this.m_lockBytes, (IStorage) null, STGM.STGM_SHARE_DENY_NONE | STGM.STGM_DIRECT_SWMR, 0, 0, out ppstgOpen);
    this.m_rootStorage = new Storage(ppstgOpen);
  }

  public ICompoundStorage RootStorage => (ICompoundStorage) this.m_rootStorage;

  public void Save(Stream stream)
  {
    this.Flush();
    if (this.m_lockBytes == null)
      throw new Exception("The method or operation is not implemented.");
    this.SaveILockBytesIntoStream(stream);
  }

  public void Save(string fileName)
  {
    using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
      this.Save((Stream) fileStream);
  }

  public void Dispose()
  {
    if (this.m_rootStorage == null)
      return;
    this.m_rootStorage.Dispose();
    this.m_rootStorage = (Storage) null;
    if (this.m_lockBytes != null)
    {
      Marshal.FinalReleaseComObject((object) this.m_lockBytes);
      GC.SuppressFinalize((object) this.m_lockBytes);
      this.m_lockBytes = (ILockBytes) null;
    }
    GC.SuppressFinalize((object) this);
  }
}
