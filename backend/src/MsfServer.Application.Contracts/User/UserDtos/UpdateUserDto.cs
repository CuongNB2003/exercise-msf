﻿using System.ComponentModel.DataAnnotations;

namespace MsfServer.Application.Contracts.User.UserDtos
{
    public class UpdateUserDto : CreateUserInput
    {
        [Required(ErrorMessage = "Name là bắt buộc.")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}
