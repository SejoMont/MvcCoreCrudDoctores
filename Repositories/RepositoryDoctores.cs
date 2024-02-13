using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http.HttpResults;
using MvcCoreCrudDoctores.Models;

#region PROCEDURES
//create procedure SP_INSERTDOCTOR
//(@HOSPITALCOD INT, @APELLIDO NVARCHAR(50), @ESPECIALIDAD NVARCHAR(50), @SALARIO INT)
//AS
//	declare @nextId int
//	select @nextId = MAX(DOCTOR_NO) +1 from DOCTOR
//	insert into DOCTOR values (@HOSPITALCOD, @nextId, @APELLIDO, @ESPECIALIDAD, @SALARIO)

//GO

//create procedure SP_UPDATEDOCTOR
//(@HOSPITALCOD INT, @DOCTORNO INT, @APELLIDO NVARCHAR(50), @ESPECIALIDAD NVARCHAR(50), @SALARIO INT)
//AS
//	UPDATE DOCTOR SET HOSPITAL_COD=@HOSPITALCOD, DOCTOR_NO = @DOCTORNO, APELLIDO = @APELLIDO, ESPECIALIDAD = @ESPECIALIDAD, SALARIO = @SALARIO WHERE DOCTOR_NO=@DOCTORNO

//GO
#endregion
namespace MvcCoreCrudDoctores.Repositories
{
    public class RepositoryDoctores
    {
        SqlConnection cn;
        SqlCommand com;
        SqlDataReader reader;

        public RepositoryDoctores()
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS01;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=SA;Password=MCSD2023";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
        }

        public async Task<List<Doctor>> GetDoctoresAsync()
        {
            string sql = "select * from DOCTOR";

            this.com.CommandType = System.Data.CommandType.Text;
            this.com.CommandText = sql;

            await this.cn.OpenAsync();
            this.reader = await this.com.ExecuteReaderAsync();

            List<Doctor> doctores = new List<Doctor>();
            while (await this.reader.ReadAsync())
            {
                Doctor doctor = new Doctor();
                doctor.DoctorNo = int.Parse(this.reader["DOCTOR_NO"].ToString());
                doctor.Apellido = this.reader["APELLIDO"].ToString();
                doctor.Especialidad = this.reader["ESPECIALIDAD"].ToString();
                doctor.Salario = int.Parse(this.reader["SALARIO"].ToString());
                doctor.HospitalCod = int.Parse(this.reader["HOSPITAL_COD"].ToString());
                doctores.Add(doctor);
            }
            await this.reader.CloseAsync();
            await this.cn.CloseAsync();

            return doctores;
        }

        public async Task<Doctor> FindDoctorAsync(int id)
        {
            string sql = "select * from DOCTOR where DOCTOR_NO=@id";
            this.com.Parameters.AddWithValue("@id", id);
            this.com.CommandType = System.Data.CommandType.Text;
            this.com.CommandText = sql;

            await this.cn.OpenAsync();
            this.reader = await this.com.ExecuteReaderAsync();

            Doctor doctor = null;
            if (await this.reader.ReadAsync())
            {
                doctor = new Doctor();
                doctor.DoctorNo = int.Parse(this.reader["DOCTOR_NO"].ToString());
                doctor.Apellido = this.reader["APELLIDO"].ToString();
                doctor.Especialidad = this.reader["ESPECIALIDAD"].ToString();
                doctor.Salario = int.Parse(this.reader["SALARIO"].ToString());
                doctor.HospitalCod = int.Parse(this.reader["HOSPITAL_COD"].ToString());
            }
            else
            {
                //No tenemos datos
            }
            await this.reader.CloseAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();

            return doctor;
        }
        public async Task InsertDoctorAsync(int hospitalCod, string apellido, string especialidad, int salario)
        {
            this.com.Parameters.AddWithValue("@HOSPITALCOD", hospitalCod);
            this.com.Parameters.AddWithValue("@APELLIDO", apellido);
            this.com.Parameters.AddWithValue("@ESPECIALIDAD", especialidad);
            this.com.Parameters.AddWithValue("@SALARIO", salario);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_INSERTDOCTOR";

            await this.cn.OpenAsync();
            int af = await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }

        public async Task UpdateDoctorAsync(int doctorNo, int hospitalCod, string apellido, string especialidad, int salario)
        {
            this.com.Parameters.AddWithValue("@DOCTORNO", doctorNo);
            this.com.Parameters.AddWithValue("@HOSPITALCOD", hospitalCod);
            this.com.Parameters.AddWithValue("@APELLIDO", apellido);
            this.com.Parameters.AddWithValue("@ESPECIALIDAD", especialidad);
            this.com.Parameters.AddWithValue("@SALARIO", salario);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_UPDATEDOCTOR";

            await this.cn.OpenAsync();
            int af = await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }
        public async Task DeleteDoctorAsync(int id)
        {
            string sql = "delete from DOCTOR where DOCTOR_NO=@id";

            this.com.Parameters.AddWithValue("@id", id);

            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;

            await this.cn.OpenAsync();
            int af = await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }

    }
}