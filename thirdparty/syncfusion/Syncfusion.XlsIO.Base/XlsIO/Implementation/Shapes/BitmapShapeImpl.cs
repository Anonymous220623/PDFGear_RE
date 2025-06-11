// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Shapes.BitmapShapeImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.XmlSerialization;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing;
using Syncfusion.XlsIO.Parser.Biff_Records.ObjRecords;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Shapes;

public class BitmapShapeImpl : ShapeImpl, IDisposable, IPictureShape, IShape, IParentApplication
{
  public const int ShapeInstance = 75;
  private uint m_uiBlipId;
  private string m_strBlipFileName;
  private MsofbtBSE m_picture;
  private Image m_bitmap;
  private Stream m_bitmapStream;
  private Stream m_streamBlipSubNodes;
  private Stream m_streamShapeProperties;
  private Stream m_svgData;
  private string m_svgRelId;
  private string m_svgPicturePath;
  private bool m_isSVGUpdated;
  private Stream m_srcRectStream;
  private string m_strMacro;
  private bool m_bDDE;
  private bool m_bCamera;
  private long m_offsetX;
  private long m_offsetY;
  private long m_extentsX;
  private long m_extentsY;
  private bool m_flipVertical;
  private bool m_flipHorizontal;
  private string m_externalLink;
  private bool m_isSvgExternalLink;
  internal new Dictionary<string, Stream> m_preservedElements;
  protected MsoOptions[] cropOptions = new MsoOptions[4]
  {
    MsoOptions.CropFromBottom,
    MsoOptions.CropFromLeft,
    MsoOptions.CropFromRight,
    MsoOptions.CropFromTop
  };
  private int m_cropLeftOffset;
  private int m_cropRightOffset;
  private int m_cropBottomOffset;
  private int m_cropTopOffset;
  private bool m_hasTransparentDetails;
  private int m_amount = 100000;
  private int m_threshold;
  private bool m_grayScale;
  private bool m_isUseAlpha = true;
  private List<ColorObject> m_duoTone;
  private List<ColorObject> m_colorChange;
  private CameraTool m_camera;

  public BitmapShapeImpl(IApplication application, object parent)
    : this(application, parent, true)
  {
  }

  public BitmapShapeImpl(IApplication application, object parent, bool IncludeShapeOptions)
    : base(application, parent)
  {
    this.m_bSupportOptions = true;
    if (IncludeShapeOptions)
    {
      this.m_bUpdateLineFill = true;
      this.Fill.Visible = false;
      this.Line.Visible = false;
    }
    this.ShapeType = ExcelShapeType.Picture;
  }

  [CLSCompliant(false)]
  public BitmapShapeImpl(IApplication application, object parent, MsoBase[] records, int index)
    : base(application, parent, records, index)
  {
    this.m_bSupportOptions = true;
    this.ShapeType = ExcelShapeType.Picture;
    this.m_bitmap = this.m_picture.PictureRecord.Picture;
  }

  [CLSCompliant(false)]
  public BitmapShapeImpl(IApplication application, object parent, MsofbtSpContainer container)
    : base(application, parent, container, ExcelParseOptions.Default)
  {
    this.m_bSupportOptions = true;
    this.ShapeType = ExcelShapeType.Picture;
    if (this.m_picture == null)
      return;
    this.m_bitmap = this.m_picture.PictureRecord.Picture;
  }

  public bool HasTransparency
  {
    get => this.m_hasTransparentDetails;
    set => this.m_hasTransparentDetails = value;
  }

  public string FileName
  {
    get => this.m_strBlipFileName;
    set => this.m_strBlipFileName = value;
  }

  internal string ExternalLink
  {
    get => this.m_externalLink;
    set => this.m_externalLink = value;
  }

  internal bool IsSvgExternalLink
  {
    get => this.m_isSvgExternalLink;
    set => this.m_isSvgExternalLink = value;
  }

