using System.IO;
using System;
using System.Media;
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
    public partial class Form1 : Form
    {
        string filePath = @"M:\00000000000000מחשבים מתוגבר שנה ב\אישי\יהודית\chat\YudaChat.txt";
        int lastReadLines = 0;
        public static string CurrentUser = "";
        public Form1()
        {

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timerRefresh.Start();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMessage.Text)) return;
            try
            {
                // יצירת שורת הודעה: [זמן] שם המחשב: ההודעה
                string msg = $"[{DateTime.Now:HH:mm}] {CurrentUser}: {txtMessage.Text}{Environment.NewLine}";
                File.AppendAllText(filePath, msg);
                txtMessage.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("שגיאה בכתיבה לקובץ: " + ex.Message);
            }
        }
        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            if (!File.Exists(filePath)) return;
            try
            {
                string[] allLines = File.ReadAllLines(filePath);
                if (allLines.Length > lastReadLines)
                {
                    for (int i = lastReadLines; i < allLines.Length; i++)
                    {
                        string line = allLines[i];
                        if (line.Length < 10)
                            continue;
                        string time = line.Substring(1, 5);
                        string rest = line.Substring(8);
                        int colonIndex = rest.IndexOf(':');
                        if (colonIndex == -1)
                            continue;
                        string sender1 =
                            rest.Substring(0, colonIndex).Trim();
                        string message =
                            rest.Substring(colonIndex + 1).Trim();
                        // אם זו הודעה ממחשב אחר
                        if (sender1 != CurrentUser)
                        {
                            SystemSounds.Beep.Play();
                            this.WindowState =
                                FormWindowState.Normal;
                            this.Show();
                            this.TopMost = true;
                            this.Activate();
                            this.TopMost = false;
                        }
                        AddMessageBubble(sender1, message, time);
                    }
                    lastReadLines = allLines.Length;
                }
            }
            catch
            {
                // אם הקובץ תפוס, פשוט ננסה שוב בטיק הבא
            }
        }
        private void AddMessageBubble(string sender, string text, string time)
        {
            // קונטיינר של כל השורה
            Panel rowPanel = new Panel();
            rowPanel.Width = chatPanel.Width - 25;
            rowPanel.Height = 60;
            rowPanel.BackColor = Color.Transparent;
            // הבועה עצמה
            Label lbl = new Label();
            lbl.AutoSize = true;
            lbl.MaximumSize = new Size(250, 0);
            lbl.Padding = new Padding(5);
            lbl.Margin = new Padding(2);

            lbl.Font = new Font("Segoe UI Emoji", 8);
            lbl.Text = text + "\n" + time;
            // הודעה שלי
            if (sender == CurrentUser)
            {
                lbl.BackColor = Color.LightBlue;
                // יישור לימין
                lbl.Left = rowPanel.Width - lbl.PreferredWidth - 25;
            }
            else
            {
                lbl.BackColor = Color.WhiteSmoke;
                // יישור לשמאל
                lbl.Left = 10;
            }
            lbl.Top = 5;
            // גובה דינמי לפי הטקסט
            lbl.Height = lbl.PreferredHeight;
            rowPanel.Height = lbl.Height + 10;
            rowPanel.Controls.Add(lbl);
            chatPanel.Controls.Add(rowPanel);
            chatPanel.ScrollControlIntoView(rowPanel);
        }

        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSend.PerformClick();

                e.SuppressKeyPress = true;
            }
        }
    }
}