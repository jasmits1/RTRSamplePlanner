using MauiReactor;
using RTRSamplePlanner.Data;
using RTRSamplePlanner.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
        [Inject]
        PlannerDatabase db;

        protected override async void OnMounted()
        {
            base.OnMounted();
            db = new PlannerDatabase();
            List<PlannerEvent> l = await db.GetAllEventsAsync();
        }

        public override VisualNode Render()
            => new Shell()
            {
                new FlyoutItem("All Events")
                {
                    new ShellContent()
                        .RenderContent(() => new FullList()),
                },

                new FlyoutItem("Today")
                {
                    new ShellContent()
                        .RenderContent(() => new ContentPage("Page 2"))
                }
            };
    }
}
