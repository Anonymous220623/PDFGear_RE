// Decompiled with JetBrains decompiler
// Type: XmpCore.Options.SerializeOptions
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.Text;

#nullable disable
namespace XmpCore.Options;

public sealed class SerializeOptions : XmpCore.Options.Options
{
  private static Encoding utf8Encoding;
  public const int OmitPacketWrapperFlag = 16 /*0x10*/;
  public const int ReadonlyPacketFlag = 32 /*0x20*/;
  public const int UseCompactFormatFlag = 64 /*0x40*/;
  public const int UseCanonicalFormatFlag = 128 /*0x80*/;
  public const int UsePlainXmpFlag = 1024 /*0x0400*/;
  public const int IncludeThumbnailPadFlag = 256 /*0x0100*/;
  public const int ExactPacketLengthFlag = 512 /*0x0200*/;
  public const int OmitXmpmetaElementFlag = 4096 /*0x1000*/;
  public const int SortFlag = 8192 /*0x2000*/;
  private const int LittleEndianBit = 1;
  private const int Utf16Bit = 2;
  private const int Utf8BomBit = 8;
  public const int EncodeUtf8 = 0;
  public const int EncodeUtf8WithBomFlag = 8;
  public const int EncodeUtf16BeFlag = 2;
  public const int EncodeUtf16LeFlag = 3;
  private const int EncodingMask = 11;

  public SerializeOptions()
  {
    this.Padding = 2048 /*0x0800*/;
    this.Newline = "\n";
    this.Indent = "  ";
  }

  public SerializeOptions(int options)
    : base(options)
  {
    this.Padding = 2048 /*0x0800*/;
    this.Newline = "\n";
    this.Indent = "  ";
  }

  public bool OmitPacketWrapper
  {
    get => this.GetOption(16 /*0x10*/);
    set => this.SetOption(16 /*0x10*/, value);
  }

  public bool OmitXmpMetaElement
  {
    get => this.GetOption(4096 /*0x1000*/);
    set => this.SetOption(4096 /*0x1000*/, value);
  }

  public bool ReadOnlyPacket
  {
    get => this.GetOption(32 /*0x20*/);
    set => this.SetOption(32 /*0x20*/, value);
  }

  public bool UseCompactFormat
  {
    get => this.GetOption(64 /*0x40*/);
    set => this.SetOption(64 /*0x40*/, value);
  }

  public bool UseCanonicalFormat
  {
    get => this.GetOption(128 /*0x80*/);
    set => this.SetOption(128 /*0x80*/, value);
  }

  public bool UsePlainXmp
  {
    get => this.GetOption(1024 /*0x0400*/);
    set => this.SetOption(1024 /*0x0400*/, value);
  }

  public bool IncludeThumbnailPad
  {
    get => this.GetOption(256 /*0x0100*/);
    set => this.SetOption(256 /*0x0100*/, value);
  }

  public bool ExactPacketLength
  {
    get => this.GetOption(512 /*0x0200*/);
    set => this.SetOption(512 /*0x0200*/, value);
  }

  public bool Sort
  {
    get => this.GetOption(8192 /*0x2000*/);
    set => this.SetOption(8192 /*0x2000*/, value);
  }

  public bool EncodeUtf16Be
  {
    get => (this.GetOptions() & 11) == 2;
    set
    {
      this.SetOption(11, false);
      this.SetOption(2, value);
    }
  }

  public bool EncodeUtf16Le
  {
    get => (this.GetOptions() & 11) == 3;
    set
    {
      this.SetOption(11, false);
      this.SetOption(3, value);
    }
  }

  public bool EncodeUtf8WithBom
  {
    get => (this.GetOptions() & 11) == 8;
    set
    {
      this.SetOption(11, false);
      this.SetOption(8, value);
    }
  }

  public int BaseIndent { set; get; }

  public string Indent { set; get; }

  public string Newline { get; set; }

  public int Padding { get; set; }

  public Encoding GetEncoding()
  {
    if (this.EncodeUtf16Be)
      return Encoding.BigEndianUnicode;
    if (this.EncodeUtf16Le)
      return Encoding.Unicode;
    if (this.EncodeUtf8WithBom)
      return Encoding.UTF8;
    if (SerializeOptions.utf8Encoding == null)
      SerializeOptions.utf8Encoding = (Encoding) new UTF8Encoding(false);
    return SerializeOptions.utf8Encoding;
  }

  public object Clone()
  {
    return (object) new SerializeOptions(this.GetOptions())
    {
      BaseIndent = this.BaseIndent,
      Indent = this.Indent,
      Newline = this.Newline,
      Padding = this.Padding
    };
  }

  protected override string DefineOptionName(int option)
  {
    switch (option)
    {
      case 16 /*0x10*/:
        return "OMIT_PACKET_WRAPPER";
      case 32 /*0x20*/:
        return "READONLY_PACKET";
      case 64 /*0x40*/:
        return "USE_COMPACT_FORMAT";
      case 256 /*0x0100*/:
        return "INCLUDE_THUMBNAIL_PAD";
      case 512 /*0x0200*/:
        return "EXACT_PACKET_LENGTH";
      case 1024 /*0x0400*/:
        return "USE_PLAIN_XMP";
      case 4096 /*0x1000*/:
        return "OMIT_XMPMETA_ELEMENT";
      case 8192 /*0x2000*/:
        return "NORMALIZED";
      default:
        return (string) null;
    }
  }

  protected override int GetValidOptions() => 14192;
}
