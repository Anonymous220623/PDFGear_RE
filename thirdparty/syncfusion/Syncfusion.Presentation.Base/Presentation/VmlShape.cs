// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.VmlShape
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Drawing;

#nullable disable
namespace Syncfusion.Presentation;

internal class VmlShape
{
  private string _vmlShapeID;
  private string _shapeType;
  private string _style;
  private string _imageRelationId;
  private byte[] _basStream;

  internal string VmlShapeID
  {
    get => this._vmlShapeID;
    set => this._vmlShapeID = value;
  }

  internal string ShapeType
  {
    get => this._shapeType;
    set => this._shapeType = value;
  }

  internal string Style
  {
    get => this._style;
    set => this._style = value;
  }

  internal string ImageRelationId
  {
    get => this._imageRelationId;
    set => this._imageRelationId = value;
  }

  internal byte[] ImageData
  {
    get
    {
      return this._basStream != null && this._basStream.Length > 0 ? (byte[]) this._basStream.Clone() : (byte[]) null;
    }
    set
    {
      if (value == null || value.Length <= 0)
        return;
      this._basStream = value;
    }
  }

  public VmlShape Clone()
  {
    VmlShape vmlShape = (VmlShape) this.MemberwiseClone();
    if (this._basStream != null)
      vmlShape._basStream = Helper.CloneByteArray(this._basStream);
    return vmlShape;
  }
}
