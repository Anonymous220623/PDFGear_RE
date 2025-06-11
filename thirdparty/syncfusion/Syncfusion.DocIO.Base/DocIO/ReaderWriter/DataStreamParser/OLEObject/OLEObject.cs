// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.OLEObject.OLEObject
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.CompoundFile.DocIO;
using Syncfusion.CompoundFile.DocIO.Net;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.OLEObject;

internal class OLEObject
{
  private const string DEF_OLE_STREAM_NAME = "\u0001Ole";
  private const string DEF_CONTENT_STREAM_NAME = "CONTENTS";
  private const string DEF_WP_STREAM_NAME = "Contents";
  private const string DEF_INFO_STREAM_NAME = "\u0003ObjInfo";
  internal const string DEF_COMP_STREAM_NAME = "\u0001CompObj";
  private const string DEF_LINK_INFO_STREAM_NAME = "\u0003LinkInfo";
  private const string DEF_NATIVE_STREAM_NAME = "\u0001Ole10Native";
  private const string DEF_PRINT_STREAM_NAME = "\u0003EPRINT";
  private const string DEF_OLE_PRES000_NAME = "\u0002OlePres000";
  private const string DEF_END_INFO_MARKER = "???";
  private const string DEF_EQUATION_STREAM_NAME = "Equation Native";
  private const string DEF_WORKBOOK_STREAM_NAME = "Workbook";
  private const string DEF_PACKAGE_STREAM_NAME = "Package";
  private const string DEF_PPT_STREAM_NAME = "PowerPoint Document";
  private const string DEF_WORD_STREAM_NAME = "WordDocument";
  private const string DEF_VISIO_STREAM_NAME = "VisioDocument";
  private const string DEF_ODP_STREAM_NAME = "EmbeddedOdf";
  private const string DEF_OOPACKAGE_STREAM_NAME = "package_stream";
  private const string DEF_SUMMARY_STREAM_NAME = "\u0005SummaryInformation";
  private const string DEF_DOC_SUMMARY_STREAM_NAME = "\u0005DocumentSummaryInformation";
  private const string DEF_OBJECT_POOL_NAME = "ObjectPool";
  private OleObjectType m_oleType;
  private Storage m_storage = new Storage("Ole");
  private Guid m_guid;
  private byte m_bFlags;

  internal Guid Guid
  {
    get => this.m_guid;
    set => this.m_guid = value;
  }

  internal Storage Storage => this.m_storage;

  internal OleObjectType OleType
  {
    get
    {
      this.m_oleType = !this.Storage.Streams.ContainsKey("\u0001CompObj") ? OleObjectType.Undefined : OleTypeConvertor.ToOleType(new CompObjectStream(this.Storage.Streams["\u0001CompObj"]).ObjectType);
      return this.m_oleType;
    }
  }

  internal bool Cloned
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal OLEObject()
  {
  }

  internal void ParseObjectPool(
    Stream objectPoolStream,
    string oleStorageName,
    Dictionary<string, Storage> oleObjectCollection)
  {
    this.Storage.Streams.Clear();
    this.Storage.StorageName = oleStorageName;
    using (Syncfusion.CompoundFile.DocIO.Net.CompoundFile compoundFile = new Syncfusion.CompoundFile.DocIO.Net.CompoundFile(objectPoolStream))
    {
      foreach (DirectoryEntry entry in compoundFile.Directory.Entries)
      {
        if ("_" + oleStorageName == entry.Name)
        {
          this.Guid = entry.StorageGuid;
          break;
        }
      }
      if (Array.IndexOf<string>(compoundFile.RootStorage.Storages, "ObjectPool") != -1)
      {
        if (Array.IndexOf<string>(compoundFile.RootStorage.OpenStorage("ObjectPool").Storages, "_" + oleStorageName) != -1)
        {
          ICompoundStorage storage = compoundFile.RootStorage.OpenStorage("ObjectPool").OpenStorage("_" + oleStorageName);
          this.Storage.ParseStreams(storage);
          this.Storage.ParseStorages(storage);
          string collection = this.AddOleObjectToCollection(oleObjectCollection, oleStorageName);
          if (!string.IsNullOrEmpty(collection))
            this.Storage.StorageName = collection;
        }
      }
    }
    objectPoolStream.Position = 0L;
  }

