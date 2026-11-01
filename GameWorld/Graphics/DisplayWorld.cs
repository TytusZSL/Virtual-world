using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Virtual_world.GameWorld.Environment;

namespace Virtual_world.GameWorld.Graphics
{
    public partial class DisplayWorld : ObservableObject
    {
        public DisplayWorld()
        {
            Singleton = this;
        }

        public static DisplayWorld Singleton = new DisplayWorld();

        public ObservableCollection<ObservableCollection<DisplayableTile>> TileRows { get; } = new();
        public ObservableCollection<NarratorEntry> NarratorMessages { get; } = new();

        private int _oldNarratorMessagesLength = 0;
        private List<NarratorEntry>? _narratorMessagesSource;

        [ObservableProperty]
        public partial bool ShowDeathMessages { get; set; } = true;

        [ObservableProperty]
        public partial bool ShowFightMessages { get; set; } = true;

        [ObservableProperty]
        public partial bool ShowConsumeMessages { get; set; } = true;

        [ObservableProperty]
        public partial bool ShowBreedMessages { get; set; } = true;

        [ObservableProperty]
        public partial bool ShowWorldMessages { get; set; } = true;

        partial void OnShowDeathMessagesChanged(bool value) => RefreshNarratorMessages();
        partial void OnShowFightMessagesChanged(bool value) => RefreshNarratorMessages();
        partial void OnShowConsumeMessagesChanged(bool value) => RefreshNarratorMessages();
        partial void OnShowBreedMessagesChanged(bool value) => RefreshNarratorMessages();
        partial void OnShowWorldMessagesChanged(bool value) => RefreshNarratorMessages();

        public void InitializeTilesCollection()
        {
            var world = World.Singleton;

            TileRows.Clear();
            for (int x = 0; x < world.Size; x++)
            {
                var row = new ObservableCollection<DisplayableTile>();
                for (int y = 0; y < world.Size; y++)
                {
                    row.Add(new(world.TileSet[x, y]));
                }
                TileRows.Add(row);
            }
        }

        public void Update()
        {
            var world = World.Singleton;

            for (int x = 0; x < world.Size; x++)
            {
                for (int y = 0; y < world.Size; y++)
                {
                    TileRows[x][y].UpdateTile(world.TileSet[x, y]);
                }
            }

            var narratorMessages = world.NarratorMessages;
            if (!ReferenceEquals(_narratorMessagesSource, narratorMessages)
                || _oldNarratorMessagesLength > narratorMessages.Count)
            {
                NarratorMessages.Clear();
                _oldNarratorMessagesLength = 0;
                _narratorMessagesSource = narratorMessages;
            }

            for (int i = _oldNarratorMessagesLength; i < narratorMessages.Count; i++)
            {
                var entry = narratorMessages[i];
                if (IsNarratorTypeEnabled(entry.Type))
                {
                    NarratorMessages.Add(entry);
                }
            }
            _oldNarratorMessagesLength = narratorMessages.Count;
        }

        private void RefreshNarratorMessages()
        {
            var narratorMessages = World.Singleton.NarratorMessages;
            NarratorMessages.Clear();
            foreach (var entry in narratorMessages)
            {
                if (IsNarratorTypeEnabled(entry.Type))
                {
                    NarratorMessages.Add(entry);
                }
            }

            _narratorMessagesSource = narratorMessages;
            _oldNarratorMessagesLength = narratorMessages.Count;
        }

        private bool IsNarratorTypeEnabled(EntryType type) => type switch
        {
            EntryType.Death => ShowDeathMessages,
            EntryType.Fight => ShowFightMessages,
            EntryType.Consume => ShowConsumeMessages,
            EntryType.Breed => ShowBreedMessages,
            EntryType.World => ShowWorldMessages,
            _ => false
        };

        [RelayCommand]
        public void DoTurn()
        {
            World.Singleton.DoTurn();
        }
    }
}
