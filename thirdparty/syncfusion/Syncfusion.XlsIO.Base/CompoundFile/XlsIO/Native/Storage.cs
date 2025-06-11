// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.XlsIO.Native.Storage
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.CompoundFile.XlsIO.Native;

public class Storage : ICompoundStorage, IDisposable
{
  private const string DefaultStorageName = "_Root";
  private IStorage m_storage;
  private List<string> m_arrStorages = new List<string>();
  private List<string> m_arrStreams = new List<string>();
  private string m_strName;

  [CLSCompliant(false)]
  public IStorage COMStorage => this.m_storage;

  private void CalculateSubItemsNames()
  {
    this.m_arrStorages.Clear();
    this.m_arrStreams.Clear();
    this.CalculateSubItems(new Storage.SubItemNameEventHandler(this.ByTypeAccumulate_All), (object) null);
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

  private void CalculateSubItems(Storage.SubItemNameEventHandler caller, object userData)
  {
    if (caller == null)
      throw new ArgumentNullException(nameof (caller));
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

  public Storage(string fileName, STGM storageOptions) => throw new NotImplementedException();

  [CLSCompliant(false)]
  public Storage(IStorage root)
    : this(root, "_Root")
  {
  }

  [CLSCompliant(false)]
  public Storage(IStorage root, string storageName)
  {
    if (root == null)
      throw new ArgumentNullException(nameof (root));
    if (storageName != null && storageName.Length != 0)
      this.m_strName = storageName;
    this.m_storage = root;
    this.CalculateSubItemsNames();
  }

  ~Storage() => this.Dispose();

  public CompoundStream CreateStream(string streamName)
  {
    IStream ppstm = (IStream) null;
    this.m_storage.CreateStream(streamName, STGM.STGM_READWRITE | STGM.STGM_SHARE_EXCLUSIVE | STGM.STGM_CREATE, 0U, 0U, ref ppstm);
    this.m_arrStreams.Add(streamName);
    return (CompoundStream) new NativeStream(ppstm, streamName);
  }

  public CompoundStream OpenStream(string streamName)
  {
    IStream ppstm;
    this.m_storage.OpenStream(streamName, 0U, STGM.STGM_SHARE_EXCLUSIVE, 0U, out ppstm);
    return (CompoundStream) new NativeStream(ppstm, streamName);
  }

  public void DeleteStream(string streamName)
  {
    if (!this.ContainsStream(streamName))
      return;
    this.m_storage.DestroyElement(streamName);
    this.m_arrStreams.Remove(streamName);
  }

  public bool ContainsStream(string streamName) => this.m_arrStreams.Contains(streamName);

  public ICompoundStorage CreateStorage(string storageName)
  {
    IStorage ppstg;
    int errorCode = storageName != null && storageName.Length != 0 ? this.m_storage.CreateStorage(storageName, STGM.STGM_READWRITE | STGM.STGM_SHARE_EXCLUSIVE | STGM.STGM_CREATE, 0U, 0U, out ppstg) : throw new ArgumentOutOfRangeException(nameof (storageName));
    if (errorCode != 0)
      throw new ExternalException("Problems during storage creation", errorCode);
    this.m_arrStorages.Add(storageName);
    return (ICompoundStorage) new Storage(ppstg, storageName);
  }

  public ICompoundStorage OpenStorage(string storageName)
  {
    if (storageName == null || storageName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (storageName));
    IStorage ppstg;
    int errorCode = this.m_storage.OpenStorage(storageName, IntPtr.Zero, STGM.STGM_SHARE_EXCLUSIVE, IntPtr.Zero, 0U, out ppstg);
    if (errorCode != 0)
      throw new ExternalException("Problems during storage creation", errorCode);
    return (ICompoundStorage) new Storage(ppstg, storageName);
  }

  public void DeleteStorage(string storageName)
  {
    if (!this.ContainsStorage(storageName))
      return;
    this.m_storage.DestroyElement(storageName);
    this.m_arrStorages.Remove(storageName);
  }

  public bool ContainsStorage(string storageName) => this.m_arrStorages.Contains(storageName);

  public void Flush() => this.m_storage.Commit(0U);

  public string[] Streams => this.m_arrStreams.ToArray();

  public string[] Storages => this.m_arrStorages.ToArray();

  public string Name => this.m_strName;

  public void InsertCopy(ICompoundStorage storageToCopy)
  {
    string storageName = storageToCopy is Storage storage1 ? storageToCopy.Name : throw new NotImplementedException("Copying between different storage types is not implemented");
    if (this.ContainsStorage(storageName))
      this.DeleteStorage(storageName);
    using (Storage storage2 = this.CreateStorage(storageName) as Storage)
      storage1.m_storage.CopyTo(0U, IntPtr.Zero, IntPtr.Zero, storage2.m_storage);
  }

  public void InsertCopy(CompoundStream streamToCopy)
  {
    string name = streamToCopy.Name;
    using (CompoundStream stream = this.ContainsStream(name) ? this.OpenStream(name) : this.CreateStream(streamToCopy.Name))
    {
      stream.SetLength(0L);
      streamToCopy.CopyTo(stream);
    }
  }

  public void Dispose()
  {
    if (this.m_storage == null)
      return;
    Marshal.FinalReleaseComObject((object) this.m_storage);
    GC.SuppressFinalize((object) this.m_storage);
    this.m_storage = (IStorage) null;
  }

  private delegate void SubItemNameEventHandler(STATSTG item, object userData);
}
