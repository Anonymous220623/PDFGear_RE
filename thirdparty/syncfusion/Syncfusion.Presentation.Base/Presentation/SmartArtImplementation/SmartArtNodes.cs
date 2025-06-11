// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.SmartArtImplementation.SmartArtNodes
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.RichText;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.SmartArtImplementation;

internal class SmartArtNodes : ISmartArtNodes, IEnumerable<ISmartArtNode>, IEnumerable
{
  private List<ISmartArtNode> _list;
  private object _parent;

  internal SmartArtNodes(object parent)
  {
    this._parent = parent;
    this._list = new List<ISmartArtNode>();
  }

  internal object Parent => this._parent;

  public int IndexOf(ISmartArtNode smartArtNode) => this._list.IndexOf(smartArtNode);

  public int Count => this._list.Count;

  public ISmartArtNode Add()
  {
    SmartArt parentSmartArt = this.GetParentSmartArt();
    switch (parentSmartArt.Layout)
    {
      case SmartArtType.TableList:
      case SmartArtType.RadialCycle:
      case SmartArtType.BasicRadial:
      case SmartArtType.DivergingRadial:
      case SmartArtType.RadialVenn:
      case SmartArtType.ConvergingRadial:
      case SmartArtType.RadialList:
        if (this._parent is SmartArt && this._list.Count == 1)
          throw new ArgumentException($"Node limit exceed for {parentSmartArt.Layout.ToString()} smart art type");
        break;
      case SmartArtType.StaggeredProcess:
      case SmartArtType.UpwardArrow:
      case SmartArtType.BasicTarget:
        if (this._parent is SmartArt && this._list.Count == 5)
          throw new ArgumentException($"Node limit exceed for {parentSmartArt.Layout.ToString()} smart art type");
        break;
      case SmartArtType.Funnel:
      case SmartArtType.BasicMatrix:
      case SmartArtType.GridMatrix:
      case SmartArtType.CycleMatrix:
        if (this._parent is SmartArt && this._list.Count == 4)
          throw new ArgumentException($"Node limit exceed for {parentSmartArt.Layout.ToString()} smart art type");
        break;
      case SmartArtType.Gear:
      case SmartArtType.NestedTarget:
        if (this._parent is SmartArt && this._list.Count == 3)
          throw new ArgumentException($"Node limit exceed for {parentSmartArt.Layout.ToString()} smart art type");
        break;
      case SmartArtType.ArrowRibbon:
      case SmartArtType.OpposingArrows:
      case SmartArtType.CounterBalanceArrows:
        if (this._parent is SmartArt && this._list.Count == 2)
          throw new ArgumentException($"Node limit exceed for {parentSmartArt.Layout.ToString()} smart art type");
        break;
      case SmartArtType.SegmentedCycle:
      case SmartArtType.BasicPie:
      case SmartArtType.StackedVenn:
        if (this._parent is SmartArt && this._list.Count == 7)
          throw new ArgumentException($"Node limit exceed for {parentSmartArt.Layout.ToString()} smart art type");
        break;
      case SmartArtType.TitledMatrix:
        if (this._parent is SmartArt && this._list.Count == 1 || this._parent is SmartArtNode && this._list.Count == 4)
          throw new ArgumentException($"Node limit exceed for {parentSmartArt.Layout.ToString()} smart art type");
        break;
    }
    SmartArtNode newNode = new SmartArtNode(this);
    newNode.Id = Guid.NewGuid();
    SmartArtShape smartArtShape1 = new SmartArtShape(parentSmartArt, ShapeType.Point);
    smartArtShape1.DrawingType = DrawingType.SmartArt;
    newNode.ObtainShapes().Add(smartArtShape1);
    SmartArtShape smartArtShape2 = new SmartArtShape(parentSmartArt, ShapeType.Point);
    smartArtShape2.DrawingType = DrawingType.SmartArt;
    newNode.ObtainShapes().Add(smartArtShape2);
    TextBody textBody = new TextBody((Shape) parentSmartArt);
    newNode.SetTextBody(textBody);
    if (this._parent is SmartArt)
      this.AddNodeToSmartArt(newNode, (uint) this._list.Count);
    else
      this.AddChildNodeToNodeCollection(parentSmartArt, newNode, (uint) this._list.Count);
    return (ISmartArtNode) newNode;
  }

