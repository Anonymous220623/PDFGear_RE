// Decompiled with JetBrains decompiler
// Type: FontSettings
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Office;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;

#nullable disable
public class FontSettings
{
  internal Dictionary<string, Stream> FontStreams = new Dictionary<string, Stream>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
  internal Dictionary<string, Font> FontCollection = new Dictionary<string, Font>();
  internal PrivateFontCollection PrivateFonts = new PrivateFontCollection();
  private List<FallbackFont> _fallbackFonts;

  public event SubstituteFontEventHandler SubstituteFont;

  public List<FallbackFont> FallbackFonts
  {
    get => this._fallbackFonts ?? (this._fallbackFonts = new List<FallbackFont>());
  }

  public void InitializeFallbackFonts()
  {
    this.FallbackFonts.Add(new FallbackFont(1536U /*0x0600*/, 1791U /*0x06FF*/, "Arial, Times New Roman, Microsoft Uighur"));
    this.FallbackFonts.Add(new FallbackFont(1872U, 1919U, "Arial, Times New Roman, Microsoft Uighur"));
    this.FallbackFonts.Add(new FallbackFont(2208U, 2303U /*0x08FF*/, "Arial, Times New Roman, Microsoft Uighur"));
    this.FallbackFonts.Add(new FallbackFont(64336U, 65023U, "Arial, Times New Roman, Microsoft Uighur"));
    this.FallbackFonts.Add(new FallbackFont(65136U, 65279U, "Arial, Times New Roman, Microsoft Uighur"));
    this.FallbackFonts.Add(new FallbackFont(1424U, 1535U /*0x05FF*/, "Arial, Times New Roman, David"));
    this.FallbackFonts.Add(new FallbackFont(64285U, 64335U, "Arial, Times New Roman, David"));
    this.FallbackFonts.Add(new FallbackFont(2304U /*0x0900*/, 2431U, "Mangal, Utsaah"));
    this.FallbackFonts.Add(new FallbackFont(43232U, 43263U, "Mangal, Utsaah"));
    this.FallbackFonts.Add(new FallbackFont(7376U, 7423U, "Mangal, Utsaah"));
    this.FallbackFonts.Add(new FallbackFont(19968U, 40959U /*0x9FFF*/, "DengXian, MingLiU, MS Gothic"));
    this.FallbackFonts.Add(new FallbackFont(13312U, 19903U, "DengXian, MingLiU, MS Gothic"));
    this.FallbackFonts.Add(new FallbackFont(55360U, 55401U, "DengXian, MingLiU, MS Gothic"));
    this.FallbackFonts.Add(new FallbackFont(56320U, 57055U, "DengXian, MingLiU, MS Gothic"));
    this.FallbackFonts.Add(new FallbackFont(43360U, 43391U, "DengXian, MingLiU, MS Gothic"));
    this.FallbackFonts.Add(new FallbackFont(65280U, 65519U, "DengXian, MingLiU, MS Gothic"));
    this.FallbackFonts.Add(new FallbackFont(12288U /*0x3000*/, 12351U, "DengXian, MingLiU, MS Gothic"));
    this.FallbackFonts.Add(new FallbackFont(12448U, 12543U, "Yu Mincho, MS Mincho"));
    this.FallbackFonts.Add(new FallbackFont(12352U, 12447U, "Yu Mincho, MS Mincho"));
    this.FallbackFonts.Add(new FallbackFont(44032U, 55203U, "Malgun Gothic, Batang"));
    this.FallbackFonts.Add(new FallbackFont(4352U, 4607U, "Malgun Gothic, Batang"));
    this.FallbackFonts.Add(new FallbackFont(12592U, 12687U, "Malgun Gothic, Batang"));
    this.FallbackFonts.Add(new FallbackFont(43360U, 43391U, "Malgun Gothic, Batang"));
    this.FallbackFonts.Add(new FallbackFont(55216U, 55295U, "Malgun Gothic, Batang"));
    this.FallbackFonts.Add(new FallbackFont(44032U, 55215U, "Malgun Gothic, Batang"));
  }

