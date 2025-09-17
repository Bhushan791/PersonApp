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
            // DataGridView
            dataGridView = new DataGridView
            {
                Width = 750,
                Height = 400,
                Top = 50,
                Left = 20,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            // Search Box
            searchBox = new TextBox
            {
                Width = 200,
                Top = 10,
                Left = 20,
                PlaceholderText = "Search..."
            };
            searchBox.TextChanged += (s, e) => {
                dataGridView.DataSource = personService.Search(searchBox.Text);
            };

            // Buttons
            addButton = new Button { Text = "Add", Top = 470, Left = 20, Width = 100 };
            editButton = new Button { Text = "Edit", Top = 470, Left = 140, Width = 100 };
            deleteButton = new Button { Text = "Delete", Top = 470, Left = 260, Width = 100 };
            exportButton = new Button { Text = "Export CSV", Top = 470, Left = 380, Width = 120 };

            addButton.Click += AddButton_Click;
            editButton.Click += EditButton_Click;
            deleteButton.Click += DeleteButton_Click;
            exportButton.Click += ExportButton_Click;

            // Add controls to form
            Controls.Add(dataGridView);
            Controls.Add(searchBox);
            Controls.Add(addButton);
            Controls.Add(editButton);
            Controls.Add(deleteButton);
            Controls.Add(exportButton);
        }

        private void LoadData()
        {
            dataGridView.DataSource = personService.GetAllPersons();
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
