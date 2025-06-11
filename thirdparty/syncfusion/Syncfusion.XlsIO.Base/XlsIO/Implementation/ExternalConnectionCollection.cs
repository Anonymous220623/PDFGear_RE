// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.ExternalConnectionCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class ExternalConnectionCollection(IApplication application, object parent) : 
  CollectionBaseEx<IConnection>(application, parent),
  IConnections,
  IParentApplication,
  ICloneParent,
  IList<IConnection>,
  ICollection<IConnection>,
  IEnumerable<IConnection>,
  IEnumerable
{
  private const string Default_Oledb_Provider = "Provider=Microsoft.ACE.OLEDB.12.0";

  public IConnection Add(
    string connectionName,
    string description,
    object connectionString,
    object commandText,
    ExcelCommandType commandType)
  {
    this.checkname(connectionName);
    ExternalConnection externalConnection = new ExternalConnection(this.Application, this.Parent);
    string connectionSting = (string) connectionString;
    if (connectionSting.StartsWith("OLEDB;"))
      this.CreateOledbConnection(externalConnection, (object) connectionSting, commandText, commandType);
    else if (connectionSting.StartsWith("ODBC;"))
      this.CreateObdcConnection(externalConnection, connectionString, commandText, commandType);
    externalConnection.Name = connectionName;
    externalConnection.Description = description;
    externalConnection.SourceFile = this.FindDataSource(connectionString.ToString());
    externalConnection.ConncetionId = (uint) this.GetConnectionId();
    this.Add((IConnection) externalConnection);
    return (IConnection) externalConnection;
  }

  public IConnection Add(ExcelConnectionsType Type)
  {
    ExternalConnection connection = new ExternalConnection(this.Application, this.Parent);
    connection.DataBaseType = Type;
    switch (Type)
    {
      case ExcelConnectionsType.ConnectionTypeODBC:
        connection.ODBCConnection = new ODBCConnection(connection);
        break;
      case ExcelConnectionsType.ConnectionTypeOLEDB:
        connection.OLEDBConnection = new OLEDBConnection(connection);
        break;
    }
    this.Add((IConnection) connection);
    return (IConnection) connection;
  }

  public IConnection AddFromFile(string Name, string FilePath)
  {
    string extension = Path.GetExtension(FilePath);
    this.checkname(Name);
    if (extension != ".xml")
      throw new ArgumentException("The file Format is wrong");
    ExternalConnection externalConnection = new ExternalConnection(this.Application, this.Parent);
    externalConnection.ConnectionURL = FilePath;
    externalConnection.DataBaseType = ExcelConnectionsType.ConnectionTypeWEB;
    externalConnection.ConncetionId = (uint) this.GetConnectionId();
    externalConnection.Name = Path.GetFileNameWithoutExtension(FilePath);
    externalConnection.IsXml = true;
    this.Add((IConnection) externalConnection);
    return (IConnection) externalConnection;
  }

  private void checkname(string name)
  {
    int count = this.Count;
    for (int index = 0; index < count; ++index)
    {
      if ((this.Parent as WorkbookImpl).Connections[index].Name == name)
        throw new ArgumentException("Connection Name Already Exist");
    }
  }

  private void CreateOledbConnection(
    ExternalConnection connection,
    object connectionSting,
    object commandText,
    ExcelCommandType commandType)
  {
    string str = connectionSting.ToString();
    string connection1 = str.Substring(str.IndexOf(";") + 1);
    OLEDBConnection oledbConnection = new OLEDBConnection(connection);
    connection.DataBaseType = ExcelConnectionsType.ConnectionTypeOLEDB;
    connection.DBConnectionString = connection1;
    oledbConnection.ConnectionString = (object) ExternalConnectionCollection.Checkconnection(connection1);
    oledbConnection.CommandText = (object) (string) commandText;
    oledbConnection.CommandType = commandType;
    oledbConnection.RefreshOnFileOpen = true;
    connection.OLEDBConnection = oledbConnection;
  }

  private void CreateObdcConnection(
    ExternalConnection Connection,
    object ConnectionSting,
    object CommandText,
    ExcelCommandType CommandType)
  {
    if (CommandType != ExcelCommandType.Sql)
      throw new ArgumentException("command type is not valid for ODBC connection");
    string str1 = ConnectionSting.ToString();
    string str2 = str1.Substring(str1.IndexOf(";") + 1);
    ODBCConnection odbcConnection = new ODBCConnection(Connection);
    Connection.DataBaseType = ExcelConnectionsType.ConnectionTypeODBC;
    odbcConnection.ConnectionString = (object) str2;
    Connection.DBConnectionString = str2;
    odbcConnection.CommandText = (object) (string) CommandText;
    odbcConnection.CommandType = CommandType;
    odbcConnection.RefreshOnFileOpen = true;
    Connection.ODBCConnection = odbcConnection;
  }

  private string FindDataSource(string connectionString)
  {
    string lower = connectionString.ToLower();
    if (!lower.Contains("data source"))
      return string.Empty;
    int startIndex = lower.IndexOf("data source") + 12;
    string dataSource = connectionString.Substring(startIndex);
    if (dataSource.Contains(";"))
      dataSource = dataSource.Remove(dataSource.IndexOf(";") + 1);
    return dataSource;
  }

  internal static string Checkconnection(string connection)
  {
    string lower = connection.ToLower();
    if (lower.Contains("provider"))
    {
      int startIndex = lower.IndexOf("provider");
      int num = lower.IndexOf(";", startIndex);
      num.ToString();
      string oldValue = connection.Substring(startIndex, num - startIndex);
      string str = lower.Substring(startIndex, num - startIndex);
      if (oldValue != null && oldValue != "" && str.Contains("jet"))
        connection = connection.Replace(oldValue, "Provider=Microsoft.ACE.OLEDB.12.0");
    }
    return connection;
  }

  internal int GetConnectionId()
  {
    int Id = (this.Parent as WorkbookImpl).Connections.Count + 1 + (this.Parent as WorkbookImpl).DeletedConnections.Count;
    while (this.Checkconnections((IConnections) this, Id) || this.Checkconnections((this.Parent as WorkbookImpl).DeletedConnections, Id))
      ++Id;
    return Id;
  }

  private bool Checkconnections(IConnections connections, int Id)
  {
    for (int index = 0; index < connections.Count; ++index)
    {
      if ((int) connections[index].ConncetionId == Id)
        return true;
    }
    return false;
  }

  public void Dispose()
  {
    for (int index = this.Count - 1; index > -1; --index)
    {
      ((ExternalConnection) this[index]).Disposeall();
      this[index] = (IConnection) null;
      this.RemoveAt(index);
    }
  }
}
