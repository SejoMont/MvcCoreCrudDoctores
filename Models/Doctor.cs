﻿namespace MvcCoreCrudDoctores.Models
{
    public class Doctor
    {
        public int DoctorNo { get; set; }
        public string Apellido { get; set; }
        public string Especialidad { get; set; }
        public int Salario { get; set; }
        public int HospitalCod { get; set; }
    }
}
