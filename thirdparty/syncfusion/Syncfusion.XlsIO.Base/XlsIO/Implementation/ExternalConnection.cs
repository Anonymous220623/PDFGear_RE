// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.ExternalConnection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class ExternalConnection : CommonObject, IConnection, IParentApplication
{
  private string m_name;
  private string m_description;
  private uint m_connectionId;
  private string m_connectionFile;
  private string m_sourceFile = string.Empty;
  private ExcelConnectionsType m_dataBaseType;
  private OLEDBConnection m_oledbConnection;
  private ODBCConnection m_odbcConnection;
  private uint m_refershedVersion;
  private bool m_deleted;
  private bool m_backgroundQuery = true;
  private object m_connectionstring;
  private bool m_isexist;
  private string m_dbConnectionString;
  private IRange m_range;
  private string m_password;
  private bool m_isXml;
  private string m_connectionURL;
  private Stream m_olapProperty;
  private Stream m_extLst;
  internal Stream m_textPr;
  internal ParametersCollection m_parameters;

  public ExternalConnection(IApplication application, object parent)
    : base(application, parent)
  {
    this.initialize();
  }

  private void initialize()
  {
  }

  public event ConnectionPasswordEventHandler OnConnectionPassword;

  public string Description
  {
    get => this.m_description;
    set => this.m_description = value;
  }

  public uint ConncetionId
  {
    get => this.m_connectionId;
    set => this.m_connectionId = value;
  }

  public string ConnectionFile
  {
    get => this.m_connectionFile;
    set => this.m_connectionFile = value;
  }

  public string SourceFile
  {
    get => this.m_sourceFile;
    set => this.m_sourceFile = value;
  }

  public ExcelConnectionsType DataBaseType
  {
    get => this.m_dataBaseType;
    set => this.m_dataBaseType = value;
  }

  public string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  public OLEDBConnection OLEDBConnection
  {
    get => this.m_oledbConnection;
    set => this.m_oledbConnection = value;
  }

  public ODBCConnection ODBCConnection
  {
    get => this.m_odbcConnection;
    set => this.m_odbcConnection = value;
  }

  public uint RefershedVersion
  {
    get => this.m_refershedVersion;
    set => this.m_refershedVersion = value;
  }

  public bool Deleted
  {
    get => this.m_deleted;
    set => this.m_deleted = value;
  }

  public bool BackgroundQuery
  {
    get => this.m_backgroundQuery;
    set => this.m_backgroundQuery = value;
  }

  public void Delete()
  {
    if (this.OLEDBConnection != null)
    {
      this.OLEDBConnection.ConnectionString = (object) string.Empty;
      this.OLEDBConnection.CommandText = (object) string.Empty;
    }
    else if (this.ODBCConnection != null)
    {
      this.ODBCConnection.ConnectionString = (object) string.Empty;
      this.ODBCConnection.CommandText = (object) string.Empty;
    }
    this.m_deleted = true;
    (this.Range.Worksheet.Workbook as WorkbookImpl).DeleteConnection((IConnection) this);
  }

  internal object ConnectionString
  {
    set
    {
      if (this.OLEDBConnection != null)
      {
        this.OLEDBConnection.ConnectionString = value;
      }
      else
      {
        if (this.ODBCConnection == null)
          return;
        this.ODBCConnection.ConnectionString = value;
      }
    }
    get
    {
      if (this.OLEDBConnection != null)
        return this.OLEDBConnection.ConnectionString;
      return this.ODBCConnection != null ? this.ODBCConnection.ConnectionString : (object) null;
    }
  }

  public bool IsExist
  {
    get => this.m_isexist;
    set => this.m_isexist = value;
  }

  public string DBConnectionString
  {
    get => this.m_dbConnectionString;
    set => this.m_dbConnectionString = value;
  }

  public IRange Range
  {
    get
    {
      if (this.m_range == null)
        this.m_range = (IRange) new RangeImpl(this.Application, this.Parent);
      return this.m_range;
    }
    set => this.m_range = value;
  }

  internal bool IsXml
  {
    get => this.m_isXml;
    set => this.m_isXml = value;
  }

  internal string ConnectionURL
  {
    get => this.m_connectionURL;
    set => this.m_connectionURL = value;
  }

  public string password
  {
    get => this.m_password;
    set => this.m_password = value;
  }

  public Stream OlapProperty
  {
    get => this.m_olapProperty;
    set => this.m_olapProperty = value;
  }

  public Stream ExtLstProperty
  {
    get => this.m_extLst;
    set => this.m_extLst = value;
  }

  internal IParameters Parameters
  {
    get
    {
      if (this.m_parameters == null)
        this.m_parameters = new ParametersCollection(this.Application, (object) this);
      return (IParameters) this.m_parameters;
    }
  }

  public void RaiseEvent(object sender, ConnectionPassword args)
  {
    if (this.OnConnectionPassword == null)
      return;
    this.OnConnectionPassword(sender, args);
  }

  internal ExternalConnection Clone(WorkbookImpl book, string ConnectionName)
  {
    ExternalConnection externalConnection = (ExternalConnection) this.MemberwiseClone();
    ExternalConnectionCollection connectionCollection = !externalConnection.Deleted ? book.Connections as ExternalConnectionCollection : book.DeletedConnections as ExternalConnectionCollection;
    externalConnection.ConncetionId = (uint) connectionCollection.GetConnectionId();
    externalConnection.Name = ConnectionName;
    if (this.m_parameters != null)
      externalConnection.m_parameters = this.m_parameters.Clone((object) this);
    connectionCollection.Add((IConnection) externalConnection);
    return externalConnection;
  }

  public void Disposeall()
  {
    this.ODBCConnection = (ODBCConnection) null;
    this.OLEDBConnection = (OLEDBConnection) null;
    this.ConnectionString = (object) null;
    if (this.m_parameters != null)
    {
      this.m_parameters.Dispose();
      this.m_parameters = (ParametersCollection) null;
    }
    this.Dispose();
  }

  ~ExternalConnection()
  {
  }
}
