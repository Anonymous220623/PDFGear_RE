// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.OLEObject.Storage
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.CompoundFile.DocIO;
using Syncfusion.CompoundFile.DocIO.Net;
using Syncfusion.DocIO.DLS;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.OLEObject;

internal class Storage
{
  private Dictionary<string, Stream> m_streams = new Dictionary<string, Stream>();
  private List<Storage> m_storages = new List<Storage>();
  private string m_storageName;
  private int m_occurenceCount;
  private Guid m_guid;

  internal string StorageName
  {
    get => this.m_storageName;
    set => this.m_storageName = value;
  }

  internal Guid Guid
  {
    get => this.m_guid;
    set => this.m_guid = value;
  }

  internal Dictionary<string, Stream> Streams => this.m_streams;

  internal List<Storage> Storages => this.m_storages;

  internal int OccurrenceCount
  {
    get => this.m_occurenceCount;
    set => this.m_occurenceCount = value;
  }

  internal Storage(string storageName) => this.m_storageName = storageName;

  internal void ParseStorages(ICompoundStorage storage)
  {
    foreach (string storage1 in storage.Storages)
    {
      ICompoundStorage storage2 = storage.OpenStorage(storage1);
      Storage storage3 = new Storage(storage1);
      storage3.ParseStorages(storage2);
      storage3.ParseStreams(storage2);
      this.Storages.Add(storage3);
    }
  }

  internal void ParseStreams(ICompoundStorage storage)
  {
    foreach (string stream in storage.Streams)
    {
      CompoundStream compoundStream = storage.OpenStream(stream);
      byte[] buffer = new byte[compoundStream.Length];
      compoundStream.Read(buffer, 0, buffer.Length);
      compoundStream.Dispose();
      this.Streams.Add(stream, (Stream) new MemoryStream(buffer));
    }
  }

  internal void WriteToStorage(ICompoundStorage storage)
  {
    foreach (Storage storage1 in this.Storages)
    {
      ICompoundStorage storage2 = storage.CreateStorage(storage1.StorageName);
      storage1.WriteToStorage(storage2);
    }
    foreach (KeyValuePair<string, Stream> stream1 in this.Streams)
    {
      CompoundStream stream2 = storage.CreateStream(stream1.Key);
      stream2.Write((stream1.Value as MemoryStream).ToArray(), 0, (int) stream1.Value.Length);
      stream2.Flush();
    }
    storage.Flush();
  }

  private byte[] GetByteArray(Stream stream)
  {
    stream.Position = 0L;
    byte[] buffer = new byte[stream.Length];
    stream.Read(buffer, 0, buffer.Length);
    return buffer;
  }

  internal string CompareStorage(Dictionary<string, Storage> oleObjectCollection)
  {
    bool flag = false;
    foreach (string key1 in oleObjectCollection.Keys)
    {
      if (oleObjectCollection[key1].Streams.Count == this.Streams.Count)
      {
        foreach (string key2 in this.Streams.Keys)
        {
          if (!oleObjectCollection[key1].Streams.ContainsKey(key2))
          {
            flag = false;
            break;
          }
          byte[] byteArray1 = this.GetByteArray(this.Streams[key2]);
          byte[] byteArray2 = this.GetByteArray(oleObjectCollection[key1].Streams[key2]);
          if (byteArray1.Length == byteArray2.Length)
          {
            flag = WordDocument.CompareArray(byteArray1, byteArray2);
          }
          else
          {
            flag = false;
            break;
          }
        }
      }
      else
        flag = false;
      if (flag)
        return key1;
    }
    return string.Empty;
  }

  internal void UpdateGuid(Syncfusion.CompoundFile.DocIO.Net.CompoundFile cmpFile, int storageIndex, string storageName)
  {
    for (int index = storageIndex; index < cmpFile.Directory.Entries.Count; ++index)
    {
      DirectoryEntry entry = cmpFile.Directory.Entries[index];
      if (entry.Name == "_" + storageName || entry.Name == "Root Entry")
        entry.StorageGuid = this.Guid;
    }
  }

  internal Storage Clone()
  {
    Storage storage1 = new Storage(this.StorageName);
    foreach (KeyValuePair<string, Stream> stream in this.Streams)
    {
      MemoryStream memoryStream = new MemoryStream((stream.Value as MemoryStream).ToArray());
      storage1.Streams.Add(stream.Key, (Stream) memoryStream);
    }
    foreach (Storage storage2 in this.Storages)
      storage1.Storages.Add(storage2.Clone());
    return storage1;
  }

  internal void Close()
  {
    --this.m_occurenceCount;
    if (this.m_occurenceCount > 0)
      return;
    foreach (KeyValuePair<string, Stream> stream in this.Streams)
      stream.Value.Close();
    this.Streams.Clear();
    foreach (Storage storage in this.Storages)
      storage.Close();
    this.Storages.Clear();
  }
}
