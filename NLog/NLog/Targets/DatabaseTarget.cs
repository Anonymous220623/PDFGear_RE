// Decompiled with JetBrains decompiler
// Type: NLog.Targets.DatabaseTarget
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using NLog.Layouts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Transactions;

#nullable disable
namespace NLog.Targets;

[Target("Database")]
public class DatabaseTarget : Target, IInstallable
{
  private IDbConnection _activeConnection;
  private string _activeConnectionString;
  private IPropertyTypeConverter _propertyTypeConverter;
  private SortHelpers.KeySelector<AsyncLogEventInfo, string> _buildConnectionStringDelegate;
  private TransformedLayout _dbPassword;

  public DatabaseTarget()
  {
    this.InstallDdlCommands = (IList<DatabaseCommandInfo>) new List<DatabaseCommandInfo>();
    this.UninstallDdlCommands = (IList<DatabaseCommandInfo>) new List<DatabaseCommandInfo>();
    this.DBProvider = "sqlserver";
    this.DBHost = (Layout) ".";
    this.ConnectionStringsSettings = System.Configuration.ConfigurationManager.ConnectionStrings;
    this.CommandType = CommandType.Text;
    this.OptimizeBufferReuse = this.GetType() == typeof (DatabaseTarget);
  }

  public DatabaseTarget(string name)
    : this()
  {
    this.Name = name;
  }

  [RequiredParameter]
  [DefaultValue("sqlserver")]
  public string DBProvider { get; set; }

  public string ConnectionStringName { get; set; }

  public Layout ConnectionString { get; set; }

  public Layout InstallConnectionString { get; set; }

  [ArrayParameter(typeof (DatabaseCommandInfo), "install-command")]
  public IList<DatabaseCommandInfo> InstallDdlCommands { get; private set; }

  [ArrayParameter(typeof (DatabaseCommandInfo), "uninstall-command")]
  public IList<DatabaseCommandInfo> UninstallDdlCommands { get; private set; }

  [DefaultValue(false)]
  public bool KeepConnection { get; set; }

  [Obsolete("Value will be ignored as logging code always executes outside of a transaction. Marked obsolete on NLog 4.0 and it will be removed in NLog 6.")]
  public bool? UseTransactions { get; set; }

  public Layout DBHost { get; set; }

  public Layout DBUserName { get; set; }

  public Layout DBPassword
  {
    get => this._dbPassword?.Layout;
    set
    {
      this._dbPassword = TransformedLayout.Create(value, new System.Func<string, string>(DatabaseTarget.EscapeValueForConnectionString), new Func<Layout, LogEventInfo, string>(((Target) this).RenderLogEvent));
    }
  }

  public Layout DBDatabase { get; set; }

  [RequiredParameter]
  public Layout CommandText { get; set; }

  [DefaultValue(CommandType.Text)]
  public CommandType CommandType { get; set; }

  [ArrayParameter(typeof (DatabaseParameterInfo), "parameter")]
  public IList<DatabaseParameterInfo> Parameters { get; } = (IList<DatabaseParameterInfo>) new List<DatabaseParameterInfo>();

  [ArrayParameter(typeof (DatabaseObjectPropertyInfo), "connectionproperty")]
  public IList<DatabaseObjectPropertyInfo> ConnectionProperties { get; } = (IList<DatabaseObjectPropertyInfo>) new List<DatabaseObjectPropertyInfo>();

  [ArrayParameter(typeof (DatabaseObjectPropertyInfo), "commandproperty")]
  public IList<DatabaseObjectPropertyInfo> CommandProperties { get; } = (IList<DatabaseObjectPropertyInfo>) new List<DatabaseObjectPropertyInfo>();

  public System.Data.IsolationLevel? IsolationLevel { get; set; }

  internal DbProviderFactory ProviderFactory { get; set; }

  internal ConnectionStringSettingsCollection ConnectionStringsSettings { get; set; }

  internal Type ConnectionType { get; set; }

  private IPropertyTypeConverter PropertyTypeConverter
  {
    get
    {
      return this._propertyTypeConverter ?? (this._propertyTypeConverter = ConfigurationItemFactory.Default.PropertyTypeConverter);
    }
    set => this._propertyTypeConverter = value;
  }

  public void Install(InstallationContext installationContext)
  {
    this.RunInstallCommands(installationContext, (IEnumerable<DatabaseCommandInfo>) this.InstallDdlCommands);
  }

