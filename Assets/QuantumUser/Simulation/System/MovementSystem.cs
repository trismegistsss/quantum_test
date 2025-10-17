namespace Quantum
{
    using Photon.Deterministic;

    public unsafe class MovementSystem : SystemMainThreadFilter<MovementSystem.Filter>, ISignalOnPlayerAdded
    {
        public void OnPlayerAdded(Frame f, PlayerRef player, bool firstTime)
        {
            RuntimePlayer runtimePlayer = f.GetPlayerData(player);
            EntityRef shipEntity = f.Create(runtimePlayer.PlayerAvatar);
        }

        public override void Update(Frame frame, ref Filter filter)
        {
            var input = frame.GetPlayerInput(filter.Linlk->Player);
            var direction = input->Direction;
            if (direction.Magnitude > 1)
            {
                direction = direction.Normalized;
            }

            if (input->Jump.WasPressed)
            {
                filter.KCC->Jump(frame);
            }

            filter.KCC->Move(frame, filter.Entity, direction.XOY);
        }


        public struct Filter
        {
            public EntityRef Entity;
            public Transform3D* Transform;
            public CharacterController3D* KCC;
            public MyPlayerLink* Linlk;
        }
    }
}
