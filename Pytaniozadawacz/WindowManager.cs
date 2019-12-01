using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;

namespace Pytaniozadawacz
{
    class WindowManager
    {
        public static void ClearGrid(Grid grid)
        {
            grid.Children.Clear();
            grid.RowDefinitions.Clear();
            grid.UpdateLayout();
        }
        public static void ChangeText(Label label, Color color, string text)
        {
            label.Foreground = new SolidColorBrush(color);
            label.Content = text;
        }
        private static void FillViewBoxProperties(Viewbox viewBox, StretchDirection vbDirection, Stretch vbStretch, Thickness vbThickness)
        {
            viewBox.StretchDirection = vbDirection;
            viewBox.Stretch = vbStretch;
            viewBox.Margin = vbThickness;
        }
        private static void FillTextBlockProperties(TextBlock textBlock, VerticalAlignment tbVerAlignment, HorizontalAlignment tbHorAlignment, TextWrapping tbWrapping, TextAlignment tbAlignment, int tbWidth, string tbContent)
        {
            textBlock.VerticalAlignment = tbVerAlignment;
            textBlock.HorizontalAlignment = tbHorAlignment;
            textBlock.TextWrapping = tbWrapping;
            textBlock.TextAlignment = tbAlignment;
            textBlock.Width = tbWidth;
            textBlock.Text = tbContent;
            textBlock.FontSize = 15;
        }
        private static void FillButtonProperties(Button button, Thickness marginThickness, SolidColorBrush color)
        {
            button.Margin = marginThickness;
            button.BorderBrush = color;
        }

        public static void FillDynamicGrid(Grid grid, RowDefinition gridRow, Button gridButton, Viewbox gridVBox, TextBlock gridTBlock, KeyValuePair<int, string> answer)
        {
            gridRow.Height = new GridLength(1, GridUnitType.Star);
            grid.RowDefinitions.Add(gridRow);
            FillButtonProperties(gridButton, new Thickness(10, 5, 10, 5), new SolidColorBrush(Color.FromArgb(255, 221, 221, 221)));
            FillViewBoxProperties(gridVBox, StretchDirection.Both, Stretch.Uniform, new Thickness(20, 0, 20, 0));
            FillTextBlockProperties(gridTBlock, VerticalAlignment.Stretch, HorizontalAlignment.Left, TextWrapping.Wrap, TextAlignment.Justify, 900, answer.Value);
            gridVBox.Child = gridTBlock;
            gridButton.Content = gridVBox;
            //answerBut.Content = answer.Value;
            Grid.SetRow(gridButton, answer.Key);
            grid.Children.Add(gridButton);
        }

        public static void FillDynamicGrid(Grid grid, RowDefinition gridRow, ColumnDefinition gridColumn, Button gridButton, int questNo, int rowNo, int colNo)
        {
            gridRow.Height = new GridLength(1, GridUnitType.Star);
            gridColumn.Width = new GridLength(1, GridUnitType.Star);
            grid.RowDefinitions.Add(gridRow);
            grid.ColumnDefinitions.Add(gridColumn);
            gridButton.Content = questNo.ToString();
            gridButton.Name = "Question" + questNo.ToString();
            Grid.SetColumn(gridButton, colNo);
            Grid.SetRow(gridButton, rowNo);
            grid.Children.Add(gridButton);
        }
    }
}
