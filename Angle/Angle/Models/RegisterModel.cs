using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Angle.Models
{
    public class RegisterModel
    {
        public RegisterModel()
        {
            this.Usermodel = new UserAdd();
            this.Universities = new List<University>();
            this.Departmants = new List<Departmant>();
            this.Classes = new List<Class>();
            this.AreaCodes = new List<AreaCode>();
        }
        public UserAdd Usermodel { get; set; }
        public List<University> Universities { get; set; }
        public List<Departmant> Departmants { get; set; }
        public List<Class> Classes { get; set; }
        public List<AreaCode> AreaCodes { get; set; }
    }
}
