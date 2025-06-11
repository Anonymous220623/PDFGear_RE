// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.UpdateHelper
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using CommomLib.Config;
using CommomLib.Controls;
using CommomLib.IAP;
using CommomLib.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

#nullable disable
namespace CommomLib.Commom;

public class UpdateHelper
{
  private static string updateMutexName = "BD3FAAC3-FE7A-4E18-B6E8-D0E164F5CD2B";
  private static bool updateInfoRequested = false;
  private static SemaphoreSlim getUpdateInfoLocker = new SemaphoreSlim(1, 1);

  public static Mutex CreateUpdateMutex()
  {
    Mutex updateMutex = (Mutex) null;
    bool createdNew = false;
    try
    {
      updateMutex = new Mutex(false, UpdateHelper.updateMutexName, out createdNew);
      if (createdNew)
        return updateMutex;
    }
    catch
    {
    }
    finally
    {
      if (!createdNew && updateMutex != null)
        updateMutex.Dispose();
    }
    return (Mutex) null;
  }

  public static async Task<bool> EndOfServicing()
  {
    Version version = AppIdHelper.Version;
    UpdateHelper.UpdateResponse model = await UpdateHelper.GetUpdateInfoAsync(true);
    if (model == null)
      return false;
    bool forceUpdate = false;
    if (model != null)
    {
      int num = await ConfigUtils.TrySetAsync<UpdateHelper.UpdateResponse>(ConfigureKeys.CheckUpdate, model, new CancellationToken()) ? 1 : 0;
      Version result;
      if (Version.TryParse(model.Eos, out result))
        forceUpdate = result >= version;
    }
    if (forceUpdate)
    {
      int num = (int) ModernMessageBox.Show(Resources.UpdateMessageNewEos, UtilManager.GetProductName());
      IAPHelper.LaunchDownloadUri();
      Application.Current.Shutdown();
    }
    return forceUpdate;
  }

  public static bool ShouldShowUpdateDialog()
  {
    return (DateTimeOffset.Now - ConfigManager.GetLastUpdateDialogShowTime()).TotalHours > 6.0;
  }

  public static void RemoveUpdateDialogShownFlag()
  {
    ConfigManager.SetLastUpdateDialogShowTime(new DateTimeOffset());
  }

  public static async Task<bool> UpdateAndExit(bool checkUpdateManually = false)
  {
    UpdateResult updateResult = await UpdateHelper.Update(checkUpdateManually);
    return updateResult != null && updateResult.ShouldExit;
  }

