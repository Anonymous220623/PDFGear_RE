// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.OleObject
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.CompoundFile.XlsIO.Native;
using Syncfusion.XlsIO.Implementation.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class OleObject : IOleObject
{
  private const string DEF_OBJECT_POOL_NAME = "ObjectPool";
  private const string DEF_OLE_STREAM_NAME = "\u0001Ole";
  private const string DEF_INFO_STREAM_NAME = "\u0003ObjInfo";
  private const string DEF_COMP_STREAM_NAME = "\u0001CompObj";
  private const string DEF_NATIVE_STREAM_NAME = "\u0001Ole10Native";
  private byte[] m_fileNativeData;
  private byte[] m_file;
  private CompObjectStream m_compObjectStream;
  private OleStream m_oleStream;
  private ObjectInfoStream m_objectInfoStream;
  private Dictionary<string, int> m_location;
  private bool m_isContainer;
  private Stream m_container;
  private SizeF m_size;
  private bool m_isIcon = true;
  private DVAspect m_dvAspect;
  private ShapeImpl m_shape;
  private string m_shapeRId;
  private string m_fallbackShapeId;
  private string m_defaultSizeValue;
  private string m_moveWithCellsValue;
  private string m_sizeWithCellsValue;
  private string m_objectPrRelationId;
  private OleLinkType m_oleLinkType;
  private bool m_isStream;
  private WorksheetImpl m_sheet;
  private string m_strContentType = "application/vnd.openxmlformats-officedocument.oleObject";
  private string m_strRelationType = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/oleObject";
  private OleObjectType m_oleObjectType = OleObjectType.Package;
  private string m_storageName;
  private string m_objectType = string.Empty;
  private string m_oleFileName = string.Empty;
  private string m_fileName = string.Empty;

  internal string FallbackShapeId
  {
    get => this.m_fallbackShapeId;
    set => this.m_fallbackShapeId = value;
  }

  internal string DefaultSizeValue
  {
    get => this.m_defaultSizeValue;
    set => this.m_defaultSizeValue = value;
  }

  internal string MoveWithCellsValue
  {
    get => this.m_moveWithCellsValue;
    set => this.m_moveWithCellsValue = value;
  }

  internal string SizeWithCellsValue
  {
    get => this.m_sizeWithCellsValue;
    set => this.m_sizeWithCellsValue = value;
  }

  internal string ObjectPrRelationId
  {
    get => this.m_objectPrRelationId;
    set => this.m_objectPrRelationId = value;
  }

  public IRange Location
  {
    get => this.m_sheet[this.m_shape.TopRow, this.m_shape.LeftColumn];
    set
    {
      this.m_shape.TopRow = value.Row;
      this.m_shape.LeftColumn = value.Column;
    }
  }

  public Size Size
  {
    get => new Size(this.m_shape.Width, this.m_shape.Height);
    set
    {
      this.m_shape.Width = value.Width;
      this.m_shape.Height = value.Height;
    }
  }

  public Image Picture
  {
    get => this.Shape?.Picture;
    set
    {
      if (value == null)
        throw new Exception("Image");
      throw new NotImplementedException();
    }
  }

  public bool DisplayAsIcon
  {
    get => this.m_isIcon;
    set
    {
      if (value)
      {
        this.DvAspect = DVAspect.DVASPECT_ICON;
        this.m_isIcon = value;
      }
      else
      {
        this.DvAspect = DVAspect.DVASPECT_CONTENT;
        this.m_isIcon = value;
      }
    }
  }

  public OleLinkType OleType
  {
    get => this.m_oleLinkType;
    set => this.m_oleLinkType = value;
  }

  public bool IsStream
  {
    get => this.m_isStream;
    set => this.m_isStream = value;
  }

  public WorksheetImpl OleSheet => this.m_sheet;

  public bool IsContainer
  {
    get => this.m_isContainer;
    set => this.m_isContainer = value;
  }

  public Stream Container
  {
    get
    {
      if (this.m_container == null)
        this.m_container = this.GetOleContainer();
      return this.m_container;
    }
    set => this.m_container = value;
  }

  public byte[] FileNativeData
  {
    get => this.m_fileNativeData;
    set => this.m_fileNativeData = value;
  }

  [Obsolete("This property has been depreceated. Use the OleObjectType property instead.")]
  public string ObjectType
  {
    get => this.m_objectType;
    set => this.m_objectType = value;
  }

  public Dictionary<string, int> Locate => this.m_location;

  public string FileName
  {
    get => this.m_fileName;
    set => this.m_fileName = value;
  }

  public string StorageName
  {
    get => this.m_storageName;
    set => this.m_storageName = value;
  }

  public OleObjectType OleObjectType
  {
    get => this.m_oleObjectType;
    set => this.m_oleObjectType = value;
  }

  public DVAspect DvAspect
  {
    get => this.m_dvAspect;
    set => this.m_dvAspect = value;
  }

  public int ShapeID
  {
    get => this.m_shape.ShapeId;
    set
    {
      this.m_shape = this.m_sheet.InnerShapes.GetOLEShapeById(value) as ShapeImpl;
      if (this.m_shape != null)
        return;
      this.m_shape = this.m_sheet.InnerShapes.GetShapeById(value) as ShapeImpl;
    }
  }

  public string ShapeRId
  {
    get => this.m_shapeRId;
    set => this.m_shapeRId = value;
  }

  public IPictureShape Shape
  {
    get => this.m_shape as IPictureShape;
    set
    {
      this.m_shape = (ShapeImpl) value;
      this.m_shape.VmlShape = true;
    }
  }

  public string Name => this.m_shape == null ? (string) null : this.m_shape.Name;

  public string ContentType
  {
    get => this.m_strContentType;
    set => this.m_strContentType = value;
  }

  public string RelationType
  {
    get => this.m_strRelationType;
    set => this.m_strRelationType = value;
  }

  public OleObject(WorksheetImpl sheet)
  {
    this.m_sheet = sheet != null ? sheet : throw new ArgumentNullException();
  }

  internal OleObject(string filePath, WorksheetImpl sheet, OleLinkType oleLinkType)
  {
    this.m_sheet = sheet;
    this.FileName = filePath;
    this.OleType = oleLinkType;
    this.DisplayAsIcon = true;
    this.FileName = filePath;
  }

  public OleObject(string fileName, Image image)
  {
    if (!File.Exists(fileName))
      throw new Exception("Values Should Be passed");
    this.CheckFileName(fileName);
    this.Picture = image != null ? image : throw new ArgumentNullException("Image");
    this.FileName = fileName;
    this.DisplayAsIcon = true;
    this.SetFile(fileName);
  }

  public OleObject(string fileName, Image image, OleLinkType oleLinkType)
  {
    if (!File.Exists(fileName))
      throw new Exception("Values Should Be passed");
    this.CheckFileName(fileName);
    this.Picture = image != null ? image : throw new ArgumentNullException("Image");
    this.FileName = Path.GetFullPath(fileName);
    this.OleType = oleLinkType;
    this.DisplayAsIcon = true;
    this.SetFile(fileName);
  }

  public OleObject(string fileName, IPictureShape shape, OleLinkType linkType)
  {
    this.m_sheet = shape != null ? (shape as ShapeImpl).Worksheet as WorksheetImpl : throw new ArgumentNullException(nameof (shape));
    if (!File.Exists(fileName))
      throw new ArgumentException("File not found.");
    this.CheckFileName(fileName);
    this.Shape = shape;
    this.FileName = Path.GetFullPath(fileName);
    this.OleType = linkType;
    this.DisplayAsIcon = true;
    this.SetFile(fileName);
  }

  public static OleObject OleFromFile(string fileName, Image image)
  {
    return new OleObject(fileName, image);
  }

  public static OleObject OleFromStream(Stream stream, Image image, string extension)
  {
    return new OleObject(stream, image, extension);
  }

  public OleObject(Stream stream, Image image, string extension)
  {
    if (!stream.CanSeek || !stream.CanRead)
      throw new Exception("Stream");
    if (image == null)
      throw new ArgumentNullException("Image");
    this.IsStream = true;
    this.Picture = image;
    byte[] numArray = new byte[stream.Length];
    stream.Read(numArray, 0, numArray.Length);
    stream.Close();
    this.m_fileName = this.GetStreamFileName(numArray, extension);
    using (FileStream fileStream = new FileStream(this.m_fileName, FileMode.Open, FileAccess.Read))
    {
      this.m_file = new byte[fileStream.Length];
      fileStream.Read(this.m_file, 0, this.m_file.Length);
    }
    this.StorageName = OleTypeConvertor.GetOleFileName();
    this.OleType = OleLinkType.Embed;
    this.DisplayAsIcon = true;
    this.SetOleFile(this.m_fileName, this.StorageName);
  }

  internal string GetStreamFileName(byte[] readValue, string extension)
  {
    string path = Path.ChangeExtension(Path.GetTempPath() + Guid.NewGuid().ToString(), extension).Replace('\\', '/');
    FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);
    fileStream.Write(readValue, 0, readValue.Length);
    fileStream.Close();
    return path;
  }

  internal string SetFile(string fileName)
  {
    if (!File.Exists(fileName))
      throw new Exception("The File does not exists in the path, please ensure the existence of the file");
    using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
    {
      this.m_file = new byte[fileStream.Length];
      fileStream.Read(this.m_file, 0, this.m_file.Length);
    }
    this.m_fileName = fileName;
    return fileName;
  }

  public void ExtractOleData(string fileName)
  {
    this.CheckFileName(fileName);
    using (FileStream fileStream = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write, FileShare.None))
    {
      this.Container.Position = 0L;
      byte[] buffer = new byte[this.Container.Length - 1536L /*0x0600*/];
      this.Container.Read(buffer, 1537, buffer.Length);
      fileStream.Write(buffer, 0, buffer.Length);
    }
  }

  private static void CopyStream(Stream input, Stream output)
  {
    byte[] buffer = new byte[2000];
    int count;
    while ((count = input.Read(buffer, 0, 2000)) > 0)
      output.Write(buffer, 0, count);
    output.Flush();
  }

  private void CheckFileName(string fileName)
  {
    bool flag = false;
    if (fileName.Length >= 252)
      flag = true;
    else if (Path.GetDirectoryName(fileName).Length >= 248)
      flag = true;
    if (flag)
      throw new PathTooLongException("The file name is too long. The fully qualified file name must be less than 260 characters and the directory name must be less than 248 characters");
  }

  internal void SetOleFile(string fileName, string storageName)
  {
    if (storageName == null)
      return;
    this.CreateOleObjContainer(fileName, storageName);
  }

  internal void SetOleFile(string fileName, string storageName, byte[] fileBytes)
  {
    if (storageName == null)
      return;
    this.m_file = fileBytes;
    this.CreateOleObjContainer(fileName, storageName);
  }

  internal void CreateOleObjContainer(string filePath, string olestorageName)
  {
    this.Save(this.CreateOrGetObjPool(olestorageName), filePath, olestorageName);
    if (!this.IsStream || !File.Exists(filePath))
      return;
    File.Delete(filePath);
  }

  internal void Save(byte[] objectPool, string dataPath, string storageName)
  {
    StgStream stgStream1 = new StgStream((Stream) new MemoryStream(objectPool), STGM.STGM_READWRITE | STGM.STGM_SHARE_EXCLUSIVE);
    StgStream stgStream2 = stgStream1.OpenSubStorage("ObjectPool", STGM.STGM_READWRITE | STGM.STGM_SHARE_EXCLUSIVE);
    StgStream rootStg = stgStream2.OpenSubStorage(storageName, STGM.STGM_READWRITE | STGM.STGM_SHARE_EXCLUSIVE);
    this.WriteOleStream(rootStg, dataPath);
    this.WriteObjInfoStream(rootStg);
    this.WriteCompObjStream(rootStg);
    this.WritePackage(rootStg, dataPath);
    stgStream2.Flush();
    stgStream1.Flush();
    rootStg.Close();
    rootStg.Dispose();
    MemoryStream memoryStream = new MemoryStream();
    stgStream1.SaveILockBytesIntoStream((Stream) memoryStream);
    memoryStream.Flush();
    this.m_fileNativeData = memoryStream.ToArray();
    memoryStream.Close();
    memoryStream.Dispose();
    stgStream1.Close();
    stgStream1.Dispose();
    stgStream2.Close();
    stgStream2.Dispose();
  }

  private byte[] CreateOrGetObjPool(string name)
  {
    try
    {
      MemoryStream memoryStream1 = (MemoryStream) null;
      StgStream stgStream1;
      StgStream destination;
      if (this.FileNativeData == null || this.FileNativeData.Length == 0)
      {
        stgStream1 = StgStream.CreateStorageOnILockBytes();
        destination = stgStream1.CreateSubStorage("ObjectPool");
      }
      else
      {
        memoryStream1 = new MemoryStream(this.FileNativeData);
        stgStream1 = new StgStream((Stream) memoryStream1);
        destination = stgStream1.OpenSubStorage("ObjectPool", STGM.STGM_READWRITE | STGM.STGM_SHARE_EXCLUSIVE);
      }
      Syncfusion.CompoundFile.XlsIO.Net.CompoundFile compoundFile = new Syncfusion.CompoundFile.XlsIO.Net.CompoundFile();
      compoundFile.RootStorage.CreateStorage(name);
      compoundFile.Directory.Entries[1].StorageGuid = OleTypeConvertor.GetGUID();
      MemoryStream memoryStream2 = new MemoryStream();
      compoundFile.Flush();
      compoundFile.Save((Stream) memoryStream2);
      compoundFile.Dispose();
      memoryStream2.Flush();
      byte[] array = memoryStream2.ToArray();
      memoryStream2.Close();
      MemoryStream memoryStream3 = new MemoryStream(array);
      MemoryStream memoryStream4 = new MemoryStream();
      StgStream stgStream2 = new StgStream((Stream) memoryStream3);
      StgStream source = stgStream2.OpenSubStorage(name);
      StgStream.CopySourceStorages(source, destination);
      stgStream1.Flush();
      stgStream1.SaveILockBytesIntoStream((Stream) memoryStream4);
      memoryStream4.Position = 0L;
      this.m_fileNativeData = memoryStream4.ToArray();
      stgStream2.Close();
      stgStream2.Dispose();
      source.Close();
      source.Dispose();
      stgStream1.Close();
      stgStream1.Dispose();
      destination.Close();
      destination.Dispose();
      memoryStream4.Close();
      memoryStream4.Dispose();
      memoryStream3.Close();
      memoryStream3.Dispose();
      if (memoryStream1 != null)
      {
        memoryStream1.Close();
        memoryStream1.Dispose();
      }
    }
    catch (Exception ex)
    {
    }
    return this.m_fileNativeData;
  }

  private void WriteOleStream(StgStream rootStg, string dataPath)
  {
    rootStg.CreateStream("\u0001Ole");
    this.m_oleStream = new OleStream(dataPath);
    this.m_oleStream.SaveTo(rootStg);
    rootStg.Close();
  }

  private void WriteObjInfoStream(StgStream rootStg)
  {
    rootStg.CreateStream("\u0003ObjInfo");
    this.m_objectInfoStream = new ObjectInfoStream();
    this.m_objectInfoStream.SaveTo(rootStg);
    rootStg.Close();
  }

  private Stream GetOleContainer()
  {
    if (this.FileNativeData == null || this.FileNativeData.Length == 0)
      return (Stream) null;
    MemoryStream memoryStream = new MemoryStream(this.FileNativeData);
    StgStream stgStream1 = (StgStream) null;
    StgStream stgStream2 = (StgStream) null;
    StgStream source = (StgStream) null;
    StgStream destination = (StgStream) null;
    MemoryStream oleContainer = (MemoryStream) null;
    try
    {
      memoryStream = new MemoryStream(this.FileNativeData);
      stgStream1 = new StgStream((Stream) memoryStream);
      stgStream2 = stgStream1.OpenSubStorage("ObjectPool");
      source = stgStream2.OpenSubStorage(this.StorageName.ToString());
      destination = StgStream.CreateStorageOnILockBytes();
      StgStream.CopySourceStorages(source, destination);
      oleContainer = new MemoryStream();
      destination.SaveILockBytesIntoStream((Stream) oleContainer);
      oleContainer.Position = 0L;
    }
    catch (Exception ex)
    {
    }
    finally
    {
      if (memoryStream != null)
      {
        memoryStream.Close();
        memoryStream.Dispose();
      }
      if (stgStream1 != null)
      {
        stgStream1.Close();
        stgStream2.Dispose();
      }
      if (stgStream2 != null)
      {
        stgStream2.Close();
        stgStream2.Dispose();
      }
      if (source != null)
      {
        source.Close();
        source.Dispose();
      }
      if (destination != null)
      {
        destination.Close();
        destination.Dispose();
      }
    }
    return (Stream) oleContainer;
  }

  private void WriteCompObjStream(StgStream rootStg)
  {
    if (this.ContainStream(rootStg.Streams, "\u0001CompObj"))
      return;
    rootStg.CreateStream("\u0001CompObj");
    this.m_compObjectStream = new CompObjectStream();
    this.m_compObjectStream.SaveTo(rootStg);
    rootStg.Close();
  }

  private void WritePackage(StgStream rootStg, string dataPath)
  {
    ASCIIEncoding asciiEncoding = new ASCIIEncoding();
    string fileName = Path.GetFileName(dataPath);
    byte[] bytes1 = asciiEncoding.GetBytes(fileName);
    byte[] bytes2 = asciiEncoding.GetBytes(dataPath);
    byte[] bytes3 = new byte[2]{ (byte) 2, (byte) 0 };
    byte[] bytes4 = new byte[4]
    {
      (byte) 0,
      (byte) 0,
      (byte) 3,
      (byte) 0
    };
    int length = 4 + bytes3.Length + (bytes1.Length + 1) + (bytes2.Length + 1) + bytes4.Length + 4 + (bytes2.Length + 1) + 4 + this.m_file.Length + 2;
    int iOffset1 = 0;
    byte[] numArray = new byte[length];
    DataStructure.WriteInt32(numArray, ref iOffset1, length - 4);
    DataStructure.WriteBytes(numArray, ref iOffset1, bytes3);
    DataStructure.WriteBytes(numArray, ref iOffset1, bytes1);
    int iOffset2 = iOffset1 + 1;
    DataStructure.WriteBytes(numArray, ref iOffset2, bytes2);
    int iOffset3 = iOffset2 + 1;
    DataStructure.WriteBytes(numArray, ref iOffset3, bytes4);
    DataStructure.WriteInt32(numArray, ref iOffset3, bytes2.Length + 1);
    DataStructure.WriteBytes(numArray, ref iOffset3, bytes2);
    int iOffset4 = iOffset3 + 1;
    DataStructure.WriteInt32(numArray, ref iOffset4, this.m_file.Length);
    DataStructure.WriteBytes(numArray, ref iOffset4, this.m_file);
    GC.SuppressFinalize((object) this.m_file);
    this.m_file = (byte[]) null;
    rootStg.CreateStream("\u0001Ole10Native");
    rootStg.Write(numArray, 0, numArray.Length);
    rootStg.Close();
  }

  private bool ContainStream(string[] streamNames, string name)
  {
    bool flag = false;
    int index = 0;
    for (int length = streamNames.Length; index < length; ++index)
    {
      if (streamNames[index] == name)
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  public int GetWorkbookIndex()
  {
    return this.m_sheet.ParentWorkbook.ExternWorkbooks[this.m_fileName].Index;
  }
}
