using System;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;

namespace Playlist_Kanaeva
{
    internal class Playlist
    {
        private List<Track> list;
        private int currentIndex;

        public Playlist()
        {
            list = new List<Track>();
            currentIndex = 0;
        }

        // Текущая песня
        public Track CurrentSong()
        {
            if (list.Count > 0)
                return list[currentIndex];
            else
                throw new IndexOutOfRangeException("Ошибка");
        }

        // Добавление по частям
        public void AddTrack(string author, string title, string filename)
        {
            list.Add(new Track(author, title, filename));
        }

        // Добавление только другое 
        public void AddTrack(Track track)
        {
            list.Add(track);
        }

        // Следующая песня
        public void NextTrack()
        {
            if (list.Count == 0) return;
            currentIndex = (currentIndex + 1) % list.Count;
        }

        // Предыдущая песня
        public void PrevTrack()
        {
            if (list.Count == 0) return;
            currentIndex = (currentIndex - 1 + list.Count) % list.Count;
        }

        // Переход по индексу
        public void GoToIndex(int index)
        {
            if (index >= 0 && index < list.Count)
                currentIndex = index;
            else
                throw new IndexOutOfRangeException("Неверный индекс!");
        }

        // В начало
        public void GoToStart()
        {
            currentIndex = 0;
        }

        // Удаление по индексу
        public bool RemoveAt(int index)
        {
            if (index >= 0 && index < list.Count)
            {
                list.RemoveAt(index);  // Удаляем только один раз!

                if (list.Count == 0)
                {
                    currentIndex = 0;
                }
                else if (currentIndex > index)
                {
                    currentIndex--;
                }
                else if (currentIndex == index)
                {
                    if (currentIndex >= list.Count)
                    {
                        currentIndex = list.Count - 1;
                    }
                }
                return true;
            }
            return false;
        }

        // Удаление по значению (первое совпадение)
        public bool RemoveAt(Track track)
        {
            int index = list.FindIndex(t => t.Author == track.Author && t.Title == track.Title);
            if (index != -1)
            {
                return RemoveAt(index);
            }
            return false;
        }

        // Удаление текущей песни (которая играет)
        public bool RemoveCurrentTrack()
        {
            if (list.Count == 0) return false;
            return RemoveAt(currentIndex);
        }

        // Очистка плейлиста
        public void Clear()
        {
            list.Clear();
            currentIndex = 0;
        }

        // Получить список для отображения
        public List<Track> GetAllTracks()
        {
            return new List<Track>(list);
        }

        // Количество треков
        public int Count => list.Count;

        // Текущий индекс
        public int CurrentIndex => currentIndex;
    }
}