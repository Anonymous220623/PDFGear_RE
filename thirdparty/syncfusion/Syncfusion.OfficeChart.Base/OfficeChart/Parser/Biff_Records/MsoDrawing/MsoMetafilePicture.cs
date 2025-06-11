// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing.MsoMetafilePicture
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.Compression;
using Syncfusion.OfficeChart.Implementation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security;
using System.Security.Cryptography;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing;

[CLSCompliant(false)]
internal class MsoMetafilePicture : MsoBase, IDisposable, IPictureRecord
{
  private const int DEF_BUFFER_SIZE = 32768 /*0x8000*/;
  private const int DEF_UID_OFFSET = 0;
  private const int DEF_METAFILE_SIZE_OFFSET = 16 /*0x10*/;
  private const int DEF_COMPRESSED_SIZE_OFFSET = 40;
  internal const uint BlipEMFWithTwoUIDs = 981;
  internal const uint BlipWMFWithTwoUIDs = 535;
  internal const uint BlipPICTWithTwoUIDs = 1347;
  internal const uint BlipTIFFWithTwoUIDs = 1765;
  private MemoryStream m_stream;
  private byte[] m_arrCompressedPicture;
  private byte[] m_arrRgbUid = new byte[16 /*0x10*/];
  private byte[] m_arrRgbUidPrimary;
  private int m_iMetafileSize;
  private Rectangle m_rcBounds;
  private Point m_ptSize;
  private int m_iSavedSize;
  private MsoBlipCompression m_compression;
  private MsoBlipFilter m_filter = MsoBlipFilter.msofilterNone;
  private Image m_picture;
  private Stream m_pictStream;

  public MsoMetafilePicture(MsoBase parent)
    : base(parent)
  {
  }

  public MsoMetafilePicture(MsoBase parent, byte[] data, int iOffset)
    : base(parent, data, iOffset)
  {
  }

  public MsoMetafilePicture(MsoBase parent, Stream stream)
    : base(parent, stream, (GetNextMsoDrawingData) null)
  {
  }

  public Image Picture
  {
    get => this.m_picture;
    [SecuritySafeCritical] set
    {
      this.m_picture = value != null ? value : throw new ArgumentNullException(nameof (Picture));
      MemoryStream metaFile = MsoMetafilePicture.SerializeMetafile(this.m_picture);
      this.m_arrCompressedPicture = this.CompressMetafile((Stream) metaFile, 0);
      metaFile.Close();
    }
  }

  public Stream PictureStream
  {
    get => this.m_pictStream;
    set
    {
      this.m_pictStream = value != null ? value : throw new ArgumentNullException(nameof (value));
    }
  }

