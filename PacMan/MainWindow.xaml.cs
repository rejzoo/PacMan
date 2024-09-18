using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using PacMan.Gui;
using PacMan.Maps;

namespace PacMan
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainGame? _mainGame;
        private readonly MainGUI? _mainGUI;
        private readonly Map? _mainMap;

        public MainWindow()
        {
            var spriteSheet = new BitmapImage(new Uri("../../../Properties/spriteSheet.png", UriKind.Relative));

            InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;

            _mainMap = new Map(spriteSheet);
            _canvasGame.Children.Add(_mainMap);

            _mainGUI = new MainGUI(spriteSheet);
            _canvasLetters.Children.Add(_mainGUI);

            _mainGame = new MainGame(spriteSheet, _mainMap, _mainGUI);
            _canvasGame.Children.Add(_mainGame);
            _canvasGame.Focus();
        }

        private void CanvasGame_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                MenuButton_Click(sender, e);
                return;
            }

            _mainGame!.KeyBoardInput(e);
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            _mainGame!.StopGame();
            CreateMenu();
            _mainGame!.StartGame();
            _canvasGame.Focus();
        }

        private void CreateMenu()
        {
            PauseMenu pauseMenu = new()
            {
                WindowStartupLocation = WindowStartupLocation.Manual
            };
            pauseMenu.Left = this.Left + (this.Width - pauseMenu.Width) / 2;
            pauseMenu.Top = this.Top + (this.Height - pauseMenu.Height) / 2;
            pauseMenu.ShowDialog();

            if (pauseMenu.DialogResult == true)
            {
                _mainGUI!.SaveHighScore();
                Application.Current.Shutdown();
            }
        }
    }
}
