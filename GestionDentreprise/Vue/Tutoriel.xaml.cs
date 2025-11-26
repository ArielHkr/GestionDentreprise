using System;
using System.IO;
using System.Windows;

namespace GestionDentreprise.Vue
{
    public partial class Tutoriel : Window
    {
        private static string cheminVideo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tutoriel.mp4");

        public Tutoriel()
        {
            InitializeComponent();
            Loaded += Tutoriel_Loaded;
        }

        private void Tutoriel_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(cheminVideo))
            {
                VideoPlayer.Source = new Uri(cheminVideo);
                VideoPlayer.Play();
            }
            else
            {
                MessageBox.Show($"Fichier vidéo introuvable : {cheminVideo}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            VideoPlayer.Play();
        }

        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            VideoPlayer.Pause();
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            VideoPlayer.Stop();
        }

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {
            if (VideoPlayer.Position.TotalSeconds > 10)
            {
                VideoPlayer.Position = VideoPlayer.Position.Subtract(TimeSpan.FromSeconds(10));
            }
            else
            {
                VideoPlayer.Position = TimeSpan.Zero;
            }
        }

        private void BtnAvance_Click(object sender, RoutedEventArgs e)
        {
            if (VideoPlayer.NaturalDuration.HasTimeSpan)
            {
                var nouvellePosition = VideoPlayer.Position.Add(TimeSpan.FromSeconds(10));
                if (nouvellePosition < VideoPlayer.NaturalDuration.TimeSpan)
                {
                    VideoPlayer.Position = nouvellePosition;
                }
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            VideoPlayer.Stop();
            VideoPlayer.Close();
            base.OnClosed(e);
        }
    }
}