  public void Uninstall(InstallationContext installationContext)
  {
    this.RunInstallCommands(installationContext, (IEnumerable<DatabaseCommandInfo>) this.UninstallDdlCommands);
  }

  public bool? IsInstalled(InstallationContext installationContext) => new bool?();

  internal IDbConnection OpenConnection(string connectionString, LogEventInfo logEventInfo)
  {
    IDbConnection databaseObject = this.ProviderFactory == null ? (IDbConnection) Activator.CreateInstance(this.ConnectionType) : (IDbConnection) this.ProviderFactory.CreateConnection();
    if (databaseObject == null)
      throw new NLogRuntimeException("Creation of connection failed");
    databaseObject.ConnectionString = connectionString;
    IList<DatabaseObjectPropertyInfo> connectionProperties = this.ConnectionProperties;
    if ((connectionProperties != null ? (connectionProperties.Count > 0 ? 1 : 0) : 0) != 0)
      this.ApplyDatabaseObjectProperties((object) databaseObject, this.ConnectionProperties, logEventInfo ?? LogEventInfo.CreateNullEvent());
    databaseObject.Open();
    return databaseObject;
  }

  private void ApplyDatabaseObjectProperties(
    object databaseObject,
    IList<DatabaseObjectPropertyInfo> objectProperties,
    LogEventInfo logEventInfo)
  {
    for (int index = 0; index < objectProperties.Count; ++index)
    {
      DatabaseObjectPropertyInfo objectProperty = objectProperties[index];
      try
      {
        object objectPropertyValue = this.GetDatabaseObjectPropertyValue(logEventInfo, objectProperty);
        if (!objectProperty.SetPropertyValue(databaseObject, objectPropertyValue))
          InternalLogger.Warn<string, string, Type>("DatabaseTarget(Name={0}): Failed to lookup property {1} on {2}", this.Name, objectProperty.Name, databaseObject.GetType());
      }
      catch (Exception ex)
      {
        InternalLogger.Error(ex, "DatabaseTarget(Name={0}): Failed to assign value for property {1} on {2}", (object) this.Name, (object) objectProperty.Name, (object) databaseObject.GetType());
        if (this.ExceptionMustBeRethrown(ex))
          throw;
      }
    }
  }

  protected override void InitializeTarget()
  {
    base.InitializeTarget();
    if (this.UseTransactions.HasValue)
      InternalLogger.Warn<string>("DatabaseTarget(Name={0}): UseTransactions property is obsolete and will not be used - will be removed in NLog 6", this.Name);
    bool flag = false;
    string providerName = string.Empty;
    if (!string.IsNullOrEmpty(this.ConnectionStringName))
    {
      ConnectionStringSettings connectionStringsSetting = this.ConnectionStringsSettings[this.ConnectionStringName];
      if (connectionStringsSetting == null)
        throw new NLogConfigurationException($"Connection string '{this.ConnectionStringName}' is not declared in <connectionStrings /> section.");
      if (!string.IsNullOrEmpty(connectionStringsSetting.ConnectionString?.Trim()))
        this.ConnectionString = (Layout) SimpleLayout.Escape(connectionStringsSetting.ConnectionString.Trim());
      providerName = connectionStringsSetting.ProviderName?.Trim() ?? string.Empty;
    }
    if (this.ConnectionString != null)
      providerName = this.InitConnectionString(providerName);
    if (string.IsNullOrEmpty(providerName))
      providerName = this.GetProviderNameFromDbProviderFactories(providerName);
    if (!string.IsNullOrEmpty(providerName))
      flag = this.InitProviderFactory(providerName);
    if (flag)
      return;
    try
    {
      this.SetConnectionType();
      if (!(this.ConnectionType == (Type) null))
        return;
      InternalLogger.Warn<string, string>("DatabaseTarget(Name={0}): No ConnectionType created from DBProvider={1}", this.Name, this.DBProvider);
    }
    catch (Exception ex)
    {
      object[] objArray = new object[2]
      {
        (object) this.Name,
        (object) this.DBProvider
      };
      InternalLogger.Error(ex, "DatabaseTarget(Name={0}): Failed to create ConnectionType from DBProvider={1}", objArray);
      throw;
    }
  }

