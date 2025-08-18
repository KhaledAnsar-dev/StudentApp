using System;
using System.Collections.Generic;

namespace StudentData.DataModels;

public partial class Student
{
    public int StudentId { get; set; }

    public string? Name { get; set; }

    public int? Grade { get; set; }

    public int? Age { get; set; }
}
