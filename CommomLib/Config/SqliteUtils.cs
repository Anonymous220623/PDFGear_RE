// Decompiled with JetBrains decompiler
// Type: CommomLib.Config.SqliteUtils
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using CommomLib.Commom;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace CommomLib.Config;

public static class SqliteUtils
{
  private const string DefaultDbName = "pdfdata.db";
  private const string CreateDbMutexName = "PDFGEAR_CREATE_DB";
  private const int DefaultTimeoutSeconds = 60;
  private static TaskCompletionSource<bool> waitForInit = new TaskCompletionSource<bool>();
  private static volatile bool initializing = false;
  private static volatile bool initialized = false;
  private static string dbName = "";
  private static string dbFullName = "";

  private static string GetLocalAppDataFolderPath() => AppDataHelper.LocalFolder;

  public static Task InitializeDatabase(string dbName = "pdfdata.db")
  {
    switch (SqliteUtils.Status)
    {
      case SqliteStatus.Initializing:
        if (dbName != SqliteUtils.dbName)
          throw new ArgumentException(nameof (dbName), SqliteStatus.Initializing.ToString());
        return (Task) SqliteUtils.waitForInit.Task;
      case SqliteStatus.Initialized:
        return Task.CompletedTask;
      default:
        IEnumerable<char> source = ((IEnumerable<char>) Path.GetInvalidFileNameChars()).Distinct<char>();
        if (string.IsNullOrEmpty(dbName) || dbName.Length <= 3 || !dbName.ToUpperInvariant().EndsWith(".DB") || source.Any<char>((Func<char, bool>) (c => dbName.Contains<char>(c))))
          throw new ArgumentException(nameof (dbName));
        SqliteUtils.dbName = dbName;
        SqliteUtils.initializing = true;
        string dbpath = Path.Combine(SqliteUtils.GetLocalAppDataFolderPath(), dbName ?? "");
        Task.Run(TaskExceptionHelper.ExceptionBoundary((Func<Task>) (async () =>
        {
          try
          {
            bool createdNewDb;
            SqliteUtils.TryCreateDatabase(dbpath, out createdNewDb);
            SqliteUtils.dbFullName = dbpath;
            SqliteUtils.initializing = false;
            SqliteUtils.initialized = true;
            if (!createdNewDb)
              await SqliteUtils.TestConnectionAsync();
            SqliteUtils.waitForInit.SetResult(true);
          }
          catch (Exception ex)
          {
            SqliteUtils.waitForInit.SetException(ex);
          }
        }))).ConfigureAwait(false);
        return (Task) SqliteUtils.waitForInit.Task;
    }
  }

  public static SqliteStatus Status
  {
    get
    {
      if (SqliteUtils.initialized)
        return SqliteStatus.Initialized;
      return SqliteUtils.initializing ? SqliteStatus.Initializing : SqliteStatus.Uninitialized;
    }
  }

  private static void TryCreateDatabase(string dbFullName, out bool createdNewDb)
  {
    createdNewDb = false;
    if (!File.Exists(dbFullName))
    {
      bool createdNew;
      Mutex mutex = new Mutex(true, "PDFGEAR_CREATE_DB", out createdNew);
      try
      {
        if (!createdNew)
          mutex.WaitOne();
        if (!File.Exists(dbFullName))
        {
          createdNewDb = true;
          File.Create(dbFullName).Dispose();
        }
      }
      finally
      {
        mutex.ReleaseMutex();
      }
    }
    using (SQLiteConnection connection = new SQLiteConnection($"Data Source={dbFullName};Journal Mode=WAL"))
    {
      connection.Open();
      using (SQLiteCommand sqLiteCommand = new SQLiteCommand("CREATE TABLE IF NOT EXISTS configs (id INTEGER PRIMARY KEY NOT NULL, key NVARCHAR(500) NOT NULL UNIQUE, value TEXT NULL);", connection))
        sqLiteCommand.ExecuteNonQuery();
    }
  }

  private static async Task TestConnectionAsync()
  {
    await Task.Run(TaskExceptionHelper.ExceptionBoundary((Func<Task>) (async () =>
    {
      using (SQLiteConnection db = SqliteUtils.GetConnectionUnsafe())
      {
        using (SQLiteCommand select = new SQLiteCommand("SELECT 1", db))
        {
          try
          {
            int num = await select.ExecuteNonQueryAsync().ConfigureAwait(false);
          }
          catch
          {
          }
        }
      }
    }))).ConfigureAwait(false);
  }

  private static SQLiteConnection GetConnection(string dbFullName)
  {
    if (string.IsNullOrEmpty(dbFullName))
      return (SQLiteConnection) null;
    try
    {
      SQLiteConnection connection = new SQLiteConnection("Data Source=" + dbFullName);
      connection.DefaultTimeout = 60;
      connection.Open();
      return connection;
    }
    catch (Exception ex)
    {
      throw ex;
    }
  }

