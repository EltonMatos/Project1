namespace Game
{
    public struct CheckpointController
    {
        public Checkpoint Checkpoint { get; }
        public bool Passed { get; }

        public CheckpointController(Checkpoint checkpoint, bool passed = false)
        {
            Checkpoint = checkpoint;
            Passed = passed;
        }
    }
}