  internal void Save(Stream stream, WOleObject oleObject)
  {
    this.WriteOleStream(oleObject.LinkType, oleObject.OleObjectType, string.Empty);
    this.WriteObjInfoStream(oleObject.LinkType, oleObject.OleObjectType);
    this.WriteCompObjStream(oleObject.OleObjectType);
    if (oleObject.OleObjectType == OleObjectType.Undefined)
      this.Storage.Streams.Add("Package", stream);
    else
      this.WriteNativeData((stream as MemoryStream).ToArray(), string.Empty, oleObject.OleObjectType);
  }

  internal void Save(byte[] nativeData, string dataPath, WOleObject oleObject)
  {
    this.WriteOleStream(oleObject.LinkType, oleObject.OleObjectType, dataPath);
    this.WriteObjInfoStream(oleObject.LinkType, oleObject.OleObjectType);
    if (oleObject.LinkType == OleLinkType.Embed)
    {
      this.WriteCompObjStream(oleObject.OleObjectType);
      this.WriteNativeData(nativeData, dataPath, oleObject.OleObjectType);
    }
    else
      this.WriteLinkInfoStream(oleObject.OleObjectType, dataPath);
  }

  internal string AddOleObjectToCollection(
    Dictionary<string, Storage> oleObjectCollection,
    string oleStorageName)
  {
    if (oleObjectCollection != null && oleObjectCollection.Count == 0)
    {
      oleObjectCollection.Add(oleStorageName, this.Storage);
      oleObjectCollection[oleStorageName].Guid = this.Guid;
      ++oleObjectCollection[oleStorageName].OccurrenceCount;
    }
    else
    {
      if (this.Cloned)
      {
        string str = this.Storage.CompareStorage(oleObjectCollection);
        if (!string.IsNullOrEmpty(str))
          oleStorageName = str;
      }
      if (!oleObjectCollection.ContainsKey(oleStorageName))
      {
        oleObjectCollection.Add(oleStorageName, this.Storage);
        ++oleObjectCollection[oleStorageName].OccurrenceCount;
        oleObjectCollection[oleStorageName].Guid = this.Guid;
      }
      else
      {
        ++oleObjectCollection[oleStorageName].OccurrenceCount;
        this.SetStorage(oleObjectCollection[oleStorageName]);
      }
    }
    return oleStorageName;
  }

  private void WriteNativeData(byte[] nativeData, string dataPath, OleObjectType objType)
  {
    switch (objType)
    {
      case OleObjectType.AdobeAcrobatDocument:
        this.WriteNativeData(nativeData, "CONTENTS");
        break;
      case OleObjectType.BitmapImage:
        this.WritePBrush(nativeData);
        break;
      case OleObjectType.Equation:
        this.WriteNativeData(nativeData, "Equation Native");
        break;
      case OleObjectType.GraphChart:
        this.WriteNativeData(nativeData, "Workbook");
        break;
      case OleObjectType.Excel_97_2003_Worksheet:
      case OleObjectType.ExcelChart:
      case OleObjectType.PowerPoint_97_2003_Presentation:
      case OleObjectType.PowerPoint_97_2003_Slide:
      case OleObjectType.Word_97_2003_Document:
      case OleObjectType.VisioDrawing:
      case OleObjectType.OpenOfficeSpreadsheet1_1:
      case OleObjectType.OpenOfficeText_1_1:
      case OleObjectType.OpenOfficeSpreadsheet:
      case OleObjectType.OpenOfficeText:
        this.WriteNativeStreams((Stream) new MemoryStream(nativeData));
        break;
      case OleObjectType.ExcelBinaryWorksheet:
      case OleObjectType.ExcelMacroWorksheet:
      case OleObjectType.ExcelWorksheet:
      case OleObjectType.PowerPointMacroPresentation:
      case OleObjectType.PowerPointMacroSlide:
      case OleObjectType.PowerPointPresentation:
      case OleObjectType.PowerPointSlide:
      case OleObjectType.WordDocument:
      case OleObjectType.WordMacroDocument:
        this.WriteNativeData(nativeData, "Package");
        break;
      case OleObjectType.OpenDocumentPresentation:
      case OleObjectType.OpenDocumentSpreadsheet:
        this.WriteNativeData(nativeData, "EmbeddedOdf");
        break;
      case OleObjectType.Package:
        this.WritePackage(nativeData, dataPath);
        break;
      case OleObjectType.WordPadDocument:
        this.WriteNativeData(nativeData, "Contents");
        break;
    }
  }

