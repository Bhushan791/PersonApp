using System;
using System.Windows.Forms;
using PersonApp.core.services; 
using PersonApp.core.models;

namespace PersonApp.forms
{
    public class MainForm : Form
    {
        private DataGridView dataGridView;
        private TextBox searchBox;
        private Button addButton;
        private Button editButton;
        private Button deleteButton;
        private Button exportButton;

        private PersonService personService;

        public MainForm()
        {
            Text = "Person Management App";
            Width = 800;
            Height = 600;

            personService = new PersonService();

            InitializeComponents();
            LoadData();
        }

private void InitializeComponents()
{
    // ---------------------- Form Styling ----------------------
    this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245); // subtle light gray
    this.Font = new System.Drawing.Font("Segoe UI", 10);
    this.StartPosition = FormStartPosition.CenterScreen;
    this.Width = 820;
    this.Height = 580;

    // ---------------------- DataGridView Styling ----------------------
    dataGridView = new DataGridView
    {
        Width = 760,
        Height = 380,
        Top = 60,
        Left = 30,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        ReadOnly = true,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        BackgroundColor = System.Drawing.Color.White,
        BorderStyle = BorderStyle.None,
        EnableHeadersVisualStyles = false,
        ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = System.Drawing.Color.FromArgb(100, 149, 237), // Cornflower Blue
            ForeColor = System.Drawing.Color.White,
            Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold)
        },
        DefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = System.Drawing.Color.White,
            ForeColor = System.Drawing.Color.Black,
            SelectionBackColor = System.Drawing.Color.FromArgb(173, 216, 230), // light blue
            SelectionForeColor = System.Drawing.Color.Black
        },
        RowHeadersVisible = false,
        AllowUserToAddRows = false,
        AllowUserToDeleteRows = false,
        MultiSelect = false
    };

    // ---------------------- Search Box ----------------------
    searchBox = new TextBox
    {
        Width = 220,
        Top = 20,
        Left = 30,
        PlaceholderText = "Search...",
        BackColor = System.Drawing.Color.White,
        ForeColor = System.Drawing.Color.Black,
        BorderStyle = BorderStyle.FixedSingle
    };
    searchBox.TextChanged += (s, e) => dataGridView.DataSource = personService.Sort("name");

    // ---------------------- Buttons Styling ----------------------
    int buttonTop = dataGridView.Bottom + 15;
    int buttonHeight = 40;
    int buttonWidth = 120;
    int spacing = 20;

    addButton = CreateStyledButton("Add", 30, buttonTop, buttonWidth, buttonHeight, System.Drawing.Color.FromArgb(0, 123, 255));
    editButton = CreateStyledButton("Edit", addButton.Right + spacing, buttonTop, buttonWidth, buttonHeight, System.Drawing.Color.FromArgb(40, 167, 69));
    deleteButton = CreateStyledButton("Delete", editButton.Right + spacing, buttonTop, buttonWidth, buttonHeight, System.Drawing.Color.FromArgb(220, 53, 69));
    exportButton = CreateStyledButton("Export CSV", deleteButton.Right + spacing, buttonTop, 140, buttonHeight, System.Drawing.Color.FromArgb(255, 193, 7));

    addButton.Click += AddButton_Click;
    editButton.Click += EditButton_Click;
    deleteButton.Click += DeleteButton_Click;
    exportButton.Click += ExportButton_Click;

    // ---------------------- Add Controls ----------------------
    Controls.Add(dataGridView);
    Controls.Add(searchBox);
    Controls.Add(addButton);
    Controls.Add(editButton);
    Controls.Add(deleteButton);
    Controls.Add(exportButton);
}

// ---------------------- Helper Method to Create Stylish Buttons ----------------------
private Button CreateStyledButton(string text, int left, int top, int width, int height, System.Drawing.Color color)
{
    Button btn = new Button
    {
        Text = text,
        Left = left,
        Top = top,
        Width = width,
        Height = height,
        BackColor = color,
        ForeColor = System.Drawing.Color.White,
        FlatStyle = FlatStyle.Flat,
        Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
        Cursor = Cursors.Hand,
    };
    btn.FlatAppearance.BorderSize = 0;
    btn.FlatAppearance.MouseOverBackColor = ControlPaint.Light(color); // smooth hover
    btn.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(color);

    // Rounded corners
    btn.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, btn.Width, btn.Height, 15, 15));

    return btn;
}

// ---------------------- P/Invoke for Rounded Corners ----------------------
[System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
private static extern IntPtr CreateRoundRectRgn(
    int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
    int nWidthEllipse, int nHeightEllipse);

        private void LoadData()
        {
            dataGridView.DataSource = personService.Sort("name");
        }


        private void AddButton_Click(object sender, EventArgs e)
        {
            var personForm = new PersonForm(personService);
            personForm.ShowDialog();
            LoadData();
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0) return;

            var selected = (Person)dataGridView.SelectedRows[0].DataBoundItem;
            var personForm = new PersonForm(personService, selected);
            personForm.ShowDialog();
            LoadData();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0) return;

            var selected = (Person)dataGridView.SelectedRows[0].DataBoundItem;
            var confirm = MessageBox.Show($"Are you sure you want to delete {selected.FullName}?", 
                "Confirm Delete", MessageBoxButtons.YesNo);

            if (confirm == DialogResult.Yes)
            {
                personService.DeletePerson(selected.Id);
                LoadData();
            }
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog() { Filter = "CSV|*.csv", FileName = "Persons.csv" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    personService.ExportToCSV(sfd.FileName);
                    MessageBox.Show("Exported successfully!");
                }
            }
        }
    }
}
