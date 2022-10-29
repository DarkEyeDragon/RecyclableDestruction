using RecyclableDestruction.Types;

namespace RecyclableDestruction.Events;

public class DeconstructedCancelEvent
{
    public Deconstructable Deconstructable { get; }

    public DeconstructedCancelEvent(Deconstructable deconstructable)
    {
        Deconstructable = deconstructable;
    }
}