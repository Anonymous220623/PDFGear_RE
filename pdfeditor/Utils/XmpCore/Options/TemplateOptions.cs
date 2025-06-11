// Decompiled with JetBrains decompiler
// Type: XmpCore.Options.TemplateOptions
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

#nullable disable
namespace XmpCore.Options;

public sealed class TemplateOptions : XmpCore.Options.Options
{
  public const int ClearUnnamedPropertiesFlag = 2;
  public const int ReplaceExistingPropertiesFlag = 16 /*0x10*/;
  public const int IncludeInternalPropertiesFlag = 32 /*0x20*/;
  public const int AddNewPropertiesFlag = 64 /*0x40*/;
  public const int ReplaceWithDeleteEmptyFlag = 128 /*0x80*/;

  public TemplateOptions()
  {
  }

  public TemplateOptions(int options)
    : base(options)
  {
  }

  public bool ClearUnnamedProperties
  {
    get => this.GetOption(2);
    set => this.SetOption(2, value);
  }

  public bool ReplaceExistingProperties
  {
    get => this.GetOption(16 /*0x10*/);
    set => this.SetOption(16 /*0x10*/, value);
  }

  public bool IncludeInternalProperties
  {
    get => this.GetOption(32 /*0x20*/);
    set => this.SetOption(32 /*0x20*/, value);
  }

  public bool AddNewProperties
  {
    get => this.GetOption(64 /*0x40*/);
    set => this.SetOption(64 /*0x40*/, value);
  }

  public bool ReplaceWithDeleteEmpty
  {
    get => this.GetOption(128 /*0x80*/);
    set => this.SetOption(128 /*0x80*/, value);
  }

  public object Clone() => (object) new TemplateOptions(this.GetOptions());

  protected override string DefineOptionName(int option)
  {
    switch (option)
    {
      case 2:
        return "CLEAR_UNNAMED_PROPERTIES";
      case 16 /*0x10*/:
        return "REPLACE_EXISTING_PROPERTIES";
      case 32 /*0x20*/:
        return "INCLUDE_INTERNAL_PROPERTIES";
      case 64 /*0x40*/:
        return "ADD_NEW_PROPERTIES";
      case 128 /*0x80*/:
        return "REPLACE_WITH_DELETE_EMPTY";
      default:
        return (string) null;
    }
  }

  protected override int GetValidOptions() => 242;
}
