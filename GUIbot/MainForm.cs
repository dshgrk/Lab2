namespace GUIbot
{
    public partial class MainForm : Form
    {
        Host? bot = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private void OnRunClick(object sender, EventArgs e)
        {
            if (cbBotRun.Checked)
            {
                bot = new Host();
                _ = bot.StartAsync();
            }
            else
            {
                bot?.Cancel();
                bot = null;
            }
        }
    }
}