  public static async Task<UpdateResult> Update(bool checkUpdateManually)
  {
    if (!checkUpdateManually)
    {
      Mutex updateMutex = UpdateHelper.CreateUpdateMutex();
      if (updateMutex == null)
        return new UpdateResult();
      updateMutex.Dispose();
    }
    try
    {
      Version version = AppIdHelper.Version;
      Task<UpdateResult> task = Task.Run<UpdateResult>((Func<Task<UpdateResult>>) (async () =>
      {
        UpdateResult res = await UpdateHelper.TryUpdateCore(checkUpdateManually);
        UpdateResult updateResult = res;
        if ((updateResult != null ? (updateResult.ShouldExit ? 1 : 0) : 0) != 0)
          await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Send, (Delegate) (() => Application.Current.Shutdown()));
        return res;
      }));
      return checkUpdateManually ? await task : new UpdateResult();
    }
    catch
    {
    }
    return new UpdateResult();
  }

  private static async Task<UpdateResult> TryUpdateCore(bool checkUpdateManually)
  {
    Version version = AppIdHelper.Version;
    Version newVer = (Version) null;
    UpdateHelper.UpdateResponse model = await UpdateHelper.GetUpdateInfoAsync(checkUpdateManually);
    bool canUpdate = false;
    if (model != null)
    {
      if (Version.TryParse(model.Ver, out newVer))
        canUpdate = newVer > version;
    }
    else if (model == null & checkUpdateManually)
      return new UpdateResult()
      {
        HasUpdate = canUpdate,
        UpdateSuccess = false
      };
    bool updateSuccess = false;
    if (canUpdate && !string.IsNullOrEmpty(model.Down))
    {
      TaskCompletionSource<UpdateResult> updateResult1 = new TaskCompletionSource<UpdateResult>();
      await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Delegate) (async () =>
      {
        bool flag = true;
        Mutex mutex = (Mutex) null;
        try
        {
          if (!checkUpdateManually && ConfigManager.getNotShowVersion() == newVer)
            flag = false;
          if (flag)
          {
            mutex = UpdateHelper.CreateUpdateMutex();
            if (mutex == null)
            {
              flag = false;
              int num = (int) ModernMessageBox.Show(Resources.UpdateMessageRuning);
              updateResult1.SetResult(new UpdateResult());
            }
          }
          if (flag)
          {
            GAManager.SendEvent("AppUpdate", "UpdateDetected", newVer.ToString(), 1L);
            ConfigManager.SetLastUpdateDialogShowTime(DateTimeOffset.Now);
            bool? nullable = UpdateMessage.CreateHasNewVersionDialog(newVer, checkUpdateManually).ShowDialog();
            flag = nullable.HasValue && nullable.GetValueOrDefault();
            GAManager.SendEvent("AppUpdate", "UserChoose" + (flag ? "UpdateNow" : "NotUpdate"), newVer.ToString(), 1L);
          }
          if (flag)
          {
            UpdateWindow updater = new UpdateWindow(model.Down, model.MD5, newVer)
            {
              Owner = Application.Current.MainWindow
            };
            updater.WindowStartupLocation = updater.Owner != null ? WindowStartupLocation.CenterOwner : WindowStartupLocation.CenterScreen;
            bool? nullable1 = updater.ShowDialog();
            updater.UpdateResult.HasUpdate = canUpdate;
            bool showFailedMessage = false;
            if (nullable1.GetValueOrDefault())
            {
              GAManager.SendEvent("AppUpdate", "AppInstall", newVer.ToString(), 1L);
              UpdateResult updateResult2 = updater.UpdateResult;
              updateResult2.UpdateSuccess = await UpdateHelper.ExecuteFile(updater.UpdateResult.SetupFilePath, (Window) updater);
              updateResult2 = (UpdateResult) null;
              if (updater.UpdateResult.UpdateSuccess)
              {
                GAManager.SendEvent("AppUpdate", "UpdateSuccess", newVer.ToString(), 1L);
                updateResult1.SetResult(updater.UpdateResult);
              }
              else
              {
                GAManager.SendEvent("AppUpdate", "UpdateFailed", newVer.ToString(), 1L);
                showFailedMessage = true;
              }
            }
            else if (updater.Canceled)
            {
              GAManager.SendEvent("AppUpdate", "UserCancelDownload", newVer.ToString(), 1L);
              int num = (int) ModernMessageBox.Show(Resources.UpdateMessageCancelSuccess);
            }
            else if (!updater.Install)
            {
              GAManager.SendEvent("AppUpdate", "UserCancelUpdate", newVer.ToString(), 1L);
              int num = (int) ModernMessageBox.Show(Resources.UpdateMessageCancelSuccess);
            }
            else
            {
              GAManager.SendEvent("AppUpdate", "DownloadFailed", newVer.ToString(), 1L);
              showFailedMessage = true;
            }
            if (showFailedMessage)
            {
              bool? nullable2 = UpdateMessage.CreateUpdateFailedDialog().ShowDialog();
              if ((!nullable2.HasValue ? 0 : (nullable2.GetValueOrDefault() ? 1 : 0)) != 0)
                updateResult1.SetResult(updater.UpdateResult);
              else
                updateResult1.SetResult(new UpdateResult());
            }
            updater = (UpdateWindow) null;
            mutex = (Mutex) null;
          }
          else
          {
            updateResult1.SetResult(new UpdateResult());
            mutex = (Mutex) null;
          }
        }
        catch (Exception ex)
        {
          GAManager.SendEvent("AppUpdate", "Exception", $"{ex.GetType().Name}, {ex.Message}", 1L);
          updateResult1.TrySetException(ex);
          mutex = (Mutex) null;
        }
        finally
        {
          mutex?.Dispose();
          mutex = (Mutex) null;
        }
      }));
      return await updateResult1.Task;
    }
    if (!canUpdate & checkUpdateManually)
      await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Delegate) (() => UpdateMessage.CreateUptodateWindows().ShowDialog()));
    return new UpdateResult()
    {
      HasUpdate = canUpdate,
      UpdateSuccess = updateSuccess
    };
  }

  private static async Task<UpdateHelper.UpdateResponse> GetUpdateInfoAsync(bool checkUpdateManually)
  {
    UpdateHelper.UpdateResponse updateInfoAsync;
    try
    {
      await UpdateHelper.getUpdateInfoLocker.WaitAsync();
      UpdateHelper.UpdateResponse model = (UpdateHelper.UpdateResponse) null;
      if (checkUpdateManually || !UpdateHelper.updateInfoRequested)
        model = await InternalActivateHelper.CheckUpdate();
      if (model != null)
      {
        UpdateHelper.updateInfoRequested = true;
        int num = await ConfigUtils.TrySetAsync<UpdateHelper.UpdateResponse>(ConfigureKeys.CheckUpdate, model, new CancellationToken()) ? 1 : 0;
      }
      else
      {
        (bool, UpdateHelper.UpdateResponse) async = await ConfigUtils.TryGetAsync<UpdateHelper.UpdateResponse>(ConfigureKeys.CheckUpdate, new CancellationToken());
        if (async.Item1 && async.Item2 != null)
          model = async.Item2;
      }
      updateInfoAsync = model;
    }
    finally
    {
      UpdateHelper.getUpdateInfoLocker.Release();
    }
    return updateInfoAsync;
  }

  internal static async Task<string> DownloadUpdateFile(
    string downloadUrl,
    string validationMD5,
    Action<HttpHelperDownloadResponse> progressReporter,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrEmpty(downloadUrl) || cancellationToken.IsCancellationRequested)
      return (string) null;
    string tempFolder = Path.Combine(Path.GetTempPath(), UtilManager.GetProductName());
    string path2 = "";
    try
    {
      Uri result;
      if (Uri.TryCreate(downloadUrl, UriKind.Absolute, out result))
      {
        path2 = ((IEnumerable<string>) result.AbsolutePath.Split('/')).LastOrDefault<string>();
        if (string.IsNullOrEmpty(path2) || path2.IndexOfAny(Path.GetInvalidFileNameChars()) > 0)
          path2 = "Setup.exe";
      }
      Directory.CreateDirectory(tempFolder);
    }
    catch
    {
      return (string) null;
    }
    string filePath = Path.Combine(tempFolder, path2);
    FileStream tmpStream;
    try
    {
      if (File.Exists(filePath))
      {
        if (string.IsNullOrEmpty(validationMD5))
          throw new ArgumentException("filePath");
        tmpStream = File.OpenRead(filePath);
        try
        {
          if (string.Equals(await UpdateHelper.GetMD5((Stream) tmpStream), validationMD5, StringComparison.OrdinalIgnoreCase))
            return filePath;
          throw new ArgumentException("filePath");
        }
        finally
        {
          tmpStream?.Dispose();
        }
      }
    }
    catch
    {
      try
      {
        if (File.Exists(filePath))
          File.Delete(filePath);
      }
      catch
      {
      }
    }
    try
    {
      tmpStream = new FileStream(filePath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);
      try
      {
        await HttpHelper.DownloadAsync(downloadUrl, (Stream) tmpStream, progressReporter, cancellationToken).ConfigureAwait(false);
        tmpStream.SetLength(tmpStream.Position);
        tmpStream.Seek(0L, SeekOrigin.Begin);
        if (!string.IsNullOrEmpty(validationMD5))
        {
          if (!string.Equals(validationMD5, await UpdateHelper.GetMD5((Stream) tmpStream), StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("md5");
        }
      }
      finally
      {
        tmpStream?.Dispose();
      }
      tmpStream = (FileStream) null;
      return filePath;
    }
    catch
    {
      try
      {
        Directory.Delete(tempFolder, true);
      }
      catch
      {
      }
      throw;
    }
  }

  private static async Task<string> GetMD5(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (stream.Position != 0L)
    {
      if (!stream.CanSeek)
        throw new ArgumentException(nameof (stream));
      stream.Seek(0L, SeekOrigin.Begin);
    }
    string str = await Task.Run<string>((Func<string>) (() =>
    {
      try
      {
        using (MD5 md5 = MD5.Create())
          return ((IEnumerable<byte>) md5.ComputeHash(stream)).Aggregate<byte, StringBuilder>(new StringBuilder(), (Func<StringBuilder, byte, StringBuilder>) ((sb, c) => sb.AppendFormat("{0:x2}", (object) c))).ToString();
      }
      catch
      {
      }
      return "";
    }));
    return !string.IsNullOrEmpty(str) ? str : throw new ArgumentException("md5");
  }

  internal static UpdateHelper.FileSizeFormatType AdjustFileSizeFormatType(params long[] sizes)
  {
    long? nullable1 = sizes != null ? new long?(((IEnumerable<long>) sizes).DefaultIfEmpty<long>().Max()) : new long?();
    long? nullable2 = nullable1;
    long num1 = 100;
    if (nullable2.GetValueOrDefault() < num1 & nullable2.HasValue)
      return UpdateHelper.FileSizeFormatType.Byte;
    nullable2 = nullable1;
    long num2 = 100000;
    if (nullable2.GetValueOrDefault() < num2 & nullable2.HasValue)
      return UpdateHelper.FileSizeFormatType.KB;
    nullable2 = nullable1;
    long num3 = 100000000;
    return nullable2.GetValueOrDefault() < num3 & nullable2.HasValue ? UpdateHelper.FileSizeFormatType.MB : UpdateHelper.FileSizeFormatType.GB;
  }

  internal static string FormatFileSize(
    long size,
    IFormatProvider formatProvider,
    UpdateHelper.FileSizeFormatType type)
  {
    double num = Math.Pow(1000.0, (double) type);
    return FormattableStringFactory.Create("{0} {1}", (object) $"{(double) size * 1.0 / num:F2}", (object) type).ToString(formatProvider);
  }

  private static async Task<bool> ExecuteFile(string filePath, Window window)
  {
    return await Task.Run<bool>((Func<Task<bool>>) (async () =>
    {
      try
      {
        Process process = Process.Start(new ProcessStartInfo(filePath, "/silent /update")
        {
          Verb = "runas",
          UseShellExecute = true
        });
        if (process != null)
        {
          try
          {
            if (process.WaitForInputIdle())
              return true;
          }
          catch (Exception ex)
          {
            Log.WriteLog($"ExecuteFile WaitForInputIdle failed: {ex}");
            try
            {
              string processName = Path.GetFileNameWithoutExtension(filePath);
              if (string.IsNullOrEmpty(processName))
                return false;
              CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(30.0));
              while (true)
              {
                try
                {
                  if (Process.GetProcessesByName(processName) != null)
                    return true;
                }
                catch
                {
                }
                try
                {
                  if (((IEnumerable<Process>) Process.GetProcesses()).Any<Process>((Func<Process, bool>) (c => string.Equals(c.ProcessName, processName))))
                    return true;
                }
                catch
                {
                }
                await Task.Delay(1000, cts.Token);
              }
            }
            catch
            {
              return false;
            }
          }
        }
      }
      catch (Exception ex)
      {
        Log.WriteLog($"ExecuteFile start process failed: {ex}");
      }
      return false;
    })).ConfigureAwait(false);
  }

  internal enum FileSizeFormatType
  {
    Byte,
    KB,
    MB,
    GB,
  }

  internal class UpdateResponse
  {
    public string Eos { get; set; }

    public string Ver { get; set; }

    public string Down { get; set; }

    public string MD5 { get; set; }
  }
}
