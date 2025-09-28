# Album Musical - Application MAUI

## Vue d'ensemble

Cette application musicale développée avec .NET MAUI utilise l'architecture **MVVM (Model-View-ViewModel)** pour organiser le code de manière claire et maintenable. L'application permet de naviguer entre artistes, albums et chansons avec une interface moderne et intuitive.

## Architecture MVVM

### Modèles (Models)
Les modèles représentent les données de l'application et définissent la structure des entités métier.

#### `Artist.cs`
```csharp
public class Artist
{
    public string Name { get; set; } = "";
    public string PhotoUrl { get; set; } = "";
    public string Genre { get; set; } = "";
    public string Bio { get; set; } = "";
    public List<Album> Albums { get; set; } = new();
}
```
- **Rôle** : Représente un artiste musical
- **Relations** : Contient une liste d'albums
- **Données** : Nom, photo, genre, biographie

#### `Album.cs`
```csharp
public class Album
{
    public string Title { get; set; } = "";
    public int Year { get; set; }
    public string CoverUrl { get; set; } = "";
    public List<Song> Songs { get; set; } = new();
}
```
- **Rôle** : Représente un album musical
- **Relations** : Appartient à un artiste, contient des chansons
- **Données** : Titre, année, pochette

#### `Song.cs`
```csharp
public class Song
{
    public string Title { get; set; } = "";
    public string YoutubeUrl { get; set; } = "";
}
```
- **Rôle** : Représente une chanson
- **Relations** : Appartient à un album
- **Données** : Titre, URL YouTube

### ViewModels
Les ViewModels font le lien entre les modèles et les vues, gèrent la logique métier et implémentent `INotifyPropertyChanged` pour la liaison de données.

#### `ArtistsViewModel.cs`
**Responsabilités principales :**
- Gestion de la liste des artistes
- Filtrage par nom, genre et année
- Navigation vers les détails d'un artiste
- Chargement des données de test

**Fonctionnalités clés :**
```csharp
// Propriétés pour le filtrage
public string SearchText { get; set; }
public string GenreFilter { get; set; }
public string YearFilter { get; set; }

// Collections observables pour la liaison de données
public ObservableCollection<Artist> Artists { get; } = new();
public ObservableCollection<Artist> FilteredArtists { get; } = new();

// Commande pour la navigation
public ICommand SelectArtistCommand { get; }
```

**Logique de filtrage :**
```csharp
void ApplyFilter()
{
    var q = Artists.AsEnumerable();
    
    // Filtre par nom
    if (!string.IsNullOrWhiteSpace(SearchText))
        q = q.Where(a => a.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
    
    // Filtre par genre
    if (!string.Equals(GenreFilter, "All", StringComparison.OrdinalIgnoreCase))
        q = q.Where(a => string.Equals(a.Genre, GenreFilter, StringComparison.OrdinalIgnoreCase));
    
    // Mise à jour de la collection filtrée
    ReplaceCollection(FilteredArtists, q);
}
```

#### `ArtistDetailViewModel.cs`
**Responsabilités :**
- Affichage des détails d'un artiste sélectionné
- Gestion de la liste des albums de l'artiste
- Navigation vers les chansons d'un album

**Navigation entre pages :**
```csharp
ViewSongsCommand = new Command<Album>(async album =>
{
    await Shell.Current.GoToAsync("songs", new Dictionary<string, object>
    {
        { "Album", album },
        { "ArtistName", Artist?.Name ?? "Artiste" }
    });
});
```

#### `SongsViewModel.cs`
**Responsabilités :**
- Affichage des chansons d'un album
- Lecture des chansons via YouTube
- Gestion des erreurs de navigation

**Lecture des chansons :**
```csharp
PlaySongCommand = new Command<string>(async url =>
{
    try
    {
        // Trouver le titre de la chanson
        var song = Songs.FirstOrDefault(s => s.YoutubeUrl == url);
        if (song != null)
        {
            CurrentSongTitle = $"🎵 {song.Title} - Ouvert dans YouTube";
        }
        
        // Ouvrir directement dans le navigateur
        await Launcher.OpenAsync(url);
    }
    catch (Exception ex)
    {
        CurrentSongTitle = "❌ Erreur d'ouverture";
    }
});
```

#### `VideoPlayerViewModel.cs`
**Responsabilités :**
- Conversion des URLs YouTube en URLs d'embed
- Gestion de la lecture vidéo intégrée

**Conversion d'URL :**
```csharp
private string ConvertToEmbedUrl(string youtubeUrl)
{
    var videoId = ExtractVideoId(youtubeUrl);
    if (!string.IsNullOrEmpty(videoId))
    {
        return $"https://www.youtube.com/embed/{videoId}?autoplay=1&rel=0&modestbranding=1";
    }
    return youtubeUrl;
}
```

### Vues (Views)
Les vues définissent l'interface utilisateur en XAML et utilisent la liaison de données pour afficher les informations.

#### `ArtistsPage.xaml`
**Fonctionnalités :**
- Liste des artistes avec images
- Barre de recherche
- Filtres par genre et année
- Interface moderne avec cartes

**Liaison de données :**
```xml
<ContentPage.BindingContext>
    <viewmodels:ArtistsViewModel />
</ContentPage.BindingContext>

<SearchBar Text="{Binding SearchText}" />
<Picker ItemsSource="{Binding Genres}" SelectedItem="{Binding GenreFilter}" />
<CollectionView ItemsSource="{Binding FilteredArtists}" />
```

#### `SongsPage.xaml`
**Fonctionnalités :**
- Affichage des chansons d'un album
- Boutons de lecture YouTube
- Interface élégante avec informations d'album

**Template de chanson :**
```xml
<DataTemplate x:DataType="models:Song">
    <Frame CornerRadius="15" HasShadow="True">
        <Grid ColumnDefinitions="Auto,*,Auto">
            <Label Text="🎵" />
            <Label Text="{Binding Title}" />
            <Button Command="{Binding Source={RelativeSource AncestorType={x:Type ContentPage}}, 
                            Path=BindingContext.PlaySongCommand}"
                    CommandParameter="{Binding YoutubeUrl}" />
        </Grid>
    </Frame>
</DataTemplate>
```

## Flux de données et navigation

### 1. Navigation entre pages
```
ArtistsPage → ArtistDetailPage → SongsPage
     ↓              ↓              ↓
ArtistsViewModel → ArtistDetailViewModel → SongsViewModel
```

### 2. Passage de données
- **Shell Navigation** : Utilisation de `Dictionary<string, object>` pour passer des objets entre pages
- **Query Attributes** : Implémentation de `IQueryAttributable` dans les ViewModels
- **Binding Context** : Liaison automatique des ViewModels aux vues

## Avantages de l'architecture MVVM

### Séparation des responsabilités
- **Models** : Données et logique métier
- **Views** : Interface utilisateur uniquement
- **ViewModels** : Logique de présentation et liaison

### Testabilité
- ViewModels testables indépendamment
- Logique métier isolée de l'interface
- Mocking facile des dépendances

### Maintenabilité
- Code organisé et structuré
- Réutilisabilité des composants
- Évolutivité facilitée


## Fonctionnalités de l'application

- **Navigation fluide** entre artistes, albums et chansons
- **Filtrage avancé** par nom, genre et année
- **Lecture YouTube** intégrée
- **Interface moderne** avec animations et ombres
- **Données de test** pré-chargées