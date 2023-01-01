using Godot;
using System;

public partial class player : CharacterBody2D
{
    public const float Speed = 400.0f;
    public const float JumpVelocity = -400.0f;

    private Sprite2D _sprite;
    private AnimationPlayer _animationPlayer;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _Ready()
    {
        _sprite = GetNode<Sprite2D>("Sprite2D");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
            velocity.y += gravity * (float)delta;

        // Handle Jump.
        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
            velocity.y = JumpVelocity;

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        if (direction != Vector2.Zero)
        {
            velocity.x = direction.x * Speed;
        }
        else
        {
            velocity.x = Mathf.MoveToward(Velocity.x, 0, Speed);
        }

        if (direction.x == 0 && IsOnFloor())
        {
            _animationPlayer.Play("idle");
        }
        else if (IsOnFloor() && direction.x != 0)
        {
            _sprite.FlipH = direction.x <= 0;

            _animationPlayer.Play("run");
        }
        else if (!IsOnFloor())
        {
            _animationPlayer.Play("jump");
        }

        Velocity = velocity;
        MoveAndSlide();
    }
}