  [CLSCompliant(false)]
  public uint BlipId
  {
    get => this.m_uiBlipId;
    set
    {
      this.m_uiBlipId = value;
      this.m_picture = this.m_shapes.ShapeData.Pictures[(int) value - 1];
      this.m_bitmap = this.m_picture.PictureRecord.Picture;
      this.m_bitmapStream = this.m_picture.PictureRecord.PictureStream;
    }
  }

  internal int CropLeftOffset
  {
    get => this.m_cropLeftOffset;
    set => this.m_cropLeftOffset = value;
  }

  internal int CropRightOffset
  {
    get => this.m_cropRightOffset;
    set => this.m_cropRightOffset = value;
  }

  internal int CropBottomOffset
  {
    get => this.m_cropBottomOffset;
    set => this.m_cropBottomOffset = value;
  }

  internal int CropTopOffset
  {
    get => this.m_cropTopOffset;
    set => this.m_cropTopOffset = value;
  }

  public Image Picture
  {
    get
    {
      if (this.m_bitmapStream != null)
        this.m_bitmap = Image.FromStream(this.m_bitmapStream);
      if (!this.ParentWorkbook.Saving && !this.m_isSvgExternalLink && !string.IsNullOrEmpty(this.m_externalLink))
      {
        Stream stream = this.DownloadImage(this.m_externalLink);
        if (stream != null)
          this.m_bitmap = Image.FromStream(stream);
      }
      return this.m_bitmap;
    }
    set
    {
      if (value == null)
        throw new ArgumentNullException("Bitmap");
      WorkbookShapeDataImpl shapeData = this.m_shapes.ShapeData;
      shapeData.RemovePicture(this.BlipId, true);
      this.BlipId = (uint) shapeData.AddPicture(value, ExcelImageFormat.Png, this.Name);
    }
  }

  public Stream BlipSubNodesStream
  {
    get => this.m_streamBlipSubNodes;
    set => this.m_streamBlipSubNodes = value;
  }

  [Obsolete]
  public Stream ShapePropertiesStream
  {
    get => this.m_streamShapeProperties;
    set => this.m_streamShapeProperties = value;
  }

  public Stream SvgData
  {
    get
    {
      if (!this.ParentWorkbook.Saving && this.m_isSvgExternalLink && !string.IsNullOrEmpty(this.m_externalLink))
        this.m_svgData = this.DownloadImage(this.m_externalLink);
      return this.m_svgData;
    }
    set
    {
      if (value == null || value.Length <= 0L)
        return;
      this.m_svgData = value;
      if (this.m_svgRelId == null)
        return;
      this.m_svgRelId = (string) null;
      this.m_isSVGUpdated = true;
    }
  }

  internal string SvgRelId
  {
    get => this.m_svgRelId;
    set => this.m_svgRelId = value;
  }

  internal bool IsSvgUpdated
  {
    get => this.m_isSVGUpdated;
    set => this.m_isSVGUpdated = value;
  }

  internal string SvgPicturePath
  {
    get => this.m_svgPicturePath;
    set => this.m_svgPicturePath = value;
  }

  internal new Dictionary<string, Stream> PreservedElements
  {
    get
    {
      if (this.m_preservedElements == null)
        this.m_preservedElements = new Dictionary<string, Stream>();
      return this.m_preservedElements;
    }
  }

  public Stream SourceRectStream
  {
    get => this.m_srcRectStream;
    set => this.m_srcRectStream = value;
  }

  public override int Instance => this.m_shape == null ? 75 : this.m_shape.Instance;

  public string Macro
  {
    get => this.m_strMacro;
    set => this.m_strMacro = value;
  }

  public bool IsDDE
  {
    get => this.m_bDDE;
    set => this.m_bDDE = value;
  }

  public bool IsCamera
  {
    get => this.m_bCamera;
    set => this.m_bCamera = value;
  }

  internal long OffsetX
  {
    get => this.m_offsetX;
    set => this.m_offsetX = value;
  }

  internal long OffsetY
  {
    get => this.m_offsetY;
    set => this.m_offsetY = value;
  }