  private string InitConnectionString(string providerName)
  {
    try
    {
      string str = this.BuildConnectionString(LogEventInfo.CreateNullEvent());
      DbConnectionStringBuilder connectionStringBuilder = new DbConnectionStringBuilder()
      {
        ConnectionString = str
      };
      object obj1;
      if (connectionStringBuilder.TryGetValue("provider connection string", out obj1))
      {
        object obj2;
        if (connectionStringBuilder.TryGetValue("provider", out obj2))
          providerName = obj2.ToString()?.Trim() ?? string.Empty;
        this.ConnectionString = (Layout) SimpleLayout.Escape(obj1.ToString());
      }
    }
    catch (Exception ex)
    {
      if (!string.IsNullOrEmpty(this.ConnectionStringName))
        InternalLogger.Warn(ex, "DatabaseTarget(Name={0}): DbConnectionStringBuilder failed to parse '{1}' ConnectionString", (object) this.Name, (object) this.ConnectionStringName);
      else
        InternalLogger.Warn(ex, "DatabaseTarget(Name={0}): DbConnectionStringBuilder failed to parse ConnectionString", (object) this.Name);
    }
    return providerName;
  }

  private bool InitProviderFactory(string providerName)
  {
    try
    {
      this.ProviderFactory = DbProviderFactories.GetFactory(providerName);
      return true;
    }
    catch (Exception ex)
    {
      object[] objArray = new object[2]
      {
        (object) this.Name,
        (object) providerName
      };
      InternalLogger.Error(ex, "DatabaseTarget(Name={0}): DbProviderFactories failed to get factory from ProviderName={1}", objArray);
      throw;
    }
  }

  private string GetProviderNameFromDbProviderFactories(string providerName)
  {
    string b = this.DBProvider?.Trim() ?? string.Empty;
    if (!string.IsNullOrEmpty(b))
    {
      foreach (DataRow row in (InternalDataCollectionBase) DbProviderFactories.GetFactoryClasses().Rows)
      {
        string a = (string) row["InvariantName"];
        if (string.Equals(a, b, StringComparison.OrdinalIgnoreCase))
        {
          providerName = a;
          break;
        }
      }
    }
    return providerName;
  }

  private void SetConnectionType()
  {
    switch (this.DBProvider.ToUpperInvariant())
    {
      case "MICROSOFT":
      case "MSDE":
      case "MSSQL":
      case "SQLSERVER":
      case "SYSTEM.DATA.SQLCLIENT":
        this.ConnectionType = typeof (IDbConnection).GetAssembly().GetType("System.Data.SqlClient.SqlConnection", true, true);
        break;
      case "ODBC":
      case "SYSTEM.DATA.ODBC":
        this.ConnectionType = typeof (IDbConnection).GetAssembly().GetType("System.Data.Odbc.OdbcConnection", true, true);
        break;
      case "OLEDB":
        this.ConnectionType = typeof (IDbConnection).GetAssembly().GetType("System.Data.OleDb.OleDbConnection", true, true);
        break;
      default:
        this.ConnectionType = Type.GetType(this.DBProvider, true, true);
        break;
    }
  }

  protected override void CloseTarget()
  {
    this.PropertyTypeConverter = (IPropertyTypeConverter) null;
    base.CloseTarget();
    InternalLogger.Trace<string>("DatabaseTarget(Name={0}): Close connection because of CloseTarget", this.Name);
    this.CloseConnection();
  }

  protected override void Write(LogEventInfo logEvent)
  {
    try
    {
      this.WriteLogEventToDatabase(logEvent, this.BuildConnectionString(logEvent));
    }
    finally
    {
      if (!this.KeepConnection)
      {
        InternalLogger.Trace<string>("DatabaseTarget(Name={0}): Close connection (KeepConnection = false).", this.Name);
        this.CloseConnection();
      }
    }
  }

  [Obsolete("Instead override Write(IList<AsyncLogEventInfo> logEvents. Marked obsolete on NLog 4.5")]
  protected override void Write(AsyncLogEventInfo[] logEvents)
  {
    this.Write((IList<AsyncLogEventInfo>) logEvents);
  }

