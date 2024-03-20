using MauiReactor;

namespace RTRSamplePlanner.Pages
{

    enum PageState
    {
        Home,
        List,
        New,
    }
    internal class MainPageState
    {
        public PageState CurrentPage { get; set; }
    }

    partial class MainPage : Component<MainPageState>
    {

        private MauiControls.Shell? _shell;

        protected override void OnMounted()
        {
            Routing.RegisterRoute<EditEventPage>();
            base.OnMounted();
        }

        private void UpdateStatusBarAppearance()
        {
#if ANDROID
    MainActivity.SetWindowTheme(false);
#endif
        }

        public override VisualNode Render()
            => Shell(shell => _shell = shell,
                new FlyoutItem("Today's Events")
                {
                    new ShellContent()
                        .RenderContent(
                            () => new TodayPage()
                                .OnCreateEvent(OnAddEvent)
                        )
                },
                new FlyoutItem("All Events")
                {
                        new ShellContent()
                            .RenderContent(
                                () => new FullList()
                                    .OnCreateEvent(OnAddEvent)
                                    .OnEditEvent(eventId => OnEditEvent(eventId))
                            )
                }
          
            )    
            .OnAppearing(UpdateStatusBarAppearance);

        private async void OnAddEvent()
        {
            await _shell!.GoToAsync<EditEventPage>();
        }

        private async void OnEditEvent(int eventId)
        {    
            await _shell!.GoToAsync<EditEventPage, EditEventProps>(props => props.EventId = eventId);
            

        }
    }
}
