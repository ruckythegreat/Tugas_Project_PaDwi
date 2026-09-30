namespace SMKRestaurant.Forms
{
    public partial class FrmAdmin : Form
    {
        public FrmAdmin()
        {
            InitializeComponent();
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            FrmMenu form = new FrmMenu();
            form.ShowDialog();
        }

        private void btnMember_Click(object sender, EventArgs e)
        {
            FrmMember form = new FrmMember();
            form.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}