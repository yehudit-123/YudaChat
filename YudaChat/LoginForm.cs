using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YudaChat
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        string usersFile = @"M:\00000000000000מחשבים מתוגבר שנה ב\אישי\יהודית\chat\Users.txt";

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!File.Exists(usersFile))
            {
                MessageBox.Show("קובץ משתמשים לא נמצא");
                return;
            }

            string[] users = File.ReadAllLines(usersFile);

            foreach (string user in users)
            {
                string[] parts = user.Split('|');

                if (parts.Length < 2)
                    continue;

                string username = parts[0];
                string password = parts[1];

                if (txtUsername.Text == username &&
                    txtPassword.Text == password)
                {
                    Form1.CurrentUser = username;

                    Form1 chat = new Form1();

                    chat.Show();

                    this.Hide();

                    return;
                }
            }

            MessageBox.Show("שם משתמש או סיסמה שגויים");
        }

      
    }
}