  internal long ExtentsX
  {
    get => this.m_extentsX;
    set => this.m_extentsX = value;
  }

  internal long ExtentsY
  {
    get => this.m_extentsY;
    set => this.m_extentsY = value;
  }

  internal bool FlipVertical
  {
    get => this.m_flipVertical;
    set => this.m_flipVertical = value;
  }

  internal bool FlipHorizontal
  {
    get => this.m_flipHorizontal;
    set => this.m_flipHorizontal = value;
  }

  public new IHyperLink Hyperlink
  {
    get => base.Hyperlink;
    internal set => base.Hyperlink = value;
  }

  internal bool IsUseAlpha
  {
    get => this.m_isUseAlpha;
    set => this.m_isUseAlpha = value;
  }

  internal List<ColorObject> DuoTone
  {
    get
    {
      if (this.m_duoTone == null)
        this.m_duoTone = new List<ColorObject>();
      return this.m_duoTone;
    }
  }

  internal List<ColorObject> ColorChange
  {
    get
    {
      if (this.m_colorChange == null)
        this.m_colorChange = new List<ColorObject>();
      return this.m_colorChange;
    }
  }

  internal int Amount
  {
    get => this.m_amount;
    set => this.m_amount = value;
  }

  internal int Threshold
  {
    get => this.m_threshold;
    set => this.m_threshold = value;
  }

  internal bool GrayScale
  {
    get => this.m_grayScale;
    set => this.m_grayScale = value;
  }

  internal CameraTool Camera
  {
    get => this.m_camera;
    set => this.m_camera = value;
  }

  [CLSCompliant(false)]
  protected override bool ParseOption(MsofbtOPT.FOPTE option)
  {
    if (base.ParseOption(option))
      return true;
    switch (option.Id)
    {
      case MsoOptions.CropFromTop:
      case MsoOptions.CropFromBottom:
      case MsoOptions.CropFromLeft:
      case MsoOptions.CropFromRight:
        this.ParseCropRectangle(option);
        return true;
      case MsoOptions.BlipId:
        this.ParseBlipId(option);
        return true;
      case MsoOptions.BlipName:
        this.ParseBlipName(option);
        return true;
      default:
        return false;
    }
  }

  protected void ParseCropRectangle(MsofbtOPT.FOPTE option)
  {
    if (option == null)
      throw new ArgumentNullException(nameof (option));
    if (Array.IndexOf<MsoOptions>(this.cropOptions, option.Id) < 0)
      throw new ArgumentOutOfRangeException("Crop option expected");
    switch (option.Id)
    {
      case MsoOptions.CropFromTop:
        this.m_cropTopOffset = option.Int32Value + option.Int32Value / 2;
        break;
      case MsoOptions.CropFromBottom:
        this.m_cropBottomOffset = option.Int32Value + option.Int32Value / 2;
        break;
      case MsoOptions.CropFromLeft:
        this.m_cropLeftOffset = option.Int32Value + option.Int32Value / 2;
        break;
      case MsoOptions.CropFromRight:
        this.m_cropRightOffset = option.Int32Value + option.Int32Value / 2;
        break;
    }
  }

  [CLSCompliant(false)]
  protected virtual void ParseBlipId(MsofbtOPT.FOPTE option)
  {
    if (option == null)
      throw new ArgumentNullException(nameof (option));
    this.m_uiBlipId = option.Id == MsoOptions.BlipId ? option.UInt32Value : throw new ArgumentOutOfRangeException("BlipId option expected");
    IList pictures = (IList) this.m_shapes.ShapeData.Pictures;
    this.m_picture = this.m_uiBlipId > 0U ? (MsofbtBSE) pictures[(int) this.m_uiBlipId - 1] : (MsofbtBSE) null;
  }

  [CLSCompliant(false)]
  protected virtual void ParseBlipName(MsofbtOPT.FOPTE option)
  {
    if (option == null)
      throw new ArgumentNullException(nameof (option));
    if (option.Id != MsoOptions.BlipName)
      throw new ArgumentOutOfRangeException("BlipName option expected");
    if (option.AdditionalData == null)
      return;
    byte[] additionalData = option.AdditionalData;
    this.m_strBlipFileName = Encoding.Unicode.GetString(additionalData, 0, additionalData.Length);
  }

