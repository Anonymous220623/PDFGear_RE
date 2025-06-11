// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.Copilot.CopilotHelper
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using CommomLib.Config;
using CommomLib.Views;
using LruCacheNet;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Newtonsoft.Json;
using Nito.AsyncEx;
using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using pdfeditor.Controls.Copilot;
using pdfeditor.Models.Thumbnails;
using pdfeditor.Utils.Enums;
using pdfeditor.ViewModels;
using pdfeditor.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Resources;
using System.Windows.Threading;

#nullable enable
namespace pdfeditor.Utils.Copilot;

public class CopilotHelper : IDisposable
{
  private const int maxPageCountForAnalyze = 120;
  public const int MaxSendChatCount = 50;
  private const bool requestStreamingApi = true;
  private bool disposedValue;
  private 
  #nullable disable
  CancellationTokenSource cts;
  private WeakReference<PdfDocument> _document;
  private string filePath;
  private Dictionary<int, CopilotHelper.AnalyzeResponseModel> memoryCache = new Dictionary<int, CopilotHelper.AnalyzeResponseModel>();
  private Dictionary<string, CopilotHelper.AnalyzeResponseModel> memoryCache2 = new Dictionary<string, CopilotHelper.AnalyzeResponseModel>();
  private LruCache<int, string> cachedPageContent = new LruCache<int, string>(50);
  private Dictionary<int, string> cachedPageOcrContent = new Dictionary<int, string>(50);
  private List<CopilotHelper.ChatMessage> cachedChatMessage = new List<CopilotHelper.ChatMessage>();
  private bool chatting;
  private CopilotHelper.AnalyzeTask analyzeTask;
  private bool ocrGaSended;
  private CultureInfo ocrCultureInfo;
  private Dictionary<string, int> ocrCultureInfoCount = new Dictionary<string, int>();
  private int pageCountForAnalyze;

  public PdfDocument Document
  {
    get
    {
      PdfDocument target;
      return this._document != null && this._document.TryGetTarget(out target) ? target : (PdfDocument) null;
    }
  }

  internal bool Initialized { get; private set; }

  internal bool InitializeSucceed { get; private set; }

  public CopilotHelper(PdfDocument document, string filePath)
  {
    this.cts = new CancellationTokenSource();
    this._document = new WeakReference<PdfDocument>(document);
    this.filePath = filePath;
    this.pageCountForAnalyze = Math.Min(120, document.Pages.Count);
    this.analyzeTask = new CopilotHelper.AnalyzeTask(this);
  }

  public async Task InitializeAsync(IProgress<double> progressReporter)
  {
    if (this.Document == null)
      return;
    await CopilotHelper.AppActionHelper.InitializeAllActions();
    await this.analyzeTask.Start(progressReporter);
    this.Initialized = true;
  }

  public async Task<CopilotHelper.CopilotResult> GetSummaryAsync(
    Func<string, CancellationToken, Task> summaryAction,
    CancellationToken cancellationToken)
  {
    if (!this.Initialized || this.chatting)
      return CopilotHelper.CopilotResult.EmptyUnknownFailed;
    if (!await this.CanChatAsync())
      return CopilotHelper.CopilotResult.ChatCountLimitFailed;
    PdfDocument document = this.Document;
    if (document == null)
      return CopilotHelper.CopilotResult.ContentEmptyFailed;
    CancellationTokenSource cts2 = CancellationTokenSource.CreateLinkedTokenSource(this.cts.Token, cancellationToken);
    CopilotHelper.PdfModel pdfModel = (CopilotHelper.PdfModel) null;
    try
    {
      this.chatting = true;
      int count = 5;
      if (count > this.pageCountForAnalyze)
        count = this.pageCountForAnalyze;
      if (count > document.Pages.Count)
        count = document.Pages.Count;
      pdfModel = await this.BuildPdfModel(await Task.WhenAll<(int, CopilotHelper.AnalyzeResponseModel)>(Enumerable.Range(0, count).Select<int, Task<(int, CopilotHelper.AnalyzeResponseModel)>>((Func<int, Task<(int, CopilotHelper.AnalyzeResponseModel)>>) (async c =>
      {
        int num = c;
        return (num, await this.AnalyzeAsync(c));
      })).ToArray<Task<(int, CopilotHelper.AnalyzeResponseModel)>>()), cts2.Token);
      if (pdfModel?.Pages == null || pdfModel.Pages.Count == 0)
      {
        CommomLib.Commom.GAManager.SendEvent("ChatPdf", "Summary", "Summary_Pdf_Content_Empty", 1L);
        return CopilotHelper.CopilotResult.ContentEmptyFailed;
      }
      StringBuilder sb = new StringBuilder();
      CommomLib.Commom.GAManager.SendEvent("ChatPdf", "Summary", "Start", 1L);
      CopilotHelper.StreamRequestResult summaryAsync = await CopilotHelper.InternalCopilotHelper.GetSummaryAsync(pdfModel, true, (Func<string, CancellationToken, Task>) (async (result, ct) =>
      {
        sb.Append(result);
        await summaryAction(result, ct);
      }), cts2.Token);
      if (!summaryAsync.Success)
        return new CopilotHelper.CopilotResult((int[]) null, (string) null, summaryAsync.Error);
      string summaryContent = sb.ToString();
      CommomLib.Commom.GAManager.SendEvent("ChatPdf", "Summary", "Done", 1L);
      await CopilotHelper.Cache.SetSummaryToCache(this.filePath, sb.ToString());
      List<CopilotHelper.PdfPageModel> pages = pdfModel.Pages;
      return new CopilotHelper.CopilotResult(pages != null ? pages.Select<CopilotHelper.PdfPageModel, int>((Func<CopilotHelper.PdfPageModel, int>) (c => c.PageIndex)).OrderBy<int, int>((Func<int, int>) (c => c)).ToArray<int>() : (int[]) null, summaryContent, (CopilotHelper.AppActionModel) null, (string) null, false);
    }
    catch (OperationCanceledException ex) when (ex.CancellationToken == cancellationToken)
    {
      return CopilotHelper.CopilotResult.UserCanceledFailed;
    }
    catch (Exception ex)
    {
      try
      {
        Log.WriteLog("PdfModel: " + JsonConvert.SerializeObject((object) pdfModel, Formatting.Indented));
      }
      catch
      {
      }
      Log.WriteLog(ex.ToString());
      CommomLib.Commom.GAManager.SendEvent("ChatPdf", "Summary", "Summary_Failed_" + ex.GetType().Name, 1L);
    }
    finally
    {
      this.chatting = false;
    }
    return CopilotHelper.CopilotResult.EmptyUnknownFailed;
  }

  public async void LikedAsyne(CopilotHelper.ChatMessage message)
  {
    int num = await CopilotHelper.Cache.AppendChatMessagesLikedToCache(this.filePath, message) ? 1 : 0;
  }

  public async Task<CopilotHelper.CopilotResult> GetAppActionAsync(
    string message,
    CancellationToken cancellationToken)
  {
    if (!this.Initialized || this.chatting || string.IsNullOrEmpty(message))
      return CopilotHelper.CopilotResult.EmptyUnknownMaybeAppActionFailed;
    if (!await this.CanChatAsync())
      return CopilotHelper.CopilotResult.EmptyUnknownMaybeAppActionFailed;
    CancellationTokenSource cts2 = CancellationTokenSource.CreateLinkedTokenSource(this.cts.Token, cancellationToken);
    try
    {
      CopilotHelper.AnalyzeResponseModel analyzeResponseModel = await this.AnalyzeMessageAsync(message, cts2.Token);
      CopilotHelper.AnalyzeResponseModel.AnalyzeResponseDataModel responseDataModel;
      if (analyzeResponseModel == null)
      {
        responseDataModel = (CopilotHelper.AnalyzeResponseModel.AnalyzeResponseDataModel) null;
      }
      else
      {
        List<CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel> items = analyzeResponseModel.Items;
        if (items == null)
        {
          responseDataModel = (CopilotHelper.AnalyzeResponseModel.AnalyzeResponseDataModel) null;
        }
        else
        {
          CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel responseItemModel = items.FirstOrDefault<CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel>();
          if (responseItemModel == null)
          {
            responseDataModel = (CopilotHelper.AnalyzeResponseModel.AnalyzeResponseDataModel) null;
          }
          else
          {
            List<CopilotHelper.AnalyzeResponseModel.AnalyzeResponseDataModel> data = responseItemModel.Data;
            responseDataModel = data != null ? data.FirstOrDefault<CopilotHelper.AnalyzeResponseModel.AnalyzeResponseDataModel>() : (CopilotHelper.AnalyzeResponseModel.AnalyzeResponseDataModel) null;
          }
        }
      }
      if (responseDataModel == null)
        return CopilotHelper.CopilotResult.EmptyUnknownMaybeAppActionFailed;
      string input = message;
      float[] embedding;
      if (analyzeResponseModel == null)
      {
        embedding = (float[]) null;
      }
      else
      {
        List<CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel> items = analyzeResponseModel.Items;
        if (items == null)
        {
          embedding = (float[]) null;
        }
        else
        {
          CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel responseItemModel = items.FirstOrDefault<CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel>();
          if (responseItemModel == null)
          {
            embedding = (float[]) null;
          }
          else
          {
            List<CopilotHelper.AnalyzeResponseModel.AnalyzeResponseDataModel> data = responseItemModel.Data;
            embedding = data != null ? data.FirstOrDefault<CopilotHelper.AnalyzeResponseModel.AnalyzeResponseDataModel>()?.Values : (float[]) null;
          }
        }
      }
      CancellationToken token = cts2.Token;
      (CopilotHelper.AppActionModel appAction, bool maybeNotAppAction) = await CopilotHelper.AppActionHelper.GetAction(input, embedding, token);
      return appAction == null ? (maybeNotAppAction ? CopilotHelper.CopilotResult.EmptyUnknownMaybeAppActionFailed : CopilotHelper.CopilotResult.EmptyUnknownFailed) : new CopilotHelper.CopilotResult((int[]) null, appAction.Confirm, appAction, (string) null, maybeNotAppAction);
    }
    catch (OperationCanceledException ex) when (ex.CancellationToken != this.cts.Token)
    {
      return CopilotHelper.CopilotResult.UserCanceledFailed;
    }
    catch (Exception ex)
    {
      Log.WriteLog(ex.ToString());
      CommomLib.Commom.GAManager.SendEvent("ChatPdf", "GetAppAction_Failed_" + ex.GetType().Name, "Count", 1L);
    }
    finally
    {
      this.chatting = false;
    }
    return CopilotHelper.CopilotResult.EmptyUnknownMaybeAppActionFailed;
  }

