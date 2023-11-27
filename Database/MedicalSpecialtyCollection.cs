using CoopMedica.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopMedica.Database
{
    public class MedicalSpecialtyCollection : BaseCollection<MedicalSpecialty>
    {
        public override MedicalSpecialty ReadResult(MySqlDataReader rs)
        {
            MedicalSpecialty specialty = new()
            {
                Id = rs.GetInt32("speciality_id"),
                Nome = rs.GetString("nome")
            };
            return specialty;
        }

        protected override MySqlCommand GetDeleteSQL(MedicalSpecialty item)
        {
            MySqlCommand cmd = new("DELETE FROM specialities WHERE speciality_id = @id");
            cmd.Parameters.AddWithValue("@id", item.Id);
            return cmd;
        }

        protected override MySqlCommand GetInsertSQL(MedicalSpecialty item)
        {
            MySqlCommand cmd = new("INSERT INTO specialities (nome) VALUES (@nome)");
            cmd.Parameters.AddWithValue("@nome", item.Nome);
            return cmd;
        }

        protected override MySqlCommand GetSelectSQL()
        {
            MySqlCommand cmd = new("SELECT * FROM specialities");
            return cmd;
        }

        protected override MySqlCommand GetUpdateSQL(MedicalSpecialty item)
        {
            MySqlCommand cmd = new("UPDATE specialities SET nome = @nome WHERE speciality_id = @id");
            cmd.Parameters.AddWithValue("@nome", item.Nome);
            cmd.Parameters.AddWithValue("@id", item.Id);
            return cmd;
        }
    }
}