  [CLSCompliant(false)]
  protected override bool ExtractNecessaryOption(MsofbtOPT.FOPTE option)
  {
    if (base.ExtractNecessaryOption(option))
      return true;
    switch (option.Id)
    {
      case MsoOptions.BlipId:
        this.ParseBlipId(option);
        return true;
      case MsoOptions.BlipName:
        this.ParseBlipName(option);
        return true;
      default:
        return false;
    }
  }

  public new void Dispose() => base.Dispose();

  [CLSCompliant(false)]
  protected override void SerializeShape(MsofbtSpgrContainer spgrContainer)
  {
    MsofbtSpContainer record = (MsofbtSpContainer) MsoFactory.GetRecord(MsoRecords.msofbtSpContainer);
    record.AddItem((MsoBase) this.m_shape);
    this.SerializeOptions(record);
    this.SerializeClientAnchor(record);
    this.SerializeClientData(record);
    spgrContainer.AddItem((MsoBase) record);
  }

  protected override void OnPrepareForSerialization()
  {
    if (this.m_shape == null)
    {
      this.m_shape = (MsofbtSp) MsoFactory.GetRecord(MsoRecords.msofbtSp);
      this.m_shape.Instance = 75;
    }
    this.m_shape.IsHaveAnchor = true;
    this.m_shape.IsHaveSpt = true;
  }

  private void SerializeOptions(MsofbtSpContainer spContainer)
  {
    MsofbtOPT msofbtOpt = this.m_options;
    if (msofbtOpt == null)
    {
      msofbtOpt = this.CreateDefaultOptions();
      this.SerializeOptionSorted(msofbtOpt, MsoOptions.NoLineDrawDash, 524296U /*0x080008*/);
      this.SerializeOptionSorted(msofbtOpt, MsoOptions.NoFillHitTest, 1048576U /*0x100000*/);
    }
    if (this.m_bUpdateLineFill)
      msofbtOpt = this.SerializeMsoOptions(msofbtOpt);
    msofbtOpt.AddOptionSorted(new MsofbtOPT.FOPTE()
    {
      Id = MsoOptions.BlipId,
      UInt32Value = this.m_uiBlipId,
      IsValid = true,
      IsComplex = false
    });
    MsofbtOPT.FOPTE option = new MsofbtOPT.FOPTE();
    option.Id = MsoOptions.BlipName;
    if (this.m_strBlipFileName != null)
    {
      if (this.m_strBlipFileName[this.m_strBlipFileName.Length - 1] != char.MinValue)
        this.m_strBlipFileName += (string) (object) char.MinValue;
      option.UInt32Value = (uint) (this.m_strBlipFileName.Length * 2);
      option.IsValid = true;
      option.IsComplex = true;
      option.AdditionalData = Encoding.Unicode.GetBytes(this.m_strBlipFileName);
      msofbtOpt.AddOptionSorted(option);
    }
    this.SerializeShapeName(msofbtOpt);
    this.SerializeName(msofbtOpt, MsoOptions.AlternativeText, this.AlternativeText);
    this.SerializeShapeVisibility(msofbtOpt);
    msofbtOpt.Version = 3;
    msofbtOpt.Instance = 2;
    spContainer.AddItem((MsoBase) msofbtOpt);
  }

  private void SerializeClientAnchor(MsofbtSpContainer spContainer)
  {
    if (spContainer == null)
      throw new ArgumentNullException(nameof (spContainer));
    spContainer.AddItem((MsoBase) this.ClientAnchor);
  }

