// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Drawing.OleObject
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.CompoundFile.Presentation;
using Syncfusion.CompoundFile.Presentation.Net;
using Syncfusion.Presentation.SlideImplementation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Presentation.Drawing;

internal class OleObject : Shape, IOleObject, IShape, ISlideItem
{
  private const string DEF_OLE_STREAM_NAME = "\u0001Ole";
  private const string DEF_CONTENT_STREAM_NAME = "CONTENTS";
  private const string DEF_WP_STREAM_NAME = "Contents";
  private const string DEF_INFO_STREAM_NAME = "\u0003ObjInfo";
  internal const string DEF_COMP_STREAM_NAME = "\u0001CompObj";
  private const string DEF_NATIVE_STREAM_NAME = "\u0001Ole10Native";
  private const string DEF_EQUATION_STREAM_NAME = "Equation Native";
  private const string DEF_WORKBOOK_STREAM_NAME = "Workbook";
  private const string DEF_PACKAGE_STREAM_NAME = "Package";
  private const string DEF_ODP_STREAM_NAME = "EmbeddedOdf";
  internal OleLinkType _linkType;
  internal string _linkPath;
  private string _strObjectType;
  private string _name;
  private int _imageWidth = -1;
  private BaseSlide _baseSlide;
  private int _imageHeight = -1;
  private string _relationId;
  private OleObjectType _oleObjectType;
  private Picture _olePicture;
  private Stream _oleStream;
  private Stream _nativeStream;
  private VmlShape _vmlShape;
  private bool _displayAsIcon;
  private string _oleExtension;
  private string _vmlShapeId;
  private string _fileName = string.Empty;

  internal OleObject(BaseSlide baseSlide)
    : base(ShapeType.GraphicFrame, baseSlide)
  {
    this.DrawingType = DrawingType.OleObject;
    this._baseSlide = baseSlide;
  }

  public string ProgID
  {
    get
    {
      return this._strObjectType != null ? this._strObjectType : Helper.ToString(this.OleObjectType, false);
    }
    set
    {
      this._oleObjectType = Helper.ToOleType(value);
      this._strObjectType = Helper.ToString(this._oleObjectType, false);
      if (!(this._strObjectType == string.Empty) || !(value != string.Empty))
        return;
      this._strObjectType = value;
    }
  }

  public byte[] ImageData => this.OlePicture != null ? this.OlePicture.ImageData : (byte[]) null;

  public string LinkPath => this.LinkType == OleLinkType.Link ? this._linkPath : (string) null;

  public string FileName
  {
    get
    {
      if (this.LinkType == OleLinkType.Link)
        return Helper.GetFileName(this._linkPath);
      if (this._fileName.Contains(".bin") && this._nativeStream == null)
        this.UpdateObjectData();
      return this._fileName;
    }
  }

  public byte[] ObjectData
  {
    get
    {
      if (this.LinkType == OleLinkType.Embed && this.OleStream != null && this._nativeStream == null)
      {
        this.UpdateObjectData();
        return (this._nativeStream as MemoryStream).ToArray();
      }
      return this._nativeStream != null && this._nativeStream.Length != 0L ? (this._nativeStream as MemoryStream).ToArray() : (byte[]) null;
    }
  }

  public bool DisplayAsIcon
  {
    get => this._displayAsIcon;
    set => this._displayAsIcon = value;
  }

  internal VmlShape VmlShape
  {
    get => this._vmlShape;
    set => this._vmlShape = value;
  }

  internal Picture OlePicture
  {
    get
    {
      if (this._olePicture != null && this._olePicture.LinkId == null)
        this._olePicture.LinkId = "";
      return this._olePicture;
    }
  }

  internal Stream OleStream
  {
    get => this._oleStream;
    set => this._oleStream = value;
  }

  internal string RelationId
  {
    get => this._relationId;
    set => this._relationId = value;
  }

  internal string Name
  {
    get => this._name;
    set => this._name = value;
  }

  internal int ImageWidth
  {
    get => this._imageWidth;
    set => this._imageWidth = value;
  }

