using System;
using System.Windows;
using System.Windows.Controls;

namespace Playlist_Kanaeva
{
    public partial class MainWindow : Window
    {
        private Playlist playlist;

        public MainWindow()
        {
            InitializeComponent();
            playlist = new Playlist();

            // для примера
            playlist.AddTrack("The Beatles", "Yesterday", "yesterday.mp3");
            playlist.AddTrack("МС Хованский", "Прости меня, Оксимирон", "sorryOximiron.mp3");
            playlist.AddTrack("Время и Стекло", "Навернопотомучто", "maybe.mp3");

            UpdateUI();
        }

        private void UpdateUI()
        {
            // Обновляем список
            lstTracks.ItemsSource = null;
            lstTracks.ItemsSource = playlist.GetAllTracks();

            // Обновляем информацию о текущей песне
            if (playlist.Count > 0)
            {
                var current = playlist.CurrentSong();
                txtCurrentSong.Text = $"{current.Author} - {current.Title} ({current.Filename})";
                txtCurrentIndex.Text = $"Песня {playlist.CurrentIndex + 1} из {playlist.Count}";
            }
            else
            {
                txtCurrentSong.Text = "Плейлист пуст";
                txtCurrentIndex.Text = "Нет песен";
            }
        }

        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            if (playlist.Count == 0) return;
            playlist.PrevTrack();
            UpdateUI();
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            if (playlist.Count == 0) return;
            playlist.NextTrack();
            UpdateUI();
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            if (playlist.Count == 0) return;
            playlist.GoToStart();
            UpdateUI();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Очистить весь плейлист?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                playlist.Clear();
                UpdateUI();
            }
        }

        private void BtnAddDemo_Click(object sender, RoutedEventArgs e)
        {
            playlist.AddTrack("ДИМА БИЛАН", "Я ТВОЙ НОМЕР ОДИИИН", "one.mp3");
            UpdateUI();
        }

        private void BtnRemoveSelected_Click(object sender, RoutedEventArgs e)
        {
            if (playlist.Count == 0)
            {
                MessageBox.Show("Плейлист пуст!", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var currentTrack = playlist.CurrentSong();

            if (MessageBox.Show($"Удалить текущую песню:\n\"{currentTrack.Author} - {currentTrack.Title}\"?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                playlist.RemoveAt(playlist.CurrentIndex);
                UpdateUI();
            }
        }


      
        private void LstTracks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstTracks.SelectedItem != null && playlist.Count > 0)
            {
                Track selected = (Track)lstTracks.SelectedItem;
                int index = playlist.GetAllTracks().IndexOf(selected);
                try
                {
                    playlist.GoToIndex(index);
                    UpdateUI();
                }
                catch { }
            }
        }

        // Новые методы для меню
        private void MenuItem_AddSong_Click(object sender, RoutedEventArgs e)
        {
            // Создаем диалоговое окно
            Window dialog = new Window
            {
                Title = "Добавление песни",
                Width = 400,
                Height = 250,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this,
                ResizeMode = ResizeMode.NoResize
            };

            StackPanel panel = new StackPanel { Margin = new Thickness(20) };

            // Поле для автора
            panel.Children.Add(new TextBlock
            {
                Text = "Исполнитель:",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 5)
            });
            TextBox txtAuthor = new TextBox
            {
                Margin = new Thickness(0, 0, 0, 15),
                Height = 30,
                FontSize = 14
            };
            panel.Children.Add(txtAuthor);

            // Поле для названия
            panel.Children.Add(new TextBlock
            {
                Text = "Название песни:",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 5)
            });
            TextBox txtTitle = new TextBox
            {
                Margin = new Thickness(0, 0, 0, 15),
                Height = 30,
                FontSize = 14
            };
            panel.Children.Add(txtTitle);

            // Кнопки
            StackPanel buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 0)
            };

            Button btnOk = new Button
            {
                Content = "Добавить",
                Width = 100,
                Height = 35,
                Margin = new Thickness(5),
                Background = System.Windows.Media.Brushes.LightGreen
            };
           

            buttonPanel.Children.Add(btnOk);
           
            panel.Children.Add(buttonPanel);

            dialog.Content = panel;

            // Обработчик кноп
            btnOk.Click += (s, args) =>
            {
                if (string.IsNullOrWhiteSpace(txtAuthor.Text) || string.IsNullOrWhiteSpace(txtTitle.Text))
                {
                    MessageBox.Show("заполните все поля!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                playlist.AddTrack(txtAuthor.Text, txtTitle.Text, $"{txtAuthor.Text}_{txtTitle.Text}.mp3");
                UpdateUI();
                dialog.Close();

                MessageBox.Show($"Песня \"{txtAuthor.Text} - {txtTitle.Text}\"добавлена!",
                    "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            };

         

            dialog.ShowDialog();
        }

        private void MenuItem_RemoveName_Click(object sender, RoutedEventArgs e)
        {
            // Создаем диалоговое окно 
            Window dialog = new Window
            {
                Title = "Удаление песни по названию",  
                Width = 400,
                Height = 250,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this,
                ResizeMode = ResizeMode.NoResize
            };

            StackPanel panel = new StackPanel { Margin = new Thickness(20) };

            // Поле для автора
            panel.Children.Add(new TextBlock
            {
                Text = "Исполнитель:",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 5)
            });
            TextBox txtAuthor = new TextBox
            {
                Margin = new Thickness(0, 0, 0, 15),
                Height = 30,
                FontSize = 14
            };
            panel.Children.Add(txtAuthor);

            // Поле для названия
            panel.Children.Add(new TextBlock
            {
                Text = "Название песни:",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 5)
            });
            TextBox txtTitle = new TextBox
            {
                Margin = new Thickness(0, 0, 0, 15),
                Height = 30,
                FontSize = 14
            };
            panel.Children.Add(txtTitle);

            // Кнопкa
            StackPanel buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 0)
            };

            Button btnDelete = new Button
            {
                Content = "Удалить",
                Width = 100,
                Height = 35,
                Margin = new Thickness(5),
                Background = System.Windows.Media.Brushes.LightCoral 
            };

          

            buttonPanel.Children.Add(btnDelete);
           
            panel.Children.Add(buttonPanel);
            dialog.Content = panel;

            
            btnDelete.Click += (s, args) =>
            {
                // Проверяем, что поля заполнены
                if (string.IsNullOrWhiteSpace(txtAuthor.Text) || string.IsNullOrWhiteSpace(txtTitle.Text))
                {
                    MessageBox.Show("Заполните все поля!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

               
                Track trackToDelete = new Track(
                    txtAuthor.Text.Trim(),
                    txtTitle.Text.Trim(),
                    $"{txtAuthor.Text.Trim()}_{txtTitle.Text.Trim()}.mp3"
                );

                // Вызываем метод RemoveAt 
                bool deleted = playlist.RemoveAt(trackToDelete);

                if (deleted)
                {
                    UpdateUI();
                    dialog.Close();

                    MessageBox.Show($"Песня \"{txtAuthor.Text.Trim()} - {txtTitle.Text.Trim()}\" удалена!",
                        "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    
                    MessageBox.Show($"Песня \"{txtAuthor.Text.Trim()} - {txtTitle.Text.Trim()}\" не найдена в плейлисте!",
                        "Не найдено", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            };

          

            dialog.ShowDialog();
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

     
    }
}
