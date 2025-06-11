// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.BitmapRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.Bitmap)]
[CLSCompliant(false)]
internal class BitmapRecord : BiffContinueRecordRaw
{
  private const int DEF_ALIGN = 4;
  private const int DEF_HEADER_START = 8;
  [BiffRecordPos(0, 2)]
  private ushort m_usUnknown = 9;
  [BiffRecordPos(2, 2)]
  private ushort m_usUnknown2 = 1;
  [BiffRecordPos(4, 4, true)]
  private int m_iTotalSize;
  [BiffRecordPos(8, 4, true)]
  private int m_iHeaderSize = 12;
  [BiffRecordPos(12, 2)]
  private ushort m_usWidth;
  [BiffRecordPos(14, 2)]
  private ushort m_usHeight;
  [BiffRecordPos(16 /*0x10*/, 2)]
  private ushort m_usPlanes = 1;
  [BiffRecordPos(18, 2)]
  private ushort m_usColorDepth = 24;
  private Bitmap m_bitmap;
  private IntPtr m_scan0;

  public ushort Unknown
  {
    get => this.m_usUnknown;
    set => this.m_usUnknown = value;
  }

  public ushort Unknown2
  {
    get => this.m_usUnknown2;
    set => this.m_usUnknown2 = value;
  }

  public int TotalSize
  {
    get => this.m_iTotalSize;
    set => this.m_iTotalSize = value;
  }

  public int HeaderSize
  {
    get => this.m_iHeaderSize;
    set => this.m_iHeaderSize = value;
  }

  public ushort Width
  {
    get => this.m_usWidth;
    set => this.m_usWidth = value;
  }

  public ushort Height
  {
    get => this.m_usHeight;
    set => this.m_usHeight = value;
  }

  public ushort Planes
  {
    get => this.m_usPlanes;
    set => this.m_usPlanes = value;
  }

  public ushort ColorDepth
  {
    get => this.m_usColorDepth;
    set => this.m_usColorDepth = value;
  }

  public Bitmap Picture
  {
    get => this.m_bitmap;
    [SecurityCritical] set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      if (this.m_scan0.ToInt64() != 0L)
      {
        Marshal.FreeHGlobal(this.m_scan0);
        this.m_scan0 = IntPtr.Zero;
      }
      this.m_bitmap = value;
      this.m_usWidth = (ushort) this.Picture.Width;
      this.m_usHeight = (ushort) this.Picture.Height;
    }
  }

  public BitmapRecord()
  {
  }

  public BitmapRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public BitmapRecord(int iReserve)
    : base(iReserve)
  {
  }

  [SecuritySafeCritical]
  public override void ParseStructure()
  {
    base.ParseStructure();
    this.m_usUnknown = BiffRecordRaw.GetUInt16(this.m_data, 0);
    this.m_usUnknown2 = BiffRecordRaw.GetUInt16(this.m_data, 2);
    this.m_iTotalSize = BiffRecordRaw.GetInt32(this.m_data, 4);
    this.m_iHeaderSize = BiffRecordRaw.GetInt32(this.m_data, 8);
    this.m_usWidth = BiffRecordRaw.GetUInt16(this.m_data, 12);
    this.m_usHeight = BiffRecordRaw.GetUInt16(this.m_data, 14);
    this.m_usPlanes = BiffRecordRaw.GetUInt16(this.m_data, 16 /*0x10*/);
    this.m_usColorDepth = BiffRecordRaw.GetUInt16(this.m_data, 18);
    int num1 = 3 * (int) this.m_usWidth;
    int num2 = num1 % 4;
    if (num2 != 0)
    {
      int num3 = 4 - num2;
      num1 += num3;
    }
    int num4 = this.TotalSize - this.HeaderSize;
    IntPtr destination = Marshal.AllocHGlobal(num4);
    IntPtr scan0 = (IntPtr) (destination.ToInt64() + (long) num4);
    Marshal.Copy(this.m_data, 8 + this.HeaderSize, destination, num4);
    this.m_bitmap = new Bitmap((int) this.m_usWidth, (int) this.m_usHeight, -num1, PixelFormat.Format24bppRgb, scan0);
    this.m_scan0 = destination;
  }

  [SecuritySafeCritical]
  public override void InfillInternalData(OfficeVersion version)
  {
    byte[] imageData = this.GetImageData();
    this.AutoGrowData = true;
    int length1 = imageData.Length;
    int num = this.m_iLength = length1 + this.HeaderSize + 8;
    this.m_iLength = num > this.MaximumRecordSize ? this.MaximumRecordSize : num;
    int offset = this.ManualHeaderInfill();
    this.SetBytes(offset, imageData, 0, this.m_iLength - offset);
    base.InfillInternalData(version);
    if (num <= this.MaximumRecordSize)
      return;
    int start = this.m_iLength - offset;
    int length2 = length1 - start;
    this.Builder.AppendBytes(imageData, start, length2);
    this.m_iLength = this.Builder.Total;
  }

  [SecurityCritical]
  private byte[] GetImageData()
  {
    BitmapData bitmapdata = this.m_bitmap.LockBits(new Rectangle(0, 0, this.m_bitmap.Width, this.m_bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
    int stride = bitmapdata.Stride;
    int length1 = Math.Abs(stride);
    int length2 = Math.Abs(stride) * this.m_bitmap.Height;
    this.m_iTotalSize = length2 + this.HeaderSize;
    int headerSize1 = this.HeaderSize;
    int headerSize2 = this.HeaderSize;
    byte[] destination = new byte[length2];
    IntPtr source = bitmapdata.Scan0;
    source = stride <= 0 ? (IntPtr) (source.ToInt64() - (long) length2) : (IntPtr) (source.ToInt64() + (long) length2 - (long) stride);
    int num = 0;
    int startIndex = 0;
    while (num < (int) this.m_usHeight)
    {
      Marshal.Copy(source, destination, startIndex, length1);
      source = (IntPtr) (source.ToInt64() - (long) stride);
      ++num;
      startIndex += length1;
    }
    this.m_bitmap.UnlockBits(bitmapdata);
    return destination;
  }

  private int ManualHeaderInfill()
  {
    this.SetUInt16(0, this.m_usUnknown);
    this.SetUInt16(2, this.m_usUnknown2);
    this.SetInt32(4, this.m_iTotalSize);
    this.SetInt32(8, this.m_iHeaderSize);
    this.SetUInt16(12, this.m_usWidth);
    this.SetUInt16(14, this.m_usHeight);
    this.SetUInt16(16 /*0x10*/, this.m_usPlanes);
    this.SetUInt16(18, this.m_usColorDepth);
    return 8 + this.HeaderSize;
  }
}