  protected override void Write(IList<AsyncLogEventInfo> logEvents)
  {
    if (this._buildConnectionStringDelegate == null)
      this._buildConnectionStringDelegate = (SortHelpers.KeySelector<AsyncLogEventInfo, string>) (l => this.BuildConnectionString(l.LogEvent));
    foreach (KeyValuePair<string, IList<AsyncLogEventInfo>> keyValuePair in logEvents.BucketSort<AsyncLogEventInfo, string>(this._buildConnectionStringDelegate))
    {
      try
      {
        this.WriteLogEventsToDatabase(keyValuePair.Value, keyValuePair.Key);
      }
      finally
      {
        if (!this.KeepConnection)
        {
          InternalLogger.Trace<string>("DatabaseTarget(Name={0}): Close connection because of KeepConnection=false", this.Name);
          this.CloseConnection();
        }
      }
    }
  }

  private void WriteLogEventsToDatabase(IList<AsyncLogEventInfo> logEvents, string connectionString)
  {
    if (this.IsolationLevel.HasValue && logEvents.Count > 1)
    {
      this.WriteLogEventBatchToDatabase(logEvents, connectionString);
    }
    else
    {
      for (int index = 0; index < logEvents.Count; ++index)
      {
        try
        {
          AsyncLogEventInfo logEvent = logEvents[index];
          this.WriteLogEventToDatabase(logEvent.LogEvent, connectionString);
          logEvent = logEvents[index];
          logEvent.Continuation((Exception) null);
        }
        catch (Exception ex)
        {
          if (ex.MustBeRethrownImmediately())
            throw;
          logEvents[index].Continuation(ex);
          if (this.ExceptionMustBeRethrown(ex))
            throw;
        }
      }
    }
  }

