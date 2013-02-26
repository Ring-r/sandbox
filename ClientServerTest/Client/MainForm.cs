using System.Windows.Forms;

namespace ClientServerTest
{
    public partial class MainForm : Form
    {
        private Client client = new Client();

        public MainForm()
        {
            InitializeComponent();
        }

        private void textBox_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            switch (e.KeyData)
            {
                case Keys.Enter:
                    this.client.CommandRun(textBox.Text);

                    switch (textBox.Text)
                    {
                        case "exit":
                            this.Close();
                            break;
                    }

                    textBox.Text = "";
                    break;
            }
        }
    }
}
