using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Virtual_world.GameWorld.Environment;
using Virtual_world.GameWorld.Graphics;

namespace Virtual_world.ViewModels
{
    public partial class StartViewModel(DisplayWorld displayWorld) : ObservableObject
    {
        [ObservableProperty]
        public partial int WorldSize { get; set; } = 20;

        [RelayCommand]
        public async Task StartGame()
        {
            new World(WorldSize, displayWorld);
            await Shell.Current.GoToAsync("GamePage");
        }
    }
}
