// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.ByteEncodingBase
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal abstract class ByteEncodingBase
{
  private readonly PdfRangeCalculator range;

  public static ByteEncodingCollectionBase DictByteEncodings { get; private set; }

  public static ByteEncodingCollectionBase CharStringByteEncodings { get; private set; }

  private static void InitializeDictByteEncodings()
  {
    ByteEncodingBase.DictByteEncodings = new ByteEncodingCollectionBase();
    ByteEncodingBase.DictByteEncodings.Add((ByteEncodingBase) new SingleByteIntegerEncodingBase());
    ByteEncodingBase.DictByteEncodings.Add((ByteEncodingBase) new TwoByteIntegerEncodingType1Base());
    ByteEncodingBase.DictByteEncodings.Add((ByteEncodingBase) new TwoByteIntegerEncodingType2Base());
    ByteEncodingBase.DictByteEncodings.Add((ByteEncodingBase) new ThreeByteIntegerEncodingBase());
    ByteEncodingBase.DictByteEncodings.Add((ByteEncodingBase) new FiveByteIntegerEncodingBase());
    ByteEncodingBase.DictByteEncodings.Add((ByteEncodingBase) new RealByteEncodingBase());
  }

  private static void InitializeCharStringByteEncodings()
  {
    ByteEncodingBase.CharStringByteEncodings = new ByteEncodingCollectionBase();
    ByteEncodingBase.CharStringByteEncodings.Add((ByteEncodingBase) new ThreeByteIntegerEncodingBase());
    ByteEncodingBase.CharStringByteEncodings.Add((ByteEncodingBase) new SingleByteIntegerEncodingBase());
    ByteEncodingBase.CharStringByteEncodings.Add((ByteEncodingBase) new TwoByteIntegerEncodingType1Base());
    ByteEncodingBase.CharStringByteEncodings.Add((ByteEncodingBase) new TwoByteIntegerEncodingType2Base());
    ByteEncodingBase.CharStringByteEncodings.Add((ByteEncodingBase) new FiveByteFixedEncodingBase());
  }

  static ByteEncodingBase()
  {
    ByteEncodingBase.InitializeDictByteEncodings();
    ByteEncodingBase.InitializeCharStringByteEncodings();
  }

  public ByteEncodingBase(byte start, byte end)
  {
    this.range = new PdfRangeCalculator((int) start, (int) end);
  }

  public abstract object Read(EncodedDataParser reader);

  public bool IsInRange(byte b0) => this.range.IsInRange((int) b0);
}
