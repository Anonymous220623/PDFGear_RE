// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.StreamsManager
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.CompoundFile.DocIO;
using Syncfusion.CompoundFile.DocIO.Native;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

internal class StreamsManager
{
  public const string MacrosStorageName = "Macros";
  public const string ObjectPoolStorageName = "ObjectPool";
  private const STGM c_flagsDenyWrite = STGM.STGM_SHARE_DENY_WRITE;
  private const STGM c_flagsReadExclusive = STGM.STGM_SHARE_EXCLUSIVE;
  private const STGM c_flagsReadOnly = STGM.STGM_SHARE_DENY_NONE | STGM.STGM_DIRECT_SWMR;
  private const STGM c_flagsReadWriteExclusive = STGM.STGM_READWRITE | STGM.STGM_SHARE_EXCLUSIVE;
  private const string c_mainStream = "WordDocument";
  private const string c_dataStream = "Data";
  private const string c_tableStream = "1Table";
  private const string c_summaryInfoStream = "\u0005SummaryInformation";
  private const string c_documentSummaryInfoStream = "\u0005DocumentSummaryInformation";
  private byte[] m_compObjData = new byte[121]
  {
    (byte) 1,
    (byte) 0,
    (byte) 254,
    byte.MaxValue,
    (byte) 3,
    (byte) 10,
    (byte) 0,
    (byte) 0,
    byte.MaxValue,
    byte.MaxValue,
    byte.MaxValue,
    byte.MaxValue,
    (byte) 6,
    (byte) 9,
    (byte) 2,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 192 /*0xC0*/,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 70,
    (byte) 39,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 77,
    (byte) 105,
    (byte) 99,
    (byte) 114,
    (byte) 111,
    (byte) 115,
    (byte) 111,
    (byte) 102,
    (byte) 116,
    (byte) 32 /*0x20*/,
    (byte) 79,
    (byte) 102,
    (byte) 102,
    (byte) 105,
    (byte) 99,
    (byte) 101,
    (byte) 32 /*0x20*/,
    (byte) 87,
    (byte) 111,
    (byte) 114,
    (byte) 100,
    (byte) 32 /*0x20*/,
    (byte) 57,
    (byte) 55,
    (byte) 45,
    (byte) 50,
    (byte) 48 /*0x30*/,
    (byte) 48 /*0x30*/,
    (byte) 51,
    (byte) 32 /*0x20*/,
    (byte) 68,
    (byte) 111,
    (byte) 99,
    (byte) 117,
    (byte) 109,
    (byte) 101,
    (byte) 110,
    (byte) 116,
    (byte) 0,
    (byte) 10,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 77,
    (byte) 83,
    (byte) 87,
    (byte) 111,
    (byte) 114,
    (byte) 100,
    (byte) 68,
    (byte) 111,
    (byte) 99,
    (byte) 0,
    (byte) 16 /*0x10*/,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 87,
    (byte) 111,
    (byte) 114,
    (byte) 100,
    (byte) 46,
    (byte) 68,
    (byte) 111,
    (byte) 99,
    (byte) 117,
    (byte) 109,
    (byte) 101,
    (byte) 110,
    (byte) 116,
    (byte) 46,
    (byte) 56,
    (byte) 0,
    (byte) 244,
    (byte) 57,
    (byte) 178,
    (byte) 113,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0
  };
  private string m_fileName;
  private Stream m_outStream;
  private StgStream m_stgStream;
  private ICompoundFile m_compoundFile;
  private MemoryStream m_mainStream;
  private MemoryStream m_tableStream;
  private MemoryStream m_dataStream;
  private MemoryStream m_macrosStream;
  private MemoryStream m_summaryInfoStream;
  private MemoryStream m_documentSummaryInfoStream;
  private MemoryStream m_objectPoolStream;
  private BinaryWriter m_mainWriter;
  private BinaryWriter m_tableWriter;
  private BinaryWriter m_dataWriter;
  private BinaryWriter m_summaryInfoWriter;
  private BinaryWriter m_documentSummaryInfoWriter;
  private BinaryReader m_mainReader;
  private BinaryReader m_tableReader;
  private BinaryReader m_dataReader;
  private BinaryReader m_summaryInfoReader;
  private BinaryReader m_documentSummaryInfoReader;
  private bool m_bNetStorage;

