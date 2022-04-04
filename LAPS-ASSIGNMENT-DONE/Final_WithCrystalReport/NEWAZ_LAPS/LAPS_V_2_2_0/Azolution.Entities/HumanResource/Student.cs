﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.HumanResource
{
    [Serializable]
   public class Student
    {
       public Student() { }

       public int StudentId { get; set; }
       public string StudentName { get; set; }
       public string RegNo { get; set; }
       public int Gender { get; set; }
       public int DepartmentId { get; set; }
       public string DepartmentName { get; set; }
       public int SectionId { get; set; }
       public int IsActive { get; set; }
       public string StudentGender { get; set; }
       public string StudentSection { get; set; }
       public string Exam { get; set; }
       public DateTime Year { get; set; }
       public string Institute { get; set; }
       public string Result { get; set; }
       public List<StudentEducationInfo> StudentEducationInfo { get; set; }
       


    }
}