  internal int ImageHeight
  {
    get => this._imageHeight;
    set => this._imageHeight = value;
  }

  internal OleObjectType OleObjectType
  {
    get => this._oleObjectType;
    set => this._oleObjectType = value;
  }

  internal OleLinkType LinkType => this._linkType;

  internal string OleExtension
  {
    get => this._oleExtension;
    set => this._oleExtension = value;
  }

  internal string VMLShapeId
  {
    get => this._vmlShapeId;
    set => this._vmlShapeId = value;
  }

  internal Stream CreateBinaryOLEStream(Stream oleStream, string oleName)
  {
    byte[] numArray = new byte[oleStream.Length];
    oleStream.Read(numArray, 0, numArray.Length);
    oleStream.Position = 0L;
    if (Syncfusion.CompoundFile.Presentation.Net.CompoundFile.CheckHeader(oleStream))
      return oleStream;
    string dataPath = "Package." + Helper.GetOleExtension(oleName);
    Storage storage = this.SaveToStorage(numArray, dataPath);
    Syncfusion.CompoundFile.Presentation.Net.CompoundFile cmpFile = new Syncfusion.CompoundFile.Presentation.Net.CompoundFile();
    this.UpdateGuid(cmpFile, 0);
    storage.WriteToStorage(cmpFile.RootStorage);
    cmpFile.Flush();
    storage.Close();
    MemoryStream binaryOleStream = new MemoryStream();
    cmpFile.Save((Stream) binaryOleStream);
    cmpFile.Dispose();
    binaryOleStream.Position = 0L;
    return (Stream) binaryOleStream;
  }

  internal void UpdateGuid(Syncfusion.CompoundFile.Presentation.Net.CompoundFile cmpFile, int index)
  {
    for (int index1 = index; index1 < cmpFile.Directory.Entries.Count; ++index1)
    {
      DirectoryEntry entry = cmpFile.Directory.Entries[index1];
      if (entry.Name == "Root Entry")
        entry.StorageGuid = OleTypeConvertor.GetGUID(OleObjectType.Package);
    }
  }

  private Storage SaveToStorage(byte[] nativeData, string dataPath)
  {
    Storage oleStorage = new Storage("Ole");
    this.WriteOleStream(this.LinkType, OleObjectType.Package, dataPath, oleStorage);
    this.WriteObjInfoStream(this.LinkType, OleObjectType.Package, oleStorage);
    if (this.LinkType == OleLinkType.Embed)
    {
      this.WriteCompObjStream(OleObjectType.Package, oleStorage);
      this.WriteNativeData(nativeData, dataPath, OleObjectType.Package, oleStorage);
    }
    return oleStorage;
  }

