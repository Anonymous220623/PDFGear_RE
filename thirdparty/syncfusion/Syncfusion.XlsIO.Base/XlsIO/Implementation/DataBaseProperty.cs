// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.DataBaseProperty
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class DataBaseProperty
{
  private string m_sourceDataFile;
  private bool m_refreshOnFileOpen;
  private ExcelCommandType m_commandType;
  private object m_commandText;
  private object m_connectionString;
  private bool m_backgroundQuery = true;
  private int m_refreshPeriod = -1;
  private bool m_savePassword;
  private bool m_alwaysUseConnectionFile;
  private bool m_enableRefresh = true;
  private ExcelCredentialsMethod m_serverCredentialsMethod;

  public string SourceDataFile
  {
    get
    {
      this.FindDataSource();
      return this.m_sourceDataFile;
    }
  }

  public bool RefreshOnFileOpen
  {
    get => this.m_refreshOnFileOpen;
    set => this.m_refreshOnFileOpen = value;
  }

  public ExcelCommandType CommandType
  {
    get => this.m_commandType;
    set => this.m_commandType = value;
  }

  public object CommandText
  {
    get => this.m_commandText;
    set => this.m_commandText = value;
  }

  public virtual object ConnectionString
  {
    get => this.m_connectionString;
    set => this.m_connectionString = value;
  }

  public bool BackgroundQuery
  {
    get => this.m_backgroundQuery;
    set => this.m_backgroundQuery = value;
  }

  public int RefreshPeriod
  {
    get => this.m_refreshPeriod;
    set => this.m_refreshPeriod = value;
  }

  public bool SavePassword
  {
    get => this.m_savePassword;
    set => this.m_savePassword = value;
  }

  public bool AlwaysUseConnectionFile
  {
    get => this.m_alwaysUseConnectionFile;
    set => this.m_alwaysUseConnectionFile = value;
  }

  public bool EnableRefresh
  {
    get => this.m_enableRefresh;
    set => this.m_enableRefresh = value;
  }

  public ExcelCredentialsMethod ServerCredentialsMethod
  {
    get => this.m_serverCredentialsMethod;
    set => this.m_serverCredentialsMethod = value;
  }

  internal void FindDataSource()
  {
    string connectionString = (string) this.ConnectionString;
    int num = connectionString.IndexOf("Data Source=");
    this.m_sourceDataFile = connectionString.Substring(num + 12);
    if (this.m_sourceDataFile.IndexOf(";") == -1)
      return;
    this.m_sourceDataFile = this.m_sourceDataFile.Remove(this.m_sourceDataFile.IndexOf(";"));
  }
}
