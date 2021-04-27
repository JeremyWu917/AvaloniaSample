using Avalonia.MusicStore.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Avalonia.MusicStore.ViewModels
{
    public class MusicStoreViewModel : ViewModelBase
    {

        public MusicStoreViewModel()
        {
            BuyMusicCommand = ReactiveCommand.Create(() =>
            {
                return SelectedAlbum;
            });

            this.WhenAnyValue(x => x.SearchText).Where(x => !string.IsNullOrWhiteSpace(x)).Throttle(TimeSpan.FromMilliseconds(400)).ObserveOn(RxApp.MainThreadScheduler).Subscribe(DoSearch!);

            //BuyMusicCommand = ReactiveCommand.CreateFromTask(async () =>
            //{
            //    var store = new MusicStoreViewModel();

            //    var result = await ShowDialog.Handle(store);
            //});
        }

        private CancellationTokenSource? _CancellationTokenSource;

        private async void DoSearch(string s)
        {
            IsBusy = true;
            SearchResults.Clear();

            _CancellationTokenSource?.Cancel();
            _CancellationTokenSource = new CancellationTokenSource();

            var albums = await Album.SearchAsync(s);
            foreach (var album in albums)
            {
                var vm = new AlbumViewModel(album);
                SearchResults.Add(vm);
            }
            IsBusy = false;

            if (!_CancellationTokenSource.IsCancellationRequested)
            {
                LoadCovers(_CancellationTokenSource.Token);
            }
        }

        private async void LoadCovers(CancellationToken cancellationToken)
        {
            foreach (var album in SearchResults.ToList())
            {
                await album.LoadCover();

                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
            }
        }

        //选择的专辑集合
        public ObservableCollection<AlbumViewModel> SearchResults { get; } = new();

        //选中的专辑
        private AlbumViewModel? _SelectedAlbum;

        public AlbumViewModel? SelectedAlbum
        {
            get => _SelectedAlbum;
            set => this.RaiseAndSetIfChanged(ref _SelectedAlbum, value);
        }

        //搜索文字
        private string? _SearchText;

        public string? SearchText
        {
            get => _SearchText;
            set => this.RaiseAndSetIfChanged(ref _SearchText, value);
        }

        //状态进度条指示
        private bool _IsBusy;

        public bool IsBusy
        {
            get => _IsBusy;
            set => this.RaiseAndSetIfChanged(ref _IsBusy, value);
        }

        public ReactiveCommand<Unit, AlbumViewModel?> BuyMusicCommand { get; }
    }
}
