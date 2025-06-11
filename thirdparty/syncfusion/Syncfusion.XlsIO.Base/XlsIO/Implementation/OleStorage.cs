// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.OleStorage
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.CompoundFile.XlsIO;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class OleStorage
{
  private Dictionary<string, MemoryStream> m_arrayStreams = new Dictionary<string, MemoryStream>();
  private List<string> m_streamNames = new List<string>();
  private string m_storageName;

  public string StorageName => this.m_storageName;

  public Dictionary<string, MemoryStream> StorageStreams => this.m_arrayStreams;

  public OleStorage(string Name) => this.m_storageName = Name;

  public List<string> StreamNames => this.m_streamNames;

  internal void ParseStream(CompoundStream oleStream)
  {
    MemoryStream memoryStream = new MemoryStream((int) oleStream.Length);
    UtilityMethods.CopyStreamTo((Stream) oleStream, (Stream) memoryStream);
    oleStream.Position = 0L;
    this.Add(oleStream.Name, memoryStream);
    this.m_streamNames.Add(oleStream.Name);
  }

  internal MemoryStream OpenStream(string streamName) => this.m_arrayStreams[streamName];

  private void Add(string StreamName, MemoryStream ControlStream)
  {
    this.m_arrayStreams.Add(StreamName, ControlStream);
  }
}
