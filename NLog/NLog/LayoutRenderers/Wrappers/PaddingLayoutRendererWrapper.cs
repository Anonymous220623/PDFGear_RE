// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.PaddingLayoutRendererWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using System.ComponentModel;

#nullable disable
namespace NLog.LayoutRenderers.Wrappers;

[LayoutRenderer("pad")]
[AmbientProperty("Padding")]
[AmbientProperty("PadCharacter")]
[AmbientProperty("FixedLength")]
[AmbientProperty("AlignmentOnTruncation")]
[AppDomainFixedOutput]
[ThreadAgnostic]
[ThreadSafe]
public sealed class PaddingLayoutRendererWrapper : WrapperLayoutRendererBase
{
  public PaddingLayoutRendererWrapper() => this.PadCharacter = ' ';

  public int Padding { get; set; }

  [DefaultValue(' ')]
  public char PadCharacter { get; set; }

  [DefaultValue(false)]
  public bool FixedLength { get; set; }

  [DefaultValue(PaddingHorizontalAlignment.Left)]
  public PaddingHorizontalAlignment AlignmentOnTruncation { get; set; }

  protected override string Transform(string text)
  {
    string str = text ?? string.Empty;
    if (this.Padding != 0)
    {
      str = this.Padding <= 0 ? str.PadRight(-this.Padding, this.PadCharacter) : str.PadLeft(this.Padding, this.PadCharacter);
      int length = this.Padding;
      if (length < 0)
        length = -length;
      if (this.FixedLength && str.Length > length)
        str = this.AlignmentOnTruncation != PaddingHorizontalAlignment.Right ? str.Substring(0, length) : str.Substring(str.Length - length);
    }
    return str;
  }
}