  public void Clear()
  {
    if (!(this._parent is SmartArt))
      return;
    SmartArt parent = (SmartArt) this._parent;
    DataModel dataModel = parent.DataModel;
    dataModel.PointCollection.Clear();
    SmartArtPoint point1 = new SmartArtPoint(dataModel);
    point1.ModelId = Guid.NewGuid();
    point1.HasPropertySet = false;
    point1.HasShapeProperties = false;
    point1.TextBody = new TextBody((Shape) parent);
    point1.Type = SmartArtPointType.ParentTransition;
    parent.DataModel.PointCollection.Add(point1.ModelId, point1);
    SmartArtPoint point2 = new SmartArtPoint(dataModel);
    point2.ModelId = Guid.NewGuid();
    point2.HasPropertySet = false;
    point2.HasShapeProperties = false;
    point2.TextBody = new TextBody((Shape) parent);
    point2.Type = SmartArtPointType.SiblingTransition;
    dataModel.PointCollection.Add(point2.ModelId, point2);
    dataModel.ConnectionCollection.Clear();
    this._list.Clear();
  }

  public ISmartArtNode this[int index]
  {
    get
    {
      return this._list.Count != 0 && index < this._list.Count ? this._list[index] : (ISmartArtNode) null;
    }
  }

  public void RemoveAt(int index)
  {
    if (index >= this._list.Count)
      throw new ArgumentException("Index out of range");
    this.Remove((ISmartArtNode) (this._list[index] as SmartArtNode));
  }

  public bool Remove(ISmartArtNode smartArtNode)
  {
    if (this._parent is SmartArt && this._list.Count == 1 && this._list[0].ChildNodes.Count == 0)
    {
      this.Clear();
      return true;
    }
    uint sourcePosition = this.RemovePointAndConnectionForNode(smartArtNode as SmartArtNode);
    this.ManipulateChildNodeOperations(smartArtNode as SmartArtNode, sourcePosition);
    return this.Remove(smartArtNode as SmartArtNode);
  }

  private uint RemovePointAndConnectionForNode(SmartArtNode smartArtNode)
  {
    uint num = 0;
    SmartArt parentSmartArt = this.GetParentSmartArt();
    if (parentSmartArt != null)
    {
      DataModel dataModel = parentSmartArt.DataModel;
      SmartArtPoints pointCollection = dataModel.PointCollection;
      SmartArtConnections connectionCollection = dataModel.ConnectionCollection;
      pointCollection.RemoveByPointType(SmartArtPointType.Presentation);
      connectionCollection.RemoveByConnectionType(SmartArtConnectionType.PresentationOf);
      connectionCollection.RemoveByConnectionType(SmartArtConnectionType.PresentationParentOf);
      int index = pointCollection.IndexOf(smartArtNode.Id);
      Guid[] guidArray = new Guid[3]
      {
        pointCollection.GetKeyByIndex(index),
        pointCollection.GetKeyByIndex(index + 1),
        pointCollection.GetKeyByIndex(index + 2)
      };
      foreach (Guid key in guidArray)
      {
        SmartArtPoint point = dataModel.PointCollection[key];
        if (point.Type == SmartArtPointType.SiblingTransition)
        {
          num = connectionCollection[point.ConnectionId].SourcePosition;
          connectionCollection.Remove(point.ConnectionId);
        }
        pointCollection.Remove(key);
      }
    }
    return num;
  }

  private void ManipulateChildNodeOperations(SmartArtNode smartArtNode, uint sourcePosition)
  {
    if (smartArtNode.ChildNodes.Count == 0)
      return;
    SmartArtNode childNode = smartArtNode.ChildNodes[0] as SmartArtNode;
    DataModel dataModel = this.GetParentSmartArt().DataModel;
    SmartArtConnection connection = dataModel.ConnectionCollection[childNode.BasePoint.ParentSiblingConnectionId];
    if (smartArtNode.Parent is SmartArt)
      connection.SourceId = dataModel.PointCollection[0].ModelId;
    connection.DestinationId = childNode.BasePoint.ModelId;
    connection.ParentTransitionId = childNode.BasePoint.ParentTransitionId;
    connection.SiblingTransitionId = childNode.BasePoint.SiblingTransitionId;
    connection.SourcePosition = sourcePosition;
    this._list.Insert(1, (ISmartArtNode) childNode);
    ((SmartArtNodes) smartArtNode.ChildNodes).Remove(childNode);
  }

  private int IndexOf(SmartArtNode node) => this._list.IndexOf((ISmartArtNode) node);

  private bool Remove(SmartArtNode smartArtNode) => this._list.Remove((ISmartArtNode) smartArtNode);

