﻿using static BusinessObject.Models.Profile;

namespace BusinessObject.DTO;

public class GetUserResponeDTO
{
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? ProfilePicture { get; set; } = null!;
    public string? Bio { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
}
