// Decompiled with JetBrains decompiler
// Type: XmpCore.Options.PropertyOptions
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

#nullable disable
namespace XmpCore.Options;

public sealed class PropertyOptions : XmpCore.Options.Options
{
  internal const int NoOptionsFlag = 0;
  internal const int IsUriFlag = 2;
  internal const int HasQualifiersFlag = 16 /*0x10*/;
  internal const int QualifierFlag = 32 /*0x20*/;
  internal const int HasLanguageFlag = 64 /*0x40*/;
  internal const int HasTypeFlag = 128 /*0x80*/;
  internal const int StructFlag = 256 /*0x0100*/;
  internal const int ArrayFlag = 512 /*0x0200*/;
  internal const int ArrayOrderedFlag = 1024 /*0x0400*/;
  internal const int ArrayAlternateFlag = 2048 /*0x0800*/;
  internal const int ArrayAltTextFlag = 4096 /*0x1000*/;
  internal const int SchemaNodeFlag = -2147483648 /*0x80000000*/;
  internal const int DeleteExisting = 536870912 /*0x20000000*/;
  private int arrayElementsLimit = -1;

  public PropertyOptions()
  {
  }

  public PropertyOptions(int options)
    : base(options)
  {
  }

  public bool IsUri
  {
    get => this.GetOption(2);
    set => this.SetOption(2, value);
  }

  public bool HasQualifiers
  {
    get => this.GetOption(16 /*0x10*/);
    set => this.SetOption(16 /*0x10*/, value);
  }

  public bool IsQualifier
  {
    get => this.GetOption(32 /*0x20*/);
    set => this.SetOption(32 /*0x20*/, value);
  }

  public bool HasLanguage
  {
    get => this.GetOption(64 /*0x40*/);
    set => this.SetOption(64 /*0x40*/, value);
  }

  public bool HasType
  {
    get => this.GetOption(128 /*0x80*/);
    set => this.SetOption(128 /*0x80*/, value);
  }

  public bool IsStruct
  {
    get => this.GetOption(256 /*0x0100*/);
    set => this.SetOption(256 /*0x0100*/, value);
  }

  public bool IsArray
  {
    get => this.GetOption(512 /*0x0200*/);
    set => this.SetOption(512 /*0x0200*/, value);
  }

  public bool IsArrayOrdered
  {
    get => this.GetOption(1024 /*0x0400*/);
    set => this.SetOption(1024 /*0x0400*/, value);
  }

  public bool IsArrayAlternate
  {
    get => this.GetOption(2048 /*0x0800*/);
    set => this.SetOption(2048 /*0x0800*/, value);
  }

  public bool IsArrayAltText
  {
    get => this.GetOption(4096 /*0x1000*/);
    set => this.SetOption(4096 /*0x1000*/, value);
  }

  public bool IsArrayLimited => this.arrayElementsLimit != -1;

  public PropertyOptions SetArrayElementLimit(int arrayLimit)
  {
    this.arrayElementsLimit = arrayLimit;
    return this;
  }

  public int ArrayElementsLimit => this.arrayElementsLimit;

  public bool IsSchemaNode
  {
    get => this.GetOption(int.MinValue);
    set => this.SetOption(int.MinValue, value);
  }

  public bool IsCompositeProperty => (this.GetOptions() & 768 /*0x0300*/) > 0;

  public bool IsSimple => !this.IsCompositeProperty;

  public bool EqualArrayTypes(PropertyOptions options)
  {
    return this.IsArray == options.IsArray && this.IsArrayOrdered == options.IsArrayOrdered && this.IsArrayAlternate == options.IsArrayAlternate && this.IsArrayAltText == options.IsArrayAltText;
  }

  public void MergeWith(PropertyOptions options)
  {
    if (options == null)
      return;
    this.SetOptions(this.GetOptions() | options.GetOptions());
  }

  public bool IsOnlyArrayOptions => (this.GetOptions() & -7681) == 0;

  protected override int GetValidOptions() => -1610604558;

  protected override string DefineOptionName(int option)
  {
    switch (option)
    {
      case int.MinValue:
        return "SCHEMA_NODE";
      case 2:
        return "URI";
      case 16 /*0x10*/:
        return "HAS_QUALIFIER";
      case 32 /*0x20*/:
        return "QUALIFIER";
      case 64 /*0x40*/:
        return "HAS_LANGUAGE";
      case 128 /*0x80*/:
        return "HAS_TYPE";
      case 256 /*0x0100*/:
        return "STRUCT";
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

  internal override void AssertConsistency(int options)
  {
    if ((options & 256 /*0x0100*/) > 0 && (options & 512 /*0x0200*/) > 0)
      throw new XmpException("IsStruct and IsArray options are mutually exclusive", XmpErrorCode.BadOptions);
    if ((options & 2) > 0 && (options & 768 /*0x0300*/) > 0)
      throw new XmpException("Structs and arrays can't have \"value\" options", XmpErrorCode.BadOptions);
  }
}
