using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Angle.Models
{
    public class UserAdd
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string AreaCodeId { get; set; }
        public int GenderId { get; set; }
        public int UniversityId { get; set; }
        public int DepartmantId { get; set; }
        public int ClassId { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public Role role { get; set; }
        public string Student { get; set; }
        public string Instructor { get; set; }
        public string UploadCV { get; set; }
        public string UploadLogo { get; set; }
        public bool PDPL { get; set; }
        public string companyname { get; set; }
        public string titlecompanyname { get; set; }
    }

    public enum Role
    {
        Instructor,
        Student
    }
    public enum areacode
    {
        seksen,
        doksan
    }
}
