﻿using BusinessObject.Models;
using System.ComponentModel.DataAnnotations;
using static BusinessObject.Models.Profile;

namespace BusinessObject.DTO;

public class RegisterRequestDTO
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public string Role { get; set; } = null!;
}
