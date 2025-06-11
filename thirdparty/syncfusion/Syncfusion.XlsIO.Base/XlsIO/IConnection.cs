// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IConnection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IConnection : IParentApplication
{
  string Name { get; set; }

  string Description { get; set; }

  IRange Range { get; }

  OLEDBConnection OLEDBConnection { get; }

  ODBCConnection ODBCConnection { get; }

  void Delete();

  uint ConncetionId { get; }

  event ConnectionPasswordEventHandler OnConnectionPassword;

  ExcelConnectionsType DataBaseType { get; }
}
