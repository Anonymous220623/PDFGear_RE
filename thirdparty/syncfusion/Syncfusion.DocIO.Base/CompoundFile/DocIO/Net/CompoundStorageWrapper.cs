// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.DocIO.Net.CompoundStorageWrapper
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.CompoundFile.DocIO.Net;

internal class CompoundStorageWrapper : ICompoundStorage, IDisposable
{
  private CompoundStorage m_storage;

  public CompoundStorageWrapper(CompoundStorage wrapped) => this.m_storage = wrapped;

  public void Dispose()
  {
    if (this.m_storage == null)
      return;
    this.m_storage = (CompoundStorage) null;
    GC.SuppressFinalize((object) this);
  }

  public CompoundStream CreateStream(string streamName) => this.m_storage.CreateStream(streamName);

  public CompoundStream OpenStream(string streamName) => this.m_storage.OpenStream(streamName);

  public void DeleteStream(string streamName) => this.m_storage.DeleteStream(streamName);

  public bool ContainsStream(string streamName) => this.m_storage.ContainsStream(streamName);

  public ICompoundStorage CreateStorage(string storageName)
  {
    return this.m_storage.CreateStorage(storageName);
  }

  public ICompoundStorage OpenStorage(string storageName)
  {
    return this.m_storage.OpenStorage(storageName);
  }

  public void DeleteStorage(string storageName) => this.m_storage.DeleteStorage(storageName);

  public bool ContainsStorage(string storageName) => this.m_storage.ContainsStorage(storageName);

  public void Flush() => this.m_storage.Flush();

  public string[] Streams => this.m_storage.Streams;

  public string[] Storages => this.m_storage.Storages;

  public string Name => this.m_storage.Name;

  public DirectoryEntry Entry => this.m_storage.Entry;

  public void InsertCopy(ICompoundStorage storageToCopy)
  {
    this.m_storage.InsertCopy(storageToCopy);
  }

  internal void UpdateStorageGuid(ICompoundStorage storageToCopy)
  {
    this.m_storage.Entry.StorageGuid = (storageToCopy as CompoundStorageWrapper).m_storage.Entry.StorageGuid;
  }

  public void InsertCopy(CompoundStream streamToCopy) => this.m_storage.InsertCopy(streamToCopy);
}