  public async Task<CopilotHelper.CopilotResult> ChatAsync(
    string message,
    Func<string, CancellationToken, Task> chatAction,
    CancellationToken cancellationToken)
  {
    if (!this.Initialized || this.chatting || string.IsNullOrEmpty(message))
      return CopilotHelper.CopilotResult.EmptyUnknownFailed;
    if (!await this.CanChatAsync())
      return CopilotHelper.CopilotResult.EmptyUnknownFailed;
    CopilotHelper.PdfModel pdfModel = (CopilotHelper.PdfModel) null;
    CancellationTokenSource cts2 = CancellationTokenSource.CreateLinkedTokenSource(this.cts.Token, cancellationToken);
    try
    {
      PdfDocument doc = this.Document;
      if (doc == null)
        return CopilotHelper.CopilotResult.EmptyUnknownFailed;
      this.chatting = true;
      CopilotHelper.AnalyzeResponseModel messageEmbedding = await this.AnalyzeMessageAsync(message, cts2.Token);
      CopilotHelper.AnalyzeResponseModel analyzeResponseModel = messageEmbedding;
      CopilotHelper.AnalyzeResponseModel.AnalyzeResponseDataModel responseDataModel1;
      if (analyzeResponseModel == null)
      {
        responseDataModel1 = (CopilotHelper.AnalyzeResponseModel.AnalyzeResponseDataModel) null;
      }
      else
      {
        List<CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel> items = analyzeResponseModel.Items;
        if (items == null)
        {
          responseDataModel1 = (CopilotHelper.AnalyzeResponseModel.AnalyzeResponseDataModel) null;
        }
        else
        {
          CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel responseItemModel = items.FirstOrDefault<CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel>();
          if (responseItemModel == null)
          {
            responseDataModel1 = (CopilotHelper.AnalyzeResponseModel.AnalyzeResponseDataModel) null;
          }
          else
          {
            List<CopilotHelper.AnalyzeResponseModel.AnalyzeResponseDataModel> data = responseItemModel.Data;
            responseDataModel1 = data != null ? data.FirstOrDefault<CopilotHelper.AnalyzeResponseModel.AnalyzeResponseDataModel>() : (CopilotHelper.AnalyzeResponseModel.AnalyzeResponseDataModel) null;
          }
        }
      }
      if (responseDataModel1 == null)
        return CopilotHelper.CopilotResult.EmptyUnknownFailed;
      pdfModel = await this.BuildPdfModel(((IEnumerable<(int, CopilotHelper.AnalyzeResponseModel)>) await Task.WhenAll<(int, CopilotHelper.AnalyzeResponseModel)>(Enumerable.Range(0, Math.Min(this.pageCountForAnalyze, doc.Pages.Count)).Select<int, Task<(int, CopilotHelper.AnalyzeResponseModel)>>((Func<int, Task<(int, CopilotHelper.AnalyzeResponseModel)>>) (async c =>
      {
        int num = c;
        return (num, await this.AnalyzeAsync(c));
      })).ToArray<Task<(int, CopilotHelper.AnalyzeResponseModel)>>())).Where<(int, CopilotHelper.AnalyzeResponseModel)>((Func<(int, CopilotHelper.AnalyzeResponseModel), bool>) (c =>
      {
        CopilotHelper.AnalyzeResponseModel data1 = c.data;
        float[] numArray;
        if (data1 == null)
        {
          numArray = (float[]) null;
        }
        else
        {
          List<CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel> items = data1.Items;
          if (items == null)
          {
            numArray = (float[]) null;
          }
          else
          {
            CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel responseItemModel = items.FirstOrDefault<CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel>();
            if (responseItemModel == null)
            {
              numArray = (float[]) null;
            }
            else
            {
              List<CopilotHelper.AnalyzeResponseModel.AnalyzeResponseDataModel> data2 = responseItemModel.Data;
              numArray = data2 != null ? data2.FirstOrDefault<CopilotHelper.AnalyzeResponseModel.AnalyzeResponseDataModel>()?.Values : (float[]) null;
            }
          }
        }
        return numArray != null;
      })).SelectMany<(int, CopilotHelper.AnalyzeResponseModel), (int, CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel)>((Func<(int, CopilotHelper.AnalyzeResponseModel), IEnumerable<(int, CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel)>>) (c => c.data.Items.Select<CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel, (int, CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel)>((Func<CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel, (int, CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel)>) (x => (c.pageIndex, x))))).OrderBy<(int, CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel), float>((Func<(int, CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel), float>) (c =>
      {
        float val2 = float.MaxValue;
        foreach (CopilotHelper.AnalyzeResponseModel.AnalyzeResponseDataModel responseDataModel2 in c.data.Data)
        {
          float[] values1 = responseDataModel2.Values;
          float[] values2 = messageEmbedding.Items[0].Data[0].Values;
          if (values1 != null && values2 != null)
            val2 = Math.Min(CopilotHelper.SimpleCosineSimilarityFloatVersion.ComputeDistance(values1, values2), val2);
        }
        return val2;
      })).ToArray<(int, CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel)>(), cts2.Token);
      if (pdfModel?.Pages == null || pdfModel.Pages.Count == 0)
      {
        Log.WriteLog("ChatAsync: PdfModel is Empty");
        CommomLib.Commom.GAManager.SendEvent("ChatPdf", "Chat_Pdf_Content_Empty", "Count", 1L);
      }
      if (pdfModel.PageCount == 0)
        return CopilotHelper.CopilotResult.EmptyUnknownFailed;
      List<CopilotHelper.ChatMessage> list = this.cachedChatMessage.ToList<CopilotHelper.ChatMessage>();
      CopilotHelper.ChatMessage userMessage = new CopilotHelper.ChatMessage()
      {
        Content = message,
        Role = "user"
      };
      list.Add(userMessage);
      StringBuilder sb = new StringBuilder();
      int num1 = await CopilotHelper.Cache.IncreaseChatCountAsync(this.filePath);
      this.analyzeTask.Continue();
      CommomLib.Commom.GAManager.SendEvent("ChatPdf", "ChatCount", $"{num1}", 1L);
      CopilotHelper.StreamRequestResult streamRequestResult = await CopilotHelper.InternalCopilotHelper.ChatAsync(list, pdfModel, true, (Func<string, CancellationToken, Task>) (async (result, ct) =>
      {
        sb.Append(result);
        await chatAction(result, ct);
      }), cts2.Token);
      if (!streamRequestResult.Success)
        return new CopilotHelper.CopilotResult((int[]) null, (string) null, streamRequestResult.Error);
      CopilotHelper.ChatMessage systemMessage = new CopilotHelper.ChatMessage()
      {
        Content = sb.ToString(),
        Role = "assistant",
        Pages = pdfModel.Pages.Select<CopilotHelper.PdfPageModel, int>((Func<CopilotHelper.PdfPageModel, int>) (c => c.PageIndex)).ToArray<int>()
      };
      list.Add(systemMessage);
      while (list.Count > 6)
        list.RemoveAt(0);
      this.cachedChatMessage = list;
      if (!string.IsNullOrEmpty(systemMessage.Content))
      {
        int num2 = await CopilotHelper.Cache.AppendChatMessagesToCache(this.filePath, userMessage, systemMessage) ? 1 : 0;
      }
      return new CopilotHelper.CopilotResult(systemMessage?.Pages, systemMessage.Content, (string) null);
    }
    catch (OperationCanceledException ex) when (ex.CancellationToken != this.cts.Token)
    {
      return CopilotHelper.CopilotResult.UserCanceledFailed;
    }
    catch (Exception ex)
    {
      try
      {
        Log.WriteLog("PdfModel: " + JsonConvert.SerializeObject((object) pdfModel, Formatting.Indented));
        Log.WriteLog("Chat records: " + JsonConvert.SerializeObject((object) this.cachedChatMessage.ToList<CopilotHelper.ChatMessage>()));
      }
      catch
      {
      }
      Log.WriteLog(ex.ToString());
      CommomLib.Commom.GAManager.SendEvent("ChatPdf", "Chat_Failed_" + ex.GetType().Name, "Count", 1L);
    }
    finally
    {
      this.chatting = false;
    }
    return CopilotHelper.CopilotResult.EmptyUnknownFailed;
  }

  private Task<CopilotHelper.PdfModel> BuildPdfModel(
    (int pageIndex, CopilotHelper.AnalyzeResponseModel data)[] items,
    CancellationToken cancellationToken)
  {
    return this.BuildPdfModel(((IEnumerable<(int, CopilotHelper.AnalyzeResponseModel)>) items).Where<(int, CopilotHelper.AnalyzeResponseModel)>((Func<(int, CopilotHelper.AnalyzeResponseModel), bool>) (c =>
    {
      CopilotHelper.AnalyzeResponseModel data1 = c.data;
      float[] numArray;
      if (data1 == null)
      {
        numArray = (float[]) null;
      }
      else
      {
        List<CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel> items1 = data1.Items;
        if (items1 == null)
        {
          numArray = (float[]) null;
        }
        else
        {
          CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel responseItemModel = items1.FirstOrDefault<CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel>();
          if (responseItemModel == null)
          {
            numArray = (float[]) null;
          }
          else
          {
            List<CopilotHelper.AnalyzeResponseModel.AnalyzeResponseDataModel> data2 = responseItemModel.Data;
            numArray = data2 != null ? data2.FirstOrDefault<CopilotHelper.AnalyzeResponseModel.AnalyzeResponseDataModel>()?.Values : (float[]) null;
          }
        }
      }
      return numArray != null;
    })).SelectMany<(int, CopilotHelper.AnalyzeResponseModel), (int, CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel)>((Func<(int, CopilotHelper.AnalyzeResponseModel), IEnumerable<(int, CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel)>>) (c => c.data.Items.Select<CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel, (int, CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel)>((Func<CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel, (int, CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel)>) (x => (c.pageIndex, x))))).ToArray<(int, CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel)>(), cancellationToken);
  }

  private async Task<CopilotHelper.PdfModel> BuildPdfModel(
    (int pageIndex, CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel data)[] items,
    CancellationToken cancellationToken)
  {
    PdfDocument doc = this.Document;
    if (doc == null)
      return (CopilotHelper.PdfModel) null;
    int num1 = 0;
    CopilotHelper.PdfModel pdfModel = new CopilotHelper.PdfModel()
    {
      Pages = new List<CopilotHelper.PdfPageModel>()
    };
    Dictionary<int, List<(int, int)>> dictionary = new Dictionary<int, List<(int, int)>>();
    foreach ((int num2, CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel data) in items)
    {
      num1 += data.Usage;
      if (num1 < 2500)
      {
        List<(int, int)> valueTupleList;
        if (!dictionary.TryGetValue(num2, out valueTupleList))
        {
          valueTupleList = new List<(int, int)>();
          dictionary[num2] = valueTupleList;
        }
        valueTupleList.Add((data.TextIndex, data.TextLength));
      }
    }
    if (dictionary.Count > 0)
    {
      foreach (KeyValuePair<int, List<(int, int)>> keyValuePair in dictionary)
      {
        KeyValuePair<int, List<(int, int)>> item = keyValuePair;
        int pageIndex = item.Key;
        if (item.Value.Count > 0)
        {
          string pageContentAsync = await this.GetPageContentAsync(pageIndex, cancellationToken);
          if (!string.IsNullOrEmpty(pageContentAsync))
          {
            List<(int, int)> list = item.Value.OrderBy<(int, int), int>((Func<(int, int), int>) (c => c.index)).ToList<(int, int)>();
            for (int index = list.Count - 2; index >= 0; --index)
            {
              (int, int) tuple1 = list[index + 1];
              (int, int) tuple2 = list[index];
              if (tuple2.Item1 + tuple2.Item2 >= tuple1.Item1)
              {
                list[index] = (tuple2.Item1, tuple1.Item1 + tuple1.Item2 - tuple2.Item1);
                list.RemoveAt(index + 1);
              }
            }
            StringBuilder stringBuilder = new StringBuilder();
            for (int index = 0; index < list.Count; ++index)
            {
              try
              {
                int num3 = list[index].Item1;
                int num4 = Math.Min(pageContentAsync.Length - list[index].Item1, list[index].Item2);
                if (num3 >= 0)
                {
                  if (num4 > 0)
                  {
                    if (num3 < pageContentAsync.Length)
                    {
                      if (num3 + num4 <= pageContentAsync.Length)
                        stringBuilder.Append(pageContentAsync.Substring(list[index].Item1, Math.Min(pageContentAsync.Length - list[index].Item1, list[index].Item2)));
                    }
                  }
                }
              }
              catch
              {
              }
            }
            pdfModel.Pages.Add(new CopilotHelper.PdfPageModel()
            {
              Content = stringBuilder.ToString(),
              PageIndex = pageIndex
            });
          }
        }
        item = new KeyValuePair<int, List<(int, int)>>();
      }
    }
    pdfModel.PageCount = doc.Pages.Count;
    pdfModel.FileName = Path.GetFileNameWithoutExtension(this.filePath);
    return pdfModel;
  }

  public async Task ClearAsync()
  {
    CopilotHelper helper = this;
    lock (helper.memoryCache)
      helper.memoryCache.Clear();
    lock (helper.cachedPageContent)
      helper.cachedPageContent.Clear();
    lock (helper.memoryCache2)
      helper.memoryCache2.Clear();
    helper.cachedChatMessage = new List<CopilotHelper.ChatMessage>();
    try
    {
      helper.analyzeTask?.Dispose();
      helper.analyzeTask = new CopilotHelper.AnalyzeTask(helper);
    }
    catch
    {
    }
    try
    {
      await CopilotHelper.Cache.RemoveFromCache(helper.filePath);
    }
    catch
    {
    }
    try
    {
      await CopilotHelper.Cache.ClearChatMessagesInCache(helper.filePath);
    }
    catch
    {
    }
    try
    {
      await CopilotHelper.Cache.SetSummaryToCache(helper.filePath, "");
    }
    catch
    {
    }
  }

  public async Task<string> GetCachedSummaryAsync()
  {
    return await CopilotHelper.Cache.GetSummaryFromCache(this.filePath);
  }

  public async Task<List<CopilotHelper.ChatMessage>> GetCachedMessageListAsync()
  {
    return await CopilotHelper.Cache.GetChatMessagesFromCache(this.filePath);
  }

  public async Task ClearMessageListAsync()
  {
    this.cachedChatMessage = new List<CopilotHelper.ChatMessage>();
    try
    {
      await CopilotHelper.Cache.ClearChatMessagesInCache(this.filePath);
    }
    catch
    {
    }
  }