  private void WriteNativeData(byte[] nativeData, string streamName)
  {
    MemoryStream memoryStream = new MemoryStream(nativeData);
    memoryStream.Position = 0L;
    this.Storage.Streams.Add(streamName, (Stream) memoryStream);
  }

  private void WritePBrush(byte[] nativeData)
  {
    int iOffset = 0;
    byte[] numArray = new byte[nativeData.Length + 4];
    ByteConverter.WriteInt32(numArray, ref iOffset, nativeData.Length);
    ByteConverter.WriteBytes(numArray, ref iOffset, nativeData);
    MemoryStream memoryStream = new MemoryStream(numArray);
    memoryStream.Position = 0L;
    this.Storage.Streams.Add("\u0001Ole10Native", (Stream) memoryStream);
  }

  private void WriteNativeStreams(Stream stream)
  {
    Syncfusion.CompoundFile.DocIO.Net.CompoundFile compoundFile = new Syncfusion.CompoundFile.DocIO.Net.CompoundFile(stream);
    string[] streams = compoundFile.RootStorage.Streams;
    int index = 0;
    for (int length = streams.Length; index < length; ++index)
    {
      CompoundStream compoundStream = compoundFile.RootStorage.OpenStream(streams[index]);
      byte[] buffer = new byte[compoundStream.Length];
      compoundStream.Read(buffer, 0, buffer.Length);
      compoundStream.Dispose();
      this.Storage.Streams.Add(streams[index], (Stream) new MemoryStream(buffer));
    }
    compoundFile.Dispose();
  }

  private void WriteCompObjStream(OleObjectType objType)
  {
    switch (objType)
    {
      case OleObjectType.AdobeAcrobatDocument:
      case OleObjectType.BitmapImage:
      case OleObjectType.Equation:
      case OleObjectType.GraphChart:
      case OleObjectType.Excel_97_2003_Worksheet:
      case OleObjectType.ExcelBinaryWorksheet:
      case OleObjectType.ExcelChart:
      case OleObjectType.ExcelMacroWorksheet:
      case OleObjectType.ExcelWorksheet:
      case OleObjectType.PowerPoint_97_2003_Presentation:
      case OleObjectType.PowerPoint_97_2003_Slide:
      case OleObjectType.PowerPointMacroPresentation:
      case OleObjectType.PowerPointMacroSlide:
      case OleObjectType.PowerPointPresentation:
      case OleObjectType.PowerPointSlide:
      case OleObjectType.Word_97_2003_Document:
      case OleObjectType.WordDocument:
      case OleObjectType.WordMacroDocument:
      case OleObjectType.VisioDrawing:
      case OleObjectType.OpenDocumentPresentation:
      case OleObjectType.OpenDocumentSpreadsheet:
      case OleObjectType.OpenOfficeSpreadsheet1_1:
      case OleObjectType.OpenOfficeText_1_1:
      case OleObjectType.Package:
      case OleObjectType.OpenOfficeSpreadsheet:
      case OleObjectType.OpenOfficeText:
        if (this.Storage.Streams.ContainsKey("\u0001CompObj"))
          break;
        MemoryStream memoryStream = new MemoryStream();
        new CompObjectStream(objType).SaveTo((Stream) memoryStream);
        memoryStream.Flush();
        memoryStream.Position = 0L;
        this.Storage.Streams.Add("\u0001CompObj", (Stream) memoryStream);
        break;
    }
  }

  private void WriteLinkInfoStream(OleObjectType objType, string dataPath)
  {
    MemoryStream memoryStream = new MemoryStream();
    new LinkInfoStream(dataPath).SaveTo((Stream) memoryStream);
    memoryStream.Flush();
    memoryStream.Position = 0L;
    this.Storage.Streams.Add("\u0003LinkInfo", (Stream) memoryStream);
  }

