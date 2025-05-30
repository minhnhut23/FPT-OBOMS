﻿using Microsoft.AspNetCore.Http;

namespace BusinessObject.DTO;

public class UpdateProfileRequestDTO
{
    public string? Email { get; set; } = null!;
    public string? FullName { get; set; } = null!;
    public IFormFile? ProfilePicture { get; set; } = null!;
    public string? Bio { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
}
