using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace PenguinSlide;

public class PlayerPatch(float speed, float turnSpeed) : IScriptMod
{
    public bool ShouldRun(string path) => path == "res://Scenes/Entities/Player/player.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
    {
        // var dive_distance = 9.0
        var diveDistanceWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.PrVar,
            t => t is IdentifierToken {Name: "dive_distance"},
            t => t.Type is TokenType.OpAssign,
            t => t is ConstantToken {Value: RealVariant {Value: 9.0}},
            t => t.Type is TokenType.Newline
        ]);

        // var request_jump = false
        var requestJumpWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.PrVar,
            t => t is IdentifierToken {Name: "request_jump"},
            t => t.Type is TokenType.OpAssign,
            t => t is ConstantToken {Value: BoolVariant {Value: false}},
            t => t.Type is TokenType.Newline
        ]);

        // if Input.is_action_just_pressed("move_jump"): request_jump = true
        var moveJumpWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.CfIf,
            t => t is IdentifierToken {Name: "Input"},
            t => t.Type is TokenType.Period,
            t => t is IdentifierToken {Name: "is_action_just_pressed"},
            t => t.Type is TokenType.ParenthesisOpen,
            t => t is ConstantToken {Value: StringVariant {Value: "move_jump"}},
            t => t.Type is TokenType.ParenthesisClose,
            t => t.Type is TokenType.Colon,
            t => t is IdentifierToken {Name: "request_jump"},
            t => t.Type is TokenType.OpAssign,
            t => t is ConstantToken {Value: BoolVariant {Value: true}},
            t => t.Type is TokenType.Newline
        ]);

        // if Input.is_action_pressed_("move_forward")
        var moveForwardWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.CfIf,
            t => t is IdentifierToken {Name: "Input"},
            t => t.Type is TokenType.Period,
            t => t is IdentifierToken {Name: "is_action_pressed"},
            t => t.Type is TokenType.ParenthesisOpen,
            t => t is ConstantToken {Value: StringVariant {Value: "move_forward"}},
            t => t.Type is TokenType.ParenthesisClose,
            t => t.Type is TokenType.Colon,
        ]);
        // if Input.is_action_pressed_("move_back")
        var moveBackWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.CfIf,
            t => t is IdentifierToken {Name: "Input"},
            t => t.Type is TokenType.Period,
            t => t is IdentifierToken {Name: "is_action_pressed"},
            t => t.Type is TokenType.ParenthesisOpen,
            t => t.Type is TokenType.BuiltInType,
            t => t.Type is TokenType.ParenthesisClose,
            t => t.Type is TokenType.Colon,
        ]);
        // if Input.is_action_pressed_("move_right")
        var moveRightWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.CfIf,
            t => t is IdentifierToken {Name: "Input"},
            t => t.Type is TokenType.Period,
            t => t is IdentifierToken {Name: "is_action_pressed"},
            t => t.Type is TokenType.ParenthesisOpen,
            t => t is ConstantToken {Value: StringVariant {Value: "move_right"}},
            t => t.Type is TokenType.ParenthesisClose,
            t => t.Type is TokenType.Colon,
        ]);
        // if Input.is_action_pressed_("move_left")
        var moveLeftWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.CfIf,
            t => t is IdentifierToken {Name: "Input"},
            t => t.Type is TokenType.Period,
            t => t is IdentifierToken {Name: "is_action_pressed"},
            t => t.Type is TokenType.ParenthesisOpen,
            t => t is ConstantToken {Value: StringVariant {Value: "move_left"}},
            t => t.Type is TokenType.ParenthesisClose,
            t => t.Type is TokenType.Colon,
        ]);

        // var y_slow = 0.02
        var ySlowWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.PrVar,
            t => t is IdentifierToken {Name: "y_slow"},
            t => t.Type is TokenType.OpAssign,
            t => t is ConstantToken {Value: RealVariant {Value: 0.02}},
            t => t.Type is TokenType.Newline
        ]);

        // if animation_data["mushroom"]:
        var mushroomFovWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.CfIf,
            t => t is IdentifierToken {Name: "animation_data"},
            t => t.Type is TokenType.BracketOpen,
            t => t is ConstantToken {Value: StringVariant {Value: "mushroom"}},
            t => t.Type is TokenType.BracketClose,
            t => t.Type is TokenType.Colon,
        ]);

        // dive_vec = dive_vec.move_toward(Vector3.ZERO, delta * _accel * y_slow)
        var diveVecWaiter = new MultiTokenWaiter([
            // t => t is IdentifierToken {Name: "dive_vec"},
            // t => t.Type is TokenType.OpAssign,
            // t => t is IdentifierToken {Name: "dive_vec"},
            // t => t.Type is TokenType.Period,
            // t => t is IdentifierToken {Name: "move_toward"},
            // t => t.Type is TokenType.ParenthesisOpen,
            // t => t is IdentifierToken {Name: "Vector3"}, // THIS LINE IS BROKEN (numbers don't seem to work), workaround is just modifying the line from after this point
            t => t.Type is TokenType.Period,
            t => t is IdentifierToken {Name: "ZERO"},
            t => t.Type is TokenType.Comma,
            t => t is IdentifierToken {Name: "delta"},
            t => t.Type is TokenType.OpMul,
            t => t is IdentifierToken {Name: "_accel"},
            t => t.Type is TokenType.OpMul,
            t => t is IdentifierToken {Name: "y_slow"},
            // t => t.Type is TokenType.ParenthesisClose,
        ]);

        // if diving: speed_mult = 0.0
        var ifDivingWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.CfIf,
            t => t is IdentifierToken {Name: "diving"},
            t => t.Type is TokenType.Colon,
            t => t is IdentifierToken {Name: "speed_mult"},
            t => t.Type is TokenType.OpAssign,
            t => t is ConstantToken {Value: RealVariant {Value: 0.0}},
            t => t.Type is TokenType.Newline
        ]);

        foreach (var token in tokens)
        {
            if (diveDistanceWaiter.Check(token))
            {
                // var dive_distance = 9.0 + 6.0
                yield return new Token(TokenType.OpAdd);
                yield return new ConstantToken(new RealVariant(speed));
                yield return new Token(TokenType.Newline);

                // var was_diving = false
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("was_diving");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new BoolVariant(false));
                yield return new Token(TokenType.Newline);
            }

            if (requestJumpWaiter.Check(token))
            {
                yield return token;

                // var jump_held = false
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("jump_held");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new BoolVariant(false));
                yield return new Token(TokenType.Newline);
            }

            else if (moveJumpWaiter.Check(token))
            {
                yield return token;

                // jump_held = Input.is_action_pressed("move_jump")
                yield return new IdentifierToken("jump_held");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("Input");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("is_action_pressed");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("move_jump"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline);
            }

            else if (moveForwardWaiter.Check(token))
            {
                // and not diving
                yield return new Token(TokenType.OpAnd);
                yield return new Token(TokenType.OpNot);
                yield return new IdentifierToken("diving");

                yield return token;
            }
            else if (moveBackWaiter.Check(token))
            {
                // and not diving
                yield return new Token(TokenType.OpAnd);
                yield return new Token(TokenType.OpNot);
                yield return new IdentifierToken("diving");

                yield return token;
            }
            else if (moveLeftWaiter.Check(token))
            {
                // and not diving
                yield return new Token(TokenType.OpAnd);
                yield return new Token(TokenType.OpNot);
                yield return new IdentifierToken("diving");

                yield return token;
            }
            else if (moveRightWaiter.Check(token))
            {
                // and not diving
                yield return new Token(TokenType.OpAnd);
                yield return new Token(TokenType.OpNot);
                yield return new IdentifierToken("diving");

                yield return token;
            }

            else if (ySlowWaiter.Check(token))
            {
                yield return token;

                // if (diving and is_on_floor() and not jump_held) or (not is_on_floor() and not diving and jump_held):
                yield return new Token(TokenType.CfIf);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("diving");
                yield return new Token(TokenType.OpAnd);
                yield return new IdentifierToken("is_on_floor");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.OpAnd);
                yield return new Token(TokenType.OpNot);
                yield return new IdentifierToken("jump_held");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.OpOr);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.OpNot);
                yield return new IdentifierToken("is_on_floor");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.OpAnd);
                yield return new Token(TokenType.OpNot);
                yield return new IdentifierToken("diving");
                yield return new Token(TokenType.OpAnd);
                yield return new IdentifierToken("jump_held");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 2);

                // request_jump = true
                yield return new IdentifierToken("request_jump");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new BoolVariant(true));
                yield return new Token(TokenType.Newline, 1);

                // was_diving = diving
                yield return new IdentifierToken("was_diving");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("diving");
            }

            else if (diveVecWaiter.Check(token))
            {
                // dive_vec = dive_vec.move_toward(Vector3.ZERO, delta * 0)
                yield return new ConstantToken(new RealVariant(0));
            }

            else if (mushroomFovWaiter.Check(token))
            {
                // if animation_data["mushroom"] or was_diving:
                yield return new Token(TokenType.OpOr);
                yield return new IdentifierToken("was_diving");

                yield return token;
            }

            else if (ifDivingWaiter.Check(token))
            {
                yield return token;

                // if was_diving and Input.is_action_pressed("move_left") and not Input.is_action_pressed("move_right"):
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("was_diving");
                yield return new Token(TokenType.OpAnd);
                yield return new IdentifierToken("Input");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("is_action_pressed");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("move_left"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.OpAnd);
                yield return new Token(TokenType.OpNot);
                yield return new IdentifierToken("Input");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("is_action_pressed");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("move_right"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 2);

                // dive_vec = dive_vec.rotated(Vector3.UP, deg2rad(delta * 2))
                yield return new IdentifierToken("dive_vec");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("dive_vec");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("rotated");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.BuiltInType, (uint?) VariantType.Vector3);
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("UP");
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("deg2rad");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("delta");
                yield return new Token(TokenType.OpMul);
                yield return new ConstantToken(new RealVariant(turnSpeed));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 2);

                // rotation.y += deg2rad(delta * 2)
                yield return new IdentifierToken("rotation");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("y");
                yield return new Token(TokenType.OpAssignAdd);
                yield return new IdentifierToken("deg2rad");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("delta");
                yield return new Token(TokenType.OpMul);
                yield return new ConstantToken(new RealVariant(turnSpeed));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);

                // if was_diving and Input.is_action_pressed("move_right") and not Input.is_action_pressed("move_left"):
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("was_diving");
                yield return new Token(TokenType.OpAnd);
                yield return new IdentifierToken("Input");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("is_action_pressed");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("move_right"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.OpAnd);
                yield return new Token(TokenType.OpNot);
                yield return new IdentifierToken("Input");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("is_action_pressed");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("move_left"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 2);

                // dive_vec = dive_vec.rotated(Vector3.UP, deg2rad(delta * 1))
                yield return new IdentifierToken("dive_vec");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("dive_vec");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("rotated");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.BuiltInType, (uint?) VariantType.Vector3);
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("UP");
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("deg2rad");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.OpSub);
                yield return new IdentifierToken("delta");
                yield return new Token(TokenType.OpMul);
                yield return new ConstantToken(new RealVariant(turnSpeed));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 2);

                // rotation.y -= deg2rad(delta * 2)
                yield return new IdentifierToken("rotation");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("y");
                yield return new Token(TokenType.OpAssignSub);
                yield return new IdentifierToken("deg2rad");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("delta");
                yield return new Token(TokenType.OpMul);
                yield return new ConstantToken(new RealVariant(2));
                yield return new Token(TokenType.ParenthesisClose);

                yield return token;
            }

            else
            {
                yield return token;
            }
        }
    }
}
