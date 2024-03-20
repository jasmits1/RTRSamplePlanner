using MauiReactor;
using ReactorData;
using RTRSamplePlanner.Model;
using RTRSamplePlanner.Resources;
using System;
using System.Linq;

namespace RTRSamplePlanner.Pages
{
    class FullListPageState
    {
        public IQuery<PlannerEvent> PlannerEvents { get; set; } = default!;
    }
    partial class FullList : Component<FullListPageState>
    {
        [Inject]
        IModelContext _modelContext;

        [Prop]
        Action _onCreateEvent;

        [Prop]
        Action<int> _onEditEvent;

        protected override void OnMounted()
        {
            _modelContext.Load<PlannerEvent>();
           
            base.OnMounted();
        }

        protected override void OnMountedOrPropsChanged()
        {
            State.PlannerEvents = _modelContext.Query<PlannerEvent>(query => query.OrderBy(_ => _.Date));

            base.OnMounted();
        }
        public override VisualNode Render()
        {
            return new ContentPage("All Events")
             {                
                VStack(spacing: 5,

                    CollectionView()
                        .ItemsSource(State.PlannerEvents, RenderEvent),
                    Button("Add Event")
                        .HFill()
                        .HCenter()
                        .VEnd()
                        .OnClicked(_onCreateEvent)
               )
            };
        }

        public VisualNode RenderEvent(PlannerEvent plannerEvent)
            => SwipeView(
                    VStack(spacing: 5,
                            Label($"{plannerEvent.Name}"),
                            Label($"{plannerEvent.Description}"),
                            Label($"{plannerEvent.Location}"),
                            Label($"{plannerEvent.Date.ToString()}")
                        )
                .VCenter()    
                )
            .LeftItems(new SwipeItems
            {
                new SwipeItem()
                    .IconImageSource("delete_white")
                    .OnInvoked(() => DeleteEvent(plannerEvent))
                    .BackgroundColor(Colors.Red),
                new SwipeItem()
                    .IconImageSource("edit_white")
                    .OnInvoked(()=>_onEditEvent?.Invoke(plannerEvent.Id))
                    .BackgroundColor(Theme.Current.BlackColor)
            })
            .HeightRequest(100);
        
        private void DeleteEvent(PlannerEvent plannerEvent)
        {
            _modelContext.Delete(plannerEvent);
            _modelContext.Save();
        }
    }
}
