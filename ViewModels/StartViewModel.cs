using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Virtual_world.GameWorld.Environment;

namespace Virtual_world.ViewModels
{
    public partial class StartViewModel() : ObservableObject
    {
        [ObservableProperty]
        int _worldSizeX = 20;

        [ObservableProperty]
        int _WorldSizeY = 20;

        [RelayCommand]
        public async Task StartGame()
        {
            await Shell.Current.GoToAsync("GamePage");
            new World(WorldSizeX, WorldSizeY);
        }
    }
}
