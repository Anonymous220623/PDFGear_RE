// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.DeflateCodec
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Compression.JBIG2.ZLib;

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal class DeflateCodec : CodecWithPredictor
{
  public const int ZSTATE_INIT_DECODE = 1;
  public const int ZSTATE_INIT_ENCODE = 2;
  public ZStream m_stream = new ZStream();
  public int m_zipquality;
  public int m_state;
  private static readonly TiffFieldInfo[] zipFieldInfo = new TiffFieldInfo[1]
  {
    new TiffFieldInfo(Syncfusion.Pdf.Compression.JBIG2.TiffTag.ZIPQUALITY, (short) 0, (short) 0, Syncfusion.Pdf.Compression.JBIG2.TiffType.NOTYPE, (short) 0, true, false, string.Empty)
  };
  private TiffTagMethods m_tagMethods;

  public DeflateCodec(Tiff tif, Syncfusion.Pdf.Compression.JBIG2.Compression scheme, string name)
    : base(tif, scheme, name)
  {
    this.m_tagMethods = (TiffTagMethods) new DeflateCodecTagMethods();
  }

  public override bool Init()
  {
    this.m_tif.MergeFieldInfo(DeflateCodec.zipFieldInfo, DeflateCodec.zipFieldInfo.Length);
    this.m_zipquality = -1;
    this.m_state = 0;
    this.TIFFPredictorInit(this.m_tagMethods);
    return true;
  }

  public override bool CanDecode => true;

  public override bool PreDecode(short plane) => this.ZIPPreDecode(plane);

  public override void Cleanup() => this.ZIPCleanup();

  public override bool predictor_setupdecode() => this.ZIPSetupDecode();

  public override bool predictor_decoderow(byte[] buffer, int offset, int count, short plane)
  {
    return this.ZIPDecode(buffer, offset, count, plane);
  }

  public override bool predictor_decodestrip(byte[] buffer, int offset, int count, short plane)
  {
    return this.ZIPDecode(buffer, offset, count, plane);
  }

  public override bool predictor_decodetile(byte[] buffer, int offset, int count, short plane)
  {
    return this.ZIPDecode(buffer, offset, count, plane);
  }

  private void ZIPCleanup()
  {
    this.TIFFPredictorCleanup();
    if ((this.m_state & 2) != 0)
    {
      this.m_stream.deflateEnd();
      this.m_state = 0;
    }
    else
    {
      if ((this.m_state & 1) == 0)
        return;
      this.m_stream.inflateEnd();
      this.m_state = 0;
    }
  }

  private bool ZIPDecode(byte[] buffer, int offset, int count, short plane)
  {
    this.m_stream.next_out = buffer;
    this.m_stream.next_out_index = offset;
    this.m_stream.avail_out = count;
    do
    {
      switch (this.m_stream.inflate(1))
      {
        case -3:
          if (this.m_stream.inflateSync() != 0)
            return false;
          goto case 0;
        case 0:
          continue;
        case 1:
          goto label_6;
        default:
          return false;
      }
    }
    while (this.m_stream.avail_out > 0);
label_6:
    return this.m_stream.avail_out == 0;
  }

  private bool ZIPPreDecode(short s)
  {
    if ((this.m_state & 1) == 0)
      this.SetupDecode();
    this.m_stream.next_in = this.m_tif.m_rawdata;
    this.m_stream.next_in_index = 0;
    this.m_stream.avail_in = this.m_tif.m_rawcc;
    return this.m_stream.inflateInit() == 0;
  }

  private bool ZIPSetupDecode()
  {
    if ((this.m_state & 2) != 0)
    {
      this.m_stream.deflateEnd();
      this.m_state = 0;
    }
    if (this.m_stream.inflateInit() != 0)
      return false;
    this.m_state |= 1;
    return true;
  }
}
