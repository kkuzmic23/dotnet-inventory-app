using BusinessLogicLayer;
using System.Windows;

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

    }
}
