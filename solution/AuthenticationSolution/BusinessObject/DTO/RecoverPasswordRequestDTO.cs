﻿namespace BusinessObject.DTO;

public class RecoverPasswordRequestDTO
{
    public string Token { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
}
