namespace AncinInsaat.Models;

// One row of a FloorPlanModel's room list. Display numbering (1, 2, 3…) is
// the item's position in the list, not a stored field.
public class FloorPlanRoomModel
{
    public required string Name { get; init; }
    public required decimal AreaM2 { get; init; }
}
