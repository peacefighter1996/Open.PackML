namespace Autabee.Utility.IEC61131TypeConversionTests
{
    class Person
    {
        public string Name { get; set; }
        public int PasswordHash { get; set; }
        public Address Address { get; set; }
    }
    class Address
    {
        public string Postcode { get; set; }
        public GpsCoordinates GPS { get; set; }
    }

    class GpsCoordinates
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}