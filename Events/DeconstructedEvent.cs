using RecyclableDestruction.Types;

namespace RecyclableDestruction.Events;

public class DeconstructedEvent
{
    public Deconstructable Deconstructable { get; }

    public DeconstructedEvent(Deconstructable deconstructable)
    {
        Deconstructable = deconstructable;
    }
}