  public IEnumerator<ISmartArtNode> GetEnumerator()
  {
    return (IEnumerator<ISmartArtNode>) this._list.GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._list.GetEnumerator();

  internal void Add(SmartArtNode newNode) => this._list.Add((ISmartArtNode) newNode);

  internal void AddNodes()
  {
    SmartArt parent = (SmartArt) this._parent;
    for (uint index = 0; index < parent.DataModel.NodeCount; ++index)
    {
      SmartArtNode newNode = new SmartArtNode(parent.Nodes as SmartArtNodes);
      newNode.Id = Guid.NewGuid();
      SmartArtShape smartArtShape1 = new SmartArtShape(parent, ShapeType.Point);
      smartArtShape1.DrawingType = DrawingType.SmartArt;
      newNode.ObtainShapes().Add(smartArtShape1);
      SmartArtShape smartArtShape2 = new SmartArtShape(parent, ShapeType.Point);
      smartArtShape2.DrawingType = DrawingType.SmartArt;
      newNode.ObtainShapes().Add(smartArtShape2);
      TextBody textBody = new TextBody((Shape) parent);
      newNode.SetTextBody(textBody);
      this.AddNodeToSmartArt(newNode, index);
    }
  }

  internal void AddChildNodes(uint childNodeCount)
  {
    SmartArt parentSmartArt = this.GetParentSmartArt();
    for (uint index = 0; index < childNodeCount; ++index)
    {
      SmartArtNode newNode = new SmartArtNode(this);
      newNode.Id = Guid.NewGuid();
      SmartArtShape smartArtShape1 = new SmartArtShape(parentSmartArt, ShapeType.Point);
      smartArtShape1.DrawingType = DrawingType.SmartArt;
      newNode.ObtainShapes().Add(smartArtShape1);
      SmartArtShape smartArtShape2 = new SmartArtShape(parentSmartArt, ShapeType.Point);
      smartArtShape2.DrawingType = DrawingType.SmartArt;
      newNode.ObtainShapes().Add(smartArtShape2);
      TextBody textBody = new TextBody((Shape) parentSmartArt);
      newNode.SetTextBody(textBody);
      this.AddChildNodeToNodeCollection(parentSmartArt, newNode, index);
    }
  }

  private SmartArt GetParentSmartArt()
  {
    return this._parent is SmartArt ? (SmartArt) this._parent : ((SmartArtNode) this._parent).ParentNodes.GetParentSmartArt();
  }

  private void AddChildNodeToNodeCollection(SmartArt smartArt, SmartArtNode newNode, uint index)
  {
    this.Add(newNode);
    SmartArtConnection connection = new SmartArtConnection(smartArt.DataModel);
    connection.ModelId = Guid.NewGuid();
    SmartArtPoint point1 = new SmartArtPoint(smartArt.DataModel);
    point1.ModelId = newNode.Id;
    point1.HasPropertySet = true;
    point1.HasShapeProperties = false;
    point1.IsPlaceholder = true;
    point1.PlaceholderText = "[Text]";
    point1.TextBody = newNode.ObtainTextBody();
    point1.Type = SmartArtPointType.Node;
    newNode.BasePoint = point1;
    point1.SetPointShape(newNode.ObtainShapes()[0] as SmartArtShape);
    smartArt.DataModel.PointCollection.Add(point1.ModelId, point1);
    SmartArtPoint point2 = new SmartArtPoint(smartArt.DataModel);
    point2.ModelId = Guid.NewGuid();
    point2.ConnectionId = connection.ModelId;
    point2.HasPropertySet = false;
    point2.HasShapeProperties = false;
    point2.TextBody = new TextBody((Shape) smartArt);
    point2.Type = SmartArtPointType.ParentTransition;
    smartArt.DataModel.PointCollection.Add(point2.ModelId, point2);
    SmartArtPoint point3 = new SmartArtPoint(smartArt.DataModel);
    newNode.SiblingPoint = point3;
    point3.ModelId = Guid.NewGuid();
    point3.ConnectionId = connection.ModelId;
    point3.HasPropertySet = false;
    point3.HasShapeProperties = false;
    point3.TextBody = new TextBody((Shape) smartArt);
    point3.Type = SmartArtPointType.SiblingTransition;
    point3.SetPointShape(newNode.Shapes[1] as SmartArtShape);
    smartArt.DataModel.PointCollection.Add(point3.ModelId, point3);
    connection.ParentTransitionId = point2.ModelId;
    connection.SiblingTransitionId = point3.ModelId;
    connection.SourceId = ((SmartArtNode) newNode.ParentNodes.Parent).Id;
    connection.DestinationId = point1.ModelId;
    connection.SourcePosition = index;
    smartArt.DataModel.ConnectionCollection.Add(connection.ModelId, connection);
    point1.ParentSiblingConnectionId = connection.ModelId;
    point1.ParentTransitionId = point2.ModelId;
    point1.SiblingTransitionId = point3.ModelId;
  }

  internal void AddDocumentPoint()
  {
    SmartArt parent = (SmartArt) this._parent;
    SmartArtPoint point = new SmartArtPoint(parent.DataModel);
    point.ModelId = Guid.NewGuid();
    point.HasPropertySet = true;
    point.HasShapeProperties = false;
    point.IsPlaceholder = false;
    point.TextBody = new TextBody((Shape) parent);
    point.Type = SmartArtPointType.Document;
    parent.DataModel.PointCollection.Add(point.ModelId, point);
  }

  private void AddNodeToSmartArt(SmartArtNode newNode, uint index)
  {
    SmartArt parent = (SmartArt) this._parent;
    this.Add(newNode);
    SmartArtConnection connection = new SmartArtConnection(parent.DataModel);
    connection.ModelId = Guid.NewGuid();
    SmartArtPoint point1 = new SmartArtPoint(parent.DataModel);
    point1.ModelId = newNode.Id;
    point1.HasPropertySet = true;
    point1.HasShapeProperties = false;
    point1.IsPlaceholder = true;
    point1.PlaceholderText = "[Text]";
    point1.TextBody = newNode.ObtainTextBody();
    point1.Type = SmartArtPointType.Node;
    newNode.BasePoint = point1;
    point1.SetPointShape(newNode.ObtainShapes()[0] as SmartArtShape);
    parent.DataModel.PointCollection.Add(point1.ModelId, point1);
    SmartArtPoint point2 = new SmartArtPoint(parent.DataModel);
    point2.ModelId = Guid.NewGuid();
    point2.ConnectionId = connection.ModelId;
    point2.HasPropertySet = false;
    point2.HasShapeProperties = false;
    point2.TextBody = new TextBody((Shape) parent);
    point2.Type = SmartArtPointType.ParentTransition;
    parent.DataModel.PointCollection.Add(point2.ModelId, point2);
    SmartArtPoint point3 = new SmartArtPoint(parent.DataModel);
    point3.ModelId = Guid.NewGuid();
    newNode.SiblingPoint = point3;
    point3.ConnectionId = connection.ModelId;
    point3.HasPropertySet = false;
    point3.HasShapeProperties = false;
    point3.TextBody = new TextBody((Shape) parent);
    point3.Type = SmartArtPointType.SiblingTransition;
    point3.SetPointShape(newNode.Shapes[1] as SmartArtShape);
    parent.DataModel.PointCollection.Add(point3.ModelId, point3);
    connection.ParentTransitionId = point2.ModelId;
    connection.SiblingTransitionId = point3.ModelId;
    connection.SourceId = parent.DataModel.PointCollection[0].ModelId;
    connection.DestinationId = point1.ModelId;
    connection.SourcePosition = index;
    parent.DataModel.ConnectionCollection.Add(connection.ModelId, connection);
    point1.ParentSiblingConnectionId = connection.ModelId;
    point1.ParentTransitionId = point2.ModelId;
    point1.SiblingTransitionId = point3.ModelId;
  }

  internal void AddSmartArtRelation()
  {
    SmartArt parent = (SmartArt) this._parent;
    string str = (++parent.BaseSlide.Presentation.SmartArtCount).ToString();
    Relation relation = new Relation(Helper.GenerateRelationId(parent.BaseSlide.TopRelation), "http://schemas.openxmlformats.org/officeDocument/2006/relationships/diagramData", $"../diagrams/data{str}.xml", (string) null);
    parent.BaseSlide.TopRelation.Add(relation.Id, relation);
    parent.BaseSlide.Presentation.AddOverrideContentType($"/ppt/diagrams/data{str}.xml", "application/vnd.openxmlformats-officedocument.drawingml.diagramData+xml");
    parent.DataModel.RelationId = relation.Id;
  }

  internal void Close()
  {
    this._parent = (object) null;
    foreach (SmartArtNode smartArtNode in this._list)
      smartArtNode.Close();
  }

  public SmartArtNodes Clone()
  {
    SmartArtNodes smartArtNodes = (SmartArtNodes) this.MemberwiseClone();
    smartArtNodes._list = this.CloneNodesList();
    return smartArtNodes;
  }

  private List<ISmartArtNode> CloneNodesList()
  {
    List<ISmartArtNode> smartArtNodeList = new List<ISmartArtNode>();
    foreach (ISmartArtNode smartArtNode in this._list)
      smartArtNodeList.Add(((SmartArtNode) smartArtNode).Clone());
    return smartArtNodeList;
  }

  internal void SetParent(SmartArt smartArt)
  {
    this._parent = (object) smartArt;
    foreach (SmartArtNode smartArtNode in this._list)
      smartArtNode.SetParent(this);
  }
}