  internal StreamsManager(string fileName, bool createNewStorage)
  {
    if (createNewStorage)
    {
      this.InitStreams();
      this.m_fileName = fileName;
      if (this.m_bNetStorage)
        this.m_compoundFile = (ICompoundFile) new Syncfusion.CompoundFile.DocIO.Net.CompoundFile();
      else
        this.m_stgStream = StgStream.CreateStorage(fileName);
    }
    else
      this.LoadStg(fileName);
  }

  internal StreamsManager(Stream stream, bool createNewStorage)
  {
    if (createNewStorage)
    {
      this.InitStreams();
      this.m_outStream = stream;
      if (this.m_bNetStorage)
        this.m_compoundFile = (ICompoundFile) new Syncfusion.CompoundFile.DocIO.Net.CompoundFile();
      else
        this.m_stgStream = StgStream.CreateStorageOnILockBytes();
    }
    else
      this.LoadStg(stream);
  }

  internal ICompoundFile CompoundFile => this.m_compoundFile;

  internal StgStream Storage => this.m_stgStream;

  internal MemoryStream MainStream => this.m_mainStream;

  internal MemoryStream TableStream => this.m_tableStream;

  internal MemoryStream DataStream => this.m_dataStream;

  internal MemoryStream MacrosStream
  {
    get => this.m_macrosStream;
    set => this.m_macrosStream = value;
  }

  internal MemoryStream ObjectPoolStream
  {
    get => this.m_objectPoolStream;
    set => this.m_objectPoolStream = value;
  }

  internal MemoryStream SummaryInfoStream
  {
    get => this.m_summaryInfoStream;
    set => this.m_summaryInfoStream = value;
  }

  internal MemoryStream DocumentSummaryInfoStream
  {
    get => this.m_documentSummaryInfoStream;
    set => this.m_documentSummaryInfoStream = value;
  }

  internal BinaryWriter SummaryInfoWriter
  {
    get => this.m_summaryInfoWriter;
    set => this.m_summaryInfoWriter = value;
  }

  internal BinaryWriter DocumentSummaryInfoWriter
  {
    get => this.m_documentSummaryInfoWriter;
    set => this.m_documentSummaryInfoWriter = value;
  }

  internal BinaryReader SummaryInfoReader
  {
    get => this.m_summaryInfoReader;
    set => this.m_summaryInfoReader = value;
  }

  internal BinaryReader DocumentSummaryInfoReader
  {
    get => this.m_documentSummaryInfoReader;
    set => this.m_documentSummaryInfoReader = value;
  }

  internal BinaryWriter MainWriter => this.m_mainWriter;

  internal BinaryWriter TableWriter => this.m_tableWriter;

  internal BinaryWriter DataWriter => this.m_dataWriter;

  internal BinaryReader MainReader => this.m_mainReader;

  internal BinaryReader TableReader => this.m_tableReader;

  internal BinaryReader DataReader => this.m_dataReader;

  internal void LoadStg(string fileName)
  {
    if (this.m_bNetStorage)
      this.m_compoundFile = (ICompoundFile) new Syncfusion.CompoundFile.DocIO.Net.CompoundFile(fileName, false);
    else
      this.m_stgStream = new StgStream(fileName, STGM.STGM_SHARE_DENY_NONE | STGM.STGM_DIRECT_SWMR);
    this.LoadStreams();
  }

  internal void LoadStg(Stream stream)
  {
    if (this.m_bNetStorage)
      this.m_compoundFile = (ICompoundFile) new Syncfusion.CompoundFile.DocIO.Net.CompoundFile(stream);
    else
      this.m_stgStream = new StgStream(stream, STGM.STGM_SHARE_DENY_NONE | STGM.STGM_DIRECT_SWMR);
    this.LoadStreams();
  }

  internal void LoadTableStream(string tableStreamName)
  {
    this.m_tableStream = !this.m_bNetStorage ? this.LoadStreamFromStg(tableStreamName) : this.LoadStreamFromCompound(tableStreamName);
    this.m_tableReader = new BinaryReader((Stream) this.m_tableStream);
  }

