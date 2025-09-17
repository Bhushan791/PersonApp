using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PersonApp.core.models;
using PersonApp.core.interfaces;
using PersonApp.core.DbCon;

namespace PersonApp.core.repositories
{
    public class SqlPersonRepository : IPersonRepository
    {
        private readonly DbConnection db;

        public SqlPersonRepository()
        {
            db = new DbConnection();
        }

        public void Add(Person person)
        {
            using (var connection = new SqlConnection(db.ConnectionString))
            {
                connection.Open();
                string query = @"INSERT INTO Persons (FullName, Address, Email, Phone)
                                 VALUES (@FullName, @Address, @Email, @Phone);";
                using (var cmd = db.CreateCommand(query, connection,
                    new SqlParameter("@FullName", person.FullName),
                    new SqlParameter("@Address", person.Address ?? string.Empty),
                    new SqlParameter("@Email", person.Email),
                    new SqlParameter("@Phone", person.Phone)))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Person person)
        {
            using (var connection = new SqlConnection(db.ConnectionString))
            {
                connection.Open();
                string query = @"UPDATE Persons
                                 SET FullName=@FullName, Address=@Address, Email=@Email, Phone=@Phone
                                 WHERE Id=@Id;";
                using (var cmd = db.CreateCommand(query, connection,
                    new SqlParameter("@FullName", person.FullName),
                    new SqlParameter("@Address", person.Address ?? string.Empty),
                    new SqlParameter("@Email", person.Email),
                    new SqlParameter("@Phone", person.Phone),
                    new SqlParameter("@Id", person.Id)))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(db.ConnectionString))
            {
                connection.Open();
                string query = @"DELETE FROM Persons WHERE Id=@Id;";
                using (var cmd = db.CreateCommand(query, connection,
                    new SqlParameter("@Id", id)))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Person> GetAll()
        {
            var list = new List<Person>();

            using (var connection = new SqlConnection(db.ConnectionString))
            {
                connection.Open();
                string query = "SELECT Id, FullName, Address, Email, Phone FROM Persons;";
                using (var cmd = new SqlCommand(query, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Person
                        {
                            Id = reader.GetInt32(0),
                            FullName = reader.GetString(1),
                            Address = reader.IsDBNull(2) ? "" : reader.GetString(2),
                            Email = reader.GetString(3),
                            Phone = reader.GetString(4)
                        });
                    }
                }
            }

            return list;
        }
    }
}
