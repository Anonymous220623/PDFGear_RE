// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontType1Font
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontType1Font : SystemFontPostScriptObject, ISystemFontBuildCharHolder
{
  private readonly SystemFontBuildChar buildChar;
  private readonly Dictionary<string, SystemFontType1GlyphData> glyphOutlines;
  private readonly SystemFontProperty<SystemFontPostScriptArray> fontMatrix;
  private readonly SystemFontProperty<SystemFontFontInfo> fontInfo;
  private readonly SystemFontProperty<SystemFontPostScriptArray> encoding;
  private readonly SystemFontProperty<SystemFontPostScriptDictionary> charStrings;
  private readonly SystemFontProperty<SystemFontPrivateType1Format> priv;

  public SystemFontPostScriptArray FontMatrix => this.fontMatrix.GetValue();

  public SystemFontFontInfo FontInfo => this.fontInfo.GetValue();

  public SystemFontPostScriptArray Encoding => this.encoding.GetValue();

  public SystemFontPostScriptDictionary CharStrings => this.charStrings.GetValue();

  public SystemFontPrivateType1Format Private => this.priv.GetValue();

  public SystemFontType1Font()
  {
    this.buildChar = new SystemFontBuildChar((ISystemFontBuildCharHolder) this);
    this.glyphOutlines = new Dictionary<string, SystemFontType1GlyphData>();
    this.fontMatrix = this.CreateProperty<SystemFontPostScriptArray>(new SystemFontPropertyDescriptor()
    {
      Name = nameof (FontMatrix)
    }, SystemFontPostScriptArray.MatrixIdentity);
    this.fontInfo = this.CreateProperty<SystemFontFontInfo>(new SystemFontPropertyDescriptor()
    {
      Name = nameof (FontInfo)
    }, new SystemFontFontInfo());
    this.encoding = this.CreateProperty<SystemFontPostScriptArray>(new SystemFontPropertyDescriptor()
    {
      Name = nameof (Encoding)
    }, (ISystemFontConverter) SystemFontType1Converters.EncodingConverter, SystemFontCFFPredefinedEncoding.StandardEncoding.ToArray());
    this.charStrings = this.CreateProperty<SystemFontPostScriptDictionary>(new SystemFontPropertyDescriptor()
    {
      Name = nameof (CharStrings)
    });
    this.priv = this.CreateProperty<SystemFontPrivateType1Format>(new SystemFontPropertyDescriptor()
    {
      Name = nameof (Private)
    }, (ISystemFontConverter) SystemFontType1Converters.PostScriptObjectConverter);
  }

  public ushort GetAdvancedWidth(SystemFontGlyph glyph)
  {
    return this.GetGlyphData(glyph.Name).AdvancedWidth;
  }

  public void GetGlyphOutlines(SystemFontGlyph glyph, double fontSize)
  {
    SystemFontGlyphOutlinesCollection outlinesCollection = this.GetGlyphData(glyph.Name).Oultlines.Clone();
    SystemFontMatrix matrix = this.FontMatrix.ToMatrix();
    matrix.ScaleAppend(fontSize, -fontSize, 0.0, 0.0);
    outlinesCollection.Transform(matrix);
    glyph.Outlines = outlinesCollection;
  }

  public SystemFontType1GlyphData GetGlyphData(string name)
  {
    SystemFontType1GlyphData glyphData;
    if (!this.glyphOutlines.TryGetValue(name, out glyphData))
    {
      glyphData = this.ReadGlyphData(name);
      this.glyphOutlines[name] = glyphData;
    }
    return glyphData;
  }

  public byte[] GetSubr(int index) => this.Private.GetSubr(index);

  public byte[] GetGlobalSubr(int index) => throw new NotImplementedException();

  internal string GetGlyphName(ushort cid)
  {
    return this.Encoding == null ? ".notdef" : this.Encoding.GetElementAs<string>((int) cid);
  }

  private SystemFontType1GlyphData ReadGlyphData(string name)
  {
    SystemFontPostScriptString elementAs = this.CharStrings.GetElementAs<SystemFontPostScriptString>(name);
    if (elementAs == null)
      return new SystemFontType1GlyphData(new SystemFontGlyphOutlinesCollection(), new ushort?((ushort) 0));
    this.buildChar.Execute(elementAs.ToByteArray());
    SystemFontGlyphOutlinesCollection glyphOutlines = this.buildChar.GlyphOutlines;
    int? width = this.buildChar.Width;
    return new SystemFontType1GlyphData(glyphOutlines, width.HasValue ? new ushort?((ushort) width.GetValueOrDefault()) : new ushort?());
  }

  byte[] ISystemFontBuildCharHolder.GetSubr(int index) => throw new NotImplementedException();

  byte[] ISystemFontBuildCharHolder.GetGlobalSubr(int index) => throw new NotImplementedException();

  SystemFontType1GlyphData ISystemFontBuildCharHolder.GetGlyphData(string glyphName)
  {
    throw new NotImplementedException();
  }
}
