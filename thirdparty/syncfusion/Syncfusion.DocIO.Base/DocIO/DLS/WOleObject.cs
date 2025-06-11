// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WOleObject
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.CompoundFile.DocIO;
using Syncfusion.CompoundFile.DocIO.Net;
using Syncfusion.DocIO.ReaderWriter.DataStreamParser.OLEObject;
using Syncfusion.DocIO.Rendering;
using Syncfusion.Layouting;
using System;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WOleObject : ParagraphItem, ILeafWidget, IWidget
{
  private const string DEF_OBJECT_POOL_NAME = "ObjectPool";
  private const string DEF_INFO_STREAM_NAME = "\u0003ObjInfo";
  private const int DEF_STRUCT_SIZE = 6;
  internal WPicture m_picture;
  private WField m_field;
  private string m_oleStorageName;
  private string m_linkAddress;
  private string m_strObjType;
  private OleObjectType m_oleObjType;
  internal OleLinkType m_linkType;
  private XmlParagraphItem m_oleXmlItem;
  private Syncfusion.DocIO.ReaderWriter.DataStreamParser.OLEObject.OLEObject m_oleObject;
  private static Random m_oleRandomIdGen;
  private string m_packageFileName = string.Empty;
  private byte m_bFlags = 1;

  public bool DisplayAsIcon
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set
    {
      this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
      if (this.Document.IsOpening)
        return;
      this.UpdateOleObjInfoStream();
    }
  }

  public WPicture OlePicture => this.m_picture;

  public override EntityType EntityType => EntityType.OleObject;

  public Stream Container => this.GetOleContainer();

  internal WField Field
  {
    get
    {
      if (this.m_field == null)
        this.m_field = new WField((IWordDocument) this.m_doc);
      if (this.m_field.FieldType == FieldType.FieldNone)
        this.SetFieldType();
      if (this.Owner != this)
        this.m_field.SetOwner((OwnerHolder) this);
      return this.m_field;
    }
    set => this.m_field = value;
  }

  public string OleStorageName
  {
    get => this.m_oleStorageName;
    set
    {
      if (string.IsNullOrEmpty(value))
        value = new Random().Next().ToString();
      if (this.m_doc != null && !this.m_doc.IsOpening && this.m_doc.OleObjectCollection.ContainsKey(this.m_oleStorageName) && value != this.m_oleStorageName && this.m_doc.OleObjectCollection[this.m_oleStorageName].OccurrenceCount != 0)
      {
        --this.m_doc.OleObjectCollection[this.m_oleStorageName].OccurrenceCount;
        this.OleObject.SetStorage(this.m_doc.OleObjectCollection[this.m_oleStorageName].Clone());
        this.m_doc.OleObjectCollection.Add(value, this.OleObject.Storage);
        this.m_doc.OleObjectCollection[value].Guid = this.Guid;
      }
      this.m_oleStorageName = value;
    }
  }

  public string LinkPath
  {
    get
    {
      if (string.IsNullOrEmpty(this.m_linkAddress))
        this.UpdateProps();
      return this.m_linkAddress;
    }
    set
    {
      value = Path.GetFullPath(value);
      this.m_linkAddress = value;
    }
  }

  public OleLinkType LinkType => this.m_linkType;

  internal XmlParagraphItem OleXmlItem
  {
    get => this.m_oleXmlItem;
    set => this.m_oleXmlItem = value;
  }

  internal OleObjectType OleObjectType
  {
    get
    {
      this.m_oleObjType = this.m_oleObjType == OleObjectType.Undefined ? this.OleObject.OleType : this.m_oleObjType;
      if (this.m_oleObjType == OleObjectType.Undefined)
        this.UpdateProps();
      return this.m_oleObjType;
    }
    set => this.m_oleObjType = value;
  }

  internal UpdateMode UpdateMode
  {
    get => ((int) this.m_bFlags & 2) >> 1 == 0 ? UpdateMode.Always : UpdateMode.OnCall;
    set
    {
      this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value == UpdateMode.OnCall ? 1 : 0) << 1);
    }
  }

  public string ObjectType
  {
    get
    {
      return !string.IsNullOrEmpty(this.m_strObjType) ? this.m_strObjType : OleTypeConvertor.ToString(this.OleObjectType, false);
    }
    set
    {
      if (!this.Document.IsOpening && OleTypeConvertor.ToOleType(value) != this.m_oleObjType)
      {
        if (this.OleObject.Storage.Streams.ContainsKey("\u0001CompObj"))
          this.OleObject.Storage.Streams.Remove("\u0001CompObj");
        MemoryStream memoryStream = new MemoryStream();
        new CompObjectStream(OleTypeConvertor.ToOleType(value)).SaveTo((Stream) memoryStream);
        memoryStream.Flush();
        memoryStream.Position = 0L;
        this.OleObject.Storage.Streams.Add("\u0001CompObj", (Stream) memoryStream);
      }
      this.m_oleObjType = OleTypeConvertor.ToOleType(value);
      this.m_strObjType = this.m_oleObjType != OleObjectType.Undefined ? OleTypeConvertor.ToString(this.m_oleObjType, false) : value;
    }
  }

  public byte[] NativeData
  {
    get => !this.IsEmpty ? (this.GetOlePartStream(true) as MemoryStream).ToArray() : (byte[]) null;
  }

  private Syncfusion.DocIO.ReaderWriter.DataStreamParser.OLEObject.OLEObject OleObject
  {
    get
    {
      if (this.m_oleObject == null)
        this.m_oleObject = new Syncfusion.DocIO.ReaderWriter.DataStreamParser.OLEObject.OLEObject();
      return this.m_oleObject;
    }
  }

  internal static int NextOleObjId
  {
    get
    {
      if (WOleObject.m_oleRandomIdGen == null)
        WOleObject.m_oleRandomIdGen = new Random(new DateTime().Millisecond);
      return WOleObject.m_oleRandomIdGen.Next();
    }
  }

  public string PackageFileName => this.m_packageFileName;

  internal bool IsEmpty => this.OleObject.Storage.Streams.Count == 0;

  internal Guid Guid
  {
    get
    {
      Guid guid = this.OleObject.Guid;
      return this.OleObject.Guid == Guid.Empty ? OleTypeConvertor.GetGUID(this.OleObjectType) : this.OleObject.Guid;
    }
  }

  public WOleObject(WordDocument doc)
    : base(doc)
  {
    this.m_oleStorageName = string.Empty;
    this.m_linkAddress = string.Empty;
  }

  internal void AddFieldCodeText()
  {
    WTextRange wtextRange = new WTextRange((IWordDocument) this.m_doc);
    if (this.LinkType == OleLinkType.Embed)
      wtextRange.Text = "EMBED " + this.ObjectType;
    else
      wtextRange.Text = $"LINK {this.ObjectType} \"{this.LinkPath.Replace("\\", "\\\\")}\"";
    this.OwnerParagraph.Items.Add((IEntity) wtextRange);
  }

  internal void SetLinkPathValue(string path) => this.m_linkAddress = path;

  internal void ParseObjectPool(Stream objectPoolStream)
  {
    this.OleObject.Storage.Streams.Clear();
    using (Syncfusion.CompoundFile.DocIO.Net.CompoundFile compoundFile = new Syncfusion.CompoundFile.DocIO.Net.CompoundFile(objectPoolStream))
    {
      foreach (DirectoryEntry entry in compoundFile.Directory.Entries)
      {
        if ("_" + this.OleStorageName == entry.Name)
        {
          this.OleObject.Guid = entry.StorageGuid;
          break;
        }
      }
      if (Array.IndexOf<string>(compoundFile.RootStorage.Storages, "ObjectPool") != -1)
      {
        if (Array.IndexOf<string>(compoundFile.RootStorage.OpenStorage("ObjectPool").Storages, "_" + this.OleStorageName) != -1)
        {
          ICompoundStorage storage = compoundFile.RootStorage.OpenStorage("ObjectPool").OpenStorage("_" + this.OleStorageName);
          this.ParseStreams(storage);
          this.OleObject.Storage.ParseStorages(storage);
          this.CheckObjectInfoStream();
          this.UpdateStorageName();
        }
      }
    }
    objectPoolStream.Position = 0L;
  }

  internal void UpdateStorageName()
  {
    if (this.Document == null)
      return;
    string collection = this.OleObject.AddOleObjectToCollection(this.Document.OleObjectCollection, this.OleStorageName);
    if (string.IsNullOrEmpty(collection))
      return;
    this.m_oleStorageName = collection;
  }

  internal void ParseOlePartStream(Stream stream)
  {
    this.OleObject.Storage.Streams.Clear();
    if (Syncfusion.CompoundFile.DocIO.Net.CompoundFile.CheckHeader(stream))
    {
      Syncfusion.CompoundFile.DocIO.Net.CompoundFile compoundFile = new Syncfusion.CompoundFile.DocIO.Net.CompoundFile(stream);
      foreach (DirectoryEntry entry in compoundFile.Directory.Entries)
      {
        if (compoundFile.RootStorage.Name == entry.Name)
        {
          this.OleObject.Guid = entry.StorageGuid;
          break;
        }
      }
      this.ParseStreams(compoundFile.RootStorage);
      this.OleObject.Storage.ParseStorages(compoundFile.RootStorage);
      this.CheckObjectInfoStream();
      this.UpdateStorageName();
      compoundFile.Dispose();
    }
    else
    {
      this.OleObject.Save((Stream) new MemoryStream((stream as MemoryStream).ToArray()), this);
      if (this.m_doc == null || this.m_doc.OleObjectCollection.ContainsKey(this.OleStorageName))
        return;
      this.m_doc.OleObjectCollection.Add(this.OleStorageName, this.OleObject.Storage);
      this.m_doc.OleObjectCollection[this.OleStorageName].Guid = this.Guid;
      this.OleObject.Guid = this.Guid;
    }
  }

  private void ParseStreams(ICompoundStorage storage)
  {
    foreach (string stream in storage.Streams)
    {
      CompoundStream compoundStream = storage.OpenStream(stream);
      byte[] buffer = new byte[compoundStream.Length];
      compoundStream.Read(buffer, 0, buffer.Length);
      compoundStream.Dispose();
      if (stream == "\u0003ObjInfo")
      {
        this.DisplayAsIcon = ((int) buffer[0] & 64 /*0x40*/) == 64 /*0x40*/;
        if (((int) buffer[0] & 16 /*0x10*/) == 16 /*0x10*/)
          this.UpdateMode = ((int) buffer[1] & 1) == 1 ? UpdateMode.OnCall : UpdateMode.Always;
      }
      if (this.OleObject.Storage.Streams.ContainsKey(stream))
        this.OleObject.Storage.Streams.Remove(stream);
      this.OleObject.Storage.Streams.Add(stream, (Stream) new MemoryStream(buffer));
    }
  }

  internal void ParseOleStream(Stream stream)
  {
    if (Syncfusion.CompoundFile.DocIO.Net.CompoundFile.CheckHeader(stream))
    {
      Syncfusion.CompoundFile.DocIO.Net.CompoundFile compoundFile = new Syncfusion.CompoundFile.DocIO.Net.CompoundFile(stream);
      string s = string.Empty;
      if (compoundFile.RootStorage.Storages.Length > 0)
        s = compoundFile.RootStorage.Storages[0].Replace("_", string.Empty);
      int result;
      if (int.TryParse(s, out result))
      {
        this.m_oleStorageName = result.ToString();
        foreach (DirectoryEntry entry in compoundFile.Directory.Entries)
        {
          if (entry.Name == "_" + s)
          {
            this.OleObject.Guid = entry.StorageGuid;
            break;
          }
        }
        ICompoundStorage storage = compoundFile.RootStorage.OpenStorage("_" + s);
        this.ParseStreams(storage);
        this.OleObject.Storage.ParseStorages(storage);
        this.CheckObjectInfoStream();
        this.UpdateStorageName();
        storage.Dispose();
      }
      else
      {
        this.m_oleStorageName = WOleObject.NextOleObjId.ToString();
        this.ParseStreams(compoundFile.RootStorage);
        this.OleObject.Storage.ParseStorages(compoundFile.RootStorage);
        this.CheckObjectInfoStream();
        this.UpdateStorageName();
      }
      compoundFile.Dispose();
    }
    else
    {
      this.m_oleStorageName = WOleObject.NextOleObjId.ToString();
      byte[] buffer = new byte[(int) stream.Length];
      stream.Read(buffer, 0, (int) stream.Length);
      this.OleObject.Save((Stream) new MemoryStream(buffer), this);
      if (this.m_doc == null || this.m_doc.OleObjectCollection.ContainsKey(this.OleStorageName))
        return;
      this.m_doc.OleObjectCollection.Add(this.OleStorageName, this.OleObject.Storage);
      this.m_doc.OleObjectCollection[this.OleStorageName].Guid = this.Guid;
      this.OleObject.Guid = this.Guid;
    }
  }

  private void CheckObjectInfoStream()
  {
    if (this.OleObject.Storage.Streams.ContainsKey("\u0003ObjInfo"))
      return;
    this.OleObject.Storage.Streams.Add("\u0003ObjInfo", (Stream) new MemoryStream(this.UpdateObjInfoBytes()));
  }

  internal void CreateOleObjContainer(byte[] nativeData, string dataPath)
  {
    dataPath = dataPath == null ? string.Empty : dataPath;
    this.m_oleStorageName = WOleObject.NextOleObjId.ToString();
    this.m_packageFileName = dataPath;
    this.m_oleObject = new Syncfusion.DocIO.ReaderWriter.DataStreamParser.OLEObject.OLEObject();
    MemoryStream memoryStream = new MemoryStream(nativeData);
    if (Syncfusion.CompoundFile.DocIO.Net.CompoundFile.CheckHeader((Stream) memoryStream))
    {
      this.ParseOleStream((Stream) memoryStream);
    }
    else
    {
      this.m_oleObject.Save(nativeData, dataPath, this);
      if (this.m_doc != null && !this.m_doc.OleObjectCollection.ContainsKey(this.OleStorageName))
      {
        this.m_doc.OleObjectCollection.Add(this.OleStorageName, this.OleObject.Storage);
        this.m_doc.OleObjectCollection[this.OleStorageName].Guid = this.Guid;
        this.m_oleObject.Guid = this.Guid;
      }
    }
    this.OleObjectType = this.m_oleObject.OleType;
    this.m_strObjType = OleTypeConvertor.ToString(this.OleObjectType, false);
    if (this.DisplayAsIcon)
      return;
    this.UpdateOleObjInfoStream();
  }

  internal Stream GetOlePartStream(bool isNativeData)
  {
    if (this.IsNativeItem() && this.OleObject.Storage.Streams.ContainsKey("Package"))
      return (Stream) new MemoryStream((this.OleObject.Storage.Streams["Package"] as MemoryStream).ToArray());
    if (this.OleObject.OleType == OleObjectType.AdobeAcrobatDocument && isNativeData && this.OleObject.Storage.Streams.ContainsKey("CONTENTS"))
      return (Stream) new MemoryStream((this.OleObject.Storage.Streams["CONTENTS"] as MemoryStream).ToArray());
    Syncfusion.CompoundFile.DocIO.Net.CompoundFile cmpFile = new Syncfusion.CompoundFile.DocIO.Net.CompoundFile();
    this.UpdateGuid(cmpFile, 0);
    this.OleObject.Storage.WriteToStorage(cmpFile.RootStorage);
    if (this.IsNativeItem())
    {
      if (Array.IndexOf<string>(cmpFile.RootStorage.Streams, "\u0003ObjInfo") != -1)
        cmpFile.RootStorage.DeleteStream("\u0003ObjInfo");
      if (Array.IndexOf<string>(cmpFile.RootStorage.Streams, "\u0001Ole") != -1)
        cmpFile.RootStorage.DeleteStream("\u0001Ole");
      if (Array.IndexOf<string>(cmpFile.RootStorage.Streams, "\u0001CompObj") != -1)
        cmpFile.RootStorage.DeleteStream("\u0001CompObj");
      if (Array.IndexOf<string>(cmpFile.RootStorage.Streams, "\u0003LinkInfo") != -1)
        cmpFile.RootStorage.DeleteStream("\u0003LinkInfo");
      if (Array.IndexOf<string>(cmpFile.RootStorage.Streams, "\u0003EPRINT") != -1)
        cmpFile.RootStorage.DeleteStream("\u0003EPRINT");
    }
    cmpFile.Flush();
    MemoryStream olePartStream = new MemoryStream();
    cmpFile.Save((Stream) olePartStream);
    cmpFile.Dispose();
    olePartStream.Position = 0L;
    return (Stream) olePartStream;
  }

  private bool IsNativeItem()
  {
    switch (this.OleObjectType)
    {
      case OleObjectType.Undefined:
        return this.m_strObjType == "Visio.Drawing.15";
      case OleObjectType.Excel_97_2003_Worksheet:
      case OleObjectType.ExcelBinaryWorksheet:
      case OleObjectType.ExcelChart:
      case OleObjectType.ExcelMacroWorksheet:
      case OleObjectType.ExcelWorksheet:
      case OleObjectType.PowerPoint_97_2003_Presentation:
      case OleObjectType.PowerPointMacroPresentation:
      case OleObjectType.PowerPointMacroSlide:
      case OleObjectType.PowerPointPresentation:
      case OleObjectType.PowerPointSlide:
      case OleObjectType.Word_97_2003_Document:
      case OleObjectType.WordDocument:
      case OleObjectType.WordMacroDocument:
        return true;
      default:
        return false;
    }
  }

  internal void WriteToStorage(ICompoundStorage storage)
  {
    this.OleObject.Storage.WriteToStorage(storage);
  }

  internal void UpdateGuid(Syncfusion.CompoundFile.DocIO.Net.CompoundFile cmpFile, int index)
  {
    for (int index1 = index; index1 < cmpFile.Directory.Entries.Count; ++index1)
    {
      DirectoryEntry entry = cmpFile.Directory.Entries[index1];
      if (entry.Name == "_" + this.OleStorageName || entry.Name == "Root Entry")
        entry.StorageGuid = this.Guid;
    }
  }

  private Stream GetOleContainer()
  {
    if (this.OleObject.Storage.Streams.Count == 0)
      return (Stream) null;
    Syncfusion.CompoundFile.DocIO.Net.CompoundFile compoundFile = new Syncfusion.CompoundFile.DocIO.Net.CompoundFile();
    ICompoundStorage storage = compoundFile.RootStorage.CreateStorage("_" + this.OleStorageName);
    compoundFile.Directory.Entries[1].StorageGuid = this.OleObject.Guid;
    this.OleObject.Storage.WriteToStorage(storage);
    compoundFile.Flush();
    MemoryStream oleContainer = new MemoryStream();
    compoundFile.Save((Stream) oleContainer);
    compoundFile.Dispose();
    oleContainer.Position = 0L;
    return (Stream) oleContainer;
  }

  private void UpdateProps()
  {
    if (this.m_field == null)
      return;
    string[] strArray = this.m_field.FieldValue.Split('"');
    if (this.m_oleObjType == OleObjectType.Undefined)
      this.m_oleObjType = OleTypeConvertor.ToOleType(strArray[0].Trim());
    if (!string.IsNullOrEmpty(this.m_linkAddress) || strArray.Length <= 1)
      return;
    this.m_linkAddress = strArray[1];
  }

  internal void SetOlePicture(WPicture picture) => this.m_picture = picture;

  internal void SetLinkType(OleLinkType type) => this.m_linkType = type;

  internal void SetFieldType()
  {
    if (this.m_linkType == OleLinkType.Link)
      this.m_field.FieldType = FieldType.FieldLink;
    else
      this.m_field.FieldType = FieldType.FieldEmbed;
  }

  private void UpdateOleObjInfoStream()
  {
    if (this.m_doc.OleObjectCollection.ContainsKey(this.m_oleStorageName))
      this.OleStorageName = new Random().Next().ToString();
    if (!this.OleObject.Storage.Streams.ContainsKey("\u0003ObjInfo"))
      return;
    byte[] buffer = this.UpdateObjInfoBytes();
    Stream stream = this.OleObject.Storage.Streams["\u0003ObjInfo"];
    stream.Position = 0L;
    stream.Write(buffer, 0, buffer.Length);
    stream.Flush();
  }

  private byte[] UpdateObjInfoBytes()
  {
    byte[] numArray = new byte[6];
    switch (this.OleObjectType)
    {
      case OleObjectType.Undefined:
      case OleObjectType.WordPadDocument:
        if (this.LinkType == OleLinkType.Embed)
        {
          numArray = new byte[6]
          {
            (byte) 0,
            (byte) 0,
            (byte) 3,
            (byte) 0,
            (byte) 4,
            (byte) 0
          };
          break;
        }
        numArray = new byte[6]
        {
          (byte) 16 /*0x10*/,
          (byte) 2,
          (byte) 3,
          (byte) 0,
          (byte) 13,
          (byte) 0
        };
        break;
      case OleObjectType.AdobeAcrobatDocument:
      case OleObjectType.Excel_97_2003_Worksheet:
      case OleObjectType.ExcelBinaryWorksheet:
      case OleObjectType.ExcelMacroWorksheet:
      case OleObjectType.ExcelWorksheet:
      case OleObjectType.PowerPoint_97_2003_Presentation:
      case OleObjectType.PowerPointMacroPresentation:
      case OleObjectType.PowerPointMacroSlide:
      case OleObjectType.PowerPointPresentation:
      case OleObjectType.PowerPointSlide:
      case OleObjectType.WordMacroDocument:
      case OleObjectType.VisioDrawing:
      case OleObjectType.OpenDocumentPresentation:
      case OleObjectType.OpenDocumentSpreadsheet:
      case OleObjectType.OpenOfficeSpreadsheet1_1:
      case OleObjectType.OpenOfficeText_1_1:
      case OleObjectType.OpenOfficeSpreadsheet:
      case OleObjectType.OpenOfficeText:
        if (this.LinkType == OleLinkType.Embed)
        {
          if (this.DisplayAsIcon)
          {
            numArray = new byte[6]
            {
              (byte) 64 /*0x40*/,
              (byte) 0,
              (byte) 3,
              (byte) 0,
              (byte) 4,
              (byte) 0
            };
            break;
          }
          numArray = new byte[6]
          {
            (byte) 0,
            (byte) 0,
            (byte) 3,
            (byte) 0,
            (byte) 13,
            (byte) 0
          };
          break;
        }
        numArray = new byte[6]
        {
          (byte) 16 /*0x10*/,
          (byte) 0,
          (byte) 3,
          (byte) 0,
          (byte) 13,
          (byte) 0
        };
        break;
      case OleObjectType.BitmapImage:
      case OleObjectType.MIDISequence:
      case OleObjectType.VideoClip:
        if (this.LinkType == OleLinkType.Embed)
        {
          numArray = new byte[6]
          {
            (byte) 0,
            (byte) 0,
            (byte) 3,
            (byte) 0,
            (byte) 4,
            (byte) 0
          };
          break;
        }
        numArray = new byte[6]
        {
          (byte) 16 /*0x10*/,
          (byte) 0,
          (byte) 3,
          (byte) 0,
          (byte) 4,
          (byte) 0
        };
        break;
      case OleObjectType.MediaClip:
      case OleObjectType.Package:
      case OleObjectType.WaveSound:
        numArray = new byte[6]
        {
          (byte) 64 /*0x40*/,
          (byte) 0,
          (byte) 3,
          (byte) 0,
          (byte) 4,
          (byte) 0
        };
        break;
      case OleObjectType.Equation:
        if (this.LinkType == OleLinkType.Embed)
        {
          numArray = new byte[6]
          {
            (byte) 0,
            (byte) 0,
            (byte) 3,
            (byte) 0,
            (byte) 4,
            (byte) 0
          };
          break;
        }
        break;
      case OleObjectType.GraphChart:
      case OleObjectType.ExcelChart:
        if (this.LinkType == OleLinkType.Embed)
        {
          numArray = new byte[6]
          {
            (byte) 0,
            (byte) 2,
            (byte) 3,
            (byte) 0,
            (byte) 13,
            (byte) 0
          };
          break;
        }
        numArray = new byte[6]
        {
          (byte) 16 /*0x10*/,
          (byte) 0,
          (byte) 3,
          (byte) 0,
          (byte) 13,
          (byte) 0
        };
        break;
      case OleObjectType.PowerPoint_97_2003_Slide:
      case OleObjectType.WordDocument:
        if (this.LinkType == OleLinkType.Embed)
        {
          numArray = new byte[6]
          {
            (byte) 0,
            (byte) 0,
            (byte) 3,
            (byte) 0,
            (byte) 1,
            (byte) 0
          };
          break;
        }
        numArray = new byte[6]
        {
          (byte) 16 /*0x10*/,
          (byte) 0,
          (byte) 3,
          (byte) 0,
          (byte) 13,
          (byte) 0
        };
        break;
      case OleObjectType.Word_97_2003_Document:
        if (this.LinkType == OleLinkType.Embed)
        {
          numArray = new byte[6]
          {
            (byte) 0,
            (byte) 2,
            (byte) 3,
            (byte) 0,
            (byte) 1,
            (byte) 0
          };
          break;
        }
        numArray = new byte[6]
        {
          (byte) 16 /*0x10*/,
          (byte) 2,
          (byte) 3,
          (byte) 0,
          (byte) 13,
          (byte) 0
        };
        break;
    }
    return numArray;
  }

  protected override void CreateLayoutInfo()
  {
    this.m_layoutInfo = (ILayoutInfo) new LayoutInfo(ChildrenLayoutDirection.Horizontal);
    this.m_layoutInfo.IsSkip = true;
    if (this.Field.FieldSeparator == null)
      return;
    this.Field.SetSkipForFieldCode(this.NextSibling);
  }

  internal override void InitLayoutInfo(Entity entity, ref bool isLastTOCEntry)
  {
    this.m_layoutInfo = (ILayoutInfo) null;
    if (this.m_picture != null)
      this.m_picture.InitLayoutInfo(entity, ref isLastTOCEntry);
    if (this != entity)
      return;
    isLastTOCEntry = true;
  }

  protected override object CloneImpl()
  {
    WOleObject woleObject = (WOleObject) base.CloneImpl();
    woleObject.m_oleStorageName = this.m_oleStorageName;
    if (this.m_oleObject != null)
      woleObject.m_oleObject = this.m_oleObject.Clone();
    woleObject.m_oleObjType = this.OleObjectType;
    if (this.m_field != null)
      woleObject.Field = this.m_field.Clone() as WField;
    if (this.m_oleXmlItem != null)
      woleObject.OleXmlItem = this.m_oleXmlItem.Clone() as XmlParagraphItem;
    woleObject.IsCloned = true;
    return (object) woleObject;
  }

  internal override void AddSelf()
  {
    if (!(this.NextSibling is WFieldMark) || (this.NextSibling as WFieldMark).Type != FieldMarkType.FieldSeparator || !(this.NextSibling.NextSibling is WPicture))
      return;
    this.m_picture = this.NextSibling.NextSibling as WPicture;
  }

  internal override void AttachToParagraph(WParagraph owner, int itemPos)
  {
    base.AttachToParagraph(owner, itemPos);
    this.Document.FloatingItems.Add((Entity) this);
  }

  internal override void Detach()
  {
    if (this.Document == null)
      return;
    if (this.Document.OleObjectCollection.Count != 0 && this.Document.OleObjectCollection.ContainsKey(this.OleStorageName))
    {
      --this.Document.OleObjectCollection[this.OleStorageName].OccurrenceCount;
      if (this.Document.OleObjectCollection[this.OleStorageName].OccurrenceCount <= 0)
        this.Document.OleObjectCollection.Remove(this.OleStorageName);
    }
    this.Document.FloatingItems.Remove((Entity) this);
  }

  internal override void CloneRelationsTo(WordDocument doc, OwnerHolder nextOwner)
  {
    base.CloneRelationsTo(doc, nextOwner);
    this.OleObject.Cloned = true;
    if (doc != null)
    {
      string collection = this.OleObject.AddOleObjectToCollection(doc.OleObjectCollection, this.OleStorageName);
      if (!string.IsNullOrEmpty(collection))
        this.m_oleStorageName = collection;
    }
    this.UpdateOleItemReferences();
    this.IsCloned = false;
    this.OleObject.Cloned = false;
  }

  private void UpdateOleItemReferences()
  {
    if (this.OwnerParagraph == null)
      return;
    if (this.Field.FieldSeparator != null && this.IsValidIndex(this.Field.FieldSeparator.Index))
      this.Field.FieldSeparator = this.OwnerParagraph.Items[this.Field.FieldSeparator.Index] as WFieldMark;
    if (this.m_picture != null && this.IsValidIndex(this.m_picture.Index) && this.OwnerParagraph.Items[this.m_picture.Index] is WPicture)
      this.m_picture = this.OwnerParagraph.Items[this.m_picture.Index] as WPicture;
    if (this.Field.FieldEnd == null || !this.IsValidIndex(this.Field.FieldEnd.Index))
      return;
    this.Field.FieldEnd = this.OwnerParagraph.Items[this.Field.FieldEnd.Index] as WFieldMark;
  }

  private bool IsValidIndex(int index) => index >= 0 && index < this.OwnerParagraph.Items.Count;

  internal override void Close()
  {
    if (this.m_picture != null)
    {
      this.m_picture.Close();
      this.m_picture = (WPicture) null;
    }
    if (this.m_oleObject != null)
    {
      this.m_oleObject.Close();
      this.m_oleObject = (Syncfusion.DocIO.ReaderWriter.DataStreamParser.OLEObject.OLEObject) null;
    }
    this.m_field = (WField) null;
    if (this.m_oleXmlItem != null)
    {
      this.m_oleXmlItem.Close();
      this.m_oleXmlItem = (XmlParagraphItem) null;
    }
    base.Close();
  }

  SizeF ILeafWidget.Measure(DrawingContext dc) => SizeF.Empty;

  void IWidget.InitLayoutInfo() => this.m_layoutInfo = (ILayoutInfo) null;

  void IWidget.InitLayoutInfo(IWidget widget)
  {
  }
}
