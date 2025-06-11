// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontByteEncoding
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal abstract class SystemFontByteEncoding
{
  private readonly SystemFontRange range;

  public static SystemFontByteEncodingCollection DictByteEncodings { get; private set; }

  public static SystemFontByteEncodingCollection CharStringByteEncodings { get; private set; }

  private static void InitializeDictByteEncodings()
  {
    SystemFontByteEncoding.DictByteEncodings = new SystemFontByteEncodingCollection();
    SystemFontByteEncoding.DictByteEncodings.Add((SystemFontByteEncoding) new SystemFontSingleByteIntegerEncoding());
    SystemFontByteEncoding.DictByteEncodings.Add((SystemFontByteEncoding) new SystemFontTwoByteIntegerEncodingType1());
    SystemFontByteEncoding.DictByteEncodings.Add((SystemFontByteEncoding) new SystemFontTwoByteIntegerEncodingType2());
    SystemFontByteEncoding.DictByteEncodings.Add((SystemFontByteEncoding) new SystemFontThreeByteIntegerEncoding());
    SystemFontByteEncoding.DictByteEncodings.Add((SystemFontByteEncoding) new SystemFontFiveByteIntegerEncoding());
    SystemFontByteEncoding.DictByteEncodings.Add((SystemFontByteEncoding) new SystemFontRealByteEncoding());
  }

  private static void InitializeCharStringByteEncodings()
  {
    SystemFontByteEncoding.CharStringByteEncodings = new SystemFontByteEncodingCollection();
    SystemFontByteEncoding.CharStringByteEncodings.Add((SystemFontByteEncoding) new SystemFontThreeByteIntegerEncoding());
    SystemFontByteEncoding.CharStringByteEncodings.Add((SystemFontByteEncoding) new SystemFontSingleByteIntegerEncoding());
    SystemFontByteEncoding.CharStringByteEncodings.Add((SystemFontByteEncoding) new SystemFontTwoByteIntegerEncodingType1());
    SystemFontByteEncoding.CharStringByteEncodings.Add((SystemFontByteEncoding) new SystemFontTwoByteIntegerEncodingType2());
    SystemFontByteEncoding.CharStringByteEncodings.Add((SystemFontByteEncoding) new SystemFontFiveByteFixedEncoding());
  }

  static SystemFontByteEncoding()
  {
    SystemFontByteEncoding.InitializeDictByteEncodings();
    SystemFontByteEncoding.InitializeCharStringByteEncodings();
  }

  public SystemFontByteEncoding(byte start, byte end)
  {
    this.range = new SystemFontRange((int) start, (int) end);
  }

  public abstract object Read(SystemFontEncodedDataReader reader);

  public bool IsInRange(byte b0) => this.range.IsInRange((int) b0);
}
