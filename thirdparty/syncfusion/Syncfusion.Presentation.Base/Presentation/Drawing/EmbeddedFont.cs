// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Drawing.EmbeddedFont
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.Drawing;

internal class EmbeddedFont
{
  private string _boldRelationId;
  private string _boldItalicRelationId;
  private string _regularRelationId;
  private string _italicRelationId;
  private string _typeface;

  internal string Typeface => this._typeface;

  internal string BoldRelationId => this._boldRelationId;

  internal string BoldItalicRelationId => this._boldItalicRelationId;

  internal string RegularRelationId => this._regularRelationId;

  internal string ItalicRelationId => this._italicRelationId;

  internal void SetBoldId(string value) => this._boldRelationId = value;

  internal void SetBoldItalicId(string value) => this._boldItalicRelationId = value;

  internal void SetRegularId(string value) => this._regularRelationId = value;

  internal void SetItalicId(string value) => this._italicRelationId = value;

  internal void SetTypeface(string value) => this._typeface = value;
}
