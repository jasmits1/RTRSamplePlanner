using MauiReactor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RTRSamplePlanner.Data;
using RTRSamplePlanner.Pages;


namespace RTRSamplePlanner
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiReactorApp<MainPage>(app =>
                {
                    app.AddResource("Resources/Styles/Colors.xaml");
                    app.AddResource("Resources/Styles/Styles.xaml");
                })
#if DEBUG
                .EnableMauiReactorHotReload()
#endif
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-SemiBold.ttf", "OpenSansSemiBold");
                });
            builder.Services.AddSingleton<MainPage>();

            builder.Services.AddSingleton<PlannerDatabase>();
#if DEBUG
        		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
