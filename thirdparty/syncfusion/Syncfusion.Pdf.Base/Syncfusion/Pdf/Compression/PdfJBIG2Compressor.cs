// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.PdfJBIG2Compressor
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Compression.JBIG2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.Pdf.Compression;

internal class PdfJBIG2Compressor
{
  private const int m_red = 0;
  private const int m_green = 1;
  private const int m_blue = 2;
  private const int m_alpha = 3;
  private const int m_insert = 0;
  private const int m_copy = 1;
  private const int m_clone = 2;
  private const int m_copyClone = 3;
  private const int m_jbAddedPixels = 6;
  private const int m_maxDiffWidth = 2;
  private const int m_maxDiffHeight = 2;
  private float m_threshold = 0.85f;
  private int m_bwThreshold = 188;
  private bool m_up2;
  private bool m_up4;
  private int i;
  private int pageno = -1;
  private int numsubimages;
  private int subimage;
  private int m_removeCmapToBinary;
  private int m_removeCmapToGrayScale = 1;
  private int m_removeCmapToFullColor = 2;
  private int m_removeCmapBasedOnSrc = 3;
  private JBIG2Context m_jbig2Context;
  private int MORPH_BC = 1;
  private int m_redShift = 24;
  private int m_greenShift = 16 /*0x10*/;
  private int m_blueShift = 8;
  private int m_alphaShift;
  private IntEncRange[] m_intEncRange = new IntEncRange[13]
  {
    new IntEncRange(0, 3, 0, 2, (short) 0, 2),
    new IntEncRange(-1, -1, 9, 4, (short) 0, 0),
    new IntEncRange(-3, -2, 5, 3, (short) 2, 1),
    new IntEncRange(4, 19, 2, 3, (short) 4, 4),
    new IntEncRange(-19, -4, 3, 3, (short) 4, 4),
    new IntEncRange(20, 83, 6, 4, (short) 20, 6),
    new IntEncRange(-83, -20, 7, 4, (short) 20, 6),
    new IntEncRange(84, 339, 14, 5, (short) 84, 8),
    new IntEncRange(-339, -84, 15, 5, (short) 84, 8),
    new IntEncRange(340, 4435, 30, 6, (short) 340, 12),
    new IntEncRange(-4435, -340, 31 /*0x1F*/, 6, (short) 340, 12),
    new IntEncRange(4436, 2000000000, 62, 6, (short) 4436, 32 /*0x20*/),
    new IntEncRange(-2000000000, -4436, 63 /*0x3F*/, 6, (short) 4436, 32 /*0x20*/)
  };
  private List<byte> m_symbolArray;
  private List<byte> m_symbolPage;

  internal List<byte> SymbolArray
  {
    get => this.m_symbolArray;
    set => this.m_symbolArray = value;
  }

  internal List<byte> SymbolPage
  {
    get => this.m_symbolPage;
    set => this.m_symbolPage = value;
  }

  internal void Dispose()
  {
    if (this.m_symbolPage != null)
      this.m_symbolPage.Clear();
    if (this.m_symbolArray == null)
      return;
    this.m_symbolArray.Clear();
  }

  internal PdfJBIG2Compressor(object file)
  {
    int num1 = 0;
    this.Initialize();
    Pix pixs1 = this.PixRead(file);
    if (pixs1 == null)
      return;
    Pix pixs2;
    if ((pixs2 = this.PixRemoveColormap(pixs1, this.m_removeCmapBasedOnSrc)) == null)
    {
      Console.WriteLine("Failed to remove colormap");
    }
    else
    {
      ++this.pageno;
      Pix pix;
      if (pixs2.D > 1)
      {
        Pix pixs3;
        if (pixs2.D > 8)
        {
          pixs3 = this.PixConvertRGBToGrayFast(pixs2);
          if (pixs3 == null)
            return;
        }
        else
          pixs3 = pixs2;
        pix = !this.m_up2 ? (!this.m_up4 ? this.PixThresholdToBinary(pixs3, this.m_bwThreshold) : this.PixScaleGray4xLIThresh(pixs3, this.m_bwThreshold)) : this.PixScaleGray2xLIThresh(pixs3, this.m_bwThreshold);
      }
      else
        pix = pixs2;
      this.AddPage(this.m_jbig2Context, pix);
      int num2 = num1 + 1;
      int length = -1;
      this.SymbolArray = this.JBIG2PagesComplete(this.m_jbig2Context);
      for (int page_no = 0; page_no < num2; ++page_no)
        this.SymbolPage = this.JBIG2ProducePage(this.m_jbig2Context, page_no, -1, -1, ref length);
    }
  }

  private Pix PixRead(object file) => this.PixReadStreamTiff(file);

  private Pix PixReadStreamTiff(object file)
  {
    switch (file)
    {
      case Stream _:
        using (Stream stream = file as Stream)
        {
          stream.Position = 0L;
          byte[] buffer = new byte[stream.Length];
          stream.Read(buffer, 0, buffer.Length);
          using (MemoryStream clientData = new MemoryStream(buffer))
          {
            using (Tiff tiff = Tiff.ClientOpen("in-memory", "r", (object) clientData, new TiffStream()))
              return this.ReadTiff(tiff);
          }
        }
      case string _:
        using (Tiff tiff = Tiff.Open(file as string, "r"))
          return this.ReadTiff(tiff);
      default:
        return (Pix) null;
    }
  }

  private Pix ReadTiff(Tiff tiff)
  {
    if (tiff == null)
      return (Pix) null;
    int num1 = tiff.GetField(Syncfusion.Pdf.Compression.JBIG2.TiffTag.IMAGEWIDTH)[0].ToInt();
    int num2 = tiff.GetField(Syncfusion.Pdf.Compression.JBIG2.TiffTag.IMAGELENGTH)[0].ToInt();
    int depth = tiff.GetField(Syncfusion.Pdf.Compression.JBIG2.TiffTag.BITSPERSAMPLE)[0].ToInt();
    int num3 = tiff.GetField(Syncfusion.Pdf.Compression.JBIG2.TiffTag.SAMPLESPERPIXEL)[0].ToInt();
    int num4 = tiff.GetField(Syncfusion.Pdf.Compression.JBIG2.TiffTag.XRESOLUTION)[0].ToInt();
    int num5 = tiff.GetField(Syncfusion.Pdf.Compression.JBIG2.TiffTag.YRESOLUTION)[0].ToInt();
    int tiffcomp = tiff.GetField(Syncfusion.Pdf.Compression.JBIG2.TiffTag.COMPRESSION)[0].ToInt();
    int num6 = 0;
    PlanarConfig planarConfig = (PlanarConfig) tiff.GetField(Syncfusion.Pdf.Compression.JBIG2.TiffTag.PLANARCONFIG)[0].ToInt();
    if (depth * num3 > 32 /*0x20*/)
      throw new Exception();
    switch (num3)
    {
      case 1:
        num6 = depth;
        break;
      case 3:
      case 4:
        num6 = 32 /*0x20*/;
        break;
    }
    int num7 = (num1 * num6 + 31 /*0x1F*/) / 32 /*0x20*/;
    int length1 = 4 * num7;
    Pix pixs = new Pix();
    pixs.W = num1;
    pixs.H = num2;
    pixs.D = num6;
    pixs.Wpl = num7;
    pixs.XRes = num4;
    pixs.YRes = num5;
    pixs.Informat = this.GetTiffCompressedFormat(tiffcomp);
    if (num3 != 1)
      return (Pix) null;
    tiff.ScanlineSize();
    uint[] src = new uint[num2 * num7];
    int num8 = 0;
    for (int row = 0; row < num2; ++row)
    {
      byte[] buffer = new byte[length1];
      int num9;
      if (tiff.ReadScanline(buffer, row, (short) 0))
      {
        for (int index = 0; index < buffer.Length; index = num9 + 1)
        {
          int num10;
          int num11;
          byte[] numArray = new byte[4]
          {
            buffer[index],
            buffer[num10 = index + 1],
            buffer[num11 = num10 + 1],
            buffer[num9 = num11 + 1]
          };
          src[num8++] = BitConverter.ToUInt32(numArray, 0);
        }
      }
    }
    if (depth <= 8)
      this.PixEndianByteSwap(pixs, src);
    else
      this.PixEndianTwoByteSwap(pixs, src);
    FieldValue[] field1 = tiff.GetField(Syncfusion.Pdf.Compression.JBIG2.TiffTag.COLORMAP);
    if (field1 != null)
    {
      short[] shortArray1 = field1[0].ToShortArray();
      short[] shortArray2 = field1[1].ToShortArray();
      short[] shortArray3 = field1[2].ToShortArray();
      int length2 = 1 << depth;
      ushort[] dst1 = new ushort[length2];
      ushort[] dst2 = new ushort[length2];
      ushort[] dst3 = new ushort[length2];
      Buffer.BlockCopy((Array) shortArray1, 0, (Array) dst1, 0, length2 * 2);
      Buffer.BlockCopy((Array) shortArray2, 0, (Array) dst2, 0, length2 * 2);
      Buffer.BlockCopy((Array) shortArray3, 0, (Array) dst3, 0, length2 * 2);
      if (planarConfig != PlanarConfig.CONTIG || num3 == 1 || depth >= 8)
      {
        PixColormap pixCmap = JBIG2Statics.CreatePixCmap(depth);
        int num12 = 1 << depth;
        for (this.i = 0; this.i < num12; ++this.i)
          this.PixCmapAddColor(ref pixCmap, (int) dst1[this.i] >> 8, (int) dst2[this.i] >> 8, (int) dst3[this.i] >> 8);
        pixs.Colormap = pixCmap;
      }
    }
    FieldValue[] field2 = tiff.GetField(Syncfusion.Pdf.Compression.JBIG2.TiffTag.ORIENTATION);
    if (field2 != null)
    {
      if (field2[0].ToInt() >= 1)
        ;
    }
    return pixs;
  }

  private void PixEndianByteSwap(Pix pixs, uint[] src)
  {
    if (!BitConverter.IsLittleEndian)
      return;
    int wpl = pixs.Wpl;
    int h = pixs.H;
    List<uint> uintList = new List<uint>();
    int index1 = 0;
    for (int index2 = 0; index2 < h; ++index2)
    {
      int num1 = 0;
      while (num1 < wpl)
      {
        uint num2 = src[index1];
        uint num3 = (uint) ((int) (num2 >> 24) | (int) (num2 >> 8) & 65280 | (int) num2 << 8 & 16711680 /*0xFF0000*/ | (int) num2 << 24);
        uintList.Add(num3);
        ++num1;
        ++index1;
      }
    }
    pixs.Data = uintList.ToArray();
  }

  private void PixEndianTwoByteSwap(Pix pixs, uint[] src)
  {
    if (!BitConverter.IsLittleEndian)
      return;
    int wpl = pixs.Wpl;
    int h = pixs.H;
    uint[] numArray = new uint[h * wpl];
    for (int index = 0; index < h * wpl; ++index)
    {
      uint num = src[index];
      numArray[index] = num << 16 /*0x10*/ | num >> 16 /*0x10*/;
    }
    pixs.Data = numArray;
  }

  private int GetTiffCompressedFormat(int tiffcomp)
  {
    int compressedFormat;
    switch (tiffcomp)
    {
      case 2:
        compressedFormat = 6;
        break;
      case 3:
        compressedFormat = 7;
        break;
      case 4:
        compressedFormat = 8;
        break;
      case 5:
        compressedFormat = 9;
        break;
      case 8:
        compressedFormat = 10;
        break;
      case 32773:
        compressedFormat = 5;
        break;
      default:
        compressedFormat = 4;
        break;
    }
    return compressedFormat;
  }

  private int FindFileFormatStream(object fp)
  {
    Stream stream = fp as Stream;
    Image image = !(fp is string) ? Image.FromStream(stream) : Image.FromFile(fp as string);
    int fileFormatStream;
    if (ImageFormat.Jpeg.Equals((object) image.RawFormat))
      fileFormatStream = 2;
    else if (ImageFormat.Png.Equals((object) image.RawFormat))
      fileFormatStream = 3;
    else if (ImageFormat.Gif.Equals((object) image.RawFormat))
      fileFormatStream = 13;
    else if (ImageFormat.Tiff.Equals((object) image.RawFormat))
    {
      fileFormatStream = 4;
    }
    else
    {
      if (!ImageFormat.Bmp.Equals((object) image.RawFormat))
        return 1;
      fileFormatStream = 1;
    }
    image.Dispose();
    return fileFormatStream;
  }

  private void Initialize()
  {
    this.m_jbig2Context = new JBIG2Context(this.m_threshold, 0.5f, 0, 0, false);
  }

  private int CompareCharArray(int[] buf, char[] comp)
  {
    for (int index = 0; index < buf.Length; ++index)
    {
      if ((int) (ushort) buf[index] != (int) comp[index])
        return 0;
    }
    return 1;
  }

  private List<byte> JBIG2ProducePage(
    JBIG2Context ctx,
    int page_no,
    int xres,
    int yres,
    ref int length)
  {
    bool flag1 = page_no == ctx.Classifier.NPages && ctx.FullHeaders;
    int num1 = 51;
    int num2 = 6;
    int num3 = 49;
    JBIG2EncoderContext ctx1 = new JBIG2EncoderContext();
    this.Initialize(ref ctx1);
    Segment x1 = new Segment();
    Segment x2 = new Segment();
    Segment x3 = new Segment();
    Segment x4 = new Segment();
    JBIG2PageInfo jbiG2PageInfo = new JBIG2PageInfo();
    JBIG2TextRegion jbiG2TextRegion = new JBIG2TextRegion();
    JBIG2TextRegionSym jbiG2TextRegionSym = new JBIG2TextRegionSym();
    JBIG2TextRegionAtFlags textRegionAtFlags = new JBIG2TextRegionAtFlags();
    Segment x5 = new Segment();
    x1.Number = (uint) ctx.SegNumber;
    ++ctx.SegNumber;
    x1.SType = 48 /*0x30*/;
    x1.Page = ctx.PDFPageNumbering ? 1U : (uint) (1 + page_no);
    x1.Length = 19U;
    jbiG2PageInfo.Width = JBIG2Statics.Htonl((object) ctx.PageWidth[page_no]);
    jbiG2PageInfo.Height = JBIG2Statics.Htonl((object) ctx.PageHeight[page_no]);
    jbiG2PageInfo.XRes = JBIG2Statics.Htonl((object) (xres == -1 ? ctx.PageXRes[page_no] : xres));
    jbiG2PageInfo.YRes = JBIG2Statics.Htonl((object) (yres == -1 ? ctx.PageYRes[page_no] : yres));
    jbiG2PageInfo.IsLossless = ctx.Refinement ? (byte) 1 : (byte) 0;
    SortedDictionary<int, int> symmap = new SortedDictionary<int, int>();
    bool flag2 = ctx.SingleUseSymbols.ContainsKey(page_no) && ctx.SingleUseSymbols[page_no].Count > 0;
    JBIG2EncoderContext ctx2 = new JBIG2EncoderContext();
    JBIG2SymbolDict jbiG2SymbolDict = new JBIG2SymbolDict();
    if (flag2)
    {
      this.Initialize(ref ctx2);
      x2.Number = (uint) ctx.SegNumber++;
      x2.SType = 0;
      x2.Page = ctx.PDFPageNumbering ? 1U : (uint) (1 + page_no);
      List<int> singleUseSymbol = ctx.SingleUseSymbols[page_no];
      this.JBIG2SymbolTable(ref ctx2, ctx.AvgTemplates != null ? ctx.AvgTemplates : ctx.Classifier.Pixat, ref singleUseSymbol, ref symmap, ctx.AvgTemplates == null);
      ctx.SingleUseSymbols[page_no] = singleUseSymbol;
      jbiG2SymbolDict.a1x = (sbyte) 3;
      jbiG2SymbolDict.a1y = (sbyte) -1;
      jbiG2SymbolDict.a2x = (sbyte) -3;
      jbiG2SymbolDict.a2y = (sbyte) -1;
      jbiG2SymbolDict.a3x = (sbyte) 2;
      jbiG2SymbolDict.a3y = (sbyte) -2;
      jbiG2SymbolDict.a4x = (sbyte) -2;
      jbiG2SymbolDict.a4y = (sbyte) -2;
      jbiG2SymbolDict.ExSyms = jbiG2SymbolDict.NewSyms = JBIG2Statics.Htonl((object) ctx.SingleUseSymbols[page_no].Count);
      x2.Length = (uint) (byte) (this.JBIG2EncSize(ctx2) + Marshal.SizeOf<JBIG2SymbolDict>(jbiG2SymbolDict));
    }
    int v = ctx.NumGlobalSymbols + (ctx.SingleUseSymbols.ContainsKey(page_no) ? ctx.SingleUseSymbols[page_no].Count : 0);
    int baseIndex = ctx.Refinement ? (ctx.BaseIndexes.Contains(page_no) ? ctx.BaseIndexes[page_no] : 0) : 0;
    this.JBIG2EncText(ctx1, ctx.SymbolMap, symmap, ctx.PageComps[page_no], ctx.Classifier.PtaLL, ctx.AvgTemplates != null ? ctx.AvgTemplates : ctx.Classifier.Pixat, ctx.Classifier.NaClass, 1, this.Log2Up(v), (Pixa) null, (Boxa) null, baseIndex, ctx.RefineLevel, ctx.AvgTemplates == null);
    int num4 = this.JBIG2EncSize(ctx1);
    jbiG2TextRegion.Width = JBIG2Statics.Htonl((object) ctx.PageWidth[page_no]);
    jbiG2TextRegion.Height = JBIG2Statics.Htonl((object) ctx.PageHeight[page_no]);
    jbiG2TextRegion.LogSBStrips = (byte) 0;
    jbiG2TextRegion.SBRefine = ctx.Refinement ? (byte) 1 : (byte) 0;
    jbiG2TextRegionSym.SBNumInstances = JBIG2Statics.Htonl((object) ctx.PageComps[page_no].Count);
    textRegionAtFlags.a1x = -1;
    textRegionAtFlags.a1y = -1;
    textRegionAtFlags.a2x = -1;
    textRegionAtFlags.a2y = -1;
    x5.Number = (uint) ctx.SegNumber;
    ++ctx.SegNumber;
    x5.SType = (int) (byte) num2;
    x5.ReferredTo.Add(ctx.SymbolTableSegment);
    if (flag2)
      x5.ReferredTo.Add((int) x2.Number);
    x5.Length = !ctx.Refinement ? (uint) (23 + num4) : (uint) (Marshal.SizeOf<JBIG2TextRegion>(jbiG2TextRegion) + Marshal.SizeOf<JBIG2TextRegionSym>(jbiG2TextRegionSym) + Marshal.SizeOf<JBIG2TextRegionAtFlags>(textRegionAtFlags) + num4);
    x5.RetainBits = 2;
    x5.Page = ctx.PDFPageNumbering ? 1U : (uint) (1 + page_no);
    int num5 = flag2 ? this.JBIG2EncSize(ctx2) : 0;
    if (ctx.FullHeaders)
    {
      x3.Number = (uint) ctx.SegNumber;
      ++ctx.SegNumber;
      x3.SType = num3;
      x3.Page = ctx.PDFPageNumbering ? 1U : (uint) (1 + page_no);
    }
    if (flag1)
    {
      x4.Number = (uint) ctx.SegNumber;
      ++ctx.SegNumber;
      x4.SType = num1;
      x4.Page = 0U;
    }
    int length1 = (int) x1.Length;
    Marshal.SizeOf<JBIG2PageInfo>(jbiG2PageInfo);
    if (flag2)
    {
      int length2 = (int) x2.Length;
      Marshal.SizeOf<JBIG2SymbolDict>(jbiG2SymbolDict);
    }
    int length3 = (int) x5.Length;
    Marshal.SizeOf<JBIG2TextRegion>(jbiG2TextRegion);
    Marshal.SizeOf<JBIG2TextRegionSym>(jbiG2TextRegionSym);
    if (ctx.Refinement)
      Marshal.SizeOf<JBIG2TextRegionAtFlags>(textRegionAtFlags);
    if (ctx.FullHeaders)
    {
      int length4 = (int) x3.Length;
    }
    if (flag1)
    {
      int length5 = (int) x4.Length;
    }
    List<byte> byteList = new List<byte>();
    int offset = 0;
    this.UpdateSegment(x1, ref offset, ref byteList);
    this.Fill((object) jbiG2PageInfo, ref offset, ref byteList);
    if (flag2)
    {
      this.UpdateSegment(x2, ref offset, ref byteList);
      this.Fill((object) jbiG2SymbolDict, ref offset, ref byteList);
      this.JBIG2EncBuffer(ctx2, ref byteList);
      offset += num5;
    }
    this.UpdateSegment(x5, ref offset, ref byteList);
    this.Fill((object) jbiG2TextRegion, ref offset, ref byteList);
    if (ctx.Refinement)
      this.Fill((object) textRegionAtFlags, ref offset, ref byteList);
    this.Fill((object) jbiG2TextRegionSym, ref offset, ref byteList);
    this.JBIG2EncBuffer(ctx1, ref byteList);
    offset = byteList.Count;
    if (ctx.FullHeaders)
      this.UpdateSegment(x3, ref offset, ref byteList);
    if (flag1)
      this.UpdateSegment(x4, ref offset, ref byteList);
    if (offset == 0)
      return (List<byte>) null;
    ctx1 = (JBIG2EncoderContext) null;
    if (flag2)
      ctx2 = (JBIG2EncoderContext) null;
    length = offset;
    return byteList;
  }

  private int Log2Up(int v)
  {
    int num = 0;
    bool flag = (v & v - 1) == 0;
    while ((v >>= 1) > 0)
      ++num;
    return flag ? num : num + 1;
  }

  private void UpdateSegment(Segment x, ref int offset, ref List<byte> ret)
  {
    x.Write(ret);
    offset = ret.Count;
  }

  private void JBIG2EncText(
    JBIG2EncoderContext ctx,
    SortedDictionary<int, int> symmap,
    SortedDictionary<int, int> symmap2,
    List<int> comps,
    Pta in_ll,
    Pixa symbols,
    Numa assignments,
    int stripwidth,
    int symbits,
    Pixa source,
    Boxa boxes,
    int baseindex,
    int refine_level,
    bool unborder_symbols)
  {
    int proc1 = 2;
    int proc2 = 3;
    int proc3 = 6;
    int proc4 = 7;
    int proc5 = 12;
    if (stripwidth != 1 && stripwidth != 2 && stripwidth != 4 && stripwidth != 8)
      return;
    Pta pta;
    if (source != null)
    {
      pta = JBIG2Statics.CreatePta(0);
      for (int index = 0; index < boxes.N; ++index)
        this.PtaAddPt(pta, (float) boxes.Box[index].X, (float) (boxes.Box[index].Y + boxes.Box[index].H - 1));
    }
    else
      pta = in_ll;
    int count = comps.Count;
    List<int> list1 = comps;
    this.HeightSorter(ref list1, pta);
    int num1 = 0;
    int num2 = 0;
    this.IntegerEncoder(ctx, proc2, 0);
    List<int> list2 = new List<int>();
    int index1;
    for (int index2 = 0; index2 < count; index2 = index1)
    {
      int num3 = (int) ((double) pta.Y[list1[index2]] / (double) stripwidth) * stripwidth;
      list2.Clear();
      list2.Add(list1[index2]);
      for (index1 = index2 + 1; index1 < count; ++index1)
      {
        if ((double) pta.Y[list1[index1]] < (double) num3)
          return;
        if ((double) pta.Y[list1[index1]] < (double) (num3 + stripwidth))
          list2.Add(list1[index1]);
        else
          break;
      }
      this.WidthSorter(ref list2, pta);
      int num4 = num3 - num1;
      this.IntegerEncoder(ctx, proc2, num4 / stripwidth);
      num1 = num3;
      bool flag = true;
      int num5 = 0;
      foreach (int index3 in list2)
      {
        if (flag)
        {
          flag = false;
          int num6 = (int) ((double) pta.X[index3] - (double) num2);
          this.IntegerEncoder(ctx, proc3, num6);
          num2 += num6;
          num5 = num2;
        }
        else
        {
          int num7 = (int) ((double) pta.X[index3] - (double) num5);
          this.IntegerEncoder(ctx, proc1, num7);
          num5 += num7;
        }
        if (stripwidth > 1)
        {
          int num8 = (int) ((double) pta.Y[index3] - (double) num1);
          this.IntegerEncoder(ctx, proc4, num8);
        }
        int num9 = (int) assignments.Array[index3 + (source != null ? baseindex : 0)];
        int num10;
        if (symmap.ContainsKey(num9))
        {
          num10 = symmap[num9];
        }
        else
        {
          if (!symmap2.ContainsKey(num9))
            return;
          num10 = symmap2[num9] + symmap.Count;
        }
        this.JBIG2EncIaid(ctx, symbits, num10);
        if (source != null)
        {
          int index4 = baseindex + index3;
          Pix pix1 = !unborder_symbols ? symbols.Pix[num9] : this.PixRemoveBorder(symbols.Pix[num9], 6);
          this.PixSetPadBits(ref pix1, 0);
          int w1 = boxes.Box[index3].W;
          int h1 = boxes.Box[index3].H;
          int x = boxes.Box[index3].X;
          int y = boxes.Box[index3].Y;
          int num11 = (int) ((double) in_ll.Y[index4] - (double) pix1.H) + 1;
          int num12 = (int) in_ll.X[index4];
          int w2 = pix1.W;
          int h2 = pix1.H;
          int dx = x - num12;
          int dy = y - num11;
          Pix pix2 = source.Pix[index3];
          this.PixSetPadBits(ref pix2, 0);
          source.Pix[index3] = pix2;
          Pix pix3 = this.PixCopy((Pix) null, source.Pix[index3]);
          this.PixRasterop(pix3, dx, dy, pix1.W, pix1.H, JBIG2Statics.PixSrc ^ JBIG2Statics.PixDst, pix1, 0, 0);
          int pcount = 0;
          this.PixCountPixels(pix3, ref pcount, (int[]) null);
          if (pcount <= refine_level || dx < -1 || dx > 1)
          {
            this.IntegerEncoder(ctx, proc5, 0);
            num5 += symbols.Pix[num9].W - (unborder_symbols ? 12 : 0) - 1;
          }
        }
        else
          num5 += symbols.Pix[num9].W - (unborder_symbols ? 12 : 0) - 1;
      }
      this.JBIG2EncOob(ctx, proc1);
    }
    this.JBIG2EncFinal(ctx);
  }

  private void JBIG2EncFinal(JBIG2EncoderContext ctx)
  {
    int num = ctx.C + ctx.A;
    ctx.C |= (int) ushort.MaxValue;
    if (ctx.C >= num)
      ctx.C -= 32768 /*0x8000*/;
    ctx.C <<= ctx.CT;
    this.ByteOut(ctx);
    ctx.C <<= ctx.CT;
    this.ByteOut(ctx);
    this.Flush(ctx);
    if (ctx.B != byte.MaxValue)
    {
      ctx.B = byte.MaxValue;
      this.Flush(ctx);
    }
    ctx.B = (byte) 172;
    this.Flush(ctx);
  }

  private void ByteOut(JBIG2EncoderContext ctx)
  {
    if (ctx.B != byte.MaxValue)
    {
      if (ctx.C >= 134217728 /*0x08000000*/)
      {
        ++ctx.B;
        if (ctx.B == byte.MaxValue)
        {
          ctx.C &= 134217727 /*0x07FFFFFF*/;
          goto label_4;
        }
      }
      if (ctx.BP >= 0)
        this.Flush(ctx);
      ctx.B = (byte) (ctx.C >> 19);
      ++ctx.BP;
      ctx.C &= 524287 /*0x07FFFF*/;
      ctx.CT = 8;
      return;
    }
label_4:
    if (ctx.BP >= 0)
      this.Flush(ctx);
    ctx.B = (byte) (ctx.C >> 20);
    ++ctx.BP;
    ctx.C &= 1048575 /*0x0FFFFF*/;
    ctx.CT = 7;
  }

  private void Flush(JBIG2EncoderContext ctx)
  {
    int length = 20480 /*0x5000*/;
    if (ctx.OutbufUsed == length)
    {
      ctx.OutputChunks.Add(ctx.OutputChunks.Count, ctx.Outbuf);
      ctx.OutbufUsed = 0;
      ctx.Outbuf = new byte[length];
    }
    ctx.Outbuf[ctx.OutbufUsed++] = ctx.B;
  }

  private void JBIG2EncIaid(JBIG2EncoderContext ctx, int symcodelen, int value)
  {
    if (ctx.Iaidctx == null | ctx.Iaidctx.Count == 0)
      ctx.Iaidctx = new List<int>(1 << symcodelen);
    uint num1 = (uint) ((1 << symcodelen + 1) - 1);
    value <<= 32 /*0x20*/ - symcodelen;
    uint num2 = 1;
    for (int index = 0; index < symcodelen; ++index)
    {
      uint ctxnum = num2 & num1;
      uint d = (uint) ((ulong) value & 2147483648UL /*0x80000000*/) >> 31 /*0x1F*/;
      List<int> iaidctx = ctx.Iaidctx;
      this.EncodeBit(ctx, ref iaidctx, ctxnum, d);
      ctx.Iaidctx = iaidctx;
      num2 = num2 << 1 | d;
      value <<= 1;
    }
  }

  private void Initialize(ref JBIG2EncoderContext ctx)
  {
    ctx.A = 32768 /*0x8000*/;
    ctx.C = 0;
    ctx.CT = 12;
    ctx.BP = -1;
    ctx.B = (byte) 0;
    ctx.OutbufUsed = 0;
    ctx.OutputChunks = new Dictionary<int, byte[]>();
  }

  private List<byte> JBIG2PagesComplete(JBIG2Context ctx)
  {
    int num1 = 0;
    bool flag = ctx.Classifier.NPages == 1;
    List<int> intList1 = new List<int>(ctx.Classifier.Pixat.N);
    for (int index1 = 0; index1 < ctx.Classifier.NaClass.N; ++index1)
    {
      int pival = 0;
      this.NumaGetIValue(ctx.Classifier.NaClass, index1, ref pival);
      while (intList1.Count <= pival)
        intList1.Add(0);
      List<int> intList2;
      int index2;
      (intList2 = intList1)[index2 = pival] = intList2[index2] + 1;
    }
    List<int> symbol_list = new List<int>();
    for (int index = 0; index < ctx.Classifier.Pixat.N && intList1[index] != 0; ++index)
    {
      if (intList1[index] > 1 || flag)
        symbol_list.Add(index);
    }
    ctx.NumGlobalSymbols = symbol_list.Count;
    for (int index = 0; index < ctx.Classifier.NaPage.N; ++index)
    {
      int pival1 = 0;
      this.NumaGetIValue(ctx.Classifier.NaPage, index, ref pival1);
      if (ctx.PageComps.ContainsKey(pival1))
        ctx.PageComps[pival1].Add(index);
      else
        ctx.PageComps.Add(pival1, new List<int>() { index });
      int pival2 = 0;
      this.NumaGetIValue(ctx.Classifier.NaClass, index, ref pival2);
      if (intList1[pival2] == 1 && !flag)
        ctx.SingleUseSymbols.Add(pival1, new List<int>()
        {
          pival2
        });
    }
    this.JBGetLLCorners(ctx.Classifier);
    JBIG2EncoderContext ctx1 = new JBIG2EncoderContext();
    JBIG2FileHeader x1 = new JBIG2FileHeader();
    byte[] numArray = new byte[8]
    {
      (byte) 151,
      (byte) 74,
      (byte) 66,
      (byte) 50,
      (byte) 13,
      (byte) 10,
      (byte) 26,
      (byte) 10
    };
    if (ctx.FullHeaders)
    {
      x1.NPages = JBIG2Statics.Htonl((object) ctx.Classifier.NPages);
      x1.OrganisationType = (byte) 1;
      x1.Id = new byte[numArray.Length];
      int index = 0;
      foreach (byte num2 in numArray)
      {
        x1.Id[index] = (byte) Convert.ToInt32(num2);
        ++index;
      }
    }
    Segment x2 = new Segment();
    JBIG2SymbolDict x3 = new JBIG2SymbolDict();
    SortedDictionary<int, int> symmap = new SortedDictionary<int, int>();
    ctx.SymbolMap = symmap;
    this.JBIG2SymbolTable(ref ctx1, ctx.AvgTemplates != null ? ctx.AvgTemplates : ctx.Classifier.Pixat, ref symbol_list, ref symmap, ctx.AvgTemplates == null);
    int num3 = this.JBIG2EncSize(ctx1);
    x3.a1x = (sbyte) 3;
    x3.a1y = (sbyte) -1;
    x3.a2x = (sbyte) -3;
    x3.a2y = (sbyte) -1;
    x3.a3x = (sbyte) 2;
    x3.a3y = (sbyte) -2;
    x3.a4x = (sbyte) -2;
    x3.a4y = (sbyte) -2;
    x3.ExSyms = x3.NewSyms = JBIG2Statics.Htonl((object) symbol_list.Count);
    ctx.SymbolTableSegment = ctx.SegNumber;
    x2.Number = (uint) ctx.SegNumber;
    ++ctx.SegNumber;
    x2.SType = num1;
    x2.Length = (uint) (18 + num3);
    x2.Page = 0U;
    x2.RetainBits = 1;
    List<byte> byteList = new List<byte>();
    int offset = 0;
    if (ctx.FullHeaders)
      this.Fill((object) x1, ref offset, ref byteList);
    this.UpdateSegment(x2, ref offset, ref byteList);
    this.Fill((object) x3, ref offset, ref byteList);
    this.JBIG2EncBuffer(ctx1, ref byteList);
    offset += num3;
    return byteList;
  }

  private void JBIG2EncBuffer(JBIG2EncoderContext ctx, ref List<byte> buffer)
  {
    foreach (KeyValuePair<int, byte[]> outputChunk in ctx.OutputChunks)
      buffer.AddRange((IEnumerable<byte>) outputChunk.Value);
    byte[] numArray1 = new byte[ctx.OutbufUsed];
    Array.Copy((Array) ctx.Outbuf, (Array) numArray1, ctx.OutbufUsed);
    buffer.AddRange((IEnumerable<byte>) numArray1);
    byte[] numArray2 = new byte[1];
  }

  private void Fill(object x, ref int offset, ref List<byte> ret)
  {
    if (x is JBIG2FileHeader jbiG2FileHeader)
    {
      foreach (char ch in jbiG2FileHeader.Id)
        ret.Add((byte) ch);
      ret.Add(jbiG2FileHeader.OrganisationType);
      ret.AddRange((IEnumerable<byte>) BitConverter.GetBytes(jbiG2FileHeader.NPages));
    }
    if (x is JBIG2SymbolDict jbiG2SymbolDict)
    {
      ret.Add(jbiG2SymbolDict.sdrtemplate);
      ret.Add(jbiG2SymbolDict.sdtemplate);
      ret.Add((byte) jbiG2SymbolDict.a1x);
      ret.Add((byte) jbiG2SymbolDict.a1y);
      ret.Add((byte) jbiG2SymbolDict.a2x);
      ret.Add((byte) jbiG2SymbolDict.a2y);
      ret.Add((byte) jbiG2SymbolDict.a3x);
      ret.Add((byte) jbiG2SymbolDict.a3y);
      ret.Add((byte) jbiG2SymbolDict.a4x);
      ret.Add((byte) jbiG2SymbolDict.a4y);
      ret.AddRange((IEnumerable<byte>) BitConverter.GetBytes(jbiG2SymbolDict.ExSyms));
      ret.AddRange((IEnumerable<byte>) BitConverter.GetBytes(jbiG2SymbolDict.NewSyms));
    }
    if (x is JBIG2PageInfo jbiG2PageInfo)
    {
      ret.AddRange((IEnumerable<byte>) BitConverter.GetBytes(jbiG2PageInfo.Width));
      ret.AddRange((IEnumerable<byte>) BitConverter.GetBytes(jbiG2PageInfo.Height));
      ret.AddRange((IEnumerable<byte>) BitConverter.GetBytes(jbiG2PageInfo.XRes));
      ret.AddRange((IEnumerable<byte>) BitConverter.GetBytes(jbiG2PageInfo.YRes));
      ret.Add(jbiG2PageInfo.IsLossless);
      ret.Add(jbiG2PageInfo.ContainsRefinements);
      ret.Add(jbiG2PageInfo.DefaultPixel);
    }
    if (x is JBIG2TextRegion jbiG2TextRegion)
    {
      ret.AddRange((IEnumerable<byte>) BitConverter.GetBytes(jbiG2TextRegion.Width));
      ret.AddRange((IEnumerable<byte>) BitConverter.GetBytes(jbiG2TextRegion.Height));
      ret.AddRange((IEnumerable<byte>) BitConverter.GetBytes(jbiG2TextRegion.X));
      ret.AddRange((IEnumerable<byte>) BitConverter.GetBytes(jbiG2TextRegion.Y));
      ret.Add(jbiG2TextRegion.LogSBStrips);
      ret.Add(jbiG2TextRegion.SBRefine);
      ret.Add(jbiG2TextRegion.Sbrtemplate);
    }
    if (x is JBIG2TextRegionSym jbiG2TextRegionSym)
      ret.AddRange((IEnumerable<byte>) BitConverter.GetBytes(jbiG2TextRegionSym.SBNumInstances));
    offset = ret.Count;
  }

  private int JBIG2EncSize(JBIG2EncoderContext ctx)
  {
    return 20480 /*0x5000*/ * ctx.OutputChunks.Count + ctx.OutbufUsed;
  }

  private void JBIG2SymbolTable(
    ref JBIG2EncoderContext ctx,
    Pixa symbols,
    ref List<int> symbol_list,
    ref SortedDictionary<int, int> symmap,
    bool unborder_symbols)
  {
    int count = symbol_list.Count;
    int num1 = 0;
    int npix = 6;
    int proc1 = 4;
    int proc2 = 5;
    int proc3 = 1;
    List<int> list1 = symbol_list;
    this.HeightSorter(ref list1, symbols.Pix);
    List<int> list2 = new List<int>();
    int num2 = 0;
    int index1;
    for (int index2 = 0; index2 < count; index2 = index1)
    {
      int my = symbols.Pix[list1[index2]].H - (unborder_symbols ? 2 * npix : 0);
      list2.Clear();
      list2.Add(list1[index2]);
      for (index1 = index2 + 1; index1 < count && symbols.Pix[list1[index1]].H - (unborder_symbols ? 2 * npix : 0) == my; ++index1)
        list2.Add(list1[index1]);
      this.WidthSorter(ref list2, symbols.Pix);
      int num3 = my - num2;
      this.IntegerEncoder(ctx, proc3, num3);
      num2 = my;
      int num4 = 0;
      foreach (int num5 in list2)
      {
        int mx = symbols.Pix[num5].W - (unborder_symbols ? 2 * npix : 0);
        int num6 = mx - num4;
        num4 += num6;
        this.IntegerEncoder(ctx, proc1, num6);
        Pix pix1 = (Pix) null;
        Pix pix2 = !unborder_symbols ? symbols.Pix[num5] : this.PixRemoveBorder(symbols.Pix[num5], npix);
        this.PixSetPadBits(ref pix2, 0);
        this.JBIG2EncBitImage(ctx, pix2.Data, mx, my, false);
        symmap[num5] = num1++;
        pix1 = (Pix) null;
      }
      this.JBIG2EncOob(ctx, proc1);
    }
    this.IntegerEncoder(ctx, proc2, 0);
    this.IntegerEncoder(ctx, proc2, count);
    this.JBIG2EncFinal(ctx);
  }

  private void HeightSorter(ref List<int> list, Pta symbols)
  {
    list.ToArray();
    float[] numArray = new float[list.Count];
    for (int index = 0; index < list.Count; ++index)
      numArray[index] = symbols.Y[list[index]];
    SortedDictionary<float, List<int>> sortedDictionary = new SortedDictionary<float, List<int>>();
    for (int index = 0; index < list.Count; ++index)
    {
      if (sortedDictionary.ContainsKey(numArray[index]))
      {
        sortedDictionary[numArray[index]].Add(list[index]);
      }
      else
      {
        sortedDictionary.Add(numArray[index], new List<int>());
        sortedDictionary[numArray[index]].Add(list[index]);
      }
    }
    list = new List<int>();
    foreach (KeyValuePair<float, List<int>> keyValuePair in sortedDictionary)
      list.AddRange((IEnumerable<int>) keyValuePair.Value);
  }

  private void HeightSorter(ref List<int> list, List<Pix> symbols)
  {
    list.ToArray();
    int[] numArray = new int[list.Count];
    for (int index = 0; index < list.Count; ++index)
      numArray[index] = symbols[list[index]].H;
    SortedDictionary<int, List<int>> sortedDictionary = new SortedDictionary<int, List<int>>();
    for (int index = 0; index < list.Count; ++index)
    {
      if (sortedDictionary.ContainsKey(numArray[index]))
      {
        sortedDictionary[numArray[index]].Add(list[index]);
      }
      else
      {
        sortedDictionary.Add(numArray[index], new List<int>());
        sortedDictionary[numArray[index]].Add(list[index]);
      }
    }
    list = new List<int>();
    foreach (KeyValuePair<int, List<int>> keyValuePair in sortedDictionary)
      list.AddRange((IEnumerable<int>) keyValuePair.Value);
  }

  private void WidthSorter(ref List<int> list, Pta symbols)
  {
    if (list.Count <= 1)
      return;
    list.ToArray();
    float[] numArray = new float[list.Count];
    for (int index = 0; index < list.Count; ++index)
      numArray[index] = symbols.X[list[index]];
    SortedDictionary<float, List<int>> sortedDictionary = new SortedDictionary<float, List<int>>();
    for (int index = 0; index < list.Count; ++index)
    {
      if (sortedDictionary.ContainsKey(numArray[index]))
      {
        sortedDictionary[numArray[index]].Add(list[index]);
      }
      else
      {
        sortedDictionary.Add(numArray[index], new List<int>());
        sortedDictionary[numArray[index]].Add(list[index]);
      }
    }
    list = new List<int>();
    foreach (KeyValuePair<float, List<int>> keyValuePair in sortedDictionary)
      list.AddRange((IEnumerable<int>) keyValuePair.Value);
  }

  private void WidthSorter(ref List<int> list, List<Pix> symbols)
  {
    if (list.Count <= 1)
      return;
    list.ToArray();
    int[] numArray = new int[list.Count];
    for (int index = 0; index < list.Count; ++index)
      numArray[index] = symbols[list[index]].W;
    SortedDictionary<int, List<int>> sortedDictionary = new SortedDictionary<int, List<int>>();
    for (int index = 0; index < list.Count; ++index)
    {
      if (sortedDictionary.ContainsKey(numArray[index]))
      {
        sortedDictionary[numArray[index]].Add(list[index]);
      }
      else
      {
        sortedDictionary.Add(numArray[index], new List<int>());
        sortedDictionary[numArray[index]].Add(list[index]);
      }
    }
    list = new List<int>();
    foreach (KeyValuePair<int, List<int>> keyValuePair in sortedDictionary)
      list.AddRange((IEnumerable<int>) keyValuePair.Value);
  }

  private void JBIG2EncOob(JBIG2EncoderContext ctx, int proc)
  {
    List<int> context = ctx.IndexContext[proc];
    this.EncodeBit(ctx, ref context, 1U, 1U);
    this.EncodeBit(ctx, ref context, 3U, 0U);
    this.EncodeBit(ctx, ref context, 6U, 0U);
    this.EncodeBit(ctx, ref context, 12U, 0U);
    ctx.IndexContext[proc] = context;
  }

  private void JBIG2EncBitImage(
    JBIG2EncoderContext ctx,
    uint[] idata,
    int mx,
    int my,
    bool duplicate_line_removal)
  {
    uint[] numArray = idata;
    List<int> context = ctx.Context;
    int num1 = (mx + 31 /*0x1F*/) / 32 /*0x20*/;
    int length = num1 * 4;
    uint num2 = 0;
    uint d1 = 0;
    for (int index1 = 0; index1 < my; ++index1)
    {
      int num3;
      uint num4 = (uint) (num3 = 0);
      uint num5 = (uint) num3;
      uint num6 = (uint) num3;
      if (index1 >= 2)
        num6 = numArray[(index1 - 2) * num1];
      if (index1 >= 1)
      {
        num5 = numArray[(index1 - 1) * num1];
        if (duplicate_line_removal)
        {
          if (length == 0)
          {
            d1 = num2 ^ 1U;
            num2 = 1U;
          }
          else
          {
            Array.Copy((Array) numArray, (index1 - 1) * num1, (Array) numArray, index1 * num1, length);
            d1 = num2;
            num2 = 0U;
          }
        }
      }
      if (duplicate_line_removal)
      {
        uint ctxnum = 39717;
        this.EncodeBit(ctx, ref context, ctxnum, d1);
        if (num2 == 0U)
          continue;
      }
      uint num7 = numArray[index1 * num1];
      uint num8 = num6 >> 29;
      uint num9 = num5 >> 28;
      uint num10 = num6 << 3;
      uint num11 = num5 << 4;
      uint num12 = 0;
      for (int index2 = 0; index2 < mx; ++index2)
      {
        uint ctxnum = (uint) ((int) num8 << 11 | (int) num9 << 4) | num12;
        uint d2 = (num7 & 2147483648U /*0x80000000*/) >> 31 /*0x1F*/;
        this.EncodeBit(ctx, ref context, ctxnum, d2);
        uint num13 = num8 << 1;
        uint num14 = num9 << 1;
        uint num15 = num12 << 1;
        uint num16 = num13 | (num10 & 2147483648U /*0x80000000*/) >> 31 /*0x1F*/;
        uint num17 = num14 | (num11 & 2147483648U /*0x80000000*/) >> 31 /*0x1F*/;
        uint num18 = num15 | d2;
        int num19 = index2 % 32 /*0x20*/;
        if (num19 == 28 && index1 >= 2)
        {
          int num20 = index2 / 32 /*0x20*/ + 1;
          num10 = num20 < num1 ? numArray[(index1 - 2) * num1 + num20] : 0U;
        }
        else
          num10 <<= 1;
        if (num19 == 27 && index1 >= 1)
        {
          int num21 = index2 / 32 /*0x20*/ + 1;
          num11 = num21 < num1 ? numArray[(index1 - 1) * num1 + num21] : 0U;
        }
        else
          num11 <<= 1;
        if (num19 == 31 /*0x1F*/)
        {
          int num22 = index2 / 32 /*0x20*/ + 1;
          num7 = num22 < num1 ? numArray[index1 * num1 + num22] : 0U;
        }
        else
          num7 <<= 1;
        num8 = num16 & 31U /*0x1F*/;
        num9 = num17 & (uint) sbyte.MaxValue;
        num12 = num18 & 15U;
      }
    }
    ctx.Context = context;
  }

  private void IntegerEncoder(JBIG2EncoderContext ctx, int proc, int value)
  {
    List<int> context = ctx.IndexContext[proc];
    if (value > 2000000000 || value < -2000000000)
      return;
    uint ctxnum = 1;
    int index1 = 0;
    while (this.m_intEncRange[index1].Bot > value || this.m_intEncRange[index1].Top < value)
      ++index1;
    if (value < 0)
      value = -value;
    value -= (int) this.m_intEncRange[index1].Delta;
    char data = (char) this.m_intEncRange[index1].Data;
    for (int index2 = 0; index2 < this.m_intEncRange[index1].Bits; ++index2)
    {
      uint d = (uint) data & 1U;
      this.EncodeBit(ctx, ref context, ctxnum, d);
      data >>= 1;
      ctxnum = (ctxnum & 256U /*0x0100*/) <= 0U ? ctxnum << 1 | d : (uint) (((int) ctxnum << 1 | (int) d) & 511 /*0x01FF*/ | 256 /*0x0100*/);
    }
    value <<= 32 /*0x20*/ - this.m_intEncRange[index1].IntBits;
    for (int index3 = 0; index3 < this.m_intEncRange[index1].IntBits; ++index3)
    {
      uint d = (uint) ((ulong) value & 2147483648UL /*0x80000000*/) >> 31 /*0x1F*/;
      this.EncodeBit(ctx, ref context, ctxnum, d);
      value <<= 1;
      ctxnum = (ctxnum & 256U /*0x0100*/) <= 0U ? ctxnum << 1 | d : (uint) (((int) ctxnum << 1 | (int) d) & 511 /*0x01FF*/ | 256 /*0x0100*/);
    }
    ctx.IndexContext[proc] = context;
  }

  private void EncodeBit(JBIG2EncoderContext ctx, ref List<int> context, uint ctxnum, uint d)
  {
    bool flag = false;
    if (context == null)
    {
      flag = true;
      context = new List<int>((IEnumerable<int>) ctx.Context);
    }
    int index = (long) context.Count > (long) ctxnum ? context[(int) ctxnum] : 0;
    int num = index > 46 ? 1 : 0;
    int qe = (int) ContextCollection.StateTable[index].Qe;
    if ((long) d == (long) num)
    {
      ctx.A -= qe;
      if ((ctx.A & 32768 /*0x8000*/) == 0)
      {
        if (ctx.A < qe)
          ctx.A = qe;
        else
          ctx.C += qe;
        if ((long) context.Count <= (long) ctxnum)
        {
          while ((long) context.Count < (long) ctxnum)
            context.Add(0);
          context.Add((int) ContextCollection.StateTable[index].Mps);
        }
        else
          context[(int) ctxnum] = (int) ContextCollection.StateTable[index].Mps;
      }
      else
      {
        ctx.C += qe;
        return;
      }
    }
    else
    {
      ctx.A -= qe;
      if (ctx.A < qe)
        ctx.C += qe;
      else
        ctx.A = qe;
      if ((long) context.Count <= (long) ctxnum)
      {
        while ((long) context.Count < (long) ctxnum)
          context.Add(0);
        context.Add((int) ContextCollection.StateTable[index].Lps);
      }
      else
        context[(int) ctxnum] = (int) ContextCollection.StateTable[index].Lps;
    }
    do
    {
      ctx.A <<= 1;
      ctx.C <<= 1;
      --ctx.CT;
      if (ctx.CT == 0)
        this.ByteOut(ctx);
    }
    while ((ctx.A & 32768 /*0x8000*/) == 0);
    if (!flag)
      return;
    ctx.Context = context;
  }

  private void JBGetLLCorners(JBIG2Classifier classer)
  {
    int pival = 0;
    int px = 0;
    int py = 0;
    Pta ptaUl = classer.PtaUL;
    Numa naClass = classer.NaClass;
    Pixa pixat = classer.Pixat;
    int n = ptaUl.N;
    Pta pta = JBIG2Statics.CreatePta(n);
    classer.PtaLL = pta;
    for (int index = 0; index < n; ++index)
    {
      this.PtaGetIPt(ptaUl, index, ref px, ref py);
      this.NumaGetIValue(naClass, index, ref pival);
      int h = this.PixaGetPix(pixat, pival, 2).H;
      this.PtaAddPt(pta, (float) px, (float) (py + h - 1 - 12));
    }
  }

  private void AddPage(JBIG2Context ctx, Pix pix)
  {
    this.JBAddPage(ctx.Classifier, pix);
    ctx.PageWidth.Add(pix.W);
    ctx.PageHeight.Add(pix.H);
    ctx.PageXRes.Add(pix.XRes);
    ctx.PageYRes.Add(pix.YRes);
  }

  private Pix RemoveSpot(Pix source, int size)
  {
    Sel brick1 = this.SelCreateBrick(1, size, 0, 2, 1);
    Sel brick2 = this.SelCreateBrick(size, 1, 2, 0, 1);
    Pix pixs2 = this.PixOpen((Pix) null, source, brick1);
    Pix pix = this.PixOpen((Pix) null, source, brick2);
    this.PixOr(pix, pix, pixs2);
    return pix;
  }

  private Pix PixOr(Pix pixd, Pix pixs1, Pix pixs2)
  {
    if (pixd == pixs2)
      return (Pix) null;
    if (pixs1.D != pixs2.D)
      return (Pix) null;
    if ((pixd = this.PixCopy(pixd, pixs1)) == null)
      return (Pix) null;
    this.PixRasterop(pixd, 0, 0, pixd.W, pixd.H, JBIG2Statics.PixSrc | JBIG2Statics.PixDst, pixs2, 0, 0);
    return pixd;
  }

  private void JBAddPage(JBIG2Classifier classer, Pix pixs)
  {
    Boxa pboxad = (Boxa) null;
    Pixa ppixad = (Pixa) null;
    if (pixs == null || pixs.D != 1)
      return;
    classer.W = pixs.W;
    classer.H = pixs.H;
    if (!this.JBGetComponents(pixs, classer.Components, classer.MaxWidth, classer.MaxHeight, ref pboxad, ref ppixad))
      return;
    this.JBAddPageComponents(classer, pixs, pboxad, ppixad);
  }

  private void JBAddPageComponents(JBIG2Classifier classer, Pix pixs, Boxa boxas, Pixa pixas)
  {
    if (boxas == null || pixas == null || boxas.N == 0)
    {
      ++classer.NPages;
    }
    else
    {
      if (classer.Method == 0)
        this.JBClassifyRankHaus(classer, boxas, pixas);
      else
        this.JBClassifyCorrelation(classer, boxas, pixas);
      this.JBGetULCorners(classer, pixs, boxas);
      int n = boxas.N;
      classer.BaseIndex += n;
      this.NumaAddNumber(classer.NaComps, (float) n);
      ++classer.NPages;
    }
  }

  private bool JBGetULCorners(JBIG2Classifier classer, Pix pixs, Boxa boxa)
  {
    int pival = 0;
    int pdx = 0;
    int pdy = 0;
    float px1 = 0.0f;
    float px2 = 0.0f;
    float py1 = 0.0f;
    float py2 = 0.0f;
    int n = boxa.N;
    Pta ptaUl = classer.PtaUL;
    Numa naClass = classer.NaClass;
    Pta ptac = classer.Ptac;
    Pta ptaTemplate = classer.PtaTemplate;
    int baseIndex = classer.BaseIndex;
    int[] sumtab = this.MakePixelSumTab8();
    for (int index1 = 0; index1 < n; ++index1)
    {
      int index2 = baseIndex + index1;
      this.PtaGetPt(ptac, index2, ref px1, ref py1);
      this.NumaGetIValue(naClass, index2, ref pival);
      this.PtaGetPt(ptaTemplate, pival, ref px2, ref py2);
      float num1 = px2 - px1;
      float num2 = py2 - py1;
      int idelx = (double) num1 < 0.0 ? (int) ((double) num1 - 0.5) : (int) ((double) num1 + 0.5);
      int idely = (double) num2 < 0.0 ? (int) ((double) num2 - 0.5) : (int) ((double) num2 + 0.5);
      Box box;
      if ((box = this.BoxaGetBox(boxa, index1, 2)) == null)
      {
        Console.WriteLine("box not found");
        return false;
      }
      int x = box.X;
      int y = box.Y;
      Pix pix = this.PixaGetPix(classer.Pixat, pival, 2);
      this.FinalPositioningForAlignment(pixs, x, y, idelx, idely, pix, sumtab, ref pdx, ref pdy);
      this.PtaAddPt(ptaUl, (float) (x - idelx + pdx), (float) (y - idely + pdy));
    }
    return false;
  }

  private int FinalPositioningForAlignment(
    Pix pixs,
    int x,
    int y,
    int idelx,
    int idely,
    Pix pixt,
    int[] sumtab,
    ref int pdx,
    ref int pdy)
  {
    int num1 = 0;
    int num2 = 0;
    int w = pixt.W;
    int h = pixt.H;
    Box box = this.BoxCreate(x - idelx - 6, y - idely - 6, w, h);
    Pix pixs1 = this.PixClipRectangle(pixs, box, (Box) null);
    Pix pix = this.PixCreate(pixs1.W, pixs1.H, 1);
    int num3 = int.MaxValue;
    for (int dy = -1; dy <= 1; ++dy)
    {
      for (int dx = -1; dx <= 1; ++dx)
      {
        int pcount = 0;
        pix = this.PixCopy(pix, pixs1);
        this.PixRasterop(pix, dx, dy, w, h, JBIG2Statics.PixSrc ^ JBIG2Statics.PixDst, pixt, 0, 0);
        this.PixCountPixels(pix, ref pcount, sumtab);
        if (pcount < num3)
        {
          num1 = dx;
          num2 = dy;
          num3 = pcount;
        }
      }
    }
    pdx = num1;
    pdy = num2;
    return 0;
  }

  private bool JBClassifyCorrelation(JBIG2Classifier classer, Boxa boxa, Pixa pixas)
  {
    int pival1 = 0;
    int pival2 = 0;
    float px1 = 0.0f;
    float py1 = 0.0f;
    float px2 = 0.0f;
    float py2 = 0.0f;
    int npages = classer.NPages;
    int n1 = pixas.N;
    Pixa pixa1 = JBIG2Statics.CreatePixa(n1);
    for (int index = 0; index < n1; ++index)
    {
      Pix pix = this.PixAddBorderGeneral(this.PixaGetPix(pixas, index, 2), 6, 6, 6, 6, 0U);
      this.PixaAddPix(pixa1, pix, 0);
    }
    Numa naClass = classer.NaClass;
    Numa naPage = classer.NaPage;
    Numa nafgt = classer.Nafgt;
    int[] tab = this.MakePixelSumTab8();
    int[] numArray1 = new int[n1];
    List<int[]> numArrayList = new List<int[]>(n1);
    int[] numArray2 = this.MakePixelCentroidTab8();
    if (numArray1 == null || numArrayList == null || numArray2 == null)
    {
      Console.WriteLine("calloc fail in pix*cts or centtab");
      return false;
    }
    Pta pta = JBIG2Statics.CreatePta(n1);
    for (int index1 = 0; index1 < n1; ++index1)
    {
      Pix pix = this.PixaGetPix(pixa1, index1, 2);
      numArrayList.Add(new int[pix.H]);
      float num1 = 0.0f;
      float num2 = 0.0f;
      int wpl = pix.Wpl;
      uint[] destinationArray = new uint[(pix.H - 1) * wpl];
      Array.Copy((Array) pix.Data, 0, (Array) destinationArray, 0, destinationArray.Length);
      int num3 = destinationArray.Length - wpl;
      int num4 = 0;
      int index2 = pix.H - 2;
      while (index2 >= 0)
      {
        numArrayList[index1][index2] = num4;
        int num5 = 0;
        for (int index3 = num3; index3 < num3 + wpl; ++index3)
        {
          uint num6 = destinationArray[index3];
          char index4 = (char) (num6 & (uint) byte.MaxValue);
          int num7 = num5 + tab[(int) index4];
          float num8 = num1 + (float) (numArray2[(int) index4] + ((index3 - num3) * 32 /*0x20*/ + 24) * tab[(int) index4]);
          char index5 = (char) (num6 >> 8 & (uint) byte.MaxValue);
          int num9 = num7 + tab[(int) index5];
          float num10 = num8 + (float) (numArray2[(int) index5] + ((index3 - num3) * 32 /*0x20*/ + 16 /*0x10*/) * tab[(int) index5]);
          char index6 = (char) (num6 >> 16 /*0x10*/ & (uint) byte.MaxValue);
          int num11 = num9 + tab[(int) index6];
          float num12 = num10 + (float) (numArray2[(int) index6] + ((index3 - num3) * 32 /*0x20*/ + 8) * tab[(int) index6]);
          char index7 = (char) (num6 >> 24 & (uint) byte.MaxValue);
          num5 = num11 + tab[(int) index7];
          num1 = num12 + (float) (numArray2[(int) index7] + (index3 - num3) * 32 /*0x20*/ * tab[(int) index7]);
        }
        num4 += num5;
        num2 += (float) (num5 * index2);
        --index2;
        num3 -= wpl;
      }
      numArray1[index1] = num4;
      this.PtaAddPt(pta, num1 / (float) num4, num2 / (float) num4);
    }
    this.PtaJoin(classer.Ptac, pta, 0, 0);
    Pta ptaTemplate = classer.PtaTemplate;
    Pixaa pixaa = classer.Pixaa;
    Pixa pixat = classer.Pixat;
    float thresh = classer.Thresh;
    float weightFactor = classer.WeightFactor;
    Numa naArea = classer.NaArea;
    NumaHash naHash = classer.NaHash;
    for (int index = 0; index < n1; ++index)
    {
      Pix pix1 = this.PixaGetPix(pixa1, index, 2);
      int num = numArray1[index];
      this.PtaGetPt(pta, index, ref px1, ref py1);
      int n2 = pixat.N;
      bool flag = false;
      JbTemplatesState sizedTemplatesInit = this.FindSimilarSizedTemplatesInit(classer, pix1);
      int sizedTemplatesNext;
      while ((sizedTemplatesNext = this.FindSimilarSizedTemplatesNext(sizedTemplatesInit)) > -1)
      {
        Pix pix2 = this.PixaGetPix(pixat, sizedTemplatesNext, 2);
        this.NumaGetIValue(nafgt, sizedTemplatesNext, ref pival2);
        this.PtaGetPt(ptaTemplate, sizedTemplatesNext, ref px2, ref py2);
        float score_threshold;
        if ((double) weightFactor > 0.0)
        {
          this.NumaGetIValue(naArea, sizedTemplatesNext, ref pival1);
          score_threshold = thresh + (1f - thresh) * weightFactor * (float) pival2 / (float) pival1;
        }
        else
          score_threshold = thresh;
        if (this.PixCorrelationScoreThresholded(pix1, pix2, num, pival2, px1 - px2, py1 - py2, 2, 2, tab, numArrayList[index], score_threshold))
        {
          flag = true;
          this.NumaAddNumber(naClass, (float) sizedTemplatesNext);
          this.NumaAddNumber(naPage, (float) npages);
          if (classer.KeepPixaa > 0)
          {
            Pixa pixa2 = this.PixaaGetPixa(pixaa, sizedTemplatesNext, 2);
            Pix pix3 = this.PixaGetPix(pixas, index, 2);
            this.PixaAddPix(pixa2, pix3, 0);
            Box box = this.BoxaGetBox(boxa, index, 2);
            this.PixaAddBox(pixa2, box, 0);
            break;
          }
          break;
        }
      }
      this.FindSimilarSizedTemplatesDestroy(ref sizedTemplatesInit);
      if (!flag)
      {
        this.NumaAddNumber(naClass, (float) n2);
        this.NumaAddNumber(naPage, (float) npages);
        Pixa pixa3 = JBIG2Statics.CreatePixa(0);
        Pix pix4 = this.PixaGetPix(pixas, index, 2);
        this.PixaAddPix(pixa3, pix4, 0);
        int w = pix4.W;
        int h = pix4.H;
        this.NumaHashAdd(naHash, h * w, (float) n2);
        Box box = this.BoxaGetBox(boxa, index, 2);
        this.PixaAddBox(pixa3, box, 0);
        this.PixaaAddPixa(pixaa, pixa3, 0);
        this.PtaAddPt(ptaTemplate, px1, py1);
        this.NumaAddNumber(nafgt, (float) num);
        this.PixaAddPix(pixat, pix1, 0);
        pival1 = (pix1.W - 12) * (pix1.H - 12);
        this.NumaAddNumber(naArea, (float) pival1);
      }
    }
    classer.NClass = pixat.N;
    return false;
  }

  private void FindSimilarSizedTemplatesDestroy(ref JbTemplatesState pstate)
  {
    pstate.Numa = (Numa) null;
  }

  private bool PixCorrelationScoreThresholded(
    Pix pix1,
    Pix pix2,
    int area1,
    int area2,
    float delx,
    float dely,
    int maxdiffw,
    int maxdiffh,
    int[] tab,
    int[] downcount,
    float score_threshold)
  {
    int num1 = 0;
    if (pix1 != null)
    {
      int d1 = pix1.D;
    }
    if (pix2 != null)
    {
      int d2 = pix2.D;
    }
    if (area1 > 0)
      ;
    int w1 = pix1.W;
    int h1 = pix1.H;
    int w2 = pix2.W;
    int h2 = pix2.H;
    if (Math.Abs(w1 - w2) > maxdiffw || Math.Abs(h1 - h2) > maxdiffh)
      return false;
    int val1_1 = (double) delx < 0.0 ? (int) ((double) delx - 0.5) : (int) ((double) delx + 0.5);
    int val1_2 = (double) dely < 0.0 ? (int) ((double) dely - 0.5) : (int) ((double) dely + 0.5);
    int num2 = int.Parse(Math.Ceiling((Decimal) Math.Sqrt((double) ((Decimal) score_threshold * (Decimal) area1 * (Decimal) area2))).ToString());
    int num3 = 0;
    int wpl1 = pix1.Wpl;
    int wpl2 = pix2.Wpl;
    int num4 = wpl2;
    int num5 = Math.Max(val1_2, 0);
    int num6 = Math.Min(h2 + val1_2, h1);
    int sourceIndex1 = wpl1 * num5;
    uint[] destinationArray1 = new uint[pix1.Data.Length - sourceIndex1];
    Array.Copy((Array) pix1.Data, sourceIndex1, (Array) destinationArray1, 0, destinationArray1.Length);
    int sourceIndex2 = wpl2 * (num5 - val1_2);
    uint[] destinationArray2 = new uint[pix2.Data.Length - sourceIndex2];
    Array.Copy((Array) pix2.Data, sourceIndex2, (Array) destinationArray2, 0, destinationArray2.Length);
    if (num6 <= h1)
      num1 = downcount[num6 - 1];
    int num7 = Math.Max(val1_1, 0);
    int num8 = Math.Min(w2 + val1_1, w1);
    int index1 = 0;
    int index2 = 0;
    if (val1_1 >= 32 /*0x20*/)
    {
      int num9 = val1_1 >> 5;
      index1 += num9;
      num7 -= num9 << 5;
      num8 -= num9 << 5;
      val1_1 &= 31 /*0x1F*/;
    }
    else if (val1_1 <= -32)
    {
      int num10 = -(val1_1 + 31 /*0x1F*/ >> 5);
      index2 += num10;
      num4 -= num10;
      val1_1 += num10 << 5;
    }
    if (num7 >= num8 || num5 >= num6)
    {
      num3 = 0;
    }
    else
    {
      int num11 = num8 + 31 /*0x1F*/ >> 5;
      if (val1_1 == 0)
      {
        int index3 = num5;
        while (index3 < num6)
        {
          for (int index4 = 0; index4 < num11; ++index4)
          {
            uint num12 = destinationArray1[index1 + index4] & destinationArray2[index2 + index4];
            num3 += tab[(IntPtr) (num12 & (uint) byte.MaxValue)] + tab[(IntPtr) (num12 >> 8 & (uint) byte.MaxValue)] + tab[(IntPtr) (num12 >> 16 /*0x10*/ & (uint) byte.MaxValue)] + tab[(IntPtr) (num12 >> 24)];
          }
          if (num3 >= num2)
            return true;
          if (num3 + downcount[index3] - num1 < num2)
            return false;
          ++index3;
          index1 += wpl1;
          index2 += wpl2;
        }
      }
      else if (val1_1 > 0)
      {
        if (num4 < num11)
        {
          int index5 = num5;
          while (index5 < num6)
          {
            uint num13 = destinationArray1[index1] & destinationArray2[index2] >> val1_1;
            int num14 = num3 + (tab[(IntPtr) (num13 & (uint) byte.MaxValue)] + tab[(IntPtr) (num13 >> 8 & (uint) byte.MaxValue)] + tab[(IntPtr) (num13 >> 16 /*0x10*/ & (uint) byte.MaxValue)] + tab[(IntPtr) (num13 >> 24)]);
            int num15;
            for (num15 = 1; num15 < num4; ++num15)
            {
              uint num16 = destinationArray1[index1 + num15] & (destinationArray2[index2 + num15] >> val1_1 | destinationArray2[index2 + num15 - 1] << 32 /*0x20*/ - val1_1);
              num14 += tab[(IntPtr) (num16 & (uint) byte.MaxValue)] + tab[(IntPtr) (num16 >> 8 & (uint) byte.MaxValue)] + tab[(IntPtr) (num16 >> 16 /*0x10*/ & (uint) byte.MaxValue)] + tab[(IntPtr) (num16 >> 24)];
            }
            uint num17 = destinationArray1[index1 + num15] & destinationArray2[index2 + num15 - 1] << 32 /*0x20*/ - val1_1;
            num3 = num14 + (tab[(IntPtr) (num17 & (uint) byte.MaxValue)] + tab[(IntPtr) (num17 >> 8 & (uint) byte.MaxValue)] + tab[(IntPtr) (num17 >> 16 /*0x10*/ & (uint) byte.MaxValue)] + tab[(IntPtr) (num17 >> 24)]);
            if (num3 >= num2)
              return true;
            if (num3 + downcount[index5] - num1 < num2)
              return false;
            ++index5;
            index1 += wpl1;
            index2 += wpl2;
          }
        }
        else
        {
          int index6 = num5;
          while (index6 < num6)
          {
            uint num18 = destinationArray1[index1] & destinationArray2[index2] >> val1_1;
            num3 += tab[(IntPtr) (num18 & (uint) byte.MaxValue)] + tab[(IntPtr) (num18 >> 8 & (uint) byte.MaxValue)] + tab[(IntPtr) (num18 >> 16 /*0x10*/ & (uint) byte.MaxValue)] + tab[(IntPtr) (num18 >> 24)];
            for (int index7 = 1; index7 < num11; ++index7)
            {
              uint num19 = destinationArray1[index1 + index7] & (destinationArray2[index2 + index7] >> val1_1 | destinationArray2[index2 + index7 - 1] << 32 /*0x20*/ - val1_1);
              num3 += tab[(IntPtr) (num19 & (uint) byte.MaxValue)] + tab[(IntPtr) (num19 >> 8 & (uint) byte.MaxValue)] + tab[(IntPtr) (num19 >> 16 /*0x10*/ & (uint) byte.MaxValue)] + tab[(IntPtr) (num19 >> 24)];
            }
            if (num3 >= num2)
              return true;
            if (num3 + downcount[index6] - num1 < num2)
              return false;
            ++index6;
            index1 += wpl1;
            index2 += wpl2;
          }
        }
      }
      else if (num11 < num4)
      {
        int index8 = num5;
        while (index8 < num6)
        {
          for (int index9 = 0; index9 < num11; ++index9)
          {
            uint num20 = destinationArray1[index1 + index9] & (destinationArray2[index2 + index9] << -val1_1 | destinationArray2[index2 + index9 + 1] >> 32 /*0x20*/ + val1_1);
            num3 += tab[(IntPtr) (num20 & (uint) byte.MaxValue)] + tab[(IntPtr) (num20 >> 8 & (uint) byte.MaxValue)] + tab[(IntPtr) (num20 >> 16 /*0x10*/ & (uint) byte.MaxValue)] + tab[(IntPtr) (num20 >> 24)];
          }
          if (num3 >= num2)
            return true;
          if (num3 + downcount[index8] - num1 < num2)
            return false;
          ++index8;
          index1 += wpl1;
          index2 += wpl2;
        }
      }
      else
      {
        int index10 = num5;
        while (index10 < num6)
        {
          int num21;
          for (num21 = 0; num21 < num11 - 1; ++num21)
          {
            uint num22 = destinationArray1[index1 + num21] & (destinationArray2[index2 + num21] << -val1_1 | destinationArray2[index2 + num21 + 1] >> 32 /*0x20*/ + val1_1);
            num3 += tab[(IntPtr) (num22 & (uint) byte.MaxValue)] + tab[(IntPtr) (num22 >> 8 & (uint) byte.MaxValue)] + tab[(IntPtr) (num22 >> 16 /*0x10*/ & (uint) byte.MaxValue)] + tab[(IntPtr) (num22 >> 24)];
          }
          uint num23 = destinationArray1[index1 + num21] & destinationArray2[index2 + num21] << -val1_1;
          num3 += tab[(IntPtr) (num23 & (uint) byte.MaxValue)] + tab[(IntPtr) (num23 >> 8 & (uint) byte.MaxValue)] + tab[(IntPtr) (num23 >> 16 /*0x10*/ & (uint) byte.MaxValue)] + tab[(IntPtr) (num23 >> 24)];
          if (num3 >= num2)
            return true;
          if (num3 + downcount[index10] - num1 < num2)
            return false;
          ++index10;
          index1 += wpl1;
          index2 += wpl2;
        }
      }
    }
    double num24 = (double) ((float) (num3 * num3) / (float) (area1 * area2));
    return false;
  }

  private bool JBClassifyRankHaus(JBIG2Classifier classer, Boxa boxa, Pixa pixas)
  {
    int pival1 = 0;
    int pival2 = 0;
    float px1 = 0.0f;
    float py1 = 0.0f;
    float px2 = 0.0f;
    float py2 = 0.0f;
    int npages = classer.NPages;
    int sizeHaus = classer.SizeHaus;
    Sel brick = this.SelCreateBrick(sizeHaus, sizeHaus, sizeHaus / 2, sizeHaus / 2, 1);
    int n1 = pixas.N;
    Pixa pixa1 = JBIG2Statics.CreatePixa(n1);
    Pixa pixa2 = JBIG2Statics.CreatePixa(n1);
    for (int index = 0; index < n1; ++index)
    {
      Pix pix1 = this.PixAddBorderGeneral(this.PixaGetPix(pixas, index, 2), 6, 6, 6, 6, 0U);
      Pix pix2 = this.PixDilate((Pix) null, pix1, brick);
      this.PixaAddPix(pixa1, pix1, 0);
      this.PixaAddPix(pixa2, pix2, 0);
    }
    Pta pta = this.PixaCentroids(pixa1);
    this.PtaJoin(classer.Ptac, pta, 0, 0);
    Pta ptaTemplate = classer.PtaTemplate;
    Numa naClass = classer.NaClass;
    Numa naPage = classer.NaPage;
    this.MakePixelSumTab8();
    Pixaa pixaa = classer.Pixaa;
    Pixa pixat = classer.Pixat;
    Pixa pixatd = classer.Pixatd;
    float rankHaus = classer.RankHaus;
    NumaHash naHash = classer.NaHash;
    if ((double) rankHaus == 1.0)
    {
      for (int index = 0; index < n1; ++index)
      {
        Pix pix3 = this.PixaGetPix(pixa1, index, 2);
        Pix pix4 = this.PixaGetPix(pixa2, index, 2);
        this.PtaGetPt(pta, index, ref px1, ref py1);
        int n2 = pixat.N;
        bool flag = false;
        JbTemplatesState sizedTemplatesInit = this.FindSimilarSizedTemplatesInit(classer, pix3);
        int sizedTemplatesNext;
        while ((sizedTemplatesNext = this.FindSimilarSizedTemplatesNext(sizedTemplatesInit)) > -1)
        {
          Pix pix5 = this.PixaGetPix(pixat, sizedTemplatesNext, 2);
          Pix pix6 = this.PixaGetPix(pixatd, sizedTemplatesNext, 2);
          this.PtaGetPt(ptaTemplate, sizedTemplatesNext, ref px2, ref py2);
          if (this.PixHausTest(pix3, pix4, pix5, pix6, px1 - px2, py1 - py2, 2, 2) == 1)
          {
            flag = true;
            this.NumaAddNumber(naClass, (float) sizedTemplatesNext);
            this.NumaAddNumber(naPage, (float) npages);
            if (classer.KeepPixaa == 1)
            {
              Pixa pixa3 = this.PixaaGetPixa(pixaa, sizedTemplatesNext, 2);
              Pix pix7 = this.PixaGetPix(pixas, index, 2);
              this.PixaAddPix(pixa3, pix7, 0);
              Box box = this.BoxaGetBox(boxa, index, 2);
              this.PixaAddBox(pixa3, box, 0);
              break;
            }
            break;
          }
        }
        if (!flag)
        {
          this.NumaAddNumber(naClass, (float) n2);
          this.NumaAddNumber(naPage, (float) npages);
          Pixa pixa4 = JBIG2Statics.CreatePixa(0);
          Pix pix8 = this.PixaGetPix(pixas, index, 2);
          this.PixaAddPix(pixa4, pix8, 0);
          int w = pix8.W;
          int h = pix8.H;
          this.NumaHashAdd(naHash, h * w, (float) n2);
          Box box = this.BoxaGetBox(boxa, index, 2);
          this.PixaAddBox(pixa4, box, 0);
          this.PixaaAddPixa(pixaa, pixa4, 0);
          this.PtaAddPt(ptaTemplate, px1, py1);
          this.PixaAddPix(pixat, pix3, 0);
          this.PixaAddPix(pixatd, pix4, 0);
        }
      }
    }
    else
    {
      Numa na;
      if ((na = this.PixaCountPixels(pixas)) == null)
      {
        Console.WriteLine("nafg not made");
        return false;
      }
      Numa nafgt = classer.Nafgt;
      int[] tab8 = this.MakePixelSumTab8();
      for (int index = 0; index < n1; ++index)
      {
        Pix pix9 = this.PixaGetPix(pixa1, index, 2);
        this.NumaGetIValue(na, index, ref pival1);
        Pix pix10 = this.PixaGetPix(pixa2, index, 2);
        this.PtaGetPt(pta, index, ref px1, ref py1);
        int n3 = pixat.N;
        bool flag = false;
        JbTemplatesState sizedTemplatesInit = this.FindSimilarSizedTemplatesInit(classer, pix9);
        int sizedTemplatesNext;
        while ((sizedTemplatesNext = this.FindSimilarSizedTemplatesNext(sizedTemplatesInit)) > -1)
        {
          Pix pix11 = this.PixaGetPix(pixat, sizedTemplatesNext, 2);
          this.NumaGetIValue(nafgt, sizedTemplatesNext, ref pival2);
          Pix pix12 = this.PixaGetPix(pixatd, sizedTemplatesNext, 2);
          this.PtaGetPt(ptaTemplate, sizedTemplatesNext, ref px2, ref py2);
          if (this.PixRankHaustest(pix9, pix10, pix11, pix12, px1 - px2, py1 - py2, 2, 2, pival1, pival2, rankHaus, tab8) == 1)
          {
            flag = true;
            this.NumaAddNumber(naClass, (float) sizedTemplatesNext);
            this.NumaAddNumber(naPage, (float) npages);
            if (classer.KeepPixaa > 0)
            {
              Pixa pixa5 = this.PixaaGetPixa(pixaa, sizedTemplatesNext, 2);
              Pix pix13 = this.PixaGetPix(pixas, index, 2);
              this.PixaAddPix(pixa5, pix13, 0);
              Box box = this.BoxaGetBox(boxa, index, 2);
              this.PixaAddBox(pixa5, box, 0);
              break;
            }
            break;
          }
        }
        if (!flag)
        {
          this.NumaAddNumber(naClass, (float) n3);
          this.NumaAddNumber(naPage, (float) npages);
          Pixa pixa6 = JBIG2Statics.CreatePixa(0);
          Pix pix14 = this.PixaGetPix(pixas, index, 2);
          this.PixaAddPix(pixa6, pix14, 0);
          int w = pix14.W;
          int h = pix14.H;
          this.NumaHashAdd(naHash, h * w, (float) n3);
          Box box = this.BoxaGetBox(boxa, index, 2);
          this.PixaAddBox(pixa6, box, 0);
          this.PixaaAddPixa(pixaa, pixa6, 0);
          this.PtaAddPt(ptaTemplate, px1, py1);
          this.PixaAddPix(pixat, pix9, 0);
          this.PixaAddPix(pixatd, pix10, 0);
          this.NumaAddNumber(nafgt, (float) pival1);
        }
      }
    }
    classer.NClass = pixat.N;
    return false;
  }

  private Numa PixaCountPixels(Pixa pixa)
  {
    int pcount = 0;
    int n;
    if ((n = pixa.N) == 0)
      return JBIG2Statics.CreateNuma(1);
    int d = this.PixaGetPix(pixa, 0, 2).D;
    int[] tab8 = this.MakePixelSumTab8();
    Numa numa = JBIG2Statics.CreateNuma(n);
    for (int index = 0; index < n; ++index)
    {
      this.PixCountPixels(this.PixaGetPix(pixa, index, 2), ref pcount, tab8);
      this.NumaAddNumber(numa, (float) pcount);
    }
    return numa;
  }

  private int PixCountPixels(Pix pix, ref int pcount, int[] tab8)
  {
    if (pix == null || pix.D != 1)
      return -1;
    int[] numArray = tab8 != null ? tab8 : this.MakePixelSumTab8();
    int w = pix.W;
    int h = pix.H;
    int wpl = pix.Wpl;
    uint[] data = pix.Data;
    int num1 = w >> 5;
    int num2 = w & 31 /*0x1F*/;
    uint num3 = (uint) (-1 << 32 /*0x20*/ - num2);
    int num4 = 0;
    int num5 = 0;
    int num6 = 0;
    while (num6 < h)
    {
      int num7;
      for (num7 = 0; num7 < num1; ++num7)
      {
        uint num8 = data[num5 + num7];
        if (num8 > 0U)
          num4 += numArray[(IntPtr) (num8 & (uint) byte.MaxValue)] + numArray[(IntPtr) (num8 >> 8 & (uint) byte.MaxValue)] + numArray[(IntPtr) (num8 >> 16 /*0x10*/ & (uint) byte.MaxValue)] + numArray[(IntPtr) (num8 >> 24 & (uint) byte.MaxValue)];
      }
      if (num2 > 0)
      {
        uint num9 = data[num5 + num7] & num3;
        if (num9 > 0U)
          num4 += numArray[(IntPtr) (num9 & (uint) byte.MaxValue)] + numArray[(IntPtr) (num9 >> 8 & (uint) byte.MaxValue)] + numArray[(IntPtr) (num9 >> 16 /*0x10*/ & (uint) byte.MaxValue)] + numArray[(IntPtr) (num9 >> 24 & (uint) byte.MaxValue)];
      }
      ++num6;
      num5 += wpl;
    }
    pcount = num4;
    return 0;
  }

  private int PixRankHaustest(
    Pix pix1,
    Pix pix2,
    Pix pix3,
    Pix pix4,
    float delx,
    float dely,
    int maxdiffw,
    int maxdiffh,
    int area1,
    int area3,
    float rank,
    int[] tab8)
  {
    int pabove = 0;
    int w1 = pix1.W;
    int h1 = pix1.H;
    int w2 = pix3.W;
    int h2 = pix3.H;
    if (Math.Abs(w1 - w2) > maxdiffw || Math.Abs(h1 - h2) > maxdiffh)
      return 1;
    int thresh1 = (int) ((double) area1 * (1.0 - (double) rank) + 0.5);
    int thresh2 = (int) ((double) area3 * (1.0 - (double) rank) + 0.5);
    int dx = (double) delx < 0.0 ? (int) ((double) delx - 0.5) : (int) ((double) delx + 0.5);
    int dy = (double) dely < 0.0 ? (int) ((double) dely - 0.5) : (int) ((double) dely + 0.5);
    Pix template = this.PixCreateTemplate(pix1);
    this.PixRasterop(template, 0, 0, w1, h1, JBIG2Statics.PixSrc, pix1, 0, 0);
    this.PixRasterop(template, dx, dy, w1, h1, JBIG2Statics.PixDst & JBIG2Statics.PixNot(JBIG2Statics.PixSrc), pix4, 0, 0);
    this.PixThresholdPixelSum(template, thresh1, pabove, tab8);
    if (pabove == 1)
      return 1;
    this.PixRasterop(template, dx, dy, w2, h2, JBIG2Statics.PixSrc, pix3, 0, 0);
    this.PixRasterop(template, 0, 0, w2, h2, JBIG2Statics.PixDst & JBIG2Statics.PixNot(JBIG2Statics.PixSrc), pix2, 0, 0);
    this.PixThresholdPixelSum(template, thresh2, pabove, tab8);
    return pabove == 1 ? 1 : 0;
  }

  private void PixThresholdPixelSum(Pix pix, int thresh, int pabove, int[] tab8)
  {
    if (pix != null)
    {
      int d = pix.D;
    }
    int[] numArray = tab8 != null ? tab8 : this.MakePixelSumTab8();
    int w = pix.W;
    int h = pix.H;
    int wpl = pix.Wpl;
    uint[] data = pix.Data;
    int num1 = w >> 5;
    int num2 = w & 31 /*0x1F*/;
    uint num3 = (uint) (-1 << 32 /*0x20*/ - num2);
    int num4 = 0;
    for (int index = 0; index < h; ++index)
    {
      int num5 = wpl * index;
      int num6;
      for (num6 = 0; num6 < num1; ++num6)
      {
        uint num7 = data[num5 + num6];
        if (num7 > 0U)
          num4 += numArray[(IntPtr) (num7 & (uint) byte.MaxValue)] + numArray[(IntPtr) (num7 >> 8 & (uint) byte.MaxValue)] + numArray[(IntPtr) (num7 >> 16 /*0x10*/ & (uint) byte.MaxValue)] + numArray[(IntPtr) (num7 >> 24 & (uint) byte.MaxValue)];
      }
      if (num2 > 0)
      {
        uint num8 = data[num5 + num6] & num3;
        if (num8 > 0U)
          num4 += numArray[(IntPtr) (num8 & (uint) byte.MaxValue)] + numArray[(IntPtr) (num8 >> 8 & (uint) byte.MaxValue)] + numArray[(IntPtr) (num8 >> 16 /*0x10*/ & (uint) byte.MaxValue)] + numArray[(IntPtr) (num8 >> 24 & (uint) byte.MaxValue)];
      }
      if (num4 > thresh)
      {
        pabove = 1;
        break;
      }
    }
  }

  private int NumaHashAdd(NumaHash nahash, int key, float value)
  {
    Numa na = (Numa) null;
    int key1 = key % nahash.NBuckets;
    if (nahash.Numa.ContainsKey(key1))
      na = nahash.Numa[key1];
    if (na == null)
    {
      na = JBIG2Statics.CreateNuma(nahash.InitSize);
      nahash.Numa.Add(key1, na);
    }
    this.NumaAddNumber(na, value);
    return 0;
  }

  private int PixaaAddPixa(Pixaa pixaa, Pixa pixa, int copyflag)
  {
    if (copyflag != 0 && copyflag != 1 && copyflag != 2)
      ;
    Pixa pixa1 = copyflag != 0 ? this.PixaCopy(pixa, copyflag) : pixa;
    if (pixaa.N >= pixaa.Nalloc)
      pixaa.Nalloc *= 2;
    pixaa.Pixa.Add(pixa1);
    ++pixaa.N;
    return 0;
  }

  private Pixa PixaaGetPixa(Pixaa pixaa, int index, int accesstype)
  {
    if (index < 0 || index >= pixaa.N)
    {
      Console.WriteLine("index not valid");
      return (Pixa) null;
    }
    if (accesstype != 1 && accesstype != 2 && accesstype != 3)
    {
      Console.WriteLine("invalid accesstype");
      return (Pixa) null;
    }
    Pixa pixa;
    if ((pixa = pixaa.Pixa[index]) != null)
      return this.PixaCopy(pixa, accesstype);
    Console.WriteLine("no pixa[index]");
    return (Pixa) null;
  }

  private Pixa PixaCopy(Pixa pixa, int copyflag)
  {
    switch (copyflag)
    {
      case 1:
        Pixa pixa1 = JBIG2Statics.CreatePixa(pixa.N);
        for (int index = 0; index < pixa.N; ++index)
        {
          Pix pix;
          Box box;
          if (copyflag == 1)
          {
            pix = this.PixaGetPix(pixa, index, 1);
            box = this.PixaGetBox(pixa, index, 1);
          }
          else
          {
            pix = this.PixaGetPix(pixa, index, 2);
            box = this.PixaGetBox(pixa, index, 2);
          }
          this.PixaAddPix(pixa1, pix, 0);
          this.PixaAddBox(pixa1, box, 0);
        }
        return pixa1;
      case 2:
        ++pixa.RefCount;
        return pixa;
      default:
        goto case 1;
    }
  }

  private int PixHausTest(
    Pix pix1,
    Pix pix2,
    Pix pix3,
    Pix pix4,
    float delx,
    float dely,
    int maxdiffw,
    int maxdiffh)
  {
    int pempty = 0;
    int w1 = pix1.W;
    int h1 = pix1.H;
    int w2 = pix3.W;
    int h2 = pix3.H;
    if (Math.Abs(w1 - w2) > maxdiffw || Math.Abs(h1 - h2) > maxdiffh)
      return 1;
    int dx = (double) delx < 0.0 ? (int) ((double) delx - 0.5) : (int) ((double) delx + 0.5);
    int dy = (double) dely < 0.0 ? (int) ((double) dely - 0.5) : (int) ((double) dely + 0.5);
    Pix template = this.PixCreateTemplate(pix1);
    this.PixRasterop(template, 0, 0, w1, h1, JBIG2Statics.PixSrc, pix1, 0, 0);
    this.PixRasterop(template, dx, dy, w1, h1, JBIG2Statics.PixDst & JBIG2Statics.PixNot(JBIG2Statics.PixSrc), pix4, 0, 0);
    this.PixZero(template, ref pempty);
    if (pempty == 0)
      return pempty;
    this.PixRasterop(template, dx, dy, w2, h2, JBIG2Statics.PixSrc, pix3, 0, 0);
    this.PixRasterop(template, 0, 0, w2, h2, JBIG2Statics.PixDst & JBIG2Statics.PixNot(JBIG2Statics.PixSrc), pix2, 0, 0);
    this.PixZero(template, ref pempty);
    return pempty;
  }

  private int FindSimilarSizedTemplatesNext(JbTemplatesState state)
  {
    while (state.I < 25)
    {
      int num1 = state.W + JBIG2Statics.MatchOffset[2 * state.I];
      int num2 = state.H + JBIG2Statics.MatchOffset[2 * state.I + 1];
      if (num2 < 1 || num1 < 1)
      {
        ++state.I;
      }
      else
      {
        if (state.Numa == null)
        {
          state.Numa = this.NumaHashGetNuma(state.Classer.NaHash, num2 * num1);
          if (state.Numa == null)
          {
            ++state.I;
            continue;
          }
          state.N = 0;
        }
        int n = state.Numa.N;
        while (state.N < n)
        {
          int index = (int) ((double) state.Numa.Array[state.N++] + 0.5);
          Pix pix = this.PixaGetPix(state.Classer.Pixat, index, 2);
          if (pix.W - 12 == num1 && pix.H - 12 == num2)
            return index;
        }
        ++state.I;
        state.Numa = (Numa) null;
      }
    }
    return -1;
  }

  private Numa NumaHashGetNuma(NumaHash nahash, int key)
  {
    Numa numa = (Numa) null;
    int key1 = key % nahash.NBuckets;
    if (nahash.Numa == null)
      return (Numa) null;
    if (nahash.Numa.ContainsKey(key1))
      numa = nahash.Numa[key1];
    return numa ?? (Numa) null;
  }

  private JbTemplatesState FindSimilarSizedTemplatesInit(JBIG2Classifier classer, Pix pixs)
  {
    return new JbTemplatesState()
    {
      W = pixs.W - 12,
      H = pixs.H - 12,
      Classer = classer
    };
  }

  private void PtaGetPt(Pta pta, int index, ref float px, ref float py)
  {
    px = py = -1f;
    if (pta.X.Count > index)
      px = pta.X[index];
    if (pta.Y.Count <= index)
      return;
    py = pta.Y[index];
  }

  private Pta PixaCentroids(Pixa pixa)
  {
    int[] numArray1 = new int[0];
    int[] numArray2 = new int[0];
    float num1 = 0.0f;
    float num2 = 0.0f;
    int n = pixa.N;
    Pta pta = JBIG2Statics.CreatePta(n);
    int[] centtab = this.MakePixelCentroidTab8();
    int[] sumtab = this.MakePixelSumTab8();
    for (int index = 0; index < n; ++index)
    {
      this.PixCentroid(this.PixaGetPix(pixa, index, 2), centtab, sumtab, num1, num2);
      this.PtaAddPt(pta, num1, num2);
    }
    return pta;
  }

  private int PixCentroid(Pix pix, int[] centtab, int[] sumtab, float pxave, float pyave)
  {
    int w = pix.W;
    int h = pix.H;
    int d = pix.D;
    if (d != 1)
      ;
    int[] numArray1 = centtab == null ? centtab : this.MakePixelCentroidTab8();
    int[] numArray2 = sumtab == null ? sumtab : this.MakePixelSumTab8();
    uint[] data = pix.Data;
    int wpl = pix.Wpl;
    float num1;
    float num2 = num1 = 0.0f;
    int num3 = 0;
    if (d == 1)
    {
      for (int index1 = 0; index1 < h; ++index1)
      {
        int num4 = wpl * index1;
        int num5 = 0;
        for (int index2 = 0; index2 < wpl; ++index2)
        {
          uint num6 = data[num4 + index2];
          if (num6 == 1U)
          {
            uint index3 = num6 & (uint) byte.MaxValue;
            int num7 = num5 + numArray2[(IntPtr) index3];
            float num8 = num2 + (float) (numArray1[(IntPtr) index3] + (index2 * 32 /*0x20*/ + 24) * numArray2[(IntPtr) index3]);
            uint index4 = num6 >> 8 & (uint) byte.MaxValue;
            int num9 = num7 + numArray2[(IntPtr) index4];
            float num10 = num8 + (float) (numArray1[(IntPtr) index4] + (index2 * 32 /*0x20*/ + 16 /*0x10*/) * numArray2[(IntPtr) index4]);
            uint index5 = num6 >> 16 /*0x10*/ & (uint) byte.MaxValue;
            int num11 = num9 + numArray2[(IntPtr) index5];
            float num12 = num10 + (float) (numArray1[(IntPtr) index5] + (index2 * 32 /*0x20*/ + 8) * numArray2[(IntPtr) index5]);
            uint index6 = num6 >> 24 & (uint) byte.MaxValue;
            num5 = num11 + numArray2[(IntPtr) index6];
            num2 = num12 + (float) (numArray1[(IntPtr) index6] + index2 * 32 /*0x20*/ * numArray2[(IntPtr) index6]);
          }
        }
        num3 += num5;
        num1 += (float) (num5 * index1);
      }
      if (num3 != 0)
      {
        pxave = num2 / (float) num3;
        pyave = num1 / (float) num3;
      }
    }
    else
    {
      for (int index7 = 0; index7 < h; ++index7)
      {
        int num13 = wpl * index7;
        for (int index8 = 0; index8 < w; ++index8)
        {
          uint dataByte = JBIG2Statics.GetDataByte(data, num13 + index8);
          num2 += (float) ((long) dataByte * (long) index8);
          num1 += (float) ((long) dataByte * (long) index7);
          num3 += (int) dataByte;
        }
      }
      if (num3 != 0)
      {
        pxave = num2 / (float) num3;
        pyave = num1 / (float) num3;
      }
    }
    return 0;
  }

  private int[] MakePixelCentroidTab8()
  {
    int[] numArray = new int[256 /*0x0100*/];
    numArray[0] = 0;
    numArray[1] = 7;
    for (int index = 2; index < 4; ++index)
      numArray[index] = numArray[index - 2] + 6;
    for (int index = 4; index < 8; ++index)
      numArray[index] = numArray[index - 4] + 5;
    for (int index = 8; index < 16 /*0x10*/; ++index)
      numArray[index] = numArray[index - 8] + 4;
    for (int index = 16 /*0x10*/; index < 32 /*0x20*/; ++index)
      numArray[index] = numArray[index - 16 /*0x10*/] + 3;
    for (int index = 32 /*0x20*/; index < 64 /*0x40*/; ++index)
      numArray[index] = numArray[index - 32 /*0x20*/] + 2;
    for (int index = 64 /*0x40*/; index < 128 /*0x80*/; ++index)
      numArray[index] = numArray[index - 64 /*0x40*/] + 1;
    for (int index = 128 /*0x80*/; index < 256 /*0x0100*/; ++index)
      numArray[index] = numArray[index - 128 /*0x80*/];
    return numArray;
  }

  private int PtaJoin(Pta ptad, Pta ptas, int istart, int iend)
  {
    int px = 0;
    int py = 0;
    int n = ptas.N;
    if (istart < 0)
      istart = 0;
    if (iend <= 0)
      iend = n - 1;
    for (int index = istart; index <= iend; ++index)
    {
      this.PtaGetIPt(ptas, index, ref px, ref py);
      this.PtaAddPt(ptad, (float) px, (float) py);
    }
    return 0;
  }

  private void PtaAddPt(Pta pta, float x, float y)
  {
    if (pta.N >= pta.Nalloc)
      pta.Nalloc *= 2;
    pta.X.Add(x);
    pta.Y.Add(y);
    ++pta.N;
  }

  private int PtaGetIPt(Pta pta, int index, ref int px, ref int py)
  {
    px = (int) ((double) pta.X[index] + 0.5);
    py = (int) ((double) pta.Y[index] + 0.5);
    return 0;
  }

  private bool JBGetComponents(
    Pix pixs,
    int components,
    int maxwidth,
    int maxheight,
    ref Boxa pboxad,
    ref Pixa ppixad)
  {
    int pempty = 0;
    Pixa ppixa1 = (Pixa) null;
    Pixa ppixa2 = (Pixa) null;
    int num1 = 0;
    int num2 = 1;
    int num3 = 2;
    if (components != num1 && components != num2 && components != num3)
      return false;
    this.PixZero(pixs, ref pempty);
    if (pempty < 0)
    {
      pboxad = JBIG2Statics.CreateBoxa(0);
      ppixad = JBIG2Statics.CreatePixa(0);
      return true;
    }
    Boxa boxas;
    if (components == num1)
      boxas = this.PixConnComp(pixs, ref ppixa1, 8);
    else if (components == num2)
    {
      boxas = this.PixConnComp(this.PixMorphSequence(pixs, "c1.6".ToCharArray(), 0), ref ppixa2, 8);
      ppixa1 = this.PixaClipToPix(ppixa2, pixs);
    }
    else
    {
      int xres = pixs.XRes;
      int factor;
      Pix pixs1;
      if (xres <= 200)
      {
        factor = 1;
        pixs1 = pixs;
      }
      else if (xres <= 400)
      {
        factor = 2;
        pixs1 = this.PixReduceRankBinaryCascade(pixs, 1, 0, 0, 0);
      }
      else
      {
        factor = 4;
        pixs1 = this.PixReduceRankBinaryCascade(pixs, 1, 1, 0, 0);
      }
      boxas = this.PixConnComp(this.PixExpandReplicate(this.PixWordMaskByDilation(pixs1, 0, -1), factor), ref ppixa2, 4);
      ppixa1 = this.PixaClipToPix(ppixa2, pixs);
    }
    ppixad = this.PixaSelectBySize(ppixa1, maxwidth, maxheight, 4, JBIG2Statics.SelectIfLte, false);
    pboxad = this.BoxaSelectBySize(boxas, maxwidth, maxheight, 4, JBIG2Statics.SelectIfLte, false);
    --ppixad.RefCount;
    --pboxad.RefCount;
    return true;
  }

  private Boxa BoxaSelectBySize(
    Boxa boxas,
    int width,
    int height,
    int type,
    int relation,
    bool pchanged)
  {
    if (type != 1 && type != 2 && type != 3)
      ;
    if (relation != JBIG2Statics.SelectIfLt && relation != JBIG2Statics.SelectIfGt && relation != JBIG2Statics.SelectIfLte)
    {
      int selectIfGte = JBIG2Statics.SelectIfGte;
    }
    if (pchanged)
      pchanged = false;
    Numa na = this.BoxaMakeSizeIndicator(boxas, width, height, type, relation);
    return this.BoxaSelectWithIndicator(boxas, na, pchanged);
  }

  private Boxa BoxaSelectWithIndicator(Boxa boxas, Numa na, bool pchanged)
  {
    int pival = 0;
    int n1 = 0;
    int n2 = na.N;
    for (int index = 0; index < n2; ++index)
    {
      this.NumaGetIValue(na, index, ref pival);
      if (pival == 1)
        ++n1;
    }
    if (n1 == n2)
    {
      if (pchanged)
        pchanged = false;
      return this.BoxaCopy(boxas, 2);
    }
    if (pchanged)
      pchanged = true;
    Boxa boxa = this.CreateBoxa(n1);
    for (int index = 0; index < n2; ++index)
    {
      this.NumaGetIValue(na, index, ref pival);
      if (pival != 0)
      {
        Box box = this.BoxaGetBox(boxas, index, 2);
        this.BoxaAddBox(boxa, box, 0);
      }
    }
    return boxa;
  }

  private Boxa CreateBoxa(int n)
  {
    int num = 20;
    if (n <= 0)
      n = num;
    return new Boxa(n);
  }

  private Pixa PixaSelectBySize(
    Pixa pixas,
    int width,
    int height,
    int type,
    int relation,
    bool pchanged)
  {
    if (type != 1 && type != 2 && type != 3 && type != 4)
    {
      Console.WriteLine("invalid type");
      return (Pixa) null;
    }
    if (relation != JBIG2Statics.SelectIfLt && relation != JBIG2Statics.SelectIfGt && relation != JBIG2Statics.SelectIfLte && relation != JBIG2Statics.SelectIfGte)
    {
      Console.WriteLine("invalid relation");
      return (Pixa) null;
    }
    Boxa boxa = this.PixaGetBoxa(pixas, 2);
    Numa na = this.BoxaMakeSizeIndicator(boxa, width, height, type, relation);
    --boxa.RefCount;
    return this.PixaSelectWithIndicator(pixas, na, pchanged);
  }

  private Pixa PixaSelectWithIndicator(Pixa pixas, Numa na, bool pchanged)
  {
    int pival = 0;
    int n1 = 0;
    int n2 = na.N;
    for (int index = 0; index < n2; ++index)
    {
      this.NumaGetIValue(na, index, ref pival);
      if (pival == 1)
        ++n1;
    }
    if (n1 == n2)
    {
      if (pchanged)
        pchanged = false;
      return this.PixaCopy(pixas, 2);
    }
    if (pchanged)
      pchanged = true;
    Pixa pixa = JBIG2Statics.CreatePixa(n1);
    for (int index = 0; index < n2; ++index)
    {
      this.NumaGetIValue(na, index, ref pival);
      if (pival != 0)
      {
        Pix pix = this.PixaGetPix(pixas, index, 2);
        Box box = this.PixaGetBox(pixas, index, 2);
        this.PixaAddPix(pixa, pix, 0);
        this.PixaAddBox(pixa, box, 0);
      }
    }
    return pixa;
  }

  private int NumaGetIValue(Numa na, int index, ref int pival)
  {
    pival = 0;
    if (index >= 0)
    {
      int n = na.N;
    }
    float num1 = na.Array[index];
    int num2 = (double) num1 < 0.0 ? -1 : 1;
    pival = (int) ((double) num1 + (double) num2 * 0.5);
    return 0;
  }

  private Numa BoxaMakeSizeIndicator(Boxa boxa, int width, int height, int type, int relation)
  {
    int pw = 0;
    int ph = 0;
    if (type != 1 && type != 2 && type != 3 && type != 4)
    {
      Console.WriteLine("invalid type");
      return (Numa) null;
    }
    if (relation != JBIG2Statics.SelectIfLt && relation != JBIG2Statics.SelectIfGt && relation != JBIG2Statics.SelectIfLte && relation != JBIG2Statics.SelectIfGte)
    {
      Console.WriteLine("invalid relation");
      return (Numa) null;
    }
    int n = boxa.N;
    Numa numa = JBIG2Statics.CreateNuma(n);
    for (int index = 0; index < n; ++index)
    {
      int val = 0;
      int px = 0;
      int py = 0;
      this.BoxaGetBoxGeometry(boxa, index, ref px, ref py, ref pw, ref ph);
      switch (type)
      {
        case 1:
          if (relation == JBIG2Statics.SelectIfLt && pw < width || relation == JBIG2Statics.SelectIfGt && pw > width || relation == JBIG2Statics.SelectIfLte && pw <= width || relation == JBIG2Statics.SelectIfGte && pw >= width)
          {
            val = 1;
            break;
          }
          break;
        case 2:
          if (relation == JBIG2Statics.SelectIfLt && ph < height || relation == JBIG2Statics.SelectIfGt && ph > height || relation == JBIG2Statics.SelectIfLte && ph <= height || relation == JBIG2Statics.SelectIfGte && ph >= height)
          {
            val = 1;
            break;
          }
          break;
        case 3:
          if (relation == JBIG2Statics.SelectIfLt && (pw < width || ph < height) || relation == JBIG2Statics.SelectIfGt && (pw > width || ph > height) || relation == JBIG2Statics.SelectIfLte && (pw <= width || ph <= height) || relation == JBIG2Statics.SelectIfGte && (pw >= width || ph >= height))
          {
            val = 1;
            break;
          }
          break;
        case 4:
          if (relation == JBIG2Statics.SelectIfLt && pw < width && ph < height || relation == JBIG2Statics.SelectIfGt && pw > width && ph > height || relation == JBIG2Statics.SelectIfLte && pw <= width && ph <= height || relation == JBIG2Statics.SelectIfGte && pw >= width && ph >= height)
          {
            val = 1;
            break;
          }
          break;
      }
      this.NumaAddNumber(numa, (float) val);
    }
    return numa;
  }

  private void NumaAddNumber(Numa na, float val)
  {
    if (na.N >= na.Nalloc)
      na.Nalloc *= 2;
    na.Array.Add(val);
    ++na.N;
  }

  private void BoxaGetBoxGeometry(
    Boxa boxa,
    int index,
    ref int px,
    ref int py,
    ref int pw,
    ref int ph)
  {
    Box box = this.BoxaGetBox(boxa, index, 2);
    px = box.X;
    py = box.Y;
    pw = box.W;
    ph = box.H;
  }

  private Boxa PixaGetBoxa(Pixa pixa, int accesstype)
  {
    if (pixa.Boxa == null)
    {
      Console.WriteLine("boxa not defined");
      return (Boxa) null;
    }
    if (accesstype == 1 || accesstype == 2 || accesstype == 3)
      return this.BoxaCopy(pixa.Boxa, accesstype);
    Console.WriteLine("invalid accesstype");
    return (Boxa) null;
  }

  private Pix PixWordMaskByDilation(Pix pixs, int maxsize, int psize)
  {
    int num1 = 14;
    int num2 = 0;
    int[] numArray = new int[num1 + 1];
    int num3 = 1000000;
    Pixa pixa = JBIG2Statics.CreatePixa(8);
    Pix pix1 = this.PixCopy((Pix) null, pixs);
    this.PixaAddPix(pixa, pix1, 2);
    if (maxsize <= 0)
      maxsize = 7;
    if (maxsize > num1)
      maxsize = num1;
    Numa numa = JBIG2Statics.CreateNuma(maxsize);
    Pix pix2;
    for (int index = 0; index <= maxsize; ++index)
    {
      Pix pix3 = index != 0 ? this.PixMorphSequence(pix1, "d2.1".ToCharArray(), 0) : this.PixCopy((Pix) null, pix1);
      Boxa boxa = this.PixConnCompBB(pix3, 4);
      numArray[index] = boxa.N;
      this.NumaAddNumber(numa, (float) numArray[index]);
      if (index > 0)
      {
        int num4 = numArray[index - 1] - numArray[index];
        if (num4 < num3)
        {
          num2 = index;
          num3 = num4;
        }
      }
      this.PixaAddPix(pixa, pix3, 1);
      pix2 = (Pix) null;
      pix1 = pix3;
    }
    pix2 = (Pix) null;
    Pix pix4 = this.PixErode((Pix) null, this.PixaGetPix(pixa, num2, 2), this.SelCreateBrick(1, num2, 0, num2 - 1, 1));
    if (psize < 0)
      psize = num2 + 1;
    return pix4;
  }

  private Pix PixMorphSequence(Pix pixs, char[] sequence, int dispsep)
  {
    int[] numArray = new int[4];
    Pix pix1 = (Pix) null;
    Sarray sa = this.SarrayCreate(0);
    this.SarraySplitString(sa, sequence, '+');
    int n = sa.N;
    Sarray sarray;
    if (this.MorphSequenceVerify(sa) != 1)
    {
      sarray = (Sarray) null;
      Console.WriteLine("sequence not valid");
      return (Pix) null;
    }
    int npix = 0;
    Pix pix2 = this.PixCopy((Pix) null, pixs);
    Pix pix3 = (Pix) null;
    int y;
    int x = y = 0;
    int hsize = 1;
    int vsize = 6;
    for (int index1 = 0; index1 < n; ++index1)
    {
      char[] charArray = sa.Array[index1].Replace(" \n\t", "").ToCharArray();
      switch (charArray[0])
      {
        case 'B':
        case 'b':
          npix = 1;
          Pix pix4 = this.PixAddBorder(pix2, npix, 0U);
          pix1 = (Pix) null;
          pix2 = pix4;
          pix3 = (Pix) null;
          if (dispsep > 0)
          {
            this.PixDisplay(pix2, x, y);
            x += dispsep;
            break;
          }
          break;
        case 'C':
        case 'c':
          this.PixCloseSafeBrick(pix2, pix2, hsize, vsize);
          if (dispsep > 0)
          {
            this.PixDisplay(pix2, x, y);
            x += dispsep;
            break;
          }
          break;
        case 'D':
        case 'd':
          Pix pix5 = this.PixDilateBrick((Pix) null, pix2, hsize, vsize);
          pix1 = (Pix) null;
          pix2 = pix5;
          pix3 = (Pix) null;
          if (dispsep > 0)
          {
            this.PixDisplay(pix2, x, y);
            x += dispsep;
            break;
          }
          break;
        case 'E':
        case 'e':
          Pix pix6 = this.PixErodeBrick((Pix) null, pix2, hsize, vsize);
          pix1 = (Pix) null;
          pix2 = pix6;
          pix3 = (Pix) null;
          if (dispsep > 0)
          {
            this.PixDisplay(pix2, x, y);
            x += dispsep;
            break;
          }
          break;
        case 'O':
        case 'o':
          this.PixOpenBrick(pix2, pix2, hsize, vsize);
          if (dispsep > 0)
          {
            this.PixDisplay(pix2, x, y);
            x += dispsep;
            break;
          }
          break;
        case 'R':
        case 'r':
          int num = charArray.Length - 1;
          for (int index2 = 0; index2 < num; ++index2)
            numArray[index2] = (int) charArray[index2 + 1] - 48 /*0x30*/;
          for (int index3 = num; index3 < 4; ++index3)
            numArray[index3] = 0;
          Pix pix7 = this.PixReduceRankBinaryCascade(pix2, numArray[0], numArray[1], numArray[2], numArray[3]);
          pix1 = (Pix) null;
          pix2 = pix7;
          pix3 = (Pix) null;
          if (dispsep > 0)
          {
            this.PixDisplay(pix2, x, y);
            x += dispsep;
            break;
          }
          break;
        case 'X':
        case 'x':
          int factor = 1;
          Pix pix8 = this.PixExpandReplicate(pix2, factor);
          pix1 = (Pix) null;
          pix2 = pix8;
          pix3 = (Pix) null;
          if (dispsep > 0)
          {
            this.PixDisplay(pix2, x, y);
            x += dispsep;
            break;
          }
          break;
      }
    }
    if (npix > 0)
    {
      Pix pix9 = this.PixRemoveBorder(pix2, npix);
      pix1 = (Pix) null;
      pix2 = pix9;
      pix3 = (Pix) null;
    }
    sarray = (Sarray) null;
    return pix2;
  }

  private Pix PixExpandReplicate(Pix pixs, int factor)
  {
    int w = pixs.W;
    int h = pixs.H;
    int d = pixs.D;
    switch (d)
    {
      case 1:
      case 2:
      case 4:
      case 8:
      case 16 /*0x10*/:
      case 32 /*0x20*/:
        if (factor <= 0)
          return (Pix) null;
        if (factor == 1)
          return this.PixCopy((Pix) null, pixs);
        if (d == 1)
          return this.PixExpandBinaryReplicate(pixs, factor);
        int width = factor * w;
        int height = factor * h;
        Pix pix;
        if ((pix = this.PixCreate(width, height, d)) == null)
        {
          Console.WriteLine("pixd not made");
          return (Pix) null;
        }
        this.PixCopyColormap(pix, pixs);
        this.PixCopyResolution(pix, pixs);
        this.PixScaleResolution(pix, (float) factor, (float) factor);
        uint[] data1 = pixs.Data;
        int wpl1 = pixs.Wpl;
        uint[] data2 = pix.Data;
        int wpl2 = pix.Wpl;
        int num1 = 0;
        switch (d)
        {
          case 2:
            int num2 = (width + 3) / 4;
            for (int index1 = 0; index1 < h; ++index1)
            {
              int index2 = index1 * wpl1;
              int num3 = factor * index1 * wpl2;
              for (int n = 0; n < w; ++n)
              {
                uint dataDibit = JBIG2Statics.GetDataDibit(data1[index2], n);
                int num4 = factor * n;
                for (int index3 = 0; index3 < factor; ++index3)
                  JBIG2Statics.SetDataDibit(ref data2, num3 + num4 + index3, dataDibit);
              }
              for (int index4 = 1; index4 < factor; ++index4)
              {
                Array.Copy((Array) data2, 0, (Array) data2, index4 * wpl2, index4 * wpl2);
                uint[] sourceArray = new uint[index4 * wpl2];
                Array.Copy((Array) sourceArray, (Array) data2, sourceArray.Length);
              }
            }
            break;
          case 4:
            int num5 = (width + 1) / 2;
            for (int index5 = 0; index5 < h; ++index5)
            {
              int index6 = index5 * wpl1;
              int num6 = factor * index5 * wpl2;
              for (int n = 0; n < w; ++n)
              {
                uint dataQbit = JBIG2Statics.GetDataQbit(data1[index6], n);
                int num7 = factor * n;
                for (int index7 = 0; index7 < factor; ++index7)
                  JBIG2Statics.SetDataQbit(ref data2, num6 + num7 + index7, dataQbit);
              }
              for (int index8 = 1; index8 < factor; ++index8)
              {
                Array.Copy((Array) data2, 0, (Array) data2, index8 * wpl2, index8 * wpl2);
                uint[] sourceArray = new uint[index8 * wpl2];
                Array.Copy((Array) sourceArray, (Array) data2, sourceArray.Length);
              }
            }
            break;
          case 8:
            for (int index9 = 0; index9 < h; ++index9)
            {
              int num8 = index9 * wpl1;
              num1 = factor * index9 * wpl2;
              for (int index10 = 0; index10 < w; ++index10)
              {
                uint dataByte = JBIG2Statics.GetDataByte(data1, num8 + index10);
                int num9 = factor * index10;
                for (int index11 = 0; index11 < factor; ++index11)
                  JBIG2Statics.SetDataByte(ref data2, num8 + num9 + index11, dataByte);
              }
              for (int index12 = 1; index12 < factor; ++index12)
              {
                Array.Copy((Array) data2, 0, (Array) data2, index12 * wpl2, index12 * wpl2);
                uint[] sourceArray = new uint[index12 * wpl2];
                Array.Copy((Array) sourceArray, (Array) data2, sourceArray.Length);
              }
            }
            break;
          case 16 /*0x10*/:
            for (int index13 = 0; index13 < h; ++index13)
            {
              int num10 = index13 * wpl1;
              int num11 = factor * index13 * wpl2;
              for (int index14 = 0; index14 < w; ++index14)
              {
                short dataTwoBytes = JBIG2Statics.GetDataTwoBytes(data1, num10 + index14);
                int num12 = factor * index14;
                for (int index15 = 0; index15 < factor; ++index15)
                  JBIG2Statics.SetDataTwoBytes(ref data2, num11 + num12 + index15, dataTwoBytes);
              }
              for (int index16 = 1; index16 < factor; ++index16)
              {
                Array.Copy((Array) data2, 0, (Array) data2, index16 * wpl2, index16 * wpl2);
                uint[] sourceArray = new uint[index16 * wpl2];
                Array.Copy((Array) sourceArray, (Array) data2, sourceArray.Length);
              }
            }
            break;
          case 32 /*0x20*/:
            for (int index17 = 0; index17 < h; ++index17)
            {
              int num13 = index17 * wpl1;
              int num14 = factor * index17 * wpl2;
              for (int index18 = 0; index18 < w; ++index18)
              {
                uint num15 = data1[num13 + index18];
                int num16 = factor * index18;
                for (int index19 = 0; index19 < factor; ++index19)
                  data2[num14 + num16 + index19] = num15;
              }
              for (int index20 = 1; index20 < factor; ++index20)
              {
                Array.Copy((Array) data2, 0, (Array) data2, index20 * wpl2, index20 * wpl2);
                uint[] sourceArray = new uint[index20 * wpl2];
                Array.Copy((Array) sourceArray, (Array) data2, sourceArray.Length);
              }
            }
            break;
          default:
            Console.WriteLine("invalid depth");
            break;
        }
        return pix;
      default:
        return (Pix) null;
    }
  }

  private Pix PixExpandBinaryReplicate(Pix pixs, int factor)
  {
    int w = pixs.W;
    int h = pixs.H;
    int d = pixs.D;
    switch (factor)
    {
      case 1:
        return this.PixCopy((Pix) null, pixs);
      case 2:
      case 4:
      case 8:
      case 16 /*0x10*/:
        return this.PixExpandBinaryPower2(pixs, factor);
      default:
        int wpl1 = pixs.Wpl;
        uint[] data1 = pixs.Data;
        Pix pix = this.PixCreate(factor * w, factor * h, 1);
        this.PixCopyResolution(pix, pixs);
        this.PixScaleResolution(pix, (float) factor, (float) factor);
        int wpl2 = pix.Wpl;
        uint[] data2 = pix.Data;
        for (int index1 = 0; index1 < h; ++index1)
        {
          int index2 = wpl1 * index1;
          int index3 = factor * index1 * wpl2;
          for (int n = 0; n < w; ++n)
          {
            if (JBIG2Statics.GetDataBit(data1, index2, n) == 1U)
            {
              int num = factor * n;
              for (int index4 = 0; index4 < factor; ++index4)
                JBIG2Statics.SetDataBit(ref data2[index3], num + index4);
            }
          }
          for (int index5 = 1; index5 < factor; ++index5)
          {
            Array.Copy((Array) data2, 0, (Array) data2, index5 * wpl2, index5 * wpl2);
            uint[] sourceArray = new uint[index5 * wpl2];
            Array.Copy((Array) sourceArray, (Array) data2, sourceArray.Length);
          }
        }
        return pix;
    }
  }

  private Pix PixExpandBinaryPower2(Pix pixs, int factor)
  {
    int w = pixs.W;
    int h = pixs.H;
    int d = pixs.D;
    switch (factor)
    {
      case 1:
        return this.PixCopy((Pix) null, pixs);
      case 2:
      case 4:
      case 8:
      case 16 /*0x10*/:
        int wpl1 = pixs.Wpl;
        uint[] data = pixs.Data;
        int num1 = factor * w;
        int num2 = factor * h;
        Pix pix = this.PixCreate(num1, num2, 1);
        this.PixCopyResolution(pix, pixs);
        this.PixScaleResolution(pix, (float) factor, (float) factor);
        int wpl2 = pix.Wpl;
        this.ExpandBinaryPower2Low(pix.Data, num1, num2, wpl2, data, w, h, wpl1, factor);
        return pix;
      default:
        return (Pix) null;
    }
  }

  private Pix PixCloseSafeBrick(Pix pixd, Pix pixs, int hsize, int vsize)
  {
    int d = pixs.D;
    if (hsize >= 1)
      ;
    if (hsize == 1 && vsize == 1)
      return this.PixCopy(pixd, pixs);
    if (this.MORPH_BC == 0)
      return this.PixCloseBrick(pixd, pixs, hsize, vsize);
    int npix = 32 /*0x20*/ * ((Math.Max(hsize / 2, vsize / 2) + 31 /*0x1F*/) / 32 /*0x20*/);
    Pix pixs1 = this.PixAddBorder(pixs, npix, 0U);
    Pix pix1;
    if (hsize == 1 || vsize == 1)
    {
      Sel brick = this.SelCreateBrick(vsize, hsize, vsize / 2, hsize / 2, JBIG2Statics.SelHit);
      pix1 = this.PixClose((Pix) null, pixs1, brick);
    }
    else
    {
      Sel brick1 = this.SelCreateBrick(1, hsize, 0, hsize / 2, JBIG2Statics.SelHit);
      Sel brick2 = this.SelCreateBrick(vsize, 1, vsize / 2, 0, JBIG2Statics.SelHit);
      Pix pix2 = this.PixDilate((Pix) null, pixs1, brick1);
      pix1 = this.PixDilate((Pix) null, pix2, brick2);
      this.PixErode(pix2, pix1, brick1);
      this.PixErode(pix1, pix2, brick2);
    }
    Pix pixs2 = this.PixRemoveBorder(pix1, npix);
    if (pixd == null)
      pixd = pixs2;
    else
      this.PixCopy(pixd, pixs2);
    return pixd;
  }

  private Pix PixRemoveBorder(Pix pixs, int npix)
  {
    return npix == 0 ? pixs : this.PixRemoveBorderGeneral(pixs, npix, npix, npix, npix);
  }

  private Pix PixRemoveBorderGeneral(Pix pixs, int left, int right, int top, int bot)
  {
    if (left >= 0 && right >= 0 && top >= 0)
      ;
    int w = pixs.W;
    int h = pixs.H;
    int d = pixs.D;
    int num1 = w - left - right;
    int num2 = h - top - bot;
    Pix noInit = this.PixCreateNoInit(num1, num2, d);
    this.PixCopyResolution(noInit, pixs);
    this.PixCopyColormap(noInit, pixs);
    this.PixRasterop(noInit, 0, 0, num1, num2, JBIG2Statics.PixSrc, pixs, left, top);
    return noInit;
  }

  private Pix PixAddBorder(Pix pixs, int npix, uint val)
  {
    return npix == 0 ? pixs : this.PixAddBorderGeneral(pixs, npix, npix, npix, npix, val);
  }

  private Pix PixAddBorderGeneral(Pix pixs, int left, int right, int top, int bot, uint val)
  {
    if (left < 0 || right < 0 || top < 0 || bot < 0)
      return (Pix) null;
    int w = pixs.W;
    int h = pixs.H;
    int d = pixs.D;
    int num1 = w + left + right;
    int num2 = h + top + bot;
    Pix noInit;
    if ((noInit = this.PixCreateNoInit(num1, num2, d)) == null)
      return (Pix) null;
    this.PixCopyResolution(noInit, pixs);
    this.PixCopyColormap(noInit, pixs);
    int op = JBIG2Statics.Undef;
    if (val == 0U)
      op = JBIG2Statics.PixClr;
    else if (d == 1 && val == 1U || d == 2 && val == 3U || d == 4 && val == 15U || d == 8 && val == (uint) byte.MaxValue || d == 16 /*0x10*/ && val == (uint) ushort.MaxValue || d == 32 /*0x20*/ && val >> 8 == 16777215U /*0xFFFFFF*/)
      op = JBIG2Statics.PixSet;
    if (op == JBIG2Statics.Undef)
    {
      this.PixSetAllArbitrary(noInit, val);
    }
    else
    {
      this.PixRasterop(noInit, 0, 0, left, num2, op, (Pix) null, 0, 0);
      this.PixRasterop(noInit, num1 - right, 0, right, num2, op, (Pix) null, 0, 0);
      this.PixRasterop(noInit, 0, 0, num1, top, op, (Pix) null, 0, 0);
      this.PixRasterop(noInit, 0, num2 - bot, num1, bot, op, (Pix) null, 0, 0);
    }
    this.PixRasterop(noInit, left, top, w, h, JBIG2Statics.PixSrc, pixs, 0, 0);
    return noInit;
  }

  private Pix PixCloseBrick(Pix pixd, Pix pixs, int hsize, int vsize)
  {
    int d = pixs.D;
    if (hsize >= 1)
      ;
    if (hsize == 1 && vsize == 1)
      return this.PixCopy(pixd, pixs);
    if (hsize == 1 || vsize == 1)
    {
      Sel brick = this.SelCreateBrick(vsize, hsize, vsize / 2, hsize / 2, JBIG2Statics.SelHit);
      pixd = this.PixClose(pixd, pixs, brick);
    }
    else
    {
      Sel brick1 = this.SelCreateBrick(1, hsize, 0, hsize / 2, JBIG2Statics.SelHit);
      Sel brick2 = this.SelCreateBrick(vsize, 1, vsize / 2, 0, JBIG2Statics.SelHit);
      Pix pix = this.PixDilate((Pix) null, pixs, brick1);
      pixd = this.PixDilate(pixd, pix, brick2);
      this.PixErode(pix, pixd, brick1);
      this.PixErode(pixd, pix, brick2);
    }
    return pixd;
  }

  private Pix PixClose(Pix pixd, Pix pixs, Sel sel)
  {
    pixd = this.ProcessMorphArgs2(pixd, pixs, sel);
    Pix pixs1 = this.PixDilate((Pix) null, pixs, sel);
    this.PixErode(pixd, pixs1, sel);
    return pixd;
  }

  private Pix PixOpenBrick(Pix pixd, Pix pixs, int hsize, int vsize)
  {
    int d = pixs.D;
    if (hsize >= 1)
      ;
    if (hsize == 1 && vsize == 1)
      return this.PixCopy(pixd, pixs);
    if (hsize == 1 || vsize == 1)
    {
      Sel brick = this.SelCreateBrick(vsize, hsize, vsize / 2, hsize / 2, JBIG2Statics.SelHit);
      pixd = this.PixOpen(pixd, pixs, brick);
    }
    else
    {
      Sel brick1 = this.SelCreateBrick(1, hsize, 0, hsize / 2, JBIG2Statics.SelHit);
      Sel brick2 = this.SelCreateBrick(vsize, 1, vsize / 2, 0, JBIG2Statics.SelHit);
      Pix pix = this.PixErode((Pix) null, pixs, brick1);
      pixd = this.PixErode(pixd, pix, brick2);
      this.PixDilate(pix, pixd, brick1);
      this.PixDilate(pixd, pix, brick2);
    }
    return pixd;
  }

  private Pix PixOpen(Pix pixd, Pix pixs, Sel sel)
  {
    pixd = this.ProcessMorphArgs2(pixd, pixs, sel);
    Pix pixs1 = this.PixErode((Pix) null, pixs, sel);
    this.PixDilate(pixd, pixs1, sel);
    return pixd;
  }

  private Pix ProcessMorphArgs2(Pix pixd, Pix pixs, Sel sel)
  {
    int d = pixs.D;
    int sx = sel.SX;
    int sy = sel.SY;
    if (sx != 0)
      ;
    if (pixd == null)
      return this.PixCreateTemplate(pixs);
    this.PixResizeImageData(pixd, pixs);
    return pixd;
  }

  private void PixDisplay(Pix pixs, int x, int y)
  {
    this.PixDisplayWithTitle(pixs, x, y, char.MinValue, 1);
  }

  private void PixDisplayWithTitle(Pix pixs, int x, int y, char title, int dispflag)
  {
    int num1 = 1;
    int num2 = 2;
    int num3 = 3;
    int num4 = 4;
    int num5 = 1000;
    int num6 = 800;
    if (dispflag != 1)
      return;
    if (num4 != num1 && num4 != num2 && num4 != num3)
      ;
    int w = pixs.W;
    int h = pixs.H;
    int d = pixs.D;
    if (w <= num5 && h <= num6)
    {
      if (d != 16 /*0x10*/)
        return;
      this.PixConvert16To8(pixs, 1);
    }
    else
    {
      float num7 = Math.Min((float) num5 / (float) w, (float) num6 / (float) h);
      if ((double) num7 < 0.125 && d == 1)
        this.PixScaleToGray8(pixs);
      else if ((double) num7 < 0.25 && d == 1)
        this.PixScaleToGray4(pixs);
      else if ((double) num7 < 0.33 && d == 1)
        this.PixScaleToGray3(pixs);
      else if ((double) num7 < 0.5 && d == 1)
        this.PixScaleToGray2(pixs);
      else
        this.PixScale(pixs, num7, num7);
    }
  }

  private Pix PixScale(Pix pixs, float scalex, float scaley)
  {
    float num = Math.Max(scalex, scaley);
    float sharpfract = (double) num < 0.699999988079071 ? 0.2f : 0.4f;
    int sharpwidth = (double) num < 0.7 ? 1 : 2;
    return this.PixScaleGeneral(pixs, scalex, scaley, sharpfract, sharpwidth);
  }

  private Pix PixScaleGeneral(
    Pix pixs,
    float scalex,
    float scaley,
    float sharpfract,
    int sharpwidth)
  {
    int d1 = pixs.D;
    switch (d1)
    {
      case 1:
      case 2:
      case 4:
      case 8:
      case 16 /*0x10*/:
        if ((double) scalex == 1.0 && (double) scaley == 1.0)
          return this.PixCopy((Pix) null, pixs);
        if (d1 == 1)
          return this.PixScaleBinary(pixs, scalex, scaley);
        Pix pix1 = this.PixConvertTo8Or32(pixs, 0, 1);
        int d2 = pix1.D;
        float num = Math.Max(scalex, scaley);
        Pix pix2;
        if ((double) num < 0.7)
        {
          Pix pixs1 = this.PixScaleAreaMap(pix1, scalex, scaley);
          pix2 = (double) num <= 0.2 || (double) sharpfract <= 0.0 || sharpwidth <= 0 ? pixs1 : this.PixUnsharpMasking(pixs1, sharpwidth, sharpfract);
        }
        else
        {
          Pix pixs2 = d2 != 8 ? this.PixScaleColorLI(pix1, scalex, scaley) : this.PixScaleGrayLI(pix1, scalex, scaley);
          pix2 = (double) num >= 1.4 || (double) sharpfract <= 0.0 || sharpwidth <= 0 ? pixs2 : this.PixUnsharpMasking(pixs2, sharpwidth, sharpfract);
        }
        return pix2;
      default:
        goto case 1;
    }
  }

  private Pix PixScaleColorLI(Pix pixs, float scalex, float scaley)
  {
    if (pixs != null)
    {
      int d = pixs.D;
    }
    if ((double) Math.Max(scalex, scaley) < 0.7)
      return this.PixScale(pixs, scalex, scaley);
    if ((double) scalex == 1.0 && (double) scaley == 1.0)
      return this.PixCopy((Pix) null, pixs);
    if ((double) scalex == 2.0 && (double) scaley == 2.0)
      return this.PixScaleColor2xLI(pixs);
    if ((double) scalex == 4.0 && (double) scaley == 4.0)
      return this.PixScaleColor4xLI(pixs);
    int w = pixs.W;
    int h = pixs.H;
    uint[] data1 = pixs.Data;
    int wpl1 = pixs.Wpl;
    int num1 = (int) ((double) scalex * (double) w + 0.5);
    int num2 = (int) ((double) scaley * (double) h + 0.5);
    Pix pix = this.PixCreate(num1, num2, 32 /*0x20*/);
    this.PixCopyResolution(pix, pixs);
    this.PixScaleResolution(pix, scalex, scaley);
    uint[] data2 = pix.Data;
    int wpl2 = pix.Wpl;
    this.ScaleColorLILow(data2, num1, num2, wpl2, data1, w, h, wpl1);
    return pix;
  }

  private void ScaleColorLILow(
    uint[] datad,
    int wd,
    int hd,
    int wpld,
    uint[] datas,
    int ws,
    int hs,
    int wpls)
  {
    float num1 = 16f * (float) ws / (float) wd;
    float num2 = 16f * (float) hs / (float) hd;
    int num3 = ws - 2;
    int num4 = hs - 2;
    for (int index1 = 0; index1 < hd; ++index1)
    {
      uint num5 = (uint) ((double) num2 * (double) index1);
      uint num6 = num5 >> 4;
      uint num7 = num5 & 15U;
      int num8 = index1 * wpld;
      for (int index2 = 0; index2 < wd; ++index2)
      {
        uint num9 = (uint) ((double) num1 * (double) index2);
        uint index3 = num9 >> 4;
        uint num10 = num9 & 15U;
        uint data = datas[(IntPtr) index3];
        uint num11;
        uint num12;
        uint num13;
        if ((long) index3 > (long) num3 || (long) num6 > (long) num4)
        {
          if ((long) num6 > (long) num4 && (long) index3 <= (long) num3)
          {
            num11 = datas[(IntPtr) (index3 + 1U)];
            num12 = data;
            num13 = num11;
          }
          else if ((long) index3 > (long) num3 && (long) num6 <= (long) num4)
          {
            num11 = data;
            num12 = datas[(long) wpls + (long) index3];
            num13 = num12;
          }
          else
          {
            int num14;
            num11 = (uint) (num14 = (int) data);
            num12 = (uint) num14;
            num13 = (uint) num14;
          }
        }
        else
        {
          num11 = datas[(IntPtr) (index3 + 1U)];
          num12 = datas[(long) wpls + (long) index3];
          num13 = datas[(long) wpls + (long) index3 + 1L];
        }
        uint num15 = (uint) ((16 /*0x10*/ - (int) num10) * (16 /*0x10*/ - (int) num7));
        uint num16 = num10 * (16U /*0x10*/ - num7);
        uint num17 = (16U /*0x10*/ - num10) * num7;
        uint num18 = num10 * num7;
        uint num19 = num15 * (data >> this.m_redShift & (uint) byte.MaxValue);
        uint num20 = num15 * (data >> this.m_greenShift & (uint) byte.MaxValue);
        uint num21 = num15 * (data >> this.m_blueShift & (uint) byte.MaxValue);
        uint num22 = num16 * (num11 >> this.m_redShift & (uint) byte.MaxValue);
        uint num23 = num16 * (num11 >> this.m_greenShift & (uint) byte.MaxValue);
        uint num24 = num16 * (num11 >> this.m_blueShift & (uint) byte.MaxValue);
        uint num25 = num17 * (num12 >> this.m_redShift & (uint) byte.MaxValue);
        uint num26 = num17 * (num12 >> this.m_greenShift & (uint) byte.MaxValue);
        uint num27 = num17 * (num12 >> this.m_blueShift & (uint) byte.MaxValue);
        uint num28 = num18 * (num13 >> this.m_redShift & (uint) byte.MaxValue);
        uint num29 = num18 * (num13 >> this.m_greenShift & (uint) byte.MaxValue);
        uint num30 = num18 * (num13 >> this.m_blueShift & (uint) byte.MaxValue);
        uint num31 = (uint) ((int) num19 + (int) num22 + (int) num25 + (int) num28 + 128 /*0x80*/ << 16 /*0x10*/ & -16777216 /*0xFF000000*/ | (int) num20 + (int) num23 + (int) num26 + (int) num29 + 128 /*0x80*/ << 8 & 16711680 /*0xFF0000*/ | (int) num21 + (int) num24 + (int) num27 + (int) num30 + 128 /*0x80*/ & 65280);
        datad[num8 + index2] = num31;
      }
    }
  }

  private Pix PixScaleColor4xLI(Pix pixs)
  {
    if (pixs != null)
    {
      int d = pixs.D;
    }
    return this.PixCreateRGBImage(this.PixScaleGray4xLI(this.PixGetRGBComponent(pixs, 0)), this.PixScaleGray4xLI(this.PixGetRGBComponent(pixs, 1)), this.PixScaleGray4xLI(this.PixGetRGBComponent(pixs, 2)));
  }

  private Pix PixScaleColor2xLI(Pix pixs)
  {
    if (pixs != null)
    {
      int d = pixs.D;
    }
    int w = pixs.W;
    int h = pixs.H;
    uint[] data1 = pixs.Data;
    int wpl1 = pixs.Wpl;
    Pix pix = this.PixCreate(2 * w, 2 * h, 32 /*0x20*/);
    this.PixCopyResolution(pix, pixs);
    this.PixScaleResolution(pix, 2f, 2f);
    uint[] data2 = pix.Data;
    int wpl2 = pix.Wpl;
    this.ScaleColor2xLILow(ref data2, wpl2, data1, w, h, wpl1);
    pix.Data = data2;
    return pix;
  }

  private void ScaleColor2xLILow(
    ref uint[] datad,
    int wpld,
    uint[] datas,
    int ws,
    int hs,
    int wpls)
  {
    int num = hs - 1;
    for (int index = 0; index < num; ++index)
    {
      int srcIndex = index * wpls;
      int destIndex = 2 * index * wpld;
      this.ScaleColor2xLILineLow(ref datad, destIndex, wpld, datas, srcIndex, ws, wpls, 0);
    }
    int srcIndex1 = num * wpls;
    int destIndex1 = 2 * num * wpld;
    this.ScaleColor2xLILineLow(ref datad, destIndex1, wpld, datas, srcIndex1, ws, wpls, 1);
  }

  private void ScaleColor2xLILineLow(
    ref uint[] datad,
    int destIndex,
    int wpld,
    uint[] datas,
    int srcIndex,
    int ws,
    int wpls,
    int lastlineflag)
  {
    int num1 = ws - 1;
    if (lastlineflag == 0)
    {
      int index = srcIndex + wpls;
      int num2 = destIndex + wpld;
      uint data1 = datas[srcIndex];
      uint data2 = datas[index];
      uint num3 = data1 >> 24;
      uint num4 = data1 >> 16 /*0x10*/ & (uint) byte.MaxValue;
      uint num5 = data1 >> 8 & (uint) byte.MaxValue;
      uint num6 = data2 >> 24;
      uint num7 = data2 >> 16 /*0x10*/ & (uint) byte.MaxValue;
      uint num8 = data2 >> 8 & (uint) byte.MaxValue;
      int num9 = 0;
      int num10 = 0;
      while (num9 < num1)
      {
        uint num11 = num3;
        uint num12 = num4;
        uint num13 = num5;
        uint num14 = num6;
        uint num15 = num7;
        uint num16 = num8;
        uint data3 = datas[srcIndex + num9 + 1];
        uint data4 = datas[index + num9 + 1];
        num3 = data3 >> 24;
        num4 = data3 >> 16 /*0x10*/ & (uint) byte.MaxValue;
        num5 = data3 >> 8 & (uint) byte.MaxValue;
        num6 = data4 >> 24;
        num7 = data4 >> 16 /*0x10*/ & (uint) byte.MaxValue;
        num8 = data4 >> 8 & (uint) byte.MaxValue;
        uint num17 = (uint) ((int) num11 << 24 | (int) num12 << 16 /*0x10*/ | (int) num13 << 8);
        datad[destIndex + num10] = num17;
        uint num18 = (uint) ((int) num11 + (int) num3 << 23 & -16777216 /*0xFF000000*/ | (int) num12 + (int) num4 << 15 & 16711680 /*0xFF0000*/ | (int) num13 + (int) num5 << 7 & 65280);
        datad[destIndex + num10 + 1] = num18;
        uint num19 = (uint) ((int) num11 + (int) num14 << 23 & -16777216 /*0xFF000000*/ | (int) num12 + (int) num15 << 15 & 16711680 /*0xFF0000*/ | (int) num13 + (int) num16 << 7 & 65280);
        datad[num2 + num10] = num19;
        uint num20 = (uint) ((int) num11 + (int) num3 + (int) num14 + (int) num6 << 22 & -16777216 /*0xFF000000*/ | (int) num12 + (int) num4 + (int) num15 + (int) num7 << 14 & 16711680 /*0xFF0000*/ | (int) num13 + (int) num5 + (int) num16 + (int) num8 << 6 & 65280);
        datad[num2 + num10 + 1] = num20;
        ++num9;
        num10 += 2;
      }
      uint num21 = num3;
      uint num22 = num4;
      uint num23 = num5;
      uint num24 = num6;
      uint num25 = num7;
      uint num26 = num8;
      uint num27 = (uint) ((int) num21 << 24 | (int) num22 << 16 /*0x10*/ | (int) num23 << 8);
      datad[destIndex + 2 * num1] = num27;
      datad[destIndex + 2 * num1 + 1] = num27;
      uint num28 = (uint) ((int) num21 + (int) num24 << 23 & -16777216 /*0xFF000000*/ | (int) num22 + (int) num25 << 15 & 16711680 /*0xFF0000*/ | (int) num23 + (int) num26 << 7 & 65280);
      datad[num2 + 2 * num1] = num28;
      datad[num2 + 2 * num1 + 1] = num28;
    }
    else
    {
      int num29 = srcIndex + wpld;
      uint data5 = datas[srcIndex];
      uint num30 = data5 >> 24;
      uint num31 = data5 >> 16 /*0x10*/ & (uint) byte.MaxValue;
      uint num32 = data5 >> 8 & (uint) byte.MaxValue;
      int num33 = 0;
      int num34 = 0;
      while (num33 < num1)
      {
        uint num35 = num30;
        uint num36 = num31;
        uint num37 = num32;
        uint data6 = datas[srcIndex + num33 + 1];
        num30 = data6 >> 24;
        num31 = data6 >> 16 /*0x10*/ & (uint) byte.MaxValue;
        num32 = data6 >> 8 & (uint) byte.MaxValue;
        uint num38 = (uint) ((int) num35 << 24 | (int) num36 << 16 /*0x10*/ | (int) num37 << 8);
        datad[destIndex + num34] = num38;
        datad[num29 + num33] = num38;
        uint num39 = (uint) ((int) num35 + (int) num30 << 23 & -16777216 /*0xFF000000*/ | (int) num36 + (int) num31 << 15 & 16711680 /*0xFF0000*/ | (int) num37 + (int) num32 << 7 & 65280);
        datad[destIndex + num34 + 1] = num39;
        datad[num29 + num34 + 1] = num39;
        ++num33;
        num34 += 2;
      }
      uint num40 = (uint) ((int) num30 << 24 | (int) num31 << 16 /*0x10*/ | (int) num32 << 8);
      datad[destIndex + 2 * num1] = num40;
      datad[destIndex + 2 * num1 + 1] = num40;
      datad[num29 + 2 * num1] = num40;
      datad[num29 + 2 * num1 + 1] = num40;
    }
  }

  private Pix PixScaleGrayLI(Pix pixs, float scalex, float scaley)
  {
    if (pixs != null)
    {
      int d = pixs.D;
    }
    if ((double) Math.Max(scalex, scaley) < 0.7)
      return this.PixScale(pixs, scalex, scaley);
    PixColormap colormap = pixs.Colormap;
    if ((double) scalex == 1.0 && (double) scaley == 1.0)
      return this.PixCopy((Pix) null, pixs);
    if ((double) scalex == 2.0 && (double) scaley == 2.0)
      return this.PixScaleGray2xLI(pixs);
    if ((double) scalex == 4.0 && (double) scaley == 4.0)
      return this.PixScaleGray4xLI(pixs);
    int w = pixs.W;
    int h = pixs.H;
    uint[] data1 = pixs.Data;
    int wpl1 = pixs.Wpl;
    int num1 = (int) ((double) scalex * (double) w + 0.5);
    int num2 = (int) ((double) scaley * (double) h + 0.5);
    Pix pix = this.PixCreate(num1, num2, 8);
    this.PixCopyResolution(pix, pixs);
    this.PixScaleResolution(pix, scalex, scaley);
    uint[] data2 = pix.Data;
    int wpl2 = pix.Wpl;
    this.ScaleGrayLILow(ref data2, num1, num2, wpl2, data1, w, h, wpl1);
    pix.Data = data2;
    return pix;
  }

  private Pix PixScaleGray4xLI(Pix pixs)
  {
    if (pixs != null)
    {
      int d = pixs.D;
    }
    PixColormap colormap = pixs.Colormap;
    int w = pixs.W;
    int h = pixs.H;
    uint[] data1 = pixs.Data;
    int wpl1 = pixs.Wpl;
    Pix pix = this.PixCreate(4 * w, 4 * h, 8);
    this.PixCopyResolution(pix, pixs);
    this.PixScaleResolution(pix, 4f, 4f);
    uint[] data2 = pix.Data;
    int wpl2 = pix.Wpl;
    this.ScaleGray4xLILow(ref data2, wpl2, data1, w, h, wpl1);
    pix.Data = data2;
    return pix;
  }

  private void ScaleGray4xLILow(
    ref uint[] datad,
    int wpld,
    uint[] datas,
    int ws,
    int hs,
    int wpls)
  {
    int num = hs - 1;
    for (int index = 0; index < num; ++index)
    {
      int indexs = index * wpls;
      int indexd = 4 * index * wpld;
      this.ScaleGray4xLILineLow(ref datad, indexd, wpld, datas, indexs, ws, wpls, 0);
    }
    int indexs1 = num * wpls;
    int indexd1 = 4 * num * wpld;
    this.ScaleGray4xLILineLow(ref datad, indexd1, wpld, datas, indexs1, ws, wpls, 1);
  }

  private void ScaleGrayLILow(
    ref uint[] datad,
    int wd,
    int hd,
    int wpld,
    uint[] datas,
    int ws,
    int hs,
    int wpls)
  {
    float num1 = 16f * (float) ws / (float) wd;
    float num2 = 16f * (float) hs / (float) hd;
    int num3 = ws - 2;
    int num4 = hs - 2;
    for (int index = 0; index < hd; ++index)
    {
      uint num5 = (uint) ((double) num2 * (double) index);
      uint num6 = num5 >> 4;
      uint num7 = num5 & 15U;
      int num8 = (int) num6 * wpls;
      for (int n = 0; n < wd; ++n)
      {
        uint num9 = (uint) ((double) num1 * (double) n);
        uint num10 = num9 >> 4;
        uint num11 = num9 & 15U;
        uint dataByte = JBIG2Statics.GetDataByte(datas, num8 + (int) num10);
        uint num12;
        uint num13;
        uint num14;
        if ((long) num10 > (long) num3 || (long) num6 > (long) num4)
        {
          if ((long) num6 > (long) num4 && (long) num10 <= (long) num3)
          {
            num12 = dataByte;
            num13 = JBIG2Statics.GetDataByte(datas, num8 + (int) num10 + 1);
            num14 = num13;
          }
          else if ((long) num10 > (long) num3 && (long) num6 <= (long) num4)
          {
            num12 = JBIG2Statics.GetDataByte(datas, num8 + wpls + (int) num10);
            num13 = dataByte;
            num14 = num12;
          }
          else
          {
            int num15;
            num14 = (uint) (num15 = (int) dataByte);
            num12 = (uint) num15;
            num13 = (uint) num15;
          }
        }
        else
        {
          num13 = JBIG2Statics.GetDataByte(datas, num8 + (int) num10 + 1);
          num12 = JBIG2Statics.GetDataByte(datas, num8 + wpls + (int) num10);
          num14 = JBIG2Statics.GetDataByte(datas, num8 + wpls + (int) num10 + 1);
        }
        uint num16 = (uint) ((16 /*0x10*/ - (int) num11) * (16 /*0x10*/ - (int) num7)) * dataByte;
        uint num17 = num11 * (16U /*0x10*/ - num7) * num13;
        uint num18 = (16U /*0x10*/ - num11) * num7 * num12;
        uint num19 = num11 * num7 * num14;
        uint val = (uint) ((int) num16 + (int) num18 + (int) num17 + (int) num19 + 128 /*0x80*/) / 256U /*0x0100*/;
        JBIG2Statics.SetDataByte(ref datad, n, val);
      }
    }
  }

  private Pix PixScaleGray2xLI(Pix pixs)
  {
    if (pixs != null)
    {
      int d = pixs.D;
    }
    PixColormap colormap = pixs.Colormap;
    int w = pixs.W;
    int h = pixs.H;
    uint[] data1 = pixs.Data;
    int wpl1 = pixs.Wpl;
    Pix pix = this.PixCreate(2 * w, 2 * h, 8);
    this.PixCopyResolution(pix, pixs);
    this.PixScaleResolution(pix, 2f, 2f);
    uint[] data2 = pix.Data;
    int wpl2 = pix.Wpl;
    this.ScaleGray2xLILow(ref data2, wpl2, data1, w, h, wpl1);
    pix.Data = data2;
    return pix;
  }

  private void ScaleGray2xLILow(
    ref uint[] datad,
    int wpld,
    uint[] datas,
    int ws,
    int hs,
    int wpls)
  {
    int num = hs - 1;
    for (int index = 0; index < num; ++index)
    {
      int indexs = index * wpls;
      int indexd = 2 * index * wpld;
      this.ScaleGray2xLILineLow(ref datad, indexd, wpld, datas, indexs, ws, wpls, 0);
    }
    int indexs1 = num * wpls;
    int indexd1 = 2 * num * wpld;
    this.ScaleGray2xLILineLow(ref datad, indexd1, wpld, datas, indexs1, ws, wpls, 1);
  }

  private Pix PixUnsharpMasking(Pix pixs, int halfwidth, float fract)
  {
    if (pixs != null)
    {
      int d = pixs.D;
    }
    if ((double) fract <= 0.0 || halfwidth <= 0)
      return pixs;
    if (halfwidth == 1 || halfwidth == 2)
      return this.PixUnsharpMaskingFast(pixs, halfwidth, fract, 3);
    Pix pixs1 = this.PixConvertTo8Or32(pixs, 0, 1);
    return pixs1.D != 8 ? this.PixCreateRGBImage(this.PixUnsharpMaskingGray(this.PixGetRGBComponent(pixs, 0), halfwidth, fract), this.PixUnsharpMaskingGray(this.PixGetRGBComponent(pixs, 1), halfwidth, fract), this.PixUnsharpMaskingGray(this.PixGetRGBComponent(pixs, 2), halfwidth, fract)) : this.PixUnsharpMaskingGray(pixs1, halfwidth, fract);
  }

  private Pix PixUnsharpMaskingFast(Pix pixs, int halfwidth, float fract, int direction)
  {
    if (pixs != null)
    {
      int d = pixs.D;
    }
    if ((double) fract <= 0.0 || halfwidth <= 0)
      return pixs;
    if (halfwidth != 1)
      ;
    if (direction != 1 && direction != 2)
      ;
    Pix pixs1 = this.PixConvertTo8Or32(pixs, 0, 1);
    return pixs1.D != 8 ? this.PixCreateRGBImage(this.PixUnsharpMaskingGrayFast(this.PixGetRGBComponent(pixs, 0), halfwidth, fract, direction), this.PixUnsharpMaskingGrayFast(this.PixGetRGBComponent(pixs, 1), halfwidth, fract, direction), this.PixUnsharpMaskingGrayFast(this.PixGetRGBComponent(pixs, 2), halfwidth, fract, direction)) : this.PixUnsharpMaskingGrayFast(pixs1, halfwidth, fract, direction);
  }

  private Pix PixCreateRGBImage(Pix pixr, Pix pixg, Pix pixb)
  {
    int w1 = pixr.W;
    int h1 = pixr.H;
    int d1 = pixr.D;
    int w2 = pixg.W;
    int h2 = pixg.H;
    int d2 = pixg.D;
    int w3 = pixb.W;
    int h3 = pixb.H;
    int d3 = pixb.D;
    if (d1 == 8 && d2 == 8)
      ;
    if (w1 == w2)
      ;
    if (h1 == h2)
      ;
    Pix pixd = this.PixCreate(w1, h1, 32 /*0x20*/);
    this.PixCopyResolution(pixd, pixr);
    this.PixSetRGBComponent(pixd, pixr, 0);
    this.PixSetRGBComponent(pixd, pixg, 1);
    this.PixSetRGBComponent(pixd, pixb, 2);
    return pixd;
  }

  private void PixSetRGBComponent(Pix pixd, Pix pixs, int color)
  {
    int d1 = pixd.D;
    int d2 = pixs.D;
    if (color != 0 && color != 1 && color != 2)
      ;
    int w = pixs.W;
    int h1 = pixs.H;
    if (w == pixd.W)
    {
      int h2 = pixd.H;
    }
    uint[] data1 = pixs.Data;
    uint[] data2 = pixd.Data;
    int wpl1 = pixs.Wpl;
    int wpl2 = pixd.Wpl;
    for (int index1 = 0; index1 < h1; ++index1)
    {
      int num1 = index1 * wpl1;
      int num2 = index1 * wpl2;
      for (int index2 = 0; index2 < w; ++index2)
      {
        uint dataByte = JBIG2Statics.GetDataByte(data1, num1 + index2);
        JBIG2Statics.SetDataByte(ref data2, num2 + index2 + color, dataByte);
      }
    }
  }

  private Pix PixGetRGBComponent(Pix pixs, int color)
  {
    int d = pixs.D;
    if (color != 0 && color != 1 && color != 2)
      ;
    int w = pixs.W;
    int h = pixs.H;
    Pix pixd = this.PixCreate(w, h, 8);
    this.PixCopyResolution(pixd, pixs);
    int wpl1 = pixs.Wpl;
    int wpl2 = pixd.Wpl;
    uint[] data1 = pixs.Data;
    uint[] data2 = pixd.Data;
    for (int index1 = 0; index1 < h; ++index1)
    {
      int num1 = index1 * wpl1;
      int num2 = index1 * wpl2;
      for (int index2 = 0; index2 < w; ++index2)
      {
        uint dataByte = JBIG2Statics.GetDataByte(data1, num1 + index2 + color);
        JBIG2Statics.SetDataByte(ref data2, num2 + index2, dataByte);
      }
    }
    pixd.Data = data2;
    return pixd;
  }

  private Pix PixUnsharpMaskingGray(Pix pixs, int halfwidth, float fract)
  {
    int w = pixs.W;
    int h = pixs.H;
    if (pixs.D == 8)
    {
      PixColormap colormap = pixs.Colormap;
    }
    if ((double) fract <= 0.0 || halfwidth <= 0)
      return pixs;
    if (halfwidth == 1 || halfwidth == 2)
      return this.PixUnsharpMaskingGrayFast(pixs, halfwidth, fract, 3);
    Pix pix = this.PixBlockconvGray(pixs, (Pix) null, halfwidth, halfwidth);
    Pixacc pixacc = this.PixaccCreate(w, h, 1);
    this.PixaccAdd(pixacc, pixs);
    this.PixaccSubtract(pixacc, pix);
    this.PixaccMultConst(pixacc, fract);
    this.PixaccAdd(pixacc, pixs);
    return this.PixaccFinal(pixacc, 8);
  }

  private Pix PixaccFinal(Pixacc pixacc, int outdepth)
  {
    return this.PixFinalAccumulate(pixacc.Pix, pixacc.Offset, outdepth);
  }

  private Pix PixFinalAccumulate(Pix pixs, uint offset, int depth)
  {
    int d = pixs.D;
    if (depth != 8 && depth != 16 /*0x10*/)
      ;
    if (offset > 1073741824U /*0x40000000*/)
      offset = 1073741824U /*0x40000000*/;
    int w = pixs.W;
    int h = pixs.H;
    Pix pixd = this.PixCreate(w, h, depth);
    this.PixCopyResolution(pixd, pixs);
    uint[] data1 = pixs.Data;
    uint[] data2 = pixd.Data;
    int wpl1 = pixs.Wpl;
    int wpl2 = pixd.Wpl;
    this.FinalAccumulateLow(ref data2, w, h, depth, wpl2, data1, wpl1, offset);
    return pixd;
  }

  private void FinalAccumulateLow(
    ref uint[] datad,
    int w,
    int h,
    int d,
    int wpld,
    uint[] datas,
    int wpls,
    uint offset)
  {
    switch (d)
    {
      case 8:
        for (int index1 = 0; index1 < h; ++index1)
        {
          int num1 = index1 * wpls;
          int num2 = index1 * wpld;
          for (int index2 = 0; index2 < w; ++index2)
          {
            uint val = Math.Min((uint) byte.MaxValue, Math.Max(0U, datas[num1 + index2] - offset));
            JBIG2Statics.SetDataByte(ref datad, num2 + index2, val);
          }
        }
        break;
      case 16 /*0x10*/:
        for (int index3 = 0; index3 < h; ++index3)
        {
          int num3 = index3 * wpls;
          int num4 = index3 * wpld;
          for (int index4 = 0; index4 < w; ++index4)
          {
            uint val = Math.Min((uint) ushort.MaxValue, Math.Max(0U, datas[num3 + index4] - offset));
            JBIG2Statics.SetDataTwoBytes(ref datad, num4 + index4, (short) val);
          }
        }
        break;
      case 32 /*0x20*/:
        for (int index5 = 0; index5 < h; ++index5)
        {
          int num5 = index5 * wpls;
          int num6 = index5 * wpld;
          for (int index6 = 0; index6 < w; ++index6)
            datad[num6 + index6] = datas[num5 + index6] - offset;
        }
        break;
    }
  }

  private void PixaccMultConst(Pixacc pixacc, float factor)
  {
    this.PixMultConstAccumulate(pixacc.Pix, factor, pixacc.Offset);
  }

  private void PixMultConstAccumulate(Pix pixs, float factor, uint offset)
  {
    int d = pixs.D;
    if (offset > 1073741824U /*0x40000000*/)
      offset = 1073741824U /*0x40000000*/;
    int w = pixs.W;
    int h = pixs.H;
    uint[] data = pixs.Data;
    int wpl = pixs.Wpl;
    this.MultConstAccumulateLow(data, w, h, wpl, factor, offset);
  }

  private void MultConstAccumulateLow(
    uint[] data,
    int w,
    int h,
    int wpl,
    float factor,
    uint offset)
  {
    for (int index1 = 0; index1 < h; ++index1)
    {
      int num1 = index1 * wpl;
      for (int index2 = 0; index2 < w; ++index2)
      {
        uint num2 = (uint) ((double) (data[num1 + index2] - offset) * (double) factor) + offset;
        data[num1 + index2] = num2;
      }
    }
  }

  private void PixaccSubtract(Pixacc pixacc, Pix pix) => this.PixAccumulate(pixacc.Pix, pix, 2);

  private void PixaccAdd(Pixacc pixacc, Pix pix) => this.PixAccumulate(pixacc.Pix, pix, 1);

  private void PixAccumulate(Pix pixd, Pix pixs, int op)
  {
    if (pixd != null)
    {
      int d1 = pixd.D;
    }
    int d2 = pixs.D;
    switch (d2)
    {
      case 1:
      case 8:
      case 16 /*0x10*/:
        if (op != 1)
          ;
        uint[] data1 = pixs.Data;
        uint[] data2 = pixd.Data;
        int wpl1 = pixs.Wpl;
        int wpl2 = pixd.Wpl;
        int w1 = pixs.W;
        int h1 = pixs.H;
        int w2 = pixd.W;
        int h2 = pixd.H;
        int w3 = Math.Min(w1, w2);
        int h3 = Math.Min(h1, h2);
        this.AccumulateLow(ref data2, w3, h3, wpl2, data1, d2, wpl1, op);
        pixd.Data = data2;
        break;
      default:
        goto case 1;
    }
  }

  private void AccumulateLow(
    ref uint[] datad,
    int w,
    int h,
    int wpld,
    uint[] datas,
    int d,
    int wpls,
    int op)
  {
    int num1 = 0;
    switch (d)
    {
      case 1:
        for (int index1 = 0; index1 < h; ++index1)
        {
          num1 = index1 * wpls;
          int index2 = index1 * wpld;
          if (op == 1)
          {
            for (int n = 0; n < w; ++n)
              datad[index2 + n] += JBIG2Statics.GetDataBit(datas, index2, n);
          }
          else
          {
            for (int n = 0; n < w; ++n)
              datad[index2 + n] -= JBIG2Statics.GetDataBit(datas, index2, n);
          }
        }
        break;
      case 8:
        for (int index3 = 0; index3 < h; ++index3)
        {
          int num2 = index3 * wpls;
          int num3 = index3 * wpld;
          if (op == 1)
          {
            for (int index4 = 0; index4 < w; ++index4)
              datad[num3 + index4] += JBIG2Statics.GetDataByte(datas, num2 + index4);
          }
          else
          {
            for (int index5 = 0; index5 < w; ++index5)
              datad[num3 + index5] -= JBIG2Statics.GetDataByte(datas, num2 + index5);
          }
        }
        break;
      case 16 /*0x10*/:
        for (int index6 = 0; index6 < h; ++index6)
        {
          int num4 = index6 * wpls;
          int num5 = index6 * wpld;
          if (op == 1)
          {
            for (int index7 = 0; index7 < w; ++index7)
              datad[num5 + index7] += (uint) JBIG2Statics.GetDataTwoBytes(datas, num4 + index7);
          }
          else
          {
            for (int index8 = 0; index8 < w; ++index8)
              datad[num5 + index8] -= (uint) JBIG2Statics.GetDataTwoBytes(datas, num4 + index8);
          }
        }
        break;
      case 32 /*0x20*/:
        for (int index9 = 0; index9 < h; ++index9)
        {
          int num6 = index9 * wpls;
          int num7 = index9 * wpld;
          if (op == 1)
          {
            for (int index10 = 0; index10 < w; ++index10)
              datad[num7 + index10] += datas[num6 + index10];
          }
          else
          {
            for (int index11 = 0; index11 < w; ++index11)
              datad[num7 + index11] -= datas[num6 + index11];
          }
        }
        break;
    }
  }

  private Pixacc PixaccCreate(int w, int h, int negflag)
  {
    Pixacc pixacc = new Pixacc();
    pixacc.W = w;
    pixacc.H = h;
    Pix pix = pixacc.Pix = this.PixCreate(w, h, 32 /*0x20*/);
    if (negflag > 0)
    {
      pixacc.Offset = 1073741824U /*0x40000000*/;
      this.PixSetAllArbitrary(pixacc.Pix, pixacc.Offset);
    }
    return pixacc;
  }

  private void PixSetAllArbitrary(Pix pix, uint val)
  {
    PixColormap colormap;
    if ((colormap = pix.Colormap) != null)
    {
      int n = colormap.N;
      if (val < 0U)
        val = 0U;
      else if ((long) val >= (long) n)
        val = (uint) (n - 1);
    }
    int w = pix.W;
    int h = pix.H;
    int d = pix.D;
    uint num1 = d != 32 /*0x20*/ ? (uint) ((1 << d) - 1) : uint.MaxValue;
    if (val < 0U)
      val = 0U;
    if (val > num1)
      val = num1;
    uint num2 = 0;
    int num3 = 32 /*0x20*/ / d;
    for (int index = 0; index < num3; ++index)
      num2 |= val << index * d;
    int wpl = pix.Wpl;
    for (int index1 = 0; index1 < h; ++index1)
    {
      int num4 = index1 * wpl;
      for (int index2 = 0; index2 < wpl; ++index2)
        pix.Data[num4 + index2] = num2;
    }
  }

  private Pix PixBlockconvGray(Pix pixs, Pix pixacc, int wc, int hc)
  {
    int w = pixs.W;
    int h = pixs.H;
    int d = pixs.D;
    if (wc < 0)
      wc = 0;
    if (hc < 0)
      hc = 0;
    if (w < 2 * wc + 1 || h < 2 * hc + 1)
    {
      wc = Math.Min(wc, (w - 1) / 2);
      hc = Math.Min(hc, (h - 1) / 2);
    }
    if (wc == 0 && hc == 0)
      return this.PixCopy((Pix) null, pixs);
    Pix pix;
    if (pixacc != null)
    {
      if (pixacc.D == 32 /*0x20*/)
        pix = pixacc;
      else if ((pix = this.PixBlockconvAccum(pixs)) != null)
        ;
    }
    else
      pix = this.PixBlockconvAccum(pixs);
    Pix template = this.PixCreateTemplate(pixs);
    int wpl1 = pixs.Wpl;
    int wpl2 = pix.Wpl;
    uint[] data1 = template.Data;
    uint[] data2 = pix.Data;
    this.BlockconvLow(ref data1, w, h, wpl1, data2, wpl2, wc, hc);
    template.Data = data1;
    return template;
  }

  private void BlockconvLow(
    ref uint[] data,
    int w,
    int h,
    int wpl,
    uint[] dataa,
    int wpla,
    int wc,
    int hc)
  {
    int num1 = w - wc;
    int num2 = h - hc;
    if (num1 <= 0 || num2 <= 0)
      return;
    int num3 = 2 * wc + 1;
    int num4 = 2 * hc + 1;
    float num5 = (float) (1 / (num3 * num4));
    for (int index1 = 0; index1 < h; ++index1)
    {
      int num6 = Math.Max(index1 - 1 - hc, 0);
      int num7 = Math.Min(index1 + hc, h - 1);
      int num8 = wpl * index1;
      int num9 = wpla * num6;
      int num10 = wpla * num7;
      for (int index2 = 0; index2 < w; ++index2)
      {
        int num11 = Math.Max(index2 - 1 - wc, 0);
        int num12 = Math.Min(index2 + wc, w - 1);
        uint num13 = dataa[num10 + num12] - dataa[num10 + num11] + dataa[num9 + num11] - dataa[num9 + num12];
        uint val = (uint) ((double) num5 * (double) num13 + 0.5);
        JBIG2Statics.SetDataByte(ref data, num8 + index2, val);
      }
    }
    for (int index3 = 0; index3 <= hc; ++index3)
    {
      int num14 = hc + index3;
      float num15 = (float) num4 / (float) num14;
      int num16 = wpl * index3;
      for (int index4 = 0; index4 <= wc; ++index4)
      {
        int num17 = wc + index4;
        float num18 = (float) num3 / (float) num17;
        uint val = (uint) Math.Min((float) JBIG2Statics.GetDataByte(data, num16 + index4) * num15 * num18, (float) byte.MaxValue);
        JBIG2Statics.SetDataByte(ref data, num16 + index4, val);
      }
      for (int index5 = wc + 1; index5 < num1; ++index5)
      {
        uint val = (uint) Math.Min((float) JBIG2Statics.GetDataByte(data, num16 + index5) * num15, (float) byte.MaxValue);
        JBIG2Statics.SetDataByte(ref data, num16 + index5, val);
      }
      for (int index6 = num1; index6 < w; ++index6)
      {
        int num19 = wc + w - index6;
        float num20 = (float) num3 / (float) num19;
        uint val = (uint) Math.Min((float) JBIG2Statics.GetDataByte(data, num16 + index6) * num15 * num20, (float) byte.MaxValue);
        JBIG2Statics.SetDataByte(ref data, num16 + index6, val);
      }
    }
    for (int index7 = num2; index7 < h; ++index7)
    {
      int num21 = hc + h - index7;
      float num22 = (float) num4 / (float) num21;
      int num23 = wpl * index7;
      for (int index8 = 0; index8 <= wc; ++index8)
      {
        int num24 = wc + index8;
        float num25 = (float) num3 / (float) num24;
        uint val = (uint) Math.Min((float) JBIG2Statics.GetDataByte(data, num23 + index8) * num22 * num25, (float) byte.MaxValue);
        JBIG2Statics.SetDataByte(ref data, num23 + index8, val);
      }
      for (int index9 = wc + 1; index9 < num1; ++index9)
      {
        uint val = (uint) Math.Min((float) JBIG2Statics.GetDataByte(data, num23 + index9) * num22, (float) byte.MaxValue);
        JBIG2Statics.SetDataByte(ref data, num23 + index9, val);
      }
      for (int index10 = num1; index10 < w; ++index10)
      {
        int num26 = wc + w - index10;
        float num27 = (float) num3 / (float) num26;
        uint val = (uint) Math.Min((float) JBIG2Statics.GetDataByte(data, num23 + index10) * num22 * num27, (float) byte.MaxValue);
        JBIG2Statics.SetDataByte(ref data, num23 + index10, val);
      }
    }
    for (int index11 = hc + 1; index11 < num2; ++index11)
    {
      int num28 = wpl * index11;
      for (int index12 = 0; index12 <= wc; ++index12)
      {
        int num29 = wc + index12;
        float num30 = (float) num3 / (float) num29;
        uint val = (uint) Math.Min((float) JBIG2Statics.GetDataByte(data, num28 + index12) * num30, (float) byte.MaxValue);
        JBIG2Statics.SetDataByte(ref data, num28 + index12, val);
      }
      for (int index13 = num1; index13 < w; ++index13)
      {
        int num31 = wc + w - index13;
        float num32 = (float) num3 / (float) num31;
        uint val = (uint) Math.Min((float) JBIG2Statics.GetDataByte(data, num28 + index13) * num32, (float) byte.MaxValue);
        JBIG2Statics.SetDataByte(ref data, num28 + index13, val);
      }
    }
  }

  private Pix PixBlockconvAccum(Pix pixs)
  {
    int w = pixs.W;
    int h = pixs.H;
    int d = pixs.D;
    switch (d)
    {
      case 1:
      case 8:
        Pix pix = this.PixCreate(w, h, 32 /*0x20*/);
        uint[] data1 = pixs.Data;
        uint[] data2 = pix.Data;
        int wpl1 = pixs.Wpl;
        int wpl2 = pix.Wpl;
        this.BlockconvAccumLow(ref data2, w, h, wpl2, data1, d, wpl1);
        pix.Data = data2;
        return pix;
      default:
        goto case 1;
    }
  }

  private void BlockconvAccumLow(
    ref uint[] datad,
    int w,
    int h,
    int wpld,
    uint[] datas,
    int d,
    int wpls)
  {
    int index1 = 0;
    switch (d)
    {
      case 1:
        for (int n = 0; n < w; ++n)
        {
          uint dataBit = JBIG2Statics.GetDataBit(datas, index1, n);
          if (n == 0)
            datad[0] = dataBit;
          else
            datad[n] = datad[n - 1] + dataBit;
        }
        for (int index2 = 1; index2 < h; ++index2)
        {
          int index3 = index2 * wpls;
          int index4 = index2 * wpld;
          int index5 = index4 - wpld;
          for (int n = 0; n < w; ++n)
          {
            uint dataBit = JBIG2Statics.GetDataBit(datas, index3, n);
            if (n == 0)
              datad[index4] = dataBit + datad[index5];
            else
              datad[index4 + n] = dataBit + datad[index4 + n - 1] + datad[index5 + n] - datad[index5 + n - 1];
          }
        }
        break;
      case 8:
        for (int index6 = 0; index6 < w; ++index6)
        {
          uint dataByte = JBIG2Statics.GetDataByte(datas, index1 + index6);
          if (index6 == 0)
            datad[0] = dataByte;
          else
            datad[index6] = datad[index6 - 1] + dataByte;
        }
        for (int index7 = 1; index7 < h; ++index7)
        {
          int num = index7 * wpls;
          int index8 = index7 * wpld;
          int index9 = index8 - wpld;
          for (int index10 = 0; index10 < w; ++index10)
          {
            uint dataByte = JBIG2Statics.GetDataByte(datas, num + index10);
            if (index10 == 0)
              datad[index8] = dataByte + datad[index9];
            else
              datad[index8 + index10] = dataByte + datad[index8 + index10 - 1] + datad[index9 + index10] - datad[index9 + index10 - 1];
          }
        }
        break;
      case 32 /*0x20*/:
        for (int index11 = 0; index11 < w; ++index11)
        {
          uint data = datas[index11];
          if (index11 == 0)
            datad[0] = data;
          else
            datad[index11] = datad[index11 - 1] + data;
        }
        for (int index12 = 1; index12 < h; ++index12)
        {
          int num = index12 * wpls;
          int index13 = index12 * wpld;
          int index14 = index13 - wpld;
          for (int index15 = 0; index15 < w; ++index15)
          {
            uint data = datas[num + index15];
            if (index15 == 0)
              datad[index13] = data + datad[index14];
            else
              datad[index13 + index15] = data + datad[index13 + index15 - 1] + datad[index14 + index15] - datad[index14 + index15 - 1];
          }
        }
        break;
    }
  }

  private Pix PixUnsharpMaskingGrayFast(Pix pixs, int halfwidth, float fract, int direction)
  {
    if (pixs.D == 8)
    {
      PixColormap colormap = pixs.Colormap;
    }
    if ((double) fract <= 0.0 || halfwidth <= 0)
      return pixs;
    if (halfwidth != 1)
      ;
    if (direction != 1 && direction != 2)
      ;
    return direction == 3 ? this.PixUnsharpMaskingGray2D(pixs, halfwidth, fract) : this.PixUnsharpMaskingGray1D(pixs, halfwidth, fract, direction);
  }

  private Pix PixUnsharpMaskingGray2D(Pix pixs, int halfwidth, float fract)
  {
    float[] numArray = new float[9];
    int w = pixs.W;
    int h = pixs.H;
    if (pixs.D == 8)
    {
      PixColormap colormap = pixs.Colormap;
    }
    if ((double) fract <= 0.0 || halfwidth <= 0)
      return pixs;
    if (halfwidth != 1)
      ;
    Pix pix = this.PixCopyBorder((Pix) null, pixs, halfwidth, halfwidth, halfwidth, halfwidth);
    uint[] data1 = pix.Data;
    int wpl1 = pix.Wpl;
    uint[] data2 = pixs.Data;
    int wpl2 = pixs.Wpl;
    if (halfwidth == 1)
    {
      for (int index = 0; index < 9; ++index)
        numArray[index] = (float) (-(double) fract / 9.0);
      numArray[4] = (float) (1.0 + (double) fract * 8.0 / 9.0);
      for (int index1 = 1; index1 < h - 1; ++index1)
      {
        int num1 = (index1 - 1) * wpl2;
        int num2 = index1 * wpl2;
        int num3 = (index1 + 1) * wpl2;
        int num4 = index1 * wpl1;
        for (int index2 = 1; index2 < w - 1; ++index2)
        {
          uint val = Math.Min((uint) byte.MaxValue, Math.Max(0U, (uint) ((double) (uint) ((double) numArray[0] * (double) JBIG2Statics.GetDataByte(data2, num1 + index2 - 1) + (double) numArray[1] * (double) JBIG2Statics.GetDataByte(data2, num1 + index2) + (double) numArray[2] * (double) JBIG2Statics.GetDataByte(data2, num1 + index2 + 1) + (double) numArray[3] * (double) JBIG2Statics.GetDataByte(data2, num2 + index2 - 1) + (double) numArray[4] * (double) JBIG2Statics.GetDataByte(data2, num2 + index2) + (double) numArray[5] * (double) JBIG2Statics.GetDataByte(data2, num2 + index2 + 1) + (double) numArray[6] * (double) JBIG2Statics.GetDataByte(data2, num3 + index2 - 1) + (double) numArray[7] * (double) JBIG2Statics.GetDataByte(data2, num3 + index2) + (double) numArray[8] * (double) JBIG2Statics.GetDataByte(data2, num3 + index2 + 1)) + 0.5)));
          JBIG2Statics.SetDataByte(ref data1, num4 + index2, val);
        }
      }
      return pix;
    }
    FPix fpix = this.FpixCreate(w, h);
    float[] data3 = fpix.Data;
    int wpl3 = fpix.Wpl;
    for (int index3 = 2; index3 < h - 2; ++index3)
    {
      int num5 = index3 * wpl2;
      int num6 = index3 * wpl3;
      for (int index4 = 2; index4 < w - 2; ++index4)
      {
        uint num7 = JBIG2Statics.GetDataByte(data2, num5 + index4 - 2) + JBIG2Statics.GetDataByte(data2, num5 + index4 - 1) + JBIG2Statics.GetDataByte(data2, num5 + index4) + JBIG2Statics.GetDataByte(data2, num5 + index4 + 1) + JBIG2Statics.GetDataByte(data2, num5 + index4 + 2);
        data3[num6 + index4] = (float) num7;
      }
    }
    for (int index5 = 2; index5 < h - 2; ++index5)
    {
      int num8 = (index5 - 2) * wpl3;
      int num9 = (index5 - 1) * wpl3;
      int num10 = index5 * wpl3;
      int num11 = (index5 + 1) * wpl3;
      int num12 = (index5 + 2) * wpl3;
      int num13 = index5 * wpl1;
      int num14 = index5 * wpl2;
      for (int index6 = 2; index6 < w - 2; ++index6)
      {
        uint num15 = (uint) (0.039999999105930328 * ((double) data3[num8 + index6] + (double) data3[num9 + index6] + (double) data3[num10 + index6] + (double) data3[num11 + index6] + (double) data3[num12 + index6]));
        uint dataByte = JBIG2Statics.GetDataByte(data2, num14 + index6);
        uint val = Math.Min((uint) byte.MaxValue, Math.Max(0U, (uint) ((double) dataByte + (double) fract * (double) (dataByte - num15) + 0.5)));
        JBIG2Statics.SetDataByte(ref data1, num13 + index6, val);
      }
    }
    pix.Data = data1;
    return pix;
  }

  private FPix FpixCreate(int width, int height)
  {
    return new FPix()
    {
      W = width,
      H = height,
      Wpl = width,
      RefCount = 1,
      Data = new float[width * height]
    };
  }

  private Pix PixUnsharpMaskingGray1D(Pix pixs, int halfwidth, float fract, int direction)
  {
    float[] numArray = new float[5];
    int w = pixs.W;
    int h = pixs.H;
    if (pixs.D == 8)
    {
      PixColormap colormap = pixs.Colormap;
    }
    if ((double) fract <= 0.0 || halfwidth <= 0)
      return pixs;
    if (halfwidth != 1)
      ;
    Pix pix = this.PixCopyBorder((Pix) null, pixs, halfwidth, halfwidth, halfwidth, halfwidth);
    uint[] data1 = pixs.Data;
    uint[] data2 = pix.Data;
    int wpl1 = pixs.Wpl;
    int wpl2 = pix.Wpl;
    if (halfwidth == 1)
    {
      numArray[0] = (float) (-(double) fract / 3.0);
      numArray[1] = (float) (1.0 + (double) fract * 2.0 / 3.0);
      numArray[2] = numArray[0];
    }
    else
    {
      numArray[0] = (float) (-(double) fract / 5.0);
      numArray[1] = numArray[0];
      numArray[2] = (float) (1.0 + (double) fract * 4.0 / 5.0);
      numArray[3] = numArray[0];
      numArray[4] = numArray[0];
    }
    if (direction == 1)
    {
      for (int index1 = 0; index1 < h; ++index1)
      {
        int num1 = index1 * wpl1;
        int num2 = index1 * wpl2;
        if (halfwidth == 1)
        {
          for (int index2 = 1; index2 < w - 1; ++index2)
          {
            uint val = Math.Min((uint) byte.MaxValue, Math.Max(0U, (uint) (float) ((double) numArray[0] * (double) JBIG2Statics.GetDataByte(data1, num1 + index2 - 1) + (double) numArray[1] * (double) JBIG2Statics.GetDataByte(data1, num1 + index2) + (double) numArray[2] * (double) JBIG2Statics.GetDataByte(data1, num1 + index2 + 1))));
            JBIG2Statics.SetDataByte(ref data2, num2 + index2, val);
          }
        }
        else
        {
          for (int index3 = 2; index3 < w - 2; ++index3)
          {
            uint val = Math.Min((uint) byte.MaxValue, Math.Max(0U, (uint) (float) ((double) numArray[0] * (double) JBIG2Statics.GetDataByte(data1, num1 + index3 - 2) + (double) numArray[1] * (double) JBIG2Statics.GetDataByte(data1, num1 + index3 - 1) + (double) numArray[2] * (double) JBIG2Statics.GetDataByte(data1, num1 + index3) + (double) numArray[3] * (double) JBIG2Statics.GetDataByte(data1, num1 + index3 + 1) + (double) numArray[4] * (double) JBIG2Statics.GetDataByte(data1, num1 + index3 + 2))));
            JBIG2Statics.SetDataByte(ref data2, num2 + index3, val);
          }
        }
      }
    }
    else if (halfwidth == 1)
    {
      for (int index4 = 1; index4 < h - 1; ++index4)
      {
        int num3 = (index4 - 1) * wpl1;
        int num4 = index4 * wpl1;
        int num5 = (index4 + 1) * wpl1;
        int num6 = index4 * wpl2;
        for (int index5 = 0; index5 < w; ++index5)
        {
          uint val = Math.Min((uint) byte.MaxValue, Math.Max(0U, (uint) (float) ((double) numArray[0] * (double) JBIG2Statics.GetDataByte(data1, num3 + index5) + (double) numArray[1] * (double) JBIG2Statics.GetDataByte(data1, num4 + index5) + (double) numArray[2] * (double) JBIG2Statics.GetDataByte(data1, num5 + index5))));
          JBIG2Statics.SetDataByte(ref data2, num6 + index5, val);
        }
      }
    }
    else
    {
      for (int index6 = 2; index6 < h - 2; ++index6)
      {
        int num7 = (index6 - 2) * wpl1;
        int num8 = (index6 - 1) * wpl1;
        int num9 = index6 * wpl1;
        int num10 = (index6 + 1) * wpl1;
        int num11 = (index6 + 2) * wpl1;
        int num12 = index6 * wpl2;
        for (int index7 = 0; index7 < w; ++index7)
        {
          uint val = Math.Min((uint) byte.MaxValue, Math.Max(0U, (uint) (float) ((double) numArray[0] * (double) JBIG2Statics.GetDataByte(data1, num7 + index7) + (double) numArray[1] * (double) JBIG2Statics.GetDataByte(data1, num8 + index7) + (double) numArray[2] * (double) JBIG2Statics.GetDataByte(data1, num9 + index7) + (double) numArray[3] * (double) JBIG2Statics.GetDataByte(data1, num10 + index7) + (double) numArray[4] * (double) JBIG2Statics.GetDataByte(data1, num11 + index7))));
          JBIG2Statics.SetDataByte(ref data2, num12 + index7, val);
        }
      }
    }
    return pix;
  }

  private Pix PixCopyBorder(Pix pixd, Pix pixs, int left, int right, int top, int bot)
  {
    if (pixd != null)
    {
      if (pixd == pixs)
        return pixd;
      if (pixs.W != pixd.W || pixs.H != pixd.H || pixs.D == pixd.D)
        ;
    }
    else
      pixd = this.PixCreateTemplateNoInit(pixs);
    int w = pixs.W;
    int h = pixs.H;
    this.PixRasterop(pixd, 0, 0, left, h, JBIG2Statics.PixSrc, pixs, 0, 0);
    this.PixRasterop(pixd, w - right, 0, right, h, JBIG2Statics.PixSrc, pixs, w - right, 0);
    this.PixRasterop(pixd, 0, 0, w, top, JBIG2Statics.PixSrc, pixs, 0, 0);
    this.PixRasterop(pixd, 0, h - bot, w, bot, JBIG2Statics.PixSrc, pixs, 0, h - bot);
    return pixd;
  }

  private Pix PixScaleAreaMap(Pix pix, float scalex, float scaley)
  {
    int depth = pix.D;
    switch (depth)
    {
      case 2:
      case 4:
      case 8:
        if ((double) Math.Max(scalex, scaley) >= 0.7)
          return this.PixScale(pix, scalex, scaley);
        if ((double) scalex == 0.5 && (double) scaley == 0.5)
          return this.PixScaleAreaMap2(pix);
        if ((double) scalex == 0.25 && (double) scaley == 0.25)
          return this.PixScaleAreaMap2(this.PixScaleAreaMap2(pix));
        if ((double) scalex == 0.125 && (double) scaley == 0.125)
          return this.PixScaleAreaMap2(this.PixScaleAreaMap2(this.PixScaleAreaMap2(pix)));
        if ((double) scalex == 1.0 / 16.0 && (double) scaley == 1.0 / 16.0)
          return this.PixScaleAreaMap2(this.PixScaleAreaMap2(this.PixScaleAreaMap2(this.PixScaleAreaMap2(pix))));
        Pix pixs;
        if ((depth == 2 || depth == 4 || depth == 8) && pix.Colormap != null)
        {
          pixs = this.PixRemoveColormap(pix, this.m_removeCmapBasedOnSrc);
          depth = pixs.D;
        }
        else if (depth == 2 || depth == 4)
        {
          pixs = this.PixConvertTo8(pix, 0);
          depth = 8;
        }
        else
          pixs = pix;
        int w = pixs.W;
        int h = pixs.H;
        uint[] data1 = pixs.Data;
        int wpl1 = pixs.Wpl;
        int num1 = (int) ((double) scalex * (double) w + 0.5);
        int num2 = (int) ((double) scaley * (double) h + 0.5);
        if (num1 >= 1)
          ;
        Pix pix1 = this.PixCreate(num1, num2, depth);
        this.PixCopyResolution(pix1, pixs);
        this.PixScaleResolution(pix1, scalex, scaley);
        uint[] data2 = pix1.Data;
        int wpl2 = pix1.Wpl;
        if (depth == 8)
          this.ScaleGrayAreaMapLow(ref data2, num1, num2, wpl2, data1, w, h, wpl1);
        else
          this.ScaleColorAreaMapLow(ref data2, num1, num2, wpl2, data1, w, h, wpl1);
        pix1.Data = data2;
        return pix1;
      default:
        goto case 2;
    }
  }

  private void ScaleColorAreaMapLow(
    ref uint[] datad,
    int wd,
    int hd,
    int wpld,
    uint[] datas,
    int ws,
    int hs,
    int wpls)
  {
    float num1 = 16f * (float) ws / (float) wd;
    float num2 = 16f * (float) hs / (float) hd;
    int num3 = ws - 2;
    int num4 = hs - 2;
    for (int index1 = 0; index1 < hd; ++index1)
    {
      uint num5 = (uint) ((double) num2 * (double) index1);
      uint num6 = (uint) ((double) num2 * ((double) index1 + 1.0));
      uint num7 = num5 >> 4;
      uint num8 = num5 & 15U;
      uint num9 = num6 >> 4;
      uint num10 = num6 & 15U;
      uint num11 = num9 - num7;
      int num12 = index1 * wpld;
      int num13 = (int) ((long) num7 * (long) wpls);
      for (int index2 = 0; index2 < wd; ++index2)
      {
        uint num14 = (uint) ((double) num1 * (double) index2);
        uint num15 = (uint) ((double) num1 * ((double) index2 + 1.0));
        uint num16 = num14 >> 4;
        uint num17 = num14 & 15U;
        uint num18 = num15 >> 4;
        uint num19 = num15 & 15U;
        uint num20 = num18 - num16;
        if ((long) num18 > (long) num3 || (long) num9 > (long) num4)
        {
          datad[num12 + index2] = datas[(long) num13 + (long) num16];
        }
        else
        {
          uint num21 = (uint) ((16 /*0x10*/ - (int) num17 + 16 /*0x10*/ * ((int) num20 - 1) + (int) num19) * (16 /*0x10*/ - (int) num8 + 16 /*0x10*/ * ((int) num11 - 1) + (int) num10));
          uint data1 = datas[(long) num13 + (long) num16];
          uint data2 = datas[(long) num13 + (long) num18];
          uint data3 = datas[(long) num13 + (long) num11 * (long) wpls + (long) num16];
          uint data4 = datas[(long) num13 + (long) num11 * (long) wpls + (long) num18];
          uint num22 = (uint) ((16 /*0x10*/ - (int) num17) * (16 /*0x10*/ - (int) num8));
          uint num23 = num19 * (16U /*0x10*/ - num8);
          uint num24 = (16U /*0x10*/ - num17) * num10;
          uint num25 = num19 * num10;
          uint num26 = num22 * (data1 >> this.m_redShift & (uint) byte.MaxValue);
          uint num27 = num22 * (data1 >> this.m_greenShift & (uint) byte.MaxValue);
          uint num28 = num22 * (data1 >> this.m_blueShift & (uint) byte.MaxValue);
          uint num29 = num23 * (data2 >> this.m_redShift & (uint) byte.MaxValue);
          uint num30 = num23 * (data2 >> this.m_greenShift & (uint) byte.MaxValue);
          uint num31 = num23 * (data2 >> this.m_blueShift & (uint) byte.MaxValue);
          uint num32 = num24 * (data3 >> this.m_redShift & (uint) byte.MaxValue);
          uint num33 = num24 * (data3 >> this.m_greenShift & (uint) byte.MaxValue);
          uint num34 = num24 * (data3 >> this.m_blueShift & (uint) byte.MaxValue);
          uint num35 = num25 * (data4 >> this.m_redShift & (uint) byte.MaxValue);
          uint num36 = num25 * (data4 >> this.m_greenShift & (uint) byte.MaxValue);
          uint num37 = num25 * (data4 >> this.m_blueShift & (uint) byte.MaxValue);
          int num38;
          uint num39 = (uint) (num38 = 0);
          uint num40 = (uint) num38;
          uint num41 = (uint) num38;
          for (int index3 = 1; (long) index3 < (long) num11; ++index3)
          {
            for (int index4 = 1; (long) index4 < (long) num20; ++index4)
            {
              uint data5 = datas[(long) (num13 + index3 * wpls) + (long) num16 + (long) index4];
              num41 += (uint) (256 /*0x0100*/ * ((int) (data5 >> this.m_redShift) & (int) byte.MaxValue));
              num40 += (uint) (256 /*0x0100*/ * ((int) (data5 >> this.m_greenShift) & (int) byte.MaxValue));
              num39 += (uint) (256 /*0x0100*/ * ((int) (data5 >> this.m_blueShift) & (int) byte.MaxValue));
            }
          }
          int num42;
          uint num43 = (uint) (num42 = 0);
          uint num44 = (uint) num42;
          uint num45 = (uint) num42;
          uint num46 = (uint) ((16 /*0x10*/ - (int) num17) * 16 /*0x10*/);
          uint num47 = num19 * 16U /*0x10*/;
          uint num48 = (uint) (16 /*0x10*/ * (16 /*0x10*/ - (int) num8));
          uint num49 = 16U /*0x10*/ * num10;
          for (int index5 = 1; (long) index5 < (long) num11; ++index5)
          {
            uint data6 = datas[(long) (num13 + index5 * wpls) + (long) num16];
            num45 += num46 * (data6 >> this.m_redShift & (uint) byte.MaxValue);
            num44 += num46 * (data6 >> this.m_greenShift & (uint) byte.MaxValue);
            num43 += num46 * (data6 >> this.m_blueShift & (uint) byte.MaxValue);
          }
          for (int index6 = 1; (long) index6 < (long) num11; ++index6)
          {
            uint data7 = datas[(long) (num13 + index6 * wpls) + (long) num18];
            num45 += num47 * (data7 >> this.m_redShift & (uint) byte.MaxValue);
            num44 += num47 * (data7 >> this.m_greenShift & (uint) byte.MaxValue);
            num43 += num47 * (data7 >> this.m_blueShift & (uint) byte.MaxValue);
          }
          for (int index7 = 1; (long) index7 < (long) num20; ++index7)
          {
            uint data8 = datas[(long) num13 + (long) num16 + (long) index7];
            num45 += num48 * (data8 >> this.m_redShift & (uint) byte.MaxValue);
            num44 += num48 * (data8 >> this.m_greenShift & (uint) byte.MaxValue);
            num43 += num48 * (data8 >> this.m_blueShift & (uint) byte.MaxValue);
          }
          for (int index8 = 1; (long) index8 < (long) num20; ++index8)
          {
            uint data9 = datas[(long) num13 + (long) num11 * (long) wpls + (long) num16 + (long) index8];
            num45 += num49 * (data9 >> this.m_redShift & (uint) byte.MaxValue);
            num44 += num49 * (data9 >> this.m_greenShift & (uint) byte.MaxValue);
            num43 += num49 * (data9 >> this.m_blueShift & (uint) byte.MaxValue);
          }
          this.ComposeRGBPixel((int) ((uint) ((int) num26 + (int) num32 + (int) num29 + (int) num35 + (int) num41 + (int) num45 + 128 /*0x80*/) / num21), (int) ((uint) ((int) num27 + (int) num33 + (int) num30 + (int) num36 + (int) num40 + (int) num44 + 128 /*0x80*/) / num21), (int) ((uint) ((int) num28 + (int) num34 + (int) num31 + (int) num37 + (int) num39 + (int) num43 + 128 /*0x80*/) / num21), (int) datad[num12 + index2]);
        }
      }
    }
  }

  private void ScaleGrayAreaMapLow(
    ref uint[] datad,
    int wd,
    int hd,
    int wpld,
    uint[] datas,
    int ws,
    int hs,
    int wpls)
  {
    float num1 = 16f * (float) ws / (float) wd;
    float num2 = 16f * (float) hs / (float) hd;
    int num3 = ws - 2;
    int num4 = hs - 2;
    for (int index1 = 0; index1 < hd; ++index1)
    {
      int num5 = (int) ((double) num2 * (double) index1);
      int num6 = (int) ((double) num2 * ((double) index1 + 1.0));
      int num7 = num5 >> 4;
      int num8 = num5 & 15;
      int num9 = num6 >> 4;
      int num10 = num6 & 15;
      int num11 = num9 - num7;
      int num12 = index1 * wpld;
      int num13 = num7 * wpls;
      for (int index2 = 0; index2 < wd; ++index2)
      {
        int num14 = (int) ((double) num1 * (double) index2);
        int num15 = (int) ((double) num1 * ((double) index2 + 1.0));
        int num16 = num14 >> 4;
        int num17 = num14 & 15;
        int num18 = num15 >> 4;
        int num19 = num15 & 15;
        int num20 = num18 - num16;
        if (num18 > num3 || num9 > num4)
        {
          JBIG2Statics.SetDataByte(ref datad, num12 + index2, JBIG2Statics.GetDataByte(datas, num13 + num16));
        }
        else
        {
          int num21 = (16 /*0x10*/ - num17 + 16 /*0x10*/ * (num20 - 1) + num19) * (16 /*0x10*/ - num8 + 16 /*0x10*/ * (num11 - 1) + num10);
          uint num22 = (uint) ((ulong) ((16 /*0x10*/ - num17) * (16 /*0x10*/ - num8)) * (ulong) JBIG2Statics.GetDataByte(datas, num13 + num16));
          uint num23 = (uint) ((ulong) (num19 * (16 /*0x10*/ - num8)) * (ulong) JBIG2Statics.GetDataByte(datas, num13 + num18));
          uint num24 = (uint) ((ulong) ((16 /*0x10*/ - num17) * num10) * (ulong) JBIG2Statics.GetDataByte(datas, num13 + num11 * wpls + num16));
          uint num25 = (uint) ((ulong) (num19 * num10) * (ulong) JBIG2Statics.GetDataByte(datas, num13 + num11 * wpls + num18));
          uint num26 = 0;
          for (int index3 = 1; index3 < num11; ++index3)
          {
            for (int index4 = 1; index4 < num20; ++index4)
              num26 += 256U /*0x0100*/ * JBIG2Statics.GetDataByte(datas, num13 + index3 * wpls + num16 + index4);
          }
          uint num27 = 0;
          for (int index5 = 1; index5 < num11; ++index5)
            num27 += (uint) ((ulong) ((16 /*0x10*/ - num17) * 16 /*0x10*/) * (ulong) JBIG2Statics.GetDataByte(datas, num13 + index5 * wpls + num16));
          for (int index6 = 1; index6 < num11; ++index6)
            num27 += (uint) ((ulong) (num19 * 16 /*0x10*/) * (ulong) JBIG2Statics.GetDataByte(datas, num13 + index6 * wpls + num18));
          for (int index7 = 1; index7 < num20; ++index7)
            num27 += (uint) ((ulong) (16 /*0x10*/ * (16 /*0x10*/ - num8)) * (ulong) JBIG2Statics.GetDataByte(datas, num13 + num16 + index7));
          for (int index8 = 1; index8 < num20; ++index8)
            num27 += (uint) ((ulong) (16 /*0x10*/ * num10) * (ulong) JBIG2Statics.GetDataByte(datas, num13 + num11 * wpls + num16 + index8));
          uint val = (uint) ((ulong) (uint) ((int) num22 + (int) num24 + (int) num23 + (int) num25 + (int) num26 + (int) num27 + 128 /*0x80*/) / (ulong) num21);
          JBIG2Statics.SetDataByte(ref datad, num12 + index2, val);
        }
      }
    }
  }

  private Pix PixScaleAreaMap2(Pix pix)
  {
    int num1 = pix.D;
    switch (num1)
    {
      case 2:
      case 4:
      case 8:
        Pix pixs;
        if ((num1 == 2 || num1 == 4 || num1 == 8) && pix.Colormap != null)
        {
          pixs = this.PixRemoveColormap(pix, this.m_removeCmapBasedOnSrc);
          num1 = pixs.D;
        }
        else if (num1 == 2 || num1 == 4)
        {
          pixs = this.PixConvertTo8(pix, 0);
          num1 = 8;
        }
        else
          pixs = pix;
        int num2 = pixs.W / 2;
        int num3 = pixs.H / 2;
        uint[] data1 = pixs.Data;
        int wpl1 = pixs.Wpl;
        Pix pix1 = this.PixCreate(num2, num3, num1);
        uint[] data2 = pix1.Data;
        int wpl2 = pix1.Wpl;
        this.PixCopyResolution(pix1, pixs);
        this.PixScaleResolution(pix1, 0.5f, 0.5f);
        this.ScaleAreaMapLow2(data2, num2, num3, wpl2, data1, num1, wpl1);
        return pix1;
      default:
        goto case 2;
    }
  }

  private void ScaleAreaMapLow2(
    uint[] datad,
    int wd,
    int hd,
    int wpld,
    uint[] datas,
    int d,
    int wpls)
  {
    if (d == 8)
    {
      for (int index1 = 0; index1 < hd; ++index1)
      {
        int num1 = 2 * index1 * wpls;
        int num2 = index1 * wpld;
        for (int index2 = 0; index2 < wd; ++index2)
        {
          uint val = JBIG2Statics.GetDataByte(datas, num1 + 2 * index2) + JBIG2Statics.GetDataByte(datas, num1 + 2 * index2 + 1) + JBIG2Statics.GetDataByte(datas, num1 + wpls + 2 * index2) + JBIG2Statics.GetDataByte(datas, num1 + wpls + 2 * index2 + 1) >> 2;
          JBIG2Statics.SetDataByte(ref datad, num2 + index2, val);
        }
      }
    }
    else
    {
      for (int index3 = 0; index3 < hd; ++index3)
      {
        int num3 = 2 * index3 * wpls;
        int num4 = index3 * wpld;
        for (int index4 = 0; index4 < wd; ++index4)
        {
          uint data1 = datas[num3 + 2 * index4];
          uint num5 = data1 >> this.m_redShift & (uint) byte.MaxValue;
          uint num6 = data1 >> this.m_greenShift & (uint) byte.MaxValue;
          uint num7 = data1 >> this.m_blueShift & (uint) byte.MaxValue;
          uint data2 = datas[num3 + 2 * index4 + 1];
          uint num8 = num5 + (data2 >> this.m_redShift & (uint) byte.MaxValue);
          uint num9 = num6 + (data2 >> this.m_greenShift & (uint) byte.MaxValue);
          uint num10 = num7 + (data2 >> this.m_blueShift & (uint) byte.MaxValue);
          uint data3 = datas[num3 + wpls + 2 * index4];
          uint num11 = num8 + (data3 >> this.m_redShift & (uint) byte.MaxValue);
          uint num12 = num9 + (data3 >> this.m_greenShift & (uint) byte.MaxValue);
          uint num13 = num10 + (data3 >> this.m_blueShift & (uint) byte.MaxValue);
          uint data4 = datas[num3 + wpls + 2 * index4 + 1];
          this.ComposeRGBPixel((int) (num11 + (data4 >> this.m_redShift & (uint) byte.MaxValue)) >> 2, (int) (num12 + (data4 >> this.m_greenShift & (uint) byte.MaxValue)) >> 2, (int) (num13 + (data4 >> this.m_blueShift & (uint) byte.MaxValue)) >> 2, (int) data4);
          datad[num4 + index4] = data4;
        }
      }
    }
  }

  private Pix PixConvertTo8Or32(Pix pixs, int copyflag, int warnflag)
  {
    int d = pixs.D;
    Pix pix = pixs.Colormap == null ? (d == 8 || d == 32 /*0x20*/ ? (copyflag != 0 ? this.PixCopy((Pix) null, pixs) : pixs) : this.PixConvertTo8(pixs, 0)) : this.PixRemoveColormap(pixs, this.m_removeCmapBasedOnSrc);
    if (pix.D != 8)
      ;
    return pix;
  }

  private Pix PixConvertTo8(Pix pixs, int cmapflag)
  {
    int d = pixs.D;
    switch (d)
    {
      case 1:
      case 2:
      case 4:
      case 8:
      case 16 /*0x10*/:
        if (d == 1)
        {
          if (cmapflag == 0)
            return this.PixConvert1To8((Pix) null, pixs, byte.MaxValue, (byte) 0);
          Pix pix = this.PixConvert1To8((Pix) null, pixs, (byte) 0, (byte) 1);
          PixColormap pixCmap = this.CreatePixCmap(8);
          this.PixCmapAddColor(ref pixCmap, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          this.PixCmapAddColor(ref pixCmap, 0, 0, 0);
          pix.Colormap = pixCmap;
          return pix;
        }
        if (d == 2)
          return this.PixConvert2To8(pixs, (byte) 0, (byte) 85, (byte) 170, byte.MaxValue, cmapflag);
        if (d == 4)
          return this.PixConvert4To8(pixs, cmapflag);
        if (d == 8)
        {
          PixColormap colormap = pixs.Colormap;
          if (colormap != null && cmapflag == 1 || colormap != null)
            return this.PixCopy((Pix) null, pixs);
          if (colormap != null)
            return this.PixRemoveColormap(pixs, this.m_removeCmapToGrayScale);
          Pix pixs1 = this.PixCopy((Pix) null, pixs);
          this.PixAddGrayColormap8(pixs1);
          return pixs1;
        }
        if (d == 16 /*0x10*/)
        {
          Pix pixs2 = this.PixConvert16To8(pixs, 1);
          if (cmapflag > 0)
            this.PixAddGrayColormap8(pixs2);
          return pixs2;
        }
        Pix luminance = this.PixConvertRGBToLuminance(pixs);
        if (cmapflag > 0)
          this.PixAddGrayColormap8(luminance);
        return luminance;
      default:
        goto case 1;
    }
  }

  private Pix PixConvertRGBToLuminance(Pix pixs)
  {
    return this.PixConvertRGBToGray(pixs, 0.0f, 0.0f, 0.0f);
  }

  private Pix PixConvertRGBToGray(Pix pixs, float rwt, float gwt, float bwt)
  {
    int d = pixs.D;
    if ((double) rwt >= 0.0 && (double) gwt >= 0.0)
      ;
    if ((double) rwt == 0.0 && (double) gwt == 0.0 && (double) bwt == 0.0)
    {
      rwt = 0.3f;
      gwt = 0.5f;
      bwt = 0.2f;
    }
    float num1 = rwt + gwt + bwt;
    if (Math.Abs((double) num1 - 1.0) > 0.0001)
    {
      rwt /= num1;
      gwt /= num1;
      bwt /= num1;
    }
    int w = pixs.W;
    int h = pixs.H;
    int wpl1 = pixs.Wpl;
    Pix pixd = this.PixCreate(w, h, 8);
    this.PixCopyResolution(pixd, pixs);
    uint[] numArray1 = new uint[pixs.Data.Length];
    int wpl2 = pixd.Wpl;
    for (int index = 0; index < h * w; ++index)
    {
      uint num2 = pixs.Data[index];
      byte[] numArray2 = new byte[4]
      {
        (byte) 0,
        (byte) 0,
        (byte) 0,
        byte.MaxValue
      };
      numArray2[2] = (byte) (num2 >> 16 /*0x10*/ & (uint) byte.MaxValue);
      numArray2[1] = (byte) (num2 >> 8 & (uint) byte.MaxValue);
      numArray2[0] = (byte) (num2 & (uint) byte.MaxValue);
      int num3 = ((int) numArray2[2] << 1) + ((int) numArray2[1] << 2 + (int) numArray2[1]) + (int) numArray2[0] >> 3;
      numArray1[index] = (uint) num3;
    }
    pixd.Data = numArray1;
    return pixd;
  }

  private Pix PixConvertRGBToGrayFast(Pix pixs)
  {
    if (pixs.D != 32 /*0x20*/)
      return (Pix) null;
    int wpl1 = pixs.Wpl;
    int w = pixs.W;
    int h = pixs.H;
    Pix pixd = this.PixCreate(w, h, 8);
    this.PixCopyResolution(pixd, pixs);
    int wpl2 = pixd.Wpl;
    uint[] line = new uint[h * wpl2];
    for (int index1 = 0; index1 < h; ++index1)
    {
      int index2 = index1 * wpl1;
      int num1 = index1 * wpl2;
      int num2 = 0;
      while (num2 < w)
      {
        int n = num1 * 4 + num2;
        uint val = pixs.Data[index2] >> this.m_greenShift & (uint) byte.MaxValue;
        JBIG2Statics.SetDataByte(ref line, n, val);
        ++num2;
        ++index2;
      }
    }
    pixd.Data = line;
    return pixd;
  }

  private void PixAddGrayColormap8(Pix pixs)
  {
    if (pixs == null || pixs.D != 8)
    {
      Console.WriteLine("pixs not defined or not 8 bpp");
    }
    else
    {
      if (pixs.Colormap == null)
        return;
      PixColormap linear = this.PixcmapCreateLinear(8, 256 /*0x0100*/);
      pixs.Colormap = linear;
    }
  }

  private PixColormap PixcmapCreateLinear(int d, int nlevels)
  {
    if (d != 1 && d != 2 && d != 4)
      ;
    int num1 = 1 << d;
    if (nlevels >= 2)
      ;
    PixColormap pixCmap = this.CreatePixCmap(d);
    for (int index = 0; index < nlevels; ++index)
    {
      int num2 = (int) byte.MaxValue * index / (nlevels - 1);
      this.PixCmapAddColor(ref pixCmap, num2, num2, num2);
    }
    return pixCmap;
  }

  private Pix PixConvert4To8(Pix pixs, int cmapflag)
  {
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    int w1 = pixs.W;
    PixColormap colormap = pixs.Colormap;
    if (colormap == null && cmapflag == 0)
      return this.PixRemoveColormap(pixs, this.m_removeCmapToGrayScale);
    int w2 = pixs.W;
    int h = pixs.H;
    Pix pixd = this.PixCreate(w2, h, 8);
    this.PixCopyResolution(pixd, pixs);
    uint[] data1 = pixs.Data;
    int wpl1 = pixs.Wpl;
    uint[] data2 = pixd.Data;
    int wpl2 = pixd.Wpl;
    if (cmapflag > 0)
    {
      PixColormap pixCmap = this.CreatePixCmap(8);
      if (colormap != null)
      {
        int n = colormap.N;
        for (int index = 0; index < n; ++index)
        {
          this.PixCmapGetColor(colormap, index, num1, num2, num3);
          this.PixCmapAddColor(ref pixCmap, num1, num2, num3);
        }
      }
      else
      {
        for (int index = 0; index < 16 /*0x10*/; ++index)
          this.PixCmapAddColor(ref pixCmap, 17 * index, 17 * index, 17 * index);
      }
      pixd.Colormap = pixCmap;
      for (int index1 = 0; index1 < h; ++index1)
      {
        int index2 = index1 * wpl1;
        int num4 = index1 * wpl2;
        for (int n = 0; n < w2; ++n)
        {
          uint dataQbit = JBIG2Statics.GetDataQbit(data1[index2], n);
          JBIG2Statics.SetDataByte(ref data2, num4 + n, dataQbit);
        }
      }
      return pixd;
    }
    for (int index3 = 0; index3 < h; ++index3)
    {
      int index4 = index3 * wpl1;
      int num5 = index3 * wpl2;
      for (int n = 0; n < w2; ++n)
      {
        uint dataQbit = JBIG2Statics.GetDataQbit(data1[index4], n);
        uint val = dataQbit << 4 | dataQbit;
        JBIG2Statics.SetDataByte(ref data2, num5 + n, val);
      }
    }
    pixd.Data = data2;
    return pixd;
  }

  private Pix PixConvert2To8(Pix pixs, byte val0, byte val1, byte val2, byte val3, int cmapflag)
  {
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    byte[] numArray1 = new byte[4];
    uint[] numArray2 = new uint[256 /*0x0100*/];
    int d = pixs.D;
    PixColormap colormap = pixs.Colormap;
    if (colormap == null && cmapflag == 0)
      return this.PixRemoveColormap(pixs, this.m_removeCmapToGrayScale);
    int w = pixs.W;
    int h = pixs.H;
    Pix pixd = this.PixCreate(w, h, 8);
    this.PixCopyResolution(pixd, pixs);
    uint[] data1 = pixs.Data;
    int wpl1 = pixs.Wpl;
    uint[] data2 = pixd.Data;
    int wpl2 = pixd.Wpl;
    if (cmapflag > 0)
    {
      PixColormap pixCmap = this.CreatePixCmap(8);
      if (colormap != null)
      {
        int n = colormap.N;
        for (int index = 0; index < n; ++index)
        {
          this.PixCmapGetColor(colormap, index, num1, num2, num3);
          this.PixCmapAddColor(ref pixCmap, num1, num2, num3);
        }
      }
      else
      {
        this.PixCmapAddColor(ref pixCmap, (int) val0, (int) val0, (int) val0);
        this.PixCmapAddColor(ref pixCmap, (int) val1, (int) val1, (int) val1);
        this.PixCmapAddColor(ref pixCmap, (int) val2, (int) val2, (int) val2);
        this.PixCmapAddColor(ref pixCmap, (int) val3, (int) val3, (int) val3);
      }
      pixd.Colormap = pixCmap;
      for (int index1 = 0; index1 < h; ++index1)
      {
        int index2 = index1 * wpl1;
        int num4 = index1 * wpl2;
        for (int n = 0; n < w; ++n)
        {
          uint dataDibit = JBIG2Statics.GetDataDibit(data1[index2], n);
          JBIG2Statics.SetDataByte(ref data2, num4 + n, dataDibit);
        }
      }
      return pixd;
    }
    numArray1[0] = val0;
    numArray1[1] = val1;
    numArray1[2] = val2;
    numArray1[3] = val3;
    for (uint index = 0; index < 256U /*0x0100*/; ++index)
      numArray2[(IntPtr) index] = (uint) ((int) numArray1[(IntPtr) (index >> 6 & 3U)] << 24 | (int) numArray1[(IntPtr) (index >> 4 & 3U)] << 16 /*0x10*/ | (int) numArray1[(IntPtr) (index >> 2 & 3U)] << 8) | (uint) numArray1[(IntPtr) (index & 3U)];
    int num5 = (w + 3) / 4;
    for (int index3 = 0; index3 < h; ++index3)
    {
      int num6 = index3 * wpl1;
      int num7 = index3 * wpl2;
      for (int index4 = 0; index4 < num5; ++index4)
      {
        uint dataByte = JBIG2Statics.GetDataByte(data1, num6 + index4);
        data2[num7 + index4] = numArray2[(IntPtr) dataByte];
      }
    }
    pixd.Data = data2;
    return pixd;
  }

  private void PixCmapAddColor(ref PixColormap cmap, int rval, int gval, int bval)
  {
    int n = cmap.N;
    int nalloc = cmap.Nalloc;
    if (cmap.Array[cmap.N] == null)
      cmap.Array[cmap.N] = new RGBA_Quad();
    cmap.Array[cmap.N].Red = rval;
    cmap.Array[cmap.N].Green = gval;
    cmap.Array[cmap.N].Blue = bval;
    ++cmap.N;
  }

  private PixColormap CreatePixCmap(int depth)
  {
    PixColormap pixCmap = new PixColormap();
    if (depth != 1 && depth != 2 && depth != 4)
      ;
    pixCmap.Depth = depth;
    pixCmap.Nalloc = 1 << depth;
    pixCmap.N = 0;
    return pixCmap;
  }

  private Pix PixConvert1To8(Pix pixd, Pix pixs, byte val0, byte val1)
  {
    byte[] numArray1 = new byte[2];
    uint[] numArray2 = new uint[16 /*0x10*/];
    int d = pixs.D;
    int w = pixs.W;
    int h1 = pixs.H;
    if (pixd != null)
    {
      if (w == pixd.W)
      {
        int h2 = pixd.H;
      }
      if (pixd.D == 8)
        ;
    }
    else
      pixd = this.PixCreate(w, h1, 8);
    this.PixCopyResolution(pixd, pixs);
    numArray1[0] = val0;
    numArray1[1] = val1;
    for (uint index = 0; index < 16U /*0x10*/; ++index)
      numArray2[(IntPtr) index] = (uint) ((int) numArray1[(IntPtr) (index >> 3 & 1U)] << 24 | (int) numArray1[(IntPtr) (index >> 2 & 1U)] << 16 /*0x10*/ | (int) numArray1[(IntPtr) (index >> 1 & 1U)] << 8) | (uint) numArray1[(IntPtr) (index & 1U)];
    uint[] data1 = pixs.Data;
    int wpl1 = pixs.Wpl;
    uint[] data2 = pixd.Data;
    int wpl2 = pixd.Wpl;
    int num1 = (w + 3) / 4;
    for (int index1 = 0; index1 < h1; ++index1)
    {
      int index2 = index1 * wpl1;
      int num2 = index1 * wpl2;
      for (int n = 0; n < num1; ++n)
      {
        uint dataQbit = JBIG2Statics.GetDataQbit(data1[index2], n);
        data2[num2 + n] = numArray2[(IntPtr) dataQbit];
      }
    }
    pixd.Data = data2;
    return pixd;
  }

  private Pix PixScaleBinary(Pix pixs, float scalex, float scaley)
  {
    int d = pixs.D;
    if ((double) scalex == 1.0 && (double) scaley == 1.0)
      return this.PixCopy((Pix) null, pixs);
    int w = pixs.W;
    int h = pixs.H;
    uint[] data1 = pixs.Data;
    int wpl1 = pixs.Wpl;
    int num1 = (int) ((double) scalex * (double) w + 0.5);
    int num2 = (int) ((double) scaley * (double) h + 0.5);
    Pix pix = this.PixCreate(num1, num2, 1);
    this.PixCopyColormap(pix, pixs);
    this.PixCopyResolution(pix, pixs);
    this.PixScaleResolution(pix, scalex, scaley);
    uint[] data2 = pix.Data;
    int wpl2 = pix.Wpl;
    this.ScaleBinaryLow(ref data2, num1, num2, wpl2, data1, w, h, wpl1);
    return pix;
  }

  private void ScaleBinaryLow(
    ref uint[] datad,
    int wd,
    int hd,
    int wpld,
    uint[] datas,
    int ws,
    int hs,
    int wpls)
  {
    int length = 4 * wpld;
    datad = new uint[hd * length];
    int[] numArray1 = new int[hd];
    int[] numArray2 = new int[wd];
    float num1 = (float) ws / (float) wd;
    float num2 = (float) hs / (float) hd;
    for (int index = 0; index < hd; ++index)
      numArray1[index] = Math.Min((int) ((double) num2 * (double) index + 0.5), hs - 1);
    for (int index = 0; index < wd; ++index)
      numArray2[index] = Math.Min((int) ((double) num1 * (double) index + 0.5), ws - 1);
    int num3 = 0;
    int num4 = -1;
    uint num5 = 0;
    for (int index1 = 0; index1 < hd; ++index1)
    {
      int index2 = numArray1[index1] * wpls;
      int destinationIndex = index1 * wpld;
      if ((long) datas[index2] != (long) num3)
      {
        for (int n1 = 0; n1 < wd; ++n1)
        {
          int n2 = numArray2[n1];
          if (n2 != num4)
          {
            if ((num5 = JBIG2Statics.GetDataBit(datas, index2, n2)) != 0U)
              JBIG2Statics.SetDataBit(ref datad[destinationIndex], n1);
            num4 = n2;
          }
          else if (num5 > 0U)
            JBIG2Statics.SetDataBit(ref datad[destinationIndex], n1);
        }
      }
      else
      {
        int sourceIndex = destinationIndex - wpld;
        Array.Copy((Array) datad, sourceIndex, (Array) datad, destinationIndex, length);
      }
      num3 = index2;
    }
  }

  private Pix PixScaleToGray2(Pix pixs)
  {
    int w = pixs.W;
    int h = pixs.H;
    int num1 = w / 2;
    int num2 = h / 2;
    Pix gray2 = this.PixCreate(num1, num2, 8);
    this.PixCopyResolution(gray2, pixs);
    this.PixScaleResolution(gray2, 0.5f, 0.5f);
    uint[] data1 = pixs.Data;
    uint[] data2 = gray2.Data;
    int wpl1 = pixs.Wpl;
    int wpl2 = gray2.Wpl;
    uint[] sumtab = this.MakeSumTabSG2();
    byte[] valtab = this.MakeValTabSG2();
    this.ScaleToGray2Low(ref data2, num1, num2, wpl2, data1, wpl1, sumtab, valtab);
    return gray2;
  }

  private void ScaleToGray2Low(
    ref uint[] datad,
    int wd,
    int hd,
    int wpld,
    uint[] datas,
    int wpls,
    uint[] sumtab,
    byte[] valtab)
  {
    int num1 = (int) ((long) wd & 4294967292L);
    int num2 = wd - num1;
    int num3 = 0;
    int num4 = 0;
    while (num3 < hd)
    {
      int num5 = num4 * wpls;
      int num6 = num3 * wpld;
      int num7 = 0;
      int num8 = 0;
      while (num7 < num1)
      {
        uint dataByte1 = JBIG2Statics.GetDataByte(datas, num5 + num8);
        uint dataByte2 = JBIG2Statics.GetDataByte(datas, num5 + wpls + num8);
        uint num9 = sumtab[(IntPtr) dataByte1] + sumtab[(IntPtr) dataByte2];
        JBIG2Statics.SetDataByte(ref datad, num6 + num7, (uint) valtab[(IntPtr) (num9 >> 24)]);
        JBIG2Statics.SetDataByte(ref datad, num6 + num7 + 1, (uint) valtab[(IntPtr) (num9 >> 16 /*0x10*/ & (uint) byte.MaxValue)]);
        JBIG2Statics.SetDataByte(ref datad, num6 + num7 + 2, (uint) valtab[(IntPtr) (num9 >> 8 & (uint) byte.MaxValue)]);
        JBIG2Statics.SetDataByte(ref datad, num6 + num7 + 3, (uint) valtab[(IntPtr) (num9 & (uint) byte.MaxValue)]);
        num7 += 4;
        ++num8;
      }
      if (num2 > 0)
      {
        uint dataByte3 = JBIG2Statics.GetDataByte(datas, num5 + num8);
        uint dataByte4 = JBIG2Statics.GetDataByte(datas, num5 + wpls + num8);
        uint num10 = sumtab[(IntPtr) dataByte3] + sumtab[(IntPtr) dataByte4];
        for (int index = 0; index < num2; ++index)
          JBIG2Statics.SetDataByte(ref datad, num6 + num7 + index, (uint) valtab[(IntPtr) (num10 >> 24 - 8 * index & (uint) byte.MaxValue)]);
      }
      ++num3;
      num4 += 2;
    }
  }

  private byte[] MakeValTabSG2()
  {
    byte[] numArray = new byte[5];
    for (int index = 0; index < 5; ++index)
      numArray[index] = (byte) ((int) byte.MaxValue - index * (int) byte.MaxValue / 4);
    return numArray;
  }

  private uint[] MakeSumTabSG2()
  {
    int[] numArray1 = new int[4]{ 0, 1, 1, 2 };
    uint[] numArray2 = new uint[256 /*0x0100*/];
    for (int index = 0; index < 256 /*0x0100*/; ++index)
      numArray2[index] = (uint) (numArray1[index & 3] | numArray1[index >> 2 & 3] << 8 | numArray1[index >> 4 & 3] << 16 /*0x10*/ | numArray1[index >> 6 & 3] << 24);
    return numArray2;
  }

  private Pix PixScaleToGray3(Pix pixs)
  {
    int w = pixs.W;
    int h = pixs.H;
    int num1 = (int) ((long) (w / 3) & 4294967288L);
    int num2 = h / 3;
    Pix gray3 = this.PixCreate(num1, num2, 8);
    this.PixCopyResolution(gray3, pixs);
    this.PixScaleResolution(gray3, 0.33333f, 0.33333f);
    uint[] data1 = pixs.Data;
    uint[] data2 = gray3.Data;
    int wpl1 = pixs.Wpl;
    int wpl2 = gray3.Wpl;
    uint[] sumtab = this.MakeSumTabSG3();
    byte[] valtab = this.MakeValTabSG3();
    this.ScaleToGray3Low(ref data2, num1, num2, wpl2, data1, wpl1, sumtab, valtab);
    return gray3;
  }

  private void ScaleToGray3Low(
    ref uint[] datad,
    int wd,
    int hd,
    int wpld,
    uint[] datas,
    int wpls,
    uint[] sumtab,
    byte[] valtab)
  {
    int num1 = 0;
    int num2 = 0;
    while (num1 < hd)
    {
      int num3 = num2 * wpls;
      int num4 = num1 * wpld;
      int num5 = 0;
      int num6 = 0;
      while (num5 < wd)
      {
        uint num7 = (uint) ((int) JBIG2Statics.GetDataByte(datas, num3 + num6) << 16 /*0x10*/ | (int) JBIG2Statics.GetDataByte(datas, num3 + num6 + 1) << 8) | JBIG2Statics.GetDataByte(datas, num3 + num6 + 2);
        uint num8 = (uint) ((int) JBIG2Statics.GetDataByte(datas, num3 + wpls + num6) << 16 /*0x10*/ | (int) JBIG2Statics.GetDataByte(datas, num3 + wpls + num6 + 1) << 8) | JBIG2Statics.GetDataByte(datas, num3 + wpls + num6 + 2);
        uint num9 = (uint) ((int) JBIG2Statics.GetDataByte(datas, num3 + 2 * wpls + num6) << 16 /*0x10*/ | (int) JBIG2Statics.GetDataByte(datas, num3 + 2 * wpls + num6 + 1) << 8) | JBIG2Statics.GetDataByte(datas, num3 + 2 * wpls + num6 + 2);
        uint line1 = sumtab[(IntPtr) (num7 >> 18)] + sumtab[(IntPtr) (num8 >> 18)] + sumtab[(IntPtr) (num9 >> 18)];
        JBIG2Statics.SetDataByte(ref datad, num4 + num5, (uint) valtab[(IntPtr) JBIG2Statics.GetDataByte(line1, 2)]);
        JBIG2Statics.SetDataByte(ref datad, num4 + num5 + 1, (uint) valtab[(IntPtr) JBIG2Statics.GetDataByte(line1, 3)]);
        uint line2 = sumtab[(IntPtr) (num7 >> 12 & 63U /*0x3F*/)] + sumtab[(IntPtr) (num8 >> 12 & 63U /*0x3F*/)] + sumtab[(IntPtr) (num9 >> 12 & 63U /*0x3F*/)];
        JBIG2Statics.SetDataByte(ref datad, num4 + num5 + 2, (uint) valtab[(IntPtr) JBIG2Statics.GetDataByte(line2, 2)]);
        JBIG2Statics.SetDataByte(ref datad, num4 + num5 + 3, (uint) valtab[(IntPtr) JBIG2Statics.GetDataByte(line2, 3)]);
        uint line3 = sumtab[(IntPtr) (num7 >> 6 & 63U /*0x3F*/)] + sumtab[(IntPtr) (num8 >> 6 & 63U /*0x3F*/)] + sumtab[(IntPtr) (num9 >> 6 & 63U /*0x3F*/)];
        JBIG2Statics.SetDataByte(ref datad, num4 + num5 + 4, (uint) valtab[(IntPtr) JBIG2Statics.GetDataByte(line3, 2)]);
        JBIG2Statics.SetDataByte(ref datad, num4 + num5 + 5, (uint) valtab[(IntPtr) JBIG2Statics.GetDataByte(line3, 3)]);
        uint line4 = sumtab[(IntPtr) (num7 & 63U /*0x3F*/)] + sumtab[(IntPtr) (num8 & 63U /*0x3F*/)] + sumtab[(IntPtr) (num9 & 63U /*0x3F*/)];
        JBIG2Statics.SetDataByte(ref datad, num4 + num5 + 6, (uint) valtab[(IntPtr) JBIG2Statics.GetDataByte(line4, 2)]);
        JBIG2Statics.SetDataByte(ref datad, num4 + num5 + 7, (uint) valtab[(IntPtr) JBIG2Statics.GetDataByte(line4, 3)]);
        num5 += 8;
        num6 += 3;
      }
      ++num1;
      num2 += 3;
    }
  }

  private byte[] MakeValTabSG3()
  {
    byte[] numArray = new byte[10];
    for (int index = 0; index < 10; ++index)
      numArray[index] = (byte) ((int) byte.MaxValue - index * (int) byte.MaxValue / 9);
    return numArray;
  }

  private uint[] MakeSumTabSG3()
  {
    uint[] numArray1 = new uint[8]
    {
      0U,
      1U,
      1U,
      2U,
      1U,
      2U,
      2U,
      3U
    };
    uint[] numArray2 = new uint[64 /*0x40*/];
    for (int index = 0; index < 64 /*0x40*/; ++index)
      numArray2[index] = numArray1[index & 7] | numArray1[index >> 3 & 7] << 8;
    return numArray2;
  }

  private Pix PixScaleToGray4(Pix pixs)
  {
    int w = pixs.W;
    int h = pixs.H;
    int num1 = (int) ((long) (w / 4) & 4294967294L);
    int num2 = h / 4;
    Pix gray4 = this.PixCreate(num1, num2, 8);
    this.PixCopyResolution(gray4, pixs);
    this.PixScaleResolution(gray4, 0.25f, 0.25f);
    uint[] data1 = pixs.Data;
    uint[] data2 = gray4.Data;
    int wpl1 = pixs.Wpl;
    int wpl2 = gray4.Wpl;
    uint[] sumtab = this.MakeSumTabSG4();
    byte[] valtab = this.MakeValTabSG4();
    this.ScaleToGray4Low(ref data2, num1, num2, wpl2, data1, wpl1, sumtab, valtab);
    return gray4;
  }

  private void ScaleToGray4Low(
    ref uint[] datad,
    int wd,
    int hd,
    int wpld,
    uint[] datas,
    int wpls,
    uint[] sumtab,
    byte[] valtab)
  {
    int num1 = 0;
    int num2 = 0;
    while (num1 < hd)
    {
      int num3 = num2 * wpls;
      int num4 = num1 * wpld;
      int num5 = 0;
      int num6 = 0;
      while (num5 < wd)
      {
        uint dataByte1 = JBIG2Statics.GetDataByte(datas, num3 + num6);
        uint dataByte2 = JBIG2Statics.GetDataByte(datas, num3 + wpls + num6);
        uint dataByte3 = JBIG2Statics.GetDataByte(datas, num3 + 2 * wpls + num6);
        uint dataByte4 = JBIG2Statics.GetDataByte(datas, num3 + 3 * wpls + num6);
        uint line = sumtab[(IntPtr) dataByte1] + sumtab[(IntPtr) dataByte2] + sumtab[(IntPtr) dataByte3] + sumtab[(IntPtr) dataByte4];
        JBIG2Statics.SetDataByte(ref datad, num4 + num5, (uint) valtab[(IntPtr) JBIG2Statics.GetDataByte(line, 2)]);
        JBIG2Statics.SetDataByte(ref datad, num4 + num5 + 1, (uint) valtab[(IntPtr) JBIG2Statics.GetDataByte(line, 3)]);
        num5 += 2;
        ++num6;
      }
      ++num1;
      num2 += 4;
    }
  }

  private byte[] MakeValTabSG4()
  {
    byte[] numArray = new byte[17];
    for (int index = 0; index < 17; ++index)
      numArray[index] = (byte) ((int) byte.MaxValue - index * (int) byte.MaxValue / 16 /*0x10*/);
    return numArray;
  }

  private uint[] MakeSumTabSG4()
  {
    uint[] numArray1 = new uint[16 /*0x10*/]
    {
      0U,
      1U,
      1U,
      2U,
      1U,
      2U,
      2U,
      3U,
      1U,
      2U,
      2U,
      3U,
      2U,
      3U,
      3U,
      4U
    };
    uint[] numArray2 = new uint[256 /*0x0100*/];
    for (int index = 0; index < 256 /*0x0100*/; ++index)
      numArray2[index] = numArray1[index & 15] | numArray1[index >> 4 & 15] << 8;
    return numArray2;
  }

  private Pix PixScaleToGray8(Pix pixs)
  {
    int d = pixs.D;
    int w = pixs.W;
    int h = pixs.H;
    int num1 = w / 8;
    int num2 = h / 8;
    if (num1 != 0)
      ;
    Pix gray8 = this.PixCreate(num1, num2, 8);
    this.PixCopyResolution(gray8, pixs);
    this.PixScaleResolution(gray8, 0.125f, 0.125f);
    uint[] data1 = pixs.Data;
    uint[] data2 = gray8.Data;
    int wpl1 = pixs.Wpl;
    int wpl2 = gray8.Wpl;
    int[] tab8 = this.MakePixelSumTab8();
    short[] valtab = this.MakeValTabSG8();
    this.ScaleToGray8Low(ref data2, num1, num2, wpl2, data1, wpl1, tab8, valtab);
    int[] numArray1 = new int[1];
    short[] numArray2 = new short[0];
    return gray8;
  }

  private void ScaleToGray8Low(
    ref uint[] datad,
    int wd,
    int hd,
    int wpld,
    uint[] datas,
    int wpls,
    int[] tab8,
    short[] valtab)
  {
    int num1 = 0;
    int num2 = 0;
    while (num1 < hd)
    {
      int num3 = num2 * wpls;
      int num4 = num1 * wpld;
      for (int index1 = 0; index1 < wd; ++index1)
      {
        uint dataByte1 = JBIG2Statics.GetDataByte(datas, num3 + index1);
        uint dataByte2 = JBIG2Statics.GetDataByte(datas, num3 + wpls + index1);
        uint dataByte3 = JBIG2Statics.GetDataByte(datas, num3 + 2 * wpls + index1);
        uint dataByte4 = JBIG2Statics.GetDataByte(datas, num3 + 3 * wpls + index1);
        uint dataByte5 = JBIG2Statics.GetDataByte(datas, num3 + 4 * wpls + index1);
        uint dataByte6 = JBIG2Statics.GetDataByte(datas, num3 + 5 * wpls + index1);
        uint dataByte7 = JBIG2Statics.GetDataByte(datas, num3 + 6 * wpls + index1);
        uint dataByte8 = JBIG2Statics.GetDataByte(datas, num3 + 7 * wpls + index1);
        uint index2 = (uint) (tab8[(IntPtr) dataByte1] + tab8[(IntPtr) dataByte2] + tab8[(IntPtr) dataByte3] + tab8[(IntPtr) dataByte4] + tab8[(IntPtr) dataByte5] + tab8[(IntPtr) dataByte6] + tab8[(IntPtr) dataByte7] + tab8[(IntPtr) dataByte8]);
        JBIG2Statics.SetDataByte(ref datad, num4 + index1, (uint) valtab[(IntPtr) index2]);
      }
      ++num1;
      num2 += 8;
    }
  }

  private short[] MakeValTabSG8()
  {
    short[] numArray = new short[65];
    for (int index = 0; index < 65; ++index)
      numArray[index] = (short) ((int) byte.MaxValue - index * (int) byte.MaxValue / 64 /*0x40*/);
    return numArray;
  }

  private int[] MakePixelSumTab8()
  {
    int[] numArray = new int[256 /*0x0100*/];
    for (int index = 0; index < 256 /*0x0100*/; ++index)
    {
      short num = (short) index;
      numArray[index] = ((int) num & 1) + ((int) num >> 1 & 1) + ((int) num >> 2 & 1) + ((int) num >> 3 & 1) + ((int) num >> 4 & 1) + ((int) num >> 5 & 1) + ((int) num >> 6 & 1) + ((int) num >> 7 & 1);
    }
    return numArray;
  }

  private Pix PixConvert16To8(Pix pixs, int whichbyte)
  {
    int d = pixs.D;
    int w = pixs.W;
    int h = pixs.H;
    Pix pixd = this.PixCreate(w, h, 8);
    this.PixCopyResolution(pixd, pixs);
    int wpl1 = pixs.Wpl;
    uint[] data1 = pixs.Data;
    int wpl2 = pixd.Wpl;
    uint[] data2 = pixd.Data;
    for (int index1 = 0; index1 < h; ++index1)
    {
      int num1 = index1 * wpl1;
      int num2 = index1 * wpl2;
      if (whichbyte == 0)
      {
        for (int index2 = 0; index2 < wpl1; ++index2)
        {
          uint num3 = data1[num1 + index2];
          uint val = (uint) ((int) (num3 >> 8) & 65280 | (int) num3 & (int) byte.MaxValue);
          JBIG2Statics.SetDataTwoBytes(ref data2, num2 + index2, (short) val);
        }
      }
      else
      {
        for (int index3 = 0; index3 < wpl1; ++index3)
        {
          uint num4 = data1[num1 + index3];
          uint val = (uint) ((int) (num4 >> 16 /*0x10*/) & 65280 | (int) (num4 >> 8) & (int) byte.MaxValue);
          JBIG2Statics.SetDataTwoBytes(ref data2, num2 + index3, (short) val);
        }
      }
    }
    pixd.Data = data2;
    return pixd;
  }

  private Pix PixErodeBrick(Pix pixd, Pix pixs, int hsize, int vsize)
  {
    if (pixs.D != 1)
    {
      Console.WriteLine("pixs not 1 bpp");
      return (Pix) null;
    }
    if (hsize < 1 || vsize < 1)
    {
      Console.WriteLine("hsize and vsize not >= 1");
      return (Pix) null;
    }
    if (hsize == 1 && vsize == 1)
      return this.PixCopy(pixd, pixs);
    if (hsize == 1 || vsize == 1)
    {
      Sel brick = this.SelCreateBrick(vsize, hsize, vsize / 2, hsize / 2, 1);
      pixd = this.PixErode(pixd, pixs, brick);
    }
    else
    {
      Sel brick1 = this.SelCreateBrick(1, hsize, 0, hsize / 2, 1);
      Sel brick2 = this.SelCreateBrick(vsize, 1, vsize / 2, 0, 1);
      Pix pixs1 = this.PixErode((Pix) null, pixs, brick1);
      pixd = this.PixErode(pixd, pixs1, brick2);
    }
    return pixd;
  }

  private Pix PixErode(Pix pixd, Pix pixs, Sel sel)
  {
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    int num4 = 0;
    Pix ppixt = (Pix) null;
    pixd = this.ProcessMorphArgs1(pixd, pixs, sel, ref ppixt);
    if (pixd == null)
      return (Pix) null;
    int w = pixs.W;
    int h = pixs.H;
    int sy = sel.SY;
    int sx = sel.SX;
    int cy = sel.CY;
    int cx = sel.CX;
    this.PixSetAll(pixd);
    for (int index1 = 0; index1 < sy; ++index1)
    {
      for (int index2 = 0; index2 < sx; ++index2)
      {
        if (sel.Data[index1][index2] == 1)
          this.PixRasterop(pixd, cx - index2, cy - index1, w, h, JBIG2Statics.PixSrc & JBIG2Statics.PixDst, ppixt, 0, 0);
      }
    }
    if (this.MORPH_BC == 1)
    {
      this.SelFindMaxTranslations(sel, num1, num2, num3, num4);
      if (num1 > 0)
        this.PixRasterop(pixd, 0, 0, num1, h, JBIG2Statics.PixClr, (Pix) null, 0, 0);
      if (num3 > 0)
        this.PixRasterop(pixd, w - num3, 0, num3, h, JBIG2Statics.PixClr, (Pix) null, 0, 0);
      if (num2 > 0)
        this.PixRasterop(pixd, 0, 0, w, num2, JBIG2Statics.PixClr, (Pix) null, 0, 0);
      if (num4 > 0)
        this.PixRasterop(pixd, 0, h - num4, w, num4, JBIG2Statics.PixClr, (Pix) null, 0, 0);
    }
    return pixd;
  }

  private void SelFindMaxTranslations(Sel sel, int pxp, int pyp, int pxn, int pyn)
  {
    int num1;
    pyn = num1 = 0;
    pxn = num1;
    pyp = num1;
    pxp = num1;
    int sy = sel.SY;
    int sx = sel.SX;
    int cy = sel.CY;
    int cx = sel.CX;
    int num2;
    int val1_1 = num2 = 0;
    int val1_2 = num2;
    int val1_3 = num2;
    int val1_4 = num2;
    for (int index1 = 0; index1 < sy; ++index1)
    {
      for (int index2 = 0; index2 < sx; ++index2)
      {
        if (sel.Data[index1][index2] == 1)
        {
          val1_4 = Math.Max(val1_4, cx - index2);
          val1_3 = Math.Max(val1_3, cy - index1);
          val1_2 = Math.Max(val1_2, index2 - cx);
          val1_1 = Math.Max(val1_1, index1 - cy);
        }
      }
    }
    pxp = val1_4;
    pyp = val1_3;
    pxn = val1_2;
    pyn = val1_1;
  }

  private void PixSetAll(Pix pix)
  {
    int op = 30;
    PixColormap colormap;
    if ((colormap = pix.Colormap) != null && colormap.N < colormap.Nalloc)
      return;
    this.PixRasterop(pix, 0, 0, pix.W, pix.H, op, (Pix) null, 0, 0);
  }

  private Pix ProcessMorphArgs1(Pix pixd, Pix pixs, Sel sel, ref Pix ppixt)
  {
    ppixt = (Pix) null;
    if (pixs.D != 1)
    {
      Console.WriteLine("pixs not 1 bpp");
      return (Pix) null;
    }
    int sx = sel.SX;
    int sy = sel.SY;
    if (pixd == null)
    {
      if ((pixd = this.PixCreateTemplate(pixs)) == null)
      {
        Console.WriteLine("pixd not made");
        return (Pix) null;
      }
      ppixt = pixs;
    }
    else
    {
      this.PixResizeImageData(pixd, pixs);
      if (pixd == pixs)
      {
        if ((ppixt = this.PixCopy((Pix) null, pixs)) == null)
        {
          Console.WriteLine("pixt not made");
          return (Pix) null;
        }
      }
      else
        ppixt = pixs;
    }
    return pixd;
  }

  private int PixResizeImageData(Pix pixd, Pix pixs)
  {
    if (pixs == pixd || pixs.W == pixd.W || pixs.H == pixd.H || pixs.D == pixd.D)
      return 0;
    int w = pixs.W;
    int h = pixs.H;
    int d = pixs.D;
    int wpl = pixs.Wpl;
    pixd.W = w;
    pixd.H = h;
    pixd.D = d;
    pixd.Wpl = wpl;
    int length = 4 * wpl * h;
    pixd.Data = new uint[length];
    return 0;
  }

  private Pix PixDilateBrick(Pix pixd, Pix pixs, int hsize, int vsize)
  {
    if (hsize == 1 && vsize == 1)
      return this.PixCopy(pixd, pixs);
    if (hsize == 1 || vsize == 1)
    {
      Sel brick = this.SelCreateBrick(vsize, hsize, vsize / 2, hsize / 2, JBIG2Statics.SelHit);
      pixd = this.PixDilate(pixd, pixs, brick);
    }
    else
    {
      Sel brick1 = this.SelCreateBrick(1, hsize, 0, hsize / 2, 1);
      Sel brick2 = this.SelCreateBrick(vsize, 1, vsize / 2, 0, 1);
      Pix pixs1 = this.PixDilate((Pix) null, pixs, brick1);
      pixd = this.PixDilate(pixd, pixs1, brick2);
    }
    return pixd;
  }

  private Sel SelCreateBrick(int h, int w, int cy, int cx, int type)
  {
    if (h <= 0 || w <= 0)
      return (Sel) null;
    if (type != JBIG2Statics.SelHit && type != JBIG2Statics.SelMiss && type != JBIG2Statics.SelDontCare)
      return (Sel) null;
    Sel sel = JBIG2Statics.CreateSel(h, w, (string) null);
    sel.CY = cy;
    sel.CX = cx;
    for (int index1 = 0; index1 < h; ++index1)
    {
      for (int index2 = 0; index2 < w; ++index2)
        sel.Data[index1][index2] = type;
    }
    return sel;
  }

  private Pix PixDilate(Pix pixd, Pix pixs, Sel sel)
  {
    Pix ppixt = (Pix) null;
    if ((pixd = this.ProcessMorphArgs1(pixd, pixs, sel, ref ppixt)) == null)
    {
      Console.WriteLine("processMorphArgs1 failed");
      return (Pix) null;
    }
    int w = pixs.W;
    int h = pixs.H;
    int sy = sel.SY;
    int sx = sel.SX;
    int cy = sel.CY;
    int cx = sel.CX;
    this.PixClearAll(ref pixd);
    for (int index1 = 0; index1 < sy; ++index1)
    {
      for (int index2 = 0; index2 < sx; ++index2)
      {
        if (sel.Data[index1][index2] == 1)
          this.PixRasterop(pixd, index2 - cx, index1 - cy, w, h, JBIG2Statics.PixSrc | JBIG2Statics.PixDst, ppixt, 0, 0);
      }
    }
    return pixd;
  }

  private void PixClearAll(ref Pix pix)
  {
    this.PixRasterop(pix, 0, 0, pix.W, pix.H, JBIG2Statics.PixClr, (Pix) null, 0, 0);
  }

  private int MorphSequenceVerify(Sarray sa)
  {
    int[] numArray1 = new int[4];
    int[] numArray2 = new int[5]{ 1, 2, 3, 0, 4 };
    int n = sa.N;
    bool flag = true;
    int num1 = 0;
    int num2 = 0;
    for (int index1 = 0; index1 < n; ++index1)
    {
      char[] charArray = sa.Array[index1].Replace(" \n\t", "").ToCharArray();
      switch (charArray[0])
      {
        case 'B':
        case 'b':
          int num3 = 1;
          if (index1 > 0)
          {
            flag = false;
            break;
          }
          if (num3 < 1)
          {
            flag = false;
            break;
          }
          num2 = num3;
          break;
        case 'C':
        case 'D':
        case 'E':
        case 'O':
        case 'c':
        case 'd':
        case 'e':
        case 'o':
          int num4 = 1;
          int num5 = 6;
          if (num4 <= 0 || num5 <= 0)
          {
            flag = false;
            break;
          }
          break;
        case 'R':
        case 'r':
          int num6 = charArray.Length - 1;
          num1 += num6;
          if (num6 < 1 || num6 > 4)
          {
            flag = false;
            break;
          }
          for (int index2 = 0; index2 < num6; ++index2)
          {
            numArray1[index2] = (int) charArray[index2 + 1] - 48 /*0x30*/;
            if (numArray1[index2] < 1 || numArray1[index2] > 4)
            {
              flag = false;
              break;
            }
          }
          if (flag)
          {
            for (int index3 = 0; index3 < num6; ++index3)
              numArray1[index3] = (int) charArray[index3 + 1] - 48 /*0x30*/;
            break;
          }
          break;
        case 'X':
        case 'x':
          int num7 = 1;
          switch (num7)
          {
            case 2:
            case 4:
            case 8:
            case 16 /*0x10*/:
              num1 -= numArray2[num7 / 4];
              break;
            default:
              flag = false;
              break;
          }
          break;
        default:
          flag = false;
          break;
      }
    }
    if (num2 != 0 && num1 != 0)
      flag = false;
    return !flag ? 0 : 1;
  }

  private string StringRemoveChars(string src, char[] remchars)
  {
    char[] chArray = new char[src.Length];
    if (remchars == null)
      return src;
    int length = src.Length;
    int index = 0;
    int num = 0;
    for (; index < length; ++index)
    {
      char ch = src[index];
      chArray[num++] = ch;
    }
    return new string(chArray);
  }

  private int SarraySplitString(Sarray sa, char[] str, char separators)
  {
    char[] psaveptr;
    char[] str1 = this.StrtokSafe(new string(str), separators, out psaveptr);
    if (str1.Length != 0)
      this.SarrayAddString(sa, str1, 0);
    while (str1 == this.StrtokSafe((string) null, separators, out psaveptr))
      this.SarrayAddString(sa, str1, 0);
    return 0;
  }

  private int SarrayAddString(Sarray sa, char[] str, int copyflag)
  {
    int n = sa.N;
    if (n >= sa.Nalloc)
      sa.Nalloc *= 2;
    if (sa.Array.Count <= n)
    {
      while (sa.Array.Count < n)
        sa.Array.Add(string.Empty);
      sa.Array.Add(new string(str));
    }
    else
      sa.Array[n] = new string(str);
    ++sa.N;
    return 0;
  }

  private char[] StrtokSafe(string cstr, char seps, out char[] psaveptr)
  {
    char[] chArray1 = new char[0];
    string empty = string.Empty;
    psaveptr = new char[cstr.Length];
    char[] chArray2 = !(cstr == string.Empty) ? cstr.ToCharArray() : psaveptr;
    int index1 = 0;
    if (cstr != string.Empty)
    {
      char ch;
      for (index1 = 0; (ch = chArray2[index1]) != char.MinValue; ++index1)
      {
        if (new string(new char[1]{ seps }).IndexOf(ch) == -1)
          goto label_6;
      }
      psaveptr = (char[]) null;
      return (char[]) null;
    }
label_6:
    int index2;
    char ch1;
    for (index2 = index1; (ch1 = chArray2[index2]) != char.MinValue; ++index2)
    {
      if (new string(new char[1]{ seps }).IndexOf(ch1) > -1)
        break;
    }
    int length = index2 - index1;
    char[] charArray = (new string(new char[length + 1]) + (new string(chArray2) + (object) (char) index1).Substring(0, length)).ToCharArray();
    char ch2;
    for (int index3 = index2; (ch2 = chArray2[index3]) != char.MinValue; ++index3)
    {
      if (new string(new char[1]{ seps }).IndexOf(ch2) == -1)
      {
        string str = new string(chArray2);
        psaveptr = (str + (object) index3).ToCharArray();
        goto label_16;
      }
    }
    psaveptr = (char[]) null;
label_16:
    return charArray;
  }

  private Pix PixReduceRankBinaryCascade(
    Pix pixs,
    int level1,
    int level2,
    int level3,
    int level4)
  {
    if (pixs.D != 1)
      return (Pix) null;
    if (level1 > 4 || level2 > 4 || level3 > 4 || level4 > 4)
      return (Pix) null;
    if (level1 <= 0)
      return this.PixCopy((Pix) null, pixs);
    short[] intab = this.MakeSubsampleTab2x();
    Pix pixs1 = this.PixReduceRankBinary2(pixs, level1, intab);
    short[] numArray;
    if (level2 <= 0)
    {
      numArray = new short[0];
      return pixs1;
    }
    Pix pixs2 = this.PixReduceRankBinary2(pixs1, level2, intab);
    if (level3 <= 0)
    {
      numArray = new short[0];
      return pixs2;
    }
    Pix pixs3 = this.PixReduceRankBinary2(pixs2, level3, intab);
    if (level4 <= 0)
    {
      numArray = new short[0];
      return pixs3;
    }
    Pix pix = this.PixReduceRankBinary2(pixs3, level4, intab);
    numArray = new short[0];
    return pix;
  }

  private Pix PixReduceRankBinary2(Pix pixs, int level, short[] intab)
  {
    if (level < 1 || level > 4)
      return (Pix) null;
    short[] tab = intab.Length == 0 ? this.MakeSubsampleTab2x() : intab;
    int w = pixs.W;
    int h = pixs.H;
    if (h <= 1)
      return (Pix) null;
    int wpl1 = pixs.Wpl;
    uint[] data1 = pixs.Data;
    Pix pix = this.PixCreate(w / 2, h / 2, 1);
    if (pix == null)
      return (Pix) null;
    this.PixCopyResolution(pix, pixs);
    this.PixScaleResolution(pix, 0.5f, 0.5f);
    int wpl2 = pix.Wpl;
    uint[] data2 = pix.Data;
    this.ReduceRankBinary2Low(ref data2, wpl2, data1, h, wpl1, tab, level);
    pix.Data = data2;
    short[] numArray = new short[1];
    return pix;
  }

  private void ReduceRankBinary2Low(
    ref uint[] datad,
    int wpld,
    uint[] datas,
    int hs,
    int wpls,
    short[] tab,
    int level)
  {
    int num1 = Math.Min(wpls, 2 * wpld);
    switch (level)
    {
      case 1:
        int num2 = 0;
        int num3 = 0;
        while (num2 < hs - 1)
        {
          int num4 = num2 * wpls;
          int num5 = num3 * wpld;
          for (int index1 = 0; index1 < num1; ++index1)
          {
            uint num6 = datas[num4 + index1] | datas[num4 + wpls + index1];
            uint num7 = (num6 | num6 << 1) & 2863311530U /*0xAAAAAAAA*/;
            uint num8 = num7 | num7 << 7;
            byte index2 = (byte) (num8 >> 24);
            byte index3 = (byte) (num8 >> 8 & (uint) byte.MaxValue);
            short val = (short) ((int) tab[(int) index2] << 8 | (int) tab[(int) index3]);
            JBIG2Statics.SetDataTwoBytes(ref datad, num5 + index1, val);
          }
          num2 += 2;
          ++num3;
        }
        break;
      case 2:
        int num9 = 0;
        int num10 = 0;
        while (num9 < hs - 1)
        {
          int num11 = num9 * wpls;
          int num12 = num10 * wpld;
          for (int index4 = 0; index4 < num1; ++index4)
          {
            uint data1 = datas[num11 + index4];
            uint data2 = datas[num11 + wpls + index4];
            uint num13 = data1 & data2;
            uint num14 = num13 | num13 << 1;
            uint num15 = data1 | data2;
            uint num16 = num15 & num15 << 1;
            uint num17 = (num14 | num16) & 2863311530U /*0xAAAAAAAA*/;
            uint num18 = num17 | num17 << 7;
            byte index5 = (byte) (num18 >> 24);
            byte index6 = (byte) (num18 >> 8 & (uint) byte.MaxValue);
            short val = (short) ((int) tab[(int) index5] << 8 | (int) tab[(int) index6]);
            JBIG2Statics.SetDataTwoBytes(ref datad, num12 + index4, val);
          }
          num9 += 2;
          ++num10;
        }
        break;
      case 3:
        int num19 = 0;
        int num20 = 0;
        while (num19 < hs - 1)
        {
          int num21 = num19 * wpls;
          int num22 = num20 * wpld;
          for (int index7 = 0; index7 < num1; ++index7)
          {
            uint data3 = datas[num21 + index7];
            uint data4 = datas[num21 + wpls + index7];
            uint num23 = data3 & data4;
            uint num24 = num23 | num23 << 1;
            uint num25 = data3 | data4;
            uint num26 = num25 & num25 << 1;
            uint num27 = num24 & num26 & 2863311530U /*0xAAAAAAAA*/;
            uint num28 = num27 | num27 << 7;
            byte index8 = (byte) (num28 >> 24);
            byte index9 = (byte) (num28 >> 8 & (uint) byte.MaxValue);
            short val = (short) ((int) tab[(int) index8] << 8 | (int) tab[(int) index9]);
            JBIG2Statics.SetDataTwoBytes(ref datad, num22 + index7, val);
          }
          num19 += 2;
          ++num20;
        }
        break;
      case 4:
        int num29 = 0;
        int num30 = 0;
        while (num29 < hs - 1)
        {
          int num31 = num29 * wpls;
          int num32 = num30 * wpld;
          for (int index10 = 0; index10 < num1; ++index10)
          {
            uint num33 = datas[num31 + index10] & datas[num31 + wpls + index10];
            uint num34 = num33 & num33 << 1 & 2863311530U /*0xAAAAAAAA*/;
            uint num35 = num34 | num34 << 7;
            byte index11 = (byte) (num35 >> 24);
            byte index12 = (byte) (num35 >> 8 & (uint) byte.MaxValue);
            short val = (short) ((int) tab[(int) index11] << 8 | (int) tab[(int) index12]);
            JBIG2Statics.SetDataTwoBytes(ref datad, num32 + index10, val);
          }
          num29 += 2;
          ++num30;
        }
        break;
    }
  }

  private short[] MakeSubsampleTab2x()
  {
    short[] numArray = new short[256 /*0x0100*/];
    for (int index = 0; index < 256 /*0x0100*/; ++index)
      numArray[index] = (short) (index & 1 | (index & 4) >> 1 | (index & 16 /*0x10*/) >> 2 | (index & 64 /*0x40*/) >> 3 | (index & 2) << 3 | (index & 8) << 2 | (index & 32 /*0x20*/) << 1 | index & 128 /*0x80*/);
    return numArray;
  }

  private Pixa PixaClipToPix(Pixa pixas, Pix pixs)
  {
    int n = pixas.N;
    Pixa pixa;
    if ((pixa = JBIG2Statics.CreatePixa(n)) == null)
    {
      Console.WriteLine("pixad not made");
      return (Pixa) null;
    }
    for (int index = 0; index < n; ++index)
    {
      Pix pix1 = this.PixaGetPix(pixas, index, 2);
      Box box = this.PixaGetBox(pixas, index, 1);
      Pix pix2 = this.PixClipRectangle(pixs, box, (Box) null);
      this.PixAnd(pix2, pix2, pix1);
      this.PixaAddPix(pixa, pix2, 0);
      this.PixaAddBox(pixa, box, 0);
    }
    return pixa;
  }

  private Pix PixAnd(Pix pixd, Pix pixs1, Pix pixs2)
  {
    pixd = this.PixCopy(pixd, pixs1);
    this.PixRasterop(pixd, 0, 0, pixd.W, pixd.H, JBIG2Statics.PixSrc & JBIG2Statics.PixDst, pixs2, 0, 0);
    return pixd;
  }

  private void PixaAddBox(Pixa pixa, Box box, int copyflag)
  {
    this.BoxaAddBox(pixa.Boxa, box, copyflag);
  }

  private Box PixaGetBox(Pixa pixa, int index, int accesstype)
  {
    Box box = pixa.Boxa.Box[index];
    return box != null ? new Box(box.X, box.Y, box.W, box.H) : (Box) null;
  }

  private Pix PixaGetPix(Pixa pixa, int index, int accesstype)
  {
    if (index < 0 || index >= pixa.N)
      return (Pix) null;
    return accesstype == 1 || accesstype == 2 ? this.PixCopy((Pix) null, pixa.Pix[index]) : (Pix) null;
  }

  private Boxa PixConnComp(Pix pixs, ref Pixa ppixa, int connectivity)
  {
    if (pixs.D != 1)
      return (Boxa) null;
    if (connectivity != 4 && connectivity != 8)
      return (Boxa) null;
    return ppixa != null ? this.PixConnCompBB(pixs, connectivity) : this.PixConnCompPixa(pixs, ref ppixa, connectivity);
  }

  private Boxa PixConnCompPixa(Pix pixs, ref Pixa ppixa, int connectivity)
  {
    int pempty = 0;
    int px = 0;
    int py = 0;
    Pix pix1 = (Pix) null;
    Pix pix2 = (Pix) null;
    L_Stack lStack = (L_Stack) null;
    if (pixs == null || pixs.D != 1)
      return (Boxa) null;
    if (connectivity != 4 && connectivity != 8)
      return (Boxa) null;
    Pixa pixa = JBIG2Statics.CreatePixa(0);
    ppixa = pixa;
    this.PixZero(pixs, ref pempty);
    if (pempty == 1)
      return JBIG2Statics.CreateBoxa(1);
    Pix pixs1 = this.PixCopy((Pix) null, pixs);
    Pix pix3 = this.PixCopy((Pix) null, pixs);
    L_Stack lstack1 = JBIG2Statics.CreateLStack(pixs.H);
    L_Stack lstack2 = JBIG2Statics.CreateLStack(0);
    lstack1.AuxStack = lstack2;
    Boxa boxa = this.CreateBoxa(0);
    int xstart = 0;
    for (int ystart = 0; this.NextOnPixelInRaster(pixs1, xstart, ystart, ref px, ref py) != 0; ystart = py)
    {
      Box box = this.PixSeedfillBB(pixs1, lstack1, px, py, connectivity);
      if (box == null)
        return (Boxa) null;
      this.BoxaAddBox(boxa, box, 0);
      Pix pix4 = this.PixClipRectangle(pixs1, box, (Box) null);
      Pix pixs2 = this.PixClipRectangle(pix3, box, (Box) null);
      this.PixXor(pix4, pix4, pixs2);
      this.PixRasterop(pix3, box.X, box.Y, box.W, box.H, JBIG2Statics.PixSrc ^ JBIG2Statics.PixDst, pix4, 0, 0);
      this.PixaAddPix(pixa, pix4, 0);
      xstart = px;
    }
    pixa.Boxa = (Boxa) null;
    pixa.Boxa = this.BoxaCopy(boxa, 2);
    lStack = (L_Stack) null;
    pix1 = (Pix) null;
    pix2 = (Pix) null;
    return boxa;
  }

  private Pix PixXor(Pix pixd, Pix pixs1, Pix pixs2)
  {
    pixd = this.PixCopy(pixd, pixs1);
    this.PixRasterop(pixd, 0, 0, pixd.W, pixd.H, JBIG2Statics.PixSrc ^ JBIG2Statics.PixDst, pixs2, 0, 0);
    return pixd;
  }

  private void PixaAddPix(Pixa pixa, Pix pix, int copyflag)
  {
    Pix pix1 = new Pix();
    switch (copyflag)
    {
      case 0:
        pix1 = pix;
        break;
      case 1:
      case 2:
        pix1 = this.PixCopy((Pix) null, pix);
        break;
    }
    int n = pixa.N;
    if (n >= pixa.Nalloc)
      this.PixaExtendArray(pixa);
    if (pixa.Pix.Count <= n)
    {
      while (pixa.Pix.Count < n)
        pixa.Pix.Add((Pix) null);
      pixa.Pix.Add(pix1);
    }
    else
      pixa.Pix[n] = pix1;
    ++pixa.N;
  }

  private void PixaExtendArray(Pixa pixa) => this.PixaExtendArrayToSize(pixa, 2 * pixa.Nalloc);

  private void PixaExtendArrayToSize(Pixa pixa, int size)
  {
    if (size > pixa.Nalloc)
      pixa.Nalloc = size;
    this.BoxaExtendArrayToSize(pixa.Boxa, size);
  }

  private Boxa BoxaCopy(Boxa boxa, int copyflag)
  {
    if (copyflag == 2)
    {
      ++boxa.RefCount;
      return boxa;
    }
    Boxa boxa1 = JBIG2Statics.CreateBoxa(boxa.Nalloc);
    for (int index = 0; index < boxa.N; ++index)
    {
      Box box = this.BoxaGetBox(boxa, index, 2);
      this.BoxaAddBox(boxa1, box, 0);
    }
    return boxa1;
  }

  private Box BoxaGetBox(Boxa boxa, int index, int accessflag)
  {
    if (index < 0 || index >= boxa.N)
      return (Box) null;
    Box box = boxa.Box[index];
    return accessflag == 1 ? new Box(box.W, box.Y, box.X, box.H) : box;
  }

  private Pix PixClipRectangle(Pix pixs, Box box, Box pboxc)
  {
    int op = 24;
    int w1 = pixs.W;
    int h1 = pixs.H;
    int d = pixs.D;
    Box rectangle = this.BoxClipToRectangle(box, w1, h1);
    int x = rectangle.X;
    int y = rectangle.Y;
    int w2 = rectangle.W;
    int h2 = rectangle.H;
    Pix pixd = this.PixCreate(w2, h2, d);
    this.PixCopyResolution(pixd, pixs);
    this.PixCopyColormap(pixd, pixs);
    this.PixRasterop(pixd, 0, 0, w2, h2, op, pixs, x, y);
    if (pboxc != null)
      pboxc = rectangle;
    return pixd;
  }

  private Box BoxClipToRectangle(Box box, int wi, int hi)
  {
    Box rectangle = new Box(box.X, box.Y, box.W, box.H);
    if (rectangle.X < 0)
    {
      rectangle.W += rectangle.X;
      rectangle.W = 0;
    }
    if (rectangle.Y < 0)
    {
      rectangle.H += rectangle.Y;
      rectangle.Y = 0;
    }
    if (rectangle.X + rectangle.W > wi)
      rectangle.W = wi - rectangle.X;
    if (rectangle.Y + rectangle.H > hi)
      rectangle.H = hi - rectangle.Y;
    return rectangle;
  }

  private Boxa PixConnCompBB(Pix pixs, int connectivity)
  {
    int pempty = 0;
    int px = 0;
    int py = 0;
    this.PixZero(pixs, ref pempty);
    if (pempty == 0)
      return JBIG2Statics.CreateBoxa(1);
    Pix pixs1 = this.PixCopy((Pix) null, pixs);
    L_Stack lstack1 = JBIG2Statics.CreateLStack(pixs.H);
    L_Stack lstack2 = JBIG2Statics.CreateLStack(0);
    lstack1.AuxStack = lstack2;
    Boxa boxa = JBIG2Statics.CreateBoxa(0);
    int xstart = 0;
    for (int ystart = 0; this.NextOnPixelInRaster(pixs1, xstart, ystart, ref px, ref py) <= 0; ystart = py)
    {
      Box box;
      if ((box = this.PixSeedfillBB(pixs1, lstack1, px, py, connectivity)) == null)
        return (Boxa) null;
      this.BoxaAddBox(boxa, box, 0);
      xstart = px;
    }
    return boxa;
  }

  private void BoxaAddBox(Boxa boxa, Box box, int copyflag)
  {
    Box box1;
    switch (copyflag)
    {
      case 0:
        box1 = box;
        break;
      case 1:
      case 2:
        box1 = new Box(box.X, box.Y, box.W, box.H);
        break;
      default:
        return;
    }
    if (box1 == null)
      return;
    int n = boxa.N;
    if (n >= boxa.Nalloc)
      this.BoxaExtendArray(boxa);
    if (boxa.Box.Count < n)
    {
      boxa.Box[n] = box1;
    }
    else
    {
      while (boxa.Box.Count <= n)
        boxa.Box.Add((Box) null);
      boxa.Box[n] = box1;
    }
    ++boxa.N;
  }

  private void BoxaExtendArray(Boxa boxa) => this.BoxaExtendArrayToSize(boxa, 2 * boxa.Nalloc);

  private void BoxaExtendArrayToSize(Boxa boxa, int size)
  {
    if (size <= boxa.Nalloc)
      return;
    boxa.Nalloc = size;
  }

  private Box PixSeedfillBB(Pix pixs, L_Stack lstack, int x, int y, int connectivity)
  {
    if (pixs == null || pixs.D != 1)
      return (Box) null;
    if (connectivity != 4 && connectivity != 8)
      return (Box) null;
    Box box;
    switch (connectivity)
    {
      case 4:
        if ((box = this.PixSeedfill4BB(pixs, lstack, x, y)) == null)
          return (Box) null;
        break;
      case 8:
        if ((box = this.PixSeedfill8BB(pixs, lstack, x, y)) == null)
          return (Box) null;
        break;
      default:
        return (Box) null;
    }
    return box;
  }

  private Box PixSeedfill4BB(Pix pixs, L_Stack lstack, int x, int y)
  {
    int pxleft = 0;
    int pxright = 0;
    int pdy = 0;
    int xleft = 0;
    if (pixs == null || pixs.D != 1)
      return (Box) null;
    int w = pixs.W;
    int h = pixs.H;
    int num = w - 1;
    int ymax = h - 1;
    uint[] data = pixs.Data;
    int wpl = pixs.Wpl;
    int index1 = y * wpl;
    uint[] line = data;
    if (x < 0 || x > num || y < 0 || y > ymax || JBIG2Statics.GetDataBit(data, index1, x) == 0U)
      return (Box) null;
    int pminy1;
    int pminx = pminy1 = 100000;
    int pmaxy;
    int pmaxx1 = pmaxy = 0;
    this.PushFillsegBB(lstack, x, x, y, 1, ymax, ref pminx, ref pmaxx1, ref pminy1, ref pmaxy);
    this.PushFillsegBB(lstack, x, x, y + 1, -1, ymax, ref pminx, ref pmaxx1, ref pminy1, ref pmaxy);
    int pmaxx2;
    pminx = pmaxx2 = x;
    int pminy2 = pmaxy = y;
    while (lstack.Array.Count > 0)
    {
      this.PopFillseg(lstack, ref pxleft, ref pxright, ref y, ref pdy);
      int index2 = y * wpl;
      for (x = pxleft; x >= 0 && JBIG2Statics.GetDataBit(line, index2, x) == 1U; --x)
        JBIG2Statics.ClearDataBit(ref line, index2, x);
      bool flag = false;
      if (x >= pxleft)
        flag = true;
      if (!flag)
      {
        xleft = x + 1;
        if (xleft < pxleft - 1)
          this.PushFillsegBB(lstack, xleft, pxleft - 1, y, -pdy, ymax, ref pminx, ref pmaxx2, ref pminy2, ref pmaxy);
        x = pxleft + 1;
      }
label_14:
      if (!flag)
      {
        for (; x <= num && JBIG2Statics.GetDataBit(line, index2, x) == 1U; ++x)
          JBIG2Statics.ClearDataBit(ref line, index2, x);
        this.PushFillsegBB(lstack, xleft, x - 1, y, pdy, ymax, ref pminx, ref pmaxx2, ref pminy2, ref pmaxy);
        if (x > pxright + 1)
          this.PushFillsegBB(lstack, pxright + 1, x - 1, y, -pdy, ymax, ref pminx, ref pmaxx2, ref pminy2, ref pmaxy);
      }
      ++x;
      while (x <= pxright && x <= num && JBIG2Statics.GetDataBit(line, index2, x) == 0U)
        ++x;
      xleft = x;
      flag = false;
      if (x <= pxright && x <= num)
        goto label_14;
    }
    Box box;
    return (box = this.BoxCreate(pminx, pminy2, pmaxx2 - pminx + 1, pmaxy - pminy2 + 1)) == null ? (Box) null : box;
  }

  private Box PixSeedfill8BB(Pix pixs, L_Stack lstack, int x, int y)
  {
    int xleft = 0;
    int pxleft = 0;
    int pxright = 0;
    int pdy = 0;
    if (pixs == null || pixs.D != 1)
      return (Box) null;
    int w = pixs.W;
    int h = pixs.H;
    int num = w - 1;
    int ymax = h - 1;
    uint[] data = pixs.Data;
    int wpl = pixs.Wpl;
    uint[] line = data;
    int index1 = y * wpl;
    if (x < 0 || x > num || y < 0 || y > ymax || JBIG2Statics.GetDataBit(line, index1, x) == 0U)
      return (Box) null;
    int pminy1;
    int pminx = pminy1 = 100000;
    int pmaxy;
    int pmaxx1 = pmaxy = 0;
    this.PushFillsegBB(lstack, x, x, y, 1, ymax, ref pminx, ref pmaxx1, ref pminy1, ref pmaxy);
    this.PushFillsegBB(lstack, x, x, y + 1, -1, ymax, ref pminx, ref pmaxx1, ref pminy1, ref pmaxy);
    int pmaxx2;
    pminx = pmaxx2 = x;
    int pminy2 = pmaxy = y;
    while (lstack.Array.Count > 0)
    {
      this.PopFillseg(lstack, ref pxleft, ref pxright, ref y, ref pdy);
      int index2 = y * wpl;
      for (x = pxleft - 1; x >= 0 && JBIG2Statics.GetDataBit(line, index2, x) == 1U; --x)
        JBIG2Statics.ClearDataBit(ref line, index2, x);
      bool flag = false;
      if (x >= pxleft - 1)
        flag = true;
      if (!flag)
      {
        xleft = x + 1;
        if (xleft < pxleft)
          this.PushFillsegBB(lstack, xleft, pxleft - 1, y, -pdy, ymax, ref pminx, ref pmaxx2, ref pminy2, ref pmaxy);
        x = pxleft;
      }
label_14:
      if (!flag)
      {
        for (; x <= num && JBIG2Statics.GetDataBit(line, index2, x) == 1U; ++x)
          JBIG2Statics.ClearDataBit(ref line, index2, x);
        this.PushFillsegBB(lstack, xleft, x - 1, y, pdy, ymax, ref pminx, ref pmaxx2, ref pminy2, ref pmaxy);
        if (x > pxright)
          this.PushFillsegBB(lstack, pxright + 1, x - 1, y, -pdy, ymax, ref pminx, ref pmaxx2, ref pminy2, ref pmaxy);
      }
      ++x;
      while (x <= pxright + 1 && x <= num && JBIG2Statics.GetDataBit(line, index2, x) == 0U)
        ++x;
      xleft = x;
      flag = false;
      if (x <= pxright + 1 && x <= num)
        goto label_14;
    }
    Box box;
    return (box = this.BoxCreate(pminx, pminy2, pmaxx2 - pminx + 1, pmaxy - pminy2 + 1)) == null ? (Box) null : box;
  }

  private Box BoxCreate(int x, int y, int w, int h)
  {
    if (w >= 0)
      ;
    if (x < 0)
    {
      w += x;
      x = 0;
    }
    if (y < 0)
    {
      h += y;
      y = 0;
    }
    return new Box(x, y, w, h) { RefCount = 1 };
  }

  private void PopFillseg(
    L_Stack lstack,
    ref int pxleft,
    ref int pxright,
    ref int py,
    ref int pdy)
  {
    L_Stack auxStack;
    FillSeg fillSeg;
    if (lstack == null || (auxStack = lstack.AuxStack) == null || (fillSeg = this.LstackRemove(lstack)) == null)
      return;
    pxleft = fillSeg.XLeft;
    pxright = fillSeg.XRight;
    py = fillSeg.Y + fillSeg.Dy;
    pdy = fillSeg.Dy;
    this.LstackAdd(auxStack, (object) fillSeg);
  }

  private void PushFillsegBB(
    L_Stack lstack,
    int xleft,
    int xright,
    int y,
    int dy,
    int ymax,
    ref int pminx,
    ref int pmaxx,
    ref int pminy,
    ref int pmaxy)
  {
    pminx = Math.Min(pminx, xleft);
    pmaxx = Math.Max(pmaxx, xright);
    pminy = Math.Min(pminy, y);
    pmaxy = Math.Max(pmaxy, y);
    L_Stack auxStack;
    if (y + dy < 0 || y + dy > ymax || (auxStack = lstack.AuxStack) == null)
      return;
    FillSeg fillSeg = !lstack.Array.Contains((object) auxStack) ? new FillSeg() : this.LstackRemove(auxStack);
    fillSeg.XLeft = xleft;
    fillSeg.XRight = xright;
    fillSeg.Y = y;
    fillSeg.Dy = dy;
    this.LstackAdd(lstack, (object) fillSeg);
  }

  private int LstackAdd(L_Stack lstack, object item)
  {
    lstack.Array.Add(item);
    return lstack.Array.Count;
  }

  private FillSeg LstackRemove(L_Stack lstack)
  {
    if (lstack.Array.Count == 0)
      return (FillSeg) null;
    FillSeg fillSeg = (FillSeg) lstack.Array[lstack.Array.Count - 1];
    lstack.Array.RemoveAt(lstack.Array.Count - 1);
    return fillSeg;
  }

  private int NextOnPixelInRaster(Pix pixs, int xstart, int ystart, ref int px, ref int py)
  {
    int w = pixs.W;
    int h = pixs.H;
    if (pixs.D != 1)
      return 0;
    int wpl = pixs.Wpl;
    return this.NextOnPixelInRasterLow(pixs.Data, w, h, wpl, xstart, ystart, ref px, ref py);
  }

  private int NextOnPixelInRasterLow(
    uint[] data,
    int w,
    int h,
    int wpl,
    int xstart,
    int ystart,
    ref int px,
    ref int py)
  {
    uint[] line = data;
    int index1 = ystart * wpl;
    if (line[index1 + xstart / 32 /*0x20*/] > 0U)
    {
      int num = xstart - xstart % 32 /*0x20*/ + 31 /*0x1F*/;
      for (int n = xstart; n <= num && n < w; ++n)
      {
        if (JBIG2Statics.GetDataBit(line, index1, n) == 1U)
        {
          px = n;
          py = ystart;
          return 1;
        }
      }
    }
    int num1 = xstart / 32 /*0x20*/ + 1;
    for (int n = 32 /*0x20*/ * num1; n < w; n += 32 /*0x20*/)
    {
      if (line[index1 + num1] > 0U)
      {
        for (int index2 = 0; index2 < 32 /*0x20*/ && n < w; ++n)
        {
          if (JBIG2Statics.GetDataBit(line, index1, n) == 1U)
          {
            px = n;
            py = ystart;
            return 1;
          }
          ++index2;
        }
      }
      ++num1;
    }
    for (int index3 = ystart + 1; index3 < h; ++index3)
    {
      int index4 = index3 * wpl;
      for (int n = 0; n < w; n += 32 /*0x20*/)
      {
        if (line[index4] > 0U)
        {
          for (int index5 = 0; index5 < 32 /*0x20*/ && n < w; ++n)
          {
            if (JBIG2Statics.GetDataBit(line, index3 * wpl, n) == 1U)
            {
              px = n;
              py = index3;
              return 1;
            }
            ++index5;
          }
        }
        ++index4;
      }
    }
    return 0;
  }

  private void PixZero(Pix pix, ref int pempty)
  {
    pempty = 0;
    if (pix.Colormap != null)
      throw new Exception("pix is colormapped");
    int num1 = pix.W * pix.D;
    int h = pix.H;
    int wpl = pix.Wpl;
    int num2 = num1 / 32 /*0x20*/;
    int num3 = num1 & 31 /*0x1F*/;
    uint num4 = (uint) (-1 << 32 /*0x20*/ - num3);
    for (int index1 = 0; index1 < h; ++index1)
    {
      int index2 = wpl * index1;
      for (int index3 = 0; index3 < num2; ++index3)
      {
        if (pix.Data[index2++] != 0U)
        {
          pempty = 0;
          return;
        }
      }
      if (num3 > 0 && ((int) pix.Data[index2] & (int) num4) != 0)
      {
        pempty = 0;
        break;
      }
    }
  }

  private Sarray SarrayCreate(int n)
  {
    int num = 50;
    if (n <= 0)
      n = num;
    return new Sarray(n);
  }

  private Pix PixSubtract(Pix pixd, Pix pixs1, Pix pixs2)
  {
    int w = pixs1.W;
    int h = pixs1.H;
    if (pixd == null)
    {
      pixd = this.PixCopy((Pix) null, pixs1);
      this.PixRasterop(pixd, 0, 0, w, h, JBIG2Statics.PixDst & JBIG2Statics.PixNot(JBIG2Statics.PixSrc), pixs2, 0, 0);
    }
    else if (pixd == pixs1)
      this.PixRasterop(pixd, 0, 0, w, h, JBIG2Statics.PixDst & JBIG2Statics.PixNot(JBIG2Statics.PixSrc), pixs2, 0, 0);
    else if (pixd == pixs2)
    {
      this.PixRasterop(pixd, 0, 0, w, h, JBIG2Statics.PixNot(JBIG2Statics.PixDst) & JBIG2Statics.PixSrc, pixs1, 0, 0);
    }
    else
    {
      this.PixCopy(pixd, pixs1);
      this.PixRasterop(pixd, 0, 0, w, h, JBIG2Statics.PixDst & JBIG2Statics.PixNot(JBIG2Statics.PixSrc), pixs2, 0, 0);
    }
    return pixd;
  }

  private void ExpandBinaryPower2Low(
    uint[] datad,
    int wd,
    int hd,
    int wpld,
    uint[] datas,
    int ws,
    int hs,
    int wpls,
    int factor)
  {
    int num1 = 0;
    uint[] numArray1 = new uint[4]
    {
      0U,
      (uint) ushort.MaxValue,
      4294901760U,
      uint.MaxValue
    };
    switch (factor)
    {
      case 2:
        short[] numArray2 = this.MakeExpandTab2x();
        int num2 = (ws + 7) / 8;
        for (int index = 0; index < hs; ++index)
        {
          num1 = index * wpls;
          int num3 = 2 * index * wpld;
          for (int n = 0; n < num2; ++n)
          {
            uint dataByte = JBIG2Statics.GetDataByte(datas, n);
            JBIG2Statics.SetDataTwoBytes(ref datad, num3 + n, numArray2[(IntPtr) dataByte]);
          }
          Array.Copy((Array) datad, 0, (Array) datad, wpld, wpld);
          uint[] sourceArray = new uint[wpld];
          Array.Copy((Array) sourceArray, (Array) datad, sourceArray.Length);
        }
        short[] numArray3 = new short[1];
        break;
      case 4:
        short[] numArray4 = this.MakeExpandTab4x();
        int num4 = (ws + 7) / 8;
        for (int index1 = 0; index1 < hs; ++index1)
        {
          int num5 = index1 * wpls;
          int num6 = 4 * index1 * wpld;
          for (int index2 = 0; index2 < num4; ++index2)
          {
            uint dataByte = JBIG2Statics.GetDataByte(datas, num5 + index2);
            datad[num6 + index2] = (uint) numArray4[(IntPtr) dataByte];
          }
          for (int index3 = 1; index3 < 4; ++index3)
          {
            Array.Copy((Array) datad, 0, (Array) datad, index3 * wpld, index3 * wpld);
            uint[] sourceArray = new uint[index3 * wpld];
            Array.Copy((Array) sourceArray, (Array) datad, sourceArray.Length);
          }
        }
        short[] numArray5 = new short[1];
        break;
      case 8:
        short[] numArray6 = this.MakeExpandTab8x();
        int num7 = (ws + 3) / 4;
        for (int index4 = 0; index4 < hs; ++index4)
        {
          int index5 = index4 * wpls;
          int num8 = 8 * index4 * wpld;
          for (int n = 0; n < num7; ++n)
          {
            uint dataQbit = JBIG2Statics.GetDataQbit(datas[index5], n);
            if (dataQbit > 15U)
              Console.WriteLine("sval = %d; should be < 16", (object) dataQbit);
            datad[num8 + n] = (uint) numArray6[(IntPtr) dataQbit];
          }
          for (int index6 = 1; index6 < 8; ++index6)
          {
            Array.Copy((Array) datad, 0, (Array) datad, index6 * wpld, index6 * wpld);
            uint[] sourceArray = new uint[index6 * wpld];
            Array.Copy((Array) sourceArray, (Array) datad, sourceArray.Length);
          }
        }
        short[] numArray7 = new short[1];
        break;
      case 16 /*0x10*/:
        int num9 = (ws + 1) / 2;
        for (int index7 = 0; index7 < hs; ++index7)
        {
          int index8 = index7 * wpls;
          int num10 = 16 /*0x10*/ * index7 * wpld;
          for (int n = 0; n < num9; ++n)
          {
            uint dataDibit = JBIG2Statics.GetDataDibit(datas[index8], n);
            datad[num10 + n] = numArray1[(IntPtr) dataDibit];
          }
          for (int index9 = 1; index9 < 16 /*0x10*/; ++index9)
          {
            Array.Copy((Array) datad, 0, (Array) datad, index9 * wpld, index9 * wpld);
            uint[] sourceArray = new uint[index9 * wpld];
            Array.Copy((Array) sourceArray, (Array) datad, sourceArray.Length);
          }
        }
        break;
      default:
        Console.WriteLine("expansion factor not in {2,4,8,16}");
        break;
    }
  }

  private short[] MakeExpandTab8x()
  {
    short[] numArray = new short[16 /*0x10*/];
    for (int index = 0; index < 16 /*0x10*/; ++index)
    {
      if ((index & 1) != 0)
        numArray[index] = (short) byte.MaxValue;
      if ((index & 2) != 0)
        numArray[index] = (short) ((int) numArray[index] | 65280);
      if ((index & 4) != 0)
        numArray[index] = (short) ((int) numArray[index] | 16711680 /*0xFF0000*/);
      if ((index & 8) != 0)
        numArray[index] = (short) ((long) numArray[index] | 4278190080L /*0xFF000000*/);
    }
    return numArray;
  }

  private short[] MakeExpandTab2x()
  {
    short[] numArray = new short[256 /*0x0100*/];
    for (int index = 0; index < 256 /*0x0100*/; ++index)
    {
      if ((index & 1) != 0)
        numArray[index] = (short) 3;
      if ((index & 2) != 0)
        numArray[index] |= (short) 12;
      if ((index & 4) != 0)
        numArray[index] |= (short) 48 /*0x30*/;
      if ((index & 8) != 0)
        numArray[index] |= (short) 192 /*0xC0*/;
      if ((index & 16 /*0x10*/) != 0)
        numArray[index] |= (short) 768 /*0x0300*/;
      if ((index & 32 /*0x20*/) != 0)
        numArray[index] |= (short) 3072 /*0x0C00*/;
      if ((index & 64 /*0x40*/) != 0)
        numArray[index] |= (short) 12288 /*0x3000*/;
      if ((index & 128 /*0x80*/) != 0)
        numArray[index] = (short) ((int) numArray[index] | 49152 /*0xC000*/);
    }
    return numArray;
  }

  private short[] MakeExpandTab4x()
  {
    short[] numArray = new short[256 /*0x0100*/];
    for (int index = 0; index < 256 /*0x0100*/; ++index)
    {
      if ((index & 1) != 0)
        numArray[index] = (short) 15;
      if ((index & 2) != 0)
        numArray[index] |= (short) 240 /*0xF0*/;
      if ((index & 4) != 0)
        numArray[index] |= (short) 3840 /*0x0F00*/;
      if ((index & 8) != 0)
        numArray[index] = (short) ((int) numArray[index] | 61440 /*0xF000*/);
      if ((index & 16 /*0x10*/) != 0)
        numArray[index] = (short) ((int) numArray[index] | 983040 /*0x0F0000*/);
      if ((index & 32 /*0x20*/) != 0)
        numArray[index] = (short) ((int) numArray[index] | 15728640 /*0xF00000*/);
      if ((index & 64 /*0x40*/) != 0)
        numArray[index] = (short) ((int) numArray[index] | 251658240 /*0x0F000000*/);
      if ((index & 128 /*0x80*/) != 0)
        numArray[index] = (short) ((long) numArray[index] | 4026531840L /*0xF0000000*/);
    }
    return numArray;
  }

  private Pix PixThresholdToBinary(Pix pixs, int thresh)
  {
    int w = pixs.W;
    int h = pixs.H;
    int d = pixs.D;
    switch (d)
    {
      case 4:
      case 8:
        Pix pix = this.PixRemoveColormap(pixs, this.m_removeCmapToGrayScale);
        uint[] data = pix.Data;
        int wpl1 = pix.Wpl;
        if (pixs.Colormap != null && d == 4)
        {
          d = 8;
          thresh *= 16 /*0x10*/;
        }
        Pix pixd = this.PixCreate(w, h, 1);
        this.PixCopyResolution(pixd, pixs);
        int wpl2 = pixd.Wpl;
        uint[] datad;
        this.ThresholdToBinaryLow(out datad, w, h, wpl2, data, d, wpl1, thresh);
        pixd.Data = datad;
        return pixd;
      default:
        return (Pix) null;
    }
  }

  private void ThresholdToBinaryLow(
    out uint[] datad,
    int w,
    int h,
    int wpld,
    uint[] datas,
    int d,
    int wpls,
    int thresh)
  {
    datad = new uint[h * wpld];
    for (int index = 0; index < h; ++index)
    {
      int srcIndex = index * wpls;
      int destIndex = index * wpld;
      this.ThresholdToBinaryLineLow(ref datad, w, datas, d, thresh, ref srcIndex, ref destIndex);
    }
  }

  private void ThresholdToBinaryLineLow(
    ref uint[] lined,
    int w,
    uint[] lines,
    int d,
    int thresh,
    ref int srcIndex,
    ref int destIndex)
  {
    int num1 = srcIndex;
    int index1 = destIndex;
    uint num2 = 0;
    switch (d)
    {
      case 4:
        int num3;
        for (num3 = 0; num3 + 31 /*0x1F*/ < w; num3 += 32 /*0x20*/)
        {
          uint num4 = 0;
          for (int index2 = 0; index2 < 4; ++index2)
          {
            num2 = lines[num1++];
            num4 = num4 << 8 | (num2 >> 28 & 15U) - (uint) thresh >> 24 & 128U /*0x80*/ | (num2 >> 24 & 15U) - (uint) thresh >> 25 & 64U /*0x40*/ | (num2 >> 20 & 15U) - (uint) thresh >> 26 & 32U /*0x20*/ | (num2 >> 16 /*0x10*/ & 15U) - (uint) thresh >> 27 & 16U /*0x10*/ | (num2 >> 12 & 15U) - (uint) thresh >> 28 & 8U | (num2 >> 8 & 15U) - (uint) thresh >> 29 & 4U | (num2 >> 4 & 15U) - (uint) thresh >> 30 & 2U | (num2 & 15U) - (uint) thresh >> 31 /*0x1F*/ & 1U;
          }
          lined[index1++] = num4;
        }
        srcIndex = num1;
        destIndex = index1;
        if (num3 < w)
        {
          uint num5 = 0;
          for (; num3 < w; ++num3)
          {
            if ((num3 & 7) == 0)
              num2 = lines[num1++];
            uint num6 = num2 >> 28 & 15U;
            num2 <<= 4;
            num5 |= (uint) (((int) (num6 - (uint) thresh >> 31 /*0x1F*/) & 1) << 31 /*0x1F*/ - (num3 & 31 /*0x1F*/));
          }
          lined[index1] = num5;
          break;
        }
        break;
      case 8:
        int num7;
        for (num7 = 0; num7 + 31 /*0x1F*/ < w; num7 += 32 /*0x20*/)
        {
          uint num8 = 0;
          for (int index3 = 0; index3 < 8; ++index3)
          {
            num2 = lines[num1++];
            num8 = num8 << 4 | (num2 >> 24 & (uint) byte.MaxValue) - (uint) thresh >> 28 & 8U | (num2 >> 16 /*0x10*/ & (uint) byte.MaxValue) - (uint) thresh >> 29 & 4U | (num2 >> 8 & (uint) byte.MaxValue) - (uint) thresh >> 30 & 2U | (num2 & (uint) byte.MaxValue) - (uint) thresh >> 31 /*0x1F*/ & 1U;
          }
          lined[index1++] = num8;
        }
        if (num7 < w)
        {
          uint num9 = 0;
          for (; num7 < w; ++num7)
          {
            if ((num7 & 3) == 0)
              num2 = lines[num1++];
            uint num10 = num2 >> 24 & (uint) byte.MaxValue;
            num2 <<= 8;
            num9 |= (uint) (((int) (num10 - (uint) thresh >> 31 /*0x1F*/) & 1) << 31 /*0x1F*/ - (num7 & 31 /*0x1F*/));
          }
          lined[index1] = num9;
          break;
        }
        break;
    }
    srcIndex = num1;
    destIndex = index1;
  }

  private Pix PixScaleGray4xLIThresh(Pix pixs, int thresh)
  {
    if (pixs.D != 8)
      return (Pix) null;
    int w = pixs.W;
    int h = pixs.H;
    int num1 = 4 * w;
    int height = 4 * h;
    int num2 = h - 1;
    uint[] data1 = pixs.Data;
    int wpl1 = pixs.Wpl;
    int wpld = (num1 + 3) / 4;
    uint[] lined = new uint[4 * wpld];
    Pix pix;
    if ((pix = this.PixCreate(num1, height, 1)) == null)
      return (Pix) null;
    this.PixCopyResolution(pix, pixs);
    this.PixScaleResolution(pix, 4f, 4f);
    int wpl2 = pix.Wpl;
    uint[] data2 = pix.Data;
    int num3 = 0;
    int num4 = 0;
    for (int index1 = 0; index1 < num2; ++index1)
    {
      int indexs = index1 * wpl1;
      this.ScaleGray4xLILineLow(ref lined, 0, wpld, data1, indexs, w, wpl1, 0);
      for (int index2 = 0; index2 < 4; ++index2)
      {
        int destIndex = index2 * wpl2;
        int srcIndex = index2 * wpld;
        this.ThresholdToBinaryLineLow(ref data2, num1, lined, 8, thresh, ref srcIndex, ref destIndex);
      }
    }
    int indexs1 = num2 * wpl1;
    this.ScaleGray4xLILineLow(ref lined, 0, wpld, data1, indexs1, w, wpl1, 1);
    num3 = num4 = 0;
    for (int index = 0; index < 4; ++index)
    {
      int destIndex = index * wpl2;
      int srcIndex = index * wpld;
      this.ThresholdToBinaryLineLow(ref data2, num1, lined, 8, thresh, ref srcIndex, ref destIndex);
    }
    pix.Data = data2;
    return pix;
  }

  private void ScaleGray4xLILineLow(
    ref uint[] lined,
    int indexd,
    int wpld,
    uint[] lines,
    int indexs,
    int ws,
    int wpls,
    int lastlineflag)
  {
    int num1 = ws - 1;
    int num2 = 4 * num1;
    if (lastlineflag == 0)
    {
      int n = indexs + wpls;
      int num3 = indexd + wpld;
      int num4 = indexd + 2 * wpld;
      int num5 = indexd + 3 * wpld;
      uint dataByte1 = JBIG2Statics.GetDataByte(lines, indexs);
      uint dataByte2 = JBIG2Statics.GetDataByte(lines, n);
      int num6 = 0;
      int num7 = 0;
      while (num6 < num1)
      {
        uint val = dataByte1;
        uint num8 = dataByte2;
        dataByte1 = JBIG2Statics.GetDataByte(lines, indexs + num6 + 1);
        dataByte2 = JBIG2Statics.GetDataByte(lines, n + num6 + 1);
        uint num9 = 3U * val;
        uint num10 = 3U * dataByte1;
        uint num11 = 3U * num8;
        uint num12 = 3U * dataByte2;
        JBIG2Statics.SetDataByte(ref lined, indexd + num7, val);
        JBIG2Statics.SetDataByte(ref lined, indexd + num7 + 1, (num9 + dataByte1) / 4U);
        JBIG2Statics.SetDataByte(ref lined, indexd + num7 + 2, (val + dataByte1) / 2U);
        JBIG2Statics.SetDataByte(ref lined, indexd + num7 + 3, (val + num10) / 4U);
        JBIG2Statics.SetDataByte(ref lined, num3 + num7, (num9 + num8) / 4U);
        JBIG2Statics.SetDataByte(ref lined, num3 + num7 + 1, (9U * val + num10 + num11 + dataByte2) / 16U /*0x10*/);
        JBIG2Statics.SetDataByte(ref lined, num3 + num7 + 2, (num9 + num10 + num8 + dataByte2) / 8U);
        JBIG2Statics.SetDataByte(ref lined, num3 + num7 + 3, (num9 + 9U * dataByte1 + num8 + num12) / 16U /*0x10*/);
        JBIG2Statics.SetDataByte(ref lined, num4 + num7, (val + num8) / 2U);
        JBIG2Statics.SetDataByte(ref lined, num4 + num7 + 1, (num9 + dataByte1 + num11 + dataByte2) / 8U);
        JBIG2Statics.SetDataByte(ref lined, num4 + num7 + 2, (val + dataByte1 + num8 + dataByte2) / 4U);
        JBIG2Statics.SetDataByte(ref lined, num4 + num7 + 3, (val + num10 + num8 + num12) / 8U);
        JBIG2Statics.SetDataByte(ref lined, num5 + num7, (val + num11) / 4U);
        JBIG2Statics.SetDataByte(ref lined, num5 + num7 + 1, ((uint) ((int) num9 + (int) dataByte1 + 9 * (int) num8) + num12) / 16U /*0x10*/);
        JBIG2Statics.SetDataByte(ref lined, num5 + num7 + 2, (val + dataByte1 + num11 + num12) / 8U);
        JBIG2Statics.SetDataByte(ref lined, num5 + num7 + 3, (uint) ((int) val + (int) num10 + (int) num11 + 9 * (int) dataByte2) / 16U /*0x10*/);
        ++num6;
        num7 += 4;
      }
      uint val1 = dataByte1;
      uint num13 = dataByte2;
      uint num14 = 3U * val1;
      uint num15 = 3U * num13;
      JBIG2Statics.SetDataByte(ref lined, indexd + num2, val1);
      JBIG2Statics.SetDataByte(ref lined, indexd + num2 + 1, val1);
      JBIG2Statics.SetDataByte(ref lined, indexd + num2 + 2, val1);
      JBIG2Statics.SetDataByte(ref lined, indexd + num2 + 3, val1);
      JBIG2Statics.SetDataByte(ref lined, num3 + num2, (num14 + num13) / 4U);
      JBIG2Statics.SetDataByte(ref lined, num3 + num2 + 1, (num14 + num13) / 4U);
      JBIG2Statics.SetDataByte(ref lined, num3 + num2 + 2, (num14 + num13) / 4U);
      JBIG2Statics.SetDataByte(ref lined, num3 + num2 + 3, (num14 + num13) / 4U);
      JBIG2Statics.SetDataByte(ref lined, num4 + num2, (val1 + num13) / 2U);
      JBIG2Statics.SetDataByte(ref lined, num4 + num2 + 1, (val1 + num13) / 2U);
      JBIG2Statics.SetDataByte(ref lined, num4 + num2 + 2, (val1 + num13) / 2U);
      JBIG2Statics.SetDataByte(ref lined, num4 + num2 + 3, (val1 + num13) / 2U);
      JBIG2Statics.SetDataByte(ref lined, num5 + num2, (val1 + num15) / 4U);
      JBIG2Statics.SetDataByte(ref lined, num5 + num2 + 1, (val1 + num15) / 4U);
      JBIG2Statics.SetDataByte(ref lined, num5 + num2 + 2, (val1 + num15) / 4U);
      JBIG2Statics.SetDataByte(ref lined, num5 + num2 + 3, (val1 + num15) / 4U);
    }
    else
    {
      int num16 = indexd + wpld;
      int num17 = indexd + 2 * wpld;
      int num18 = indexd + 3 * wpld;
      uint dataByte = JBIG2Statics.GetDataByte(lines, indexs);
      int num19 = 0;
      int num20 = 0;
      while (num19 < num1)
      {
        uint val = dataByte;
        dataByte = JBIG2Statics.GetDataByte(lines, num19 + 1);
        uint num21 = 3U * val;
        uint num22 = 3U * dataByte;
        JBIG2Statics.SetDataByte(ref lined, indexd + num20, val);
        JBIG2Statics.SetDataByte(ref lined, indexd + num20 + 1, (num21 + dataByte) / 4U);
        JBIG2Statics.SetDataByte(ref lined, indexd + num20 + 2, (val + dataByte) / 2U);
        JBIG2Statics.SetDataByte(ref lined, indexd + num20 + 3, (val + num22) / 4U);
        JBIG2Statics.SetDataByte(ref lined, num16 + num20, val);
        JBIG2Statics.SetDataByte(ref lined, num16 + num20 + 1, (num21 + dataByte) / 4U);
        JBIG2Statics.SetDataByte(ref lined, num16 + num20 + 2, (val + dataByte) / 2U);
        JBIG2Statics.SetDataByte(ref lined, num16 + num20 + 3, (val + num22) / 4U);
        JBIG2Statics.SetDataByte(ref lined, num17 + num20, val);
        JBIG2Statics.SetDataByte(ref lined, num17 + num20 + 1, (num21 + dataByte) / 4U);
        JBIG2Statics.SetDataByte(ref lined, num17 + num20 + 2, (val + dataByte) / 2U);
        JBIG2Statics.SetDataByte(ref lined, num17 + num20 + 3, (val + num22) / 4U);
        JBIG2Statics.SetDataByte(ref lined, num18 + num20, val);
        JBIG2Statics.SetDataByte(ref lined, num18 + num20 + 1, (num21 + dataByte) / 4U);
        JBIG2Statics.SetDataByte(ref lined, num18 + num20 + 2, (val + dataByte) / 2U);
        JBIG2Statics.SetDataByte(ref lined, num18 + num20 + 3, (val + num22) / 4U);
        ++num19;
        num20 += 4;
      }
      uint val2 = dataByte;
      JBIG2Statics.SetDataByte(ref lined, indexd + num2, val2);
      JBIG2Statics.SetDataByte(ref lined, indexd + num2 + 1, val2);
      JBIG2Statics.SetDataByte(ref lined, indexd + num2 + 2, val2);
      JBIG2Statics.SetDataByte(ref lined, indexd + num2 + 3, val2);
      JBIG2Statics.SetDataByte(ref lined, num16 + num2, val2);
      JBIG2Statics.SetDataByte(ref lined, num16 + num2 + 1, val2);
      JBIG2Statics.SetDataByte(ref lined, num16 + num2 + 2, val2);
      JBIG2Statics.SetDataByte(ref lined, num16 + num2 + 3, val2);
      JBIG2Statics.SetDataByte(ref lined, num17 + num2, val2);
      JBIG2Statics.SetDataByte(ref lined, num17 + num2 + 1, val2);
      JBIG2Statics.SetDataByte(ref lined, num17 + num2 + 2, val2);
      JBIG2Statics.SetDataByte(ref lined, num17 + num2 + 3, val2);
      JBIG2Statics.SetDataByte(ref lined, num18 + num2, val2);
      JBIG2Statics.SetDataByte(ref lined, num18 + num2 + 1, val2);
      JBIG2Statics.SetDataByte(ref lined, num18 + num2 + 2, val2);
      JBIG2Statics.SetDataByte(ref lined, num18 + num2 + 3, val2);
    }
  }

  private Pix PixScaleGray2xLIThresh(Pix pixs, int thresh)
  {
    if (pixs.D != 8)
      return (Pix) null;
    int xres = pixs.XRes;
    int yres = pixs.YRes;
    int num1 = 2 * xres;
    int height = 2 * yres;
    int num2 = yres - 1;
    uint[] data1 = pixs.Data;
    int wpl1 = pixs.Wpl;
    int wpld = (num1 + 3) / 4;
    uint[] lined = new uint[2 * wpld];
    Pix pix = this.PixCreate(num1, height, 1);
    this.PixCopyResolution(pix, pixs);
    this.PixScaleResolution(pix, 2f, 2f);
    int wpl2 = pix.Wpl;
    uint[] data2 = pix.Data;
    int srcIndex1 = 0;
    for (int index = 0; index < num2; ++index)
    {
      int indexs = index * wpl1;
      int destIndex1 = 2 * index * wpl2;
      this.ScaleGray2xLILineLow(ref lined, destIndex1, wpld, data1, indexs, xres, wpl1, 0);
      this.ThresholdToBinaryLineLow(ref data2, num1, lined, 8, thresh, ref srcIndex1, ref destIndex1);
      int destIndex2 = destIndex1 + wpl2;
      srcIndex1 += wpld;
      this.ThresholdToBinaryLineLow(ref data2, num1, lined, 8, thresh, ref srcIndex1, ref destIndex2);
    }
    int srcIndex2 = 0;
    int indexs1 = num2 * wpl1;
    int destIndex3 = 2 * num2 * wpl2;
    this.ScaleGray2xLILineLow(ref lined, destIndex3, wpld, data1, indexs1, xres, wpl1, 1);
    this.ThresholdToBinaryLineLow(ref data2, num1, lined, 8, thresh, ref srcIndex2, ref destIndex3);
    int destIndex4 = destIndex3 + wpl2;
    srcIndex2 += wpld;
    this.ThresholdToBinaryLineLow(ref data2, num1, lined, 8, thresh, ref srcIndex2, ref destIndex4);
    lined = (uint[]) null;
    pix.Data = data2;
    return pix;
  }

  private void ScaleGray2xLILineLow(
    ref uint[] lined,
    int indexd,
    int wpld,
    uint[] lines,
    int indexs,
    int ws,
    int wpls,
    int lastlineflag)
  {
    int num1 = ws - 1;
    if (lastlineflag == 0)
    {
      int index = indexs + wpls;
      int num2 = indexd + wpld;
      uint line1 = lines[indexs];
      uint line2 = lines[index];
      uint num3 = line1 >> 24 & (uint) byte.MaxValue;
      uint num4 = line2 >> 24 & (uint) byte.MaxValue;
      int num5 = 0;
      int num6 = 0;
      int num7 = 0;
      while (num5 + 3 < num1)
      {
        uint num8 = num3;
        uint num9 = line1 >> 16 /*0x10*/ & (uint) byte.MaxValue;
        uint num10 = num4;
        uint num11 = line2 >> 16 /*0x10*/ & (uint) byte.MaxValue;
        uint num12 = (uint) ((int) num8 << 24 | (int) (num8 + num9 >> 1) << 16 /*0x10*/);
        uint num13 = (uint) ((int) (num8 + num10 >> 1) << 24 | (int) (num8 + num9 + num10 + num11 >> 2) << 16 /*0x10*/);
        uint num14 = num9;
        uint num15 = line1 >> 8 & (uint) byte.MaxValue;
        uint num16 = num11;
        uint num17 = line2 >> 8 & (uint) byte.MaxValue;
        uint num18 = num12 | num14 << 8 | num14 + num15 >> 1;
        uint num19 = num13 | num14 + num16 >> 1 << 8 | num14 + num15 + num16 + num17 >> 2;
        lined[indexd + num7 * 2] = num18;
        lined[num2 + num7 * 2] = num19;
        uint num20 = num15;
        uint num21 = line1 & (uint) byte.MaxValue;
        uint num22 = num17;
        uint num23 = line2 & (uint) byte.MaxValue;
        uint num24 = (uint) ((int) num20 << 24 | (int) (num20 + num21 >> 1) << 16 /*0x10*/);
        uint num25 = (uint) ((int) (num20 + num22 >> 1) << 24 | (int) (num20 + num21 + num22 + num23 >> 2) << 16 /*0x10*/);
        line1 = lines[indexd + num7 + 1];
        line2 = lines[index + num7 + 1];
        uint num26 = num21;
        num3 = line1 >> 24 & (uint) byte.MaxValue;
        uint num27 = num23;
        num4 = line2 >> 24 & (uint) byte.MaxValue;
        uint num28 = num24 | num26 << 8 | num26 + num3 >> 1;
        uint num29 = num25 | num26 + num27 >> 1 << 8 | num26 + num3 + num27 + num4 >> 2;
        lined[indexd + num7 * 2 + 1] = num28;
        lined[num2 + num7 * 2 + 1] = num29;
        num5 += 4;
        num6 += 8;
        ++num7;
      }
      while (num5 < num1)
      {
        uint val = num3;
        uint num30 = num4;
        num3 = JBIG2Statics.GetDataByte(lines, indexs + num5 + 1);
        num4 = JBIG2Statics.GetDataByte(lines, index + num5 + 1);
        JBIG2Statics.SetDataByte(ref lined, indexd + num6, val);
        JBIG2Statics.SetDataByte(ref lined, indexd + num6 + 1, (val + num3) / 2U);
        JBIG2Statics.SetDataByte(ref lined, num2 + num6, (val + num30) / 2U);
        JBIG2Statics.SetDataByte(ref lined, num2 + num6 + 1, (val + num3 + num30 + num4) / 4U);
        ++num5;
        num6 += 2;
      }
      uint val1 = num3;
      uint num31 = num4;
      JBIG2Statics.SetDataByte(ref lined, indexd + 2 * num1, val1);
      JBIG2Statics.SetDataByte(ref lined, indexd + 2 * num1 + 1, val1);
      JBIG2Statics.SetDataByte(ref lined, num2 + 2 * num1, (val1 + num31) / 2U);
      JBIG2Statics.SetDataByte(ref lined, num2 + 2 * num1 + 1, (val1 + num31) / 2U);
    }
    else
    {
      int num32 = indexd + wpld;
      uint dataByte = JBIG2Statics.GetDataByte(lines, indexs);
      int num33 = 0;
      int num34 = 0;
      while (num33 < num1)
      {
        uint val = dataByte;
        dataByte = JBIG2Statics.GetDataByte(lines, indexs + num33 + 1);
        JBIG2Statics.SetDataByte(ref lined, indexd + num34, val);
        JBIG2Statics.SetDataByte(ref lined, num32 + num34, val);
        JBIG2Statics.SetDataByte(ref lined, indexd + num34 + 1, (val + dataByte) / 2U);
        JBIG2Statics.SetDataByte(ref lined, num32 + num34 + 1, (val + dataByte) / 2U);
        ++num33;
        num34 += 2;
      }
      uint val2 = dataByte;
      JBIG2Statics.SetDataByte(ref lined, indexd + 2 * num1, val2);
      JBIG2Statics.SetDataByte(ref lined, indexd + 2 * num1 + 1, val2);
      JBIG2Statics.SetDataByte(ref lined, num32 + 2 * num1, val2);
      JBIG2Statics.SetDataByte(ref lined, num32 + 2 * num1 + 1, val2);
    }
  }

  private void PixScaleResolution(Pix pix, float xscale, float yscale)
  {
    if (pix.XRes == 0 || pix.YRes == 0)
      return;
    pix.XRes = (int) ((double) xscale * (double) pix.XRes + 0.5);
    pix.YRes = (int) ((double) yscale * (double) pix.YRes + 0.5);
  }

  private Pix PixRemoveColormap(Pix pixs, int type)
  {
    bool pcolor = false;
    int num1 = 0;
    int num2 = 1;
    int num3 = 2;
    int num4 = 3;
    if (pixs == null)
      throw new NullReferenceException("Pixs not defined");
    if (pixs.Colormap == null)
      return pixs;
    PixColormap colormap = pixs.Colormap;
    if (type != num1 && type != num2 && type != num3 && type != num4)
    {
      Console.WriteLine("Invalid type; converting based on src");
      type = num4;
    }
    int w = pixs.W;
    int h = pixs.H;
    int d = pixs.D;
    switch (d)
    {
      case 1:
      case 2:
      case 4:
      case 8:
        int[] rmap = new int[0];
        int[] gmap = new int[0];
        int[] bmap = new int[0];
        int num5;
        uint pbval = (uint) (num5 = 0);
        uint pgval = (uint) num5;
        uint prval = (uint) num5;
        if (this.PixCmapToArrays(colormap, ref rmap, ref gmap, ref bmap))
        {
          Console.WriteLine("colormap arrays not made");
          return (Pix) null;
        }
        if (d != 1 && type == num1)
        {
          Console.WriteLine("not 1 bpp; can't remove cmap to binary");
          type = num4;
        }
        if (type == num4)
        {
          this.PixCmapHasColor(colormap, ref pcolor);
          type = pcolor ? num3 : (d != 1 ? num2 : num1);
        }
        int n = colormap.N;
        uint[] data1 = pixs.Data;
        int wpl1 = pixs.Wpl;
        Pix pix;
        if (type == num1)
        {
          if ((pix = this.PixCopy((Pix) null, pixs)) == null)
            throw new NullReferenceException("pixd not made");
          this.PixCmapGetColor(colormap, 0, (int) prval, (int) pgval, (int) pbval);
          if (prval == 0U)
            this.PixInvert(pix, pix);
        }
        else if (type == num2)
        {
          if ((pix = this.PixCreate(w, h, 8)) == null)
            throw new Exception("pixd not made");
          pix.XRes = pixs.XRes;
          pix.YRes = pixs.YRes;
          uint[] data2 = pix.Data;
          int wpl2 = pix.Wpl;
          int[] numArray = new int[n];
          for (int index = 0; index < colormap.N; ++index)
            numArray[index] = (rmap[index] + 2 * gmap[index] + bmap[index]) / 4;
          for (int index1 = 0; index1 < h; ++index1)
          {
            int index2 = index1 * wpl1;
            int num6 = index1 * wpl2;
            switch (d)
            {
              case 1:
                int num7 = 0;
                int num8 = 0;
                while (num7 + 31 /*0x1F*/ < w)
                {
                  uint num9 = data1[index2 + num8];
                  for (int index3 = 0; index3 < 4; ++index3)
                  {
                    uint num10 = (uint) (numArray[(IntPtr) (num9 >> 31 /*0x1F*/ & 1U)] << 24 | numArray[(IntPtr) (num9 >> 30 & 1U)] << 16 /*0x10*/ | numArray[(IntPtr) (num9 >> 29 & 1U)] << 8 | numArray[(IntPtr) (num9 >> 28 & 1U)]);
                    data2[num6 + 8 * num8 + 2 * index3] = num10;
                    uint num11 = (uint) (numArray[(IntPtr) (num9 >> 27 & 1U)] << 24 | numArray[(IntPtr) (num9 >> 26 & 1U)] << 16 /*0x10*/ | numArray[(IntPtr) (num9 >> 25 & 1U)] << 8 | numArray[(IntPtr) (num9 >> 24 & 1U)]);
                    data2[num6 + 8 * num8 + 2 * index3 + 1] = num11;
                    num9 <<= 8;
                  }
                  num7 += 32 /*0x20*/;
                  ++num8;
                }
                for (; num7 < w; ++num7)
                {
                  uint dataBit = JBIG2Statics.GetDataBit(data1, index2, index2 + num7);
                  uint val = (uint) numArray[(IntPtr) dataBit];
                  JBIG2Statics.SetDataByte(ref data2, num6 + num7, val);
                }
                break;
              case 2:
                int num12 = 0;
                int num13 = 0;
                while (num12 + 15 < w)
                {
                  uint num14 = data1[index2 + num13];
                  uint num15 = (uint) (numArray[(IntPtr) (num14 >> 30 & 3U)] << 24 | numArray[(IntPtr) (num14 >> 28 & 3U)] << 16 /*0x10*/ | numArray[(IntPtr) (num14 >> 26 & 3U)] << 8 | numArray[(IntPtr) (num14 >> 24 & 3U)]);
                  data2[num6 + 4 * num13] = num15;
                  uint num16 = (uint) (numArray[(IntPtr) (num14 >> 22 & 3U)] << 24 | numArray[(IntPtr) (num14 >> 20 & 3U)] << 16 /*0x10*/ | numArray[(IntPtr) (num14 >> 18 & 3U)] << 8 | numArray[(IntPtr) (num14 >> 16 /*0x10*/ & 3U)]);
                  data2[num6 + 4 * num13 + 1] = num16;
                  uint num17 = (uint) (numArray[(IntPtr) (num14 >> 14 & 3U)] << 24 | numArray[(IntPtr) (num14 >> 12 & 3U)] << 16 /*0x10*/ | numArray[(IntPtr) (num14 >> 10 & 3U)] << 8 | numArray[(IntPtr) (num14 >> 8 & 3U)]);
                  data2[num6 + 4 * num13 + 2] = num17;
                  uint num18 = (uint) (numArray[(IntPtr) (num14 >> 6 & 3U)] << 24 | numArray[(IntPtr) (num14 >> 4 & 3U)] << 16 /*0x10*/ | numArray[(IntPtr) (num14 >> 2 & 3U)] << 8 | numArray[(IntPtr) (num14 & 3U)]);
                  data2[num6 + 4 * num13 + 3] = num18;
                  num12 += 16 /*0x10*/;
                  ++num13;
                }
                for (; num12 < w; ++num12)
                {
                  uint dataDibit = JBIG2Statics.GetDataDibit(data1[index2], index2 + num12);
                  uint val = (uint) numArray[(IntPtr) dataDibit];
                  JBIG2Statics.SetDataByte(ref data2, num6 + num12, val);
                }
                break;
              case 4:
                int num19 = 0;
                int num20 = 0;
                while (num19 + 7 < w)
                {
                  uint num21 = data1[index2 + num20];
                  uint num22 = (uint) (numArray[(IntPtr) (num21 >> 28 & 15U)] << 24 | numArray[(IntPtr) (num21 >> 24 & 15U)] << 16 /*0x10*/ | numArray[(IntPtr) (num21 >> 20 & 15U)] << 8 | numArray[(IntPtr) (num21 >> 16 /*0x10*/ & 15U)]);
                  data2[num6 + 2 * num20] = num22;
                  uint num23 = (uint) (numArray[(IntPtr) (num21 >> 12 & 15U)] << 24 | numArray[(IntPtr) (num21 >> 8 & 15U)] << 16 /*0x10*/ | numArray[(IntPtr) (num21 >> 4 & 15U)] << 8 | numArray[(IntPtr) (num21 & 15U)]);
                  data2[num6 + 2 * num20 + 1] = num23;
                  num19 += 8;
                  ++num20;
                }
                for (; num19 < w; ++num19)
                {
                  uint dataQbit = JBIG2Statics.GetDataQbit(data1[index2], index2 + num19);
                  uint val = (uint) numArray[(IntPtr) dataQbit];
                  JBIG2Statics.SetDataByte(ref data2, num6 + num19, val);
                }
                break;
              case 8:
                int num24 = 0;
                int num25 = 0;
                while (num24 + 3 < w)
                {
                  uint num26 = data1[index2 + num25];
                  uint num27 = (uint) (numArray[(IntPtr) (num26 >> 24 & (uint) byte.MaxValue)] << 24 | numArray[(IntPtr) (num26 >> 16 /*0x10*/ & (uint) byte.MaxValue)] << 16 /*0x10*/ | numArray[(IntPtr) (num26 >> 8 & (uint) byte.MaxValue)] << 8 | numArray[(IntPtr) (num26 & (uint) byte.MaxValue)]);
                  data2[num6 + num25] = num27;
                  num24 += 4;
                  ++num25;
                }
                for (; num24 < w; ++num24)
                {
                  uint dataByte = JBIG2Statics.GetDataByte(data1, index2 + num24);
                  uint val = (uint) numArray[(IntPtr) dataByte];
                  JBIG2Statics.SetDataByte(ref data2, num6 + num24, val);
                }
                break;
              default:
                return (Pix) null;
            }
          }
          pix.Data = data2;
        }
        else
        {
          if ((pix = this.PixCreate(w, h, 32 /*0x20*/)) == null)
            throw new Exception("Pixd not made");
          pix.XRes = pixs.XRes;
          pix.YRes = pixs.YRes;
          uint[] data3 = pix.Data;
          int wpl3 = pix.Wpl;
          uint[] numArray = new uint[n];
          for (int index = 0; index < n; ++index)
            this.ComposeRGBPixel(rmap[index], gmap[index], bmap[index], (int) numArray[index]);
          for (int index4 = 0; index4 < h; ++index4)
          {
            int index5 = index4 * wpl1;
            int num28 = index4 * wpl3;
            for (int index6 = 0; index6 < w; ++index6)
            {
              uint index7;
              switch (d)
              {
                case 1:
                  index7 = JBIG2Statics.GetDataBit(data1, index5, index5 + index6);
                  break;
                case 2:
                  index7 = JBIG2Statics.GetDataDibit(data1[index5], index5 + index6);
                  break;
                case 4:
                  index7 = JBIG2Statics.GetDataQbit(data1[index5], index5 + index6);
                  break;
                case 8:
                  index7 = JBIG2Statics.GetDataByte(data1, index5 + index6);
                  break;
                default:
                  return (Pix) null;
              }
              if ((long) index7 >= (long) n)
                Console.WriteLine("pixel value out of bounds");
              else
                data3[num28 + index6] = numArray[(IntPtr) index7];
            }
          }
          pix.Data = data3;
        }
        bmap = (int[]) null;
        return pix;
      default:
        throw new Exception("Pixs must be {1,2,4,8} bpp");
    }
  }

  private Pix PixInvert(Pix pixd, Pix pixs)
  {
    int op = 20;
    if ((pixd = this.PixCopy(pixd, pixs)) == null)
      throw new NullReferenceException("pixd not made");
    this.PixRasterop(pixd, 0, 0, pixd.W, pixd.H, JBIG2Statics.PixNot(op), (Pix) null, 0, 0);
    return pixd;
  }

  private void PixRasterop(
    Pix pixd,
    int dx,
    int dy,
    int dw,
    int dh,
    int op,
    Pix pixs,
    int sx,
    int sy)
  {
    if (op == JBIG2Statics.PixDst)
      return;
    int d = pixd.D;
    if (op == JBIG2Statics.PixClr || op == JBIG2Statics.PixSet || op == (JBIG2Statics.PixDst ^ 30))
    {
      uint[] data = pixd.Data;
      this.RasteropUniLow(ref data, pixd.W, pixd.H, d, pixd.Wpl, dx, dy, dw, dh, op);
      pixd.Data = data;
    }
    else
    {
      if (d != pixs.D)
        return;
      uint[] data = pixd.Data;
      this.RasteropLow(ref data, pixd.W, pixd.H, d, pixd.Wpl, dx, dy, dw, dh, op, pixs.Data, pixs.W, pixs.H, pixs.Wpl, sx, sy);
      pixd.Data = data;
    }
  }

  private void RasteropLow(
    ref uint[] datad,
    int dpixw,
    int dpixh,
    int depth,
    int dwpl,
    int dx,
    int dy,
    int dw,
    int dh,
    int op,
    uint[] datas,
    int spixw,
    int spixh,
    int swpl,
    int sx,
    int sy)
  {
    if (depth != 1)
    {
      dpixw *= depth;
      dx *= depth;
      dw *= depth;
      spixw *= depth;
      sx *= depth;
    }
    if (dx < 0)
    {
      sx -= dx;
      dw += dx;
      dx = 0;
    }
    if (sx < 0)
    {
      dx -= sx;
      dw += sx;
      sx = 0;
    }
    int num1 = dx + dw - dpixw;
    if (num1 > 0)
      dw -= num1;
    int num2 = sx + dw - spixw;
    if (num2 > 0)
      dw -= num2;
    if (dy < 0)
    {
      sy -= dy;
      dh += dy;
      dy = 0;
    }
    if (sy < 0)
    {
      dy -= sy;
      dh += sy;
      sy = 0;
    }
    int num3 = dy + dh - dpixh;
    if (num3 > 0)
      dh -= num3;
    int num4 = sy + dh - spixh;
    if (num4 > 0)
      dh -= num4;
    if (dw <= 0 || dh <= 0)
      return;
    if ((dx & 31 /*0x1F*/) == 0 && (sx & 31 /*0x1F*/) == 0)
      this.RasteropWordAlignedLow(ref datad, dwpl, dx, dy, dw, dh, op, datas, swpl, sx, sy);
    else if ((dx & 31 /*0x1F*/) == (sx & 31 /*0x1F*/))
      this.RasteropVAlignedLow(ref datad, dwpl, dx, dy, dw, dh, op, datas, swpl, sx, sy);
    else
      this.RasteropGeneralLow(ref datad, dwpl, dx, dy, dw, dh, op, datas, swpl, sx, sy);
  }

  private void RasteropGeneralLow(
    ref uint[] datad,
    int dwpl,
    int dx,
    int dy,
    int dw,
    int dh,
    int op,
    uint[] datas,
    int swpl,
    int sx,
    int sy)
  {
    uint m1 = 0;
    uint m2 = 0;
    int num1 = 0;
    bool flag1 = false;
    bool flag2 = false;
    int num2 = 0;
    int num3 = 0;
    int num4 = 0;
    int num5 = 0;
    int num6 = 0;
    int num7 = 0;
    int num8 = (sx & 31 /*0x1F*/) != 0 ? 32 /*0x20*/ - (sx & 31 /*0x1F*/) : 0;
    int num9 = (dx & 31 /*0x1F*/) != 0 ? 32 /*0x20*/ - (dx & 31 /*0x1F*/) : 0;
    int index1;
    int num10;
    uint m3;
    if (num8 == 0 && num9 == 0)
    {
      index1 = 0;
      num10 = 0;
      m3 = JBIG2Statics.RightMask[0];
    }
    else
    {
      index1 = num9 <= num8 ? 32 /*0x20*/ - (num8 - num9) : num9 - num8;
      num10 = 32 /*0x20*/ - index1;
      m3 = JBIG2Statics.RightMask[index1];
    }
    int num11;
    int index2;
    if ((dx & 31 /*0x1F*/) == 0)
    {
      num11 = 0;
      index2 = 0;
    }
    else
    {
      num11 = 1;
      index2 = 32 /*0x20*/ - (dx & 31 /*0x1F*/);
      m1 = JBIG2Statics.RightMask[index2];
      num2 = dwpl * dy + (dx >> 5);
      num3 = swpl * sy + (sx >> 5);
      int num12 = 32 /*0x20*/ - (sx & 31 /*0x1F*/);
      if (index2 > num12)
      {
        num1 = JBIG2Statics.ShiftLeft;
        flag1 = dw >= num8;
      }
      else
        num1 = JBIG2Statics.ShiftRight;
    }
    int num13;
    if (dw >= index2)
    {
      num13 = 0;
    }
    else
    {
      num13 = 1;
      m1 &= JBIG2Statics.LeftMask[32 /*0x20*/ - index2 + dw];
    }
    bool flag3;
    int num14;
    if (num13 == 1)
    {
      flag3 = false;
      num14 = 0;
    }
    else
    {
      num14 = dw - index2 >> 5;
      if (num14 == 0)
      {
        flag3 = false;
      }
      else
      {
        flag3 = true;
        num4 = dwpl * dy + (dx + num9 >> 5);
        num5 = swpl * sy + (sx + num9 >> 5);
      }
    }
    int index3 = dx + dw & 31 /*0x1F*/;
    int num15;
    if (num13 == 1 || index3 == 0)
    {
      num15 = 0;
    }
    else
    {
      num15 = 1;
      m2 = JBIG2Statics.LeftMask[index3];
      num6 = dwpl * dy + (dx + num9 >> 5) + num14;
      num7 = swpl * sy + (sx + num9 >> 5) + num14;
      flag2 = index3 > num10;
    }
    if (op == JBIG2Statics.PixSrc)
    {
      if (num11 > 0)
      {
        int index4 = num3;
        int index5 = num2;
        for (int index6 = 0; index6 < dh; ++index6)
        {
          uint num16;
          if (num1 == JBIG2Statics.ShiftLeft)
          {
            num16 = datas[index4] << index1;
            if (flag1)
              num16 = this.COMBINE_PARTIAL(num16, datas[index4 + 1] >> num10, m3);
          }
          else
            num16 = datas[index4] >> num10;
          datad[index5] = this.COMBINE_PARTIAL(datad[index5], num16, m1);
          index5 += dwpl;
          index4 += swpl;
        }
      }
      if (flag3)
      {
        int num17 = num5;
        int num18 = num4;
        for (int index7 = 0; index7 < dh; ++index7)
        {
          for (int index8 = 0; index8 < num14; ++index8)
          {
            uint num19 = this.COMBINE_PARTIAL(datas[num17 + index8] << index1, datas[num17 + index8 + 1] >> num10, m3);
            datad[num18 + index8] = num19;
          }
          num18 += dwpl;
          num17 += swpl;
        }
      }
      if (num15 <= 0)
        return;
      int index9 = num7;
      int index10 = num6;
      for (int index11 = 0; index11 < dh; ++index11)
      {
        uint num20 = datas[index9] << index1;
        if (flag2)
          num20 = this.COMBINE_PARTIAL(num20, datas[index9 + 1] >> num10, m3);
        datad[index10] = this.COMBINE_PARTIAL(datad[index10], num20, m2);
        index10 += dwpl;
        index9 += swpl;
      }
    }
    else if (op == JBIG2Statics.PixNot(JBIG2Statics.PixSrc))
    {
      if (num11 > 0)
      {
        int index12 = num3;
        int index13 = num2;
        for (int index14 = 0; index14 < dh; ++index14)
        {
          uint d;
          if (num1 == JBIG2Statics.ShiftLeft)
          {
            d = datas[index12] << index1;
            if (flag1)
              d = this.COMBINE_PARTIAL(d, datas[index12 + 1] >> num10, m3);
          }
          else
            d = datas[index12] >> num10;
          datad[index13] = this.COMBINE_PARTIAL(datad[index13], ~d, m1);
          index13 += dwpl;
          index12 += swpl;
        }
      }
      if (flag3)
      {
        int num21 = num5;
        int num22 = num4;
        for (int index15 = 0; index15 < dh; ++index15)
        {
          for (int index16 = 0; index16 < num14; ++index16)
          {
            uint num23 = this.COMBINE_PARTIAL(datas[num21 + index16] << index1, datas[num21 + index16 + 1] >> num10, m3);
            datad[index16] = ~num23;
          }
          num22 += dwpl;
          num21 += swpl;
        }
      }
      if (num15 <= 0)
        return;
      int index17 = num7;
      int index18 = num6;
      for (int index19 = 0; index19 < dh; ++index19)
      {
        uint d = datas[index17] << index1;
        if (flag2)
          d = this.COMBINE_PARTIAL(d, datas[index17 + 1] >> num10, m3);
        datad[index18] = this.COMBINE_PARTIAL(datad[index18], ~d, m2);
        index18 += dwpl;
        index17 += swpl;
      }
    }
    else if (op == (JBIG2Statics.PixSrc | JBIG2Statics.PixDst))
    {
      if (num11 > 0)
      {
        int index20 = num3;
        int index21 = num2;
        for (int index22 = 0; index22 < dh; ++index22)
        {
          uint d;
          if (num1 == JBIG2Statics.ShiftLeft)
          {
            d = datas[index20] << index1;
            if (flag1)
              d = this.COMBINE_PARTIAL(d, datas[index20 + 1] >> num10, m3);
          }
          else
            d = datas[index20] >> num10;
          datad[index21] = this.COMBINE_PARTIAL(datad[index21], d | datad[index21], m1);
          index21 += dwpl;
          index20 += swpl;
        }
      }
      if (flag3)
      {
        int num24 = num5;
        int num25 = num4;
        for (int index23 = 0; index23 < dh; ++index23)
        {
          for (int index24 = 0; index24 < num14; ++index24)
          {
            uint num26 = this.COMBINE_PARTIAL(datas[num24 + index24] << index1, datas[num24 + index24 + 1] >> num10, m3);
            datad[index24] |= num26;
          }
          num25 += dwpl;
          num24 += swpl;
        }
      }
      if (num15 <= 0)
        return;
      int index25 = num7;
      int index26 = num6;
      for (int index27 = 0; index27 < dh; ++index27)
      {
        uint d = datas[index25] << index1;
        if (flag2)
          d = this.COMBINE_PARTIAL(d, datas[index25 + 1] >> num10, m3);
        datad[index26] = this.COMBINE_PARTIAL(datad[index26], d | datad[index26], m2);
        index26 += dwpl;
        index25 += swpl;
      }
    }
    else if (op == (JBIG2Statics.PixSrc & JBIG2Statics.PixDst))
    {
      if (num11 > 0)
      {
        int index28 = num3;
        int index29 = num2;
        for (int index30 = 0; index30 < dh; ++index30)
        {
          uint d;
          if (num1 == JBIG2Statics.ShiftLeft)
          {
            d = datas[index28] << index1;
            if (flag1)
              d = this.COMBINE_PARTIAL(d, datas[index28] + 1U >> num10, m3);
          }
          else
            d = datas[index28] >> num10;
          datad[index29] = this.COMBINE_PARTIAL(datad[index29], d & datad[index29], m1);
          index29 += dwpl;
          index28 += swpl;
        }
      }
      if (flag3)
      {
        int num27 = num5;
        int num28 = num4;
        for (int index31 = 0; index31 < dh; ++index31)
        {
          for (int index32 = 0; index32 < num14; ++index32)
          {
            uint num29 = this.COMBINE_PARTIAL(datas[num27 + index32] << index1, datas[num27 + index32 + 1] >> num10, m3);
            datad[index32] &= num29;
          }
          num28 += dwpl;
          num27 += swpl;
        }
      }
      if (num15 <= 0)
        return;
      int index33 = num7;
      int index34 = num6;
      for (int index35 = 0; index35 < dh; ++index35)
      {
        uint d = datas[index33] << index1;
        if (flag2)
          d = this.COMBINE_PARTIAL(d, datas[index33 + 1] >> num10, m3);
        datad[index34] = this.COMBINE_PARTIAL(datad[index34], d & datad[index34], m2);
        index34 += dwpl;
        index33 += swpl;
      }
    }
    else if (op == (JBIG2Statics.PixSrc ^ JBIG2Statics.PixDst))
    {
      if (num11 > 0)
      {
        int index36 = num3;
        int index37 = num2;
        for (int index38 = 0; index38 < dh; ++index38)
        {
          uint d;
          if (num1 == JBIG2Statics.ShiftLeft)
          {
            d = datas[index36] << index1;
            if (flag1)
              d = this.COMBINE_PARTIAL(d, datas[index36 + 1] >> num10, m3);
          }
          else
            d = datas[index36] >> num10;
          datad[index37] = this.COMBINE_PARTIAL(datad[index37], d ^ datad[index37], m1);
          index37 += dwpl;
          index36 += swpl;
        }
      }
      if (flag3)
      {
        int num30 = num5;
        int num31 = num4;
        for (int index39 = 0; index39 < dh; ++index39)
        {
          for (int index40 = 0; index40 < num14; ++index40)
          {
            uint num32 = this.COMBINE_PARTIAL(datas[num30 + index40] << index1, datas[num30 + index40 + 1] >> num10, m3);
            datad[num31 + index40] ^= num32;
          }
          num31 += dwpl;
          num30 += swpl;
        }
      }
      if (num15 <= 0)
        return;
      int index41 = num7;
      int index42 = num6;
      for (int index43 = 0; index43 < dh; ++index43)
      {
        uint d = datas[index41] << index1;
        if (flag2)
          d = this.COMBINE_PARTIAL(d, datas[index41 + 1] >> num10, m3);
        datad[index42] = this.COMBINE_PARTIAL(datad[index42], d ^ datad[index42], m2);
        index42 += dwpl;
        index41 += swpl;
      }
    }
    else if (op == (JBIG2Statics.PixNot(JBIG2Statics.PixSrc) | JBIG2Statics.PixDst))
    {
      if (num11 > 0)
      {
        int index44 = num3;
        int index45 = num2;
        for (int index46 = 0; index46 < dh; ++index46)
        {
          uint d;
          if (num1 == JBIG2Statics.ShiftLeft)
          {
            d = datas[index44] << index1;
            if (flag1)
              d = this.COMBINE_PARTIAL(d, datas[index44 + 1] >> num10, m3);
          }
          else
            d = datas[index44] >> num10;
          datad[index45] = this.COMBINE_PARTIAL(datad[index45], ~d | datad[index45], m1);
          index45 += dwpl;
          index44 += swpl;
        }
      }
      if (flag3)
      {
        int num33 = num5;
        int num34 = num4;
        for (int index47 = 0; index47 < dh; ++index47)
        {
          for (int index48 = 0; index48 < num14; ++index48)
          {
            uint num35 = this.COMBINE_PARTIAL(datas[num33 + index48] << index1, datas[num33 + index48 + 1] >> num10, m3);
            datad[index48] |= ~num35;
          }
          num34 += dwpl;
          num33 += swpl;
        }
      }
      if (num15 <= 0)
        return;
      int index49 = num7;
      int index50 = num6;
      for (int index51 = 0; index51 < dh; ++index51)
      {
        uint d = datas[index49] << index1;
        if (flag2)
          d = this.COMBINE_PARTIAL(d, datas[index49 + 1] >> num10, m3);
        datad[index50] = this.COMBINE_PARTIAL(datad[index50], ~d | datad[index50], m2);
        index50 += dwpl;
        index49 += swpl;
      }
    }
    else if (op == (JBIG2Statics.PixNot(JBIG2Statics.PixSrc) & JBIG2Statics.PixDst))
    {
      if (num11 > 0)
      {
        int index52 = num3;
        int index53 = num2;
        for (int index54 = 0; index54 < dh; ++index54)
        {
          uint d;
          if (num1 == JBIG2Statics.ShiftLeft)
          {
            d = datas[index52] << index1;
            if (flag1)
              d = this.COMBINE_PARTIAL(d, datas[index52 + 1] >> num10, m3);
          }
          else
            d = datas[index52] >> num10;
          datad[index53] = this.COMBINE_PARTIAL(datad[index53], ~d & datad[index53], m1);
          index53 += dwpl;
          index52 += swpl;
        }
      }
      if (flag3)
      {
        int num36 = num5;
        int num37 = num4;
        for (int index55 = 0; index55 < dh; ++index55)
        {
          for (int index56 = 0; index56 < num14; ++index56)
          {
            uint num38 = this.COMBINE_PARTIAL(datas[num36 + index56] << index1, datas[num36 + index56 + 1] >> num10, m3);
            datad[index56] &= ~num38;
          }
          num37 += dwpl;
          num36 += swpl;
        }
      }
      if (num15 <= 0)
        return;
      int index57 = num7;
      int index58 = num6;
      for (int index59 = 0; index59 < dh; ++index59)
      {
        uint d = datas[index57] << index1;
        if (flag2)
          d = this.COMBINE_PARTIAL(d, datas[index57 + 1] >> num10, m3);
        datad[index58] = this.COMBINE_PARTIAL(datad[index58], ~d & datad[index58], m2);
        index58 += dwpl;
        index57 += swpl;
      }
    }
    else if (op == (JBIG2Statics.PixSrc | JBIG2Statics.PixNot(JBIG2Statics.PixDst)))
    {
      if (num11 > 0)
      {
        int index60 = num3;
        int index61 = num2;
        for (int index62 = 0; index62 < dh; ++index62)
        {
          uint d;
          if (num1 == JBIG2Statics.ShiftLeft)
          {
            d = datas[index60] << index1;
            if (flag1)
              d = this.COMBINE_PARTIAL(d, datas[index60 + 1] >> num10, m3);
          }
          else
            d = datas[index60] >> num10;
          datad[index61] = this.COMBINE_PARTIAL(datad[index61], d | ~datad[index61], m1);
          index61 += dwpl;
          index60 += swpl;
        }
      }
      if (flag3)
      {
        int num39 = num5;
        int num40 = num4;
        for (int index63 = 0; index63 < dh; ++index63)
        {
          for (int index64 = 0; index64 < num14; ++index64)
          {
            uint num41 = this.COMBINE_PARTIAL(datas[num39 + index64] << index1, datas[num39 + index64 + 1] >> num10, m3);
            datad[num40 + index64] = num41 | ~datad[num40 + index64];
          }
          num40 += dwpl;
          num39 += swpl;
        }
      }
      if (num15 <= 0)
        return;
      int index65 = num7;
      int index66 = num6;
      for (int index67 = 0; index67 < dh; ++index67)
      {
        uint d = datas[index65] << index1;
        if (flag2)
          d = this.COMBINE_PARTIAL(d, datas[index65 + 1] >> num10, m3);
        datad[index66] = this.COMBINE_PARTIAL(datad[index66], d | ~datad[index66], m2);
        index66 += dwpl;
        index65 += swpl;
      }
    }
    else if (op == (JBIG2Statics.PixSrc & JBIG2Statics.PixNot(JBIG2Statics.PixDst)))
    {
      if (num11 > 0)
      {
        int index68 = num3;
        int index69 = num2;
        for (int index70 = 0; index70 < dh; ++index70)
        {
          uint d;
          if (num1 == JBIG2Statics.ShiftLeft)
          {
            d = datas[index68] << index1;
            if (flag1)
              d = this.COMBINE_PARTIAL(d, datas[index68 + 1] >> num10, m3);
          }
          else
            d = datas[index68] >> num10;
          datad[index69] = this.COMBINE_PARTIAL(datad[index69], d & ~datad[index69], m1);
          index69 += dwpl;
          index68 += swpl;
        }
      }
      if (flag3)
      {
        int num42 = num5;
        int num43 = num4;
        for (int index71 = 0; index71 < dh; ++index71)
        {
          for (int index72 = 0; index72 < num14; ++index72)
          {
            uint num44 = this.COMBINE_PARTIAL(datas[num42 + index72] << index1, datas[num42 + index72 + 1] >> num10, m3);
            datad[num43 + index72] = num44 & ~datad[num43 + index72];
          }
          num43 += dwpl;
          num42 += swpl;
        }
      }
      if (num15 <= 0)
        return;
      int index73 = num7;
      int index74 = num6;
      for (int index75 = 0; index75 < dh; ++index75)
      {
        uint d = datas[index73] << index1;
        if (flag2)
          d = this.COMBINE_PARTIAL(d, datas[index73 + 1] >> num10, m3);
        datad[index74] = this.COMBINE_PARTIAL(datad[index74], d & ~datad[index74], m2);
        index74 += dwpl;
        index73 += swpl;
      }
    }
    else if (op == JBIG2Statics.PixNot(JBIG2Statics.PixSrc | JBIG2Statics.PixDst))
    {
      if (num11 > 0)
      {
        int index76 = num3;
        int index77 = num2;
        for (int index78 = 0; index78 < dh; ++index78)
        {
          uint d;
          if (num1 == JBIG2Statics.ShiftLeft)
          {
            d = datas[index76] << index1;
            if (flag1)
              d = this.COMBINE_PARTIAL(d, datas[index76 + 1] >> num10, m3);
          }
          else
            d = datas[index76] >> num10;
          datad[index77] = this.COMBINE_PARTIAL(datad[index77], (uint) ~((int) d | (int) datad[index77]), m1);
          index77 += dwpl;
          index76 += swpl;
        }
      }
      if (flag3)
      {
        int num45 = num5;
        int num46 = num4;
        for (int index79 = 0; index79 < dh; ++index79)
        {
          for (int index80 = 0; index80 < num14; ++index80)
          {
            uint num47 = this.COMBINE_PARTIAL(datas[num45 + index80] << index1, datas[num45 + index80 + 1] >> num10, m3);
            datad[num46 + index80] = (uint) ~((int) num47 | (int) datad[num46 + index80]);
          }
          num46 += dwpl;
          num45 += swpl;
        }
      }
      if (num15 <= 0)
        return;
      int index81 = num7;
      int index82 = num6;
      for (int index83 = 0; index83 < dh; ++index83)
      {
        uint d = datas[index81] << index1;
        if (flag2)
          d = this.COMBINE_PARTIAL(d, datas[index81 + 1] >> num10, m3);
        datad[index82] = this.COMBINE_PARTIAL(datad[index82], (uint) ~((int) d | (int) datad[index82]), m2);
        index82 += dwpl;
        index81 += swpl;
      }
    }
    else if (op == JBIG2Statics.PixNot(JBIG2Statics.PixSrc & JBIG2Statics.PixDst))
    {
      if (num11 > 0)
      {
        int index84 = num3;
        int index85 = num2;
        for (int index86 = 0; index86 < dh; ++index86)
        {
          uint d;
          if (num1 == JBIG2Statics.ShiftLeft)
          {
            d = datas[index84] << index1;
            if (flag1)
              d = this.COMBINE_PARTIAL(d, datas[index84 + 1] >> num10, m3);
          }
          else
            d = datas[index84] >> num10;
          datad[index85] = this.COMBINE_PARTIAL(datad[index85], (uint) ~((int) d & (int) datad[index85]), m1);
          index85 += dwpl;
          index84 += swpl;
        }
      }
      if (flag3)
      {
        int num48 = num5;
        int num49 = num4;
        for (int index87 = 0; index87 < dh; ++index87)
        {
          for (int index88 = 0; index88 < num14; ++index88)
          {
            uint num50 = this.COMBINE_PARTIAL(datas[num48 + index88] << index1, datas[num48 + index88 + 1] >> num10, m3);
            datad[num49 + index88] = (uint) ~((int) num50 & (int) datad[num49 + index88]);
          }
          num49 += dwpl;
          num48 += swpl;
        }
      }
      if (num15 <= 0)
        return;
      int index89 = num7;
      int index90 = num6;
      for (int index91 = 0; index91 < dh; ++index91)
      {
        uint d = datas[index89] << index1;
        if (flag2)
          d = this.COMBINE_PARTIAL(d, datas[index89 + 1] >> num10, m3);
        datad[index90] = this.COMBINE_PARTIAL(datad[index90], (uint) ~((int) d & (int) datad[index90]), m2);
        index90 += dwpl;
        index89 += swpl;
      }
    }
    else
    {
      if (op != JBIG2Statics.PixNot(JBIG2Statics.PixSrc ^ JBIG2Statics.PixDst))
        return;
      if (num11 > 0)
      {
        int index92 = num3;
        int index93 = num2;
        for (int index94 = 0; index94 < dh; ++index94)
        {
          uint d;
          if (num1 == JBIG2Statics.ShiftLeft)
          {
            d = datas[index92] << index1;
            if (flag1)
              d = this.COMBINE_PARTIAL(d, datas[index92 + 1] >> num10, m3);
          }
          else
            d = datas[index92] >> num10;
          datad[index93] = this.COMBINE_PARTIAL(datad[index93], (uint) ~((int) d ^ (int) datad[index93]), m1);
          index93 += dwpl;
          index92 += swpl;
        }
      }
      if (flag3)
      {
        int num51 = num5;
        int num52 = num4;
        for (int index95 = 0; index95 < dh; ++index95)
        {
          for (int index96 = 0; index96 < num14; ++index96)
          {
            uint num53 = this.COMBINE_PARTIAL(datas[num51 + index96] << index1, datas[num51 + index96 + 1] >> num10, m3);
            datad[num52 + index96] = (uint) ~((int) num53 ^ (int) datad[num52 + index96]);
          }
          num52 += dwpl;
          num51 += swpl;
        }
      }
      if (num15 <= 0)
        return;
      int index97 = num7;
      int index98 = num6;
      int num54;
      for (int index99 = 0; index99 < dh; index99 = num54 + 1)
      {
        for (num54 = 0; num54 < dh; ++num54)
        {
          uint d = datas[index97] << index1;
          if (flag2)
            d = this.COMBINE_PARTIAL(d, datas[index97 + 1] >> num10, m3);
          datad[index98] = this.COMBINE_PARTIAL(datad[index98], (uint) ~((int) d ^ (int) datad[index98]), m2);
          index98 += dwpl;
          index97 += swpl;
        }
      }
    }
  }

  private void RasteropVAlignedLow(
    ref uint[] datad,
    int dwpl,
    int dx,
    int dy,
    int dw,
    int dh,
    int op,
    uint[] datas,
    int swpl,
    int sx,
    int sy)
  {
    uint m1 = 0;
    uint m2 = 0;
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    int num4 = 0;
    int num5 = 0;
    int num6 = 0;
    int num7;
    int index1;
    if ((dx & 31 /*0x1F*/) == 0)
    {
      num7 = 0;
      index1 = 0;
    }
    else
    {
      num7 = 1;
      index1 = 32 /*0x20*/ - (dx & 31 /*0x1F*/);
      m1 = JBIG2Statics.RightMask[index1];
      num3 = dwpl * dy + (dx >> 5);
      num4 = swpl * sy + (sx >> 5);
    }
    int num8;
    if (dw >= index1)
    {
      num8 = 0;
    }
    else
    {
      num8 = 1;
      m1 &= JBIG2Statics.LeftMask[32 /*0x20*/ - index1 + dw];
    }
    int num9;
    int num10;
    if (num8 == 1)
    {
      num9 = 0;
      num10 = 0;
    }
    else
    {
      num10 = dw - index1 >> 5;
      if (num10 == 0)
      {
        num9 = 0;
      }
      else
      {
        num9 = 1;
        if (num7 > 0)
        {
          num1 = num3 + 1;
          num2 = num4 + 1;
        }
        else
        {
          num1 = dwpl * dy + (dx >> 5);
          num2 = swpl * sy + (sx >> 5);
        }
      }
    }
    int index2 = dx + dw & 31 /*0x1F*/;
    int num11;
    if (num8 == 1 || index2 == 0)
    {
      num11 = 0;
    }
    else
    {
      num11 = 1;
      m2 = JBIG2Statics.LeftMask[index2];
      if (num7 > 0)
      {
        num5 = num3 + 1 + num10;
        num6 = num4 + 1 + num10;
      }
      else
      {
        num5 = dwpl * dy + (dx >> 5) + num10;
        num6 = swpl * sy + (sx >> 5) + num10;
      }
    }
    if (op == JBIG2Statics.PixSrc)
    {
      if (num7 > 0)
      {
        int index3 = num4;
        int index4 = num3;
        for (int index5 = 0; index5 < dh; ++index5)
        {
          datad[index4] = this.COMBINE_PARTIAL(datad[index4], datas[index3], m1);
          index4 += dwpl;
          index3 += swpl;
        }
      }
      if (num9 > 0)
      {
        int num12 = num2;
        int num13 = num1;
        for (int index6 = 0; index6 < dh; ++index6)
        {
          for (int index7 = 0; index7 < num10; ++index7)
            datad[num13 + index7] = datas[num12 + index7];
          num13 += dwpl;
          num12 += swpl;
        }
      }
      if (num11 <= 0)
        return;
      int index8 = num6;
      int index9 = num5;
      for (int index10 = 0; index10 < dh; ++index10)
      {
        datad[index9] = this.COMBINE_PARTIAL(datad[index9], datas[index8], m2);
        index9 += dwpl;
        index8 += swpl;
      }
    }
    else if (op == JBIG2Statics.PixNot(JBIG2Statics.PixSrc))
    {
      if (num7 > 0)
      {
        int index11 = num4;
        int index12 = num3;
        for (int index13 = 0; index13 < dh; ++index13)
        {
          datad[index12] = this.COMBINE_PARTIAL(datad[index12], ~datas[index11], m1);
          index12 += dwpl;
          index11 += swpl;
        }
      }
      if (num9 > 0)
      {
        int num14 = num2;
        int num15 = num1;
        for (int index14 = 0; index14 < dh; ++index14)
        {
          for (int index15 = 0; index15 < num10; ++index15)
            datad[num15 + index15] = ~datas[num14 + index15];
          num15 += dwpl;
          num14 += swpl;
        }
      }
      if (num11 <= 0)
        return;
      int index16 = num6;
      int index17 = num5;
      for (int index18 = 0; index18 < dh; ++index18)
      {
        datad[index17] = this.COMBINE_PARTIAL(datad[index17], ~datas[index16], m2);
        index17 += dwpl;
        index16 += swpl;
      }
    }
    else if (op == (JBIG2Statics.PixSrc | JBIG2Statics.PixDst))
    {
      if (num7 > 0)
      {
        int index19 = num4;
        int index20 = num3;
        for (int index21 = 0; index21 < dh; ++index21)
        {
          datad[index20] = this.COMBINE_PARTIAL(datad[index20], datas[index19] | datad[index20], m1);
          index20 += dwpl;
          index19 += swpl;
        }
      }
      if (num9 > 0)
      {
        int num16 = num2;
        int num17 = num1;
        for (int index22 = 0; index22 < dh; ++index22)
        {
          for (int index23 = 0; index23 < num10; ++index23)
            datad[num17 + index23] |= datas[num16 + index23];
          num17 += dwpl;
          num16 += swpl;
        }
      }
      if (num11 <= 0)
        return;
      int index24 = num6;
      int index25 = num5;
      for (int index26 = 0; index26 < dh; ++index26)
      {
        datad[index25] = this.COMBINE_PARTIAL(datad[index25], datas[index24] | datad[index25], m2);
        index25 += dwpl;
        index24 += swpl;
      }
    }
    else if (op == (JBIG2Statics.PixSrc & JBIG2Statics.PixDst))
    {
      if (num7 > 0)
      {
        int index27 = num4;
        int index28 = num3;
        for (int index29 = 0; index29 < dh; ++index29)
        {
          datad[index28] = this.COMBINE_PARTIAL(datad[index28], datas[index27] & datad[index28], m1);
          index28 += dwpl;
          index27 += swpl;
        }
      }
      if (num9 > 0)
      {
        int num18 = num2;
        int num19 = num1;
        for (int index30 = 0; index30 < dh; ++index30)
        {
          for (int index31 = 0; index31 < num10; ++index31)
            datad[num19 + index31] &= datas[num18 + index31];
          num19 += dwpl;
          num18 += swpl;
        }
      }
      if (num11 <= 0)
        return;
      int index32 = num6;
      int index33 = num5;
      for (int index34 = 0; index34 < dh; ++index34)
      {
        datad[index33] = this.COMBINE_PARTIAL(datad[index33], datas[index32] & datad[index33], m2);
        index33 += dwpl;
        index32 += swpl;
      }
    }
    else if (op == (JBIG2Statics.PixSrc ^ JBIG2Statics.PixDst))
    {
      if (num7 > 0)
      {
        int index35 = num4;
        int index36 = num3;
        for (int index37 = 0; index37 < dh; ++index37)
        {
          datad[index36] = this.COMBINE_PARTIAL(datad[index36], datas[index35] ^ datad[index36], m1);
          index36 += dwpl;
          index35 += swpl;
        }
      }
      if (num9 > 0)
      {
        int num20 = num2;
        int num21 = num1;
        for (int index38 = 0; index38 < dh; ++index38)
        {
          for (int index39 = 0; index39 < num10; ++index39)
            datad[num21 + index39] ^= datas[num20 + index39];
          num21 += dwpl;
          num20 += swpl;
        }
      }
      if (num11 <= 0)
        return;
      int index40 = num6;
      int index41 = num5;
      for (int index42 = 0; index42 < dh; ++index42)
      {
        datad[index41] = this.COMBINE_PARTIAL(datad[index41], datas[index40] ^ datad[index41], m2);
        index41 += dwpl;
        index40 += swpl;
      }
    }
    else if (op == (JBIG2Statics.PixNot(JBIG2Statics.PixSrc) | JBIG2Statics.PixDst))
    {
      if (num7 > 0)
      {
        int index43 = num4;
        int index44 = num3;
        for (int index45 = 0; index45 < dh; ++index45)
        {
          datad[index44] = this.COMBINE_PARTIAL(datad[index44], ~datas[index43] | datad[index44], m1);
          index44 += dwpl;
          index43 += swpl;
        }
      }
      if (num9 > 0)
      {
        int num22 = num2;
        int num23 = num1;
        for (int index46 = 0; index46 < dh; ++index46)
        {
          for (int index47 = 0; index47 < num10; ++index47)
            datad[num23 + index47] |= ~datas[num22 + index47];
          num23 += dwpl;
          num22 += swpl;
        }
      }
      if (num11 <= 0)
        return;
      int index48 = num6;
      int index49 = num5;
      for (int index50 = 0; index50 < dh; ++index50)
      {
        datad[index49] = this.COMBINE_PARTIAL(datad[index49], ~datas[index48] | datad[index49], m2);
        index49 += dwpl;
        index48 += swpl;
      }
    }
    else if (op == (JBIG2Statics.PixNot(JBIG2Statics.PixSrc) & JBIG2Statics.PixDst))
    {
      if (num7 > 0)
      {
        int index51 = num4;
        int index52 = num3;
        for (int index53 = 0; index53 < dh; ++index53)
        {
          datad[index52] = this.COMBINE_PARTIAL(datad[index52], ~datas[index51] & datad[index52], m1);
          index52 += dwpl;
          index51 += swpl;
        }
      }
      if (num9 > 0)
      {
        int num24 = num2;
        int num25 = num1;
        for (int index54 = 0; index54 < dh; ++index54)
        {
          for (int index55 = 0; index55 < num10; ++index55)
            datad[num25 + index55] &= ~datas[num24 + index55];
          num25 += dwpl;
          num24 += swpl;
        }
      }
      if (num11 <= 0)
        return;
      int index56 = num6;
      int index57 = num5;
      for (int index58 = 0; index58 < dh; ++index58)
      {
        datad[index57] = this.COMBINE_PARTIAL(datad[index57], ~datas[index56] & datad[index57], m2);
        index57 += dwpl;
        index56 += swpl;
      }
    }
    else if (op == (JBIG2Statics.PixSrc | JBIG2Statics.PixNot(JBIG2Statics.PixDst)))
    {
      if (num7 > 0)
      {
        int index59 = num4;
        int index60 = num3;
        for (int index61 = 0; index61 < dh; ++index61)
        {
          datad[index60] = this.COMBINE_PARTIAL(datad[index60], datas[index59] | ~datad[index60], m1);
          index60 += dwpl;
          index59 += swpl;
        }
      }
      if (num9 > 0)
      {
        int num26 = num2;
        int num27 = num1;
        for (int index62 = 0; index62 < dh; ++index62)
        {
          for (int index63 = 0; index63 < num10; ++index63)
            datad[num27 + index63] = datas[num26 + index63] | ~datad[num27 + index63];
          num27 += dwpl;
          num26 += swpl;
        }
      }
      if (num11 <= 0)
        return;
      int index64 = num6;
      int index65 = num5;
      for (int index66 = 0; index66 < dh; ++index66)
      {
        datad[index65] = this.COMBINE_PARTIAL(datad[index65], datas[index64] | ~datad[index65], m2);
        index65 += dwpl;
        index64 += swpl;
      }
    }
    else if (op == (JBIG2Statics.PixSrc & JBIG2Statics.PixNot(JBIG2Statics.PixDst)))
    {
      if (num7 > 0)
      {
        int index67 = num4;
        int index68 = num3;
        for (int index69 = 0; index69 < dh; ++index69)
        {
          datad[index68] = this.COMBINE_PARTIAL(datad[index68], datas[index67] & ~datad[index68], m1);
          index68 += dwpl;
          index67 += swpl;
        }
      }
      if (num9 > 0)
      {
        int num28 = num2;
        int num29 = num1;
        for (int index70 = 0; index70 < dh; ++index70)
        {
          for (int index71 = 0; index71 < num10; ++index71)
            datad[num29 + index71] = datas[num28 + index71] & ~datad[num29 + index71];
          num29 += dwpl;
          num28 += swpl;
        }
      }
      if (num11 <= 0)
        return;
      int index72 = num6;
      int index73 = num5;
      for (int index74 = 0; index74 < dh; ++index74)
      {
        datad[index73] = this.COMBINE_PARTIAL(datad[index73], datas[index72] & ~datad[index73], m2);
        index73 += dwpl;
        index72 += swpl;
      }
    }
    else if (op == JBIG2Statics.PixNot(JBIG2Statics.PixSrc | JBIG2Statics.PixDst))
    {
      if (num7 > 0)
      {
        int index75 = num4;
        int index76 = num3;
        for (int index77 = 0; index77 < dh; ++index77)
        {
          datad[index76] = this.COMBINE_PARTIAL(datad[index76], (uint) ~((int) datas[index75] | (int) datad[index76]), m1);
          index76 += dwpl;
          index75 += swpl;
        }
      }
      if (num9 > 0)
      {
        int num30 = num2;
        int num31 = num1;
        for (int index78 = 0; index78 < dh; ++index78)
        {
          for (int index79 = 0; index79 < num10; ++index79)
            datad[num31 + index79] = (uint) ~((int) datas[num30 + index79] | (int) datad[num31 + index79]);
          num31 += dwpl;
          num30 += swpl;
        }
      }
      if (num11 <= 0)
        return;
      int index80 = num6;
      int index81 = num5;
      for (int index82 = 0; index82 < dh; ++index82)
      {
        datad[index81] = this.COMBINE_PARTIAL(datad[index81], (uint) ~((int) datas[index80] | (int) datad[index81]), m2);
        index81 += dwpl;
        index80 += swpl;
      }
    }
    else if (op == JBIG2Statics.PixNot(JBIG2Statics.PixSrc & JBIG2Statics.PixDst))
    {
      if (num7 > 0)
      {
        int index83 = num4;
        int index84 = num3;
        for (int index85 = 0; index85 < dh; ++index85)
        {
          datad[index84] = this.COMBINE_PARTIAL(datad[index84], (uint) ~((int) datas[index83] & (int) datad[index84]), m1);
          index84 += dwpl;
          index83 += swpl;
        }
      }
      if (num9 > 0)
      {
        int num32 = num2;
        int num33 = num1;
        for (int index86 = 0; index86 < dh; ++index86)
        {
          for (int index87 = 0; index87 < num10; ++index87)
            datad[num33 + index87] = (uint) ~((int) datas[num32 + index87] & (int) datad[num33 + index87]);
          num33 += dwpl;
          num32 += swpl;
        }
      }
      if (num11 <= 0)
        return;
      int index88 = num6;
      int index89 = num5;
      for (int index90 = 0; index90 < dh; ++index90)
      {
        datad[index89] = this.COMBINE_PARTIAL(datad[index89], (uint) ~((int) datas[index88] & (int) datad[index89]), m2);
        index89 += dwpl;
        index88 += swpl;
      }
    }
    else
    {
      if (op != JBIG2Statics.PixNot(JBIG2Statics.PixSrc ^ JBIG2Statics.PixDst))
        return;
      if (num7 > 0)
      {
        int index91 = num4;
        int index92 = num3;
        for (int index93 = 0; index93 < dh; ++index93)
        {
          datad[index92] = this.COMBINE_PARTIAL(datad[index92], (uint) ~((int) datas[index91] ^ (int) datad[index92]), m1);
          index92 += dwpl;
          index91 += swpl;
        }
      }
      if (num9 > 0)
      {
        int num34 = num2;
        int num35 = num1;
        for (int index94 = 0; index94 < dh; ++index94)
        {
          for (int index95 = 0; index95 < num10; ++index95)
            datad[num35 + index95] = (uint) ~((int) datas[num34 + index95] ^ (int) datad[num35 + index95]);
          num35 += dwpl;
          num34 += swpl;
        }
      }
      if (num11 <= 0)
        return;
      int index96 = num6;
      int index97 = num5;
      for (int index98 = 0; index98 < dh; ++index98)
      {
        datad[index97] = this.COMBINE_PARTIAL(datad[index97], (uint) ~((int) datas[index96] ^ (int) datad[index97]), m2);
        index97 += dwpl;
        index96 += swpl;
      }
    }
  }

  private void RasteropWordAlignedLow(
    ref uint[] datad,
    int dwpl,
    int dx,
    int dy,
    int dw,
    int dh,
    int op,
    uint[] datas,
    int swpl,
    int sx,
    int sy)
  {
    uint m = 0;
    int num1 = dw >> 5;
    int index1 = dw & 31 /*0x1F*/;
    if (index1 > 0)
      m = JBIG2Statics.LeftMask[index1];
    int num2 = swpl * sy + (sx >> 5);
    int num3 = dwpl * dy + (dx >> 5);
    if (op == JBIG2Statics.PixSrc)
    {
      for (int index2 = 0; index2 < dh; ++index2)
      {
        int index3 = index2 * swpl + num2;
        int index4 = index2 * dwpl + num3;
        for (int index5 = 0; index5 < num1; ++index5)
        {
          datad[index4] = datas[index3];
          ++index4;
          ++index3;
        }
        if (index1 > 0)
          datad[index4] = this.COMBINE_PARTIAL(datad[index4], datas[index3], m);
      }
    }
    else if (op == JBIG2Statics.PixNot(JBIG2Statics.PixSrc))
    {
      for (int index6 = 0; index6 < dh; ++index6)
      {
        int index7 = index6 * swpl + num2;
        int index8 = index6 * dwpl + num3;
        for (int index9 = 0; index9 < num1; ++index9)
        {
          datad[index8] = ~datas[index7];
          ++index8;
          ++index7;
        }
        if (index1 > 0)
          datad[index8] = this.COMBINE_PARTIAL(datad[index8], ~datas[index7], m);
      }
    }
    else if (op == (JBIG2Statics.PixSrc | JBIG2Statics.PixDst))
    {
      for (int index10 = 0; index10 < dh; ++index10)
      {
        int index11 = index10 * swpl + num2;
        int index12 = index10 * dwpl + num3;
        for (int index13 = 0; index13 < num1; ++index13)
        {
          datad[index12] = datas[index11] | datad[index12];
          ++index12;
          ++index11;
        }
        if (index1 > 0)
          datad[index12] = this.COMBINE_PARTIAL(datad[index12], datas[index11] | datad[index12], m);
      }
    }
    else if (op == (JBIG2Statics.PixSrc & JBIG2Statics.PixDst))
    {
      for (int index14 = 0; index14 < dh; ++index14)
      {
        int index15 = index14 * swpl + num2;
        int index16 = index14 * dwpl + num3;
        for (int index17 = 0; index17 < num1; ++index17)
        {
          datad[index16] = datas[index15] & datad[index16];
          ++index16;
          ++index15;
        }
        if (index1 > 0)
          datad[index16] = this.COMBINE_PARTIAL(datad[index16], datas[index15] & datad[index16], m);
      }
    }
    else if (op == (JBIG2Statics.PixSrc ^ JBIG2Statics.PixDst))
    {
      for (int index18 = 0; index18 < dh; ++index18)
      {
        int index19 = index18 * swpl + num2;
        int index20 = index18 * dwpl + num3;
        for (int index21 = 0; index21 < num1; ++index21)
        {
          datad[index20] = datas[index19] ^ datad[index20];
          ++index20;
          ++index19;
        }
        if (index1 > 0)
          datad[index20] = this.COMBINE_PARTIAL(datad[index20], datas[index19] ^ datad[index20], m);
      }
    }
    else if (op == (JBIG2Statics.PixNot(JBIG2Statics.PixSrc) | JBIG2Statics.PixDst))
    {
      for (int index22 = 0; index22 < dh; ++index22)
      {
        int index23 = index22 * swpl + num2;
        int index24 = index22 * dwpl + num3;
        for (int index25 = 0; index25 < num1; ++index25)
        {
          datad[index24] = ~datas[index23] | datad[index24];
          ++index24;
          ++index23;
        }
        if (index1 > 0)
          datad[index24] = this.COMBINE_PARTIAL(datad[index24], ~datas[index23] | datad[index24], m);
      }
    }
    else if (op == (JBIG2Statics.PixNot(JBIG2Statics.PixSrc) & JBIG2Statics.PixDst))
    {
      for (int index26 = 0; index26 < dh; ++index26)
      {
        int index27 = index26 * swpl + num2;
        int index28 = index26 * dwpl + num3;
        for (int index29 = 0; index29 < num1; ++index29)
        {
          datad[index28] = ~datas[index27] & datad[index28];
          ++index28;
          ++index27;
        }
        if (index1 > 0)
          datad[index28] = this.COMBINE_PARTIAL(datad[index28], ~datas[index27] & datad[index28], m);
      }
    }
    else if (op == (JBIG2Statics.PixSrc | JBIG2Statics.PixNot(JBIG2Statics.PixDst)))
    {
      for (int index30 = 0; index30 < dh; ++index30)
      {
        int index31 = index30 * swpl + num2;
        int index32 = index30 * dwpl + num3;
        for (int index33 = 0; index33 < num1; ++index33)
        {
          datad[index32] = datas[index31] | ~datad[index32];
          ++index32;
          ++index31;
        }
        if (index1 > 0)
          datad[index32] = this.COMBINE_PARTIAL(datad[index32], datas[index31] | ~datad[index32], m);
      }
    }
    else if (op == (JBIG2Statics.PixSrc & JBIG2Statics.PixNot(JBIG2Statics.PixDst)))
    {
      for (int index34 = 0; index34 < dh; ++index34)
      {
        int index35 = index34 * swpl + num2;
        int index36 = index34 * dwpl + num3;
        for (int index37 = 0; index37 < num1; ++index37)
        {
          datad[index36] = datas[index35] & ~datad[index36];
          ++index36;
          ++index35;
        }
        if (index1 > 0)
          datad[index36] = this.COMBINE_PARTIAL(datad[index36], datas[index35] & ~datad[index36], m);
      }
    }
    else if (op == JBIG2Statics.PixNot(JBIG2Statics.PixSrc | JBIG2Statics.PixDst))
    {
      for (int index38 = 0; index38 < dh; ++index38)
      {
        int index39 = index38 * swpl + num2;
        int index40 = index38 * dwpl + num3;
        for (int index41 = 0; index41 < num1; ++index41)
        {
          datad[index40] = (uint) ~((int) datas[index39] | (int) datad[index40]);
          ++index40;
          ++index39;
        }
        if (index1 > 0)
          datad[index40] = this.COMBINE_PARTIAL(datad[index40], (uint) ~((int) datas[index39] | (int) datad[index40]), m);
      }
    }
    else if (op == JBIG2Statics.PixNot(JBIG2Statics.PixSrc & JBIG2Statics.PixDst))
    {
      for (int index42 = 0; index42 < dh; ++index42)
      {
        int index43 = index42 * swpl + num2;
        int index44 = index42 * dwpl + num3;
        for (int index45 = 0; index45 < num1; ++index45)
        {
          datad[index44] = (uint) ~((int) datas[index43] & (int) datad[index44]);
          ++index44;
          ++index43;
        }
        if (index1 > 0)
          datad[index44] = this.COMBINE_PARTIAL(datad[index44], (uint) ~((int) datas[index43] & (int) datad[index44]), m);
      }
    }
    else
    {
      if (op != JBIG2Statics.PixNot(JBIG2Statics.PixSrc ^ JBIG2Statics.PixDst))
        return;
      for (int index46 = 0; index46 < dh; ++index46)
      {
        int index47 = index46 * swpl + num2;
        int index48 = index46 * dwpl + num3;
        for (int index49 = 0; index49 < num1; ++index49)
        {
          datad[index48] = (uint) ~((int) datas[index47] ^ (int) datad[index48]);
          ++index48;
          ++index47;
        }
        if (index1 > 0)
          datad[index48] = this.COMBINE_PARTIAL(datad[index48], (uint) ~((int) datas[index47] ^ (int) datad[index48]), m);
      }
    }
  }

  private void RasteropUniLow(
    ref uint[] datad,
    int dpixw,
    int dpixh,
    int depth,
    int dwpl,
    int dx,
    int dy,
    int dw,
    int dh,
    int op)
  {
    if (depth != 1)
    {
      dpixw *= depth;
      dx *= depth;
      dw *= depth;
    }
    if (dx < 0)
    {
      dw += dx;
      dx = 0;
    }
    int num1 = dx + dw - dpixw;
    if (num1 > 0)
      dw -= num1;
    if (dy < 0)
    {
      dh += dy;
      dy = 0;
    }
    int num2 = dy + dh - dpixh;
    if (num2 > 0)
      dh -= num2;
    if (dw <= 0 || dh <= 0)
      return;
    if ((dx & 31 /*0x1F*/) == 0)
      this.RasteropUniWordAlignedLow(ref datad, dwpl, dx, dy, dw, dh, op);
    else
      this.RasteropUniGeneralLow(ref datad, dwpl, dx, dy, dw, dh, op);
  }

  private void RasteropUniGeneralLow(
    ref uint[] datad,
    int dwpl,
    int dx,
    int dy,
    int dw,
    int dh,
    int op)
  {
    uint m1 = 0;
    uint m2 = 0;
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    bool flag1;
    int index1;
    if ((dx & 31 /*0x1F*/) == 0)
    {
      flag1 = false;
      index1 = 0;
    }
    else
    {
      flag1 = true;
      index1 = 32 /*0x20*/ - (dx & 31 /*0x1F*/);
      m1 = JBIG2Statics.RightMask[index1];
      num1 = dwpl * dy + (dx >> 5);
    }
    bool flag2;
    if (dw >= index1)
    {
      flag2 = false;
    }
    else
    {
      flag2 = true;
      m1 &= JBIG2Statics.LeftMask[32 /*0x20*/ - index1 + dw];
    }
    bool flag3;
    int num4;
    if (flag2)
    {
      flag3 = false;
      num4 = 0;
    }
    else
    {
      num4 = dw - index1 >> 5;
      if (num4 == 0)
      {
        flag3 = false;
      }
      else
      {
        flag3 = true;
        num2 = !flag1 ? dwpl * dy + (dx >> 5) : num1 + 1;
      }
    }
    int index2 = dx + dw & 31 /*0x1F*/;
    bool flag4;
    if (flag2 || index2 == 0)
    {
      flag4 = false;
    }
    else
    {
      flag4 = true;
      m2 = JBIG2Statics.LeftMask[index2];
      num3 = !flag1 ? dwpl * dy + (dx >> 5) + num4 : num1 + 1 + num4;
    }
    if (op == JBIG2Statics.PixClr)
    {
      if (flag1)
      {
        int index3 = num1;
        for (int index4 = 0; index4 < dh; ++index4)
        {
          datad[index3] = this.COMBINE_PARTIAL(datad[index3], 0U, m1);
          index3 += dwpl;
        }
      }
      if (flag3)
      {
        int num5 = num2;
        for (int index5 = 0; index5 < dh; ++index5)
        {
          for (int index6 = 0; index6 < num4; ++index6)
            datad[num5 + index6] = 0U;
          num5 += dwpl;
        }
      }
      if (!flag4)
        return;
      int index7 = num3;
      for (int index8 = 0; index8 < dh; ++index8)
      {
        datad[index7] = this.COMBINE_PARTIAL(datad[index7], 0U, m2);
        index7 += dwpl;
      }
    }
    else if (op == JBIG2Statics.PixSet)
    {
      if (flag1)
      {
        int index9 = num1;
        for (int index10 = 0; index10 < dh; ++index10)
        {
          datad[index9] = this.COMBINE_PARTIAL(datad[index9], uint.MaxValue, m1);
          index9 += dwpl;
        }
      }
      if (flag3)
      {
        int num6 = num2;
        for (int index11 = 0; index11 < dh; ++index11)
        {
          for (int index12 = 0; index12 < num4; ++index12)
            datad[num6 + index12] = uint.MaxValue;
          num6 += dwpl;
        }
      }
      if (!flag4)
        return;
      int index13 = num3;
      for (int index14 = 0; index14 < dh; ++index14)
      {
        datad[index13] = this.COMBINE_PARTIAL(datad[index13], uint.MaxValue, m2);
        index13 += dwpl;
      }
    }
    else
    {
      if (op != JBIG2Statics.PixNot(JBIG2Statics.PixDst))
        return;
      if (flag1)
      {
        int index15 = num1;
        for (int index16 = 0; index16 < dh; ++index16)
        {
          datad[index15] = this.COMBINE_PARTIAL(datad[index15], ~datad[index15], m1);
          index15 += dwpl;
        }
      }
      if (flag3)
      {
        int num7 = num2;
        for (int index17 = 0; index17 < dh; ++index17)
        {
          for (int index18 = 0; index18 < num4; ++index18)
            datad[num7 + index18] = ~datad[num7 + index18];
          num7 += dwpl;
        }
      }
      if (!flag4)
        return;
      int index19 = num1;
      for (int index20 = 0; index20 < dh; ++index20)
      {
        datad[index19] = this.COMBINE_PARTIAL(datad[index19], ~datad[index19], m2);
        index19 += dwpl;
      }
    }
  }

  private void RasteropUniWordAlignedLow(
    ref uint[] datad,
    int dwpl,
    int dx,
    int dy,
    int dw,
    int dh,
    int op)
  {
    uint m = 0;
    int num1 = dw >> 5;
    int index1 = dw & 31 /*0x1F*/;
    if (index1 > 0)
      m = JBIG2Statics.LeftMask[index1];
    int num2 = dwpl * dy + (dx >> 5);
    if (op == JBIG2Statics.PixClr)
    {
      for (int index2 = 0; index2 < dh; ++index2)
      {
        int index3 = index2 * dwpl + num2;
        for (int index4 = 0; index4 < num1; ++index4)
          datad[index3++] = 0U;
        if (index1 > 0)
          datad[index3] = this.COMBINE_PARTIAL(datad[index3], 0U, m);
      }
    }
    else if (op == JBIG2Statics.PixSet)
    {
      for (int index5 = 0; index5 < dh; ++index5)
      {
        int index6 = index5 * dwpl + num2;
        for (int index7 = 0; index7 < num1; ++index7)
          datad[index6++] = uint.MaxValue;
        if (index1 > 0)
          datad[index6] = this.COMBINE_PARTIAL(datad[index6], uint.MaxValue, m);
      }
    }
    else
    {
      if (op != JBIG2Statics.PixNot(JBIG2Statics.PixDst))
        return;
      for (int index8 = 0; index8 < dh; ++index8)
      {
        int index9 = index8 * dwpl + num2;
        for (int index10 = 0; index10 < num1; ++index10)
        {
          datad[index9] = ~datad[index9];
          ++index9;
        }
        if (index1 > 0)
          datad[index9] = this.COMBINE_PARTIAL(datad[index9], ~datad[index9], m);
      }
    }
  }

  private uint COMBINE_PARTIAL(uint d, uint s, uint m)
  {
    return (uint) ((int) d & ~(int) m | (int) s & (int) m);
  }

  private void ComposeRGBPixel(int rval, int gval, int bval, int ppixel)
  {
    ppixel = rval << this.m_redShift | gval << this.m_greenShift | bval << this.m_blueShift;
  }

  private Pix PixCreate(int width, int height, int depth)
  {
    return this.PixCreateNoInit(width, height, depth);
  }

  private void PixCmapGetColor(PixColormap cmap, int index, int prval, int pgval, int pbval)
  {
    if (index < 0 || index >= cmap.N)
      throw new NullReferenceException("index out of bounds");
    RGBA_Quad[] array = cmap.Array;
    prval = array[index].Red;
    pgval = array[index].Green;
    pbval = array[index].Blue;
  }

  private Pix PixCopy(Pix pixd, Pix pixs)
  {
    if (pixs == null)
      throw new NullReferenceException("Pixs not defined");
    if (pixs == pixd)
      return pixd;
    int num = 4 * pixs.Wpl * pixs.H;
    if (pixd == null)
    {
      uint[] sourceArray = (pixd = this.PixCreateTemplate(pixs)) != null ? pixs.Data : throw new NullReferenceException("pixd not made");
      if (sourceArray != null)
      {
        pixd.Data = new uint[sourceArray.Length];
        Array.Copy((Array) sourceArray, (Array) pixd.Data, sourceArray.Length);
      }
      return pixd;
    }
    if (this.PixResizeImageData(pixd, pixs) == 1)
      return (Pix) null;
    this.PixCopyColormap(pixd, pixs);
    this.PixCopyResolution(pixd, pixs);
    pixd.Informat = pixs.Informat;
    pixd.Text = pixs.Text;
    pixd.Data = new uint[num / 4];
    uint[] data = pixd.Data;
    Array.Copy((Array) pixs.Data, (Array) data, num / 4);
    pixd.Data = data;
    return pixd;
  }

  private Pix PixCreateTemplate(Pix pixs)
  {
    Pix templateNoInit;
    if ((templateNoInit = this.PixCreateTemplateNoInit(pixs)) != null)
      return templateNoInit;
    Console.WriteLine("pixd not made");
    return (Pix) null;
  }

  private Pix PixCreateTemplateNoInit(Pix pixs)
  {
    Pix noInit = this.PixCreateNoInit(pixs.W, pixs.H, pixs.D);
    if (noInit == null)
      throw new Exception("Pixd not made");
    this.PixCopyResolution(noInit, pixs);
    this.PixCopyColormap(noInit, pixs);
    noInit.Text = pixs.Text;
    noInit.Informat = pixs.Informat;
    return noInit;
  }

  private void PixCopyColormap(Pix pixd, Pix pixs)
  {
    if (pixs == pixd)
      return;
    pixd = (Pix) null;
    PixColormap colormap;
    if ((colormap = pixs.Colormap) == null)
      return;
    PixColormap pixColormap;
    if ((pixColormap = this.PixCmapCopy(colormap)) == null)
      throw new NullReferenceException("Cmapd not made");
    pixd.Colormap = pixColormap;
  }

  private PixColormap PixCmapCopy(PixColormap cmaps)
  {
    return new PixColormap()
    {
      N = cmaps.N,
      Nalloc = cmaps.Nalloc,
      Depth = cmaps.Depth,
      Array = cmaps.Array
    };
  }

  private void PixCopyResolution(Pix pixd, Pix pixs)
  {
    pixd.XRes = pixs.XRes;
    pixd.YRes = pixs.YRes;
  }

  private Pix PixCreateNoInit(int width, int height, int depth)
  {
    Pix header = this.PixCreateHeader(width, height, depth);
    int num = header != null ? header.Wpl : throw new NullReferenceException("Pixd is null");
    header.Data = new uint[num * height];
    for (int index = 0; index < num * height; ++index)
      header.Data[index] = 3452816845U /*0xCDCDCDCD*/;
    this.PixSetPadBits(ref header, 0);
    return header;
  }

  private void PixSetPadBits(ref Pix pix, int val)
  {
    int w = pix.W;
    int h = pix.H;
    int d = pix.D;
    if (d == 32 /*0x20*/)
      return;
    uint[] data = pix.Data;
    int wpl = pix.Wpl;
    int index1 = 32 /*0x20*/ - w * d % 32 /*0x20*/;
    if (index1 == 32 /*0x20*/)
      return;
    int num1 = w * d / 32 /*0x20*/;
    uint num2 = JBIG2Statics.RightMask[index1];
    if (val == 0)
      num2 = ~num2;
    for (int index2 = 0; index2 < h; ++index2)
    {
      int index3 = index2 * wpl + num1;
      data[index3] = val != 0 ? data[index3] | num2 : data[index3] & num2;
    }
    pix.Data = data;
  }

  private Pix PixCreateHeader(int width, int height, int depth)
  {
    Pix header = new Pix();
    if (depth != 1 && depth != 2 && depth != 4 && depth != 8 && depth != 16 /*0x10*/ && depth != 24 && depth != 32 /*0x20*/)
      throw new ArgumentOutOfRangeException("depth must be {1, 2, 4, 8, 16, 24, 32}");
    if (width <= 0)
      throw new ArgumentOutOfRangeException("width must be > 0");
    if (height <= 0)
      throw new ArgumentOutOfRangeException("height must be > 0");
    header.W = width;
    header.H = height;
    header.D = depth;
    int num = (width * depth + 31 /*0x1F*/) / 32 /*0x20*/;
    header.Wpl = num;
    header.Informat = 0;
    return header;
  }

  private void PixCmapHasColor(PixColormap cmap, ref bool pcolor)
  {
    int[] rmap = new int[0];
    int[] gmap = new int[0];
    int[] bmap = new int[0];
    pcolor = false;
    if (cmap == null)
      throw new NullReferenceException("cmap not defined");
    if (!this.PixCmapToArrays(cmap, ref rmap, ref gmap, ref bmap))
      return;
    int n = cmap.N;
    for (int index = 0; index < n; ++index)
    {
      if (rmap[index] != gmap[index] || rmap[index] != bmap[index])
      {
        pcolor = true;
        break;
      }
    }
    rmap = (int[]) null;
  }

  private bool PixCmapToArrays(PixColormap cmap, ref int[] rmap, ref int[] gmap, ref int[] bmap)
  {
    int length = cmap != null ? cmap.N : throw new NullReferenceException("cmap not defined");
    rmap = new int[length];
    gmap = new int[length];
    bmap = new int[length];
    RGBA_Quad[] array = cmap.Array;
    for (int index = 0; index < length; ++index)
    {
      rmap[index] = array[index].Red;
      gmap[index] = array[index].Green;
      bmap[index] = array[index].Blue;
    }
    return false;
  }
}
