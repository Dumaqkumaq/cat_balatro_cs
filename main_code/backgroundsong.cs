using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace project_cat_balatro
{
    class backgroundsong : IDisposable
    {
        private WaveOutEvent _waveOut;
        private AudioFileReader _audioFile;

        public void Play()
        {
            Stop();
            Random rnd = new Random();
            string filePath = $"Res\\Music\\meow{rnd.Next(4) + 1}.mp3";
            _audioFile = new AudioFileReader(filePath);
            _waveOut = new WaveOutEvent();
            _waveOut.Init(_audioFile);
            _waveOut.Play();
        }
        public void InitializeBackgroundMusic(string filePath)
        {
            // Освобождаем предыдущие ресурсы
            _waveOut?.Dispose();
            _audioFile?.Dispose();

            // Инициализируем аудиопоток
            _audioFile = new AudioFileReader(filePath);
            _waveOut = new WaveOutEvent();

            // Настраиваем цикличное воспроизведение
            _waveOut.PlaybackStopped += OnPlaybackStopped;
            _waveOut.Init(_audioFile);
            _waveOut.Play();
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (_waveOut != null && _audioFile != null)
            {
                _audioFile.Position = 0;
                _waveOut.Play();
            }
        }

        public void Dispose()
        {
            Stop();
        }

        public void Stop()
        {
            _waveOut?.Stop();
            _waveOut?.Dispose();
            _audioFile?.Dispose();
            _waveOut = null;
            _audioFile = null;
        }
    }
}
