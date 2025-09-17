using System;
using System.Drawing;
using System.Windows.Forms;
using PersonApp.core.services;
using PersonApp.core.models;

namespace PersonApp.forms
{
    public class PersonForm : Form
    {
        private Label fullNameLabel;
        private Label addressLabel;
        private Label emailLabel;
        private Label phoneLabel;

        private TextBox fullNameTextBox;
        private TextBox addressTextBox;
        private TextBox emailTextBox;
        private TextBox phoneTextBox;

        private Button saveButton;
        private Button cancelButton;

        private PersonService personService;
        private Person? person; // nullable for edit mode

        public PersonForm(PersonService service, Person? existingPerson = null)
        {
            Text = "Person Form";
            Width = 400;
            Height = 350;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;

            personService = service;
            person = existingPerson;

            InitializeComponents();

            if (person != null)
                LoadPersonData();
        }

        private void InitializeComponents()
        {
            int labelX = 20, textBoxX = 120, topY = 20, spacingY = 40, textBoxWidth = 220;

            // Labels
            fullNameLabel = new Label { Text = "Full Name:", Left = labelX, Top = topY, Width = 100 };
            addressLabel = new Label { Text = "Address:", Left = labelX, Top = topY + spacingY, Width = 100 };
            emailLabel = new Label { Text = "Email:", Left = labelX, Top = topY + spacingY * 2, Width = 100 };
            phoneLabel = new Label { Text = "Phone:", Left = labelX, Top = topY + spacingY * 3, Width = 100 };

            // TextBoxes
            fullNameTextBox = new TextBox { Left = textBoxX, Top = topY, Width = textBoxWidth };
            addressTextBox = new TextBox { Left = textBoxX, Top = topY + spacingY, Width = textBoxWidth };
            emailTextBox = new TextBox { Left = textBoxX, Top = topY + spacingY * 2, Width = textBoxWidth };
            phoneTextBox = new TextBox { Left = textBoxX, Top = topY + spacingY * 3, Width = textBoxWidth };

            // Buttons
            saveButton = new Button
            {
                Text = "Save",
                Width = 100,
                Left = 80,
                Top = topY + spacingY * 4 + 10
            };
            cancelButton = new Button
            {
                Text = "Cancel",
                Width = 100,
                Left = 200,
                Top = topY + spacingY * 4 + 10
            };

            saveButton.Click += SaveButton_Click;
            cancelButton.Click += (s, e) => Close();

            // Add controls
            Controls.AddRange(new Control[]
            {
                fullNameLabel, addressLabel, emailLabel, phoneLabel,
                fullNameTextBox, addressTextBox, emailTextBox, phoneTextBox,
                saveButton, cancelButton
            });
        }

        private void LoadPersonData()
        {
            if (person == null) return;

            fullNameTextBox.Text = person.FullName;
            addressTextBox.Text = person.Address;
            emailTextBox.Text = person.Email;
            phoneTextBox.Text = person.Phone;
        }

private void SaveButton_Click(object sender, EventArgs e)
{
    if (person == null)
        person = new Person();

    person.FullName = fullNameTextBox.Text;
    person.Address = addressTextBox.Text;
    person.Email = emailTextBox.Text;
    person.Phone = phoneTextBox.Text;

    // Use the new Validate tuple
    var (isValid, errorMsg) = personService.Validate(person);

    if (!isValid)
    {
        MessageBox.Show(errorMsg, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return; // stop saving
    }

    if (person.Id == 0)
        personService.AddPerson(person);
    else
        personService.UpdatePerson(person);

    Close();
}

    }
}