  private async Task<CopilotHelper.AnalyzeResponseModel> AnalyzeMessageAsync(
    string message,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(message))
      return (CopilotHelper.AnalyzeResponseModel) null;
    message = message.Trim();
    lock (this.memoryCache2)
    {
      CopilotHelper.AnalyzeResponseModel analyzeResponseModel;
      if (this.memoryCache2.TryGetValue(message, out analyzeResponseModel))
        return analyzeResponseModel;
    }
    CopilotHelper.AnalyzeResponseModel analyzeResponseModel1 = await CopilotHelper.InternalCopilotHelper.AnalyzeAsync(message, cancellationToken);
    if (analyzeResponseModel1 == null)
    {
      lock (this.memoryCache2)
        this.memoryCache2[message] = analyzeResponseModel1;
    }
    return analyzeResponseModel1;
  }

  private async Task<CopilotHelper.AnalyzeResponseModel> AnalyzeAsync(int pageIndex)
  {
    PdfDocument document = this.Document;
    if (document == null || pageIndex < 0 || pageIndex >= this.pageCountForAnalyze || pageIndex >= document.Pages.Count)
      return (CopilotHelper.AnalyzeResponseModel) null;
    lock (this.memoryCache)
    {
      CopilotHelper.AnalyzeResponseModel analyzeResponseModel;
      if (this.memoryCache.TryGetValue(pageIndex, out analyzeResponseModel))
        return analyzeResponseModel;
    }
    try
    {
      CopilotHelper.AnalyzeResponseModel modelFromCache = await CopilotHelper.Cache.GetModelFromCache(this.filePath, pageIndex);
      if (modelFromCache != null)
      {
        lock (this.memoryCache)
          this.memoryCache[pageIndex] = modelFromCache;
        return modelFromCache;
      }
    }
    catch
    {
    }
    return (CopilotHelper.AnalyzeResponseModel) null;
  }

  private async Task<string> GetPageContentAsync(int pageIndex, CancellationToken cancellationToken)
  {
    string pageContent = this.GetPageContent(pageIndex);
    if (!string.IsNullOrEmpty(pageContent) && pageContent.Length > 200)
      return pageContent;
    if (!this.ocrGaSended)
    {
      lock (this.cachedPageOcrContent)
      {
        if (this.cachedPageOcrContent.Count > 3)
        {
          this.ocrGaSended = true;
          CommomLib.Commom.GAManager.SendEvent("ChatPdf", "Doc_Ocr", "Count", 1L);
        }
      }
    }
    return await this.GetOCRPageContentAsync(pageIndex, cancellationToken);
  }

  private string GetPageContent(int pageIndex)
  {
    PdfDocument document = this.Document;
    if (document == null || pageIndex < 0 || pageIndex >= this.pageCountForAnalyze || pageIndex >= document.Pages.Count)
      return (string) null;
    lock (this.cachedPageContent)
    {
      if (this.cachedPageContent.Count > 0)
      {
        string data;
        if (this.cachedPageContent.TryGetValue(pageIndex, out data))
          return data;
      }
    }
    IntPtr page = IntPtr.Zero;
    IntPtr text_page = IntPtr.Zero;
    string pageContent = (string) null;
    try
    {
      page = Pdfium.FPDF_LoadPage(document.Handle, pageIndex);
      if (page != IntPtr.Zero)
      {
        text_page = Pdfium.FPDFText_LoadPage(page);
        if (text_page != IntPtr.Zero)
        {
          int count = Pdfium.FPDFText_CountChars(text_page);
          pageContent = Pdfium.FPDFText_GetText(text_page, 0, count);
        }
      }
    }
    catch
    {
    }
    finally
    {
      if (text_page != IntPtr.Zero)
        Pdfium.FPDFText_ClosePage(text_page);
      if (page != IntPtr.Zero)
        Pdfium.FPDF_ClosePage(page);
    }
    if (pageContent != null)
    {
      lock (this.cachedPageContent)
        this.cachedPageContent[pageIndex] = pageContent;
    }
    return pageContent;
  }

  private async Task<string> GetOCRPageContentAsync(
    int pageIndex,
    CancellationToken cancellationToken)
  {
    PdfDocument doc = this.Document;
    if (doc == null || pageIndex < 0 || pageIndex >= this.pageCountForAnalyze || pageIndex >= doc.Pages.Count)
      return (string) null;
    lock (this.cachedPageOcrContent)
    {
      if (this.cachedPageOcrContent.Count > 0)
      {
        string str;
        if (this.cachedPageOcrContent.TryGetValue(pageIndex, out str))
          return str ?? string.Empty;
      }
    }
    IntPtr page = IntPtr.Zero;
    IntPtr textPage = IntPtr.Zero;
    try
    {
      page = Pdfium.FPDF_LoadPage(doc.Handle, pageIndex);
      if (page != IntPtr.Zero)
      {
        double width;
        double height;
        Pdfium.FPDF_GetPageSizeByIndex(doc.Handle, pageIndex, out width, out height);
        double num1 = Math.Min(width, height);
        using (PdfPage pageObj = PdfPage.FromHandle(doc, page, pageIndex))
        {
          bool flag = false;
          PdfPageObject[] array = pageObj.PageObjects.SelectMany<PdfPageObject, PdfPageObject>((Func<PdfPageObject, IEnumerable<PdfPageObject>>) (c => FlattenCore(c))).ToArray<PdfPageObject>();
          foreach (PdfPageObject pdfPageObject in array.OfType<PdfImageObject>())
          {
            FS_RECTF boundingBox = pdfPageObject.BoundingBox;
            if ((double) boundingBox.Width > num1 / 4.0 || (double) boundingBox.Height > num1 / 4.0)
            {
              flag = true;
              break;
            }
          }
          if (!flag && array.OfType<PdfPathObject>().Count<PdfPathObject>() >= 20)
            flag = true;
          if (flag)
          {
            double num2 = 5.0 / 3.0;
            using (PdfBitmap pdfBitmap = new PdfBitmap((int) (width * num2), (int) (height * num2), true))
            {
              Pdfium.FPDFBitmap_FillRect(pdfBitmap.Handle, 0, 0, pdfBitmap.Width, pdfBitmap.Height, -1);
              pageObj.RenderEx(pdfBitmap, 0, 0, pdfBitmap.Width, pdfBitmap.Height, PageRotate.Normal, RenderFlags.FPDF_NONE);
              string str1;
              if (this.ocrCultureInfo == null)
              {
                (string str2, CultureInfo cultureInfo) = await OcrUtils.GetStringAndCultureAsync(pdfBitmap.Image, cancellationToken);
                str1 = str2;
                if (!string.IsNullOrEmpty(str2) && !string.IsNullOrEmpty(cultureInfo?.Name) && this.ocrCultureInfo == null)
                {
                  lock (this.ocrCultureInfoCount)
                  {
                    int num3;
                    int num4 = !this.ocrCultureInfoCount.TryGetValue(cultureInfo.Name, out num3) ? 1 : num3 + 1;
                    this.ocrCultureInfoCount[cultureInfo.Name] = num4;
                    string key = this.ocrCultureInfoCount.FirstOrDefault<KeyValuePair<string, int>>((Func<KeyValuePair<string, int>, bool>) (c => c.Value > 8)).Key;
                    if (!string.IsNullOrEmpty(key))
                    {
                      try
                      {
                        this.ocrCultureInfo = CultureInfo.GetCultureInfo(key);
                      }
                      catch
                      {
                      }
                    }
                  }
                }
              }
              else
                str1 = await OcrUtils.GetStringAsync(pdfBitmap.Image, this.ocrCultureInfo).WaitAsync<string>(this.cts.Token);
              if (!doc.IsDisposed)
              {
                if (!string.IsNullOrEmpty(str1))
                {
                  textPage = Pdfium.FPDFText_LoadPage(page);
                  string str3;
                  if (textPage != IntPtr.Zero)
                  {
                    string text = Pdfium.FPDFText_GetText(textPage, 0, Pdfium.FPDFText_CountChars(textPage));
                    str3 = text == null || text.Length <= str1.Length / 4 * 3 ? str1 : text;
                  }
                  else
                    str3 = str1;
                  lock (this.cachedPageOcrContent)
                    this.cachedPageOcrContent[pageIndex] = str3;
                  return str3 ?? string.Empty;
                }
              }
            }
          }
        }
      }
    }
    catch
    {
    }
    finally
    {
      if (!doc.IsDisposed && textPage != IntPtr.Zero)
        Pdfium.FPDFText_ClosePage(textPage);
      if (!doc.IsDisposed && page != IntPtr.Zero)
        Pdfium.FPDF_ClosePage(page);
    }
    return string.Empty;

    static IEnumerable<PdfPageObject> FlattenCore(PdfPageObject _pageObject)
    {
      if (_pageObject is PdfFormObject pdfFormObject)
        return pdfFormObject.PageObjects.SelectMany<PdfPageObject, PdfPageObject>((Func<PdfPageObject, IEnumerable<PdfPageObject>>) (_c => FlattenCore(_c)));
      return _pageObject != null ? Enumerable.Repeat<PdfPageObject>(_pageObject, 1) : Enumerable.Empty<PdfPageObject>();
    }
  }

  private async Task<bool> CanChatAsync()
  {
    return await CopilotHelper.Cache.GetChatCountAsync(this.filePath) < 50;
  }

  public async Task<int> GetChatRemaining()
  {
    return Math.Max(0, 50 - await CopilotHelper.Cache.GetChatCountAsync(this.filePath));
  }

  public void ShowFeedbackWindow(bool fromDislike)
  {
    string documentPath = Ioc.Default.GetRequiredService<MainViewModel>().DocumentWrapper.DocumentPath;
    FeedbackWindow feedbackWindow = new FeedbackWindow();
    feedbackWindow.Owner = App.Current.MainWindow;
    feedbackWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
    feedbackWindow.source = "ChatPdf";
    feedbackWindow.HideFaq();
    if (fromDislike)
      feedbackWindow.SetChatDislike();
    if (!string.IsNullOrEmpty(documentPath))
    {
      feedbackWindow.flist.Add(documentPath);
      feedbackWindow.showAttachmentCB(true);
    }
    feedbackWindow.ShowDialog();
  }

  public async Task<CopilotHelper.CopilotResult> SummarizeAsync(
    string input,
    Func<string, CancellationToken, Task> action,
    CancellationToken cancellationToken)
  {
    return await this.DoAppActionAsync("Summarize", new CopilotHelper.DoActionRequestModel()
    {
      Input = input,
      Stream = true
    }, action, cancellationToken);
  }

  public async Task<CopilotHelper.CopilotResult> TranslateAsync(
    string input,
    string language,
    Func<string, CancellationToken, Task> action,
    CancellationToken cancellationToken)
  {
    return await this.DoAppActionAsync("Translate", new CopilotHelper.DoActionRequestModel()
    {
      Input = input,
      Stream = true,
      Language = language
    }, action, cancellationToken);
  }

  public async Task<CopilotHelper.CopilotResult> RewriteAsync(
    string input,
    string style,
    Func<string, CancellationToken, Task> action,
    CancellationToken cancellationToken)
  {
    return await this.DoAppActionAsync("Rewrite", new CopilotHelper.DoActionRequestModel()
    {
      Input = input,
      Stream = true,
      Style = style
    }, action, cancellationToken);
  }

  private async Task<CopilotHelper.CopilotResult> DoAppActionAsync(
    string actionName,
    CopilotHelper.DoActionRequestModel model,
    Func<string, CancellationToken, Task> action,
    CancellationToken cancellationToken)
  {
    CancellationTokenSource linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(this.cts.Token, cancellationToken);
    StringBuilder sb = new StringBuilder();
    CopilotHelper.StreamRequestResult streamRequestResult = await CopilotHelper.InternalCopilotHelper.DoAppActionAsync("Summarize", model, (Func<string, CancellationToken, Task>) (async (s, ct) =>
    {
      sb.Append(s);
      await action(s, ct);
    }), linkedTokenSource.Token);
    return streamRequestResult != null ? new CopilotHelper.CopilotResult((int[]) null, sb.ToString(), streamRequestResult.Error) : CopilotHelper.CopilotResult.EmptyUnknownFailed;
  }

  public async Task<bool> ProcessNativeAppAction(CopilotHelper.AppActionModel appAction)
  {
    if (appAction == null)
      return false;
    await CopilotHelper.AppActionHelper.ProcessAction(appAction);
    return true;
  }

  protected virtual void Dispose(bool disposing)
  {
    if (this.disposedValue)
      return;
    if (disposing)
    {
      this.analyzeTask?.Dispose();
      this.analyzeTask = (CopilotHelper.AnalyzeTask) null;
    }
    this.cts?.Cancel();
    this.disposedValue = true;
  }

  ~CopilotHelper() => this.Dispose(false);

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }

  private class AnalyzeTask : IDisposable
  {
    private const int StartPageCount = 10;
    private const int PageCountPreStage = 10;
    private readonly CopilotHelper helper;
    private bool disposedValue;
    private CancellationTokenSource cts;
    private int curToken;
    private int curPageIdx;
    private Task<int> currentTask;

    public int ProcessedPageIndex => this.curPageIdx;

    public AnalyzeTask(CopilotHelper helper)
    {
      this.cts = new CancellationTokenSource();
      this.helper = helper;
    }

    public async Task Start(IProgress<double> progressReporter)
    {
      this.currentTask = this.RunCore(10, progressReporter);
      this.curToken = await this.currentTask;
      this.Continue();
    }

    public async Task Continue()
    {
      if (this.currentTask != null && !this.currentTask.IsCompleted || this.cts.IsCancellationRequested || this.curPageIdx >= this.helper.pageCountForAnalyze)
        return;
      int chatCountAsync = await CopilotHelper.Cache.GetChatCountAsync(this.helper.filePath);
      int num1 = chatCountAsync / 3 + 1;
      if (chatCountAsync > 8)
        num1 = 999;
      int num2 = num1 * 10 + 10;
      if (num2 > this.helper.pageCountForAnalyze)
        num2 = this.helper.pageCountForAnalyze;
      Log.WriteLog($"AnalyzeTask.Continue() pageCount: {num2}, stage: {num1}");
      if (num2 <= this.curPageIdx)
        return;
      this.currentTask = this.RunCore(10, (IProgress<double>) null);
      this.curToken += await this.currentTask;
      if (this.cts.IsCancellationRequested)
        return;
      this.Continue();
    }

    private async Task<int> RunCore(int pageCount, IProgress<double> progressReporter)
    {
      int processPageCount = Math.Min(pageCount, this.helper.pageCountForAnalyze - this.curPageIdx);
      if (processPageCount <= 0)
        return 0;
      double progress = 0.0;
      (int, CopilotHelper.AnalyzeResponseModel)[] source = await Task.WhenAll<(int, CopilotHelper.AnalyzeResponseModel)>(Enumerable.Range(this.curPageIdx, processPageCount).Select<int, Task<(int, CopilotHelper.AnalyzeResponseModel)>>((Func<int, Task<(int, CopilotHelper.AnalyzeResponseModel)>>) (async c =>
      {
        (int, CopilotHelper.AnalyzeResponseModel) valueTuple = await this.AnalyzeAsync(c);
        if (progressReporter != null)
        {
          lock (progressReporter)
          {
            progress += 1.0 / (double) processPageCount;
            progressReporter.Report(progress);
          }
        }
        return valueTuple;
      })).ToArray<Task<(int, CopilotHelper.AnalyzeResponseModel)>>());
      this.curPageIdx += processPageCount;
      progressReporter?.Report(1.0);
      return ((IEnumerable<(int, CopilotHelper.AnalyzeResponseModel)>) source).SelectMany<(int, CopilotHelper.AnalyzeResponseModel), CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel>((Func<(int, CopilotHelper.AnalyzeResponseModel), IEnumerable<CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel>>) (c => (IEnumerable<CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel>) c.model?.Items ?? Enumerable.Empty<CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel>())).Sum<CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel>((Func<CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel, int>) (c => c == null ? 0 : c.Usage));
    }

    private async Task Run()
    {
      CopilotHelper.AnalyzeTask analyzeTask = this;
      if (analyzeTask.cts.IsCancellationRequested)
        return;
      int tmp = analyzeTask.curToken;
      do
      {
        // ISSUE: reference to a compiler-generated method
        tmp += await Task.Run<int>(new Func<Task<int>>(analyzeTask.\u003CRun\u003Eb__14_0));
      }
      while (!analyzeTask.cts.IsCancellationRequested && analyzeTask.curPageIdx < analyzeTask.helper.pageCountForAnalyze);
      analyzeTask.curToken = tmp;
    }

    private async Task<(int pageIndex, CopilotHelper.AnalyzeResponseModel model)> AnalyzeAsync(
      int pageIndex)
    {
      if (!this.cts.IsCancellationRequested && pageIndex >= 0 && pageIndex < this.helper.pageCountForAnalyze)
      {
        CopilotHelper.AnalyzeResponseModel model = await CopilotHelper.Cache.GetModelFromCache(this.helper.filePath, pageIndex);
        if (!this.cts.IsCancellationRequested && model == null)
        {
          string pageContentAsync = await this.helper.GetPageContentAsync(pageIndex, this.cts.Token);
          if (!string.IsNullOrEmpty(pageContentAsync))
          {
            try
            {
              model = await CopilotHelper.InternalCopilotHelper.AnalyzeAsync(pageContentAsync, this.cts.Token);
              await CopilotHelper.Cache.SetModelToCache(this.helper.filePath, pageIndex, model);
              return (pageIndex, model);
            }
            catch (Exception ex)
            {
            }
          }
        }
        model = (CopilotHelper.AnalyzeResponseModel) null;
      }
      return (pageIndex, (CopilotHelper.AnalyzeResponseModel) null);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposedValue)
        return;
      if (disposing)
        this.cts.Cancel();
      this.disposedValue = true;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }
  }

  internal static class Cache
  {
    private const string CacheKey = "CopilotAnalyzeCache";
    private const string ChatMessagesCacheTemplate = "CopilotChatMessage_";
    private const string SummaryCacheTemplate = "CopilotSummaryCache_";
    private const string EditCountTemplate = "CopilotEditCount_";
    private static SemaphoreSlim locker = new SemaphoreSlim(1, 1);

    public static async Task<List<CopilotHelper.ChatMessage>> GetChatMessagesFromCache(
      string fileName)
    {
      string idAsync = await CopilotHelper.Cache.GetIdAsync(fileName);
      if (string.IsNullOrEmpty(idAsync))
        return new List<CopilotHelper.ChatMessage>();
      List<CopilotHelper.Cache.ChatMessageCacheModel> messagesFromCacheCore = await CopilotHelper.Cache.GetChatMessagesFromCacheCore(idAsync);
      return messagesFromCacheCore == null ? (List<CopilotHelper.ChatMessage>) null : messagesFromCacheCore.Select<CopilotHelper.Cache.ChatMessageCacheModel, CopilotHelper.ChatMessage>((Func<CopilotHelper.Cache.ChatMessageCacheModel, CopilotHelper.ChatMessage>) (c => c.ToChatMessage())).ToList<CopilotHelper.ChatMessage>();
    }

    public static async Task ClearChatMessagesInCache(string fileName)
    {
      string idAsync = await CopilotHelper.Cache.GetIdAsync(fileName);
      if (string.IsNullOrEmpty(idAsync))
        return;
      await CopilotHelper.Cache.SetChatMessagesToCacheCore(idAsync, (List<CopilotHelper.Cache.ChatMessageCacheModel>) null);
    }

    public static async Task<bool> AppendChatMessagesToCache(
      string fileName,
      params CopilotHelper.ChatMessage[] messages)
    {
      if (messages == null || messages.Length == 0)
        return false;
      CopilotHelper.Cache.ChatMessageCacheModel[] messages2 = ((IEnumerable<CopilotHelper.ChatMessage>) messages).Where<CopilotHelper.ChatMessage>((Func<CopilotHelper.ChatMessage, bool>) (c =>
      {
        if (string.IsNullOrEmpty(c.Content))
          return false;
        return c.Role == "assistant" || c.Role == "user";
      })).Select<CopilotHelper.ChatMessage, CopilotHelper.Cache.ChatMessageCacheModel>((Func<CopilotHelper.ChatMessage, CopilotHelper.Cache.ChatMessageCacheModel>) (c => new CopilotHelper.Cache.ChatMessageCacheModel(c))).ToArray<CopilotHelper.Cache.ChatMessageCacheModel>();
      if (messages2.Length == 0)
        return false;
      string id = await CopilotHelper.Cache.GetIdAsync(fileName);
      if (string.IsNullOrEmpty(id))
        return false;
      List<CopilotHelper.Cache.ChatMessageCacheModel> messagesFromCacheCore = await CopilotHelper.Cache.GetChatMessagesFromCacheCore(id);
      messagesFromCacheCore.AddRange((IEnumerable<CopilotHelper.Cache.ChatMessageCacheModel>) messages2);
      await CopilotHelper.Cache.SetChatMessagesToCacheCore(id, messagesFromCacheCore);
      return true;
    }

    public static async Task<bool> AppendChatMessagesLikedToCache(
      string fileName,
      CopilotHelper.ChatMessage messages)
    {
      if (messages == null)
        return false;
      string id = await CopilotHelper.Cache.GetIdAsync(fileName);
      if (string.IsNullOrEmpty(id))
        return false;
      List<CopilotHelper.Cache.ChatMessageCacheModel> messagesFromCacheCore = await CopilotHelper.Cache.GetChatMessagesFromCacheCore(id);
      foreach (CopilotHelper.Cache.ChatMessageCacheModel messageCacheModel in messagesFromCacheCore)
      {
        if (messageCacheModel.Content == messages.Content && messageCacheModel.Role == messages.Role)
          messageCacheModel.Liked = messages.Liked;
      }
      await CopilotHelper.Cache.SetChatMessagesToCacheCore(id, messagesFromCacheCore);
      return true;
    }

    public static async Task<string> GetSummaryFromCache(string fileName)
    {
      string idAsync = await CopilotHelper.Cache.GetIdAsync(fileName);
      return string.IsNullOrEmpty(idAsync) ? "" : await CopilotHelper.Cache.GetSummaryFromCacheCore(idAsync);
    }

    public static async Task SetSummaryToCache(string fileName, string summary)
    {
      string idAsync = await CopilotHelper.Cache.GetIdAsync(fileName);
      if (string.IsNullOrEmpty(idAsync))
        return;
      await CopilotHelper.Cache.SetSummaryToCacheCore(idAsync, summary);
    }

    public static async Task<int> GetChatCountAsync(string fileName)
    {
      string idAsync = await CopilotHelper.Cache.GetIdAsync(fileName);
      return string.IsNullOrEmpty(idAsync) ? 0 : await CopilotHelper.Cache.GetChatCountCore(idAsync);
    }

    public static async Task<int> IncreaseChatCountAsync(string fileName)
    {
      string id = await CopilotHelper.Cache.GetIdAsync(fileName);
      if (string.IsNullOrEmpty(id))
        return 0;
      int count = await CopilotHelper.Cache.GetChatCountCore(id);
      ++count;
      await CopilotHelper.Cache.SetChatCountCore(id, count);
      return count;
    }

    public static async Task<CopilotHelper.AnalyzeResponseModel> GetModelFromCache(
      string fileName,
      int pageIndex)
    {
      string idAsync = await CopilotHelper.Cache.GetIdAsync(fileName);
      return string.IsNullOrEmpty(idAsync) ? (CopilotHelper.AnalyzeResponseModel) null : await CopilotHelper.Cache.GetModelFromCacheCoreAsync(idAsync, pageIndex);
    }

    public static async Task SetModelToCache(
      string fileName,
      int pageIndex,
      CopilotHelper.AnalyzeResponseModel model)
    {
      string id = await CopilotHelper.Cache.GetIdAsync(fileName);
      if (string.IsNullOrEmpty(id))
      {
        id = Guid.NewGuid().ToString("N");
        await CopilotHelper.Cache.SetIdAsync(fileName, id);
      }
      await CopilotHelper.Cache.SetModelToCacheCoreAsync(id, pageIndex, model);
      id = (string) null;
    }

    public static async Task RemoveFromCache(string fileName)
    {
      string id = await CopilotHelper.Cache.GetIdAsync(fileName);
      if (string.IsNullOrEmpty(id))
      {
        id = (string) null;
      }
      else
      {
        await CopilotHelper.Cache.SetIdAsync(fileName, "");
        string path = Path.Combine(CopilotHelper.Cache.GetCacheFolder(), id);
        if (!Directory.Exists(path))
        {
          id = (string) null;
        }
        else
        {
          try
          {
            Directory.Delete(path, true);
            id = (string) null;
          }
          catch
          {
            id = (string) null;
          }
        }
      }
    }

    private static async Task<CopilotHelper.AnalyzeResponseModel> GetModelFromCacheCoreAsync(
      string id,
      int pageIndex)
    {
      if (string.IsNullOrEmpty(id))
        return (CopilotHelper.AnalyzeResponseModel) null;
      string str = Path.Combine(CopilotHelper.Cache.GetCacheFolder(), id);
      if (Directory.Exists(str))
      {
        string path = Path.Combine(str, $"{pageIndex}");
        if (File.Exists(path))
        {
          try
          {
            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
              using (StreamReader reader = new StreamReader((Stream) fileStream, Encoding.UTF8, true, 1024 /*0x0400*/, true))
                return JsonConvert.DeserializeObject<CopilotHelper.AnalyzeResponseModel>(await reader.ReadToEndAsync());
            }
          }
          catch
          {
          }
        }
      }
      return (CopilotHelper.AnalyzeResponseModel) null;
    }

    private static async Task SetModelToCacheCoreAsync(
      string id,
      int pageIndex,
      CopilotHelper.AnalyzeResponseModel model)
    {
      if (string.IsNullOrEmpty(id) || model == null)
        return;
      string str1 = Path.Combine(CopilotHelper.Cache.GetCacheFolder(), id);
      if (!Directory.Exists(str1))
        Directory.CreateDirectory(str1);
      string path = Path.Combine(str1, $"{pageIndex}");
      try
      {
        string str2 = JsonConvert.SerializeObject((object) model, Formatting.Indented);
        if (string.IsNullOrEmpty(str2))
          return;
        using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
        {
          using (StreamWriter writer = new StreamWriter((Stream) fileStream, Encoding.UTF8, 1024 /*0x0400*/, true))
          {
            if (model != null)
              await writer.WriteAsync(str2);
          }
          fileStream.SetLength(fileStream.Position);
        }
      }
      catch
      {
      }
    }

    private static async Task<List<CopilotHelper.Cache.ChatMessageCacheModel>> GetChatMessagesFromCacheCore(
      string id)
    {
      if (!string.IsNullOrEmpty(id))
      {
        (bool flag, List<CopilotHelper.Cache.ChatMessageCacheModel> messagesFromCacheCore) = await ConfigUtils.TryGetAsync<List<CopilotHelper.Cache.ChatMessageCacheModel>>("CopilotChatMessage_" + id, new CancellationToken());
        if (flag && messagesFromCacheCore != null)
          return messagesFromCacheCore;
      }
      return new List<CopilotHelper.Cache.ChatMessageCacheModel>();
    }

    private static async Task SetChatMessagesToCacheCore(
      string id,
      List<CopilotHelper.Cache.ChatMessageCacheModel> messages)
    {
      if (string.IsNullOrEmpty(id))
        return;
      int num = await ConfigUtils.TrySetAsync<List<CopilotHelper.Cache.ChatMessageCacheModel>>("CopilotChatMessage_" + id, messages, new CancellationToken()) ? 1 : 0;
    }

    private static async Task<string> GetSummaryFromCacheCore(string id)
    {
      if (!string.IsNullOrEmpty(id))
      {
        (bool flag, string str) = await SqliteUtils.TryGetAsync("CopilotSummaryCache_" + id, new CancellationToken());
        if (flag)
          return str ?? "";
      }
      return "";
    }

    private static async Task SetSummaryToCacheCore(string id, string summary)
    {
      if (string.IsNullOrEmpty(id))
        return;
      int num = await SqliteUtils.TrySetAsync("CopilotSummaryCache_" + id, summary, new CancellationToken()) ? 1 : 0;
    }

    private static async Task<int> GetChatCountCore(string id)
    {
      if (!string.IsNullOrEmpty(id))
      {
        (bool flag, string str) = await SqliteUtils.TryGetAsync("CopilotEditCount_" + id, new CancellationToken());
        if (flag && !string.IsNullOrEmpty(str))
        {
          string[] strArray = str.Split('|');
          long result1;
          int result2;
          if (strArray.Length == 2 && long.TryParse(strArray[0], out result1) && int.TryParse(strArray[1], out result2))
          {
            DateTimeOffset now = DateTimeOffset.Now;
            DateTimeOffset dateTimeOffset = new DateTimeOffset(DateTimeOffset.FromUnixTimeSeconds(result1).UtcTicks, now.Offset);
            return !(now.Date == dateTimeOffset.Date) ? 0 : result2;
          }
        }
      }
      return 0;
    }

    private static async Task SetChatCountCore(string id, int count)
    {
      if (string.IsNullOrEmpty(id))
        return;
      int num = await SqliteUtils.TrySetAsync("CopilotEditCount_" + id, $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}|{count}", new CancellationToken()) ? 1 : 0;
    }

    private static string GetCacheFolder()
    {
      string path = Path.Combine(UtilManager.GetLocalCachePath(), "Copilot");
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      return path;
    }

    private static async Task<string> GetIdAsync(string fileName)
    {
      fileName = fileName?.Trim()?.ToLowerInvariant();
      if (string.IsNullOrWhiteSpace(fileName))
        return (string) null;
      try
      {
        await CopilotHelper.Cache.locker.WaitAsync();
        (bool flag, List<CopilotHelper.Cache.CacheModel> source) = await ConfigUtils.TryGetAsync<List<CopilotHelper.Cache.CacheModel>>("CopilotAnalyzeCache", new CancellationToken());
        if (flag)
        {
          if (source != null)
            return source.LastOrDefault<CopilotHelper.Cache.CacheModel>((Func<CopilotHelper.Cache.CacheModel, bool>) (c => c.FileName == fileName))?.Id;
        }
      }
      finally
      {
        CopilotHelper.Cache.locker.Release();
      }
      return (string) null;
    }

    private static async Task SetIdAsync(string fileName, string id)
    {
      fileName = fileName?.Trim()?.ToLowerInvariant();
      if (string.IsNullOrWhiteSpace(fileName))
        return;
      try
      {
        await CopilotHelper.Cache.locker.WaitAsync();
        (bool flag1, List<CopilotHelper.Cache.CacheModel> cacheModelList) = await ConfigUtils.TryGetAsync<List<CopilotHelper.Cache.CacheModel>>("CopilotAnalyzeCache", new CancellationToken());
        if (flag1 && cacheModelList != null)
        {
          if (string.IsNullOrEmpty(id))
          {
            for (int index = cacheModelList.Count - 1; index >= 0; --index)
            {
              if (cacheModelList[index].Id == id)
              {
                cacheModelList.RemoveAt(index);
                break;
              }
            }
          }
          else
          {
            bool flag2 = false;
            for (int index = 0; index < cacheModelList.Count; ++index)
            {
              if (cacheModelList[index].FileName == fileName)
              {
                flag2 = true;
                cacheModelList[index].Id = id;
              }
            }
            if (!flag2)
              cacheModelList.Add(new CopilotHelper.Cache.CacheModel()
              {
                FileName = fileName,
                Id = id
              });
          }
          int num = await ConfigUtils.TrySetAsync<List<CopilotHelper.Cache.CacheModel>>("CopilotAnalyzeCache", cacheModelList, new CancellationToken()) ? 1 : 0;
        }
        else
        {
          if (string.IsNullOrEmpty(id))
            return;
          int num = await ConfigUtils.TrySetAsync<List<CopilotHelper.Cache.CacheModel>>("CopilotAnalyzeCache", new List<CopilotHelper.Cache.CacheModel>()
          {
            new CopilotHelper.Cache.CacheModel()
            {
              FileName = fileName,
              Id = id
            }
          }, new CancellationToken()) ? 1 : 0;
        }
      }
      finally
      {
        CopilotHelper.Cache.locker.Release();
      }
    }

    private class CacheModel
    {
      public string FileName { get; set; }

      public string Id { get; set; }
    }

    private class ChatMessageCacheModel
    {
      public ChatMessageCacheModel()
      {
      }

      public ChatMessageCacheModel(CopilotHelper.ChatMessage message)
      {
        this.Role = message.Role;
        this.Content = message.Content;
        this.Pages = message.Pages;
        this.Liked = message.Liked;
      }

      [JsonProperty("role")]
      public string Role { get; set; }

      [JsonProperty("content")]
      public string Content { get; set; }

      [JsonProperty("pages")]
      public int[] Pages { get; set; }

      [JsonProperty("like")]
      public string Liked { get; set; }

      public CopilotHelper.ChatMessage ToChatMessage()
      {
        return new CopilotHelper.ChatMessage()
        {
          Role = this.Role,
          Content = this.Content,
          Pages = this.Pages,
          Liked = this.Liked
        };
      }
    }
  }

  private class InternalCopilotHelper
  {
    private const string baseUri = "https://chatapi.pdfgear.com";
    private static HttpClient httpClient;

    private static HttpClient HttpClient
    {
      get
      {
        if (CopilotHelper.InternalCopilotHelper.httpClient == null)
          CopilotHelper.InternalCopilotHelper.httpClient = new HttpClient()
          {
            Timeout = TimeSpan.FromSeconds(120.0),
            BaseAddress = new Uri("https://chatapi.pdfgear.com")
          };
        return CopilotHelper.InternalCopilotHelper.httpClient;
      }
    }

    public static async Task<CopilotHelper.AnalyzeResponseModel> AnalyzeAsync(
      string text,
      CancellationToken cancellationToken)
    {
      HttpResponseMessage httpResponseMessage = await CopilotHelper.InternalCopilotHelper.HttpClient.PostAsync("/pdf/analyze", CopilotHelper.InternalCopilotHelper.BuildJsonContent<CopilotHelper.AnalyzeRequestModel>(new CopilotHelper.AnalyzeRequestModel()
      {
        Text = text,
        User = UtilManager.GetUUID()
      }), cancellationToken);
      httpResponseMessage.EnsureSuccessStatusCode();
      return JsonConvert.DeserializeObject<CopilotHelper.ResponseResult<CopilotHelper.AnalyzeResponseModel>>(await httpResponseMessage.Content.ReadAsStringAsync()).Content;
    }

    public static async Task<CopilotHelper.StreamRequestResult> GetSummaryAsync(
      CopilotHelper.PdfModel pdf,
      bool stream,
      Func<string, CancellationToken, Task> action,
      CancellationToken cancellationToken)
    {
      CopilotHelper.ChatRequestModel model = new CopilotHelper.ChatRequestModel()
      {
        Pdf = pdf,
        Stream = stream,
        User = UtilManager.GetUUID(),
        Language = CopilotHelper.InternalCopilotHelper.GetLanguage()
      };
      return await CopilotHelper.InternalCopilotHelper.ProcessStreamResponse(await CopilotHelper.InternalCopilotHelper.HttpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, "/pdf/getsummary")
      {
        Content = CopilotHelper.InternalCopilotHelper.BuildJsonContent<CopilotHelper.ChatRequestModel>(model)
      }, HttpCompletionOption.ResponseHeadersRead, cancellationToken), action, cancellationToken).ConfigureAwait(false);
    }

    public static async Task<CopilotHelper.StreamRequestResult> ChatAsync(
      List<CopilotHelper.ChatMessage> messages,
      CopilotHelper.PdfModel pdf,
      bool stream,
      Func<string, CancellationToken, Task> action,
      CancellationToken cancellationToken)
    {
      CopilotHelper.ChatRequestModel model = new CopilotHelper.ChatRequestModel()
      {
        Messages = messages.ToArray(),
        Pdf = pdf,
        Stream = stream,
        User = UtilManager.GetUUID(),
        Language = CopilotHelper.InternalCopilotHelper.GetLanguage()
      };
      return await CopilotHelper.InternalCopilotHelper.ProcessStreamResponse(await CopilotHelper.InternalCopilotHelper.HttpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, "/pdf/chat")
      {
        Content = CopilotHelper.InternalCopilotHelper.BuildJsonContent<CopilotHelper.ChatRequestModel>(model)
      }, HttpCompletionOption.ResponseHeadersRead, cancellationToken), action, cancellationToken).ConfigureAwait(false);
    }

    public static async Task<CopilotHelper.AppActionModel> GetActionAsync(
      string input,
      CopilotHelper.ActionModel[] actionList,
      CancellationToken cancellationToken)
    {
      if (string.IsNullOrEmpty(input) || actionList == null || actionList.Length == 0)
        return (CopilotHelper.AppActionModel) null;
      CopilotHelper.GetActionRequestModel model = new CopilotHelper.GetActionRequestModel()
      {
        Input = input,
        Actions = ((IEnumerable<CopilotHelper.ActionModel>) actionList).Select<CopilotHelper.ActionModel, CopilotHelper.ActionModel>((Func<CopilotHelper.ActionModel, CopilotHelper.ActionModel>) (c => new CopilotHelper.ActionModel()
        {
          Description = c.Description,
          Examples = c.Examples,
          Name = c.Name,
          Parameters = c.Parameters
        })).ToArray<CopilotHelper.ActionModel>(),
        User = UtilManager.GetUUID()
      };
      HttpResponseMessage httpResponseMessage = await CopilotHelper.InternalCopilotHelper.HttpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, "/appaction/getaction")
      {
        Content = CopilotHelper.InternalCopilotHelper.BuildJsonContent<CopilotHelper.GetActionRequestModel>(model)
      }, HttpCompletionOption.ResponseContentRead, cancellationToken);
      httpResponseMessage.EnsureSuccessStatusCode();
      string str = await httpResponseMessage.Content.ReadAsStringAsync();
      try
      {
        CopilotHelper.ResponseResult<CopilotHelper.AppActionModel> responseResult = JsonConvert.DeserializeObject<CopilotHelper.ResponseResult<CopilotHelper.AppActionModel>>(str);
        if (responseResult != null)
        {
          if (responseResult.Success)
            return responseResult.Content;
        }
      }
      catch (Exception ex)
      {
        Log.WriteLog($"Json: {str},\nException: {ex}");
      }
      return (CopilotHelper.AppActionModel) null;
    }

    public static async Task<CopilotHelper.StreamRequestResult> DoAppActionAsync(
      string actionName,
      CopilotHelper.DoActionRequestModel model,
      Func<string, CancellationToken, Task> action,
      CancellationToken cancellationToken)
    {
      if (string.IsNullOrEmpty(actionName) || model == null)
        return CopilotHelper.StreamRequestResult.CreateFailed();
      model.User = UtilManager.GetUUID();
      return await CopilotHelper.InternalCopilotHelper.ProcessStreamResponse(await CopilotHelper.InternalCopilotHelper.HttpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, "/appaction/" + actionName)
      {
        Content = CopilotHelper.InternalCopilotHelper.BuildJsonContent<CopilotHelper.DoActionRequestModel>(model)
      }, HttpCompletionOption.ResponseHeadersRead, cancellationToken), action, cancellationToken).ConfigureAwait(false);
    }

    private static string GetLanguage()
    {
      if (CultureInfo.DefaultThreadCurrentUICulture != null)
        return CultureInfo.DefaultThreadCurrentUICulture.Name;
      return CultureInfo.CurrentUICulture != null ? CultureInfo.CurrentUICulture.Name : "";
    }

    private static async Task<CopilotHelper.StreamRequestResult> ProcessStreamResponse(
      HttpResponseMessage response,
      Func<string, CancellationToken, Task> action,
      CancellationToken cancellationToken)
    {
      response.EnsureSuccessStatusCode();
      int textLen = 0;
      using (Stream responseStream = await response.Content.ReadAsStreamAsync())
      {
        using (StreamReader reader = new StreamReader(responseStream, Encoding.UTF8, true, 1024 /*0x0400*/, true))
        {
          string str = await reader.ReadLineAsync();
          CancellationTokenRegistration cancellationTokenRegistration = cancellationToken.Register((Action) (() =>
          {
            try
            {
              responseStream.Dispose();
            }
            catch
            {
            }
          }));
          try
          {
            for (; str != null; str = await reader.ReadLineAsync())
            {
              CopilotHelper.StreamResult streamResult = (CopilotHelper.StreamResult) null;
              try
              {
                streamResult = JsonConvert.DeserializeObject<CopilotHelper.StreamResult>(str);
              }
              catch
              {
              }
              if (streamResult != null)
              {
                if (!string.IsNullOrEmpty(streamResult.Error))
                {
                  if (!(streamResult.Error == "end"))
                    return CopilotHelper.StreamRequestResult.CreateFailed(streamResult.Error);
                  if (streamResult.Success == 1)
                  {
                    int num = textLen;
                    string text = streamResult.Text;
                    int length = text != null ? text.Length : 0;
                    textLen = num + length;
                    if (!string.IsNullOrEmpty(streamResult.Text))
                      await action(streamResult.Text, cancellationToken);
                  }
                  return textLen != 0 ? CopilotHelper.StreamRequestResult.CreateSuccess() : CopilotHelper.StreamRequestResult.CreateFailed();
                }
                if (streamResult.Success != 1)
                  return !(streamResult.Text == "end") || textLen == 0 ? CopilotHelper.StreamRequestResult.CreateFailed() : CopilotHelper.StreamRequestResult.CreateSuccess();
                int num1 = textLen;
                string text1 = streamResult.Text;
                int length1 = text1 != null ? text1.Length : 0;
                textLen = num1 + length1;
                if (!string.IsNullOrEmpty(streamResult.Text))
                  await action(streamResult.Text, cancellationToken);
              }
              else
                break;
            }
          }
          catch (ObjectDisposedException ex) when (cancellationToken.IsCancellationRequested)
          {
            throw new OperationCanceledException(ex.Message, (Exception) ex, cancellationToken);
          }
          finally
          {
            cancellationTokenRegistration.Dispose();
          }
          cancellationTokenRegistration = new CancellationTokenRegistration();
        }
      }
      return CopilotHelper.StreamRequestResult.CreateFailed();
    }

    private static HttpContent BuildJsonContent<T>(T model)
    {
      return !object.Equals((object) model, (object) null) ? (HttpContent) new StringContent(JsonConvert.SerializeObject((object) model, Formatting.None), Encoding.UTF8, "application/json") : (HttpContent) new StringContent("{}", Encoding.UTF8, "application/json");
    }
  }

  private class SimpleCosineSimilarityFloatVersion
  {
    public static float[][] ComputeDistances(
      float[][] dataSet,
      bool useMultipleThread = false,
      int maxDegreeOfParallelism = 0)
    {
      int numPoints = dataSet.Length;
      float[][] distances = new float[numPoints][];
      for (int index = 0; index < distances.Length; ++index)
        distances[index] = new float[numPoints];
      if (useMultipleThread)
      {
        int toExclusive = numPoints * numPoints;
        if (maxDegreeOfParallelism == 0)
          maxDegreeOfParallelism = Environment.ProcessorCount;
        ParallelOptions parallelOptions = new ParallelOptions()
        {
          MaxDegreeOfParallelism = Math.Max(1, maxDegreeOfParallelism)
        };
        Parallel.For(0, toExclusive, parallelOptions, (Action<int>) (index =>
        {
          int index1 = index % numPoints;
          int index2 = index / numPoints;
          if (index1 >= index2)
            return;
          float distance = CopilotHelper.SimpleCosineSimilarityFloatVersion.ComputeDistance(dataSet[index1], dataSet[index2]);
          distances[index1][index2] = distance;
          distances[index2][index1] = distance;
        }));
      }
      else
      {
        for (int index3 = 0; index3 < numPoints; ++index3)
        {
          for (int index4 = 0; index4 < index3; ++index4)
          {
            float distance = CopilotHelper.SimpleCosineSimilarityFloatVersion.ComputeDistance(dataSet[index3], dataSet[index4]);
            distances[index3][index4] = distance;
            distances[index4][index3] = distance;
          }
        }
      }
      return distances;
    }

    public static float ComputeDistance(float[] attributesOne, float[] attributesTwo)
    {
      double num1 = 0.0;
      double num2 = 0.0;
      double num3 = 0.0;
      int index = 0;
      if (System.Numerics.Vector.IsHardwareAccelerated)
      {
        int count = Vector<float>.Count;
        int num4 = attributesOne.Length / count * count;
        for (index = 0; index < num4; index += count)
        {
          Vector<float> vector1 = new Vector<float>(attributesOne, index);
          Vector<float> vector2 = new Vector<float>(attributesTwo, index);
          num1 += (double) System.Numerics.Vector.Dot<float>(vector1, vector2);
          num2 += (double) System.Numerics.Vector.Dot<float>(vector1, vector1);
          num3 += (double) System.Numerics.Vector.Dot<float>(vector2, vector2);
        }
      }
      for (; index < attributesOne.Length; ++index)
      {
        num1 += (double) attributesOne[index] * (double) attributesTwo[index];
        num2 += (double) attributesOne[index] * (double) attributesOne[index];
        num3 += (double) attributesTwo[index] * (double) attributesTwo[index];
      }
      return (float) Math.Max(0.0, 1.0 - num1 / Math.Sqrt(num2 * num3));
    }
  }

  private class ResponseResult<T>
  {
    public T Content { get; set; }

    public bool Success { get; set; }

    public string Messaage { get; set; } = "";
  }

  public class StreamResult
  {
    [JsonProperty("s")]
    public int Success { get; set; }

    [JsonProperty("t")]
    public string Text { get; set; }

    [JsonProperty("e")]
    public string Error { get; set; }
  }

  public class StreamRequestResult
  {
    private static CopilotHelper.StreamRequestResult successResult = new CopilotHelper.StreamRequestResult((string) null);

    public StreamRequestResult(string error) => this.Error = error;

    public string Error { get; }

    public bool Success => string.IsNullOrEmpty(this.Error);

    public static CopilotHelper.StreamRequestResult CreateSuccess()
    {
      return CopilotHelper.StreamRequestResult.successResult;
    }

    public static CopilotHelper.StreamRequestResult CreateFailed(string error = "unknown")
    {
      return new CopilotHelper.StreamRequestResult(!string.IsNullOrEmpty(error) ? error : "unknown");
    }
  }

  public class AnalyzeRequestModel
  {
    [JsonProperty("user")]
    public string User { get; set; }

    [JsonProperty("text")]
    public string Text { get; set; }
  }

  public class AnalyzeResponseModel
  {
    [JsonProperty("items")]
    public List<CopilotHelper.AnalyzeResponseModel.AnalyzeResponseItemModel> Items { get; set; }

    public class AnalyzeResponseItemModel
    {
      [JsonProperty("usage")]
      public int Usage { get; set; }

      [JsonProperty("textIndex")]
      public int TextIndex { get; set; }

      [JsonProperty("textLength")]
      public int TextLength { get; set; }

      [JsonProperty("data")]
      public List<CopilotHelper.AnalyzeResponseModel.AnalyzeResponseDataModel> Data { get; set; }
    }

    public class AnalyzeResponseDataModel
    {
      [JsonProperty("values")]
      public float[] Values { get; set; }
    }
  }

  public class PdfModel
  {
    [JsonProperty("fileName")]
    public string FileName { get; set; }

    [JsonProperty("pageCount")]
    public int PageCount { get; set; }

    [JsonProperty("pages")]
    public List<CopilotHelper.PdfPageModel> Pages { get; set; }
  }

  public class PdfPageModel
  {
    [JsonProperty("pageIndex")]
    public int PageIndex { get; set; }

    [JsonProperty("content")]
    public string Content { get; set; }
  }

  public class ChatMessage
  {
    [JsonProperty("role")]
    public string Role { get; set; }

    [JsonProperty("content")]
    public string Content { get; set; }

    [JsonIgnore]
    public int[] Pages { get; set; }

    [JsonProperty("like")]
    public string Liked { get; set; }
  }

  public class ChatRequestModel
  {
    [JsonProperty("user")]
    public string User { get; set; }

    [JsonProperty("messages")]
    public CopilotHelper.ChatMessage[] Messages { get; set; }

    [JsonProperty("pdf")]
    public CopilotHelper.PdfModel Pdf { get; set; }

    [JsonProperty("stream")]
    public bool Stream { get; set; }

    [JsonProperty("language")]
    public string Language { get; set; }
  }

  public class CopilotResult
  {
    public CopilotResult(
      int[] pages,
      string text,
      CopilotHelper.AppActionModel appAction,
      CopilotHelper.ChatResultError error,
      bool maybeNotAppAction)
    {
      this.Pages = pages;
      this.Text = text;
      this.AppAction = appAction;
      this.Error = error;
      this.MaybeNotAppAction = maybeNotAppAction;
    }

    public CopilotResult(
      int[] pages,
      string text,
      CopilotHelper.AppActionModel appAction,
      string error,
      bool maybeNotAppAction)
      : this(pages, text, appAction, CopilotHelper.CopilotResult.ErrorToEnum(error), maybeNotAppAction)
    {
    }

    public CopilotResult(int[] pages, string text, CopilotHelper.ChatResultError error)
      : this(pages, text, (CopilotHelper.AppActionModel) null, error, true)
    {
    }

    public CopilotResult(int[] pages, string text, string error)
      : this(pages, text, (CopilotHelper.AppActionModel) null, CopilotHelper.CopilotResult.ErrorToEnum(error), true)
    {
    }

    public int[] Pages { get; }

    public string Text { get; }

    public CopilotHelper.AppActionModel AppAction { get; }

    public bool MaybeNotAppAction { get; }

    public CopilotHelper.ChatResultError Error { get; }

    public static CopilotHelper.CopilotResult EmptyUnknownFailed { get; } = new CopilotHelper.CopilotResult((int[]) null, (string) null, CopilotHelper.ChatResultError.Unknown);

    public static CopilotHelper.CopilotResult ContentEmptyFailed { get; } = new CopilotHelper.CopilotResult((int[]) null, (string) null, CopilotHelper.ChatResultError.ContentEmpty);

    public static CopilotHelper.CopilotResult ChatCountLimitFailed { get; } = new CopilotHelper.CopilotResult((int[]) null, (string) null, CopilotHelper.ChatResultError.CountLimit);

    public static CopilotHelper.CopilotResult UserCanceledFailed { get; } = new CopilotHelper.CopilotResult((int[]) null, (string) null, CopilotHelper.ChatResultError.UserCanceled);

    public static CopilotHelper.CopilotResult EmptyUnknownMaybeAppActionFailed { get; } = new CopilotHelper.CopilotResult((int[]) null, (string) null, (CopilotHelper.AppActionModel) null, CopilotHelper.ChatResultError.Unknown, true);

    public static CopilotHelper.ChatResultError ErrorToEnum(string error)
    {
      error = error?.Trim().ToLowerInvariant();
      switch (error)
      {
        case "":
        case null:
          return CopilotHelper.ChatResultError.None;
        case "content_filtered":
          return CopilotHelper.ChatResultError.ContentFiltered;
        case "too_many_request":
          return CopilotHelper.ChatResultError.TooManyRequest;
        case "empty":
          return CopilotHelper.ChatResultError.ContentEmpty;
        default:
          return CopilotHelper.ChatResultError.Unknown;
      }
    }
  }

  public enum ChatResultError
  {
    None = 0,
    Unknown = 1,
    TooManyRequest = 2,
    AccountBaned = 3,
    ResponseEndedPrematurely = 4,
    ContentFiltered = 5,
    ContentEmpty = 500, // 0x000001F4
    CountLimit = 501, // 0x000001F5
    UserCanceled = 9999, // 0x0000270F
  }

  public class AppActionModel
  {
    private string confirm;

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("params")]
    public Dictionary<string, string> Parameters { get; set; }

    [JsonIgnore]
    public string Confirm
    {
      get
      {
        return this.confirm ?? (this.confirm = CopilotHelper.AppActionHelper.GetAppActionConfirm(this));
      }
    }
  }

  private class ActionModelRoot
  {
    [JsonProperty("actions")]
    public CopilotHelper.ActionModel[] Actions { get; set; }
  }

  private class ActionModel
  {
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("desc")]
    public string Description { get; set; }

    [JsonProperty("embedding")]
    public float[] Embedding { get; set; }

    [JsonProperty("params")]
    public Dictionary<string, string> Parameters { get; set; }

    [JsonProperty("examples")]
    public CopilotHelper.ExampleModel[] Examples { get; set; }

    [JsonProperty("static")]
    public bool IsStatic { get; set; }

    [JsonProperty("confirm")]
    public Dictionary<string, string> Confirm { get; set; }

    [JsonProperty("disabled")]
    public bool Disabled { get; set; }
  }

  private class ExampleModel
  {
    [JsonProperty("q")]
    public string Question { get; set; }

    [JsonProperty("a")]
    public string Answer { get; set; }
  }

  private class GetActionRequestModel
  {
    public string Input { get; set; }

    public CopilotHelper.ActionModel[] Actions { get; set; }

    public string User { get; set; }
  }

  public class DoActionRequestModel
  {
    public string Input { get; set; }

    public string Language { get; set; }

    public string Style { get; set; }

    public bool Stream { get; set; }

    public string User { get; set; }
  }

  public class AppSupportActions
  {
    public const string Summarize = "Summarize";
    public const string Translate = "Translate";
    public const string Rewrite = "Rewrite";
  }

  private class AppActionHelper
  {
    private static CopilotHelper.ActionModel[] actionModels;
    private static AsyncLock asyncLock = new AsyncLock();

    public static async Task<(CopilotHelper.AppActionModel action, bool maybeNotAppAction)> GetAction(
      string input,
      float[] embedding,
      CancellationToken cancellationToken)
    {
      bool appendFindAction = false;
      if (input != null && input.Length < 30)
        appendFindAction = true;
      (CopilotHelper.ActionModel[] actionModelArray, bool flag) = await CopilotHelper.AppActionHelper.GetActionsForInput(embedding, appendFindAction, cancellationToken);
      CopilotHelper.AppActionModel action = await CopilotHelper.InternalCopilotHelper.GetActionAsync(input, actionModelArray, cancellationToken);
      if (action == null)
        return ((CopilotHelper.AppActionModel) null, flag);
      CopilotHelper.ActionModel actionModel = ((IEnumerable<CopilotHelper.ActionModel>) actionModelArray).FirstOrDefault<CopilotHelper.ActionModel>((Func<CopilotHelper.ActionModel, bool>) (c => c.Name == action.Name));
      if (actionModel == null)
        action = (CopilotHelper.AppActionModel) null;
      else if (actionModel.Parameters != null)
      {
        if (action.Parameters != null)
        {
          foreach (string key in action.Parameters.Keys)
          {
            if (!actionModel.Parameters.ContainsKey(key))
              action.Parameters.Remove(key);
          }
          if (action.Parameters.Count != actionModel.Parameters.Count)
          {
            action = (CopilotHelper.AppActionModel) null;
          }
          else
          {
            foreach (KeyValuePair<string, string> parameter in action.Parameters)
            {
              if (!CopilotHelper.AppActionHelper.ValidParameter(actionModel.Parameters[parameter.Key], parameter.Value))
              {
                action = (CopilotHelper.AppActionModel) null;
                break;
              }
            }
          }
        }
        else
          action = (CopilotHelper.AppActionModel) null;
      }
      return (action, flag);
    }

    public static async Task ProcessAction(CopilotHelper.AppActionModel action)
    {
      MainViewModel vm = Ioc.Default.GetService<MainViewModel>();
      if (vm == null)
        vm = (MainViewModel) null;
      else if (vm.Document == null)
      {
        vm = (MainViewModel) null;
      }
      else
      {
        MainView mainView = CopilotHelper.AppActionHelper.GetMainView(vm.Document);
        if (mainView == null)
        {
          vm = (MainViewModel) null;
        }
        else
        {
          string mode;
          int errorCharIndex;
          if (action.Name == "summary")
          {
            ChatPanel chatPanel = mainView.FindName("ChatPanel") as ChatPanel;
            if (chatPanel == null)
            {
              vm = (MainViewModel) null;
              return;
            }
            App.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) (async () =>
            {
              await Task.Delay(500);
              await chatPanel.RequestSummaryAsync();
            }));
          }
          else if (action.Name == "print")
            vm.PrintDocCmd?.Execute((object) null);
          else if (action.Name == "select-page")
          {
            string s;
            int result;
            if (action.Parameters.TryGetValue("page", out s) && int.TryParse(s, out result))
            {
              vm.CurrnetPageIndex = result;
              if (!CopilotHelper.AppActionHelper.IsPdfControlVisible(vm.Document))
                mainView.Menus_SelectItem("View");
            }
          }
          else if (action.Name == "page-display")
          {
            string str;
            if (action.Parameters.TryGetValue("mode", out str))
            {
              switch (str)
              {
                case "actual-size":
                  vm.ViewToolbar.DocSizeModeWrap = SizeModesWrap.ZoomActualSize;
                  break;
                case "fit-page":
                  vm.ViewToolbar.DocSizeModeWrap = SizeModesWrap.FitToSize;
                  break;
                case "fit-width":
                  vm.ViewToolbar.DocSizeModeWrap = SizeModesWrap.FitToWidth;
                  break;
                case "fit-height":
                  vm.ViewToolbar.DocSizeModeWrap = SizeModesWrap.FitToHeight;
                  break;
                case "single-line":
                  vm.ViewToolbar.SubViewModePage = SubViewModePage.SinglePage;
                  break;
                case "double-line":
                  vm.ViewToolbar.SubViewModePage = SubViewModePage.DoublePages;
                  break;
                case "enable-continuous":
                  vm.ViewToolbar.SubViewModeContinuous = SubViewModeContinuous.Verticalcontinuous;
                  break;
                case "disable-continuous":
                  vm.ViewToolbar.SubViewModeContinuous = SubViewModeContinuous.Discontinuous;
                  break;
              }
              if (!CopilotHelper.AppActionHelper.IsPdfControlVisible(vm.Document))
                mainView.Menus_SelectItem("View");
            }
          }
          else if (action.Name == "page-zoom")
          {
            string str;
            if (action.Parameters.TryGetValue("mode", out str))
            {
              if (str == "zoom-in")
                vm.ViewToolbar.DocZoomIn();
              if (str == "zoom-out")
                vm.ViewToolbar.DocZoomOut();
              if (!CopilotHelper.AppActionHelper.IsPdfControlVisible(vm.Document))
                mainView.Menus_SelectItem("View");
            }
          }
          else if (action.Name == "rotate-page")
          {
            if (action.Parameters.TryGetValue("mode", out mode))
            {
              if (!CopilotHelper.AppActionHelper.IsPdfControlVisible(vm.Document))
              {
                mainView.Menus_SelectItem("View");
                await Task.Delay(500);
              }
              if (mode == "left")
                vm.ViewToolbar.PageRotateLeftCmd.Execute((object) null);
              if (mode == "right")
                vm.ViewToolbar.PageRotateRightCmd.Execute((object) null);
            }
            mode = (string) null;
          }
          else if (action.Name == "auto-scroll")
          {
            string str;
            if (action.Parameters.TryGetValue("mode", out str))
            {
              if (!CopilotHelper.AppActionHelper.IsPdfControlVisible(vm.Document))
                mainView.Menus_SelectItem("View");
              if (str == "on" && !vm.ViewToolbar.AutoScrollButtonModel.IsChecked)
                vm.ViewToolbar.AutoScrollButtonModel.Tap();
              if (str == "off")
                vm.ViewToolbar.StopAutoScroll();
            }
          }
          else if (action.Name == "slide-show")
            vm.ViewToolbar.PresentButtonModel.Tap();
          else if (action.Name == "set-background")
          {
            string str;
            if (action.Parameters.TryGetValue("mode", out str))
            {
              string settingName = "";
              switch (str)
              {
                case "default":
                  settingName = "Default";
                  break;
                case "day":
                  settingName = "DayMode";
                  break;
                case "night":
                  settingName = "NightMode";
                  break;
                case "eye-protection":
                  settingName = "EyeProtectionMode";
                  break;
                case "yellow-background":
                  settingName = "YellowBackground";
                  break;
              }
              if (!string.IsNullOrEmpty(settingName))
                vm.ViewToolbar.SetViewerBackground(settingName);
              if (!CopilotHelper.AppActionHelper.IsPdfControlVisible(vm.Document))
                mainView.Menus_SelectItem("View");
            }
          }
          else if (action.Name == "add-annotation")
          {
            if (!CopilotHelper.AppActionHelper.IsPdfControlVisible(vm.Document))
            {
              mainView.Menus_SelectItem("Annotate");
              await Task.Delay(500);
            }
            string str;
            if (action.Parameters.TryGetValue("mode", out str))
            {
              if (str == "highlight-line")
                vm.AnnotationToolbar.HighlightButtonModel.Tap();
              if (str == "underline")
                vm.AnnotationToolbar.UnderlineButtonModel.Tap();
              if (str == "strike-throught")
                vm.AnnotationToolbar.StrikeButtonModel.Tap();
              if (str == "highlight-area")
                vm.AnnotationToolbar.HighlightAreaButtonModel.Tap();
              if (str == "line")
                vm.AnnotationToolbar.LineButtonModel.Tap();
              if (str == "rectangle")
                vm.AnnotationToolbar.SquareButtonModel.Tap();
              if (str == "oval")
                vm.AnnotationToolbar.CircleButtonModel.Tap();
              if (str == "ink")
                vm.AnnotationToolbar.InkButtonModel.Tap();
              if (str == "textbox")
                vm.AnnotationToolbar.TextBoxButtonModel.Tap();
              if (str == "note")
                vm.AnnotationToolbar.NoteButtonModel.Tap();
            }
          }
          else if (action.Name == "show-annotation")
          {
            string str;
            if (action.Parameters.TryGetValue("mode", out str))
            {
              switch (str)
              {
                case "on":
                  vm.IsAnnotationVisible = true;
                  break;
                case "off":
                  vm.IsAnnotationVisible = false;
                  break;
              }
              if (!CopilotHelper.AppActionHelper.IsPdfControlVisible(vm.Document))
                mainView.Menus_SelectItem("View");
            }
          }
          else if (action.Name == "edit-text")
          {
            if (!CopilotHelper.AppActionHelper.IsPdfControlVisible(vm.Document))
            {
              mainView.Menus_SelectItem("View");
              await Task.Delay(500);
            }
            vm.ViewToolbar.EditDocumentButtomModel.Tap();
          }
          else if (action.Name == "extract-page")
          {
            if (vm.PageEditors.PageEditListItemSource != null && action.Parameters.TryGetValue("page", out mode))
            {
              if (CopilotHelper.AppActionHelper.IsPdfControlVisible(vm.Document))
              {
                mainView.Menus_SelectItem("Page");
                await Task.Delay(500);
              }
              int[] pageIndexes;
              if (PdfObjectExtensions.TryParsePageRange(mode, out pageIndexes, out errorCharIndex))
              {
                vm.PageEditors.PageEditListItemSource.AllItemSelected = new bool?(false);
                int count = vm.PageEditors.PageEditListItemSource.Count;
                foreach (int index in pageIndexes)
                {
                  if (index >= 0 && index < count)
                    vm.PageEditors.PageEditListItemSource[index].Selected = true;
                }
              }
              await vm.PageEditors.PageEditorExtractCmd.ExecuteAsync((PdfPageEditListModel) null);
            }
            mode = (string) null;
          }
          else if (action.Name == "delete-page")
          {
            if (vm.PageEditors.PageEditListItemSource != null && action.Parameters.TryGetValue("page", out mode))
            {
              if (CopilotHelper.AppActionHelper.IsPdfControlVisible(vm.Document))
              {
                mainView.Menus_SelectItem("Page");
                await Task.Delay(500);
              }
              int[] pageIndexes;
              if (PdfObjectExtensions.TryParsePageRange(mode, out pageIndexes, out errorCharIndex))
              {
                vm.PageEditors.PageEditListItemSource.AllItemSelected = new bool?(false);
                int count = vm.PageEditors.PageEditListItemSource.Count;
                foreach (int index in pageIndexes)
                {
                  if (index >= 0 && index < count)
                    vm.PageEditors.PageEditListItemSource[index].Selected = true;
                }
              }
              await vm.PageEditors.PageEditorDeleteCmd.ExecuteAsync((PdfPageEditListModel) null);
            }
            mode = (string) null;
          }
          else if (action.Name == "insert-page")
          {
            if (vm.PageEditors.PageEditListItemSource != null && action.Parameters.TryGetValue("mode", out mode))
            {
              if (CopilotHelper.AppActionHelper.IsPdfControlVisible(vm.Document))
              {
                mainView.Menus_SelectItem("Page");
                await Task.Delay(500);
              }
              switch (mode)
              {
                case "blank-page":
                  await vm.PageEditors.PageEditorInsertBlankCmd.ExecuteAsync((PdfPageEditListModel) null);
                  break;
                case "exist-pdf":
                  await vm.PageEditors.PageEditorInsertFromPDFCmd.ExecuteAsync((PdfPageEditListModel) null);
                  break;
                case "word":
                  await vm.PageEditors.PageEditorInsertFromWordCmd.ExecuteAsync((PdfPageEditListModel) null);
                  break;
                case "image":
                  await vm.PageEditors.PageEditorInsertFromImageCmd.ExecuteAsync((PdfPageEditListModel) null);
                  break;
                default:
                  vm.PageEditors.InsertPageButtonModel.Tap();
                  break;
              }
            }
            mode = (string) null;
          }
          else if (action.Name == "crop-page")
          {
            if (vm.PageEditors.PageEditListItemSource.SelectedItems.Count == 0)
              vm.PageEditors.PageEditListItemSource[vm.CurrnetPageIndex].Selected = true;
            vm.PageEditors.CropPageCmd.Execute((object) null);
          }
          else if (action.Name == "convert-from-pdf")
          {
            string _type;
            if (action.Parameters.TryGetValue("mode", out _type))
            {
              ConvFromPDFType? nullable = MapType(_type);
              if (nullable.HasValue)
                AppManager.OpenPDFConverterFromPdf(nullable.Value, vm.DocumentWrapper.DocumentPath);
            }

            static ConvFromPDFType? MapType(string _type)
            {
              if (_type != null)
              {
                switch (_type.Length)
                {
                  case 3:
                    switch (_type[1])
                    {
                      case 'm':
                        if (_type == "xml")
                          return new ConvFromPDFType?(ConvFromPDFType.PDFToXml);
                        break;
                      case 'n':
                        if (_type == "png")
                          return new ConvFromPDFType?(ConvFromPDFType.PDFToPng);
                        break;
                      case 'p':
                        if (_type == "ppt")
                          return new ConvFromPDFType?(ConvFromPDFType.PDFToPPT);
                        break;
                      case 't':
                        if (_type == "rtf")
                          return new ConvFromPDFType?(ConvFromPDFType.PDFToRTF);
                        break;
                      case 'x':
                        if (_type == "txt")
                          return new ConvFromPDFType?(ConvFromPDFType.PDFToTxt);
                        break;
                    }
                    break;
                  case 4:
                    switch (_type[0])
                    {
                      case 'h':
                        if (_type == "html")
                          return new ConvFromPDFType?(ConvFromPDFType.PDFToHtml);
                        break;
                      case 'j':
                        if (_type == "jpeg")
                          return new ConvFromPDFType?(ConvFromPDFType.PDFToJpg);
                        break;
                      case 'w':
                        if (_type == "word")
                          return new ConvFromPDFType?(ConvFromPDFType.PDFToWord);
                        break;
                    }
                    break;
                  case 5:
                    if (_type == "excel")
                      return new ConvFromPDFType?(ConvFromPDFType.PDFToExcel);
                    break;
                }
              }
              return new ConvFromPDFType?();
            }
          }
          else
          {
            if (action.Name == "convert-to-pdf")
            {
              string _type;
              if (action.Parameters.TryGetValue("mode", out _type))
              {
                ConvToPDFType? nullable = MapType(_type);
                if (nullable.HasValue)
                  AppManager.OpenPDFConverterToPdf(nullable.Value);
              }
            }
            else if (action.Name == "compress")
            {
              string str;
              if (action.Parameters.TryGetValue("mode", out str))
              {
                switch (str)
                {
                  case "high":
                    vm.ConverterCommands.DoPDFCompress(new ConverterCommands.CompressMode?(ConverterCommands.CompressMode.High));
                    vm = (MainViewModel) null;
                    return;
                  case "medium":
                    vm.ConverterCommands.DoPDFCompress(new ConverterCommands.CompressMode?(ConverterCommands.CompressMode.Medium));
                    vm = (MainViewModel) null;
                    return;
                  case "low":
                    vm.ConverterCommands.DoPDFCompress(new ConverterCommands.CompressMode?(ConverterCommands.CompressMode.Low));
                    vm = (MainViewModel) null;
                    return;
                  default:
                    vm.ConverterCommands.DoPDFCompress();
                    vm = (MainViewModel) null;
                    return;
                }
              }
              else
                vm.ConverterCommands.DoPDFCompress();
            }
            else if (action.Name == "protect-pdf")
              vm.EncryptCMD.Execute((object) null);
            else if (action.Name == "remove-password")
              vm.RemovePasswordCMD.Execute((object) null);
            else if (action.Name == "contact-us")
              vm.FeedBackCmd.Execute((object) null);
            else if (action.Name == "settings")
              vm.SettingsCmd.Execute((object) null);
            else if (action.Name == "check-update")
              vm.UpdateCmd.Execute((object) null);
            else if (action.Name == "about")
              vm.AboutCmd.Execute((object) null);
            else if (action.Name == "close-pdf")
            {
              mainView.Close();
            }
            else
            {
              string str;
              if (action.Name == "find-in-pdf" && action.Parameters.TryGetValue("text", out str) && !string.IsNullOrWhiteSpace(str))
              {
                vm.Menus.SearchModel.IsSearchVisible = true;
                vm.Menus.SearchModel.SearchText = str.Trim();
              }
            }

            static ConvToPDFType? MapType(string _type)
            {
              switch (_type)
              {
                case "word":
                  return new ConvToPDFType?(ConvToPDFType.WordToPDF);
                case "excel":
                  return new ConvToPDFType?(ConvToPDFType.ExcelToPDF);
                case "image":
                  return new ConvToPDFType?(ConvToPDFType.ImageToPDF);
                case "ppt":
                  return new ConvToPDFType?(ConvToPDFType.PPTToPDF);
                case "rtf":
                  return new ConvToPDFType?(ConvToPDFType.RtfToPDF);
                case "txt":
                  return new ConvToPDFType?(ConvToPDFType.TxtToPDF);
                default:
                  return new ConvToPDFType?();
              }
            }
          }
          await Task.CompletedTask;
          vm = (MainViewModel) null;
        }
      }
    }

    private static MainView GetMainView(PdfDocument pdfDocument)
    {
      PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(pdfDocument);
      return pdfControl != null ? Window.GetWindow((DependencyObject) pdfControl) as MainView : (MainView) null;
    }

    private static bool IsPdfControlVisible(PdfDocument pdfDocument)
    {
      PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(pdfDocument);
      return pdfControl != null && pdfControl.IsVisible;
    }

    private static bool ValidParameter(string type, string value)
    {
      type = type?.Trim()?.ToLowerInvariant();
      if (string.IsNullOrEmpty(type))
        return false;
      int num;
      switch (type)
      {
        case "string":
          return !string.IsNullOrEmpty(value);
        case "int":
          return int.TryParse(value, out num);
        case "int-array":
          return PdfObjectExtensions.TryParsePageRange(value, out int[] _, out num);
        default:
          if (!type.Contains(" | ") || string.IsNullOrEmpty(value) || value.Contains("|"))
            return false;
          return new HashSet<string>(((IEnumerable<string>) type.Split('|')).Select<string, string>((Func<string, string>) (c => c.Trim().ToLowerInvariant())).Distinct<string>()).Contains(value.Trim().ToLowerInvariant());
      }
    }

    private static async Task<(CopilotHelper.ActionModel[] actions, bool maybeNotAppAction)> GetActionsForInput(
      float[] embedding,
      bool appendFindAction,
      CancellationToken cancellationToken)
    {
      CopilotHelper.ActionModel[] allActions = await CopilotHelper.AppActionHelper.GetAllActions();
      if (embedding == null || embedding.Length == 0)
        return (Array.Empty<CopilotHelper.ActionModel>(), true);
      List<(CopilotHelper.ActionModel, float)> list = ((IEnumerable<CopilotHelper.ActionModel>) allActions).Select<CopilotHelper.ActionModel, (CopilotHelper.ActionModel, float)>((Func<CopilotHelper.ActionModel, (CopilotHelper.ActionModel, float)>) (c => (c, CopilotHelper.SimpleCosineSimilarityFloatVersion.ComputeDistance(c.Embedding, embedding)))).OrderBy<(CopilotHelper.ActionModel, float), float>((Func<(CopilotHelper.ActionModel, float), float>) (c => c.distance)).ToList<(CopilotHelper.ActionModel, float)>();
      IEnumerable<CopilotHelper.ActionModel> collection = ((IEnumerable<CopilotHelper.ActionModel>) allActions).Where<CopilotHelper.ActionModel>((Func<CopilotHelper.ActionModel, bool>) (c => c.IsStatic));
      List<CopilotHelper.ActionModel> actionModelList = new List<CopilotHelper.ActionModel>();
      bool flag = true;
      foreach ((CopilotHelper.ActionModel, float) valueTuple in list)
      {
        if ((double) valueTuple.Item2 < 0.08)
          flag = false;
        if ((valueTuple.Item1.Confirm == null ? 0 : (valueTuple.Item1.Confirm.Count > 0 ? 1 : 0)) != 0)
        {
          if ((double) valueTuple.Item2 < 0.3)
            actionModelList.Add(valueTuple.Item1);
        }
        else if ((double) valueTuple.Item2 < 0.15)
          actionModelList.Add(valueTuple.Item1);
        if (actionModelList.Count == 5)
          break;
      }
      if (appendFindAction)
        actionModelList.AddRange(collection);
      return (actionModelList.ToArray(), flag);
    }

    private static async Task<CopilotHelper.ActionModel[]> GetAllActions()
    {
      if (CopilotHelper.AppActionHelper.actionModels != null)
        return CopilotHelper.AppActionHelper.actionModels;
      using (await CopilotHelper.AppActionHelper.asyncLock.LockAsync())
      {
        if (CopilotHelper.AppActionHelper.actionModels != null)
          return CopilotHelper.AppActionHelper.actionModels;
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
        DispatcherHelper.CheckBeginInvokeOnUI((Action) (async () =>
        {
          try
          {
            StreamResourceInfo resourceStream = Application.GetResourceStream(new Uri("pack://application:,,,/pdfeditor;component/Utils/Copilot/actions.json", UriKind.Absolute));
            if (resourceStream.Stream.CanSeek)
              resourceStream.Stream.Seek(0L, SeekOrigin.Begin);
            using (StreamReader streamReader = new StreamReader(resourceStream.Stream, Encoding.UTF8, true, 4096 /*0x1000*/, true))
              CopilotHelper.AppActionHelper.actionModels = ((IEnumerable<CopilotHelper.ActionModel>) JsonConvert.DeserializeObject<CopilotHelper.ActionModelRoot>(await streamReader.ReadToEndAsync()).Actions).Where<CopilotHelper.ActionModel>((Func<CopilotHelper.ActionModel, bool>) (c => c != null && !c.Disabled)).ToArray<CopilotHelper.ActionModel>();
          }
          catch
          {
          }
          if (CopilotHelper.AppActionHelper.actionModels == null)
            CopilotHelper.AppActionHelper.actionModels = Array.Empty<CopilotHelper.ActionModel>();
          tcs.SetResult(true);
        }));
        int num = await tcs.Task ? 1 : 0;
      }
      return CopilotHelper.AppActionHelper.actionModels;
    }

    internal static string GetAppActionConfirm(CopilotHelper.ActionModel action)
    {
      if (action == null)
        return (string) null;
      string resourceName = Ioc.Default.GetService<AppSettingsViewModel>().ActualLanguage.ResourceName;
      string appActionConfirm;
      if (action.Confirm == null || action.Confirm.Count <= 0 || !action.Confirm.TryGetValue(resourceName, out appActionConfirm) && !action.Confirm.TryGetValue("en", out appActionConfirm) || string.IsNullOrEmpty(appActionConfirm))
        return (string) null;
      if (action.Parameters != null && action.Parameters.Count > 0)
      {
        foreach (KeyValuePair<string, string> parameter in action.Parameters)
          appActionConfirm = appActionConfirm.Replace($"{{${parameter.Key}}}", parameter.Value);
      }
      return appActionConfirm;
    }

    internal static async Task InitializeAllActions()
    {
      CopilotHelper.ActionModel[] allActions = await CopilotHelper.AppActionHelper.GetAllActions();
    }

    internal static string GetAppActionConfirm(CopilotHelper.AppActionModel action)
    {
      if (action == null)
        return (string) null;
      if (CopilotHelper.AppActionHelper.actionModels == null)
        return (string) null;
      CopilotHelper.ActionModel actionModel = ((IEnumerable<CopilotHelper.ActionModel>) CopilotHelper.AppActionHelper.actionModels).FirstOrDefault<CopilotHelper.ActionModel>((Func<CopilotHelper.ActionModel, bool>) (c => c.Name == action.Name));
      if (actionModel == null)
        return (string) null;
      string resourceName = Ioc.Default.GetService<AppSettingsViewModel>().ActualLanguage.ResourceName;
      string appActionConfirm;
      if (actionModel.Confirm == null || actionModel.Confirm.Count <= 0 || !actionModel.Confirm.TryGetValue(resourceName, out appActionConfirm) && !actionModel.Confirm.TryGetValue("en", out appActionConfirm) || string.IsNullOrEmpty(appActionConfirm))
        return (string) null;
      if (action.Parameters != null && action.Parameters.Count > 0)
      {
        foreach (KeyValuePair<string, string> parameter in action.Parameters)
          appActionConfirm = appActionConfirm.Replace($"{{${parameter.Key}}}", parameter.Value);
      }
      return appActionConfirm;
    }
  }
}
