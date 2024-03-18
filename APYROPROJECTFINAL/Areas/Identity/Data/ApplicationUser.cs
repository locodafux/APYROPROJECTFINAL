using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace APYROPROJECTFINAL.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    [PersonalData]
    [Column(TypeName ="nvarchar(100)")]
    public string? Firstname { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string? Lastname { get; set; }
}

public class Student : ApplicationUser
{
    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string FirstName { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string LastName { get; set; }


    [PersonalData]
    public string EmailStudent { get; set; }

    [PersonalData]
    public string contactnumber { get; set; }

    [PersonalData]
    public string University { get; set; }

    [PersonalData]
    public string IDnumber { get; set; }


    [PersonalData]
    public string Section { get; set; }

    [PersonalData]
    public string Username { get; set; }

    [PersonalData]
    public string PasswordStudent { get; set; }

    [PersonalData]
    public string? FileName { get; set; }

    [PersonalData]
    public string? FilePath { get; set; }


}

public class Educator : ApplicationUser
{

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string FirstName { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string LastName { get; set; }


    [PersonalData]
    public string EmailEducator { get; set; }

    [PersonalData]
    public string contactnumber { get; set; }

    [PersonalData]
    public string University { get; set; }

    [PersonalData]
    public string IDnumber { get; set; }


    [PersonalData]
    public string Username { get; set; }

    [PersonalData]
    public string PasswordEducator { get; set; }


}
