using BusinessLogicLayer;
using System.Windows;
using System.Windows.Input;

namespace WPFLayer
{
    public partial class LoginWindow : Window
    {
        private readonly UserService userService;

        public LoginWindow()
        {
            InitializeComponent();
            userService = new UserService();
            txtUsername.Focus();
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Help, OnHelp));
            InputBindings.Add(new KeyBinding(ApplicationCommands.Help, new KeyGesture(Key.F1)));
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            lblText.Text = string.Empty;

            var username = txtUsername.Text?.Trim();
            var password = txtPassword.Password;

            var user = userService.Authenticate(username, password);
            if (user == null)
            {
                lblText.Text = "Invalid username or password.";
                txtPassword.Clear();
                txtPassword.Focus();
                return;
            }

            var mainWindow = new MainWindow();
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
            Close();
        }

        private void OnHelp(object sender, ExecutedRoutedEventArgs e)
        {
            HelpService.ShowHelpForContext(this);
        }

    }
}
