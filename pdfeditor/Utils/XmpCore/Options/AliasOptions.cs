// Decompiled with JetBrains decompiler
// Type: XmpCore.Options.AliasOptions
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

#nullable disable
namespace XmpCore.Options;

public sealed class AliasOptions : XmpCore.Options.Options
{
  public const int PropDirect = 0;
  public const int PropArray = 512 /*0x0200*/;
  public const int PropArrayOrdered = 1024 /*0x0400*/;
  public const int PropArrayAlternate = 2048 /*0x0800*/;
  public const int PropArrayAltText = 4096 /*0x1000*/;

  public AliasOptions()
  {
  }

  public AliasOptions(int options)
    : base(options)
  {
  }

  public bool IsSimple() => this.GetOptions() == 0;

  public bool IsArray
  {
    get => this.GetOption(512 /*0x0200*/);
    set => this.SetOption(512 /*0x0200*/, value);
  }

  public bool IsArrayOrdered
  {
    get => this.GetOption(1024 /*0x0400*/);
    set => this.SetOption(1536 /*0x0600*/, value);
  }

  public bool IsArrayAlternate
  {
    get => this.GetOption(2048 /*0x0800*/);
    set => this.SetOption(3584 /*0x0E00*/, value);
  }

  public bool IsArrayAltText
  {
    get => this.GetOption(4096 /*0x1000*/);
    set => this.SetOption(7680, value);
  }

  public PropertyOptions ToPropertyOptions() => new PropertyOptions(this.GetOptions());

  protected override string DefineOptionName(int option)
  {
    switch (option)
    {
      case 0:
        return "PROP_DIRECT";
      case 512 /*0x0200*/:
        return "ARRAY";
      case 1024 /*0x0400*/:
        return "ARRAY_ORDERED";
      case 2048 /*0x0800*/:
        return "ARRAY_ALTERNATE";
      case 4096 /*0x1000*/:
        return "ARRAY_ALT_TEXT";
      default:
        return (string) null;
    }
  }

  protected override int GetValidOptions() => 7680;
}
