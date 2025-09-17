using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PersonApp.core.models;
using PersonApp.core.repositories;
using PersonApp.core.interfaces;

namespace PersonApp.core.services
{
    public class PersonService
    {
        private readonly IPersonRepository repository;

        public PersonService()
        {
            repository = new SqlPersonRepository(); // in-memory repo
        }

        // ---------------------------
        // Add new person
        // ---------------------------
        public bool AddPerson(Person person)
        {
            var (isValid, errorMsg) = Validate(person);
            if (!isValid)
                return false;

            repository.Add(person);
            return true;
        }

        // ---------------------------
        // Update existing person
        // ---------------------------
        public bool UpdatePerson(Person person)
        {
            var (isValid, errorMsg) = Validate(person);
            if (!isValid)
                return false;

            repository.Update(person);
            return true;
        }

        // Delete person
        public void DeletePerson(int id)
        {
            repository.Delete(id);
        }

        // Get all persons
        public List<Person> GetAllPersons()
        {
            return repository.GetAll();
        }

        // Search
        public List<Person> Search(string keyword)
        {
            var all = repository.GetAll();
            return all.Where(p =>
                p.FullName.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                p.Email.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                p.Phone.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            ).ToList();
        }

        // Sort
        public List<Person> Sort(string field)
        {
            var all = repository.GetAll();
            return field.ToLower() switch
            {
                "name" => all.OrderBy(p => p.FullName).ToList(),
                "email" => all.OrderBy(p => p.Email).ToList(),
                "phone" => all.OrderBy(p => p.Phone).ToList(),
                _ => all
            };
        }

        // Export to CSV
        public void ExportToCSV(string filePath)
        {
            var all = repository.GetAll();
            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Id,FullName,Address,Email,Phone");
                foreach (var p in all)
                    writer.WriteLine($"{p.Id},{p.FullName},{p.Address},{p.Email},{p.Phone}");
            }
        }

        // ---------------------------
        // Validation
        // Returns tuple: (IsValid, ErrorMessage)
        // ---------------------------
        public (bool IsValid, string ErrorMessage) Validate(Person person)
        {
            if (string.IsNullOrWhiteSpace(person.FullName))
                return (false, "Full Name cannot be empty.");

            if (string.IsNullOrWhiteSpace(person.Email))
                return (false, "Email cannot be empty.");

            if (!person.Email.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase))
                return (false, "Email must end with @gmail.com.");

            if (string.IsNullOrWhiteSpace(person.Phone))
                return (false, "Phone number cannot be empty.");

            if (person.Phone.Length < 10 ||   person.Phone.Length >10 ||   !person.Phone.All(char.IsDigit))
                return (false, "Phone number must be 10 digits and contain only numbers.");

            if (string.IsNullOrWhiteSpace(person.Address))
                return (false, "Address cannot be empty.");

            return (true, string.Empty);
        }
    }
}
