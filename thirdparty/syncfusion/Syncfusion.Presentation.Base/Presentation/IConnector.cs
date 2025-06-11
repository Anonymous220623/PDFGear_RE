// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.IConnector
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation;

public interface IConnector : IShape, ISlideItem
{
  ConnectorType Type { get; set; }

  IShape BeginConnectedShape { get; }

  int BeginConnectionSiteIndex { get; }

  IShape EndConnectedShape { get; }

  int EndConnectionSiteIndex { get; }

  void BeginConnect(IShape connectedShape, int connectionSiteIndex);

  void BeginDisconnect();

  void EndConnect(IShape connectedShape, int connectionSiteIndex);

  void EndDisconnect();

  void Update();
}
