// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Relation
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation;

internal class Relation
{
  private string _id;
  private string _target;
  private string _targetMode;
  private string _type;

  internal Relation()
  {
    this._id = (string) null;
    this._type = (string) null;
    this._target = (string) null;
    this._targetMode = (string) null;
  }

  internal Relation(string id, string type, string target, string targetMode)
  {
    this._id = id;
    this._type = type;
    this._target = target;
    this._targetMode = targetMode;
  }

  public Relation(Relation relation)
  {
    this._id = relation._id;
    this._target = relation._target;
    this._targetMode = relation._targetMode;
    this._type = relation._type;
  }

  internal string Type
  {
    get => this._type;
    set => this._type = value;
  }

  internal string TargetMode
  {
    get => this._targetMode;
    set => this._targetMode = value;
  }

  internal string Target
  {
    get => this._target;
    set => this._target = value;
  }

  internal string Id
  {
    get => this._id;
    set => this._id = value;
  }

  public Relation Clone() => new Relation(this);
}