  private void WriteLogEventBatchToDatabase(
    IList<AsyncLogEventInfo> logEvents,
    string connectionString)
  {
    try
    {
      using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Suppress))
      {
        string connectionString1 = connectionString;
        AsyncLogEventInfo logEvent;
        LogEventInfo logEventInfo;
        if (logEvents.Count <= 0)
        {
          logEventInfo = (LogEventInfo) null;
        }
        else
        {
          logEvent = logEvents[0];
          logEventInfo = logEvent.LogEvent;
        }
        this.EnsureConnectionOpen(connectionString1, logEventInfo);
        IDbTransaction dbTransaction = this._activeConnection.BeginTransaction(this.IsolationLevel.Value);
        try
        {
          for (int index = 0; index < logEvents.Count; ++index)
          {
            logEvent = logEvents[index];
            this.ExecuteDbCommandWithParameters(logEvent.LogEvent, this._activeConnection, dbTransaction);
          }
          dbTransaction?.Commit();
          for (int index = 0; index < logEvents.Count; ++index)
          {
            logEvent = logEvents[index];
            logEvent.Continuation((Exception) null);
          }
          dbTransaction?.Dispose();
          transactionScope.Complete();
        }
        catch
        {
          try
          {
            if (dbTransaction?.Connection != null && dbTransaction != null)
              dbTransaction.Rollback();
            dbTransaction?.Dispose();
          }
          catch (Exception ex)
          {
            InternalLogger.Error(ex, "DatabaseTarget(Name={0}): Error during rollback of batch writing {1} logevents to database.", (object) this.Name, (object) logEvents.Count);
            if (ex.MustBeRethrownImmediately())
              throw;
          }
          throw;
        }
      }
    }
    catch (Exception ex)
    {
      InternalLogger.Error(ex, "DatabaseTarget(Name={0}): Error when batch writing {1} logevents to database.", (object) this.Name, (object) logEvents.Count);
      if (ex.MustBeRethrownImmediately())
        throw;
      for (int index = 0; index < logEvents.Count; ++index)
        logEvents[index].Continuation(ex);
      InternalLogger.Trace<string>("DatabaseTarget(Name={0}): Close connection because of error", this.Name);
      this.CloseConnection();
      if (!this.ExceptionMustBeRethrown(ex))
        return;
      throw;
    }
  }

  private void WriteLogEventToDatabase(LogEventInfo logEvent, string connectionString)
  {
    try
    {
      using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Suppress))
      {
        this.EnsureConnectionOpen(connectionString, logEvent);
        this.ExecuteDbCommandWithParameters(logEvent, this._activeConnection, (IDbTransaction) null);
        transactionScope.Complete();
      }
    }
    catch (Exception ex)
    {
      InternalLogger.Error(ex, "DatabaseTarget(Name={0}): Error when writing to database.", (object) this.Name);
      if (ex.MustBeRethrownImmediately())
        throw;
      InternalLogger.Trace<string>("DatabaseTarget(Name={0}): Close connection because of error", this.Name);
      this.CloseConnection();
      throw;
    }
  }

  private void ExecuteDbCommandWithParameters(
    LogEventInfo logEvent,
    IDbConnection dbConnection,
    IDbTransaction dbTransaction)
  {
    using (IDbCommand dbCommand = this.CreateDbCommand(logEvent, dbConnection))
    {
      if (dbTransaction != null)
        dbCommand.Transaction = dbTransaction;
      InternalLogger.Trace<string, int>("DatabaseTarget(Name={0}): Finished execution, result = {1}", this.Name, dbCommand.ExecuteNonQuery());
    }
  }

  internal IDbCommand CreateDbCommand(LogEventInfo logEvent, IDbConnection dbConnection)
  {
    string dbCommandText = this.RenderLogEvent(this.CommandText, logEvent);
    InternalLogger.Trace<string, CommandType, string>("DatabaseTarget(Name={0}): Executing {1}: {2}", this.Name, this.CommandType, dbCommandText);
    return this.CreateDbCommandWithParameters(logEvent, dbConnection, this.CommandType, dbCommandText, this.Parameters);
  }

  private IDbCommand CreateDbCommandWithParameters(
    LogEventInfo logEvent,
    IDbConnection dbConnection,
    CommandType commandType,
    string dbCommandText,
    IList<DatabaseParameterInfo> databaseParameterInfos)
  {
    IDbCommand command = dbConnection.CreateCommand();
    command.CommandType = commandType;
    IList<DatabaseObjectPropertyInfo> commandProperties = this.CommandProperties;
    if ((commandProperties != null ? (commandProperties.Count > 0 ? 1 : 0) : 0) != 0)
      this.ApplyDatabaseObjectProperties((object) command, this.CommandProperties, logEvent);
    command.CommandText = dbCommandText;
    for (int index = 0; index < databaseParameterInfos.Count; ++index)
    {
      DatabaseParameterInfo databaseParameterInfo = databaseParameterInfos[index];
      IDbDataParameter databaseParameter = this.CreateDatabaseParameter(command, databaseParameterInfo);
      object databaseParameterValue = this.GetDatabaseParameterValue(logEvent, databaseParameterInfo);
      databaseParameter.Value = databaseParameterValue;
      command.Parameters.Add((object) databaseParameter);
      InternalLogger.Trace<string, object, DbType>("  DatabaseTarget: Parameter: '{0}' = '{1}' ({2})", databaseParameter.ParameterName, databaseParameter.Value, databaseParameter.DbType);
    }
    return command;
  }

  protected string BuildConnectionString(LogEventInfo logEvent)
  {
    if (this.ConnectionString != null)
      return this.RenderLogEvent(this.ConnectionString, logEvent);
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("Server=");
    stringBuilder.Append(this.RenderLogEvent(this.DBHost, logEvent));
    stringBuilder.Append(";");
    string str1 = this.RenderLogEvent(this.DBUserName, logEvent);
    if (string.IsNullOrEmpty(str1))
    {
      stringBuilder.Append("Trusted_Connection=SSPI;");
    }
    else
    {
      stringBuilder.Append("User id=");
      stringBuilder.Append(str1);
      stringBuilder.Append(";Password=");
      string str2 = this._dbPassword.Render(logEvent);
      stringBuilder.Append(str2);
      stringBuilder.Append(";");
    }
    string str3 = this.RenderLogEvent(this.DBDatabase, logEvent);
    if (!string.IsNullOrEmpty(str3))
    {
      stringBuilder.Append("Database=");
      stringBuilder.Append(str3);
    }
    return stringBuilder.ToString();
  }

  private static string EscapeValueForConnectionString(string value)
  {
    if (value.StartsWith("'") && value.EndsWith("'") || value.StartsWith("\"") && value.EndsWith("\""))
      return value;
    bool flag1 = value.Contains("'");
    bool flag2 = value.Contains("\"");
    if (!(value.Contains(";") | flag1 | flag2))
      return value;
    if (!flag1)
      return $"'{value}'";
    return !flag2 ? $"\"{value}\"" : $"\"{value.Replace("\"", "\"\"")}\"";
  }

  private void EnsureConnectionOpen(string connectionString, LogEventInfo logEventInfo)
  {
    if (this._activeConnection != null && this._activeConnectionString != connectionString)
    {
      InternalLogger.Trace<string>("DatabaseTarget(Name={0}): Close connection because of opening new.", this.Name);
      this.CloseConnection();
    }
    if (this._activeConnection != null)
      return;
    InternalLogger.Trace<string>("DatabaseTarget(Name={0}): Open connection.", this.Name);
    this._activeConnection = this.OpenConnection(connectionString, logEventInfo);
    this._activeConnectionString = connectionString;
  }

  private void CloseConnection()
  {
    this._activeConnectionString = (string) null;
    if (this._activeConnection == null)
      return;
    this._activeConnection.Close();
    this._activeConnection.Dispose();
    this._activeConnection = (IDbConnection) null;
  }

  private void RunInstallCommands(
    InstallationContext installationContext,
    IEnumerable<DatabaseCommandInfo> commands)
  {
    LogEventInfo logEvent = installationContext.CreateLogEvent();
    try
    {
      foreach (DatabaseCommandInfo command in commands)
      {
        string stringFromCommand = this.GetConnectionStringFromCommand(command, logEvent);
        if (this.ConnectionType == (Type) null)
          this.SetConnectionType();
        this.EnsureConnectionOpen(stringFromCommand, logEvent);
        string dbCommandText = this.RenderLogEvent(command.Text, logEvent);
        installationContext.Trace("DatabaseTarget(Name={0}) - Executing {1} '{2}'", (object) this.Name, (object) command.CommandType, (object) dbCommandText);
        using (IDbCommand commandWithParameters = this.CreateDbCommandWithParameters(logEvent, this._activeConnection, command.CommandType, dbCommandText, command.Parameters))
        {
          try
          {
            commandWithParameters.ExecuteNonQuery();
          }
          catch (Exception ex)
          {
            if (ex.MustBeRethrownImmediately())
              throw;
            if (command.IgnoreFailures || installationContext.IgnoreFailures)
            {
              installationContext.Warning(ex.Message);
            }
            else
            {
              installationContext.Error(ex.Message);
              throw;
            }
          }
        }
      }
    }
    finally
    {
      InternalLogger.Trace<string>("DatabaseTarget(Name={0}): Close connection after install.", this.Name);
      this.CloseConnection();
    }
  }

  private string GetConnectionStringFromCommand(
    DatabaseCommandInfo commandInfo,
    LogEventInfo logEvent)
  {
    return commandInfo.ConnectionString == null ? (this.InstallConnectionString == null ? this.BuildConnectionString(logEvent) : this.RenderLogEvent(this.InstallConnectionString, logEvent)) : this.RenderLogEvent(commandInfo.ConnectionString, logEvent);
  }

  protected virtual IDbDataParameter CreateDatabaseParameter(
    IDbCommand command,
    DatabaseParameterInfo parameterInfo)
  {
    IDbDataParameter parameter = command.CreateParameter();
    parameter.Direction = ParameterDirection.Input;
    if (parameterInfo.Name != null)
      parameter.ParameterName = parameterInfo.Name;
    if (parameterInfo.Size != 0)
      parameter.Size = parameterInfo.Size;
    if (parameterInfo.Precision != (byte) 0)
      parameter.Precision = parameterInfo.Precision;
    if (parameterInfo.Scale != (byte) 0)
      parameter.Scale = parameterInfo.Scale;
    try
    {
      if (!parameterInfo.SetDbType(parameter))
        InternalLogger.Warn<string, string>("  DatabaseTarget: Parameter: '{0}' - Failed to assign DbType={1}", parameterInfo.Name, parameterInfo.DbType);
    }
    catch (Exception ex)
    {
      if (ex.MustBeRethrownImmediately())
        throw;
      InternalLogger.Error(ex, "  DatabaseTarget: Parameter: '{0}' - Failed to assign DbType={1}", (object) parameterInfo.Name, (object) parameterInfo.DbType);
      if (this.ExceptionMustBeRethrown(ex))
        throw;
    }
    return parameter;
  }

  protected internal virtual object GetDatabaseParameterValue(
    LogEventInfo logEvent,
    DatabaseParameterInfo parameterInfo)
  {
    return this.RenderObjectValue(logEvent, parameterInfo.Name, parameterInfo.Layout, parameterInfo.ParameterType, parameterInfo.Format, (IFormatProvider) parameterInfo.Culture, parameterInfo.AllowDbNull);
  }

  private object GetDatabaseObjectPropertyValue(
    LogEventInfo logEvent,
    DatabaseObjectPropertyInfo connectionInfo)
  {
    return this.RenderObjectValue(logEvent, connectionInfo.Name, connectionInfo.Layout, connectionInfo.PropertyType, connectionInfo.Format, (IFormatProvider) connectionInfo.Culture, false);
  }

  private object RenderObjectValue(
    LogEventInfo logEvent,
    string propertyName,
    Layout valueLayout,
    Type valueType,
    string valueFormat,
    IFormatProvider formatProvider,
    bool allowDbNull)
  {
    if (string.IsNullOrEmpty(valueFormat) && valueType == typeof (string) && !allowDbNull)
      return (object) this.RenderLogEvent(valueLayout, logEvent) ?? (object) string.Empty;
    IFormatProvider formatProvider1 = formatProvider;
    if (formatProvider1 == null)
    {
      IFormatProvider formatProvider2 = logEvent.FormatProvider;
      if (formatProvider2 == null)
      {
        LoggingConfiguration loggingConfiguration = this.LoggingConfiguration;
        formatProvider1 = loggingConfiguration != null ? (IFormatProvider) loggingConfiguration.DefaultCultureInfo : (IFormatProvider) null;
      }
      else
        formatProvider1 = formatProvider2;
    }
    formatProvider = formatProvider1;
    try
    {
      object rawValue;
      if (this.TryRenderObjectRawValue(logEvent, valueLayout, valueType, valueFormat, formatProvider, allowDbNull, out rawValue))
        return rawValue;
    }
    catch (Exception ex)
    {
      if (ex.MustBeRethrownImmediately())
        throw;
      InternalLogger.Warn(ex, "  DatabaseTarget: Failed to convert raw value for '{0}' into {1}", (object) propertyName, (object) valueType);
      if (this.ExceptionMustBeRethrown(ex))
        throw;
    }
    try
    {
      InternalLogger.Trace<string, Type>("  DatabaseTarget: Attempt to convert layout value for '{0}' into {1}", propertyName, valueType);
      string propertyValue = this.RenderLogEvent(valueLayout, logEvent);
      return string.IsNullOrEmpty(propertyValue) ? DatabaseTarget.CreateDefaultValue(valueType, allowDbNull) : this.PropertyTypeConverter.Convert((object) propertyValue, valueType, valueFormat, formatProvider) ?? DatabaseTarget.CreateDefaultValue(valueType, allowDbNull);
    }
    catch (Exception ex)
    {
      if (ex.MustBeRethrownImmediately())
        throw;
      InternalLogger.Warn(ex, "  DatabaseTarget: Failed to convert layout value for '{0}' into {1}", (object) propertyName, (object) valueType);
      if (!this.ExceptionMustBeRethrown(ex))
        return DatabaseTarget.CreateDefaultValue(valueType, allowDbNull);
      throw;
    }
  }

  private bool TryRenderObjectRawValue(
    LogEventInfo logEvent,
    Layout valueLayout,
    Type valueType,
    string valueFormat,
    IFormatProvider formatProvider,
    bool allowDbNull,
    out object rawValue)
  {
    if (!valueLayout.TryGetRawValue(logEvent, out rawValue))
      return false;
    if (rawValue == DBNull.Value)
      return true;
    if (rawValue == null)
    {
      rawValue = DatabaseTarget.CreateDefaultValue(valueType, allowDbNull);
      return true;
    }
    if (valueType == typeof (string))
      return rawValue is string;
    rawValue = this.PropertyTypeConverter.Convert(rawValue, valueType, valueFormat, formatProvider) ?? DatabaseTarget.CreateDefaultValue(valueType, allowDbNull);
    return true;
  }

  private static object CreateDefaultValue(Type dbParameterType, bool allowDbNull)
  {
    if (allowDbNull)
      return (object) DBNull.Value;
    if (dbParameterType == typeof (string))
      return (object) string.Empty;
    return dbParameterType.IsValueType() ? Activator.CreateInstance(dbParameterType) : (object) DBNull.Value;
  }
}