  private void WriteOleStream(
    OleLinkType linkType,
    OleObjectType objType,
    string dataPath,
    Storage oleStorage)
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
        oleStorage.Streams.Add("\u0001Ole", (Stream) memoryStream);
        break;
    }
  }

  private void WriteObjInfoStream(OleLinkType linkType, OleObjectType objType, Storage oleStorage)
  {
    MemoryStream memoryStream = new MemoryStream();
    new ObjectInfoStream().SaveTo((Stream) memoryStream, linkType, objType);
    memoryStream.Flush();
    memoryStream.Position = 0L;
    oleStorage.Streams.Add("\u0003ObjInfo", (Stream) memoryStream);
  }

  private void WriteCompObjStream(OleObjectType objType, Storage oleStorage)
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
        if (oleStorage.Streams.ContainsKey("\u0001CompObj"))
          break;
        MemoryStream memoryStream = new MemoryStream();
        new CompObjectStream(objType).SaveTo((Stream) memoryStream);
        memoryStream.Flush();
        memoryStream.Position = 0L;
        oleStorage.Streams.Add("\u0001CompObj", (Stream) memoryStream);
        break;
    }
  }

  private void WriteNativeData(
    byte[] nativeData,
    string dataPath,
    OleObjectType objType,
    Storage oleStorage)
  {
    switch (objType)
    {
      case OleObjectType.AdobeAcrobatDocument:
        this.WriteNativeData(nativeData, "CONTENTS", oleStorage);
        break;
      case OleObjectType.BitmapImage:
        this.WritePBrush(nativeData, oleStorage);
        break;
      case OleObjectType.Equation:
        this.WriteNativeData(nativeData, "Equation Native", oleStorage);
        break;
      case OleObjectType.GraphChart:
        this.WriteNativeData(nativeData, "Workbook", oleStorage);
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
        this.WriteNativeStreams((Stream) new MemoryStream(nativeData), oleStorage);
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
        this.WriteNativeData(nativeData, "Package", oleStorage);
        break;
      case OleObjectType.OpenDocumentPresentation:
      case OleObjectType.OpenDocumentSpreadsheet:
        this.WriteNativeData(nativeData, "EmbeddedOdf", oleStorage);
        break;
      case OleObjectType.Package:
        this.WritePackage(nativeData, dataPath, oleStorage);
        break;
      case OleObjectType.WordPadDocument:
        this.WriteNativeData(nativeData, "Contents", oleStorage);
        break;
    }
  }

  private void WriteNativeData(byte[] nativeData, string streamName, Storage oleStorage)
  {
    MemoryStream memoryStream = new MemoryStream(nativeData);
    memoryStream.Position = 0L;
    oleStorage.Streams.Add(streamName, (Stream) memoryStream);
  }

  private void WritePBrush(byte[] nativeData, Storage oleStorage)
  {
    int iOffset = 0;
    byte[] numArray = new byte[nativeData.Length + 4];
    ByteConverter.WriteInt32(numArray, ref iOffset, nativeData.Length);
    ByteConverter.WriteBytes(numArray, ref iOffset, nativeData);
    MemoryStream memoryStream = new MemoryStream(numArray);
    memoryStream.Position = 0L;
    oleStorage.Streams.Add("\u0001Ole10Native", (Stream) memoryStream);
  }

  private void WriteNativeStreams(Stream stream, Storage oleStorage)
  {
    Syncfusion.CompoundFile.Presentation.Net.CompoundFile compoundFile = new Syncfusion.CompoundFile.Presentation.Net.CompoundFile(stream);
    string[] streams = compoundFile.RootStorage.Streams;
    int index = 0;
    for (int length = streams.Length; index < length; ++index)
    {
      CompoundStream compoundStream = compoundFile.RootStorage.OpenStream(streams[index]);
      byte[] buffer = new byte[compoundStream.Length];
      compoundStream.Read(buffer, 0, buffer.Length);
      compoundStream.Dispose();
      oleStorage.Streams.Add(streams[index], (Stream) new MemoryStream(buffer));
    }
    compoundFile.Dispose();
  }

  private void WritePackage(byte[] nativeData, string dataPath, Storage oleStorage)
  {
    Encoding encoding = (Encoding) new UTF8Encoding();
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
    oleStorage.Streams.Add("\u0001Ole10Native", (Stream) new MemoryStream(numArray));
  }

  internal void UpdateObjectData()
  {
    this.OleStream.Position = 0L;
    this._nativeStream = (Stream) new MemoryStream();
    Picture.CopyStream(this.OleStream, this._nativeStream);
    this._nativeStream.Position = 0L;
    if (!Syncfusion.CompoundFile.Presentation.Net.CompoundFile.CheckHeader(this._nativeStream))
      return;
    Syncfusion.CompoundFile.Presentation.Net.CompoundFile compoundFile = new Syncfusion.CompoundFile.Presentation.Net.CompoundFile(this._nativeStream);
    List<string> stringList = new List<string>();
    foreach (string stream in compoundFile.RootStorage.Streams)
      stringList.Add(stream);
    if (this.OleObjectType == OleObjectType.AdobeAcrobatDocument && stringList.Contains("CONTENTS"))
    {
      this._nativeStream = (Stream) new MemoryStream(this.GetDataFromCompoundStorage(compoundFile.RootStorage, "CONTENTS"));
      this._fileName = this.GetDefaultOlePackageName(this.OleObjectType);
    }
    else if (stringList.Contains("Package"))
    {
      this._nativeStream = (Stream) new MemoryStream(this.GetDataFromCompoundStorage(compoundFile.RootStorage, "Package"));
      this._fileName = this.GetDefaultOlePackageName(this.OleObjectType);
    }
    else
    {
      if (!stringList.Contains("\u0001Ole10Native"))
        return;
      byte[] fromCompoundStorage = this.GetDataFromCompoundStorage(compoundFile.RootStorage, "\u0001Ole10Native");
      Ole10NativeParser ole10NativeParser = new Ole10NativeParser(fromCompoundStorage);
      this._fileName = ole10NativeParser.FileName;
      if (ole10NativeParser.NativeData != null)
        this._nativeStream = (Stream) new MemoryStream(ole10NativeParser.NativeData);
      else
        this._nativeStream = (Stream) new MemoryStream(fromCompoundStorage);
    }
  }

  internal byte[] GetDataFromCompoundStorage(ICompoundStorage storage, string streamName)
  {
    CompoundStream compoundStream = storage.OpenStream(streamName);
    byte[] buffer = new byte[compoundStream.Length];
    compoundStream.Read(buffer, 0, buffer.Length);
    compoundStream.Dispose();
    return buffer;
  }

  internal static void CopyStream(Stream input, Stream output)
  {
    if (input == null)
      throw new ArgumentNullException(nameof (input));
    if (output == null)
      throw new ArgumentNullException(nameof (output));
    input.Position = 0L;
    input.CopyTo(output);
  }

  internal string GetDefaultOlePackageName(OleObjectType objectType)
  {
    switch (objectType)
    {
      case OleObjectType.AdobeAcrobatDocument:
        return "Adobe_Acrobat_Document.pdf";
      case OleObjectType.ExcelWorksheet:
        return "Microsoft_Excel_Worksheet.xlsx";
      case OleObjectType.PowerPointPresentation:
        return "Microsoft_PowerPoint_Presentation.pptx";
      case OleObjectType.WordDocument:
        return "Microsoft_Word_Document.docx";
      default:
        return this._fileName;
    }
  }

  internal void SetLinkType(OleLinkType type) => this._linkType = type;

  internal void SetLinkPath(string path) => this._linkPath = path.Replace("file:///", string.Empty);

  internal void SetFileName(string fileName) => this._fileName = fileName;

  internal void SetOlePicture(Picture picture) => this._olePicture = picture;

  internal void GetOlePicture(Stream pictureStream)
  {
    Picture picture = new Picture(this._baseSlide);
    picture.SetSlideItemType(SlideItemType.Picture);
    picture.IsPresetGeometry = true;
    picture.DrawingType = DrawingType.None;
    picture.AddImageStream(pictureStream);
    picture.IsEmbed = true;
    picture.ShapeName = "Picture " + (object) picture.ShapeId;
    this._olePicture = picture;
  }

  public override ISlideItem Clone()
  {
    OleObject oleObject = (OleObject) this.MemberwiseClone();
    this.Clone((Shape) oleObject);
    if (this._olePicture != null)
      oleObject._olePicture = (Picture) this._olePicture.Clone();
    if (this._oleStream != null)
    {
      oleObject._oleStream = (Stream) new MemoryStream();
      OleObject.CopyStream(this._oleStream, oleObject._oleStream);
    }
    if (oleObject._vmlShape != null)
      oleObject._vmlShape = this._vmlShape.Clone();
    if (oleObject.RelationId != null)
      oleObject.RelationId = (string) null;
    return (ISlideItem) oleObject;
  }

  internal override void Close()
  {
    base.Close();
    if (this._oleStream != null)
      this._oleStream.Dispose();
    if (this._nativeStream != null)
      this._nativeStream.Dispose();
    if (this._olePicture == null)
      return;
    this._olePicture.Close();
  }

  internal override void SetParent(BaseSlide newParent)
  {
    base.SetParent(newParent);
    if (this._olePicture == null)
      return;
    this._olePicture.SetParent(newParent);
  }
}
