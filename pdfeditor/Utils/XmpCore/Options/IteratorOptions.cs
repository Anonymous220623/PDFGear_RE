// Decompiled with JetBrains decompiler
// Type: XmpCore.Options.IteratorOptions
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

#nullable disable
namespace XmpCore.Options;

public sealed class IteratorOptions : XmpCore.Options.Options
{
  public const int JustChildren = 256 /*0x0100*/;
  public const int JustLeafNodes = 512 /*0x0200*/;
  public const int JustLeafName = 1024 /*0x0400*/;
  public const int OmitQualifiers = 4096 /*0x1000*/;

  public bool IsJustChildren
  {
    get => this.GetOption(256 /*0x0100*/);
    set => this.SetOption(256 /*0x0100*/, value);
  }

  public bool IsJustLeafName
  {
    get => this.GetOption(1024 /*0x0400*/);
    set => this.SetOption(1024 /*0x0400*/, value);
  }

  public bool IsJustLeafNodes
  {
    get => this.GetOption(512 /*0x0200*/);
    set => this.SetOption(512 /*0x0200*/, value);
  }

  public bool IsOmitQualifiers
  {
    get => this.GetOption(4096 /*0x1000*/);
    set => this.SetOption(4096 /*0x1000*/, value);
  }

  protected override string DefineOptionName(int option)
  {
    switch (option)
    {
      case 256 /*0x0100*/:
        return "JUST_CHILDREN";
      case 512 /*0x0200*/:
        return "JUST_LEAFNODES";
      case 1024 /*0x0400*/:
        return "JUST_LEAFNAME";
      case 4096 /*0x1000*/:
        return "OMIT_QUALIFIERS";
      default:
        return (string) null;
    }
  }

  protected override int GetValidOptions() => 5888;
}
