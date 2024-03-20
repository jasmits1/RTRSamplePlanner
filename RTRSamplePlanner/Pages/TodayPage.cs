using MauiReactor;
using ReactorData;
using RTRSamplePlanner.Model;
using RTRSamplePlanner.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTRSamplePlanner.Pages
{
    class TodayPageState
    {
        public IQuery<PlannerEvent> TodaysEvents { get; set; } = default!;
    }

    partial class TodayPage : Component<TodayPageState>
    {
        [Inject]
        IModelContext _modelContext;

        [Prop]
        Action _onCreateEvent;

        protected override void OnMounted()
        {
            _modelContext.Load<PlannerEvent>();

            base.OnMounted();
        }

        protected override void OnMountedOrPropsChanged()
        {
            State.TodaysEvents = _modelContext.Query<PlannerEvent>(query =>
                query
                    .Where(_ => _.Date.Date == DateTime.Today.Date)
                    .OrderBy(_ => _.Date));
            base.OnMounted();
        }

        public override VisualNode Render()
        {
            return new ContentPage("Today")
            {
                RenderBody()
            };
        }

        Grid RenderBody()
        {
            return new Grid("40, *, 24, Auto, Auto", "*")
            {
                Button("Add Event")
                    .OnClicked(_onCreateEvent)
                    .HFill(),
                CollectionView()
                    .ItemsSource(State.TodaysEvents, RenderEvents)
                    .GridRow(1)
            };
        }

        public VisualNode RenderEvents(PlannerEvent plannerEvent)
            => SwipeView(
                    VStack(spacing: 5,
                            Label($"{plannerEvent.Name}"),
                            Label($"{plannerEvent.Description}"),
                            Label($"{plannerEvent.Location}"),
                            Label($"{plannerEvent.Date.ToString()}")
                        )
                .VCenter()
                )
            .HeightRequest(100);
    }
}