  internal static SQLiteConnection GetConnectionUnsafe()
  {
    return SqliteUtils.GetConnection(SqliteUtils.dbFullName);
  }

  public static async Task<SQLiteConnection> GetConnectionAsync()
  {
    await SqliteUtils.WaitForInit().ConfigureAwait(false);
    return SqliteUtils.GetConnection(SqliteUtils.dbFullName);
  }

  public static bool TryGet(string key, out string value)
  {
    value = (string) null;
    try
    {
      SqliteUtils.WaitForInit().Wait();
      value = (string) null;
      using (SQLiteConnection connectionUnsafe = SqliteUtils.GetConnectionUnsafe())
      {
        SQLiteDataReader core = SqliteUtils.TryGetCore(connectionUnsafe, key);
        try
        {
          if (core != null)
          {
            if (core.HasRows)
            {
              if (core.Read())
              {
                value = core.GetString(2);
                return true;
              }
            }
          }
        }
        catch
        {
        }
        finally
        {
          if (core != null && !core.IsClosed && core != null)
            core.Close();
        }
      }
    }
    catch
    {
    }
    return false;
  }

  public static async Task<(bool res, string value)> TryGetAsync(
    string key,
    CancellationToken cancellationToken)
  {
    cancellationToken.ThrowIfCancellationRequested();
    try
    {
      await SqliteUtils.WaitForInit().ConfigureAwait(false);
      using (SQLiteConnection db = SqliteUtils.GetConnectionUnsafe())
      {
        SQLiteDataReader result = SqliteUtils.TryGetCore(db, key);
        try
        {
          bool flag = result != null && result.HasRows;
          if (flag)
            flag = await result.ReadAsync().ConfigureAwait(false);
          if (flag)
            return (true, result.GetString(2));
        }
        catch (Exception ex) when (!(ex is OperationCanceledException))
        {
        }
        finally
        {
          SQLiteDataReader sqLiteDataReader = result;
          if ((sqLiteDataReader != null ? (!sqLiteDataReader.IsClosed ? 1 : 0) : 0) != 0)
            result?.Close();
        }
        result = (SQLiteDataReader) null;
      }
    }
    catch
    {
    }
    return ();
  }

  public static bool TrySet(string key, string value)
  {
    if (string.IsNullOrEmpty(key))
      return false;
    try
    {
      SqliteUtils.WaitForInit().Wait();
      using (SQLiteConnection connectionUnsafe = SqliteUtils.GetConnectionUnsafe())
      {
        SQLiteCommand sqLiteCommand = SqliteUtils.TrySetCore(connectionUnsafe, key, value);
        if (sqLiteCommand != null)
        {
          try
          {
            return sqLiteCommand.ExecuteNonQuery() > 0;
          }
          catch
          {
          }
        }
      }
    }
    catch
    {
    }
    return false;
  }

