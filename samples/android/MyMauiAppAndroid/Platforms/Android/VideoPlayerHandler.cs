using Android.Content;
using Microsoft.Maui.Handlers;
// Removed unnecessary using directive to avoid namespace ambiguity
using Android.Views;
using Com.Google.Android.Exoplayer2;
using Com.Google.Android.Exoplayer2.UI; // For MediaItem
using Com.Google.Android.Exoplayer2.Source;
using CommunityToolkit.Mvvm.Messaging;
using Com.Google.Android.Exoplayer2.Upstream;
using Com.Google.Android.Exoplayer2.Source.Dash;
using Android.Net;

namespace MyMauiAppAndroid.Platforms.Android
{
    public class VideoPlayerHandler : ViewHandler<VideoPlayerView, PlayerView>
    {
        private SimpleExoPlayer? _player;

        public VideoPlayerHandler() : base(ViewMapper)
        {
        }

        protected override PlayerView CreatePlatformView()
        {
            var playerView = new PlayerView(Context);
            InitializePlayer(playerView);
            SubscribeToMessages();
            return playerView;
        }

        private void InitializePlayer(PlayerView playerView)
        {
            _player = new SimpleExoPlayer.Builder(Context).Build();
            playerView.Player = _player;

            // var mediaSource = new ProgressiveMediaSource.Factory(new DefaultDataSourceFactory(Context, "MyMauiAppAndroid"))
            //    .CreateMediaSource(MediaItem.FromUri(global::Android.Net.Uri.Parse("https://turtle-tube.appspot.com/t/t2/dash.mpd")));
            var dataSourceFactory = new DefaultDataSourceFactory(Context, "MyMauiAppAndroid");
    var dashMediaSourceFactory = new DashMediaSource.Factory(dataSourceFactory);
    var mediaSource = dashMediaSourceFactory.CreateMediaSource(MediaItem.FromUri(global::Android.Net.Uri.Parse("https://turtle-tube.appspot.com/t/t2/dash.mpd")));

            _player.Prepare(mediaSource);
            _player.PlayWhenReady = true;
        }

        private void SubscribeToMessages()
        {
            WeakReferenceMessenger.Default.Register<PlayVideoMessage>(this, (r, message) =>
            {
                _player.PlayWhenReady = message.Value;
            });

            WeakReferenceMessenger.Default.Register<PauseVideoMessage>(this, (r, message) =>
            {
                _player.PlayWhenReady = !message.Value;
            });
        }

        protected override void DisconnectHandler(PlayerView platformView)
        {
            WeakReferenceMessenger.Default.Unregister<PlayVideoMessage>(this);
            WeakReferenceMessenger.Default.Unregister<PauseVideoMessage>(this);

            _player.Release();
            _player = null;
        }
    }
}