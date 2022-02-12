﻿namespace Sol0
{
    internal class Unit : IFacility
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FactoryId { get; set; }
        public void GetInfo() => Console.WriteLine($"Id = {Id}, Name = {Name}, FactoryId = {FactoryId}");
    }
}