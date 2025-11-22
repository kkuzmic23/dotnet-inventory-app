using SIR.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIR
{
    public partial class frmLogin : Form
    {
        User kkuzmic;
        User tlevanic;
        User tracic;
        List<User> users;
        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            kkuzmic = new User("Karlo", "Kuzmic", "kkuzmic", "pw123");
            tlevanic = new User("Tibor", "Levanic", "tlevanic", "pw123");
            tracic = new User("Tin", "Racic", "tracic", "pw123");
            users = new List<User>();
            users.Add(kkuzmic); users.Add(tlevanic); users.Add(tracic);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text != "" && txtPassword.Text != "")
            {
                User found = users.FirstOrDefault(user => user.Username == txtUsername.Text && user.Password == txtPassword.Text);

                if (found != null)
                {
                    MessageBox.Show("Login successful!");
                }
                else
                {
                    MessageBox.Show("Invalid login info.");
                }
            }
            else
            {
                MessageBox.Show("Enter username and password.");
            }
        }
    }
}
