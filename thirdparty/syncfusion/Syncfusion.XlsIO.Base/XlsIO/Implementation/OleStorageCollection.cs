// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.OleStorageCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.CompoundFile.XlsIO;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class OleStorageCollection
{
  private string[] OleConstants = new string[3]
  {
    "_VBA_PROJECT_CUR",
    "_SX_DB_CUR",
    "MsoDataStore"
  };
  private Dictionary<string, OleStorage> m_OleStorage = new Dictionary<string, OleStorage>();
  private Dictionary<string, MemoryStream> m_arrayStream = new Dictionary<string, MemoryStream>();
  private WorkbookImpl m_workbook;
  private List<string> m_OleStoragesNames = new List<string>();
  private List<string> m_arrayStreamNames = new List<string>();

  internal void Add(OleStorage storage)
  {
    this.m_OleStorage.Add(storage.StorageName, storage);
    this.m_OleStoragesNames.Add(storage.StorageName);
  }

  internal void Add(string streamName, CompoundStream stream)
  {
    MemoryStream destination = new MemoryStream((int) stream.Length);
    UtilityMethods.CopyStreamTo((Stream) stream, (Stream) destination);
    stream.Position = 0L;
    this.m_arrayStream.Add(streamName, destination);
    this.m_arrayStreamNames.Add(streamName);
  }

  internal void Add(string streamName, MemoryStream stream)
  {
    stream.Position = 0L;
    this.m_arrayStream.Add(streamName, stream);
    this.m_arrayStreamNames.Add(streamName);
  }

  public List<string> OleStoragesNames => this.m_OleStoragesNames;

  public List<string> ArrayStreamNames => this.m_arrayStreamNames;

  internal void ParseStorage(ICompoundStorage CompoundStorage)
  {
    string[] streams = CompoundStorage.Streams;
    OleStorage storage = new OleStorage(CompoundStorage.Name);
    if (Array.IndexOf<string>(streams, "CONTENTS") != -1)
    {
      using (CompoundStream oleStream = CompoundStorage.OpenStream("CONTENTS"))
        storage.ParseStream(oleStream);
    }
    else
    {
      foreach (string streamName in streams)
      {
        using (CompoundStream oleStream = CompoundStorage.OpenStream(streamName))
          storage.ParseStream(oleStream);
      }
    }
    this.Add(storage);
  }

  internal OleStorage OpenStorage(string StorageName) => this.m_OleStorage[StorageName];

  internal MemoryStream OpenStream(string streamName) => this.m_arrayStream[streamName];
}
