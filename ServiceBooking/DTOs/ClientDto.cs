﻿namespace ServiceBooking.DTOs
{
    public class ClientDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string? Email { get; set; }
    }

}