  public byte[] RgbUid
  {
    get => this.m_arrRgbUid;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      if (value.Length != this.m_arrRgbUid.Length)
        throw new ArgumentOutOfRangeException("value.Length");
      this.m_arrRgbUid = value;
    }
  }

  public override void InfillInternalData(
    Stream stream,
    int iOffset,
    List<int> arrBreaks,
    List<List<BiffRecordRaw>> arrRecords)
  {
    this.m_rcBounds.Y = 0;
    this.m_rcBounds.X = 0;
    this.m_rcBounds.Width = this.m_picture.Width;
    this.m_rcBounds.Height = this.m_picture.Height;
    this.m_ptSize.X = (int) ApplicationImpl.ConvertFromPixel((double) this.m_picture.Width, MeasureUnits.EMU);
    this.m_ptSize.Y = (int) ApplicationImpl.ConvertFromPixel((double) this.m_picture.Height, MeasureUnits.EMU);
    if (this.MsoRecordType == (MsoRecords) 0)
    {
      this.MsoRecordType = (MsoRecords) 61466;
      this.Instance = 980;
    }
    this.m_compression = MsoBlipCompression.msoCompressionDeflate;
    int num1 = 0;
    stream.Write(this.m_arrRgbUid, 0, this.m_arrRgbUid.Length);
    int num2 = num1 + this.m_arrRgbUid.Length;
    if (this.HasTwoUIDs())
    {
      stream.Write(this.m_arrRgbUidPrimary, 0, this.m_arrRgbUidPrimary.Length);
      num2 += this.m_arrRgbUidPrimary.Length;
    }
    MsoBase.WriteInt32(stream, this.m_iMetafileSize);
    int num3 = num2 + 4;
    MsoBase.WriteInt32(stream, this.m_rcBounds.Left);
    int num4 = num3 + 4;
    MsoBase.WriteInt32(stream, this.m_rcBounds.Top);
    int num5 = num4 + 4;
    MsoBase.WriteInt32(stream, this.m_rcBounds.Right);
    int num6 = num5 + 4;
    MsoBase.WriteInt32(stream, this.m_rcBounds.Bottom);
    int num7 = num6 + 4;
    MsoBase.WriteInt32(stream, this.m_ptSize.X);
    int num8 = num7 + 4;
    MsoBase.WriteInt32(stream, this.m_ptSize.Y);
    int num9 = num8 + 4;
    MsoBase.WriteInt32(stream, this.m_iSavedSize);
    int num10 = num9 + 4;
    stream.WriteByte((byte) this.m_compression);
    int num11 = num10 + 1;
    stream.WriteByte((byte) this.m_filter);
    int num12 = num11 + 1;
    stream.Write(this.m_arrCompressedPicture, 0, this.m_arrCompressedPicture.Length);
    this.m_iLength = num12 + this.m_arrCompressedPicture.Length;
  }

  public override void ParseStructure(Stream stream)
  {
    long position = stream.Position;
    stream.Read(this.m_arrRgbUid, 0, 16 /*0x10*/);
    this.LoadPrimaryUID(stream);
    this.m_iMetafileSize = MsoBase.ReadInt32(stream);
    this.m_rcBounds = Rectangle.FromLTRB(MsoBase.ReadInt32(stream), MsoBase.ReadInt32(stream), MsoBase.ReadInt32(stream), MsoBase.ReadInt32(stream));
    this.m_ptSize = new Point(MsoBase.ReadInt32(stream), MsoBase.ReadInt32(stream));
    this.m_iSavedSize = MsoBase.ReadInt32(stream);
    this.m_compression = (MsoBlipCompression) stream.ReadByte();
    this.m_filter = (MsoBlipFilter) stream.ReadByte();
    if (this.m_stream != null)
      this.m_stream.Close();
    this.m_stream = new MemoryStream();
    int num = (int) ((long) this.m_iLength - (stream.Position - position));
    MemoryStream memoryStream = new MemoryStream(num);
    memoryStream.SetLength((long) num);
    stream.Read(memoryStream.GetBuffer(), 0, num);
    this.m_arrCompressedPicture = new byte[memoryStream.Length];
    memoryStream.Read(this.m_arrCompressedPicture, 0, (int) memoryStream.Length);
    memoryStream.Position = 0L;
    if (this.m_compression == MsoBlipCompression.msoCompressionDeflate)
    {
      CompressedStreamReader compressedStreamReader = new CompressedStreamReader((Stream) memoryStream);
      byte[] buffer = new byte[32768 /*0x8000*/];
      int count;
      while ((count = compressedStreamReader.Read(buffer, 0, buffer.Length)) > 0)
        this.m_stream.Write(buffer, 0, count);
    }
    else
      this.m_stream.Write(this.m_arrCompressedPicture, 0, this.m_arrCompressedPicture.Length);
    this.m_stream.Position = 0L;
    this.m_picture = ApplicationImpl.CreateImage((Stream) this.m_stream);
    memoryStream.Close();
  }

  private bool HasTwoUIDs()
  {
    int instance = this.Instance;
    switch (instance)
    {
      case 535:
      case 981:
      case 1347:
        return true;
      default:
        return instance == 1765;
    }
  }

  private int LoadPrimaryUID(Stream stream)
  {
    int num = 0;
    if (this.HasTwoUIDs())
    {
      this.m_arrRgbUidPrimary = new byte[16 /*0x10*/];
      stream.Read(this.m_arrRgbUidPrimary, 0, 16 /*0x10*/);
      num += 16 /*0x10*/;
    }
    return num;
  }

  [SecurityCritical]
  public static MemoryStream SerializeMetafile(Image picture)
  {
    if (picture == null)
      return (MemoryStream) null;
    MemoryStream memoryStream = new MemoryStream();
    int height = picture.Height;
    using (Bitmap bitmap = new Bitmap(picture.Width + 1, height + 1))
    {
      using (Graphics graphics1 = Graphics.FromImage((Image) bitmap))
      {
        IntPtr hdc = graphics1.GetHdc();
        GraphicsUnit pageUnit = GraphicsUnit.Pixel;
        RectangleF bounds = picture.GetBounds(ref pageUnit);
        MetafileFrameUnit metaUnit = MsoMetafilePicture.GetMetaUnit(pageUnit);
        using (Metafile metafile = new Metafile((Stream) memoryStream, hdc, bounds, metaUnit))
        {
          graphics1.ReleaseHdc(hdc);
          using (Graphics graphics2 = Graphics.FromImage((Image) metafile))
          {
            RectangleF rect = new RectangleF(bounds.X, bounds.Y, bounds.Width - 1f, bounds.Height - 1f);
            graphics2.DrawImage(picture, rect);
          }
        }
      }
    }
    memoryStream.Position = 0L;
    return memoryStream;
  }

  private static MetafileFrameUnit GetMetaUnit(GraphicsUnit unit)
  {
    switch (unit)
    {
      case GraphicsUnit.World:
        return MetafileFrameUnit.Pixel;
      case GraphicsUnit.Display:
        return MetafileFrameUnit.GdiCompatible;
      case GraphicsUnit.Pixel:
        return MetafileFrameUnit.Pixel;
      case GraphicsUnit.Point:
        return MetafileFrameUnit.Point;
      case GraphicsUnit.Inch:
        return MetafileFrameUnit.Inch;
      case GraphicsUnit.Document:
        return MetafileFrameUnit.Document;
      case GraphicsUnit.Millimeter:
        return MetafileFrameUnit.Millimeter;
      default:
        throw new Exception("The method or operation is not implemented.");
    }
  }

  private byte[] CompressMetafile(Stream metaFile, int iDataOffset)
  {
    MemoryStream outputStream = new MemoryStream();
    try
    {
      new MD5CryptoServiceProvider().ComputeHash(metaFile).CopyTo((Array) this.m_arrRgbUid, 0);
    }
    catch (InvalidOperationException ex)
    {
      new MACTripleDES().ComputeHash(metaFile).CopyTo((Array) this.m_arrRgbUid, 0);
    }
    this.m_iMetafileSize = (int) metaFile.Length;
    CompressedStreamWriter compressedStreamWriter = new CompressedStreamWriter((Stream) outputStream, CompressionLevel.Best, false);
    byte[] numArray = new byte[32768 /*0x8000*/];
    metaFile.Position = 0L;
    long length1 = metaFile.Length;
    int length2;
    while ((length2 = metaFile.Read(numArray, 0, 32768 /*0x8000*/)) > 0)
      compressedStreamWriter.Write(numArray, 0, length2, metaFile.Position + 1L >= length1);
    outputStream.Position = 0L;
    this.m_iLength = iDataOffset;
    this.m_iSavedSize = (int) outputStream.Length;
    byte[] buffer = new byte[outputStream.Length];
    outputStream.Position = 0L;
    outputStream.Read(buffer, 0, (int) outputStream.Length);
    return buffer;
  }

  protected override object InternalClone()
  {
    MsoMetafilePicture msoMetafilePicture = (MsoMetafilePicture) base.InternalClone();
    msoMetafilePicture.m_arrCompressedPicture = CloneUtils.CloneByteArray(this.m_arrCompressedPicture);
    if (this.m_stream != null)
    {
      msoMetafilePicture.m_stream = UtilityMethods.CloneStream(this.m_stream);
      msoMetafilePicture.m_stream.Position = 0L;
    }
    if (this.m_picture != null)
      msoMetafilePicture.m_picture = this.m_stream != null ? ApplicationImpl.CreateImage((Stream) msoMetafilePicture.m_stream) : (Image) this.m_picture.Clone();
    return (object) msoMetafilePicture;
  }

  protected override void OnDispose()
  {
    if (this.m_stream == null)
      return;
    this.m_stream.Close();
    this.m_stream = (MemoryStream) null;
  }

  ~MsoMetafilePicture()
  {
    if (this.m_stream == null)
      return;
    this.Dispose();
  }
}
