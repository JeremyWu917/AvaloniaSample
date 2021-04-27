using Avalonia.Media.Imaging;
using Avalonia.MusicStore.Models;
using ReactiveUI;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avalonia.MusicStore.ViewModels
{
    public class AlbumViewModel : ViewModelBase
    {
        private readonly Album _Album;

        public AlbumViewModel(Album album)
        {
            _Album = album;
        }

        public string Artist => _Album.Artist;

        public string Title => _Album.Title;


        private Bitmap? _Cover;
        public Bitmap? Cover
        {
            get => _Cover;
            private set => this.RaiseAndSetIfChanged(ref _Cover, value);
        }
        public async Task LoadCover()
        {
            await using var imageStream = await _Album.LoadCoverBitmapAsync();
            Cover = await Task.Run(() => Bitmap.DecodeToWidth(imageStream, 400));
        }
        public static async Task<IEnumerable<AlbumViewModel>> LoadCached()
        {
            return (await Album.LoadCachedAsync()).Select(x => new AlbumViewModel(x));
        }
        public async Task SaveToDiskAsync()
        {
            await _Album.SaveAsync();
            if (Cover != null)
            {
                var bitmap = Cover;
                await Task.Run(() =>
                {
                    using var fs = _Album.SaveCoverBitmapSteam();
                    bitmap.Save(fs);
                });
            }
        }
    }
}