  internal void LoadSummaryInfoStream()
  {
    if (this.m_bNetStorage)
    {
      if (this.m_compoundFile.RootStorage.ContainsStream("\u0005SummaryInformation"))
        this.m_summaryInfoStream = this.LoadStreamFromCompound("\u0005SummaryInformation");
    }
    else if (this.m_stgStream.ContainsStream("\u0005SummaryInformation"))
      this.m_summaryInfoStream = this.LoadStreamFromStg("\u0005SummaryInformation");
    if (this.m_summaryInfoStream == null)
      return;
    this.m_summaryInfoReader = new BinaryReader((Stream) this.m_summaryInfoStream);
  }

  internal void LoadDocumentSummaryInfoStream()
  {
    if (this.m_bNetStorage)
    {
      if (this.m_compoundFile.RootStorage.ContainsStream("\u0005DocumentSummaryInformation"))
        this.m_documentSummaryInfoStream = this.LoadStreamFromCompound("\u0005DocumentSummaryInformation");
    }
    else if (this.m_stgStream.ContainsStream("\u0005DocumentSummaryInformation"))
      this.m_documentSummaryInfoStream = this.LoadStreamFromStg("\u0005DocumentSummaryInformation");
    if (this.m_documentSummaryInfoStream == null)
      return;
    this.m_documentSummaryInfoReader = new BinaryReader((Stream) this.m_documentSummaryInfoStream);
  }

  internal void UpdateStreams(
    MemoryStream mainStream,
    MemoryStream tableStream,
    MemoryStream dataStream)
  {
    this.m_mainStream = mainStream;
    this.m_tableStream = tableStream;
    this.m_dataStream = dataStream;
    if (this.m_dataStream == null)
      return;
    this.m_dataReader = new BinaryReader((Stream) this.m_dataStream);
  }

  internal void WriteSubStorage(MemoryStream stream, string storageName)
  {
    if (this.m_bNetStorage)
      return;
    stream.Position = 0L;
    StgStream stgStream = new StgStream((Stream) stream);
    StgStream source = stgStream.OpenSubStorage(storageName);
    StgStream.CopySourceStorages(source, this.m_stgStream);
    stgStream.Dispose();
    source.Dispose();
  }

  internal void SaveStg(Dictionary<string, Syncfusion.DocIO.ReaderWriter.DataStreamParser.OLEObject.Storage> oleObjectCollection)
  {
    this.SaveStream("WordDocument", this.m_mainStream);
    this.SaveStream("1Table", this.m_tableStream);
    this.SaveCompObjStream();
    if (this.m_dataStream.Length != 0L)
      this.SaveStream("Data", this.m_dataStream);
    if (this.m_macrosStream != null)
      this.WriteSubStorage(this.m_macrosStream, "Macros");
    if (oleObjectCollection.Count > 0)
    {
      Syncfusion.CompoundFile.DocIO.Net.CompoundFile cmpFile = new Syncfusion.CompoundFile.DocIO.Net.CompoundFile();
      ICompoundStorage storage1 = cmpFile.RootStorage.CreateStorage("ObjectPool");
      int storageIndex = 2;
      foreach (string key in oleObjectCollection.Keys)
      {
        ICompoundStorage storage2 = storage1.CreateStorage("_" + key);
        oleObjectCollection[key].UpdateGuid(cmpFile, storageIndex, key);
        ++storageIndex;
        oleObjectCollection[key].WriteToStorage(storage2);
      }
      cmpFile.Flush();
      MemoryStream stream = new MemoryStream((cmpFile.BaseStream as MemoryStream).ToArray());
      cmpFile.Dispose();
      this.WriteSubStorage(stream, "ObjectPool");
      stream.Dispose();
    }
    if (this.m_summaryInfoStream != null && this.m_summaryInfoStream.Length != 0L)
      this.SaveStream("\u0005SummaryInformation", this.m_summaryInfoStream);
    if (this.m_documentSummaryInfoStream != null && this.m_documentSummaryInfoStream.Length != 0L)
      this.SaveStream("\u0005DocumentSummaryInformation", this.m_documentSummaryInfoStream);
    if (this.m_outStream != null)
    {
      if (this.m_bNetStorage)
      {
        this.m_compoundFile.Save(this.m_outStream);
        this.m_outStream.Flush();
      }
      else
      {
        this.m_stgStream.Flush();
        this.m_stgStream.SaveILockBytesIntoStream(this.m_outStream);
        this.m_outStream.Flush();
      }
    }
    this.CloseStg();
  }

