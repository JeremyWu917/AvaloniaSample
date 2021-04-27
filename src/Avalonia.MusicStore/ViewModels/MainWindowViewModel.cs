using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows.Input;

namespace Avalonia.MusicStore.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        //public string Greeting => "Welcome to Avalonia!";
        public ICommand BuyMusicCommand { get; }
        public Interaction<MusicStoreViewModel, AlbumViewModel?> ShowDialog { get; }
        private bool _CollectionEmpty;
        public bool CollectionEmpty
        {
            get => _CollectionEmpty;
            set => this.RaiseAndSetIfChanged(ref _CollectionEmpty, value);
        }
        public ObservableCollection<AlbumViewModel> Albums { get; } = new();

        public MainWindowViewModel()
        {
            ShowDialog = new Interaction<MusicStoreViewModel, AlbumViewModel?>();
            
            BuyMusicCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                // Code here will be execute when the button is clicked.
                var store = new MusicStoreViewModel();

                var result = await ShowDialog.Handle(store);

                if (result != null)
                {
                    Albums.Add(result);
                    await result.SaveToDiskAsync();
                }                
            });
            this.WhenAnyValue(x => x.Albums.Count).Subscribe(x => CollectionEmpty = x == 0);
            RxApp.MainThreadScheduler.Schedule(LoadAlbums);
        }

        private async void LoadAlbums()
        {
            var albums = await AlbumViewModel.LoadCached();
            foreach (var album in albums)
            {
                Albums.Add(album);
            }
            LoadCovers();
        }

        private async void LoadCovers()
        {
            foreach (var album in Albums.ToList())
            {
                await album.LoadCover();
            }
        }
    }
}
