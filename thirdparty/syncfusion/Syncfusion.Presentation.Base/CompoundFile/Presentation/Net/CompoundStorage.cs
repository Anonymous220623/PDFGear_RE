// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.Presentation.Net.CompoundStorage
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.CompoundFile.Presentation.Net;

internal class CompoundStorage : ICompoundStorage, IDisposable, ICompoundItem
{
  private Syncfusion.CompoundFile.Presentation.Net.CompoundFile m_parentFile;
  private SortedList<string, ICompoundItem> m_nodes = new SortedList<string, ICompoundItem>((IComparer<string>) new ItemNamesComparer());
  private DirectoryEntry m_entry;
  private List<string> m_arrStorages = new List<string>();
  private List<string> m_arrStreams = new List<string>();

  public CompoundStorage(Syncfusion.CompoundFile.Presentation.Net.CompoundFile parent, string name, int entryIndex)
  {
    this.m_parentFile = parent != null ? parent : throw new ArgumentNullException(nameof (parent));
    this.m_entry = new DirectoryEntry(name, DirectoryEntry.EntryType.Storage, entryIndex);
  }

  public CompoundStorage(Syncfusion.CompoundFile.Presentation.Net.CompoundFile parentFile, DirectoryEntry entry)
  {
    if (parentFile == null)
      throw new ArgumentNullException(nameof (parentFile));
    if (entry == null)
      throw new ArgumentNullException(nameof (entry));
    this.m_entry = entry.Type == DirectoryEntry.EntryType.Storage || entry.Type == DirectoryEntry.EntryType.Root ? entry : throw new ArgumentOutOfRangeException(nameof (entry));
    this.m_parentFile = parentFile;
    this.AddItem(entry.ChildId);
  }

  private void AddItem(int entryIndex)
  {
    if (entryIndex < 0)
      return;
    DirectoryEntry entry = this.m_parentFile.Directory.Entries[entryIndex];
    int leftId = entry.LeftId;
    string name = entry.Name;
    this.AddItem(leftId);
    switch (entry.Type)
    {
      case DirectoryEntry.EntryType.Storage:
        this.m_nodes.Add(name, (ICompoundItem) new CompoundStorage(this.m_parentFile, entry));
        this.m_arrStorages.Add(name);
        break;
      case DirectoryEntry.EntryType.Stream:
        if (!this.m_arrStreams.Contains(name))
        {
          CompoundStreamNet compoundStreamNet = new CompoundStreamNet(this.m_parentFile, entry);
          this.m_nodes.Add(name, (ICompoundItem) compoundStreamNet);
          this.m_arrStreams.Add(name);
          break;
        }
        break;
      default:
        throw new NotImplementedException();
    }
    this.AddItem(entry.RightId);
  }

  public CompoundStream CreateStream(string streamName)
  {
    DirectoryEntry entry = !this.ContainsStream(streamName) && !this.ContainsStorage(streamName) ? this.m_parentFile.AllocateDirectoryEntry(streamName, DirectoryEntry.EntryType.Stream) : throw new ArgumentOutOfRangeException(nameof (streamName), "Object with such name already exists");
    CompoundStreamNet wrapped = this.m_parentFile.DirectMode ? (CompoundStreamNet) new CompoundStreamDirect(this.m_parentFile, entry) : new CompoundStreamNet(this.m_parentFile, entry);
    this.m_arrStreams.Add(streamName);
    this.m_nodes.Add(streamName, (ICompoundItem) wrapped);
    wrapped.Open();
    return (CompoundStream) new CompoundStreamWrapper((CompoundStream) wrapped);
  }

  public CompoundStream OpenStream(string streamName)
  {
    if (this.m_nodes[streamName] is CompoundStreamNet node)
      node.Open();
    return (CompoundStream) new CompoundStreamWrapper((CompoundStream) node);
  }

  public void DeleteStream(string streamName)
  {
    if (!(this.m_nodes[streamName] is CompoundStreamNet node))
      return;
    this.m_parentFile.RemoveItem(node.Entry);
    node.Dispose();
    this.m_nodes.Remove(streamName);
  }

  public bool ContainsStream(string streamName) => this.m_nodes.ContainsKey(streamName);

  public ICompoundStorage CreateStorage(string storageName)
  {
    DirectoryEntry entry = !this.ContainsStream(storageName) && !this.ContainsStorage(storageName) ? this.m_parentFile.AllocateDirectoryEntry(storageName, DirectoryEntry.EntryType.Storage) : throw new ArgumentOutOfRangeException("streamName", "Object with such name already exists");
    entry.DateCreate = entry.DateModify = DateTime.Now;
    CompoundStorage wrapped = new CompoundStorage(this.m_parentFile, entry);
    this.m_arrStorages.Add(storageName);
    this.m_nodes.Add(storageName, (ICompoundItem) wrapped);
    wrapped.Open();
    return (ICompoundStorage) new CompoundStorageWrapper(wrapped);
  }

