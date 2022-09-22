using System.Diagnostics;
using System.Security;
using System.Windows.Forms;

namespace EasyGMMLInstaller
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            OpenFileDialogForm();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private Button selectButton;

        public void OpenFileDialogForm()
        {
            

            selectButton = new Button()
            {
                Size = new Size(100, 20),
                Location = new Point(15, 15),
                Text = "Select will you snail directory"
            };
            selectButton.Click += new EventHandler(selectButton_Click);
            Controls.Add(selectButton);
        }

        private void selectButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
            // This is what will execute if the user selects a folder and hits OK (File if you change to FileBrowserDialog)
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string folder = dlg.SelectedPath;
            }
            else
            {
                // This prevents a crash when you close out of the window with nothing
            }
        }
    }
}