// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.TextFormat
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class TextFormat
{
  internal const short FontSizeKey = 0;
  internal const short FontFamilyKey = 1;
  internal const short BoldKey = 2;
  internal const short UnderlineKey = 3;
  internal const short ItalicKey = 4;
  internal const short StrikeKey = 5;
  internal const short FontColorKey = 6;
  internal const short SuperScriptKey = 7;
  internal const short SubScriptKey = 8;
  internal const short BackColorKey = 9;
  internal const short DisplayKey = 10;
  internal const short TextAlignKey = 11;
  internal const short HorizontalAlignKey = 12;
  internal const short VerticalAlignKey = 13;
  internal Dictionary<int, object> formattingProperty;

  internal TextFormat() => this.formattingProperty = new Dictionary<int, object>();

  internal TextFormat Clone()
  {
    return new TextFormat()
    {
      formattingProperty = new Dictionary<int, object>((IDictionary<int, object>) this.formattingProperty)
    };
  }

  internal bool HasKey(int Key) => this.formattingProperty.ContainsKey(Key);

  internal bool HasValue(int key) => this.formattingProperty.ContainsKey(key);

  private void SetPropertyValue(int Key, bool value)
  {
    if (!this.formattingProperty.ContainsKey(Key))
      this.formattingProperty.Add(Key, (object) value);
    else
      this.formattingProperty[Key] = (object) value;
  }

  private void SetPropertyValue(int Key, object value)
  {
    if (!this.formattingProperty.ContainsKey(Key))
      this.formattingProperty.Add(Key, value);
    else
      this.formattingProperty[Key] = value;
  }

  internal string TextAlignment
  {
    get => this.HasKey(11) ? (string) this.formattingProperty[11] : string.Empty;
    set => this.SetPropertyValue(11, (object) value);
  }

  internal string HorizantalAlignment
  {
    get => this.HasKey(10) ? (string) this.formattingProperty[12] : string.Empty;
    set => this.SetPropertyValue(12, (object) value);
  }

  internal string VerticalAlignment
  {
    get => this.HasKey(13) ? (string) this.formattingProperty[13] : string.Empty;
    set => this.SetPropertyValue(13, (object) value);
  }

  internal bool Display
  {
    get => !this.HasKey(10) || (bool) this.formattingProperty[10];
    set => this.SetPropertyValue(10, value);
  }

  internal bool SuperScript
  {
    get => this.HasKey(7) && (bool) this.formattingProperty[7];
    set => this.SetPropertyValue(7, value);
  }

  internal bool SubScript
  {
    get => this.HasKey(8) && (bool) this.formattingProperty[8];
    set => this.SetPropertyValue(8, value);
  }

  internal bool Bold
  {
    get => this.HasKey(2) && (bool) this.formattingProperty[2];
    set => this.SetPropertyValue(2, value);
  }

  internal bool Italic
  {
    get => this.HasKey(4) && (bool) this.formattingProperty[4];
    set => this.SetPropertyValue(4, value);
  }

  internal bool Underline
  {
    get => this.HasKey(3) && (bool) this.formattingProperty[3];
    set => this.SetPropertyValue(3, value);
  }

  internal bool Strike
  {
    get => this.HasKey(5) && (bool) this.formattingProperty[5];
    set => this.SetPropertyValue(5, value);
  }

  internal Color FontColor
  {
    get => this.HasKey(6) ? (Color) this.formattingProperty[6] : Color.Empty;
    set => this.SetPropertyValue(6, (object) value);
  }

  internal float FontSize
  {
    get => this.HasKey(0) ? (float) this.formattingProperty[0] : 12f;
    set => this.SetPropertyValue(0, (object) value);
  }

  internal string FontFamily
  {
    get => this.HasKey(1) ? (string) this.formattingProperty[1] : string.Empty;
    set => this.SetPropertyValue(1, (object) value);
  }

  internal Color BackColor
  {
    get => this.HasKey(9) ? (Color) this.formattingProperty[9] : Color.Empty;
    set => this.SetPropertyValue(9, (object) value);
  }
}
