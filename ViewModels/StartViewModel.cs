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
        public partial int WorldSize { get; set; } = 20;

        [RelayCommand]
        public async Task StartGame()
        {
            await Shell.Current.GoToAsync("GamePage");
            new World(WorldSize);
        }
    }
}
