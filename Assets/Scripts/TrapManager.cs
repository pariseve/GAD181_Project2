using UnityEngine;

public class TrapManager : MonoBehaviour
{
    public TrapObject smallTrap;
    public TrapObject largeTrap;

    public RabbitPathing rabbit;
    public WolfManager wolf;

    // Call this method when a creature interacts with a trap
    public void CreatureInteractWithTrap(CreatureType creatureType, TrapObject trap)
    {
        //if (creatureType == CreatureType.Rabbit && trap == smallTrap)
        //{
        //    rabbit.InteractWithTrap(trap);
        //}
        //else if (creatureType == CreatureType.Wolf && trap == largeTrap)
        //{
        //    wolf.InteractWithTrap(trap);
        //}
    }
}

