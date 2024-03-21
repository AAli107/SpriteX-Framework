using OpenTK.Mathematics;

namespace SpriteX_Engine.EngineContents.Components
{
    public class PhysicsComponent : Component
    {
        public Vector2 velocity = new Vector2(0, 0);
        public float mass = 10f;
        public float friction = 0.1f;
        public bool isAirborne = true;
        public Constraint2D movementConstraint = new Constraint2D(false, false);
        public bool gravityEnabled = true;
        public Vector2 gravityVector = new Vector2(0, 1);
        public float gravityMultiplier = 1;

        public PhysicsComponent(GameObject parent) : base(parent) { parent.SetMass(mass); }

        public override void GameTick(MainWindow win)
        {
            base.GameTick(win);

            if (gravityEnabled) velocity += gravityVector * gravityMultiplier;

            velocity *= 1 / (((isAirborne ? (friction * (1 / parent.GetMass())) : friction) + 1) >= 1 ? ((isAirborne ? (friction * (1 / parent.GetMass())) : friction) + 1) : 1);

            Vector2 constraintedVelocity = Vector2.Zero;
            constraintedVelocity.X = movementConstraint.X ? 0 : velocity.X;
            constraintedVelocity.Y = movementConstraint.Y ? 0 : velocity.Y;

            parent.SetPosition(parent.GetPosition() + constraintedVelocity);
        }
    }

    public struct Constraint2D 
    {
        public bool X = false;
        public bool Y = false;
        public Constraint2D() { }
        public Constraint2D(bool X, bool Y) 
        { this.X = X; this.Y = Y; }
    }
}
