using Microsoft.Maui.Controls;

namespace Album_music_toma
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            
            // Animation d'entrée pour les éléments
            if (Content is Grid grid && grid.Children.Count > 0)
            {
                foreach (var child in grid.Children)
                {
                    if (child is StackLayout stackLayout)
                    {
                        stackLayout.Opacity = 0;
                        stackLayout.TranslationY = 50;
                        await stackLayout.FadeTo(1, 800);
                        await stackLayout.TranslateTo(0, 0, 600, Easing.CubicOut);
                    }
                }
            }
        }

        private async void OnViewArtistsClicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                // Animation du bouton
                await button.ScaleTo(0.95, 100);
                await button.ScaleTo(1, 100);
            }
            
            await Shell.Current.GoToAsync("//artists");
        }
    }
}