  private void SerializeClientData(MsofbtSpContainer spContainer)
  {
    if (this.IsShortVersion)
      return;
    if (spContainer == null)
      throw new ArgumentNullException(nameof (spContainer));
    MsofbtClientData record1 = (MsofbtClientData) MsoFactory.GetRecord(MsoRecords.msofbtClientData);
    OBJRecord record2 = this.Obj;
    ftCmo record3;
    if (record2 == null)
    {
      record2 = (OBJRecord) BiffRecordFactory.GetRecord(TBIFFRecord.OBJ);
      record3 = new ftCmo();
      record3.ObjectType = TObjType.otPicture;
      record3.Printable = true;
      record3.Locked = true;
      record3.AutoFill = true;
      record3.AutoLine = true;
      ftEnd record4 = new ftEnd();
      record2.AddSubRecord((ObjSubRecord) record3);
      record2.AddSubRecord((ObjSubRecord) record4);
    }
    else
      record3 = (ftCmo) record2.Records[0];
    record3.ID = this.OldObjId > 0U ? (ushort) this.OldObjId : (ushort) this.ParentWorkbook.CurrentObjectId;
    record1.AddRecord((BiffRecordRaw) record2);
    spContainer.AddItem((MsoBase) record1);
  }

  public override void RegisterInSubCollection()
  {
    this.m_shapes.WorksheetBase.InnerPictures.AddPicture((IPictureShape) this);
  }

  protected override void OnDelete() => this.OnDelete(true);

  protected void OnDelete(bool removeImage)
  {
    base.OnDelete();
    if (this.BlipId > 0U)
    {
      this.ParentShapes.Workbook.ShapesData.RemovePicture(this.BlipId, removeImage);
      this.m_uiBlipId = 0U;
    }
    ((PicturesCollection) this.m_shapes.Worksheet.Pictures).RemovePicture((IPictureShape) this);
  }

  public void Remove(bool removeImage)
  {
    this.OnDelete(removeImage);
    this.m_shapes.Remove((IShape) this);
  }

  public override IShape Clone(
    object parent,
    Dictionary<string, string> hashNewNames,
    Dictionary<int, int> dicFontIndexes,
    bool addToCollection)
  {
    bool flag = true;
    ShapeCollectionBase parent1 = (ShapeCollectionBase) CommonObject.FindParent(parent, typeof (ShapeCollectionBase), true);
    WorksheetBaseImpl worksheetBaseImpl;
    if (parent1 != null)
    {
      worksheetBaseImpl = parent1.WorksheetBase;
    }
    else
    {
      worksheetBaseImpl = CommonObject.FindParent(parent, typeof (WorksheetBaseImpl), true) as WorksheetBaseImpl;
      flag = false;
    }
    WorkbookImpl parentWorkbook1 = this.ParentWorkbook;
    WorkbookImpl parentWorkbook2 = worksheetBaseImpl.ParentWorkbook;
    int num = (int) this.BlipId;
    if (flag)
      flag = !(parent1 is HeaderFooterShapeCollection);
    WorkbookShapeDataImpl workbookShapeDataImpl = flag ? parentWorkbook2.ShapesData : parentWorkbook2.HeaderFooterData;
    if (parentWorkbook2 != parentWorkbook1 && num > 0)
    {
      MsofbtBSE picture = (flag ? parentWorkbook1.ShapesData : parentWorkbook1.HeaderFooterData).GetPicture(num);
      num = workbookShapeDataImpl.AddPicture((MsofbtBSE) picture.Clone());
    }
    BitmapShapeImpl bitmapShapeImpl;
    if (flag || !addToCollection)
    {
      bitmapShapeImpl = (BitmapShapeImpl) this.MemberwiseClone();
      bitmapShapeImpl.SetParent(parent);
      bitmapShapeImpl.SetParents();
      bitmapShapeImpl.AttachEvents();
      bitmapShapeImpl.CopyFrom((ShapeImpl) this, hashNewNames, dicFontIndexes);
      bitmapShapeImpl.CloneLineFill((ShapeImpl) this);
      if (this.m_duoTone != null)
      {
        bitmapShapeImpl.m_duoTone = new List<ColorObject>();
        for (int index = 0; index < this.m_duoTone.Count; ++index)
          bitmapShapeImpl.m_duoTone.Add(this.m_duoTone[index].Clone());
      }
      if (this.m_colorChange != null)
      {
        bitmapShapeImpl.m_colorChange = new List<ColorObject>();
        for (int index = 0; index < this.m_colorChange.Count; ++index)
          bitmapShapeImpl.m_colorChange.Add(this.m_colorChange[index].Clone());
      }
      if (this.Hyperlink != null)
        bitmapShapeImpl.Hyperlink = (IHyperLink) (this.Hyperlink as HyperLinkImpl).Clone((object) bitmapShapeImpl);
      if (addToCollection)
        worksheetBaseImpl.InnerShapes.AddPicture(bitmapShapeImpl);
      if (num > 0)
      {
        bitmapShapeImpl.BlipId = (uint) num;
        ++workbookShapeDataImpl.Pictures[num - 1].RefCount;
      }
      if (this.m_camera != null)
        bitmapShapeImpl.Camera = this.m_camera.Clone((object) bitmapShapeImpl);
    }
    else
    {
      if (flag)
        throw new NotImplementedException();
      bitmapShapeImpl = worksheetBaseImpl.HeaderFooterShapes.SetPicture(this.Name, this.Picture, num, false, this.PreserveStyleString) as BitmapShapeImpl;
      bitmapShapeImpl.m_options = (MsofbtOPT) CloneUtils.CloneCloneable((ICloneable) this.m_options);
      bitmapShapeImpl.m_srcRectStream = CloneUtils.CloneStream(this.m_srcRectStream);
      bitmapShapeImpl.m_streamBlipSubNodes = CloneUtils.CloneStream(this.m_streamBlipSubNodes);
      bitmapShapeImpl.m_preservedElements = CloneUtils.CloneHash<string, Stream>(this.PreservedElements);
      bitmapShapeImpl.AttachEvents();
    }
    if (this.ImageRelation != null)
      bitmapShapeImpl.ImageRelation = (Relation) this.ImageRelation.Clone();
    return (IShape) bitmapShapeImpl;
  }

