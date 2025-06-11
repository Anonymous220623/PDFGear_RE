// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.DataGridAttach
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

#nullable disable
namespace HandyControl.Controls;

public class DataGridAttach
{
  public static readonly DependencyProperty ApplyDefaultStyleProperty = DependencyProperty.RegisterAttached("ApplyDefaultStyle", typeof (bool), typeof (DataGridAttach), new PropertyMetadata(ValueBoxes.FalseBox, new PropertyChangedCallback(DataGridAttach.OnApplyDefaultStyleChanged)));
  public static readonly DependencyProperty TextColumnStyleProperty = DependencyProperty.RegisterAttached("TextColumnStyle", typeof (Style), typeof (DataGridAttach), new PropertyMetadata((object) null, new PropertyChangedCallback(DataGridAttach.OnTextColumnStyleChanged)));
  public static readonly DependencyProperty EditingTextColumnStyleProperty = DependencyProperty.RegisterAttached("EditingTextColumnStyle", typeof (Style), typeof (DataGridAttach), new PropertyMetadata((object) null, new PropertyChangedCallback(DataGridAttach.OnTextColumnStyleChanged)));
  public static readonly DependencyProperty ComboBoxColumnStyleProperty = DependencyProperty.RegisterAttached("ComboBoxColumnStyle", typeof (Style), typeof (DataGridAttach), new PropertyMetadata((object) null, new PropertyChangedCallback(DataGridAttach.OnComboBoxColumnStyleChanged)));
  public static readonly DependencyProperty EditingComboBoxColumnStyleProperty = DependencyProperty.RegisterAttached("EditingComboBoxColumnStyle", typeof (Style), typeof (DataGridAttach), new PropertyMetadata((object) null, new PropertyChangedCallback(DataGridAttach.OnComboBoxColumnStyleChanged)));
  public static readonly DependencyProperty CheckBoxColumnStyleProperty = DependencyProperty.RegisterAttached("CheckBoxColumnStyle", typeof (Style), typeof (DataGridAttach), new PropertyMetadata((object) null, new PropertyChangedCallback(DataGridAttach.OnCheckBoxColumnStyleChanged)));
  public static readonly DependencyProperty EditingCheckBoxColumnStyleProperty = DependencyProperty.RegisterAttached("EditingCheckBoxColumnStyle", typeof (Style), typeof (DataGridAttach), new PropertyMetadata((object) null, new PropertyChangedCallback(DataGridAttach.OnCheckBoxColumnStyleChanged)));
  public static DependencyProperty ShowRowNumberProperty = DependencyProperty.RegisterAttached("ShowRowNumber", typeof (bool), typeof (DataGridAttach), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(DataGridAttach.OnShowRowNumberChanged)));
  public static readonly DependencyProperty CanUnselectAllWithBlankAreaProperty = DependencyProperty.RegisterAttached("CanUnselectAllWithBlankArea", typeof (bool), typeof (DataGridAttach), new PropertyMetadata(ValueBoxes.FalseBox, new PropertyChangedCallback(DataGridAttach.OnCanUnselectAllWithBlankAreaChanged)));
  public static readonly DependencyProperty ShowSelectAllButtonProperty = DependencyProperty.RegisterAttached("ShowSelectAllButton", typeof (bool), typeof (DataGridAttach), new PropertyMetadata(ValueBoxes.TrueBox));

  private static void OnApplyDefaultStyleChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    DataGrid grid = (DataGrid) d;
    if ((bool) e.NewValue)
      grid.Columns.CollectionChanged += new NotifyCollectionChangedEventHandler(OnDataGridColumnsCollectionChanged);
    else
      grid.Columns.CollectionChanged -= new NotifyCollectionChangedEventHandler(OnDataGridColumnsCollectionChanged);

    void OnDataGridColumnsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      bool flag;
      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Remove:
        case NotifyCollectionChangedAction.Move:
          flag = true;
          break;
        default:
          flag = false;
          break;
      }
      if (flag)
        return;
      DataGridAttach.UpdateTextColumnStyles(grid);
      DataGridAttach.UpdateComboBoxColumnStyles(grid);
      DataGridAttach.UpdateCheckBoxColumnStyles(grid);
    }
  }

  public static void SetApplyDefaultStyle(DependencyObject element, bool value)
  {
    element.SetValue(DataGridAttach.ApplyDefaultStyleProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetApplyDefaultStyle(DependencyObject element)
  {
    return (bool) element.GetValue(DataGridAttach.ApplyDefaultStyleProperty);
  }

  private static void OnTextColumnStyleChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    DataGrid grid = (DataGrid) d;
    if (e.OldValue != null || e.NewValue == null)
      return;
    DataGridAttach.UpdateTextColumnStyles(grid);
  }

  public static void SetTextColumnStyle(DependencyObject element, Style value)
  {
    element.SetValue(DataGridAttach.TextColumnStyleProperty, (object) value);
  }

  public static Style GetTextColumnStyle(DependencyObject element)
  {
    return (Style) element.GetValue(DataGridAttach.TextColumnStyleProperty);
  }

  public static void SetEditingTextColumnStyle(DependencyObject element, Style value)
  {
    element.SetValue(DataGridAttach.EditingTextColumnStyleProperty, (object) value);
  }

  public static Style GetEditingTextColumnStyle(DependencyObject element)
  {
    return (Style) element.GetValue(DataGridAttach.EditingTextColumnStyleProperty);
  }

  private static void UpdateTextColumnStyles(DataGrid grid)
  {
    Style textColumnStyle = DataGridAttach.GetTextColumnStyle((DependencyObject) grid);
    Style editingTextColumnStyle = DataGridAttach.GetEditingTextColumnStyle((DependencyObject) grid);
    if (textColumnStyle != null)
    {
      foreach (DataGridTextColumn dataGridTextColumn in grid.Columns.OfType<DataGridTextColumn>())
      {
        Style style = new Style()
        {
          BasedOn = dataGridTextColumn.ElementStyle,
          TargetType = textColumnStyle.TargetType
        };
        foreach (Setter setter in textColumnStyle.Setters.OfType<Setter>())
          style.Setters.Add((SetterBase) setter);
        dataGridTextColumn.ElementStyle = style;
      }
    }
    if (editingTextColumnStyle == null)
      return;
    foreach (DataGridTextColumn dataGridTextColumn in grid.Columns.OfType<DataGridTextColumn>())
    {
      Style style = new Style()
      {
        BasedOn = dataGridTextColumn.EditingElementStyle,
        TargetType = editingTextColumnStyle.TargetType
      };
      foreach (Setter setter in editingTextColumnStyle.Setters.OfType<Setter>())
        style.Setters.Add((SetterBase) setter);
      dataGridTextColumn.EditingElementStyle = style;
    }
  }

  private static void OnComboBoxColumnStyleChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    DataGrid grid = (DataGrid) d;
    if (e.OldValue != null || e.NewValue == null)
      return;
    DataGridAttach.UpdateComboBoxColumnStyles(grid);
  }

  public static void SetComboBoxColumnStyle(DependencyObject element, Style value)
  {
    element.SetValue(DataGridAttach.ComboBoxColumnStyleProperty, (object) value);
  }

  public static Style GetComboBoxColumnStyle(DependencyObject element)
  {
    return (Style) element.GetValue(DataGridAttach.ComboBoxColumnStyleProperty);
  }

  public static void SetEditingComboBoxColumnStyle(DependencyObject element, Style value)
  {
    element.SetValue(DataGridAttach.EditingComboBoxColumnStyleProperty, (object) value);
  }

  public static Style GetEditingComboBoxColumnStyle(DependencyObject element)
  {
    return (Style) element.GetValue(DataGridAttach.EditingComboBoxColumnStyleProperty);
  }

  private static void UpdateComboBoxColumnStyles(DataGrid grid)
  {
    Style comboBoxColumnStyle1 = DataGridAttach.GetComboBoxColumnStyle((DependencyObject) grid);
    Style comboBoxColumnStyle2 = DataGridAttach.GetEditingComboBoxColumnStyle((DependencyObject) grid);
    if (comboBoxColumnStyle1 != null)
    {
      foreach (DataGridComboBoxColumn gridComboBoxColumn in grid.Columns.OfType<DataGridComboBoxColumn>())
      {
        Style style = new Style()
        {
          BasedOn = gridComboBoxColumn.ElementStyle,
          TargetType = comboBoxColumnStyle1.TargetType
        };
        foreach (Setter setter in comboBoxColumnStyle1.Setters.OfType<Setter>())
          style.Setters.Add((SetterBase) setter);
        gridComboBoxColumn.ElementStyle = style;
      }
    }
    if (comboBoxColumnStyle2 == null)
      return;
    foreach (DataGridComboBoxColumn gridComboBoxColumn in grid.Columns.OfType<DataGridComboBoxColumn>())
    {
      Style style = new Style()
      {
        BasedOn = gridComboBoxColumn.EditingElementStyle,
        TargetType = comboBoxColumnStyle2.TargetType
      };
      foreach (Setter setter in comboBoxColumnStyle2.Setters.OfType<Setter>())
        style.Setters.Add((SetterBase) setter);
      gridComboBoxColumn.EditingElementStyle = style;
    }
  }

  private static void OnCheckBoxColumnStyleChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    DataGrid grid = (DataGrid) d;
    if (e.OldValue != null || e.NewValue == null)
      return;
    DataGridAttach.UpdateCheckBoxColumnStyles(grid);
  }

  public static void SetCheckBoxColumnStyle(DependencyObject element, Style value)
  {
    element.SetValue(DataGridAttach.CheckBoxColumnStyleProperty, (object) value);
  }

  public static Style GetCheckBoxColumnStyle(DependencyObject element)
  {
    return (Style) element.GetValue(DataGridAttach.CheckBoxColumnStyleProperty);
  }

  public static void SetEditingCheckBoxColumnStyle(DependencyObject element, Style value)
  {
    element.SetValue(DataGridAttach.EditingCheckBoxColumnStyleProperty, (object) value);
  }

  public static Style GetEditingCheckBoxColumnStyle(DependencyObject element)
  {
    return (Style) element.GetValue(DataGridAttach.EditingCheckBoxColumnStyleProperty);
  }

  private static void UpdateCheckBoxColumnStyles(DataGrid grid)
  {
    Style checkBoxColumnStyle1 = DataGridAttach.GetCheckBoxColumnStyle((DependencyObject) grid);
    Style checkBoxColumnStyle2 = DataGridAttach.GetEditingCheckBoxColumnStyle((DependencyObject) grid);
    if (checkBoxColumnStyle1 != null)
    {
      foreach (DataGridCheckBoxColumn gridCheckBoxColumn in grid.Columns.OfType<DataGridCheckBoxColumn>())
      {
        Style style = new Style()
        {
          BasedOn = gridCheckBoxColumn.ElementStyle,
          TargetType = checkBoxColumnStyle1.TargetType
        };
        foreach (Setter setter in checkBoxColumnStyle1.Setters.OfType<Setter>())
          style.Setters.Add((SetterBase) setter);
        gridCheckBoxColumn.ElementStyle = style;
      }
    }
    if (checkBoxColumnStyle2 == null)
      return;
    foreach (DataGridCheckBoxColumn gridCheckBoxColumn in grid.Columns.OfType<DataGridCheckBoxColumn>())
    {
      Style style = new Style()
      {
        BasedOn = gridCheckBoxColumn.EditingElementStyle,
        TargetType = checkBoxColumnStyle2.TargetType
      };
      foreach (Setter setter in checkBoxColumnStyle2.Setters.OfType<Setter>())
        style.Setters.Add((SetterBase) setter);
      gridCheckBoxColumn.EditingElementStyle = style;
    }
  }

  public static bool GetShowRowNumber(DependencyObject target)
  {
    return (bool) target.GetValue(DataGridAttach.ShowRowNumberProperty);
  }

  public static void SetShowRowNumber(DependencyObject target, bool value)
  {
    target.SetValue(DataGridAttach.ShowRowNumberProperty, ValueBoxes.BooleanBox(value));
  }

  private static void OnShowRowNumberChanged(
    DependencyObject target,
    DependencyPropertyChangedEventArgs e)
  {
    DataGrid dataGrid = target as DataGrid;
    if (dataGrid == null)
      return;
    bool show = (bool) e.NewValue;
    if (show)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      dataGrid.LoadingRow += DataGridAttach.\u003C\u003EO.\u003C0\u003E__DataGrid_LoadingRow ?? (DataGridAttach.\u003C\u003EO.\u003C0\u003E__DataGrid_LoadingRow = new EventHandler<DataGridRowEventArgs>(DataGridAttach.DataGrid_LoadingRow));
      dataGrid.ItemContainerGenerator.ItemsChanged += new ItemsChangedEventHandler(ItemContainerGeneratorItemsChanged);
    }
    else
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      dataGrid.LoadingRow -= DataGridAttach.\u003C\u003EO.\u003C0\u003E__DataGrid_LoadingRow ?? (DataGridAttach.\u003C\u003EO.\u003C0\u003E__DataGrid_LoadingRow = new EventHandler<DataGridRowEventArgs>(DataGridAttach.DataGrid_LoadingRow));
      dataGrid.ItemContainerGenerator.ItemsChanged -= new ItemsChangedEventHandler(ItemContainerGeneratorItemsChanged);
    }

    void ItemContainerGeneratorItemsChanged(object sender, ItemsChangedEventArgs e)
    {
      ItemContainerGenerator containerGenerator = dataGrid.ItemContainerGenerator;
      int count = dataGrid.Items.Count;
      if (show)
      {
        for (int index = 0; index < count; ++index)
        {
          DataGridRow dataGridRow = (DataGridRow) containerGenerator.ContainerFromIndex(index);
          if (dataGridRow != null)
            dataGridRow.Header = (object) (index + 1).ToString();
        }
      }
      else
      {
        for (int index = 0; index < count; ++index)
        {
          DataGridRow dataGridRow = (DataGridRow) containerGenerator.ContainerFromIndex(index);
          if (dataGridRow != null)
            dataGridRow.Header = (object) null;
        }
      }
    }
  }

  private static void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
  {
    e.Row.Header = (object) (e.Row.GetIndex() + 1).ToString();
  }

  private static void OnCanUnselectAllWithBlankAreaChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is DataGrid dataGrid))
      return;
    if ((bool) e.NewValue)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      dataGrid.PreviewMouseDown += DataGridAttach.\u003C\u003EO.\u003C1\u003E__DataGrid_PreviewMouseDown ?? (DataGridAttach.\u003C\u003EO.\u003C1\u003E__DataGrid_PreviewMouseDown = new MouseButtonEventHandler(DataGridAttach.DataGrid_PreviewMouseDown));
    }
    else
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      dataGrid.PreviewMouseDown -= DataGridAttach.\u003C\u003EO.\u003C1\u003E__DataGrid_PreviewMouseDown ?? (DataGridAttach.\u003C\u003EO.\u003C1\u003E__DataGrid_PreviewMouseDown = new MouseButtonEventHandler(DataGridAttach.DataGrid_PreviewMouseDown));
    }
  }

  private static void DataGrid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
  {
    if (!(sender is DataGrid dataGrid) || !(e.OriginalSource is System.Windows.Controls.ScrollViewer))
      return;
    dataGrid.CommitEdit();
    dataGrid.UnselectAll();
  }

  public static void SetCanUnselectAllWithBlankArea(DependencyObject element, bool value)
  {
    element.SetValue(DataGridAttach.CanUnselectAllWithBlankAreaProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetCanUnselectAllWithBlankArea(DependencyObject element)
  {
    return (bool) element.GetValue(DataGridAttach.CanUnselectAllWithBlankAreaProperty);
  }

  public static void SetShowSelectAllButton(DependencyObject element, bool value)
  {
    element.SetValue(DataGridAttach.ShowSelectAllButtonProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetShowSelectAllButton(DependencyObject element)
  {
    return (bool) element.GetValue(DataGridAttach.ShowSelectAllButtonProperty);
  }
}
