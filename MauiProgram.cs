using Microsoft.Extensions.Logging;
using Virtual_world.GameWorld.Environment;
using Virtual_world.GameWorld.Graphics;
using Virtual_world.ViewModels;

namespace Virtual_world
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddTransient<DisplayWorld>();
            builder.Services.AddTransient<StartViewModel>();

            return builder.Build();
        }
    }
}
