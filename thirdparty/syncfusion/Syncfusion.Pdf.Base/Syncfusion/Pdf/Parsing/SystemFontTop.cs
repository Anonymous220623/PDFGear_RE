// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontTop
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontTop(SystemFontCFFFontFile file, long offset, int length) : 
  SystemFontDict(file, offset, length),
  ISystemFontBuildCharHolder
{
  private ISystemFontEncoding encoding;
  private SystemFontCharString charString;
  private SystemFontCharset charset;
  private string familyName;
  private SystemFontMatrix? fontMatrix;
  private SystemFontPrivate priv;
  private int? charstringType;
  private int? defaultWidthX;
  private int? nominalWidthX;
  private bool? usesCIDFontOperators;

  public static SystemFontOperatorDescriptor FamilyNameOperator { get; private set; }

  public static SystemFontOperatorDescriptor WeightOperator { get; private set; }

  public static SystemFontOperatorDescriptor EncodingOperator { get; private set; }

  public static SystemFontOperatorDescriptor CharStringsOperator { get; private set; }

  public static SystemFontOperatorDescriptor ItalicAngleOperator { get; private set; }

  public static SystemFontOperatorDescriptor CharstringTypeOperator { get; private set; }

  public static SystemFontOperatorDescriptor CharsetOperator { get; private set; }

  public static SystemFontOperatorDescriptor FontMatrixOperator { get; private set; }

  public static SystemFontOperatorDescriptor PrivateOperator { get; private set; }

  public static SystemFontOperatorDescriptor DefaultWidthXOperator { get; private set; }

  public static SystemFontOperatorDescriptor NominalWidthXOperator { get; private set; }

  public static SystemFontOperatorDescriptor ROSOperator { get; private set; }

  public static SystemFontOperatorDescriptor FDArrayOperator { get; private set; }

  public static SystemFontOperatorDescriptor FDSelectOperator { get; private set; }

  public int CharstringType
  {
    get
    {
      if (!this.charstringType.HasValue)
        this.charstringType = new int?(this.GetInt(SystemFontTop.CharstringTypeOperator));
      return this.charstringType.Value;
    }
  }

  public ISystemFontEncoding Encoding
  {
    get
    {
      if (this.encoding == null)
        this.ReadEncoding();
      return this.encoding;
    }
  }

  public SystemFontCharset Charset
  {
    get
    {
      if (this.charset == null)
        this.ReadCharset();
      return this.charset;
    }
  }

  public SystemFontCharString CharString
  {
    get
    {
      if (this.charString == null)
        this.ReadCharString();
      return this.charString;
    }
  }

  public string FamilyName
  {
    get
    {
      if (this.familyName == null)
        this.familyName = this.File.ReadString((ushort) this.GetInt(SystemFontTop.FamilyNameOperator));
      return this.familyName;
    }
  }

  public SystemFontMatrix FontMatrix
  {
    get
    {
      if (!this.fontMatrix.HasValue)
        this.fontMatrix = new SystemFontMatrix?(this.GetArray(SystemFontTop.FontMatrixOperator).ToMatrix());
      return this.fontMatrix.Value;
    }
  }

  public SystemFontPrivate Private
  {
    get
    {
      if (this.priv == null)
        this.ReadPrivate();
      return this.priv;
    }
  }

  public int DefaultWidthX
  {
    get
    {
      if (!this.defaultWidthX.HasValue)
        this.defaultWidthX = new int?(this.GetInt(SystemFontTop.DefaultWidthXOperator));
      return this.defaultWidthX.Value;
    }
  }

  public int NominalWidthX
  {
    get
    {
      if (!this.nominalWidthX.HasValue)
        this.nominalWidthX = new int?(this.GetInt(SystemFontTop.NominalWidthXOperator));
      return this.nominalWidthX.Value;
    }
  }

  public bool UsesCIDFontOperators
  {
    get
    {
      if (!this.usesCIDFontOperators.HasValue)
        this.usesCIDFontOperators = new bool?(this.Data.ContainsKey(SystemFontTop.ROSOperator));
      return this.usesCIDFontOperators.Value;
    }
  }

  static SystemFontTop()
  {
    SystemFontTop.FontMatrixOperator = new SystemFontOperatorDescriptor(SystemFontHelper.CreateByteArray((byte) 12, (byte) 7), (object) new SystemFontPostScriptArray(new object[6]
    {
      (object) 0.001,
      (object) 0,
      (object) 0,
      (object) 0.001,
      (object) 0,
      (object) 0
    }));
    SystemFontTop.FamilyNameOperator = new SystemFontOperatorDescriptor((byte) 3);
    SystemFontTop.WeightOperator = new SystemFontOperatorDescriptor((byte) 4);
    SystemFontTop.ItalicAngleOperator = new SystemFontOperatorDescriptor(SystemFontHelper.CreateByteArray((byte) 12, (byte) 2), (object) 0);
    SystemFontTop.CharstringTypeOperator = new SystemFontOperatorDescriptor(SystemFontHelper.CreateByteArray((byte) 12, (byte) 6), (object) 2);
    SystemFontTop.CharsetOperator = new SystemFontOperatorDescriptor((byte) 15, (object) 0);
    SystemFontTop.EncodingOperator = new SystemFontOperatorDescriptor((byte) 16 /*0x10*/, (object) 0);
    SystemFontTop.CharStringsOperator = new SystemFontOperatorDescriptor((byte) 17);
    SystemFontTop.PrivateOperator = new SystemFontOperatorDescriptor((byte) 18);
    SystemFontTop.DefaultWidthXOperator = new SystemFontOperatorDescriptor((byte) 20, (object) 0);
    SystemFontTop.NominalWidthXOperator = new SystemFontOperatorDescriptor((byte) 21, (object) 0);
    SystemFontTop.ROSOperator = new SystemFontOperatorDescriptor(SystemFontHelper.CreateByteArray((byte) 12, (byte) 30));
    SystemFontTop.FDArrayOperator = new SystemFontOperatorDescriptor(SystemFontHelper.CreateByteArray((byte) 12, (byte) 36));
    SystemFontTop.FDSelectOperator = new SystemFontOperatorDescriptor(SystemFontHelper.CreateByteArray((byte) 12, (byte) 37));
  }

  public byte[] GetSubr(int index) => this.Private.Subrs[index];

  public byte[] GetGlobalSubr(int index) => this.File.GlobalSubrs[index];

  public SystemFontType1GlyphData GetGlyphData(string name)
  {
    return this.CharString[this.GetGlyphId(name)];
  }

  public ushort GetGlyphId(string name) => this.Charset[name];

  public ushort GetGlyphId(ushort cid) => this.GetGlyphId(this.File.ReadString(cid));

  internal string GetGlyphName(ushort cid) => this.Encoding.GetGlyphName(this.File, cid);

  public ushort GetAdvancedWidth(ushort glyphId)
  {
    return (ushort) this.CharString.GetAdvancedWidth(glyphId, this.DefaultWidthX, this.NominalWidthX);
  }

  public void GetGlyphOutlines(SystemFontGlyph glyph, double fontSize)
  {
    this.CharString.GetGlyphOutlines(glyph, fontSize);
  }

  private void ReadEncoding()
  {
    int num = this.GetInt(SystemFontTop.EncodingOperator);
    if (SystemFontCFFPredefinedEncoding.IsPredefinedEncoding(num))
    {
      this.encoding = (ISystemFontEncoding) SystemFontCFFPredefinedEncoding.GetPredefinedEncoding(num);
    }
    else
    {
      SystemFontEncoding table = new SystemFontEncoding(this.File, this.Charset, (long) num);
      this.File.ReadTable((SystemFontCFFTable) table);
      this.encoding = (ISystemFontEncoding) table;
    }
  }

  private void ReadPrivate()
  {
    SystemFontOperandsCollection operands = this.GetOperands(SystemFontTop.PrivateOperator);
    this.priv = new SystemFontPrivate(this, (long) operands.GetLastAsInt(), operands.GetLastAsInt());
    this.File.ReadTable((SystemFontCFFTable) this.priv);
  }

  private void ReadCharset()
  {
    int num = this.GetInt(SystemFontTop.CharsetOperator);
    if (SystemFontPredefinedCharset.IsPredefinedCharset(num))
    {
      this.charset = new SystemFontCharset(this.File, SystemFontPredefinedCharset.GetPredefinedCodes(num));
    }
    else
    {
      this.charset = new SystemFontCharset(this.File, (long) num, (int) this.CharString.Count);
      this.File.ReadTable((SystemFontCFFTable) this.charset);
    }
  }

  private void ReadCharString()
  {
    this.charString = new SystemFontCharString(this, (long) this.GetInt(SystemFontTop.CharStringsOperator));
    this.File.ReadTable((SystemFontCFFTable) this.charString);
  }
}
