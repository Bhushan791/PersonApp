using System;
using System.Drawing;
using System.Windows.Forms;
using PersonApp.core.services;
using PersonApp.core.models;
using System.Runtime.InteropServices;

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
            Width = 420;
            Height = 380;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.FromArgb(245, 245, 245); // warm light gray
            Font = new Font("Segoe UI", 10);

            personService = service;
            person = existingPerson;

            InitializeComponents();

            if (person != null)
                LoadPersonData();
        }

        private void InitializeComponents()
        {
            int labelX = 20, textBoxX = 140, topY = 30, spacingY = 50, textBoxWidth = 220;

            // ---------- Labels ----------
            fullNameLabel = CreateLabel("Full Name:", labelX, topY);
            addressLabel = CreateLabel("Address:", labelX, topY + spacingY);
            emailLabel = CreateLabel("Email:", labelX, topY + spacingY * 2);
            phoneLabel = CreateLabel("Phone:", labelX, topY + spacingY * 3);

            // ---------- TextBoxes ----------
            fullNameTextBox = CreateTextBox(textBoxX, topY, textBoxWidth);
            addressTextBox = CreateTextBox(textBoxX, topY + spacingY, textBoxWidth);
            emailTextBox = CreateTextBox(textBoxX, topY + spacingY * 2, textBoxWidth);
            phoneTextBox = CreateTextBox(textBoxX, topY + spacingY * 3, textBoxWidth);

            // ---------- Buttons ----------
            int buttonTop = topY + spacingY * 4 + 10;
            saveButton = CreateButton("Save", 80, buttonTop, 100, Color.FromArgb(0, 123, 255));
            cancelButton = CreateButton("Cancel", 220, buttonTop, 100, Color.FromArgb(220, 53, 69));

            saveButton.Click += SaveButton_Click;
            cancelButton.Click += (s, e) => Close();

            // ---------- Add Controls ----------
            Controls.AddRange(new Control[]
            {
                fullNameLabel, addressLabel, emailLabel, phoneLabel,
                fullNameTextBox, addressTextBox, emailTextBox, phoneTextBox,
                saveButton, cancelButton
            });
        }

        private Label CreateLabel(string text, int left, int top)
        {
            return new Label
            {
                Text = text,
                Left = left,
                Top = top,
                Width = 100,
                ForeColor = Color.FromArgb(50, 50, 50)
            };
        }

        private TextBox CreateTextBox(int left, int top, int width)
        {
            return new TextBox
            {
                Left = left,
                Top = top,
                Width = width,
                BackColor = Color.White,
                ForeColor = Color.Black,
                BorderStyle = BorderStyle.FixedSingle
            };
        }

        private Button CreateButton(string text, int left, int top, int width, Color color)
        {
            Button btn = new Button
            {
                Text = text,
                Left = left,
                Top = top,
                Width = width,
                Height = 40,
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = ControlPaint.Light(color);
            btn.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(color);

            // Rounded corners
            btn.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btn.Width, btn.Height, 15, 15));

            return btn;
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

            var (isValid, errorMsg) = personService.Validate(person);
            if (!isValid)
            {
                MessageBox.Show(errorMsg, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (person.Id == 0)
                personService.AddPerson(person);
            else
                personService.UpdatePerson(person);

            Close();
        }

        // ---------------------- P/Invoke for Rounded Buttons ----------------------
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);
    }
}
