﻿using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.DTOs.TableDTO
{
    public class CreateTableRequestDTO
    {
        [Required(ErrorMessage = "Table number is required.")]
        [StringLength(50, ErrorMessage = "Table number cannot exceed 50 characters.")]
        public string TableNumber { get; set; }

        [Required(ErrorMessage = "Capacity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be a positive number.")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "StatusEnum is required.")]
        [StringLength(50, ErrorMessage = "StatusEnum cannot exceed 50 characters.")]
        public string Status { get; set; }

        [StringLength(1000, ErrorMessage = "Location description cannot exceed 1000 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Shop ID is required.")]
        public Guid ShopId { get; set; }
        [Required(ErrorMessage = "Type ID is required.")]
        public Guid TypeId { get; set; }
    }
}
