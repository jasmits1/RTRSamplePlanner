using MauiReactor;
using Microsoft.Maui.Devices;
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
    class EditEventPageState
    {
        public PlannerEvent PlannerEvent { get; set; } = default!;

        public IQuery<PlannerEvent> PlannerEvents { get; set; } = default!;

        public DateTime SelectedDate { get; set; } = DateTime.Now;

        public TimeSpan SelectedTime { get; set; } = DateTime.Now.TimeOfDay;

        public bool IsEditing { get; set; } = false;

        public IModelContext ScopedContext { get; set; } = default!;
    }

    class EditEventProps
    {
        public int? EventId { get; set; }
    }

    partial class EditEventPage : Component<EditEventPageState, EditEventProps>
    {
        [Inject]
        IModelContext _modelContext;

        [Prop]
        int? _eventId;

        protected override void OnMountedOrPropsChanged()
        {
            State.ScopedContext = _modelContext.CreateScope();
            State.PlannerEvents = State.ScopedContext.Query<PlannerEvent>(query => query.OrderBy(_ => _.Id));
            State.PlannerEvent = new PlannerEvent();
            State.PlannerEvents.CollectionChanged += (sender, args) =>
            {
                if (args.NewItems?.Count == 1)
                {
                    var newEvent = (PlannerEvent)args.NewItems[0]!;
                    SetState(s =>
                    {
                        s.PlannerEvent = newEvent;
                        s.SelectedDate = newEvent.Date;
                        s.SelectedTime = newEvent.Date.TimeOfDay;
                    });
                }
            };

            if(Props.EventId != null)
            {
                State.IsEditing = true;
                State.ScopedContext.Load<PlannerEvent>(x => x.Where(_ => _.Id == Props.EventId));
                State.SelectedDate = State.PlannerEvent.Date;
                State.SelectedTime = State.PlannerEvent.Date.TimeOfDay;                
            }

            base.OnMountedOrPropsChanged();
        }

        public override VisualNode Render()
        {
            return new ContentPage("Event")
            {
                RenderBody()
            }
            .Set(MauiControls.Shell.NavBarIsVisibleProperty, false);
        }

        Grid RenderBody()
        {
            return new Grid("108, *, 24, Auto, Auto", "*")
            {
                RenderTop(),
                RenderEventFields(OnSaveClicked)
                    .GridRow(1)
            }
            .BackgroundColor(Theme.Current.WhiteColor);
        }

        Grid RenderTop()
        => Grid("108", "64 * 48 64",

            DeviceInfo.Idiom == DeviceIdiom.Phone ?

            Theme.Current.ImageButton("back_white.png")
                .Aspect(Aspect.Center)
                .HeightRequest(64)
                .BackgroundColor(Colors.Transparent)
                .OnClicked(OnBack)
                :null,

            Theme.Current.H3(State.PlannerEvent.Name ?? "New Card")
                .GridColumn(1)
                .VCenter()
                .HCenter()
        )
        .BackgroundColor(Theme.Current.BlackColor);

        Grid RenderEventFields(Action OnSave)
            => Grid("*, Auto", "*, Auto",
                VStack(spacing: 10,
                    Label("Event Name"),
                    Entry()
                        .Text(State.PlannerEvent.Name ?? "")
                        .OnTextChanged(text => State.PlannerEvent.Name = text),
                    Label("Description"),
                    Entry()
                        .Text(State.PlannerEvent.Description ?? "")
                        .OnTextChanged(text =>State.PlannerEvent.Description = text),
                    Label("Location"),
                    Entry()
                        .Text(State.PlannerEvent.Location ?? "")
                        .OnTextChanged(text => State.PlannerEvent.Location = text),
                    Label("Date"),
                    DatePicker()
                        .MinimumDate(DateTime.Today)
                        .Date(State.SelectedDate)
                        .OnDateSelected(date => State.SelectedDate = new DateTime(date.Year, date.Month, date.Day)),
                    Label("Time"),
                    TimePicker()
                        .Time(State.SelectedTime)
                        .OnTimeSelected(time => State.SelectedTime = time),
                    Button("Save")
                        .HStart()
                        .VStart()
                        .OnClicked(OnSave)
                    )
                );


        private async void OnSaveClicked()
        { 
            State.PlannerEvent.Date = new DateTime(
                State.SelectedDate.Year,
                State.SelectedDate.Month,
                State.SelectedDate.Day,
                State.SelectedTime.Hours,
                State.SelectedTime.Minutes,
                State.SelectedTime.Seconds
                );
            if (!State.IsEditing)
            {
                _modelContext.Add(State.PlannerEvent);
            }
            else
            {            
                _modelContext.Update(State.PlannerEvent);
            }
            _modelContext.Save();
            State.ScopedContext.Save();
            await State.ScopedContext.Flush();
            _modelContext.Load<PlannerEvent>(_ => _.Where(_ => _.Id == State.PlannerEvent.Id));
            
            OnBack();

        }

        async void OnBack()
        {
            if (Navigation == null)
            {
                return;
            }

            await Navigation.PopAsync();
        }
    }
}