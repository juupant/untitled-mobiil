namespace kolmas_view
{
    public partial class MainPage : ContentPage
    {
        private double hunger = 1.0;
        private double fun = 1.0;
        private readonly double decreaseRate = 0.1;
        private readonly double increaseAmount = 0.2;
        private IDispatcherTimer timer;
        private const string HungerKey = "creature_hunger";
        private const string FunKey = "creature_fun";

        public MainPage()
        {
            InitializeComponent();
            LoadGameState();
            StartDecreaseTimers();
        }

        private void LoadGameState()
        {
            hunger = Preferences.Default.Get(HungerKey, 1.0);
            fun = Preferences.Default.Get(FunKey, 1.0);
            UpdateUI();
        }

        private void SaveGameState()
        {
            Preferences.Default.Set(HungerKey, hunger);
            Preferences.Default.Set(FunKey, fun);
        }

        private void StartDecreaseTimers()
        {
            timer = Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += (s, e) => DecreaseStats();
            timer.Start();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            timer?.Stop();
            SaveGameState();
        }

        private void DecreaseStats()
        {
            hunger = Math.Max(0, hunger - decreaseRate);
            fun = Math.Max(0, fun - decreaseRate);
            UpdateUI();
            SaveGameState();
        }

        private void UpdateUI()
        {
            HungerBar.Progress = hunger;
            FunBar.Progress = fun;
        }

        private void OnFeedClicked(object sender, EventArgs e)
        {
            hunger = Math.Min(1.0, hunger + increaseAmount);
            UpdateUI();
            SaveGameState();
        }

        private void OnPlayClicked(object sender, EventArgs e)
        {
            fun = Math.Min(1.0, fun + increaseAmount);
            UpdateUI();
            SaveGameState();
        }
    }
}
