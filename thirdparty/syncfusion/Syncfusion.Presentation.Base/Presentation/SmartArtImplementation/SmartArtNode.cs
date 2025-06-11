// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.SmartArtImplementation.SmartArtNode
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Drawing;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.SmartArtImplementation;

internal class SmartArtNode : ISmartArtNode
{
  private SmartArtNodes _childNodes;
  private ISmartArtShapes _shapes;
  private SmartArtNodes _parent;
  private Syncfusion.Presentation.RichText.TextBody _textBody;
  private SmartArtPoint _basePoint;
  private SmartArtPoint _siblingPoint;
  private Guid _id;

  internal SmartArtNode(SmartArtNodes smartArtNodes)
  {
    this._parent = smartArtNodes;
    this._childNodes = new SmartArtNodes((object) this);
    this._shapes = (ISmartArtShapes) new SmartArtShapes((ISmartArtNode) this);
  }

  internal SmartArtNodes ParentNodes => this._parent;

  public object Parent => this._parent.Parent;

  internal SmartArtPoint BasePoint
  {
    get => this._basePoint;
    set => this._basePoint = value;
  }

  public ISmartArtNodes ChildNodes => (ISmartArtNodes) this._childNodes;

  public int Level => this._parent.Parent is SmartArt ? 0 : this.GetLevelFromParent(0);

  private int GetLevelFromParent(int level)
  {
    if (!(this._parent.Parent is SmartArtNode))
      return level;
    ++level;
    return ((SmartArtNode) this._parent.Parent).GetLevelFromParent(level);
  }

  public ISmartArtShapes Shapes
  {
    get
    {
      this._basePoint.HasShapeProperties = true;
      this._siblingPoint.HasShapeProperties = true;
      return this._shapes;
    }
  }

  internal SmartArtPoint SiblingPoint
  {
    get => this._siblingPoint;
    set => this._siblingPoint = value;
  }

  internal Guid Id
  {
    get => this._id;
    set => this._id = value;
  }

  public bool IsAssistant
  {
    get => this._basePoint.Type == SmartArtPointType.AssistantElement;
    set => this._basePoint.Type = SmartArtPointType.AssistantElement;
  }

  public ITextBody TextBody
  {
    get
    {
      this._basePoint.IsPlaceholder = false;
      return (ITextBody) this._textBody;
    }
  }

  internal Syncfusion.Presentation.RichText.TextBody ObtainTextBody() => this._textBody;

  internal void SetTextBody(Syncfusion.Presentation.RichText.TextBody textBody)
  {
    this._textBody = textBody;
  }

  internal SmartArtShapes ObtainShapes() => this._shapes as SmartArtShapes;

  internal void SetShapes(SmartArtShape[] shapes)
  {
    SmartArtShapes shapes1 = this._shapes as SmartArtShapes;
    shapes1.Add(shapes[0]);
    shapes1.Add(shapes[1]);
  }

  internal void Close()
  {
    this._basePoint.Close();
    this._childNodes.Close();
    this._parent = (SmartArtNodes) null;
    foreach (Shape shape in (IEnumerable<ISmartArtShape>) this._shapes)
      shape.Close();
    this._siblingPoint.Close();
    this._textBody.Close();
  }

  public ISmartArtNode Clone()
  {
    SmartArtNode smartArtNode = (SmartArtNode) this.MemberwiseClone();
    smartArtNode._basePoint = this._basePoint.Clone();
    smartArtNode._childNodes = this._childNodes.Clone();
    smartArtNode._shapes = ((SmartArtShapes) this._shapes).Clone();
    smartArtNode._siblingPoint = this._siblingPoint.Clone();
    smartArtNode._textBody = this._textBody.Clone();
    return (ISmartArtNode) smartArtNode;
  }

  internal void SetParent(SmartArtNodes parent)
  {
    this._parent = parent;
    this._childNodes.SetParent(parent.Parent as SmartArt);
    if (parent.Parent is SmartArt)
    {
      this._siblingPoint.SetParent((parent.Parent as SmartArt).DataModel);
      this._basePoint.SetParent((parent.Parent as SmartArt).DataModel);
    }
    ((SmartArtShapes) this._shapes).SetParent(this);
    this._textBody.SetParent((Shape) (parent.Parent as SmartArt));
  }
}