  public ICompoundStorage OpenStorage(string storageName)
  {
    if (this.m_nodes[storageName] is CompoundStorage node)
      node.Open();
    return (ICompoundStorage) new CompoundStorageWrapper(node);
  }

  private void Open()
  {
  }

  public void DeleteStorage(string storageName)
  {
    if (!(this.m_nodes[storageName] is CompoundStorage node))
      return;
    node.Dispose();
    this.m_nodes.Remove(storageName);
  }

  public void Dispose()
  {
    if (this.m_parentFile == null)
      return;
    this.m_parentFile = (Syncfusion.CompoundFile.Presentation.Net.CompoundFile) null;
    this.m_nodes = (SortedList<string, ICompoundItem>) null;
    this.m_entry = (DirectoryEntry) null;
    GC.SuppressFinalize((object) this);
  }

  public bool ContainsStorage(string storageName) => this.m_nodes.ContainsKey(storageName);

  public void Flush()
  {
    this.m_entry.LeftId = -1;
    foreach (ICompoundItem compoundItem in (IEnumerable<ICompoundItem>) this.m_nodes.Values)
      compoundItem.Flush();
    ICompoundItem compoundItem1 = (ICompoundItem) null;
    foreach (ICompoundItem compoundItem2 in (IEnumerable<ICompoundItem>) this.m_nodes.Values)
    {
      if (compoundItem1 != null)
      {
        compoundItem1.Entry.RightId = compoundItem2.Entry.EntryId;
        compoundItem1.Entry.LeftId = -1;
      }
      else
        this.m_entry.ChildId = compoundItem2.Entry.EntryId;
      compoundItem1 = compoundItem2;
    }
  }

  private void UpdateDirectory(RBTreeNode node)
  {
    object obj = node.Value;
    if (obj == null)
      return;
    DirectoryEntry entry = (obj as ICompoundItem).Entry;
    entry.Color = (byte) node.Color;
    entry.LeftId = this.GetNodeId(node.Left);
    entry.RightId = this.GetNodeId(node.Right);
    if (this.m_entry.ChildId < 0)
      this.m_entry.ChildId = entry.EntryId;
    if (!(obj is CompoundStreamNet compoundStreamNet))
      return;
    entry.Size = (uint) compoundStreamNet.Length;
  }

  private int GetNodeId(RBTreeNode node)
  {
    return node.IsNil ? -1 : (node.Value as ICompoundItem).Entry.EntryId;
  }

  public string[] Streams => this.m_arrStreams.ToArray();

  public string[] Storages => this.m_arrStorages.ToArray();

  public string Name => this.m_entry.Name;

  public DirectoryEntry Entry => this.m_entry;

  public void InsertCopy(ICompoundStorage storageToCopy)
  {
    ICompoundStorage storage = this.CreateStorage(storageToCopy.Name);
    if (storageToCopy is CompoundStorageWrapper && storage is CompoundStorageWrapper && (storageToCopy as CompoundStorageWrapper).Entry.StorageGuid != Guid.Empty)
      (storage as CompoundStorageWrapper).Entry.StorageGuid = new Guid((storageToCopy as CompoundStorageWrapper).Entry.StorageGuid.ToByteArray());
    string[] streams = storageToCopy.Streams;
    int index1 = 0;
    for (int length = streams.Length; index1 < length; ++index1)
    {
      using (CompoundStream streamToCopy = storageToCopy.OpenStream(streams[index1]))
        storage.InsertCopy(streamToCopy);
    }
    string[] storages = storageToCopy.Storages;
    int index2 = 0;
    for (int length = storages.Length; index2 < length; ++index2)
    {
      using (ICompoundStorage storageToCopy1 = storageToCopy.OpenStorage(storages[index2]))
        storage.InsertCopy(storageToCopy1);
    }
  }

  public void InsertCopy(CompoundStream streamToCopy)
  {
    CompoundStream compoundStream = streamToCopy != null ? this.CreateStream(streamToCopy.Name) : throw new ArgumentNullException(nameof (streamToCopy));
    byte[] buffer = new byte[32768 /*0x8000*/];
    long position = streamToCopy.Position;
    streamToCopy.Position = 0L;
    int count;
    while ((count = streamToCopy.Read(buffer, 0, 32768 /*0x8000*/)) > 0)
      compoundStream.Write(buffer, 0, count);
    streamToCopy.Position = position;
  }
}
