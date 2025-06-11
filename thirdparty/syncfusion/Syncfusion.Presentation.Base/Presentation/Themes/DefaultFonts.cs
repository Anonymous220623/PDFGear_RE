// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Themes.DefaultFonts
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Drawing;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.Themes;

internal class DefaultFonts
{
  private string _cs;
  private string _ea;
  private string _latin;
  private Dictionary<string, string> _font;

  internal string Cs
  {
    get => this._cs;
    set => this._cs = value;
  }

  internal Dictionary<string, string> Font
  {
    get => this._font ?? (this._font = new Dictionary<string, string>(32 /*0x20*/));
  }

  internal string Ea
  {
    get => this._ea;
    set => this._ea = value;
  }

  internal string Latin
  {
    get => this._latin;
    set => this._latin = value;
  }

  internal void Close()
  {
    if (this._font == null)
      return;
    this._font.Clear();
    this._font = (Dictionary<string, string>) null;
  }

  public DefaultFonts Clone()
  {
    DefaultFonts defaultFonts = (DefaultFonts) this.MemberwiseClone();
    if (this._font != null)
      defaultFonts._font = Helper.CloneDictionary(this._font);
    return defaultFonts;
  }
}
