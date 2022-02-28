namespace BattleshipGame.Core.Domain.Pattern;

public class Ship
{
    public Ship(string name, int length)
    {
        Name = name;
        if (length < 2)
            throw new ArgumentException("Length of ship must have minimum value of 2");
        Length = length;
    }

    private Ship()
    {

    }
    public string Name { get; }
    public int Length { get; }

    public static IEnumerable<Ship> GetClassicShips()
    {
        return new List<Ship>()
        {
            new Ship("Carrier", 5),
            new Ship("Battleship", 4),
            new Ship("Cruiser", 3),
            new Ship("Submarine", 3),
            new Ship("Destroyer", 2),
        };
    }
    public static bool operator ==(Ship a, Ship b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Name == b.Name && a.Length == b.Length;
    }

    public static bool operator !=(Ship a, Ship b)
    {
        return !(a == b);
    }

    public override bool Equals(object? obj)
    {
        if (obj is Ship ship)
            return ship == this;

        return false;
    }

    public override int GetHashCode()
    {
        return (Name.GetHashCode() + Length.GetHashCode()).GetHashCode();
    }
}