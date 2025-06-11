// Decompiled with JetBrains decompiler
// Type: pdfconverter.ViewModels.MainWindow2ViewModel
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using Microsoft.Toolkit.Mvvm.ComponentModel;
using pdfconverter.Models;
using pdfconverter.Properties;
using System.Collections.ObjectModel;

#nullable disable
namespace pdfconverter.ViewModels;

public class MainWindow2ViewModel : ObservableObject
{
  public ObservableCollection<ActionMenuGroup> ActionMenus { get; private set; }

  public MainWindow2ViewModel()
  {
    ObservableCollection<ActionMenuGroup> observableCollection = new ObservableCollection<ActionMenuGroup>();
    observableCollection.Add(new ActionMenuGroup()
    {
      Title = Resources.WinMergeSplitLeftMergeTile,
      Tag = "mergepdf"
    });
    observableCollection.Add(new ActionMenuGroup()
    {
      Title = Resources.WinMergeSplitLeftSplitTile,
      Tag = "splitpdf"
    });
    observableCollection.Add(new ActionMenuGroup()
    {
      Title = Resources.CompressPDFText,
      Tag = "compresspdf"
    });
    observableCollection.Add(new ActionMenuGroup()
    {
      Title = Resources.WinListHeaderWordToPDFText,
      Tag = "wordtopdf"
    });
    observableCollection.Add(new ActionMenuGroup()
    {
      Title = Resources.WinListHeaderExcelToPDFText,
      Tag = "exceltopdf"
    });
    observableCollection.Add(new ActionMenuGroup()
    {
      Title = Resources.WinListHeaderPPTToPDFText,
      Tag = "ppttopdf"
    });
    observableCollection.Add(new ActionMenuGroup()
    {
      Title = Resources.WinListHeaderImageToPDFText,
      Tag = "imagetopdf"
    });
    observableCollection.Add(new ActionMenuGroup()
    {
      Title = Resources.WinListHeaderRTFToPDFText,
      Tag = "rtftopdf"
    });
    observableCollection.Add(new ActionMenuGroup()
    {
      Title = Resources.WinListHeaderTXTToPDFText,
      Tag = "txttopdf"
    });
    this.ActionMenus = observableCollection;
  }
}
