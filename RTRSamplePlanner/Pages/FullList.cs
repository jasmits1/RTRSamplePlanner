using MauiReactor;

namespace RTRSamplePlanner.Pages
{
    partial class FullList : Component
    {
        public override VisualNode Render()
        {
            return new ContentPage("All Events")
             {
                new Label("Label")
            };
        }
    }
}