  [CLSCompliant(false)]
  protected override bool UpdateMso(MsoBase mso)
  {
    if (base.UpdateMso(mso))
      return true;
    if (!(mso is MsofbtBSE))
      return false;
    this.m_picture = mso as MsofbtBSE;
    this.m_bitmap = this.m_picture.PictureRecord.Picture;
    return true;
  }

  public override void GenerateDefaultName()
  {
    this.Name = CollectionBaseEx<IShape>.GenerateDefaultName((ICollection<IShape>) this.m_shapes, "Picture ");
  }

  [CLSCompliant(false)]
  public void SetBlipId(uint newId) => this.m_uiBlipId = newId;

  internal Stream DownloadImage(string url)
  {
    if (string.IsNullOrEmpty(url))
      return (Stream) null;
    byte[] buffer = (byte[]) null;
    try
    {
      if (url.StartsWith("http") || url.StartsWith("www"))
      {
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(url);
        httpWebRequest.AllowWriteStreamBuffering = true;
        WebResponse response = httpWebRequest.GetResponse();
        Stream responseStream = response.GetResponseStream();
        buffer = new byte[response.ContentLength];
        using (BinaryReader binaryReader = new BinaryReader(responseStream))
          buffer = binaryReader.ReadBytes(buffer.Length);
        response.Close();
      }
      else
      {
        if (url.StartsWith("file:///"))
          url = url.Replace("file:///", string.Empty);
        FileStream fileStream = new FileStream(url, FileMode.Open, FileAccess.Read);
        buffer = new byte[fileStream.Length];
        fileStream.Read(buffer, 0, (int) fileStream.Length);
        fileStream.Dispose();
      }
    }
    catch
    {
      FileLoadException fileLoadException = new FileLoadException("Can't load image, on this url: " + url);
    }
    if (buffer != null)
      return (Stream) new MemoryStream(buffer);
    return (Stream) null;
  }
}
