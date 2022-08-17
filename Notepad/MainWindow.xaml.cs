using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

using System.Diagnostics;
using System.IO;
using Microsoft.Win32;


namespace Notepad
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Window;
        private string _filePath = "C:\\Users\\dfyzp\\Desktop\\Notepad\\Notepad\\bin\\Debug\\test.txt";
        private string _contents;
        public MainWindow()
        {
            InitializeComponent();

            Window = this;
            //string filename = @"a.txt";
            string[] args = Environment.GetCommandLineArgs();

            if (args.Length > 1)
                _filePath = args[1];
            _contents = File.ReadAllText(_filePath);

            textBox1.Text += _contents;
            openMenuItem.IsSubmenuOpen = true;
        }

        private void MovingWin(object sender, RoutedEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                MainWindow.Window.DragMove();
        }

        private void ExitButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                this.Close();
        }

        private void MaxButton_MouseDown(object sendem, MouseButtonEventArgs e)
        {
            if (WindowState != WindowState.Maximized)
            {
                this.WindowState = WindowState.Maximized;
                this.BorderThickness = new System.Windows.Thickness(6);
            }

            else
            {
                this.WindowState = WindowState.Normal;
                this.BorderThickness = new System.Windows.Thickness(0);
            }


        }

        private void MinButton_MouseDown(object sendem, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                int index = textBox1.SelectionStart;
                textBox1.Text = textBox1.Text.Substring(0, textBox1.SelectionStart) + Environment.NewLine
                    + textBox1.Text.Substring(textBox1.SelectionStart);
                textBox1.SelectionStart = textBox1.Text.Length;
            }

            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.S)
            {
                Save();
            }
        }

        private void Save()
        {
            using (StreamWriter writer = new StreamWriter(_filePath))
            {
                writer.Write(textBox1.Text);
                //File.WriteAllText(_filePath, _contents);
                writer.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Save();
        }

        private void openMenuItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                textBox1.Text = File.ReadAllText(openFileDialog.FileName);
        }

        private void MenuItem_PreviewMouseDown_1(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true || !String.IsNullOrEmpty(openFileDialog.FileName))
            {
                File.WriteAllText(openFileDialog.FileName, textBox1.Text);
            }
        }

        private void textBox1_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers != ModifierKeys.Control)
                return;

            if (e.Delta > 0)
                textBox1.FontSize += 1;

            else if (e.Delta < 0)
                textBox1.FontSize -= 1;
        }
    }
}