  private void WriteOleStream(OleLinkType linkType, OleObjectType objType, string dataPath)
  {
    switch (objType)
    {
      case OleObjectType.AdobeAcrobatDocument:
      case OleObjectType.BitmapImage:
      case OleObjectType.Equation:
      case OleObjectType.GraphChart:
      case OleObjectType.Excel_97_2003_Worksheet:
      case OleObjectType.ExcelBinaryWorksheet:
      case OleObjectType.ExcelChart:
      case OleObjectType.ExcelMacroWorksheet:
      case OleObjectType.ExcelWorksheet:
      case OleObjectType.PowerPoint_97_2003_Presentation:
      case OleObjectType.PowerPoint_97_2003_Slide:
      case OleObjectType.PowerPointMacroPresentation:
      case OleObjectType.PowerPointMacroSlide:
      case OleObjectType.PowerPointSlide:
      case OleObjectType.VisioDrawing:
      case OleObjectType.OpenDocumentPresentation:
      case OleObjectType.OpenDocumentSpreadsheet:
      case OleObjectType.OpenOfficeSpreadsheet1_1:
      case OleObjectType.OpenOfficeText_1_1:
      case OleObjectType.Package:
      case OleObjectType.WordPadDocument:
      case OleObjectType.OpenOfficeText:
        MemoryStream memoryStream = new MemoryStream();
        new OLEStream(linkType, dataPath).SaveTo((Stream) memoryStream);
        memoryStream.Flush();
        memoryStream.Position = 0L;
        this.Storage.Streams.Add("\u0001Ole", (Stream) memoryStream);
        break;
    }
  }

  private void WriteObjInfoStream(OleLinkType linkType, OleObjectType objType)
  {
    MemoryStream memoryStream = new MemoryStream();
    new ObjectInfoStream().SaveTo((Stream) memoryStream, linkType, objType);
    memoryStream.Flush();
    memoryStream.Position = 0L;
    this.Storage.Streams.Add("\u0003ObjInfo", (Stream) memoryStream);
  }

  private void WritePackage(byte[] nativeData, string dataPath)
  {
    Encoding encoding = (Encoding) new ASCIIEncoding();
    string fileName = Path.GetFileName(dataPath);
    byte[] bytes1 = encoding.GetBytes(fileName);
    byte[] bytes2 = encoding.GetBytes(dataPath);
    byte[] bytes3 = new byte[2]{ (byte) 2, (byte) 0 };
    byte[] bytes4 = new byte[4]
    {
      (byte) 0,
      (byte) 0,
      (byte) 3,
      (byte) 0
    };
    int length = 4 + bytes3.Length + (bytes1.Length + 1) + (bytes2.Length + 1) + bytes4.Length + 4 + (bytes2.Length + 1) + 4 + nativeData.Length + 2;
    int iOffset1 = 0;
    byte[] numArray = new byte[length];
    ByteConverter.WriteInt32(numArray, ref iOffset1, length - 4);
    ByteConverter.WriteBytes(numArray, ref iOffset1, bytes3);
    ByteConverter.WriteBytes(numArray, ref iOffset1, bytes1);
    int iOffset2 = iOffset1 + 1;
    ByteConverter.WriteBytes(numArray, ref iOffset2, bytes2);
    int iOffset3 = iOffset2 + 1;
    ByteConverter.WriteBytes(numArray, ref iOffset3, bytes4);
    ByteConverter.WriteInt32(numArray, ref iOffset3, bytes2.Length + 1);
    ByteConverter.WriteBytes(numArray, ref iOffset3, bytes2);
    int iOffset4 = iOffset3 + 1;
    ByteConverter.WriteInt32(numArray, ref iOffset4, nativeData.Length);
    ByteConverter.WriteBytes(numArray, ref iOffset4, nativeData);
    this.Storage.Streams.Add("\u0001Ole10Native", (Stream) new MemoryStream(numArray));
  }

  internal Syncfusion.DocIO.ReaderWriter.DataStreamParser.OLEObject.OLEObject Clone()
  {
    return new Syncfusion.DocIO.ReaderWriter.DataStreamParser.OLEObject.OLEObject()
    {
      m_guid = this.m_guid,
      m_oleType = this.m_oleType,
      m_storage = this.m_storage.Clone()
    };
  }

  internal void Close() => this.m_storage.Close();

  internal void SetStorage(Storage storage) => this.m_storage = storage;
}
