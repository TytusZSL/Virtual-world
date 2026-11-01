using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Globalization;
using Virtual_world.GameWorld.Creatures;
using Virtual_world.GameWorld.Creatures.Animals;
using Virtual_world.GameWorld.Creatures.Plants;
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
        public ObservableCollection<string> OrganismTypes { get; } = new()
        {
            "Wilk", "Owca", "Lis", "Mysz", "Trawa", "Mlecz", "Cierń"
        };

        private int? _selectedX;
        private int? _selectedY;

        [ObservableProperty]
        public partial bool HasSelection { get; set; }

        [ObservableProperty]
        public partial bool SelectedTileOccupied { get; set; }

        [ObservableProperty]
        public partial string SelectionMessage { get; set; } = "Kliknij pole świata, aby rozpocząć edycję.";

        [ObservableProperty]
        public partial string SelectionPositionText { get; set; } = "Nie wybrano pola.";

        [ObservableProperty]
        public partial string? SelectedSpecies { get; set; } = "Wilk";

        [ObservableProperty]
        public partial Organism? SelectedOrganism { get; set; }

        [ObservableProperty]
        public partial bool IsTrackingOrganism { get; set; }

        [ObservableProperty]
        public partial bool IsSelectedPlant { get; set; }

        [ObservableProperty]
        public partial bool IsSelectedAnimal { get; set; }

        [ObservableProperty]
        public partial bool CanEditSelectedTile { get; set; }

        [ObservableProperty]
        public partial bool CanAddOrganismToSelectedTile { get; set; }

        [ObservableProperty]
        public partial bool CanManageOrganismOnSelectedTile { get; set; }

        [ObservableProperty]
        public partial string EditName { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string EditAge { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string EditHealth { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string EditStrength { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string EditInitiative { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string EditAgility { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string EditSpeed { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string EditReproductionChance { get; set; } = string.Empty;

        partial void OnHasSelectionChanged(bool value) => RefreshSelectionDisplayState();
        partial void OnSelectedTileOccupiedChanged(bool value) => RefreshSelectionDisplayState();
        partial void OnSelectedOrganismChanged(Organism? value)
        {
            IsSelectedPlant = value is Plant;
            IsSelectedAnimal = value is Animal;
            RefreshSelectionDisplayState();
        }

        private void RefreshSelectionDisplayState()
        {
            IsTrackingOrganism = SelectedOrganism is not null;
            CanEditSelectedTile = HasSelection && !IsTrackingOrganism;
            CanAddOrganismToSelectedTile = CanEditSelectedTile && !SelectedTileOccupied;
            CanManageOrganismOnSelectedTile = CanEditSelectedTile && SelectedTileOccupied;
        }

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
            Update();
        }

        public void Update()
        {
            var world = World.Singleton;
            RefreshTrackedSelection(world);

            for (int x = 0; x < world.Size; x++)
            {
                for (int y = 0; y < world.Size; y++)
                {
                    var isSelected = HasSelection && _selectedX == x && _selectedY == y;
                    TileRows[x][y].UpdateTile(world.TileSet[x, y], isSelected);
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

        public void SelectTile(DisplayableTile displayableTile)
        {
            var world = World.Singleton;
            if (displayableTile.X < 0 || displayableTile.X >= world.Size
                || displayableTile.Y < 0 || displayableTile.Y >= world.Size)
                return;

            SelectedOrganism = null;
            SetSelectedPosition(displayableTile.X, displayableTile.Y);
            HasSelection = true;
            var organism = world.TileSet[displayableTile.X, displayableTile.Y].Content;
            SelectedTileOccupied = organism is not null;
            SelectionMessage = organism is null
                ? "Pole jest puste. Możesz dodać organizm."
                : $"Na polu znajduje się: {organism.Name}. Możesz go edytować lub usunąć.";
            Update();
        }

        [RelayCommand]
        public void EditOrganismOnSelectedTile()
        {
            if (!HasSelection || _selectedX is not int x || _selectedY is not int y)
                return;

            var organism = World.Singleton.TileSet[x, y].Content;
            if (organism is null)
            {
                SelectionMessage = "Wybrane pole jest puste.";
                return;
            }

            SelectedOrganism = organism;
            LoadEditableAttributes(organism);
            SelectionMessage = $"Śledzisz organizm: {organism.Name}.";
        }

        [RelayCommand]
        public void AddOrganismToSelectedTile()
        {
            if (!HasSelection || _selectedX is not int x || _selectedY is not int y || SelectedSpecies is null)
                return;

            Organism organism = SelectedSpecies switch
            {
                "Wilk" => new Wolf(x, y),
                "Owca" => new Sheep(x, y),
                "Lis" => new Fox(x, y),
                "Mysz" => new Mouse(x, y),
                "Trawa" => new Grass(x, y),
                "Mlecz" => new Dandelion(x, y),
                "Cierń" => new Thorn(x, y),
                _ => throw new InvalidOperationException($"Nieznany rodzaj organizmu: {SelectedSpecies}.")
            };

            if (!World.Singleton.AddOrganism(organism))
            {
                SelectionMessage = "Nie można dodać organizmu: wybrane pole nie jest puste.";
                return;
            }

            SelectionMessage = $"Dodano organizm: {organism.Name}.";
            Update();
        }

        [RelayCommand]
        public void RemoveOrganismFromSelectedTile()
        {
            if (!HasSelection || _selectedX is not int x || _selectedY is not int y)
                return;

            var world = World.Singleton;
            var organism = world.TileSet[x, y].Content;
            if (organism is null)
            {
                SelectionMessage = "Wybrane pole jest już puste.";
                return;
            }

            organism.MarkDead(world);
            SelectedOrganism = null;
            SelectionMessage = $"Usunięto organizm: {organism.Name}.";
            Update();
        }

        [RelayCommand]
        public void RemoveTrackedOrganism()
        {
            if (SelectedOrganism is not { } organism)
                return;

            organism.MarkDead(World.Singleton);
            SelectedOrganism = null;
            HasSelection = false;
            SetSelectedPosition(null, null);
            SelectionMessage = $"Usunięto organizm: {organism.Name}.";
            Update();
        }

        [RelayCommand]
        public void ReturnToTrackedOrganismTile()
        {
            if (SelectedOrganism is not { } organism)
                return;

            var x = organism.X;
            var y = organism.Y;
            SelectedOrganism = null;
            SetSelectedPosition(x, y);
            HasSelection = true;
            SelectionMessage = $"Śledzisz pole zajmowane przez: {organism.Name}.";
        }

        [RelayCommand]
        public void SaveOrganismChanges()
        {
            if (SelectedOrganism is not { } organism)
                return;

            if (!int.TryParse(EditAge, NumberStyles.Integer, CultureInfo.CurrentCulture, out var age)
                || !double.TryParse(EditHealth, NumberStyles.Float, CultureInfo.CurrentCulture, out var health)
                || !double.TryParse(EditStrength, NumberStyles.Float, CultureInfo.CurrentCulture, out var strength)
                || !int.TryParse(EditInitiative, NumberStyles.Integer, CultureInfo.CurrentCulture, out var initiative)
                || !int.TryParse(EditAgility, NumberStyles.Integer, CultureInfo.CurrentCulture, out var agility)
                || !double.TryParse(EditSpeed, NumberStyles.Float, CultureInfo.CurrentCulture, out var speed)
                || !double.TryParse(EditReproductionChance, NumberStyles.Float, CultureInfo.CurrentCulture, out var reproductionChance))
            {
                SelectionMessage = "Wprowadź poprawne wartości liczbowe we wszystkich polach.";
                return;
            }

            try
            {
                organism.UpdateEditableAttributes(EditName, age, health, strength, initiative, agility, speed, reproductionChance);
            }
            catch (ArgumentException exception)
            {
                SelectionMessage = exception.Message;
                return;
            }

            if (organism.IsAlive)
            {
                SelectionMessage = "Zapisano zmiany organizmu.";
                Update();
            }
            else
            {
                Update();
            }
        }

        private void LoadEditableAttributes(Organism organism)
        {
            EditName = organism.Name;
            EditAge = organism.Age.ToString();
            EditHealth = organism.Health.ToString();
            EditStrength = organism.Strength.ToString();
            EditInitiative = organism.Initiative.ToString();
            EditAgility = organism.Agility.ToString();
            EditSpeed = organism.Speed.ToString();
            EditReproductionChance = organism.ReproductionChance.ToString();
        }

        private void RefreshTrackedSelection(World world)
        {
            if (SelectedOrganism is not { } organism)
            {
                RefreshTrackedTile(world);
                return;
            }

            if (!organism.IsAlive
                || organism.X < 0 || organism.X >= world.Size
                || organism.Y < 0 || organism.Y >= world.Size
                || !ReferenceEquals(world.TileSet[organism.X, organism.Y].Content, organism))
            {
                SelectedOrganism = null;
                HasSelection = false;
                SetSelectedPosition(null, null);
                SelectionMessage = "Element, który został wybrany, przestał istnieć.";
                return;
            }

            if (_selectedX != organism.X || _selectedY != organism.Y)
                SetSelectedPosition(organism.X, organism.Y);

            LoadEditableAttributes(organism);
        }

        private void RefreshTrackedTile(World world)
        {
            if (!HasSelection || _selectedX is not int x || _selectedY is not int y)
                return;

            var organism = world.TileSet[x, y].Content;
            SelectedTileOccupied = organism is not null;
            SelectionMessage = organism is null
                ? "Pole jest puste. Możesz dodać organizm."
                : $"Na polu znajduje się: {organism.Name}. Możesz go edytować lub usunąć.";
        }

        private void SetSelectedPosition(int? x, int? y)
        {
            _selectedX = x;
            _selectedY = y;
            SelectionPositionText = x.HasValue && y.HasValue
                ? $"Pozycja (X, Y): ({x}, {y})"
                : "Nie wybrano pola.";
        }

        [RelayCommand]
        public void DoTurn()
        {
            World.Singleton.DoTurn();
        }
    }
}
