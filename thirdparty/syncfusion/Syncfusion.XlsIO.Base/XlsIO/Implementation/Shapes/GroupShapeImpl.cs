// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Shapes.GroupShapeImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Shapes;

public class GroupShapeImpl : ShapeImpl, IGroupShape, IShape, IParentApplication
{
  private IShape[] m_items;
  private bool m_flipVertical;
  private bool m_flipHorizontal;
  private new Dictionary<string, Stream> m_preservedElements;

  internal GroupShapeImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.m_bSupportOptions = true;
    this.ShapeType = ExcelShapeType.Group;
  }

  public IShape[] Items
  {
    get => this.m_items;
    internal set => this.m_items = value;
  }

  internal bool FlipVertical
  {
    get => this.m_flipVertical;
    set => this.m_flipVertical = value;
  }

  internal bool FlipHorizontal
  {
    get => this.m_flipHorizontal;
    set => this.m_flipHorizontal = value;
  }

  internal new Dictionary<string, Stream> PreservedElements
  {
    get
    {
      if (this.m_preservedElements == null)
        this.m_preservedElements = new Dictionary<string, Stream>();
      return this.m_preservedElements;
    }
  }

  public override IShape Clone(
    object parent,
    Dictionary<string, string> hashNewNames,
    Dictionary<int, int> dicFontIndexes,
    bool addToCollections)
  {
    GroupShapeImpl parent1 = (GroupShapeImpl) base.Clone(parent, hashNewNames, dicFontIndexes, addToCollections);
    if (this.m_items != null && this.m_items.Length > 0)
    {
      List<IShape> shapeList = new List<IShape>();
      for (int index = 0; index < this.m_items.Length; ++index)
      {
        IShape shape = (this.m_items[index] as ShapeImpl).Clone((object) parent1, hashNewNames, dicFontIndexes, false);
        shapeList.Add(shape);
      }
      parent1.m_items = shapeList.ToArray();
    }
    parent1.m_preservedElements = CloneUtils.CloneHash<string, Stream>(this.PreservedElements);
    return (IShape) parent1;
  }

  internal void LayoutGroupShape(bool isAll)
  {
    this.UpdateGroupFrame(isAll);
    if (this.Items == null)
      return;
    foreach (ShapeImpl shapeImpl in this.Items)
    {
      if (shapeImpl.IsGroup)
      {
        (shapeImpl as GroupShapeImpl).LayoutGroupShape(isAll);
      }
      else
      {
        shapeImpl.UpdateGroupFrame(isAll);
        if (shapeImpl.GroupFrame != null)
        {
          RectangleF rect = new RectangleF((float) shapeImpl.GroupFrame.OffsetX, (float) shapeImpl.GroupFrame.OffsetY, (float) shapeImpl.GroupFrame.OffsetCX, (float) shapeImpl.GroupFrame.OffsetCY);
          rect = this.UpdateShapeBounds(rect, shapeImpl.GroupFrame.Rotation / 60000);
          shapeImpl.GroupFrame.SetAnchor(shapeImpl.GroupFrame.Rotation, (long) rect.X, (long) rect.Y, (long) rect.Width, (long) rect.Height);
        }
      }
    }
  }

  internal void LayoutGroupShape()
  {
    if (this.Items == null)
      return;
    foreach (ShapeImpl shapeImpl in this.Items)
    {
      if (shapeImpl.IsGroup)
      {
        shapeImpl.UpdateGroupFrame();
        (shapeImpl as GroupShapeImpl).LayoutGroupShape();
      }
      else
        shapeImpl.UpdateGroupFrame();
    }
  }

  internal void SetUpdatedChildOffset()
  {
    this.ShapeFrame.SetChildAnchor(this.ShapeFrame.OffsetX, this.ShapeFrame.OffsetY, this.ShapeFrame.OffsetCX, this.ShapeFrame.OffsetCY);
    if (this.Items == null)
      return;
    foreach (ShapeImpl shapeImpl in this.Items)
    {
      if (shapeImpl.IsGroup)
      {
        if (shapeImpl.GroupFrame != null)
        {
          shapeImpl.SetPostion(shapeImpl.GroupFrame.OffsetX, shapeImpl.GroupFrame.OffsetY, shapeImpl.GroupFrame.OffsetCX, shapeImpl.GroupFrame.OffsetCY);
          shapeImpl.ShapeFrame.SetAnchor(shapeImpl.GroupFrame.Rotation, shapeImpl.GroupFrame.OffsetX, shapeImpl.GroupFrame.OffsetY, shapeImpl.GroupFrame.OffsetCX, shapeImpl.GroupFrame.OffsetCY);
        }
        (shapeImpl as GroupShapeImpl).SetUpdatedChildOffset();
      }
      else if (shapeImpl.GroupFrame != null)
      {
        shapeImpl.SetPostion(shapeImpl.GroupFrame.OffsetX, shapeImpl.GroupFrame.OffsetY, shapeImpl.GroupFrame.OffsetCX, shapeImpl.GroupFrame.OffsetCY);
        shapeImpl.ShapeFrame.SetAnchor(shapeImpl.GroupFrame.Rotation, shapeImpl.GroupFrame.OffsetX, shapeImpl.GroupFrame.OffsetY, shapeImpl.GroupFrame.OffsetCX, shapeImpl.GroupFrame.OffsetCY);
        if (shapeImpl is AutoShapeImpl)
          (shapeImpl as AutoShapeImpl).ShapeExt.Coordinates = new Rectangle((int) shapeImpl.GroupFrame.OffsetX, (int) shapeImpl.GroupFrame.OffsetY, (int) shapeImpl.GroupFrame.OffsetCX, (int) shapeImpl.GroupFrame.OffsetCY);
      }
    }
  }

  internal bool RemoveGroupShapeItem(IShape shape)
  {
    for (int index1 = 0; index1 < this.Items.Length; ++index1)
    {
      if (this.Items[index1].ShapeType == ExcelShapeType.Group && (this.Items[index1] as IGroupShape as GroupShapeImpl).RemoveGroupShapeItem(shape))
        return true;
      if (this.Items[index1] == shape)
      {
        for (int index2 = index1; index2 < this.Items.Length - 1; ++index2)
          this.Items[index2] = this.Items[index2 + 1];
        IShape[] items = this.Items;
        Array.Resize<IShape>(ref items, items.Length - 1);
        if (this.FindParent(typeof (ShapesCollection)) is ShapesCollection parent)
        {
          parent.Group(this, items, false);
          if (items.Length <= 1)
            parent.Ungroup((IGroupShape) this);
        }
        return true;
      }
    }
    return false;
  }
}
