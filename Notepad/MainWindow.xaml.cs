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
        private string _filePath = "";
        private string _contents;
        private string[] _args = Environment.GetCommandLineArgs();

        private bool _change = false;
        private bool _newFile = true;
        public MainWindow()
        {
            InitializeComponent();

            Window = this;
            

            if (_args.Length > 1)
            {
                _filePath = _args[1];
                _contents = File.ReadAllText(_filePath);

                textBox1.Text += _contents;
                openMenuItem.IsSubmenuOpen = true;

                nameFile.Content = Path.GetFileName(_filePath);

                _newFile = false;
            }
            else
            {
                nameFile.Content = "New file";
            }
        }

        private void MovingWin(object sender, RoutedEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                MainWindow.Window.DragMove();
        }

        private void ExitButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                string messageBoxText = "Do you want to save changes?";
                string caption = "Word Processor";
                MessageBoxButton button = MessageBoxButton.YesNoCancel;

                if (_change)
                {
                    MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button);

                    // Process message box results
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            Save();
                            this.Close();

                            break;
                        case MessageBoxResult.No:
                            Save();
                            this.Close();

                            break;
                        case MessageBoxResult.Cancel:


                            break;

                    }
                }
                else
                    this.Close();
            }
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

            if (e.Key != null && !_change && e.Key != Key.CapsLock && e.Key != Key.LeftCtrl && e.Key != Key.LeftShift && e.Key != Key.LeftAlt && e.Key != Key.RightAlt && e.Key != Key.RightCtrl && e.Key != Key.RightShift && e.Key != Key.Right && e.Key != Key.Left && e.Key != Key.Up && e.Key != Key.Down && e.Key != Key.Escape)
            {
                _change = true;
                nameFile.Content += "*";
            }
                

            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.S)
            {
                Save();
                string nameString = nameFile.Content.ToString();
                if (_change && !_newFile)
                {
                    nameFile.Content = nameString.Remove(nameString.Length - 1);
                    _change = false;
                }
            }
        }

        private void SaveNewFile()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "NewFile"; 
            dialog.DefaultExt = ".text"; 
            dialog.Filter = "Text documents (.txt)|*.txt";

            Nullable<bool> result = dialog.ShowDialog();
            if (result == true)
            {
                _newFile = false;
                _filePath = dialog.FileName;
                nameFile.Content = Path.GetFileName(_filePath);
                
            }

            
        }

        private void Save()
        {
            if(_newFile)
            {
                SaveNewFile();
                return;
            }

            using (StreamWriter writer = new StreamWriter(_filePath))
            {
                writer.Write(textBox1.Text);
                //File.WriteAllText(_filePath, _contents);
                writer.Close();
            }
        }
        private void Open()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                textBox1.Text = File.ReadAllText(openFileDialog.FileName);
        }

        private void SaveAs()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true || !String.IsNullOrEmpty(openFileDialog.FileName))
            {
                File.WriteAllText(openFileDialog.FileName, textBox1.Text);
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
            Open();
        }

        private void MenuItem_PreviewMouseDown_1(object sender, MouseButtonEventArgs e)
        {
            SaveNewFile();
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

        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            textBox1.SelectionStart = textBox1.Text.Length;
        }
    }
}