  internal Font GetFont(string fontName, float fontSize, FontStyle fontStyle)
  {
    string key = $"{fontName};{(object) fontStyle};{(object) fontSize}";
    Font font;
    if (this.FontCollection.ContainsKey(key))
    {
      font = this.FontCollection[key];
    }
    else
    {
      font = this.CreateFont(fontName, fontSize, fontStyle);
      if (fontName.Equals(font.Name.ToLower(), StringComparison.InvariantCultureIgnoreCase))
        this.FontCollection.Add(key, font);
    }
    if ((font.Name.ToLower() != fontName.ToLower() || font.Style != fontStyle) && this.SubstituteFont != null)
    {
      SubstituteFontEventArgs args = new SubstituteFontEventArgs(fontName, "Microsoft Sans Serif", fontStyle);
      while (this.SubstituteFont != null)
      {
        string alternateFontName = args.AlternateFontName;
        if (!this.FontStreams.ContainsKey($"{fontName}_{fontStyle.ToString().ToLower()}"))
          this.SubstituteFont((object) this, args);
        if (args.AlternateFontStream != null && args.AlternateFontStream.Length > 0L)
        {
          Stream alternateFontStream = args.AlternateFontStream;
          if (this.FontStreams != null)
          {
            if (!this.FontStreams.ContainsKey($"{fontName}_{fontStyle.ToString().ToLower()}"))
            {
              alternateFontStream.Position = 0L;
              byte[] numArray = new byte[alternateFontStream.Length];
              alternateFontStream.Position = 0L;
              alternateFontStream.Read(numArray, 0, (int) alternateFontStream.Length);
              IntPtr num = Marshal.AllocCoTaskMem((int) alternateFontStream.Length);
              Marshal.Copy(numArray, 0, num, (int) alternateFontStream.Length);
              this.PrivateFonts.AddMemoryFont(num, (int) alternateFontStream.Length);
              alternateFontStream.Position = 0L;
              this.FontStreams.Add($"{args.OriginalFontName}_{args.FontStyle.ToString().ToLower()}", alternateFontStream);
            }
            else
              this.FontStreams[$"{args.OriginalFontName}_{args.FontStyle.ToString().ToLower()}"] = alternateFontStream;
          }
          Font privateFont = this.GetPrivateFont(fontName, fontSize, fontStyle);
          if (privateFont.Name != fontName)
            return this.GetPrivateFont(args.AlternateFontName, fontSize, fontStyle);
          if (!this.FontCollection.ContainsKey(key))
            this.FontCollection.Add(key, privateFont);
          return font;
        }
        font = this.CreateFont(args.AlternateFontName, fontSize, fontStyle);
        if (font.Name == args.AlternateFontName || alternateFontName != "Microsoft Sans Serif" || alternateFontName == args.AlternateFontName)
          break;
      }
    }
    else if (this.PrivateFonts.Families.Length > 0 && !font.Name.Equals(fontName, StringComparison.OrdinalIgnoreCase))
      return this.GetPrivateFont(fontName, fontSize, fontStyle);
    return font;
  }

  private Font GetPrivateFont(string FontName, float fontsize, FontStyle style)
  {
    foreach (FontFamily family in this.PrivateFonts.Families)
    {
      if (family.Name.Equals(FontName, StringComparison.OrdinalIgnoreCase))
        return new Font(family, fontsize, style);
    }
    return new Font(FontName, fontsize, style);
  }

  private Font CreateFont(string fontName, float fontSize, FontStyle fontStyle)
  {
    try
    {
      Font font = new Font(fontName, fontSize, fontStyle);
      if (!fontName.Equals(font.Name, StringComparison.OrdinalIgnoreCase) && this.FontStreams.ContainsKey($"{fontName}_{fontStyle.ToString().ToLower()}"))
        font = this.GetPrivateFont(fontName, fontSize, fontStyle);
      return font;
    }
    catch
    {
      FontFamily fontFamily = new FontFamily(fontName);
      if (fontFamily.IsStyleAvailable(FontStyle.Bold))
        fontStyle |= FontStyle.Bold;
      if (fontFamily.IsStyleAvailable(FontStyle.Italic))
        fontStyle |= FontStyle.Italic;
      if (fontFamily.IsStyleAvailable(FontStyle.Underline))
        fontStyle |= FontStyle.Underline;
      if (fontFamily.IsStyleAvailable(FontStyle.Strikeout))
        fontStyle |= FontStyle.Strikeout;
      return new Font(fontName, fontSize, fontStyle);
    }
  }

  internal void Close()
  {
    if (this.FontCollection != null)
    {
      foreach (string key in this.FontCollection.Keys)
        this.FontCollection[key]?.Dispose();
      this.FontCollection.Clear();
      this.FontCollection = (Dictionary<string, Font>) null;
    }
    if (this.PrivateFonts != null)
    {
      this.PrivateFonts.Dispose();
      this.PrivateFonts = (PrivateFontCollection) null;
    }
    if (this.FontStreams != null)
    {
      foreach (string key in this.FontStreams.Keys)
        this.FontStreams[key]?.Dispose();
      this.FontStreams.Clear();
      this.FontStreams = (Dictionary<string, Stream>) null;
    }
    if (this.SubstituteFont != null)
      this.SubstituteFont = (SubstituteFontEventHandler) null;
    if (this._fallbackFonts == null)
      return;
    this._fallbackFonts.Clear();
    this._fallbackFonts = (List<FallbackFont>) null;
  }
}
