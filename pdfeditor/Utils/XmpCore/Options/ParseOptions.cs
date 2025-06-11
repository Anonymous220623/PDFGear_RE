// Decompiled with JetBrains decompiler
// Type: XmpCore.Options.ParseOptions
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.Collections.Generic;

#nullable disable
namespace XmpCore.Options;

public sealed class ParseOptions : XmpCore.Options.Options
{
  private const int RequireXmpMetaFlag = 1;
  private const int StrictAliasingFlag = 4;
  private const int FixControlCharsFlag = 8;
  private const int AcceptLatin1Flag = 16 /*0x10*/;
  private const int OmitNormalizationFlag = 32 /*0x20*/;
  public const int DisallowDoctypeFlag = 64 /*0x40*/;
  private readonly Dictionary<string, int> mXMPNodesToLimit = new Dictionary<string, int>();

  public ParseOptions() => this.SetOption(88, true);

  public bool RequireXmpMeta
  {
    get => this.GetOption(1);
    set => this.SetOption(1, value);
  }

  public bool StrictAliasing
  {
    get => this.GetOption(4);
    set => this.SetOption(4, value);
  }

  public bool FixControlChars
  {
    get => this.GetOption(8);
    set => this.SetOption(8, value);
  }

  public bool AcceptLatin1
  {
    get => this.GetOption(16 /*0x10*/);
    set => this.SetOption(16 /*0x10*/, value);
  }

  public bool OmitNormalization
  {
    get => this.GetOption(32 /*0x20*/);
    set => this.SetOption(32 /*0x20*/, value);
  }

  public bool DisallowDoctype
  {
    get => this.GetOption(64 /*0x40*/);
    set => this.SetOption(64 /*0x40*/, value);
  }

  public bool AreXMPNodesLimited => this.mXMPNodesToLimit.Count > 0;

  public ParseOptions SetXMPNodesToLimit(Dictionary<string, int> nodeMap)
  {
    foreach (KeyValuePair<string, int> node in nodeMap)
      this.mXMPNodesToLimit[node.Key] = node.Value;
    return this;
  }

  public Dictionary<string, int> GetXMPNodesToLimit()
  {
    return new Dictionary<string, int>((IDictionary<string, int>) this.mXMPNodesToLimit);
  }

  protected override string DefineOptionName(int option)
  {
    switch (option)
    {
      case 1:
        return "REQUIRE_XMP_META";
      case 4:
        return "STRICT_ALIASING";
      case 8:
        return "FIX_CONTROL_CHARS";
      case 16 /*0x10*/:
        return "ACCEPT_LATIN_1";
      case 32 /*0x20*/:
        return "OMIT_NORMALIZATION";
      case 64 /*0x40*/:
        return "DISALLOW_DOCTYPE";
      default:
        return (string) null;
    }
  }

  protected override int GetValidOptions() => 125;
}