  public static async Task<bool> TrySetAsync(
    string key,
    string value,
    CancellationToken cancellationToken)
  {
    cancellationToken.ThrowIfCancellationRequested();
    try
    {
      if (string.IsNullOrEmpty(key))
        return false;
      await SqliteUtils.WaitForInit().ConfigureAwait(false);
      using (SQLiteConnection db = SqliteUtils.GetConnectionUnsafe())
      {
        using (SQLiteCommand command = SqliteUtils.TrySetCore(db, key, value))
        {
          if (command != null)
          {
            try
            {
              return await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false) > 0;
            }
            catch (Exception ex) when (!(ex is OperationCanceledException))
            {
            }
          }
        }
      }
    }
    catch
    {
    }
    return false;
  }

  public static async Task<bool> TrySetAsync(
    IEnumerable<KeyValuePair<string, string>> values,
    CancellationToken cancellationToken)
  {
    KeyValuePair<string, string>[] vs = SqliteUtils.PrepareKeyValues(values);
    try
    {
      await SqliteUtils.WaitForInit().ConfigureAwait(false);
      using (SQLiteConnection db = SqliteUtils.GetConnectionUnsafe())
      {
        using (SQLiteTransaction transaction = db.BeginTransaction())
        {
          try
          {
            KeyValuePair<string, string>[] keyValuePairArray = vs;
            for (int index = 0; index < keyValuePairArray.Length; ++index)
            {
              KeyValuePair<string, string> keyValuePair = keyValuePairArray[index];
              using (SQLiteCommand command = SqliteUtils.TrySetCore(db, keyValuePair.Key, keyValuePair.Value))
              {
                int num = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
              }
            }
            keyValuePairArray = (KeyValuePair<string, string>[]) null;
            transaction.Commit();
            return true;
          }
          catch (Exception ex)
          {
            transaction.Rollback();
            if (ex is OperationCanceledException)
              throw;
          }
        }
      }
    }
    catch
    {
    }
    return false;
  }

  public static bool TrySet(IEnumerable<KeyValuePair<string, string>> values)
  {
    try
    {
      KeyValuePair<string, string>[] keyValuePairArray = SqliteUtils.PrepareKeyValues(values);
      SqliteUtils.WaitForInit().Wait();
      using (SQLiteConnection connectionUnsafe = SqliteUtils.GetConnectionUnsafe())
      {
        using (SQLiteTransaction sqLiteTransaction = connectionUnsafe.BeginTransaction())
        {
          try
          {
            foreach (KeyValuePair<string, string> keyValuePair in keyValuePairArray)
            {
              using (SQLiteCommand sqLiteCommand = SqliteUtils.TrySetCore(connectionUnsafe, keyValuePair.Key, keyValuePair.Value))
                sqLiteCommand.ExecuteNonQuery();
            }
            sqLiteTransaction.Commit();
            return true;
          }
          catch
          {
            sqLiteTransaction.Rollback();
          }
        }
      }
      return false;
    }
    catch
    {
    }
    return false;
  }

  public static async Task<IEnumerable<string>> GetAllKeysAsync(CancellationToken cancellationToken)
  {
    try
    {
      await SqliteUtils.WaitForInit().ConfigureAwait(false);
      using (SQLiteConnection db = SqliteUtils.GetConnectionUnsafe())
      {
        using (SQLiteCommand select = new SQLiteCommand("SELECT key FROM configs", db))
        {
          List<string> list = new List<string>();
          try
          {
            SQLiteDataReader dr = select.ExecuteReader();
            while (true)
            {
              if (await dr.ReadAsync(cancellationToken).ConfigureAwait(false))
              {
                try
                {
                  string str = dr.GetString(0);
                  if (!string.IsNullOrEmpty(str))
                    list.Add(str);
                }
                catch
                {
                }
              }
              else
                break;
            }
            dr = (SQLiteDataReader) null;
          }
          catch (Exception ex) when (!(ex is OperationCanceledException))
          {
          }
          return (IEnumerable<string>) list;
        }
      }
    }
    catch
    {
    }
    return (IEnumerable<string>) null;
  }

  public static async Task<int> GetCountAsync(CancellationToken cancellationToken)
  {
    try
    {
      await SqliteUtils.WaitForInit().ConfigureAwait(false);
      using (SQLiteConnection db = SqliteUtils.GetConnectionUnsafe())
      {
        using (SQLiteCommand select = new SQLiteCommand("SELECT COUNT(*) FROM configs", db))
        {
          try
          {
            SQLiteDataReader dr = select.ExecuteReader();
            int num = await dr.ReadAsync(cancellationToken).ConfigureAwait(false) ? 1 : 0;
            return dr.GetInt32(0);
          }
          catch (Exception ex) when (!(ex is OperationCanceledException))
          {
          }
        }
        return 0;
      }
    }
    catch
    {
    }
    return -1;
  }

  private static SQLiteDataReader TryGetCore(SQLiteConnection db, string key)
  {
    if (SqliteUtils.CheckDb())
    {
      using (SQLiteCommand sqLiteCommand = new SQLiteCommand("SELECT * FROM configs WHERE key=@key", db))
      {
        sqLiteCommand.Parameters.Add(new SQLiteParameter(nameof (key), (object) key));
        try
        {
          return sqLiteCommand.ExecuteReader();
        }
        catch
        {
        }
      }
    }
    return (SQLiteDataReader) null;
  }

  private static SQLiteCommand TrySetCore(SQLiteConnection db, string key, string value)
  {
    if (!SqliteUtils.CheckDb())
      return (SQLiteCommand) null;
    return new SQLiteCommand("INSERT OR REPLACE INTO configs (key, value) VALUES (@key, @value)", db)
    {
      Parameters = {
        new SQLiteParameter(nameof (key), (object) key),
        new SQLiteParameter(nameof (value), (object) value)
      }
    };
  }

  private static KeyValuePair<string, string>[] PrepareKeyValues(
    IEnumerable<KeyValuePair<string, string>> values)
  {
    KeyValuePair<string, string>[] array = values.ToArray<KeyValuePair<string, string>>();
    if (((IEnumerable<KeyValuePair<string, string>>) array).Any<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (c => string.IsNullOrEmpty(c.Key))))
      return Array.Empty<KeyValuePair<string, string>>();
    return array.Length == 0 ? Array.Empty<KeyValuePair<string, string>>() : ((IEnumerable<KeyValuePair<string, string>>) array).GroupBy<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (c => c.Key.Trim())).Select<IGrouping<string, KeyValuePair<string, string>>, KeyValuePair<string, string>>((Func<IGrouping<string, KeyValuePair<string, string>>, KeyValuePair<string, string>>) (c => new KeyValuePair<string, string>(c.Key, c.LastOrDefault<KeyValuePair<string, string>>().Value))).ToArray<KeyValuePair<string, string>>();
  }

  internal static async Task WaitForInit()
  {
    if (SqliteUtils.waitForInit.Task.IsCompleted)
      return;
    int num = await SqliteUtils.waitForInit.Task.ConfigureAwait(false) ? 1 : 0;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static bool CheckDb() => SqliteUtils.Status == SqliteStatus.Initialized;
}