  internal void CloseStg()
  {
    if (this.m_outStream != null)
      this.m_outStream = (Stream) null;
    if (this.m_compoundFile != null)
    {
      this.m_compoundFile.Dispose();
      this.m_compoundFile = (ICompoundFile) null;
    }
    else if (this.m_stgStream != null)
    {
      this.m_stgStream.Dispose();
      this.m_stgStream = (StgStream) null;
    }
    if (this.m_mainStream != null)
    {
      this.m_mainStream.Close();
      this.m_mainStream = (MemoryStream) null;
    }
    if (this.m_tableStream != null)
    {
      this.m_tableStream.Close();
      this.m_tableStream = (MemoryStream) null;
    }
    if (this.m_dataStream != null)
    {
      this.m_dataStream.Close();
      this.m_dataStream = (MemoryStream) null;
    }
    if (this.m_macrosStream != null)
    {
      this.m_macrosStream.Close();
      this.m_macrosStream = (MemoryStream) null;
    }
    if (this.m_objectPoolStream != null)
    {
      this.m_objectPoolStream.Close();
      this.m_objectPoolStream = (MemoryStream) null;
    }
    if (this.m_summaryInfoStream != null)
    {
      this.m_summaryInfoStream.Close();
      this.m_summaryInfoStream = (MemoryStream) null;
    }
    if (this.m_documentSummaryInfoStream != null)
    {
      this.m_documentSummaryInfoStream.Close();
      this.m_documentSummaryInfoStream = (MemoryStream) null;
    }
    if (this.m_mainWriter != null)
      this.m_mainWriter.Close();
    if (this.m_tableWriter != null)
      this.m_tableWriter.Close();
    if (this.m_dataWriter != null)
      this.m_dataWriter.Close();
    if (this.m_summaryInfoWriter != null)
      this.m_summaryInfoWriter.Close();
    if (this.m_documentSummaryInfoWriter != null)
      this.m_documentSummaryInfoWriter.Close();
    if (this.m_mainReader != null)
      this.m_mainReader.Close();
    if (this.m_tableReader != null)
      this.m_tableReader.Close();
    if (this.m_dataReader != null)
      this.m_dataReader.Close();
    if (this.m_summaryInfoReader != null)
      this.m_summaryInfoReader.Close();
    if (this.m_documentSummaryInfoReader == null)
      return;
    this.m_documentSummaryInfoReader.Close();
  }

  private void SaveStream(string name, MemoryStream stream)
  {
    if (this.m_bNetStorage)
    {
      using (CompoundStream stream1 = this.m_compoundFile.RootStorage.CreateStream(name))
        stream1.Write(stream.GetBuffer(), 0, (int) stream.Length);
    }
    else
    {
      this.m_stgStream.CreateStream(name);
      this.m_stgStream.Write(stream.GetBuffer(), 0, (int) stream.Length);
      this.m_stgStream.Close();
    }
  }

  private void SaveCompObjStream()
  {
    if (this.m_bNetStorage)
    {
      using (CompoundStream stream = this.m_compoundFile.RootStorage.CreateStream("\u0001CompObj"))
        stream.Write(this.m_compObjData, 0, this.m_compObjData.Length);
    }
    else
    {
      this.m_stgStream.CreateStream("\u0001CompObj");
      this.m_stgStream.Write(this.m_compObjData, 0, this.m_compObjData.Length);
      this.m_stgStream.Close();
    }
  }

  private void InitStreams()
  {
    this.m_mainStream = new MemoryStream(4095 /*0x0FFF*/);
    this.m_mainWriter = new BinaryWriter((Stream) this.m_mainStream);
    this.m_tableStream = new MemoryStream(4095 /*0x0FFF*/);
    this.m_tableWriter = new BinaryWriter((Stream) this.m_mainStream);
    this.m_dataStream = new MemoryStream();
    this.m_dataWriter = new BinaryWriter((Stream) this.m_dataStream);
    this.m_documentSummaryInfoStream = new MemoryStream();
    this.m_documentSummaryInfoWriter = new BinaryWriter((Stream) this.m_documentSummaryInfoStream);
    this.m_summaryInfoStream = new MemoryStream();
    this.m_summaryInfoWriter = new BinaryWriter((Stream) this.m_summaryInfoStream);
  }

