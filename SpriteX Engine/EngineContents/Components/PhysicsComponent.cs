using OpenTK.Mathematics;

namespace SpriteX_Engine.EngineContents.Components
{
    /// <summary>
    /// Can be used on GameObjects to enable physics simulation.
    /// </summary>
    public class PhysicsComponent : Component
    {
        /// <summary>
        /// velocity of parent game object
        /// </summary>
        public Vector2 velocity = new Vector2(0, 0);
        /// <summary>
        /// mass of parent game object
        /// </summary>
        public float mass = 10f;
        /// <summary>
        /// slows down parent game object the higher the value
        /// </summary>
        public float friction = 0.1f;
        /// <summary>
        /// controls the behavior of the parent game object friction so that it behaves airborne when true
        /// </summary>
        public bool isAirborne = true;
        /// <summary>
        /// controls which axis (X,Y) cannot move
        /// </summary>
        public Constraint2D movementConstraint = new Constraint2D(false, false);
        /// <summary>
        /// Controls whether parent game object has gravity or not
        /// </summary>
        public bool gravityEnabled = true;
        /// <summary>
        /// controls the direction of gravity
        /// </summary>
        public Vector2 gravityVector = new Vector2(0, 1);
        /// <summary>
        /// controls the strength of gravity affecting the parent game object
        /// </summary>
        public float gravityMultiplier = 1;

        public PhysicsComponent(GameObject parent) : base(parent) { parent.SetMass(mass); }

        public override void UpdateTick(MainWindow win)
        {
            base.UpdateTick(win);

            if (gravityEnabled) velocity += gravityVector * gravityMultiplier;

            velocity *= 1 / (((isAirborne ? (friction * (1 / parent.GetMass())) : friction) + 1) >= 1 ? ((isAirborne ? (friction * (1 / parent.GetMass())) : friction) + 1) : 1);

            Vector2 constraintedVelocity = Vector2.Zero;
            constraintedVelocity.X = movementConstraint.X ? 0 : velocity.X;
            constraintedVelocity.Y = movementConstraint.Y ? 0 : velocity.Y;

            if (movementConstraint.X) velocity.X = 0;
            if (movementConstraint.Y) velocity.Y = 0;

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
