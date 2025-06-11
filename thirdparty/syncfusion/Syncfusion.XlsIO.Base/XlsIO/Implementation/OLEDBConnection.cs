// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.OLEDBConnection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class OLEDBConnection : DataBaseProperty
{
  private ExternalConnection m_connection;

  public override object ConnectionString
  {
    get => base.ConnectionString;
    set
    {
      base.ConnectionString = value;
      string lower = value.ToString().ToLower();
      if (lower.StartsWith("provider"))
        return;
      string connection = value.ToString().Substring(lower.IndexOf(";") + 1);
      if (this.m_connection != null)
        this.m_connection.DBConnectionString = connection;
      base.ConnectionString = (object) ExternalConnectionCollection.Checkconnection(connection);
    }
  }

  public OLEDBConnection(ExternalConnection connection) => this.m_connection = connection;
}