  private void LoadStreams()
  {
    if (this.m_bNetStorage)
    {
      this.m_mainStream = this.LoadStreamFromCompound("WordDocument");
      if (this.m_compoundFile.RootStorage.ContainsStream("Data"))
        this.m_dataStream = this.LoadStreamFromCompound("Data");
      this.m_macrosStream = this.ReadSubStorage("Macros");
      this.m_objectPoolStream = this.ReadSubStorage("ObjectPool");
    }
    else
    {
      this.m_mainStream = this.LoadStreamFromStg("WordDocument");
      if (this.m_stgStream.ContainsStream("Data"))
        this.m_dataStream = this.LoadStreamFromStg("Data");
      this.m_macrosStream = this.ReadSubStorage("Macros");
      this.m_objectPoolStream = this.ReadSubStorage("ObjectPool");
    }
    this.m_mainReader = new BinaryReader((Stream) this.m_mainStream);
    if (this.m_dataStream == null)
      return;
    this.m_dataReader = new BinaryReader((Stream) this.m_dataStream);
  }

  private MemoryStream LoadStreamFromCompound(string name)
  {
    byte[] buffer = (byte[]) null;
    using (CompoundStream compoundStream = this.m_compoundFile.RootStorage.OpenStream(name))
    {
      int length = (int) compoundStream.Length;
      buffer = new byte[length];
      compoundStream.Read(buffer, 0, length);
    }
    return new MemoryStream(buffer);
  }

  private MemoryStream LoadSubStorage(string name) => (MemoryStream) null;

  private MemoryStream ReadSubStorage(string stgName)
  {
    if (this.m_bNetStorage)
    {
      if (this.m_compoundFile.RootStorage.ContainsStorage(stgName))
      {
        ICompoundStorage storageToCopy = this.m_compoundFile.RootStorage.OpenStorage(stgName);
        Syncfusion.CompoundFile.DocIO.Net.CompoundFile compoundFile = new Syncfusion.CompoundFile.DocIO.Net.CompoundFile();
        compoundFile.RootStorage.InsertCopy(storageToCopy);
        compoundFile.Flush();
        MemoryStream memoryStream = this.CopyStream(compoundFile.BaseStream);
        memoryStream.Position = 0L;
        compoundFile.Dispose();
        return memoryStream;
      }
    }
    else if (this.m_stgStream.ContainsStorage(stgName))
    {
      StgStream source = this.m_stgStream.OpenSubStorage(stgName);
      StgStream storageOnIlockBytes = StgStream.CreateStorageOnILockBytes();
      StgStream.CopySourceStorages(source, storageOnIlockBytes);
      MemoryStream memoryStream = new MemoryStream();
      storageOnIlockBytes.SaveILockBytesIntoStream((Stream) memoryStream);
      storageOnIlockBytes.Close();
      storageOnIlockBytes.Dispose();
      source.Close();
      source.Dispose();
      return memoryStream;
    }
    return (MemoryStream) null;
  }

  private MemoryStream CopyStream(Stream inputStream)
  {
    try
    {
      byte[] buffer = new byte[inputStream.Length];
      long position = inputStream.Position;
      inputStream.Position = 0L;
      inputStream.Read(buffer, 0, buffer.Length);
      inputStream.Position = position;
      return new MemoryStream(buffer);
    }
    catch
    {
      throw new ArgumentException("Cannot read data from stream ");
    }
  }

  private MemoryStream LoadStreamFromStg(string streamName)
  {
    try
    {
      this.m_stgStream.OpenStream(streamName, STGM.STGM_SHARE_EXCLUSIVE);
      Stream stgStream = (Stream) this.m_stgStream;
      byte[] buffer = new byte[stgStream.Length];
      long position = stgStream.Position;
      stgStream.Position = 0L;
      stgStream.Read(buffer, 0, buffer.Length);
      stgStream.Position = position;
      return new MemoryStream(buffer);
    }
    catch
    {
      throw new ArgumentException("Cannot read data from stream ");
    }
    finally
    {
      this.m_stgStream.Close();
    }
  }
}
